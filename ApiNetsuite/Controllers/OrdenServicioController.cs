using ApiNetsuite.Clases;
using ApiNetsuite.DTO.AMI;
using ApiNetsuite.DTO.Cliente;
using ApiNetsuite.DTO.Cobertura;
using ApiNetsuite.DTO.Digitalizacion;
using ApiNetsuite.DTO.OrdenServicio;
using ApiNetsuite.Modelo;
using ApiNetsuite.Repositorio;
using ApiNetsuite.Repositorio.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Data;
using System.Net;


namespace ApiNetsuite.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("OrdenServicio")]
    public class OrdenServicioController : ControllerBase
    {

        private readonly IOrdenServicioSQL repositorio;
        protected RespuestAPI _respuestaApi;

        public OrdenServicioController(IOrdenServicioSQL r)
        {
            this.repositorio = r;
            this._respuestaApi = new();
        }


        [HttpPost]
        [Authorize]
        [Route("CambioPropietario")]
        public ActionResult<string> CambioPropietario(CambioPropietarioDTO p)
        {
            conexion ruta = new conexion();
            string API_URL = ruta.ObtenerRuta("685", "1");
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
        [Route("CrearOrden")]
        public ActionResult<string> CrearOrden(CambioPropietarioDTO p)
        {
            conexion ruta = new conexion();
            string API_URL = ruta.ObtenerRuta("685", "1");
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
           // return Ok(p);
        }


        [HttpGet]
        [Authorize]
        [Route("VerificarInstalacion")]
        public ActionResult<string> VerificarInstalacion(string opcion, string idvehiculo, string codgrupoproducto, string idestadodispositivo)
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
            //API_URL = API_URL + "&opcion=" + opcion + "&idvehiculo=" + idvehiculo + "&codgrupoproducto=" + codgrupoproducto + "&idestadodispositivo=" + idestadodispositivo;
            if (opcion != "" && opcion != null)
            {
                API_URL = API_URL + "&opcion=" + opcion;
            }
            if (idvehiculo != "" && idvehiculo != null)
            {
                API_URL = API_URL + "&idvehiculo=" + idvehiculo;
            }
            if (codgrupoproducto != "" && codgrupoproducto != null)
            {
                API_URL = API_URL + "&codgrupoproducto=" + codgrupoproducto;
            }
            if (idestadodispositivo != "" && idestadodispositivo != null)
            {
                API_URL = API_URL + "&idestadodispositivo=" + idestadodispositivo;
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
            //request.AddParameter("application/json", p, ParameterType.RequestBody);
            Console.WriteLine(request);
            var response = client.Execute(request);
            Console.WriteLine(response.Content);
            //NotaCreditoRespCXPDTO myObj = JsonConvert.DeserializeObject<NotaCreditoRespCXPDTO>(response.Content);
            return ("{\"results\":" + response.Content + "}");
        }

       
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        [Route("/OrdenServicioNS")]
        public ActionResult<string> OrdenServicioNS( OrdenServicioDTO data)
        {
            //string respuesta = "";
            bool bandera = true;
            string mensaje = "";

            if (data.origen =="OS" && data.accion == "E")
            {
                bandera = true;
            }
            else
            {
                if (data.Codigo_Empresa == 0 || string.IsNullOrEmpty(data.Codigo_Empresa.ToString()))
                {
                    mensaje = "Valor de Codigo_Empresa en blanco";
                    bandera = false;
                }
                else if (data.Codigo_Sucursal == 0 || string.IsNullOrEmpty(data.Codigo_Sucursal.ToString()))
                {
                    mensaje = "Valor de Codigo_Sucursal en blanco";
                    bandera = false;
                }
                else if (data.Numero_General == null || data.Numero_General == "string" || string.IsNullOrEmpty(data.Numero_General.ToString()))
                {
                    mensaje = "Valor de Numero_General en blanco";
                    bandera = false;
                }
                else if (data.Id_os == 0 || string.IsNullOrEmpty(data.Id_os.ToString()))
                {
                    mensaje = "Valor de Id_os en blanco";
                    bandera = false;
                }
                else if (data.Fecha == null || data.Fecha == "string" || string.IsNullOrEmpty(data.Fecha.ToString()))
                {
                    mensaje = "Valor de Fecha en blanco";
                    bandera = false;
                }
                else if (data.Identificacion_Cliente == null || data.Identificacion_Cliente == "string" || string.IsNullOrEmpty(data.Identificacion_Cliente.ToString()))
                {
                    mensaje = "Valor de Identificacion_Cliente en blanco";
                    bandera = false;
                }
                else if (data.Codigo_Vehiculo == null || data.Codigo_Vehiculo == "string" || string.IsNullOrEmpty(data.Codigo_Vehiculo.ToString()))
                {
                    mensaje = "Valor de Codigo_Vehiculo en blanco";
                    bandera = false;
                }
                else if (data.Id_Vehiculo_ns == 0 || string.IsNullOrEmpty(data.Id_Vehiculo_ns.ToString()))
                {
                    mensaje = "Valor de Id_Vehiculo_ns en blanco";
                    bandera = false;
                }
                else if (data.Id_Cliente_ns == 0 || string.IsNullOrEmpty(data.Id_Cliente_ns.ToString()))
                {
                    mensaje = "Valor de Id_Cliente_ns en blanco";
                    bandera = false;
                }
                else if (data.Estado == null || data.Estado == "string" || string.IsNullOrEmpty(data.Estado.ToString()))
                {
                    mensaje = "Valor de Estado en blanco";
                    bandera = false;
                }
                //else if (data.Cobrado == null || data.Cobrado == "string" || string.IsNullOrEmpty(data.Cobrado.ToString()))
                //{
                //    mensaje = "Valor de Cobrado en blanco";
                //    bandera = false;
                //}
                //else if (data.Total_Item == 0 || string.IsNullOrEmpty(data.Total_Item.ToString()))
                //{
                //    mensaje = "Valor de Total_Item en blanco";
                //    bandera = false;
                //}

                for (int i = 0; i < data.DetalleOrden.Count; i++)
                {
                    if (data.DetalleOrden[i].Secuencia == 0 || string.IsNullOrEmpty(data.DetalleOrden[i].Secuencia.ToString()))
                    {
                        mensaje = "Valor de Codigo_Sucursal en blanco";
                        bandera = false;
                    }
                    else if (data.DetalleOrden[i].IdItem == 0 || string.IsNullOrEmpty(data.DetalleOrden[i].IdItem.ToString()))
                    {
                        mensaje = "Valor de IdItem en blanco";
                        bandera = false;
                    }
                    else if (data.DetalleOrden[i].CodigoItem == null || data.DetalleOrden[i].CodigoItem == "string" || string.IsNullOrEmpty(data.DetalleOrden[i].CodigoItem.ToString()))
                    {
                        mensaje = "Valor de CodigoItem en blanco";
                        bandera = false;
                    }
                    else if (data.DetalleOrden[i].ItemDescripcion == null || data.DetalleOrden[i].ItemDescripcion == "string" || string.IsNullOrEmpty(data.DetalleOrden[i].ItemDescripcion.ToString()))
                    {
                        mensaje = "Valor de ItemDescripcion en blanco";
                        bandera = false;
                    }
                    else if (data.DetalleOrden[i].ADP == null || data.DetalleOrden[i].ADP == "string" || string.IsNullOrEmpty(data.DetalleOrden[i].ADP.ToString()))
                    {
                        mensaje = "Valor de ADP en blanco";
                        bandera = false;
                    }
                    else if (data.DetalleOrden[i].Tipo_Transaccion == null || data.DetalleOrden[i].Tipo_Transaccion == "string" || string.IsNullOrEmpty(data.DetalleOrden[i].Tipo_Transaccion.ToString()))
                    {
                        mensaje = "Valor de Tipo_Transaccion en blanco";
                        bandera = false;
                    }
                    else if (data.DetalleOrden[i].Grupo_Producto == null || data.DetalleOrden[i].Grupo_Producto == "string" || string.IsNullOrEmpty(data.DetalleOrden[i].Grupo_Producto.ToString()))
                    {
                        mensaje = "Valor de Grupo_Producto en blanco";
                        bandera = false;
                    }
                    //else if (data.DetalleOrden[i].Total_Item == 0  || string.IsNullOrEmpty(data.DetalleOrden[i].Total_Item.ToString()))
                    //{
                    //    mensaje = "Valor de Total_Item en blanco";
                    //    bandera = false;
                    //}
                    //else if (data.DetalleOrden[i].Iva == 0  || string.IsNullOrEmpty(data.DetalleOrden[i].Iva.ToString()))
                    //{
                    //    mensaje = "Valor de Iva en blanco";
                    //    bandera = false;
                    //}
                    //else if (data.DetalleOrden[i].PrecioCliente == 0 || string.IsNullOrEmpty(data.DetalleOrden[i].PrecioCliente.ToString()))
                    //{
                    //    mensaje = "Valor de PrecioCliente en blanco";
                    //    bandera = false;
                    //}
                }

            }
            if (bandera)
            {
                string Datos = OrdenServicioSQL.OrdenServicio(data);
                string Valor = Utilidades.Catalogo(texto: ((Datos.ToUpper()).TrimEnd()).TrimStart());
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


        [HttpGet]
        [Authorize]
        [Route("CambioPropietarioyEstado")]
        public ActionResult<string> CambioPropietarioyEstado(string opcion, string idbien, string familia, string idOrden, string item)
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
            //NotaCreditoRespCXPDTO myObj = JsonConvert.DeserializeObject<NotaCreditoRespCXPDTO>(response.Content);
            return (  response.Content);
        }


        [HttpPost]
        [Authorize]
        [Route("ActualizarCodigoAmi")]
        public ActionResult<string> ActualizarCodigoAmi(ActualizaAMIDTO p)
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
            return (response.Content);
        }


        [HttpGet]
        [Authorize]
        [Route("ConsultaOrdenNS")]
        public ActionResult<string> ConsultaOrdenNS(string opcion,  string idOrden, string idbien)
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
            if (opcion != "" && opcion != null)
            {
                API_URL = API_URL + "&opcion=" + opcion;
            }
            if (idbien != "" && idbien != null)
            {
                API_URL = API_URL + "&idbien=" + idbien;
            }

            if (idOrden != "" && idOrden != null)
            {
                API_URL = API_URL + "&idorden=" + idOrden;
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
            Console.WriteLine(request);
            var response = client.Execute(request);
            Console.WriteLine(response.Content);
            return (response.Content);
        }


        [HttpGet]
        [Authorize]
        [Route("ConsultaOrdenItemNS")]
        public ActionResult<string> ConsultaOrdenItemNS(string opcion, string idOrden, string idbien)
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
            if (opcion != "" && opcion != null)
            {
                API_URL = API_URL + "&opcion=" + opcion;
            }
            if (idbien != "" && idbien != null)
            {
                API_URL = API_URL + "&idbien=" + idbien;
            }

            if (idOrden != "" && idOrden != null)
            {
                API_URL = API_URL + "&idorden=" + idOrden;
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
            Console.WriteLine(request);
            var response = client.Execute(request);
            Console.WriteLine(response.Content);
            return (response.Content);
        }



    }
}
