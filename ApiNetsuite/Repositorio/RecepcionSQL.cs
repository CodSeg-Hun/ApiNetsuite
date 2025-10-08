using ApiNetsuite.Repositorio.IRepository;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;

namespace ApiNetsuite.Repositorio
{
    public class RecepcionSQL : IRecepcionSQL
    {
        private string CadenaConexion;
        private readonly ILogger<RecepcionSQL> log;


        public RecepcionSQL(AccesoDatos cadenaConexion, ILogger<RecepcionSQL> l)
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
