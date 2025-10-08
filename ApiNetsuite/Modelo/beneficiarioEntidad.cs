using System;

namespace ApiNetsuite.Modelo
{
    public class beneficiarioEntidad
    {
        public string Identificacion { get; set; }

        public string Mail { get; set; }

        public string Celular { get; set; }

        public Int32 Relacion { get; set; }

        public beneficiarioEntidadCollection beneficiarioEntidadCollection { get; internal set; }

    }
}
