using ApiNetsuite.Clases;
using ApiNetsuite.DTO.Bien;
using ApiNetsuite.DTO.Cliente;
using ApiNetsuite.DTO.OrdenServicio;
using ApiNetsuite.DTO.Turno;
using ApiNetsuite.Modelo;
using ApiNetsuite.Repositorio;
using ApiNetsuite.Repositorio.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Drawing;
using System.Net;

namespace ApiNetsuite.Controllers
{
    [ApiController]
    [Route("Cliente")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ClienteController : ControllerBase
    {

        private readonly IClienteSQL repositorio;
        protected RespuestAPI _respuestaApi;
        public ClienteController(IClienteSQL r)
        {
            this.repositorio = r;
            this._respuestaApi = new();
        }


        [HttpPost]
        [Authorize]
        public ActionResult<ClienteRespDTO> ActualizarCliente(ClienteDTO p)
        {
            ClienteRespDTO cliente = null;
            bool bandera = true;
            string mensaje = "";
            if (p.plataforma == "AMI")
            {
                if (p.id_cliente == "" || p.id_cliente.ToUpper() == "NULL")
                {
                    mensaje = "Debe de Enviar el Cod. del Cliente";
                    bandera = false;
                }else if (p.primer_nombre == "" || p.primer_nombre.ToUpper() == "NULL")
                {
                    mensaje = "Debe de Enviar el primer nombre";
                    bandera = false;
                }else if (p.telefono_celular == "" || p.telefono_celular.ToUpper() == "NULL")
                {
                    mensaje = "Debe de Enviar el celular";
                    bandera = false;
                }else if (p.telefono_convencional == "" || p.telefono_convencional.ToUpper() == "NULL")
                {
                    mensaje = "Debe de Enviar telefono convencional";
                    bandera = false;
                }else if (p.email == "" || p.email.ToUpper() == "NULL")
                {
                    mensaje = "Debe de Enviar email";
                    bandera = false;
                }else if (p.id_usuario == "" || p.id_usuario.ToUpper() == "NULL")
                {
                    mensaje = "Debe de Enviar usuario";
                    bandera = false;
                }
                if (bandera)
                {
                    string mensajeAMI = ConsultaAMI.ActualizacionCliente(p);
                    if (mensajeAMI == "OK")
                    {
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
                string mensajePX = ConsultaPX.CambioCliente(p);
                if (mensajePX == "OK")
                {
                    //falta guaradado log
                    mensaje = mensajePX;
                    bandera = true;
                }else
                {
                    bandera = false;
                    mensaje = mensajePX;
                }
            }
            if (bandera)
            {
                cliente = new ClienteRespDTO
                {
                    id_cliente = p.id_cliente,
                    plataforma = p.plataforma,
                    nombres = p.primer_nombre + " " + p.segundo_nombre,
                    apellidos = p.apellido_paterno + " " + p.apellido_materno,
                    email = p.email,
                    status = 200,
                    mensaje = "Ok",
                };
            }else
            {
                cliente = new ClienteRespDTO
                {
                    id_cliente = p.id_cliente,
                    plataforma = p.plataforma,
                    nombres = p.primer_nombre + " " + p.segundo_nombre,
                    apellidos = p.apellido_paterno + " " + p.apellido_materno,
                    email = p.email,
                    status = 400,
                    mensaje = mensaje,
                };
            }
            return cliente;
        }


        [HttpGet]
        [Authorize]
        public ActionResult<GenClienteResp> ConsultaCliente(string opcion, string identificacion, string idcliente, string oficina, string idtipocliente, string indice)
        {
            conexion ruta = new conexion();
            string API_URL = ruta.ObtenerRuta("649", "1");
            string HMACSHA256SignatureType = ruta.ObtenerDatos("1");
            string OAuthVersion = ruta.ObtenerDatos("2");
            var oauthConsumerKey = ruta.ObtenerDatos("3");
            var oauthToken = ruta.ObtenerDatos("4");
            var oauthConsumerSecret = ruta.ObtenerDatos("5");
            var oauthTokenSecret = ruta.ObtenerDatos("6");
            var realm = ruta.ObtenerDatos("9");
            var httpMethod = ruta.ObtenerDatos("8");
            API_URL = API_URL + "&opcion=" + opcion + "&identificacion=" + identificacion + "&idcliente=" + idcliente + "&oficina=" + oficina + "&idtipocliente=" + idtipocliente + "&indice=" + indice;
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
            //GenClienteResp myObj = JsonConvert.DeserializeObject<GenClienteResp>(response.Content);
            GenClienteResp myObj = JsonConvert.DeserializeObject<GenClienteResp>("{\"results\":" + response.Content + "}");
            return myObj;
        }


        [HttpGet]
        [Authorize]
        [Route("Telefono")]
        public ActionResult<GenTelefonoResp> ConsultaTelefono(string opcion, string identificacion, string idcliente)
        {
            conexion ruta = new conexion();
            string API_URL = ruta.ObtenerRuta("649", "1");
            string HMACSHA256SignatureType = ruta.ObtenerDatos("1");
            string OAuthVersion = ruta.ObtenerDatos("2");
            var oauthConsumerKey = ruta.ObtenerDatos("3");
            var oauthToken = ruta.ObtenerDatos("4");
            var oauthConsumerSecret = ruta.ObtenerDatos("5");
            var oauthTokenSecret = ruta.ObtenerDatos("6");
            var realm = ruta.ObtenerDatos("9");
            var httpMethod = ruta.ObtenerDatos("8");
            API_URL = API_URL + "&opcion=" + opcion + "&identificacion=" + identificacion + "&idcliente=" + idcliente + "&oficina=" ;
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
            //GenTelefonoResp myObj = JsonConvert.DeserializeObject<GenTelefonoResp>(response.Content);
            GenTelefonoResp myObj = JsonConvert.DeserializeObject<GenTelefonoResp>("{\"results\":" + response.Content + "}");
            return myObj;
        }


        [HttpGet]
        [Authorize]
        [Route("Direccion")]
        public ActionResult<GenDireccionResp> ConsultaDireccion(string opcion, string identificacion, string idcliente)
        {
            conexion ruta = new conexion();
            string API_URL = ruta.ObtenerRuta("649", "1");
            string HMACSHA256SignatureType = ruta.ObtenerDatos("1");
            string OAuthVersion = ruta.ObtenerDatos("2");
            var oauthConsumerKey = ruta.ObtenerDatos("3");
            var oauthToken = ruta.ObtenerDatos("4");
            var oauthConsumerSecret = ruta.ObtenerDatos("5");
            var oauthTokenSecret = ruta.ObtenerDatos("6");
            var realm = ruta.ObtenerDatos("9");
            var httpMethod = ruta.ObtenerDatos("8");
            API_URL = API_URL + "&opcion=" + opcion + "&identificacion=" + identificacion + "&idcliente=" + idcliente + "&oficina=";
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
            //GenDireccionResp myObj = JsonConvert.DeserializeObject<GenDireccionResp>(response.Content);
            GenDireccionResp myObj = JsonConvert.DeserializeObject<GenDireccionResp>("{\"results\":" + response.Content + "}");
            return myObj;
        }


        [HttpGet]
        [Authorize]
        [Route("Mail")]
        public ActionResult<GenMailResp> ConsultaMail(string opcion, string identificacion, string idcliente)
        {
            conexion ruta = new conexion();
            string API_URL = ruta.ObtenerRuta("649", "1");
            string HMACSHA256SignatureType = ruta.ObtenerDatos("1");
            string OAuthVersion = ruta.ObtenerDatos("2");
            var oauthConsumerKey = ruta.ObtenerDatos("3");
            var oauthToken = ruta.ObtenerDatos("4");
            var oauthConsumerSecret = ruta.ObtenerDatos("5");
            var oauthTokenSecret = ruta.ObtenerDatos("6");
            var realm = ruta.ObtenerDatos("9");
            var httpMethod = ruta.ObtenerDatos("8");
            API_URL = API_URL + "&opcion=" + opcion + "&identificacion=" + identificacion + "&idcliente=" + idcliente + "&oficina=";
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
            //GenMailResp myObj = JsonConvert.DeserializeObject<GenMailResp>(response.Content);
            GenMailResp myObj = JsonConvert.DeserializeObject<GenMailResp>("{\"results\":" + response.Content + "}");
            return myObj;
        }


        [HttpGet]
        [Authorize]
        [Route("ConsultarCliente")]
        public ActionResult<string> ConsultarClienteNetsuite(string identificacion)
        {
            conexion ruta = new conexion();
            string API_URL = ruta.ObtenerRuta("656", "1");
            string HMACSHA256SignatureType = ruta.ObtenerDatos("1");
            string OAuthVersion = ruta.ObtenerDatos("2");
            var oauthConsumerKey = ruta.ObtenerDatos("3");
            var oauthToken = ruta.ObtenerDatos("4");
            var oauthConsumerSecret = ruta.ObtenerDatos("5");
            var oauthTokenSecret = ruta.ObtenerDatos("6");
            var realm = ruta.ObtenerDatos("9");
            var httpMethod = ruta.ObtenerDatos("8");
            if (identificacion != "" && identificacion != null)
            {
                API_URL = API_URL + "&vatregnumber=" + identificacion;
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
        [Route("ClienteNetsuite")]
        public ActionResult<string> ClienteNetsuite(ClienteNetsuiteDTO p)
        {
            conexion ruta = new conexion();
            string API_URL = ruta.ObtenerRuta("656", "1");
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
        [Route("RegistrarCliente")]
        public ActionResult<string> RegistrarCliente(RegClienteDTO data)
        {
            bool bandera = true;
            string mensaje = "";

        
            if (data.Id_ns_cliente == null || data.Id_ns_cliente == "string" || string.IsNullOrEmpty(data.Id_ns_cliente.ToString()))
            {
                mensaje = "Valor de Id_ns_cliente en blanco";
                bandera = false;
            }
            else if (data.Identificacion == null || data.Identificacion == "string" || string.IsNullOrEmpty(data.Identificacion.ToString()))
            {
                mensaje = "Valor de Identificacion en blanco";
                bandera = false;
            }
            else if (data.Nombre_Completo == null || data.Nombre_Completo == "string" || string.IsNullOrEmpty(data.Nombre_Completo.ToString()))
            {
                mensaje = "Valor de Nombre_Completo en blanco";
                bandera = false;
            }

            for (int i = 0; i < data.Direccion.Count; i++)
            {
                if (data.Direccion[i].Identificacion == null || data.Direccion[i].Identificacion == "string" || string.IsNullOrEmpty(data.Direccion[i].Identificacion.ToString()))
                {
                    mensaje = "Valor de Identificacion de la dirección en blanco";
                    bandera = false;
                }
                else if (data.Direccion[i].Cedula_Ruc == null || data.Direccion[i].Cedula_Ruc == "string" || string.IsNullOrEmpty(data.Direccion[i].Cedula_Ruc.ToString()))
                {
                    mensaje = "Valor de Cedula_Ruc de la  dirección en blanco";
                    bandera = false;
                }
                else if (data.Direccion[i].Direccion1 == null || data.Direccion[i].Direccion1 == "string" || string.IsNullOrEmpty(data.Direccion[i].Direccion1.ToString()))
                {
                    mensaje = "Valor de Direccion1  en blanco";
                    bandera = false;
                }

            }

            for (int i = 0; i < data.Email.Count; i++)
            {
                if (data.Email[i].Identificacion == null || data.Email[i].Identificacion == "string" || string.IsNullOrEmpty(data.Email[i].Identificacion.ToString()))
                {
                    mensaje = "Valor de Identificacion del Email en blanco";
                    bandera = false;
                }
                else if (data.Email[i].Cedula == null || data.Email[i].Cedula == "string" || string.IsNullOrEmpty(data.Email[i].Cedula.ToString()))
                {
                    mensaje = "Valor de Cedula del Email en blanco";
                    bandera = false;
                }
                else if (data.Email[i].Email == null || data.Email[i].Email == "string" || string.IsNullOrEmpty(data.Email[i].Email.ToString()))
                {
                    mensaje = "Valor de Email en blanco";
                    bandera = false;
                }

            }

            for (int i = 0; i < data.Telefono.Count; i++)
            {
                if (data.Telefono[i].Identificacion == null || data.Telefono[i].Identificacion == "string" || string.IsNullOrEmpty(data.Telefono[i].Identificacion.ToString()))
                {
                    mensaje = "Valor de Identificacion del Telefono en blanco";
                    bandera = false;
                }
                else if (data.Telefono[i].Cedula == null || data.Telefono[i].Cedula == "string" || string.IsNullOrEmpty(data.Telefono[i].Cedula.ToString()))
                {
                    mensaje = "Valor de Cedula del Telefono en blanco";
                    bandera = false;
                }
                else if (data.Telefono[i].Telefono == null || data.Telefono[i].Telefono == "string" || string.IsNullOrEmpty(data.Telefono[i].Telefono.ToString()))
                {
                    mensaje = "Valor de Telefono en blanco";
                    bandera = false;
                }

            }

            if (bandera)
            {
                string Datos = ClienteSQL.CrearCliente(data);
                string Valor = Utilidades.Catalogo(texto: ((Datos.ToUpper()).TrimEnd()).TrimStart());
                //string Valor = "OK";
                if (Valor == "OK")
                {
                    _respuestaApi.StatusCode = HttpStatusCode.OK;
                    _respuestaApi.IsSuccess = true;
                    _respuestaApi.Result = Valor;
                    _respuestaApi.Messages.Add("Grabado con exito");
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
                //// ClienteRespDTO cliente = null;
                //_respuestaApi.StatusCode = HttpStatusCode.OK;
                //_respuestaApi.IsSuccess = true;
                //_respuestaApi.Messages.Add("Grabado con exito");
                ////mensaje = _respuestaApi.Messages[0];
                ////respuesta = ("{\"respuesta\":" + _respuestaApi.Messages[0] + "}");
                //return Ok(_respuestaApi);
            }
            else
            {
                _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.Result = "No Ingreso al proceso";
                _respuestaApi.Messages.Add(mensaje);
                return BadRequest(_respuestaApi);
            }
            
        }

      

    }
}
