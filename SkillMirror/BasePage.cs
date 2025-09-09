using System;
using System.Web.UI;
using BLL;
using BE;
using System.Collections.Specialized; // Necesario para NameValueCollection
using System.Web; // Necesario para HttpUtility

namespace SkillMirror
{
    public class BasePage : Page, ITraducible
    {
        protected Traductor _traductor;

        protected override void OnInit(EventArgs e)
        {
            _traductor = Traductor.ObtenerInstancia();

            if (Request.QueryString["langid"] != null)
            {
                int idIdiomaUrl = Convert.ToInt32(Request.QueryString["langid"]);
                int idIdiomaActual = _traductor.IdiomaSeleccionado?.Codigo ?? 0;

                // Solo cambiamos el idioma y redirigimos si es un idioma NUEVO
                if (idIdiomaUrl != idIdiomaActual)
                {
                    _traductor.CambiarIdioma(idIdiomaUrl);

                    // --- ¡LA SOLUCIÓN! ---
                    // Reconstruimos la URL manteniendo todos los otros parámetros
                    NameValueCollection query = HttpUtility.ParseQueryString(Request.QueryString.ToString());
                    query.Set("langid", idIdiomaUrl.ToString());
                    string newUrl = Request.Url.AbsolutePath + "?" + query;
                    Response.Redirect(newUrl, true);
                }
            }

            _traductor.Suscribir(this);
            base.OnInit(e);
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (_traductor != null)
            {
                _traductor.Desuscribir(this);
            }
        }

        public virtual void ActualizarTraducciones()
        {
            // Lógica específica en cada página hija
        }
    }
}