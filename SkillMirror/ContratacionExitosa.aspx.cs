using System;

namespace SkillMirror
{
    public partial class ContratacionExitosa : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Leemos el nombre del plan desde el QueryString
                string nombrePlan = Request.QueryString["plan"];
                if (!string.IsNullOrEmpty(nombrePlan))
                {
                    litPlan.Text = nombrePlan;
                }
                else
                {
                    // Si por alguna razón no llega el nombre del plan, mostramos un texto genérico
                    litPlan.Text = "tu nuevo plan";
                }
            }
        }

        public override void ActualizarTraducciones()
        {
            this.Title = _traductor.Traducir("ContratacionExitosa_Page_Title");
            headerTitle.InnerText = _traductor.Traducir("ContratacionExitosa_Header");

            // Usamos String.Format para insertar el nombre del plan en el texto traducido
            subHeader.InnerHtml = string.Format(_traductor.Traducir("ContratacionExitosa_SubHeader"), $"<strong>{litPlan.Text}</strong>");

            textoAccion.InnerText = _traductor.Traducir("ContratacionExitosa_Texto");
            btnIrAConsola.Text = _traductor.Traducir("ContratacionExitosa_Boton");
        }
    }
}
