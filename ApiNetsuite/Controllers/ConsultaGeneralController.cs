using ApiNetsuite.Clases;
using ApiNetsuite.DTO.Bien;
using ApiNetsuite.DTO.Convenio;
using ApiNetsuite.DTO.Factura;
using ApiNetsuite.DTO.General;
using ApiNetsuite.Repositorio;
using ApiNetsuite.Repositorio.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nancy;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.Json;

namespace ApiNetsuite.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("General")]
    public class ConsultaGeneralController : ControllerBase
    {

        private readonly ITurnoSQL repositorio;

        public ConsultaGeneralController(ITurnoSQL r)
        {
            this.repositorio = r;
        }


        [HttpGet]
        [Authorize]
        [Route("Taller")]
        public ActionResult<GenTallerResp> ConsultaTaller(string opcion, string idtaller, string codtaller, string descripcion, string idoficina)
        {
            conexion ruta = new conexion();
            string API_URL = ruta.ObtenerRuta("1181", "1");
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
            if (idtaller != "" && idtaller != null)
            {
                API_URL = API_URL + "&idtaller=" + idtaller;
            }
            if (codtaller != "" && codtaller != null)
            {
                API_URL = API_URL + "&codtaller=" + codtaller;
            }
            if (descripcion != "" && descripcion != null)
            {
                API_URL = API_URL + "&descripcion=" + descripcion;
            }
            if (idoficina != "" && idoficina != null)
            {
                API_URL = API_URL + "&idoficina=" + idoficina;
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
            //GenTallerResp myObj = JsonConvert.DeserializeObject<GenTallerResp>(response.Content);
            GenTallerResp myObj = JsonConvert.DeserializeObject<GenTallerResp>("{\"results\":" + response.Content + "}");
            return myObj;

        }


        [HttpPost]
        [Authorize]
        [Route("Tabla")]
        public ActionResult<String> ConsultaDatos(TablaDTO p)
        {
            conexion ruta = new conexion();
            //string API_URL = ruta.ObtenerRuta("4066", "1");
            string API_URL = ruta.ObtenerRuta("1070", "1");
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
            var signature = Clases.OAuthBase.GenerateSignature(url
                                                                , oauthConsumerKey
                                                                , oauthConsumerSecret
                                                                , oauthToken
                                                                , oauthTokenSecret
                                                                , httpMethod
                                                                , timestamp
                                                                , nonce);
            request.AddHeader("Authorization", "OAuth realm=\"" + realm + "\", oauth_token=\"" + oauthToken + "\", oauth_consumer_key=\"" + oauthConsumerKey + "\"," + " oauth_nonce=\"" + nonce + "\", oauth_timestamp=\"" + timestamp + "\", oauth_signature_method=\"" + HMACSHA256SignatureType + "\", oauth_version=\"" + OAuthVersion + "\", oauth_signature=\"" + signature + "\"");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", p, ParameterType.RequestBody);
            Console.WriteLine(request);
            var response = client.Execute(request);
            Console.WriteLine(response.Content);
            return response.Content;
        }


        [HttpPost]
        [Authorize]
        [Route("ConsultaGeneralBD")]
        public ActionResult<string> ConsultaGeneralBD(TablaDB p)
        {
            DataSet cnstGenrl = new DataSet();
            cnstGenrl = EstadoSQL.Cnstdatos(p.opcion, p.datSelect, p.datFrom, p.datWhere);
            string jsonData = "";
            if (cnstGenrl.Tables.Count > 0)
            {
                DataTable tabla = new DataTable();
                tabla.Columns.Add("RESULTADO", typeof(string));
                tabla = cnstGenrl.Tables[0];
                DataTable firstTable = tabla;
                ConvertJson serializar = new ConvertJson();
                jsonData = JsonConvert.SerializeObject(firstTable);
            }
            return ("{\"results\":" + jsonData + "}");
        }


        [HttpGet]
        [Authorize]
        [Route("Catalogo")]
        public ActionResult<String> Catalogo(string codigotabla, string idfactura, string idproveedor, string marca, string seriedocumento)
        {
            conexion ruta = new conexion();
            string API_URL = ruta.ObtenerRuta("1070", "1");
            string HMACSHA256SignatureType = ruta.ObtenerDatos("1");
            string OAuthVersion = ruta.ObtenerDatos("2");
            var oauthConsumerKey = ruta.ObtenerDatos("3");
            var oauthToken = ruta.ObtenerDatos("4");
            var oauthConsumerSecret = ruta.ObtenerDatos("5");
            var oauthTokenSecret = ruta.ObtenerDatos("6");
            var realm = ruta.ObtenerDatos("9");
            var httpMethod = ruta.ObtenerDatos("8");
            if (codigotabla != "" && codigotabla != null)
            {
                API_URL = API_URL + "&codigotabla=" + codigotabla;
            }
           
            if (codigotabla == "MAR")
            {
                if (marca != "" && marca != null)
                {
                    API_URL = API_URL + "&marca=" + marca;
                }
            }
            if (codigotabla == "FAC")
            {
                if (idproveedor != "" && idproveedor != null)
                {
                    API_URL = API_URL + "&idproveedor=" + idproveedor;
                }
                if (seriedocumento != "" && seriedocumento != null)
                {
                    API_URL = API_URL + "&seriedocumento=" + seriedocumento;
                }
                if (idfactura != "" && idfactura != null)
                {
                    API_URL = API_URL + "&idfactura=" + idfactura;
                }
            }
           

            //if (descripcion != "" && descripcion != null)
            //{
            //    API_URL = API_URL + "&descripcion=" + descripcion;
            //}
            //if (idoficina != "" && idoficina != null)
            //{
            //    API_URL = API_URL + "&idoficina=" + idoficina;
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
            request.AddHeader("Cookie", "NS_ROUTING_VERSION=LAGGING");
            Console.WriteLine(request);
            var response = client.Execute(request);
            Console.WriteLine(response.Content);
            //GenTallerResp myObj = JsonConvert.DeserializeObject<GenTallerResp>(response.Content);
            //GenTallerResp myObj = JsonConvert.DeserializeObject<GenTallerResp>("{\"results\":" + response.Content + "}");
            //return myObj;
            return ("{\"results\":" + response.Content + "}");
        }


        [HttpPost]
        [Authorize]
        [Route("CatalogoJson")]
        public ActionResult<String> CatalogoJson(TablaJsonDTO p)
        {
            conexion ruta = new conexion();
            string API_URL = ruta.ObtenerRuta("1070", "1");
            string HMACSHA256SignatureType = ruta.ObtenerDatos("1");
            string OAuthVersion = ruta.ObtenerDatos("2");
            var oauthConsumerKey = ruta.ObtenerDatos("3");
            var oauthToken = ruta.ObtenerDatos("4");
            var oauthConsumerSecret = ruta.ObtenerDatos("5");
            var oauthTokenSecret = ruta.ObtenerDatos("6");
            var realm = ruta.ObtenerDatos("9");
            var httpMethod = ruta.ObtenerDatos("8");
            p.codigotabla=p.codigotabla.ToUpper();
            if (p.codigotabla != "" && p.codigotabla != null)
            {
                API_URL = API_URL + "&codigotabla=" + p.codigotabla;
            }
            if (p.tiporetorno != "" && p.tiporetorno != null)
            {
                API_URL = API_URL + "&tiporetorno=" + p.tiporetorno;
            }
            if (p.codigotabla == "FAC")
            {
                if (p.idproveedor != "" && p.idproveedor != null)
                {
                    API_URL = API_URL + "&idproveedor=" + p.idproveedor;
                }
                if (p.seriedocumento != "" && p.seriedocumento != null)
                {
                    API_URL = API_URL + "&seriedocumento=" + p.seriedocumento;
                }
                if (p.idfactura != "" && p.idfactura != null)
                {
                    API_URL = API_URL + "&idfactura=" + p.idfactura;
                }
            }
            if (p.codigotabla == "IVA")
            {
                if ( p.idiva != null)
                {
                    API_URL = API_URL + "&idiva=" + p.idiva;
                }
            }
            if (p.codigotabla == "MAR")
            {
                if (p.marca != null)
                {
                    API_URL = API_URL + "&marca=" + p.marca;
                }
            }
            if (p.codigotabla == "MDA")
            {
                if (p.marca != null)
                {
                    API_URL = API_URL + "&marca=" + p.marca;
                }
                if (p.modelo != null)
                {
                    API_URL = API_URL + "&modelo=" + p.modelo;
                }
                if ( p.indice != null)
                {
                    API_URL = API_URL + "&indice=" + p.indice;
                }
            }
            if ((p.codigotabla == "TTE")  || (p.codigotabla == "TDI") || (p.codigotabla == "TMAI") || (p.codigotabla == "TVH") ||
                (p.codigotabla == "TCLI") || (p.codigotabla == "OFI") || (p.codigotabla == "TID")  || (p.codigotabla == "TAC") || 
                (p.codigotabla == "SEX"))
            {
                if (p.id != "" && p.id != null)
                {
                    API_URL = API_URL + "&id=" + p.id;
                }             
            }
            if (p.codigotabla == "ITEM") 
            {
                if (p.idproducto != null)
                {
                    API_URL = API_URL + "&idproducto=" + p.idproducto;
                }
                if (p.nivelprecio != null)
                {
                    API_URL = API_URL + "&nivelprecio=" + p.nivelprecio;
                }
            }
            if (p.codigotabla == "ITEM_PARAM")
            {
                if (p.idproducto != null)
                {
                    API_URL = API_URL + "&idproducto=" + p.idproducto;
                }
                if (p.parametro != "" && p.parametro != null)
                {
                    API_URL = API_URL + "&parametro=" + p.parametro;
                }
            }
            if (p.codigotabla == "PRO_PARAM")
            {
                if (p.grupoproducto != null)
                {
                    API_URL = API_URL + "&grupoproducto=" + p.grupoproducto;
                }
                if (p.parametro != null)
                {
                    API_URL = API_URL + "&parametro=" + p.parametro;
                }
                if (p.valor != null)
                {
                    API_URL = API_URL + "&valor=" + p.valor;
                }
            }
            if (p.codigotabla == "PROV") 
            {
                if ( p.idprovincia != null)
                {
                    API_URL = API_URL + "&idprovincia=" + p.idprovincia;
                }
            }
            if (p.codigotabla == "CIU")
            {
                if ( p.idprovincia != null)
                {
                    API_URL = API_URL + "&idprovincia=" + p.idprovincia;
                }
                if ( p.idcanton != null)
                {
                    API_URL = API_URL + "&idcanton=" + p.idcanton;
                }
                if ( p.indice != null)
                {
                    API_URL = API_URL + "&indice=" + p.indice;
                }
            }
            if (p.codigotabla == "PARR") 
            {
                if (p.idprovincia != null)
                {
                    API_URL = API_URL + "&idprovincia=" + p.idprovincia;
                }
                if ( p.idcanton != null)
                {
                    API_URL = API_URL + "&idcanton=" + p.idcanton;
                }
                if ( p.idparroquia != null)
                {
                    API_URL = API_URL + "&idparroquia=" + p.idparroquia;
                }
                if ( p.indice != null)
                {
                    API_URL = API_URL + "&indice=" + p.indice;
                }
            }
            if (p.codigotabla == "TVHD") 
            {
                if (p.tipovehiculo != null)
                {
                    API_URL = API_URL + "&tipovehiculo=" + p.tipovehiculo;
                }
                if (p.tipovehdet != "" && p.tipovehdet != null)
                {
                    API_URL = API_URL + "&tipovehdet=" + p.tipovehdet;
                }
            }
            if ((p.codigotabla == "ASE") || (p.codigotabla == "FIN") || (p.codigotabla == "CON") || (p.codigotabla == "VND"))
            {
                if (p.idcanal != null)
                {
                    API_URL = API_URL + "&idcanal=" + p.idcanal;
                }
                if (p.ruc != null)
                {
                    API_URL = API_URL + "&ruc=" + p.ruc;
                }
                if (p.criterio != null)
                {
                    API_URL = API_URL + "&criterio=" + p.criterio;
                }
                if (p.indice != null)
                {
                    API_URL = API_URL + "&indice=" + p.indice;
                }
            }
            if (p.codigotabla == "COL") 
            {
                if ( p.idcolor != null)
                {
                    API_URL = API_URL + "&idcolor=" + p.idcolor;
                }
            }
            if (p.codigotabla == "PAI")
            {
                if ( p.idpais != null)
                {
                    API_URL = API_URL + "&idpais=" + p.idpais;
                }
                if ( p.criterio != null)
                {
                    API_URL = API_URL + "&criterio=" + p.criterio;
                }
            }
            if (p.codigotabla == "EJE" || (p.codigotabla == "USER") )
            {
                if (p.idejecutiva != null)
                {
                    API_URL = API_URL + "&idejecutiva=" + p.idejecutiva;
                }
                if ( p.criterio != null)
                {
                    API_URL = API_URL + "&criterio=" + p.criterio;
                }
                if (p.clase != null)
                {
                    API_URL = API_URL + "&clase=" + p.clase;
                }
                if ( p.ciudad != null)
                {
                    API_URL = API_URL + "&ciudad=" + p.ciudad;
                }             
            }
            if (p.codigotabla == "ITEM_COMERCIAL")
            {           
                if (p.item != null && p.item != "")
                {
                    API_URL = API_URL + "&item=" + p.item;
                }
                if (p.transaccion != null && p.transaccion != "")
                {
                    API_URL = API_URL + "&transaccion=" + p.transaccion;
                }
                if (p.idproducto != null && p.idproducto != "")
                {
                    API_URL = API_URL + "&idproducto=" + p.idproducto;
                }
                if (p.id != null && p.id  != "0")
                {
                    API_URL = API_URL + "&id=" + p.id;
                }
            }
            if (p.codigotabla == "CNV")
            {
                if (p.descripcion != null)
                {
                    API_URL = API_URL + "&descripcion=" + p.descripcion;
                }
                if (p.codigo != null)
                {
                    API_URL = API_URL + "&codigo=" + p.codigo;
                }

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
            //GenTallerResp myObj = JsonConvert.DeserializeObject<GenTallerResp>(response.Content);
            //GenTallerResp myObj = JsonConvert.DeserializeObject<GenTallerResp>("{\"results\":" + response.Content + "}");
            //return myObj;
            return ("{\"results\":" + response.Content + "}");
        }


        [HttpPost]
        [Route("/CodificarString")]
        [Produces("application/json")]
        [AllowAnonymous]
        public ActionResult<String> CodificarString([FromBody] Encriptar jsonData)
        {
            if (jsonData.data != null )
            {
                   List<string> encodedStrings = Utilidades.EncryptStrings(jsonData.data);
                   return Ok(encodedStrings);
            }
            else
            {
                // Si "data" no es una lista o no existe, retorna un mensaje apropiado
                return BadRequest("La clave 'data' es requerida y debe ser una lista de strings.");
            }

        }


        [HttpGet]
        [Authorize]
        [Route("DatosNS")]
        public ActionResult<String> DatosNS(string idcript, string parametros)
        {
            conexion ruta = new conexion();
            string API_URL = ruta.ObtenerRuta(idcript, "1");
            parametros = parametros.Replace("|", "&");
            API_URL = API_URL + "&" + parametros;
            string HMACSHA256SignatureType = ruta.ObtenerDatos("1");
            string OAuthVersion = ruta.ObtenerDatos("2");
            var oauthConsumerKey = ruta.ObtenerDatos("3");
            var oauthToken = ruta.ObtenerDatos("4");
            var oauthConsumerSecret = ruta.ObtenerDatos("5");
            var oauthTokenSecret = ruta.ObtenerDatos("6");
            var realm = ruta.ObtenerDatos("9");
            var httpMethod = ruta.ObtenerDatos("8");
            OAuthBase auth = new OAuthBase();
            var timestamp = Clases.OAuthBase.GenerateTimeStamp();
            var nonce = Clases.OAuthBase.GenerateNonce();
            var client = new RestClient(API_URL);
            var request = new RestRequest("", Method.Get);
            Uri url = new Uri(API_URL);
            var signature = Clases.OAuthBase.GenerateSignature(url
                                                                , oauthConsumerKey
                                                                , oauthConsumerSecret
                                                                , oauthToken
                                                                , oauthTokenSecret
                                                                , httpMethod
                                                                , timestamp
                                                                , nonce);
            request.AddHeader("Authorization", "OAuth realm=\"" + realm + "\", oauth_token=\"" + oauthToken + "\", oauth_consumer_key=\"" + oauthConsumerKey + "\"," + " oauth_nonce=\"" + nonce + "\", oauth_timestamp=\"" + timestamp + "\", oauth_signature_method=\"" + HMACSHA256SignatureType + "\", oauth_version=\"" + OAuthVersion + "\", oauth_signature=\"" + signature + "\"");
            request.AddHeader("Content-Type", "application/json");
            //request.AddParameter("application/json", "", ParameterType.RequestBody);
            Console.WriteLine(request);
            var response = client.Execute(request);
            Console.WriteLine(response.Content);
            //return response.Content;
            return ("{\"results\":" + response.Content + "}");
        }


        [HttpPost]
        [Authorize]
        [Route("EnvioDatosNS")]
        public ActionResult<String> EnvioDatosNS(string idcript, string body)
        {
            conexion ruta = new conexion();

            string cadena = "";
            DataSet cnstGenrl = new DataSet();
            cnstGenrl = EstadoSQL.Cnstcadena(opcion: "100", idcadena: body);
            if (cnstGenrl.Tables.Count > 0)
            {
                cadena = (string)cnstGenrl.Tables[0].Rows[0]["CADENA"];
            }

            string API_URL = ruta.ObtenerRuta(idcript, "1");
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
            request.AddParameter("application/json", cadena, ParameterType.RequestBody);
            Console.WriteLine(request);
            var response = client.Execute(request);
            Console.WriteLine(response.Content);
            return response.Content;
        }


        [HttpPost]
        [Authorize]
        [Route("ActualizaPlaforma")]
        public ActionResult<string> ActualizaPlaforma(PlataformaDTO p)
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
            if (p.opcion != "" && p.opcion != null)
            {
                API_URL = API_URL + "&opcion=" + p.opcion;
            }
            if (p.idbien != "" && p.idbien != null)
            {
                API_URL = API_URL + "&idbien=" + p.idbien;
            }

            if (p.clienteid != "" && p.clienteid != null)
            {
                API_URL = API_URL + "&clienteid=" + p.clienteid;
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
