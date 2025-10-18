using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Web.UI;
using BLL;
using BE;

namespace SkillMirror
{
    public partial class EditarEncuesta : BasePage
    {
        protected string _jsonTraducciones = "{}";
        protected string _jsonPreguntas = "[]";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    int encuestaId = Convert.ToInt32(Request.QueryString["id"]);
                    hfEncuestaId.Value = encuestaId.ToString();
                    CargarDatos(encuestaId);
                }
            }
        }

        public override void ActualizarTraducciones()
        {
            this.Title = _traductor.Traducir("EditarEncuesta_Page_Title");
            litTitulo.Text = (hfEncuestaId.Value == "0") ? _traductor.Traducir("EditarEncuesta_Titulo_Crear") : _traductor.Traducir("EditarEncuesta_Titulo_Editar");

            lblTitulo.Text = _traductor.Traducir("EditarEncuesta_Label_Titulo");
            lblDescripcion.Text = _traductor.Traducir("EditarEncuesta_Label_Descripcion");
            lblFechaVencimiento.Text = _traductor.Traducir("EditarEncuesta_Label_FechaVencimiento");
            preguntasTitle.InnerText = _traductor.Traducir("EditarEncuesta_Subtitulo_Preguntas");
            litBotonAgregarPregunta.Text = _traductor.Traducir("EditarEncuesta_Boton_AgregarPregunta");
            btnGuardar.Text = _traductor.Traducir("Admin_Boton_Guardar");
            btnCancelar.Text = _traductor.Traducir("Admin_Boton_Cancelar");

            var serializer = new JavaScriptSerializer();
            _jsonTraducciones = serializer.Serialize(new
            {
                pregunta = _traductor.Traducir("JS_Pregunta"),
                placeholderTextoPregunta = _traductor.Traducir("JS_Placeholder_Pregunta"),
                tipoTextoLibre = _traductor.Traducir("JS_Tipo_TextoLibre"),
                tipoPuntaje = _traductor.Traducir("JS_Tipo_Puntaje"),
                tipoMultipleChoice = _traductor.Traducir("JS_Tipo_MultipleChoice"),
                agregarOpcion = _traductor.Traducir("JS_Boton_AgregarOpcion"),
                placeholderOpcion = _traductor.Traducir("JS_Placeholder_Opcion")
            });
        }

        private void CargarDatos(int id)
        {
            BLLEncuesta bll = new BLLEncuesta();
            BEEncuesta encuesta = bll.ListarObjeto(new BEEncuesta { Codigo = id });
            if (encuesta != null)
            {
                txtTitulo.Text = encuesta.Titulo;
                txtDescripcion.Text = encuesta.Descripcion;
                if (encuesta.FechaVencimiento.HasValue)
                    txtFechaVencimiento.Text = encuesta.FechaVencimiento.Value.ToString("yyyy-MM-dd");

                // Pasamos las preguntas existentes al JavaScript
                _jsonPreguntas = new JavaScriptSerializer().Serialize(encuesta.Preguntas);
            }
        }

        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            var serializer = new JavaScriptSerializer();
            var preguntas = serializer.Deserialize<List<BEPregunta>>(hfPreguntasData.Value);

            BEEncuesta encuesta = new BEEncuesta
            {
                Codigo = Convert.ToInt32(hfEncuestaId.Value),
                Titulo = txtTitulo.Text,
                Descripcion = txtDescripcion.Text,
                FechaVencimiento = string.IsNullOrEmpty(txtFechaVencimiento.Text) ? (DateTime?)null : Convert.ToDateTime(txtFechaVencimiento.Text),
                Activa = true,
                Preguntas = preguntas
            };

            BLLEncuesta bll = new BLLEncuesta();
            bll.Guardar(encuesta);

            Response.Redirect("AdminEncuestas.aspx");
        }

        protected void BtnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminEncuestas.aspx");
        }
    }
}