

using System.Collections.Generic;
using System.Net;

namespace ApiNetsuite.DTO.Banco
{
    public class SaldoBancoResp
    {
        public SaldoBancoResp()
        {
           Messages = new List<string>();
        }

        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string> Messages { get; set; }

        public string index { get; set; } = "0";

        public string pages { get; set; } = "0";

        public List<SaldoResp> results { get; set; }


    }


}
