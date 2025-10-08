using ApiNetsuite.Clases;
using ApiNetsuite.DTO.Bien;
using ApiNetsuite.DTO.Cliente;
using ApiNetsuite.DTO.General;
using ApiNetsuite.DTO.OrdenServicio;
using ApiNetsuite.DTO.Retencion;
using ApiNetsuite.Modelo;
using ApiNetsuite.Repositorio;
using ApiNetsuite.Repositorio.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using RestSharp;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Drawing;
using System.Net;
using System.Reflection;
using System.ServiceModel.Channels;

namespace ApiNetsuite.Controllers
{
    [ApiController]
    [Route("Bien")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BienController : ControllerBase
    {
        private readonly IBienSQL repositorio;
        protected RespuestAPI _respuestaApi;

        public BienController(IBienSQL r)
        {
            this.repositorio = r;
            this._respuestaApi = new();
        }


        [HttpPost]
        [Authorize]
        public ActionResult<BienRespDTO> ActualizarBien(BienDTO p)
        {
            BienRespDTO bien = null;
            bool bandera = true;
            string mensaje = "";
            if (p.plataforma == "AMI")
            {
                if (p.codvehiculo == "" || p.codvehiculo.ToUpper() == "NULL")
                {
                    mensaje = "Debe de Enviar el Cod. del Vehiculo";
                    bandera = false;
                }else if (p.plataforma == "" || p.plataforma.ToUpper() == "NULL")
                {
                    mensaje = "Debe de Enviar la plataforma";
                    bandera = false;
                }else if (p.cliente == "" || p.cliente.ToUpper() == "NULL")
                {
                    mensaje = "Debe de Enviar el cliente";
                    bandera = false;
                }else if (p.username == "" || p.username.ToUpper() == "NULL")
                {
                    mensaje = "Debe de Enviar el username";
                    bandera = false;
                }else if (p.amivehiculo == "" || p.amivehiculo.ToUpper() == "NULL")
                {
                    mensaje = "Debe de Enviar el Codigo del Vehículo de AMI";
                    bandera = false;
                }else if (p.amicliente == "" || p.amicliente.ToUpper() == "NULL")
                {
                    mensaje = "Debe de Enviar el Codigo del Cliente de AMI";
                    bandera = false;
                }else if (p.descripcionvehiculo == "" || p.descripcionvehiculo.ToUpper() == "NULL")
                {
                    mensaje = "Debe de Enviar la descripción del Vehículo";
                    bandera = false;
                }else if (p.chasis == "" || p.chasis.ToUpper() == "NULL")
                {
                    mensaje = "Debe de Enviar el chasis del Vehículo";
                    bandera = false;
                }else if (p.motor == "" || p.motor.ToUpper() == "NULL")
                {
                    mensaje = "Debe de Enviar el motor del Vehículo";
                    bandera = false;
                }else if (p.marca == "" || p.marca.ToUpper() == "NULL")
                {
                    mensaje = "Debe de Enviar la marca del Vehículo";
                    bandera = false;
                }else if (p.modelo == "" || p.modelo.ToUpper() == "NULL")
                {
                    mensaje = "Debe de Enviar el modelo del Vehículo";
                    bandera = false;
                }
                if (bandera)
                {
                    string mensajeAMI = ConsultaAMI.ActualizacionAsset(p);
                    if (mensajeAMI == "OK")
                    {
                        //falta guaradado log
                        mensaje = mensajeAMI;
                        bandera = true;
                    }else
                    {
                        bandera = false;
                        mensaje = mensajeAMI;
                    }
                }
            }else if (p.plataforma == "PX")
            {
                if (p.codvehiculo == "" || p.codvehiculo.ToUpper() == "NULL")
                {
                    mensaje = "Debe de Enviar el Cod. del Vehiculo";
                    bandera = false;
                }else if (p.vehiculoid == "" || p.vehiculoid.ToUpper() == "NULL")
                {
                    mensaje = "Debe de Enviar el Cod. del Vehiculo";
                    bandera = false;
                }else if (p.cliente == "" || p.cliente.ToUpper() == "NULL")
                {
                    mensaje = "Debe de Enviar el cliente";
                    bandera = false;
                }else if (p.chasis == "" || p.chasis.ToUpper() == "NULL")
                {
                    mensaje = "Debe de Enviar el chasis del Vehículo";
                    bandera = false;
                }else if (p.motor == "" || p.motor.ToUpper() == "NULL")
                {
                    mensaje = "Debe de Enviar el motor del Vehículo";
                    bandera = false;
                }else if (p.marca == "" || p.marca.ToUpper() == "NULL")
                {
                    mensaje = "Debe de Enviar la marca del Vehículo";
                    bandera = false;
                }else if (p.modelo == "" || p.modelo.ToUpper() == "NULL")
                {
                    mensaje = "Debe de Enviar el modelo del Vehículo";
                    bandera = false;
                }else if (p.tipovehiculo == "" || p.tipovehiculo.ToUpper() == "NULL")
                {
                    mensaje = "Debe de Enviar el Tipo del Vehículo";
                    bandera = false;
                }else if (p.idmarca == "" || p.idmarca.ToUpper() == "NULL")
                {
                    mensaje = "Debe de Enviar el codigo de marca del Vehículo";
                    bandera = false;
                }else if (p.idmodelo == "" || p.idmodelo.ToUpper() == "NULL")
                {
                    mensaje = "Debe de Enviar el codigo de modelo del Vehículo";
                    bandera = false;
                }
               if (bandera)
                {
                    string mensajePX = ConsultaPX.CambioConAseFin(p);
                    if (mensajePX == "OK")
                    {
                        mensaje = mensajePX;
                        bandera = true;
                    }else
                    {
                        bandera = false;
                        mensaje = mensajePX;
                    }
                }
               
            }
            if (bandera)
            {
                if (p.plataforma == "PX")
                {
                    bien = new BienRespDTO
                    {
                        codvehiculo = p.codvehiculo,
                        plataforma = p.plataforma,
                        mensaje = mensaje,
                        username = p.username,
                        amivehiculo = p.amivehiculo,
                        amicliente = p.amicliente,
                        cliente = p.cliente,
                        chasis = p.chasis,
                        placa = p.placa,
                        status = 200,
                        vehiculoid = p.vehiculoid,
                       
                    };
                }

                if (p.plataforma == "AMI")
                {
                    bien = new BienRespDTO
                    {
                        codvehiculo = p.codvehiculo,
                        plataforma = p.plataforma,
                        mensaje = mensaje,
                        username = p.username,
                        amivehiculo = p.amivehiculo,
                        amicliente = p.amicliente,
                        cliente = p.cliente,
                        chasis = p.chasis,
                        placa = p.placa,
                        status = 200,
                        vehiculoid = p.vehiculoid,
                    };
                }
            }else
            {
                bien = new BienRespDTO
                {
                    codvehiculo = p.codvehiculo,
                    plataforma = p.plataforma,
                    mensaje = mensaje,
                    username = p.username,
                    amivehiculo = p.amivehiculo,
                    amicliente = p.amicliente,
                    cliente = p.cliente,
                    chasis = p.chasis,
                    placa = p.placa,
                    status = 400,
                    vehiculoid = p.vehiculoid,
                  };
            }
            return bien;
        }


        [HttpGet]
        [Authorize]
        public ActionResult<GenVehiculoResp> ConsultaVehiculo(string opcion, string codigovehiculo, string idvehiculo, string codigocliente, string idcliente, string indice, string criterio, string tipobien)
        {
            conexion ruta = new conexion();
            string API_URL = ruta.ObtenerRuta("700", "1");
            string HMACSHA256SignatureType = ruta.ObtenerDatos("1");
            string OAuthVersion = ruta.ObtenerDatos("2");
            var oauthConsumerKey = ruta.ObtenerDatos("3");
            var oauthToken = ruta.ObtenerDatos("4");
            var oauthConsumerSecret = ruta.ObtenerDatos("5");
            var oauthTokenSecret = ruta.ObtenerDatos("6");
            var realm = ruta.ObtenerDatos("9");
            var httpMethod = ruta.ObtenerDatos("8");
            API_URL = API_URL + "&opcion=" + opcion + "&codigovehiculo=" + codigovehiculo + "&idcliente=" + idcliente + "&codigocliente=" + codigocliente + "&idvehiculo=" + idvehiculo + "&indice=" + indice + "&criterio=" + criterio + "&tipobien=" + tipobien;
            OAuthBase auth = new OAuthBase();
            var timestamp = Clases.OAuthBase.GenerateTimeStamp();
            var nonce = Clases.OAuthBase.GenerateNonce();
            var client = new RestClient(API_URL);
            var request = new RestRequest("", Method.Get);
            Uri url = new Uri(API_URL);
            var signature = Clases.OAuthBase.GenerateSignature(url, oauthConsumerKey, oauthConsumerSecret, oauthToken, oauthTokenSecret, httpMethod, timestamp, nonce);
            request.AddHeader("Authorization", "OAuth realm=\"" + realm + "\", oauth_token=\"" + oauthToken + "\", oauth_consumer_key=\"" + oauthConsumerKey + "\"," + " oauth_nonce=\"" + nonce + "\", oauth_timestamp=\"" + timestamp + "\", oauth_signature_method=\"" + HMACSHA256SignatureType + "\", oauth_version=\"" + OAuthVersion + "\", oauth_signature=\"" + signature + "\"");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cookie", "NS_ROUTING_VERSION=LAGGING");
            Console.WriteLine(request);
            var response = client.Execute(request);
            Console.WriteLine(response.Content);
            //GenVehiculoResp myObj = JsonConvert.DeserializeObject<GenVehiculoResp>(response.Content);
            GenVehiculoResp myObj = JsonConvert.DeserializeObject<GenVehiculoResp>("{\"results\":" + response.Content + "}");
            return myObj;
        }


        [HttpGet]
        [Authorize]
        [Route("Cobertura")]
        public ActionResult<GenCoberturaResp> Cobertura(string opcion, string codigovehiculo, string idvehiculo, string codigocliente, string idcliente, string idgrupoproducto, string codgrupoproducto, string fechaini, string fechafin, string criterio)
        {
            conexion ruta = new conexion();
            string API_URL = ruta.ObtenerRuta("650", "1");
            string HMACSHA256SignatureType = ruta.ObtenerDatos("1");
            string OAuthVersion = ruta.ObtenerDatos("2");
            var oauthConsumerKey = ruta.ObtenerDatos("3");
            var oauthToken = ruta.ObtenerDatos("4");
            var oauthConsumerSecret = ruta.ObtenerDatos("5");
            var oauthTokenSecret = ruta.ObtenerDatos("6");
            var realm = ruta.ObtenerDatos("9");
            var httpMethod = ruta.ObtenerDatos("8");
            API_URL = API_URL + "&opcion=" + opcion + "&codigovehiculo=" + codigovehiculo + "&idcliente=" + idcliente + "&codigocliente=" + codigocliente + "&idvehiculo=" + idvehiculo + "&idgrupoproducto=" + idgrupoproducto + "&criterio=" + criterio + "&codgrupoproducto=" + codgrupoproducto + "&fechaini=" + fechaini + "&fechafin=" + fechafin;
            OAuthBase auth = new OAuthBase();
            var timestamp = Clases.OAuthBase.GenerateTimeStamp();
            var nonce = Clases.OAuthBase.GenerateNonce();
            var client = new RestClient(API_URL);
            var request = new RestRequest("", Method.Get);
            Uri url = new Uri(API_URL);
            var signature = Clases.OAuthBase.GenerateSignature(url, oauthConsumerKey, oauthConsumerSecret, oauthToken, oauthTokenSecret, httpMethod, timestamp, nonce);
            request.AddHeader("Authorization", "OAuth realm=\"" + realm + "\", oauth_token=\"" + oauthToken + "\", oauth_consumer_key=\"" + oauthConsumerKey + "\"," + " oauth_nonce=\"" + nonce + "\", oauth_timestamp=\"" + timestamp + "\", oauth_signature_method=\"" + HMACSHA256SignatureType + "\", oauth_version=\"" + OAuthVersion + "\", oauth_signature=\"" + signature + "\"");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cookie", "NS_ROUTING_VERSION=LAGGING");
            Console.WriteLine(request);
            var response = client.Execute(request);
            Console.WriteLine(response.Content);
            GenCoberturaResp myObj = JsonConvert.DeserializeObject<GenCoberturaResp>(response.Content);
            return myObj;
        }


        [HttpGet]
        [Authorize]
        //[SwaggerParameter("The import ID of the group to set the rule parts to", Required = true)]
        //[SwaggerParameter()]
        [Route("ConsultarVehiculo")]
        public ActionResult<string> ConsultarVehiculoNetsuite(string chasis, string motor,  string codigo)
        {
            conexion ruta = new conexion();
            string API_URL = ruta.ObtenerRuta("654", "1");
            string HMACSHA256SignatureType = ruta.ObtenerDatos("1");
            string OAuthVersion = ruta.ObtenerDatos("2");
            var oauthConsumerKey = ruta.ObtenerDatos("3");
            var oauthToken = ruta.ObtenerDatos("4");
            var oauthConsumerSecret = ruta.ObtenerDatos("5");
            var oauthTokenSecret = ruta.ObtenerDatos("6");
            var realm = ruta.ObtenerDatos("9");
            var httpMethod = ruta.ObtenerDatos("8");
            //API_URL = API_URL + "&chasis=" + chasis + "&motor=" + motor + "&codigo=" + codigo;
            if (chasis != "" && chasis != null)
            {
                API_URL = API_URL + "&chasis=" + chasis;
            }
            if (motor != "" && motor != null)
            {
                API_URL = API_URL + "&motor=" + motor;
            }
            if (codigo != "" && codigo != null)
            {
                API_URL = API_URL + "&codigo=" + codigo;
            }

            OAuthBase auth = new OAuthBase();
            var timestamp = Clases.OAuthBase.GenerateTimeStamp();
            var nonce = Clases.OAuthBase.GenerateNonce();
            var client = new RestClient(API_URL);
            var request = new RestRequest("", Method.Get);
            Uri url = new Uri(API_URL);
            var signature = Clases.OAuthBase.GenerateSignature(url, oauthConsumerKey, oauthConsumerSecret, oauthToken, oauthTokenSecret, httpMethod, timestamp, nonce);
            request.AddHeader("Authorization", "OAuth realm=\"" + realm + "\", oauth_token=\"" + oauthToken + "\", oauth_consumer_key=\"" + oauthConsumerKey + "\"," + " oauth_nonce=\"" + nonce + "\", oauth_timestamp=\"" + timestamp + "\", oauth_signature_method=\"" + HMACSHA256SignatureType + "\", oauth_version=\"" + OAuthVersion + "\", oauth_signature=\"" + signature + "\"");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cookie", "NS_ROUTING_VERSION=LAGGING");
            Console.WriteLine(request);
            var response = client.Execute(request);
            Console.WriteLine(response.Content);
            //GenCoberturaResp myObj = JsonConvert.DeserializeObject<GenCoberturaResp>(response.Content);
            return response.Content;
        }


        [HttpPost]
        [Authorize]
        [Route("Vehiculo")]
        public ActionResult<string> VehiculoNetsuite(VehiculoDTO p)
        {
            conexion ruta = new conexion();
            string API_URL = ruta.ObtenerRuta("654", "1");
            string HMACSHA256SignatureType = ruta.ObtenerDatos("1");
            string OAuthVersion = ruta.ObtenerDatos("2");
            var oauthConsumerKey = ruta.ObtenerDatos("3");
            var oauthToken = ruta.ObtenerDatos("4");
            var oauthConsumerSecret = ruta.ObtenerDatos("5");
            var oauthTokenSecret = ruta.ObtenerDatos("6");
            var realm = ruta.ObtenerDatos("9");
            var httpMethod = ruta.ObtenerDatos("7");
            OAuthBase auth = new OAuthBase();
            var timestamp = Clases.OAuthBase.GenerateTimeStamp();
            var nonce = Clases.OAuthBase.GenerateNonce();
            var client = new RestClient(API_URL);
            var request = new RestRequest("", Method.Post);
            Uri url = new Uri(API_URL);
            var signature = Clases.OAuthBase.GenerateSignature(url, oauthConsumerKey, oauthConsumerSecret, oauthToken, oauthTokenSecret, httpMethod, timestamp, nonce);
            request.AddHeader("Authorization", "OAuth realm=\"" + realm + "\", oauth_token=\"" + oauthToken + "\", oauth_consumer_key=\"" + oauthConsumerKey + "\"," + " oauth_nonce=\"" + nonce + "\", oauth_timestamp=\"" + timestamp + "\", oauth_signature_method=\"" + HMACSHA256SignatureType + "\", oauth_version=\"" + OAuthVersion + "\", oauth_signature=\"" + signature + "\"");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", p, ParameterType.RequestBody);
            Console.WriteLine(request);
            var response = client.Execute(request);
            Console.WriteLine(response.Content);
            //NotaCreditoRespCXPDTO myObj = JsonConvert.DeserializeObject<NotaCreditoRespCXPDTO>(response.Content);
            return response.Content;
        }


        [HttpPost]
        [Authorize]
        [Route("CambioPropietario")]
        public ActionResult<BienRespDTO> CambioPropietario(PropietarioDTO p)
        {
            bool bandera = true;
            string mensaje = "";
            BienRespDTO bien = null;
            if (p.plataforma == "PX")
            {
                if (p.codvehiculo == "" || p.codvehiculo.ToUpper() == "NULL")
                {
                    mensaje = "Debe de Enviar el Cod. del Vehiculo";
                    bandera = false;
                }
                //else if (p.vehiculoid == "" || p.vehiculoid.ToUpper() == "NULL")
                //{
                //    mensaje = "Debe de Enviar el Cod. del Vehiculo";
                //    bandera = false;
                //}
                //else if (p.cliente == "" || p.cliente.ToUpper() == "NULL")
                //{
                //    mensaje = "Debe de Enviar el cliente";
                //    bandera = false;
                //}
                else if (p.chasis == "" || p.chasis.ToUpper() == "NULL")
                {
                    mensaje = "Debe de Enviar el chasis del Vehículo";
                    bandera = false;
                }
                else if (p.motor == "" || p.motor.ToUpper() == "NULL")
                {
                    mensaje = "Debe de Enviar el motor del Vehículo";
                    bandera = false;
                }
                else if (p.marca == "" || p.marca.ToUpper() == "NULL")
                {
                    mensaje = "Debe de Enviar la marca del Vehículo";
                    bandera = false;
                }
                else if (p.modelo == "" || p.modelo.ToUpper() == "NULL")
                {
                    mensaje = "Debe de Enviar el modelo del Vehículo";
                    bandera = false;
                }
                //else if (p.tipovehiculo == "" || p.tipovehiculo.ToUpper() == "NULL")
                //{
                //    mensaje = "Debe de Enviar el Tipo del Vehículo";
                //    bandera = false;
                //}
                else if (p.idmarca == "" || p.idmarca.ToUpper() == "NULL")
                {
                    mensaje = "Debe de Enviar el codigo de marca del Vehículo";
                    bandera = false;
                }
                else if (p.idmodelo == "" || p.idmodelo.ToUpper() == "NULL")
                {
                    mensaje = "Debe de Enviar el codigo de modelo del Vehículo";
                    bandera = false;
                }
                if (bandera)
                {
                    string mensajePX = ConsultaPX.CambioPropietario(p);
                    if (mensajePX == "OK")
                    {
                        mensaje = mensajePX;
                        bandera = true;
                    }
                    else
                    {
                        bandera = false;
                        mensaje = mensajePX;
                    }
                }

            }

            if (bandera)
            {
                if (p.plataforma == "PX")
                {
                    bien = new BienRespDTO
                    {
                        codvehiculo = p.codvehiculo,
                        plataforma = p.plataforma,
                        mensaje = mensaje,
                        username = "",
                        amivehiculo = "",
                        amicliente = "",
                        cliente = "",
                        chasis = p.chasis,
                        placa = p.placa,
                        status = 200,
                        vehiculoid = "",

                    };
                }

                
            }
            else
            {
                bien = new BienRespDTO
                {
                    codvehiculo = p.codvehiculo,
                    plataforma = p.plataforma,
                    mensaje = mensaje,
                    username = "",
                    amivehiculo = "",
                    amicliente = "",
                    cliente = "",
                    chasis = p.chasis,
                    placa = p.placa,
                    status = 400,
                    vehiculoid = "",
                };
            }
            return bien;
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        [Route("/VehiculoNS")]
        public ActionResult<string> VehiculoNS(VehiculoNSDTO data)
        {
            //string respuesta = "";
            bool bandera = true;
            string mensaje = "";

            if (data.IdVehiculo == "" || data.IdVehiculo == null || data.IdVehiculo == "string" || string.IsNullOrEmpty(data.IdVehiculo.ToString()))
            {
                mensaje = "Valor de IdVehiculo en blanco";
                bandera = false;
            }
            else if(data.IdCliente == "" || data.IdCliente == null || data.IdCliente == "string" || string.IsNullOrEmpty(data.IdCliente.ToString()))
            {
                mensaje = "Valor de IdCliente en blanco";
                bandera = false;
            }
            else if (data.TipoBien == "" || data.TipoBien == null || data.TipoBien == "string" || string.IsNullOrEmpty(data.TipoBien.ToString()))
            {
                mensaje = "Valor de TipoBien en blanco";
                bandera = false;
            }
            //else if (data.TipoTerrestre == "" || data.TipoTerrestre == null || data.TipoTerrestre == "string" || string.IsNullOrEmpty(data.TipoTerrestre.ToString()))
            //{
            //    mensaje = "Valor de TipoTerrestre en blanco";
            //    bandera = false;
            //}

            if (bandera)
            {
                string Datos = BienSQL.Vehiculo(data);
                string Valor = Utilidades.Catalogo(texto: ((Datos.ToUpper()).TrimEnd()).TrimStart());
                if (Valor == "OK")
                {
                    _respuestaApi.StatusCode = HttpStatusCode.OK;
                    _respuestaApi.IsSuccess = true;
                    _respuestaApi.Messages.Add("Grabado con exito");
                    _respuestaApi.Result = Valor;
                    //mensaje = _respuestaApi.Messages[0];
                    //respuesta = ("{\"respuesta\":" + _respuestaApi.Messages[0] + "}");
                    return Ok(_respuestaApi);
                }
                else
                {
                    _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
                    _respuestaApi.IsSuccess = false;
                    _respuestaApi.Result = Valor;
                    _respuestaApi.Messages.Add("Error, No se encuentra datos, Verificar");
                    return BadRequest(_respuestaApi);
                }
            }
            else
            {
                _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.Result = "No Ingreso al proceso";
                _respuestaApi.Messages.Add(mensaje);
                return BadRequest(_respuestaApi);
            }
            //return respuesta;
        }

        //[HttpPost]
        //[Authorize]
        //[Route("ActualizaVehiculo")]
        //public ActionResult<string> ActualizaVehiculoNetsuite(VehiculoDTO p)
        //{
        //    conexion ruta = new conexion();
        //    string API_URL = ruta.ObtenerRuta("654", "1");
        //    string HMACSHA256SignatureType = ruta.ObtenerDatos("1");
        //    string OAuthVersion = ruta.ObtenerDatos("2");
        //    var oauthConsumerKey = ruta.ObtenerDatos("3");
        //    var oauthToken = ruta.ObtenerDatos("4");
        //    var oauthConsumerSecret = ruta.ObtenerDatos("5");
        //    var oauthTokenSecret = ruta.ObtenerDatos("6");
        //    var realm = ruta.ObtenerDatos("9");
        //    var httpMethod = ruta.ObtenerDatos("7");
        //    OAuthBase auth = new OAuthBase();
        //    var timestamp = Clases.OAuthBase.GenerateTimeStamp();
        //    var nonce = Clases.OAuthBase.GenerateNonce();
        //    var client = new RestClient(API_URL);
        //    var request = new RestRequest("", Method.Post);
        //    Uri url = new Uri(API_URL);
        //    var signature = Clases.OAuthBase.GenerateSignature(url, oauthConsumerKey, oauthConsumerSecret, oauthToken, oauthTokenSecret, httpMethod, timestamp, nonce);
        //    request.AddHeader("Authorization", "OAuth realm=\"" + realm + "\", oauth_token=\"" + oauthToken + "\", oauth_consumer_key=\"" + oauthConsumerKey + "\"," + " oauth_nonce=\"" + nonce + "\", oauth_timestamp=\"" + timestamp + "\", oauth_signature_method=\"" + HMACSHA256SignatureType + "\", oauth_version=\"" + OAuthVersion + "\", oauth_signature=\"" + signature + "\"");
        //    request.AddHeader("Content-Type", "application/json");
        //    request.AddParameter("application/json", p, ParameterType.RequestBody);
        //    Console.WriteLine(request);
        //    var response = client.Execute(request);
        //    Console.WriteLine(response.Content);
        //    //NotaCreditoRespCXPDTO myObj = JsonConvert.DeserializeObject<NotaCreditoRespCXPDTO>(response.Content);
        //    return response.Content;
        //}


    }

}
