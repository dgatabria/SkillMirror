using BE;
using MPP;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class BLLMensajeria
    {
        private readonly MPPMensajeria oMPPMensajeria;

        public BLLMensajeria()
        {
            oMPPMensajeria = new MPPMensajeria();
        }

        /// <summary>
        /// Valida y crea una nueva conversación.
        /// </summary>
        /// <returns>El ID de la nueva conversación o -1 si falla.</returns>
        public int CrearConversacion(BEConversacion conversacion)
        {
            // Regla de negocio: El asunto no puede estar vacío.
            if (string.IsNullOrWhiteSpace(conversacion.Asunto))
            {
                throw new ArgumentException("El asunto no puede estar vacío.");
            }
            // Regla de negocio: Debe haber al menos un mensaje inicial.
            if (conversacion.Mensajes == null || conversacion.Mensajes.Count == 0 || string.IsNullOrWhiteSpace(conversacion.Mensajes[0].CuerpoMensaje))
            {
                throw new ArgumentException("El primer mensaje no puede estar vacío.");
            }
            // Regla de negocio: El usuario que inicia debe ser válido.
            if (conversacion.Usuario == null || conversacion.Usuario.Codigo <= 0)
            {
                throw new ArgumentException("El usuario remitente no es válido.");
            }

            return oMPPMensajeria.CrearConversacion(conversacion);
        }

        /// <summary>
        /// Valida y envía un nuevo mensaje a una conversación existente.
        /// </summary>
        public bool EnviarMensaje(BEMensaje mensaje)
        {
            // Regla de negocio: El cuerpo del mensaje no puede estar vacío.
            if (string.IsNullOrWhiteSpace(mensaje.CuerpoMensaje))
            {
                throw new ArgumentException("El cuerpo del mensaje no puede estar vacío.");
            }
            // Regla de negocio: El remitente debe ser válido.
            if (mensaje.Remitente == null || mensaje.Remitente.Codigo <= 0)
            {
                throw new ArgumentException("El remitente del mensaje no es válido.");
            }

            return oMPPMensajeria.EnviarMensaje(mensaje);
        }

        /// <summary>
        /// Obtiene la lista de todas las conversaciones para la vista del agente.
        /// </summary>
        public List<BEConversacion> ListarConversacionesParaAgente()
        {
            // En este caso, no hay reglas de negocio complejas,
            // así que llamamos directamente a la capa de persistencia.
            return oMPPMensajeria.ListarConversacionesParaAgente();
        }

        /// <summary>
        /// Obtiene todos los mensajes de una conversación y los marca como leídos.
        /// </summary>
        public List<BEMensaje> ListarMensajesPorConversacion(int conversacionId, int lectorId)
        {
            // Regla de negocio: Los IDs deben ser válidos.
            if (conversacionId <= 0 || lectorId <= 0)
            {
                throw new ArgumentException("Los identificadores de conversación y lector no son válidos.");
            }

            return oMPPMensajeria.ListarMensajesPorConversacion(conversacionId, lectorId);
        }
    }
}