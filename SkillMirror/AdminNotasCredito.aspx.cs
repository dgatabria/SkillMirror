using BE;
using BLL;
using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace SkillMirror
{
    public partial class AdminNotasCredito : BasePage
    {
        private BLLFacturacion _bllFacturacion;
        private BLLEmpresa _bllEmpresa;

        protected void Page_Load(object sender, EventArgs e)
        {
            _bllFacturacion = new BLLFacturacion();
            _bllEmpresa = new BLLEmpresa();

            if (!IsPostBack)
            {
                CargarEmpresas();
                BindGrid();
            }
        }

        public override void ActualizarTraducciones()
        {
            this.Title = _traductor.Traducir("AdminNC_Page_Title");
            headerTitle.InnerText = _traductor.Traducir("AdminNC_Header_Titulo");
            headerCrearNC.InnerText = _traductor.Traducir("AdminNC_Header_Crear");

            lblEmpresa.InnerText = _traductor.Traducir("AdminNC_Label_Empresa");
            lblFacturaOrigen.InnerText = _traductor.Traducir("AdminNC_Label_FacturaOrigen");
            lblValor.InnerText = _traductor.Traducir("AdminNC_Label_Valor");
            lblVigencia.InnerText = _traductor.Traducir("AdminNC_Label_Vigencia");

            rfvEmpresa.ErrorMessage = _traductor.Traducir("Error_Seleccion_Requerida");
            rfvFactura.ErrorMessage = _traductor.Traducir("Error_Seleccion_Requerida");
            rfvValor.ErrorMessage = _traductor.Traducir("Error_Campo_Requerido");
            cvValor.ErrorMessage = _traductor.Traducir("Error_Valor_Mayor_Cero");
            rfvVigencia.ErrorMessage = _traductor.Traducir("Error_Campo_Requerido");
            cvVigencia.ErrorMessage = _traductor.Traducir("Error_Valor_Mayor_Cero");

            btnCrearNC.Text = _traductor.Traducir("AdminNC_Boton_Crear");

            // Traducir Headers de la Grilla
            if (gvNotasCredito.HeaderRow != null)
            {
                gvNotasCredito.HeaderRow.Cells[0].Text = _traductor.Traducir("AdminNC_Grid_Header_Codigo");
                gvNotasCredito.HeaderRow.Cells[1].Text = _traductor.Traducir("AdminNC_Grid_Header_Empresa");
                gvNotasCredito.HeaderRow.Cells[2].Text = _traductor.Traducir("AdminNC_Grid_Header_Valor");
                gvNotasCredito.HeaderRow.Cells[3].Text = _traductor.Traducir("AdminNC_Grid_Header_Estado");
                gvNotasCredito.HeaderRow.Cells[4].Text = _traductor.Traducir("AdminNC_Grid_Header_Vencimiento");
            }
        }

        private void BindGrid()
        {
            gvNotasCredito.DataSource = _bllFacturacion.ListarTodasLasNotasDeCredito();
            gvNotasCredito.DataBind();
        }

        private void CargarEmpresas()
        {
            var empresas = _bllEmpresa.ListarTodos().Where(emp => emp.Codigo != 1).ToList(); // Excluimos SkillMirror
            ddlEmpresa.DataSource = empresas;
            ddlEmpresa.DataTextField = "Nombre";
            ddlEmpresa.DataValueField = "Codigo";
            ddlEmpresa.DataBind();
            ddlEmpresa.Items.Insert(0, new ListItem(_traductor.Traducir("AdminNC_Dropdown_Seleccionar"), "0"));
        }

        protected void ddlEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idEmpresa = Convert.ToInt32(ddlEmpresa.SelectedValue);
            ddlFacturaOrigen.Items.Clear();

            if (idEmpresa > 0)
            {
                var facturas = _bllFacturacion.ListarFacturasPorEmpresa(idEmpresa);
                ddlFacturaOrigen.DataSource = facturas;
                // Asumiendo que BEFactura tiene una propiedad DisplayMember como esta:
                // public string DisplayMember => $"{NumeroFactura} ({FechaEmision:dd/MM/yy}) - ${MontoTotal:N2}";
                ddlFacturaOrigen.DataTextField = "DisplayMember";
                ddlFacturaOrigen.DataValueField = "Codigo";
                ddlFacturaOrigen.DataBind();
                ddlFacturaOrigen.Enabled = true;
            }
            else
            {
                ddlFacturaOrigen.Enabled = false;
            }
            ddlFacturaOrigen.Items.Insert(0, new ListItem(_traductor.Traducir("AdminNC_Dropdown_Seleccionar"), "0"));
        }

        protected void btnCrearNC_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            try
            {
                int idEmpresa = Convert.ToInt32(ddlEmpresa.SelectedValue);
                int idFactura = Convert.ToInt32(ddlFacturaOrigen.SelectedValue);
                double valor = Convert.ToDouble(txtValor.Text);
                int diasVigencia = Convert.ToInt32(txtDiasVigencia.Text);

                string codigoNC = _bllFacturacion.CrearNotaCredito(idEmpresa, idFactura, valor, diasVigencia);

                if (!string.IsNullOrEmpty(codigoNC))
                {
                    ShowAlert($"Nota de crédito {codigoNC} creada exitosamente.", "alert-success");
                    BindGrid(); // Refrescar la grilla
                    LimpiarFormulario();
                }
                else
                {
                    ShowAlert("Error al crear la nota de crédito.", "alert-danger");
                }
            }
            catch (Exception ex)
            {
                ShowAlert($"Error: {ex.Message}", "alert-danger");
            }
        }

        private void ShowAlert(string message, string cssClass)
        {
            pnlAlert.CssClass = $"alert {cssClass}";
            litAlert.Text = message;
            pnlAlert.Visible = true;
        }

        private void LimpiarFormulario()
        {
            ddlEmpresa.SelectedIndex = 0;
            ddlFacturaOrigen.Items.Clear();
            ddlFacturaOrigen.Enabled = false;
            txtValor.Text = string.Empty;
            txtDiasVigencia.Text = "365";
        }
    }
}

