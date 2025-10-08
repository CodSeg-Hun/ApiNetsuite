using ApiNetsuite.Repositorio.IRepository;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace ApiNetsuite.Repositorio
{
    public class DiarioSQL : IDiarioSQL
    {
        private string CadenaConexion;
        private readonly ILogger<DiarioSQL> log;


        public DiarioSQL(AccesoDatos cadenaConexion, ILogger<DiarioSQL> l)
        {
            CadenaConexion = cadenaConexion.CadenaConexionSQL;
            this.log = l;
        }


        private SqlConnection conexion()
        {
            return new SqlConnection(CadenaConexion);
        }


        public static DataSet CnstDiarios(string secuenciaid, string proceso)
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string cadena = MyConfig.GetValue<string>("ConnectionStrings:SQL");
            DataSet ds = new DataSet();
            try
            {
                using SqlConnection connection = new (cadena);
                {
                    using SqlCommand command = new("CRMDATOS.DBO.NTS_SP_PASO_INFORMACION_ASIENTO_DIARIO", connection);
                    {
                        connection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@SECUENCIA_ID", SqlDbType.VarChar).Value = secuenciaid;
                        command.Parameters.Add("@PROCESO", SqlDbType.VarChar).Value = proceso;
                        var result = command.ExecuteReader();
                        ds.Load(result, LoadOption.OverwriteChanges, "Table");
                        connection.Close();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return ds;
        }
   

    }
}
 