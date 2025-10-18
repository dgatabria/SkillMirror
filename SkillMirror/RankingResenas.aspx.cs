using System;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.Script.Serialization;
using BE;
using BLL;

namespace SkillMirror
{
    public partial class RankingResenas : BasePage
    {
        protected string _jsonChartData = "null";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarDatos();
            }
        }

        public override void ActualizarTraducciones()
        {
            this.Title = _traductor.Traducir("Ranking_Page_Title");
            headerTitle.InnerText = _traductor.Traducir("Ranking_Header_Titulo_Admin"); // Usamos un tag nuevo para el admin

            cardPromedioTitle.InnerText = _traductor.Traducir("Ranking_Card_Promedio");
            cardTotalTitle.InnerText = _traductor.Traducir("Ranking_Card_Total");
            graficoTitle.InnerText = _traductor.Traducir("Ranking_Grafico_Titulo");
            destacadasTitle.InnerText = _traductor.Traducir("Ranking_Destacadas_Titulo");

            noDatosTitle.InnerText = _traductor.Traducir("Ranking_NoDatos_Titulo");
            noDatosText.InnerText = _traductor.Traducir("Ranking_NoDatos_Texto");

            CargarDatos();
        }

        private void CargarDatos()
        {
            BLLResena bll = new BLLResena();
            BERankingEstadisticas stats = bll.ObtenerEstadisticas();

            if (stats.TotalResenas > 0)
            {
                pnlDashboard.Visible = true;
                pnlNoHayDatos.Visible = false;

                litPromedio.Text = stats.PromedioPuntuacion.ToString("0.0");
                litTotalResenas.Text = stats.TotalResenas.ToString();

                rptResenasDestacadas.DataSource = stats.ResenasDestacadas;
                rptResenasDestacadas.DataBind();

                var labels = new[] {
                    _traductor.Traducir("Ranking_Grafico_Label_1_Estrella"),
                    _traductor.Traducir("Ranking_Grafico_Label_2_Estrellas"),
                    _traductor.Traducir("Ranking_Grafico_Label_3_Estrellas"),
                    _traductor.Traducir("Ranking_Grafico_Label_4_Estrellas"),
                    _traductor.Traducir("Ranking_Grafico_Label_5_Estrellas")
                };
                var data = new int[5];
                for (int i = 1; i <= 5; i++)
                {
                    data[i - 1] = stats.DistribucionEstrellas.ContainsKey(i) ? stats.DistribucionEstrellas[i] : 0;
                }

                var chartData = new
                {
                    labels = labels,
                    data = data,
                    datasetLabel = _traductor.Traducir("Ranking_Grafico_DatasetLabel")
                };
                _jsonChartData = new JavaScriptSerializer().Serialize(chartData);
            }
            else
            {
                pnlDashboard.Visible = false;
                pnlNoHayDatos.Visible = true;
            }
        }

        public string RenderStars(object puntuacion)
        {
            int p = Convert.ToInt32(puntuacion);
            var sb = new StringBuilder();
            for (int i = 0; i < 5; i++)
            {
                sb.Append(i < p ? "★" : "☆");
            }
            return sb.ToString();
        }
    }
}