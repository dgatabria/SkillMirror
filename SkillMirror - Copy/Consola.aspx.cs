using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLL; // Necesario para la BasePage

namespace SkillMirror
{
    public partial class Consola : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // La suscripción al traductor ya se maneja en la BasePage
        }

        public override void ActualizarTraducciones()
        {
            // Traducimos todos los controles de la página
            this.Title = _traductor.Traducir("Consola_Page_Title");
            headerTitle.InnerText = _traductor.Traducir("Consola_Header_Titulo");

            welcomeTitle.InnerText = _traductor.Traducir("Consola_Bienvenida_Titulo");
            welcomeSubtitle.InnerText = _traductor.Traducir("Consola_Bienvenida_Subtitulo");

            // Card Usuarios
            cardUsuariosTitle.InnerText = _traductor.Traducir("Consola_CardUsuarios_Titulo");
            cardUsuariosText.InnerText = _traductor.Traducir("Consola_CardUsuarios_Texto");
            btnIrUsuarios.Text = _traductor.Traducir("Consola_CardUsuarios_Boton");

            // Card Bitácora
            cardBitacoraTitle.InnerText = _traductor.Traducir("Consola_CardBitacora_Titulo");
            cardBitacoraText.InnerText = _traductor.Traducir("Consola_CardBitacora_Texto");
            btnVerBitacora.Text = _traductor.Traducir("Consola_CardBitacora_Boton");

            // Card Backup
            cardBackupTitle.InnerText = _traductor.Traducir("Consola_CardBackup_Titulo");
            cardBackupText.InnerText = _traductor.Traducir("Consola_CardBackup_Texto");
            btnIrBackup.Text = _traductor.Traducir("Consola_CardBackup_Boton");
        }
    }
}