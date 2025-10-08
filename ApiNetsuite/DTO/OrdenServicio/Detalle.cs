namespace ApiNetsuite.DTO.OrdenServicio
{
    public class Detalle
    {
        public int Secuencia { get; set; } = 0;

        public int IdItem { get; set; } = 0;

        public string CodigoItem { get; set; } = "";

        public string ItemDescripcion { get; set; } = "";

        public string IdItemRelacion { get; set; } = "";

        public int Clase { get; set; } = 0;

        public string ADP { get; set; } = "";

        public string Tipo_Transaccion { get; set; } = "";

        public string Grupo_Producto { get; set; } = "";

        public float PrecioBase { get; set; } = 0;

        public float Total_Item { get; set; } = 0;

        public float Iva { get; set; } = 0;

        public float PrecioUnitario { get; set; } = 0;

        public float PrecioCliente { get; set; } = 0;

        public string OrdenFabricacion { get; set; } = "";

        public int IdTipoPlazo { get; set; } = 0;

        public string TipoPlazo { get; set; } = "";

        public int Plazo { get; set; } = 0;

        public string TipoArticulo { get; set; } = "";

        public string IdIvaDescripcion { get; set; } = "0";

        public int IdIva { get; set; } = 0;

        public int PorcentajeIva { get; set; } = 0;

        public string Descuento_General { get; set; } = "";

        public string PPS { get; set; } = "";

        public float ValorPromocion { get; set; } = 0;

        public float TasaDsctoItem { get; set; } = 0;

        public float TasaDsctoGeneral { get; set; } = 0;

        public string Chequeado { get; set; } = "";

        public string Fecha_Chequeo { get; set; } = "";

        public string Hora_Chequeo { get; set; } = "";

        public int Id_OT { get; set; } = 0;

        public string Codigo_Cliente_Nuevo { get; set; } = "";

        public int Codigo_Cliente_Nuevo_Ns { get; set; } = 0;

        public int IdTecnicoChequeo { get; set; } = 0;

        public string TecnicoChequeo { get; set; } = "";



    }
}
