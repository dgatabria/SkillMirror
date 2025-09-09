using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Newtonsoft.Json; 


namespace BLL
{
    public class BLLReCaptcha
    {
        public class RecaptchaResponse
        {
            [JsonProperty("success")]
            public bool Success { get; set; }
        }

        public bool Verificar(string recaptchaResponseToken) {
            try
            {
                // 1. Obtiene tu clave secreta desde Web.config
                string secretKey = ConfigurationManager.AppSettings["RecaptchaSecretKey"];

                // 2. Prepara la petición a la API de Google
                var client = new WebClient();
                string googleReply = client.DownloadString(
                    $"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={recaptchaResponseToken}"
                );

                // 3. Interpreta la respuesta JSON
                var response = JsonConvert.DeserializeObject<RecaptchaResponse>(googleReply);

                // 4. Devuelve el resultado (true o false)
                return response.Success;
            }
            catch (Exception ex)
            {
                // En caso de error (ej. sin conexión), asumimos que la validación falla
                // Aquí podrías agregar un log del error 'ex'
                return false;
            }
        }

    }
    
}
