using System;
using System.Web;
using BLL;
using BE;

namespace SkillMirror
{
    public partial class SiteMaster : System.Web.UI.MasterPage, ITraducible
    {
        private Traductor _traductor;

        protected void Page_Init(object sender, EventArgs e)
        {
            // Este bloque maneja el cambio de idioma.
            // Si se detecta el parámetro 'langid' en la URL, se cambia el idioma y se recarga la página.
            if (Request.QueryString["langid"] != null)
            {
                if (int.TryParse(Request.QueryString["langid"], out int idiomaId))
                {
                    // Solo cambiamos el idioma y recargamos si el ID es diferente al actual
                    // para evitar bucles de recarga infinitos.
                    if ((Traductor.ObtenerInstancia().IdiomaSeleccionado?.Codigo ?? 0) != idiomaId)
                    {
                        Traductor.ObtenerInstancia().CambiarIdioma(idiomaId);
                        Response.Redirect(Request.Url.AbsolutePath, true);
                    }
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _traductor = Traductor.ObtenerInstancia();
            _traductor.Suscribir(this);

            if (!IsPostBack)
            {
                CargarIdiomas();
            }

            // Muestra u oculta los paneles de login/usuario según el estado de autenticación
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                pnlAnonymous.Visible = false;
                pnlAuthenticated.Visible = true;
                lblUserName.Text = HttpContext.Current.User.Identity.Name;
            }
            else
            {
                pnlAnonymous.Visible = true;
                pnlAuthenticated.Visible = false;
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (_traductor != null) { _traductor.Desuscribir(this); }
        }

        private void CargarIdiomas()
        {
            RepeaterIdiomas.DataSource = _traductor.IdiomasDisponibles;
            RepeaterIdiomas.DataBind();
        }

        public void ActualizarTraducciones()
        {
            // Traducciones de los elementos de navegación existentes
            navInicio.InnerText = _traductor.Traducir("SiteMaster_Menu_Link_Inicio");
            navQuienesSomos.InnerText = _traductor.Traducir("SiteMaster_Menu_Link_QuienesSomos");
            navServicios.InnerText = _traductor.Traducir("SiteMaster_Menu_Link_Servicios");
            navContacto.InnerText = _traductor.Traducir("SiteMaster_Menu_Link_Contactenos");
            navResenas.InnerText = _traductor.Traducir("SiteMaster_Menu_Link_Resenas");
            navLogin.InnerText = _traductor.Traducir("SiteMaster_Menu_Boton_Login");
            litIdioma.Text = _traductor.Traducir("SiteMaster_Menu_Link_Idioma");
            btnLogout.Text = _traductor.Traducir("SiteMaster_MenuUsuario_Boton_CerrarSesion");
            areaderechos.InnerText = _traductor.Traducir("SiteMaster_Footer_Texto_DerechosReservados");
            areapolitica.InnerText = _traductor.Traducir("SiteMaster_Footer_Link_PoliticaPrivacidad");
            navNovedades.InnerText = _traductor.Traducir("SiteMaster_Menu_Link_Novedades");
            navFAQs.InnerText = _traductor.Traducir("SiteMaster_Menu_Link_FAQs");

            // --- TRADUCCIONES PARA LA BÚSQUEDA ---
            txtBusquedaGlobal.Attributes["placeholder"] = _traductor.Traducir("Busqueda_Placeholder");
            btnBusquedaGlobal.Text = _traductor.Traducir("Busqueda_Boton");
        }

        protected void BtnLogout_Click(object sender, EventArgs e)
        {
            System.Web.Security.FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Login.aspx");
        }

        // --- EVENTO PARA EL BOTÓN DE BÚSQUEDA ---
        protected void btnBusquedaGlobal_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtBusquedaGlobal.Text))
            {
                // Codificamos el término de búsqueda para pasarlo de forma segura en la URL
                Response.Redirect($"~/Busqueda.aspx?q={Server.UrlEncode(txtBusquedaGlobal.Text)}");
            }
        }
    }
}