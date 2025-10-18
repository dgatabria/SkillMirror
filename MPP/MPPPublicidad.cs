using BE;
using DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace MPP
{
    public class MPPPublicidad
    {
        private readonly Acceso oAcceso;
        public MPPPublicidad() { oAcceso = new Acceso(); }

        public int Guardar(BEPublicidad pub)
        {
            var ht = new Hashtable();
            ht.Add("@ID", pub.Codigo);
            ht.Add("@Titulo", pub.Titulo);
            ht.Add("@URLDestino", pub.URLDestino);
            ht.Add("@RutaImagen", pub.RutaImagen);
            ht.Add("@FechaInicio", pub.FechaInicio);
            ht.Add("@FechaExpiracion", pub.FechaExpiracion);
            ht.Add("@Activo", pub.Activo);

            DataTable dt = oAcceso.LeerSP("sp_GuardarPublicidad", ht);
            return Convert.ToInt32(dt.Rows[0]["ID"]);
        }

        public bool Eliminar(BEPublicidad pub)
        {
            var ht = new Hashtable();
            ht.Add("@ID", pub.Codigo);
            return oAcceso.EscribirSP("sp_EliminarPublicidad", ht) > 0;
        }

        public BEPublicidad ObtenerPorId(BEPublicidad pub)
        {
            var ht = new Hashtable();
            ht.Add("@ID", pub.Codigo);
            DataTable dt = oAcceso.LeerSP("sp_ObtenerPublicidadPorID", ht);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                return new BEPublicidad
                {
                    Codigo = Convert.ToInt32(dr["ID"]),
                    Titulo = dr["Titulo"].ToString(),
                    URLDestino = dr["URLDestino"].ToString(),
                    RutaImagen = dr["RutaImagen"].ToString(),
                    FechaInicio = Convert.ToDateTime(dr["FechaInicio"]),
                    FechaExpiracion = Convert.ToDateTime(dr["FechaExpiracion"]),
                    Activo = Convert.ToBoolean(dr["Activo"])
                };
            }
            return null;
        }
        public List<BEPublicidad> ListarAdmin()
        {
            var lista = new List<BEPublicidad>();
            DataTable dt = oAcceso.LeerSP("sp_ListarPublicidadesAdmin", null);
            foreach (DataRow dr in dt.Rows)
            {
                lista.Add(new BEPublicidad
                {
                    Codigo = Convert.ToInt32(dr["ID"]),
                    Titulo = dr["Titulo"].ToString(),
                    RutaImagen = dr["RutaImagen"].ToString(),
                    FechaInicio = Convert.ToDateTime(dr["FechaInicio"]),
                    FechaExpiracion = Convert.ToDateTime(dr["FechaExpiracion"]),
                    Activo = Convert.ToBoolean(dr["Activo"])
                });
            }
            return lista;
        }
        public List<BEPublicidad> ListarActivas()
        {
            var lista = new List<BEPublicidad>();
            DataTable dt = oAcceso.LeerSP("sp_ListarPublicidadesActivas", null);
            foreach (DataRow dr in dt.Rows)
            {
                lista.Add(new BEPublicidad
                {
                    Titulo = dr["Titulo"].ToString(),
                    URLDestino = dr["URLDestino"].ToString(),
                    RutaImagen = dr["RutaImagen"].ToString()
                });
            }
            return lista;
        }
    }
}