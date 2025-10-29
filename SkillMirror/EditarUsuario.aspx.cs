using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using BE;
using System.Web;

namespace SkillMirror
{
    public partial class EditarUsuario : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarRoles();

                if (Request.QueryString["id"] != null)
                {
                    int usuarioId = Convert.ToInt32(Request.QueryString["id"]);
                    hfUsuarioId.Value = usuarioId.ToString();
                    btnResetPassword.Visible = true;
                    CargarDatosUsuario(usuarioId);
                }
            }
        }

        public override void ActualizarTraducciones()
        {
            this.Title = _traductor.Traducir("EditarUsuario_Page_Title");

            if (Request.QueryString["id"] != null)
            {
                litTitulo.Text = _traductor.Traducir("EditarUsuario_Titulo_Editar");
            }
            else
            {
                litTitulo.Text = _traductor.Traducir("EditarUsuario_Titulo_Crear");
            }

            lblNombre.Text = _traductor.Traducir("EditarUsuario_Label_Nombre");
            lblApellido.Text = _traductor.Traducir("EditarUsuario_Label_Apellido");
            lblEmail.Text = _traductor.Traducir("EditarUsuario_Label_Email");
            lblDNI.Text = _traductor.Traducir("EditarUsuario_Label_DNI");
            lblRoles.Text = _traductor.Traducir("EditarUsuario_Label_Roles");
            lblBloqueado.Text = _traductor.Traducir("EditarUsuario_Label_Bloqueado");
            btnGuardar.Text = _traductor.Traducir("EditarUsuario_Boton_Guardar");
            btnCancelar.Text = _traductor.Traducir("EditarUsuario_Boton_Cancelar");
            btnResetPassword.Text = _traductor.Traducir("EditarUsuario_Boton_ResetPassword");
        }

        private void CargarRoles()
        {
            BLLRbac bllRbac = new BLLRbac();
            cblRoles.DataSource = bllRbac.ListarRoles();
            cblRoles.DataTextField = "Nombre";
            cblRoles.DataValueField = "Codigo";
            cblRoles.DataBind();
        }

        private void CargarDatosUsuario(int id)
        {
            BLLUsuario bllUsuario = new BLLUsuario();
            BEUsuario usuario = bllUsuario.ListarObjeto(new BEUsuario { Codigo = id });

            if (usuario != null)
            {
                txtNombre.Text = usuario.Nombre;
                txtApellido.Text = usuario.Apellido;
                txtEmail.Text = usuario.Email;
                chkBloqueado.Checked = Convert.ToBoolean(usuario.Bloqueado);

                foreach (ListItem item in cblRoles.Items)
                {
                    item.Selected = usuario.Roles.Exists(r => r.Codigo.ToString() == item.Value);
                }
            }
        }

        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            BLLUsuario bllUsuario = new BLLUsuario();
            BEUsuario usuario;
            int usuarioId = Convert.ToInt32(hfUsuarioId.Value);

            if (usuarioId > 0)
            {
                usuario = bllUsuario.ListarObjeto(new BEUsuario { Codigo = usuarioId });
            }
            else
            {
                usuario = new BEUsuario();
                usuario.TokenRecupero = Guid.NewGuid().ToString();
                BEUsuario yo = bllUsuario.ListarObjeto(new BEUsuario() { Email = HttpContext.Current.User.Identity.Name });
                usuario.Empresa = yo.Empresa;
            }

            usuario.Nombre = txtNombre.Text;
            usuario.Apellido = txtApellido.Text;
            usuario.Email = txtEmail.Text;
            usuario.Bloqueado = chkBloqueado.Checked;
            usuario.DNI = txtDNI.Text.Trim();

            bllUsuario.Guardar(usuario);

            BLLRbac bllRbac = new BLLRbac();
            List<BERol> rolesSeleccionados = new List<BERol>();
            foreach (ListItem item in cblRoles.Items)
            {
                if (item.Selected)
                {
                    rolesSeleccionados.Add(new BERol(Convert.ToInt32(item.Value), item.Text));
                }
            }

            if (usuario.Codigo == 0)
            {
                BEUsuario usuariocreado = bllUsuario.ListarObjeto(usuario);
                bllRbac.GuardarRolesDeUsuario(usuariocreado, rolesSeleccionados);

                string emailDestino = txtEmail.Text;
                string asunto = _traductor.Traducir("Email_Activacion_Asunto");
                string recoveryUrl = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl($"~/ResetPassword.aspx?id={usuariocreado.Codigo}&token={usuariocreado.TokenRecupero}");
                string cuerpo = BLLEmail.ObtenerCuerpoMail(
                    _traductor.Traducir("Email_Activacion_Titulo"),
                    _traductor.Traducir("Email_Activacion_Texto1"),
                    _traductor.Traducir("Email_Activacion_Texto2"),
                    _traductor.Traducir("Email_Activacion_Boton"),
                    recoveryUrl);

                bool emailEnviado = BLLEmail.EnviarMail(emailDestino, asunto, cuerpo);
            }
            else
            {
                bllRbac.GuardarRolesDeUsuario(usuario, rolesSeleccionados);
            }

            Response.Redirect("AdminUsuarios.aspx");
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminUsuarios.aspx");
        }

        protected void BtnResetPassword_Click(object sender, EventArgs e)
        {
            try
            {
                int usuarioId = Convert.ToInt32(hfUsuarioId.Value);
                if (usuarioId > 0)
                {
                    BLLUsuario bllUsuario = new BLLUsuario();
                    BEUsuario usuario = bllUsuario.ListarObjeto(new BEUsuario { Codigo = usuarioId });
                    if (usuario != null)
                    {
                        string tokenRecupero = Guid.NewGuid().ToString();
                        usuario.TokenRecupero = tokenRecupero;
                        bllUsuario.Guardar(usuario);

                        string recoveryUrl = Request.Url.GetLeftPart(UriPartial.Authority) + Page.ResolveUrl($"~/ResetPassword.aspx?token={tokenRecupero}");
                        string asunto = _traductor.Traducir("Email_Recupero_Asunto");
                        string cuerpo = BLLEmail.ObtenerCuerpoMail(
                           _traductor.Traducir("Email_Recupero_Titulo"),
                           _traductor.Traducir("Email_Recupero_Texto1"),
                           _traductor.Traducir("Email_Recupero_Texto2"),
                           _traductor.Traducir("Email_Recupero_Boton"),
                           recoveryUrl);

                        BLLEmail.EnviarMail(usuario.Email, asunto, cuerpo);

                        string exitoMsg = _traductor.Traducir("EditarUsuario_Msg_Exito_ResetPassword").Replace("{email}", usuario.Email);
                        litMensaje.Text = $"<div class='alert alert-success'>{exitoMsg}</div>";
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMsg = _traductor.Traducir("EditarUsuario_Msg_Error_ResetPassword");
                litMensaje.Text = $"<div class='alert alert-danger'>{errorMsg}</div>";
            }
        }
    }
}