using ApiNetsuite.Clases;
using ApiNetsuite.DTO.Banco;
using ApiNetsuite.DTO.Cliente;
using ApiNetsuite.Modelo;
using ApiNetsuite.Repositorio.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace ApiNetsuite.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("Banco")]
    public class BancoController : ControllerBase
    {

        private readonly IBancoSQL repositorio;
        protected RespuestAPI _respuestaApi;
        public BancoController(IBancoSQL r)
        {
            this.repositorio = r;
            this._respuestaApi = new();
        }


        [HttpGet]
        [Authorize]
        [Route("SaldoBanco")]
        public ActionResult<SaldoBancoResp> SaldoBanco(string fechaInicio,  string opcion, string indice, string fechaCorte)
        {
            conexion ruta = new conexion();
            string API_URL = ruta.ObtenerRuta("2016", "1");
           // string API_URL = ruta.ObtenerRuta("1404", "1");
            string HMACSHA256SignatureType = ruta.ObtenerDatos("1");
            string OAuthVersion = ruta.ObtenerDatos("2");
            var oauthConsumerKey = ruta.ObtenerDatos("3");
            var oauthToken = ruta.ObtenerDatos("4");
            var oauthConsumerSecret = ruta.ObtenerDatos("5");
            var oauthTokenSecret = ruta.ObtenerDatos("6");
            var realm = ruta.ObtenerDatos("9");
            var httpMethod = ruta.ObtenerDatos("8");
            if (indice == "" || indice == null || indice == "0")
            {
                indice = "1";
            }
            if (fechaInicio != "" && fechaInicio != null)
            {
                API_URL = API_URL + "&fechaInicio=" + Strings.Chr(34) + fechaInicio + Strings.Chr(34);
            }
            if (fechaCorte != "" && fechaCorte != null)
            {
                API_URL = API_URL + "&fechaCorte=" + Strings.Chr(34) + fechaCorte + Strings.Chr(34);
            }
            if (indice != "" && indice != null)
            {
                API_URL = API_URL + "&indice=" + indice;
            }
            if (opcion != "" && opcion != null)
            {
                API_URL = API_URL + "&opcion=" + opcion;
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
            string texto_devuelto = "";
            if (response.Content == "{\"results\":[],\"index\":-1,\"pages\":0}")
            {
                _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.Messages.Add("Error, No se encuentra datos, Verificar");
                return BadRequest(_respuestaApi);
            }
            else
            {
                texto_devuelto = response.Content;
            }

            Console.WriteLine(response.Content);
            SaldoBancoResp myObj = JsonConvert.DeserializeObject<SaldoBancoResp>(texto_devuelto);
            myObj.StatusCode = HttpStatusCode.OK;
            myObj.Messages.Add("Correcto");
            return myObj;
        }


        [HttpGet]
        [Authorize]
        [Route("SaldoActual")]
        public ActionResult<ConsultaResp> Saldos(string opcion, string indice, string fechaCorte)
        {
            conexion ruta = new conexion();
            string API_URL = ruta.ObtenerRuta("2017", "1");
            //string API_URL = ruta.ObtenerRuta("1406", "1");
            string HMACSHA256SignatureType = ruta.ObtenerDatos("1");
            string OAuthVersion = ruta.ObtenerDatos("2");
            var oauthConsumerKey = ruta.ObtenerDatos("3");
            var oauthToken = ruta.ObtenerDatos("4");
            var oauthConsumerSecret = ruta.ObtenerDatos("5");
            var oauthTokenSecret = ruta.ObtenerDatos("6");
            var realm = ruta.ObtenerDatos("9");
            var httpMethod = ruta.ObtenerDatos("8");
            if (indice == "" || indice == null || indice == "0")
            {
                indice = "1";
            }
            if (indice != "" && indice != null)
            {
                API_URL = API_URL + "&indice=" + indice;
            }
            if (fechaCorte != "" && fechaCorte != null)
            {
                API_URL = API_URL + "&fechaCorte=" + Strings.Chr(34) + fechaCorte + Strings.Chr(34);
            }
            if (opcion != "" && opcion != null)
            {
                API_URL = API_URL + "&opcion=" + opcion;
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
            string texto_devuelto = "";
            if (response.Content == "{\"results\":[],\"index\":-1,\"pages\":0}")
            {
                _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.Messages.Add("Error, No se encuentra datos, Verificar");
                return BadRequest(_respuestaApi);
            }
            else
            {
                texto_devuelto = response.Content;
            }
            Console.WriteLine(response.Content);
            ConsultaResp myObj = JsonConvert.DeserializeObject<ConsultaResp>(texto_devuelto);
            myObj.StatusCode = HttpStatusCode.OK;
            myObj.Messages.Add("Correcto");
            return myObj;
        }


        [HttpGet]
        [Authorize]
        [Route("SaldoProveedor")]
        public ActionResult<SaldoProveedorResp> SaldoProveedor(string fechaInicio, string opcion, string indice, string fechaCorte)
        {
            conexion ruta = new conexion();
            string API_URL = ruta.ObtenerRuta("5953", "1");
            // string API_URL = ruta.ObtenerRuta("1404", "1");
            string HMACSHA256SignatureType = ruta.ObtenerDatos("1");
            string OAuthVersion = ruta.ObtenerDatos("2");
            var oauthConsumerKey = ruta.ObtenerDatos("3");
            var oauthToken = ruta.ObtenerDatos("4");
            var oauthConsumerSecret = ruta.ObtenerDatos("5");
            var oauthTokenSecret = ruta.ObtenerDatos("6");
            var realm = ruta.ObtenerDatos("9");
            var httpMethod = ruta.ObtenerDatos("8");
            if (indice == "" || indice == null || indice == "0")
            {
                indice = "1";
            }
            if (fechaInicio != "" && fechaInicio != null)
            {
                API_URL = API_URL + "&fechaInicio=" + Strings.Chr(34) + fechaInicio + Strings.Chr(34);
            }
            if (fechaCorte != "" && fechaCorte != null)
            {
                API_URL = API_URL + "&fechaCorte=" + Strings.Chr(34) + fechaCorte + Strings.Chr(34);
            }
            if (indice != "" && indice != null)
            {
                API_URL = API_URL + "&indice=" + indice;
            }
            if (opcion != "" && opcion != null)
            {
                API_URL = API_URL + "&opcion=" + opcion;
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
            string texto_devuelto = "";
            if (response.Content == "{\"results\":[],\"index\":-1,\"pages\":0}")
            {
                _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.Messages.Add("Error, No se encuentra datos, Verificar");
                return BadRequest(_respuestaApi);
            }
            else
            {
                texto_devuelto = response.Content;
            }

            Console.WriteLine(response.Content);
            SaldoProveedorResp myObj = JsonConvert.DeserializeObject<SaldoProveedorResp>(texto_devuelto);
            myObj.StatusCode = HttpStatusCode.OK;
            myObj.Messages.Add("Correcto");
            return myObj;
        }


    }
}



