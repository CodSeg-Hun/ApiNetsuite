using System.Collections.Generic;

namespace ApiNetsuite.DTO.OrdenServicio
{
    public class OrdenServicioDTO
    {

        public int Paginas { get; set; } = 0;

        public int Indice { get; set; } = 0;

        public int NumeroReg { get; set; } = 0;

        public int Codigo_Empresa { get; set; } = 0;

        public int Codigo_Sucursal { get; set; } = 0;

        public string Numero_General { get; set; } = "";

        public int Id_os { get; set; } = 0;

        public string Fecha { get; set; } = "";

        public int Orden_Trabajo { get; set; } = 0;

        public int Ejecutiva_Servicio { get; set; } = 0;

        public int Ejecutiva_Renovacion { get; set; } = 0;

        public string Identificacion_Cliente { get; set; } = "";

        public int Id_Cliente_ns { get; set; } = 0;

        public string Codigo_Vehiculo { get; set; } = "";

        public int Id_Vehiculo_ns { get; set; } = 0;

        public string Codigo_Vehiculo_Sh { get; set; } = "";

        public int Codigo_Aseguradora { get; set; } = 0;

        public int Codigo_Concesionario { get; set; } = 0;

        public int Vendedor { get; set; } = 0;

        public string Fecha_Inicio { get; set; } = "";

        public string Fecha_Fin { get; set; } = "";

        public int Facturar_a { get; set; } = 0;

        public string IdEstado { get; set; } = "";

        public string Estado { get; set; } = "";

        public int Usuario_Ingreso { get; set; } = 0;

        public string Fecha_Ingreso { get; set; } = "";

        public string Fecha_Modificacion { get; set; } = "";

        public string Fecha_Anulacion { get; set; } = "";

        public int IdAprobacion_Venta { get; set; } = 0;

        public string Aprobacion_Venta { get; set; } = "";

        public string Nota_Factura { get; set; } = "";

        public string Consideracion_Trabajo { get; set; } = "";

        public string Requiere_Aprobacion_Cartera { get; set; } = "";

        public string Aprobacion_Cartera { get; set; } = "";

        public int Codigo_Financiera { get; set; } = 0;

        public string Dispositivo_Custodia { get; set; } = "";

        public string Trabajado { get; set; } = "";

        public string Fecha_Fin_Trabajo { get; set; } = "";

        public int Ejecutiva_Inicio_Gestion { get; set; } = 0;

        public string Paralizador { get; set; } = "";

        public string Moneda { get; set; } = "";

        public int Codigo_Cliente_Monitoreo { get; set; } = 0;


        public string Facturado { get; set; } = "";

        public int IdFactura { get; set; } = 0;

        public string ReferenciaFactura { get; set; } = "";

        public string FechaFactura { get; set; } = "";

        public int IdPago { get; set; } = 0;

        public string RefPago { get; set; } = "";

        public string FechaPago { get; set; } = "";

        public float TotalCobrado { get; set; } = 0;

        public string Cobrado { get; set; } = "";

        public float Total_Item { get; set; } = 0;

        public int Id_Vehiculo_Custodia { get; set; } = 0;

        public List<Detalle> DetalleOrden { get; set; }

        public string origen { get; set; } = "";

        public string accion { get; set; } = "";



        public int TerminoPago { get; set; } = 0;

        public int Banco { get; set; } = 0;

        public int EmisorTarjeta { get; set; } = 0;

        public string FechaVencimientoTarjeta { get; set; } = "";

        public string NumeroCuenta { get; set; } = "";

        public string PlazoTarjeta { get; set; } = "";

        public string PromocioDA { get; set; } = "";

        public string TipoCuentaBanco { get; set; } = "";

        public string TitularCuenta { get; set; } = "";

        public string TipoEventoUsuario { get; set; } = "";

        public float Total_General { get; set; } = 0;


        public float Iva_Total { get; set; } = 0;

        public float Subtotal { get; set; } = 0;

        public float ValorPromocionDA { get; set; } = 0;


        public string TitularCuentaId { get; set; } = "";

        public string FacturaraId { get; set; } = "";

        public string OrigenConvenioOS { get; set; } = "";


        public string EsFacturaInterna { get; set; } = "";


        public string FechaFacturaInterna { get; set; } = "";



    }
}
