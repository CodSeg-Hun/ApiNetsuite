using ApiNetsuite.Repositorio.IRepository;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;

namespace ApiNetsuite.Repositorio
{
    public class BancoSQL : IBancoSQL
    {
        private string CadenaConexion;
        private readonly ILogger<BancoSQL> log;


        public BancoSQL(AccesoDatos cadenaConexion, ILogger<BancoSQL> l)
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
