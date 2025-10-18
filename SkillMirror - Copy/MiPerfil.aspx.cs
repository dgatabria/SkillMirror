using System;
using System.Web;
using System.Web.UI;
using BE;
using BLL;

namespace SkillMirror
{
    public partial class MiPerfil : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarDatosUsuario();
            }
        }

        public override void ActualizarTraducciones()
        {
            this.Title = _traductor.Traducir("MiPerfil_Page_Title");
            headerTitle.InnerText = _traductor.Traducir("MiPerfil_Header_Titulo");

            // Card Datos Personales
            cardDatosTitle.InnerText = _traductor.Traducir("MiPerfil_CardDatos_Titulo");
            lblNombre.Text = _traductor.Traducir("MiPerfil_CardDatos_Label_Nombre");
            valNombre.ErrorMessage = _traductor.Traducir("MiPerfil_CardDatos_Error_Nombre");
            lblApellido.Text = _traductor.Traducir("MiPerfil_CardDatos_Label_Apellido");
            valApellido.ErrorMessage = _traductor.Traducir("MiPerfil_CardDatos_Error_Apellido");
            lblDNI.Text = _traductor.Traducir("MiPerfil_CardDatos_Label_DNI");
            valDNI.ErrorMessage = _traductor.Traducir("MiPerfil_CardDatos_Error_DNI");
            btnGuardarDatos.Text = _traductor.Traducir("MiPerfil_CardDatos_Boton_Guardar");
            lblSuscrito.Text = _traductor.Traducir("MiPerfil_Label_Suscrito");
            lblSuscritoEncuestas.Text = _traductor.Traducir("MiPerfil_Label_Suscrito_Encuestas");

            // Card Cambiar Contraseña
            cardPasswordTitle.InnerText = _traductor.Traducir("MiPerfil_CardPass_Titulo");
            lblPassActual.Text = _traductor.Traducir("MiPerfil_CardPass_Label_Actual");
            valPassActual.ErrorMessage = _traductor.Traducir("MiPerfil_CardPass_Error_Actual");
            lblPassNueva.Text = _traductor.Traducir("MiPerfil_CardPass_Label_Nueva");
            valPassNueva.ErrorMessage = _traductor.Traducir("MiPerfil_CardPass_Error_Nueva");
            lblConfirmPassNueva.Text = _traductor.Traducir("MiPerfil_CardPass_Label_Confirmar");
            valComparePass.ErrorMessage = _traductor.Traducir("MiPerfil_CardPass_Error_Confirmar");
            btnCambiarPassword.Text = _traductor.Traducir("MiPerfil_CardPass_Boton_Cambiar");
        }

        private void CargarDatosUsuario()
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                BLLUsuario bll = new BLLUsuario();
                // Obtenemos el usuario completo a partir del email guardado en la sesión de autenticación
                BEUsuario usuario = bll.ListarObjeto(new BEUsuario { Email = HttpContext.Current.User.Identity.Name });
                if (usuario != null)
                {
                    // Guardamos el ID en un ViewState para no perderlo entre postbacks
                    ViewState["UsuarioID"] = usuario.Codigo;
                    txtNombre.Text = usuario.Nombre;
                    txtApellido.Text = usuario.Apellido;
                    txtDNI.Text = usuario.DNI;
                    chkSuscrito.Checked = usuario.SuscritoNewsletter;
                    chkSuscritoEncuestas.Checked = usuario.SuscritoEncuestas;
                }
            }
        }

        protected void BtnGuardarDatos_Click(object sender, EventArgs e)
        {
            if (Page.IsValid && ViewState["UsuarioID"] != null)
            {
                BLLUsuario bll = new BLLUsuario();
                int usuarioId = (int)ViewState["UsuarioID"];

                BEUsuario usuario = bll.ListarObjeto(new BEUsuario { Codigo = usuarioId });
                if (usuario != null)
                {
                    usuario.Nombre = txtNombre.Text.Trim();
                    usuario.Apellido = txtApellido.Text.Trim();
                    usuario.DNI = txtDNI.Text.Trim();
                    usuario.SuscritoNewsletter = chkSuscrito.Checked;
                    usuario.SuscritoEncuestas = chkSuscritoEncuestas.Checked;
                    bll.Guardar(usuario);

                    litMensaje.Text = $"<div class='alert alert-success'>{_traductor.Traducir("MiPerfil_Msg_Exito_Datos")}</div>";
                }
            }
        }

        protected void BtnCambiarPassword_Click(object sender, EventArgs e)
        {
            if (Page.IsValid && ViewState["UsuarioID"] != null)
            {
                BLLUsuario bll = new BLLUsuario();
                int usuarioId = (int)ViewState["UsuarioID"];

                BEUsuario usuario = new BEUsuario { Codigo = usuarioId, Password = txtPassNueva.Text };
                string passActual = txtPassActual.Text;

                try
                {
                    if (bll.CambiarPassword(usuario, passActual))
                    {
                        litMensaje.Text = $"<div class='alert alert-success'>{_traductor.Traducir("MiPerfil_Msg_Exito_Password")}</div>";
                        txtPassActual.Text = "";
                        txtPassNueva.Text = "";
                        txtConfirmPassNueva.Text = "";

                        BEUsuario usuarioCompleto = bll.ListarObjeto(new BEUsuario { Codigo = usuarioId });
                        if (usuarioCompleto != null)
                        {
                            string asunto = _traductor.Traducir("Email_PassCambiada_Asunto");
                            string cuerpo = BLLEmail.ObtenerCuerpoMail(
                                _traductor.Traducir("Email_PassCambiada_Titulo").Replace("{nombre}", usuarioCompleto.Nombre),
                                _traductor.Traducir("Email_PassCambiada_Texto1"),
                                _traductor.Traducir("Email_PassCambiada_Texto2"),
                                _traductor.Traducir("Email_PassCambiada_Boton"),
                                Page.ResolveUrl("~/Login.aspx")); // El botón lleva al login

                            BLLEmail.EnviarMail(usuarioCompleto.Email, asunto, cuerpo);
                        }
                    }
                    else
                    {
                        litMensaje.Text = $"<div class='alert alert-danger'>{_traductor.Traducir("MiPerfil_Msg_Error_PasswordIncorrecta")}</div>";
                    }
                }
                catch (Exception ex)
                {
                    litMensaje.Text = $"<div class='alert alert-danger'>{_traductor.Traducir("MiPerfil_Msg_Error_PasswordGeneral")}: {ex.Message}</div>";
                }
            }
        }
    }
}