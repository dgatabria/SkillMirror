using BLL;
using System;
using System.Net.Mail; // Este using ya no es estrictamente necesario aquí
using System.Web.Mail;
using System.Web.UI;

namespace SkillMirror
{
    public partial class Contactenos : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // La suscripción al traductor ya se hace en la BasePage
        }

        public override void ActualizarTraducciones()
        {
            this.Title = _traductor.Traducir("Contactenos_Page_Title");
            headerTitle.InnerText = _traductor.Traducir("Contactenos_Header_Titulo");
            headerSubtitle.InnerText = _traductor.Traducir("Contactenos_Header_Subtitulo");

            // Formulario
            formTitle.InnerText = _traductor.Traducir("Contactenos_Form_Titulo");
            lblNombre.Text = _traductor.Traducir("Contactenos_Form_Label_Nombre");
            valName.ErrorMessage = _traductor.Traducir("Contactenos_Form_Error_Nombre");
            lblEmail.Text = _traductor.Traducir("Contactenos_Form_Label_Email");
            valEmail.ErrorMessage = _traductor.Traducir("Contactenos_Form_Error_Email");
            lblAsunto.Text = _traductor.Traducir("Contactenos_Form_Label_Asunto");
            valSubject.ErrorMessage = _traductor.Traducir("Contactenos_Form_Error_Asunto");
            lblMensaje.Text = _traductor.Traducir("Contactenos_Form_Label_Mensaje");
            valMessage.ErrorMessage = _traductor.Traducir("Contactenos_Form_Error_Mensaje");
            btnSend.Text = _traductor.Traducir("Contactenos_Form_Boton_Enviar");

            // Info de la empresa
            infoTitle.InnerText = _traductor.Traducir("Contactenos_Info_Titulo");
            infoText.InnerText = _traductor.Traducir("Contactenos_Info_Texto");
            litDireccion.Text = _traductor.Traducir("Contactenos_Info_Label_Direccion");
            litTelefono.Text = _traductor.Traducir("Contactenos_Info_Label_Telefono");
            litEmailInfo.Text = _traductor.Traducir("Contactenos_Info_Label_Email");
            litHorario.Text = _traductor.Traducir("Contactenos_Info_Label_Horario");
        }


        protected void BtnSend_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {

                    string emailDestino = "dgatabria@gmail.com";
                    string asunto = $"[Contacto SkillMirror] - {txtSubject.Text}";


                    string cuerpo = $@"
                        <h3>Nuevo Mensaje de Contacto desde SkillMirror.com</h3>
                        <hr>
                        <p><strong>Nombre:</strong> {txtName.Text}</p>
                        <p><strong>Email del Remitente:</strong> {txtEmail.Text}</p>
                        <p><strong>Asunto:</strong> {txtSubject.Text}</p>
                        <hr>
                        <p><strong>Mensaje:</strong></p>
                        <p>{txtMessage.Text.Replace(Environment.NewLine, "<br />")}</p>";


                    bool emailEnviado = BLLEmail.EnviarMail(emailDestino, asunto, cuerpo);

                    if (emailEnviado)
                    {

                        string exitoMsg = _traductor.Traducir("Contactenos_Msg_Exito");
                        litMensaje.Text = $"<div class='alert alert-success'>{exitoMsg}</div>";


                        txtName.Text = string.Empty;
                        txtEmail.Text = string.Empty;
                        txtSubject.Text = string.Empty;
                        txtMessage.Text = string.Empty;
                    }
                    else
                    {

                        string errorMsg = _traductor.Traducir("Contactenos_Msg_Error");
                        litMensaje.Text = $"<div class='alert alert-danger'>{errorMsg}</div>";
                    }
                }
                catch (Exception ex)
                {

                    string errorMsg = _traductor.Traducir("Contactenos_Msg_Error");
                    litMensaje.Text = $"<div class='alert alert-danger'>{errorMsg}</div>";
                    // mostrar el ex.message en la bitácora!!
                }
            }
        }
    }
}