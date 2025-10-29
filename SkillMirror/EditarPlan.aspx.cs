using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;

namespace SkillMirror
{
    public partial class EditarPlan : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarDatos();
            }
        }

        public override void ActualizarTraducciones()
        {
            // Títulos y etiquetas del formulario
            bool esNuevo = string.IsNullOrEmpty(Request.QueryString["id"]);
            this.Title = _traductor.Traducir(esNuevo ? "EditarPlan_Page_Title_Nuevo" : "EditarPlan_Page_Title_Editar");
            headerTitle.InnerText = _traductor.Traducir(esNuevo ? "AdminPlanes_Modal_Titulo_Nuevo" : "AdminPlanes_Modal_Titulo_Editar");

            lblNombre.InnerText = _traductor.Traducir("AdminPlanes_Modal_Nombre");
            lblSubtitulo.InnerText = _traductor.Traducir("AdminPlanes_Modal_Subtitulo");
            lblPrecio.InnerText = _traductor.Traducir("AdminPlanes_Modal_Precio");
            lblOrden.InnerText = _traductor.Traducir("AdminPlanes_Modal_Orden");
            lblActivo.InnerText = _traductor.Traducir("AdminPlanes_Modal_Activo");
            lblDestacado.InnerText = _traductor.Traducir("AdminPlanes_Modal_Destacado");
            headerFeatures.InnerText = _traductor.Traducir("AdminPlanes_Modal_Header_Features");

            btnGuardar.Text = _traductor.Traducir("AdminPlanes_Boton_Guardar");
            btnCancelar.Text = _traductor.Traducir("AdminPlanes_Boton_Cancelar");
        }

        private void CargarDatos()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                // --- Modo Edición ---
                int planId = Convert.ToInt32(Request.QueryString["id"]);
                var bll = new BLLPlanes();
                BEPlan plan = bll.ListarPorID(planId);

                if (plan != null)
                {
                    hidPlanID.Value = plan.ID.ToString();
                    txtNombre.Text = plan.Nombre;
                    txtSubtitulo.Text = plan.Subtitulo;
                    txtPrecio.Text = plan.PrecioMensual.ToString("F2", CultureInfo.InvariantCulture);
                    txtOrden.Text = plan.Orden.ToString();
                    chkActivo.Checked = plan.Activo;
                    chkDestacado.Checked = plan.EsDestacado;
                    rptFeatures.DataSource = plan.Features;
                    rptFeatures.DataBind();
                }
            }
            else
            {
                // --- Modo Creación ---
                hidPlanID.Value = "0";
                chkActivo.Checked = true;

                var bllFeatures = new BLLFeatures();
                var featuresBase = bllFeatures.Listar();
                var planFeatures = new List<BEPlanFeature>();

                foreach (var feature in featuresBase)
                {
                    planFeatures.Add(new BEPlanFeature
                    {
                        ID_Feature = feature.ID,
                        FeatureNombre = feature.Nombre,
                        Descripcion = ""
                    });
                }
                rptFeatures.DataSource = planFeatures;
                rptFeatures.DataBind();
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                var plan = new BEPlan
                {
                    ID = Convert.ToInt32(hidPlanID.Value),
                    Nombre = txtNombre.Text,
                    Subtitulo = txtSubtitulo.Text,
                    PrecioMensual = decimal.Parse(txtPrecio.Text, CultureInfo.InvariantCulture),
                    Orden = int.Parse(txtOrden.Text),
                    Activo = chkActivo.Checked,
                    EsDestacado = chkDestacado.Checked,
                    Features = new List<BEPlanFeature>()
                };

                foreach (RepeaterItem item in rptFeatures.Items)
                {
                    var hidFeatureID = (HiddenField)item.FindControl("hidFeatureID");
                    var txtFeatureDescripcion = (TextBox)item.FindControl("txtFeatureDescripcion");
                    plan.Features.Add(new BEPlanFeature
                    {
                        ID_Feature = Convert.ToInt32(hidFeatureID.Value),
                        Descripcion = txtFeatureDescripcion.Text
                    });
                }

                var bll = new BLLPlanes();
                bll.Guardar(plan);

                Response.Redirect("AdminPlanes.aspx");
            }
            catch (Exception)
            {
                // Aquí podrías agregar lógica para mostrar un mensaje de error
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminPlanes.aspx");
        }
    }
}

