using System;
using System.Web.UI.WebControls;
using BE;
using BLL;

namespace SkillMirror
{
    public partial class AdminPublicidad : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        private void BindGrid()
        {
            BLLPublicidad bll = new BLLPublicidad();
            gvPublicidades.DataSource = bll.ListarAdmin();
            gvPublicidades.DataBind();
        }

        public override void ActualizarTraducciones()
        {
            this.Title = _traductor.Traducir("AdminPublicidad_Page_Title");
            headerTitle.InnerText = _traductor.Traducir("AdminPublicidad_Header_Titulo");
            btnNuevaPublicidad.Text = _traductor.Traducir("AdminPublicidad_Boton_Crear");
            
            BindGrid();
        }

        protected void GvPublicidades_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Text = _traductor.Traducir("AdminPublicidad_Grid_ID");
                e.Row.Cells[1].Text = _traductor.Traducir("AdminPublicidad_Grid_Titulo");
                e.Row.Cells[2].Text = _traductor.Traducir("AdminPublicidad_Grid_Inicio");
                e.Row.Cells[3].Text = _traductor.Traducir("AdminPublicidad_Grid_Expiracion");
                e.Row.Cells[4].Text = _traductor.Traducir("AdminPublicidad_Grid_Estado");
                e.Row.Cells[5].Text = _traductor.Traducir("AdminPublicidad_Grid_Acciones");
            }
        }

        protected void GvPublicidades_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem is BEPublicidad pub)
                {
                    var litEstado = (Literal)e.Row.FindControl("litEstado");
                    litEstado.Text = pub.Activo
                        ? $"<span class='badge bg-success'>{_traductor.Traducir("Admin_Estado_Activo")}</span>"
                        : $"<span class='badge bg-secondary'>{_traductor.Traducir("Admin_Estado_Inactivo")}</span>";

                    ((LinkButton)e.Row.FindControl("btnEditar")).Text = _traductor.Traducir("Admin_Boton_Editar");

                    var btnEliminar = (LinkButton)e.Row.FindControl("btnEliminar");
                    btnEliminar.Text = _traductor.Traducir("Admin_Boton_Eliminar");
                    btnEliminar.OnClientClick = $"return confirm('{_traductor.Traducir("AdminPublicidad_Grid_ConfirmarEliminar")}');";
                }
            }
        }

        protected void GvPublicidades_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int pubId = Convert.ToInt32(e.CommandArgument);
            BLLPublicidad bll = new BLLPublicidad();

            if (e.CommandName == "EditarPub")
            {
                Response.Redirect($"EditarPublicidad.aspx?id={pubId}");
            }
            else if (e.CommandName == "EliminarPub")
            {
                bll.Eliminar(new BEPublicidad { Codigo = pubId });
                BindGrid();
            }
        }

        protected void BtnNuevaPublicidad_Click(object sender, EventArgs e)
        {
            Response.Redirect("EditarPublicidad.aspx");
        }
    }
}