using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using BE;
using DAL;

namespace MPP
{
    public class MPPBackup
    {
        private readonly Acceso oAcceso;
        public MPPBackup() { oAcceso = new Acceso(); }

        public void CrearBackup(BEBackup backup)
        {
            // Ojo: Este comando requiere permisos elevados para el usuario de SQL Server.
            // En un entorno de producción, se manejaría de forma más segura.
            string dbName = "SkillMirror";
            string backupCommand = $"BACKUP DATABASE [{dbName}] TO DISK = @Path WITH FORMAT, MEDIANAME = 'SkillMirror_Backup', NAME = 'Full Backup of SkillMirror';";

            var ht = new Hashtable();
            ht.Add("@Path", backup.Path);

            // Usamos el método parametrizado para prevenir SQL Injection.
            oAcceso.EjecutarConsultaParametrizada(backupCommand, ht);
        }

        public void RegistrarBackup(BEBackup backup)
        {
            var ht = new Hashtable();
            ht.Add("@Path", backup.Path);
            ht.Add("@Checksum", " pendiente"); // Calcular checksum puede ser lento. Por ahora lo omitimos.
            ht.Add("@Size", backup.Size);
            ht.Add("@Usuario", backup.Usuario.Codigo);
            oAcceso.EscribirSP("sp_RegistrarBackup", ht);
        }

        public List<BEBackup> Listar()
        {
            var lista = new List<BEBackup>();
            DataTable dt = oAcceso.LeerSP("sp_ListarBackups", null);
            foreach (DataRow dr in dt.Rows)
            {
                lista.Add(new BEBackup
                {
                    Codigo = Convert.ToInt32(dr["ID"]),
                    TimeStamp = Convert.ToDateTime(dr["TimeStamp"]),
                    Path = dr["Path"].ToString(),
                    Size = Convert.ToInt64(dr["Size"]),
                    UsuarioEmail = dr["UsuarioEmail"].ToString()
                });
            }
            return lista;
        }

        public void RestaurarBackup(string path)
        {
            // Creamos una cadena de conexión ESPECIAL que apunta a la base de datos 'master'.
            string masterConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=master;Integrated Security=True";

            using (var masterCnn = new SqlConnection(masterConnectionString))
            {
                masterCnn.Open();

                try
                {
                    // 1. Poner la base de datos en modo de usuario único para que nadie más la use.
                    string sqlSetSingleUser = "ALTER DATABASE [SkillMirror] SET SINGLE_USER WITH ROLLBACK IMMEDIATE";
                    using (var cmdSingleUser = new SqlCommand(sqlSetSingleUser, masterCnn))
                    {
                        cmdSingleUser.ExecuteNonQuery();
                    }

                    // 2. Ejecutar el comando de restauración con parámetros para prevenir SQL Injection.
                    string sqlRestore = "RESTORE DATABASE [SkillMirror] FROM DISK = @Path WITH REPLACE";
                    using (var cmdRestore = new SqlCommand(sqlRestore, masterCnn))
                    {
                        cmdRestore.Parameters.AddWithValue("@Path", path);
                        cmdRestore.ExecuteNonQuery();
                    }
                }
                finally
                {
                    // 3. (MUY IMPORTANTE) Volver a poner la base de datos en modo multi-usuario, pase lo que pase.
                    string sqlSetMultiUser = "ALTER DATABASE [SkillMirror] SET MULTI_USER";
                    using (var cmdMultiUser = new SqlCommand(sqlSetMultiUser, masterCnn))
                    {
                        cmdMultiUser.ExecuteNonQuery();
                    }
                }
            }
        }
        public BEBackup ObtenerPorId(int id)
        {
            var ht = new Hashtable();
            ht.Add("@ID", id);
            DataTable dt = oAcceso.LeerSP("sp_ObtenerBackupPorID", ht);
            if (dt.Rows.Count > 0)
            {
                return new BEBackup { Path = dt.Rows[0]["Path"].ToString() };
            }
            return null;
        }
    }
}