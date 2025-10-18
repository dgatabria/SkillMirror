using BE;  // Necesario para la interfaz ITraducible
using BLL; // Necesario para el Traductor
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;

namespace SkillMirror
{
    // PASO 1: Implementamos la interfaz ITraducible
    public partial class _Default : BasePage
    {
        private BLLEncuesta _bllEncuesta;
        private BLLUsuario _bllUsuario;

        protected void Page_Load(object sender, EventArgs e)
        {
            _bllEncuesta = new BLLEncuesta();
            _bllUsuario = new BLLUsuario();

            if (!IsPostBack)
            {
                CargarEncuesta();
            }
        }

        private void CargarEncuesta()
        {
            // La encuesta solo se muestra si el usuario está logueado
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var encuestaPregunta = _bllEncuesta.ObtenerEncuestaParaPortada();
                if (encuestaPregunta != null)
                {
                    var usuario = _bllUsuario.ListarObjeto(new BEUsuario { Email = HttpContext.Current.User.Identity.Name });

                    // Guardamos los IDs en ViewState para usarlos después
                    ViewState["EncuestaID"] = encuestaPregunta.EncuestaPadre.Codigo;
                    ViewState["PreguntaID"] = encuestaPregunta.Codigo;

                    // Verificamos si el usuario ya respondió
                    if (_bllEncuesta.UsuarioYaRespondio(encuestaPregunta.EncuestaPadre.Codigo, usuario.Codigo))
                    {
                        // Si ya respondió, mostramos los resultados directamente
                        MostrarResultados();
                    }
                    else
                    {
                        // Si no ha respondido, mostramos la pregunta
                        pnlEncuestaFlotante.Visible = true;
                        pnlPregunta.Visible = true;
                        pnlResultados.Visible = false;

                        litEncuestaTitulo.Text = encuestaPregunta.EncuestaPadre.Titulo;
                        litPreguntaTexto.Text = encuestaPregunta.TextoPregunta;
                        rptOpciones.DataSource = encuestaPregunta.Opciones;
                        rptOpciones.DataBind();
                    }
                }
            }
        }

        protected void rptOpciones_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Votar")
            {
                int idOpcion = Convert.ToInt32(e.CommandArgument);
                int idEncuesta = (int)ViewState["EncuestaID"];
                int idPregunta = (int)ViewState["PreguntaID"];
                var usuario = _bllUsuario.ListarObjeto(new BEUsuario { Email = HttpContext.Current.User.Identity.Name });

                // Registramos la respuesta
                _bllEncuesta.RegistrarRespuestaPortada(idEncuesta, usuario.Codigo, idPregunta, idOpcion);

                // Mostramos los resultados
                MostrarResultados();
            }
        }

        private void MostrarResultados()
        {
            int idEncuesta = (int)ViewState["EncuestaID"];
            var resultados = _bllEncuesta.ObtenerResultados(idEncuesta);

            pnlEncuestaFlotante.Visible = true;
            pnlPregunta.Visible = false;
            pnlResultados.Visible = true;

            if (resultados != null && resultados.ResultadosPorPregunta.Any())
            {
                var preguntaResultados = resultados.ResultadosPorPregunta.First();
                litEncuestaTitulo.Text = resultados.TituloEncuesta;

                // Calculamos los porcentajes
                var totalVotos = preguntaResultados.ConteoOpciones.Values.Sum();
                var resultadosConPorcentaje = preguntaResultados.ConteoOpciones.ToDictionary(
                    kvp => kvp.Key,
                    kvp => (totalVotos > 0) ? (int)Math.Round((double)kvp.Value * 100 / totalVotos) : 0
                );

                rptResultados.DataSource = resultadosConPorcentaje;
                rptResultados.DataBind();
            }
        }




        public string Traducir(string tag)
        {
            return _traductor.Traducir(tag);
        }

        public override void ActualizarTraducciones()
        {
            var traductor = Traductor.ObtenerInstancia();

            // Título de la página
            this.Title = traductor.Traducir("Default_Page_Title");

            // Sección 1: Hero
            heroTitle.InnerText = traductor.Traducir("Default_Hero_Titulo_TalentoVerdadero");
            heroSubtitle.InnerText = traductor.Traducir("Default_Hero_Subtitulo_Optimizacion");
            heroButton.Text = traductor.Traducir("Default_Hero_Boton_SolicitarDemo");

            // Sección 2: Características
            uniqueTitle.InnerText = traductor.Traducir("Default_SeccionUnicos_Titulo_QueHaceUnico");
            feature1Title.InnerText = traductor.Traducir("Default_SeccionUnicos_Card1_Titulo");
            feature1Text.InnerText = traductor.Traducir("Default_SeccionUnicos_Card1_Texto");
            feature2Title.InnerText = traductor.Traducir("Default_SeccionUnicos_Card2_Titulo");
            feature2Text.InnerText = traductor.Traducir("Default_SeccionUnicos_Card2_Texto");
            feature3Title.InnerText = traductor.Traducir("Default_SeccionUnicos_Card3_Titulo");
            feature3Text.InnerText = traductor.Traducir("Default_SeccionUnicos_Card3_Texto");
            feature4Title.InnerText = traductor.Traducir("Default_SeccionUnicos_Card4_Titulo");
            feature4Text.InnerText = traductor.Traducir("Default_SeccionUnicos_Card4_Texto");

            // Sección 3: Propuesta de Valor
            valueTitle.InnerText = traductor.Traducir("Default_SeccionValor_Titulo_DecisionesBasadasEnDatos");
            valueText.InnerText = traductor.Traducir("Default_SeccionValor_Texto_ResultadosObjetivos");
            valueButton.Text = traductor.Traducir("Default_SeccionValor_Boton_ConocerMas");

            // Sección 4: Call To Action Final
            ctaTitle.InnerText = traductor.Traducir("Default_SeccionCTA_Titulo_TransformaTuProceso");
            ctaSubtitle.InnerText = traductor.Traducir("Default_SeccionCTA_Texto_DescubreComo");
            ctaButton.Text = traductor.Traducir("Default_SeccionCTA_Boton_ContactarVentas");
        }
    }
}