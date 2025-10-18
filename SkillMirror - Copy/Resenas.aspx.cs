using System;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL;
using BE;
using System.Web.Script.Serialization; // Necesario para JSON

namespace SkillMirror
{
    // 1. Heredamos de BasePage (ya no necesita ITraducible directamente)
    public partial class Resenas : BasePage
    {
        // 2. Propiedad para inyectar las traducciones al JavaScript
        protected string _jsonTraducciones = "{}";

        protected void Page_Load(object sender, EventArgs e)
        {
            // La suscripción al traductor se maneja en la BasePage

            // La lógica para mostrar el formulario de nueva reseña se mantiene
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                pnlNuevaResena.Visible = true;
            }

            // Ya no llamamos a BindResenas() aquí
        }

        // 3. Sobreescribimos el método de la BasePage
        public override void ActualizarTraducciones()
        {
            // Traducción de los elementos estáticos de la página
            this.Title = _traductor.Traducir("Resenas_Page_Title");
            headerTitle.InnerText = _traductor.Traducir("Resenas_Header_Titulo");
            headerSubtitle.InnerText = _traductor.Traducir("Resenas_Header_Subtitulo");
            litCargando.Text = _traductor.Traducir("Resenas_Msg_Cargando");

            // Traducción del formulario para dejar una reseña
            litFormTitle.Text = _traductor.Traducir("Resenas_Form_Titulo");
            lblTuPuntuacion.Text = _traductor.Traducir("Resenas_Form_Label_Puntuacion");
            lblAsunto.Text = _traductor.Traducir("Resenas_Form_Label_Asunto");
            valAsunto.ErrorMessage = _traductor.Traducir("Resenas_Form_Error_Asunto");
            lblComentario.Text = _traductor.Traducir("Resenas_Form_Label_Comentario");
            valComentario.ErrorMessage = _traductor.Traducir("Resenas_Form_Error_Comentario");
            btnEnviarResena.Text = _traductor.Traducir("Resenas_Form_Boton_Enviar");

            // Traducción del panel de agradecimiento
            litGraciasTitle.Text = _traductor.Traducir("Resenas_Gracias_Titulo");
            litGraciasText.Text = _traductor.Traducir("Resenas_Gracias_Texto");

            // 4. Preparamos el objeto con las traducciones para el JavaScript
            var traduccionesJS = new
            {
                // El locale ayuda al JS a formatear fechas correctamente
                locale = _traductor.IdiomaSeleccionado.Nombre.ToLower() == "english" ? "en-US" : "es-AR",
                por = _traductor.Traducir("Resenas_Repeater_Por"),
                publicadoEl = _traductor.Traducir("Resenas_Repeater_PublicadoEl"),
                noHayResenas = _traductor.Traducir("Resenas_NoHayResenas_Texto"),
                errorCarga = _traductor.Traducir("Resenas_Msg_ErrorCarga"),
                errorPuntuacion = _traductor.Traducir("Resenas_Form_Error_Puntuacion")

            };

            // 5. Serializamos el objeto a JSON para inyectarlo en el script del .aspx
            _jsonTraducciones = new JavaScriptSerializer().Serialize(traduccionesJS);
        }

        // --- Los métodos que ya no se necesitan se eliminan ---
        // Se elimina BindResenas()
        // Se elimina RptResenas_ItemDataBound()
        // Se elimina RenderStars() (ahora se hace en JS)

        // --- Los métodos que siguen siendo necesarios se mantienen ---
        protected void BtnEnviarResena_Click(object sender, EventArgs e)
        {
            if (Page.IsValid && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                int puntuacion = GetPuntuacionSeleccionada();
                if (puntuacion == 0)
                {
                    // Mostramos un error y detenemos la ejecución.
                    // (Necesitaremos un Literal para este mensaje en el .aspx)
                    litFormularioError.Text = $"<div class='alert alert-warning'>{_traductor.Traducir("Resenas_Form_Error_Puntuacion")}</div>";
                    return;
                }
                BLLUsuario bllUsuario = new BLLUsuario();
                BEUsuario usuarioActual = bllUsuario.ListarObjeto(new BEUsuario { Email = HttpContext.Current.User.Identity.Name });

                if (usuarioActual != null)
                {

                    var resena = new BEResena
                    {
                        Autor = { Codigo = usuarioActual.Codigo },
                        Asunto = txtAsunto.Text.Trim(),
                        Comentario = txtComentario.Text.Trim(),
                        Puntuacion = GetPuntuacionSeleccionada()
                    };

                    BLLResena bllResena = new BLLResena();
                    bllResena.Guardar(resena);

                    pnlNuevaResena.Visible = false;
                    pnlGracias.Visible = true;
                }
            }
        }

        private int GetPuntuacionSeleccionada()
        {
            if (star5.Checked) return 5;
            if (star4.Checked) return 4;
            if (star3.Checked) return 3;
            if (star2.Checked) return 2;
            if (star1.Checked) return 1;
            return 0;
        }
    }
}