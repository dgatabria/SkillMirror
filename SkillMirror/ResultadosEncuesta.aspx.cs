using System;
using System.Linq;
using System.Web; // Necesario para HttpUtility
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using BE;
using BLL;

namespace SkillMirror
{
    public partial class ResultadosEncuesta : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarEncuestasDropdown();
                if (Request.QueryString["id"] != null)
                {
                    int encuestaId = Convert.ToInt32(Request.QueryString["id"]);
                    // Asegurarnos de que el valor exista en el dropdown antes de seleccionarlo
                    if (ddlOtrasEncuestas.Items.FindByValue(encuestaId.ToString()) != null)
                    {
                        ddlOtrasEncuestas.SelectedValue = encuestaId.ToString();
                    }
                    CargarResultados(encuestaId);
                }
            }
        }

        public override void ActualizarTraducciones()
        {
            this.Title = _traductor.Traducir("ResultadosEncuesta_Page_Title");
            // El headerTitle se actualiza en CargarResultados
            lblSeleccionarOtra.Text = _traductor.Traducir("ResultadosEncuesta_Label_SeleccionarOtra");

            if (!string.IsNullOrEmpty(ddlOtrasEncuestas.SelectedValue))
            {
                CargarResultados(Convert.ToInt32(ddlOtrasEncuestas.SelectedValue));
            }
        }

        private void CargarEncuestasDropdown()
        {
            BLLEncuesta bll = new BLLEncuesta();
            ddlOtrasEncuestas.DataSource = bll.ListarAdmin();
            ddlOtrasEncuestas.DataTextField = "Titulo";
            ddlOtrasEncuestas.DataValueField = "Codigo";
            ddlOtrasEncuestas.DataBind();
        }

        protected void DdlOtrasEncuestas_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Redirect($"ResultadosEncuesta.aspx?id={ddlOtrasEncuestas.SelectedValue}");
        }

        private void CargarResultados(int encuestaId)
        {
            BLLEncuesta bll = new BLLEncuesta();
            BEResultadoEncuesta resultados = bll.ObtenerResultados(encuestaId);

            if (resultados == null)
            {
                headerTitle.InnerText = _traductor.Traducir("ResultadosEncuesta_Header_Titulo");
                lblTotalRespuestas.Text = "";
                phGraficos.Controls.Clear();
                return;
            }

            headerTitle.InnerText = resultados.TituloEncuesta;
            lblTotalRespuestas.Text = $"{resultados.TotalRespuestas} {_traductor.Traducir("ResultadosEncuesta_Label_Respuestas")}";

            phGraficos.Controls.Clear();

            foreach (var resPregunta in resultados.ResultadosPorPregunta.OrderBy(p => p.PreguntaID))
            {
                var card = new Panel { CssClass = "card mb-4" };
                card.Controls.Add(new LiteralControl($"<div class='card-header'><h5>{HttpUtility.HtmlEncode(resPregunta.TextoPregunta)}</h5></div>"));
                var cardBody = new Panel { CssClass = "card-body" };

                if (resPregunta.TipoPregunta == "TEXTO_LIBRE")
                {
                    
                    var listGroup = new Panel { CssClass = "list-group list-group-flush" };
                    listGroup.Style.Add("max-height", "300px");
                    listGroup.Style.Add("overflow-y", "auto");

                    foreach (string respuesta in resPregunta.RespuestasAbiertas)
                    {
                        listGroup.Controls.Add(new LiteralControl($"<li class='list-group-item'>{HttpUtility.HtmlEncode(respuesta)}</li>"));
                    }
                    cardBody.Controls.Add(listGroup);
                }
                else
                {
                    string chartId = $"chart_{resPregunta.PreguntaID}";
                    cardBody.Controls.Add(new LiteralControl($"<canvas id='{chartId}'></canvas>"));

                    var serializer = new JavaScriptSerializer();
                    string labels = serializer.Serialize(resPregunta.ConteoOpciones.Keys);
                    string data = serializer.Serialize(resPregunta.ConteoOpciones.Values);

                    string script = $@"
                        <script>
                            new Chart(document.getElementById('{chartId}'), {{
                                type: 'bar',
                                data: {{
                                    labels: {labels},
                                    datasets: [{{
                                        label: '{_traductor.Traducir("ResultadosEncuesta_Grafico_Label_Votos")}',
                                        data: {data},
                                        backgroundColor: 'rgba(0, 128, 128, 0.6)',
                                        borderColor: 'rgba(0, 128, 128, 1)',
                                        borderWidth: 1
                                    }}]
                                }},
                                options: {{
                                    indexAxis: 'y',
                                    scales: {{ x: {{ ticks: {{ precision: 0 }} }} }}
                                }}
                            }});
                        </script>";
                    cardBody.Controls.Add(new LiteralControl(script));
                }
                card.Controls.Add(cardBody);
                phGraficos.Controls.Add(card);
            }
        }
    }
}