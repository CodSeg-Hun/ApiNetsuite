using ApiNetsuite.Clases;
using ApiNetsuite.DTO.OrdenServicio;
using ApiNetsuite.Repositorio.IRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data;
using System;
using System.Data.SqlClient;
using ApiNetsuite.DTO.PX;
using Microsoft.Extensions.Options;
using ApiNetsuite.DTO.Bien;

namespace ApiNetsuite.Repositorio
{
    public class ProcesoPXSQL : IProcesoPXSQL
    {
        private string CadenaConexion;
        private readonly ILogger<ProcesoPXSQL> log;


        public ProcesoPXSQL(AccesoDatos cadenaConexion, ILogger<ProcesoPXSQL> l)
        {
            CadenaConexion = cadenaConexion.CadenaConexionSQL;
            this.log = l;
        }


        private SqlConnection conexion()
        {
            return new SqlConnection(CadenaConexion);
        }


        public static DataSet ObtenerXml(GeneralPXDTO data, string proceso)
        {

            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string cadena = MyConfig.GetValue<string>("ConnectionStrings:SQL");
            DataSet ds = new DataSet();
            try
            {
                using SqlConnection connection = new SqlConnection(cadena);
                {
                    using SqlCommand command = new SqlCommand("Extranet.EXT_SP_NS_HUNTER_PLATAFORMA_PX", connection);
                    {
                        connection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@OPCION", SqlDbType.VarChar).Value = proceso;
                        command.Parameters.Add("@SECUENCIA_LOG", SqlDbType.VarChar).Value = data.secuencia;
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


        public static string GuardarRespuesta(string proceso, string sucuencial, string estado, string mensaje)
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string cadena = MyConfig.GetValue<string>("ConnectionStrings:SQL");
            //DataSet ds = new DataSet();
            string resultado = "";
            try
            {
                using SqlConnection connection = new SqlConnection(cadena);
                {
                    connection.Open();
                    SqlTransaction transaction = null;
                    try
                    {
                        // Iniciar una transacción
                        transaction = connection.BeginTransaction();
                        using SqlCommand command = new SqlCommand("Extranet.EXT_SP_NS_HUNTER_PLATAFORMA_PX", connection);
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Transaction = transaction;
                            command.Parameters.Clear();
                            command.Parameters.Add("@OPCION", SqlDbType.VarChar).Value = proceso;
                            command.Parameters.Add("@SECUENCIA_LOG", SqlDbType.VarChar).Value = sucuencial;
                            command.Parameters.Add("@ESTADO", SqlDbType.VarChar).Value = estado;
                            command.Parameters.Add("@COMENTARIO", SqlDbType.VarChar).Value = mensaje;
                            command.ExecuteNonQuery();
                        }
                        //transaction.Rollback();
                        transaction.Commit();
                        resultado = "OK";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                resultado = ex.Message.ToString();
                throw;
            }
            return resultado;
        }

    }
}
