using BE;
using DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace MPP
{
    public class MPPBusqueda
    {
        private readonly Acceso oAcceso;
        public MPPBusqueda() { oAcceso = new Acceso(); }

        public List<BEResultadoBusqueda> Buscar(string termino, bool esPublica, int idIdioma)
        {
            var ht = new Hashtable();
            ht.Add("@TerminoBusqueda", termino);
            ht.Add("@EsBusquedaPublica", esPublica);
            ht.Add("@ID_Idioma", idIdioma);

            DataTable dt = oAcceso.LeerSP("sp_BuscarEnPlataforma", ht);
            var resultados = new List<BEResultadoBusqueda>();

            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    resultados.Add(new BEResultadoBusqueda
                    {
                        Tipo = row["Tipo"].ToString(),
                        Titulo = row["Titulo"].ToString(),
                        URL = row["URL"].ToString(),
                        Descripcion = row["Descripcion"].ToString()
                    });
                }
            }
            return resultados;
        }
    }
}
