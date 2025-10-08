namespace ApiNetsuite.Modelo
{
    public class attributeEntidad
    {

        public int AttributeId { get; set; }

        public string Valor { get; set; }

        public attributeEntidadCollection attributeEntidadCollection { get; internal set; }
    }
}
