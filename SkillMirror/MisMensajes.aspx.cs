using BE;
using BLL;
using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace SkillMirror
{
    public partial class MisMensajes : BasePage
    {
        private BLLMensajeria _bllMensajeria;
        private BEUsuario _usuarioActual;

        protected void Page_Load(object sender, EventArgs e)
        {
            _bllMensajeria = new BLLMensajeria();
            var bllUsuario = new BLLUsuario();
            _usuarioActual = bllUsuario.ListarObjeto(new BEUsuario { Email = HttpContext.Current.User.Identity.Name });

            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        public override void ActualizarTraducciones()
        {
            this.Title = _traductor.Traducir("UserMensajes_Page_Title");
            headerTitle.InnerText = _traductor.Traducir("UserMensajes_Header_Titulo");
            btnNuevoMensaje.Text = _traductor.Traducir("UserMensajes_Boton_NuevoMensaje");
            BindGrid();
        }

        private void BindGrid()
        {
            _bllMensajeria = new BLLMensajeria();
            BLLUsuario _bllUsuario = new BLLUsuario();

            _usuarioActual = _bllUsuario.ListarObjeto(new BEUsuario { Email = HttpContext.Current.User.Identity.Name });
            var todasLasConversaciones = _bllMensajeria.ListarConversacionesParaAgente();

            // ESTA LÍNEA AHORA FUNCIONARÁ CORRECTAMENTE
            var misConversaciones = todasLasConversaciones.Where(c => c.Usuario.Codigo == _usuarioActual.Codigo).ToList();

            gvConversaciones.DataSource = misConversaciones;
            gvConversaciones.DataBind();
        }

        protected void GvConversaciones_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Text = _traductor.Traducir("UserMensajes_Grid_Asunto");
                e.Row.Cells[1].Text = _traductor.Traducir("UserMensajes_Grid_Estado");
                e.Row.Cells[2].Text = _traductor.Traducir("UserMensajes_Grid_UltimaActividad");
                e.Row.Cells[3].Text = _traductor.Traducir("UserMensajes_Grid_Acciones");
            }
        }

        protected void GvConversaciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var conversacion = (BEConversacion)e.Row.DataItem;
                var litEstado = (Literal)e.Row.FindControl("litEstado");
                var btnVer = (LinkButton)e.Row.FindControl("btnVer");
                btnVer.Text = _traductor.Traducir("UserMensajes_Grid_Boton_Ver");

                string badgeClass = "bg-secondary";
                if (conversacion.Estado == "Abierto") badgeClass = "bg-warning text-dark";
                if (conversacion.Estado == "Respondido por Agente") badgeClass = "bg-success";

                litEstado.Text = $"<span class='badge {badgeClass}'>{conversacion.Estado}</span>";
            }
            else if (e.Row.RowType == DataControlRowType.EmptyDataRow)
            {
                var litNoConversaciones = (Literal)e.Row.FindControl("litNoConversaciones");
                if (litNoConversaciones != null)
                {
                    litNoConversaciones.Text = _traductor.Traducir("UserMensajes_Grid_NoConversaciones");
                }
            }
        }

        protected void GvConversaciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "VerConversacion")
            {
                string conversacionId = e.CommandArgument.ToString();
                Response.Redirect($"~/MiVerConversacion.aspx?id={conversacionId}");
            }
        }

        protected void BtnNuevoMensaje_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/NuevoMensaje.aspx");
        }
    }
}