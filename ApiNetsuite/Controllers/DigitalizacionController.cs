using ApiNetsuite.Clases;
using ApiNetsuite.DTO.Digitalizacion;
using ApiNetsuite.DTO.Factura;
using ApiNetsuite.DTO.General;
using ApiNetsuite.Modelo;
using ApiNetsuite.Repositorio;
using ApiNetsuite.Repositorio.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Data;
using System.Net;
using System.Reflection;
using System.ServiceModel.Channels;

namespace ApiNetsuite.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("Digitalizacion")]
    public class DigitalizacionController : Controller
    {

        private readonly IDigitalizacionSQL repositorio;
        protected RespuestAPI _respuestaApi;

        public DigitalizacionController(IDigitalizacionSQL r)
        {
            this.repositorio = r;
            this._respuestaApi = new();
        }


        [HttpGet]
        [Authorize]
        [Route("/ConsultaDocumento")]
        public ActionResult<Digitalizacion> ConsultaDocumento(string opcion, string codigo, string proveedor, string referencia)
        {
            Digitalizacion respuesta;
            bool bandera = true;
            string mensaje = "";
            if (opcion == null || string.IsNullOrEmpty(opcion.ToString()))
            {
                mensaje = "Valor de Opcion en blanco";
                bandera = false;
            }else if (codigo == null || string.IsNullOrEmpty(codigo.ToString()))
            {
                mensaje = "Valor de codigo en blanco";
                bandera = false;
            }else if (proveedor == null || string.IsNullOrEmpty(proveedor.ToString()))
            {
                mensaje = "Valor de proveedor en blanco";
                bandera = false;
            }else if (referencia == null || string.IsNullOrEmpty(referencia.ToString()))
            {
                mensaje = "Valor de referencia en blanco";
                bandera = false;
            }
            if (bandera)
            {
                DataSet cnstGenrl = DigitalizacionSQL.ConsultarDocumento(opcion, codigo, proveedor, referencia);
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
                        //_respuestaApi.StatusCode = HttpStatusCode.BadRequest;
                        //_respuestaApi.IsSuccess = false;
                        //_respuestaApi.Messages.Add("Error, No se existen datos, Verificar");
                        //return BadRequest(_respuestaApi);
                        jsonData = "[{\"Codigo\": \"\"}]";
                    }
                    respuesta = JsonConvert.DeserializeObject<Digitalizacion>("{\"respuesta\":" + jsonData + "}");
                }else
                {
                    _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
                    _respuestaApi.IsSuccess = false;
                    _respuestaApi.Messages.Add("Error, No se encuentra datos, Verificar");
                    return BadRequest(_respuestaApi);
                }
            }else
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
        [Route("/ActualizaDocumento")]
        public ActionResult<Digitalizacion> ActualizaDocumento(ActualizarDocumento p)
        {
            Digitalizacion respuesta;
            bool bandera = true;
            string mensaje = "";
            if (p.opcion == null || string.IsNullOrEmpty(p.opcion.ToString()))
            {
                mensaje = "Valor de Opcion en blanco";
                bandera = false;
            }else if (p.codigo == null || string.IsNullOrEmpty(p.codigo.ToString()))
            {
                mensaje = "Valor de codigo en blanco";
                bandera = false;
            }else if (p.proveedor == null || string.IsNullOrEmpty(p.proveedor.ToString()))
            {
                mensaje = "Valor de proveedor en blanco";
                bandera = false;
            }else if (p.referencia == null || string.IsNullOrEmpty(p.referencia.ToString()))
            {
                mensaje = "Valor de referencia en blanco";
                bandera = false;
            }else if (p.codigoEscaneo == null || string.IsNullOrEmpty(p.codigoEscaneo.ToString()))
            {
                mensaje = "Valor de Código Escaneo en blanco";
                bandera = false;
            }else if (p.numeroDocumento == null || string.IsNullOrEmpty(p.numeroDocumento.ToString()))
            {
                mensaje = "Valor de Numero Documento en blanco";
                bandera = false;
            }else if (p.serie == null || string.IsNullOrEmpty(p.serie.ToString()))
            {
                mensaje = "Valor de Serie en blanco";
                bandera = false;
            }else if (p.usuario == null || string.IsNullOrEmpty(p.usuario.ToString()))
            {
                mensaje = "Valor de Usuario en blanco";
                bandera = false;
            }
            if (bandera)
            {
                DataSet cnstGenrl = DigitalizacionSQL.ActualizarDocumento(p.opcion, p.codigo, p.proveedor, p.referencia, p.codigoEscaneo, p.numeroDocumento, p.serie, p.usuario) ;
                if (cnstGenrl.Tables.Count > 0)
                {
                    DataTable tabla = new DataTable();
                    tabla.Columns.Add("RESULTADO", typeof(string));
                    string jsonData = "";
                    if (cnstGenrl.Tables[0].Rows.Count > 0)
                    {
                        jsonData = (string)Utilidades.DataTableToJSON(cnstGenrl.Tables[0]);
                        jsonData = jsonData.ToUpper();
                    }else
                    {
                        _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
                        _respuestaApi.IsSuccess = false;
                        _respuestaApi.Messages.Add("Error, No se existen datos, Verificar");
                        return BadRequest(_respuestaApi);
                    }
                    respuesta = JsonConvert.DeserializeObject<Digitalizacion>("{\"respuesta\":" + jsonData + "}");
                }else
                {
                    _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
                    _respuestaApi.IsSuccess = false;
                    _respuestaApi.Messages.Add("Error, No se encuentra datos, Verificar");
                    return BadRequest(_respuestaApi);
                }

            }else
            {
                _respuestaApi.StatusCode = HttpStatusCode.BadRequest;
                _respuestaApi.IsSuccess = false;
                _respuestaApi.Messages.Add(mensaje);
                return BadRequest(_respuestaApi);
            }
            return respuesta;
        }


    }
}
