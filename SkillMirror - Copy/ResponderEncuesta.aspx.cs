using BE;
using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SkillMirror
{
    public partial class ResponderEncuesta : BasePage
    {
        // Guardamos la estructura de la invitación para tenerla disponible en todo el ciclo de vida
        private BEEncuestaInvitacion _invitacion;

        // Page_Init se ejecuta antes que Page_Load. Es el lugar ideal para crear controles dinámicos.
        protected void Page_Init(object sender, EventArgs e)
        {
            if (Guid.TryParse(Request.QueryString["token"], out Guid token))
            {
                BLLEncuesta bll = new BLLEncuesta();
                _invitacion = bll.ObtenerEncuestaPorToken(token);

                // ¡LA CLAVE! Recreamos los controles en CADA carga de la página (GET y POST).
                // Si es un PostBack, el framework se encargará de rellenarlos con los valores que envió el usuario.
                if (_invitacion != null)
                {
                    GenerarControlesPreguntas(_invitacion.Encuesta.Preguntas);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // La lógica que solo debe correr en la primera carga (GET) se queda aquí.
            if (!IsPostBack)
            {
                if (_invitacion == null)
                {
                    MostrarResultado("error", "ResponderEncuesta_Msg_Error_LinkInvalido");
                    return;
                }
                if (_invitacion.Respondida)
                {
                    MostrarResultado("info", "ResponderEncuesta_Msg_Info_YaRespondida");
                    return;
                }
                if (_invitacion.Encuesta.FechaVencimiento.HasValue && _invitacion.Encuesta.FechaVencimiento < DateTime.Now)
                {
                    MostrarResultado("warning", "ResponderEncuesta_Msg_Warning_Vencida");
                    return;
                }

                // Si todo está bien, poblamos los datos de la encuesta
                hfInvitacionId.Value = _invitacion.Codigo.ToString();
                litTituloEncuesta.Text = _invitacion.Encuesta.Titulo;
                litDescripcionEncuesta.Text = _invitacion.Encuesta.Descripcion;
            }
        }

        public override void ActualizarTraducciones()
        {
            this.Title = _traductor.Traducir("ResponderEncuesta_Page_Title");
            btnEnviarRespuestas.Text = _traductor.Traducir("ResponderEncuesta_Boton_Enviar");
        }

        private void GenerarControlesPreguntas(List<BEPregunta> preguntas)
        {
            foreach (var p in preguntas)
            {
                var div = new Panel { CssClass = "mb-4" };
                var label = new Label { Text = p.TextoPregunta, CssClass = "form-label fw-bold" };
                div.Controls.Add(label);

                string controlId = $"pregunta_{p.Codigo}";

                switch (p.TipoPregunta)
                {
                    case "TEXTO_LIBRE":
                        var txt = new TextBox { ID = controlId, CssClass = "form-control", TextMode = TextBoxMode.MultiLine, Rows = 3 };
                        div.Controls.Add(txt);
                        break;
                    case "PUNTAJE_1_5":
                        var rblPuntaje = new RadioButtonList { ID = controlId, CssClass = "form-check form-check-inline", RepeatDirection = RepeatDirection.Horizontal, RepeatLayout = RepeatLayout.Flow };
                        for (int i = 1; i <= 5; i++) { rblPuntaje.Items.Add(new ListItem(i.ToString(), i.ToString())); }
                        div.Controls.Add(rblPuntaje);
                        break;
                    case "OPCION_MULTIPLE":
                        var rblOpciones = new RadioButtonList { ID = controlId, CssClass = "form-check" };
                        foreach (var opt in p.Opciones) { rblOpciones.Items.Add(new ListItem(HttpUtility.HtmlEncode(opt.TextoOpcion), opt.Codigo.ToString())); }
                        div.Controls.Add(rblOpciones);
                        break;
                }
                phPreguntas.Controls.Add(div);
            }
        }

        protected void BtnEnviarRespuestas_Click(object sender, EventArgs e)
        {
            // Como los controles se recrearon en Page_Init, ahora sí los podemos encontrar.
            if (_invitacion != null)
            {
                BLLEncuesta bll = new BLLEncuesta();
                var respuestas = new List<BERespuesta>();

                foreach (BEPregunta pregunta in _invitacion.Encuesta.Preguntas)
                {
                    Control controlRespuesta = phPreguntas.FindControl($"pregunta_{pregunta.Codigo}");
                    if (controlRespuesta != null)
                    {
                        var respuesta = new BERespuesta
                        {
                            Invitacion = { Codigo = _invitacion.Codigo },
                            Pregunta = { Codigo = pregunta.Codigo }
                        };

                        switch (pregunta.TipoPregunta)
                        {
                            case "TEXTO_LIBRE":
                                if (controlRespuesta is TextBox txt) respuesta.RespuestaTexto = txt.Text;
                                break;
                            case "PUNTAJE_1_5":
                                if (controlRespuesta is RadioButtonList rblPuntaje && !string.IsNullOrEmpty(rblPuntaje.SelectedValue))
                                    respuesta.Puntuacion = Convert.ToInt32(rblPuntaje.SelectedValue);
                                break;
                            case "OPCION_MULTIPLE":
                                if (controlRespuesta is RadioButtonList rblOpciones && !string.IsNullOrEmpty(rblOpciones.SelectedValue))
                                    respuesta.OpcionSeleccionada.Codigo = Convert.ToInt32(rblOpciones.SelectedValue);
                                break;
                        }
                        respuestas.Add(respuesta);
                    }
                }

                bll.GuardarRespuestas(respuestas);
                bll.FinalizarEncuesta(_invitacion.Token);

                MostrarResultado("exito", "ResponderEncuesta_Msg_Exito");
            }
            else
            {
                MostrarResultado("error", "ResponderEncuesta_Msg_Error_Inesperado");
            }
        }

        private void MostrarResultado(string tipo, string tag)
        {
            pnlEncuesta.Visible = false;
            pnlResultado.Visible = true;
            string claseAlerta = (tipo == "exito") ? "alert-success" : (tipo == "info") ? "alert-info" : (tipo == "warning") ? "alert-warning" : "alert-danger";
            litResultado.Text = $"<div class='alert {claseAlerta}'><h4>{_traductor.Traducir(tag)}</h4></div>";
        }
    }
}