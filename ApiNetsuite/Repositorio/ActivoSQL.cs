using ApiNetsuite.Repositorio.IRepository;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;

namespace ApiNetsuite.Repositorio
{
    public class ActivoSQL : IActivoSQL
    {
        private string CadenaConexion;
        private readonly ILogger<ActivoSQL> log;


        public ActivoSQL(AccesoDatos cadenaConexion, ILogger<ActivoSQL> l)
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
