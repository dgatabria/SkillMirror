using BE;
using MPP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLRbac
    {
        private MPPRbac mpp;

        public BLLRbac()
        {
            mpp = new MPPRbac();
        }

        #region Métodos para Roles
        public List<BERol> ListarRoles()
        {
            return mpp.ListarRoles();
        }

        public bool GuardarRol(BERol rol)
        {
            return mpp.GuardarRol(rol);
        }

        public bool BorrarRol(BERol rol)
        {
            return mpp.BorrarRol(rol);
        }
        #endregion

        #region Métodos para Permisos
        public List<BEPermiso> ListarPermisos()
        {
            return mpp.ListarPermisos();
        }

        public bool GuardarPermiso(BEPermiso permiso)
        {
            return mpp.GuardarPermiso(permiso);
        }

        public bool BorrarPermiso(BEPermiso permiso)
        {
            return mpp.BorrarPermiso(permiso);
        }
        #endregion

        #region Métodos para Relaciones
        public bool AgregarPermisoARol(BERol rol, BEPermiso permiso)
        {
            return mpp.AgregarPermisoARol(rol, permiso);
        }

        public bool QuitarPermisoDeRol(BERol rol, BEPermiso permiso)
        {
            return mpp.QuitarPermisoDeRol(rol, permiso);
        }

        public List<BEPermiso> ListarPermisosPorRol(BERol rol)
        {
            return mpp.ListarPermisosPorRol(rol);
        }

        public bool GuardarRolesDeUsuario(BEUsuario usuario, List<BERol> roles)
        {
            return mpp.GuardarRolesDeUsuario(usuario, roles);
        }

        public List<BERol> ListarRolesPorUsuario(BEUsuario usuario)
        {
            return mpp.ListarRolesPorUsuario(usuario);
        }

        public bool UsuarioTienePermiso(BEUsuario usuario, string nombrePermiso)
        {
            MPPRbac _mppRbac = new MPPRbac();
            // Regla de Negocio 1: Un usuario inválido no tiene permisos.
            if (usuario == null || usuario.Codigo <= 0)
            {
                return false;
            }

            // Regla de Negocio 2: Un usuario bloqueado o eliminado no tiene permisos.
            if (usuario.Bloqueado || usuario.Eliminado)
            {
                return false;
            }

            // Regla de Negocio 3: El nombre del permiso es obligatorio.
            if (string.IsNullOrWhiteSpace(nombrePermiso))
            {
                throw new ArgumentException("El nombre del permiso no puede estar vacío.");
            }

            // Si todas las reglas de negocio pasan, consultamos a la capa de persistencia.
            return _mppRbac.UsuarioTienePermiso(usuario, nombrePermiso);
        }

        #endregion
    }
}