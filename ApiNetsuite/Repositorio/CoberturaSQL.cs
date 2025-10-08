

using ApiNetsuite.Clases;
using ApiNetsuite.DTO.Cobertura;
using ApiNetsuite.DTO.OrdenServicio;
using ApiNetsuite.Repositorio.IRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Numerics;
using System.Reflection;
using System.ServiceModel.Channels;

namespace ApiNetsuite.Repositorio
{
    public class CoberturaSQL : ICoberturaSQL
    {
        private string CadenaConexion;
        private readonly ILogger<CoberturaSQL> log;


        public CoberturaSQL(AccesoDatos cadenaConexion, ILogger<CoberturaSQL> l)
        {
            CadenaConexion = cadenaConexion.CadenaConexionSQL;
            this.log = l;
        }


        private SqlConnection conexion()
        {
            return new SqlConnection(CadenaConexion);
        }


        public static DataSet ActualizarHistorico(string bienid, string ordenid, string numerogeneral, string origen, string docorigen, string productoid, string producto,
                                                  string fechainicio, string fechafin, string plazo, string tipoplazo, string estadoid,
                                                  string fechainicioant, string fechafinant, string estadoinstalacionid, string ejecutivaid, string usuarioid)
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string cadena = MyConfig.GetValue<string>("ConnectionStrings:SQL");
            DataSet ds = new DataSet();
            try
            {
                using SqlConnection connection = new(cadena);
                {
                    using SqlCommand command = new("VENTAS.DBO.VEN_SP_REGISTRA_PIV_NS_HISTORICO", connection);
                    {
                        connection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@ID_BIEN", SqlDbType.Int).Value = bienid;
                        command.Parameters.Add("@ID_OS", SqlDbType.Int).Value = ordenid;
                        command.Parameters.Add("@NUMERO_GENERAL", SqlDbType.VarChar).Value = numerogeneral;
                        command.Parameters.Add("@ORIGEN", SqlDbType.VarChar).Value = origen;
                        command.Parameters.Add("@ID_DOC_ORIGEN", SqlDbType.Int).Value = docorigen;
                        command.Parameters.Add("@ID_PRODUCTO", SqlDbType.Int).Value = productoid;
                        command.Parameters.Add("@PRODUCTO", SqlDbType.VarChar).Value = producto;
                        command.Parameters.Add("@FECHA_INICIO", SqlDbType.VarChar).Value = fechainicio;
                        command.Parameters.Add("@FECHA_FIN", SqlDbType.VarChar).Value = fechafin;
                        command.Parameters.Add("@PLAZO", SqlDbType.Int).Value = plazo;
                        command.Parameters.Add("@TIPO_PLAZO", SqlDbType.VarChar).Value = tipoplazo;
                        command.Parameters.Add("@ID_ESTADO", SqlDbType.VarChar).Value = estadoid;
                        command.Parameters.Add("@FECHA_INICIO_ANT", SqlDbType.VarChar).Value = fechainicioant;
                        command.Parameters.Add("@FECHA_FIN_ANT", SqlDbType.VarChar).Value = fechafinant;
                        command.Parameters.Add("@ID_ESTADO_INSTALACION", SqlDbType.Int).Value = estadoinstalacionid;
                        command.Parameters.Add("@ID_EJECUTIVA", SqlDbType.Int).Value = ejecutivaid;
                        command.Parameters.Add("@ID_USUARIO", SqlDbType.Int).Value = usuarioid;
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


        public static string Cobertura(CoberturaDTO data)
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
                        using SqlCommand command = new SqlCommand("VENTAS.DBO.MIG_SP_PASO_DATO_COBERTURAS", connection);
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Transaction = transaction;
                            command.Parameters.Clear();
                            command.Parameters.Add("@IDCOBERTURA", SqlDbType.Int).Value = data.IdCobertura;
                            command.Parameters.Add("@CODCOBERTURA", SqlDbType.VarChar).Value = data.CodCobertura;
                            command.Parameters.Add("@IDBIEN", SqlDbType.Int).Value = data.IdBien;
                            command.Parameters.Add("@CODBIENSYS", SqlDbType.VarChar).Value = data.CodBienSys;
                            command.Parameters.Add("@IDCLIENTEMONITOREO", SqlDbType.Int).Value = data.Idclientemonitoreo;
                            command.Parameters.Add("@CLIENTEMONITOREO", SqlDbType.VarChar).Value = data.ClienteMonitoreo;
                            command.Parameters.Add("@EMPRESA", SqlDbType.Int).Value = int.Parse(data.Empresa);
                            command.Parameters.Add("@IDCLIENTENS", SqlDbType.Int).Value = int.Parse(data.IdClienteNs);
                            command.Parameters.Add("@IDCLIENTE", SqlDbType.VarChar).Value = data.IdCliente;
                            command.Parameters.Add("@CODITEMSYS", SqlDbType.VarChar).Value = data.CodItemSys;
                            command.Parameters.Add("@NUMEROSERIE", SqlDbType.VarChar).Value = data.NumeroSerie;
                            command.Parameters.Add("@IDESTADOCOBERTURA", SqlDbType.Int).Value = int.Parse(data.IdEstadoCobertura); 
                            command.Parameters.Add("@ESTADOCOBERTURA", SqlDbType.VarChar).Value = data.EstadoCobertura;                       
                            command.Parameters.Add("@PLAZO", SqlDbType.Int).Value = int.Parse(data.Plazo);
                            command.Parameters.Add("@TIPO_PLAZO", SqlDbType.VarChar).Value = data.TipoPlazo;
                            if (data.Accion != "delete" )
                            {
                                command.Parameters.Add("@FECHAINICIO", SqlDbType.DateTime).Value = data.FechaInicio;
                                command.Parameters.Add("@FECHAFINAL", SqlDbType.DateTime).Value = data.FechaFinal;
                                command.Parameters.Add("@FECHACREACIONDISPOSITIVO", SqlDbType.DateTime).Value = data.FechaCreacionDispositivo;
                                command.Parameters.Add("@FECHACORTE", SqlDbType.DateTime).Value = data.FechaCorte;
                            } else  {
                                command.Parameters.Add("@FECHAINICIO", SqlDbType.DateTime).Value = "1900-01-01";
                                command.Parameters.Add("@FECHAFINAL", SqlDbType.DateTime).Value = "1900-01-01";
                                command.Parameters.Add("@FECHACREACIONDISPOSITIVO", SqlDbType.DateTime).Value = "1900-01-01";
                                command.Parameters.Add("@FECHACORTE", SqlDbType.DateTime).Value = "1900-01-01";
                            }
                            command.Parameters.Add("@IDESTADOINSTALACION", SqlDbType.Int).Value = int.Parse(data.IdEstadoInstalacion); 
                            command.Parameters.Add("@ESTADOINSTALACION", SqlDbType.VarChar).Value = data.EstadoInstalacion;
                            command.Parameters.Add("@CODFAMILIAPRODUCTO", SqlDbType.VarChar).Value = data.CodFamiliaProducto;
                            command.Parameters.Add("@IDFAMILIAPRODUCTO", SqlDbType.Int).Value = int.Parse(data.IdFamiliaProducto);
                            command.Parameters.Add("@FAMILIAPRODUCTO", SqlDbType.VarChar).Value = data.FamiliaProducto;
                            command.Parameters.Add("@PLATPX_AMI", SqlDbType.VarChar).Value = data.PlatPxAMi;
                            command.Parameters.Add("@NUMERODISPOSITIVO", SqlDbType.VarChar).Value = data.NumeroDispositivo;
                            command.Parameters.Add("@MODELODISPOSITIVO", SqlDbType.VarChar).Value = data.ModeloDispositivo;
                            command.Parameters.Add("@UNIDAD", SqlDbType.VarChar).Value = data.Unidad;
                            command.Parameters.Add("@IMEI", SqlDbType.VarChar).Value = data.Imei;
                            command.Parameters.Add("@CELULARSIM", SqlDbType.VarChar).Value = data.CelularSim;
                            command.Parameters.Add("@NOCELULARSIMCARG", SqlDbType.VarChar).Value = data.NoCelularSimCarg;
                            command.Parameters.Add("@IP", SqlDbType.VarChar).Value = data.Ip;
                            command.Parameters.Add("@APN", SqlDbType.VarChar).Value = data.APN;
                            command.Parameters.Add("@OPERADORA", SqlDbType.VarChar).Value = data.Operadora;
                            command.Parameters.Add("@VID", SqlDbType.VarChar).Value = data.Vid;
                            command.Parameters.Add("@FIRMWARE", SqlDbType.VarChar).Value = data.Firmware;
                            command.Parameters.Add("@SCRIPT", SqlDbType.VarChar).Value = data.Script;
                            command.Parameters.Add("@SERVIDOR", SqlDbType.VarChar).Value = data.Servidor;
                            command.Parameters.Add("@SERIELOJACK", SqlDbType.VarChar).Value = data.SerieLojack;
                            command.Parameters.Add("@CODIGOACTIVACION", SqlDbType.VarChar).Value = data.CodigoActivacion;
                            command.Parameters.Add("@CODIGORESPUESTA", SqlDbType.VarChar).Value = data.CodigoRespuesta;
                            command.Parameters.Add("@ID_USUARIO", SqlDbType.Int).Value = data.IdUsuario;
                            command.Parameters.Add("@ID_DOCUMENTO", SqlDbType.Int).Value = data.IdDocumento;
                            command.Parameters.Add("@COD_DOCUMENTO", SqlDbType.VarChar).Value = data.CodDocumento;
                            command.Parameters.Add("@COMENTARIO", SqlDbType.VarChar).Value = data.Comentario;
                            command.Parameters.Add("@IDNUMERODISPOSITIVOCHASER", SqlDbType.Int).Value = data.IdNumeroDispositivoChaser;
                            command.Parameters.Add("@IDMODELODISPOSITIVO", SqlDbType.Int).Value = data.IdModeloDispositivo;
                            command.Parameters.Add("@IDTIPODISPOSITIVO", SqlDbType.Int).Value = data.IdTipoDispositivo;
                            command.Parameters.Add("@TIPODISPOSITIVO", SqlDbType.VarChar).Value = data.TipoDispositivo;
                            command.Parameters.Add("@IDUNIDAD", SqlDbType.Int).Value = data.IdUnidad;
                            command.Parameters.Add("@IDOPERADORA", SqlDbType.Int).Value = data.IdOperadora;
                            command.Parameters.Add("@IDFIRMWARE", SqlDbType.Int).Value = data.IdFirmware;
                            command.Parameters.Add("@IDSCRIPT", SqlDbType.Int).Value = data.IdScript;
                            command.Parameters.Add("@IDSERVIDOR", SqlDbType.Int).Value = data.IdServidor;
                            command.Parameters.Add("@IDSERIELOJACK", SqlDbType.Int).Value = data.IdSerieLojack;
                            command.Parameters.Add("@IDESTADOSIM", SqlDbType.Int).Value = data.IdEstadoSim;
                            command.Parameters.Add("@ESTADOSIM", SqlDbType.VarChar).Value = data.EstadoSim;
                            command.Parameters.Add("@IDESTADOLOJACK", SqlDbType.Int).Value = data.IdEstadoLojack;
                            command.Parameters.Add("@ESTADOLOJACK", SqlDbType.VarChar).Value = data.EstadoLojack;
                            command.Parameters.Add("@IDESTADOCHASER", SqlDbType.Int).Value = data.IdEstadoChaser;
                            command.Parameters.Add("@ESTADOCHASER", SqlDbType.VarChar).Value = data.EstadoChaser;
                            command.Parameters.Add("@ICC", SqlDbType.VarChar).Value = data.ICC;
                            command.Parameters.Add("@NOMBREDISPOSITIVO", SqlDbType.VarChar).Value = data.NombreDispositivo;
                            command.Parameters.Add("@UBICACION", SqlDbType.VarChar).Value = data.Ubicacion;
                            command.Parameters.Add("@ACCION", SqlDbType.VarChar).Value = data.Accion;
                            command.Parameters.Add("@IDBIENDT", SqlDbType.Int).Value = data.IdBienDt;
                            command.Parameters.Add("@INACTIVODT", SqlDbType.VarChar).Value = data.InacitvoDt;
                            command.Parameters.Add("@IDTIPOSIM", SqlDbType.Int).Value = data.IdTipoSim;
                            command.Parameters.Add("@TIPOSIM", SqlDbType.VarChar).Value = data.TipoSim;
                            command.Parameters.Add("@IDESTADOCONCILIACION", SqlDbType.Int).Value = data.IdEstadoConciliacion;
                            command.Parameters.Add("@ESTADOCONCILIACION", SqlDbType.VarChar).Value = data.EstadoConciliacion;
                            command.Parameters.Add("@SIMCARD", SqlDbType.Int).Value = data.SimCard;
                            command.Parameters.Add("@IDSIM", SqlDbType.Int).Value = data.IdSim;
                            command.ExecuteNonQuery();
                        }
                        //transaction.Rollback();
                        transaction.Commit();                                      
                        resultado = "OK";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        resultado = ex.Message.ToString();
                        string Datos = LogDB.LogData(resultado, JsonConvert.SerializeObject(data), "Cobertura");
                        //throw;
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
