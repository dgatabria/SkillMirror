using BE;
using BLL;
using System;
using System.Web.UI.WebControls;

namespace SkillMirror
{
    public partial class AdminConversaciones : BasePage
    {
        private BLLMensajeria _bllMensajeria;

        protected void Page_Load(object sender, EventArgs e)
        {
            _bllMensajeria = new BLLMensajeria();
            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        public override void ActualizarTraducciones()
        {
            this.Title = _traductor.Traducir("AdminConversaciones_Page_Title");
            headerTitle.InnerText = _traductor.Traducir("AdminConversaciones_Header_Titulo");
            BindGrid(); // Vuelve a enlazar para que los botones y textos internos se actualicen
        }

        private void BindGrid()
        {
            _bllMensajeria = new BLLMensajeria();
            gvConversaciones.DataSource = _bllMensajeria.ListarConversacionesParaAgente();
            gvConversaciones.DataBind();
        }

        protected void GvConversaciones_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Text = _traductor.Traducir("AdminConversaciones_Grid_Usuario");
                e.Row.Cells[1].Text = _traductor.Traducir("AdminConversaciones_Grid_Asunto");
                e.Row.Cells[2].Text = _traductor.Traducir("AdminConversaciones_Grid_Estado");
                e.Row.Cells[3].Text = _traductor.Traducir("AdminConversaciones_Grid_UltimaActividad");
                e.Row.Cells[4].Text = _traductor.Traducir("AdminConversaciones_Grid_Acciones");
            }
        }

        protected void GvConversaciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var conversacion = (BEConversacion)e.Row.DataItem;
                var litEstado = (Literal)e.Row.FindControl("litEstado");
                var btnVer = (LinkButton)e.Row.FindControl("btnVer");
                btnVer.Text = _traductor.Traducir("AdminConversaciones_Grid_Boton_Ver");

                string badgeClass = "bg-secondary";
                if (conversacion.Estado == "Abierto") badgeClass = "bg-success";
                if (conversacion.Estado == "Respondido por Agente") badgeClass = "bg-info text-dark";
                litEstado.Text = $"<span class='badge {badgeClass}'>{conversacion.Estado}</span>";

                if (conversacion.MensajesNuevos > 0)
                {
                    e.Row.Font.Bold = true;
                    e.Row.Cells[1].Text += $" <span class='badge bg-danger rounded-pill'>{conversacion.MensajesNuevos}</span>";
                }
            }
            else if (e.Row.RowType == DataControlRowType.EmptyDataRow)
            {
                // Traduce el mensaje de "No hay datos"
                var litNoConversaciones = (Literal)e.Row.FindControl("litNoConversaciones");
                if (litNoConversaciones != null)
                {
                    litNoConversaciones.Text = _traductor.Traducir("AdminConversaciones_Grid_NoConversaciones");
                }
            }
        }

        protected void GvConversaciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "VerConversacion")
            {
                string conversacionId = e.CommandArgument.ToString();
                Response.Redirect($"~/AdminVerConversacion.aspx?id={conversacionId}");
            }
        }
    }
}