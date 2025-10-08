using System.Collections.Generic;
using System.Net;

namespace ApiNetsuite.Modelo
{
    public class RespuestAPI
    {
        public RespuestAPI()
        {
            Messages = new List<string>();
        }

        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string> Messages { get; set; }

        public string index { get; set; } = "0";

        public string pages { get; set; } = "0";

        public object Result { get; set; }
    }
}
