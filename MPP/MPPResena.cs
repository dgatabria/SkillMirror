using System.Collections.Generic;
using System.Data;
using System.Collections;
using System;
using BE;
using DAL;

namespace MPP
{
    public class MPPResena
    {
        private readonly Acceso oAcceso;



        public MPPResena()
        {
            oAcceso = new Acceso();
        }

        public List<BEResena> ListarTodas()
        {
            var lista = new List<BEResena>();
            DataTable dt = oAcceso.LeerSP("sp_ListarTodasResenas", null);

            foreach (DataRow dr in dt.Rows)
            {
                var resena = new BEResena
                {
                    Codigo = Convert.ToInt32(dr["ID"]),
                    Puntuacion = Convert.ToInt32(dr["Puntuacion"]),
                    Asunto = dr["Asunto"].ToString(),
                    Comentario = dr["Comentario"].ToString(),
                    Fecha = Convert.ToDateTime(dr["Fecha"]),
                    Aprobado = Convert.ToBoolean(dr["Aprobado"]),
                    Autor = new BEUsuario
                    {
                        Nombre = dr["NombreAutor"].ToString(),
                        Apellido = dr["ApellidoAutor"].ToString()
                    }
                };
                lista.Add(resena);
            }
            return lista;
        }

        public bool Aprobar(int resenaId)
        {
            var ht = new Hashtable();
            ht.Add("@ID_Resena", resenaId);
            return oAcceso.EscribirSP("sp_AprobarResena", ht) > 0;
        }

        public bool Eliminar(int resenaId)
        {
            var ht = new Hashtable();
            ht.Add("@ID_Resena", resenaId);
            return oAcceso.EscribirSP("sp_EliminarResena", ht) > 0;
        }
        public bool Guardar(BEResena resena)
        {
            var ht = new Hashtable();
            ht.Add("@ID_Usuario", resena.Autor.Codigo);
            ht.Add("@Puntuacion", resena.Puntuacion);
            ht.Add("@Asunto", resena.Asunto);
            ht.Add("@Comentario", resena.Comentario);

            return oAcceso.EscribirSP("sp_GuardarResena", ht) > 0;
        }

        public List<BEResena> ListarAprobadas()
        {
            var lista = new List<BEResena>();
            DataTable dt = oAcceso.LeerSP("sp_ListarResenasAprobadas", null);

            foreach (DataRow dr in dt.Rows)
            {
                var resena = new BEResena
                {
                    Puntuacion = Convert.ToInt32(dr["Puntuacion"]),
                    Asunto = dr["Asunto"].ToString(),
                    Comentario = dr["Comentario"].ToString(),
                    Fecha = Convert.ToDateTime(dr["Fecha"]),
                    Autor = new BEUsuario
                    {
                        Nombre = dr["NombreAutor"].ToString(),
                        Apellido = dr["ApellidoAutor"].ToString()
                    }
                };
                lista.Add(resena);
            }
            return lista;
        }
    }
}