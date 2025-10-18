using DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MPP
{
    public class MPPCrypto
    {
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("EstaEsUnaClaveSuperSeguraDe32Chr"); // Debe tener 32 bytes (256 bits)
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("Sk1llM1rr0r_IV_16");

        public byte[] Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
                return null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                //aesAlg.IV = IV;
                aesAlg.BlockSize = 128; // El tamaño del bloque para AES es siempre 128 bits.
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;
                aesAlg.GenerateIV(); // El IV generado será del tamaño del BlockSize (128 bits / 16 bytes).
                byte[] iv = aesAlg.IV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        return msEncrypt.ToArray();
                    }
                }
            }
        }
        public string Decrypt(byte[] cipherText)
        {
            if (cipherText == null || cipherText.Length <= 0)
                return null;

            string plaintext = null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }
        public string HashPassword(string password, byte[] salt)
            {
                using (var sha256 = SHA256.Create())
                {
                    // Combinamos la contraseña y la sal en un solo array de bytes
                    if (password == null ) { password = Guid.NewGuid().ToString(); }
                    var passwordBytes = Encoding.UTF8.GetBytes(password);
                    var saltedPassword = new byte[salt.Length + passwordBytes.Length];

                    Buffer.BlockCopy(salt, 0, saltedPassword, 0, salt.Length);
                    Buffer.BlockCopy(passwordBytes, 0, saltedPassword, salt.Length, passwordBytes.Length);

                    // Calculamos el hash del conjunto combinado
                    var hashBytes = sha256.ComputeHash(saltedPassword);

                    // Convertimos el resultado a Base64 para guardarlo fácilmente en la base de datos
                    return Convert.ToBase64String(hashBytes);
                }
            }


            public byte[] GenerarSalt(int size = 16)
            {
                var salt = new byte[size];
                using (var rng = new RNGCryptoServiceProvider())
                {
                    rng.GetBytes(salt);
                }
                return salt;
            }
        public byte[] TraerSalt(string email)
        {
            Acceso bd = new Acceso();
            string Query = "TraerSal";
            Hashtable ht = new Hashtable();
            ht.Add("@email", email);
            DataTable i = bd.LeerSP(Query, ht);
            if (i != null && i.Rows.Count > 0)
            {
                return (byte[])i.Rows[0]["Salt"];
            }
            else
            {
                return null;
            }

        }
    }
    }

