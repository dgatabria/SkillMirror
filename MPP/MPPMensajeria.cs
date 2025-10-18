using BE;
using DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace MPP
{
    public class MPPMensajeria
    {
        private readonly Acceso oAcceso;

        public MPPMensajeria()
        {
            oAcceso = new Acceso();
        }

        /// <summary>
        /// Inicia una nueva conversación y guarda el primer mensaje.
        /// </summary>
        /// <returns>El ID de la nueva conversación o -1 si falla.</returns>
        public int CrearConversacion(BEConversacion conversacion)
        {
            try
            {
                var ht = new Hashtable();
                ht.Add("@UsuarioID", conversacion.Usuario.Codigo);
                ht.Add("@Asunto", conversacion.Asunto);
                // El primer mensaje está en la lista de mensajes de la conversación.
                ht.Add("@CuerpoMensaje", conversacion.Mensajes[0].CuerpoMensaje);

                DataTable dt = oAcceso.LeerSP("sp_CrearConversacion", ht);

                if (dt != null && dt.Rows.Count > 0)
                {
                    return Convert.ToInt32(dt.Rows[0]["NuevaConversacionID"]);
                }
                return -1;
            }
            catch (Exception)
            {
                // Aquí deberías registrar el error en tu bitácora.
                return -1;
            }
        }

        /// <summary>
        /// Envía un nuevo mensaje a una conversación existente.
        /// </summary>
        public bool EnviarMensaje(BEMensaje mensaje)
        {
            var ht = new Hashtable();
            ht.Add("@ConversacionID", mensaje.ConversacionID);
            ht.Add("@RemitenteID", mensaje.Remitente.Codigo);
            ht.Add("@CuerpoMensaje", mensaje.CuerpoMensaje);

            return oAcceso.EscribirSP("sp_EnviarMensaje", ht) > 0;
        }

        /// <summary>
        /// Obtiene la lista de todas las conversaciones para la vista del administrador.
        /// </summary>
        public List<BEConversacion> ListarConversacionesParaAgente()
        {
            var lista = new List<BEConversacion>();
            DataTable dt = oAcceso.LeerSP("sp_ListarConversacionesParaAgente", null);

            foreach (DataRow dr in dt.Rows)
            {
                lista.Add(new BEConversacion
                {
                    Codigo = Convert.ToInt32(dr["ID"]),
                    Asunto = dr["Asunto"].ToString(),
                    Estado = dr["Estado"].ToString(),
                    FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"]),

                    Usuario = new BEUsuario
                    {
                        Codigo = Convert.ToInt32(dr["UsuarioID"]), // Asignamos el Codigo del usuario
                        Nombre = dr["NombreUsuario"].ToString()
                    },

                    FechaUltimoMensaje = dr["FechaUltimoMensaje"] != DBNull.Value ? Convert.ToDateTime(dr["FechaUltimoMensaje"]) : DateTime.MinValue,
                    MensajesNuevos = Convert.ToInt32(dr["MensajesNuevos"])
                });
            }
            return lista;
        }

        /// <summary>
        /// Obtiene todos los mensajes de una conversación específica y los marca como leídos.
        /// </summary>
        public List<BEMensaje> ListarMensajesPorConversacion(int conversacionId, int lectorId)
        {
            var lista = new List<BEMensaje>();
            var ht = new Hashtable();
            ht.Add("@ConversacionID", conversacionId);

            DataTable dt = oAcceso.LeerSP("sp_ListarMensajesPorConversacion", ht);

            foreach (DataRow dr in dt.Rows)
            {
                lista.Add(new BEMensaje
                {
                    Codigo = Convert.ToInt32(dr["ID"]),
                    CuerpoMensaje = dr["CuerpoMensaje"].ToString(),
                    FechaEnvio = Convert.ToDateTime(dr["FechaEnvio"]),
                    Leido = Convert.ToBoolean(dr["Leido"]),
                    Remitente = new BEUsuario
                    {
                        Codigo = Convert.ToInt32(dr["RemitenteID"]),
                        Nombre = dr["NombreRemitente"].ToString()
                    }
                });
            }

            // Después de listar los mensajes, los marcamos como leídos por el usuario actual.
            MarcarMensajesComoLeidos(conversacionId, lectorId);

            return lista;
        }

        /// <summary>
        /// Marca como leídos los mensajes de una conversación que no fueron enviados por el lector.
        /// </summary>
        private void MarcarMensajesComoLeidos(int conversacionId, int lectorId)
        {
            var ht = new Hashtable();
            ht.Add("@ConversacionID", conversacionId);
            ht.Add("@LectorID", lectorId);
            oAcceso.EscribirSP("sp_MarcarMensajesComoLeidos", ht);
        }
    }
}