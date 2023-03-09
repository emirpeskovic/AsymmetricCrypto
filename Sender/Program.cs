using System.Security.Cryptography;
using System.Text;

while (true)
{
    // Clear the console
    Console.Clear();
    
    // If the private key doesn't exist, we need to create one
    if (!File.Exists("private.pem") || !File.Exists("public.pem"))
    {
        // Delete the files if they exist
        File.Delete("private.pem");
        File.Delete("public.pem");

        // Ask the user for the key size
        Console.WriteLine("Enter the key size (in bits):");
        var keySize = int.Parse(Console.ReadLine() ?? throw new InvalidOperationException("Invalid input"));

        // Create the RSA algorithm
        using var rsa = RSA.Create();

        // Set the key size
        rsa.KeySize = keySize;

        // Export the private key to a file
        File.WriteAllText("private.pem", rsa.ToXmlString(true));

        // Export the public key to a file
        File.WriteAllText("public.pem", rsa.ToXmlString(false));

        // Print the public key
        Console.WriteLine("Public key:");
        Console.WriteLine(rsa.ToXmlString(false));

        // Print the private key
        Console.WriteLine("Private key:");
        Console.WriteLine(rsa.ToXmlString(true));

        // Wait for the user to press a key
        Console.WriteLine("Press any key to continue.");
        Console.ReadKey();
    }

    // Load the public key from a file
    var publicKey = File.ReadAllText("public.pem");

    // Create the RSA algorithm
    using var pRsa = RSA.Create();

    // Import the public key
    pRsa.FromXmlString(publicKey);

    // Get the user's input
    Console.WriteLine("Enter the text to encrypt:");
    var input = Console.ReadLine();

    // Encrypt the input
    if (input != null)
    {
        var encrypted = pRsa.Encrypt(Encoding.UTF8.GetBytes(input), RSAEncryptionPadding.Pkcs1);

        // Print the encrypted text
        Console.WriteLine("Encrypted text:");
        Console.WriteLine(Convert.ToBase64String(encrypted));
        
        // Ask the user if they want to write the encrypted text to a file
        Console.WriteLine("Would you like to write the encrypted text to a file? (y/n)");
        var writeToFile = Console.ReadKey().Key == ConsoleKey.Y;
        Console.WriteLine();
        
        // If the user wants to write the encrypted text to a file
        if (writeToFile)
        {
            // Write the encrypted text to a file
            File.WriteAllText("encrypted.txt", Convert.ToBase64String(encrypted));
        }
    }
    else
    {
        Console.WriteLine("Invalid input.");
    }

    // Wait for the user to press a key to restart
    Console.WriteLine("Press any key to restart.");
    Console.ReadKey();
}