using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using BE;

namespace SkillMirror
{
    public partial class AdminEmpresas : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // La suscripción se maneja en BasePage. No necesitamos hacerla aquí.
            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        // Ya no necesitamos Page_Unload, se maneja en la BasePage.

        public override void ActualizarTraducciones()
        {
            // Usamos la variable _traductor heredada de BasePage
            this.Title = _traductor.Traducir("AdminEmpresas_Page_Title");
            headerTitle.InnerText = _traductor.Traducir("AdminEmpresas_Header_Titulo");
            btnNuevaEmpresa.Text = _traductor.Traducir("AdminEmpresas_Boton_CrearNueva");

            // ¡LA SOLUCIÓN! Forzamos a la grilla a repintarse con el nuevo idioma.
            BindGrid();
        }

        // NUEVO EVENTO para traducir los encabezados de forma segura
        protected void GvEmpresas_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Text = _traductor.Traducir("AdminEmpresas_Grid_Header_ID");
                e.Row.Cells[1].Text = _traductor.Traducir("AdminEmpresas_Grid_Header_Nombre");
                e.Row.Cells[2].Text = _traductor.Traducir("AdminEmpresas_Grid_Header_CUIT");
                e.Row.Cells[3].Text = _traductor.Traducir("AdminEmpresas_Grid_Header_Telefono");
                e.Row.Cells[4].Text = _traductor.Traducir("AdminEmpresas_Grid_Header_Acciones");
            }
        }

        protected void GvEmpresas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.FindControl("btnEditar") is LinkButton btnEditar)
                {
                    btnEditar.Text = _traductor.Traducir("AdminEmpresas_Grid_Boton_Editar");
                }

                if (e.Row.FindControl("btnEliminar") is LinkButton btnEliminar)
                {
                    btnEliminar.Text = _traductor.Traducir("AdminEmpresas_Grid_Boton_Eliminar");
                    string confirmMsg = _traductor.Traducir("AdminEmpresas_Grid_Confirmacion_Eliminar");
                    btnEliminar.OnClientClick = $"return confirm('{confirmMsg}');";
                }
            }
        }

        private void BindGrid()
        {
            BLLEmpresa bll = new BLLEmpresa();
            gvEmpresas.DataSource = bll.ListarTodos();
            gvEmpresas.DataBind();
        }

        protected void BtnNuevaEmpresa_Click(object sender, EventArgs e)
        {
            Response.Redirect("EditarEmpresa.aspx");
        }

        protected void GvEmpresas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int empresaId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditarEmpresa")
            {
                Response.Redirect($"EditarEmpresa.aspx?id={empresaId}");
            }
            else if (e.CommandName == "EliminarEmpresa")
            {
                BLLEmpresa bll = new BLLEmpresa();
                bll.Baja(new BEEmpresa { Codigo = empresaId });
                BindGrid();
            }
        }
    }
}