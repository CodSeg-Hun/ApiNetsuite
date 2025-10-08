using ApiNetsuite.Clases;
using ApiNetsuite.DTO.PX;
using ApiNetsuite.Modelo;
using ApiNetsuite.Repositorio;
using ApiNetsuite.Repositorio.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Data;
using System.Diagnostics.Contracts;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace ApiNetsuite.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("ProcesoPX")]
    public class ProcesoPXController : Controller
    {

        private readonly IProcesoPXSQL repositorio;
        protected RespuestAPI _respuestaApi;


        public ProcesoPXController(IProcesoPXSQL r)
        {
            this.repositorio = r;
            this._respuestaApi = new();
        }


        [HttpPost]
        [Authorize]
        [Route("ConsultaToken")]
        public ActionResult<string> ConsultaToken(GeneralPXDTO p)
        {
            //conecion PX
            conexionPX ruta = new conexionPX();
            string rutaurl = ruta.GetRuta("1"); ;
            string rutaAutenticacion = ruta.GetRuta("2");
            // XML de la solicitud SOAP
            string soapRequest = $@"<?xml version=""1.0"" encoding=""utf-8""?><soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""  xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/""><soap:Header><SeguridadPx xmlns=""http://tempuri.org/""><StrToken>{p.token}</StrToken><AutenticacionToken>string</AutenticacionToken><UserName>{p.usuario}</UserName><Password>{p.pass}</Password></SeguridadPx></soap:Header><soap:Body><AutenticacionUsuarioPx xmlns=""http://tempuri.org/"" /></soap:Body></soap:Envelope>";        
            // Código para enviar la solicitud HTTP
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(rutaurl);
            request.Headers.Add("SOAPAction", rutaAutenticacion);
            //request.Headers.Add("Content-Security-Policy",  "default-src 'self' https: data: 'unsafe-inline' 'unsafe-eval';");
            request.ContentType = "text/xml; charset=utf-8";
            request.Method = "POST";
            // Escribir el contenido de la solicitud
            using (Stream stream = request.GetRequestStream())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(soapRequest);
                stream.Write(bytes, 0, bytes.Length);
            }
            // Obtener la respuesta
            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    //string responseXml = reader.ReadToEnd();
                    //Console.WriteLine(responseXml);
                    string responseXml = reader.ReadToEnd();
                    // Analizar el XML de la respuesta para extraer AutenticacionUsuarioPxResult
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(responseXml);
                    // Namespace manager para manejar prefijos de namespaces
                    XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
                    nsmgr.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");
                    nsmgr.AddNamespace("tempuri", "http://tempuri.org/");
                    // Buscar el nodo AutenticacionUsuarioPxResult
                    XmlNode resultNode = xmlDoc.SelectSingleNode("//tempuri:AutenticacionUsuarioPxResult", nsmgr);
                    if (resultNode != null)
                    {
                        string result = resultNode.InnerText;
                        if (result == "-1")
                        {
                            _respuestaApi.StatusCode = System.Net.HttpStatusCode.BadRequest;
                            _respuestaApi.IsSuccess = false;
                            _respuestaApi.Messages.Add("Token Generado de forma incorrecta.");
                            //mensaje = _respuestaApi.Messages[0];
                            //respuesta = ("{\"respuesta\":" + _respuestaApi.Messages[0] + "}");
                            return Ok(_respuestaApi);
                        }
                        else
                        {
                            Console.WriteLine("Resultado: " + result);
                            _respuestaApi.StatusCode = System.Net.HttpStatusCode.OK;
                            _respuestaApi.IsSuccess = true;
                            _respuestaApi.Messages.Add("Grabado con exito");
                            //_respuestaApi.Messages.Add(result);
                            _respuestaApi.Result = result;
                            return Ok(_respuestaApi);
                        }
                    }
                    else
                    {
                        //Console.WriteLine("No se encontró el nodo AutenticacionUsuarioPxResult en la respuesta.");
                        _respuestaApi.StatusCode = System.Net.HttpStatusCode.BadRequest;
                        _respuestaApi.IsSuccess = false;
                        _respuestaApi.Messages.Add("No se encontró el nodo AutenticacionUsuarioPxResult en la respuesta.");
                        //mensaje = _respuestaApi.Messages[0];
                        //respuesta = ("{\"respuesta\":" + _respuestaApi.Messages[0] + "}");
                        return Ok(_respuestaApi);
                    }

                }
            }
            //string jsonData;
            ////jsonData = (string)Utilidades.DataTableToJSON(tabla);
            //jsonData = "{\"results\":" + _respuestaApi.Messages[0] + "}";
            //return jsonData;
        }


        [HttpPost]
        // [Authorize]
        [SwaggerOperation(Summary = "", Description = "Consulta Informacion de los Comando que se pueden ejecutar")]
        [Route("ConsultaComandos")]
        public async Task<ActionResult<string>> ConsultaComandos(GeneralPXDTO p)
        {
            DataSet infoDatos = new DataSet();
            //conecion PX
            conexionPX ruta = new conexionPX();
            string rutaurl = ruta.GetRuta("1"); ;
            string rutaComando = ruta.GetRuta("8");
            string resultToken = ruta.GetToken(p.usuario, p.pass, p.token);
            if (resultToken == "-1")
            {
                _respuestaApi.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.Messages.Add("Token Generado de forma incorrecta.");
                // return Ok(_respuestaApi);
            }
            else if (resultToken == "-2")
            {
                _respuestaApi.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.Messages.Add("No se encontró el nodo AutenticacionUsuarioPxResult en la respuesta.");
                //return Ok(_respuestaApi);
            }
            else
            {
                // no presenta datos no consulta
                string soapRequest = $@"<?xml version=""1.0"" encoding=""utf-8"" standalone=""no""?><soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/""><soap:Header><SeguridadPx xmlns=""http://tempuri.org/""><StrToken>{p.token}</StrToken><AutenticacionToken>{resultToken}</AutenticacionToken><UserName>{p.usuario}</UserName><Password>{p.pass}</Password></SeguridadPx></soap:Header><soap:Body><ConsultaComandos xmlns=""http://tempuri.org/""><usuarioCliente>{p.loginUsuario}</usuarioCliente><claveCliente>{p.passUsuario}</claveCliente><codigoVehiculo>{p.vehiculo}</codigoVehiculo><codigoProducto>{p.codProducto}</codigoProducto><marcaDispositivo>{p.marcaDispositivo}</marcaDispositivo><modeloDispositivo>{p.modeloDispositivo}</modeloDispositivo><usuarioIngreso>{p.usuario}</usuarioIngreso><strNacionalidad>{p.nacionalidad}</strNacionalidad></ConsultaComandos></soap:Body></soap:Envelope>";
                try
                {
                    // Crear la solicitud HTTP
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(rutaurl);
                    request.Headers.Add("SOAPAction", rutaComando);
                    request.ContentType = "text/xml; charset=utf-8";
                    request.Method = "POST";
                    // Escribir el contenido de la solicitud en el cuerpo
                    using (Stream stream = request.GetRequestStream())
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(soapRequest);
                        stream.Write(bytes, 0, bytes.Length);
                    }
                    // Obtener la respuesta
                    using (WebResponse response = request.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            string responseXml = reader.ReadToEnd();
                            Console.WriteLine("Respuesta del servicio:");
                            Console.WriteLine(responseXml);
                            // Procesar la respuesta
                            string result = ExtractResponseData(responseXml, "ConsultaComandos");
                            Console.WriteLine("Resultado procesado:");
                            Console.WriteLine(result);
                            // Procesar el XML de la respuesta (si es necesario)
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.LoadXml(responseXml);
                            XmlNode node = xmlDoc.SelectSingleNode("//NewDataSet");
                            if (node != null)
                            {
                                using (StringReader stringReader = new StringReader(node.OuterXml))
                                {
                                    infoDatos.ReadXml(stringReader);
                                }
                            }
                            //DTO.PX.RespuestaPX respuesta = null;
                            if (infoDatos.Tables[0].Rows.Count > 0)
                            {
                                _respuestaApi.StatusCode = System.Net.HttpStatusCode.OK;
                                _respuestaApi.IsSuccess = true;
                                _respuestaApi.Messages.Add("Grabado con exito");
                                //_respuestaApi.Result = respuesta;
                                //    // return Ok(_respuestaApi);
                            }
                            else
                            {
                                Console.WriteLine("No se encontró el nodo ConsultaComandos en la respuesta.");
                                _respuestaApi.StatusCode = System.Net.HttpStatusCode.BadRequest;
                                _respuestaApi.IsSuccess = false;
                                _respuestaApi.Messages.Add("No existen datos que mostrar.");
                                //mensaje = _respuestaApi.Messages[0];
                                //respuesta = ("{\"respuesta\":" + _respuestaApi.Messages[0] + "}");
                                //return Ok(_respuestaApi);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al consumir el servicio: " + ex.Message);
                    _respuestaApi.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    _respuestaApi.IsSuccess = false;
                    _respuestaApi.Messages.Add(ex.Message);
                    //return Ok(_respuestaApi);
                }
            }
            string jsonData;
            if (infoDatos.Tables[0].Rows.Count > 0)
            {
                jsonData = (string)Utilidades.DataTableToJSON(infoDatos.Tables[0]);
                jsonData = "{\"results\":" + jsonData + "}";
            }
            else
            {
                jsonData = "{\"results\":" + _respuestaApi.Messages[0] + "}";
            }
            //jsonData = (string)Utilidades.DataTableToJSON(_respuestaApi.Messages[0]);
            return jsonData;
        }


        [HttpPost]
        [Authorize]
        [SwaggerOperation(Summary = "", Description = "Consulta Informacion del Dispositivo que se encuentra en la plataforma PX")]
        [Route("ConsultaDispositivo")]
        public async Task<ActionResult<string>> ConsultaDispositivo(GeneralPXDTO p)
        {
            DataSet infoDatos = new DataSet();
            //conecion PX
            conexionPX ruta = new conexionPX();
            string rutaurl = ruta.GetRuta("1"); ;
            string rutaDispositivo = ruta.GetRuta("7");
            string resultToken = ruta.GetToken(p.usuario, p.pass, p.token);
            if (resultToken == "-1")
            {
                _respuestaApi.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.Messages.Add("Token Generado de forma incorrecta.");
                // return Ok(_respuestaApi);
            }
            else if (resultToken == "-2")
            {
                _respuestaApi.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.Messages.Add("No se encontró el nodo AutenticacionUsuarioPxResult en la respuesta.");
                //return Ok(_respuestaApi);
            }
            else
            {
                // no presenta datos no consulta
                string soapRequest = $@"<?xml version=""1.0"" encoding=""utf-8"" standalone=""no""?><soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/""><soap:Header><SeguridadPx xmlns=""http://tempuri.org/""><StrToken>{p.token}</StrToken><AutenticacionToken>{resultToken}</AutenticacionToken><UserName>{p.usuario}</UserName><Password>{p.pass}</Password></SeguridadPx></soap:Header><soap:Body><ConsultaDispositivo xmlns=""http://tempuri.org/""><strParametro>{p.vid}</strParametro><strNacionalidad>{p.nacionalidad}</strNacionalidad></ConsultaDispositivo></soap:Body></soap:Envelope>";
                try
                {
                    // Crear la solicitud HTTP
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(rutaurl);
                    request.Headers.Add("SOAPAction", rutaDispositivo);
                    request.ContentType = "text/xml; charset=utf-8";
                    request.Method = "POST";
                    // Escribir el contenido de la solicitud en el cuerpo
                    using (Stream stream = request.GetRequestStream())
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(soapRequest);
                        stream.Write(bytes, 0, bytes.Length);
                    }
                    // Obtener la respuesta
                    using (WebResponse response = request.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            string responseXml = reader.ReadToEnd();
                            Console.WriteLine("Respuesta del servicio:");
                            Console.WriteLine(responseXml);
                            // Procesar la respuesta
                            string result = ExtractResponseData(responseXml, "ConsultaDispositivo");
                            Console.WriteLine("Resultado procesado:");
                            Console.WriteLine(result);
                            // Procesar el XML de la respuesta (si es necesario)
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.LoadXml(responseXml);
                            XmlNode node = xmlDoc.SelectSingleNode("//NewDataSet");
                            if (node != null)
                            {
                                using (StringReader stringReader = new StringReader(node.OuterXml))
                                {
                                    infoDatos.ReadXml(stringReader);
                                }
                            }
                            //DTO.PX.RespuestaPX respuesta = null;
                            if (infoDatos.Tables[0].Rows.Count > 0)
                            {
                                _respuestaApi.StatusCode = System.Net.HttpStatusCode.OK;
                                _respuestaApi.IsSuccess = true;
                                _respuestaApi.Messages.Add("Grabado con exito");
                                //_respuestaApi.Result = respuesta;
                                //    // return Ok(_respuestaApi);
                            }
                            else
                            {
                                Console.WriteLine("No se encontró el nodo ConsultaOrdenResult en la respuesta.");
                                _respuestaApi.StatusCode = System.Net.HttpStatusCode.BadRequest;
                                _respuestaApi.IsSuccess = false;
                                _respuestaApi.Messages.Add("No existen datos que mostrar.");
                                //mensaje = _respuestaApi.Messages[0];
                                //respuesta = ("{\"respuesta\":" + _respuestaApi.Messages[0] + "}");
                                //return Ok(_respuestaApi);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al consumir el servicio: " + ex.Message);
                    _respuestaApi.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    _respuestaApi.IsSuccess = false;
                    _respuestaApi.Messages.Add(ex.Message);
                    //return Ok(_respuestaApi);
                }
            }
            string jsonData;
            if (infoDatos.Tables[0].Rows.Count > 0)
            {
                jsonData = (string)Utilidades.DataTableToJSON(infoDatos.Tables[0]);
                jsonData = "{\"results\":" + jsonData + "}";
            }
            else
            {
                jsonData = "{\"results\":" + _respuestaApi.Messages[0] + "}";
            }
            //jsonData = (string)Utilidades.DataTableToJSON(_respuestaApi.Messages[0]);
            return jsonData;
        }


        [HttpPost]
        [Authorize]
        [SwaggerOperation(Summary = "", Description = "Consulta Informacion de la Orden que se encuentra en la plataforma PX")]
        [Route("ConsultaOrden")]
        public async Task<ActionResult<string>> ConsultaOrden(GeneralPXDTO p)
        {
            DataSet infoDatos = new DataSet();
            //conecion PX
            conexionPX ruta = new conexionPX();
            string rutaurl = ruta.GetRuta("1"); ;
            string rutaOrden = ruta.GetRuta("5");
            string resultToken = ruta.GetToken(p.usuario, p.pass, p.token);
            if (resultToken == "-1")
            {
                _respuestaApi.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.Messages.Add("Token Generado de forma incorrecta.");
                //return Ok(_respuestaApi);
            }
            else if (resultToken == "-2")
            {
                _respuestaApi.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.Messages.Add("No se encontró el nodo AutenticacionUsuarioPxResult en la respuesta.");
                //return Ok(_respuestaApi);
            }
            else
            {
                //string soapRequest = $@"<?xml version=""1.0"" encoding=""utf-8""?><soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/""><soap:Header><SeguridadPx xmlns=""http://tempuri.org/""><StrToken>{p.token}</StrToken><AutenticacionToken>{resultToken}</AutenticacionToken><UserName>{p.usuario}</UserName><Password>{p.pass}</Password></SeguridadPx></soap:Header><soap:Body><ConsultaVehiculo xmlns=""http://tempuri.org/""><strParametro>1001169608</strParametro><strNacionalidad>EC</strNacionalidad></ConsultaVehiculo></soap:Body></soap:Envelope>";
                string soapRequest = $@"<?xml version=""1.0"" encoding=""utf-8"" standalone=""no""?><soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/""><soap:Header><SeguridadPx xmlns=""http://tempuri.org/""><StrToken>{p.token}</StrToken><AutenticacionToken>{resultToken}</AutenticacionToken><UserName>{p.usuario}</UserName><Password>{p.pass}</Password></SeguridadPx></soap:Header><soap:Body><ConsultaOrden xmlns=""http://tempuri.org/""><strNumeroOrden>{p.numeroOrden}</strNumeroOrden><strNacionalidad>{p.nacionalidad}</strNacionalidad></ConsultaOrden></soap:Body></soap:Envelope>";
                try
                {
                    // Crear la solicitud HTTP
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(rutaurl);
                    request.Headers.Add("SOAPAction", rutaOrden);
                    request.ContentType = "text/xml; charset=utf-8";
                    request.Method = "POST";
                    // Escribir el contenido de la solicitud en el cuerpo
                    using (Stream stream = request.GetRequestStream())
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(soapRequest);
                        stream.Write(bytes, 0, bytes.Length);
                    }

                    // Obtener la respuesta
                    using (WebResponse response = request.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            string responseXml = reader.ReadToEnd();
                            Console.WriteLine("Respuesta del servicio:");
                            Console.WriteLine(responseXml);
                            // Procesar la respuesta
                            string result = ExtractResponseData(responseXml, "ConsultaOrden");
                            Console.WriteLine("Resultado procesado:");
                            Console.WriteLine(result);
                            // Procesar el XML de la respuesta (si es necesario)
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.LoadXml(responseXml);
                            XmlNode node = xmlDoc.SelectSingleNode("//NewDataSet");

                            if (node != null)
                            {
                                using (StringReader stringReader = new StringReader(node.OuterXml))
                                {
                                    infoDatos.ReadXml(stringReader);
                                }
                            }
                            DTO.PX.RespuestaPX respuesta = null;
                            if (infoDatos.Tables[0].Rows.Count > 0)
                            {
                                respuesta = new DTO.PX.RespuestaPX
                                {
                                    NumeroOrden = (string)infoDatos.Tables[0].Rows[0]["NumeroOrden"],
                                    FechaHoraIngreso = (string)infoDatos.Tables[0].Rows[0]["FechaHoraIngreso"],
                                    UsuarioIngreso = (string)infoDatos.Tables[0].Rows[0]["UsuarioIngreso"],
                                    CodSysHunter = (string)infoDatos.Tables[0].Rows[0]["CodSysHunter"],
                                    Chasis = (string)infoDatos.Tables[0].Rows[0]["Chasis"],
                                    Vid = (string)infoDatos.Tables[0].Rows[0]["Vid"],
                                };
                                _respuestaApi.StatusCode = System.Net.HttpStatusCode.OK;
                                _respuestaApi.IsSuccess = true;
                                _respuestaApi.Messages.Add("Grabado con exito");
                                _respuestaApi.Result = respuesta;
                                // return Ok(_respuestaApi);
                            }
                            else
                            {
                                Console.WriteLine("No se encontró el nodo ConsultaOrdenResult en la respuesta.");
                                _respuestaApi.StatusCode = System.Net.HttpStatusCode.BadRequest;
                                _respuestaApi.IsSuccess = false;
                                _respuestaApi.Messages.Add("No existen datos que mostrar.");
                                //mensaje = _respuestaApi.Messages[0];
                                //respuesta = ("{\"respuesta\":" + _respuestaApi.Messages[0] + "}");
                                //return Ok(_respuestaApi);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al consumir el servicio: " + ex.Message);

                    _respuestaApi.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    _respuestaApi.IsSuccess = false;
                    _respuestaApi.Messages.Add(ex.Message);
                    //return Ok(_respuestaApi);

                }
            }
            string jsonData;
            if (infoDatos.Tables[0].Rows.Count > 0)
            {
                jsonData = (string)Utilidades.DataTableToJSON(infoDatos.Tables[0]);
                jsonData = "{\"results\":" + jsonData + "}";
            }
            else
            {
                jsonData = "{\"results\":" + _respuestaApi.Messages[0] + "}";
            }
            //jsonData = (string)Utilidades.DataTableToJSON(_respuestaApi.Messages[0]);           
            return jsonData;
        }


        [HttpPost]
        [Authorize]
        [SwaggerOperation(Summary = "", Description = "Consulta Informacion del Propietario que se encuentra en la plataforma PX")]
        [Route("ConsultaPropietario")]
        public async Task<ActionResult<string>> ConsultaPropietario(GeneralPXDTO p)
        {
            DataSet infoDatos = new DataSet();
            //conecion PX
            conexionPX ruta = new conexionPX();
            string rutaurl = ruta.GetRuta("1"); ;
            string rutaPropietario = ruta.GetRuta("6");
            string resultToken = ruta.GetToken(p.usuario, p.pass, p.token);
            if (resultToken == "-1")
            {
                _respuestaApi.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.Messages.Add("Token Generado de forma incorrecta.");
                // return Ok(_respuestaApi);
            }
            else if (resultToken == "-2")
            {
                _respuestaApi.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.Messages.Add("No se encontró el nodo AutenticacionUsuarioPxResult en la respuesta.");
                //return Ok(_respuestaApi);
            }
            else
            {
                // no presenta datos no consulta
                string soapRequest = $@"<?xml version=""1.0"" encoding=""utf-8"" standalone=""no""?><soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/""><soap:Header><SeguridadPx xmlns=""http://tempuri.org/""><StrToken>{p.token}</StrToken><AutenticacionToken>{resultToken}</AutenticacionToken><UserName>{p.usuario}</UserName><Password>{p.pass}</Password></SeguridadPx></soap:Header><soap:Body><ConsultaPropietario xmlns=""http://tempuri.org/""><strParametro>{p.propietario}</strParametro><strNacionalidad>{p.nacionalidad}</strNacionalidad></ConsultaPropietario></soap:Body></soap:Envelope>";
                try
                {
                    // Crear la solicitud HTTP
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(rutaurl);
                    request.Headers.Add("SOAPAction", rutaPropietario);
                    request.ContentType = "text/xml; charset=utf-8";
                    request.Method = "POST";
                    // Escribir el contenido de la solicitud en el cuerpo
                    using (Stream stream = request.GetRequestStream())
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(soapRequest);
                        stream.Write(bytes, 0, bytes.Length);
                    }
                    // Obtener la respuesta
                    using (WebResponse response = request.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            string responseXml = reader.ReadToEnd();
                            Console.WriteLine("Respuesta del servicio:");
                            Console.WriteLine(responseXml);
                            // Procesar la respuesta
                            string result = ExtractResponseData(responseXml, "ConsultaPropietario");
                            Console.WriteLine("Resultado procesado:");
                            Console.WriteLine(result);
                            // Procesar el XML de la respuesta (si es necesario)
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.LoadXml(responseXml);
                            XmlNode node = xmlDoc.SelectSingleNode("//NewDataSet");
                            if (node != null)
                            {
                                using (StringReader stringReader = new StringReader(node.OuterXml))
                                {
                                    infoDatos.ReadXml(stringReader);
                                }
                            }
                            //DTO.PX.RespuestaPX respuesta = null;
                            if (infoDatos.Tables[0].Rows.Count > 0)
                            {
                                _respuestaApi.StatusCode = System.Net.HttpStatusCode.OK;
                                _respuestaApi.IsSuccess = true;
                                _respuestaApi.Messages.Add("Grabado con exito");
                                //_respuestaApi.Result = respuesta;
                                //    // return Ok(_respuestaApi);
                            }
                            else
                            {
                                Console.WriteLine("No se encontró el nodo ConsultaOrdenResult en la respuesta.");
                                _respuestaApi.StatusCode = System.Net.HttpStatusCode.BadRequest;
                                _respuestaApi.IsSuccess = false;
                                _respuestaApi.Messages.Add("No existen datos que mostrar.");
                                //mensaje = _respuestaApi.Messages[0];
                                //respuesta = ("{\"respuesta\":" + _respuestaApi.Messages[0] + "}");
                                //return Ok(_respuestaApi);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al consumir el servicio: " + ex.Message);
                    _respuestaApi.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    _respuestaApi.IsSuccess = false;
                    _respuestaApi.Messages.Add(ex.Message);
                    //return Ok(_respuestaApi);
                }
            }
            string jsonData;
            if (infoDatos.Tables[0].Rows.Count > 0)
            {
                jsonData = (string)Utilidades.DataTableToJSON(infoDatos.Tables[0]);
                jsonData = "{\"results\":" + jsonData + "}";
            }
            else
            {
                jsonData = "{\"results\":" + _respuestaApi.Messages[0] + "}";
            }
            //jsonData = (string)Utilidades.DataTableToJSON(_respuestaApi.Messages[0]);
            return jsonData;
        }


        [HttpPost]
        //[Authorize]
        [SwaggerOperation(Summary = "", Description = "Consulta Informacion del vehiculo que se encuentra en la plataforma PX")]
        [Route("ConsultarVehiculo")]
        public async Task<ActionResult<string>> ConsultarVehiculo(GeneralPXDTO p)
        {
            DataSet infoDatos = new DataSet();
            //conecion PX
            conexionPX ruta = new conexionPX();
            string rutaurl = ruta.GetRuta("1"); ;
            string rutaVehiculo = ruta.GetRuta("4");
            string resultToken = ruta.GetToken(p.usuario, p.pass, p.token);
            if (resultToken == "-1")
            {
                _respuestaApi.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.Messages.Add("Token Generado de forma incorrecta.");
                return Ok(_respuestaApi);
            }
            else if (resultToken == "-2")
            {
                _respuestaApi.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.Messages.Add("No se encontró el nodo AutenticacionUsuarioPxResult en la respuesta.");
                return Ok(_respuestaApi);
            }
            else
            {
                // no presenta datos no consulta
                string soapRequest = $@"<?xml version=""1.0"" encoding=""utf-8"" standalone=""no""?><soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/""><soap:Header><SeguridadPx xmlns=""http://tempuri.org/""><StrToken>{p.token}</StrToken><AutenticacionToken>{resultToken}</AutenticacionToken><UserName>{p.usuario}</UserName><Password>{p.pass}</Password></SeguridadPx></soap:Header><soap:Body><ConsultaVehiculo xmlns=""http://tempuri.org/""><strParametro>{p.vehiculo}</strParametro><strNacionalidad>{p.nacionalidad}</strNacionalidad></ConsultaVehiculo></soap:Body></soap:Envelope>";
                using (HttpClient client = new HttpClient())
                {
                    // Set headers
                    client.DefaultRequestHeaders.Add("SOAPAction", rutaVehiculo);
                    //client.DefaultRequestHeaders.Add("Content-Security-Policy", "default-src 'self' https: data: 'unsafe-inline' 'unsafe-eval';");
                    //client.DefaultRequestHeaders.Add("Host", "10.100.97.124:808");
                    // client.DefaultRequestHeaders.Add("Content-Type", "text/xml; charset=utf-8");
                    // Create the HTTP content
                    StringContent content = new StringContent(soapRequest, Encoding.UTF8, "text/xml");
                    try
                    {
                        // Send the request
                        HttpResponseMessage response = await client.PostAsync(rutaurl, content);
                        // Read the response
                        string responseContent = await response.Content.ReadAsStringAsync();
                        // Output the response
                        Console.WriteLine("Response:");
                        Console.WriteLine(responseContent);
                        _respuestaApi.StatusCode = System.Net.HttpStatusCode.OK;
                        _respuestaApi.IsSuccess = true;
                        _respuestaApi.Messages.Add("Grabado con exito");
                        _respuestaApi.Result = responseContent;
                        return Ok(_respuestaApi);
                    }
                    catch (Exception ex)
                    {
                        // Handle any errors
                        Console.WriteLine("Error:");
                        Console.WriteLine(ex.Message);
                        _respuestaApi.StatusCode = System.Net.HttpStatusCode.BadRequest;
                        _respuestaApi.IsSuccess = false;
                        _respuestaApi.Messages.Add("No se encontró el nodo AutenticacionUsuarioPxResult en la respuesta.");
                        //mensaje = _respuestaApi.Messages[0];
                        //respuesta = ("{\"respuesta\":" + _respuestaApi.Messages[0] + "}");
                        return Ok(_respuestaApi);
                    }
                }

                //try
                //{
                //    // Crear la solicitud HTTP
                //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(rutaurl);
                //    request.Headers.Add("SOAPAction", rutaVehiculo);
                //    request.ContentType = "text/xml; charset=utf-8";
                //    request.Method = "POST";
                //    // Escribir el contenido de la solicitud en el cuerpo
                //    using (Stream stream = request.GetRequestStream())
                //    {
                //        byte[] bytes = Encoding.UTF8.GetBytes(soapRequest);
                //        stream.Write(bytes, 0, bytes.Length);
                //    }

                //    // Obtener la respuesta
                //    using (WebResponse response = request.GetResponse())
                //    {
                //        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                //        {
                //            string responseXml = reader.ReadToEnd();
                //            Console.WriteLine("Respuesta del servicio:");
                //            Console.WriteLine(responseXml);
                //            // Procesar la respuesta
                //            string result = ExtractResponseData(responseXml, "ConsultaVehiculo");
                //            Console.WriteLine("Resultado procesado:");
                //            Console.WriteLine(result);
                //            // Procesar el XML de la respuesta (si es necesario)
                //            XmlDocument xmlDoc = new XmlDocument();
                //            xmlDoc.LoadXml(responseXml);
                //            XmlNode node = xmlDoc.SelectSingleNode("//NewDataSet");
                //            if (node != null)
                //            {
                //                using (StringReader stringReader = new StringReader(node.OuterXml))
                //                {
                //                    infoDatos.ReadXml(stringReader);
                //                }
                //            }
                //            //DTO.PX.RespuestaPX respuesta = null;
                //            if (infoDatos.Tables[0].Rows.Count > 0)
                //            {
                //            //    respuesta = new DTO.PX.RespuestaPX
                //            //    {
                //            //        NumeroOrden = (string)infoDatos.Tables[0].Rows[0]["NumeroOrden"],
                //            //        FechaHoraIngreso = (string)infoDatos.Tables[0].Rows[0]["FechaHoraIngreso"],
                //            //        UsuarioIngreso = (string)infoDatos.Tables[0].Rows[0]["UsuarioIngreso"],
                //            //        CodSysHunter = (string)infoDatos.Tables[0].Rows[0]["CodSysHunter"],
                //            //        Chasis = (string)infoDatos.Tables[0].Rows[0]["Chasis"],
                //            //        Vid = (string)infoDatos.Tables[0].Rows[0]["Vid"],
                //               };
                //            _respuestaApi.StatusCode = System.Net.HttpStatusCode.OK;
                //            _respuestaApi.IsSuccess = true;
                //            _respuestaApi.Messages.Add("Grabado con exito");
                //            //_respuestaApi.Result = respuesta;
                //            //    // return Ok(_respuestaApi);
                //            //}
                //            //else
                //            //{
                //            //    Console.WriteLine("No se encontró el nodo ConsultaOrdenResult en la respuesta.");
                //            //    _respuestaApi.StatusCode = System.Net.HttpStatusCode.BadRequest;
                //            //    _respuestaApi.IsSuccess = false;
                //            //    _respuestaApi.Messages.Add("No existen datos que mostrar.");
                //            //    //mensaje = _respuestaApi.Messages[0];
                //            //    //respuesta = ("{\"respuesta\":" + _respuestaApi.Messages[0] + "}");
                //            //    //return Ok(_respuestaApi);
                //            //}
                //        }
                //    }
                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine("Error al consumir el servicio: " + ex.Message);

                //    _respuestaApi.StatusCode = System.Net.HttpStatusCode.BadRequest;
                //    _respuestaApi.IsSuccess = false;
                //    _respuestaApi.Messages.Add(ex.Message);
                //    //return Ok(_respuestaApi);

                //}
            }

            //string jsonData;

            //if (infoDatos.Tables[0].Rows.Count > 0)
            //{
            //    jsonData = (string)Utilidades.DataTableToJSON(infoDatos.Tables[0]);
            //    jsonData = "{\"results\":" + jsonData + "}";
            //}
            //else
            //{
            //    jsonData = "{\"results\":" + _respuestaApi.Messages[0] + "}";
            //}

            ////jsonData = (string)Utilidades.DataTableToJSON(_respuestaApi.Messages[0]);

            //return jsonData;

        }


        [HttpPost]
        [Authorize]
        [SwaggerOperation(Summary = "", Description = "Enviar datos para la plataforma PX por Contacto de Emergencia")]
        [Route("ContactoEmergencia")]
        public ActionResult<string> ContactoEmergencia(ContactoPXDTO p)
        {
            //conecion PX
            conexionPX ruta = new conexionPX();
            string rutaurl = ruta.GetRuta("1"); ;
            string rutaContacto = ruta.GetRuta("10");
            string resultToken = ruta.GetToken(p.usuario, p.pass, p.token);
            if (resultToken == "-1")
            {
                _respuestaApi.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.Messages.Add("Token Generado de forma incorrecta.");
                return Ok(_respuestaApi);
            }
            else if (resultToken == "-2")
            {
                _respuestaApi.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.Messages.Add("No se encontró el nodo AutenticacionUsuarioPxResult en la respuesta.");
                return Ok(_respuestaApi);
            }
            else
            {
                //string cadena = $@"<?xml version=""1.0"" encoding=""utf-8""?><Ordenes><Orden><NumeroOrden>1025034733</NumeroOrden><UsuarioIngreso>OPERAD</UsuarioIngreso><OperacionOrden>001</OperacionOrden><NombreEjecutiva>PIERINA GOMEZ</NombreEjecutiva><Ciudad>Guayaquil</Ciudad><Sucursal>Guayaquil</Sucursal><EstadoCartera>ACTIVO</EstadoCartera><FechaInicioCobertura>2025-01-23</FechaInicioCobertura><FechaFinCobertura>2026-01-23</FechaFinCobertura><Vehiculo><Placa>S/P</Placa><IdMarca>283</IdMarca><DescMarca>TOYOTA</DescMarca><IdModelo>2226</IdModelo><DescModelo>NEW FORTUNER AC 2.7 5P 4X4 TM</DescModelo><CodigoVehiculo>BN0000000088</CodigoVehiculo><Chasis>MHFDX3FS8S0055343</Chasis><Motor>2TRB233318</Motor><Color>NEGRO</Color><Anio>2025</Anio><Tipo>SUV</Tipo></Vehiculo><Dispositivo><Vid>4764437456</Vid><IdProducto>HT</IdProducto><DescProducto>HUNTER GPS HMT</DescProducto><CodMarcaDispositivo>21</CodMarcaDispositivo><MarcaDispositivo>CALAMP</MarcaDispositivo><CodModeloDispositivo>307</CodModeloDispositivo><ModeloDispositivo>LMU2630 3G</ModeloDispositivo><Sn>4764437456</Sn><Imei>352556103427543</Imei><NumeroCamaras>0</NumeroCamaras><DireccionMac>0</DireccionMac><Icc>895930100109516616</Icc><NumeroCelular>0989958084</NumeroCelular><Operadora>CLARO</Operadora><EstadoSim>ACT</EstadoSim><ServiciosInstalados><Servicio><CodServicio>001</CodServicio><DescripcionServicio>APERTURA DE SEGUROS</DescripcionServicio><FechaInicioServicio>2025-01-23</FechaInicioServicio><FechaFinServicio>2026-01-23</FechaFinServicio><EstadoServicio>ACTIVO</EstadoServicio></Servicio><Servicio><CodServicio>002</CodServicio><DescripcionServicio>SERVICIOS HM AMI</DescripcionServicio><FechaInicioServicio>2025-01-23</FechaInicioServicio><FechaFinServicio>2026-01-23</FechaFinServicio><EstadoServicio>ACTIVO</EstadoServicio></Servicio></ServiciosInstalados><OperacionDispositivo>I</OperacionDispositivo><VidAnterior></VidAnterior></Dispositivo><Propietario><IdentificadorPropietario>1792073634001</IdentificadorPropietario><NombrePropietario>TOYOTA DEL ECUADOR S.A. </NombrePropietario><ApellidosPropietario></ApellidosPropietario><DireccionPropietario></DireccionPropietario><ConvencionalPropietario>0986260084</ConvencionalPropietario><CelularPropietario>0986260084</CelularPropietario><EmailPropietario>clomas@tde.com.ec</EmailPropietario></Propietario><Monitor><IdentificadorMonitorea>1792073634001</IdentificadorMonitorea><NombreMonitorea>TOYOTA DEL ECUADOR S.A. </NombreMonitorea><ApellidosMonitorea></ApellidosMonitorea><DireccionMonitorea></DireccionMonitorea><ConvencionalMonitorea>0986260084</ConvencionalMonitorea><CelularMonitorea>0986260084</CelularMonitorea><EmailMonitorea>clomas@tde.com.ec</EmailMonitorea></Monitor><Concesionario><IdentificadorConcesionario>0190003701001</IdentificadorConcesionario><RazonSocialConcesionario>IMPORTADORA TOMEBAMBA</RazonSocialConcesionario><DireccionConcesionario>AV. ESPAÑA 1730</DireccionConcesionario><ConvencionalConcesionario>0</ConvencionalConcesionario><CelularConcesionario>0</CelularConcesionario><EmailConcesionario></EmailConcesionario></Concesionario><Financiera><IdentificadorFinanciera>9980000000004</IdentificadorFinanciera><RazonSocialFinanciera>SIN BANCO / FINANCIERA</RazonSocialFinanciera><DireccionFinanciera></DireccionFinanciera><ConvencionalFinanciera>0</ConvencionalFinanciera><CelularFinanciera>0</CelularFinanciera><EmailFinanciera></EmailFinanciera></Financiera><Aseguradora><IdentificadorAseguradora>9980000000005</IdentificadorAseguradora><RazonSocialAseguradora>SIN ASEGURADORA</RazonSocialAseguradora><DireccionAseguradora></DireccionAseguradora><ConvencionalAseguradora>0</ConvencionalAseguradora><CelularAseguradora>0</CelularAseguradora><EmailAseguradora></EmailAseguradora></Aseguradora><Convenio><IdentificadorConvenio>1792073634001</IdentificadorConvenio><RazonSocialConvenio>TOYOTA DEL ECUADOR </RazonSocialConvenio><DireccionConvenio>AV GALO PLAZA LASSO N69-309 Y SEBASTIAN MORENO.</DireccionConvenio><ConvencionalConvenio>0</ConvencionalConvenio><CelularConvenio>0986260084</CelularConvenio><EmailConvenio>CLOMAS@TDE.COM.EC</EmailConvenio></Convenio></Orden></Ordenes>";
                //string cadena = $@"<![CDATA[<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?><Contactos><Contacto><NumeroOrden>1025034733</NumeroOrden><UsuarioIngreso>OPERAD</UsuarioIngreso><CodigoVehiculo>BN0000000088</CodigoVehiculo><TipoContacto>1</TipoContacto><IdentificadorContacto>1792073634001</IdentificadorContacto><NombreContacto>TOYOTA DEL ECUADOR S.A.</NombreContacto><ApellidosContacto></ApellidosContacto><CelularContacto>0986260084</CelularContacto><EmailContacto><EmailContacto/></Contacto></Contactos>]]>";
                //string cadena = $@"<![CDATA[<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?><Ordenes><Orden><NumeroOrden> {p.numeroOrden}</NumeroOrden><UsuarioIngreso>{p.numeroOrden}</UsuarioIngreso><OperacionOrden>{p.numeroOrden}</OperacionOrden><NombreEjecutiva>{p.numeroOrden}</NombreEjecutiva><Ciudad>{p.numeroOrden}</Ciudad><Sucursal>{p.numeroOrden}</Sucursal><EstadoCartera>{p.numeroOrden}</EstadoCartera><FechaInicioCobertura>{p.numeroOrden}</FechaInicioCobertura><FechaFinCobertura>{p.numeroOrden}</FechaFinCobertura><Vehiculo><Placa>{p.numeroOrden}</Placa><IdMarca>{p.numeroOrden}</IdMarca><DescMarca>{p.numeroOrden}</DescMarca><IdModelo>{p.numeroOrden}</IdModelo><DescModelo>{p.numeroOrden}</DescModelo><CodigoVehiculo>{p.numeroOrden}</CodigoVehiculo><Chasis>{p.numeroOrden}</Chasis><Motor>{p.numeroOrden}</Motor><Color>{p.numeroOrden}</Color><Anio>{p.numeroOrden}</Anio><Tipo>{p.numeroOrden}</Tipo></Vehiculo><Dispositivo><Vid>{p.numeroOrden}</Vid><IdProducto>{p.numeroOrden}</IdProducto><DescProducto>{p.numeroOrden}</DescProducto><CodMarcaDispositivo>{p.numeroOrden}</CodMarcaDispositivo><MarcaDispositivo>{p.numeroOrden}</MarcaDispositivo><CodModeloDispositivo>{p.numeroOrden}</CodModeloDispositivo><ModeloDispositivo>{p.numeroOrden}</ModeloDispositivo><Sn>{p.numeroOrden}</Sn><Imei>{p.numeroOrden}</Imei><NumeroCamaras>{p.numeroOrden}</NumeroCamaras><DireccionMac>{p.numeroOrden}</DireccionMac><Icc>{p.numeroOrden}</Icc><NumeroCelular>{p.numeroOrden}</NumeroCelular><Operadora>{p.numeroOrden}</Operadora><EstadoSim>{p.numeroOrden}</EstadoSim><ServiciosInstalados><Servicio><CodServicio>{p.numeroOrden}</CodServicio><DescripcionServicio>{p.numeroOrden}</DescripcionServicio><FechaInicioServicio>{p.numeroOrden}</FechaInicioServicio><FechaFinServicio>{p.numeroOrden}</FechaFinServicio><EstadoServicio>ACTIVO</EstadoServicio></Servicio><Servicio><CodServicio>{p.numeroOrden}</CodServicio><DescripcionServicio>{p.numeroOrden}</DescripcionServicio><FechaInicioServicio>2025-02-03</FechaInicioServicio><FechaFinServicio>2026-02-03</FechaFinServicio><EstadoServicio>ACTIVO</EstadoServicio></Servicio></ServiciosInstalados><OperacionDispositivo>I</OperacionDispositivo><VidAnterior></VidAnterior></Dispositivo><Propietario><IdentificadorPropietario>0918485533</IdentificadorPropietario><NombrePropietario>ERWIN GUSTAVO</NombrePropietario><ApellidosPropietario>ESCALANTE RAMIREZ</ApellidosPropietario><DireccionPropietario>ALBORADA 11AVA ETAPA</DireccionPropietario><ConvencionalPropietario>046011450</ConvencionalPropietario><CelularPropietario>0998679101</CelularPropietario><EmailPropietario>ERGU.ESCALANTE@GMAIL.COM</EmailPropietario></Propietario><Monitor><IdentificadorMonitorea>0918485533</IdentificadorMonitorea><NombreMonitorea>ERWIN GUSTAVO</NombreMonitorea><ApellidosMonitorea>ESCALANTE RAMIREZ</ApellidosMonitorea><DireccionMonitorea>ALBORADA 11AVA ETAPA&lt;</DireccionMonitorea><ConvencionalMonitorea>046011450</ConvencionalMonitorea><CelularMonitorea>0998679101</CelularMonitorea><EmailMonitorea>ERGU.ESCALANTE@GMAIL.COM</EmailMonitorea></Monitor><Concesionario><IdentificadorConcesionario>9980000000006</IdentificadorConcesionario><RazonSocialConcesionario>SIN CONCESIONARIO</RazonSocialConcesionario><DireccionConcesionario></DireccionConcesionario><ConvencionalConcesionario>0</ConvencionalConcesionario><CelularConcesionario>0</CelularConcesionario><EmailConcesionario></EmailConcesionario></Concesionario><Financiera><IdentificadorFinanciera>9980000000004</IdentificadorFinanciera><RazonSocialFinanciera>SIN BANCO/FINANCIERA</RazonSocialFinanciera><DireccionFinanciera></DireccionFinanciera><ConvencionalFinanciera>0</ConvencionalFinanciera><CelularFinanciera>0</CelularFinanciera><EmailFinanciera></EmailFinanciera></Financiera><Aseguradora><IdentificadorAseguradora>9980000000005</IdentificadorAseguradora><RazonSocialAseguradora>SIN ASEGURADORA</RazonSocialAseguradora><DireccionAseguradora>ECUADOR CARSEG</DireccionAseguradora><ConvencionalAseguradora>0</ConvencionalAseguradora><CelularAseguradora>0</CelularAseguradora><EmailAseguradora></EmailAseguradora></Aseguradora><Convenio /></Orden></Ordenes>]]>";
                string cadena = $@"<![CDATA[<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?><Contactos><Contacto><NumeroOrden>{p.orden}</NumeroOrden><UsuarioIngreso>{p.usuarioIngreso}</UsuarioIngreso><CodigoVehiculo>{p.vehiculo}</CodigoVehiculo><TipoContacto>{p.tipoContacto}</TipoContacto><IdentificadorContacto>{p.identificacion}</IdentificadorContacto><NombreContacto>{p.nombreContacto}</NombreContacto><ApellidosContacto>{p.apellidoContacto}</ApellidosContacto><CelularContacto>{p.celularContacto}</CelularContacto><EmailContacto/></Contacto></Contactos>]]>";

                string soapRequest = $@"<?xml version=""1.0"" encoding=""utf-8"" standalone=""no""?><soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/""><soap:Header><SeguridadPx xmlns=""http://tempuri.org/""><StrToken>{p.token}</StrToken><AutenticacionToken>{resultToken}</AutenticacionToken><UserName>{p.usuario}</UserName><Password>{p.pass}</Password></SeguridadPx></soap:Header><soap:Body><ContactoEmergencia xmlns=""http://tempuri.org/""><strXml>{cadena}</strXml><strNacionalidad>EC</strNacionalidad></ContactoEmergencia></soap:Body></soap:Envelope>";
                try
                {
                    // Crear la solicitud HTTP
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(rutaurl);
                    request.Headers.Add("SOAPAction", rutaContacto);
                    request.ContentType = "text/xml; charset=utf-8";
                    request.Method = "POST";
                    // Escribir el contenido de la solicitud en el cuerpo
                    using (Stream stream = request.GetRequestStream())
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(soapRequest);
                        stream.Write(bytes, 0, bytes.Length);
                    }
                    // Obtener la respuesta
                    using (WebResponse response = request.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            string responseXml = reader.ReadToEnd();
                            Console.WriteLine("Respuesta del servicio:");
                            Console.WriteLine(responseXml);
                            // Procesar la respuesta
                            string result = ExtractResponseData(responseXml, "ContactoEmergencia");
                            Console.WriteLine("Resultado procesado:");
                            Console.WriteLine(result);
                            // Procesar el XML de la respuesta (si es necesario)
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.LoadXml(responseXml);
                            // Namespace manager para manejar prefijos de namespaces
                            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
                            nsmgr.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");
                            nsmgr.AddNamespace("tempuri", "http://tempuri.org/");
                            // Buscar el nodo de resultado (ajustar el XPath según la respuesta real)
                            XmlNode resultNode = xmlDoc.SelectSingleNode("//tempuri:ContactoEmergenciaResult", nsmgr);
                            if (resultNode != null)
                            {
                                string resultText = resultNode.InnerText;
                                Console.WriteLine("Resultado de ContactoEmergencia: " + result);
                                _respuestaApi.StatusCode = System.Net.HttpStatusCode.OK;
                                _respuestaApi.IsSuccess = true;
                                _respuestaApi.Messages.Add("Grabado con exito");
                                _respuestaApi.Result = resultText;
                                return Ok(_respuestaApi);
                            }
                            else
                            {
                                Console.WriteLine("No se encontró el nodo ContactoEmergenciaResult en la respuesta.");
                                _respuestaApi.StatusCode = System.Net.HttpStatusCode.BadRequest;
                                _respuestaApi.IsSuccess = false;
                                _respuestaApi.Messages.Add("No se encontró el nodo ContactoEmergenciaResult en la respuesta.");
                                //mensaje = _respuestaApi.Messages[0];
                                //respuesta = ("{\"respuesta\":" + _respuestaApi.Messages[0] + "}");
                                return Ok(_respuestaApi);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al consumir el servicio: " + ex.Message);
                    _respuestaApi.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    _respuestaApi.IsSuccess = false;
                    _respuestaApi.Messages.Add(ex.Message);
                    //mensaje = _respuestaApi.Messages[0];
                    //respuesta = ("{\"respuesta\":" + _respuestaApi.Messages[0] + "}");
                    return Ok(_respuestaApi);
                }
            }
        }


        [HttpPost]
        // [Authorize]
        [SwaggerOperation(Summary = "", Description = "Consulta Informacion de los Comando que se pueden ejecutar")]
        [Route("EjecutarComando")]
        public async Task<ActionResult<string>> EjecutarComando(GeneralPXDTO p)
        {
            DataSet infoDatos = new DataSet();
            //conecion PX
            conexionPX ruta = new conexionPX();
            string rutaurl = ruta.GetRuta("1"); ;
            string rutaEjecutarComando = ruta.GetRuta("9");
            string resultToken = ruta.GetToken(p.usuario, p.pass, p.token);
            if (resultToken == "-1")
            {
                _respuestaApi.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.Messages.Add("Token Generado de forma incorrecta.");
                // return Ok(_respuestaApi);
            }
            else if (resultToken == "-2")
            {
                _respuestaApi.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.Messages.Add("No se encontró el nodo AutenticacionUsuarioPxResult en la respuesta.");
                //return Ok(_respuestaApi);
            }
            else
            {
                // no presenta datos no consulta
                string soapRequest = $@"<?xml version=""1.0"" encoding=""utf-8"" standalone=""no""?><soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/""><soap:Header><SeguridadPx xmlns=""http://tempuri.org/""><StrToken>{p.token}</StrToken><AutenticacionToken>{resultToken}</AutenticacionToken><UserName>{p.usuario}</UserName><Password>{p.pass}</Password></SeguridadPx></soap:Header><soap:Body><EjecutaComandos xmlns=""http://tempuri.org/""><usuarioCliente>{p.loginUsuario}</usuarioCliente><claveCliente>{p.passUsuario}</claveCliente><vid>{p.vid}</vid><codigoProducto>{p.codProducto}</codigoProducto><codigoTipoDispositivo>{p.tipoDispositivo}</codigoTipoDispositivo><codigoComando>{p.idComando}</codigoComando><strNacionalidad>{p.nacionalidad}</strNacionalidad></EjecutaComandos></soap:Body></soap:Envelope>";
                try
                {
                    // Crear la solicitud HTTP
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(rutaurl);
                    request.Headers.Add("SOAPAction", rutaEjecutarComando);
                    request.ContentType = "text/xml; charset=utf-8";
                    request.Method = "POST";
                    // Escribir el contenido de la solicitud en el cuerpo
                    using (Stream stream = request.GetRequestStream())
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(soapRequest);
                        stream.Write(bytes, 0, bytes.Length);
                    }
                    // Obtener la respuesta
                    using (WebResponse response = request.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            string responseXml = reader.ReadToEnd();
                            Console.WriteLine("Respuesta del servicio:");
                            Console.WriteLine(responseXml);
                            // Procesar la respuesta
                            string result = ExtractResponseData(responseXml, "EjecutaComandos");
                            Console.WriteLine("Resultado procesado:");
                            Console.WriteLine(result);
                            // Procesar el XML de la respuesta (si es necesario)
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.LoadXml(responseXml);
                            XmlNode node = xmlDoc.SelectSingleNode("//NewDataSet");
                            if (node != null)
                            {
                                using (StringReader stringReader = new StringReader(node.OuterXml))
                                {
                                    infoDatos.ReadXml(stringReader);
                                }
                            }
                            //DTO.PX.RespuestaPX respuesta = null;
                            if (infoDatos.Tables[0].Rows.Count > 0)
                            {
                                _respuestaApi.StatusCode = System.Net.HttpStatusCode.OK;
                                _respuestaApi.IsSuccess = true;
                                _respuestaApi.Messages.Add("Grabado con exito");
                                //_respuestaApi.Result = respuesta;
                                //    // return Ok(_respuestaApi);
                            }
                            else
                            {
                                Console.WriteLine("No se encontró el nodo EjecutaComandos en la respuesta.");
                                _respuestaApi.StatusCode = System.Net.HttpStatusCode.BadRequest;
                                _respuestaApi.IsSuccess = false;
                                _respuestaApi.Messages.Add("No existen datos que mostrar.");
                                //mensaje = _respuestaApi.Messages[0];
                                //respuesta = ("{\"respuesta\":" + _respuestaApi.Messages[0] + "}");
                                //return Ok(_respuestaApi);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al consumir el servicio: " + ex.Message);
                    _respuestaApi.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    _respuestaApi.IsSuccess = false;
                    _respuestaApi.Messages.Add(ex.Message);
                    //return Ok(_respuestaApi);
                }
            }
            string jsonData;
            if (infoDatos.Tables[0].Rows.Count > 0)
            {
                jsonData = (string)Utilidades.DataTableToJSON(infoDatos.Tables[0]);
                jsonData = "{\"results\":" + jsonData + "}";
            }
            else
            {
                jsonData = "{\"results\":" + _respuestaApi.Messages[0] + "}";
            }
            //jsonData = (string)Utilidades.DataTableToJSON(_respuestaApi.Messages[0]);
            return jsonData;
        }


        [HttpPost]
        //[Authorize]
        [SwaggerOperation(Summary = "", Description = "Enviar datos para la plataforma PX")]
        [Route("InsertaOrdenV2")]
        public ActionResult<string> InsertaOrdenV2(GeneralPXDTO p)
        {
            //conecion PX
            conexionPX ruta = new conexionPX();
            string rutaurl = ruta.GetRuta("1"); ;
            string rutaInsertaOrden = ruta.GetRuta("3");
            string resultToken = ruta.GetToken(p.usuario, p.pass, p.token);
            if (resultToken =="-1")
            {
                _respuestaApi.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.Messages.Add("Token Generado de forma incorrecta.");
                return Ok(_respuestaApi);
            }
            else if (resultToken == "-2")
            {
                _respuestaApi.StatusCode = System.Net.HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.Messages.Add("No se encontró el nodo AutenticacionUsuarioPxResult en la respuesta.");
                return Ok(_respuestaApi);
            }
            else 
            {
                //string cadena = $@"<?xml version=""1.0"" encoding=""utf-8""?><Ordenes><Orden><NumeroOrden>1025034733</NumeroOrden><UsuarioIngreso>OPERAD</UsuarioIngreso><OperacionOrden>001</OperacionOrden><NombreEjecutiva>PIERINA GOMEZ</NombreEjecutiva><Ciudad>Guayaquil</Ciudad><Sucursal>Guayaquil</Sucursal><EstadoCartera>ACTIVO</EstadoCartera><FechaInicioCobertura>2025-01-23</FechaInicioCobertura><FechaFinCobertura>2026-01-23</FechaFinCobertura><Vehiculo><Placa>S/P</Placa><IdMarca>283</IdMarca><DescMarca>TOYOTA</DescMarca><IdModelo>2226</IdModelo><DescModelo>NEW FORTUNER AC 2.7 5P 4X4 TM</DescModelo><CodigoVehiculo>BN0000000088</CodigoVehiculo><Chasis>MHFDX3FS8S0055343</Chasis><Motor>2TRB233318</Motor><Color>NEGRO</Color><Anio>2025</Anio><Tipo>SUV</Tipo></Vehiculo><Dispositivo><Vid>4764437456</Vid><IdProducto>HT</IdProducto><DescProducto>HUNTER GPS HMT</DescProducto><CodMarcaDispositivo>21</CodMarcaDispositivo><MarcaDispositivo>CALAMP</MarcaDispositivo><CodModeloDispositivo>307</CodModeloDispositivo><ModeloDispositivo>LMU2630 3G</ModeloDispositivo><Sn>4764437456</Sn><Imei>352556103427543</Imei><NumeroCamaras>0</NumeroCamaras><DireccionMac>0</DireccionMac><Icc>895930100109516616</Icc><NumeroCelular>0989958084</NumeroCelular><Operadora>CLARO</Operadora><EstadoSim>ACT</EstadoSim><ServiciosInstalados><Servicio><CodServicio>001</CodServicio><DescripcionServicio>APERTURA DE SEGUROS</DescripcionServicio><FechaInicioServicio>2025-01-23</FechaInicioServicio><FechaFinServicio>2026-01-23</FechaFinServicio><EstadoServicio>ACTIVO</EstadoServicio></Servicio><Servicio><CodServicio>002</CodServicio><DescripcionServicio>SERVICIOS HM AMI</DescripcionServicio><FechaInicioServicio>2025-01-23</FechaInicioServicio><FechaFinServicio>2026-01-23</FechaFinServicio><EstadoServicio>ACTIVO</EstadoServicio></Servicio></ServiciosInstalados><OperacionDispositivo>I</OperacionDispositivo><VidAnterior></VidAnterior></Dispositivo><Propietario><IdentificadorPropietario>1792073634001</IdentificadorPropietario><NombrePropietario>TOYOTA DEL ECUADOR S.A. </NombrePropietario><ApellidosPropietario></ApellidosPropietario><DireccionPropietario></DireccionPropietario><ConvencionalPropietario>0986260084</ConvencionalPropietario><CelularPropietario>0986260084</CelularPropietario><EmailPropietario>clomas@tde.com.ec</EmailPropietario></Propietario><Monitor><IdentificadorMonitorea>1792073634001</IdentificadorMonitorea><NombreMonitorea>TOYOTA DEL ECUADOR S.A. </NombreMonitorea><ApellidosMonitorea></ApellidosMonitorea><DireccionMonitorea></DireccionMonitorea><ConvencionalMonitorea>0986260084</ConvencionalMonitorea><CelularMonitorea>0986260084</CelularMonitorea><EmailMonitorea>clomas@tde.com.ec</EmailMonitorea></Monitor><Concesionario><IdentificadorConcesionario>0190003701001</IdentificadorConcesionario><RazonSocialConcesionario>IMPORTADORA TOMEBAMBA</RazonSocialConcesionario><DireccionConcesionario>AV. ESPAÑA 1730</DireccionConcesionario><ConvencionalConcesionario>0</ConvencionalConcesionario><CelularConcesionario>0</CelularConcesionario><EmailConcesionario></EmailConcesionario></Concesionario><Financiera><IdentificadorFinanciera>9980000000004</IdentificadorFinanciera><RazonSocialFinanciera>SIN BANCO / FINANCIERA</RazonSocialFinanciera><DireccionFinanciera></DireccionFinanciera><ConvencionalFinanciera>0</ConvencionalFinanciera><CelularFinanciera>0</CelularFinanciera><EmailFinanciera></EmailFinanciera></Financiera><Aseguradora><IdentificadorAseguradora>9980000000005</IdentificadorAseguradora><RazonSocialAseguradora>SIN ASEGURADORA</RazonSocialAseguradora><DireccionAseguradora></DireccionAseguradora><ConvencionalAseguradora>0</ConvencionalAseguradora><CelularAseguradora>0</CelularAseguradora><EmailAseguradora></EmailAseguradora></Aseguradora><Convenio><IdentificadorConvenio>1792073634001</IdentificadorConvenio><RazonSocialConvenio>TOYOTA DEL ECUADOR </RazonSocialConvenio><DireccionConvenio>AV GALO PLAZA LASSO N69-309 Y SEBASTIAN MORENO.</DireccionConvenio><ConvencionalConvenio>0</ConvencionalConvenio><CelularConvenio>0986260084</CelularConvenio><EmailConvenio>CLOMAS@TDE.COM.EC</EmailConvenio></Convenio></Orden></Ordenes>";
                //string cadena = $@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no"" ?><Ordenes><Orden><NumeroOrden>202502031121</NumeroOrden><UsuarioIngreso>WSLTM202502031121</UsuarioIngreso><OperacionOrden>001</OperacionOrden><NombreEjecutiva>EJECUTIVA LATAM</NombreEjecutiva><Ciudad>CIUDAD LATAM</Ciudad><Sucursal>SUCURSAL LATAM</Sucursal><EstadoCartera>ESTADO CARTERA LATAM</EstadoCartera><FechaInicioCobertura>2025-02-03</FechaInicioCobertura><FechaFinCobertura>2027-02-04</FechaFinCobertura><Vehiculo><Placa>VIRTUAL-LATAM-202502031121</Placa><IdMarca>000</IdMarca><DescMarca>NO DEFINIDO</DescMarca><IdModelo>000</IdModelo><DescModelo>NO DEFINIDO</DescModelo><CodigoVehiculo>WSLTM202502031121</CodigoVehiculo><Chasis>0</Chasis><Motor>0</Motor><Color>AMARILLO</Color><Anio>2024</Anio><Tipo>TERRESTRE</Tipo></Vehiculo><Dispositivo><Vid>WSLTM202502031121</Vid><IdProducto>00</IdProducto><DescProducto>NO DEFINIDO</DescProducto><CodMarcaDispositivo>000</CodMarcaDispositivo><MarcaDispositivo>NO DEFINIDO</MarcaDispositivo><CodModeloDispositivo>000</CodModeloDispositivo><ModeloDispositivo>NO DEFINIDO</ModeloDispositivo><Sn>0</Sn><Imei>0</Imei><NumeroCamaras>0</NumeroCamaras><DireccionMac>0.0.0.0</DireccionMac><Icc>0</Icc><NumeroCelular>0</NumeroCelular><Operadora>TELEFONICA</Operadora><EstadoSim>ACTIVO</EstadoSim><ServiciosInstalados><Servicio><CodServicio>100</CodServicio><DescripcionServicio>APERTURA DE SEGUROS</DescripcionServicio><FechaInicioServicio>2025-02-03</FechaInicioServicio><FechaFinServicio>2026-02-03</FechaFinServicio><EstadoServicio>ACTIVO</EstadoServicio></Servicio><Servicio><CodServicio>200</CodServicio><DescripcionServicio>PARALIZACIÓN DE VEHÍCULO</DescripcionServicio><FechaInicioServicio>2025-02-03</FechaInicioServicio><FechaFinServicio>2026-02-03</FechaFinServicio><EstadoServicio>ACTIVO</EstadoServicio></Servicio></ServiciosInstalados><OperacionDispositivo>I</OperacionDispositivo><VidAnterior></VidAnterior></Dispositivo><Propietario><IdentificadorPropietario>0918485533</IdentificadorPropietario><NombrePropietario>ERWIN GUSTAVO</NombrePropietario><ApellidosPropietario>ESCALANTE RAMIREZ</ApellidosPropietario><DireccionPropietario>ALBORADA 11AVA ETAPA</DireccionPropietario><ConvencionalPropietario>046011450</ConvencionalPropietario><CelularPropietario>0998679101</CelularPropietario><EmailPropietario>ERGU.ESCALANTE@GMAIL.COM</EmailPropietario></Propietario><Monitor><IdentificadorMonitorea>0918485533</IdentificadorMonitorea><NombreMonitorea>ERWIN GUSTAVO</NombreMonitorea><ApellidosMonitorea>ESCALANTE RAMIREZ</ApellidosMonitorea><DireccionMonitorea>ALBORADA 11AVA ETAPA&lt;</DireccionMonitorea><ConvencionalMonitorea>046011450</ConvencionalMonitorea><CelularMonitorea>0998679101</CelularMonitorea><EmailMonitorea>ERGU.ESCALANTE@GMAIL.COM</EmailMonitorea></Monitor><Concesionario><IdentificadorConcesionario>9980000000006</IdentificadorConcesionario><RazonSocialConcesionario>SIN CONCESIONARIO</RazonSocialConcesionario><DireccionConcesionario></DireccionConcesionario><ConvencionalConcesionario>0</ConvencionalConcesionario><CelularConcesionario>0</CelularConcesionario><EmailConcesionario></EmailConcesionario></Concesionario><Financiera><IdentificadorFinanciera>9980000000004</IdentificadorFinanciera><RazonSocialFinanciera>SIN BANCO/FINANCIERA</RazonSocialFinanciera><DireccionFinanciera></DireccionFinanciera><ConvencionalFinanciera>0</ConvencionalFinanciera><CelularFinanciera>0</CelularFinanciera><EmailFinanciera></EmailFinanciera></Financiera><Aseguradora><IdentificadorAseguradora>9980000000005</IdentificadorAseguradora><RazonSocialAseguradora>SIN ASEGURADORA</RazonSocialAseguradora><DireccionAseguradora>ECUADOR CARSEG</DireccionAseguradora><ConvencionalAseguradora>0</ConvencionalAseguradora><CelularAseguradora>0</CelularAseguradora><EmailAseguradora></EmailAseguradora></Aseguradora><Convenio /></Orden></Ordenes>";
                string cadena = $@"<![CDATA[<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?><Ordenes><Orden><NumeroOrden>202502051315</NumeroOrden><UsuarioIngreso>WSLTM202502031121</UsuarioIngreso><OperacionOrden>001</OperacionOrden><NombreEjecutiva>EJECUTIVA LATAM</NombreEjecutiva><Ciudad>CIUDAD LATAM</Ciudad><Sucursal>SUCURSAL LATAM</Sucursal><EstadoCartera>ESTADO CARTERA LATAM</EstadoCartera><FechaInicioCobertura>2025-02-03</FechaInicioCobertura><FechaFinCobertura>2027-02-04</FechaFinCobertura><Vehiculo><Placa>VIRTUAL-LATAM-202502031121</Placa><IdMarca>000</IdMarca><DescMarca>NO DEFINIDO</DescMarca><IdModelo>000</IdModelo><DescModelo>NO DEFINIDO</DescModelo><CodigoVehiculo>WSLTM202502031121</CodigoVehiculo><Chasis>0</Chasis><Motor>0</Motor><Color>AMARILLO</Color><Anio>2024</Anio><Tipo>TERRESTRE</Tipo></Vehiculo><Dispositivo><Vid>WSLTM202502031121</Vid><IdProducto>00</IdProducto><DescProducto>NO DEFINIDO</DescProducto><CodMarcaDispositivo>000</CodMarcaDispositivo><MarcaDispositivo>NO DEFINIDO</MarcaDispositivo><CodModeloDispositivo>000</CodModeloDispositivo><ModeloDispositivo>NO DEFINIDO</ModeloDispositivo><Sn>0</Sn><Imei>0</Imei><NumeroCamaras>0</NumeroCamaras><DireccionMac>0.0.0.0</DireccionMac><Icc>0</Icc><NumeroCelular>0</NumeroCelular><Operadora>TELEFONICA</Operadora><EstadoSim>ACTIVO</EstadoSim><ServiciosInstalados><Servicio><CodServicio>100</CodServicio><DescripcionServicio>APERTURA DE SEGUROS</DescripcionServicio><FechaInicioServicio>2025-02-03</FechaInicioServicio><FechaFinServicio>2026-02-03</FechaFinServicio><EstadoServicio>ACTIVO</EstadoServicio></Servicio><Servicio><CodServicio>200</CodServicio><DescripcionServicio>PARALIZACIÓN DE VEHÍCULO</DescripcionServicio><FechaInicioServicio>2025-02-03</FechaInicioServicio><FechaFinServicio>2026-02-03</FechaFinServicio><EstadoServicio>ACTIVO</EstadoServicio></Servicio></ServiciosInstalados><OperacionDispositivo>I</OperacionDispositivo><VidAnterior></VidAnterior></Dispositivo><Propietario><IdentificadorPropietario>0918485533</IdentificadorPropietario><NombrePropietario>ERWIN GUSTAVO</NombrePropietario><ApellidosPropietario>ESCALANTE RAMIREZ</ApellidosPropietario><DireccionPropietario>ALBORADA 11AVA ETAPA</DireccionPropietario><ConvencionalPropietario>046011450</ConvencionalPropietario><CelularPropietario>0998679101</CelularPropietario><EmailPropietario>ERGU.ESCALANTE@GMAIL.COM</EmailPropietario></Propietario><Monitor><IdentificadorMonitorea>0918485533</IdentificadorMonitorea><NombreMonitorea>ERWIN GUSTAVO</NombreMonitorea><ApellidosMonitorea>ESCALANTE RAMIREZ</ApellidosMonitorea><DireccionMonitorea>ALBORADA 11AVA ETAPA&lt;</DireccionMonitorea><ConvencionalMonitorea>046011450</ConvencionalMonitorea><CelularMonitorea>0998679101</CelularMonitorea><EmailMonitorea>ERGU.ESCALANTE@GMAIL.COM</EmailMonitorea></Monitor><Concesionario><IdentificadorConcesionario>9980000000006</IdentificadorConcesionario><RazonSocialConcesionario>SIN CONCESIONARIO</RazonSocialConcesionario><DireccionConcesionario></DireccionConcesionario><ConvencionalConcesionario>0</ConvencionalConcesionario><CelularConcesionario>0</CelularConcesionario><EmailConcesionario></EmailConcesionario></Concesionario><Financiera><IdentificadorFinanciera>9980000000004</IdentificadorFinanciera><RazonSocialFinanciera>SIN BANCO/FINANCIERA</RazonSocialFinanciera><DireccionFinanciera></DireccionFinanciera><ConvencionalFinanciera>0</ConvencionalFinanciera><CelularFinanciera>0</CelularFinanciera><EmailFinanciera></EmailFinanciera></Financiera><Aseguradora><IdentificadorAseguradora>9980000000005</IdentificadorAseguradora><RazonSocialAseguradora>SIN ASEGURADORA</RazonSocialAseguradora><DireccionAseguradora>ECUADOR CARSEG</DireccionAseguradora><ConvencionalAseguradora>0</ConvencionalAseguradora><CelularAseguradora>0</CelularAseguradora><EmailAseguradora></EmailAseguradora></Aseguradora><Convenio /></Orden></Ordenes>]]>";
                //string cadena = $@"<![CDATA[<?xml version=""1.0"" encoding=""UTF-8"" standalone=""no""?><Ordenes><Orden><NumeroOrden> {p.numeroOrden}</NumeroOrden><UsuarioIngreso>{p.numeroOrden}</UsuarioIngreso><OperacionOrden>{p.numeroOrden}</OperacionOrden><NombreEjecutiva>{p.numeroOrden}</NombreEjecutiva><Ciudad>{p.numeroOrden}</Ciudad><Sucursal>{p.numeroOrden}</Sucursal><EstadoCartera>{p.numeroOrden}</EstadoCartera><FechaInicioCobertura>{p.numeroOrden}</FechaInicioCobertura><FechaFinCobertura>{p.numeroOrden}</FechaFinCobertura><Vehiculo><Placa>{p.numeroOrden}</Placa><IdMarca>{p.numeroOrden}</IdMarca><DescMarca>{p.numeroOrden}</DescMarca><IdModelo>{p.numeroOrden}</IdModelo><DescModelo>{p.numeroOrden}</DescModelo><CodigoVehiculo>{p.numeroOrden}</CodigoVehiculo><Chasis>{p.numeroOrden}</Chasis><Motor>{p.numeroOrden}</Motor><Color>{p.numeroOrden}</Color><Anio>{p.numeroOrden}</Anio><Tipo>{p.numeroOrden}</Tipo></Vehiculo><Dispositivo><Vid>{p.numeroOrden}</Vid><IdProducto>{p.numeroOrden}</IdProducto><DescProducto>{p.numeroOrden}</DescProducto><CodMarcaDispositivo>{p.numeroOrden}</CodMarcaDispositivo><MarcaDispositivo>{p.numeroOrden}</MarcaDispositivo><CodModeloDispositivo>{p.numeroOrden}</CodModeloDispositivo><ModeloDispositivo>{p.numeroOrden}</ModeloDispositivo><Sn>{p.numeroOrden}</Sn><Imei>{p.numeroOrden}</Imei><NumeroCamaras>{p.numeroOrden}</NumeroCamaras><DireccionMac>{p.numeroOrden}</DireccionMac><Icc>{p.numeroOrden}</Icc><NumeroCelular>{p.numeroOrden}</NumeroCelular><Operadora>{p.numeroOrden}</Operadora><EstadoSim>{p.numeroOrden}</EstadoSim><ServiciosInstalados><Servicio><CodServicio>{p.numeroOrden}</CodServicio><DescripcionServicio>{p.numeroOrden}</DescripcionServicio><FechaInicioServicio>{p.numeroOrden}</FechaInicioServicio><FechaFinServicio>{p.numeroOrden}</FechaFinServicio><EstadoServicio>ACTIVO</EstadoServicio></Servicio><Servicio><CodServicio>{p.numeroOrden}</CodServicio><DescripcionServicio>{p.numeroOrden}</DescripcionServicio><FechaInicioServicio>2025-02-03</FechaInicioServicio><FechaFinServicio>2026-02-03</FechaFinServicio><EstadoServicio>ACTIVO</EstadoServicio></Servicio></ServiciosInstalados><OperacionDispositivo>I</OperacionDispositivo><VidAnterior></VidAnterior></Dispositivo><Propietario><IdentificadorPropietario>0918485533</IdentificadorPropietario><NombrePropietario>ERWIN GUSTAVO</NombrePropietario><ApellidosPropietario>ESCALANTE RAMIREZ</ApellidosPropietario><DireccionPropietario>ALBORADA 11AVA ETAPA</DireccionPropietario><ConvencionalPropietario>046011450</ConvencionalPropietario><CelularPropietario>0998679101</CelularPropietario><EmailPropietario>ERGU.ESCALANTE@GMAIL.COM</EmailPropietario></Propietario><Monitor><IdentificadorMonitorea>0918485533</IdentificadorMonitorea><NombreMonitorea>ERWIN GUSTAVO</NombreMonitorea><ApellidosMonitorea>ESCALANTE RAMIREZ</ApellidosMonitorea><DireccionMonitorea>ALBORADA 11AVA ETAPA&lt;</DireccionMonitorea><ConvencionalMonitorea>046011450</ConvencionalMonitorea><CelularMonitorea>0998679101</CelularMonitorea><EmailMonitorea>ERGU.ESCALANTE@GMAIL.COM</EmailMonitorea></Monitor><Concesionario><IdentificadorConcesionario>9980000000006</IdentificadorConcesionario><RazonSocialConcesionario>SIN CONCESIONARIO</RazonSocialConcesionario><DireccionConcesionario></DireccionConcesionario><ConvencionalConcesionario>0</ConvencionalConcesionario><CelularConcesionario>0</CelularConcesionario><EmailConcesionario></EmailConcesionario></Concesionario><Financiera><IdentificadorFinanciera>9980000000004</IdentificadorFinanciera><RazonSocialFinanciera>SIN BANCO/FINANCIERA</RazonSocialFinanciera><DireccionFinanciera></DireccionFinanciera><ConvencionalFinanciera>0</ConvencionalFinanciera><CelularFinanciera>0</CelularFinanciera><EmailFinanciera></EmailFinanciera></Financiera><Aseguradora><IdentificadorAseguradora>9980000000005</IdentificadorAseguradora><RazonSocialAseguradora>SIN ASEGURADORA</RazonSocialAseguradora><DireccionAseguradora>ECUADOR CARSEG</DireccionAseguradora><ConvencionalAseguradora>0</ConvencionalAseguradora><CelularAseguradora>0</CelularAseguradora><EmailAseguradora></EmailAseguradora></Aseguradora><Convenio /></Orden></Ordenes>]]>";
                string soapRequest = $@"<?xml version=""1.0"" encoding=""utf-8"" standalone=""no""?><soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/""><soap:Header><SeguridadPx xmlns=""http://tempuri.org/""><StrToken>{p.token}</StrToken><AutenticacionToken>{resultToken}</AutenticacionToken><UserName>{p.usuario}</UserName><Password>{p.pass}</Password></SeguridadPx></soap:Header><soap:Body><InsertaOrdenV2 xmlns=""http://tempuri.org/""><strXml>{cadena}</strXml><strNacionalidad>EC</strNacionalidad></InsertaOrdenV2></soap:Body></soap:Envelope>";             
                try
                {
                    // Crear la solicitud HTTP
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(rutaurl);
                    request.Headers.Add("SOAPAction", rutaInsertaOrden);
                    request.ContentType = "text/xml; charset=utf-8";
                    request.Method = "POST";
                    // Escribir el contenido de la solicitud en el cuerpo
                    using (Stream stream = request.GetRequestStream())
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(soapRequest);
                        stream.Write(bytes, 0, bytes.Length);
                    }
                    // Obtener la respuesta
                    using (WebResponse response = request.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            string responseXml = reader.ReadToEnd();
                            Console.WriteLine("Respuesta del servicio:");
                            Console.WriteLine(responseXml);
                            // Procesar la respuesta
                            string result = ExtractResponseData(responseXml, "InsertaOrdenV2");
                            Console.WriteLine("Resultado procesado:");
                            Console.WriteLine(result);
                            // Procesar el XML de la respuesta (si es necesario)
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.LoadXml(responseXml);
                            // Namespace manager para manejar prefijos de namespaces
                            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
                            nsmgr.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");
                            nsmgr.AddNamespace("tempuri", "http://tempuri.org/");
                            // Buscar el nodo de resultado (ajustar el XPath según la respuesta real)
                            XmlNode resultNode = xmlDoc.SelectSingleNode("//tempuri:InsertaOrdenV2Result", nsmgr);
                            if (resultNode != null)
                            {
                                string resultText = resultNode.InnerText;
                                Console.WriteLine("Resultado de InsertaOrdenV2: " + result);
                                _respuestaApi.StatusCode = System.Net.HttpStatusCode.OK;
                                _respuestaApi.IsSuccess = true;
                                _respuestaApi.Messages.Add("Grabado con exito");
                                _respuestaApi.Result = resultText;
                                return Ok(_respuestaApi);
                            }
                            else
                            {
                                Console.WriteLine("No se encontró el nodo InsertaOrdenV2Result en la respuesta.");
                                _respuestaApi.StatusCode = System.Net.HttpStatusCode.BadRequest;
                                _respuestaApi.IsSuccess = false;
                                _respuestaApi.Messages.Add("No se encontró el nodo AutenticacionUsuarioPxResult en la respuesta.");
                                //mensaje = _respuestaApi.Messages[0];
                                //respuesta = ("{\"respuesta\":" + _respuestaApi.Messages[0] + "}");
                                return Ok(_respuestaApi);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al consumir el servicio: " + ex.Message);
                    _respuestaApi.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    _respuestaApi.IsSuccess = false;
                    _respuestaApi.Messages.Add(ex.Message);
                    //mensaje = _respuestaApi.Messages[0];
                    //respuesta = ("{\"respuesta\":" + _respuestaApi.Messages[0] + "}");
                    return Ok(_respuestaApi);
                }
            }
        }


        [HttpPost]
        [Authorize]
        [SwaggerOperation(Summary = "", Description = "Enviar datos para la plataforma PX")]
        [Route("ProcesarPX")]
        public ActionResult<string> ProcesarPX(GeneralPXDTO p)
        {
            //conecion PX
            conexionPX ruta = new conexionPX();
            string rutaurl = ruta.GetRuta("1"); ;
            string rutaInsertaOrden = ruta.GetRuta("3");
            DataTable tabla = new DataTable();
            tabla.Columns.Add("CODIGO", typeof(int));
            tabla.Columns.Add("MENSAJE", typeof(string));
            string resultToken = ruta.GetToken(p.usuario, p.pass, p.token);
            if (resultToken == "-1")
            {
                //_respuestaApi.StatusCode = System.Net.HttpStatusCode.BadRequest;
                //_respuestaApi.IsSuccess = false;
                //_respuestaApi.Messages.Add("Token Generado de forma incorrecta.");
                //return Ok(_respuestaApi);
                tabla.Rows.Add("-1", "Token Generado de forma incorrecta.");
            }
            else if (resultToken == "-2")
            {
                //_respuestaApi.StatusCode = System.Net.HttpStatusCode.BadRequest;
                //_respuestaApi.IsSuccess = false;
                //_respuestaApi.Messages.Add("No se encontró el nodo AutenticacionUsuarioPxResult en la respuesta.");
                //return Ok(_respuestaApi);
                tabla.Rows.Add("-1", "No se encontró el nodo AutenticacionUsuarioPxResult en la respuesta.");
            }
            else
            {
                DataSet infoDatosXml = new DataSet();
                infoDatosXml = ProcesoPXSQL.ObtenerXml(p, "201");
                string cadena = $@"<![CDATA["+ (string)infoDatosXml.Tables[0].Rows[0]["XML_STRING"] + "]]>";
                string soapRequest = $@"<?xml version=""1.0"" encoding=""utf-8""?><soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/""><soap:Header><SeguridadPx xmlns=""http://tempuri.org/""><StrToken>{p.token}</StrToken><AutenticacionToken>{resultToken}</AutenticacionToken><UserName>{p.usuario}</UserName><Password>{p.pass}</Password></SeguridadPx></soap:Header><soap:Body><InsertaOrdenV2 xmlns=""http://tempuri.org/""><strXml>{cadena}</strXml><strNacionalidad>{p.nacionalidad}</strNacionalidad></InsertaOrdenV2></soap:Body></soap:Envelope>";
                try
                {
                    // Crear la solicitud HTTP
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(rutaurl);
                    request.Headers.Add("SOAPAction", rutaInsertaOrden);
                    request.ContentType = "text/xml; charset=utf-8";
                    request.Method = "POST";
                    // Escribir el contenido de la solicitud en el cuerpo
                    using (Stream stream = request.GetRequestStream())
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(soapRequest);
                        stream.Write(bytes, 0, bytes.Length);
                    }
                    // Obtener la respuesta
                    using (WebResponse response = request.GetResponse())
                    {
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            string responseXml = reader.ReadToEnd();
                            Console.WriteLine("Respuesta del servicio:");
                            Console.WriteLine(responseXml);
                            // Procesar la respuesta
                            string result = ExtractResponseData(responseXml, "InsertaOrdenV2");
                            Console.WriteLine("Resultado procesado:");
                            Console.WriteLine(result);
                            // Procesar el XML de la respuesta (si es necesario)
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.LoadXml(responseXml);
                            // Namespace manager para manejar prefijos de namespaces
                            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
                            nsmgr.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");
                            nsmgr.AddNamespace("tempuri", "http://tempuri.org/");
                            // Buscar el nodo de resultado (ajustar el XPath según la respuesta real)
                            XmlNode resultNode = xmlDoc.SelectSingleNode("//tempuri:InsertaOrdenV2Result", nsmgr);
                            if (resultNode != null)
                            {
                                string resultText = resultNode.InnerText;
                                Console.WriteLine("Resultado de InsertaOrdenV2: " + result);
                                ProcesoPXSQL.GuardarRespuesta ("202", p.secuencia, resultText, resultText.Replace("0", "").Replace("1", "").Replace("-1", ""));                               
                                if (resultText=="1")
                                {
                                    //_respuestaApi.StatusCode = System.Net.HttpStatusCode.OK;
                                    //_respuestaApi.IsSuccess = true;
                                    //_respuestaApi.Messages.Add("Grabado con exito");
                                    //_respuestaApi.Result = resultText;
                                    //return Ok(_respuestaApi);
                                    tabla.Rows.Add("1", "Guardado Exitosamente");
                                }
                                else
                                {
                                    //_respuestaApi.StatusCode = System.Net.HttpStatusCode.BadRequest;
                                    //_respuestaApi.IsSuccess = true;
                                    //_respuestaApi.Messages.Add("No se ejecuto en plataforma");
                                    //_respuestaApi.Result = resultText;
                                    //return Ok(_respuestaApi);
                                    tabla.Rows.Add("-1", resultText);
                                }
                            }
                            else
                            {
                                Console.WriteLine("No se encontró el nodo InsertaOrdenV2Result en la respuesta.");
                                //_respuestaApi.StatusCode = System.Net.HttpStatusCode.BadRequest;
                                //_respuestaApi.IsSuccess = false;
                                //_respuestaApi.Messages.Add("No se encontró el nodo AutenticacionUsuarioPxResult en la respuesta.");
                                ////mensaje = _respuestaApi.Messages[0];
                                ////respuesta = ("{\"respuesta\":" + _respuestaApi.Messages[0] + "}");
                                //return Ok(_respuestaApi);
                                tabla.Rows.Add("-1", "No se encontró el nodo AutenticacionUsuarioPxResult en la respuesta.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al consumir el servicio: " + ex.Message);
                    //_respuestaApi.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    //_respuestaApi.IsSuccess = false;
                    //_respuestaApi.Messages.Add(ex.Message);
                    ////mensaje = _respuestaApi.Messages[0];
                    ////respuesta = ("{\"respuesta\":" + _respuestaApi.Messages[0] + "}");
                    //return Ok(_respuestaApi);
                    //}
                    tabla.Rows.Add("-1", ex.Message);
                }           
                //_respuestaApi.StatusCode = System.Net.HttpStatusCode.BadRequest;
                //_respuestaApi.IsSuccess = false;
                //_respuestaApi.Messages.Add("No se encontró el nodo AutenticacionUsuarioPxResult en la respuesta.");
                //return Ok(_respuestaApi);
            }
            string jsonData;
            jsonData = (string)Utilidades.DataTableToJSON(tabla);
            jsonData = "{\"results\":" + jsonData + "}";
            return jsonData;
        }
      

        static string ExtractResponseData(string responseXml, string tipo)
        {
            // Extraer datos específicos de la respuesta utilizando XML parsing o expresiones regulares.
            // Ejemplo básico:
            int start = 0;
            int end = 0;

            if (tipo== "InsertaOrdenV2")
            {
                start = responseXml.IndexOf("<InsertaOrdenV2Result>");
                end = responseXml.IndexOf("</InsertaOrdenV2Result>");
            }
            if (tipo == "ConsultaOrden")
            {
                start = responseXml.IndexOf("<ConsultaOrdenResult>");
                end = responseXml.IndexOf("</ConsultaOrdenResult>");
            }

            if (start >= 0 && end > start)
            {
                return responseXml.Substring(start, end - start);
            }

            return "No se pudo procesar la respuesta.";
        }


    }
}
