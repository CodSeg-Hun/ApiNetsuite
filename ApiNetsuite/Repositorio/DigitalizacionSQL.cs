using ApiNetsuite.Repositorio.IRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using System;
using System.Data.SqlClient;

namespace ApiNetsuite.Repositorio
{
    public class DigitalizacionSQL : IDigitalizacionSQL
    {

        private string CadenaConexion;
        private readonly ILogger<DigitalizacionSQL> log;


        public DigitalizacionSQL(AccesoDatos cadenaConexion, ILogger<DigitalizacionSQL> l)
        {
            CadenaConexion = cadenaConexion.CadenaConexionSQL;
            this.log = l;
        }


        private SqlConnection conexion()
        {
            return new SqlConnection(CadenaConexion);
        }


        public static DataSet ConsultarDocumento(string opcion, string codigo, string proveedor, string referencia)
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string cadena = MyConfig.GetValue<string>("ConnectionStrings:SQL");
            DataSet ds = new DataSet();
            try
            {
                using SqlConnection connection = new (cadena);
                {
                    using SqlCommand command = new ("Documentos.DOC_SP_Documento_Por_Proceso", connection);
                    {
                        connection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@OPCION", SqlDbType.Int).Value = opcion;
                        command.Parameters.Add("@CODIGO", SqlDbType.VarChar).Value = codigo;
                        command.Parameters.Add("@COD_PROVEEDOR", SqlDbType.VarChar).Value = proveedor;
                        command.Parameters.Add("@REFERENCIA", SqlDbType.VarChar).Value = referencia;
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


        public static DataSet ActualizarDocumento(string opcion, string codigo, string proveedor, string referencia,
                                                  string codigoEscaneo, string numeroDocumento, string serie, string usuario)
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string cadena = MyConfig.GetValue<string>("ConnectionStrings:SQL");
            DataSet ds = new DataSet();
            try
            {
                using SqlConnection connection = new (cadena);
                {
                    using SqlCommand command = new ("Documentos.DOC_SP_Documento_Por_Proceso", connection);
                    {
                        connection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@OPCION", SqlDbType.Int).Value = opcion;
                        command.Parameters.Add("@CODIGO", SqlDbType.VarChar).Value = codigo;
                        command.Parameters.Add("@COD_PROVEEDOR", SqlDbType.VarChar).Value = proveedor;
                        command.Parameters.Add("@REFERENCIA", SqlDbType.VarChar).Value = referencia;
                        command.Parameters.Add("@CODIGO_ESCANEO", SqlDbType.Int).Value = codigoEscaneo;
                        command.Parameters.Add("@NUM_DOCUMENTO", SqlDbType.VarChar).Value = numeroDocumento;
                        command.Parameters.Add("@NUM_SERIE", SqlDbType.VarChar).Value = serie;
                        command.Parameters.Add("@USUARIO", SqlDbType.VarChar).Value = usuario;
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
