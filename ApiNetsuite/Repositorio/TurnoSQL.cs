using ApiNetsuite.Repositorio.IRepository;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;

namespace ApiNetsuite.Repositorio
{
    public class TurnoSQL : ITurnoSQL
    {
        private string CadenaConexion;
        private readonly ILogger<TurnoSQL> log;


        public TurnoSQL(AccesoDatos cadenaConexion, ILogger<TurnoSQL> l)
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
