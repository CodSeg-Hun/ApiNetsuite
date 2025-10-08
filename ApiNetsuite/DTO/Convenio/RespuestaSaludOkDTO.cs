using System;

namespace ApiNetsuite.DTO.Convenio
{
    public class RespuestaSaludOkDTO
    {
        public Boolean status { get; set; } = false;

        public string message { get; set; } = "";


        public DataResp data { get; set; }

        public object error { get; set; }

        //public List<DataResp> data { get; set; }

        //public List<ErrorResp> error { get; set; }


    }

    public class DataResp
    {
        public string client_id { get; set; }

        public string contract_id { get; set; }

    }

}
