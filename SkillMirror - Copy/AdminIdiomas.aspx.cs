using System;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;
using BLL;
using BE;

namespace SkillMirror
{
    public partial class AdminIdiomas : BasePage
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
            this.Title = _traductor.Traducir("AdminIdiomas_Page_Title");
            headerTitle.InnerText = _traductor.Traducir("AdminIdiomas_Header_Titulo");
            cardExistentesTitle.InnerText = _traductor.Traducir("AdminIdiomas_CardExistentes_Titulo");
            cardImportarTitle.InnerText = _traductor.Traducir("AdminIdiomas_CardImportar_Titulo");
            importarText.InnerText = _traductor.Traducir("AdminIdiomas_Importar_Texto");
            btnImportar.Text = _traductor.Traducir("AdminIdiomas_Boton_Importar");
            BindGrid();
        }

        private void BindGrid()
        {
            BLLIdioma bll = new BLLIdioma();
            gvIdiomas.DataSource = bll.ListarTodos();
            gvIdiomas.DataBind();
        }

        protected void GvIdiomas_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[0].Text = _traductor.Traducir("AdminIdiomas_Grid_ID");
                e.Row.Cells[1].Text = _traductor.Traducir("AdminIdiomas_Grid_Nombre");
                e.Row.Cells[2].Text = _traductor.Traducir("AdminIdiomas_Grid_Acciones");
            }
        }

        protected void GvIdiomas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ((LinkButton)e.Row.FindControl("btnExportar")).Text = _traductor.Traducir("AdminIdiomas_Boton_Exportar");
                LinkButton btnEliminar = (LinkButton)e.Row.FindControl("btnEliminar");
                btnEliminar.Text = _traductor.Traducir("Admin_Boton_Eliminar");
                btnEliminar.OnClientClick = $"return confirm('{_traductor.Traducir("AdminIdiomas_Confirmar_Eliminar")}');";
            }
        }

        protected void GvIdiomas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idiomaId = Convert.ToInt32(e.CommandArgument);
            BLLIdioma bll = new BLLIdioma();

            if (e.CommandName == "Exportar")
            {
                string xmlString = bll.ExportarAXml(idiomaId);
                string nombreIdioma = bll.ListarIdioma(new BEIdioma { Codigo = idiomaId }).Nombre;

                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/xml";
                Response.AddHeader("content-disposition", $"attachment;filename={nombreIdioma}.xml");
                Response.Charset = "utf-8";
                Response.Output.Write(xmlString);
                Response.Flush();
                Response.End();
            }
            else if (e.CommandName == "Eliminar")
            {
                if (bll.Baja(new BEIdioma { Codigo = idiomaId }))
                {
                    litMensaje.Text = $"<div class='alert alert-success'>{_traductor.Traducir("AdminIdiomas_Msg_Exito_Eliminar")}</div>";
                }
                else
                {
                    litMensaje.Text = $"<div class='alert alert-warning'>{_traductor.Traducir("AdminIdiomas_Msg_Error_Eliminar")}</div>";
                }
                BindGrid();
            }
        }

        protected void BtnImportar_Click(object sender, EventArgs e)
        {
            if (fileUploadXml.HasFile)
            {
                try
                {
                    BLLIdioma bll = new BLLIdioma();
                    bll.ImportarDesdeXml(fileUploadXml.PostedFile.InputStream);
                    litMensaje.Text = $"<div class='alert alert-success'>{_traductor.Traducir("AdminIdiomas_Msg_Exito_Importar")}</div>";
                    BindGrid();
                }
                catch (Exception ex)
                {
                    litMensaje.Text = $"<div class='alert alert-danger'>{_traductor.Traducir("AdminIdiomas_Msg_Error_Importar")}: {ex.Message}</div>";
                }
            }
            else
            {
                litMensaje.Text = $"<div class='alert alert-warning'>{_traductor.Traducir("AdminIdiomas_Msg_Warning_NoFile")}</div>";
            }
        }
    }
}