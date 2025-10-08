namespace ApiNetsuite.Clases
{
    public class QueryParameter
    {

        // Private name As String = Nothing
        // Private value As String = Nothing


        // Public Sub New(ByVal name As String, ByVal value As String)
        // Me.name = name
        // Me.value = value
        // End Sub

        // Public Property Name As String
        // Get
        // Return Name
        // End Get
        // End Property

        // Public Property Value As String
        // Get
        // Return Value
        // End Get
        // End Property

        private string _name;
        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        private string _valor;
        public string valor
        {
            get
            {
                return _valor;
            }
            set
            {
                _valor = value;
            }
        }


        public QueryParameter(string name, string valor)
        {
            this.name = name;
            this.valor = valor;
        }
    }
}
