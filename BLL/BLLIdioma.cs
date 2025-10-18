using BE;
using MPP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLIdioma
    {
        // Instancia de la capa Mapper correspondiente.
        private readonly MPPIdioma oMPPIdioma;

        public BLLIdioma()
        {
            oMPPIdioma = new MPPIdioma();
        }

        public string ExportarAXml(int idiomaId)
        {
            return oMPPIdioma.ExportarAXml(idiomaId);
        }

        public void ImportarDesdeXml(Stream xmlStream)
        {
            // Aquí podrías agregar validaciones de negocio antes de importar,
            // como verificar que el archivo no esté vacío.
            if (xmlStream == null || xmlStream.Length == 0)
            {
                throw new Exception("El archivo XML está vacío o no es válido.");
            }
            oMPPIdioma.ImportarDesdeXml(xmlStream);
        }
        /// <summary>
        /// Llama a la capa MPP para obtener un idioma específico con sus traducciones.
        /// </summary>
        /// <param name="idioma">Objeto BEIdioma con el 'Codigo' a buscar.</param>
        /// <returns>El objeto BEIdioma completo.</returns>
        public BEIdioma ListarIdioma(BEIdioma idioma)
        {
            return oMPPIdioma.ListarIdioma(idioma);
        }

        /// <summary>
        /// Llama a la capa MPP para obtener la lista completa de idiomas.
        /// </summary>
        /// <returns>Una lista de objetos BEIdioma.</returns>
        public List<BEIdioma> ListarTodos()
        {
            return oMPPIdioma.ListarTodos();
        }

        /// <summary>
        /// Pasa el objeto Idioma a la capa MPP para ser guardado (alta o modificación).
        /// Aquí se podrían agregar validaciones de negocio antes de guardar.
        /// </summary>
        /// <param name="idioma">El objeto BEIdioma a persistir.</param>
        /// <returns>True si la operación fue exitosa.</returns>
        public bool Guardar(BEIdioma idioma)
        {
            // Ejemplo de regla de negocio: No permitir guardar un idioma sin nombre.
            if (string.IsNullOrWhiteSpace(idioma.Nombre))
            {
                throw new Exception("El nombre del idioma no puede estar vacío.");
            }
            return oMPPIdioma.Guardar(idioma);
        }

        /// <summary>
        /// Pasa el objeto Idioma a la capa MPP para ser eliminado.
        /// Aquí se podrían agregar validaciones antes de eliminar.
        /// </summary>
        /// <param name="idioma">El objeto BEIdioma a eliminar.</param>
        /// <returns>True si la operación fue exitosa.</returns>
        public bool Baja(BEIdioma idioma)
        {
            // Ejemplo de regla de negocio: Podríamos verificar algo aquí antes de llamar a la MPP.
            // Por ejemplo, si un idioma "por defecto" no se puede borrar.
            return oMPPIdioma.Baja(idioma);
        }
    }
}
