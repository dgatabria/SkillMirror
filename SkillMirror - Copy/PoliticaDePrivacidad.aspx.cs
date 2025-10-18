using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;

namespace SkillMirror
{
    public partial class PoliticaDePrivacidad : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // La suscripción al traductor se maneja en la BasePage
        }

        public override void ActualizarTraducciones()
        {
            this.Title = _traductor.Traducir("Politica_Page_Title");
            headerTitle.InnerText = _traductor.Traducir("Politica_Header_Titulo");
            headerSubtitle.InnerText = _traductor.Traducir("Politica_Header_Subtitulo");

            litIntro.Text = _traductor.Traducir("Politica_Intro");

            // Sección 1
            litTitulo1.Text = _traductor.Traducir("Politica_Titulo_1");
            litTexto1.Text = _traductor.Traducir("Politica_Texto_1");

            // Sección 2
            litTitulo2.Text = _traductor.Traducir("Politica_Titulo_2");
            litTexto2_intro.Text = _traductor.Traducir("Politica_Texto_2_Intro");
            litTexto2_li1_strong.Text = _traductor.Traducir("Politica_Texto_2_Li1_Strong");
            litTexto2_li1_text.Text = _traductor.Traducir("Politica_Texto_2_Li1_Text");
            litTexto2_li2_strong.Text = _traductor.Traducir("Politica_Texto_2_Li2_Strong");
            litTexto2_li2_text.Text = _traductor.Traducir("Politica_Texto_2_Li2_Text");

            // Sección 3
            litTitulo3.Text = _traductor.Traducir("Politica_Titulo_3");
            litTexto3_intro.Text = _traductor.Traducir("Politica_Texto_3_Intro");
            litTexto3_li1.Text = _traductor.Traducir("Politica_Texto_3_Li1");
            litTexto3_li2.Text = _traductor.Traducir("Politica_Texto_3_Li2");
            litTexto3_li3.Text = _traductor.Traducir("Politica_Texto_3_Li3");
            litTexto3_li4.Text = _traductor.Traducir("Politica_Texto_3_Li4");

            // Sección 4
            litTitulo4.Text = _traductor.Traducir("Politica_Titulo_4");
            litTexto4.Text = _traductor.Traducir("Politica_Texto_4");

            // Sección 5
            litTitulo5.Text = _traductor.Traducir("Politica_Titulo_5");
            litTexto5.Text = _traductor.Traducir("Politica_Texto_5");

            // Sección 6
            litTitulo6.Text = _traductor.Traducir("Politica_Titulo_6");
            string emailLink = "<a href='mailto:privacidad@skillmirror.com.ar'>privacidad@skillmirror.com.ar</a>";
            litTexto6.Text = _traductor.Traducir("Politica_Texto_6").Replace("{email}", emailLink);

            // Sección 7
            litTitulo7.Text = _traductor.Traducir("Politica_Titulo_7");
            litTexto7.Text = _traductor.Traducir("Politica_Texto_7");
        }
    }
}