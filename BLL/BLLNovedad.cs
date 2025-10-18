using BE;
using MPP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLNovedad
    {
        private readonly MPPNovedad oMPPNovedad;
        public BLLNovedad() { oMPPNovedad = new MPPNovedad(); }

        public List<BENovedad> ListarPublicadas()
        {
            return oMPPNovedad.ListarPublicadas();
        }

        public bool PublicarNovedadYEnviarNewsletter(BENovedad novedad)
        {
            // 1. Guardamos la novedad en la base de datos
            int novedadId = oMPPNovedad.Guardar(novedad);
            if (novedadId <= 0) return false; // Si no se guardó, detenemos todo

            // 2. Obtenemos la lista de usuarios suscritos
            BLLUsuario bllUsuario = new BLLUsuario();
            List<BEUsuario> suscriptos = bllUsuario.ListarSuscritos();

            // 3. Preparamos el asunto y el cuerpo del email
            string asunto = $"Novedades de SkillMirror: {novedad.Titulo}";

            // Creamos un cuerpo HTML simple para el email
            string cuerpo = $@"
                <html>
                <body style='font-family: Arial, sans-serif; line-height: 1.6;'>
                    <div style='max-width: 600px; margin: auto; padding: 20px; border: 1px solid #ddd;'>
                        <h1>{novedad.Titulo}</h1>
                        <p style='font-style: italic; color: #555;'>{novedad.Subtitulo}</p>
                        <hr>
                        <div>{novedad.Cuerpo}</div>
                        <hr>
                        <p style='font-size: 0.8em; color: #888;'>
                            Recibiste este correo porque estás suscrito a las novedades de SkillMirror.
                        </p>
                    </div>
                </body>
                </html>";

            // 4. Recorremos la lista y enviamos el correo a cada uno
            foreach (BEUsuario suscriptor in suscriptos)
            {
                // Usamos tu clase BLLEmail existente
                BLLEmail.EnviarMail(suscriptor.Email, asunto, cuerpo);
            }

            return true;
        }

        public List<BENovedad> ListarTodas()
        {
            return oMPPNovedad.ListarTodas();
        }

        public BENovedad ListarObjeto(BENovedad novedad)
        {
            return oMPPNovedad.ListarObjeto(novedad);
        }

        public bool Baja(BENovedad novedad)
        {
            return oMPPNovedad.Baja(novedad);
        }

        public int Guardar(BENovedad novedad)
        {
            return oMPPNovedad.Guardar(novedad);
        }
    }
}
