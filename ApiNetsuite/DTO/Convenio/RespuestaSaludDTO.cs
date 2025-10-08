using System;
using System.Collections.Generic;

namespace ApiNetsuite.DTO.Convenio
{
    public class RespuestaSaludDTO
    {

        public Boolean status { get; set; } = false;

        public string message { get; set; } = "";

        public string client_id { get; set; } = "";

        public string contract_id { get; set; } = "";

        public string data { get; set; } = "";

        public string code { get; set; } = "";


    }


}
