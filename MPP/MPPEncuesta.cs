using BE;
using DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MPP
{
    public class MPPEncuesta
    {
        private readonly Acceso oAcceso;
        public MPPEncuesta() { oAcceso = new Acceso(); }

        public int Guardar(BEEncuesta encuesta)
        {
            var ht = new Hashtable();
            ht.Add("@ID", encuesta.Codigo);
            ht.Add("@Titulo", encuesta.Titulo);
            ht.Add("@Descripcion", encuesta.Descripcion);
            ht.Add("@FechaVencimiento", encuesta.FechaVencimiento.HasValue ? (object)encuesta.FechaVencimiento.Value : DBNull.Value);
            ht.Add("@Activa", encuesta.Activa);

            // Crear DataTables para los TVPs
            DataTable dtPreguntas = new DataTable();
            dtPreguntas.Columns.Add("PreguntaID_Cliente", typeof(int));
            dtPreguntas.Columns.Add("TextoPregunta", typeof(string));
            dtPreguntas.Columns.Add("TipoPregunta", typeof(string));
            dtPreguntas.Columns.Add("Orden", typeof(int));

            DataTable dtOpciones = new DataTable();
            dtOpciones.Columns.Add("PreguntaID_Cliente", typeof(int));
            dtOpciones.Columns.Add("TextoOpcion", typeof(string));
            dtOpciones.Columns.Add("Orden", typeof(int));

            int preguntaClienteId = 1;
            foreach (var pregunta in encuesta.Preguntas)
            {
                dtPreguntas.Rows.Add(preguntaClienteId, pregunta.TextoPregunta, pregunta.TipoPregunta, encuesta.Preguntas.IndexOf(pregunta));
                if (pregunta.TipoPregunta == "OPCION_MULTIPLE")
                {
                    foreach (var opcion in pregunta.Opciones)
                    {
                        dtOpciones.Rows.Add(preguntaClienteId, opcion.TextoOpcion, pregunta.Opciones.IndexOf(opcion));
                    }
                }
                preguntaClienteId++;
            }

            // Asumiendo que tienes un método EscribirSPConTVPs en tu clase Acceso
            DataTable dtResult = oAcceso.EscribirSPConTVPs("sp_GuardarEncuesta", ht, dtPreguntas, dtOpciones);
            return Convert.ToInt32(dtResult.Rows[0]["ID"]);
        }

        public BEEncuesta ListarObjeto(BEEncuesta encuesta)
        {
            var ht = new Hashtable();
            ht.Add("@ID_Encuesta", encuesta.Codigo);

            // Asumimos que tienes un método LeerSPDS que devuelve un DataSet

            DataSet ds = oAcceso.LeerSPDS("sp_ListarEncuestaCompletaPorID", ht);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                
                DataRow drEncuesta = ds.Tables[0].Rows[0];

                
                encuesta.Titulo = drEncuesta["Titulo"].ToString();
                encuesta.Descripcion = drEncuesta["Descripcion"].ToString();
                encuesta.FechaCreacion = Convert.ToDateTime(drEncuesta["FechaCreacion"]);
                // Hacemos una validación para la fecha de vencimiento, que puede ser nula
                if (drEncuesta["FechaVencimiento"] != DBNull.Value)
                {
                    encuesta.FechaVencimiento = Convert.ToDateTime(drEncuesta["FechaVencimiento"]);
                }
                encuesta.Activa = Convert.ToBoolean(drEncuesta["Activa"]);
                

                // La segunda tabla (ds.Tables[1]) tiene las preguntas y sus opciones
                if (ds.Tables.Count > 1)
                {
                    foreach (DataRow drPregunta in ds.Tables[1].Rows)
                    {
                        int idPregunta = Convert.ToInt32(drPregunta["PreguntaID"]);
                        // Buscamos si ya agregamos esta pregunta para no duplicarla
                        BEPregunta pregunta = encuesta.Preguntas.FirstOrDefault(p => p.Codigo == idPregunta);

                        if (pregunta == null)
                        {
                            pregunta = new BEPregunta
                            {
                                Codigo = idPregunta,
                                TextoPregunta = drPregunta["TextoPregunta"].ToString(),
                                TipoPregunta = drPregunta["TipoPregunta"].ToString()
                            };
                            encuesta.Preguntas.Add(pregunta);
                        }

                        // Si la pregunta es de opción múltiple, agregamos la opción
                        if (drPregunta["OpcionID"] != DBNull.Value)
                        {
                            pregunta.Opciones.Add(new BEOpcionRespuesta
                            {
                                Codigo = Convert.ToInt32(drPregunta["OpcionID"]),
                                TextoOpcion = drPregunta["TextoOpcion"].ToString()
                            });
                        }
                    }
                }
                return encuesta;
            }
            return null;
        }

        public List<BEEncuesta> ListarAdmin()
        {
            var lista = new List<BEEncuesta>();
            DataTable dt = oAcceso.LeerSP("sp_ListarEncuestasAdmin", null);
            foreach (DataRow dr in dt.Rows)
            {
                lista.Add(new BEEncuesta
                {
                    Codigo = Convert.ToInt32(dr["ID"]),
                    Titulo = dr["Titulo"].ToString(),
                    FechaCreacion = Convert.ToDateTime(dr["FechaCreacion"]),
                    FechaVencimiento = dr["FechaVencimiento"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(dr["FechaVencimiento"]) : null,
                    Activa = Convert.ToBoolean(dr["Activa"])
                });
            }
            return lista;
        }

        public bool Baja(BEEncuesta encuesta)
        {
            var ht = new Hashtable();
            ht.Add("@ID_Encuesta", encuesta.Codigo);
            return oAcceso.EscribirSP("sp_BajaEncuesta", ht) > 0;
        }

        public BEResultadoEncuesta ObtenerResultados(int idEncuesta)
        {
            var ht = new Hashtable();
            ht.Add("@ID_Encuesta", idEncuesta);
            DataSet ds = oAcceso.LeerSPDS("sp_ObtenerResultadosEncuesta", ht);

            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                return null;

            var resultadoEncuesta = new BEResultadoEncuesta();

            // Procesar Tabla 1: Info General
            DataRow drInfo = ds.Tables[0].Rows[0];
            resultadoEncuesta.TituloEncuesta = drInfo["Titulo"].ToString();
            resultadoEncuesta.TotalRespuestas = Convert.ToInt32(drInfo["TotalRespuestas"]);

            // Procesar Tabla 2: Resultados Agrupados
            if (ds.Tables.Count > 1)
            {
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    int preguntaId = Convert.ToInt32(dr["PreguntaID"]);
                    BEResultadoPregunta resPregunta = resultadoEncuesta.ResultadosPorPregunta.FirstOrDefault(p => p.PreguntaID == preguntaId);

                    if (resPregunta == null)
                    {
                        resPregunta = new BEResultadoPregunta
                        {
                            PreguntaID = preguntaId,
                            TextoPregunta = dr["TextoPregunta"].ToString(),
                            TipoPregunta = dr["TipoPregunta"].ToString()
                        };
                        resultadoEncuesta.ResultadosPorPregunta.Add(resPregunta);
                    }
                    resPregunta.ConteoOpciones.Add(dr["OpcionTexto"].ToString(), Convert.ToInt32(dr["Cantidad"]));
                }
            }

            // Procesar Tabla 3: Respuestas de Texto Libre
            if (ds.Tables.Count > 2)
            {
                foreach (DataRow dr in ds.Tables[2].Rows)
                {
                    int preguntaId = Convert.ToInt32(dr["PreguntaID"]);
                    BEResultadoPregunta resPregunta = resultadoEncuesta.ResultadosPorPregunta.FirstOrDefault(p => p.PreguntaID == preguntaId);

                    if (resPregunta == null)
                    {
                        resPregunta = new BEResultadoPregunta
                        {
                            PreguntaID = preguntaId,
                            TextoPregunta = dr["TextoPregunta"].ToString(),
                            TipoPregunta = "TEXTO_LIBRE"
                        };
                        resultadoEncuesta.ResultadosPorPregunta.Add(resPregunta);
                    }
                    resPregunta.RespuestasAbiertas.Add(dr["RespuestaTexto"].ToString());
                }
            }

            return resultadoEncuesta;
        }
        public BEEncuestaInvitacion ObtenerEncuestaPorToken(Guid token)
        {
            var ht = new Hashtable();
            ht.Add("@Token", token);
            DataSet ds = oAcceso.LeerSPDS("sp_ObtenerEncuestaPorToken", ht);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                var invitacion = new BEEncuestaInvitacion();
                DataRow drInv = ds.Tables[0].Rows[0];
                invitacion.Token = token; 
                invitacion.Codigo = Convert.ToInt32(drInv["InvitacionID"]);
                invitacion.Respondida = Convert.ToBoolean(drInv["Respondida"]);
                invitacion.Usuario.Codigo = Convert.ToInt32(drInv["ID_Usuario"]);
                invitacion.Encuesta.Codigo = Convert.ToInt32(drInv["EncuestaID"]);
                invitacion.Encuesta.Titulo = drInv["Titulo"].ToString();
                invitacion.Encuesta.Descripcion = drInv["Descripcion"].ToString();
                if (drInv["FechaVencimiento"] != DBNull.Value)
                    invitacion.Encuesta.FechaVencimiento = Convert.ToDateTime(drInv["FechaVencimiento"]);

                if (ds.Tables.Count > 1)
                {
                    foreach (DataRow drPregunta in ds.Tables[1].Rows)
                    {
                        int idPregunta = Convert.ToInt32(drPregunta["PreguntaID"]);
                        BEPregunta pregunta = invitacion.Encuesta.Preguntas.FirstOrDefault(p => p.Codigo == idPregunta);
                        if (pregunta == null)
                        {
                            pregunta = new BEPregunta { Codigo = idPregunta, TextoPregunta = drPregunta["TextoPregunta"].ToString(), TipoPregunta = drPregunta["TipoPregunta"].ToString() };
                            invitacion.Encuesta.Preguntas.Add(pregunta);
                        }
                        if (drPregunta["OpcionID"] != DBNull.Value)
                        {
                            pregunta.Opciones.Add(new BEOpcionRespuesta { Codigo = Convert.ToInt32(drPregunta["OpcionID"]), TextoOpcion = drPregunta["TextoOpcion"].ToString() });
                        }
                    }
                }
                return invitacion;
            }
            return null;
        }

        // Reemplaza tu método GuardarRespuesta por este
        public bool GuardarRespuestas(List<BERespuesta> respuestas)
        {
            if (respuestas == null || respuestas.Count == 0) return true;

            var ht = new Hashtable();
            ht.Add("@ID_Invitacion", respuestas[0].Invitacion.Codigo);

            DataTable dtRespuestas = new DataTable();
            dtRespuestas.Columns.Add("ID_Pregunta", typeof(int));
            dtRespuestas.Columns.Add("RespuestaTexto", typeof(string));
            dtRespuestas.Columns.Add("Puntuacion", typeof(int));
            dtRespuestas.Columns.Add("ID_OpcionSeleccionada", typeof(int));

            foreach (var r in respuestas)
            {
                dtRespuestas.Rows.Add(
                    r.Pregunta.Codigo,
                    string.IsNullOrEmpty(r.RespuestaTexto) ? (object)DBNull.Value : r.RespuestaTexto,
                    r.Puntuacion.HasValue ? (object)r.Puntuacion.Value : DBNull.Value,
                    r.OpcionSeleccionada.Codigo > 0 ? (object)r.OpcionSeleccionada.Codigo : DBNull.Value
                );
            }

            // Necesitarás un método en Acceso.cs para manejar un SP con un solo TVP y parámetros
            return oAcceso.EscribirSPConTVP("sp_GuardarRespuestasEnLote", ht, dtRespuestas, "dbo.RespuestaTabla", "@Respuestas");
        }

        public BEPregunta ObtenerEncuestaParaPortada()
        {
            DataTable dt = oAcceso.LeerSP("sp_ObtenerEncuestaParaPortada", null);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new BEPregunta
                {
                    EncuestaPadre = new BEEncuesta
                    {
                        Codigo = Convert.ToInt32(row["EncuestaID"]),
                        Titulo = row["Titulo"].ToString()
                    },
                    Codigo = Convert.ToInt32(row["PreguntaID"]),
                    TextoPregunta = row["TextoPregunta"].ToString(),
                    Opciones = ObtenerOpcionesDePregunta(Convert.ToInt32(row["PreguntaID"]))
                };
            }
            return null;
        }

        public List<BEOpcionRespuesta> ObtenerOpcionesDePregunta(int idPregunta)
        {
            var ht = new Hashtable();
            ht.Add("@ID_Pregunta", idPregunta);
            DataTable dt = oAcceso.LeerSP("sp_ObtenerOpcionesDePregunta", ht);
            var opciones = new List<BEOpcionRespuesta>();
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    opciones.Add(new BEOpcionRespuesta
                    {
                        Codigo = Convert.ToInt32(row["ID"]),
                        TextoOpcion = row["TextoOpcion"].ToString()
                    });
                }
            }
            return opciones;
        }

        public bool UsuarioYaRespondio(int idEncuesta, int idUsuario)
        {
            var ht = new Hashtable();
            ht.Add("@ID_Encuesta", idEncuesta);
            ht.Add("@ID_Usuario", idUsuario);
            // LeerSP devuelve una tabla. Si tiene filas y el valor es 1, ya respondió.
            DataTable dt = oAcceso.LeerSP("sp_UsuarioYaRespondioEncuesta", ht);
            return dt != null && dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0][0]) == 1;
        }

        public void RegistrarRespuestaPortada(int idEncuesta, int idUsuario, int idPregunta, int idOpcion)
        {
            var ht = new Hashtable();
            ht.Add("@ID_Encuesta", idEncuesta);
            ht.Add("@ID_Usuario", idUsuario);
            ht.Add("@ID_Pregunta", idPregunta);
            ht.Add("@ID_OpcionSeleccionada", idOpcion);
            oAcceso.EscribirSP("sp_RegistrarRespuestaPortada", ht);
        }
    
        public bool GuardarRespuesta(BERespuesta respuesta)
        {
            var ht = new Hashtable();
            ht.Add("@ID_Invitacion", respuesta.Invitacion.Codigo);
            ht.Add("@ID_Pregunta", respuesta.Pregunta.Codigo);
            ht.Add("@RespuestaTexto", !string.IsNullOrEmpty(respuesta.RespuestaTexto) ? (object)respuesta.RespuestaTexto : DBNull.Value);
            ht.Add("@Puntuacion", respuesta.Puntuacion.HasValue ? (object)respuesta.Puntuacion.Value : DBNull.Value);
            ht.Add("@ID_OpcionSeleccionada", respuesta.OpcionSeleccionada.Codigo > 0 ? (object)respuesta.OpcionSeleccionada.Codigo : DBNull.Value);

            return oAcceso.EscribirSP("sp_GuardarRespuesta", ht) > 0;
        }

        public bool FinalizarEncuesta(Guid token)
        {
            var ht = new Hashtable();
            ht.Add("@Token", token);
            return oAcceso.EscribirSP("sp_FinalizarEncuesta", ht) > 0;
        }
        public bool GuardarInvitacion(int idEncuesta, int idUsuario, Guid token)
        {
            var ht = new Hashtable();
            ht.Add("@ID_Encuesta", idEncuesta);
            ht.Add("@ID_Usuario", idUsuario);
            ht.Add("@Token", token);

            return oAcceso.EscribirSP("sp_GuardarInvitacionEncuesta", ht) > 0;
        }
       
    }

}