using BE;
using BLL;
using System;
using System.Configuration;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SkillMirror
{
    public partial class RecuperarClave : BasePage
    {
        public override void ActualizarTraducciones()
        {
            this.Title = _traductor.Traducir("RecuperarClave_Page_Title");
            litHeaderTitle.Text = _traductor.Traducir("RecuperarClave_Header_Titulo");

            // Panel de solicitud
            litFormText.Text = _traductor.Traducir("RecuperarClave_Form_Texto");
            lblEmail.Text = _traductor.Traducir("RecuperarClave_Form_Label_Email");
            EmailRequiredValidator.ErrorMessage = _traductor.Traducir("RecuperarClave_Form_Error_Email_Req");
            EmailFormatValidator.ErrorMessage = _traductor.Traducir("RecuperarClave_Form_Error_Email_Format");
            btnSendRecovery.Text = _traductor.Traducir("RecuperarClave_Form_Boton_Enviar");

            // Panel de éxito
            litSuccessTitle.Text = _traductor.Traducir("RecuperarClave_Exito_Titulo");
            litSuccessText.Text = _traductor.Traducir("RecuperarClave_Exito_Texto");

            // Link inferior
            linkVolverLogin.Text = _traductor.Traducir("RecuperarClave_Link_VolverLogin");
        }

        public class ReCaptchaResponse { public bool success { get; set; } }

        private bool IsReCaptchaValid()
        {
            try
            {
                string userResponse = Request.Form["g-recaptcha-response"];
                if (string.IsNullOrEmpty(userResponse)) return false;

                string secretKey = ConfigurationManager.AppSettings["ReCaptchaSecretKey"];
                var client = new WebClient();
                string googleReply = client.DownloadString($"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={userResponse}");
                var serializer = new JavaScriptSerializer();
                var reCaptchaResponse = serializer.Deserialize<ReCaptchaResponse>(googleReply);
                return reCaptchaResponse.success;
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected void BtnSendRecovery_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (!IsReCaptchaValid())
                {
                    lblRecaptchaError.Text = _traductor.Traducir("RecuperarClave_Recaptcha_Error");
                    return;
                }
                try
                {
                    BLLUsuario bllUsuario = new BLLUsuario();
                    BEUsuario usuarioFiltro = new BEUsuario { Email = txtEmail.Text };
                    BEUsuario usuarioExistente = bllUsuario.ListarObjeto(usuarioFiltro);

                    if (usuarioExistente != null)
                    {
                        string tokenRecupero = Guid.NewGuid().ToString();
                        usuarioExistente.TokenRecupero = tokenRecupero;
                        bllUsuario.Guardar(usuarioExistente);

                        string recoveryUrl = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl($"~/ResetPassword.aspx?id={usuarioExistente.Codigo}&token={tokenRecupero}");

                        // Contenido del email traducido
                        string asunto = _traductor.Traducir("Email_RecuperoClave_Asunto");
                        string cuerpo = BLLEmail.ObtenerCuerpoMail(
                           _traductor.Traducir("Email_RecuperoClave_Cuerpo_Hola").Replace("{nombre}", usuarioExistente.Nombre),
                           _traductor.Traducir("Email_RecuperoClave_Cuerpo_Texto1"),
                           _traductor.Traducir("Email_RecuperoClave_Cuerpo_Texto2"),
                           _traductor.Traducir("Email_RecuperoClave_Cuerpo_Boton"),
                           recoveryUrl);

                        BLLEmail.EnviarMail(usuarioExistente.Email, asunto, cuerpo);
                    }

                    pnlRequest.Visible = false;
                    pnlSuccess.Visible = true;
                }
                catch (Exception ex)
                {
                    // Manejar el error
                }
            }
        }
    }
}