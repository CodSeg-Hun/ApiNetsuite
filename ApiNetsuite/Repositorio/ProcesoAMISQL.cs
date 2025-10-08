using ApiNetsuite.Repositorio.IRepository;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;

namespace ApiNetsuite.Repositorio
{
    public class ProcesoAMISQL : IProcesoAMISQL
    {
        private string CadenaConexion;
        private readonly ILogger<ProcesoAMISQL> log;


        public ProcesoAMISQL(AccesoDatos cadenaConexion, ILogger<ProcesoAMISQL> l)
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
