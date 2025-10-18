using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Web;

namespace SkillMirror
{
    public partial class NuevoMensaje : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // La suscripción al traductor se maneja en la BasePage.
        }

        public override void ActualizarTraducciones()
        {
            this.Title = _traductor.Traducir("UserNuevoMensaje_Page_Title");
            headerTitle.InnerText = _traductor.Traducir("UserNuevoMensaje_Header_Titulo");
            lblAsunto.Text = _traductor.Traducir("UserNuevoMensaje_Label_Asunto");
            rfvAsunto.ErrorMessage = _traductor.Traducir("UserNuevoMensaje_Error_Asunto");
            lblMensaje.Text = _traductor.Traducir("UserNuevoMensaje_Label_Mensaje");
            rfvMensaje.ErrorMessage = _traductor.Traducir("UserNuevoMensaje_Error_Mensaje");
            btnEnviar.Text = _traductor.Traducir("UserNuevoMensaje_Boton_Enviar");
            btnCancelar.Text = _traductor.Traducir("UserNuevoMensaje_Boton_Cancelar");
        }

        protected void BtnEnviar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                var bllUsuario = new BLLUsuario();
                var usuarioActual = bllUsuario.ListarObjeto(new BEUsuario { Email = HttpContext.Current.User.Identity.Name });

                var conversacion = new BEConversacion
                {
                    Usuario = usuarioActual,
                    Asunto = txtAsunto.Text.Trim(),
                    Mensajes = new List<BEMensaje>
                    {
                        new BEMensaje { CuerpoMensaje = txtMensaje.Text.Trim(), Remitente = usuarioActual }
                    }
                };

                var bllMensajeria = new BLLMensajeria();
                if (bllMensajeria.CrearConversacion(conversacion) > 0)
                {
                    pnlFormulario.Visible = false;
                    litMensajeExito.Text = $"<div class='alert alert-success'>{_traductor.Traducir("UserNuevoMensaje_Msg_Exito")}</div>";
                }
                else
                {
                    // Manejo de error
                }
            }
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/MisMensajes.aspx");
        }
    }
}