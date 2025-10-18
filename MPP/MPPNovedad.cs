using BE;
using DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPP
{
    public class MPPNovedad
    {
        private readonly Acceso oAcceso;
        public MPPNovedad() { oAcceso = new Acceso(); }

        public int Guardar(BENovedad novedad)
        {
            var ht = new Hashtable();
            ht.Add("@ID", novedad.Codigo);
            ht.Add("@Titulo", novedad.Titulo);
            ht.Add("@Subtitulo", novedad.Subtitulo);
            ht.Add("@Cuerpo", novedad.Cuerpo);
            ht.Add("@ID_Autor", novedad.Autor.Codigo);

            // Este SP devuelve una tabla con el ID, así que usamos LeerSP
            DataTable dt = oAcceso.LeerSP("sp_GuardarNovedad", ht);
            return Convert.ToInt32(dt.Rows[0]["ID"]);
        }

        public bool Baja(BENovedad novedad)
        {
            var ht = new Hashtable();
            ht.Add("@ID", novedad.Codigo);
            return oAcceso.EscribirSP("sp_BajaNovedad", ht) > 0;
        }

        public List<BENovedad> ListarPublicadas()
        {
            var lista = new List<BENovedad>();
            DataTable dt = oAcceso.LeerSP("sp_ListarNovedadesPublicadas", null);

            foreach (DataRow dr in dt.Rows)
            {
                var novedad = new BENovedad
                {
                    Codigo = Convert.ToInt32(dr["ID"]),
                    Titulo = dr["Titulo"].ToString(),
                    Subtitulo = dr["Subtitulo"].ToString(),
                    Cuerpo = dr["Cuerpo"].ToString(),
                    FechaPublicacion = Convert.ToDateTime(dr["FechaPublicacion"]),
                    Autor = new BEUsuario
                    {
                        Nombre = dr["NombreAutor"].ToString(),
                        Apellido = dr["ApellidoAutor"].ToString()
                    }
                };
                lista.Add(novedad);
            }
            return lista;
        }

        public List<BENovedad> ListarTodas()
        {
            var lista = new List<BENovedad>();
            // Usamos el SP que ya habíamos creado
            DataTable dt = oAcceso.LeerSP("sp_ListarTodasNovedades", null);

            foreach (DataRow dr in dt.Rows)
            {
                var novedad = new BENovedad
                {
                    Codigo = Convert.ToInt32(dr["ID"]),
                    Titulo = dr["Titulo"].ToString(),
                    FechaPublicacion = Convert.ToDateTime(dr["FechaPublicacion"]),
                    Activo = Convert.ToBoolean(dr["Activo"]),
                    Autor = new BEUsuario { Nombre = dr["AutorCompleto"].ToString() }
                };
                lista.Add(novedad);
            }
            return lista;
        }

        public BENovedad ListarObjeto(BENovedad novedad)
        {
            var ht = new Hashtable();
            ht.Add("@ID", novedad.Codigo);
            DataTable dt = oAcceso.LeerSP("sp_ListarNovedadPorID", ht);

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                novedad.Titulo = dr["Titulo"].ToString();
                novedad.Subtitulo = dr["Subtitulo"].ToString();
                novedad.Cuerpo = dr["Cuerpo"].ToString();
                novedad.FechaPublicacion = Convert.ToDateTime(dr["FechaPublicacion"]);
                novedad.Activo = Convert.ToBoolean(dr["Activo"]);
                novedad.Autor.Codigo = Convert.ToInt32(dr["ID_Autor"]);
                novedad.Autor.Nombre = dr["NombreAutor"].ToString();
                novedad.Autor.Apellido = dr["ApellidoAutor"].ToString();
                return novedad;
            }
            return null;
        }
    }
}
