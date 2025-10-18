using BE;
using MPP;
using System;
using System.Collections.Generic;

namespace BLL
{
    /// <summary>
    /// Encapsula la lógica de negocio para la facturación, los pagos
    /// y la gestión de notas de crédito. Actúa como intermediario
    /// entre la capa de presentación y la capa de acceso a datos.
    /// </summary>
    public class BLLFacturacion
    {
        private readonly MPPFacturacion _mppFacturacion;

        public BLLFacturacion()
        {
            _mppFacturacion = new MPPFacturacion();
        }

        /// <summary>
        /// Crea una nota de crédito, aplicando reglas de negocio antes de persistirla.
        /// </summary>
        /// <param name="idEmpresa">El ID de la empresa a la que se le asigna la NC.</param>
        /// <param name="idFacturaOrigen">El ID de la factura que origina la NC.</param>
        /// <param name="valor">El valor de la nota de crédito.</param>
        /// <param name="diasVigencia">Los días de vigencia de la nota de crédito.</param>
        /// <returns>El código único de la nota de crédito generada.</returns>
        public string CrearNotaCredito(int idEmpresa, int idFacturaOrigen, double valor, int diasVigencia)
        {
            // Aplicación de reglas de negocio
            if (valor <= 0)
            {
                throw new Exception("El valor de la nota de crédito debe ser mayor a cero.");
            }
            if (idEmpresa <= 1) // No se puede crear para la empresa interna "SkillMirror" o IDs inválidos.
            {
                throw new Exception("La empresa seleccionada no es válida.");
            }
            if (idFacturaOrigen <= 0)
            {
                throw new Exception("Debe seleccionar una factura de origen válida.");
            }
            if (diasVigencia <= 0)
            {
                throw new Exception("La vigencia debe ser de al menos un día.");
            }

            // Si las validaciones pasan, se llama a la capa de persistencia.
            return _mppFacturacion.CrearNotaCredito(idEmpresa, idFacturaOrigen, valor, diasVigencia);
        }

        /// <summary>
        /// Obtiene la lista de notas de crédito activas que una empresa puede utilizar.
        /// </summary>
        public List<BENotaCredito> ListarNotasCreditoActivasPorEmpresa(BEEmpresa empresa)
        {
            if (empresa == null || empresa.Codigo <= 0)
            {
                return new List<BENotaCredito>(); // Devuelve una lista vacía si la empresa no es válida.
            }
            return _mppFacturacion.ListarNotasCreditoActivasPorEmpresa(empresa);
        }

        public BEEstadisticasGanancias ObtenerEstadisticasGanancias()
        {
            // 1. Instanciamos el Mapper correspondiente
            MPPFacturacion oMapper = new MPPFacturacion();

            // 2. Simplemente llamamos al método del mapper y devolvemos su resultado.
            // Si hubiera lógica de negocio (ej. validar si el total es > 1000), 
            // iría aquí. En este caso, no hay.
            return oMapper.ObtenerEstadisticasGanancias();
        }

        /// <summary>
        /// Obtiene un listado de todas las notas de crédito del sistema para la consola de administración.
        /// </summary>
        public List<BENotaCredito> ListarTodasLasNotasDeCredito()
        {
            return _mppFacturacion.ListarTodasLasNotasDeCredito();
        }

        /// <summary>
        /// Orquesta el procesamiento de un pago, validando la información antes de persistirla.
        /// </summary>
        public int ProcesarPagoYFacturar(BEPago pago)
        {
            // En esta capa se podrían agregar validaciones más complejas.
            // Por ejemplo, verificar que si no se usa una nota de crédito, la suma de los
            // montos de los otros medios de pago cubra el total del plan.
            double totalPagadoConMediosDirectos = 0;
            if (pago.Tarjeta != null) totalPagadoConMediosDirectos += pago.Tarjeta.MontoAbonado;
            if (pago.Transferencia != null) totalPagadoConMediosDirectos += pago.Transferencia.MontoAbonado;

            // La lógica de "vuelto" de la nota de crédito ya la maneja el Stored Procedure,
            // por lo que la BLL no necesita preocuparse por ese cálculo.

            // Aquí podríamos añadir una validación estricta si fuera necesario:
            // if (pago.NotaCreditoUtilizada == null && totalPagadoConMediosDirectos < pago.Plan.CostoMensual)
            // {
            //     throw new Exception("El monto abonado no cubre el total del plan.");
            // }

            return _mppFacturacion.ProcesarPagoYFacturar(pago);
        }

        /// <summary>
        /// Obtiene un listado de todas las facturas de una empresa.
        /// Necesario para que el administrador pueda seleccionar una factura de origen al crear una NC.
        /// </summary>
        public List<BEFactura> ListarFacturasPorEmpresa(int idEmpresa)
        {
            if (idEmpresa <= 0)
            {
                return new List<BEFactura>();
            }
            return _mppFacturacion.ListarFacturasPorEmpresa(idEmpresa);
        }
    }
}

