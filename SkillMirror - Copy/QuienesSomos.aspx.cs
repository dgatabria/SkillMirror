using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;

namespace SkillMirror
{
    public partial class QuienesSomos : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // La suscripción al traductor ya se maneja en la BasePage
        }

        public override void ActualizarTraducciones()
        {
            this.Title = _traductor.Traducir("QuienesSomos_Page_Title");
            headerTitle.InnerText = _traductor.Traducir("QuienesSomos_Header_Titulo");
            headerSubtitle.InnerText = _traductor.Traducir("QuienesSomos_Header_Subtitulo");

            // Nuestra Esencia
            esenciaTitle.InnerText = _traductor.Traducir("QuienesSomos_Esencia_Titulo");
            esenciaText1.InnerHtml = _traductor.Traducir("QuienesSomos_Esencia_Texto1"); // Usamos InnerHtml para mantener el <strong>
            esenciaText2.InnerText = _traductor.Traducir("QuienesSomos_Esencia_Texto2");

            // Nuestra Misión
            misionTitle.InnerText = _traductor.Traducir("QuienesSomos_Mision_Titulo");
            misionText.InnerText = _traductor.Traducir("QuienesSomos_Mision_Texto");

            // Propuesta de Valor
            valorTitle.InnerText = _traductor.Traducir("QuienesSomos_Valor_Titulo");
            valorText.InnerHtml = _traductor.Traducir("QuienesSomos_Valor_Texto"); // Usamos InnerHtml para mantener el <strong>
        }
    }
}