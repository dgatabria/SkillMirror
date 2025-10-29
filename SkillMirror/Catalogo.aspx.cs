using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Management;
using System.Web.Script.Serialization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SkillMirror
{
    public partial class Catalogo : BasePage
    {
        protected string _jsonPlanData = "{}";
        protected string _jsonFeatures = "[]";
        private bool _modoContratar = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["modo"] == "contratar" && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                _modoContratar = true;
            }

            if (!IsPostBack)
            {
                CargarPlanes();
                PrepararDatosParaScript();
            }
        }

        private void CargarPlanes()
        {
            // Cambiamos a la nueva BLL de Planes
            var bll = new BLLPlanes();
            rptPlanes.DataSource = bll.Listar();
            rptPlanes.DataBind();
        }

        protected void RptPlanes_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                // El DataItem ahora es un objeto BEPlan completo
                var plan = (BEPlan)e.Item.DataItem;

                var cardHeader = (HtmlGenericControl)e.Item.FindControl("cardHeader");
                var cardTitle = (HtmlGenericControl)e.Item.FindControl("cardTitle");
                var litPrecio = (Literal)e.Item.FindControl("litPrecio");
                var cardText = (HtmlGenericControl)e.Item.FindControl("cardText");
                var litComparar = (Literal)e.Item.FindControl("litComparar");
                var pnlBotonContratar = (Panel)e.Item.FindControl("pnlBotonContratar");
                var btnSuscribir = (Button)e.Item.FindControl("btnSuscribir");

                // Asignamos los datos directamente desde el objeto, sin traducciones
                cardTitle.InnerText = plan.Nombre;
                cardText.InnerText = plan.Subtitulo;

                litComparar.Text = "Comparar Plan"; // Texto sin traducir por ahora
                btnSuscribir.Text = "Suscribir"; // Texto sin traducir por ahora

                if (plan.PrecioMensual == 0)
                {
                    litPrecio.Text = "Consultar"; // Precio para Enterprise
                }
                else
                {
                    // Usamos "/mes" como término fijo por ahora
                    litPrecio.Text = $"${plan.PrecioMensual:N0}<small class='text-muted fw-light'>/mes</small>";
                }

                // El plan destacado (Pro) se identifica con la propiedad EsDestacado
                if (plan.EsDestacado)
                {
                    cardHeader.Attributes["class"] = "card-header text-white";
                    cardHeader.Style.Add("background-color", "var(--primary-color)");
                }

                if (_modoContratar)
                {
                    pnlBotonContratar.Visible = true;
                }
            }
        }


        protected void RptPlanes_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Suscribir")
            {
                string planId = e.CommandArgument.ToString();
                Response.Redirect($"~/Contratar.aspx?planId={planId}");
            }
        }

        public override void ActualizarTraducciones()
        {
            // Mantenemos las traducciones del encabezado de la página
            this.Title = _traductor.Traducir("Catalogo_Page_Title");
            headerTitle.InnerText = _traductor.Traducir("Catalogo_Header_Titulo");
            headerSubtitle.InnerText = _traductor.Traducir("Catalogo_Header_Subtitulo");
            comparisonTitle.InnerText = _traductor.Traducir("Catalogo_Tabla_Titulo");

            // Recargamos los planes y el script en caso de cambio de idioma
            CargarPlanes();
            PrepararDatosParaScript();
        }

        private void PrepararDatosParaScript()
        {
            var bll = new BLLPlanes();
            var planes = bll.Listar();
            var planData = new Dictionary<string, object>();
            var featureList = new List<object>();

            // Agregamos el encabezado fijo para la tabla de comparación
            featureList.Add(new { key = "header", headerLabel = "Característica" });
            featureList.Add(new { key = "price", label = "Precio" });

            // Usamos el primer plan para obtener la lista de features dinámicamente
            if (planes.Any())
            {
                // Necesitamos la lista de features de la DB para la tabla
                var bllFeatures = new BLLFeatures(); // Suponiendo que la tienes
                var featuresFromDb = bllFeatures.Listar();

                foreach (var f in featuresFromDb)
                {
                    featureList.Add(new { key = f.Clave, label = f.Nombre });
                }
            }

            foreach (var plan in planes)
            {
                string planKey = plan.Nombre.Trim().ToLower();
                var planFeatures = new Dictionary<string, object>
                {
                    { "name", plan.Nombre },
                    { "price", plan.PrecioMensual > 0 ? $"${plan.PrecioMensual:N0}" : "Consultar" }
                };

                // Agregamos las descripciones de cada feature para este plan
                foreach (var feature in plan.Features)
                {
                    planFeatures[feature.Clave] = feature.Descripcion;
                }

                planData[planKey] = planFeatures;
            }

            var serializer = new JavaScriptSerializer();
            _jsonPlanData = serializer.Serialize(planData);
            _jsonFeatures = serializer.Serialize(featureList);
        }
    }
}
