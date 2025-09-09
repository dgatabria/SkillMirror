using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.Script.Serialization; // Necesario para JSON

namespace SkillMirror
{
    public partial class Catalogo : BasePage
    {
        // Propiedades públicas para inyectar JSON en la página .aspx
        protected string _jsonPlanData = "{}";
        protected string _jsonFeatures = "[]";

        protected void Page_Load(object sender, EventArgs e)
        {
            // La suscripción y manejo del idioma se hace en la BasePage
        }

        public override void ActualizarTraducciones()
        {
            // --- Traducción de la página ---
            this.Title = _traductor.Traducir("Catalogo_Page_Title");
            headerTitle.InnerText = _traductor.Traducir("Catalogo_Header_Titulo");
            headerSubtitle.InnerText = _traductor.Traducir("Catalogo_Header_Subtitulo");

            // Plan BASIC
            basicCardTitle.InnerText = _traductor.Traducir("Catalogo_Plan_Basic_Titulo");
            basicPriceTerm.InnerText = _traductor.Traducir("Catalogo_Plan_Termino_Mes");
            basicCardText.InnerText = _traductor.Traducir("Catalogo_Plan_Basic_Texto");
            litBasicCompare.Text = _traductor.Traducir("Catalogo_Plan_Boton_Comparar");

            // Plan PRO
            proCardTitle.InnerText = _traductor.Traducir("Catalogo_Plan_Pro_Titulo");
            proPriceTerm.InnerText = _traductor.Traducir("Catalogo_Plan_Termino_Mes");
            proCardText.InnerText = _traductor.Traducir("Catalogo_Plan_Pro_Texto");
            litProCompare.Text = _traductor.Traducir("Catalogo_Plan_Boton_Comparar");

            // Plan ENTERPRISE
            enterpriseCardTitle.InnerText = _traductor.Traducir("Catalogo_Plan_Enterprise_Titulo");
            enterprisePrice.InnerText = _traductor.Traducir("Catalogo_Plan_Enterprise_Precio");
            enterpriseCardText.InnerText = _traductor.Traducir("Catalogo_Plan_Enterprise_Texto");
            litEnterpriseCompare.Text = _traductor.Traducir("Catalogo_Plan_Boton_Comparar");

            // --- Traducción de la tabla comparativa (vía JSON) ---
            comparisonTitle.InnerText = _traductor.Traducir("Catalogo_Tabla_Titulo");

            // Preparamos los datos para el JavaScript
            PrepararDatosParaScript();
        }

        private void PrepararDatosParaScript()
        {
            // 1. Datos de los planes (los textos son los tags que se traducirán en el script)
            var planData = new
            {
                basic = new
                {
                    name = _traductor.Traducir("Catalogo_Plan_Basic_Nombre"),
                    price = _traductor.Traducir("Catalogo_Plan_Basic_Precio"),
                    procesos = _traductor.Traducir("Catalogo_Feature_Procesos_Basic"),
                    evaluaciones = _traductor.Traducir("Catalogo_Feature_Evaluaciones_Basic"),
                    reportes = _traductor.Traducir("Catalogo_Feature_Reportes_Basic"),
                    soporte = _traductor.Traducir("Catalogo_Feature_Soporte_Basic"),
                    // CAMBIO AQUÍ
                    integraciones = _traductor.Traducir("Catalogo_Feature_Integraciones_Basic").Replace("❌", "&#10060;")
                },
                pro = new
                {
                    name = _traductor.Traducir("Catalogo_Plan_Pro_Nombre"),
                    price = _traductor.Traducir("Catalogo_Plan_Pro_Precio"),
                    procesos = _traductor.Traducir("Catalogo_Feature_Procesos_Pro"),
                    evaluaciones = _traductor.Traducir("Catalogo_Feature_Evaluaciones_Pro"),
                    reportes = _traductor.Traducir("Catalogo_Feature_Reportes_Pro"),
                    soporte = _traductor.Traducir("Catalogo_Feature_Soporte_Pro"),
                    // CAMBIO AQUÍ
                    integraciones = _traductor.Traducir("Catalogo_Feature_Integraciones_Pro").Replace("✔️", "&#10004;")
                },
                enterprise = new
                {
                    name = _traductor.Traducir("Catalogo_Plan_Enterprise_Nombre"),
                    price = _traductor.Traducir("Catalogo_Plan_Enterprise_Precio"),
                    procesos = _traductor.Traducir("Catalogo_Feature_Procesos_Enterprise"),
                    evaluaciones = _traductor.Traducir("Catalogo_Feature_Evaluaciones_Enterprise"),
                    reportes = _traductor.Traducir("Catalogo_Feature_Reportes_Enterprise"),
                    soporte = _traductor.Traducir("Catalogo_Feature_Soporte_Enterprise"),
                    // CAMBIO AQUÍ
                    integraciones = _traductor.Traducir("Catalogo_Feature_Integraciones_Enterprise") //.Replace("✔️", "&#10004;")
                }
            };

            // 2. Etiquetas de las características (esto no cambia)
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

            // 3. Serializamos a JSON (esto no cambia)
            var serializer = new JavaScriptSerializer();
            _jsonPlanData = serializer.Serialize(planData);
            _jsonFeatures = serializer.Serialize(features);
        }
    }
}