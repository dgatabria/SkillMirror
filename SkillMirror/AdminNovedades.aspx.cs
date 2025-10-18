using System;
using System.Web.UI.WebControls;
using BE;
using BLL;

namespace SkillMirror
{
    public partial class AdminNovedades : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        public override void ActualizarTraducciones()
        {
            this.Title = _traductor.Traducir("AdminNovedades_Page_Title");
            headerTitle.InnerText = _traductor.Traducir("AdminNovedades_Header_Titulo");
            btnNuevaNovedad.Text = _traductor.Traducir("AdminNovedades_Boton_Crear");
            BindGrid();
        }

        private void BindGrid()
        {
            BLLNovedad bll = new BLLNovedad();
            gvNovedades.DataSource = bll.ListarTodas();
            gvNovedades.DataBind();
        }

        protected void GvNovedades_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Text = _traductor.Traducir("AdminNovedades_Grid_ID");
                e.Row.Cells[1].Text = _traductor.Traducir("AdminNovedades_Grid_Titulo");
                e.Row.Cells[2].Text = _traductor.Traducir("AdminNovedades_Grid_Fecha");
                e.Row.Cells[3].Text = _traductor.Traducir("AdminNovedades_Grid_Autor");
                e.Row.Cells[4].Text = _traductor.Traducir("AdminNovedades_Grid_Estado");
                e.Row.Cells[5].Text = _traductor.Traducir("AdminNovedades_Grid_Acciones");
            }
        }

        protected void GvNovedades_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem is BENovedad novedad)
                {
                    if (e.Row.FindControl("litEstado") is Literal litEstado)
                    {
                        litEstado.Text = novedad.Activo
                            ? $"<span class='badge bg-success'>{_traductor.Traducir("Admin_Estado_Activo")}</span>"
                            : $"<span class='badge bg-danger'>{_traductor.Traducir("Admin_Estado_Inactivo")}</span>";
                    }

                    LinkButton btnEditar = (LinkButton)e.Row.FindControl("btnEditar");
                    btnEditar.Text = _traductor.Traducir("Admin_Boton_Editar");

                    LinkButton btnEliminar = (LinkButton)e.Row.FindControl("btnEliminar");
                    btnEliminar.Text = _traductor.Traducir("Admin_Boton_Eliminar");
                    btnEliminar.OnClientClick = $"return confirm('{_traductor.Traducir("AdminNovedades_Grid_ConfirmarEliminar")}');";
                }
            }
        }

        protected void GvNovedades_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int novedadId = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "EditarNovedad")
            {
                Response.Redirect($"EditarNovedad.aspx?id={novedadId}");
            }
            else if (e.CommandName == "EliminarNovedad")
            {
                BLLNovedad bll = new BLLNovedad();
                bll.Baja(new BENovedad { Codigo = novedadId });
                BindGrid();
            }
        }

        protected void BtnNuevaNovedad_Click(object sender, EventArgs e)
        {
            Response.Redirect("EditarNovedad.aspx");
        }
    }
}