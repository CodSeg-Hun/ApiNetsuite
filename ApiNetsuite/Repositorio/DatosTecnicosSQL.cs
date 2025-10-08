using ApiNetsuite.Clases;
using ApiNetsuite.DTO.OrdenServicio;
using ApiNetsuite.Repositorio.IRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data;
using System;
using System.Data.SqlClient;
using ApiNetsuite.DTO.DatosTecnicos;
using System.Security.Cryptography;

namespace ApiNetsuite.Repositorio
{
    public class DatosTecnicosSQL : IDatosTecnicosSQL
    {

        private string CadenaConexion;
        private readonly ILogger<ClienteSQL> log;

        public DatosTecnicosSQL(AccesoDatos cadenaConexion, ILogger<ClienteSQL> l)
        {
            CadenaConexion = cadenaConexion.CadenaConexionSQL;
            this.log = l;
        }


        private SqlConnection conexion()
        {
            return new SqlConnection(CadenaConexion);
        }


        public static string DatosTecnicos(DatosTecnicoDTO data)
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
                        using SqlCommand command = new SqlCommand("INVENTARIO.DBO.MIG_SP_PASO_DATO_TECNICO", connection);
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Transaction = transaction;
                            command.Parameters.Clear();                          
                            command.Parameters.Add("@PROCESO", SqlDbType.VarChar).Value = "200";
                            command.Parameters.Add("@NUMEROSERIE", SqlDbType.VarChar).Value = data.datoTecnicoID;
                            command.Parameters.Add("@NOMBREDISPOSITIVO", SqlDbType.VarChar).Value = data.nombre;
                            command.Parameters.Add("@IDENSAMBLAJE", SqlDbType.Int).Value = data.ensamblaje_ID;
                            command.Parameters.Add("@ENSAMBLAJE", SqlDbType.VarChar).Value = data.ensamblaje_Text;
                            command.Parameters.Add("@UBICACION", SqlDbType.VarChar).Value = data.ubicacion;
                            command.Parameters.Add("@IDBIEN", SqlDbType.Int).Value = data.bien;
                            command.Parameters.Add("@INACTIVODT", SqlDbType.VarChar).Value = data.inactivo;
                            command.Parameters.Add("@IDNUMERODISPOSITIVOCHASER", SqlDbType.Int).Value = data.serieDispositivo_ID;
                            command.Parameters.Add("@NUMERODISPOSITIVO", SqlDbType.VarChar).Value = data.serieDispositivo_Text;
                            command.Parameters.Add("@IDMODELODISPOSITIVO", SqlDbType.Int).Value = data.modelo_ID;
                            command.Parameters.Add("@MODELODISPOSITIVO", SqlDbType.VarChar).Value = data.modelo_Text;
                            command.Parameters.Add("@IMEI", SqlDbType.VarChar).Value = data.imei;
                            command.Parameters.Add("@IDFIRMWARE", SqlDbType.Int).Value = data.firmware_ID;
                            command.Parameters.Add("@FIRMWARE", SqlDbType.VarChar).Value = data.firmware_Text;
                            command.Parameters.Add("@IDSCRIPT", SqlDbType.Int).Value = data.script_ID;
                            command.Parameters.Add("@SCRIPT", SqlDbType.VarChar).Value = data.script_Text;
                            command.Parameters.Add("@IDSERVIDOR", SqlDbType.Int).Value = data.servidor_ID;
                            command.Parameters.Add("@SERVIDOR", SqlDbType.VarChar).Value = data.servidor_Text;
                            command.Parameters.Add("@IDTIPODISPOSITIVO", SqlDbType.Int).Value = data.tipoDispositivo_ID;
                            command.Parameters.Add("@TIPODISPOSITIVO", SqlDbType.VarChar).Value = data.tipoDispositivo_Text;
                            command.Parameters.Add("@IDUNIDAD", SqlDbType.Int).Value = data.unidad_ID;
                            command.Parameters.Add("@UNIDAD", SqlDbType.VarChar).Value = data.unidad_Text;
                            command.Parameters.Add("@VID", SqlDbType.VarChar).Value = data.vid;
                            command.Parameters.Add("@MACADDRESS", SqlDbType.VarChar).Value = data.macAddress;
                            command.Parameters.Add("@SN", SqlDbType.VarChar).Value = data.sn;
                            command.Parameters.Add("@IDESTADOCHASER", SqlDbType.Int).Value = data.estadoDispositivo_ID;
                            command.Parameters.Add("@ESTADOCHASER", SqlDbType.VarChar).Value = data.estadoDispositivo_Text;
                            command.Parameters.Add("@IDSIM", SqlDbType.Int).Value = data.celularSimCard_ID;
                            command.Parameters.Add("@CELULARSIM", SqlDbType.VarChar).Value = data.celularSimCard_Text;
                            command.Parameters.Add("@IDOPERADORA", SqlDbType.Int).Value = data.operadora_ID;
                            command.Parameters.Add("@OPERADORA", SqlDbType.VarChar).Value = data.operadora_Text;
                            command.Parameters.Add("@IP", SqlDbType.VarChar).Value = data.ip;
                            command.Parameters.Add("@APN", SqlDbType.VarChar).Value = data.apn;
                            command.Parameters.Add("@NOCELULARSIMCARG", SqlDbType.VarChar).Value = data.noCelularSim;
                            command.Parameters.Add("@ICC", SqlDbType.VarChar).Value = data.icc;
                            command.Parameters.Add("@IDESTADOSIM", SqlDbType.Int).Value = data.estadoSimCard_ID;
                            command.Parameters.Add("@ESTADOSIM", SqlDbType.VarChar).Value = data.estadoSimCard_Text;
                            command.Parameters.Add("@IDDISPOSITIVOLOJACK", SqlDbType.Int).Value = data.serieDispositivoLojack_ID;
                            command.Parameters.Add("@DISPOSITIVOLOJACK", SqlDbType.VarChar).Value = data.serieDispositivoLojack_Text;
                            command.Parameters.Add("@CODIGOACTIVACION", SqlDbType.VarChar).Value = data.codigoActivacion;
                            command.Parameters.Add("@CODIGORESPUESTA", SqlDbType.VarChar).Value = data.codigoRespuesta;
                            command.Parameters.Add("@IDESTADOLOJACK", SqlDbType.Int).Value = data.estadoLojack_ID;
                            command.Parameters.Add("@ESTADOLOJACK", SqlDbType.VarChar).Value = data.estadoLojack_Text;
                            command.Parameters.Add("@EMPRESA", SqlDbType.Int).Value = data.empresa_ID;
                            command.Parameters.Add("@EMPRESANOMBRE", SqlDbType.VarChar).Value = data.empresa_Text;
                            command.Parameters.Add("@IDCLIENTENS", SqlDbType.Int).Value = data.cliente_ID;
                            command.Parameters.Add("@IDCLIENTE", SqlDbType.VarChar).Value = data.cliente_Identificacion;
                            command.Parameters.Add("@FECHAREGISTRO", SqlDbType.DateTime).Value = data.fecha;
                            command.Parameters.Add("@USUARIOREGISTRO", SqlDbType.Int).Value = data.id_usuario;
                            command.Parameters.Add("@ACCION", SqlDbType.VarChar).Value = data.accion;
                            command.ExecuteNonQuery();                         
                        }
                        transaction.Commit();
                        //transaction.Rollback();
                        resultado = "OK";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        resultado = ex.Message.ToString();
                        string Datos = LogDB.LogData(resultado, JsonConvert.SerializeObject(data), "DatosTecnicos");
                       // throw;
                    }
                }
            }
            catch (Exception ex)
            {
                resultado = ex.Message.ToString();
                //throw;
            }
            return resultado;
        }
    }
}
