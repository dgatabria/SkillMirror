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

        public bool VerificarPassword(BEUsuario usuario, string passwordEnClaro)
        {
            // 1. Instanciamos tu clase de criptografía
            MPPCrypto mppCrypto = new MPPCrypto();

            // 2. Obtenemos la "sal" (salt) específica de este usuario desde la BD
            byte[] salt = mppCrypto.TraerSalt(usuario.Email);

            // Si no se encuentra el usuario, no hay nada que verificar
            if (salt == null)
            {
                return false;
            }

            // 3. Hasheamos la contraseña que el usuario acaba de ingresar en el modal, usando su sal
            string hashedPassword = mppCrypto.HashPassword(passwordEnClaro, salt);

            // 4. Le pasamos el usuario y el hash recién calculado al método de la MPP que llama al SP
            //    El SP en la base de datos hará la comparación final de forma segura.
            return usuariomp.VerificarPassword(usuario, hashedPassword);
        }
        public bool HacerAdministrador(BEUsuario usuario)
        {
            // Podrían ir reglas de negocio aquí, por ejemplo:
            // "if(usuario.Empresa.PlanContratado.Codigo < 3) return false;"
            return usuariomp.HacerAdministrador(usuario);
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

        public List<BEUsuario> ListarSuscritosEncuestas()
        {
            // Simplemente llama al método correspondiente de la capa de mapeo
            return usuariomp.ListarSuscritosEncuestas();
        }
        public bool CambiarPassword(BEUsuario usuario, string passwordActual)
        {

            if (string.IsNullOrEmpty(usuario.Password) || usuario.Password.Length < 8)
            {
                throw new Exception("La nueva contraseña debe tener al menos 8 caracteres.");
            }
            return usuariomp.CambiarPassword(usuario, passwordActual);
        }
        public List<BEUsuario> ListarSuscritos()
        {
            return usuariomp.ListarSuscritos();
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
