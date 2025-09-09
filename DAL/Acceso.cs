using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DAL
{
    public class Acceso
    {
        private SqlConnection oCnn;
        public Acceso()
        {
            this.oCnn = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=master;Integrated Security=True");
            this.oCnn.Open();
            string query = "USE [master]; SELECT count(*) FROM sys.databases WHERE name = 'SKILLMIRROR'";

            SqlCommand command = new SqlCommand(query, oCnn);
            int i = Convert.ToInt32(command.ExecuteScalar());

            if (i == 1)
            {
                oCnn.Close();
                oCnn = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=SKILLMIRROR;Integrated Security=True");
            }
            else
            {
                string directorioEjecutable = AppDomain.CurrentDomain.BaseDirectory;
                string nombreArchivo = "SKILLMIRROR.sql";
                string rutaCompleta = Path.Combine(directorioEjecutable, nombreArchivo);
                string script = File.ReadAllText(rutaCompleta);
                string[] scriptBlocks = Regex.Split(script, @"\bGO\b", RegexOptions.IgnoreCase);
                foreach (string block in scriptBlocks)
                {
                    if (!string.IsNullOrWhiteSpace(block))
                    {
                        using (SqlCommand command2 = new SqlCommand(block, oCnn))
                        {
                            command2.ExecuteNonQuery();
                        }
                    }
                }
                oCnn.Close();
                oCnn = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=SKILLMIRROR;Integrated Security=True");

            }


        }



        //Creo el objeto command
        SqlCommand cmd;

        // creo una funcion para saber el estado de la conexion
        public string TestConnection()
        {
            oCnn.Open();
            //si no uso el metodo Abrir puedo hacer el open 
            //conexion.Open();
            //Cerrar();
            if (oCnn.State == ConnectionState.Open)
            {
                return "Conexion a la BD OK";
            }
            else
            {
                return "No se pudo conectar a la BD, que pacho???";
            }
        }



        public bool EscribirSPConTVP(string SPName, Hashtable Params, DataTable tvpData)
        {
            oCnn.Open();
            SqlCommand comm = new SqlCommand(SPName, oCnn);
            comm.CommandType = CommandType.StoredProcedure;

            if (Params != null)
            {
                foreach (string key in Params.Keys)
                {
                    comm.Parameters.AddWithValue(key, Params[key]);
                }
            }

            // Agregar el parámetro de tipo tabla
            SqlParameter tvpParam = comm.Parameters.AddWithValue("@Traducciones", tvpData);
            tvpParam.SqlDbType = SqlDbType.Structured;
            tvpParam.TypeName = "dbo.TraduccionTabla"; // El nombre del TIPO que creamos en SQL

            int resultado = comm.ExecuteNonQuery();
            oCnn.Close() ;

            return resultado >= 0; // ExecuteNonQuery devuelve -1 si no hay SET NOCOUNT ON
        }

        // Stored procedre Escritura
        public int EscribirSP(string SPName, Hashtable Params)
        {
            oCnn.Open();
            int a = 0;
            SqlCommand comm = new SqlCommand(SPName, oCnn);
            comm.CommandType = CommandType.StoredProcedure;

            try
            {
                //creo el data adapter le paso la consulta y la conexion
                SqlDataAdapter Da = new SqlDataAdapter(comm);
                if (Params != null)
                {
                    foreach (string key in Params.Keys)
                    {
                        comm.Parameters.AddWithValue(key, Params[key]);
                    }
                }

                a = comm.ExecuteNonQuery();
            }
            catch (SqlException ex)
            { throw ex; }
            catch (Exception ex)
            { throw ex; }
            finally
            { //cierro la Conexion
                oCnn.Close();
            }
            return a;
        }



        // Stored procedre Lectura
        public DataTable LeerSP(string SPName, Hashtable Params)
        {
            oCnn.Open();
            DataTable tabla = new DataTable();
            SqlCommand comm = new SqlCommand(SPName, oCnn);
            comm.CommandType = CommandType.StoredProcedure;

            try
            {
                //creo el data adapter le paso la consulta y la conexion
                SqlDataAdapter Da = new SqlDataAdapter(comm);
                if (Params != null)
                {
                    foreach (string key in Params.Keys)
                    {
                        comm.Parameters.AddWithValue(key, Params[key]);
                    }
                }
                //lleno la tabla con el metodo fill

                int a = comm.ExecuteNonQuery();
                Da.Fill(tabla);

            }
            catch (SqlException ex)
            { throw ex; }
            catch (Exception ex)
            { throw ex; }
            finally
            { //cierro la Conexion
                oCnn.Close();
            }
            return tabla;
        }


        public bool EscribirConsulta(string Consulta_SQL)
        {

            oCnn.Open();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = oCnn;
            cmd.CommandText = Consulta_SQL;
            try
            {
                int respuesta = cmd.ExecuteNonQuery();
                return true;
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            finally
            { oCnn.Close(); }

        }


        // Stored procedure de lectura con tabla de resultados y número de filas afectadas
        public Tuple<DataTable, int> LeerSPConTotalRecords(string SPName, Hashtable Params)
        {
            oCnn.Open();
            DataTable tabla = new DataTable();
            int totalRecords = 0;
            SqlCommand comm = new SqlCommand(SPName, oCnn);
            comm.CommandType = CommandType.StoredProcedure;

            try
            {
                if (Params != null)
                {
                    foreach (string key in Params.Keys)
                    {
                        comm.Parameters.AddWithValue(key, Params[key]);
                    }
                }

                // Agregamos el parámetro de salida explícitamente
                var totalRecordsParam = new SqlParameter("@TotalRecords", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                comm.Parameters.Add(totalRecordsParam);

                SqlDataAdapter Da = new SqlDataAdapter(comm);
                Da.Fill(tabla);

                // Leemos el valor del parámetro de salida DESPUÉS de ejecutar la consulta
                totalRecords = (int)totalRecordsParam.Value;
            }
            catch (SqlException ex) { throw ex; }
            catch (Exception ex) { throw ex; }
            finally { oCnn.Close(); }

            return Tuple.Create(tabla, totalRecords);
        }

        // Stored procedre con return value
        public int LeerSPRT(string SPName, Hashtable Params)
        {
            oCnn.Open();

            SqlCommand comm = new SqlCommand(SPName, oCnn);
            comm.CommandType = CommandType.StoredProcedure;

            try
            {
                //creo el data adapter le paso la consulta y la conexion
                SqlDataAdapter Da = new SqlDataAdapter(comm);
                SqlParameter retValue = comm.Parameters.Add("@RetValue", SqlDbType.Int);
                retValue.Direction = ParameterDirection.ReturnValue;
                if (Params != null)
                {
                    foreach (string key in Params.Keys)
                    {
                        comm.Parameters.AddWithValue(key, Params[key]);
                    }
                }
                //lleno la tabla con el metodo fill

                int a = comm.ExecuteNonQuery();
                return (int)(retValue.Value);

            }
            catch (SqlException ex)
            { throw ex; }
            catch (Exception ex)
            { throw ex; }
            finally
            { //cierro la Conexion
                oCnn.Close();
            }

        }
    }
}
