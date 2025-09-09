using DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MPP
{
    public class MPPCrypto
    {


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

