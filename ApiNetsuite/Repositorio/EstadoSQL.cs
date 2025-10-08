using ApiNetsuite.Modelo;
using ApiNetsuite.Repositorio.IRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Numerics;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ApiNetsuite.Repositorio
{
    public class EstadoSQL : IEstadoSQL
    {
        private string CadenaConexion;
        private readonly ILogger<EstadoSQL> log;

        public EstadoSQL(AccesoDatos cadenaConexion, ILogger<EstadoSQL> l)
        {
            CadenaConexion = cadenaConexion.CadenaConexionSQL;
            this.log = l;
        }


        private SqlConnection conexion()
        {
            return new SqlConnection(CadenaConexion);
        }


        public static DataSet Cnstdatos(string opcion, string datSelect, string datFrom, string datWhere)
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string cadena = MyConfig.GetValue<string>("ConnectionStrings:SQL");
            DataSet ds = new DataSet();
            try
            {
                using SqlConnection connection = new SqlConnection(cadena);
                {
                    using SqlCommand command = new SqlCommand("GENERAL.DBO.GEN_SP_PASO_DATOS_GENERALES ", connection);
                    {
                        connection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@OPCION", SqlDbType.VarChar).Value = opcion;
                        command.Parameters.Add("@SELECT", SqlDbType.VarChar).Value = datSelect;
                        command.Parameters.Add("@FROM", SqlDbType.VarChar).Value = datFrom;
                        command.Parameters.Add("@WHERE", SqlDbType.VarChar).Value = datWhere;
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



        public static DataSet CnstEstado(string opcion, string vehiculo, string placa, string idmarca,string marca, string idmodelo,string modelo,string chasis,string motor, 
                                         string color, string anio,string tipo,string idfamiliaProducto,string familiaProducto, string idmarcaDispositivo,string marcaDispositivo, string idmodeloDispositivo,
                                         string modeloDispositivo,string serieDispositivo,string vid,string imeiDispositivo,string direccionMac,string serieSim,string numeroCelularSim,string operadoraSim,
                                         string estadoFamiliaProducto, string parametroproducto,string nemonicoUsuario, string numeroorden)
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
                        command.Parameters.Add("@OPCION", SqlDbType.VarChar).Value = opcion;
                        command.Parameters.Add("@VEHICULO", SqlDbType.VarChar).Value = vehiculo;
                        command.Parameters.Add("@PLACA", SqlDbType.VarChar).Value = placa;
                        command.Parameters.Add("@ID_MARCA", SqlDbType.VarChar).Value = idmarca;
                        command.Parameters.Add("@MARCA", SqlDbType.VarChar).Value = marca;
                        command.Parameters.Add("@ID_MODELO", SqlDbType.VarChar).Value = idmodelo;
                        command.Parameters.Add("@MODELO", SqlDbType.VarChar).Value = modelo;
                        command.Parameters.Add("@CHASIS", SqlDbType.VarChar).Value = chasis;
                        command.Parameters.Add("@MOTOR", SqlDbType.VarChar).Value = motor;
                        command.Parameters.Add("@COLOR", SqlDbType.VarChar).Value = color;
                        command.Parameters.Add("@ANIO", SqlDbType.VarChar).Value = anio;
                        command.Parameters.Add("@TIPO", SqlDbType.VarChar).Value = tipo;
                        command.Parameters.Add("@ID_FAMILIA_PROD", SqlDbType.VarChar).Value = idfamiliaProducto;
                        command.Parameters.Add("@FAMILIA_PROD", SqlDbType.VarChar).Value = familiaProducto;
                        command.Parameters.Add("@ID_MARCA_DIS", SqlDbType.VarChar).Value = idmarcaDispositivo;
                        command.Parameters.Add("@MARCA_DIS", SqlDbType.VarChar).Value = marcaDispositivo;
                        command.Parameters.Add("@ID_MODELO_DIS", SqlDbType.VarChar).Value = idmodeloDispositivo;
                        command.Parameters.Add("@MODELO_DIS", SqlDbType.VarChar).Value = modeloDispositivo;
                        command.Parameters.Add("@SERIE_DISPOSITIVO", SqlDbType.VarChar).Value = serieDispositivo;
                        command.Parameters.Add("@VID", SqlDbType.VarChar).Value = vid;
                        command.Parameters.Add("@IMEI", SqlDbType.VarChar).Value = imeiDispositivo;
                        command.Parameters.Add("@MAC_ADRESS", SqlDbType.VarChar).Value = direccionMac;
                        command.Parameters.Add("@TELEFONO_SIM", SqlDbType.VarChar).Value = numeroCelularSim;
                        command.Parameters.Add("@OPERADORA_SIM", SqlDbType.VarChar).Value = operadoraSim;
                        command.Parameters.Add("@ESTADO", SqlDbType.VarChar).Value = estadoFamiliaProducto;
                        command.Parameters.Add("@SERIE_SIM", SqlDbType.VarChar).Value = serieSim;
                        command.Parameters.Add("@USUARIO_ID", SqlDbType.VarChar).Value = nemonicoUsuario;
                        command.Parameters.Add("@ORDEN_SERVICIO", SqlDbType.VarChar).Value = numeroorden;
                        command.Parameters.Add("@METODO", SqlDbType.VarChar).Value = "ActualizarEstado";
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


        public static DataSet CnstConsultar(string opcion)
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
                        command.Parameters.Add("@OPCION", SqlDbType.VarChar).Value = opcion;
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


        public static DataSet Cnstcadena(string opcion, string idcadena)
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string cadena = MyConfig.GetValue<string>("ConnectionStrings:SQL");
            DataSet ds = new DataSet();
            try
            {
                using SqlConnection connection = new SqlConnection(cadena);
                {
                    using SqlCommand command = new SqlCommand("Extranet.EXT_SP_NS_HUNTER_CADENA_PARAMETRO", connection);
                    {
                        connection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@OPCION", SqlDbType.VarChar).Value = opcion;
                        command.Parameters.Add("@ID_PARAMETRO", SqlDbType.BigInt).Value = idcadena;
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
