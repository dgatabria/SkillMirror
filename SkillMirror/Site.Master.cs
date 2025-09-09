using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL; 
using BE;  

namespace SkillMirror
{

    public partial class SiteMaster : MasterPage, ITraducible
    {
        private Traductor _traductor;

        protected void Page_Init(object sender, EventArgs e)
        {
            // Verificamos si este PostBack fue causado por nuestro Repeater
            if (Request.Form["__EVENTTARGET"] != null && Request.Form["__EVENTTARGET"].Contains("RepeaterIdiomas"))
            {
                // Extraemos el ID del idioma del argumento del evento
                string commandArgument = Request.Form["__EVENTARGUMENT"];
                if (int.TryParse(commandArgument, out int idiomaId))
                {
                    // Obtenemos la instancia y cambiamos el idioma INMEDIATAMENTE
                    Traductor.ObtenerInstancia().CambiarIdioma(idiomaId);
                    // Forzamos una redirección para limpiar el estado del PostBack
                    Response.Redirect(Request.RawUrl, true);
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
            // Ahora bindeamos los datos al Repeater
            RepeaterIdiomas.DataSource = _traductor.IdiomasDisponibles;
            RepeaterIdiomas.DataBind();
        }





        public void ActualizarTraducciones()
        {

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
        }

        protected void BtnLogout_Click(object sender, EventArgs e)
        {
            System.Web.Security.FormsAuthentication.SignOut();
            // Limpiamos la sesión para destruir la instancia del traductor
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Login.aspx");
        }


    }
}