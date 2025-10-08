using ApiNetsuite.Clases;
using ApiNetsuite.DTO.Bien;
using ApiNetsuite.DTO.Cliente;
using ApiNetsuite.DTO.Cobertura;
using ApiNetsuite.Repositorio.IRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nancy.Session;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Numerics;
using System.Reflection.PortableExecutable;
using System.Reflection;
using System.Security.Claims;
using System.Text.RegularExpressions;


namespace ApiNetsuite.Repositorio
{
    public class BienSQL : IBienSQL
    {
        private string CadenaConexion;
        private readonly ILogger<BienSQL> log;


        public BienSQL(AccesoDatos cadenaConexion, ILogger<BienSQL> l)
        {
            CadenaConexion = cadenaConexion.CadenaConexionSQL;
            this.log = l;
        }


        private SqlConnection conexion()
        {
            return new SqlConnection(CadenaConexion);
        }


        public static DataSet CnstVehiculo(string opcion, string cliente, string vehiculo, string parametroproducto)
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string cadena = MyConfig.GetValue<string>("ConnectionStrings:SQL");
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection connection = new SqlConnection(cadena))
                {
                    using (SqlCommand command = new SqlCommand("Extranet.EXT_SP_NS_HUNTER_PLATAFORMA_AMI", connection))
                    {
                        connection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@OPCION", SqlDbType.VarChar).Value = opcion;
                        command.Parameters.Add("@CLIENTE_ID", SqlDbType.VarChar).Value = cliente;
                        command.Parameters.Add("@VEHICULO_ID", SqlDbType.VarChar).Value = vehiculo;
                        command.Parameters.Add("@PARAMETRO_PROD", SqlDbType.VarChar).Value = parametroproducto;
                        var result = command.ExecuteReader();
                        ds.Load(result, LoadOption.OverwriteChanges, "Table");
                        connection.Close();
                    }
                }
            }
            catch (Exception )
            {
               throw;
            }
            return ds;
        }


        public static DataSet CnstProducto(string opcion, string concesionario, string producto)
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string cadena = MyConfig.GetValue<string>("ConnectionStrings:SQL");
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection connection = new SqlConnection(cadena))
                {
                    using (SqlCommand command = new SqlCommand("Extranet.EXT_SP_NS_HUNTER_PLATAFORMA_AMI", connection))
                    {
                        connection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@OPCION", SqlDbType.VarChar).Value = opcion;
                        command.Parameters.Add("@CONCESIONARIO", SqlDbType.VarChar).Value = concesionario;
                        command.Parameters.Add("@PRODUCTO", SqlDbType.VarChar).Value = producto;
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


        public static DataSet CnstVehiculoParametros(string opcion, string cliente, string vehiculo, string parametroproducto, string tipo,
                                                     string marca, string modelo, string chasis, string motor, string placa, string color,
                                                     string anio)
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string cadena = MyConfig.GetValue<string>("ConnectionStrings:SQL");
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection connection = new SqlConnection(cadena))
                {
                    using (SqlCommand command = new SqlCommand("Extranet.EXT_SP_NS_HUNTER_PLATAFORMA_AMI", connection))
                    {
                        connection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@OPCION", SqlDbType.VarChar).Value = opcion;
                        command.Parameters.Add("@CLIENTE_ID", SqlDbType.VarChar).Value = cliente;
                        command.Parameters.Add("@VEHICULO_ID", SqlDbType.VarChar).Value = vehiculo;
                        command.Parameters.Add("@PARAMETRO_PROD", SqlDbType.VarChar).Value = parametroproducto;
                        command.Parameters.Add("@TIPOTERRESTRE", SqlDbType.VarChar).Value = tipo;
                        command.Parameters.Add("@MARCA", SqlDbType.VarChar).Value = marca;
                        command.Parameters.Add("@MODELO", SqlDbType.VarChar).Value = modelo;
                        command.Parameters.Add("@CHASIS", SqlDbType.VarChar).Value = chasis;
                        command.Parameters.Add("@MOTOR", SqlDbType.VarChar).Value = motor;
                        command.Parameters.Add("@PLACA", SqlDbType.VarChar).Value = placa;
                        command.Parameters.Add("@COLOR", SqlDbType.VarChar).Value = color;
                        command.Parameters.Add("@ANIOVEH", SqlDbType.VarChar).Value = anio;
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
                using (SqlConnection connection = new SqlConnection(cadena))
                {
                    using (SqlCommand command = new SqlCommand("Extranet.EXT_SP_NS_HUNTER_PLATAFORMA_PX", connection))
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
            catch (Exception )
            {
                throw;
            }
            return ds;
        }
          
	
        public static DataSet CambioConAseFin(string opcion, string vehiculo, string placa, string idmarca, string marca, string idmodelo,
                                              string modelo, string chasis, string motor, string color, string anio, string tipo,
                                              string idconcesionario, string concesionariodesc, string concesionariodir,
                                              string idfinanciera, string financieradesc, string financieradir,
                                              string idaseguradora, string aseguradoradesc, string aseguradoradir, string metodo,
                                              string numeroorden, string idusuario, string ejecutiva, string estadocartera, string sucursal)
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string cadena = MyConfig.GetValue<string>("ConnectionStrings:SQL");
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection connection = new SqlConnection(cadena))
                {
                    using (SqlCommand command = new SqlCommand("S3_TURNOS.Extranet.EXT_SP_NS_HUNTER_PLATAFORMA_PX", connection))
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
                        command.Parameters.Add("@ID_CONCESIONARIO", SqlDbType.VarChar).Value = idconcesionario;
                        command.Parameters.Add("@CONCESIONARIO_DESC", SqlDbType.VarChar).Value = concesionariodesc;
                        command.Parameters.Add("@CONCESIONARIO_DIR", SqlDbType.VarChar).Value = concesionariodir;
                        command.Parameters.Add("@ID_FINANCIERA", SqlDbType.VarChar).Value = idfinanciera;
                        command.Parameters.Add("@FINANCIERA_DESC", SqlDbType.VarChar).Value = financieradesc;
                        command.Parameters.Add("@FINANCIERA_DIR", SqlDbType.VarChar).Value = financieradir;
                        command.Parameters.Add("@ID_ASEGURADORA", SqlDbType.VarChar).Value = idaseguradora;
                        command.Parameters.Add("@ASEGURADORA_DESC", SqlDbType.VarChar).Value = aseguradoradesc;
                        command.Parameters.Add("@ASEGURADORA_DIR", SqlDbType.VarChar).Value = aseguradoradir;
                        command.Parameters.Add("@METODO", SqlDbType.VarChar).Value = metodo;
                        command.Parameters.Add("@ORDEN_SERVICIO", SqlDbType.VarChar).Value = numeroorden;
                        command.Parameters.Add("@USUARIO_ID", SqlDbType.VarChar).Value = idusuario;

                        command.Parameters.Add("@NombreEjecutiva", SqlDbType.VarChar).Value = ejecutiva;
                        command.Parameters.Add("@EstadoCartera", SqlDbType.VarChar).Value = estadocartera;
                        command.Parameters.Add("@SUCURSAL", SqlDbType.VarChar).Value = sucursal;

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


        public static DataSet CambioPropietario(string opcion, string vehiculo, string placa, string idmarca, string marca, string idmodelo,
                                             string modelo, string chasis, string motor, string color, string anio, string tipo,
                                             string numero_documento, string primer_nombre, string segundo_nombre,
                                             string apellido_paterno, string apellido_materno, string direccion, string telefono_convencional,
                                             string telefono_celular, string email, string metodo, string numeroorden, string idusuario)
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string cadena = MyConfig.GetValue<string>("ConnectionStrings:SQL");
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection connection = new SqlConnection(cadena))
                {
                    using (SqlCommand command = new SqlCommand("S3_TURNOS.Extranet.EXT_SP_NS_HUNTER_PLATAFORMA_PX", connection))
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
                        //command.Parameters.Add("@ID_CONCESIONARIO", SqlDbType.VarChar).Value = idconcesionario;
                        //command.Parameters.Add("@CONCESIONARIO_DESC", SqlDbType.VarChar).Value = concesionariodesc;
                        //command.Parameters.Add("@CONCESIONARIO_DIR", SqlDbType.VarChar).Value = concesionariodir;
                        //command.Parameters.Add("@ID_FINANCIERA", SqlDbType.VarChar).Value = idfinanciera;
                        //command.Parameters.Add("@FINANCIERA_DESC", SqlDbType.VarChar).Value = financieradesc;
                        //command.Parameters.Add("@FINANCIERA_DIR", SqlDbType.VarChar).Value = financieradir;
                        //command.Parameters.Add("@ID_ASEGURADORA", SqlDbType.VarChar).Value = idaseguradora;
                        //command.Parameters.Add("@ASEGURADORA_DESC", SqlDbType.VarChar).Value = aseguradoradesc;
                        //command.Parameters.Add("@ASEGURADORA_DIR", SqlDbType.VarChar).Value = aseguradoradir;
                        command.Parameters.Add("@NUMERO_DOCUMENTO", SqlDbType.VarChar).Value = numero_documento;
                        command.Parameters.Add("@PRIMER_NOMBRE", SqlDbType.VarChar).Value = primer_nombre;
                        command.Parameters.Add("@SEGUNDO_NOMBRE", SqlDbType.VarChar).Value = segundo_nombre;
                        command.Parameters.Add("@APELLIDO_PATERNO", SqlDbType.VarChar).Value = apellido_paterno;
                        command.Parameters.Add("@APELLIDO_MATERNO", SqlDbType.VarChar).Value = apellido_materno;
                        command.Parameters.Add("@DIRECCION_PRINCIPAL", SqlDbType.VarChar).Value = direccion;
                        command.Parameters.Add("@TELEFONO_CONVENCIONAL", SqlDbType.VarChar).Value = telefono_convencional;
                        command.Parameters.Add("@TELEFONO_CELULAR", SqlDbType.VarChar).Value = telefono_celular;
                        command.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = email;


                        command.Parameters.Add("@METODO", SqlDbType.VarChar).Value = metodo;
                        command.Parameters.Add("@ORDEN_SERVICIO", SqlDbType.VarChar).Value = numeroorden;
                        command.Parameters.Add("@USUARIO_ID", SqlDbType.VarChar).Value = idusuario;
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




        public static string Vehiculo(VehiculoNSDTO data)
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
                        using SqlCommand command = new SqlCommand("VENTAS.DBO.MIG_SP_PASO_DATOS_VEHICULO", connection);
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Transaction = transaction;
                            command.Parameters.Clear();
                            command.Parameters.Add("@EMPRESA", SqlDbType.VarChar).Value = "001";
                            command.Parameters.Add("@IDVEHICULO", SqlDbType.Int).Value = data.IdVehiculo;
                            command.Parameters.Add("@CODIGOVEHICULO", SqlDbType.Int).Value = data.CodigoVehiculo;
                            command.Parameters.Add("@VEHICULONOMBRE", SqlDbType.VarChar).Value = data.VehiculoNombre;
                            command.Parameters.Add("@IDCLIENTE", SqlDbType.Int).Value = data.IdCliente;
                            command.Parameters.Add("@CEDULA_RUC", SqlDbType.VarChar).Value = data.Cedula_Ruc;
                            command.Parameters.Add("@IDTIPOBIEN", SqlDbType.BigInt).Value = data.IdTipoBien;
                            command.Parameters.Add("@TIPOBIEN", SqlDbType.VarChar).Value = data.TipoBien;
                            command.Parameters.Add("@IDTIPOTERRESTRE", SqlDbType.BigInt).Value = data.IdTipoTerrestre;
                            command.Parameters.Add("@TIPOTERRESTRE", SqlDbType.VarChar).Value = data.TipoTerrestre;
                            command.Parameters.Add("@PLACA", SqlDbType.VarChar).Value = data.Placa;
                            command.Parameters.Add("@IDORIGEN", SqlDbType.Int).Value = data.IdOrigen;
                            command.Parameters.Add("@CODORIGEN", SqlDbType.VarChar).Value = data.CodOrigen;
                            command.Parameters.Add("@ORIGEN", SqlDbType.VarChar).Value = data.Origen;
                            command.Parameters.Add("@MOTOR", SqlDbType.VarChar).Value = data.Motor;
                            command.Parameters.Add("@IDMARCA", SqlDbType.Int).Value = data.IdMarca;
                            command.Parameters.Add("@MARCA", SqlDbType.VarChar).Value = data.Marca;
                            command.Parameters.Add("@IDMODELO", SqlDbType.Int).Value = data.IdModelo;
                            command.Parameters.Add("@MODELO", SqlDbType.VarChar).Value = data.Modelo;
                            command.Parameters.Add("@CHASIS", SqlDbType.VarChar).Value = data.Chasis;
                            command.Parameters.Add("@KILOMETRAJE", SqlDbType.Decimal).Value = data.Kilometraje;
                            command.Parameters.Add("@AVALUO", SqlDbType.Decimal).Value = data.Avaluo;
                            command.Parameters.Add("@TONELAJE", SqlDbType.Decimal).Value = data.Tonelaje;
                            command.Parameters.Add("@CLAVE", SqlDbType.VarChar).Value = data.Clave;
                            command.Parameters.Add("@IDTIPO", SqlDbType.Int).Value = data.IdTipo;
                            command.Parameters.Add("@TIPO", SqlDbType.VarChar).Value = data.Tipo;
                            command.Parameters.Add("@IDTRANSMISION", SqlDbType.Int).Value = data.IdTransmision;
                            command.Parameters.Add("@CODTRANSMISION", SqlDbType.VarChar).Value = data.CodTransmision;
                            command.Parameters.Add("@TRANSMISION", SqlDbType.VarChar).Value = data.Transmision;
                            command.Parameters.Add("@IDCARACTERISTICA", SqlDbType.VarChar).Value = data.IdCaracteristica;
                            command.Parameters.Add("@CARACTERISTICA", SqlDbType.VarChar).Value = data.Caracteristica;
                            command.Parameters.Add("@IDCOLORCARSEG", SqlDbType.Int).Value = data.IdColorCarseg;
                            command.Parameters.Add("@COLORCARSEG", SqlDbType.VarChar).Value = data.ColorCarseg;
                            command.Parameters.Add("@IDCOLORFABRICANTE", SqlDbType.Int).Value = data.IdColorFabricante;
                            command.Parameters.Add("@COLORFABRICANTE", SqlDbType.VarChar).Value = data.ColorFabricante;
                            command.Parameters.Add("@IDCOLORMATRICULA", SqlDbType.Int).Value = data.IdColorMatricula;
                            command.Parameters.Add("@COLORMATRICULA", SqlDbType.VarChar).Value = data.ColorMatricula;
                            command.Parameters.Add("@IDVERSION", SqlDbType.Int).Value = data.IdVersion;
                            command.Parameters.Add("@VERSION", SqlDbType.VarChar).Value = data.Version;
                            command.Parameters.Add("@IDTIPOCABINA", SqlDbType.Int).Value = data.IdTipoCabina;
                            command.Parameters.Add("@TIPOCABINA", SqlDbType.VarChar).Value = data.TipoCabina;
                            command.Parameters.Add("@IDCILINDRAJE", SqlDbType.Int).Value = data.IdCilindraje;
                            command.Parameters.Add("@CILINDRAJE", SqlDbType.VarChar).Value = data.Cilindraje;
                            command.Parameters.Add("@IDTRACCION", SqlDbType.Int).Value = data.IdTraccion;
                            command.Parameters.Add("@TRACCION", SqlDbType.VarChar).Value = data.Traccion;
                            command.Parameters.Add("@IDCOMBUSTIBLE", SqlDbType.Int).Value = data.IdCombustible;
                            command.Parameters.Add("@COMBUSTIBLE", SqlDbType.VarChar).Value = data.Combustible;
                            command.Parameters.Add("@TIPOFLOTA", SqlDbType.VarChar).Value = data.TipoFlota;
                            command.Parameters.Add("@ANIO", SqlDbType.Int).Value = data.Anio;
                            command.Parameters.Add("@RECORRIDO", SqlDbType.VarChar).Value = data.Recorrido;
                            command.Parameters.Add("@IDUSOVEHICULAR", SqlDbType.Int).Value = data.IdUsoVehicular;
                            command.Parameters.Add("@USOVEHICULAR", SqlDbType.VarChar).Value = data.UsoVehicular;
                            command.Parameters.Add("@IDOFICINA", SqlDbType.Int).Value = data.IdOFicina;
                            command.Parameters.Add("@OFICINA", SqlDbType.VarChar).Value = data.OFicina;
                            command.Parameters.Add("@IDEJECUTIVA", SqlDbType.BigInt).Value = data.IdEjecutiva;
                            command.Parameters.Add("@IDFORMAPAGO", SqlDbType.VarChar).Value = data.IdFormaPago;
                            command.Parameters.Add("@FORMAPAGO", SqlDbType.VarChar).Value = data.FormaPago;
                            command.Parameters.Add("@IDCONCESIONARIO", SqlDbType.Int).Value = data.IdConcesionario;
                            command.Parameters.Add("@CODCONCESIONARIO", SqlDbType.VarChar).Value = data.CodConcesionario;
                            command.Parameters.Add("@CONCESIONARIO", SqlDbType.VarChar).Value = data.Concesionario;
                            command.Parameters.Add("@DIRECCIONCONCESIONARIO", SqlDbType.VarChar).Value = data.DireccionConcesionario;
                            command.Parameters.Add("@IDOFICINACONCESIONARIO", SqlDbType.Int).Value = data.IdOficinaConcesionario;
                            command.Parameters.Add("@OFICINACONCESIONARIO", SqlDbType.VarChar).Value = data.OficinaConcesionario;
                            command.Parameters.Add("@IDASEGURADORA", SqlDbType.Int).Value = data.IdAseguradora;
                            command.Parameters.Add("@CODASEGURADORA", SqlDbType.VarChar).Value = data.CodAseguradora;
                            command.Parameters.Add("@ASEGURADORA", SqlDbType.VarChar).Value = data.Aseguradora;
                            command.Parameters.Add("@IDOFICINAASEGURADORA", SqlDbType.Int).Value = data.IdOficinaAseguradora;
                            command.Parameters.Add("@OFICINAASEGURADORA", SqlDbType.VarChar).Value = data.OficinaAseguradora;
                            command.Parameters.Add("@IDFINANCIERA", SqlDbType.Int).Value = data.IdFinanciera;
                            command.Parameters.Add("@CODFINANCIERA", SqlDbType.VarChar).Value = data.CodFinanciera;
                            command.Parameters.Add("@FINANCIERA", SqlDbType.VarChar).Value = data.Financiera;
                            command.Parameters.Add("@IDOFICINAFINANCIERA", SqlDbType.Int).Value = data.IdOficinaFinanciera;
                            command.Parameters.Add("@OFICINAFINANCIERA", SqlDbType.VarChar).Value = data.OficinaFinanciera;
                            command.Parameters.Add("@IDCOPASOCIACION", SqlDbType.VarChar).Value = data.IdCopAsociacion;
                            command.Parameters.Add("@COPASOCIACION", SqlDbType.VarChar).Value = data.CopAsociacion;
                            command.Parameters.Add("@IDCONVENIO", SqlDbType.Int).Value = data.IdConvenio;
                            command.Parameters.Add("@CONVENIO", SqlDbType.VarChar).Value = data.Convenio;
                            command.Parameters.Add("@ESTADOCONVENIO", SqlDbType.VarChar).Value = data.EstadoConvenio;
                            command.Parameters.Add("@NUM_PUERTAS", SqlDbType.Int).Value = data.NumPuertas;
                            command.Parameters.Add("@ESTADO_VEH", SqlDbType.Int).Value = data.EstadoAutomovil;

                            //command.Parameters.Add("@OSCONVENIO", SqlDbType.VarChar).Value = data.OSConvenio;
                            command.Parameters.Add("@SEGUIMIENTO", SqlDbType.VarChar).Value = data.seguimiento;
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
                        string Datos = LogDB.LogData(resultado, JsonConvert.SerializeObject(data), "Vehiculo");

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


        //public async Task ActualizarVehiculo(BienAPI p)
        //{
        //    SqlConnection sqlConexion = conexion();
        //    SqlCommand Comm = null;
        //     try
        //    {





        //        //sqlConexion.Open();
        //        //Comm = sqlConexion.CreateCommand();
        //        //Comm.CommandText = "dbo.sp_negocio_carga_claroIngreso";
        //        //Comm.CommandType = CommandType.StoredProcedure;
        //        //Comm.Parameters.Add("@ORDERTYPE", SqlDbType.VarChar, 100).Value = p.OrderType;
        //        //Comm.Parameters.Add("@TELEFONOSIM", SqlDbType.VarChar, 15).Value = p.TelefonoSIM;
        //        //Comm.Parameters.Add("@NUMEROSIM", SqlDbType.VarChar, 50).Value = p.NumeroSIM;
        //        //Comm.Parameters.Add("@SERIESIM", SqlDbType.VarChar, 50).Value = p.SerieSIM;
        //        //Comm.Parameters.Add("@PLAN", SqlDbType.VarChar, 50).Value = p.Plan;
        //        //Comm.Parameters.Add("@ESTADO", SqlDbType.VarChar, 50).Value = p.Estado;
        //        //Comm.Parameters.Add("@FECHA", SqlDbType.DateTime).Value = p.Fecha;



        //        //Comm.Parameters.Add("@PROCESO", SqlDbType.Int).Value = 1;
        //        //SqlDataReader reader = await Comm.ExecuteReaderAsync();


        //    }
        //    catch (Exception ex)
        //    {
        //        log.LogError(ex.ToString());
        //        throw new Exception("Se produjo un error al obtener datos" + ex.Message);
        //    }
        //    finally
        //    {
        //        //Comm.Dispose();
        //        //sqlConexion.Close();
        //        //sqlConexion.Dispose();
        //    }

        //    await Task.CompletedTask;
        //}
    }
}
