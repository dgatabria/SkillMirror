using BE;
using BLL;
using System;
using System.Web;
using System.Web.UI.WebControls;

namespace SkillMirror
{
    public partial class AdminVerConversacion : BasePage
    {
        private BLLMensajeria _bllMensajeria;
        private int _conversacionId;
        private BEUsuario _usuarioActual;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(Request.QueryString["id"], out _conversacionId))
            {
                Response.Redirect("~/AdminConversaciones.aspx");
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
            this.Title = _traductor.Traducir("AdminVerConversacion_Page_Title");
            headerTitle.InnerText = _traductor.Traducir("AdminVerConversacion_Header_Titulo");
            btnVolver.Text = _traductor.Traducir("AdminVerConversacion_Boton_Volver");
            txtRespuesta.Attributes["placeholder"] = _traductor.Traducir("AdminVerConversacion_Input_Placeholder");
            btnEnviar.Text = _traductor.Traducir("AdminVerConversacion_Boton_Enviar");
            rfvRespuesta.ErrorMessage = _traductor.Traducir("AdminVerConversacion_Error_MensajeVacio");
        }

        private void CargarMensajes()
        {
            var mensajes = _bllMensajeria.ListarMensajesPorConversacion(_conversacionId, _usuarioActual.Codigo);

            if (mensajes.Count > 0)
            {
                // Buscamos el usuario que NO es el administrador actual para mostrar su nombre.
                var usuarioConversacion = mensajes.Find(m => m.Remitente.Codigo != _usuarioActual.Codigo)?.Remitente;
                if (usuarioConversacion != null)
                {
                    litUsuarioConversacion.Text = _traductor.Traducir("AdminVerConversacion_Subtitulo") + usuarioConversacion.Nombre;
                }
            }

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