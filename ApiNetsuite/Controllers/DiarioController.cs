using ApiNetsuite.Clases;
using ApiNetsuite.DTO.Diario;
using ApiNetsuite.Repositorio;
using ApiNetsuite.Repositorio.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Data;

namespace ApiNetsuite.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("Diario")]

    public class DiarioController
    {

        private readonly IDiarioSQL repositorio;
        public DiarioController(IDiarioSQL r)
        {
            this.repositorio = r;
        }


        [HttpPost]
        [Authorize]
        [Route("AsientoDiario")]
        public ActionResult<AsientoDiarioResp> AsientoDiario(AsientoDiario p)
        {
            conexion ruta = new conexion();
            string API_URL = ruta.ObtenerRuta("689", "1");
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
            AsientoDiarioResp myObj = JsonConvert.DeserializeObject<AsientoDiarioResp>(response.Content);
            return myObj;
        }


        [HttpPost]
        [Authorize]
        [Route("AsientoDiarioEnviar")]
        public ActionResult<AsientoDiarioResp> AsientoDiarioEnviar([FromBody] AsientoDiarioPost p)
        {
            DataSet cnstGenrl = new DataSet();
            AsientoDiarioResp myObj = null;
            cnstGenrl = DiarioSQL.CnstDiarios(p.secuenciaid, p.proceso);
            if (cnstGenrl.Tables.Count > 0)
            {
                DataTable tabla = new DataTable();
                tabla.Columns.Add("lines", typeof(string));
                string jsonData = "";
                if (cnstGenrl.Tables[0].Rows.Count > 0)
                {
                    tabla = cnstGenrl.Tables[0];
                    DataTable firstTable = tabla;
                    ConvertJson serializar = new ConvertJson();
                    jsonData = serializar.SerializarJson(firstTable, "lines");
                }
                conexion ruta = new conexion();
                string API_URL = ruta.ObtenerRuta("689", "1");
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
                request.AddParameter("application/json", jsonData, ParameterType.RequestBody);
                Console.WriteLine(request);
                var response = client.Execute(request);
                Console.WriteLine(response.Content);
                myObj = JsonConvert.DeserializeObject<AsientoDiarioResp>(response.Content);
            }
            return myObj;
        }
    
    
    }
}
