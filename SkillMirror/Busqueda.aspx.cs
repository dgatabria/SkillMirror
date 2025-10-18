using BLL;
using System;
using System.Web;

namespace SkillMirror
{
    public partial class Busqueda : BasePage // Hereda de BasePage para el traductor
    {
        // Se ejecuta ANTES del Page_Load
        protected void Page_PreInit(object sender, EventArgs e)
        {
            // Asigna la MasterPage dinámicamente según el estado de autenticación
            if (HttpContext.Current.User.Identity.Name != "" )
            {
                this.MasterPageFile = "~/Admin.master";
            }
            else
            {
                this.MasterPageFile = "~/Site.master";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string termino = Request.QueryString["q"];
                if (!string.IsNullOrEmpty(termino))
                {
                    litTerminoBuscado.Text = Server.HtmlEncode(termino);
                    RealizarBusqueda(termino);
                }
                else
                {
                    pnlNoResultados.Visible = true;
                }
            }
        }

        private void RealizarBusqueda(string termino)
        {
            var bllBusqueda = new BLLBusqueda();
            bool esPublica = !HttpContext.Current.User.Identity.IsAuthenticated;
            int idIdioma = _traductor.IdiomaSeleccionado.Codigo;

            var resultados = bllBusqueda.Buscar(termino, esPublica, idIdioma);

            if (resultados.Count > 0)
            {
                rptResultados.DataSource = resultados;
                rptResultados.DataBind();
            }
            else
            {
                pnlNoResultados.Visible = true;
            }
        }

        public override void ActualizarTraducciones()
        {
            this.Title = _traductor.Traducir("Busqueda_Page_Title");
            litTituloResultados.Text = _traductor.Traducir("Busqueda_Resultados_Para");
            litNoResultados.Text = _traductor.Traducir("Busqueda_Sin_Resultados");
        }
    }
}
