using System;
using System.Web.UI.WebControls;
using BE;
using BLL;

namespace SkillMirror
{
    public partial class EditarFAQ : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Siempre enlazamos los idiomas disponibles para crear los campos
                CargarIdiomasDisponibles();

                if (Request.QueryString["id"] != null)
                {
                    // Modo Edición: cargamos los datos existentes
                    int faqId = Convert.ToInt32(Request.QueryString["id"]);
                    CargarFAQ(faqId);
                }
                // En modo Creación, simplemente se muestran los campos vacíos.
            }
        }

        private void CargarIdiomasDisponibles()
        {
            rptTraducciones.DataSource = _traductor.IdiomasDisponibles;
            rptTraducciones.DataBind();
        }

        private void CargarFAQ(int id)
        {
            BLLFAQ bll = new BLLFAQ();
            BEFAQ faq = bll.ListarObjetoConTraducciones(new BEFAQ { Codigo = id });

            if (faq != null)
            {
                hfFAQId.Value = faq.Codigo.ToString();
                txtOrden.Text = faq.Orden.ToString();

                // Recorremos el Repeater para poblar cada campo de texto
                foreach (RepeaterItem item in rptTraducciones.Items)
                {
                    var hfIdiomaId = (HiddenField)item.FindControl("hfIdiomaId");
                    var txtPregunta = (TextBox)item.FindControl("txtPregunta");
                    var txtRespuesta = (TextBox)item.FindControl("txtRespuesta");

                    int idiomaId = Convert.ToInt32(hfIdiomaId.Value);

                    // Buscamos la traducción para este idioma en los diccionarios
                    if (faq.TraduccionesPregunta.ContainsKey(idiomaId))
                    {
                        txtPregunta.Text = faq.TraduccionesPregunta[idiomaId];
                    }
                    if (faq.TraduccionesRespuesta.ContainsKey(idiomaId))
                    {
                        txtRespuesta.Text = faq.TraduccionesRespuesta[idiomaId];
                    }
                }
            }
            else
            {
                Response.Redirect("AdminFAQs.aspx"); // Si el ID no existe, volvemos al listado
            }
        }

        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                BEFAQ faq = new BEFAQ
                {
                    Codigo = Convert.ToInt32(hfFAQId.Value),
                    Orden = Convert.ToInt32(txtOrden.Text)
                };

                // Leemos los textos de cada idioma desde el Repeater
                foreach (RepeaterItem item in rptTraducciones.Items)
                {
                    var hfIdiomaId = (HiddenField)item.FindControl("hfIdiomaId");
                    var txtPregunta = (TextBox)item.FindControl("txtPregunta");
                    var txtRespuesta = (TextBox)item.FindControl("txtRespuesta");

                    int idiomaId = Convert.ToInt32(hfIdiomaId.Value);

                    // Solo agregamos la traducción si el campo no está vacío
                    if (!string.IsNullOrWhiteSpace(txtPregunta.Text))
                    {
                        faq.TraduccionesPregunta[idiomaId] = txtPregunta.Text.Trim();
                    }
                    if (!string.IsNullOrWhiteSpace(txtRespuesta.Text))
                    {
                        faq.TraduccionesRespuesta[idiomaId] = txtRespuesta.Text.Trim();
                    }
                }

                BLLFAQ bll = new BLLFAQ();
                bll.Guardar(faq);

                Response.Redirect("AdminFAQs.aspx");
            }
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminFAQs.aspx");
        }

        public override void ActualizarTraducciones()
        {
            // Títulos y encabezados
            bool esEdicion = !string.IsNullOrEmpty(Request.QueryString["id"]);
            this.Title = _traductor.Traducir(esEdicion ? "EditarFAQ_Page_Title_Editar" : "EditarFAQ_Page_Title_Crear");
            headerTitle.InnerText = _traductor.Traducir(esEdicion ? "EditarFAQ_Header_Titulo_Editar" : "EditarFAQ_Header_Titulo_Crear");

            // Textos del formulario
            lblOrden.Text = _traductor.Traducir("EditarFAQ_Label_Orden");
            cvOrden.ErrorMessage = _traductor.Traducir("EditarFAQ_Validator_Orden");
            h5Traducciones.InnerText = _traductor.Traducir("EditarFAQ_Subheader_Traducciones");

            // Botones
            btnCancelar.Text = _traductor.Traducir("Admin_Boton_Cancelar");
            btnGuardar.Text = _traductor.Traducir("Admin_Boton_Guardar");

            // Textos dentro del Repeater
            foreach (RepeaterItem item in rptTraducciones.Items)
            {
                ((Literal)item.FindControl("litPreguntaLabel")).Text = _traductor.Traducir("EditarFAQ_Label_Pregunta");
                ((Literal)item.FindControl("litRespuestaLabel")).Text = _traductor.Traducir("EditarFAQ_Label_Respuesta");
            }
        }
    }
}