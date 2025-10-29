using System;
using System.Web.UI.WebControls;
using BLL;

namespace SkillMirror
{
    public partial class AdminPlanes : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarGrilla();
            }
        }

        public override void ActualizarTraducciones()
        {
            this.Title = _traductor.Traducir("AdminPlanes_Page_Title");
            headerTitle.InnerText = _traductor.Traducir("AdminPlanes_Header_Titulo");
            btnNuevo.Text = _traductor.Traducir("AdminPlanes_Boton_Nuevo");

            CargarGrilla();
        }

        private void CargarGrilla()
        {
            var bll = new BLLPlanes();
            gvPlanes.DataSource = bll.ListarAdmin();
            gvPlanes.DataBind();
        }

        protected void gvPlanes_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Text = _traductor.Traducir("AdminPlanes_Grid_Nombre");
                e.Row.Cells[1].Text = _traductor.Traducir("AdminPlanes_Grid_Precio");
                e.Row.Cells[2].Text = _traductor.Traducir("AdminPlanes_Grid_Orden");
                e.Row.Cells[3].Text = _traductor.Traducir("AdminPlanes_Grid_Activo");
                e.Row.Cells[4].Text = _traductor.Traducir("AdminPlanes_Grid_Destacado");
                e.Row.Cells[5].Text = _traductor.Traducir("AdminPlanes_Grid_Acciones");
            }
        }

        protected void gvPlanes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton btnEditar = (LinkButton)e.Row.FindControl("btnEditar");
                if (btnEditar != null)
                {
                    string pep = _traductor.Traducir("AdminPlanes_Boton_Editar");
                    btnEditar.Text = pep;
                }

                LinkButton btnEliminar = (LinkButton)e.Row.FindControl("btnEliminar");
                if (btnEliminar != null)
                {
                    btnEliminar.Text = _traductor.Traducir("AdminPlanes_Boton_Eliminar");
                    //btnEliminar.Text = "cuaaaaaac";
                    btnEliminar.OnClientClick = $"return confirm('{_traductor.Traducir("AdminPlanes_Confirmar_Baja")}');";
                }
            }
        }

        protected void gvPlanes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int planId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditarPlan")
            {
                Response.Redirect($"EditarPlan.aspx?id={planId}");
            }
            else if (e.CommandName == "EliminarPlan")
            {
                var bll = new BLLPlanes();
                bll.BajaLogica(planId);
                CargarGrilla();
            }
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            Response.Redirect("EditarPlan.aspx");
        }
    }
}

