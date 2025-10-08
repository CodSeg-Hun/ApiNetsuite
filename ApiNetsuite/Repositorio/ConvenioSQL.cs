using ApiNetsuite.Repositorio.IRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using System;
using System.Data.SqlClient;
using Microsoft.VisualBasic;
using ApiNetsuite.Modelo;
using Microsoft.Extensions.Options;
using System.ServiceModel.Channels;
using ApiNetsuite.Clases;
using Microsoft.IdentityModel.Protocols;
using System.Net.Mail;

namespace ApiNetsuite.Repositorio
{
    public class ConvenioSQL : IConvenioSQL
    {
        private string CadenaConexion;
        private readonly ILogger<ConvenioSQL> log;


        public ConvenioSQL(AccesoDatos cadenaConexion, ILogger<ConvenioSQL> l)
        {
            CadenaConexion = cadenaConexion.CadenaConexionSQL;
            this.log = l;
        }


        private SqlConnection conexion()
        {
            return new SqlConnection(CadenaConexion);
        }


        public static DataSet CnstParametrosMed(string opcion, string cliente, string beneficiario, string tipoPlan)
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string cadena = MyConfig.GetValue<string>("ConnectionStrings:SQL");
            DataSet ds = new DataSet();
            try
            {
                using SqlConnection connection = new SqlConnection(cadena);
                {
                    using SqlCommand command = new SqlCommand("Extranet.EXT_SP_HUNTER_MED", connection);
                    {
                        connection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@OPCION", SqlDbType.VarChar).Value = opcion;
                        command.Parameters.Add("@ID_CLIENTE", SqlDbType.VarChar).Value = cliente;
                        command.Parameters.Add("@BENEFICIARIO", SqlDbType.VarChar).Value = beneficiario;
                        command.Parameters.Add("@TIPO_PLAN", SqlDbType.VarChar).Value = tipoPlan;
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


        public static string Registrar_Med( string ordenservicio, string cliente, string estado, string mensaje, string contrato,
                                            string plataforma, string aplicativo, string accion, string respuesta,
                                            string region, string producto, string opcion, string texto )
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string cadena = MyConfig.GetValue<string>("ConnectionStrings:SQL");
            // DataSet ds = new DataSet();
            SqlTransaction transaccion = null;
            string resultado = "";
            try
            {
                using (SqlConnection connection = new SqlConnection(cadena))
                {
                    using (SqlCommand command = new SqlCommand("Extranet.EXT_SP_HUNTER_MED", connection))
                    {
                        try
                        {
                            connection.Open();
                            transaccion = connection.BeginTransaction();
                            command.CommandType = CommandType.StoredProcedure;
                            command.Transaction = transaccion;
                            command.Parameters.Clear();
                            command.Parameters.Add("@OPCION", SqlDbType.VarChar).Value = opcion;
                            command.Parameters.Add("@ORDEN_SERVICIO", SqlDbType.VarChar).Value = ordenservicio;
                            command.Parameters.Add("@ID_CLIENTE", SqlDbType.VarChar).Value = cliente;
                            command.Parameters.Add("@ESTADO_ID", SqlDbType.VarChar).Value = estado;
                            command.Parameters.Add("@MENSAJE", SqlDbType.VarChar).Value = mensaje;
                            command.Parameters.Add("@NUMERO_CONTRATO", SqlDbType.VarChar).Value = contrato;
                            command.Parameters.Add("@PLATAFORMA", SqlDbType.VarChar).Value = plataforma;
                            command.Parameters.Add("@APLICATIVO", SqlDbType.VarChar).Value = aplicativo;
                            command.Parameters.Add("@ACCION", SqlDbType.VarChar).Value = accion;
                            command.Parameters.Add("@RESPUESTA", SqlDbType.VarChar).Value = respuesta;
                            command.Parameters.Add("@REGION", SqlDbType.VarChar).Value = region;
                            command.Parameters.Add("@PRODUCTO", SqlDbType.VarChar).Value = producto;
                            command.Parameters.Add("@ENVIO", SqlDbType.VarChar).Value = texto;                        
                            command.ExecuteNonQuery();
                            transaccion.Commit();
                            resultado = "OK";
                            connection.Close();
                        }
                        catch (Exception ex)
                        {
                            transaccion.Rollback();
                            resultado = ex.Message.ToString();
                        }
                        finally
                        {
                            connection.Close();
                            connection.Dispose();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                resultado = ex.Message.ToString();
            }
            //return ds;
            return resultado;
        }


        public static DataSet VehiculoChequeado(string opcion)
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string cadena = MyConfig.GetValue<string>("ConnectionStrings:SQL");
            DataSet ds = new DataSet();
            try
            {
                using SqlConnection connection = new SqlConnection(cadena);
                {
                    using SqlCommand command = new SqlCommand("Extranet.PROCESAR_VEH_ICESA", connection);
                    {
                        connection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@PROCESO", SqlDbType.Int).Value = opcion;
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


        public static string ActualizarEstado(string vehiculo, string proceso, string mensaje, string cadena, int registro)
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string conectar = MyConfig.GetValue<string>("ConnectionStrings:SQL");
            SqlTransaction transaccion = null;
            string resultado = "";
            try
            {
                using SqlConnection connection = new SqlConnection(conectar);
                {
                    using SqlCommand command = new SqlCommand("Extranet.PROCESAR_VEH_ICESA", connection);
                    {
                        try
                        {
                            connection.Open();
                            transaccion = connection.BeginTransaction();
                            command.CommandType = CommandType.StoredProcedure;
                            command.Transaction = transaccion;
                            command.Parameters.Clear();
                            command.Parameters.Add("@PROCESO", SqlDbType.Int).Value = proceso;
                            command.Parameters.Add("@CODIGO_VEHICULO", SqlDbType.VarChar).Value = vehiculo;
                            command.Parameters.Add("@MENSAJE", SqlDbType.VarChar).Value = mensaje;
                            command.Parameters.Add("@CADENA", SqlDbType.VarChar).Value = cadena;
                            command.Parameters.Add("@REGISTROS", SqlDbType.Int).Value = registro;
                            command.ExecuteNonQuery();
                            transaccion.Commit();
                            resultado = "OK";
                            connection.Close();
                        }
                        catch (Exception ex)
                        {
                            transaccion.Rollback();
                            resultado = ex.Message.ToString();
                        }
                        finally
                        {
                            connection.Close();
                            connection.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                resultado = ex.Message.ToString();
            }
            return resultado;
        }


        public static DataSet LeerEmail_Med(string opcion,  string orden, string cliente, string producto)
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string cadena = MyConfig.GetValue<string>("ConnectionStrings:SQL");
            DataSet ds = new DataSet();
            try
            {
                using SqlConnection connection = new(cadena);
                {
                    using SqlCommand command = new("Extranet.EXT_SP_HUNTER_MED", connection);
                    {
                        connection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@OPCION", SqlDbType.VarChar).Value = opcion;
                        command.Parameters.Add("@ORDEN_SERVICIO", SqlDbType.VarChar).Value = orden;
                        command.Parameters.Add("@ID_CLIENTE", SqlDbType.VarChar).Value = cliente;
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



        public static DataSet ActualidaSalud(string opcion, string estado, string respuesta, string orden)
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string cadena = MyConfig.GetValue<string>("ConnectionStrings:SQL");
            DataSet ds = new DataSet();
            try
            {
                using SqlConnection connection = new SqlConnection(cadena);
                {
                    using SqlCommand command = new SqlCommand("Extranet.EXT_SP_HUNTER_MED", connection);
                    {
                        connection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@OPCION", SqlDbType.VarChar).Value = opcion;
                        command.Parameters.Add("@ESTADO_SALUD_ID", SqlDbType.VarChar).Value = estado;
                        command.Parameters.Add("@RESPUESTA_SALUD", SqlDbType.VarChar).Value = respuesta;
                        command.Parameters.Add("@ORDEN_SERVICIO", SqlDbType.VarChar).Value = orden;
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

        public static string GenerarMail(string ordenservicio, string cliente, string producto)
        {
            string resultado = "";
            try
            {
                //GenerarMail = false;
                var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
                string MailAddress = MyConfig.GetValue<string>("Mail:MailAddress");
                string ErrorMailTo = MyConfig.GetValue<string>("Mail:ErrorMailTo");
                string SmptCliente = MyConfig.GetValue<string>("Mail:SmptCliente");
                System.Data.DataSet dTDatos = new System.Data.DataSet();
                dTDatos = ConvenioSQL.LeerEmail_Med("107", ordenservicio, cliente, producto);
                if (dTDatos.Tables.Count > 0)
                {
                    System.Net.Mail.MailMessage correo = new System.Net.Mail.MailMessage();
                    correo.From = new System.Net.Mail.MailAddress(MailAddress);
                    correo.To.Add((string)dTDatos.Tables[0].Rows[0]["EMAIL"]);
                    //correo.To.Add("galvarado@carsegsa.com");
                    string mailToBcc = ErrorMailTo;
                    String[] mailToBccCollection = mailToBcc.Split(";");
                    foreach (string mailTooBcc in mailToBccCollection)
                        correo.Bcc.Add(mailTooBcc);
                    correo.Attachments.Add(new Attachment((string)dTDatos.Tables[0].Rows[0]["RUTA_FILE_1"]));
                    correo.Attachments.Add(new Attachment((string)dTDatos.Tables[0].Rows[0]["RUTA_FILE_2"]));
                    correo.Subject = " Tu salud, ahora monitoreada! Bienvenido a HunterMED";
                    string htmlbody = (string)dTDatos.Tables[0].Rows[0]["HTMLBODY"];
                    correo.Body = htmlbody;
                    correo.Priority = MailPriority.High;
                    correo.IsBodyHtml = true;
                    // ---------------------------------------------
                    // DATOS DE LA CONFIGURACION DE LA CUENTA ENVIA
                    // ---------------------------------------------
                    SmtpClient smtpClient = new SmtpClient(SmptCliente);
                    smtpClient.Send(correo);
                    correo.Dispose();
                    //GenerarMail = true;
                    resultado = "OK";
                }
            }
            catch (Exception ex)
            {
               // throw ex;
                resultado =  ex.Message.ToString();
                //GenerarMail = false;
            }
            return resultado;
        }

    }
}
