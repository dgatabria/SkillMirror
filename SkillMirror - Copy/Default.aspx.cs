using System;
using System.Web.UI;
using BLL; // Necesario para el Traductor
using BE;  // Necesario para la interfaz ITraducible

namespace SkillMirror
{
    // PASO 1: Implementamos la interfaz ITraducible
    public partial class _Default : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // nada

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