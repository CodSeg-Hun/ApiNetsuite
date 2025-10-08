using ApiNetsuite.Clases;
using ApiNetsuite.DTO.Cliente;
using ApiNetsuite.DTO.Convenio;
using ApiNetsuite.Modelo;
using ApiNetsuite.Repositorio;
using ApiNetsuite.Repositorio.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Data;

namespace ApiNetsuite.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("Convenio")]
    public class ConvenioController
    {

        private readonly IConvenioSQL repositorio;
        public ConvenioController(IConvenioSQL r)
        {
            this.repositorio = r;
        }

        [HttpPost]
        [Authorize]
        [Route("/Ambacar/ActualizarDispositivo")]
        public ActionResult<RespAmbacarDTO> ActualizarDispositivo([FromBody] ActualizarDispositivoDTO p)
        {
            RespAmbacarDTO respuesta = null;
            string textoLogin = "";
            string valor = "";
            string valorToken = "";
            string valorTokenSeguridad = "";
            string textoSeguridad = "";
            string textoActualizar = "";
            string error = "";
            bool bandera = false;
            conexionConvenio ruta = new conexionConvenio();
            string login = ruta.GetRutaAmbacar("1");
            string authenticate = ruta.GetRutaAmbacar("2");
            string actualizardispositivo = ruta.GetRutaAmbacar("3");
            //var client = new RestClient("https://ambysoftapi.ambacar.ec/MS_SeguridadesCommand/api/Login/Login");
            var client = new RestClient(login);
            var request = new RestRequest("", Method.Post);
            textoLogin = Utilidades.SerializarDatos(p.login, p.pass, "", "", "", "", "", "", "", "", "login");
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", textoLogin, ParameterType.RequestBody);
            RestResponse response = client.Execute(request);
            valor = response.StatusCode.ToString();
            if (valor.Equals("OK"))
            {
                var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(response.Content);
                valorToken = funciones.descomponer("token", jsonString); //BusquedaDatos(response.Content, "token");
                bandera = true;
            }
            else
            {
                bandera = false;
                error = "No se pudo Obtener el token";
            }

            if (bandera)
            {
                //var seguridad = new RestClient("https://ambysoftapi.ambacar.ec/MS_SeguridadesCommand/api/Token/Authenticate");
                var seguridad = new RestClient(authenticate);
                var requestSeguridad = new RestRequest("", Method.Post);
                textoSeguridad = Utilidades.SerializarDatos(p.ip, "MS_VehiculosCommand", "", "", "", "", "", "", "", "", "seguridad");
                requestSeguridad.AddHeader("content-type", "application/json");
                requestSeguridad.AddHeader("authorization", "Bearer " + valorToken);
                requestSeguridad.AddParameter("application/json", textoSeguridad, ParameterType.RequestBody);
                RestResponse responseSeguridad = seguridad.Execute(requestSeguridad);
                valor = responseSeguridad.StatusCode.ToString();
                if (valor.Equals("OK"))
                {
                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(responseSeguridad.Content);
                    valorTokenSeguridad = funciones.descomponer("jwToken", jsonString);  //BusquedaDatos(responseSeguridad.Content, "jwToken");
                    bandera = true;
                }
                else
                {
                    bandera = false;
                    error = "No se pudo Obtener el jwToken";
                }

            }
            string resultado = "";
            if (bandera)
            {
                // var actualizar = new RestClient("https://ambysoftapi.ambacar.ec/MS_VehiculosCommand/api/HunterDispositivo/ActualizarDispositivoHunter");
                var actualizar = new RestClient(actualizardispositivo);
                var requestActualizar = new RestRequest("", Method.Put);
                textoActualizar = Utilidades.SerializarDatos(p.vinVehiculo, p.estado.ToLower(), "", "", "", "", "", "", "", "", "actualizar");
                requestActualizar.AddHeader("content-type", "application/json");
                requestActualizar.AddHeader("authorization", "Bearer " + valorTokenSeguridad);
                requestActualizar.AddParameter("application/json", textoActualizar, ParameterType.RequestBody);
                RestResponse responseActualizar = actualizar.Execute(requestActualizar);
                resultado = responseActualizar.Content;
                valor = responseActualizar.StatusCode.ToString();

                error = funciones.descomponer("Message", responseActualizar.Content);
                if (valor.Equals("OK"))
                    bandera = true;
                else
                    bandera = false;
            }
            ConvertJson serializar = new ConvertJson();
            resultado = resultado.Replace("\"", "");

            
            if (bandera)
            {
                respuesta = new RespAmbacarDTO { codigo = "1", mensaje = resultado, respuesta = error };
            }
            else
            {
                respuesta = new RespAmbacarDTO { codigo = "-1", mensaje = resultado, respuesta = error };
            }
            return respuesta;
        }


        //[HttpPost]
        ////[Authorize]
        //[Route("/Ambacar/ActualizarInformacionContable")]
        //public ActionResult<RespuestaDTO> ActualizarInformacionContable([FromBody] ActualizarInformacionContableDTO p)
        //{
        //    RespuestaDTO respuesta = null;
        //    string textoLogin = "";
        //    string valor = "";
        //    string valorToken = "";
        //    string valorTokenSeguridad = "";
        //    string textoSeguridad = "";
        //    string textoActualizar = "";
        //    bool bandera = false;
        //    conexionConvenio ruta = new conexionConvenio();
        //    string login = ruta.GetRutaAmbacar("1");
        //    string authenticate = ruta.GetRutaAmbacar("2");
        //    string actualizarinformacion = ruta.GetRutaAmbacar("4");
        //    //var client = new RestClient("https://ambysoftapi.ambacar.ec/MS_SeguridadesCommand/api/Login/Login");
        //    var client = new RestClient(login);
        //    var request = new RestRequest("", Method.Post);
        //    textoLogin = Utilidades.SerializarDatos(p.login, p.pass, "", "", "", "", "", "", "", "", "login");
        //    request.AddHeader("content-type", "application/json");
        //    request.AddParameter("application/json", textoLogin, ParameterType.RequestBody);
        //    RestResponse response = client.Execute(request);
        //    valor = response.StatusCode.ToString();
        //    if (valor.Equals("OK"))
        //    {
        //        var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(response.Content);
        //        valorToken = funciones.descomponer("token", jsonString); //BusquedaDatos(response.Content, "token");
        //        bandera = true;
        //    }
        //    else
        //    {
        //        bandera = false;
        //    }

        //    if (bandera)
        //    {
        //        //var seguridad = new RestClient("https://ambysoftapi.ambacar.ec/MS_SeguridadesCommand/api/Token/Authenticate");
        //        var seguridad = new RestClient(authenticate);
        //        var requestSeguridad = new RestRequest("", Method.Post);
        //        textoSeguridad = Utilidades.SerializarDatos(p.ip, "MS_ContabilidadCommand", "", "", "", "", "", "", "", "", "seguridad");
        //        requestSeguridad.AddHeader("content-type", "application/json");
        //        requestSeguridad.AddHeader("authorization", "Bearer " + valorToken);
        //        requestSeguridad.AddParameter("application/json", textoSeguridad, ParameterType.RequestBody);
        //        RestResponse responseSeguridad = seguridad.Execute(requestSeguridad);
        //        valor = responseSeguridad.StatusCode.ToString();
        //        if (valor.Equals("OK"))
        //        {
        //            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(responseSeguridad.Content);
        //            valorTokenSeguridad = funciones.descomponer("jwToken", jsonString);  //BusquedaDatos(responseSeguridad.Content, "jwToken");
        //            bandera = true;
        //        }
        //        else
        //            bandera = false;
        //    }

        //    string resultado = "";
        //    if (bandera)
        //    {
        //        //var actualizar = new RestClient("https://ambysoftapi.ambacar.ec/MS_ContabilidadCommand/api/ServiciosHunter/ActualizarInformacionKardexContable");
        //        var actualizar = new RestClient(actualizarinformacion);
        //        var requestActualizar = new RestRequest("", Method.Post);
        //        textoActualizar = Utilidades.SerializarDatos(p.vinVehiculo, p.numeroDocumento, p.tipoDocumento, p.trabajoRealizado.ToLower(), p.fechaTrabajo, p.ordenTrabajo, p.lugar, p.observacion, p.anioAdicionales, p.facturaAnioAdicionales, "contabilidad");
        //        requestActualizar.AddHeader("content-type", "application/json");
        //        requestActualizar.AddHeader("authorization", "Bearer " + valorTokenSeguridad);
        //        requestActualizar.AddParameter("application/json", textoActualizar, ParameterType.RequestBody);
        //        RestResponse responseActualizar = actualizar.Execute(requestActualizar);
        //        resultado = responseActualizar.Content;
        //        valor = responseActualizar.StatusCode.ToString();
        //        if (valor.Equals("OK"))
        //            bandera = true;
        //        else
        //            bandera = false;
        //    }
        //    ConvertJson serializar = new ConvertJson();
        //    resultado = resultado.Replace("\"", "");
        //    if (bandera)
        //    {
        //        respuesta = new RespuestaDTO { codigo = "1", mensaje = resultado,  };
        //    }
        //    else
        //    {
        //        respuesta = new RespuestaDTO { codigo = "-1", mensaje = resultado, };
        //    }
        //    return respuesta;
        //}


        [HttpPost]
        [Authorize]
        [Route("/DpWorld/Crear")]
        public ActionResult<RespDPWorld> CrearContrato([FromBody] CrearDPWorldDTO p)
        {
            conexion ruta = new conexion();
            string API_URL = ruta.ObtenerRuta("5155", "1");
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
            RespDPWorld myObj = JsonConvert.DeserializeObject<RespDPWorld>(response.Content);
            return myObj;
        }


        [HttpPost]
        [Authorize]
        [Route("/HunterMed/CrearContrato")]
        public ActionResult<RespuestaDTO> CrearContrato([FromBody] ContratoDTO p)
        {
            RespuestaDTO salida = null;
            conexionConvenio ruta = new conexionConvenio();
            string rutaToken = ruta.GetRutaHunterMed("1");
            string rutaCrearContrato = ruta.GetRutaHunterMed("2");
            string rutaGeneral = ruta.GetRutaHunterMed("3");
            bool bandera = true;
            bool generarContrato = true;
            string mensaje = "";
            string clienteId = "";
            string usuario = "";
            string clave = "";
            string aplicativo = "";
            string plataforma = "";
            string operativo = "";
            string navegador = "";
            string direccion = "";
            string valor = "";
            string texto = "";
            string accesstoken = "";
            string tokentype = "";
            string codigoaplicativo = "";
            string codigoplataforma = "";
            string resultado = "";
            string respuesta = "";
            string contratoNumero = "";
            string region = "";
            string producto = "";
            DataSet cnstGenrl = new DataSet();
            DataSet infoDatos = new DataSet();
            DataSet infoCliente = new DataSet();
            //parametros
            cnstGenrl = ConvenioSQL.CnstParametrosMed("100", p.cliente, "0", p.tipoplan);
            if (cnstGenrl.Tables.Count > 0)
            {
                clienteId = (string)cnstGenrl.Tables[0].Rows[0]["clienteId"];
                usuario = (string)cnstGenrl.Tables[0].Rows[0]["usuario"];
                clave = (string)cnstGenrl.Tables[0].Rows[0]["clave"];
                aplicativo = (string)cnstGenrl.Tables[0].Rows[0]["aplicativo"];
                plataforma = (string)cnstGenrl.Tables[0].Rows[0]["plataforma"];
                operativo = (string)cnstGenrl.Tables[0].Rows[0]["operativo"];
                navegador = (string)cnstGenrl.Tables[0].Rows[0]["navegador"];
                direccion = (string)cnstGenrl.Tables[0].Rows[0]["direccion"];
            }
            else
            {
                bandera = false;
                mensaje = "Verificar la consulta no presenta datos ";
                salida = new RespuestaDTO { codigo = "-1", mensaje = mensaje, };
            }
            //obtener token
            if (bandera)
            {
                var client = new RestClient(rutaToken + "/oauth2/token");
                var request = new RestRequest("", Method.Post);
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddParameter("username", usuario);
                request.AddParameter("password", clave);
                request.AddParameter("grant_type", "password");
                request.AddParameter("client_id", clienteId);
                RestResponse response = client.Execute(request);
                valor = response.StatusCode.ToString();
                if (valor.Equals("OK"))
                {
                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(response.Content);
                    accesstoken = funciones.descomponer("access_token", jsonString);
                    tokentype = funciones.descomponer("token_type", jsonString);
                }
                else
                {
                    bandera = false;
                    respuesta = response.Content.ToString();
                    mensaje = "No se genero el token ";
                    salida = new RespuestaDTO { codigo = "-1", mensaje = mensaje, };
                }
            }
            //obtener codigo de plataforma
            if (bandera)
            {
                var plataformaSearch = new RestClient();
                var requesteplataformaSearch = new RestRequest();
                RestResponse responseplataformaSearch;
                plataformaSearch = new RestClient(rutaGeneral + "/api/CatalogoAplicacion/ObtenerItem");
                requesteplataformaSearch = new RestRequest("", Method.Get);
                requesteplataformaSearch.AddHeader("content-type", "application/json");
                requesteplataformaSearch.AddHeader("Authorization", tokentype + " " + accesstoken);
                requesteplataformaSearch.AddHeader("CodigoAplicacion", "0");
                requesteplataformaSearch.AddHeader("CodigoPlataforma", "0");
                requesteplataformaSearch.AddHeader("SistemaOperativo", operativo);
                requesteplataformaSearch.AddHeader("DispositivoNavegador", navegador);
                requesteplataformaSearch.AddHeader("DireccionIP", direccion);
                requesteplataformaSearch.AddParameter("codigoItem", plataforma);
                responseplataformaSearch = plataformaSearch.Execute(requesteplataformaSearch);
                valor = responseplataformaSearch.StatusCode.ToString();
                if (valor.Equals("OK"))
                {
                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(responseplataformaSearch.Content);
                    codigoplataforma = funciones.descomponer("IdItem", jsonString);
                }
                else
                {
                    bandera = false;
                    //respuesta = responseplataformaSearch.ErrorMessage.ToString();
                    respuesta = responseplataformaSearch.Content.ToString();
                    mensaje = "No trae el dato del codigo de Plataforma";
                    salida = new RespuestaDTO { codigo = "-1", mensaje = mensaje, };
                }
            }
            //obtener codigo de aplicativo
            if (bandera)
            {
                var aplicacionSearch = new RestClient();
                var requesteaplicacionSearch = new RestRequest();
                RestResponse responseaplicacionSearch;
                aplicacionSearch = new RestClient(rutaGeneral + "/api/CatalogoAplicacion/ObtenerItem");
                requesteaplicacionSearch = new RestRequest("", Method.Get);
                requesteaplicacionSearch.AddHeader("content-type", "application/json");
                requesteaplicacionSearch.AddHeader("Authorization", tokentype + " " + accesstoken);
                requesteaplicacionSearch.AddHeader("CodigoAplicacion", "0");
                requesteaplicacionSearch.AddHeader("CodigoPlataforma", "0");
                requesteaplicacionSearch.AddHeader("SistemaOperativo", operativo);
                requesteaplicacionSearch.AddHeader("DispositivoNavegador", navegador);
                requesteaplicacionSearch.AddHeader("DireccionIP", direccion);
                requesteaplicacionSearch.AddParameter("codigoItem", aplicativo);
                responseaplicacionSearch = aplicacionSearch.Execute(requesteaplicacionSearch);
                valor = responseaplicacionSearch.StatusCode.ToString();
                if (valor.Equals("OK"))
                {
                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(responseaplicacionSearch.Content);
                    codigoaplicativo = funciones.descomponer("IdItem", jsonString);
                }
                else
                {
                    bandera = false;
                    //respuesta = responseaplicacionSearch.ErrorMessage.ToString();
                    respuesta = responseaplicacionSearch.Content.ToString();
                    mensaje = "No trae el dato del codigo de Aplicativo";
                    salida = new RespuestaDTO { codigo = "-1", mensaje = mensaje, };
                }

            }
            //VERIFICACION DE SEGURIDAD contrato
            if (bandera)
            {
                var obtenerContratoSearch = new RestClient();
                var requesteobtenerContratoSearch = new RestRequest();
                RestResponse responseobtenerContratoSearch;
                obtenerContratoSearch = new RestClient(rutaCrearContrato + "/api/Sponsor/ObtenerContratoPorDocumento");
                requesteobtenerContratoSearch = new RestRequest("", Method.Post);
                requesteobtenerContratoSearch.AddHeader("content-type", "application/json");
                requesteobtenerContratoSearch.AddHeader("Authorization", tokentype + " " + accesstoken);
                requesteobtenerContratoSearch.AddHeader("CodigoAplicacion", codigoaplicativo);
                requesteobtenerContratoSearch.AddHeader("CodigoPlataforma", codigoplataforma);
                requesteobtenerContratoSearch.AddHeader("SistemaOperativo", operativo);
                requesteobtenerContratoSearch.AddHeader("DispositivoNavegador", navegador);
                requesteobtenerContratoSearch.AddHeader("DireccionIP", direccion);
                texto = Utilidades.SerializarContrato(p.cliente, "C");
                requesteobtenerContratoSearch.AddParameter("application/json", texto, ParameterType.RequestBody);
                responseobtenerContratoSearch = obtenerContratoSearch.Execute(requesteobtenerContratoSearch);
                valor = responseobtenerContratoSearch.StatusCode.ToString();
                if (valor.Equals("OK"))
                {
                    string codigo = "";
                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(responseobtenerContratoSearch.Content);
                    codigo = funciones.descomponer("Codigo", jsonString);

                    if (codigo.Equals("005"))
                    {
                        generarContrato = false;
                        respuesta = responseobtenerContratoSearch.Content.ToString();
                        contratoNumero = funciones.descomponer("Numero", jsonString);
                        region = funciones.descomponer("Region", jsonString);
                        producto = funciones.descomponer("Producto", jsonString);
                        mensaje = "Guardado Exitosamente, Contrato Existente";
                    }
                    else if (codigo.Equals("006"))
                    {
                        generarContrato = true;
                    }
                    else if (codigo.Equals("001") || codigo.Equals("002") || codigo.Equals("003"))
                    {
                        generarContrato = false;
                        bandera = false;
                        mensaje = funciones.descomponer("Mensaje", jsonString);
                        salida = new RespuestaDTO { codigo = "-1", mensaje = mensaje, };
                    }
                    else
                    {
                        generarContrato = false;
                        bandera = false;
                        mensaje = "Error en la consulta de contrato";
                        salida = new RespuestaDTO { codigo = "-1", mensaje = mensaje, };
                    }
                }
                else
                {
                    bandera = false;
                    respuesta = responseobtenerContratoSearch.ErrorMessage.ToString();
                    mensaje = "No trae el dato del contrato";
                    salida = new RespuestaDTO { codigo = "-1", mensaje = mensaje, };
                }
            }
            //generar el contrato
            if (generarContrato)
            {
                if (bandera)
                {
                    //informacion del los beneficiarios
                    beneficiarioEntidadCollection beneficiarioEntidadCollection = new beneficiarioEntidadCollection();
                    beneficiarioEntidad beneficiarioCollection = new beneficiarioEntidad();
                    infoDatos = ConvenioSQL.CnstParametrosMed("102", p.cliente, "0", p.tipoplan);
                    if (infoDatos.Tables.Count > 0)
                    {
                        for (int i = 0; i <= infoDatos.Tables[0].Rows.Count - 1; i++)
                        {
                            beneficiarioEntidad attribute = new beneficiarioEntidad();
                            attribute.Identificacion = (string)infoDatos.Tables[0].Rows[i]["cedula"];
                            attribute.Mail = (string)infoDatos.Tables[0].Rows[i]["email"];
                            attribute.Celular = (string)infoDatos.Tables[0].Rows[i]["celular"];
                            attribute.Relacion = (int)infoDatos.Tables[0].Rows[i]["relacion"];
                            beneficiarioEntidadCollection.Add(attribute);
                        }
                        beneficiarioCollection.beneficiarioEntidadCollection = beneficiarioEntidadCollection;
                    }
                    //informacion del cliente
                    infoCliente = ConvenioSQL.CnstParametrosMed("101", p.cliente, "0", p.tipoplan);
                    if (infoCliente.Tables.Count > 0)
                    {
                        texto = Utilidades.SerializarInstalacion(p.sponsor, p.tipoplan
                                                                , (string)infoCliente.Tables[0].Rows[0]["CLIENTE"]
                                                                , (string)infoCliente.Tables[0].Rows[0]["TELEFONO"]
                                                                , (string)infoCliente.Tables[0].Rows[0]["EMAIL"]
                                                                , p.fechainicio, p.fechafin, beneficiarioEntidadCollection);
                        var contratoSearch = new RestClient();
                        var requesteContratoSearch = new RestRequest();
                        RestResponse responseContratoSearch;
                        contratoSearch = new RestClient(rutaCrearContrato + "/api/Sponsor/CrearContrato");
                        requesteContratoSearch = new RestRequest("", Method.Post);
                        requesteContratoSearch.AddHeader("content-type", "application/json");
                        requesteContratoSearch.AddHeader("Authorization", tokentype + " " + accesstoken);
                        requesteContratoSearch.AddHeader("CodigoAplicacion", codigoaplicativo);
                        requesteContratoSearch.AddHeader("CodigoPlataforma", codigoplataforma);
                        requesteContratoSearch.AddHeader("SistemaOperativo", operativo);
                        requesteContratoSearch.AddHeader("DispositivoNavegador", navegador);
                        requesteContratoSearch.AddHeader("DireccionIP", direccion);
                        requesteContratoSearch.AddParameter("application/json", texto, ParameterType.RequestBody);
                        responseContratoSearch = contratoSearch.Execute(requesteContratoSearch);
                        //guarda datos en el log
                        ConvenioSQL.Registrar_Med(p.ordenservicio, p.cliente, "0", "", "", "", "", p.accion, responseContratoSearch.Content.ToString(), "", "", "108", texto);
                        valor = responseContratoSearch.StatusCode.ToString();
                        if (valor.Equals("OK"))
                        {
                            respuesta = responseContratoSearch.Content.ToString();
                            contratoNumero = funciones.descomponer("ContratoNumero", responseContratoSearch.Content);
                            region = funciones.descomponer("Region", responseContratoSearch.Content);
                            producto = funciones.descomponer("CodigoProducto", responseContratoSearch.Content);
                            mensaje = "Guardado Exitosamente";
                            if (contratoNumero.Equals(""))
                            {
                                bandera = false;
                                mensaje = "Error no presenta contrato";
                                salida = new RespuestaDTO { codigo = "-1", mensaje = mensaje, };
                            }
                        }
                        else
                        {
                            respuesta = responseContratoSearch.Content.ToString();
                            bandera = false;
                            mensaje = "No se guardo el contrato";
                            salida = new RespuestaDTO { codigo = "-1", mensaje = mensaje, };
                        }
                    }
                }
            }
            //ACTUALIZAR DATOS EJECUCION CORRECTA
            if (bandera)
            {
                resultado = ConvenioSQL.Registrar_Med(p.ordenservicio, p.cliente, "4", mensaje, contratoNumero, codigoplataforma, codigoaplicativo, p.accion, respuesta, region, producto, "104", texto);
                if (resultado.Equals("OK"))
                {
                    // bandera = true;
                    String respuestaMail = ConvenioSQL.GenerarMail(p.ordenservicio, p.cliente, producto);
                    if (respuestaMail.Equals("OK"))
                    {
                        //Enviar Mail de confirmacion
                        bandera = true;
                    }
                    else
                    {
                        bandera = false;
                    }
                }
                else
                {
                    mensaje = "Error en Actualizacion de datos";
                    bandera = false;
                    salida = new RespuestaDTO { codigo = "-1", mensaje = mensaje, };
                }
            }
            if (bandera)
            {
                salida = new RespuestaDTO { codigo = "1", mensaje = mensaje, };
            }
            else
            {
                ConvenioSQL.Registrar_Med(p.ordenservicio, p.cliente, "3", mensaje, "0", "0", "0", p.accion, respuesta, "", "", "104", texto);
            }
            return salida;
        }


        [HttpPost]
        [Authorize]
        [Route("/HunterMed/RenovarContrato")]
        public ActionResult<RespuestaDTO> RenovarContrato([FromBody] RenovacionDTO p)
        {
            RespuestaDTO salida = null;
            string mensaje = "";
            string clienteId = "";
            string usuario = "";
            string clave = "";
            string aplicativo = "";
            string plataforma = "";
            string operativo = "";
            string navegador = "";
            string direccion = "";
            string accesstoken = "";
            string tokentype = "";
            string codigoaplicativo = "";
            string codigoplataforma = "";
            string valor = "";
            string respuesta = "";
            string texto = "";
            string CodigoEstado = "";
            conexionConvenio ruta = new conexionConvenio();
            string rutaToken = ruta.GetRutaHunterMed("1");
            string rutRenoContrato = ruta.GetRutaHunterMed("2");
            string rutaGeneral = ruta.GetRutaHunterMed("3");
            bool bandera = true;
            string resultado = "";
            DataSet infoDatos = new DataSet();
            DataSet cnstGenrl = new DataSet();
            //parametros
            cnstGenrl = ConvenioSQL.CnstParametrosMed("100", p.cliente, "0", p.tipoplan);
            if (cnstGenrl.Tables.Count > 0)
            {
                clienteId = (string)cnstGenrl.Tables[0].Rows[0]["clienteId"];
                usuario = (string)cnstGenrl.Tables[0].Rows[0]["usuario"];
                clave = (string)cnstGenrl.Tables[0].Rows[0]["clave"];
                aplicativo = (string)cnstGenrl.Tables[0].Rows[0]["aplicativo"];
                plataforma = (string)cnstGenrl.Tables[0].Rows[0]["plataforma"];
                operativo = (string)cnstGenrl.Tables[0].Rows[0]["operativo"];
                navegador = (string)cnstGenrl.Tables[0].Rows[0]["navegador"];
                direccion = (string)cnstGenrl.Tables[0].Rows[0]["direccion"];
            }
            else
            {
                bandera = false;
                mensaje = "Verificar la consulta no presenta datos ";
            }
            //validar numero de contrato
            if (bandera)
            {
                if (p.contratoNumero.Equals("") || string.IsNullOrEmpty(p.contratoNumero.ToString()))
                {
                    bandera = false;
                    mensaje = "Error no presenta contrato ";
                }
            }
            //obtener token
            if (bandera)
            {
                var client = new RestClient(rutaToken + "/oauth2/token");
                var request = new RestRequest("", Method.Post);
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddParameter("username", usuario);
                request.AddParameter("password", clave);
                request.AddParameter("grant_type", "password");
                request.AddParameter("client_id", clienteId);
                RestResponse response = client.Execute(request);
                valor = response.StatusCode.ToString();
                if (valor.Equals("OK"))
                {
                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(response.Content);
                    accesstoken = funciones.descomponer("access_token", jsonString);
                    tokentype = funciones.descomponer("token_type", jsonString);
                }
                else
                {
                    bandera = false;
                    //respuesta = response.ErrorMessage.ToString();
                    respuesta = response.Content.ToString();
                    mensaje = "No se genero el token ";
                }
            }
            //obtener codigo de plataforma
            if (bandera)
            {
                var plataformaSearch = new RestClient();
                var requesteplataformaSearch = new RestRequest();
                RestResponse responseplataformaSearch;
                plataformaSearch = new RestClient(rutaGeneral + "/api/CatalogoAplicacion/ObtenerItem");
                requesteplataformaSearch = new RestRequest("", Method.Get);
                requesteplataformaSearch.AddHeader("content-type", "application/json");
                requesteplataformaSearch.AddHeader("Authorization", tokentype + " " + accesstoken);
                requesteplataformaSearch.AddHeader("CodigoAplicacion", "0");
                requesteplataformaSearch.AddHeader("CodigoPlataforma", "0");
                requesteplataformaSearch.AddHeader("SistemaOperativo", operativo);
                requesteplataformaSearch.AddHeader("DispositivoNavegador", navegador);
                requesteplataformaSearch.AddHeader("DireccionIP", direccion);
                requesteplataformaSearch.AddParameter("codigoItem", plataforma);
                responseplataformaSearch = plataformaSearch.Execute(requesteplataformaSearch);
                valor = responseplataformaSearch.StatusCode.ToString();
                if (valor.Equals("OK"))
                {
                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(responseplataformaSearch.Content);
                    codigoplataforma = funciones.descomponer("IdItem", jsonString);
                }
                else
                {
                    bandera = false;
                    respuesta = responseplataformaSearch.ErrorMessage.ToString();
                    mensaje = "No trae el dato del codigo de Plataforma ";
                }
            }
            //obtener codigo de aplicativo
            if (bandera)
            {
                var aplicacionSearch = new RestClient();
                var requesteaplicacionSearch = new RestRequest();
                RestResponse responseaplicacionSearch;
                aplicacionSearch = new RestClient(rutaGeneral + "/api/CatalogoAplicacion/ObtenerItem");
                requesteaplicacionSearch = new RestRequest("", Method.Get);
                requesteaplicacionSearch.AddHeader("content-type", "application/json");
                requesteaplicacionSearch.AddHeader("Authorization", tokentype + " " + accesstoken);
                requesteaplicacionSearch.AddHeader("CodigoAplicacion", "0");
                requesteaplicacionSearch.AddHeader("CodigoPlataforma", "0");
                requesteaplicacionSearch.AddHeader("SistemaOperativo", operativo);
                requesteaplicacionSearch.AddHeader("DispositivoNavegador", navegador);
                requesteaplicacionSearch.AddHeader("DireccionIP", direccion);
                requesteaplicacionSearch.AddParameter("codigoItem", aplicativo);
                responseaplicacionSearch = aplicacionSearch.Execute(requesteaplicacionSearch);
                valor = responseaplicacionSearch.StatusCode.ToString();
                if (valor.Equals("OK"))
                {
                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(responseaplicacionSearch.Content);
                    codigoaplicativo = funciones.descomponer("IdItem", jsonString);
                }
                else
                {
                    bandera = false;
                    //respuesta = responseaplicacionSearch.ErrorMessage.ToString();
                    respuesta = responseaplicacionSearch.Content.ToString();
                    mensaje = "No trae el dato del codigo de Aplicativo ";
                    salida = new RespuestaDTO { codigo = "-1", mensaje = mensaje, };
                }

            }
            //Proceso renovacion
            if (bandera)
            {
                texto = Utilidades.SerializarRenovacion(p.contratoNumero, p.region, p.producto, p.fechainicio, p.fechafin, p.sponsor);

                var contratoRenovaSearch = new RestClient();
                var requesteContratoRenovaSearch = new RestRequest();
                RestResponse responseContratoRenovaSearch;
                contratoRenovaSearch = new RestClient(rutRenoContrato + "/api/Sponsor/RenovacionContrato");
                requesteContratoRenovaSearch = new RestRequest("", Method.Post);
                requesteContratoRenovaSearch.AddHeader("content-type", "application/json");
                requesteContratoRenovaSearch.AddHeader("Authorization", tokentype + " " + accesstoken);
                requesteContratoRenovaSearch.AddHeader("CodigoAplicacion", codigoaplicativo);
                requesteContratoRenovaSearch.AddHeader("CodigoPlataforma", codigoplataforma);
                requesteContratoRenovaSearch.AddHeader("SistemaOperativo", operativo);
                requesteContratoRenovaSearch.AddHeader("DispositivoNavegador", navegador);
                requesteContratoRenovaSearch.AddHeader("DireccionIP", direccion);
                requesteContratoRenovaSearch.AddParameter("application/json", texto, ParameterType.RequestBody);
                responseContratoRenovaSearch = contratoRenovaSearch.Execute(requesteContratoRenovaSearch);
                //guarda datos en el log
                ConvenioSQL.Registrar_Med(p.ordenservicio, p.cliente, "0", "", "", "", "", p.accion, responseContratoRenovaSearch.Content.ToString(), "", "", "108", texto);
                valor = responseContratoRenovaSearch.StatusCode.ToString();
                if (valor.Equals("OK"))
                {
                    respuesta = responseContratoRenovaSearch.Content.ToString();
                    CodigoEstado = funciones.descomponer("CodigoEstado", responseContratoRenovaSearch.Content);
                    if (CodigoEstado.Equals("01"))
                    {
                        mensaje = "Guardado Exitosamente";
                    }
                    else
                    {
                        mensaje = funciones.descomponer("Mensaje", responseContratoRenovaSearch.Content);
                        bandera = false;
                    }
                }
                else
                {
                    mensaje = "No se guardo la renovación ";
                    bandera = false;
                }
            }
            //ACTUALIZAR DATOS EJECUCION CORRECTA
            if (bandera)
            {
                resultado = ConvenioSQL.Registrar_Med(p.ordenservicio, p.cliente, "4", mensaje, p.contratoNumero, codigoplataforma, codigoaplicativo, p.accion, respuesta, p.region, p.producto, "104", texto);
                if (resultado.Equals("OK"))
                {
                    bandera = true;
                }
                else
                {
                    mensaje = "Error en Actualizacion de datos ";
                    bandera = false;
                }
            }
            //mensaje de salida
            if (bandera)
            {
                salida = new RespuestaDTO { codigo = "1", mensaje = mensaje, };
            }
            else
            {
                ConvenioSQL.Registrar_Med(p.ordenservicio, p.cliente, "3", mensaje, "0", "0", "0", p.accion, respuesta, "", "", "104", texto);
                salida = new RespuestaDTO { codigo = "-1", mensaje = mensaje, };
            }
            return salida;
        }


        [HttpPost]
        [Authorize]
        [Route("/HunterMed/IncluirBeneficiario")]
        public ActionResult<RespuestaDTO> IncluirBeneficiario([FromBody] BeneficiarioDTO p)
        {
            RespuestaDTO salida = null;
            conexionConvenio ruta = new conexionConvenio();
            string rutaToken = ruta.GetRutaHunterMed("1");
            string rutaBeneficiario = ruta.GetRutaHunterMed("2");
            string rutaGeneral = ruta.GetRutaHunterMed("3");
            bool bandera = true;
            string accesstoken = "";
            string tokentype = "";
            string clienteId = "";
            string usuario = "";
            string clave = "";
            string aplicativo = "";
            string plataforma = "";
            string operativo = "";
            string navegador = "";
            string direccion = "";
            string mensaje = "";
            string respuesta = "";
            string codigoplataforma = "";
            string codigoaplicativo = "";
            string resultado = "";
            string texto = "";
            string valor = "";
            DataSet cnstGenrl = new DataSet();
            DataSet infoDatos = new DataSet();
            //parametros
            cnstGenrl = ConvenioSQL.CnstParametrosMed("100", p.cliente, "0", p.tipoplan);
            if (cnstGenrl.Tables.Count > 0)
            {
                clienteId = (string)cnstGenrl.Tables[0].Rows[0]["clienteId"];
                usuario = (string)cnstGenrl.Tables[0].Rows[0]["usuario"];
                clave = (string)cnstGenrl.Tables[0].Rows[0]["clave"];
                aplicativo = (string)cnstGenrl.Tables[0].Rows[0]["aplicativo"];
                plataforma = (string)cnstGenrl.Tables[0].Rows[0]["plataforma"];
                operativo = (string)cnstGenrl.Tables[0].Rows[0]["operativo"];
                navegador = (string)cnstGenrl.Tables[0].Rows[0]["navegador"];
                direccion = (string)cnstGenrl.Tables[0].Rows[0]["direccion"];
            }
            else
            {
                bandera = false;
                mensaje = "Verificar la consulta no presenta datos ";
            }
            //obtener token
            if (bandera)
            {
                var client = new RestClient(rutaToken + "/oauth2/token");
                var request = new RestRequest("", Method.Post);
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddParameter("username", usuario);
                request.AddParameter("password", clave);
                request.AddParameter("grant_type", "password");
                request.AddParameter("client_id", clienteId);
                RestResponse response = client.Execute(request);
                valor = response.StatusCode.ToString();
                if (valor.Equals("OK"))
                {
                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(response.Content);
                    accesstoken = funciones.descomponer("access_token", jsonString);
                    tokentype = funciones.descomponer("token_type", jsonString);
                }
                else
                {
                    bandera = false;
                    //respuesta = response.ErrorMessage.ToString();
                    respuesta = response.Content.ToString();
                    mensaje = "No se genero el token ";
                }
            }
            //obtener codigo de plataforma
            if (bandera)
            {
                var plataformaSearch = new RestClient();
                var requesteplataformaSearch = new RestRequest();
                RestResponse responseplataformaSearch;
                plataformaSearch = new RestClient(rutaGeneral + "/api/CatalogoAplicacion/ObtenerItem");
                requesteplataformaSearch = new RestRequest("", Method.Get);
                requesteplataformaSearch.AddHeader("content-type", "application/json");
                requesteplataformaSearch.AddHeader("Authorization", tokentype + " " + accesstoken);
                requesteplataformaSearch.AddHeader("CodigoAplicacion", "0");
                requesteplataformaSearch.AddHeader("CodigoPlataforma", "0");
                requesteplataformaSearch.AddHeader("SistemaOperativo", operativo);
                requesteplataformaSearch.AddHeader("DispositivoNavegador", navegador);
                requesteplataformaSearch.AddHeader("DireccionIP", direccion);
                requesteplataformaSearch.AddParameter("codigoItem", plataforma);
                responseplataformaSearch = plataformaSearch.Execute(requesteplataformaSearch);
                valor = responseplataformaSearch.StatusCode.ToString();
                if (valor.Equals("OK"))
                {
                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(responseplataformaSearch.Content);
                    codigoplataforma = funciones.descomponer("IdItem", jsonString);
                }
                else
                {
                    bandera = false;
                    respuesta = responseplataformaSearch.ErrorMessage.ToString();
                    mensaje = "No trae el dato del codigo de Plataforma ";
                }
            }
            //obtener codigo de aplicativo
            if (bandera)
            {
                var aplicacionSearch = new RestClient();
                var requesteaplicacionSearch = new RestRequest();
                RestResponse responseaplicacionSearch;
                aplicacionSearch = new RestClient(rutaGeneral + "/api/CatalogoAplicacion/ObtenerItem");
                requesteaplicacionSearch = new RestRequest("", Method.Get);
                requesteaplicacionSearch.AddHeader("content-type", "application/json");
                requesteaplicacionSearch.AddHeader("Authorization", tokentype + " " + accesstoken);
                requesteaplicacionSearch.AddHeader("CodigoAplicacion", "0");
                requesteaplicacionSearch.AddHeader("CodigoPlataforma", "0");
                requesteaplicacionSearch.AddHeader("SistemaOperativo", operativo);
                requesteaplicacionSearch.AddHeader("DispositivoNavegador", navegador);
                requesteaplicacionSearch.AddHeader("DireccionIP", direccion);
                requesteaplicacionSearch.AddParameter("codigoItem", aplicativo);
                responseaplicacionSearch = aplicacionSearch.Execute(requesteaplicacionSearch);
                valor = responseaplicacionSearch.StatusCode.ToString();
                if (valor.Equals("OK"))
                {
                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(responseaplicacionSearch.Content);
                    codigoaplicativo = funciones.descomponer("IdItem", jsonString);
                }
                else
                {
                    bandera = false;
                    //respuesta = responseaplicacionSearch.ErrorMessage.ToString();
                    respuesta = responseaplicacionSearch.Content.ToString();
                    mensaje = "No trae el dato del codigo de Aplicativo ";
                }
            }
            //beneficiario
            if (bandera)
            {
                infoDatos = ConvenioSQL.CnstParametrosMed("103", p.cliente, p.beneficiario, p.tipoplan);
                if (infoDatos.Tables.Count > 0)
                {
                    texto = Utilidades.SerializarBeneficiario(p.cliente
                                , (string)infoDatos.Tables[0].Rows[0]["email"]
                                , (string)infoDatos.Tables[0].Rows[0]["celular"]
                                , (string)infoDatos.Tables[0].Rows[0]["relacion"]
                                , p.sponsor, p.tipoplan, p.contratoNumero, p.region, p.producto);
                    var beneficiarioSearch = new RestClient();
                    var requesteBeneficiarioSearch = new RestRequest();
                    RestResponse responseBeneficiarioSearch;
                    beneficiarioSearch = new RestClient(rutaBeneficiario + "/api/Sponsor/IncluirBeneficiario");
                    requesteBeneficiarioSearch = new RestRequest("", Method.Post);
                    requesteBeneficiarioSearch.AddHeader("content-type", "application/json");
                    requesteBeneficiarioSearch.AddHeader("Authorization", tokentype + " " + accesstoken);
                    requesteBeneficiarioSearch.AddHeader("CodigoAplicacion", codigoaplicativo);
                    requesteBeneficiarioSearch.AddHeader("CodigoPlataforma", codigoplataforma);
                    requesteBeneficiarioSearch.AddHeader("SistemaOperativo", operativo);
                    requesteBeneficiarioSearch.AddHeader("DispositivoNavegador", navegador);
                    requesteBeneficiarioSearch.AddHeader("DireccionIP", direccion);
                    requesteBeneficiarioSearch.AddParameter("application/json", texto, ParameterType.RequestBody);
                    responseBeneficiarioSearch = beneficiarioSearch.Execute(requesteBeneficiarioSearch);
                    valor = responseBeneficiarioSearch.StatusCode.ToString();
                    if (valor.Equals("OK"))
                    {
                        respuesta = responseBeneficiarioSearch.Content.ToString();
                        mensaje = "Guardado Exitosamente";
                    }
                    else
                    {
                        bandera = false;
                        respuesta = responseBeneficiarioSearch.Content.ToString();
                        mensaje = "No se adiciono el Beneficiario ";
                    }
                }
                else
                {
                    mensaje = "No existen datos  del Beneficiario ";
                    bandera = false;
                }
            }
            //ACTUALIZAR DATOS EJECUCION CORRECTA
            if (bandera)
            {
                resultado = ConvenioSQL.Registrar_Med("", p.cliente, "4", mensaje, p.contratoNumero, codigoplataforma, codigoaplicativo, p.accion, respuesta, p.region, p.producto, "104", texto);
                if (resultado.Equals("OK"))
                {
                    bandera = true;
                }
                else
                {
                    mensaje = "Error en Actualizacion de datos ";
                    bandera = false;
                }
            }
            //mensaje
            if (bandera)
            {
                salida = new RespuestaDTO { codigo = "1", mensaje = mensaje, };
            }
            else
            {
                salida = new RespuestaDTO { codigo = "-1", mensaje = mensaje, };
                ConvenioSQL.Registrar_Med("", p.cliente, "3", mensaje, "0", "0", "0", p.accion, respuesta, "", "", "104", texto);
            }
            return salida;
        }


        [HttpPost]
        [Authorize]
        [Route("/HunterMed/AnularContrato")]
        public ActionResult<RespuestaDTO> AnularContrato([FromBody] AnularContratoDTO p)
        {
            RespuestaDTO salida = null;
            conexionConvenio ruta = new conexionConvenio();
            string rutaToken = ruta.GetRutaHunterMed("1");
            string rutaAnularContrato = ruta.GetRutaHunterMed("2");
            string rutaGeneral = ruta.GetRutaHunterMed("3");
            bool bandera = true;
            string mensaje = "";
            string clienteId = "";
            string usuario = "";
            string clave = "";
            string aplicativo = "";
            string plataforma = "";
            string operativo = "";
            string navegador = "";
            string direccion = "";
            string valor = "";
            string texto = "";
            string accesstoken = "";
            string tokentype = "";
            string codigoaplicativo = "";
            string codigoplataforma = "";
            string respuesta = "";
            string resultado = "";
            string codigoEstado = "";
            DataSet cnstGenrl = new DataSet();
            DataSet infoDatos = new DataSet();
            //parametros
            cnstGenrl = ConvenioSQL.CnstParametrosMed("100", p.cliente, "0", p.tipoplan);
            if (cnstGenrl.Tables.Count > 0)
            {
                clienteId = (string)cnstGenrl.Tables[0].Rows[0]["clienteId"];
                usuario = (string)cnstGenrl.Tables[0].Rows[0]["usuario"];
                clave = (string)cnstGenrl.Tables[0].Rows[0]["clave"];
                aplicativo = (string)cnstGenrl.Tables[0].Rows[0]["aplicativo"];
                plataforma = (string)cnstGenrl.Tables[0].Rows[0]["plataforma"];
                operativo = (string)cnstGenrl.Tables[0].Rows[0]["operativo"];
                navegador = (string)cnstGenrl.Tables[0].Rows[0]["navegador"];
                direccion = (string)cnstGenrl.Tables[0].Rows[0]["direccion"];
            }
            else
            {
                bandera = false;
                mensaje = "Verificar la consulta no presenta datos ";
            }
            //obtener token
            if (bandera)
            {
                var client = new RestClient(rutaToken + "/oauth2/token");
                var request = new RestRequest("", Method.Post);
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddParameter("username", usuario);
                request.AddParameter("password", clave);
                request.AddParameter("grant_type", "password");
                request.AddParameter("client_id", clienteId);
                RestResponse response = client.Execute(request);
                valor = response.StatusCode.ToString();
                if (valor.Equals("OK"))
                {
                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(response.Content);
                    accesstoken = funciones.descomponer("access_token", jsonString);
                    tokentype = funciones.descomponer("token_type", jsonString);
                }
                else
                {
                    bandera = false;
                    //respuesta = response.ErrorMessage.ToString();
                    respuesta = response.Content.ToString();
                    mensaje = "No se genero el token ";
                }
            }
            //obtener codigo de plataforma
            if (bandera)
            {
                var plataformaSearch = new RestClient();
                var requesteplataformaSearch = new RestRequest();
                RestResponse responseplataformaSearch;
                plataformaSearch = new RestClient(rutaGeneral + "/api/CatalogoAplicacion/ObtenerItem");
                requesteplataformaSearch = new RestRequest("", Method.Get);
                requesteplataformaSearch.AddHeader("content-type", "application/json");
                requesteplataformaSearch.AddHeader("Authorization", tokentype + " " + accesstoken);
                requesteplataformaSearch.AddHeader("CodigoAplicacion", "0");
                requesteplataformaSearch.AddHeader("CodigoPlataforma", "0");
                requesteplataformaSearch.AddHeader("SistemaOperativo", operativo);
                requesteplataformaSearch.AddHeader("DispositivoNavegador", navegador);
                requesteplataformaSearch.AddHeader("DireccionIP", direccion);
                requesteplataformaSearch.AddParameter("codigoItem", plataforma);
                responseplataformaSearch = plataformaSearch.Execute(requesteplataformaSearch);
                valor = responseplataformaSearch.StatusCode.ToString();
                if (valor.Equals("OK"))
                {
                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(responseplataformaSearch.Content);
                    codigoplataforma = funciones.descomponer("IdItem", jsonString);
                }
                else
                {
                    bandera = false;
                    //respuesta = responseplataformaSearch.ErrorMessage.ToString();
                    respuesta = responseplataformaSearch.Content.ToString();
                    mensaje = "No trae el dato del codigo de Plataforma ";
                }
            }
            //obtener codigo de aplicativo
            if (bandera)
            {
                var aplicacionSearch = new RestClient();
                var requesteaplicacionSearch = new RestRequest();
                RestResponse responseaplicacionSearch;
                aplicacionSearch = new RestClient(rutaGeneral + "/api/CatalogoAplicacion/ObtenerItem");
                requesteaplicacionSearch = new RestRequest("", Method.Get);
                requesteaplicacionSearch.AddHeader("content-type", "application/json");
                requesteaplicacionSearch.AddHeader("Authorization", tokentype + " " + accesstoken);
                requesteaplicacionSearch.AddHeader("CodigoAplicacion", "0");
                requesteaplicacionSearch.AddHeader("CodigoPlataforma", "0");
                requesteaplicacionSearch.AddHeader("SistemaOperativo", operativo);
                requesteaplicacionSearch.AddHeader("DispositivoNavegador", navegador);
                requesteaplicacionSearch.AddHeader("DireccionIP", direccion);
                requesteaplicacionSearch.AddParameter("codigoItem", aplicativo);
                responseaplicacionSearch = aplicacionSearch.Execute(requesteaplicacionSearch);
                valor = responseaplicacionSearch.StatusCode.ToString();
                if (valor.Equals("OK"))
                {
                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(responseaplicacionSearch.Content);
                    codigoaplicativo = funciones.descomponer("IdItem", jsonString);
                }
                else
                {
                    bandera = false;
                    //respuesta = responseaplicacionSearch.ErrorMessage.ToString();
                    respuesta = responseaplicacionSearch.Content.ToString();
                    mensaje = "No trae el dato del codigo de Aplicativo ";
                }

            }
            //Anular contrato
            if (bandera)
            {
                texto = Utilidades.SerializarAnulacion(p.contratoNumero, p.region, p.producto, p.sponsor, p.tipoplan);
                var anulacionSearch = new RestClient();
                var requesteAnulacionSearch = new RestRequest();
                RestResponse responseAnulacionSearch;
                anulacionSearch = new RestClient(rutaAnularContrato + "/api/Sponsor/AnularContrato");
                requesteAnulacionSearch = new RestRequest("", Method.Post);
                requesteAnulacionSearch.AddHeader("content-type", "application/json");
                requesteAnulacionSearch.AddHeader("Authorization", tokentype + " " + accesstoken);
                requesteAnulacionSearch.AddHeader("CodigoAplicacion", codigoaplicativo);
                requesteAnulacionSearch.AddHeader("CodigoPlataforma", codigoplataforma);
                requesteAnulacionSearch.AddHeader("SistemaOperativo", operativo);
                requesteAnulacionSearch.AddHeader("DispositivoNavegador", navegador);
                requesteAnulacionSearch.AddHeader("DireccionIP", direccion);
                requesteAnulacionSearch.AddParameter("application/json", texto, ParameterType.RequestBody);
                responseAnulacionSearch = anulacionSearch.Execute(requesteAnulacionSearch);
                ConvenioSQL.Registrar_Med(p.ordenservicio, p.cliente, "0", "", "", "", "", p.accion, responseAnulacionSearch.Content.ToString(), "", "", "108", texto);
                valor = responseAnulacionSearch.StatusCode.ToString();
                if (valor.Equals("OK"))
                {
                    respuesta = responseAnulacionSearch.Content.ToString();
                    codigoEstado = funciones.descomponer("CodigoEstado", responseAnulacionSearch.Content);
                    if (codigoEstado.Equals("01"))
                    {
                        mensaje = "Guardado Exitosamente";
                    }
                    else
                    {
                        mensaje = funciones.descomponer("Mensaje", responseAnulacionSearch.Content);
                        bandera = false;
                    }
                }
                else
                {
                    bandera = false;
                    respuesta = responseAnulacionSearch.Content.ToString();
                    mensaje = "No se pudo Anular el Contrato ";
                }
            }
            //ACTUALIZAR DATOS EJECUCION CORRECTA
            if (bandera)
            {
                resultado = ConvenioSQL.Registrar_Med(p.ordenservicio, p.cliente, "4", mensaje, p.contratoNumero, codigoplataforma, codigoaplicativo, p.accion, respuesta, p.region, p.producto, "104", texto);
                if (resultado.Equals("OK"))
                {
                    bandera = true;
                    // proceso de anulacion de contrato
                    ConvenioSQL.Registrar_Med(p.ordenservicio, p.cliente, "2", "", p.contratoNumero, "", "", "", "", p.region, p.producto, "106", "");
                }
                else
                {
                    mensaje = "Error en Actualizacion de datos";
                    bandera = false;
                }
            }
            //mensaje
            if (bandera)
            {
                salida = new RespuestaDTO { codigo = "1", mensaje = mensaje, };
            }
            else
            {
                salida = new RespuestaDTO { codigo = "-1", mensaje = mensaje, };
                ConvenioSQL.Registrar_Med(p.ordenservicio, p.cliente, "3", mensaje, "0", "0", "0", p.accion, respuesta, "", "", "104", texto);
            }
            return salida;
        }


        [HttpGet]
        [Authorize]
        [Route("/HunterMed/Cliente")]
        public ActionResult<string> Cliente(string opcion, string cliente,  string idOrden, string accion)
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
                API_URL = API_URL + "&cliente=" + cliente;
            }
            if (idOrden != "" && idOrden != null)
            {
                API_URL = API_URL + "&idorden=" + idOrden;
            }
            if (accion != "" && accion != null)
            {
                API_URL = API_URL + "&accion=" + accion;
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


        [HttpPost]
        [Authorize]
        [Route("/HunterMed/ConsultaContrato")]
        public ActionResult<string> ConsultaContrato(ContratoNuevoDTO p)
        {
            conexionConvenio ruta = new conexionConvenio();
            string rutaToken = ruta.GetRutaHunterMed("4");
            bool bandera = true;
            string valoretorno = "";
            string valor = "";
            string token = "";
            string texto = "";
            DataTable tabla = new DataTable();
            tabla.Columns.Add("CODIGO", typeof(int));
            tabla.Columns.Add("MENSAJE", typeof(string));
            //conuslta token
            var clientetoken = new RestClient();
            var requestetoken = new RestRequest();
            RestResponse responsetoken;
            texto = Utilidades.SerializarAuthenticate(p.username, p.password);
            clientetoken = new RestClient(rutaToken + "/api/user/authenticate");
            requestetoken = new RestRequest("", Method.Post);
            requestetoken.AddHeader("content-type", "application/json");
            requestetoken.AddParameter("application/json", texto, ParameterType.RequestBody);
            responsetoken = clientetoken.Execute(requestetoken);

            if (Convert.ToInt32(responsetoken.StatusCode) == 200)
            {
                var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(responsetoken.Content);
                token = funciones.descomponer("token", jsonString);
                bandera = true;
            }
            else
            {
                string mensaje = funciones.DevuelveMensaje(responsetoken.Content);
                tabla.Rows.Add("-1", mensaje + " " + responsetoken.StatusCode + " Token");
                bandera = false;
            }
            //consulta data
            if (bandera)
            {
                var contratosearch = new RestClient();
                var requestecontratosearch = new RestRequest();
                RestResponse responsecontratosearch;
                contratosearch = new RestClient(rutaToken + "/api/contracts" + "?Identification=" + p.identification + "&ProductCode=" + p.product_code + "&PlanCode=" + p.plan_code + "&CertificateNumber=" + p.certificate_number + " ");
                requestecontratosearch = new RestRequest("", Method.Get);
                requestecontratosearch.AddHeader("cache-control", "no-cache");
                requestecontratosearch.AddHeader("authorization", "Bearer " + token);
                responsecontratosearch = contratosearch.Execute(requestecontratosearch);
                if (Convert.ToInt32(responsecontratosearch.StatusCode) == 200)
                {
                    valoretorno = funciones.descomponer("estado_contrato", responsecontratosearch.Content);
                    tabla.Rows.Add("1", responsecontratosearch.Content + " Contracts");
                    bandera = true;
                }
                else
                {
                    string mensaje = funciones.DevuelveMensaje(responsetoken.Content);
                    tabla.Rows.Add("-1", responsecontratosearch.Content + " Contracts");
                    bandera = false;
                }
            }
            string jsonData;     
            jsonData = (string)Utilidades.DataTableToJSON(tabla);
            return jsonData;
        }


        [HttpPost]
        [Authorize]
        [Route("/HunterMed/CrearContratoNuevo")]
        public ActionResult<RespuestaSaludDTO> CrearContratoNuevo([FromBody] SaludCreaDTO p, string usuario, string password)
        {
            RespuestaSaludDTO salida = null;
            bool bandera = true;
            string texto = "";
            string token = "";
            conexionConvenio ruta = new conexionConvenio();
            string rutaToken = ruta.GetRutaHunterMed("4");
            //conuslta token
            var clientetoken = new RestClient();
            var requestetoken = new RestRequest();
            RestResponse responsetoken;
            texto = Utilidades.SerializarAuthenticate(usuario, password);
            clientetoken = new RestClient(rutaToken + "/api/user/authenticate");
            requestetoken = new RestRequest("", Method.Post);
            requestetoken.AddHeader("content-type", "application/json");
            requestetoken.AddParameter("application/json", texto, ParameterType.RequestBody);
            responsetoken = clientetoken.Execute(requestetoken);
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(responsetoken.Content);
            if (Convert.ToInt32(responsetoken.StatusCode) == 200)
            {
                token = funciones.descomponer("token", jsonString);
                bandera = true;
            }
            else
            {
                string mensaje = funciones.DevuelveMensaje(responsetoken.Content).Replace("message", "").Replace("\n", "");
                salida = new RespuestaSaludDTO
                {
                    status= false,
                    message = mensaje
                };
                bandera = false;
            }
            //crea el contrato
            if (bandera)
            {
                //jsonString = JsonConvert.SerializeObject(p, Formatting.Indented);
                var contratosearch = new RestClient();
                var requestecontratosearch = new RestRequest();
                RestResponse responsecontratosearch;
                contratosearch = new RestClient(rutaToken + "/api/sales" );
                requestecontratosearch = new RestRequest("", Method.Post);
                requestecontratosearch.AddHeader("cache-control", "no-cache");
                requestecontratosearch.AddHeader("authorization", "Bearer " + token);
                requestecontratosearch.AddParameter("application/json", p, ParameterType.RequestBody);
                responsecontratosearch = contratosearch.Execute(requestecontratosearch);
                if (Convert.ToInt32(responsecontratosearch.StatusCode) == 200)
                {
                    string status = funciones.descomponer("status", responsecontratosearch.Content);
                    string texto_devuelto = "";
                    texto_devuelto = responsecontratosearch.Content;
                    if (bool.Parse(status))
                    {
                        //// bandera = true;
                        //String respuestaMail = ConvenioSQL.GenerarMail(p.ordenservicio, p.identification, producto);
                        //if (respuestaMail.Equals("OK"))
                        //{
                        //    //Enviar Mail de confirmacion
                        //    bandera = true;
                        //}
                        //else
                        //{
                        //    bandera = false;
                        //}
                        RespuestaSaludOkDTO respuestaok = null;
                        respuestaok = JsonConvert.DeserializeObject<RespuestaSaludOkDTO>(texto_devuelto);
                        salida = new RespuestaSaludDTO
                        {
                            status = respuestaok.status,
                            message = respuestaok.message,
                            client_id = respuestaok.data.client_id,
                            contract_id = respuestaok.data.contract_id,
                            code ="",
                            data=""
                        };
                        bandera = true;
                    }
                    else
                    {
                        RespuestaSaludErrorDTO respuestaerror = null;
                        respuestaerror = JsonConvert.DeserializeObject<RespuestaSaludErrorDTO>(texto_devuelto);
                        //string jsonStringError = "";
                        //jsonStringError = JsonConvert.SerializeObject(respuestaerror.error.message, Formatting.Indented);
                        string json = respuestaerror.error.message;
                        string mensajeerror = "";
                        if (Utilidades.EsJsonObjeto(json))
                        {
                            RootError result = JsonConvert.DeserializeObject<RootError>(json);
                            foreach (var mensaje in result.errors.error)
                            {
                                Console.WriteLine("Mensaje de error: " + mensaje);
                                mensajeerror = mensaje;
                            }
                        }
                        else
                        {
                            mensajeerror = json;
                        }
                        salida = new RespuestaSaludDTO
                        {
                            status = respuestaerror.status,
                            //status = false,
                            //message = "ERROR, " + mensajeerror.Replace(":", "").Replace("<", "").Replace("'", "").Replace(",", "").Replace(".", ""),
                            message = mensajeerror,
                            //message = "ERROR, NO SE GENERO CONTRATO",
                            client_id = "",                   
                            contract_id ="",
                            //code = "",
                            code = respuestaerror.error.code,
                            data = respuestaerror.message
                            //data = ""
                        };

                        bandera = false;
                    }
                }
                else
                {
                    salida = new RespuestaSaludDTO
                    {
                        status = false,
                        message = "NO SE PUDO GENERAR LA CREACION DEL CONTRATO",
                        client_id = "",
                        contract_id = "",
                        code = "",
                        data = ""
                    };
                    bandera = false;
                }
            }
            DataSet cnstGenrl = new DataSet();
            jsonString = JsonConvert.SerializeObject(salida, Formatting.Indented);
            if (bandera)
            {             
                cnstGenrl = ConvenioSQL.ActualidaSalud("110", "4",  jsonString, p.certificate_number); 
            } else
            {
                cnstGenrl = ConvenioSQL.ActualidaSalud("110", "3", jsonString, p.certificate_number);
            }
            return salida;
        }


        [HttpPost]
        [Authorize]
        [Route("/HunterMed/CancelarContrato")]
        public ActionResult<RespuestaSaludDTO> CancelarContrato([FromBody] SaludDTO p, string usuario, string password)
        {
            RespuestaSaludDTO salida = null;
            bool bandera = true;
            string texto = "";
            string token = "";
            conexionConvenio ruta = new conexionConvenio();
            string rutaToken = ruta.GetRutaHunterMed("4");
            //conuslta token
            var clientetoken = new RestClient();
            var requestetoken = new RestRequest();
            RestResponse responsetoken;
            texto = Utilidades.SerializarAuthenticate(usuario, password);
            clientetoken = new RestClient(rutaToken + "/api/user/authenticate");
            requestetoken = new RestRequest("", Method.Post);
            requestetoken.AddHeader("content-type", "application/json");
            requestetoken.AddParameter("application/json", texto, ParameterType.RequestBody);
            responsetoken = clientetoken.Execute(requestetoken);
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(responsetoken.Content);
            if (Convert.ToInt32(responsetoken.StatusCode) == 200)
            {
                token = funciones.descomponer("token", jsonString);
                bandera = true;
            }
            else
            {
                string mensaje = funciones.DevuelveMensaje(responsetoken.Content).Replace("message", "").Replace("\n", "");
                salida = new RespuestaSaludDTO
                {
                    status = false,
                    message = mensaje
                };
                bandera = false;
            }
            //anular el contrato
            if (bandera)
            {
                var contratoanularsearch = new RestClient();
                var requestecontratoanularsearch = new RestRequest();
                RestResponse responsecontratoanularsearch;
                contratoanularsearch = new RestClient(rutaToken + "/api/contracts/cancel");
                requestecontratoanularsearch = new RestRequest("", Method.Delete);
                requestecontratoanularsearch.AddHeader("cache-control", "no-cache");
                requestecontratoanularsearch.AddHeader("authorization", "Bearer " + token);
                requestecontratoanularsearch.AddParameter("application/json", p, ParameterType.RequestBody);
                responsecontratoanularsearch = contratoanularsearch.Execute(requestecontratoanularsearch);
                if (Convert.ToInt32(responsecontratoanularsearch.StatusCode) == 200)
                {
                    string status = funciones.descomponer("status", responsecontratoanularsearch.Content);
                    string texto_devuelto = "";
                    texto_devuelto = responsecontratoanularsearch.Content;
                    if (bool.Parse(status))
                    {
                        salida = JsonConvert.DeserializeObject<RespuestaSaludDTO>(texto_devuelto);
                        bandera = true;
                    }
                    else
                    {
                        RespuestaSaludErrorDTO respuestaerror = null;
                        respuestaerror = JsonConvert.DeserializeObject<RespuestaSaludErrorDTO>(texto_devuelto);
                        string json = respuestaerror.error.message;
                        string mensajeerror = "";
                        if (Utilidades.EsJsonObjeto(json))
                        {
                            RootError result = JsonConvert.DeserializeObject<RootError>(json);
                            foreach (var mensaje in result.errors.error)
                            {
                                Console.WriteLine("Mensaje de error: " + mensaje);
                                mensajeerror = mensaje;
                            }
                        }
                        else
                        {
                            mensajeerror = json;
                        }
                        salida = new RespuestaSaludDTO
                        {
                            status = respuestaerror.status,
                            //message = mensajeerror.Replace(":", "").Replace("<", "").Replace("'", "").Replace(",", "").Replace(".", ""),
                            message = mensajeerror,
                            client_id = "",
                            contract_id = "",
                            code = respuestaerror.error.code,
                            data = respuestaerror.message
                        };
                        bandera = false;
                    }                    
                }
                else
                {
                    salida = new RespuestaSaludDTO
                    {
                        status = false,
                        message = "NO SE PUDO GENERAR LA ANULACION",
                        client_id = "",
                        contract_id = "",
                        code = "",
                        data = ""
                    };
                    bandera = false;
                }
            }
            DataSet cnstGenrl = new DataSet();
            jsonString = JsonConvert.SerializeObject(salida, Formatting.Indented);
            if (bandera)
            {
                cnstGenrl = ConvenioSQL.ActualidaSalud("110", "4", jsonString, p.certificate_number);
            }
            else
            {
                cnstGenrl = ConvenioSQL.ActualidaSalud("110", "3", jsonString, p.certificate_number);
            }
            return salida;
        }


        [HttpPost]
        [Authorize]
        [Route("/Claro/NotificacionClaro")]
        public ActionResult<string> NotificacionClaro([FromBody] ClaroDTO p)
        {
            conexion ruta = new conexion();
            string API_URL = ruta.ObtenerRuta("5276", "1");
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


        // [HttpGet]
        // // [Authorize]
        // [Route("/Icesa/ProcesarVehiculo")]
        // public ActionResult<ProcesarVehiculo> ProcesarVehiculo()
        // {
        //     ProcesarVehiculo respuesta = null;
        //     string valor = "";
        //     conexionConvenio ruta = new conexionConvenio();
        //     string consultarvehiculo = ruta.GetRutaIcesa("1");
        //     var client = new RestClient(consultarvehiculo);
        //     var request = new RestRequest("", Method.Get);
        //     request.AddHeader("x-user", "Hunter");
        //     request.AddHeader("x-password", "hunter*$$XXw");
        //     RestResponse response = client.Execute(request);
        //     valor = response.StatusCode.ToString();
        //     if (valor.Equals("OK"))
        //     {               
        //         respuesta = JsonConvert.DeserializeObject<ProcesarVehiculo>("{\"results\":" + response.Content + "}");
        //     }
        //     return respuesta;
        // }


        // [HttpPost]
        // //[Authorize]
        // [Route("/Icesa/VehiculoChequeado")]
        // public ActionResult<RespuestaDTO> VehiculoChequeado()
        // {
        //     string valor = "";
        //     string jsonData = "";
        //     string mensaje = "";
        //     RespuestaDTO salida = null;
        //     DataTable tabla = new DataTable();
        //     DataSet cnstGenrl = new DataSet();
        //     cnstGenrl = ConvenioSQL.VehiculoChequeado("2");
        //     if (cnstGenrl.Tables.Count > 0)
        //     {
        //         conexionConvenio ruta = new conexionConvenio();
        //         string VehiculoChequeado = ruta.GetRutaIcesa("1");
        //         var client = new RestClient(VehiculoChequeado);
        //         var request = new RestRequest("", Method.Post);
        //         request.AddHeader("x-user", "Hunter");
        //         request.AddHeader("x-password", "hunter*$$XXw");
        //         tabla = cnstGenrl.Tables[0];
        //         DataTable firstTable = tabla;
        //         jsonData = JsonConvert.SerializeObject(firstTable);
        //         request.AddParameter("application/json", "{\"results\":" + jsonData + "}", ParameterType.RequestBody);
        //         RestResponse response = client.Execute(request);
        //         valor = response.StatusCode.ToString();
        //         if (valor.Equals("OK"))
        //         {
        //             var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(response.Content);
        //             mensaje = funciones.descomponer("message", jsonString);
        //             valor = funciones.descomponer("ok", jsonString);
        //             if (valor.Equals("true"))
        //             {
        //                 Int32 registros = cnstGenrl.Tables[0].Rows.Count;
        //                 for (int i = 0; i <= cnstGenrl.Tables[0].Rows.Count - 1; i++)
        //                 {
        //                     ConvenioSQL.ActualizarEstado((string)cnstGenrl.Tables[0].Rows[i]["ID"], "3", mensaje, jsonData, registros);
        //                 }
        //                 salida = new RespuestaDTO { codigo = "1", mensaje = "Guardado Exitosamente, " + mensaje, };
        //             }
        //             else
        //             {
        //                 salida = new RespuestaDTO { codigo = "-1", mensaje = "Error..." + mensaje, };
        //             }
        //         }
        //         else
        //         {
        //             salida = new RespuestaDTO { codigo = "-1", mensaje = "Error en el envio", };
        //         }
        //     }
        //     return salida;
        // }


        // [HttpGet]
        // //[Authorize]
        // [Route("/Icesa/ProcesarVenta")]
        // public ActionResult<ProcesarVenta> ProcesarVenta()
        // {
        //     ProcesarVenta respuesta = null;
        //     string valor = "";
        //     conexionConvenio ruta = new conexionConvenio();
        //     string consultarvehiculo = ruta.GetRutaIcesa("2");
        //     var client = new RestClient(consultarvehiculo);
        //     var request = new RestRequest("", Method.Get);
        //     request.AddHeader("x-user", "Hunter");
        //     request.AddHeader("x-password", "hunter*$$XXw");
        //     RestResponse response = client.Execute(request);
        //     valor = response.StatusCode.ToString();
        //     if (valor.Equals("OK"))
        //     {
        //         respuesta = JsonConvert.DeserializeObject<ProcesarVenta>("{\"results\":" + response.Content + "}");
        //     }
        //     return respuesta;
        // }


        // [HttpGet]
        //// [Authorize]
        // [Route("/Icesa/ProcesarVentaNota")]
        // public ActionResult<ProcesarVenta> ProcesarVentaNota()
        // {
        //     ProcesarVenta respuesta = null;
        //     string valor = "";
        //     conexionConvenio ruta = new conexionConvenio();
        //     string consultarvehiculo = ruta.GetRutaIcesa("3");
        //     var client = new RestClient(consultarvehiculo);
        //     var request = new RestRequest("", Method.Get);
        //     request.AddHeader("x-user", "Hunter");
        //     request.AddHeader("x-password", "hunter*$$XXw");
        //     RestResponse response = client.Execute(request);
        //     valor = response.StatusCode.ToString();
        //     if (valor.Equals("OK"))
        //     {
        //         respuesta = JsonConvert.DeserializeObject<ProcesarVenta>("{\"results\":" + response.Content + "}");
        //     }
        //     return respuesta;
        // }


        // [HttpPost]
        // //  [Authorize]
        // [Route("/Mareauto/ActualizarDispositivo")]
        // public ActionResult<RespuestaDTO> ActualizarDispositivo(string empresa, string orden, string imei, string placa,string chasis, string estado, string observacion)
        // {
        //     RespuestaDTO salida = null;
        //     bool bandera = true;
        //     string mensaje = "";
        //     string valor = "";
        //     if (empresa == null || string.IsNullOrEmpty(empresa.ToString()))
        //     {
        //         mensaje = "Valor de empresa en blanco";
        //         bandera = false;
        //     }
        //     else if (orden == null || string.IsNullOrEmpty(orden.ToString()))
        //     {
        //         mensaje = "Valor de orden en blanco";
        //         bandera = false;
        //     }
        //     else if (imei == null || string.IsNullOrEmpty(imei.ToString()))
        //     {
        //         mensaje = "Valor de imei en blanco";
        //         bandera = false;
        //     }
        //     else if (placa == null || string.IsNullOrEmpty(placa.ToString()))
        //     {
        //         mensaje = "Valor de placa en blanco";
        //         bandera = false;
        //     }
        //     else if (chasis == null || string.IsNullOrEmpty(chasis.ToString()))
        //     {
        //         mensaje = "Valor de chasis en blanco";
        //         bandera = false;
        //     }
        //     else if (estado == null || string.IsNullOrEmpty(estado.ToString()))
        //     {
        //         mensaje = "Valor de orden en blanco";
        //         bandera = false;
        //     }
        //     else if (observacion == null || string.IsNullOrEmpty(observacion.ToString()))
        //     {
        //         mensaje = "Valor de observacion en blanco";
        //         bandera = false;
        //     }

        //     if (bandera)
        //     {
        //         RestResponse response;
        //         var options = new RestClientOptions("https://mareauto.com")
        //         {
        //             MaxTimeout = -1,
        //         };
        //         var client = new RestClient(options);
        //        // var requeste = new RestRequest("/api/location/solicitud/?aicempresa=" + empresa + "&ainorden=" + orden + "&aicimei=" + imei + "&aicplaca=" + placa + "&aicchasis=" + chasis + "&ainestado=" + estado + "&aicobservacion=" + observacion, Method.Post);
        //         var requeste = new RestRequest("/api/location/solicitud/", Method.Post);
        //         requeste.AddHeader("Content-Type", "application/x-www-form-urlencoded");
        //         requeste.AddParameter("aicempresa", empresa);
        //         requeste.AddParameter("ainorden", orden);
        //         requeste.AddParameter("aicimei", imei);
        //         requeste.AddParameter("aicplaca", placa);
        //         requeste.AddParameter("aicchasis", chasis);
        //         requeste.AddParameter("ainestado", estado);
        //         requeste.AddParameter("aicobservacion", observacion);
        //         response = client.Execute(requeste);
        //         valor = response.StatusCode.ToString();
        //         if (valor.Equals("OK"))
        //         {
        //             bandera = true;
        //             mensaje = response.Content.ToUpper();
        //         }
        //         else
        //         {
        //             bandera = false;
        //             mensaje = "Error en envio a Mareauto";
        //         }
        //     }
        //     mensaje = mensaje.Replace("\"", "").Replace("\n", "");
        //     if (bandera)
        //     {
        //         salida = new RespuestaDTO { codigo = "1", mensaje = mensaje, };
        //     }
        //     else
        //     {
        //         salida = new RespuestaDTO { codigo = "-1", mensaje = mensaje, };
        //     }
        //     return salida;
        // }


        // [HttpPost]
        // //[Authorize]
        // [Route("/Automekano/DispositivoAutomekano")]
        // public ActionResult<RespuestaDTO> DispositivoAutomekano([FromBody] ActualizarDispositivoDTO p)
        // {


        //     RespuestaDTO respuesta = null;
        //     string textoLogin = "";
        //     string valor = "";
        //     string valorToken = "";
        //     //string valorTokenSeguridad = "";
        //     //string textoSeguridad = "";
        //     //string textoActualizar = "";
        //     //bool bandera = false;
        //     //conexionConvenio ruta = new conexionConvenio();
        //     //string login = ruta.GetRutaAmbacar("1");
        //     //string authenticate = ruta.GetRutaAmbacar("2");
        //     //string actualizardispositivo = ruta.GetRutaAmbacar("3");
        //     //var client = new RestClient("https://ambysoftapi.ambacar.ec/MS_SeguridadesCommand/api/Login/Login");
        //     var client = new RestClient("http://wspilot.automekano.com:2052");
        //     var request = new RestRequest("api/login/authenticateHunter", Method.Post);
        //     textoLogin = Utilidades.SerializarDatos(p.login, p.pass, "", "", "", "", "", "", "", "", "login");
        //     request.AddHeader("content-type", "application/json");
        //     request.AddParameter("application/json", textoLogin, ParameterType.RequestBody);
        //     RestResponse response = client.Execute(request);
        //     valor = response.StatusCode.ToString();
        //     if (valor.Equals("OK"))
        //     {
        //         var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(response.Content);
        //         valorToken =  jsonString; //BusquedaDatos(response.Content, "token");
        //        // bandera = true;
        //     }
        //    // else
        //    // {
        //        // bandera = false;
        //    // }
        //     respuesta = new RespuestaDTO { codigo = "1", mensaje = valor, };

        //     return respuesta;
        // }


    }
}
