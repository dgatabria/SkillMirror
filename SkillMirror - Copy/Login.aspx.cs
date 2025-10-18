using BE;
using BLL;
using System;
using System.Configuration;
using System.Net;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace SkillMirror
{
    public partial class Login : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // La suscripción se maneja en BasePage
        }

        public override void ActualizarTraducciones()
        {
            this.Title = _traductor.Traducir("Login_Page_Title");
            litHeaderTitle.Text = _traductor.Traducir("Login_Header_Titulo");

            // Formulario
            lblEmail.Text = _traductor.Traducir("Login_Form_Label_Email");
            EmailValidator.ErrorMessage = _traductor.Traducir("Login_Form_Error_Email");
            lblPassword.Text = _traductor.Traducir("Login_Form_Label_Password");
            PasswordValidator.ErrorMessage = _traductor.Traducir("Login_Form_Error_Password");
            btnLogin.Text = _traductor.Traducir("Login_Boton_Acceder");
            linkOlvidoPassword.Text = _traductor.Traducir("Login_Link_OlvidoPassword");

            // Footer
            litFooterText.Text = _traductor.Traducir("Login_Footer_Texto");
            linkRegistro.Text = _traductor.Traducir("Login_Footer_Link_Registro");
        }

        public class ReCaptchaResponse
        {
            public bool success { get; set; }
        }

        private bool IsReCaptchaValid()
        {
            try
            {
                string userResponse = Request.Form["g-recaptcha-response"];
                if (string.IsNullOrEmpty(userResponse))
                {
                    return false;
                }
                string secretKey = ConfigurationManager.AppSettings["ReCaptchaSecretKey"];
                var client = new WebClient();
                string googleReply = client.DownloadString(
                    string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, userResponse)
                );
                var serializer = new JavaScriptSerializer();
                var reCaptchaResponse = serializer.Deserialize<ReCaptchaResponse>(googleReply);
                return reCaptchaResponse.success;
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            if (!IsReCaptchaValid())
            {
                lblRecaptchaError.Text = _traductor.Traducir("Login_Recaptcha_Error");
                return;
            }

            BEUsuario beu = new BEUsuario();
            BLLogin blo = new BLLogin();
            beu.Email = txtEmail.Text;

            int loginRetCode = blo.doLogin(beu, txtPassword.Text);

            switch (loginRetCode)
            {
                case 0:
                    Session["UserName"] = txtEmail.Text;
                    System.Web.Security.FormsAuthentication.SetAuthCookie(txtEmail.Text, false);
                    Response.Redirect(Page.ResolveUrl("~/Consola.aspx"));
                    break;
                case 1:
                    litErrorMessage.Text = $"<div class='alert alert-danger'>{_traductor.Traducir("Login_Msg_Error_Credenciales")}</div>";
                    break;
                case 2:
                    litErrorMessage.Text = $"<div class='alert alert-danger'>{_traductor.Traducir("Login_Msg_Error_Bloqueado")}</div>";
                    break;
                case 3:
                    litErrorMessage.Text = $"<div class='alert alert-danger'>{_traductor.Traducir("Login_Msg_Error_NoActivado")}</div>";
                    break;
            }
        }
    }
}