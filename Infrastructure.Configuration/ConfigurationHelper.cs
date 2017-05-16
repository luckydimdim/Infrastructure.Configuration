using System;
using Microsoft.Extensions.Options;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace Cmas.Infrastructure.Configuration
{
    public static class ConfigurationHelper
    {
        public static CmasConfiguration GetConfiguration(this IServiceProvider serviceProvider)
        {
            return ((IOptions<CmasConfiguration>) serviceProvider.GetService(typeof(IOptions<CmasConfiguration>))).Value;
        }

        public static string EncryptString(string text, string keyString)
        {
            var key = Encoding.UTF8.GetBytes(keyString);

            using (var aesAlg = Aes.Create())
            {
                using (var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV))
                {
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }

                        var iv = aesAlg.IV;

                        var decryptedContent = msEncrypt.ToArray();

                        var result = new byte[iv.Length + decryptedContent.Length];

                        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                        Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

                        return Convert.ToBase64String(result);
                    }
                }
            }
        }

        public static string DecryptString(string cipherText, string keyString)
        {
            if (string.IsNullOrEmpty(cipherText) || string.IsNullOrEmpty(keyString))
                return null;

            var fullCipher = Convert.FromBase64String(cipherText);

            var iv = new byte[16];
            var cipher = new byte[16];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, iv.Length);
            var key = Encoding.UTF8.GetBytes(keyString);

            using (var aesAlg = Aes.Create())
            {
                using (var decryptor = aesAlg.CreateDecryptor(key, iv))
                {
                    string result;
                    using (var msDecrypt = new MemoryStream(cipher))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                result = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                    return result;
                }
            }
        }

        public static Database WithDefaults(this Database database, string schema, string host, string login, string password)
        {
            if (string.IsNullOrEmpty(database.Schema))
            {
                database.Schema = schema;
            }

            if (string.IsNullOrEmpty(database.Host))
            {
                database.Host = host;
            }

            if (string.IsNullOrEmpty(database.Login))
            {
                database.Login = login;
            }

            if (string.IsNullOrEmpty(database.Password))
            {
                database.Password = password;
            }

            return database;

        }
    }
}
