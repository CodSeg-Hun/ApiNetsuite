using ApiNetsuite.Repositorio.IRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using System;
using System.Data.SqlClient;
using ApiNetsuite.DTO.OrdenServicio;
using System.Transactions;
using Microsoft.Extensions.Options;
using System.Data.Common;
using System.Reflection;
using ApiNetsuite.Clases;
using Newtonsoft.Json;

namespace ApiNetsuite.Repositorio
{
    public class OrdenServicioSQL : IOrdenServicioSQL
    {
        private string CadenaConexion;
        private readonly ILogger<OrdenServicioSQL> log;


        public OrdenServicioSQL(AccesoDatos cadenaConexion, ILogger<OrdenServicioSQL> l)
        {
            CadenaConexion = cadenaConexion.CadenaConexionSQL;
            this.log = l;
        }


        private SqlConnection conexion()
        {
            return new SqlConnection(CadenaConexion);
        }


        public static string OrdenServicio(OrdenServicioDTO data)
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string cadena = MyConfig.GetValue<string>("ConnectionStrings:SQL");
            //DataSet ds = new DataSet();
            string resultado = "";
            try
            {
                using SqlConnection connection = new SqlConnection(cadena);
                {
                    connection.Open();
                    SqlTransaction transaction = null;
                    try
                    {
                        // Iniciar una transacción
                        transaction = connection.BeginTransaction();
                        using SqlCommand command = new SqlCommand("VENTAS.DBO.MIG_SP_PASO_DATOS_ORDEN_SERVICIO", connection);
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Transaction = transaction;
                            command.Parameters.Clear();
                            if (data.origen == "OS" && data.accion == "E")
                            {
                                command.Parameters.Add("@OPCION", SqlDbType.VarChar).Value = "ELI";
                                command.Parameters.Add("@ORIGEN", SqlDbType.VarChar).Value = data.origen;
                                command.Parameters.Add("@ACCION", SqlDbType.VarChar).Value = data.accion;
                                command.Parameters.Add("@CODIGO_EMPRESA", SqlDbType.Int).Value = data.Codigo_Empresa;
                                command.Parameters.Add("@CODIGO_SUCURSAL", SqlDbType.Int).Value = data.Codigo_Sucursal;
                                command.Parameters.Add("@NUMERO_GENERAL", SqlDbType.VarChar).Value = data.Numero_General;
                                command.Parameters.Add("@ID_OS", SqlDbType.BigInt).Value = data.Id_os;
                                command.ExecuteNonQuery();
                            }
                            else
                            {
                                command.Parameters.Add("@OPCION", SqlDbType.VarChar).Value = "CAB";
                                command.Parameters.Add("@CODIGO_EMPRESA", SqlDbType.Int).Value = data.Codigo_Empresa;
                                command.Parameters.Add("@CODIGO_SUCURSAL", SqlDbType.Int).Value = data.Codigo_Sucursal;
                                command.Parameters.Add("@NUMERO_GENERAL", SqlDbType.VarChar).Value = data.Numero_General;
                                command.Parameters.Add("@ID_OS", SqlDbType.BigInt).Value = data.Id_os;
                                command.Parameters.Add("@FECHA", SqlDbType.DateTime).Value = data.Fecha;
                                command.Parameters.Add("@ORDEN_TRABAJO", SqlDbType.BigInt).Value = data.Orden_Trabajo;
                                command.Parameters.Add("@EJECUTIVA_SERVICIO", SqlDbType.BigInt).Value = data.Ejecutiva_Servicio;
                                command.Parameters.Add("@EJECUTIVA_RENOVACION", SqlDbType.BigInt).Value = data.Ejecutiva_Renovacion;
                                command.Parameters.Add("@IDENTIFICACION_CLIENTE", SqlDbType.VarChar).Value = data.Identificacion_Cliente;
                                command.Parameters.Add("@ID_CLIENTE_NS", SqlDbType.BigInt).Value = data.Id_Cliente_ns;
                                command.Parameters.Add("@CODIGO_VEHICULO", SqlDbType.VarChar).Value = data.Codigo_Vehiculo;
                                command.Parameters.Add("@ID_VEHICULO_NS", SqlDbType.BigInt).Value = data.Id_Vehiculo_ns;
                                command.Parameters.Add("@CODIGO_VEHICULO_SH", SqlDbType.VarChar).Value = data.Codigo_Vehiculo_Sh;
                                command.Parameters.Add("@CODIGO_ASEGURADORA", SqlDbType.BigInt).Value = data.Codigo_Aseguradora;
                                command.Parameters.Add("@CODIGO_CONCESIONARIO", SqlDbType.BigInt).Value = data.Codigo_Concesionario;
                                command.Parameters.Add("@VENDEDOR", SqlDbType.BigInt).Value = data.Vendedor;
                                command.Parameters.Add("@FECHA_INICIO", SqlDbType.DateTime).Value = data.Fecha_Inicio;
                                command.Parameters.Add("@FECHA_FIN", SqlDbType.DateTime).Value = data.Fecha_Fin;
                                command.Parameters.Add("@SUBTOTAL", SqlDbType.Float).Value = data.Subtotal;
                                //command.Parameters.Add("@TOTAL_DESCUENTO", SqlDbType.Float).Value = data.total;
                                //command.Parameters.Add("@IVA", SqlDbType.Float).Value = data.i;
                                //command.Parameters.Add("@PAGAR_CLIENTE", SqlDbType.Float).Value = data.pa;
                                command.Parameters.Add("@FACTURAR_A", SqlDbType.BigInt).Value = data.Facturar_a;
                                command.Parameters.Add("@IDESTADO", SqlDbType.VarChar, 1).Value = data.IdEstado;
                                command.Parameters.Add("@ESTADO", SqlDbType.VarChar, 100).Value = data.Estado;
                                command.Parameters.Add("@USUARIO_INGRESO", SqlDbType.BigInt).Value = data.Usuario_Ingreso;
                                command.Parameters.Add("@FECHA_INGRESO", SqlDbType.DateTime).Value = data.Fecha_Ingreso;
                                command.Parameters.Add("@FECHA_MODIFICACION", SqlDbType.DateTime).Value = data.Fecha_Modificacion;
                                command.Parameters.Add("@FECHA_ANULACION", SqlDbType.DateTime).Value = data.Fecha_Anulacion;
                                command.Parameters.Add("@IDAPROBACION_VENTA", SqlDbType.BigInt).Value = data.IdAprobacion_Venta;
                                command.Parameters.Add("@APROBACION_VENTA", SqlDbType.VarChar).Value = data.Aprobacion_Venta;
                                command.Parameters.Add("@NOTA_FACTURA", SqlDbType.VarChar).Value = data.Nota_Factura;
                                command.Parameters.Add("@CONSIDERACION_TRABAJO", SqlDbType.VarChar).Value = data.Consideracion_Trabajo;
                                command.Parameters.Add("@REQUIERE_APROBACION_CARTERA", SqlDbType.VarChar).Value = data.Requiere_Aprobacion_Cartera;
                                command.Parameters.Add("@APROBACION_CARTERA", SqlDbType.VarChar).Value = data.Aprobacion_Cartera;
                                command.Parameters.Add("@CODIGO_FINANCIERA", SqlDbType.BigInt).Value = data.Codigo_Financiera;
                                command.Parameters.Add("@DISPOSITIVO_CUSTODIA", SqlDbType.VarChar).Value = data.Dispositivo_Custodia;
                                command.Parameters.Add("@TRABAJADO", SqlDbType.VarChar).Value = data.Trabajado;
                                command.Parameters.Add("@FECHA_FIN_TRABAJO", SqlDbType.DateTime).Value = data.Fecha_Fin_Trabajo;
                                command.Parameters.Add("@EJECUTIVA_INICIO_GESTION", SqlDbType.BigInt).Value = data.Ejecutiva_Inicio_Gestion;
                                command.Parameters.Add("@PARALIZADOR", SqlDbType.VarChar).Value = data.Paralizador;
                                command.Parameters.Add("@MONEDA", SqlDbType.Int).Value = data.Moneda;
                                command.Parameters.Add("@CODIGO_CLIENTE_MONITOREO", SqlDbType.BigInt).Value = data.Codigo_Cliente_Monitoreo;
                                command.Parameters.Add("@ID_VEHICULO_CUSTODIO", SqlDbType.BigInt).Value = data.Id_Vehiculo_Custodia;
                                //command.Parameters.Add("@DESCUENTO_GENERAL", SqlDbType.VarChar).Value = data.Descuento_General;
                                command.Parameters.Add("@FACTURADO", SqlDbType.VarChar).Value = data.Facturado;
                                command.Parameters.Add("@IDFACTURA", SqlDbType.BigInt).Value = data.IdFactura;
                                command.Parameters.Add("@REFERENCIAFACTURA", SqlDbType.VarChar).Value = data.ReferenciaFactura;
                                command.Parameters.Add("@FECHAFACTURA", SqlDbType.DateTime).Value = data.FechaFactura;
                                command.Parameters.Add("@IDPAGO", SqlDbType.BigInt).Value = data.IdPago;
                                command.Parameters.Add("@REFPAGO", SqlDbType.VarChar).Value = data.RefPago;
                                command.Parameters.Add("@FECHAPAGO", SqlDbType.DateTime).Value = data.FechaPago;
                                command.Parameters.Add("@TOTALCOBRADO", SqlDbType.Float).Value = data.TotalCobrado;
                                command.Parameters.Add("@COBRADO", SqlDbType.VarChar).Value = data.Cobrado;
                                command.Parameters.Add("@ORIGEN", SqlDbType.VarChar).Value = data.origen;
                                command.Parameters.Add("@ACCION", SqlDbType.VarChar).Value = data.accion;
                                command.Parameters.Add("@TERMINO_PAGO", SqlDbType.VarChar).Value = data.TerminoPago;
                                command.Parameters.Add("@BANCO", SqlDbType.VarChar).Value = data.Banco;
                                command.Parameters.Add("@EMISOR_TARJETA", SqlDbType.VarChar).Value = data.EmisorTarjeta;
                                command.Parameters.Add("@FECHA_VENC_TARJETA", SqlDbType.DateTime).Value = data.FechaVencimientoTarjeta;
                                command.Parameters.Add("@NUMERO_CUENTA", SqlDbType.VarChar).Value = data.NumeroCuenta;
                                command.Parameters.Add("@PLAZO_TARJETA", SqlDbType.VarChar).Value = data.PlazoTarjeta;
                                command.Parameters.Add("@PROMOCION_DA", SqlDbType.VarChar).Value = data.PromocioDA;
                                command.Parameters.Add("@TIPO_CUENTA_BANCO", SqlDbType.VarChar).Value = data.TipoCuentaBanco;
                                command.Parameters.Add("@TITULAR_CUENTA", SqlDbType.VarChar).Value = data.TitularCuenta;
                                command.Parameters.Add("@TOTAL_GENERAL", SqlDbType.Float).Value = data.Total_General;
                                command.Parameters.Add("@IVA_TOTAL", SqlDbType.Float).Value = data.Iva_Total;
                                command.Parameters.Add("@VALOR_PROMO_DA", SqlDbType.Float).Value = data.ValorPromocionDA;
                                command.Parameters.Add("@TITULAR_CUENTA_ID", SqlDbType.VarChar).Value = data.TitularCuentaId;
                                command.Parameters.Add("@FACTURAR_A_ID", SqlDbType.VarChar).Value = data.FacturaraId;
                                command.Parameters.Add("@ORIGENCONVENIOOS", SqlDbType.VarChar).Value = data.OrigenConvenioOS;
                                command.Parameters.Add("@ESFACTURAINTERNA", SqlDbType.VarChar).Value = data.EsFacturaInterna;
                                command.Parameters.Add("@FECHAFACTURAINTERNA", SqlDbType.DateTime).Value = data.FechaFacturaInterna;
                                command.ExecuteNonQuery();
                                for (int i = 0; i < data.DetalleOrden.Count; i++)
                                {
                                    command.Parameters.Clear();
                                    command.Parameters.Add("@OPCION", SqlDbType.VarChar).Value = "DET";
                                    command.Parameters.Add("@ORIGEN", SqlDbType.VarChar).Value = data.origen;
                                    command.Parameters.Add("@ACCION", SqlDbType.VarChar).Value = data.accion;
                                    command.Parameters.Add("@CODIGO_EMPRESA", SqlDbType.Int).Value = data.Codigo_Empresa;
                                    command.Parameters.Add("@CODIGO_SUCURSAL", SqlDbType.Int).Value = data.Codigo_Sucursal;
                                    command.Parameters.Add("@NUMERO_GENERAL", SqlDbType.VarChar).Value = data.Numero_General;
                                    command.Parameters.Add("@ID_OS", SqlDbType.BigInt).Value = data.Id_os;
                                    command.Parameters.Add("@SECUENCIA_DETALLE", SqlDbType.Int).Value = data.DetalleOrden[i].Secuencia;
                                    command.Parameters.Add("@IDITEM", SqlDbType.BigInt).Value = data.DetalleOrden[i].IdItem;
                                    command.Parameters.Add("@CODIGOITEM", SqlDbType.VarChar).Value = data.DetalleOrden[i].CodigoItem;
                                    command.Parameters.Add("@ITEMDESCRIPCION", SqlDbType.VarChar).Value = data.DetalleOrden[i].ItemDescripcion;
                                    command.Parameters.Add("@CLASE", SqlDbType.Int).Value = data.DetalleOrden[i].Clase;
                                    command.Parameters.Add("@ADP", SqlDbType.VarChar).Value = data.DetalleOrden[i].ADP;
                                    command.Parameters.Add("@PPS", SqlDbType.VarChar).Value = data.DetalleOrden[i].PPS;
                                    command.Parameters.Add("@TIPO_TRANSACCION", SqlDbType.VarChar, 10).Value = data.DetalleOrden[i].Tipo_Transaccion;
                                    command.Parameters.Add("@GRUPO_PRODUCTO", SqlDbType.VarChar).Value = data.DetalleOrden[i].Grupo_Producto;
                                    command.Parameters.Add("@PRECIOUNITARIO", SqlDbType.Float).Value = data.DetalleOrden[i].PrecioUnitario;
                                    command.Parameters.Add("@IVA", SqlDbType.Float).Value = data.DetalleOrden[i].Iva;
                                    command.Parameters.Add("@PRECIOBASE", SqlDbType.Float).Value = data.DetalleOrden[i].PrecioBase;
                                    command.Parameters.Add("@TOTAL_ITEM", SqlDbType.Float).Value = data.DetalleOrden[i].Total_Item;
                                    command.Parameters.Add("@PRECIOCLIENTE", SqlDbType.Float).Value = data.DetalleOrden[i].PrecioCliente;
                                    command.Parameters.Add("@DESCUENTO_GENERAL", SqlDbType.VarChar).Value = data.DetalleOrden[i].Descuento_General;
                                    command.Parameters.Add("@VALOR_PROMOCION", SqlDbType.Float).Value = data.DetalleOrden[i].ValorPromocion;
                                    command.Parameters.Add("@TASA_DESCTO_ITEM", SqlDbType.Float).Value = data.DetalleOrden[i].TasaDsctoItem;
                                    command.Parameters.Add("@TASA_DESCTO_GENERAL", SqlDbType.Float).Value = data.DetalleOrden[i].TasaDsctoGeneral;
                                    command.Parameters.Add("@ORDENFABRICACION", SqlDbType.VarChar).Value = data.DetalleOrden[i].OrdenFabricacion;
                                    command.Parameters.Add("@IDTIPOPLAZO", SqlDbType.Int).Value = data.DetalleOrden[i].IdTipoPlazo;
                                    command.Parameters.Add("@TIPOPLAZO", SqlDbType.VarChar).Value = data.DetalleOrden[i].TipoPlazo;
                                    command.Parameters.Add("@PLAZO", SqlDbType.Int).Value = data.DetalleOrden[i].Plazo;
                                    command.Parameters.Add("@TIPOARTICULO", SqlDbType.VarChar).Value = data.DetalleOrden[i].TipoArticulo;
                                    command.Parameters.Add("@IVADESCRIPCION", SqlDbType.VarChar).Value = data.DetalleOrden[i].IdIvaDescripcion;
                                    command.Parameters.Add("@IDIVA", SqlDbType.Int).Value = data.DetalleOrden[i].IdIva;
                                    command.Parameters.Add("@IVAPORCENTAJE", SqlDbType.VarChar).Value = data.DetalleOrden[i].PorcentajeIva;
                                    command.Parameters.Add("@CHEQUEADO", SqlDbType.VarChar).Value = data.DetalleOrden[i].Chequeado;
                                    command.Parameters.Add("@ID_OT", SqlDbType.Int).Value = data.DetalleOrden[i].Id_OT;
                                    command.Parameters.Add("@FECHA_CHEQUEO", SqlDbType.DateTime).Value = data.DetalleOrden[i].Fecha_Chequeo;
                                    command.Parameters.Add("@HORA_CHEQUEO", SqlDbType.VarChar).Value = data.DetalleOrden[i].Hora_Chequeo;
                                    command.Parameters.Add("@CODIGO_CLIENTE_NUEVO", SqlDbType.VarChar).Value = data.DetalleOrden[i].Codigo_Cliente_Nuevo;
                                    command.Parameters.Add("@CODIGO_CLIENTE_NUEVO_NS", SqlDbType.BigInt).Value = data.DetalleOrden[i].Codigo_Cliente_Nuevo_Ns;
                                    command.Parameters.Add("@ID_TECNICO_CHQ", SqlDbType.Int).Value = data.DetalleOrden[i].IdTecnicoChequeo;
                                    command.Parameters.Add("@TECNICO_CHQ", SqlDbType.VarChar).Value = data.DetalleOrden[i].TecnicoChequeo;
                                    command.ExecuteNonQuery();
                                }
                                command.Parameters.Clear();
                                command.Parameters.Add("@OPCION", SqlDbType.VarChar).Value = "TOT";
                                command.Parameters.Add("@ORIGEN", SqlDbType.VarChar).Value = data.origen;
                                command.Parameters.Add("@ACCION", SqlDbType.VarChar).Value = data.accion;
                                command.Parameters.Add("@CODIGO_EMPRESA", SqlDbType.Int).Value = data.Codigo_Empresa;
                                command.Parameters.Add("@CODIGO_SUCURSAL", SqlDbType.Int).Value = data.Codigo_Sucursal;
                                command.Parameters.Add("@NUMERO_GENERAL", SqlDbType.VarChar).Value = data.Numero_General;
                                command.Parameters.Add("@ID_OS", SqlDbType.BigInt).Value = data.Id_os;
                                command.Parameters.Add("@IDITEM", SqlDbType.BigInt).Value = data.DetalleOrden[0].IdItem;
                                command.ExecuteNonQuery();
                            }                           
                        }
                        transaction.Commit();
                        resultado = "OK";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        resultado = ex.Message.ToString();
                        string Datos = LogDB.LogData(resultado, JsonConvert.SerializeObject(data), "OrdenServicio");
                        //throw;
                    }                  
                }
            }
            catch (Exception ex)
            {
                resultado = ex.Message.ToString();
               //throw;
            }
            return resultado;
        }


    }
}
