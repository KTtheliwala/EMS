using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TheTecniQ.Core.Infrastructure
{
    public class EncryptionUtility
    {
        #region Utilities

        private static byte[] Encrypt(string plainText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
            {
                throw new ArgumentNullException(nameof(plainText));
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException(nameof(key));
            }
            byte[] encrypted;
            // Create a Aes object
            // with the specified key and IV.
            using (Aes rijAlg = Aes.Create())
            {
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.
                var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.
                using var msEncrypt = new MemoryStream();
                using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
                using (var swEncrypt = new StreamWriter(csEncrypt))
                {
                    //Write all data to the stream.
                    swEncrypt.Write(plainText);
                }
                encrypted = msEncrypt.ToArray();
            }
            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        private static string Decrypt(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException(nameof(cipherText));
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException(nameof(key));
            }

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes rijAlg = Aes.Create())
            {
                //Settings
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                try
                {
                    // Create the streams used for decryption.
                    using var msDecrypt = new MemoryStream(cipherText);
                    using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);

                    using var srDecrypt = new StreamReader(csDecrypt);
                    // Read the decrypted bytes from the decrypting stream
                    // and place them in a string.
                    plaintext = srDecrypt.ReadToEnd();
                }
                catch
                {
                    plaintext = "keyError";
                }
            }

            return plaintext;
        }

        #endregion
        public static string CreatePasswordHash(string password)
        {
            StringBuilder sBuilder = new();
            byte[] result = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            foreach (byte data in result)
            {
                sBuilder.Append(data.ToString("x2"));
            }
            return sBuilder.ToString();
        }
        #region Methods

        /// <summary>
        /// Create a SHA256 hash
        /// </summary>
        /// <param name="plainText">Text to hash</param>
        /// <returns>SHA256 hash</returns>
        public static string CreateHash(string plainText)
        {
            StringBuilder sBuilder = new();
            using (SHA256 hash = SHA256.Create())
            {
                byte[] result = hash.ComputeHash(Encoding.UTF8.GetBytes(plainText));
                foreach (byte data in result)
                {
                    sBuilder.Append(data.ToString("x2"));
                }
            }
            return sBuilder.ToString();
        }

        /// <summary>
        /// Encrypt text
        /// </summary>
        /// <param name="plainText">Text to encrypt</param>
        /// <param name="key">Encryption private key</param>
        /// <param name="iv">Encryption iv</param>
        /// <returns>Encrypted text</returns>
        public static string Encrypt(string plainText, string key, string iv)
        {
            if (string.IsNullOrEmpty(plainText))
                return "";
            var keybytes = Encoding.UTF8.GetBytes(key);
            var ivbyets = Encoding.UTF8.GetBytes(iv);           
            var encrypted = Encrypt(plainText, keybytes, ivbyets);
            return System.Web.HttpUtility.UrlEncode(Convert.ToBase64String(encrypted));
        }

        /// <summary>
        /// Decrypt text
        /// </summary>
        /// <param name="cipherText">Text to decrypt</param>
        /// <param name="key">Decryption private key</param>
        /// <param name="iv">Decryption iv</param>
        /// <returns>Decrypted text</returns>
        public static string Decrypt(string cipherText, string key, string iv)
        {
            var keybytes = Encoding.UTF8.GetBytes(key);
            var ivbytes = Encoding.UTF8.GetBytes(iv);
            cipherText = cipherText.Replace(" ", "+");
            var encrypted = Convert.FromBase64String(System.Web.HttpUtility.UrlDecode(cipherText));
            var decripted = Decrypt(encrypted, keybytes, ivbytes);
            return decripted;
        }

        /// <summary>
        /// Encrypt and URL encoded text
        /// </summary>
        /// <param name="plainText">Text to encrypt</param>
        /// <param name="key">Encryption private key</param>
        /// <param name="iv">Encryption iv</param>
        /// <returns>Encrypted text</returns>
        public static string EncryptURLEncoded(string plainText, string key, string iv)
        {
            return System.Web.HttpUtility.UrlEncode(Encrypt(plainText, key, iv));
        }

        /// <summary>
        /// Decrypt and URL decoded text
        /// </summary>
        /// <param name="cipherText">Text to decrypt</param>
        /// <param name="key">Decryption private key</param>
        /// <param name="iv">Decryption iv</param>
        /// <returns>Decrypted text</returns>
        public static string DecryptURLDecoded(string cipherText, string key, string iv)
        {
            return Decrypt(System.Web.HttpUtility.UrlDecode(cipherText), key, iv);
        }

        #endregion
    }
}
