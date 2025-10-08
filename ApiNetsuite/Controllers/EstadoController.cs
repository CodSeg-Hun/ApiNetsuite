using ApiNetsuite.Clases;
using ApiNetsuite.DTO.Estado;
using ApiNetsuite.Repositorio.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ApiNetsuite.Controllers
{
    [ApiController]
    [Route("Estado")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EstadoController : Controller
    {

        private readonly IEstadoSQL repositorio;
        public EstadoController(IEstadoSQL r)
        {
            this.repositorio = r;
        }


        [HttpPost]
        [Authorize]
        public ActionResult<EstadoRespDTO> ActualizarEstado(EstadoDTO p)
        {
            EstadoRespDTO estado = null;
            bool bandera = true;
            string mensaje = "";
            if (p.plataforma == "PX")
            {
                //vehiculo
                if (p.idVehiculo == "" || p.idVehiculo.ToUpper() == "NULL")
                {
                    mensaje = "Debe enviar el Internal Id del Vehiculo";
                    bandera = false;
                }else if (p.codVehiculo == "" || p.codVehiculo.ToUpper() == "NULL" )
                {
                    mensaje = "Debe enviar el código del Vehiculo";
                    bandera = false;
                }else if (p.idMarcaVehiculo == "" || p.idMarcaVehiculo.ToUpper() == "NULL")
                {
                    mensaje = "Debe enviar el código marca del Vehículo";
                    bandera = false;
                }else if (p.marcaVehiculo == "" || p.marcaVehiculo.ToUpper() == "NULL") 
                {
                    mensaje = "Debe enviar la marca del Vehículo";
                    bandera = false;
                }else if (p.idModeloVehiculo == "" || p.idModeloVehiculo.ToUpper() == "NULL")
                {
                    mensaje = "Debe enviar el código modelo del Vehículo";
                    bandera = false;
                }else if (p.modeloVehiculo == "" || p.modeloVehiculo.ToUpper() == "NULL")
                {
                    mensaje = "Debe enviar el modelo del Vehículo";
                    bandera = false;
                }else if (p.tipoVehiculo == "" || p.tipoVehiculo.ToUpper() == "NULL")
                {
                    mensaje = "Debe enviar el Tipo del Vehículo";
                    bandera = false;
                }else if (p.colorVehiculo == "" || p.colorVehiculo.ToUpper() == "NULL")
                {
                    mensaje = "Debe enviar el color del Vehículo";
                    bandera = false;
                }else if (p.anioVehiculo == "" || p.anioVehiculo.ToUpper() == "NULL")
                {
                    mensaje = "Debe enviar el año del Vehículo";
                    bandera = false;
                }
                //dispositivo
                if (p.vidDispositivo == "" || p.vidDispositivo.ToUpper() == "NULL")
                {
                    mensaje = "Debe enviar el VID del Vehiculo";
                    bandera = false;
                }else if (p.idfamiliaProdructo == "" || p.idfamiliaProdructo.ToUpper() == "NULL")
                {
                    mensaje = "Debe enviar id familia de producto";
                    bandera = false;
                }else if (p.familiaProdructo == "" || p.familiaProdructo.ToUpper() == "NULL")
                {
                    mensaje = "Debe enviar familia de producto";
                    bandera = false;
                }else if (p.idmarcaDispositivo == "" || p.idmarcaDispositivo.ToUpper() == "NULL")
                {
                    mensaje = "Debe enviar id Marca de dispositivo";
                    bandera = false;
                }else if (p.marcaDispositivo == "" || p.marcaDispositivo.ToUpper() == "NULL")
                {
                    mensaje = "Debe enviar Marca de dispositivo";
                    bandera = false;
                }else if (p.idmodeloDispositivo == "" || p.idmodeloDispositivo.ToUpper() == "NULL")
                {
                    mensaje = "Debe enviar id Modelo de dispositivo";
                    bandera = false;
                }else if (p.modeloDispositivo == "" || p.modeloDispositivo.ToUpper() == "NULL")
                {
                    mensaje = "Debe enviar Modelo de dispositivo";
                    bandera = false;
                }else if (p.serieDispositivo == "" || p.serieDispositivo.ToUpper() == "NULL")
                {
                    mensaje = "Debe enviar Serie de dispositivo";
                    bandera = false;
                }
                else if (p.imeiDispositivo == "" ||  p.imeiDispositivo.ToUpper() == "NULL")
                {
                    mensaje = "Debe enviar Imei de dispositivo";
                    bandera = false;
                }else if ((p.direccionMacSim == "" || p.direccionMacSim.ToUpper() == "NULL") && p.marcaDispositivo == "TELTONIKA" && p.modeloDispositivo == "GH520")
                {
                    mensaje = "Debe enviar Dirección MAC de Sim";
                    bandera = false;
                }else if (p.serieSim == "" || p.serieSim.ToUpper() == "NULL")
                {
                    mensaje = "Debe enviar Serie de Sim";
                    bandera = false;
                }else if (p.numeroCelularSim == "" || p.numeroCelularSim.ToUpper() == "NULL")
                {
                    mensaje = "Debe enviar Número celular de Sim";
                    bandera = false;
                }else if (p.operadoraSim == "" || p.operadoraSim.ToUpper() == "NULL")
                {
                    mensaje = "Debe enviar Operadora de Sim";
                    bandera = false;
                }else if (p.estadoFamiliaProducto == "" || p.estadoFamiliaProducto.ToUpper() == "NULL")
                {
                    mensaje = "Debe enviar Estado de famila de Producto";
                    bandera = false;
                }
                //usuario
                if (p.nemonicoUsuario=="")
                {
                    mensaje = "Debe enviar nemónico de Usuario ";
                    bandera = false;
                }
                if (bandera)
                {
                    string mensajePX = ConsultaPX.CambioEstado(p);
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
            }
            if (bandera)
            {
                estado = new EstadoRespDTO
                {
                    codVehiculo = p.codVehiculo,
                    plataforma = p.plataforma,
                    mensaje = mensaje,                     
                    chasisVehiculo = p.chasisVehiculo,
                    placaVehiculo = p.placaVehiculo,
                    status = 200,
                    serieDispositivo = p.serieDispositivo,
                    serieSim=p.serieSim,
                    FamiliaProducto=p.familiaProdructo
                };
            }

            else
            {
                estado = new EstadoRespDTO
                {
                    codVehiculo = p.codVehiculo,
                    plataforma = p.plataforma,
                    mensaje = mensaje,                     
                    chasisVehiculo = p.chasisVehiculo,
                    placaVehiculo = p.placaVehiculo,
                    status = 400,
                    serieDispositivo = p.serieDispositivo,
                    serieSim = p.serieSim,
                    FamiliaProducto = p.familiaProdructo
                };
            }
            return estado;
        }
    

    }
}
