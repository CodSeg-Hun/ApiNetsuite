using ApiNetsuite.Clases;
using ApiNetsuite.DTO.Cobertura;
using ApiNetsuite.DTO.Diario;
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
using System.Drawing;
using System.Net;

namespace ApiNetsuite.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("Cobertura")]
    public class CoberturaController : Controller
    {

        private readonly ICoberturaSQL repositorio;
        protected RespuestAPI _respuestaApi;

        public CoberturaController(ICoberturaSQL r)
        {
            this.repositorio = r;
            this._respuestaApi = new();
        }


        [HttpPost]
        [Authorize]
        [Route("/HistoricoCobertura")]
        public ActionResult<string> HistoricoCobertura(CoberturaHistorico p)
        {
            string respuesta="";
            bool bandera = true;
            string mensaje = "";
            if (p.bienId == null || p.bienId == "string"  || string.IsNullOrEmpty(p.bienId.ToString()))
            {
                mensaje = "Valor de bienId en blanco";
                bandera = false;
            }
            else if (p.ordenId == null || p.ordenId == "string"  || string.IsNullOrEmpty(p.ordenId.ToString()))
            {
                mensaje = "Valor de ordenId en blanco";
                bandera = false;
            }
            else if (p.numeroGeneral == null || p.numeroGeneral == "string" || string.IsNullOrEmpty(p.numeroGeneral.ToString()))
            {
                mensaje = "Valor de Numero General en blanco";
                bandera = false;
            }
            else if (p.origen == null || p.origen == "string"  || string.IsNullOrEmpty(p.origen.ToString()))
            {
                mensaje = "Valor de origen en blanco";
                bandera = false;
            }
            else if (p.docOrigenId == null || p.docOrigenId == "string"  || string.IsNullOrEmpty(p.docOrigenId.ToString()))
            {
                mensaje = "Valor de docOrigenId en blanco";
                bandera = false;
            }
            else if (p.productoId == null || p.productoId == "string" || string.IsNullOrEmpty(p.productoId.ToString()))
            {
                mensaje = "Valor de productoId en blanco";
                bandera = false;
            }
            else if (p.producto == null || p.producto == "string"  || string.IsNullOrEmpty(p.producto.ToString()))
            {
                mensaje = "Valor de producto en blanco";
                bandera = false;
            }
            else if (p.fechaInicio == null || p.fechaInicio == "string"  || string.IsNullOrEmpty(p.fechaInicio.ToString()))
            {
                mensaje = "Valor de fecha Inicio en blanco";
                bandera = false;
            }
            else if (p.fechaFin == null || p.fechaFin == "string" || string.IsNullOrEmpty(p.fechaFin.ToString()))
            {
                mensaje = "Valor de fecha Fin en blanco";
                bandera = false;
            }
            else if (p.plazo == null || p.plazo == "string"  || string.IsNullOrEmpty(p.plazo.ToString()))
            {
                mensaje = "Valor de plazo en blanco";
                bandera = false;
            }
            else if (p.tipoPlazo == null || p.tipoPlazo == "string"  || string.IsNullOrEmpty(p.tipoPlazo.ToString()))
            {
                mensaje = "Valor de tipo Plazo en blanco";
                bandera = false;
            }
            else if (p.estadoId == null || p.estadoId == "string" || string.IsNullOrEmpty(p.estadoId.ToString()))
            {
                mensaje = "Valor de estadoId en blanco";
                bandera = false;
            }
            else if (p.estadoInstalacion == null || p.estadoInstalacion == "string" || string.IsNullOrEmpty(p.estadoInstalacion.ToString()))
            {
                mensaje = "Valor de estado Instalacion en blanco";
                bandera = false;
            }
            else if (p.ejecutivaId == null || p.ejecutivaId == "string" || string.IsNullOrEmpty(p.ejecutivaId.ToString()))
            {
                mensaje = "Valor de ejecutivaId en blanco";
                bandera = false;
            }

            else if (p.usuarioId == null || string.IsNullOrEmpty(p.usuarioId.ToString()))
            {
                mensaje = "Valor de usuarioId en blanco";
                bandera = false;
            }
            if (bandera)
            {
                DataSet cnstGenrl = CoberturaSQL.ActualizarHistorico(p.bienId, p.ordenId, p.numeroGeneral, p.origen, p.docOrigenId, p.productoId, p.producto,
                                                                     p.fechaInicio, p.fechaFin, p.plazo, p.tipoPlazo, p.estadoId,
                                                                     p.fechaInicioAnt, p.fechaFinAnt, p.estadoInstalacion, p.ejecutivaId, p.usuarioId);
                if (cnstGenrl.Tables.Count > 0)
                {
                    DataTable tabla = new DataTable();
                    tabla.Columns.Add("RESULTADO", typeof(string));
                    string jsonData = "";
                    if (cnstGenrl.Tables[0].Rows.Count > 0)
                    {
                        jsonData = (string)Utilidades.DataTableToJSON(cnstGenrl.Tables[0]);
                        jsonData = jsonData.ToUpper();
                    }
                    else
                    {
                        _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
                        _respuestaApi.IsSuccess = false;
                        _respuestaApi.Messages.Add("Error, No se existen datos, Verificar");
                        return BadRequest(_respuestaApi);
                    }
                   // respuesta = JsonConvert.DeserializeObject<Digitalizacion>("{\"respuesta\":" + jsonData + "}");
                   respuesta= ("{\"respuesta\":" + jsonData + "}");
                }
                else
                {
                    _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
                    _respuestaApi.IsSuccess = false;
                    _respuestaApi.Messages.Add("Error, No se encuentra datos, Verificar");
                    return BadRequest(_respuestaApi);
                }

            }
            else
            {
                _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.Messages.Add(mensaje);
                return BadRequest(_respuestaApi);
            }
            return respuesta;
        }


        [HttpPost]
        [Authorize]
        [Route("ActualizaCobertura")]
        public ActionResult<string> Cobertura(Cobertura p)
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
            //AsientoDiarioResp myObj = JsonConvert.DeserializeObject<AsientoDiarioResp>(response.Content);
            return response.Content;
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        [Route("/CoberturaNS")]
        public ActionResult<string> CoberturaNS(CoberturaDTO data)
        {
            //string respuesta = "";
            bool bandera = true;
            string mensaje = "";
            if (data.IdCobertura == "" || data.IdCobertura == null || data.IdCobertura == "string" || string.IsNullOrEmpty(data.IdCobertura.ToString()))
            {
                mensaje = "Valor de IdCobertura en blanco";
                bandera = false;
            }
            else if (data.IdBien == "" || data.IdBien == null || data.IdBien == "string" || string.IsNullOrEmpty(data.IdBien.ToString()))
            {
                mensaje = "Valor de IdBien en blanco";
                bandera = false;
            }
            if (data.Accion != "delete" && bandera)
            {
                //if (data.IdCobertura == "" || data.IdCobertura == null || data.IdCobertura == "string" || string.IsNullOrEmpty(data.IdCobertura.ToString()))
                //{
                //    mensaje = "Valor de IdCobertura en blanco";
                //    bandera = false;
                //}
                //else if (data.IdBien == "" || data.IdBien == null || data.IdBien == "string" || string.IsNullOrEmpty(data.IdBien.ToString()))
                //{
                //    mensaje = "Valor de IdBien en blanco";
                //    bandera = false;
                //}
                if (data.IdCliente == "" || data.IdCliente == null || data.IdCliente == "string" || string.IsNullOrEmpty(data.IdCliente.ToString()))
                {
                    mensaje = "Valor de IdCliente en blanco";
                    bandera = false;
                }
                //else if (data.NumeroSerie == "" || data.NumeroSerie == null || data.NumeroSerie == "string" || string.IsNullOrEmpty(data.NumeroSerie.ToString()))
                //{
                //    mensaje = "Valor de NumeroSerie en blanco";
                //    bandera = false;
                //}
                //else if (data.EstadoCobertura == "" || data.EstadoCobertura == null || data.EstadoCobertura == "string" || string.IsNullOrEmpty(data.EstadoCobertura.ToString()))
                //{
                //    mensaje = "Valor de EstadoCobertura en blanco";
                //    bandera = false;
                //}
                else if (data.FechaInicio == "" || data.FechaInicio == null || data.FechaInicio == "string" || string.IsNullOrEmpty(data.FechaInicio.ToString()))
                {
                    mensaje = "Valor de FechaInicio en blanco";
                    bandera = false;
                }
                else if (data.FechaFinal == "" || data.FechaFinal == null || data.FechaFinal == "string" || string.IsNullOrEmpty(data.FechaFinal.ToString()))
                {
                    mensaje = "Valor de FechaFinal en blanco";
                    bandera = false;
                }
                else if (data.CodFamiliaProducto == "" || data.CodFamiliaProducto == null || data.CodFamiliaProducto == "string" || string.IsNullOrEmpty(data.CodFamiliaProducto.ToString()))
                {
                    mensaje = "Valor de CodFamiliaProducto en blanco";
                    bandera = false;
                }
                //else if (data.NumeroDispositivo == "" || data.NumeroDispositivo == null || data.NumeroDispositivo == "string" || string.IsNullOrEmpty(data.NumeroDispositivo.ToString()))
                //{
                //    mensaje = "Valor de NumeroDispositivo en blanco";
                //    bandera = false;
                //}
            }

            if (bandera)
            {
                string Datos = CoberturaSQL.Cobertura(data);
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


    }
}
