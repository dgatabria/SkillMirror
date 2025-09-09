using BE;
using DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPP
{
    public class MPPRbac
    {
        private Acceso bd;

        // --- Mapeadores privados para reutilizar código ---
        private BERol MapearRol(DataRow row)
        {
            return new BERol(Convert.ToInt32(row["ID"]), row["Nombre"].ToString());
            
        }

        private BEPermiso MapearPermiso(DataRow row)
        {
            return new BEPermiso(Convert.ToInt32(row["ID"]), row["Nombre"].ToString());
        }

        // --- Métodos para Roles ---
        public List<BERol> ListarRoles()
        {
            bd = new Acceso();
            string query = "sp_ListarRoles";
            DataTable tabla = bd.LeerSP(query, null);
            var listaRoles = new List<BERol>();

            if (tabla != null && tabla.Rows.Count > 0)
            {
                foreach (DataRow row in tabla.Rows)
                {
                    listaRoles.Add(MapearRol(row));
                }
            }
            return listaRoles;
        }

        public bool GuardarRol(BERol rol)
        {
            bd = new Acceso();
            string query = "sp_GuardarRol";
            var ht = new Hashtable();
            ht.Add("@ID", rol.Codigo);
            ht.Add("@Nombre", rol.Nombre);

            // Asumiendo que EscribirSP devuelve el número de filas afectadas
            return bd.EscribirSP(query, ht) > 0;
        }

        public bool BorrarRol(BERol rol)
        {
            bd = new Acceso();
            string query = "sp_BorrarRol";
            var ht = new Hashtable();
            ht.Add("@ID", rol.Codigo);

            // Asumiendo que EscribirSP devuelve el código de retorno del SP (0 para éxito)
            return bd.EscribirSP(query, ht) == 1;
        }

        // --- Métodos para Permisos ---
        public List<BEPermiso> ListarPermisos()
        {
            bd = new Acceso();
            string query = "sp_ListarPermisos";
            DataTable tabla = bd.LeerSP(query, null);
            var listaPermisos = new List<BEPermiso>();

            if (tabla != null && tabla.Rows.Count > 0)
            {
                foreach (DataRow row in tabla.Rows)
                {
                    listaPermisos.Add(MapearPermiso(row));
                }
            }
            return listaPermisos;
        }
        public List<BEPermiso> ListarPermisosPorRol(BERol rol)
        {
            bd = new Acceso();
            string query = "sp_ListarPermisosPorRol";
            var ht = new Hashtable();
            ht.Add("@ID_Rol", rol.Codigo);

            DataTable tabla = bd.LeerSP(query, ht);
            var listaPermisos = new List<BEPermiso>();

            if (tabla != null && tabla.Rows.Count > 0)
            {
                foreach (DataRow row in tabla.Rows)
                {
                    int id = Convert.ToInt32(row["ID"]);
                    string nombre = row["Nombre"].ToString();
                    listaPermisos.Add(new BEPermiso(id, nombre));
                }
            }
            return listaPermisos;
        }
        public bool GuardarPermiso(BEPermiso permiso)
        {
            bd = new Acceso();
            string query = "sp_GuardarPermiso";
            var ht = new Hashtable();
            ht.Add("@ID", permiso.Codigo);
            ht.Add("@Nombre", permiso.Nombre);
            return bd.EscribirSP(query, ht) > 0;
        }

        public bool BorrarPermiso(BEPermiso permiso)
        {
            bd = new Acceso();
            string query = "sp_BorrarPermiso";
            var ht = new Hashtable();
            ht.Add("@ID", permiso.Codigo);
            return bd.EscribirSP(query, ht) == 0;
        }

        // --- Métodos para Relaciones ---
        public bool AgregarPermisoARol(BERol rol, BEPermiso permiso)
        {
            bd = new Acceso();
            string query = "sp_AgregarPermisoARol";
            var ht = new Hashtable();
            ht.Add("@ID_Rol", rol.Codigo);
            ht.Add("@ID_Permiso", permiso.Codigo);
            return bd.EscribirSP(query, ht) > 0;
        }

        public bool QuitarPermisoDeRol(BERol rol, BEPermiso permiso)
        {
            bd = new Acceso();
            string query = "sp_QuitarPermisoDeRol";
            var ht = new Hashtable();
            ht.Add("@ID_Rol", rol.Codigo);
            ht.Add("@ID_Permiso", permiso.Codigo);
            return bd.EscribirSP(query, ht) > 0;
        }

        /// <summary>
        /// Obtiene la estructura completa de roles y permisos para un usuario.
        /// </summary>
        public List<RBAC> ObtenerRolesParaUsuario(BEUsuario usuario)
        {
            bd = new Acceso();
            var rolesDelUsuario = new List<RBAC>();

            // 1. Obtener los roles base del usuario
            var htRoles = new Hashtable();
            htRoles.Add("@ID_Usuario", usuario.Codigo);
            DataTable tablaRoles = bd.LeerSP("sp_ListarRolesPorUsuario", htRoles);

            if (tablaRoles != null && tablaRoles.Rows.Count > 0)
            {
                foreach (DataRow rowRol in tablaRoles.Rows)
                {
                    var rol = new BERol(Convert.ToInt32(rowRol["ID"]), rowRol["Nombre"].ToString());

                    // 2. Para cada rol, obtener sus permisos (hijos)
                    var htPermisos = new Hashtable();
                    htPermisos.Add("@ID_Rol", rol.Codigo);
                    DataTable tablaPermisos = bd.LeerSP("sp_ListarPermisosPorRol", htPermisos);

                    if (tablaPermisos != null && tablaPermisos.Rows.Count > 0)
                    {
                        foreach (DataRow rowPermiso in tablaPermisos.Rows)
                        {
                            var permiso = new BEPermiso(Convert.ToInt32(rowPermiso["ID"]), rowPermiso["Nombre"].ToString());

                            rol.AgregarHijo(permiso);
                        }
                    }
                    rolesDelUsuario.Add(rol);
                }
            }
            return rolesDelUsuario;
        }

        public bool GuardarRolesDeUsuario(BEUsuario usuario, List<BERol> roles)
        {
            if (usuario == null || usuario.Codigo <= 0)
            {
                return false;
            }

            bd = new Acceso();
            string query = "sp_GuardarRolesDeUsuario";
            var ht = new Hashtable();

            string listaIds = string.Join(",", roles.Select(r => r.Codigo));

            ht.Add("@ID_Usuario", usuario.Codigo);
            ht.Add("@ListaIDsRoles", listaIds);

            return bd.EscribirSP(query, ht) >= 0;
        }

        /// <summary>
        /// Obtiene todos los roles asignados a un usuario específico.
        /// </summary>
        public List<BERol> ListarRolesPorUsuario(BEUsuario usuario)
        {
            bd = new Acceso();
            var ht = new Hashtable();
            ht.Add("@ID_Usuario", usuario.Codigo);
            DataTable tabla = bd.LeerSP("sp_ListarRolesPorUsuario", ht);

            var listaRoles = new List<BERol>();

            if (tabla != null && tabla.Rows.Count > 0)
            {
                foreach (DataRow row in tabla.Rows)
                {
                    int id = Convert.ToInt32(row["ID"]);
                    string nombre = row["Nombre"].ToString();
                    listaRoles.Add(new BERol(id, nombre));
                }
            }
            return listaRoles;
        }
    }
}
