using BE;
using MPP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions; // Necesario para Regex
using System.Web; // Necesario para HttpUtility

namespace BLL
{
    public class BLLEmpresa
    {
        private MPPEmpresa mpp;

        public BLLEmpresa()
        {
            mpp = new MPPEmpresa();
        }

        public int Guardar(BEEmpresa empresa)
        {
            // --- VALIDACIONES ---
            if (empresa == null)
                throw new ArgumentNullException("La empresa no puede ser nula.");

            // Sanitización (Anti-XSS) y Validación de Longitud/Vacío
            empresa.Nombre = ValidarYSanitizarString(empresa.Nombre, "Nombre Empresa", 50, true);
            empresa.Domicilio = ValidarYSanitizarString(empresa.Domicilio, "Domicilio", 50, false); // No requerido
            empresa.Telefono = ValidarYSanitizarString(empresa.Telefono, "Teléfono", 50, false); // No requerido
            empresa.CUIT = ValidarYSanitizarString(empresa.CUIT, "CUIT", 50, true);

            // Validación de Formato Específico (CUIT Argentino simple)
            // NOTA: Esta es una regex simple. Una validación real de CUIT incluye cálculo de dígito verificador.
            if (!Regex.IsMatch(empresa.CUIT, @"^\d{2}-\d{8}-\d{1}$"))
            {
                // Considera usar un sistema de excepciones más específico o devolver códigos de error.
                throw new ArgumentException("El formato del CUIT no es válido (debe ser XX-XXXXXXXX-X).");
            }

            // --- FIN VALIDACIONES ---

            // Si todo es válido, llamamos a la MPP
            return mpp.Guardar(empresa);
        }

        // --- MÉTODOS DE AYUDA PARA VALIDACIÓN ---
        private string ValidarYSanitizarString(string input, string nombreCampo, int maxLength, bool esRequerido)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                if (esRequerido)
                    throw new ArgumentException($"El campo '{nombreCampo}' es requerido.");
                else
                    return null; // O string.Empty según prefieras manejar opcionales
            }

            // Sanitización Anti-XSS
            string sanitizedInput = HttpUtility.HtmlEncode(input.Trim());

            // Validación de Longitud
            if (sanitizedInput.Length > maxLength)
                throw new ArgumentException($"El campo '{nombreCampo}' excede la longitud máxima de {maxLength} caracteres.");

            return sanitizedInput;
        }


        // --- OTROS MÉTODOS (sin cambios por ahora) ---
        public bool ActualizarPlan(int idEmpresa, int idPlan)
        {
            // Aquí también podrías validar que los IDs sean > 0
            if (idEmpresa <= 0 || idPlan <= 0)
                throw new ArgumentException("Los IDs de empresa y plan deben ser válidos.");
            return mpp.ActualizarPlan(idEmpresa, idPlan);
        }
        public bool Baja(BEEmpresa empresa)
        {
            if (empresa == null || empresa.Codigo <= 0)
                throw new ArgumentException("Debe proporcionar una empresa válida para dar de baja.");
            return mpp.Baja(empresa);
        }
        public BEEmpresa ListarObjeto(BEEmpresa empresa)
        {
            if (empresa == null || empresa.Codigo <= 0)
                throw new ArgumentException("Debe proporcionar una empresa válida para listar.");
            return mpp.ListarObjeto(empresa);
        }
        public List<BEEmpresa> ListarTodos()
        {
            return mpp.ListarTodos();
        }
    }
}
