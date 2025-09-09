using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using BE;
using BLL;

namespace SkillMirror
{
    public partial class AdminResenas : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // La suscripción se maneja en BasePage.
            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        public override void ActualizarTraducciones()
        {
            // Usamos la variable _traductor heredada de BasePage
            this.Title = _traductor.Traducir("AdminResenas_Page_Title");
            headerTitle.InnerText = _traductor.Traducir("AdminResenas_Header_Titulo");

            // Forzamos el re-bind de la grilla para que se apliquen las traducciones
            BindGrid();
        }

        private void BindGrid()
        {
            BLLResena bll = new BLLResena();
            gvResenas.DataSource = bll.ListarTodas();
            gvResenas.DataBind();
        }

        protected void GvResenas_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Text = _traductor.Traducir("AdminResenas_Grid_Header_Fecha");
                e.Row.Cells[1].Text = _traductor.Traducir("AdminResenas_Grid_Header_Autor");
                e.Row.Cells[2].Text = _traductor.Traducir("AdminResenas_Grid_Header_Asunto");
                e.Row.Cells[3].Text = _traductor.Traducir("AdminResenas_Grid_Header_Puntuacion");
                e.Row.Cells[4].Text = _traductor.Traducir("AdminResenas_Grid_Header_Estado");
                e.Row.Cells[5].Text = _traductor.Traducir("AdminResenas_Grid_Header_Acciones");
            }
        }

        protected void GvResenas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem is BEResena resena)
                {
                    if (e.Row.FindControl("litEstado") is Literal litEstado)
                    {
                        if (resena.Aprobado)
                        {
                            litEstado.Text = $"<span class='badge bg-success'>{_traductor.Traducir("AdminResenas_Grid_Estado_Aprobado")}</span>";
                        }
                        else
                        {
                            litEstado.Text = $"<span class='badge bg-warning'>{_traductor.Traducir("AdminResenas_Grid_Estado_Pendiente")}</span>";
                        }
                    }

                    if (e.Row.FindControl("btnAprobar") is LinkButton btnAprobar)
                    {
                        btnAprobar.Text = _traductor.Traducir("AdminResenas_Grid_Boton_Aprobar");
                    }

                    if (e.Row.FindControl("btnEliminar") is LinkButton btnEliminar)
                    {
                        btnEliminar.Text = _traductor.Traducir("AdminResenas_Grid_Boton_Eliminar");
                        btnEliminar.OnClientClick = $"return confirm('{_traductor.Traducir("AdminResenas_Grid_Confirmacion_Eliminar")}');";
                    }
                }
            }
        }

        protected void GvResenas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int resenaId = Convert.ToInt32(e.CommandArgument);
            BLLResena bll = new BLLResena();

            if (e.CommandName == "AprobarResena")
            {
                bll.Aprobar(resenaId);
                litMensaje.Text = $"<div class='alert alert-success'>{_traductor.Traducir("AdminResenas_Msg_AprobadoExitoso")}</div>";
            }
            else if (e.CommandName == "EliminarResena")
            {
                bll.Eliminar(resenaId);
                litMensaje.Text = $"<div class='alert alert-info'>{_traductor.Traducir("AdminResenas_Msg_EliminadoExitoso")}</div>";
            }

            BindGrid();
        }

        public string RenderStars(object puntuacion)
        {
            int p = Convert.ToInt32(puntuacion);
            var sb = new StringBuilder();
            for (int i = 0; i < 5; i++)
            {
                sb.Append(i < p ? "★" : "☆");
            }
            return sb.ToString();
        }
    }
}