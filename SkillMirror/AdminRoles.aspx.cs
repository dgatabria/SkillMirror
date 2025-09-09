using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BE;
using BLL;

namespace SkillMirror
{

    public partial class AdminRoles : BasePage
    {
        private BLLRbac bllRbac = new BLLRbac();


        protected void Page_Load(object sender, EventArgs e)
        {


            if (!IsPostBack)
            {
                BindRolesList();
                BindPermissionsList();
                pnlEdicion.Visible = false;
            }
        }


        

        public override void ActualizarTraducciones()
        {
            this.Title = _traductor.Traducir("AdminRoles_Page_Title");
            headerTitle.InnerText = _traductor.Traducir("AdminRoles_Header_Titulo");

            // Card Izquierda
            litCardRolesTitle.Text = _traductor.Traducir("AdminRoles_CardRoles_Titulo");
            litCardRolesText.Text = _traductor.Traducir("AdminRoles_CardRoles_Texto");
            btnNuevo.Text = _traductor.Traducir("AdminRoles_CardRoles_Boton_Nuevo");


            lblNombreRol.Text = _traductor.Traducir("AdminRoles_CardEdicion_Label_Nombre");
            valNombreRol.ErrorMessage = _traductor.Traducir("AdminRoles_CardEdicion_Error_Nombre");
            lblPermisosAsignados.Text = _traductor.Traducir("AdminRoles_CardEdicion_Label_Permisos");
            btnGuardar.Text = _traductor.Traducir("AdminRoles_CardEdicion_Boton_Guardar");
            btnEliminar.Text = _traductor.Traducir("AdminRoles_CardEdicion_Boton_Eliminar");

            // Mensaje de confirmación para el botón de eliminar
            string confirmMsg = _traductor.Traducir("AdminRoles_CardEdicion_Confirmacion_Eliminar");
            btnEliminar.OnClientClick = $"return confirm('{confirmMsg}');";
        }

        protected void LstRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstRoles.SelectedItem == null)
            {
                pnlEdicion.Visible = false;
                return;
            }


            int rolId = Convert.ToInt32(lstRoles.SelectedValue);
            string rolNombre = lstRoles.SelectedItem.Text;

            // TRADUCCIÓN del título dinámico
            litEdicionTitulo.Text = $"{_traductor.Traducir("AdminRoles_CardEdicion_Titulo_Editando")} {rolNombre}";
            txtNombreRol.Text = rolNombre;
            hfRolId.Value = rolId.ToString();

            // ... (resto de la lógica sin cambios)
            foreach (ListItem item in cblPermisos.Items) { item.Selected = false; }
            List<BEPermiso> permisosAsignados = bllRbac.ListarPermisosPorRol(new BERol(rolId, rolNombre));
            foreach (var permisoAsignado in permisosAsignados)
            {
                ListItem item = cblPermisos.Items.FindByValue(permisoAsignado.Codigo.ToString());
                if (item != null) { item.Selected = true; }
            }
            pnlEdicion.Visible = true;
            btnEliminar.Visible = true;
            litMensaje.Text = "";
        }

        protected void BtnNuevo_Click(object sender, EventArgs e)
        {
            lstRoles.ClearSelection();
            hfRolId.Value = "0";
            txtNombreRol.Text = string.Empty;
            
            // TRADUCCIÓN del título dinámico
            litEdicionTitulo.Text = _traductor.Traducir("AdminRoles_CardEdicion_Titulo_Creando");
            
            pnlEdicion.Visible = true;
            btnEliminar.Visible = false;
            litMensaje.Text = "";

            foreach (ListItem item in cblPermisos.Items)
            {
                item.Selected = false;
            }
        }

        // --- MÉTODOS EXISTENTES (sin cambios en su lógica principal) ---

        private void BindRolesList()
        {
            lstRoles.DataSource = bllRbac.ListarRoles();
            lstRoles.DataTextField = "Nombre";
            lstRoles.DataValueField = "Codigo";
            lstRoles.DataBind();
        }

        private void BindPermissionsList()
        {
            cblPermisos.DataSource = bllRbac.ListarPermisos();
            cblPermisos.DataTextField = "Nombre";
            cblPermisos.DataValueField = "Codigo";
            cblPermisos.DataBind();
        }
        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                int rolId = Convert.ToInt32(hfRolId.Value);
                BERol rol = new BERol(rolId, txtNombreRol.Text);

                // Guardar el rol (crea o actualiza el nombre)
                bllRbac.GuardarRol(rol);

                // Obtener el ID del rol recién guardado (si era nuevo)
                if (rolId == 0)
                {
                    var rolGuardado = bllRbac.ListarRoles().FirstOrDefault(r => r.Nombre == txtNombreRol.Text);
                    if (rolGuardado != null) rolId = rolGuardado.Codigo;
                }

                if (rolId > 0)
                {
                    // Volvemos a instanciar el rol con el ID correcto
                    rol = new BERol(rolId, txtNombreRol.Text);

                    // Sincronizar los permisos: primero quitamos todos, luego agregamos los seleccionados
                    List<BEPermiso> permisosActuales = bllRbac.ListarPermisosPorRol(rol);
                    foreach (var p in permisosActuales)
                    {
                        bllRbac.QuitarPermisoDeRol(rol, p);
                    }

                    foreach (ListItem item in cblPermisos.Items)
                    {
                        if (item.Selected)
                        {
                            var permiso = new BEPermiso(Convert.ToInt32(item.Value), item.Text);
                            bllRbac.AgregarPermisoARol(rol, permiso);
                        }
                    }
                }

                litMensaje.Text = "<div class='alert alert-success'>Rol guardado exitosamente.</div>";
                pnlEdicion.Visible = false;
                BindRolesList();
            }
            catch (Exception ex)
            {
                litMensaje.Text = $"<div class='alert alert-danger'>Error al guardar el rol: {ex.Message}</div>";
            }
        }
        protected void BtnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                int rolId = Convert.ToInt32(hfRolId.Value);
                if (rolId > 0)
                {
                    BERol rol = new BERol(rolId, txtNombreRol.Text);
                    if (bllRbac.BorrarRol(rol))
                    {
                        litMensaje.Text = "<div class='alert alert-success'>Rol eliminado exitosamente.</div>";
                    }
                    else
                    {
                        litMensaje.Text = "<div class='alert alert-warning'>No se pudo eliminar el rol. Verifique que no esté asignado a ningún usuario.</div>";
                    }
                    pnlEdicion.Visible = false;
                    BindRolesList();
                }
            }
            catch (Exception ex)
            {
                litMensaje.Text = $"<div class='alert alert-danger'>Error al eliminar el rol: {ex.Message}</div>";
            }
        }
    }
}