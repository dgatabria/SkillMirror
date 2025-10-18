using System;
using System.Collections.Generic;
using BLL;
using BE;

namespace SkillMirror
{
    public partial class FAQs : BasePage // Importante heredar de BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarFAQs();
            }
        }

        private void CargarFAQs()
        {
            BLLFAQ bll = new BLLFAQ();
            List<BEFAQ> lista = bll.ListarPublicas();

            if (lista != null && lista.Count > 0)
            {
                rptFAQs.DataSource = lista;
                rptFAQs.DataBind();
                pnlNoFAQs.Visible = false;
            }
            else
            {
                // Si no hay FAQs, mostramos un mensaje
                rptFAQs.Visible = false;
                pnlNoFAQs.Visible = true;
            }
        }

        public override void ActualizarTraducciones()
        {
            // Traducimos los textos estáticos de la página
            this.Title = _traductor.Traducir("FAQs_Public_Page_Title");
            headerTitle.InnerText = _traductor.Traducir("FAQs_Public_Header_Titulo");
            pNoFAQs.InnerText = _traductor.Traducir("FAQs_Public_NoHayFAQs");

            // Volvemos a cargar la lista de FAQs para que se muestren en el nuevo idioma
            CargarFAQs();
        }
    }
}