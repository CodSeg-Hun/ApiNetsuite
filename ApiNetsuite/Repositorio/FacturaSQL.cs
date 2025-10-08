using ApiNetsuite.Repositorio.IRepository;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;

namespace ApiNetsuite.Repositorio
{
    public class FacturaSQL : IFacturaSQL
    {
        private string CadenaConexion;
        private readonly ILogger<FacturaSQL> log;


        public FacturaSQL(AccesoDatos cadenaConexion, ILogger<FacturaSQL> l)
        {
            CadenaConexion = cadenaConexion.CadenaConexionSQL;
            this.log = l;
        }


        private SqlConnection conexion()
        {
            return new SqlConnection(CadenaConexion);
        }
    }
}
