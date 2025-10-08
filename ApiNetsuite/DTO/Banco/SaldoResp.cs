
using System.Collections.Generic;

namespace ApiNetsuite.DTO.Banco
{
    public class SaldoResp
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

        public string ID_Clasificacion { get; set; }

        public string Numero_Transaccion { get; set; }

        public string Clasificacion { get; set; }

        public string Debito { get; set; }

        public string Credito { get; set; }

        public string Importe { get; set; }

        public string Fecha_Movimiento { get; set; }

        public string Tipo_Registro { get; set; }

        //public string index { get; set; }

    }
}
