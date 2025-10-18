using BE;
using BLL;
using System;
using System.Globalization;
using System.Web;

namespace SkillMirror
{
    public partial class DashboardGanancias : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Verificamos el permiso antes de cargar los datos
                var usuario = new BLLUsuario().ListarObjeto(new BEUsuario { Email = HttpContext.Current.User.Identity.Name });
                var bllRbac = new BLLRbac();

                if (bllRbac.UsuarioTienePermiso(usuario, "VER_DASHBOARD_GANANCIAS"))
                {
                    pnlDashboard.Visible = true;
                    pnlAccesoDenegado.Visible = false;
                    CargarEstadisticas();
                }
                else
                {
                    pnlDashboard.Visible = false;
                    pnlAccesoDenegado.Visible = true;
                }
            }
        }

        public override void ActualizarTraducciones()
        {
            this.Title = _traductor.Traducir("DashboardGanancias_Page_Title");
            headerTitle.InnerText = _traductor.Traducir("DashboardGanancias_Header_Titulo");

            // Traducciones para las tarjetas
            cardHoyTitle.InnerText = _traductor.Traducir("DashboardGanancias_Card_Hoy");
            cardSemanaTitle.InnerText = _traductor.Traducir("DashboardGanancias_Card_Semana");
            cardMesTitle.InnerText = _traductor.Traducir("DashboardGanancias_Card_Mes");
            cardAnioTitle.InnerText = _traductor.Traducir("DashboardGanancias_Card_Anio");

            // Traducciones para el mensaje de acceso denegado
            headerAccesoDenegado.InnerText = _traductor.Traducir("Error_AccesoDenegado_Titulo");
            textoAccesoDenegado.InnerText = _traductor.Traducir("Error_AccesoDenegado_Texto");
        }

        private void CargarEstadisticas()
        {
            var bllFacturacion = new BLLFacturacion();
            var estadisticas = bllFacturacion.ObtenerEstadisticasGanancias();

            // Formateamos los números con dos decimales para mostrarlos
            CultureInfo culture = new CultureInfo("en-US"); // Usamos punto como separador decimal
            litGananciasHoy.Text = estadisticas.GananciasHoy.ToString("N2", culture);
            litGananciasSemana.Text = estadisticas.GananciasSemana.ToString("N2", culture);
            litGananciasMes.Text = estadisticas.GananciasMes.ToString("N2", culture);
            litGananciasAnio.Text = estadisticas.GananciasAnio.ToString("N2", culture);
        }
    }
}
