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
        #endregion
    }
}