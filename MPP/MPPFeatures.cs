using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;

namespace Mapper
{
    public class MPPFeatures
    {
        /// <summary>
        /// Obtiene la lista completa de características desde la base de datos.
        /// </summary>
        public List<BEFeature> Listar()
        {
            Acceso oDatos = new Acceso();
            // Llamamos al nuevo SP que creamos
            DataTable dt = oDatos.LeerSP("sp_ListarFeatures", null);

            var listaFeatures = new List<BEFeature>();

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    var feature = new BEFeature
                    {
                        ID = Convert.ToInt32(row["ID"]),
                        Nombre = row["Nombre"].ToString(),
                        Clave = row["Clave"].ToString(),
                        Orden = Convert.ToInt32(row["Orden"])
                    };
                    listaFeatures.Add(feature);
                }
            }
            return listaFeatures;
        }
    }
}
