using ApiNetsuite.DTO.Turno;
using System.Collections.Generic;

namespace ApiNetsuite.DTO.Convenio
{
    public class RespDPWorld
    {

        public List<results> results { get; set; }
    }


    public class results
    {

        public string idGenerado { get; set; }

        public string mensaje { get; set; }
    }

}
