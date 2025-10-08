using ApiNetsuite.DTO.Activo;
using System;
using System.Collections.Generic;

namespace ApiNetsuite.DTO.Convenio
{
    public class RespuestaSaludErrorDTO
    {

        public Boolean status { get; set; } = false;

        public string message { get; set; } = "";

        public object data { get; set; }

        public ErrorResp error { get; set; }


    }


    public class ErrorResp
    {
        public string code { get; set; }

        public string title { get; set; }

        public string message { get; set; }

    }



    //public class errors
    //{
    //    public string error { get; set; }

    //    public string type { get; set; }

    //    public string title { get; set; }

    //    public string status { get; set; }

    //    public string traceId { get; set; }

    //}

    //public class error
    //{

    //}
}
