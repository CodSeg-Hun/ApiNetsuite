using RestSharp;
using System;
using System.Data;
using ApiNetsuite.Repositorio;
using ApiNetsuite.Modelo;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using Microsoft.VisualBasic;
using ApiNetsuite.DTO.Bien;
using ApiNetsuite.DTO.Cliente;
using ApiNetsuite.DTO.AMI;
using Microsoft.Extensions.Options;

namespace ApiNetsuite.Clases
{
    public class ConsultaAMI
    {

        public static string ActualizacionAsset(BienDTO p)
        {
            string resultado = "";
            string mensaje = "OK";
            Boolean bandera = true;
            string token = "";
            var valoretorno = "";
            var customerID = "";
            var busquedaAmiCliente = "";
            var busquedaAmiVehiculo = "";
            var cobertura = "";
            var producto = "";
            var assetID = "";
            var rutalogin = "";
            var rutaassetAdd = "";
            var rutacustomerAdd = "";
            var descripcion = "";
            try
            {
                conexionAMI ruta = new conexionAMI();
                //conexion
                if (bandera)
                {
                    rutalogin = ruta.GetRuta("1");
                    rutaassetAdd = ruta.GetRuta("2");
                    rutacustomerAdd = ruta.GetRuta("3");
                   
                }
                customerEntidad cliente = null;
                assetEntidad asset = null;
                attributeEntidadCollection attributeEntidadCollection = null;
                attributeEntidad attributeCollection = null;
                //datos
                if (bandera)
                {
                    DataSet cnstGenrl = new DataSet();
                    cnstGenrl = BienSQL.CnstVehiculo(opcion: "100", cliente: "", vehiculo: "", parametroproducto: "GPT");
                    if (cnstGenrl.Tables.Count > 0)
                    {
                        busquedaAmiVehiculo = p.amivehiculo;
                        busquedaAmiCliente = p.amicliente;
                        descripcion = p.descripcionvehiculo.ToUpper();
                        cliente = new customerEntidad
                        {
                            username = p.username,
                            first_name = "",
                            last_name = "",
                            email = "",
                            login= (string)cnstGenrl.Tables[0].Rows[0]["USUARIO"],
                            password = (string)cnstGenrl.Tables[0].Rows[0]["PASS"],
                            internal_identifier= p.cliente,
                            active = "True",
                        };
                        //informacion del vehiculo 
                        asset = new assetEntidad
                        {
                            name = p.placa,
                            assettype = p.tipovehiculo,
                            codigosys = p.codvehiculo,
                            customname = "",
                            active = "True",
                            description = p.descripcionvehiculo.ToUpper(),
                            product = "",
                            productexpiredate = "",

                        };
                        //informacion del vehiculo atributos
                        cnstGenrl = BienSQL.CnstVehiculoParametros(opcion: "103", cliente: p.cliente, vehiculo: p.codvehiculo, 
                                                                   parametroproducto: "GPT", tipo: p.tipovehiculo.ToUpper(), marca: p.marca.ToUpper(),
                                                                   modelo: p.modelo.ToUpper(), chasis: p.chasis.ToUpper(), motor: p.motor.ToUpper(), placa: p.placa.ToUpper(),
                                                                   color: p.color.ToUpper(), anio: p.anio);
                        if (cnstGenrl.Tables.Count > 0)
                        {
                            attributeEntidadCollection = new attributeEntidadCollection();
                            attributeCollection = new attributeEntidad();
                            for (int i = 0; i <= cnstGenrl.Tables[0].Rows.Count - 1; i++)
                            {
                                attributeEntidad attribute = new attributeEntidad();
                                attribute.AttributeId = (int)cnstGenrl.Tables[0].Rows[i]["ID"];
                                attribute.Valor = (string)cnstGenrl.Tables[0].Rows[i]["VALOR"];
                                attributeEntidadCollection.Add(attribute);
                            }
                            attributeCollection.attributeEntidadCollection = attributeEntidadCollection;
                        }
                        //validaciones
                        if (bandera)
                        {
                            //falta validacion
                            if (cliente.username == "")
                            {
                                mensaje = "El cliente USERNAME no existe en AMI";
                                bandera = false;
                            }
                            else if (p.tipovehiculo == "0")
                            {
                                mensaje = "Verificar el tipo de Inmueble del Vehiculo en Syshunter ";
                                bandera = false;
                            
                            }
                            else if (busquedaAmiCliente == "")
                            {
                                mensaje = "No existe el vehiculo en AMI";
                                bandera = false;
                            }
                            else if (busquedaAmiVehiculo == "")
                            {
                                mensaje = "No existe el cliente en AMI";
                                bandera = false;
                            }
                            else
                            {
                                var client = new RestClient(rutalogin);
                                var request = new RestRequest("",Method.Post);
                                request.AddParameter("username", cliente.login);
                                request.AddParameter("password", cliente.password);
                                RestResponse response = client.Execute(request);
                                var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(response.Content);
                                token = funciones.descomponer("token", jsonString);

                                //int status = (int)response.StatusCode;
                                //string texto = response.Content;

                                //mensaje = " token  " + token + " - "+ rutalogin + " - " + status + " - " + texto;
                                //bandera = false;
                            }
                        }
                    }
                    else
                    {
                        bandera = false;
                        mensaje = "No existen Información de la consulta";
                    }
                }
                // Bloque cliente propietario
                if (bandera)
                {
                    var customersearch = new RestClient();
                    var requestecustomersearch = new RestRequest();
                    RestResponse responsecustomersearch;
                    // consulta si existe el customer si se encuentra inactivo
                    customersearch = new RestClient(rutacustomerAdd + "?username=" + cliente.username + "&is_active=" + true + "&id=" + busquedaAmiCliente + " ");
                    requestecustomersearch = new RestRequest("", Method.Get);
                    requestecustomersearch.AddHeader("cache-control", "no-cache");
                    requestecustomersearch.AddHeader("authorization", "HunterAPI " + token);
                    responsecustomersearch = customersearch.Execute(requestecustomersearch);
                    valoretorno = "0";
                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(responsecustomersearch.Content);
                    valoretorno = funciones.descomponer("id", jsonString);
                    if (valoretorno != "0")
                    {
                        if (valoretorno == busquedaAmiCliente)
                        {
                            customerID = valoretorno;
                            bandera = true;
                        }
                        else
                        {
                            mensaje = "busqueda incorrecta, Customer Incorrecto ";
                            bandera = false;
                        }
                    }
                    else
                    {
                        mensaje = "Error al buscar el Customer del cliente ";
                        bandera = false;
                    }

                }
                //bloque del vehiculo
                if (bandera)
                {
                    var assetSearch = new RestClient();
                    var requesteAssetSearch = new RestRequest();
                    RestResponse responseAssetSearch;
                    string texto;
                    var clienteasset = new RestClient();
                    var requesteasset = new RestRequest();
                    RestResponse responseasset;
                    assetSearch = new RestClient(rutaassetAdd + "?attributes__attribute__name=COD_SYS" + "&attributes__value=" + p.codvehiculo + " ");
                    requesteAssetSearch = new RestRequest("", Method.Get);
                    requesteAssetSearch.AddHeader("cache-control", "no-cache");
                    requesteAssetSearch.AddHeader("authorization", "HunterAPI " + token);
                    responseAssetSearch = assetSearch.Execute(requesteAssetSearch);
                    valoretorno = "0";
                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(responseAssetSearch.Content);
                    valoretorno = funciones.descomponer("id", jsonString);
                    if (valoretorno != "0")
                    {
                        if (valoretorno == busquedaAmiVehiculo)
                        {
                            cobertura = funciones.descomponer("product_expire_date", jsonString).Substring(0, 10);
                            producto = funciones.descomponer("product", jsonString);
                            texto = Utilidades.SerializarAssetActualizar(asset, attributeEntidadCollection, valoretorno, customerID);
                            clienteasset = new RestClient(rutaassetAdd + valoretorno + "/ ");
                            requesteasset = new RestRequest("", Method.Patch);
                            requesteasset.AddHeader("content-type", "application/json");
                            requesteasset.AddHeader("authorization", "HunterAPI " + token);
                            requesteasset.AddParameter("application/json", texto, ParameterType.RequestBody);
                            responseasset = clienteasset.Execute(requesteasset);
                            if ((int)responseasset.StatusCode == 200)
                            {
                                jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(responseasset.Content);
                                assetID = funciones.descomponer("id", jsonString);
                                bandera = true;
                            }
                            else if ((int)responseasset.StatusCode == 400)
                            {
                                 string datmensaje = responseasset.Content;
                                mensaje = datmensaje + " " + responseasset.StatusCode + " asset Actualiza";
                                bandera = false;
                            }
                            else
                            {
                                mensaje = "Error al procesar asset " + responseasset.StatusCode + " asset Actualiza";
                                bandera = false;
                            }
                        }
                        else
                        {
                            mensaje = "busqueda incorrecta Asset Incorrecto ";
                            bandera = false;
                        }
                    }
                    else
                    {
                        mensaje = "Error al consultar el asset no existe, Actualizando ";
                        bandera = false;
                    }


                }
                //ingreso del log del proceso de AMI
                if (bandera)
                {
                    //proceso log
                    string resultadoLog = ConsultaAMI.RegistroLog(p.cliente, customerID, "0", "0", p.codvehiculo, assetID, "", "", producto, "0", "0", 
                                                                    p.idusuario, "N", "", "VEH", cliente.username, "A", "", "", "", "", "", "", "", "", "", "", 
                                                                    "", "GPT", cobertura, "", "", "");
                    if (resultadoLog == "OK")
                    {
                        mensaje = "OK";
                        bandera = true;
                    }
                    else
                    {
                        mensaje = "Error en el Ingreso del Log";
                        bandera = false;
                    }
                }
                resultado = mensaje;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            return resultado;
        }

        public static string ActualizacionCliente(ClienteDTO p)
        {
            string resultado = "";
            string mensaje = "OK";
            Boolean bandera = true;
            string token = "";
            var valoretorno = "";
            var rutalogin = "";
            var rutacustomerAdd = "";
            var descripcion = "";
            try
            {
                conexionAMI ruta = new conexionAMI();
                //conexion
                if (bandera)
                {
                    rutalogin = ruta.GetRuta("1");
                    rutacustomerAdd = ruta.GetRuta("3");
                }
                customerEntidad cliente = null;
                //string activo = "True";
                string accountype = "1";
                string busquedaAmi = "";
                string usuario = p.id_usuario;
                string celular = p.telefono_celular;
                string telefono = p.telefono_convencional;
                string direccion = p.direccion;
                string tipodocumento = p.ami_tipodocumento;
                string company = "";
                string support = "";
                string log_usr = "";
                string log_pwd = "";
                string clienteid = "";
                string ami_telefono = "";
                string ami_celular = "";
                DataSet cnstGenrl = new DataSet();
                cnstGenrl = BienSQL.CnstVehiculo(opcion: "100", cliente: "", vehiculo: "", parametroproducto: "GPT");
                support = (string)cnstGenrl.Tables[0].Rows[0]["support"];
                log_usr = (string)cnstGenrl.Tables[0].Rows[0]["usuario"];
                log_pwd = (string)cnstGenrl.Tables[0].Rows[0]["pass"];
                if (bandera)
                {
                    busquedaAmi = p.amicliente;
                    clienteid = p.id_cliente;
                    descripcion = p.primer_nombre;
                    company = p.ami_company;
                    support = p.ami_support;
                    ami_telefono = p.ami_telefono;
                    ami_celular= p.ami_celular;
                    cliente = new customerEntidad
                    {
                        username = p.email_ami,
                        first_name = p.primer_nombre + " " + p.segundo_nombre,
                        last_name = p.apellido_paterno + " " + p.apellido_materno,
                        email = p.email,
                        login = log_usr,
                        password = log_pwd,
                        internal_identifier = p.id_cliente,
                        active = "True",
                    };
                    //validaciones
                    if (bandera)
                    {
                        //falta validacion
                        if (cliente.username == "")
                        {
                            mensaje = "El cliente USERNAME no existe en AMI";
                            bandera = false;
                        }
                        else if (busquedaAmi == "")
                        {
                            mensaje = "No existe el cliente en AMI";
                            bandera = false;
                        }
                        else
                        {
                            var client = new RestClient(rutalogin);
                            var request = new RestRequest("", Method.Post);
                            request.AddParameter("username", cliente.login);
                            request.AddParameter("password", cliente.password);
                            RestResponse response = client.Execute(request);
                            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(response.Content);
                            token = funciones.descomponer("token", jsonString);
                        }
                    }
                }
                // Bloque cliente
                if (bandera)
                {
                    var customersearch = new RestClient();
                    var requestecustomersearch = new RestRequest();
                    RestResponse responsecustomersearch;
                    if (busquedaAmi != "")
                        customersearch = new RestClient(rutacustomerAdd + "?customer__identity_document_number=" + clienteid + "&customer__company_code=" + company + "&id=" + busquedaAmi + " ");
                    requestecustomersearch = new RestRequest("", Method.Get);
                    requestecustomersearch.AddHeader("cache-control", "no-cache");
                    requestecustomersearch.AddHeader("authorization", "HunterAPI " + token);
                    responsecustomersearch = customersearch.Execute(requestecustomersearch);
                    valoretorno = "0";
                    valoretorno = funciones.descomponer("id", responsecustomersearch.Content);
                    if (valoretorno != "0")
                    {
                        if (valoretorno == busquedaAmi)
                        {
                            var customer = new RestClient(rutacustomerAdd + valoretorno + "/ ");
                            var requeste = new RestRequest("", Method.Patch);
                            RestResponse responsecustomer;
                            requeste.AddHeader("cache-control", "no-cache");
                            requeste.AddHeader("authorization", "HunterAPI " + token);
                            requeste.AddParameter("id", valoretorno);
                            requeste.AddParameter("username", cliente.username);
                            requeste.AddParameter("first_name", cliente.first_name);
                            requeste.AddParameter("last_name", cliente.last_name);
                            requeste.AddParameter("email", cliente.email);
                            requeste.AddParameter("is_active", cliente.active);
                            requeste.AddParameter("customer.company_code", company);
                            requeste.AddParameter("customer.identity_document_type", tipodocumento);
                            requeste.AddParameter("customer.identity_document_number", clienteid);
                            requeste.AddParameter("customer.business_name", "");
                            requeste.AddParameter("customer.phone_number", telefono);
                            requeste.AddParameter("customer.emergency_phone_number", ami_telefono);
                            requeste.AddParameter("customer.assistance_phone_number", ami_celular);
                            requeste.AddParameter("customer.technical_support_email", support);
                            requeste.AddParameter("customer.account_type", accountype);
                            responsecustomer = customer.Execute(requeste);
                            string result_patch = Convert.ToString(responsecustomer.StatusCode);
                            if (result_patch == "200")
                            {
                                mensaje = "OK";
                                bandera = true;
                            }
                            else if (result_patch == "400")
                            {
                                mensaje = "Error al actualizar el cliente ";
                                bandera = false;
                            }
                        }
                        else
                        {
                            mensaje = "busqueda incorrecta  cliente Incorrecto";
                            bandera = false;
                        }
                    }
                    else
                    {
                        mensaje = "No se encuentra customer para Actualizarlo";
                        bandera = false;
                    }
                }
                //ingreso del log del proceso de AMI
                if (bandera)
                {
                    //proceso log
                    string resultadoLog = ConsultaAMI.RegistroLog(clienteid, busquedaAmi, clienteid, busquedaAmi, "", "", "", "", "", "0", "0", usuario, "N", cliente.username, "CUS",
                                                                  cliente.username, "A", "", "", "", "", "", "", "", "", "", "", "", "GPT", "1900-01-01", "", "", "");
                    if (resultadoLog == "OK")
                    {
                        mensaje = "OK";
                        bandera = true;
                    }
                    else
                    {
                        mensaje = "Error en el Ingreso del Log";
                        bandera = false;
                    }
                }
                resultado = mensaje;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            return resultado;
        }


        public static string ProcesoAMIConsulta(string opcion, string idbien, string familia, string idOrden, string item, string clienteid)
        {
            
            try
            {
                conexion ruta = new conexion();
                string API_URL = ruta.ObtenerRuta("5052", "1");
                string HMACSHA256SignatureType = ruta.ObtenerDatos("1");
                string OAuthVersion = ruta.ObtenerDatos("2");
                var oauthConsumerKey = ruta.ObtenerDatos("3");
                var oauthToken = ruta.ObtenerDatos("4");
                var oauthConsumerSecret = ruta.ObtenerDatos("5");
                var oauthTokenSecret = ruta.ObtenerDatos("6");
                var realm = ruta.ObtenerDatos("9");
                var httpMethod = ruta.ObtenerDatos("8");
                //API_URL = API_URL + "&opcion=" + opcion + "&idvehiculo=" + idvehiculo + "&codgrupoproducto=" + codgrupoproducto + "&idestadodispositivo=" + idestadodispositivo;
                if (opcion != "" && opcion != null)
                {
                    API_URL = API_URL + "&opcion=" + opcion;
                }
                if (idbien != "" && idbien != null)
                {
                    API_URL = API_URL + "&idbien=" + idbien;
                }
                if (familia != "" && familia != null)
                {
                    API_URL = API_URL + "&familia=" + familia;
                }
                if (idOrden != "" && idOrden != null)
                {
                    API_URL = API_URL + "&idorden=" + idOrden;
                }
                if (item != "" && item != null)
                {
                    API_URL = API_URL + "&item=" + item;
                }
                if (clienteid != "" && clienteid != null)
                {
                    API_URL = API_URL + "&clienteid=" + clienteid;
                }
                //if (idestadodispositivo != "" && idestadodispositivo != null)
                //{
                //    API_URL = API_URL + "&idestadodispositivo=" + idestadodispositivo;
                //}
                OAuthBase auth = new OAuthBase();
                var timestamp = Clases.OAuthBase.GenerateTimeStamp();
                var nonce = Clases.OAuthBase.GenerateNonce();
                var client = new RestClient(API_URL);
                var request = new RestRequest("", Method.Get);
                Uri url = new Uri(API_URL);
                var signature = Clases.OAuthBase.GenerateSignature(url, oauthConsumerKey, oauthConsumerSecret, oauthToken, oauthTokenSecret, httpMethod, timestamp, nonce);
                request.AddHeader("Authorization", "OAuth realm=\"" + realm + "\", oauth_token=\"" + oauthToken + "\", oauth_consumer_key=\"" + oauthConsumerKey + "\"," + " oauth_nonce=\"" + nonce + "\", oauth_timestamp=\"" + timestamp + "\", oauth_signature_method=\"" + HMACSHA256SignatureType + "\", oauth_version=\"" + OAuthVersion + "\", oauth_signature=\"" + signature + "\"");
                request.AddHeader("Content-Type", "application/json");
                //request.AddParameter("application/json", p, ParameterType.RequestBody);
                Console.WriteLine(request);
                var response = client.Execute(request);
                Console.WriteLine(response.Content);
                return response.Content;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            
        }


        public static string RegistroLog(string codigocliente, string ownerID, string clientemonitoreo, string customerID, string codigovehiculo,
                                          string assetID, string comando, string serie, string producto, string ordenservicio, string ordentrabajo,
                                          string usuario, string envio, string username, string tipo, string usernameowner, string origen, string accion,
                                          string clienteOld, string clienteIdOld, string usernameold, string asociadoAmi, string asociadoID,
                                          string usernameasociado, string financieraID, string fimancieraAmi, string usernamefinan, string entityAmi,
                                          string parametroProd, string cobertura, string vid, string codproducto, string concesionario)
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
                    using (SqlCommand command = new SqlCommand("S3_TURNOS.Extranet.EXT_SP_HUNTER_AMI_LOG", connection))
                    {
                        try
                        {
                            connection.Open();
                            transaccion = connection.BeginTransaction();
                            command.CommandType = CommandType.StoredProcedure;
                            command.Transaction = transaccion;
                            command.Parameters.Clear();
                            command.Parameters.Add("@OPCION", SqlDbType.VarChar).Value = "100";
                            command.Parameters.Add("@CLIENTE_ID", SqlDbType.VarChar).Value = codigocliente;
                            command.Parameters.Add("@AMI_OWNER", SqlDbType.VarChar).Value = ownerID;
                            command.Parameters.Add("@MONITOREO_ID", SqlDbType.VarChar).Value = clientemonitoreo;
                            command.Parameters.Add("@AMI_MONITOREO", SqlDbType.VarChar).Value = customerID;
                            command.Parameters.Add("@VEHICULO_ID", SqlDbType.VarChar).Value = codigovehiculo;
                            command.Parameters.Add("@AMI_VEHICULO", SqlDbType.VarChar).Value = assetID;
                            command.Parameters.Add("@COMANDO", SqlDbType.VarChar).Value = comando;
                            command.Parameters.Add("@SERIE", SqlDbType.VarChar).Value = serie;
                            command.Parameters.Add("@PRODUCTO", SqlDbType.VarChar).Value = producto;
                            command.Parameters.Add("@ORDEN_SERVICIO", SqlDbType.VarChar).Value = ordenservicio;
                            command.Parameters.Add("@ORDEN_TRABAJO", SqlDbType.VarChar).Value = ordentrabajo;
                            command.Parameters.Add("@USUARIO_ID", SqlDbType.VarChar).Value = usuario;
                            command.Parameters.Add("@ENVIO", SqlDbType.VarChar).Value = envio;
                            command.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = username;
                            command.Parameters.Add("@TIPO", SqlDbType.VarChar).Value = tipo;
                            command.Parameters.Add("@ORIGEN", SqlDbType.VarChar).Value = origen;
                            command.Parameters.Add("@ACCION", SqlDbType.VarChar).Value = accion;
                            command.Parameters.Add("@USERNAME_OWNER", SqlDbType.VarChar).Value = usernameowner;
                            command.Parameters.Add("@CLIENTE_OLD", SqlDbType.VarChar).Value = clienteOld;
                            command.Parameters.Add("@AMI_OLD", SqlDbType.VarChar).Value = clienteIdOld;
                            command.Parameters.Add("@USERNAME_OLD", SqlDbType.VarChar).Value = usernameold;
                            command.Parameters.Add("@ASOCIADO_AMI", SqlDbType.VarChar).Value = asociadoAmi;
                            command.Parameters.Add("@ASOCIADO_ID", SqlDbType.VarChar).Value = asociadoID;
                            command.Parameters.Add("@USERNAME_ASOC", SqlDbType.VarChar).Value = usernameasociado;
                            command.Parameters.Add("@FINANCIERA_ID", SqlDbType.VarChar).Value = financieraID;
                            command.Parameters.Add("@FINANCIERA_AMI", SqlDbType.VarChar).Value = fimancieraAmi;
                            command.Parameters.Add("@USERNAME_FINAN", SqlDbType.VarChar).Value = usernamefinan;
                            command.Parameters.Add("@AMI_ENTITY", SqlDbType.VarChar).Value = entityAmi;
                            command.Parameters.Add("@PARAMETRO_PROD", SqlDbType.VarChar).Value = parametroProd;
                            command.Parameters.Add("@VID", SqlDbType.VarChar).Value = vid;
                            command.Parameters.Add("@GRUPO", SqlDbType.VarChar).Value = codproducto;
                            command.Parameters.Add("@CONCESIONARIO", SqlDbType.VarChar).Value  = concesionario;
                            if (Strings.Len(cobertura) > 19)
                                command.Parameters.Add("@COBERTURA", SqlDbType.VarChar).Value = cobertura.Replace("Z", "").Substring(0, 19);
                            else
                                command.Parameters.Add("@COBERTURA", SqlDbType.VarChar).Value = cobertura.Replace("Z", "");
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



    }
}
