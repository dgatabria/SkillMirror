using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MPP
{
    /// <summary>
    /// Se encarga de la comunicación con la capa de datos para la entidad Plan.
    /// Transforma los resultados de la base de datos (DataTable/DataSet) en objetos de negocio (BE).
    /// </summary>
    public class MPPPlanes
    {
        /// <summary>
        /// Obtiene la lista de planes activos para mostrar en el catálogo público.
        /// Utiliza un SP que devuelve un DataSet con dos tablas para ser más eficiente.
        /// </summary>
        public List<BEPlan> Listar()
        {
            Acceso oDatos = new Acceso();
            // Este SP devuelve un DataSet con dos tablas: Planes y PlanFeatures.
            DataSet ds = oDatos.LeerSPDS("sp_ListarPlanesPublico", null);
            var listaPlanes = new List<BEPlan>();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                // Usamos un diccionario para un acceso rápido a los planes por su ID.
                // Esto es mucho más rápido que buscar en la lista en cada iteración.
                var planesDict = new Dictionary<int, BEPlan>();

                // 1. Mapeamos la primera tabla (Planes) a la lista de objetos BEPlan.
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    var plan = new BEPlan
                    {
                        ID = Convert.ToInt32(row["ID"]),
                        Nombre = row["Nombre"].ToString(),
                        Subtitulo = row["Subtitulo"].ToString(),
                        PrecioMensual = Convert.ToDecimal(row["PrecioMensual"]),
                        Orden = Convert.ToInt32(row["Orden"]),
                        EsDestacado = Convert.ToBoolean(row["EsDestacado"]),
                        Activo = true // Sabemos que son activos porque el SP los filtra
                    };
                    listaPlanes.Add(plan);
                    planesDict.Add(plan.ID, plan);
                }

                // 2. Mapeamos la segunda tabla (PlanFeatures) y asignamos cada feature a su plan correspondiente.
                if (ds.Tables.Count > 1)
                {
                    foreach (DataRow featureRow in ds.Tables[1].Rows)
                    {
                        int idPlan = Convert.ToInt32(featureRow["ID_Plan"]);

                        // Si el plan existe en nuestro diccionario, le agregamos la característica.
                        if (planesDict.ContainsKey(idPlan))
                        {
                            var feature = new BEPlanFeature
                            {
                                ID_Feature = Convert.ToInt32(featureRow["ID_Feature"]),
                                FeatureNombre = featureRow["FeatureNombre"].ToString(),
                                Clave = featureRow["Clave"].ToString(),
                                Descripcion = featureRow["Descripcion"].ToString()
                            };
                            planesDict[idPlan].Features.Add(feature);
                        }
                    }
                }
            }
            return listaPlanes;
        }

        // --- MÉTODOS PARA EL ABM ---

        /// <summary>
        /// Obtiene una lista simplificada de planes para el panel de administración.
        /// </summary>
        public List<BEPlan> ListarAdmin()
        {
            Acceso oDatos = new Acceso();
            DataTable dt = oDatos.LeerSP("sp_ListarPlanesAdmin", null);
            var listaPlanes = new List<BEPlan>();
            foreach (DataRow row in dt.Rows)
            {
                listaPlanes.Add(new BEPlan
                {
                    ID = Convert.ToInt32(row["ID"]),
                    Nombre = row["Nombre"].ToString(),
                    PrecioMensual = Convert.ToDecimal(row["PrecioMensual"]),
                    Orden = Convert.ToInt32(row["Orden"]),
                    Activo = Convert.ToBoolean(row["Activo"]),
                    EsDestacado = Convert.ToBoolean(row["EsDestacado"])
                });
            }
            return listaPlanes;
        }

        /// <summary>
        /// Obtiene un plan específico por su ID, incluyendo todas sus características.
        /// </summary>
        public BEPlan ListarPorID(int id)
        {
            Acceso oDatos = new Acceso();
            var ht = new System.Collections.Hashtable();
            ht.Add("@ID", id);
            DataSet ds = oDatos.LeerSPDS("sp_ListarPlanPorID", ht);
            BEPlan plan = null;

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                plan = new BEPlan
                {
                    ID = Convert.ToInt32(row["ID"]),
                    Nombre = row["Nombre"].ToString(),
                    Subtitulo = row["Subtitulo"].ToString(),
                    PrecioMensual = Convert.ToDecimal(row["PrecioMensual"]),
                    Orden = Convert.ToInt32(row["Orden"]),
                    Activo = Convert.ToBoolean(row["Activo"]),
                    EsDestacado = Convert.ToBoolean(row["EsDestacado"])
                };

                // Asignamos las características desde la segunda tabla del DataSet
                if (ds.Tables.Count > 1)
                {
                    foreach (DataRow featureRow in ds.Tables[1].Rows)
                    {
                        plan.Features.Add(new BEPlanFeature
                        {
                            ID_Feature = Convert.ToInt32(featureRow["ID_Feature"]),
                            FeatureNombre = featureRow["FeatureNombre"].ToString(),
                            Clave = featureRow["Clave"].ToString(),
                            Descripcion = featureRow["Descripcion"].ToString()
                        });
                    }
                }
            }
            return plan;
        }

        /// <summary>
        /// Guarda un plan (ya sea nuevo o una modificación) junto con sus características.
        /// </summary>
        public int Guardar(BEPlan plan)
        {
            Acceso oDatos = new Acceso();

            var ht = new System.Collections.Hashtable();
            ht.Add("@ID", plan.ID);
            ht.Add("@Nombre", plan.Nombre);
            ht.Add("@Subtitulo", plan.Subtitulo);
            ht.Add("@PrecioMensual", plan.PrecioMensual);
            ht.Add("@Orden", plan.Orden);
            ht.Add("@Activo", plan.Activo);
            ht.Add("@EsDestacado", plan.EsDestacado);

            // Creamos el DataTable para el Table-Valued Parameter de SQL.
            DataTable dtFeatures = new DataTable();
            dtFeatures.Columns.Add("ID_Feature", typeof(int));
            dtFeatures.Columns.Add("Descripcion", typeof(string));

            foreach (BEPlanFeature feature in plan.Features)
            {
                dtFeatures.Rows.Add(feature.ID_Feature, feature.Descripcion);
            }

            // Usamos un método de la DAL que permite enviar un TVP y devuelve un DataTable.
            // Esto es importante porque el SP nos devuelve el ID del plan guardado.
            DataTable dtResult = oDatos.EscribirSPConTVPYDevolverDataTable("sp_GuardarPlan", ht, dtFeatures, "dbo.PlanFeatureTabla", "@Features");

            int nuevoId = plan.ID;
            if (dtResult != null && dtResult.Rows.Count > 0)
            {
                nuevoId = Convert.ToInt32(dtResult.Rows[0][0]);
            }
            return nuevoId;
        }

        /// <summary>
        /// Realiza la baja lógica de un plan.
        /// </summary>
        public void BajaLogica(int id)
        {
            Acceso oDatos = new Acceso();
            var ht = new System.Collections.Hashtable();
            ht.Add("@ID", id);
            oDatos.EscribirSP("sp_BajaLogicaPlan", ht);
        }
    }
}

