using BE;
using BLL;
using System;
using System.Web;
using System.Web.UI.WebControls;

namespace SkillMirror
{
    public partial class MiVerConversacion : BasePage
    {
        private BLLMensajeria _bllMensajeria;
        private int _conversacionId;
        private BEUsuario _usuarioActual;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(Request.QueryString["id"], out _conversacionId))
            {
                Response.Redirect("~/MisMensajes.aspx");
                return;
            }

            var bllUsuario = new BLLUsuario();
            _usuarioActual = bllUsuario.ListarObjeto(new BEUsuario { Email = HttpContext.Current.User.Identity.Name });
            _bllMensajeria = new BLLMensajeria();

            if (!IsPostBack)
            {
                CargarMensajes();
            }
        }

        public override void ActualizarTraducciones()
        {
            this.Title = _traductor.Traducir("UserVerConversacion_Page_Title");
            headerTitle.InnerText = _traductor.Traducir("UserVerConversacion_Header_Titulo");
            btnVolver.Text = _traductor.Traducir("UserVerConversacion_Boton_Volver");
            txtRespuesta.Attributes["placeholder"] = _traductor.Traducir("UserVerConversacion_Input_Placeholder");
            btnEnviar.Text = _traductor.Traducir("UserVerConversacion_Boton_Enviar");
            rfvRespuesta.ErrorMessage = _traductor.Traducir("UserVerConversacion_Error_MensajeVacio");
        }

        private void CargarMensajes()
        {
            var mensajes = _bllMensajeria.ListarMensajesPorConversacion(_conversacionId, _usuarioActual.Codigo);
            rptMensajes.DataSource = mensajes;
            rptMensajes.DataBind();
        }

        protected void BtnEnviar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                var nuevoMensaje = new BEMensaje
                {
                    ConversacionID = _conversacionId,
                    Remitente = _usuarioActual,
                    CuerpoMensaje = txtRespuesta.Text.Trim()
                };

                _bllMensajeria.EnviarMensaje(nuevoMensaje);
                Response.Redirect(Request.RawUrl);
            }
        }

        public string GetMensajeCssClass(object remitenteIdObj)
        {
            int remitenteId = Convert.ToInt32(remitenteIdObj);
            return remitenteId == _usuarioActual.Codigo ? "mensaje mensaje-propio" : "mensaje mensaje-ajeno";
        }
    }
}