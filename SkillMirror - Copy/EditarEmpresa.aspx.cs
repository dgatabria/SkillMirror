using System;
using System.Web.UI;
using BLL;
using BE;

namespace SkillMirror
{
    public partial class EditarEmpresa : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    // Usamos el traductor para el título en modo "Editar"
                    litTitulo.Text = _traductor.Traducir("EditarEmpresa_Titulo_Editar");
                    int empresaId = Convert.ToInt32(Request.QueryString["id"]);
                    hfEmpresaId.Value = empresaId.ToString();
                    CargarDatosEmpresa(empresaId);
                }
                else
                {
                    // Usamos el traductor para el título en modo "Crear"
                    litTitulo.Text = _traductor.Traducir("EditarEmpresa_Titulo_Crear");
                }
            }
        }

        public override void ActualizarTraducciones()
        {
            this.Title = _traductor.Traducir("EditarEmpresa_Page_Title");

            // Si estamos en modo edición, volvemos a traducir el título por si cambia el idioma
            if (Request.QueryString["id"] != null)
            {
                litTitulo.Text = _traductor.Traducir("EditarEmpresa_Titulo_Editar");
            }
            else
            {
                litTitulo.Text = _traductor.Traducir("EditarEmpresa_Titulo_Crear");
            }

            // Labels del formulario
            lblNombre.Text = _traductor.Traducir("EditarEmpresa_Label_Nombre");
            valNombre.ErrorMessage = _traductor.Traducir("EditarEmpresa_Error_Nombre");
            lblCUIT.Text = _traductor.Traducir("EditarEmpresa_Label_CUIT");
            valCUIT.ErrorMessage = _traductor.Traducir("EditarEmpresa_Error_CUIT");
            lblTelefono.Text = _traductor.Traducir("EditarEmpresa_Label_Telefono");
            lblDomicilio.Text = _traductor.Traducir("EditarEmpresa_Label_Domicilio");
            lblMedioDePago.Text = _traductor.Traducir("EditarEmpresa_Label_MedioDePago");

            // Botones
            btnGuardar.Text = _traductor.Traducir("EditarEmpresa_Boton_Guardar");
            btnCancelar.Text = _traductor.Traducir("EditarEmpresa_Boton_Cancelar");
        }

        private void CargarDatosEmpresa(int id)
        {
            BLLEmpresa bll = new BLLEmpresa();
            BEEmpresa empresa = bll.ListarObjeto(new BEEmpresa { Codigo = id });
            if (empresa != null)
            {
                txtNombre.Text = empresa.Nombre;
                txtCUIT.Text = empresa.CUIT;
                txtTelefono.Text = empresa.Telefono;
                txtDomicilio.Text = empresa.Domicilio;
                txtMedioDePago.Text = empresa.MedioDePago;
            }
        }

        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                BLLEmpresa bll = new BLLEmpresa();
                BEEmpresa empresa = new BEEmpresa();

                empresa.Codigo = Convert.ToInt32(hfEmpresaId.Value);
                empresa.Nombre = txtNombre.Text;
                empresa.CUIT = txtCUIT.Text;
                empresa.Telefono = txtTelefono.Text;
                empresa.Domicilio = txtDomicilio.Text;
                empresa.MedioDePago = txtMedioDePago.Text;

                bll.Guardar(empresa);

                Response.Redirect("AdminEmpresas.aspx");
            }
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminEmpresas.aspx");
        }
    }
}