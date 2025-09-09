using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using BE; // Asegúrate de que el namespace de tus entidades sea BE
using DAL; // Asegúrate de que el namespace de tu capa de acceso a datos sea DAL

namespace MPP
{
    /// <summary>
    /// Mapper para la entidad BEIdioma.
    /// Se encarga de la persistencia y recuperación de los datos de Idioma
    /// desde y hacia la base de datos, utilizando la capa DAL.
    /// </summary>
    public class MPPIdioma
    {
        private readonly Acceso oAcceso;

        public MPPIdioma()
        {
            // Instanciamos el objeto de acceso a datos una vez.
            oAcceso = new Acceso();
        }

        /// <summary>
        /// Obtiene un idioma específico con todas sus traducciones.
        /// Utiliza el SP sp_ListarIdiomaPorID.
        /// </summary>
        /// <param name="idioma">Objeto BEIdioma solo con la propiedad 'Codigo' seteada.</param>
        /// <returns>Un objeto BEIdioma completo o null si no se encuentra.</returns>
        public BEIdioma ListarIdioma(BEIdioma idioma)
        {
            try
            {
                var ht = new Hashtable();
                ht.Add("@ID_Idioma", idioma.Codigo);

                // Llamamos al SP que trae un idioma con sus traducciones
                DataTable dt = oAcceso.LeerSP("sp_ListarIdiomaPorID", ht);

                if (dt.Rows.Count > 0)
                {
                    BEIdioma idiomaCompleto = new BEIdioma();

                    // La primera fila tiene los datos del idioma.
                    DataRow firstRow = dt.Rows[0];
                    idiomaCompleto.Codigo = Convert.ToInt32(firstRow["IdiomaID"]);
                    idiomaCompleto.Nombre = firstRow["IdiomaNombre"].ToString();

                    // Iteramos por todas las filas para cargar el Hashtable de traducciones.
                    foreach (DataRow row in dt.Rows)
                    {
                        // Validamos que el tag no sea nulo, lo que puede pasar si un idioma no tiene traducciones.
                        if (row["TAG"] != DBNull.Value)
                        {
                            string tag = row["TAG"].ToString();
                            string texto = row["Texto"].ToString();
                            idiomaCompleto.Palabras.Add(tag, texto);
                        }
                    }
                    return idiomaCompleto;
                }
                return null; // Si no hay filas, el idioma no existe.
            }
            catch (Exception ex)
            {
                // Aquí podrías loguear el error antes de lanzarlo.
                throw new Exception("Error en MPP al listar el idioma por ID: " + ex.Message);
            }
        }

        /// <summary>
        /// Devuelve la lista completa de idiomas, cada uno con su respectivo
        /// diccionario de traducciones cargado.
        /// </summary>
        /// <returns>Una lista de objetos BEIdioma.</returns>
        public List<BEIdioma> ListarTodos()
        {
            try
            {
                // Primero, traemos la lista de todos los idiomas (solo ID y Nombre).
                DataTable dtIdiomas = oAcceso.LeerSP("sp_ListarIdiomas", null);

                var listaDeIdiomas = new List<BEIdioma>();

                if (dtIdiomas.Rows.Count > 0)
                {
                    // Por cada idioma en la lista, llamamos a ListarIdioma para cargarlo por completo.
                    foreach (DataRow row in dtIdiomas.Rows)
                    {
                        var idiomaParcial = new BEIdioma
                        {
                            Codigo = Convert.ToInt32(row["ID"])
                        };

                        // Reutilizamos el método que ya busca un idioma completo.
                        BEIdioma idiomaCompleto = ListarIdioma(idiomaParcial);
                        if (idiomaCompleto != null)
                        {
                            listaDeIdiomas.Add(idiomaCompleto);
                        }
                    }
                }
                return listaDeIdiomas;
            }
            catch (Exception ex)
            {
                throw new Exception("Error en MPP al listar todos los idiomas: " + ex.Message);
            }
        }

        /// <summary>
        /// Guarda un idioma nuevo (si Codigo = 0) o actualiza uno existente.
        /// Guarda en lote todas sus traducciones usando un Table-Valued Parameter.
        /// Utiliza el SP sp_GuardarIdiomaCompleto.
        /// </summary>
        /// <param name="idioma">El objeto BEIdioma a persistir.</param>
        /// <returns>True si la operación fue exitosa, de lo contrario False.</returns>
        public bool Guardar(BEIdioma idioma)
        {
            try
            {
                var ht = new Hashtable();
                ht.Add("@ID", idioma.Codigo);
                ht.Add("@Nombre", idioma.Nombre);

                // 1. Crear el DataTable que matchee con el tipo 'TraduccionTabla' de SQL.
                var dtTraducciones = new DataTable();
                dtTraducciones.Columns.Add("Tag", typeof(string));
                dtTraducciones.Columns.Add("Texto", typeof(string));

                // 2. Llenar el DataTable con los datos del Hashtable del objeto BEIdioma.
                foreach (DictionaryEntry traduccion in idioma.Palabras)
                {
                    dtTraducciones.Rows.Add(traduccion.Key.ToString(), traduccion.Value.ToString());
                }

                // 3. Llamar al método de la DAL que maneja Stored Procedures con Table-Valued Parameters.
                return oAcceso.EscribirSPConTVP("sp_GuardarIdiomaCompleto", ht, dtTraducciones);
            }
            catch (Exception ex)
            {
                throw new Exception("Error en MPP al guardar el idioma: " + ex.Message);
            }
        }

        /// <summary>
        /// Realiza la baja lógica de un idioma y sus traducciones asociadas.
        /// Utiliza el SP sp_EliminarIdioma.
        /// </summary>
        /// <param name="idioma">Objeto BEIdioma con el 'Codigo' del idioma a eliminar.</param>
        /// <returns>True si la eliminación fue exitosa, False si no se pudo eliminar (ej. está en uso).</returns>
        public bool Baja(BEIdioma idioma)
        {
            try
            {
                var ht = new Hashtable();
                ht.Add("@ID_Idioma", idioma.Codigo);

                // El SP sp_EliminarIdioma devuelve 0 en caso de éxito.
                // Usamos LeerSPRT que está diseñado para leer el valor de RETURN de un SP.
                int resultado = oAcceso.LeerSPRT("sp_EliminarIdioma", ht);

                // Si el SP devuelve 0, la operación fue exitosa.
                return resultado == 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error en MPP al dar de baja el idioma: " + ex.Message);
            }
        }
    }
}
