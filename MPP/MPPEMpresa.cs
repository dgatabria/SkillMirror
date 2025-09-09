using System.Collections;
using System.Collections.Generic;
using System.Data;
using BE;
using DAL;

namespace MPP
{
    public class MPPEmpresa
    {
        private Acceso bd;

        public bool Guardar(BEEmpresa empresa)
        {
            bd = new Acceso();
            string query = "sp_GuardarEmpresa";
            Hashtable ht = new Hashtable();
            ht.Add("@ID", empresa.Codigo);
            ht.Add("@Nombre", empresa.Nombre);
            ht.Add("@Telefono", empresa.Telefono);
            ht.Add("@Domicilio", empresa.Domicilio);
            ht.Add("@MedioDePago", empresa.MedioDePago);
            ht.Add("@CUIT", empresa.CUIT);

            return bd.EscribirSP(query, ht) == 1; 
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


        public BEEmpresa ListarObjeto(BEEmpresa empresaFiltro)
        {
            bd = new Acceso();
            string query = "sp_ListarEmpresaPorID";
            Hashtable ht = new Hashtable();

            if (empresaFiltro.Codigo > 0)
            {
                ht.Add("@ID", empresaFiltro.Codigo);
            }
            else
            {
                return null; // No se puede buscar sin un ID
            }

            DataTable tabla = bd.LeerSP(query, ht);

            if (tabla != null && tabla.Rows.Count > 0)
            {
                // Si encontramos un resultado, lo mapeamos usando la función que ya tenemos
                return Mapear(tabla.Rows[0]);
            }
            else
            {
                return null;
            }
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