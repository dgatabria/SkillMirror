using System;
using System.Web.UI.WebControls;
using BE;
using BLL;

namespace SkillMirror
{
    public partial class AdminFAQs : BasePage // Importante que herede de BasePage para las traducciones
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
            this.Title = _traductor.Traducir("AdminFAQs_Page_Title");
            headerTitle.InnerText = _traductor.Traducir("AdminFAQs_Header_Titulo");
            btnNuevaFAQ.Text = _traductor.Traducir("AdminFAQs_Boton_Crear");
            BindGrid(); // Para refrescar los encabezados del Grid
        }

        private void BindGrid()
        {
            BLLFAQ bll = new BLLFAQ();
            gvFAQs.DataSource = bll.ListarAdmin();
            gvFAQs.DataBind();
        }

        protected void GvFAQs_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Text = _traductor.Traducir("AdminFAQs_Grid_ID");
                e.Row.Cells[1].Text = _traductor.Traducir("AdminFAQs_Grid_Pregunta");
                e.Row.Cells[2].Text = _traductor.Traducir("AdminFAQs_Grid_Orden");
                e.Row.Cells[3].Text = _traductor.Traducir("AdminFAQs_Grid_Estado");
                e.Row.Cells[4].Text = _traductor.Traducir("AdminFAQs_Grid_Acciones");
            }
        }

        protected void GvFAQs_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.DataItem is BEFAQ faq)
                {
                    // Traducir estado con un badge de Bootstrap
                    if (e.Row.FindControl("litEstado") is Literal litEstado)
                    {
                        litEstado.Text = faq.Activo
                            ? $"<span class='badge bg-success'>{_traductor.Traducir("Admin_Estado_Activo")}</span>"
                            : $"<span class='badge bg-secondary'>{_traductor.Traducir("Admin_Estado_Inactivo")}</span>";
                    }

                    // Traducir botones de acción
                    ((LinkButton)e.Row.FindControl("btnEditar")).Text = _traductor.Traducir("Admin_Boton_Editar");
                    string pepe  = _traductor.Traducir("Admin_Boton_Editar");
                    ((LinkButton)e.Row.FindControl("btnEditar")).Text = pepe;
                    LinkButton btnBaja = (LinkButton)e.Row.FindControl("btnBaja");
                    btnBaja.Text = _traductor.Traducir("Admin_Boton_Baja");
                    btnBaja.OnClientClick = $"return confirm('{_traductor.Traducir("AdminFAQs_Grid_ConfirmarBaja")}');";
                }
            }
        }

        protected void GvFAQs_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int faqId = Convert.ToInt32(e.CommandArgument);
            BLLFAQ bll = new BLLFAQ();

            if (e.CommandName == "EditarFAQ")
            {
                Response.Redirect($"EditarFAQ.aspx?id={faqId}");
            }
            else if (e.CommandName == "BajaFAQ")
            {
                bll.Baja(new BEFAQ { Codigo = faqId });
                BindGrid(); // Refrescamos la grilla para ver el cambio de estado
            }
        }

        protected void BtnNuevaFAQ_Click(object sender, EventArgs e)
        {
            Response.Redirect("EditarFAQ.aspx");
        }
    }
}