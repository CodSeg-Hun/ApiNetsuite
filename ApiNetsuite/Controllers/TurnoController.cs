using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using RestSharp;
using ApiNetsuite.Clases;
using Newtonsoft.Json;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ApiNetsuite.DTO.Turno;
using ApiNetsuite.Repositorio.IRepository;
using ApiNetsuite.DTO.General;

namespace ApiNetsuite.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("Turno")]
    public class TurnoController : Controller
    {

        private readonly ITurnoSQL repositorio;
        public TurnoController(ITurnoSQL r)
        {
            this.repositorio = r;
        }


        [HttpGet]
        [Authorize]
        [Route("Vehiculo")]
        public ActionResult<MiVehiculoResp> ConsultaTurnoVehiculo(string codigoclientevehiculo)
        {
            conexion ruta = new conexion();
            string API_URL = ruta.ObtenerRuta("706", "1");
            string HMACSHA256SignatureType = ruta.ObtenerDatos("1");
            string OAuthVersion = ruta.ObtenerDatos("2");
            var oauthConsumerKey = ruta.ObtenerDatos("3");
            var oauthToken = ruta.ObtenerDatos("4");
            var oauthConsumerSecret = ruta.ObtenerDatos("5");
            var oauthTokenSecret = ruta.ObtenerDatos("6");
            var realm = ruta.ObtenerDatos("9");
            var httpMethod = ruta.ObtenerDatos("8");
            if (codigoclientevehiculo != "")
            {
                API_URL = API_URL + "&codigoClienteVehiculo=" + codigoclientevehiculo;
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
            //MiVehiculoResp myObj = JsonConvert.DeserializeObject<MiVehiculoResp>(response.Content);
            MiVehiculoResp myObj = JsonConvert.DeserializeObject<MiVehiculoResp>("{\"results\":" + response.Content + "}");
            return myObj;
        }


        [HttpGet]
        [Authorize]
        [Route("Cliente")]
        public ActionResult<MiClienteResp> ConsultaTurnoCliente(string cedula, string nombre)
        {
            conexion ruta = new conexion();
            string API_URL = ruta.ObtenerRuta("706", "1");
            string HMACSHA256SignatureType = ruta.ObtenerDatos("1");
            string OAuthVersion = ruta.ObtenerDatos("2");
            var oauthConsumerKey = ruta.ObtenerDatos("3");
            var oauthToken = ruta.ObtenerDatos("4");
            var oauthConsumerSecret = ruta.ObtenerDatos("5");
            var oauthTokenSecret = ruta.ObtenerDatos("6");
            var realm = ruta.ObtenerDatos("9");
            var httpMethod = ruta.ObtenerDatos("8");
            //Boolean bandera = true;
            if (cedula != "" && cedula != null)
            {
                //if (cedula.Length == 13 || cedula.Length == 10)
                //{
                    API_URL = API_URL + "&cedularuc=" + cedula;
                //}
            }
            if (nombre != "" && nombre != null)
            {
                API_URL = API_URL + "&nombrecliente=" + nombre;
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
            //MiClienteResp myObj = JsonConvert.DeserializeObject<MiClienteResp>(response.Content);
            MiClienteResp myObj = JsonConvert.DeserializeObject<MiClienteResp>("{\"results\":" + response.Content + "}");
            return myObj;
        }


        [HttpGet]
        [Authorize]
        [Route("FechaTaller")]
        public ActionResult<MiTurnoResp> ConsultaPorFecha(string fecha_desde, string fecha_hasta, string taller, string verificarturno )
        {
            conexion ruta = new conexion();
            string API_URL = ruta.ObtenerRuta("706", "1");
            string HMACSHA256SignatureType = ruta.ObtenerDatos("1");
            string OAuthVersion = ruta.ObtenerDatos("2");
            var oauthConsumerKey = ruta.ObtenerDatos("3");
            var oauthToken = ruta.ObtenerDatos("4");
            var oauthConsumerSecret = ruta.ObtenerDatos("5");
            var oauthTokenSecret = ruta.ObtenerDatos("6");
            var realm = ruta.ObtenerDatos("9");
            var httpMethod = ruta.ObtenerDatos("8");
            if (fecha_desde != "" && fecha_desde != null)
            {
                API_URL = API_URL + "&turnodesde=" + Strings.Chr(34) + fecha_desde + Strings.Chr(34);
            }
            if (fecha_hasta != "" && fecha_hasta != null)
            {
                API_URL = API_URL + "&turnohasta=" + Strings.Chr(34) + fecha_hasta + Strings.Chr(34);
            }
            if (taller != "" && taller != null)
            {
                API_URL = API_URL + "&turnocodigotaller=" + taller;
            }
            if (verificarturno != "" && verificarturno != null)
            {
                API_URL = API_URL + "&verificarturno=" + verificarturno;
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
            //MiTurnoResp myObj = JsonConvert.DeserializeObject<MiTurnoResp>(response.Content);
            MiTurnoResp myObj = JsonConvert.DeserializeObject<MiTurnoResp>("{\"results\":" + response.Content + "}");
            return myObj;
        }


        [HttpPost]
        [Authorize]
        public ActionResult<TurnoMensaje> CrearTurno(TurnoDTO p)
        {
            conexion ruta = new conexion();
            string API_URL = ruta.ObtenerRuta("706", "1");
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
            //TurnoRespDTO myObj = JsonConvert.DeserializeObject<TurnoRespDTO>(response.Content);
            TurnoMensaje myObj = JsonConvert.DeserializeObject<TurnoMensaje>("{\"results\":[" + response.Content + "]}");
            return myObj;
        }


        [HttpPost]
        [Authorize]
        [Route("BorrarTurno")]
        public ActionResult<TurnoMensaje> BorrarTurno(string opcion, string turnoId)
        {
            conexion ruta = new conexion();
            string API_URL = ruta.ObtenerRuta("706", "1");
            string HMACSHA256SignatureType = ruta.ObtenerDatos("1");
            string OAuthVersion = ruta.ObtenerDatos("2");
            var oauthConsumerKey = ruta.ObtenerDatos("3");
            var oauthToken = ruta.ObtenerDatos("4");
            var oauthConsumerSecret = ruta.ObtenerDatos("5");
            var oauthTokenSecret = ruta.ObtenerDatos("6");
            var realm = ruta.ObtenerDatos("9");
            var httpMethod = ruta.ObtenerDatos("11");
            if (opcion != "" && opcion != null)
            {
                API_URL = API_URL + "&opcion=" + opcion;
            }
            if (turnoId != "" && turnoId != null)
            {
                API_URL = API_URL + "&turnoID=" + turnoId;
            }
            OAuthBase auth = new OAuthBase();
            var timestamp = Clases.OAuthBase.GenerateTimeStamp();
            var nonce = Clases.OAuthBase.GenerateNonce();
            var client = new RestClient(API_URL);
            var request = new RestRequest("", Method.Delete);
            Uri url = new Uri(API_URL);
            var signature = Clases.OAuthBase.GenerateSignature(url, oauthConsumerKey, oauthConsumerSecret, oauthToken, oauthTokenSecret, httpMethod, timestamp, nonce);
            request.AddHeader("Authorization", "OAuth realm=\"" + realm + "\", oauth_token=\"" + oauthToken + "\", oauth_consumer_key=\"" + oauthConsumerKey + "\"," + " oauth_nonce=\"" + nonce + "\", oauth_timestamp=\"" + timestamp + "\", oauth_signature_method=\"" + HMACSHA256SignatureType + "\", oauth_version=\"" + OAuthVersion + "\", oauth_signature=\"" + signature + "\"");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Cookie", "NS_ROUTING_VERSION=LAGGING");
            Console.WriteLine(request);
            var response = client.Execute(request);
            Console.WriteLine(response.Content);
            //TurnoRespDTO myObj = JsonConvert.DeserializeObject<TurnoRespDTO>(response.Content);
            TurnoMensaje myObj = JsonConvert.DeserializeObject<TurnoMensaje>("{\"results\":[" + response.Content + "]}");
            return myObj;
        }


        [HttpGet]
        [Authorize]
        [Route("InactivarTurno")]
        public ActionResult<string> InactivarTurno(string opcion, string cliente, string turno)
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

            if (cliente != "" && cliente != null)
            {
                API_URL = API_URL + "&clienteid=" + cliente;
            }
            if (turno != "" && turno != null)
            {
                API_URL = API_URL + "&idturno=" + turno;
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
            return (response.Content);
        }
    
    }
}
