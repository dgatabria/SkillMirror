using System;
using System.Web;
using BE;
using BLL;

namespace SkillMirror
{
    public partial class EditarNovedad : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    int novedadId = Convert.ToInt32(Request.QueryString["id"]);
                    hfNovedadId.Value = novedadId.ToString();
                    CargarDatosNovedad(novedadId);
                }
            }
        }

        public override void ActualizarTraducciones()
        {
            this.Title = _traductor.Traducir("EditarNovedad_Page_Title");
            litTitulo.Text = (hfNovedadId.Value == "0")
                ? _traductor.Traducir("EditarNovedad_Titulo_Crear")
                : _traductor.Traducir("EditarNovedad_Titulo_Editar");

            lblTitulo.Text = _traductor.Traducir("EditarNovedad_Label_Titulo");
            lblSubtitulo.Text = _traductor.Traducir("EditarNovedad_Label_Subtitulo");
            lblCuerpo.Text = _traductor.Traducir("EditarNovedad_Label_Cuerpo");
            btnGuardar.Text = _traductor.Traducir("EditarNovedad_Boton_Guardar");
            btnCancelar.Text = _traductor.Traducir("Admin_Boton_Cancelar");
        }

        private void CargarDatosNovedad(int id)
        {
            BLLNovedad bll = new BLLNovedad();
            BENovedad novedad = bll.ListarObjeto(new BENovedad { Codigo = id });
            if (novedad != null)
            {
                txtTitulo.Text = novedad.Titulo;
                txtSubtitulo.Text = novedad.Subtitulo;
                txtCuerpo.Text = novedad.Cuerpo;
            }
        }

        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                BLLUsuario bllUsuario = new BLLUsuario();
                BEUsuario autor = bllUsuario.ListarObjeto(new BEUsuario { Email = HttpContext.Current.User.Identity.Name });

                BENovedad novedad = new BENovedad
                {
                    Codigo = Convert.ToInt32(hfNovedadId.Value),
                    Titulo = txtTitulo.Text,
                    Subtitulo = txtSubtitulo.Text,
                    Cuerpo = txtCuerpo.Text,
                    Autor = { Codigo = autor.Codigo }
                };

                BLLNovedad bllNovedad = new BLLNovedad();
                bllNovedad.PublicarNovedadYEnviarNewsletter(novedad);

                Response.Redirect("AdminNovedades.aspx");
            }
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminNovedades.aspx");
        }
    }
}