using System;
using System.Web.Script.Serialization;

namespace SkillMirror
{
    public partial class Novedades : BasePage
    {
        public string _jsonTraducciones = "{}";

        protected void Page_Load(object sender, EventArgs e) { }

        public override void ActualizarTraducciones()
        {
            this.Title = _traductor.Traducir("Novedades_Page_Title");
            headerTitle.InnerText = _traductor.Traducir("Novedades_Header_Titulo");
            headerSubtitle.InnerText = _traductor.Traducir("Novedades_Header_Subtitulo");

            var traduccionesJS = new
            {
                locale = _traductor.IdiomaSeleccionado.Nombre.ToLower() == "english" ? "en-US" : "es-AR",
                publicadoEl = _traductor.Traducir("Global_PublicadoEl"),
                por = _traductor.Traducir("Global_Por"),
                noHayNovedades = _traductor.Traducir("Novedades_Msg_NoHay")
            };
            _jsonTraducciones = new JavaScriptSerializer().Serialize(traduccionesJS);
        }
    }
}