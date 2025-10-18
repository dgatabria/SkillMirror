using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Script.Serialization; // Necesario para JSON
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Linq;

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
            }
        }

        private void CargarPlanes()
        {
            var bll = new BLLNivelServicio();
            rptPlanes.DataSource = bll.Listar();
            rptPlanes.DataBind();
        }

        protected void RptPlanes_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var plan = (BENivelServicio)e.Item.DataItem;

                var cardHeader = (HtmlGenericControl)e.Item.FindControl("cardHeader");
                var cardTitle = (HtmlGenericControl)e.Item.FindControl("cardTitle");
                var litPrecio = (Literal)e.Item.FindControl("litPrecio");
                var cardText = (HtmlGenericControl)e.Item.FindControl("cardText");
                var litComparar = (Literal)e.Item.FindControl("litComparar");
                var pnlBotonContratar = (Panel)e.Item.FindControl("pnlBotonContratar");
                var btnSuscribir = (Button)e.Item.FindControl("btnSuscribir");

                // --- CORRECCIÓN CLAVE ---
                // Limpiamos el nombre y lo convertimos a minúsculas para construir la etiqueta
                string planKey = plan.Nombre.Trim().ToLower();

                cardTitle.InnerText = _traductor.Traducir($"catalogo_plan_{planKey}_nombre");
                cardText.InnerText = _traductor.Traducir($"catalogo_plan_{planKey}_texto");
                // -----------------------

                litComparar.Text = _traductor.Traducir("Catalogo_Plan_Boton_Comparar");
                btnSuscribir.Text = _traductor.Traducir("Catalogo_Boton_Suscribir");

                if (plan.CostoMensual == 0)
                {
                    litPrecio.Text = _traductor.Traducir("Catalogo_Plan_Enterprise_Precio");
                }
                else
                {
                    litPrecio.Text = $"${plan.CostoMensual:N0}<small class='text-muted fw-light'>{_traductor.Traducir("Catalogo_Plan_Termino_Mes")}</small>";
                }

                if (plan.Nombre.Trim().ToUpper() == "PRO")
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
            this.Title = _traductor.Traducir("Catalogo_Page_Title");
            headerTitle.InnerText = _traductor.Traducir("Catalogo_Header_Titulo");
            headerSubtitle.InnerText = _traductor.Traducir("Catalogo_Header_Subtitulo");
            comparisonTitle.InnerText = _traductor.Traducir("Catalogo_Tabla_Titulo");

            CargarPlanes();
            PrepararDatosParaScript();
        }

        private void PrepararDatosParaScript()
        {
            var bll = new BLLNivelServicio();
            var planes = bll.Listar();
            var planData = new Dictionary<string, object>();

            foreach (var plan in planes)
            {
                // --- CORRECCIÓN CLAVE ---
                string planKey = plan.Nombre.Trim().ToLower();
                // -----------------------

                planData[planKey] = new
                {
                    name = _traductor.Traducir($"catalogo_plan_{planKey}_nombre"),
                    price = plan.CostoMensual > 0 ? $"${plan.CostoMensual:N0}" : _traductor.Traducir("Catalogo_Plan_Enterprise_Precio"),
                    procesos = _traductor.Traducir($"catalogo_feature_procesos_{planKey}"),
                    evaluaciones = _traductor.Traducir($"catalogo_feature_evaluaciones_{planKey}"),
                    reportes = _traductor.Traducir($"catalogo_feature_reportes_{planKey}"),
                    soporte = _traductor.Traducir($"catalogo_feature_soporte_{planKey}"),
                    integraciones = _traductor.Traducir($"catalogo_feature_integraciones_{planKey}")
                };
            }

            var features = new object[]
            {
        new { key = "header", headerLabel = _traductor.Traducir("Catalogo_Tabla_Header_Caracteristica") },
        new { key = "price", label = _traductor.Traducir("Catalogo_Tabla_Feature_Precio") },
        new { key = "procesos", label = _traductor.Traducir("Catalogo_Tabla_Feature_Procesos") },
        new { key = "evaluaciones", label = _traductor.Traducir("Catalogo_Tabla_Feature_Evaluaciones") },
        new { key = "reportes", label = _traductor.Traducir("Catalogo_Tabla_Feature_Reportes") },
        new { key = "soporte", label = _traductor.Traducir("Catalogo_Tabla_Feature_Soporte") },
        new { key = "integraciones", label = _traductor.Traducir("Catalogo_Tabla_Feature_Integraciones") }
            };

            var serializer = new JavaScriptSerializer();
            _jsonPlanData = serializer.Serialize(planData);
            _jsonFeatures = serializer.Serialize(features);
        }
    }
}