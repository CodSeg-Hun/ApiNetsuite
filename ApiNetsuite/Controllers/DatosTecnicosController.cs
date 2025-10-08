using ApiNetsuite.DTO.Cliente;
using ApiNetsuite.DTO.DatosTecnicos;
using ApiNetsuite.Modelo;
using ApiNetsuite.Repositorio;
using ApiNetsuite.Repositorio.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Net;

namespace ApiNetsuite.Controllers
{
    [ApiController]
    [Route("DatosTecnicos")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DatosTecnicosController : Controller
    {

        private readonly IDatosTecnicosSQL repositorio;
        protected RespuestAPI _respuestaApi;
        public DatosTecnicosController(IDatosTecnicosSQL r)
        {
            this.repositorio = r;
            this._respuestaApi = new();
        }


        [HttpPost]
        [Authorize]
        [Route("/DatosTecnicos")]
        public ActionResult<string> DatosTecnicos(DatosTecnicoDTO data)
        {
            bool bandera = true;
            string mensaje = "";

            if (data.datoTecnicoID == null || data.datoTecnicoID == "string" || string.IsNullOrEmpty(data.datoTecnicoID.ToString()))
            {
                mensaje = "Valor de datoTecnicoID en blanco";
                bandera = false;
            }else if (data.bien == null || data.bien == "string" || string.IsNullOrEmpty(data.bien.ToString()))
            {
                mensaje = "Valor de bien en blanco";
                bandera = false;
            }else if (data.empresa_ID == null || data.empresa_ID == "string" || string.IsNullOrEmpty(data.empresa_ID.ToString()))
            {
                mensaje = "Valor de empresa_ID en blanco";
                bandera = false;
            }else if (data.cliente_Identificacion == null || data.cliente_Identificacion == "string" || string.IsNullOrEmpty(data.cliente_Identificacion.ToString()))
            {
                mensaje = "Valor de cliente_Identificacion en blanco";
                bandera = false;
            }


            if (bandera)
            {
                string Datos = DatosTecnicosSQL.DatosTecnicos(data);
                string Valor = Utilidades.Catalogo(texto: ((Datos.ToUpper()).TrimEnd()).TrimStart());
               //string Valor = "OK";
                if (Valor == "OK")
                {
                    _respuestaApi.StatusCode = HttpStatusCode.OK;
                    _respuestaApi.IsSuccess = true;
                    _respuestaApi.Result = Valor;
                    _respuestaApi.Messages.Add("Grabado con exito");
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

        }
   

    }
}
