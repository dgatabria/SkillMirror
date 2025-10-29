using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SkillMirror
{
    public partial class Registro : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Terminos_ServerValidate(object source, ServerValidateEventArgs args)
        {
            // El argumento 'args.IsValid' será true si el checkbox está marcado, y false si no lo está.
            args.IsValid = chkAceptoTerminos.Checked;
        
        }


        public override void ActualizarTraducciones()
        {
            this.Title = _traductor.Traducir("Registro_Page_Title");
            litHeaderTitle.Text = _traductor.Traducir("Registro_Header_Titulo");

            // Formulario
            lblNombre.Text = _traductor.Traducir("Registro_Form_Label_Nombre");
            NombreValidator.ErrorMessage = _traductor.Traducir("Registro_Form_Error_Nombre");
            lblApellido.Text = _traductor.Traducir("Registro_Form_Label_Apellido");
            ApellidoValidator.ErrorMessage = _traductor.Traducir("Registro_Form_Error_Apellido");
            lblDNI.Text = _traductor.Traducir("Registro_Form_Label_DNI");
            DNIRequiredValidator.ErrorMessage = _traductor.Traducir("Registro_Form_Error_DNI");
            lblEmail.Text = _traductor.Traducir("Registro_Form_Label_Email");
            EmailRequiredValidator.ErrorMessage = _traductor.Traducir("Registro_Form_Error_Email_Req");
            EmailFormatValidator.ErrorMessage = _traductor.Traducir("Registro_Form_Error_Email_Format");
            lblPassword.Text = _traductor.Traducir("Registro_Form_Label_Password");
            PasswordRequiredValidator.ErrorMessage = _traductor.Traducir("Registro_Form_Error_Password_Req");
            lblConfirmPassword.Text = _traductor.Traducir("Registro_Form_Label_ConfirmPassword");
            PasswordCompareValidator.ErrorMessage = _traductor.Traducir("Registro_Form_Error_Password_Compare");
            litAceptoTerminos.Text = _traductor.Traducir("Registro_Form_Label_Acepto");
            linkTerminos.Text = _traductor.Traducir("Registro_Form_Link_Terminos");
            TerminosValidator.ErrorMessage = _traductor.Traducir("Registro_Form_Error_Terminos");
            btnRegister.Text = _traductor.Traducir("Registro_Boton_Registrarse");

            ApellidoRegexValidator.ErrorMessage = _traductor.Traducir("Registro_Form_Error_Apellido_Regex");
            NombreRegexValidator.ErrorMessage = _traductor.Traducir("Registro_Form_Error_Nombre_Regex");

            // Panel de Confirmación
            litConfirmacionTitulo.Text = _traductor.Traducir("Registro_Confirm_Titulo");
            litConfirmacionTexto1.Text = _traductor.Traducir("Registro_Confirm_Texto1");
            litConfirmacionTexto2.Text = _traductor.Traducir("Registro_Confirm_Texto2");
            litConfirmacionTexto3.Text = _traductor.Traducir("Registro_Confirm_Texto3");
            linkVolverInicio.Text = _traductor.Traducir("Registro_Confirm_Link_Volver");

            // Footer
            litFooterTexto.Text = _traductor.Traducir("Registro_Footer_Texto");
            linkIniciarSesion.Text = _traductor.Traducir("Registro_Footer_Link_Login");
        }

        public class ReCaptchaResponse
        {
            public bool success { get; set; }
        }
        private bool IsReCaptchaValid()
        {
            try
            {
                // 1. Obtener la respuesta del usuario desde el formulario
                string userResponse = Request.Form["g-recaptcha-response"];
                if (string.IsNullOrEmpty(userResponse))
                {
                    return false;
                }

                // 2. Obtener la clave secreta desde Web.config
                string secretKey = ConfigurationManager.AppSettings["ReCaptchaSecretKey"];

                // 3. Preparar y realizar la petición a la API de Google
                var client = new WebClient();
                string googleReply = client.DownloadString(
                    string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, userResponse)
                );

                // 4. Deserializar la respuesta JSON de Google
                var serializer = new JavaScriptSerializer();
                var reCaptchaResponse = serializer.Deserialize<ReCaptchaResponse>(googleReply);

                // 5. Devolver el resultado de la validación
                return reCaptchaResponse.success;
            }
            catch (Exception)
            {
                // Si algo falla (ej. no hay conexión a Google), la validación no es exitosa.
                return false;
            }
        }

        // BtnRegister_Click
        protected void BtnRegister_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (!IsReCaptchaValid())
                {
                    lblRecaptchaError.Text = _traductor.Traducir("Registro_Recaptcha_Error");
                    return;
                }

                string tokenActivacion = Guid.NewGuid().ToString();
                BEUsuario nu = new BEUsuario
                {
                    Codigo = 0,
                    Nombre = txtNombre.Text,
                    Apellido = txtApellido.Text,
                    Email = txtEmail.Text,
                    Password = txtPassword.Text,
                    DNI = txtDNI.Text,
                    TokenActivacion = tokenActivacion
                };

                BLLUsuario blu = new BLLUsuario();
                bool registroExitoso = blu.Guardar(nu);

                if (registroExitoso)
                {
                    nu = blu.ListarObjeto(nu);
                    string activationUrl = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl($"~/ActivarCuenta.aspx?id={nu.Codigo}&token={tokenActivacion}");

                    // Email traducido
                    string emailDestino = txtEmail.Text;
                    string asunto = _traductor.Traducir("Email_Registro_Asunto");
                    string cuerpo = BLLEmail.ObtenerCuerpoMail(
                        _traductor.Traducir("Email_Registro_Titulo").Replace("{nombre}", nu.Nombre),
                        _traductor.Traducir("Email_Registro_Texto1"),
                        _traductor.Traducir("Email_Registro_Texto2"),
                        _traductor.Traducir("Email_Registro_Boton"),
                        activationUrl);

                    BLLEmail.EnviarMail(emailDestino, asunto, cuerpo);

                    pnlRegistro.Visible = false;
                    pnlConfirmacion.Visible = true;
                    litEmailConfirmacion.Text = emailDestino;
                }
            }
        }

    }
}