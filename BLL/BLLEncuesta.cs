using System;
using System.Collections.Generic;
using BE;
using MPP;

namespace BLL
{
    public class BLLEncuesta
    {
        private readonly MPPEncuesta oMPPEncuesta;
        public BLLEncuesta() { oMPPEncuesta = new MPPEncuesta(); }

        public bool GuardarRespuesta(BERespuesta respuesta)
        {
            return oMPPEncuesta.GuardarRespuesta(respuesta);
        }

        public bool GuardarRespuestas(List<BERespuesta> respuestas)
        {
            return oMPPEncuesta.GuardarRespuestas(respuestas);
        }

        public BEEncuestaInvitacion ObtenerEncuestaPorToken(Guid token)
        {
            return oMPPEncuesta.ObtenerEncuestaPorToken(token);
        }
        public bool FinalizarEncuesta(Guid token)
        {
            return oMPPEncuesta.FinalizarEncuesta(token);
        }

        public bool GuardarInvitacion(int idEncuesta, int idUsuario, Guid token)
        {
            return oMPPEncuesta.GuardarInvitacion(idEncuesta, idUsuario, token);
        }

        public BEResultadoEncuesta ObtenerResultados(int idEncuesta)
        {
            return oMPPEncuesta.ObtenerResultados(idEncuesta);
        }

        public void EnviarEncuesta(BEEncuesta encuesta, string baseUrl) 
        {
            BLLUsuario bllUsuario = new BLLUsuario();
            List<BEUsuario> suscriptos = bllUsuario.ListarSuscritosEncuestas();

            foreach (var usuario in suscriptos)
            {
                Guid token = Guid.NewGuid();
                this.GuardarInvitacion(encuesta.Codigo, usuario.Codigo, token);

                string urlEncuesta = $"{baseUrl}ResponderEncuesta.aspx?token={token}";

                string asunto = $"Te invitamos a participar de nuestra encuesta: {encuesta.Titulo}";
                string cuerpo = BLLEmail.ObtenerCuerpoMail(
                    $"Hola {usuario.Nombre},",
                    $"Nos gustaría conocer tu opinión. Por favor, completa la siguiente encuesta antes del {encuesta.FechaVencimiento:dd/MM/yyyy}.",
                    "Tu participación es muy importante para nosotros.",
                    "Comenzar Encuesta",
                    urlEncuesta);

                BLLEmail.EnviarMail(usuario.Email, asunto, cuerpo);
            }
        }

        public int Guardar(BEEncuesta encuesta)
        {
            return oMPPEncuesta.Guardar(encuesta);
        }

        public BEEncuesta ListarObjeto(BEEncuesta encuesta)
        {
            return oMPPEncuesta.ListarObjeto(encuesta);
        }

        public List<BEEncuesta> ListarAdmin()
        {
            return oMPPEncuesta.ListarAdmin();
        }

        public bool Baja(BEEncuesta encuesta)
        {
            return oMPPEncuesta.Baja(encuesta);
        }
        public BEPregunta ObtenerEncuestaParaPortada()
        {
            return oMPPEncuesta.ObtenerEncuestaParaPortada();
        }

        public bool UsuarioYaRespondio(int idEncuesta, int idUsuario)
        {
            return oMPPEncuesta.UsuarioYaRespondio(idEncuesta, idUsuario);
        }

        public void RegistrarRespuestaPortada(int idEncuesta, int idUsuario, int idPregunta, int idOpcion)
        {
            // Podríamos añadir una regla de negocio para evitar dobles votos, aunque el SP ya lo controla indirectamente.
            if (!oMPPEncuesta.UsuarioYaRespondio(idEncuesta, idUsuario))
            {
                oMPPEncuesta.RegistrarRespuestaPortada(idEncuesta, idUsuario, idPregunta, idOpcion);
            }
        }

    }
}