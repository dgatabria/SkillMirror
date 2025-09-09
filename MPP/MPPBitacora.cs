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
    public class MPPBitacora
    {
        private Acceso bd;

        public List<EntradaBitacora> Listar(DateTime? fechaDesde, DateTime? fechaHasta, string accion, string detalle, int pageIndex, int pageSize, out int totalRecords)
        {
            bd = new Acceso();
            string query = "ListarBitacora";
            var ht = new Hashtable();

            ht.Add("@FechaDesde", fechaDesde.HasValue ? (object)fechaDesde.Value : DBNull.Value);
            ht.Add("@FechaHasta", fechaHasta.HasValue ? (object)fechaHasta.Value : DBNull.Value);
            ht.Add("@Accion", !string.IsNullOrWhiteSpace(accion) ? (object)accion : DBNull.Value);
            ht.Add("@Detalle", !string.IsNullOrWhiteSpace(detalle) ? (object)detalle : DBNull.Value);
            ht.Add("@PageIndex", pageIndex);
            ht.Add("@PageSize", pageSize);

            // Llamamos al nuevo método de la DAL
            var resultado = bd.LeerSPConTotalRecords(query, ht);
            DataTable tabla = resultado.Item1;
            totalRecords = resultado.Item2; // Asignamos el valor de salida

            var listaBitacora = new List<EntradaBitacora>();
            if (tabla != null && tabla.Rows.Count > 0)
            {
                foreach (DataRow row in tabla.Rows)
                {
                    var entrada = new EntradaBitacora
                    {
                        ID = Convert.ToInt32(row["ID"]),
                        TimeStamp = Convert.ToDateTime(row["TimeStamp"]),
                        NombreUsuario = row["NombreUsuario"].ToString(),
                        IP = row["IP"].ToString(),
                        Detalle = row["Detalle"].ToString(),
                        Accion = row["Accion"].ToString(),
                        Criticidad = Convert.ToInt32(row["Criticidad"])
                    };
                    listaBitacora.Add(entrada);
                }
            }
            return listaBitacora;
        }
    }
}
