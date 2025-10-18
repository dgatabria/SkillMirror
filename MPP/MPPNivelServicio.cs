using BE;
using DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace MPP
{
    public class MPPNivelServicio
    {
        private readonly Acceso oAcceso;
        public MPPNivelServicio() { oAcceso = new Acceso(); }


        public BENivelServicio ObtenerPorId(int id)
        {
            var ht = new Hashtable();
            ht.Add("@ID", id);
            DataTable dt = oAcceso.LeerSP("sp_ObtenerNivelServicioPorID", ht);

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                return new BENivelServicio
                {
                    Codigo = Convert.ToInt32(dr["ID"]),
                    Nombre = dr["Nombre"].ToString(),
                    Gratis = Convert.ToBoolean(dr["Gratis"]),
                    CostoMensual = Convert.ToDouble(dr["CostoMensual"])
                };
            }
            return null;
        }
        public List<BENivelServicio> Listar()
        {
            var lista = new List<BENivelServicio>();
            DataTable dt = oAcceso.LeerSP("sp_ListarNivelesServicio", null);
            foreach (DataRow dr in dt.Rows)
            {
                lista.Add(new BENivelServicio
                {
                    Codigo = Convert.ToInt32(dr["ID"]),
                    Nombre = dr["Nombre"].ToString(),
                    Gratis = Convert.ToBoolean(dr["Gratis"]),
                    CostoMensual = Convert.ToDouble(dr["CostoMensual"])
                });
            }
            return lista;
        }
    }
}