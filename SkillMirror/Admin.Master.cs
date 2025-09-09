using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL; // Importamos la BLL
using BE;  // Importamos BE

namespace SkillMirror
{
    // Clase auxiliar para definir la estructura de un ítem del menú
    public class MenuItem
    {
        public string Texto { get; set; }
        public string Url { get; set; }
        public string CssClass { get; set; }
    }

    // PASO 1: Implementamos la interfaz
    public partial class AdminMaster : System.Web.UI.MasterPage, ITraducible
    {
        private Traductor _traductor;

        protected void Page_Load(object sender, EventArgs e)
        {
            // PASO 2: Suscribimos la master page al traductor
            _traductor = Traductor.ObtenerInstancia();
            _traductor.Suscribir(this);

            if (!IsPostBack)
            {
                CargarDatosUsuario();
                CargarIdiomas();
                CargarMenu(); // Carga inicial del menú
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (_traductor != null)
            {
                _traductor.Desuscribir(this);
            }
        }

        // PASO 3: Implementamos el método de la interfaz
        public void ActualizarTraducciones()
        {
            // Traducimos los controles estáticos
            this.Page.Title = _traductor.Traducir("AdminMaster_Page_Title") + " - Consola SkillMirror";
            headerTitle.InnerHtml = $"<img src='{ResolveUrl("~/img/skillmirror150px.png")}' alt='SkillMirror Logo' style='height: 35px;' /> {_traductor.Traducir("AdminMaster_Header_Titulo")}";
            litIdioma.Text = _traductor.Traducir("SiteMaster_Menu_Link_Idioma"); // Reutilizamos un tag
            linkMiPerfil.InnerText = _traductor.Traducir("SiteMaster_MenuUsuario_Link_MiPerfil"); // Reutilizamos un tag
            btnLogout.Text = _traductor.Traducir("SiteMaster_MenuUsuario_Boton_CerrarSesion"); // Reutilizamos un tag

            // Volvemos a cargar el menú dinámico para que tome las nuevas traducciones
            CargarMenu();
        }

        // --- LÓGICA DEL SELECTOR DE IDIOMA ---

        private void CargarIdiomas()
        {
            RepeaterIdiomas.DataSource = _traductor.IdiomasDisponibles;
            RepeaterIdiomas.DataBind();
        }



        // --- LÓGICA DE CARGA DE MENÚ (MODIFICADA) ---
        private void CargarMenu()
        {
            string rolUsuario = "Administrador"; // Simulación de rol

            var menuItems = new List<MenuItem>();

            // Usamos el traductor para obtener el texto de cada ítem
            menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_Dashboard"), Url = "~/Consola.aspx" });

            if (rolUsuario == "Administrador")
            {
                menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_Bitacora"), Url = "~/Bitacora.aspx" });
                menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_AdminRoles"), Url = "~/AdminRoles.aspx" });
                menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_AdminEmpresas"), Url = "~/AdminEmpresas.aspx" });
                menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_AdminUsuarios"), Url = "~/AdminUsuarios.aspx" });
                menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_AdminResenas"), Url = "~/AdminResenas.aspx" });
                menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_AdminIdiomas"), Url = "~/AdminIdiomas.aspx" });
                menuItems.Add(new MenuItem { Texto = _traductor.Traducir("AdminMaster_Menu_Link_BackupRestore"), Url = "~/Backup.aspx" });
            }

            // Lógica para marcar el ítem activo (sin cambios)
            string currentPage = Request.AppRelativeCurrentExecutionFilePath;
            foreach (var item in menuItems)
            {
                if (item.Url.Equals(currentPage, StringComparison.OrdinalIgnoreCase))
                {
                    item.CssClass = "nav-link active";
                }
                else
                {
                    item.CssClass = "nav-link";
                }
            }

            rptMenu.DataSource = menuItems;
            rptMenu.DataBind();
        }

        // --- MÉTODOS EXISTENTES (sin cambios) ---

        private void CargarDatosUsuario()
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                lblUserName.Text = HttpContext.Current.User.Identity.Name;
            }
            else
            {
                System.Web.Security.FormsAuthentication.RedirectToLoginPage();
            }
        }

        protected void BtnLogout_Click(object sender, EventArgs e)
        {
            System.Web.Security.FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Login.aspx");
        }
    }
}