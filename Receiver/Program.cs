using System.Security.Cryptography;
using System.Text;

while (true)
{
    // Clear the console
    Console.Clear();
    
    // If the private key doesn't exist, exit
    if (!File.Exists("private.pem"))
    {
        Console.WriteLine("The private key doesn't exist, press any key to exit.");
        Console.ReadKey();
        return;
    }
    
    // Load the private key from a file
    var privateKey = File.ReadAllText("private.pem");
    
    // Create the RSA algorithm
    using var rsa = RSA.Create();
    
    // Import the private key
    rsa.FromXmlString(privateKey);
    
    // Check if encrypted.txt exists
    if (File.Exists("encrypted.txt"))
    {
        // Read the encrypted text
        var encrypted = File.ReadAllText("encrypted.txt");
        
        // Decrypt the text
        var decrypted = rsa.Decrypt(Convert.FromBase64String(encrypted), RSAEncryptionPadding.Pkcs1);
        
        // Print the decrypted text
        Console.WriteLine("Decrypted text:");
        Console.WriteLine(Encoding.UTF8.GetString(decrypted));
    }
    else
    {
        // Get the user's input
        Console.WriteLine("Enter the text to decrypt:");
        var input = Console.ReadLine();
        
        // Encrypt the input
        if (input != null)
        {
            var decrypted = rsa.Decrypt(Encoding.UTF8.GetBytes(input), RSAEncryptionPadding.Pkcs1);
            
            // Write the decrypted text to the console
            Console.WriteLine("Decrypted text:");
            Console.WriteLine(Encoding.UTF8.GetString(decrypted));
            
            // Ask the user if they want to save the decrypted text
            Console.WriteLine("Do you want to save the decrypted text? (y/n)");
            var writeToFile = Console.ReadKey().Key == ConsoleKey.Y;
            
            // If the user wants to save the decrypted text
            if (writeToFile)
            {
                // Write the decrypted text to a file
                File.WriteAllText("decrypted.txt", Encoding.UTF8.GetString(decrypted));
            }
        }
    }
    
    // Wait for the user to press a key to restart
    Console.WriteLine("Press any key to restart.");
    Console.ReadKey();
}