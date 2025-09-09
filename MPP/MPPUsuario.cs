using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using BE;
using System.Data;
using System.Runtime.Remoting.Messaging;

namespace MPP
{
    public class MPPUsuario
    {
        Acceso bd;
        public int ValidarUsuario(BEUsuario usuario)
        {
            bd = new Acceso();
            string Query = "doLogin";
            Hashtable ht = new Hashtable();
            ht.Add("@username", usuario.username);
            ht.Add("@hashedPassword", usuario.HashedPassword);

            return bd.LeerSPRT(Query, ht);

        }
        public int verifyPass(BEUsuario usuario, string pw)
        {
            bd = new Acceso();
            string Query = "doLogin";
            Hashtable ht = new Hashtable();
            ht.Add("@username", usuario.username);
            ht.Add("@hashedPassword", pw);
            int i = bd.LeerSPRT(Query, ht);
            return i;


        }
        public bool ResetearPassword(BEUsuario usuario)
        {
            MPPCrypto c = new MPPCrypto();

            usuario.PasswordSalt = c.GenerarSalt();
            usuario.HashedPassword = c.HashPassword(usuario.Password, usuario.PasswordSalt);

            bd = new Acceso();
            string Query = "ResetearPassword";
            Hashtable ht = new Hashtable();
            ht.Add("@Codigo", usuario.Codigo);
            ht.Add("@hashedPassword", usuario.HashedPassword);
            ht.Add("@sal", usuario.PasswordSalt);
            int i = bd.LeerSPRT(Query, ht);
            if (i == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Baja(BEUsuario usuario)
        {
            bd = new Acceso();
            string query = "BorrarUsuario";
            var ht = new Hashtable();
            ht.Add("@ID_Usuario", usuario.Codigo);

            // Asumiendo que tu método EscribirSP devuelve el número de filas afectadas o un return code
            int resultado = bd.EscribirSP(query, ht);

            return resultado == 0; // O "resultado > 0" dependiendo de tu DAL
        }

        public bool ValidarTokenRecuperacion(BEUsuario Objeto, string token)
        {
            if (token == null) { return false; }
            if (Objeto == null) { return false; }
            string Consulta_SQL = "ValidarTokenRecuperacion";
            Hashtable ht = new Hashtable();
            ht.Add("@ID_Usuario", Objeto.Codigo);
            ht.Add("@Token", token);
            bd = new Acceso();
            if (bd.LeerSPRT(Consulta_SQL, ht) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool ActivarUsuarioConToken(BEUsuario Objeto)
        {

            string Consulta_SQL = "ActivarUsuarioConToken";
            Hashtable ht = new Hashtable();
            ht.Add("@ID", Objeto.Codigo);
            bd = new Acceso();
            if (bd.LeerSPRT(Consulta_SQL, ht) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public bool CambiarPassword(BEUsuario usuario, string passwordActual)
        {
            var ht = new Hashtable();
            ht.Add("@ID_Usuario", usuario.Codigo);
            ht.Add("@PasswordActual", passwordActual);
            ht.Add("@PasswordNueva", usuario.Password); // La nueva contraseña ya viene en el objeto

            // Usamos LeerSP para obtener la tabla de resultados del SP
            DataTable dt = bd.LeerSP("sp_CambiarPasswordUsuario", ht);

            if (dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0]["Resultado"]) == 1;
            }
            return false;
        }
        public bool Guardar(BEUsuario Objeto)
        {
            MPPCrypto c = new MPPCrypto();
            if ((Objeto.Codigo == 0) || (!string.IsNullOrWhiteSpace(Objeto.Password)))
            {

                Objeto.PasswordSalt = c.GenerarSalt();
                Objeto.HashedPassword = c.HashPassword(Objeto.Password, Objeto.PasswordSalt);
            }


            string query = "GuardarUsuario";
            Hashtable ht = new Hashtable();
            ht.Add("@Codigo", Objeto.Codigo);
            ht.Add("@Nombre", Objeto.Nombre);
            ht.Add("@Apellido", Objeto.Apellido);
            
            ht.Add("@DNI", Objeto.DNI);
            ht.Add("@Email", Objeto.Email);
            ht.Add("@Password", Objeto.HashedPassword);
            ht.Add("@Bloqueado", 0);
            
            if (Objeto.Empresa == null) { Objeto.Empresa = new BEEmpresa() { Codigo = 1 }; };
            ht.Add("@Empresa", Objeto.Empresa.Codigo);
            ht.Add("@Idioma", Objeto.Idioma == null ? 1 : Objeto.Idioma.Codigo);
            ht.Add("@Eliminado", Objeto.Eliminado ? 1 : 0);
            ht.Add("@PrimerLogin", Objeto.PrimerLogin);
            ht.Add("@Salt", Objeto.PasswordSalt);
            ht.Add("@TokenActivacion", Objeto.TokenActivacion == null ? DBNull.Value : (Object)Objeto.TokenActivacion);
            ht.Add("@TokenRecupero", Objeto.TokenRecupero == null ? DBNull.Value : (Object)Objeto.TokenRecupero);

            bd = new Acceso();
            int a = bd.LeerSPRT(query, ht);
            if (a == 0) { return true; }
            return false;
        }



        public List<BEUsuario> ListarTodos()
        {
            bd = new Acceso();
            string query = "ListarUsuarios";

            

            DataTable tabla = bd.LeerSP(query, null);

            var listaUsuarios = new List<BEUsuario>();

            if (tabla != null && tabla.Rows.Count > 0)
            {
                
                foreach (DataRow row in tabla.Rows)
                {
                    
                    BEUsuario usuario = Mapear(row);

                    listaUsuarios.Add(usuario);
                }
            }

            return listaUsuarios;
        }

        public List<BEUsuario> ListarTodosPorEmpresa(BEEmpresa empresa)
        {
            bd = new Acceso();
            string query = "ListarUsuariosPorEmpresa";

            Hashtable ht = new Hashtable();
            ht.Add("@IDEmpresa", empresa.Codigo);

            DataTable tabla = bd.LeerSP(query, ht);

            var listaUsuarios = new List<BEUsuario>();

            if (tabla != null && tabla.Rows.Count > 0)
            {

                foreach (DataRow row in tabla.Rows)
                {

                    BEUsuario usuario = Mapear(row);

                    listaUsuarios.Add(usuario);
                }
            }

            return listaUsuarios;
        }

        /// <summary>
        /// Función privada que convierte un DataRow en un objeto BEUsuario.
        /// </summary>
        /// <param name="row">La fila de datos a convertir.</param>
        /// <returns>Un objeto BEUsuario populado.</returns>
        private BEUsuario Mapear(DataRow row)
        {
            BEUsuario usuario = new BEUsuario();
            MPPEmpresa mppe = new MPPEmpresa();
            MPPRbac mPPRbac = new MPPRbac();
            MPPIdioma mPPIdioma = new MPPIdioma();
            usuario.Codigo = Convert.ToInt32(row["ID"]);
            usuario.Nombre = row["Nombre"].ToString();
            usuario.Apellido = row["Apellido"].ToString();
            usuario.DNI = row["DNI"].ToString();
            usuario.Email = row["Email"].ToString();
            usuario.Roles = mPPRbac.ObtenerRolesParaUsuario(usuario);

            usuario.HashedPassword = row["Password"].ToString();
            usuario.Bloqueado = Convert.ToBoolean(row["Bloqueado"]);
            usuario.Empresa = mppe.ListarObjeto(new BEEmpresa() { Codigo = Convert.ToInt32(row["Empresa"]) });
            usuario.Idioma = mPPIdioma.ListarIdioma(new BEIdioma(Convert.ToInt32(row["IdiomaSeleccionado"])));
            usuario.PrimerLogin = Convert.ToBoolean(row["PrimerLogin"]);
            usuario.TokenActivacion = row["TokenActivacion"] == DBNull.Value ? null : row["TokenActivacion"].ToString();
            usuario.TokenRecupero = row["TokenRecuperacion"] == DBNull.Value ? null : row["TokenRecuperacion"].ToString();
            usuario.PasswordSalt = (byte[])row["Salt"];

            return usuario;
        }



        public BEUsuario ListarObjeto(BEUsuario oUsuarioFiltro)
        {
            bd = new Acceso();
            string query = "ListarUsuario";
            Hashtable ht = new Hashtable();

            // Verificamos si la búsqueda es por ID o por Email
            if (oUsuarioFiltro.Codigo > 0)
            {
                // Búsqueda por ID (Codigo)
                ht.Add("@ID", oUsuarioFiltro.Codigo);
                ht.Add("@Email", DBNull.Value); // Enviamos NULL para el parámetro que no se usa
            }
            else if (!string.IsNullOrEmpty(oUsuarioFiltro.Email))
            {
                // Búsqueda por Email
                ht.Add("@ID", DBNull.Value); // Enviamos NULL para el parámetro que no se usa
                ht.Add("@Email", oUsuarioFiltro.Email);
            }
            else
            {
                // Si no se proveyó ni ID ni Email, no hay nada que buscar.
                return null;
            }

            DataTable tabla = bd.LeerSP(query, ht);

            if (tabla != null && tabla.Rows.Count > 0)
            {
                // Si encontramos un resultado, lo mapeamos a un nuevo objeto BEUsuario
                DataRow row = tabla.Rows[0];
                BEUsuario usuarioEncontrado = new BEUsuario();
                MPPEmpresa mppe = new MPPEmpresa();
                MPPRbac mPPRbac = new MPPRbac();
                MPPIdioma mPPIdioma = new MPPIdioma();

                usuarioEncontrado.Codigo = Convert.ToInt32(row["ID"]);
                usuarioEncontrado.Nombre = row["Nombre"].ToString();
                usuarioEncontrado.Apellido = row["Apellido"].ToString();
                usuarioEncontrado.DNI = row["DNI"].ToString();
                usuarioEncontrado.Email = row["Email"].ToString();

                usuarioEncontrado.Roles = mPPRbac.ObtenerRolesParaUsuario(usuarioEncontrado);

                usuarioEncontrado.HashedPassword = row["Password"].ToString();
                usuarioEncontrado.Bloqueado = Convert.ToBoolean(row["Bloqueado"]); // Asumiendo que quieres un booleano


                usuarioEncontrado.Empresa = mppe.ListarObjeto(new BEEmpresa() { Codigo = Convert.ToInt32(row["Empresa"]) });
                usuarioEncontrado.Idioma = mPPIdioma.ListarIdioma(new BEIdioma(Convert.ToInt32(row["IdiomaSeleccionado"])));

                usuarioEncontrado.PrimerLogin = Convert.ToBoolean(row["PrimerLogin"]);

                // Manejo de campos que pueden ser nulos en la base de datos
                usuarioEncontrado.TokenActivacion = row["TokenActivacion"] == DBNull.Value ? null : row["TokenActivacion"].ToString();
                usuarioEncontrado.TokenRecupero = row["TokenRecuperacion"] == DBNull.Value ? null : row["TokenRecuperacion"].ToString();

                // El campo Salt es VARBINARY, se debe castear a byte[]
                usuarioEncontrado.PasswordSalt = (byte[])row["Salt"];

                return usuarioEncontrado;
            }
            else
            {
                // Si la tabla está vacía, no se encontró el usuario.
                return null;
            }
        }

        public string FijarTokenResetPassword(BEUsuario usuario)
        {
            string token = Guid.NewGuid().ToString();
            usuario = ListarObjeto(usuario);
            usuario.TokenRecupero = token;
            Guardar(usuario);
            return token;
        }
    }
}

