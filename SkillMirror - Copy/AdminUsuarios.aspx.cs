using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BE;
using BLL;

namespace SkillMirror
{
    public partial class AdminUsuarios : BasePage
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
            this.Title = _traductor.Traducir("AdminUsuarios_Page_Title");
            headerTitle.InnerText = _traductor.Traducir("AdminUsuarios_Header_Titulo");
            btnNuevoUsuario.Text = _traductor.Traducir("AdminUsuarios_Boton_CrearNuevo");

            if (gvUsuarios.HeaderRow != null)
            {
                gvUsuarios.HeaderRow.Cells[0].Text = _traductor.Traducir("AdminUsuarios_Grid_Header_ID");
                gvUsuarios.HeaderRow.Cells[1].Text = _traductor.Traducir("AdminUsuarios_Grid_Header_Nombre");
                gvUsuarios.HeaderRow.Cells[2].Text = _traductor.Traducir("AdminUsuarios_Grid_Header_Apellido");
                gvUsuarios.HeaderRow.Cells[3].Text = _traductor.Traducir("AdminUsuarios_Grid_Header_Email");
                gvUsuarios.HeaderRow.Cells[4].Text = _traductor.Traducir("AdminUsuarios_Grid_Header_Estado");
                gvUsuarios.HeaderRow.Cells[5].Text = _traductor.Traducir("AdminUsuarios_Grid_Header_Acciones");
            }
            BindGrid();
        }

        private void BindGrid()
        {
            BLLUsuario bll = new BLLUsuario();
            gvUsuarios.DataSource = bll.ListarTodos();
            gvUsuarios.DataBind();
        }


        protected void GvUsuarios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                if (e.Row.FindControl("btnEditar") is LinkButton btnEditar)
                {
                    btnEditar.Text = _traductor.Traducir("AdminUsuarios_Grid_Boton_Editar");
                }

                if (e.Row.FindControl("btnEliminar") is LinkButton btnEliminar)
                {
                    btnEliminar.Text = _traductor.Traducir("AdminUsuarios_Grid_Boton_Eliminar");
                    string confirmMsg = _traductor.Traducir("AdminUsuarios_Grid_Confirmacion_Eliminar");
                    btnEliminar.OnClientClick = $"return confirm('{confirmMsg}');";
                }


                if (e.Row.FindControl("litEstado") is Literal litEstado)
                {

                    if (e.Row.DataItem is BEUsuario usuario)
                    {

                        if (usuario.Bloqueado)
                        {
                            litEstado.Text = $"<span class='badge bg-danger'>{_traductor.Traducir("AdminUsuarios_Grid_Estado_Bloqueado")}</span>";
                        }
                        else
                        {
                            litEstado.Text = $"<span class='badge bg-success'>{_traductor.Traducir("AdminUsuarios_Grid_Estado_Activo")}</span>";
                        }
                    }
                }
            }
        }

        protected void BtnNuevoUsuario_Click(object sender, EventArgs e)
        {
            Response.Redirect("EditarUsuario.aspx");
        }

        protected void GvUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int usuarioId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditarUsuario")
            {
                Response.Redirect($"EditarUsuario.aspx?id={usuarioId}");
            }
            else if (e.CommandName == "EliminarUsuario")
            {
                BLLUsuario bll = new BLLUsuario();
                bll.Baja(new BEUsuario { Codigo = usuarioId });
                BindGrid();
            }
        }
    }
}