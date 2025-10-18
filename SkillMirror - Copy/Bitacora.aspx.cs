using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using BE;
using BLL;

namespace SkillMirror
{
    public partial class Bitacora : BasePage
    {
        private BLLBitacora bllBitacora;


        public Bitacora()
        {
            bllBitacora = new BLLBitacora();
        }

        protected void Page_Load(object sender, EventArgs e)
        {


            if (!IsPostBack)
            {
                BindGrid();
            }
        }



        public override void ActualizarTraducciones()
        {
            this.Title = _traductor.Traducir("Bitacora_Page_Title");
            headerTitle.InnerText = _traductor.Traducir("Bitacora_Header_Titulo");

            // Filtros
            litFiltrosTitle.Text = _traductor.Traducir("Bitacora_Filtros_Titulo");
            lblFechaDesde.Text = _traductor.Traducir("Bitacora_Filtros_Label_FechaDesde");
            lblFechaHasta.Text = _traductor.Traducir("Bitacora_Filtros_Label_FechaHasta");
            lblAccion.Text = _traductor.Traducir("Bitacora_Filtros_Label_Accion");
            lblDetalle.Text = _traductor.Traducir("Bitacora_Filtros_Label_Detalle");
            btnFiltrar.Text = _traductor.Traducir("Bitacora_Filtros_Boton_Filtrar");

            // Grid Label
            litMostrar.Text = _traductor.Traducir("Bitacora_Grid_Label_Mostrar");

            // Pager
            gvBitacora.PagerSettings.FirstPageText = _traductor.Traducir("Grid_Pager_Primero");
            gvBitacora.PagerSettings.LastPageText = _traductor.Traducir("Grid_Pager_Ultimo");

            
            BindGrid();
        }

        // NUEVO MÉTODO PARA TRADUCIR LOS HEADERS
        protected void GvBitacora_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Text = _traductor.Traducir("Bitacora_Grid_Header_Fecha");
                e.Row.Cells[1].Text = _traductor.Traducir("Bitacora_Grid_Header_Usuario");
                e.Row.Cells[2].Text = _traductor.Traducir("Bitacora_Grid_Header_Accion");
                e.Row.Cells[3].Text = _traductor.Traducir("Bitacora_Grid_Header_Detalle");
                e.Row.Cells[4].Text = _traductor.Traducir("Bitacora_Grid_Header_IP");
                e.Row.Cells[5].Text = _traductor.Traducir("Bitacora_Grid_Header_Criticidad");
            }
        }

        private void BindGrid()
        {
            DateTime? fechaDesde = string.IsNullOrWhiteSpace(txtFechaDesde.Text) ? (DateTime?)null : Convert.ToDateTime(txtFechaDesde.Text);
            DateTime? fechaHasta = string.IsNullOrWhiteSpace(txtFechaHasta.Text) ? (DateTime?)null : Convert.ToDateTime(txtFechaHasta.Text).AddDays(1).AddTicks(-1);
            string accion = txtAccion.Text;
            string detalle = txtDetalle.Text;
            int pageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
            gvBitacora.PageSize = pageSize;
            int totalRecords;

            List<EntradaBitacora> datos = bllBitacora.Listar(
                fechaDesde,
                fechaHasta,
                accion,
                detalle,
                gvBitacora.PageIndex + 1,
                pageSize,
                out totalRecords);

            gvBitacora.VirtualItemCount = totalRecords;
            gvBitacora.DataSource = datos;
            gvBitacora.DataBind();
        }

        protected void BtnFiltrar_Click(object sender, EventArgs e)
        {
            gvBitacora.PageIndex = 0;
            BindGrid();
        }

        protected void DdlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvBitacora.PageIndex = 0;
            gvBitacora.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
            BindGrid();
        }

        protected void GvBitacora_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvBitacora.PageIndex = e.NewPageIndex;
            BindGrid();
        }
    }
}