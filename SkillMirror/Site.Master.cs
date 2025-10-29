using System;
using System.Web;
using BLL;
using BE;
using System.Web.UI.WebControls; // Necesario para Label

namespace SkillMirror
{
    public partial class SiteMaster : System.Web.UI.MasterPage, ITraducible
    {
        private Traductor _traductor;

        protected void Page_Init(object sender, EventArgs e)
        {
            // Manejo de cambio de idioma (sin cambios)
            if (Request.QueryString["langid"] != null)
            {
                if (int.TryParse(Request.QueryString["langid"], out int idiomaId))
                {
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
                // Llamamos explícitamente para asegurar la traducción inicial
                ActualizarTraducciones();
            }

            // Visibilidad de paneles login/usuario (sin cambios)
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
            // Traducciones existentes (sin cambios)
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
            txtBusquedaGlobal.Attributes["placeholder"] = _traductor.Traducir("Busqueda_Placeholder");
            btnBusquedaGlobal.Text = _traductor.Traducir("Busqueda_Boton");

            // --- TRADUCCIÓN PARA LA ETIQUETA DEL TOGGLE DE ACCESIBILIDAD ---
            var lblFocus = FindControl("lblEnhancedFocus") as Label;
            if (lblFocus != null)
            {
                // Usamos la misma tag que en Admin.Master
                lblFocus.Text = _traductor.Traducir("AdminMaster_Accesibilidad_Foco_Label");
            }
            // ----------------------------------------------------------------
        }

        protected void BtnLogout_Click(object sender, EventArgs e)
        {
            // Logout (sin cambios)
            System.Web.Security.FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Login.aspx");
        }

        protected void btnBusquedaGlobal_Click(object sender, EventArgs e)
        {
            // Búsqueda (sin cambios)
            if (!string.IsNullOrWhiteSpace(txtBusquedaGlobal.Text))
            {
                Response.Redirect($"~/Busqueda.aspx?q={Server.UrlEncode(txtBusquedaGlobal.Text)}");
            }
        }
    }
}
