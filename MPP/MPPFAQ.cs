using BE;
using DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace MPP
{
    public class MPPFAQ
    {
        private readonly Acceso oAcceso;
        public MPPFAQ() { oAcceso = new Acceso(); }

        public int Guardar(BEFAQ faq)
        {
            var ht = new Hashtable();
            ht.Add("@ID", faq.Codigo);
            ht.Add("@Orden", faq.Orden);

            // 1. Preparamos la tabla para las traducciones de Preguntas
            DataTable dtPreguntas = new DataTable();
            dtPreguntas.Columns.Add("ID_Idioma", typeof(int));
            dtPreguntas.Columns.Add("Texto", typeof(string));
            foreach (var traduccion in faq.TraduccionesPregunta)
            {
                dtPreguntas.Rows.Add(traduccion.Key, traduccion.Value);
            }

            // 2. Preparamos la tabla para las traducciones de Respuestas
            DataTable dtRespuestas = new DataTable();
            dtRespuestas.Columns.Add("ID_Idioma", typeof(int));
            dtRespuestas.Columns.Add("Texto", typeof(string));
            foreach (var traduccion in faq.TraduccionesRespuesta)
            {
                dtRespuestas.Rows.Add(traduccion.Key, traduccion.Value);
            }

            // 3. Llamamos al nuevo método en la DAL que acepta dos TVPs
            DataTable dtResult = oAcceso.EscribirSPConDosTVP(
                "sp_GuardarFAQCompleta", ht,
                dtPreguntas, "dbo.TraduccionFAQTabla", "@TraduccionesPregunta",
                dtRespuestas, "dbo.TraduccionFAQTabla", "@TraduccionesRespuesta"
            );

            return Convert.ToInt32(dtResult.Rows[0]["ID"]);
        }

        public bool Baja(BEFAQ faq)
        {
            var ht = new Hashtable();
            ht.Add("@ID", faq.Codigo);
            return oAcceso.EscribirSP("sp_BajaFAQ", ht) > 0;
        }

        public List<BEFAQ> ListarAdmin(int idIdioma)
        {
            var lista = new List<BEFAQ>();
            var ht = new Hashtable();
            ht.Add("@ID_Idioma", idIdioma);
            DataTable dt = oAcceso.LeerSP("sp_ListarFAQsAdmin", ht);

            foreach (DataRow dr in dt.Rows)
            {
                lista.Add(new BEFAQ
                {
                    Codigo = Convert.ToInt32(dr["ID"]),
                    Pregunta = dr["Pregunta"].ToString(),
                    Orden = Convert.ToInt32(dr["Orden"]),
                    Activo = Convert.ToBoolean(dr["Activo"])
                });
            }
            return lista;
        }

        public List<BEFAQ> ListarPublicas(int idIdioma)
        {
            var lista = new List<BEFAQ>();
            var ht = new Hashtable();
            ht.Add("@ID_Idioma", idIdioma);
            DataTable dt = oAcceso.LeerSP("sp_ListarFAQsPublicas", ht);

            foreach (DataRow dr in dt.Rows)
            {
                lista.Add(new BEFAQ
                {
                    Codigo = Convert.ToInt32(dr["ID"]),
                    Pregunta = dr["Pregunta"].ToString(),
                    Respuesta = dr["Respuesta"].ToString()
                });
            }
            return lista;
        }

        public BEFAQ ListarObjetoConTraducciones(BEFAQ faq)
        {
            var ht = new Hashtable();
            ht.Add("@ID", faq.Codigo);
            DataSet ds = oAcceso.LeerSPDS("sp_ListarFAQConTraduccionesPorID", ht);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                faq.PreguntaTag = dr["PreguntaTag"].ToString();
                faq.RespuestaTag = dr["RespuestaTag"].ToString();
                faq.Orden = Convert.ToInt32(dr["Orden"]);

                // Cargar traducciones de la pregunta
                foreach (DataRow drPregunta in ds.Tables[1].Rows)
                {
                    faq.TraduccionesPregunta[Convert.ToInt32(drPregunta["ID_IDIOMA"])] = drPregunta["Texto"].ToString();
                }

                // Cargar traducciones de la respuesta
                foreach (DataRow drRespuesta in ds.Tables[2].Rows)
                {
                    faq.TraduccionesRespuesta[Convert.ToInt32(drRespuesta["ID_IDIOMA"])] = drRespuesta["Texto"].ToString();
                }

                return faq;
            }
            return null;
        }
    }
}