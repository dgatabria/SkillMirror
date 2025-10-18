using BE;
using DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace MPP
{
    /// <summary>
    /// Mapper para toda la lógica de negocio relacionada con la facturación,
    /// pagos y notas de crédito.
    /// </summary>
    public class MPPFacturacion
    {
        private readonly Acceso oAcceso;

        public MPPFacturacion()
        {
            oAcceso = new Acceso();
        }

        /// <summary>
        /// Crea una nueva nota de crédito en la base de datos.
        /// </summary>
        public string CrearNotaCredito(int idEmpresa, int idFacturaOrigen, double valor, int diasVigencia)
        {
            try
            {
                var ht = new Hashtable();
                ht.Add("@ID_Empresa", idEmpresa);
                ht.Add("@ID_FacturaOrigen", idFacturaOrigen);
                ht.Add("@Valor", valor); // Se usa la nueva columna 'Valor'
                ht.Add("@DiasVigencia", diasVigencia);

                DataTable dt = oAcceso.LeerSP("sp_CrearNotaCredito", ht);

                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["CodigoGenerado"].ToString();
                }
                return null;
            }
            catch (Exception)
            {
                // En un caso real, se debería registrar el error en la bitácora.
                return null;
            }
        }

        /// <summary>
        /// Lista las notas de crédito que una empresa puede utilizar.
        /// </summary>
        public List<BENotaCredito> ListarNotasCreditoActivasPorEmpresa(BEEmpresa empresa)
        {
            var ht = new Hashtable();
            ht.Add("@ID_Empresa", empresa.Codigo);
            DataTable dt = oAcceso.LeerSP("sp_ListarNotasCreditoActivasPorEmpresa", ht);
            var lista = new List<BENotaCredito>();
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    lista.Add(new BENotaCredito
                    {
                        Codigo = Convert.ToInt32(row["ID"]),
                        CodigoNC = row["Codigo"].ToString(),
                        Valor = Convert.ToDouble(row["Valor"]), // Se mapea desde 'Valor'
                        FechaVencimiento = Convert.ToDateTime(row["FechaVencimiento"])
                    });
                }
            }
            return lista;
        }

        /// <summary>
        /// Lista todas las notas de crédito del sistema para la consola de administración.
        /// </summary>
        public List<BENotaCredito> ListarTodasLasNotasDeCredito()
        {
            DataTable dt = oAcceso.LeerSP("sp_ListarTodasLasNotasDeCredito", null);
            var lista = new List<BENotaCredito>();
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    lista.Add(new BENotaCredito
                    {
                        Codigo = Convert.ToInt32(row["ID"]),
                        CodigoNC = row["CodigoNC"].ToString(),
                        Valor = Convert.ToDouble(row["Valor"]), // Se mapea desde 'Valor'
                        FechaVencimiento = Convert.ToDateTime(row["FechaVencimiento"]),
                        NombreEmpresa = row["NombreEmpresa"].ToString(),
                        Estado = row["Estado"].ToString()
                    });
                }
            }
            return lista;
        }

        /// <summary>
        /// Orquesta el proceso de pago completo en la base de datos.
        /// </summary>
        public int ProcesarPagoYFacturar(BEPago pago)
        {
            var ht = new Hashtable();
            MPPCrypto mppCrypto = new MPPCrypto();

            ht.Add("@ID_Empresa", pago.Empresa.Codigo);
            ht.Add("@ID_Usuario", pago.Usuario.Codigo);
            ht.Add("@ID_NivelServicio", pago.Plan.Codigo);
            ht.Add("@DescripcionFactura", $"Suscripción Plan {pago.Plan.Nombre}");
            ht.Add("@MontoTotal", pago.Plan.CostoMensual);

            if (pago.Tarjeta != null)
            {
                ht.Add("@MontoTarjeta", pago.Tarjeta.MontoAbonado);
                ht.Add("@Titular", mppCrypto.Encrypt(pago.Tarjeta.Titular));
                ht.Add("@Numero", mppCrypto.Encrypt(pago.Tarjeta.Numero));
                ht.Add("@Expiracion", mppCrypto.Encrypt(pago.Tarjeta.Expiracion));
                ht.Add("@CVV", mppCrypto.Encrypt(pago.Tarjeta.CVV));
            }
            if (pago.NotaCreditoUtilizada != null)
            {
                ht.Add("@ID_NotaCreditoUsada", pago.NotaCreditoUtilizada.Codigo);
            }
            if (pago.Transferencia != null)
            {
                ht.Add("@MontoTransferencia", pago.Transferencia.MontoAbonado);
                ht.Add("@CodigoComprobante", pago.Transferencia.CodigoComprobante);
            }

            try
            {
                DataTable dt = oAcceso.LeerSP("sp_ProcesarPagoYFacturar", ht);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return Convert.ToInt32(dt.Rows[0]["ID_FacturaGenerada"]);
                }
                return -1;
            }
            catch (Exception)
            {
                return -1; // Se debería registrar el error
            }
        }

        /// <summary>
        /// Lista todas las facturas pagadas de una empresa, para ser usadas como origen de una NC.
        /// </summary>
        public List<BEFactura> ListarFacturasPorEmpresa(int idEmpresa)
        {
            var ht = new Hashtable();
            ht.Add("@ID_Empresa", idEmpresa);
            DataTable dt = oAcceso.LeerSP("sp_ListarFacturasPorEmpresa", ht);
            var lista = new List<BEFactura>();
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    lista.Add(new BEFactura
                    {
                        Codigo = Convert.ToInt32(row["ID"]),
                        NumeroFactura = row["NumeroFactura"].ToString(),
                        FechaEmision = Convert.ToDateTime(row["FechaEmision"]),
                        MontoTotal = Convert.ToDouble(row["MontoTotal"]),
                        Descripcion = row["Descripcion"].ToString()
                    });
                }
            }
            return lista;
        }
    }
}

