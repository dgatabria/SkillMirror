using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using BE;
using BLL;

namespace SkillMirror
{
    public partial class MiPerfil : BasePage
    {
        // Añadimos BLLFacturacion para acceder a los nuevos datos
        private BLLFacturacion _bllFacturacion;

        protected void Page_Load(object sender, EventArgs e)
        {
            _bllFacturacion = new BLLFacturacion(); // Instanciamos la BLL

            if (!IsPostBack)
            {
                CargarDatosUsuario();
            }
        }

        public override void ActualizarTraducciones()
        {
            this.Title = _traductor.Traducir("MiPerfil_Page_Title");
            headerTitle.InnerText = _traductor.Traducir("MiPerfil_Header_Titulo");

            // Card Datos Personales (Traducciones existentes)
            cardDatosTitle.InnerText = _traductor.Traducir("MiPerfil_CardDatos_Titulo");
            lblNombre.Text = _traductor.Traducir("MiPerfil_CardDatos_Label_Nombre");
            valNombre.ErrorMessage = _traductor.Traducir("MiPerfil_CardDatos_Error_Nombre");
            lblApellido.Text = _traductor.Traducir("MiPerfil_CardDatos_Label_Apellido");
            valApellido.ErrorMessage = _traductor.Traducir("MiPerfil_CardDatos_Error_Apellido");
            lblDNI.Text = _traductor.Traducir("MiPerfil_CardDatos_Label_DNI");
            valDNI.ErrorMessage = _traductor.Traducir("MiPerfil_CardDatos_Error_DNI");
            lblEmpresa.Text = _traductor.Traducir("MiPerfil_CardDatos_Label_Empresa");
            lblSuscrito.Text = _traductor.Traducir("MiPerfil_Label_Suscrito");
            lblSuscritoEncuestas.Text = _traductor.Traducir("MiPerfil_Label_Suscrito_Encuestas");
            btnGuardarDatos.Text = _traductor.Traducir("MiPerfil_CardDatos_Boton_Guardar");

            // Card Cambiar Contraseña (Traducciones existentes)
            cardPasswordTitle.InnerText = _traductor.Traducir("MiPerfil_CardPass_Titulo");
            lblPassActual.Text = _traductor.Traducir("MiPerfil_CardPass_Label_Actual");
            valPassActual.ErrorMessage = _traductor.Traducir("MiPerfil_CardPass_Error_Actual");
            lblPassNueva.Text = _traductor.Traducir("MiPerfil_CardPass_Label_Nueva");
            valPassNueva.ErrorMessage = _traductor.Traducir("MiPerfil_CardPass_Error_Nueva");
            lblConfirmPassNueva.Text = _traductor.Traducir("MiPerfil_CardPass_Label_Confirmar");
            valComparePass.ErrorMessage = _traductor.Traducir("MiPerfil_CardPass_Error_Confirmar");
            btnCambiarPassword.Text = _traductor.Traducir("MiPerfil_CardPass_Boton_Cambiar");

            // NUEVAS TRADUCCIONES: Card Estado de Cuenta
            cardCuentaTitle.InnerText = _traductor.Traducir("MiPerfil_CardCuenta_Titulo");
            lblPlanActualHeader.Text = _traductor.Traducir("MiPerfil_CardCuenta_PlanActual");
            lblProximoPagoHeader.Text = _traductor.Traducir("MiPerfil_CardCuenta_ProximoPago");
            lblNotasCreditoHeader.Text = _traductor.Traducir("MiPerfil_CardCuenta_NotasCredito");
            btnActualizarPlan.Text = _traductor.Traducir("MiPerfil_CardPlan_Boton_Actualizar");

            // NUEVAS TRADUCCIONES: Card Historial de Pagos
            cardPagosTitle.InnerText = _traductor.Traducir("MiPerfil_CardPagos_Titulo");
            if (gvHistorialPagos.HeaderRow != null)
            {
                gvHistorialPagos.HeaderRow.Cells[0].Text = _traductor.Traducir("MiPerfil_GridPagos_Fecha");
                gvHistorialPagos.HeaderRow.Cells[1].Text = _traductor.Traducir("MiPerfil_GridPagos_Concepto");
                gvHistorialPagos.HeaderRow.Cells[2].Text = _traductor.Traducir("MiPerfil_GridPagos_Monto");
            }
        }

        // Helper para traducir en el aspx
        public string Traducir(string tag)
        {
            return _traductor.Traducir(tag);
        }

        private void CargarDatosUsuario()
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                BLLUsuario bll = new BLLUsuario();
                BEUsuario usuario = bll.ListarObjeto(new BEUsuario { Email = HttpContext.Current.User.Identity.Name });
                if (usuario != null)
                {
                    ViewState["UsuarioID"] = usuario.Codigo;
                    txtNombre.Text = usuario.Nombre;
                    txtApellido.Text = usuario.Apellido;
                    txtDNI.Text = usuario.DNI;
                    chkSuscrito.Checked = usuario.SuscritoNewsletter;
                    chkSuscritoEncuestas.Checked = usuario.SuscritoEncuestas;

                    if (usuario.Empresa != null)
                    {
                        txtEmpresa.Text = usuario.Empresa.Nombre;
                        CargarDatosDeCuenta(usuario.Empresa); // Llamada al nuevo método
                    }
                }
            }
        }

        private void CargarDatosDeCuenta(BEEmpresa empresa)
        {
            // Cargar plan actual
            if (empresa.PlanContratado != null)
            {
                //string tagNombrePlan = $"catalogo_plan_{empresa.PlanContratado.Nombre.ToLower()}_nombre";
                
                //litNombrePlan.Text = _traductor.Traducir(tagNombrePlan);
                litNombrePlan.Text = empresa.PlanContratado.Nombre;
                litProximoPago.Text = empresa.PlanContratado.CostoMensual.ToString("N2");
            }
            else
            {
                litNombrePlan.Text = _traductor.Traducir("MiPerfil_Plan_Free");
                litProximoPago.Text = "0.00";
            }

            // Cargar historial de pagos y calcular días restantes
            var historialPagos = _bllFacturacion.ListarFacturasPorEmpresa(empresa.Codigo);
            gvHistorialPagos.DataSource = historialPagos;
            gvHistorialPagos.DataBind();

            if (historialPagos.Any())
            {
                var ultimoPago = historialPagos.First(); // La lista ya viene ordenada por fecha descendente
                var fechaVencimiento = ultimoPago.FechaEmision.AddDays(30); // Asumimos ciclo de 30 días
                var diasRestantes = (fechaVencimiento - DateTime.Today).Days;

                if (diasRestantes > 0)
                {
                    litDiasRestantes.Text = string.Format(_traductor.Traducir("MiPerfil_CardCuenta_DiasRestantes"), diasRestantes);
                }
                else
                {
                    litDiasRestantes.Text = _traductor.Traducir("MiPerfil_CardCuenta_Vencido");
                }
            }
            else
            {
                litDiasRestantes.Text = "-";
            }

            // Cargar notas de crédito
            var notasCredito = _bllFacturacion.ListarNotasCreditoActivasPorEmpresa(empresa);
            if (notasCredito.Any())
            {
                pnlNotasCredito.Visible = true;
                rptNotasCredito.DataSource = notasCredito;
                rptNotasCredito.DataBind();
            }
            else
            {
                pnlNotasCredito.Visible = false;
            }
        }

        protected void BtnActualizarPlan_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Catalogo.aspx?modo=contratar");
        }

        protected void BtnGuardarDatos_Click(object sender, EventArgs e)
        {
            if (Page.IsValid && ViewState["UsuarioID"] != null)
            {
                BLLUsuario bll = new BLLUsuario();
                int usuarioId = (int)ViewState["UsuarioID"];

                BEUsuario usuario = bll.ListarObjeto(new BEUsuario { Codigo = usuarioId });
                if (usuario != null)
                {
                    usuario.Nombre = txtNombre.Text.Trim();
                    usuario.Apellido = txtApellido.Text.Trim();
                    usuario.DNI = txtDNI.Text.Trim();
                    usuario.SuscritoNewsletter = chkSuscrito.Checked;
                    usuario.SuscritoEncuestas = chkSuscritoEncuestas.Checked;
                    bll.Guardar(usuario);

                    litMensaje.Text = $"<div class='alert alert-success'>{_traductor.Traducir("MiPerfil_Msg_Exito_Datos")}</div>";
                }
            }
        }

        protected void BtnCambiarPassword_Click(object sender, EventArgs e)
        {
            if (Page.IsValid && ViewState["UsuarioID"] != null)
            {
                BLLUsuario bll = new BLLUsuario();
                int usuarioId = (int)ViewState["UsuarioID"];

                BEUsuario usuario = new BEUsuario { Codigo = usuarioId, Password = txtPassNueva.Text };
                string passActual = txtPassActual.Text;

                try
                {
                    if (bll.CambiarPassword(usuario, passActual))
                    {
                        litMensaje.Text = $"<div class='alert alert-success'>{_traductor.Traducir("MiPerfil_Msg_Exito_Password")}</div>";
                        txtPassActual.Text = "";
                        txtPassNueva.Text = "";
                        txtConfirmPassNueva.Text = "";

                        BEUsuario usuarioCompleto = bll.ListarObjeto(new BEUsuario { Codigo = usuarioId });
                        if (usuarioCompleto != null)
                        {
                            string asunto = _traductor.Traducir("Email_PassCambiada_Asunto");
                            string cuerpo = BLLEmail.ObtenerCuerpoMail(
                                _traductor.Traducir("Email_PassCambiada_Titulo").Replace("{nombre}", usuarioCompleto.Nombre),
                                _traductor.Traducir("Email_PassCambiada_Texto1"),
                                _traductor.Traducir("Email_PassCambiada_Texto2"),
                                _traductor.Traducir("Email_PassCambiada_Boton"),
                                Page.ResolveUrl("~/Login.aspx"));

                            BLLEmail.EnviarMail(usuarioCompleto.Email, asunto, cuerpo);
                        }
                    }
                    else
                    {
                        litMensaje.Text = $"<div class='alert alert-danger'>{_traductor.Traducir("MiPerfil_Msg_Error_PasswordIncorrecta")}</div>";
                    }
                }
                catch (Exception ex)
                {
                    litMensaje.Text = $"<div class='alert alert-danger'>{_traductor.Traducir("MiPerfil_Msg_Error_PasswordGeneral")}: {ex.Message}</div>";
                }
            }
        }
    }
}

