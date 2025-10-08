namespace ApiNetsuite.Clases
{
 
    using System.Collections.Generic;
   

    public class QueryParameterComparer : List<QueryParameter>
    {
        // Inherits IComparer(Of QueryParameter)

        public int Compare(QueryParameter x, QueryParameter y)
        {
            if (x.name == y.name)
                return string.Compare(x.valor, y.valor);
            else
                return string.Compare(x.name, y.name);
        }
    }
}
