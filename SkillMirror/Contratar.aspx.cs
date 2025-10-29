using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;

namespace SkillMirror
{
    public partial class Contratar : BasePage
    {
        // --- CAMBIO 1: Usamos la nueva BLL de Planes ---
        private BLLPlanes _bllPlanes;
        private BLLEmpresa _bllEmpresa;
        private BLLUsuario _bllUsuario;
        private BLLFacturacion _bllFacturacion;

        private List<BENotaCredito> NotasDeCreditoDisponibles
        {
            get { return ViewState["NotasCredito"] as List<BENotaCredito>; }
            set { ViewState["NotasCredito"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // --- CAMBIO 2: Instanciamos la BLL nueva ---
            _bllPlanes = new BLLPlanes();
            _bllEmpresa = new BLLEmpresa();
            _bllUsuario = new BLLUsuario();
            _bllFacturacion = new BLLFacturacion();

            if (!IsPostBack)
            {
                if (!int.TryParse(Request.QueryString["planId"], out int planId))
                {
                    // Redirigimos a la nueva página de catálogo de planes
                    Response.Redirect("~/Catalogo.aspx");
                    return;
                }
                CargarDatosDePagina(planId);

                chkTarjeta.Attributes.Add("onclick", $"togglePaymentPanel(this, '{pnlTarjetaCredito.ClientID}')");
                chkNotaCredito.Attributes.Add("onclick", $"togglePaymentPanel(this, '{pnlNotaCreditoDetalle.ClientID}')");
                chkTransferencia.Attributes.Add("onclick", $"togglePaymentPanel(this, '{pnlTransferencia.ClientID}')");
            }
        }

        public override void ActualizarTraducciones()
        {
            this.Title = _traductor.Traducir("Contratar_Page_Title");
            headerResumen.InnerText = _traductor.Traducir("Contratar_Header_Resumen");
            lblPlanContratado.Text = _traductor.Traducir("Contratar_Label_PlanContratado");
            lblTotalAPagar.Text = _traductor.Traducir("Contratar_Label_TotalAPagar");
            headerDatosEmpresa.InnerText = _traductor.Traducir("Contratar_Header_DatosEmpresa");
            headerMedioPago.InnerText = _traductor.Traducir("Contratar_Header_MedioPago");
            chkTarjeta.Text = _traductor.Traducir("Contratar_Check_Tarjeta");
            chkNotaCredito.Text = _traductor.Traducir("Contratar_Check_NotaCredito");
            chkTransferencia.Text = _traductor.Traducir("Contratar_Check_Transferencia");
            lblHeaderTarjeta.Text = _traductor.Traducir("Contratar_Header_Tarjeta");
            rfvNombreTitular.ErrorMessage = _traductor.Traducir("Error_Campo_Requerido");
            rfvNumeroTarjeta.ErrorMessage = _traductor.Traducir("Error_Campo_Requerido");
            rfvExpiracion.ErrorMessage = _traductor.Traducir("Error_Campo_Requerido");
            revExpiracion.ErrorMessage = _traductor.Traducir("Error_Formato_Invalido");
            rfvCVV.ErrorMessage = _traductor.Traducir("Error_Campo_Requerido");
            lblHeaderNC.Text = _traductor.Traducir("Contratar_Header_NC");
            lblHeaderTransferencia.Text = _traductor.Traducir("Contratar_Header_Transferencia");
            rfvComprobante.ErrorMessage = _traductor.Traducir("Error_Campo_Requerido");
            btnConfirmarPago.Text = _traductor.Traducir("Contratar_Boton_Confirmar");
            rfvNombreEmpresa.ErrorMessage = _traductor.Traducir("Error_Campo_Requerido");
            rfvCUIT.ErrorMessage = _traductor.Traducir("Error_Campo_Requerido");
        }

        public string Traducir(string tag)
        {
            return _traductor.Traducir(tag);
        }

        private void CargarDatosDePagina(int planId)
        {
            // --- CAMBIO 3: Obtenemos el plan desde la BLL nueva ---
            var planSeleccionado = _bllPlanes.ListarPorID(planId);
            if (planSeleccionado == null)
            {
                lblError.Text = "El plan seleccionado no es válido.";
                lblError.Visible = true;
                btnConfirmarPago.Enabled = false;
                return;
            }

            // Guardamos el plan nuevo (BEPlan) en el ViewState
            ViewState["PlanSeleccionado"] = planSeleccionado;
            litPlanNombre.Text = planSeleccionado.Nombre;
            // Usamos la propiedad correcta del nuevo objeto BEPlan
            litPlanPrecio.Text = planSeleccionado.PrecioMensual.ToString("F2", CultureInfo.InvariantCulture);

            var usuarioCompleto = _bllUsuario.ListarObjeto(new BEUsuario { Email = HttpContext.Current.User.Identity.Name });

            if (usuarioCompleto.Empresa == null || usuarioCompleto.Empresa.Codigo == 1)
            {
                pnlDatosEmpresa.Visible = true;
                pnlNotaCreditoContainer.Visible = false;
            }
            else
            {
                pnlDatosEmpresa.Visible = false;
                var notasCredito = _bllFacturacion.ListarNotasCreditoActivasPorEmpresa(usuarioCompleto.Empresa);
                if (notasCredito != null && notasCredito.Any())
                {
                    this.NotasDeCreditoDisponibles = notasCredito;
                    pnlNotaCreditoContainer.Visible = true;
                    rptNotasCredito.DataSource = notasCredito;
                    rptNotasCredito.DataBind();
                }
                else
                {
                    pnlNotaCreditoContainer.Visible = false;
                }
            }
        }

        protected void btnConfirmarPago_Click(object sender, EventArgs e)
        {
            if (ViewState["PlanSeleccionado"] == null)
            {
                lblError.Text = "La sesión del plan ha expirado. Por favor, vuelva a seleccionarlo.";
                lblError.Visible = true;
                return;
            }

            rfvNombreTitular.Enabled = rfvNumeroTarjeta.Enabled = rfvExpiracion.Enabled = rfvCVV.Enabled = revExpiracion.Enabled = chkTarjeta.Checked;
            rfvComprobante.Enabled = chkTransferencia.Checked;

            if (pnlDatosEmpresa.Visible) Page.Validate("Empresa");
            Page.Validate("Pago");
            if (!Page.IsValid) return;

            // --- CAMBIO 4: Obtenemos el BEPlan del ViewState ---
            var plan = (BEPlan)ViewState["PlanSeleccionado"];
            var usuarioActual = _bllUsuario.ListarObjeto(new BEUsuario { Email = HttpContext.Current.User.Identity.Name });

            try
            {
                int idEmpresaFinal = (usuarioActual.Empresa != null) ? usuarioActual.Empresa.Codigo : 0;
                if (pnlDatosEmpresa.Visible)
                {
                    if (!Regex.IsMatch(txtCUIT.Text.Trim(), @"^\d{2}-\d{8}-\d{1}$"))
                    {
                        // Considera usar un sistema de excepciones más específico o devolver códigos de error.
                        throw new ArgumentException("El formato del CUIT no es válido (debe ser XX-XXXXXXXX-X).");
                    }
                    var nuevaEmpresa = new BEEmpresa { Codigo = 0, Nombre = txtNombreEmpresa.Text.Trim(), CUIT = txtCUIT.Text.Trim(), Domicilio = txtDomicilio.Text.Trim(), Telefono = txtTelefono.Text.Trim() };
                    idEmpresaFinal = _bllEmpresa.Guardar(nuevaEmpresa);
                    if (idEmpresaFinal <= 0) throw new Exception("No se pudo registrar la nueva empresa.");
                }

                
                var planParaFacturacion = new BENivelServicio
                {
                    Codigo = plan.ID,
                    Nombre = plan.Nombre,
                    CostoMensual = plan.PrecioMensual
                };

                var pago = new BEPago
                {
                    Empresa = new BEEmpresa { Codigo = idEmpresaFinal },
                    Usuario = usuarioActual,
                    Plan = planParaFacturacion 
                };

                if (chkTarjeta.Checked)
                {
                    if (Convert.ToDecimal(txtMontoTarjeta.Text) < plan.PrecioMensual)
                        throw new Exception("El pago no cubre el monto de suscripcion");
                    pago.Tarjeta = new BETarjetaCredito { Titular = txtNombreTitular.Text, Numero = txtNumeroTarjeta.Text, Expiracion = txtExpiracion.Text, CVV = txtCVV.Text, MontoAbonado = double.Parse(txtMontoTarjeta.Text, CultureInfo.InvariantCulture) };
                }

                if (chkTransferencia.Checked)
                {
                    if (Convert.ToDecimal(txtMontoTransferencia.Text) < plan.PrecioMensual)
                        throw new Exception("La transferencia no cubre el monto de suscripcion");
                    pago.Transferencia = new BETransferencia { CodigoComprobante = txtComprobanteTransferencia.Text, MontoAbonado = double.Parse(txtMontoTransferencia.Text, CultureInfo.InvariantCulture) };
                }

                if (chkNotaCredito.Checked)
                {
                    foreach (RepeaterItem item in rptNotasCredito.Items)
                    {
                        var chkUsar = (CheckBox)item.FindControl("chkUsarNC");
                        if (chkUsar != null && chkUsar.Checked)
                        {
                            var hfIdNC = (HiddenField)item.FindControl("hfIdNC");
                            int idNC = Convert.ToInt32(hfIdNC.Value);
                            pago.NotaCreditoUtilizada = this.NotasDeCreditoDisponibles.FirstOrDefault(nc => nc.Codigo == idNC);
                            break;
                        }
                    }
                }

                int idFactura = _bllFacturacion.ProcesarPagoYFacturar(pago);
                if (idFactura <= 0) throw new Exception("El pago no pudo ser procesado.");

                usuarioActual.Empresa = new BEEmpresa { Codigo = idEmpresaFinal };
                if (!_bllUsuario.Guardar(usuarioActual)) throw new Exception("No se pudo actualizar la empresa del usuario.");
                _bllUsuario.HacerAdministrador(usuarioActual);

                // Pasamos el objeto convertido al email
                EnviarEmailConfirmacion(usuarioActual, planParaFacturacion, idFactura);
                Session["Usuario"] = _bllUsuario.ListarObjeto(usuarioActual);
                Response.Redirect($"~/ContratacionExitosa.aspx?facturaId={idFactura}");
            }
            catch (Exception ex)
            {
                lblError.Text = "Ocurrió un error al procesar la contratación: " + ex.Message;
                lblError.Visible = true;
            }
        }

        private void EnviarEmailConfirmacion(BEUsuario usuario, BENivelServicio plan, int facturaId)
        {
            try
            {
                string titulo = _traductor.Traducir("Email_Contratacion_Titulo");
                string texto1 = string.Format(_traductor.Traducir("Email_Contratacion_Texto1"), usuario.Nombre, plan.Nombre);
                string texto2 = _traductor.Traducir("Email_Contratacion_Texto2");
                string textoBoton = _traductor.Traducir("Email_Contratacion_Boton");
                string urlBoton = new Uri(Request.Url, ResolveUrl("~/Consola.aspx")).ToString();
                string cuerpoMail = BLLEmail.ObtenerCuerpoMail(titulo, texto1, texto2, textoBoton, urlBoton);

                BLLEmail.EnviarMail(usuario.Email, titulo, cuerpoMail);
            }
            catch { /* Ignorar errores de email para no detener el flujo */ }
        }
    }
}
