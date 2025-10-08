using ApiNetsuite.Clases;
using ApiNetsuite.DTO.Activo;
using ApiNetsuite.Modelo;
using ApiNetsuite.Repositorio.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Net;

namespace ApiNetsuite.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("Activo")]
    public class ActivoController
    {

        private readonly IActivoSQL repositorio;
        protected RespuestAPI _respuestaApi;

        public ActivoController(IActivoSQL r)
        {
            this.repositorio = r;
            this._respuestaApi = new();
        }


        [HttpPut]
        [Authorize]
        [Route("ActualizaDiario")]
        public ActionResult<ActualizarResp> ActualizaDiario([FromBody]  ActualizaDiario p)
        {
            conexion ruta = new conexion();
            string API_URL = ruta.ObtenerRuta("1497", "1");
            string HMACSHA256SignatureType = ruta.ObtenerDatos("1");
            string OAuthVersion = ruta.ObtenerDatos("2");
            var oauthConsumerKey = ruta.ObtenerDatos("3");
            var oauthToken = ruta.ObtenerDatos("4");
            var oauthConsumerSecret = ruta.ObtenerDatos("5");
            var oauthTokenSecret = ruta.ObtenerDatos("6");
            var realm = ruta.ObtenerDatos("9");
            var httpMethod = ruta.ObtenerDatos("10");
            OAuthBase auth = new OAuthBase();
            var timestamp = Clases.OAuthBase.GenerateTimeStamp();
            var nonce = Clases.OAuthBase.GenerateNonce();
            var client = new RestClient(API_URL);
            var request = new RestRequest("", Method.Put);
            Uri url = new Uri(API_URL);
            var signature = Clases.OAuthBase.GenerateSignature(url, oauthConsumerKey, oauthConsumerSecret, oauthToken, oauthTokenSecret, httpMethod, timestamp, nonce);
            request.AddHeader("Authorization", "OAuth realm=\"" + realm + "\", oauth_token=\"" + oauthToken + "\", oauth_consumer_key=\"" + oauthConsumerKey + "\"," + " oauth_nonce=\"" + nonce + "\", oauth_timestamp=\"" + timestamp + "\", oauth_signature_method=\"" + HMACSHA256SignatureType + "\", oauth_version=\"" + OAuthVersion + "\", oauth_signature=\"" + signature + "\"");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", p, ParameterType.RequestBody);
            Console.WriteLine(request);
            var response = client.Execute(request);
            Console.WriteLine(response.Content);
            string texto_devuelto = "";
            if (response.Content == "No se encontraron Activos por actualizar.")
            {
                texto_devuelto = "{\"results\":[{\"IdAsiento\": \"\",\"CodAsiento\": \"\",\"Glosa\": \"NO existen pendientes de actualizacion\",\"Diario\": \"\",\"IdActivo\": \"\",\"IdGlosa\": \"\",\"CodActivo\": \"\",\"IdAsientoAF\": \"\",\"Actualizado\": \"No\"}]}";
                //_respuestaApi.StatusCode = HttpStatusCode.BadRequest;
                //_respuestaApi.IsSuccess = false;
                //_respuestaApi.ErrorMessages.Add("No se encuentra datos, Verificar");
                //return BadRequest(_respuestaApi);
            }
            else 
            {
                texto_devuelto = response.Content;
            }
            ActualizarResp respuesta = null;
            respuesta = JsonConvert.DeserializeObject<ActualizarResp>(texto_devuelto);
            return respuesta;
        }


    }
}
