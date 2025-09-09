using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using MPP;

namespace BLL
{
    public class BLLUsuario
    {
        MPPUsuario usuariomp;
        public BLLUsuario()
        {
            usuariomp = new MPPUsuario();
        }


        public bool Guardar(BEUsuario Objeto)
        {
            return usuariomp.Guardar(Objeto);
        }

        public BEUsuario ListarObjeto(BEUsuario Objeto)
        {
            return usuariomp.ListarObjeto(Objeto);
        }

        public bool ActivarUsuarioConToken(BEUsuario Objeto)
        {
            return usuariomp.ActivarUsuarioConToken(Objeto);
        }
        public bool ResetearPassword(BEUsuario Objeto)
        {
            return usuariomp.ResetearPassword(Objeto);
        }

        public bool ValidarTokenRecuperacion(BEUsuario Objeto, string token)
        {
            return usuariomp.ValidarTokenRecuperacion(Objeto, token);
        }

        public string FijarTokenResetPassword(BEUsuario Objeto)
        {
            return usuariomp.FijarTokenResetPassword(Objeto);   
        }


        public bool CambiarPassword(BEUsuario usuario, string passwordActual)
        {

            if (string.IsNullOrEmpty(usuario.Password) || usuario.Password.Length < 8)
            {
                throw new Exception("La nueva contraseña debe tener al menos 8 caracteres.");
            }
            return usuariomp.CambiarPassword(usuario, passwordActual);
        }
        public List<BEUsuario> ListarTodos()
        {
             return usuariomp.ListarTodos();
        }
        public List<BEUsuario> ListarTodosPorEmpresa(BEEmpresa empresa)
        {
            return usuariomp.ListarTodosPorEmpresa(empresa);
        }
        public bool Baja(BEUsuario usuario)
        {
            // Agregar reglas de negocio, como "no permitir borrar al último admin"
            MPPUsuario mpp = new MPPUsuario();
            return mpp.Baja(usuario);
        }
        public List<BEUsuario> ListarFiltrado(string regex)
        {
            //return usuariomp.ListarFiltrado(regex);
            return null;
        }
    }
}
