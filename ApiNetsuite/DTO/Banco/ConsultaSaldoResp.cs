namespace ApiNetsuite.DTO.Banco
{
    public class ConsultaSaldoResp
    {

        public string ID_EMPRESA { get; set; }

        public string Empresa { get; set; }

        public string Grupo_Cuenta { get; set; }

        public string Numero_Cuenta { get; set; }

        public string ID_Cuenta_Netsuite { get; set; }

        public string Descripcion { get; set; }

        public string Saldo_Actual { get; set; }

        public string ID_Clasificacion { get; set; }

        public string Fecha_Consulta { get; set; }



    }
}
