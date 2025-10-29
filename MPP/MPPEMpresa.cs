using BE;
using DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace MPP
{
    public class MPPEmpresa
    {
        private Acceso bd;

        public int Guardar(BEEmpresa empresa)
        {
            bd = new Acceso();
            string query = "sp_GuardarEmpresa_ConRetornoID";
            Hashtable ht = new Hashtable();
            // El SP manejará si el ID es 0 para un alta.
            ht.Add("@ID", empresa.Codigo);
            ht.Add("@Nombre", empresa.Nombre);
            ht.Add("@Telefono", empresa.Telefono);
            ht.Add("@Domicilio", empresa.Domicilio);
            ht.Add("@CUIT", empresa.CUIT);

            // LeerSP devuelve una tabla, y nuestro nuevo SP devuelve una tabla con una fila y una columna "ID".
            DataTable dt = bd.LeerSP(query, ht);
            if (dt != null && dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0]["ID"]);
            }
            return -1; // Indica un error
        }
        public bool ActualizarPlan(int idEmpresa, int idPlan)
        {
            bd = new Acceso();
            string query = "sp_ActualizarPlanEmpresa";
            Hashtable ht = new Hashtable();
            ht.Add("@ID_Empresa", idEmpresa);
            ht.Add("@ID_NivelServicio", idPlan);
            return bd.EscribirSP(query, ht) > 0;
        }

        public bool Baja(BEEmpresa empresa)
        {
            bd = new Acceso();
            string query = "sp_BorrarEmpresa";
            Hashtable ht = new Hashtable();
            ht.Add("@ID", empresa.Codigo);

            int resultado = bd.EscribirSP(query, ht); 
            return resultado == 1;
        }

        public List<BEEmpresa> ListarTodos()
        {
            bd = new Acceso();
            string query = "sp_ListarEmpresas";
            DataTable tabla = bd.LeerSP(query, null);
            var lista = new List<BEEmpresa>();
            if (tabla != null && tabla.Rows.Count > 0)
            {
                foreach (DataRow row in tabla.Rows)
                {
                    lista.Add(Mapear(row));
                }
            }
            return lista;
        }


        public BEEmpresa ListarObjeto(BEEmpresa empresa)
        {
            var ht = new Hashtable();
            ht.Add("@ID", empresa.Codigo);
            Acceso oAcceso = new Acceso();
            // Ahora llamamos al Stored Procedure actualizado, que ya nos trae toda la información.
            DataTable dt = oAcceso.LeerSP("sp_ListarEmpresaPorID", ht);

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                var emp = new BEEmpresa
                {
                    Codigo = Convert.ToInt32(dr["ID"]),
                    Nombre = dr["Nombre"].ToString(),
                    CUIT = dr["CUIT"].ToString(),
                    Telefono = dr["Telefono"].ToString(),
                    Domicilio = dr["Domicilio"].ToString(),
                    MedioDePago = dr["MedioDePago"].ToString()
                };

                // Verificamos si la empresa tiene un plan asignado.
                if (dr["NivelServicio"] != DBNull.Value)
                {
                    emp.PlanContratado = new BENivelServicio
                    {
                        Codigo = Convert.ToInt32(dr["NivelServicio"]),
                        Nombre = dr["NivelServicioNombre"].ToString(),
                        CostoMensual = Convert.ToDecimal(dr["PrecioMensual"])
                    };
                }
                return emp;
            }
            return null;
        }
        private BEEmpresa Mapear(DataRow row)
        {
            return new BEEmpresa
            {
                Codigo = (int)row["ID"],
                Nombre = row["Nombre"].ToString(),
                Telefono = row["Telefono"].ToString(),
                Domicilio = row["Domicilio"].ToString(),
                MedioDePago = row["MedioDePago"].ToString(),
                CUIT = row["CUIT"].ToString()
                
            };
        }
    }
}