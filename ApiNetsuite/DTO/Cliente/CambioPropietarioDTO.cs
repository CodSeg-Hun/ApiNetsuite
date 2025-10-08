using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace ApiNetsuite.DTO.Cliente
{
    public class CambioPropietarioDTO 
    {
        
        public string accion { get; set; }

        public string cliente { get; set; }

        public string bien { get; set; }

        public string fecha { get; set; }

        public string representanteVenta { get; set; }

        public string centroCosto { get; set; }

        public string ejecutivagestion { get; set; }

        public string ejecutivareferencia { get; set; }

        public string ubicacion { get; set; }

        public string terminoPago { get; set; }

        public string aprobacionventa { get; set; }

        public string aprobacioncartera { get; set; }

        public string formapago { get; set; }

        public string banco { get; set; } = "";

        public string titularcuenta { get; set; } = "";

        public string promocionDA { get; set; } = "";

        public string valorpromocionDA { get; set; } = "";

        public string numerocta { get; set; } = "";

        public string plazo { get; set; } = "";

        public bool cambioPropietarioConvenio { get; set; }

        public string novedades { get; set; }

        public string consideracionTecnica { get; set; }

        public string origen { get; set; }

        public string convenio { get; set; }

        public string[]? servicios { get; set; }
        // servicios Servicios { get; set; }

        //public List<T> servicios { get; set; }

        public string clientefacturar { get; set; }

        public List<GenItems> items { get; set; }


        public string clase { get; set; }


        public string nota { get; set; }


    }
}
