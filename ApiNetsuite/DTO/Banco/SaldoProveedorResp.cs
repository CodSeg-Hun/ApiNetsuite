

using System.Collections.Generic;
using System.Net;

namespace ApiNetsuite.DTO.Banco
{
    public class SaldoProveedorResp
    {

        public SaldoProveedorResp()
        {
            Messages = new List<string>();
        }

        public HttpStatusCode StatusCode { get; set; }

        public bool IsSuccess { get; set; } = true;

        public List<string> Messages { get; set; }

        public string index { get; set; } = "0";

        public string pages { get; set; } = "0";

        public List<ProveedorResp> results { get; set; }
    }

    public class ProveedorResp
    {
        public string ID_EMPRESA { get; set; }

        public string Empresa { get; set; }

        public string ID_oficina { get; set; }

        public string Oficina { get; set; }

        public string ID_Sucursal { get; set; }

        public string Sucursal { get; set; }

        public string Grupo_Cuenta { get; set; }

        public string Numero_Cuenta { get; set; }

        public string ID_Cuenta_Netsuite { get; set; }

        public string Descripcion_Cuenta { get; set; }

        public string ID_Transaccion { get; set; }

        public string Descripcion_Transaccion { get; set; }

        public string Tipo_Transaccion { get; set; }

        public string Numero_Transaccion { get; set; }

        public string Debito { get; set; }

        public string Credito { get; set; }

        public string Importe { get; set; }

        public string Fecha_Movimiento { get; set; }

        public string ID_Proveedor { get; set; }

        public string ID_Proveedor_Netsuite { get; set; }

        public string nombre_Proveedor { get; set; }

        public string Nota { get; set; }

        public string Numero_Documento { get; set; }

        public string Estado { get; set; }

        public string ReferenciaTransaccion { get; set; }

        public string id_clasificacion { get; set; }


    }

}
