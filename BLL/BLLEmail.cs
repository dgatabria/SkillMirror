using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public static class BLLEmail
    {

        public static string ObtenerCuerpoMail(string titulo, string texto1, string texto2, string textoBoton, string urlBoton)
        {
            return $@"
                <html>
                <body style='font-family: Arial, sans-serif;'>
                    <h1>{titulo}</h1>
                    <p>{texto1}</p>
                    <p>{texto2}</p>
                    <p style='margin: 20px 0;'>
                        <a href='{urlBoton}' style='background-color: #008080; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px;'>
                            {textoBoton}
                        </a>
                    </p>
                    <p>Si el botón no funciona, copia y pega esta URL en tu navegador:</p>
                    <p>{urlBoton}</p>
                    <br>
                    <p>Atentamente,<br>El equipo de SkillMirror</p>
                </body>
                </html>";
        }
    
        public static bool EnviarMail(string toAddress, string subject, string body)
        {
            try
            {
                // Leer la configuración desde Web.config
                string smtpHost = ConfigurationManager.AppSettings["SmtpHost"];
                int smtpPort = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"]);
                string smtpUser = ConfigurationManager.AppSettings["SmtpUser"];
                string smtpPass = ConfigurationManager.AppSettings["SmtpPass"];

                // Crear el mensaje de correo
                var mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(smtpUser, "SkillMirror");
                mailMessage.To.Add(toAddress);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true; // Permite usar HTML en el cuerpo del correo

                // Configurar el cliente SMTP
                var smtpClient = new SmtpClient(smtpHost, smtpPort);
                smtpClient.EnableSsl = true; // Gmail requiere SSL
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(smtpUser, smtpPass);

                // Enviar el correo
                smtpClient.Send(mailMessage);

                return true; // Envío exitoso
            }
            catch (Exception ex)
            {
                // Aquí deberías registrar el error 'ex' en tu bitácora para diagnosticar problemas
                Console.WriteLine(ex.ToString()); // O usa tu sistema de logging
                return false; // Error en el envío
            }
        }
    }
}
