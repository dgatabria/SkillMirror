using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class BETarjetaCredito
    {
        public string Titular { get; set; }
        public string Numero { get; set; }
        public string Expiracion { get; set; } // Formato "MM/AA"
        public string CVV { get; set; }
        public double MontoAbonado { get; set; }
    }

    /// <summary>
    /// Representa los datos de una transferencia bancaria.
    /// </summary>
    public class BETransferencia
    {
        public string CodigoComprobante { get; set; }
        public double MontoAbonado { get; set; }
    }

    /// <summary>
    /// Representa una Nota de Crédito.
    /// </summary>
    [Serializable]
    public class BENotaCredito
    {
        /// <summary>
        /// Identificador único interno (Primary Key).
        /// </summary>
        public int Codigo { get; set; }

        /// <summary>
        /// Código alfanumérico visible para el usuario (ej: "NC-20251012...").
        /// </summary>
        public string CodigoNC { get; set; }

        /// <summary>
        /// El valor total e inmutable de la nota de crédito.
        /// </summary>
        public double Valor { get; set; }

        /// <summary>
        /// Fecha hasta la cual la nota de crédito es válida.
        /// </summary>
        public DateTime FechaVencimiento { get; set; }

        /// <summary>
        /// Estado actual de la nota de crédito ('Activa', 'Agotada', 'Vencida').
        /// </summary>
        public string Estado { get; set; }

        /// <summary>
        /// Propiedad auxiliar para mostrar el nombre de la empresa en las grillas de administración.
        /// No se mapea directamente a la tabla NotaCredito.
        /// </summary>
        public string NombreEmpresa { get; set; }

        /// <summary>
        /// ID de la factura que originó esta nota de crédito. Es obligatorio.
        /// </summary>
        public int IdFacturaOrigen { get; set; }
    }

    /// <summary>
    /// Representa una Factura generada en el sistema.
    /// </summary>
    public class BEFactura
    {
        public int Codigo { get; set; }
        public string NumeroFactura { get; set; }
        public BEEmpresa Empresa { get; set; }
        public BEUsuario Usuario { get; set; }
        public DateTime FechaEmision { get; set; }
        public string Descripcion { get; set; }
        public double MontoTotal { get; set; }
        public string DisplayMember => $"{NumeroFactura} ({FechaEmision:dd/MM/yy}) - ${MontoTotal:N2}";
    }

    /// <summary>
    /// Clase contenedora que agrupa toda la información de un pago
    /// para facilitar su envío entre las capas de la aplicación.
    /// </summary>
    public class BEPago
    {
        public BEEmpresa Empresa { get; set; }
        public BEUsuario Usuario { get; set; }
        public BENivelServicio Plan { get; set; }

        // Medios de pago (serán null si no se utilizan)
        public BETarjetaCredito Tarjeta { get; set; }
        public BENotaCredito NotaCreditoUtilizada { get; set; }
        public BETransferencia Transferencia { get; set; }
    }
}
