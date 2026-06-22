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
        // ... código existente de tu constructor y otros métodos ...
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

        // ... otros métodos existentes ...

        /// <summary>
        /// NUEVO MÉTODO: Ejecuta un SP con un único Table-Valued Parameter y devuelve un DataTable.
        /// Ideal para operaciones de guardado que retornan el ID del nuevo registro.
        /// </summary>
        public DataTable EscribirSPConTVPYDevolverDataTable(string SPName, Hashtable Params, DataTable tvpData, string tvpTypeName, string tvpParamName)
        {
            oCnn.Open();
            DataTable dtResult = new DataTable();
            SqlCommand comm = new SqlCommand(SPName, oCnn);
            comm.CommandType = CommandType.StoredProcedure;

            if (Params != null)
            {
                foreach (string key in Params.Keys)
                {
                    comm.Parameters.AddWithValue(key, Params[key]);
                }
            }

            // Configurar el parámetro de tipo tabla (TVP)
            SqlParameter tvpParam = comm.Parameters.AddWithValue(tvpParamName, tvpData);
            tvpParam.SqlDbType = SqlDbType.Structured;
            tvpParam.TypeName = tvpTypeName;

            // Usamos un SqlDataAdapter para ejecutar el comando y llenar nuestro DataTable de resultado.
            SqlDataAdapter da = new SqlDataAdapter(comm);
            da.Fill(dtResult);

            oCnn.Close();
            return dtResult; // Devuelve el DataTable con los resultados (ej. el ID)
        }

        // ... resto de tus métodos en Acceso.cs ...
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
            oCnn.Close();

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

                //int a = comm.ExecuteNonQuery();
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

        public DataTable EscribirSPConDosTVP(string SPName, Hashtable Params,
                                             DataTable tvpData1, string tvpTypeName1, string tvpParamName1,
                                             DataTable tvpData2, string tvpTypeName2, string tvpParamName2)
        {
            oCnn.Open();
            DataTable dtResult = new DataTable();
            SqlCommand comm = new SqlCommand(SPName, oCnn);
            comm.CommandType = CommandType.StoredProcedure;

            if (Params != null) { foreach (string key in Params.Keys) { comm.Parameters.AddWithValue(key, Params[key]); } }

            // Configurar primer TVP
            SqlParameter tvpParam1 = comm.Parameters.AddWithValue(tvpParamName1, tvpData1);
            tvpParam1.SqlDbType = SqlDbType.Structured;
            tvpParam1.TypeName = tvpTypeName1;

            // Configurar segundo TVP
            SqlParameter tvpParam2 = comm.Parameters.AddWithValue(tvpParamName2, tvpData2);
            tvpParam2.SqlDbType = SqlDbType.Structured;
            tvpParam2.TypeName = tvpTypeName2;

            SqlDataAdapter da = new SqlDataAdapter(comm);
            da.Fill(dtResult); // Ejecuta el SP y captura la tabla de resultados (el ID de la FAQ)

            oCnn.Close();
            return dtResult;
        }
        public bool EscribirSPConTVP(string SPName, Hashtable Params, DataTable tvpData, string tvpTypeName, string tvpParamName)
        {
            oCnn.Open();
            SqlCommand comm = new SqlCommand(SPName, oCnn);
            comm.CommandType = CommandType.StoredProcedure;
            if (Params != null) { foreach (string key in Params.Keys) { comm.Parameters.AddWithValue(key, Params[key]); } }

            SqlParameter tvpParam = comm.Parameters.AddWithValue(tvpParamName, tvpData);
            tvpParam.SqlDbType = SqlDbType.Structured;
            tvpParam.TypeName = tvpTypeName;

            int resultado = comm.ExecuteNonQuery();
            oCnn.Close();
            return resultado > 0;
        }
        public DataTable EscribirSPConTVPs(string SPName, Hashtable Params, DataTable tvpPreguntas, DataTable tvpOpciones)
        {
            oCnn.Open();
            DataTable dtResult = new DataTable();
            SqlCommand comm = new SqlCommand(SPName, oCnn);
            comm.CommandType = CommandType.StoredProcedure;

            if (Params != null) { foreach (string key in Params.Keys) { comm.Parameters.AddWithValue(key, Params[key]); } }

            SqlParameter tvpParamPreguntas = comm.Parameters.AddWithValue("@Preguntas", tvpPreguntas);
            tvpParamPreguntas.SqlDbType = SqlDbType.Structured;
            tvpParamPreguntas.TypeName = "dbo.PreguntaTabla";

            SqlParameter tvpParamOpciones = comm.Parameters.AddWithValue("@Opciones", tvpOpciones);
            tvpParamOpciones.SqlDbType = SqlDbType.Structured;
            tvpParamOpciones.TypeName = "dbo.OpcionTabla";

            SqlDataAdapter da = new SqlDataAdapter(comm);
            da.Fill(dtResult); // Fill ejecuta el SP y captura la tabla de resultados (el ID)

            oCnn.Close();
            return dtResult;
        }
        public DataSet LeerSPDS(string SPName, Hashtable Params)
        {
            oCnn.Open();
            DataSet ds = new DataSet(); // Usamos un DataSet en lugar de un DataTable
            SqlCommand comm = new SqlCommand(SPName, oCnn);
            comm.CommandType = CommandType.StoredProcedure;

            try
            {
                // Creamos el data adapter, que es el encargado de llenar el DataSet
                SqlDataAdapter Da = new SqlDataAdapter(comm);
                if (Params != null)
                {
                    foreach (string key in Params.Keys)
                    {
                        comm.Parameters.AddWithValue(key, Params[key]);
                    }
                }

                // El método Fill se encarga de ejecutar el SP y llenar el DataSet con todas las tablas que encuentre
                Da.Fill(ds);
            }
            catch (SqlException ex)
            { throw ex; }
            catch (Exception ex)
            { throw ex; }
            finally
            { // Cierro la Conexion
                oCnn.Close();
            }
            return ds; // Devolvemos el DataSet completo
        }

        /// <summary>
        /// Ejecuta una consulta SQL de texto con parámetros seguros (previene SQL Injection).
        /// Los parámetros se pasan en un Hashtable donde la clave es el nombre del parámetro
        /// (ej. "@Path") y el valor es el dato.
        /// </summary>
        public int EjecutarConsultaParametrizada(string consultaSQL, Hashtable parametros)
        {
            oCnn.Open();
            SqlCommand comm = new SqlCommand(consultaSQL, oCnn);
            comm.CommandType = CommandType.Text;

            try
            {
                if (parametros != null)
                {
                    foreach (string key in parametros.Keys)
                    {
                        comm.Parameters.AddWithValue(key, parametros[key] ?? DBNull.Value);
                    }
                }
                return comm.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                oCnn.Close();
            }
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
