﻿using System;
using System.IO;
using System.Security.Cryptography;

namespace _3_14_AES_encryption
{
    class Program
    {
        static void DumpBytes(string title, byte[] bytes)
        {
            Console.Write(title);
            foreach (byte b in bytes)
            {
                Console.Write("{0:X} ", b);
            }
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            string plainText = "This is my super secret data";

            // byte array to hold the encrypted message
            byte[] cipherText;

            // byte array to hold the key that was used for encryption
            byte[] key;

            // byte array to hold the initialization vector that was used for encryption
            byte[] initializationVector;

            // Create an Aes instance
            // This creates a random key and initialization vector

            using (Aes aes = Aes.Create())
            {
                // copy the key and the initialization vector
                key = aes.Key;
                initializationVector = aes.IV;

                // create an encryptor to encrypt some data
                // should be wrapped in using for production code
                ICryptoTransform encryptor = aes.CreateEncryptor();

                // Create a new memory stream to receive the encrypted data.
                using (MemoryStream encryptMemoryStream = new MemoryStream())
                {
                    // create a CryptoStream, tell it the stream to write
                    // and the encryptor to use. Also set the mode
                    using (CryptoStream encryptCryptoStream = new CryptoStream(encryptMemoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        // make a stream writer from the cryptostream
                        using (StreamWriter swEncrypt = new StreamWriter(encryptCryptoStream))
                        {
                            // Write the secret message to the stream.
                            swEncrypt.Write(plainText);
                        }
                        // get the encrypted message from the stream
                        cipherText = encryptMemoryStream.ToArray();
                    }
                }
            }

            // Dump out the data
            Console.WriteLine("String to encrypt: {0}", plainText);
            DumpBytes("Key: ", key);
            DumpBytes("Initialization Vector: ", initializationVector);
            DumpBytes("Encrypted: ", cipherText);

            Console.ReadKey();

            // Now do the decryption
            string decryptedText;

            using (Aes aes = Aes.Create())
            {
                // Configure the aes instances with the key and
                // initialization vector to use for the decryption
                aes.Key = key;
                aes.IV = initializationVector;

                // Create a decryptor from aes
                // should be wrapped in using for production code
                ICryptoTransform decryptor = aes.CreateDecryptor();

                using (MemoryStream decryptStream = new MemoryStream(cipherText))
                {
                    using (CryptoStream decryptCryptoStream = new CryptoStream(decryptStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(decryptCryptoStream))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            decryptedText = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            Console.WriteLine("Decrypted text: {0}", decryptedText);
            Console.ReadKey();
        }
    }
}
