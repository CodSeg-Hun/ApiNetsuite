using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using System;

namespace ApiNetsuite.Clases
{
    public class LogDB
    {


        public static string LogData(string respuesta, string trama, string origen)
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string cadena = MyConfig.GetValue<string>("ConnectionStrings:SQL");
            //DataSet ds = new DataSet();
            string resultado = "";
            try
            {
                using (SqlConnection connection = new SqlConnection(cadena))
                {
                    using (SqlCommand command = new SqlCommand("dbo.sp_automatico_log_data", connection))
                    {
                        connection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@TIPO", SqlDbType.VarChar, 100).Value = origen;
                        command.Parameters.Add("@TRAZA", SqlDbType.VarChar).Value = trama;
                        command.Parameters.Add("@RESPUESTA", SqlDbType.VarChar).Value = respuesta;
                        command.Parameters.Add("@PROCESO", SqlDbType.Int).Value = 1;
                        var result = command.ExecuteReader();
                        //ds.Load(result, LoadOption.OverwriteChanges, "Table");
                        connection.Close();
                    }
                    resultado = "OK";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            return resultado;
        }
    }
}
