using ApiNetsuite.Clases;
using ApiNetsuite.DTO.Cliente;
using ApiNetsuite.DTO.OrdenServicio;
using ApiNetsuite.Modelo;
using ApiNetsuite.Repositorio.IRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ApiNetsuite.Repositorio
{
    public class ClienteSQL : IClienteSQL
    {
        private string CadenaConexion;
        private readonly ILogger<ClienteSQL> log;
        
        public ClienteSQL(AccesoDatos cadenaConexion, ILogger<ClienteSQL> l)
        {
            CadenaConexion = cadenaConexion.CadenaConexionSQL;
            this.log = l;
        }
        

        private SqlConnection conexion()
        {
            return new SqlConnection(CadenaConexion);
        }


        public static DataSet CnstCliente(string opcion, string cliente)
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string cadena = MyConfig.GetValue<string>("ConnectionStrings:SQL");
            DataSet ds = new DataSet();
            try
            {
                using SqlConnection connection = new (cadena);
                {
                    using SqlCommand command = new ("Extranet.EXT_SP_NS_HUNTER_PLATAFORMA_AMI", connection);
                    {
                        connection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@OPCION", SqlDbType.VarChar).Value = opcion;
                        command.Parameters.Add("@CLIENTE_ID", SqlDbType.VarChar).Value = cliente;
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


        public static DataSet CnstClienteParametrosPX(string opcion, string numero_documento, string primer_nombre, string segundo_nombre,
                                                     string apellido_paterno, string apellido_materno, string direccion, string telefono_convencional,
                                                     string telefono_celular, string email, string usuario_id, string numeroorden, string ejecutiva, 
                                                     string sucursal)
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string cadena = MyConfig.GetValue<string>("ConnectionStrings:SQL");
            DataSet ds = new DataSet();
            string metodo = "ActualizarDatos";
            try
            {
                using SqlConnection connection = new (cadena);
                {
                    using SqlCommand command = new ("Extranet.EXT_SP_NS_HUNTER_PLATAFORMA_PX", connection);
                    {
                        connection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@OPCION", SqlDbType.VarChar).Value = opcion;
                        command.Parameters.Add("@NUMERO_DOCUMENTO", SqlDbType.VarChar).Value = numero_documento;
                        command.Parameters.Add("@PRIMER_NOMBRE", SqlDbType.VarChar).Value = primer_nombre;
                        command.Parameters.Add("@SEGUNDO_NOMBRE", SqlDbType.VarChar).Value = segundo_nombre;
                        command.Parameters.Add("@APELLIDO_PATERNO", SqlDbType.VarChar).Value = apellido_paterno;
                        command.Parameters.Add("@APELLIDO_MATERNO", SqlDbType.VarChar).Value = apellido_materno;
                        command.Parameters.Add("@DIRECCION_PRINCIPAL", SqlDbType.VarChar).Value = direccion;
                        command.Parameters.Add("@TELEFONO_CONVENCIONAL", SqlDbType.VarChar).Value = telefono_convencional;
                        command.Parameters.Add("@TELEFONO_CELULAR", SqlDbType.VarChar).Value = telefono_celular;
                        command.Parameters.Add("@EMAIL", SqlDbType.VarChar).Value = email;
                        command.Parameters.Add("@USUARIO_ID", SqlDbType.VarChar).Value = usuario_id;
                        command.Parameters.Add("@ORDEN_SERVICIO", SqlDbType.VarChar).Value = numeroorden;
                        command.Parameters.Add("@METODO", SqlDbType.VarChar).Value = metodo;
                        command.Parameters.Add("@NombreEjecutiva", SqlDbType.VarChar).Value = ejecutiva;
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


        public static DataSet CnstConsultar(string opcion)
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string cadena = MyConfig.GetValue<string>("ConnectionStrings:SQL");
            DataSet ds = new DataSet();
            try
            {
                using SqlConnection connection = new (cadena);
                {
                    using SqlCommand command = new ("Extranet.EXT_SP_NS_HUNTER_PLATAFORMA_PX", connection);
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

        //public async Task ActualizarCliente(ClienteAPI p)
        //{
        //    SqlConnection sqlConexion = conexion();
        //    SqlCommand Comm = null;
        //    try
        //    {
        //        //if (parametro == "PX")
        //        //{
        //        //}
        //        //else if (parametro == "AMI")
        //        //{
        //        //}
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
        //        Comm.Dispose();
        //        sqlConexion.Close();
        //        sqlConexion.Dispose();
        //    }
        //    await Task.CompletedTask;
        //}


        public static string CrearCliente(RegClienteDTO data)
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
                        using SqlCommand command = new SqlCommand("VENTAS.DBO.MIG_SP_PASO_DATOS_CLIENTE", connection);
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Transaction = transaction;
                            command.Parameters.Clear();                          
                            command.Parameters.Add("@OPCION", SqlDbType.VarChar).Value = "CLI";
                            command.Parameters.Add("@ID_NS_CLIENTE", SqlDbType.Int).Value = data.Id_ns_cliente;
                            command.Parameters.Add("@IDENTIFICACION", SqlDbType.VarChar).Value = data.Identificacion;
                            command.Parameters.Add("@NOMBRE_COMPLETO", SqlDbType.VarChar).Value = data.Nombre_Completo;
                            command.Parameters.Add("@PRIMER_NOMBRE", SqlDbType.VarChar).Value = data.Primer_Nombre;
                            command.Parameters.Add("@SEGUNDO_NOMBRE", SqlDbType.VarChar).Value = data.Segundo_Nombre;
                            command.Parameters.Add("@APELLIDO_PATERNO", SqlDbType.VarChar).Value = data.Apellido_Paterno;
                            command.Parameters.Add("@APELLIDO_MATERNO", SqlDbType.VarChar).Value = data.Apellido_Materno;
                            command.Parameters.Add("@TIPO_CLIENTE", SqlDbType.VarChar).Value = data.Tipo_Cliente;
                            command.Parameters.Add("@TIPO_PERSONA", SqlDbType.VarChar).Value = data.Tipo_Persona;
                            command.Parameters.Add("@OFICINA", SqlDbType.VarChar).Value = data.Oficina;
                            command.Parameters.Add("@ID_EJECUTIVA", SqlDbType.BigInt).Value = data.Id_Ejecutiva;
                            command.ExecuteNonQuery();


                            for (int i = 0; i < data.Direccion.Count; i++)
                            {
                                command.Parameters.Clear();
                                command.Parameters.Add("@OPCION", SqlDbType.VarChar).Value = "DIR";
                                command.Parameters.Add("@ID_NS_CLIENTE", SqlDbType.Int).Value = data.Id_ns_cliente;
                                command.Parameters.Add("@IDENTIFICACION", SqlDbType.VarChar).Value = data.Direccion[i].Cedula_Ruc;
                                command.Parameters.Add("@TIPO_DIRECCION", SqlDbType.VarChar).Value = data.Direccion[i].IdTipoDireccion;
                                command.Parameters.Add("@ID_PAIS", SqlDbType.VarChar).Value = data.Direccion[i].IdPais;
                                command.Parameters.Add("@ID_PROVINCIA", SqlDbType.VarChar).Value = data.Direccion[i].CodProvincia;
                                command.Parameters.Add("@ID_CIUDAD", SqlDbType.VarChar).Value = data.Direccion[i].CodCanton;
                                command.Parameters.Add("@ID_PARROQUIA", SqlDbType.VarChar).Value = data.Direccion[i].CodParroquia;
                                command.Parameters.Add("@DIRECCION", SqlDbType.VarChar).Value = data.Direccion[i].Direccion1;
                                command.Parameters.Add("@PREDETERMINADO", SqlDbType.VarChar).Value = data.Direccion[i].DireccionPredeterminadaEnvio;
                                command.ExecuteNonQuery();
                            }

                            for (int i = 0; i < data.Email.Count; i++)
                            {
                                command.Parameters.Clear();
                                command.Parameters.Add("@OPCION", SqlDbType.VarChar).Value = "MAI";
                                command.Parameters.Add("@ID_NS_CLIENTE", SqlDbType.Int).Value = data.Id_ns_cliente;
                                command.Parameters.Add("@IDENTIFICACION", SqlDbType.VarChar).Value = data.Email[i].Cedula;
                                command.Parameters.Add("@TIPO_EMAIL_NS", SqlDbType.VarChar).Value = data.Email[i].TipoCorreo;
                                command.Parameters.Add("@DIRECCION", SqlDbType.VarChar).Value = data.Email[i].Email;
                                command.ExecuteNonQuery();
                            }

                            for (int i = 0; i < data.Telefono.Count; i++)
                            {
                                command.Parameters.Clear();
                                command.Parameters.Add("@OPCION", SqlDbType.VarChar).Value = "TEL";
                                command.Parameters.Add("@ID_NS_CLIENTE", SqlDbType.Int).Value = data.Id_ns_cliente;
                                command.Parameters.Add("@IDENTIFICACION", SqlDbType.VarChar).Value = data.Telefono[i].Cedula;
                                command.Parameters.Add("@TIPO_TELEFONO", SqlDbType.VarChar).Value = data.Telefono[i].TipoTelefono;
                                command.Parameters.Add("@TELEFONO", SqlDbType.VarChar).Value = data.Telefono[i].Telefono;
                                command.Parameters.Add("@USUARIO", SqlDbType.VarChar).Value = data.Telefono[i].Cedula;
                                command.ExecuteNonQuery();
                            }

                        }
                        //transaction.Rollback();
                        transaction.Commit();
                        resultado = "OK";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        resultado = ex.Message.ToString();
                        string Datos = LogDB.LogData(resultado, JsonConvert.SerializeObject(data), "CrearCliente");
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
