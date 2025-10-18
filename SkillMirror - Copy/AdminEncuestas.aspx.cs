using System;
using System.Web.UI.WebControls;
using BE;
using BLL;

namespace SkillMirror
{
    public partial class AdminEncuestas : BasePage
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
            this.Title = _traductor.Traducir("AdminEncuestas_Page_Title");
            headerTitle.InnerText = _traductor.Traducir("AdminEncuestas_Header_Titulo");
            btnNuevaEncuesta.Text = _traductor.Traducir("AdminEncuestas_Boton_Crear");
            BindGrid();
        }

        private void BindGrid()
        {
            BLLEncuesta bll = new BLLEncuesta();
            gvEncuestas.DataSource = bll.ListarAdmin();
            gvEncuestas.DataBind();
        }

        protected void GvEncuestas_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Text = _traductor.Traducir("AdminEncuestas_Grid_ID");
                e.Row.Cells[1].Text = _traductor.Traducir("AdminEncuestas_Grid_Titulo");
                e.Row.Cells[2].Text = _traductor.Traducir("AdminEncuestas_Grid_FechaCreacion");
                e.Row.Cells[3].Text = _traductor.Traducir("AdminEncuestas_Grid_FechaVencimiento");
                e.Row.Cells[4].Text = _traductor.Traducir("AdminEncuestas_Grid_Estado");
                e.Row.Cells[5].Text = _traductor.Traducir("AdminEncuestas_Grid_Acciones");
            }
        }

        protected void GvEncuestas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem is BEEncuesta encuesta)
                {
                    if (e.Row.FindControl("litEstado") is Literal litEstado)
                    {
                        if (e.Row.FindControl("btnResultados") is LinkButton btnResultados)
                        {
                            btnResultados.Text = _traductor.Traducir("AdminEncuestas_Grid_Boton_Resultados");
                        }
                        litEstado.Text = encuesta.Activa
                            ? $"<span class='badge bg-success'>{_traductor.Traducir("Admin_Estado_Activo")}</span>"
                            : $"<span class='badge bg-secondary'>{_traductor.Traducir("Admin_Estado_Inactivo")}</span>";
                    }
                    ((LinkButton)e.Row.FindControl("btnEditar")).Text = _traductor.Traducir("Admin_Boton_Editar");
                    ((LinkButton)e.Row.FindControl("btnEnviar")).Text = _traductor.Traducir("AdminEncuestas_Grid_Boton_Enviar");

                    LinkButton btnEliminar = (LinkButton)e.Row.FindControl("btnEliminar");
                    btnEliminar.Text = _traductor.Traducir("Admin_Boton_Eliminar");
                    btnEliminar.OnClientClick = $"return confirm('{_traductor.Traducir("AdminEncuestas_Grid_ConfirmarEliminar")}');";
                }
            }
        }

        protected void GvEncuestas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int encuestaId = Convert.ToInt32(e.CommandArgument);
            BLLEncuesta bll = new BLLEncuesta();

            if (e.CommandName == "EditarEncuesta")
            {
                Response.Redirect($"EditarEncuesta.aspx?id={encuestaId}");
            }
            else if (e.CommandName == "EliminarEncuesta")
            {
                bll.Baja(new BEEncuesta { Codigo = encuestaId });
                BindGrid();
            }
            else if (e.CommandName == "EnviarEncuesta")
            {
                string baseUrl = Request.Url.GetLeftPart(UriPartial.Authority) + ResolveUrl("~/");
                BEEncuesta encuesta = bll.ListarObjeto(new BEEncuesta { Codigo = encuestaId });
                bll.EnviarEncuesta(encuesta, baseUrl);
                litMensaje.Text = $"<div class='alert alert-info'>{_traductor.Traducir("AdminEncuestas_Msg_Enviada")}</div>";
            }
            else if (e.CommandName == "VerResultados")
            {
                Response.Redirect($"ResultadosEncuesta.aspx?id={encuestaId}");
            }
        }

        protected void BtnNuevaEncuesta_Click(object sender, EventArgs e)
        {
            Response.Redirect("EditarEncuesta.aspx");
        }
    }
}