using BE;
using BLL;
using System;
using System.Configuration;
using System.Net;
using System.Web.Script.Serialization;
using System.Web.UI;

namespace SkillMirror
{
    public partial class ResetPassword : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string token = Request.QueryString["token"];
                string sID = Request.QueryString["id"];
                BLLUsuario bllUsuario = new BLLUsuario();

                try
                {
                    int id = Convert.ToInt32(sID);
                    BEUsuario usuario = bllUsuario.ListarObjeto(new BEUsuario { Codigo = id });

                    if (usuario == null || !bllUsuario.ValidarTokenRecuperacion(usuario, token))
                    {
                        pnlFormulario.Visible = false;
                        pnlResultado.Visible = true;
                        litResultado.Text = $"<div class='alert alert-danger'>{_traductor.Traducir("ResetPassword_Msg_Error_TokenInvalido")}</div>";
                    }
                    else
                    {
                        pnlFormulario.Visible = true;
                        pnlResultado.Visible = false;
                        hfToken.Value = token;
                    }
                }
                catch (Exception)
                {
                    pnlFormulario.Visible = false;
                    pnlResultado.Visible = true;
                    litResultado.Text = $"<div class='alert alert-danger'>{_traductor.Traducir("ResetPassword_Msg_Error_Inesperado")}</div>";
                }
            }
        }

        public override void ActualizarTraducciones()
        {
            this.Title = _traductor.Traducir("ResetPassword_Page_Title");
            litHeaderTitle.Text = _traductor.Traducir("ResetPassword_Header_Titulo");

            // Formulario
            lblPassword.Text = _traductor.Traducir("ResetPassword_Form_Label_Password");
            PasswordRequiredValidator.ErrorMessage = _traductor.Traducir("ResetPassword_Form_Error_Password_Req");
            lblConfirmPassword.Text = _traductor.Traducir("ResetPassword_Form_Label_ConfirmPassword");
            PasswordCompareValidator.ErrorMessage = _traductor.Traducir("ResetPassword_Form_Error_Password_Compare");
            btnReset.Text = _traductor.Traducir("ResetPassword_Form_Boton_Guardar");
        }

        protected void BtnReset_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    string token = hfToken.Value;
                    string nuevaPassword = txtPassword.Text;
                    BLLUsuario bllUsuario = new BLLUsuario();
                    BEUsuario beu = new BEUsuario { Codigo = Convert.ToInt32(Request.QueryString["id"]), Password = nuevaPassword };

                    bool reseteoExitoso = bllUsuario.ResetearPassword(beu);

                    pnlFormulario.Visible = false;
                    pnlResultado.Visible = true;

                    if (reseteoExitoso)
                    {
                        string exitoMsg = _traductor.Traducir("ResetPassword_Msg_Exito");
                        string botonLogin = _traductor.Traducir("ResetPassword_Boton_IrLogin");
                        string urlLogin = Page.ResolveUrl("~/Login.aspx");
                        litResultado.Text = $"<div class='alert alert-success'>{exitoMsg}</div>"
                                          + $"<div class='text-center mt-3'><a href='{urlLogin}' class='btn btn-primary'>{botonLogin}</a></div>";
                    }
                    else
                    {
                        litResultado.Text = $"<div class='alert alert-danger'>{_traductor.Traducir("ResetPassword_Msg_Error_NoSePudoActualizar")}</div>";
                    }
                }
                catch (Exception)
                {
                    pnlFormulario.Visible = false;
                    pnlResultado.Visible = true;
                    litResultado.Text = $"<div class='alert alert-danger'>{_traductor.Traducir("ResetPassword_Msg_Error_Inesperado")}</div>";
                }
            }
        }
    }
}