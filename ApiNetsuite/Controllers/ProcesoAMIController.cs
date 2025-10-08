using ApiNetsuite.Clases;
using ApiNetsuite.DTO.AMI;
using ApiNetsuite.Repositorio.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ApiNetsuite.Modelo;
using ApiNetsuite.Repositorio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using RestSharp;
using System;
using System.Data;
using Swashbuckle.AspNetCore.Annotations;

namespace ApiNetsuite.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("ProcesoAMI")]
    public class ProcesoAMIController : ControllerBase
    {

        private readonly IProcesoAMISQL repositorio;
        protected RespuestAPI _respuestaApi;

        public ProcesoAMIController(IProcesoAMISQL r)
        {
            this.repositorio = r;
            this._respuestaApi = new();
        }


        [HttpPost]
        [Authorize]
        [SwaggerOperation(Summary = "", Description = "Instalación impulso hacia la plataforma AMI")]
        [Route("Instalacion")]
        public ActionResult<string> Instalacion(InstalacionDTO p)
        {
            // ruta ami 
            conexionAMI ruta = new conexionAMI();
            string rutalogin = "";
            string rutaassetAdd = "";
            string rutacustomerAdd = "";
            string rutadeviceAdd = "";
            string rutacommandAdd = "";
            string rutausercommandAdd = "";
            string rutaasset = "";
            string rutacustomer = "";
            string rutadevice = "";
            string rutacommand = "";
            string rutausercommand = "";
            // ruta WESAFE
            string rutacompany = "";
            string rutaAddcompany = "";
            string rutaUpdatecompany = "";
            string rutaUpdatedevice = "";
            string rutaTransferdevice = "";
            string rutadevicelist = "";
            // variables
            bool bandera = true;
            bool banderawesafe = false;
            char delimiter = ',';
            string gencomando = p.comando;
            string[] comandos = p.comando.Split(delimiter);
            string customerID = "";
            string assetID = "";
            string token = "";
            string valoretorno = "";
            string userpropietario = "";
            string accion = "";
            string envio = "";
            string ownerID = "0";
            string usermonitoreo = "";
            string tiporegistro = "INS";
            string asociadousers = "";
            string valorvid = "";
            string asociadoAmi = "";
            string asociadoID = "";
            DataSet infoDatos = new DataSet();
            string log_usr = "";
            string log_pwd = "";
            string producto = "";
            string concesionario = "";
            DataTable tabla = new DataTable();
            tabla.Columns.Add("CODIGO", typeof(int));
            tabla.Columns.Add("MENSAJE", typeof(string));
            // busqueda de ruta por el parametro
            if (p.parametroproducto == "" | string.IsNullOrEmpty(p.parametroproducto))
                p.parametroproducto = "GPT";
            if (p.parametroproducto == "GPT")
            {
                rutalogin = ruta.GetRuta("1");
                rutaassetAdd = ruta.GetRuta("2");
                rutacustomerAdd = ruta.GetRuta("3");
                rutadeviceAdd = ruta.GetRuta("4");
                rutacommandAdd = ruta.GetRuta("5");
                rutausercommandAdd = ruta.GetRuta("6");
                rutaasset = ruta.GetRuta("12");
                rutacustomer = ruta.GetRuta("13");
                rutadevice = ruta.GetRuta("14");
                rutacommand = ruta.GetRuta("15");
                rutausercommand = ruta.GetRuta("16");
            }
            else if (p.parametroproducto == "GPA")
            {
                rutalogin = ruta.GetRuta("41");
                rutaassetAdd = ruta.GetRuta("42");
                rutacustomerAdd = ruta.GetRuta("43");
                rutadeviceAdd = ruta.GetRuta("44");
                rutacommandAdd = ruta.GetRuta("45");
                rutausercommandAdd = ruta.GetRuta("46");
                rutaasset = ruta.GetRuta("52");
                rutacustomer = ruta.GetRuta("53");
                rutadevice = ruta.GetRuta("54");
                rutacommand = ruta.GetRuta("55");
                rutausercommand = ruta.GetRuta("56");
            }
            else if (p.parametroproducto == "GPW")
            {
                rutalogin = ruta.GetRuta("60");
                rutacompany = ruta.GetRuta("68");
                rutaAddcompany = ruta.GetRuta("62");
                rutaUpdatecompany = ruta.GetRuta("63");
                rutadevice = ruta.GetRuta("69");
                rutadevicelist = ruta.GetRuta("64");
                rutaUpdatedevice = ruta.GetRuta("65");
                rutaTransferdevice = ruta.GetRuta("66");
                banderawesafe = true;
            }
            else
            {
                bandera = false;
                tabla.Rows.Add("-1", "No existe el parametro del producto");
            }
            // If clientemonitoreo = "0907097554001" Then
            // clientemonitoreo = "0907097554"
            // End If
            // PROCESO DE WESAFE
            if (bandera & banderawesafe == true)
            {
               // proceso de wesafe
            }
            // PROCESO DE AMI Y LATAM
            if (bandera & banderawesafe == false)
            {
                customerEntidad monitoreo = null;
                customerEntidad propietario = null;
                deviceEntidad device = null;
                assetEntidad asset = null;
                attributeEntidadCollection attributeEntidadCollection = null;
                attributeEntidad attributeCollection = null;
                string monitoreoAMI = "";
                string propietarioAMI = "";
                string deviceAMI = "";
                string assetAMI = "";
                // datos
                if (bandera)
                {
                    DataSet cnstGenrl = new DataSet();
                    cnstGenrl = BienSQL.CnstVehiculo(opcion: "100", cliente: "", vehiculo: "", parametroproducto: "GPT");
                    string support = (string)cnstGenrl.Tables[0].Rows[0]["support"];
                    log_usr = (string)cnstGenrl.Tables[0].Rows[0]["usuario"];
                    log_pwd = (string)cnstGenrl.Tables[0].Rows[0]["pass"];
                    string ami_telefono = (string)cnstGenrl.Tables[0].Rows[0]["telefono"];
                    string ami_celular = (string)cnstGenrl.Tables[0].Rows[0]["celular"];
                    string ruccompania = (string)cnstGenrl.Tables[0].Rows[0]["ruccompania"];
                    if (bandera)
                    {
                        // informacion del cliente Monitoreo
                        monitoreoAMI = ConsultaAMI.ProcesoAMIConsulta("200", p.codigovehiculo, p.producto, p.ordenservicio, p.itemNS, "");
                        accion = funciones.descomponer("ACCION", monitoreoAMI);
                        // VERIFICA SI HAY QUE ENVIAR DE NUEVO EL EMAIL DE ACTIVACION 
                        envio = "N";
                        usermonitoreo = funciones.descomponer("USERNAME", monitoreoAMI);
                        //usermonitoreo = "JCAROLINA14@HOTMAIL.COM";
                        monitoreo = new customerEntidad
                        {
                            username = funciones.descomponer("USERNAME", monitoreoAMI),
                            //username = "JUAVILA53@HOTMAIL.COM",
                            first_name = funciones.descomponer("FIRST_NAME", monitoreoAMI),
                            last_name = funciones.descomponer("LAST_NAME", monitoreoAMI),
                            email = funciones.descomponer("EMAIL", monitoreoAMI),
                            login = log_usr,
                            password = log_pwd,
                            internal_identifier = funciones.descomponer("ID_CLIENTE", monitoreoAMI),
                            active = "True",
                            telefono = funciones.descomponer("TELEFONO_CLIENTE", monitoreoAMI),
                            identity_customer_type = funciones.descomponer("TIPO", monitoreoAMI),
                            company_code = ruccompania,
                            business_name = "",
                            emergency_phone_number = ami_telefono,
                            assistance_phone_number = ami_celular,
                            technical_support_email = support,
                        };
                    }

                    // informacion del cliente
                    if (bandera)
                    {
                        propietarioAMI = ConsultaAMI.ProcesoAMIConsulta("201", p.codigovehiculo, p.producto, p.ordenservicio, p.itemNS, "");
                        // VERIFICA SI HAY QUE ENVIAR DE NUEVO EL EMAIL DE ACTIVACION 
                        userpropietario = funciones.descomponer("USERNAME", propietarioAMI);
                        //userpropietario = "JUAVILA53@HOTMAIL.COM";
                        propietario = new customerEntidad
                        {
                              username = funciones.descomponer("USERNAME", propietarioAMI),
                             //username = "JUAVILA53@HOTMAIL.COM",
                            first_name = funciones.descomponer("FIRST_NAME", propietarioAMI),
                            last_name = funciones.descomponer("LAST_NAME", propietarioAMI),
                            email = funciones.descomponer("EMAIL", propietarioAMI),
                            login = log_usr,
                            password = log_pwd,
                            internal_identifier = funciones.descomponer("ID_CLIENTE", propietarioAMI),
                            active = "True",
                            telefono = funciones.descomponer("TELEFONO_CLIENTE", propietarioAMI),
                            identity_customer_type = funciones.descomponer("TIPO", propietarioAMI),
                            company_code = ruccompania,
                            business_name = "",
                            emergency_phone_number = ami_telefono,
                            assistance_phone_number = ami_celular,
                            technical_support_email = support,
                        };
                    }
                   
                    //proceso device
                    if (bandera)
                    {
                        deviceAMI = ConsultaAMI.ProcesoAMIConsulta("202", p.codigovehiculo, p.producto, p.ordenservicio, p.itemNS, "");
                        valorvid = funciones.descomponer("vid", deviceAMI);

                        if (p.serie =="")
                        {
                            p.serie = funciones.descomponer("serie", deviceAMI);
                        }
                        device = new deviceEntidad
                        {
                            id = p.serie,
                            vid = funciones.descomponer("vid", deviceAMI),
                            company_code = ruccompania,
                            active = "True",
                            model = funciones.descomponer("id_telematic_modelo", deviceAMI),
                            //model ="4",
                            report_from = funciones.descomponer("id_telematic_servidor", deviceAMI),
                           //report_from = "5",
                            numero_celular = funciones.descomponer("noCelularSIM", deviceAMI),
                        };

                        if (valorvid=="0")
                        {
                            tabla.Rows.Add("-1", "Verificar el valor del VID ");
                            bandera = false;
                        }
                        if (device.report_from == "" || device.report_from == "0")
                        {
                            tabla.Rows.Add("-1", "Verificar el Servidor del Dispositivo ");
                            bandera = false;
                        }
                        if (device.model == "" || device.model == "0")
                        {
                            tabla.Rows.Add("-1", "Verificar el Modelo del Dispositivo ");
                            bandera = false;
                        }
                    }
                   
                    // informacion del vehiculo y atributos
                    if (bandera)
                    {
                        // informacion del vehiculo
                        assetAMI = ConsultaAMI.ProcesoAMIConsulta("203", p.codigovehiculo, p.producto, p.ordenservicio, p.itemNS, "");
                        string productoText = funciones.descomponer("familiadescripcion", assetAMI);
                        if (productoText == "HUNTER GPS HMT")
                        {
                            producto = "H.MONITOREO AMI";
                        }
                        else if (productoText == "AMI CHANGAN BY HUNTER")
                        {
                            producto = "H.MONITOREO AMI";
                        }
                        else if (productoText == "ANDOR BY HUNTER")
                        {
                            producto = "ANDOR LINK BY HUNTER";
                        }
                        else
                        {
                            producto = productoText;
                        }
                        concesionario = funciones.descomponer("concesionarios", assetAMI);
                        cnstGenrl = BienSQL.CnstProducto(opcion: "104", concesionario: concesionario, producto: p.producto);
                        if (cnstGenrl.Tables[0].Rows.Count > 0)
                        {
                            producto = (string)cnstGenrl.Tables[0].Rows[0]["NOMBRE_PRODUCTO"];                           
                        }
                        //asset = new assetEntidad
                        //{
                        //    name = funciones.descomponer("placa", assetAMI),
                        //    assettype = funciones.descomponer("tipo", assetAMI),
                        //    codigosys = p.codigosys, //CODIGO SYS O NETSUITE
                        //    codigonetsuite = p.codigovehiculo,
                        //    customname = "",
                        //    active = "True",
                        //    description = "PLC.:" + funciones.descomponer("placa", assetAMI) + ";MAR.:" + funciones.descomponer("marca", assetAMI) + ";MOD.:" + funciones.descomponer("modelo", assetAMI) + ";COL.:" + funciones.descomponer("color", assetAMI),
                        //    //description = "PLC.:" + funciones.descomponer("placa", assetAMI) + "; MAR.:" + funciones.descomponer("marca", assetAMI) + "; MOD.:" +  if (Strings.Len(funciones.descomponer("modelo", assetAMI)) > 40) (funciones.descomponer("modelo", assetAMI)).Substring(0,40) else  funciones.descomponer("modelo", assetAMI) + "; COL.:" + funciones.descomponer("color", assetAMI),
                        //    productexpiredate = p.cobertura,
                        //    product = producto,
                        //    chasis = funciones.descomponer("chasis", assetAMI),
                        //};
                        //informacion del vehiculo atributos
                        //cnstGenrl = BienSQL.CnstVehiculoParametros(opcion: "103", cliente: "", vehiculo: p.codigosys,
                        //                                               parametroproducto: "GPT", tipo: funciones.descomponer("tipo", assetAMI), marca: funciones.descomponer("marca", assetAMI),
                        //                                               modelo: funciones.descomponer("modelo", assetAMI), chasis: funciones.descomponer("chasis", assetAMI), motor: funciones.descomponer("motor", assetAMI),
                        //                                               placa: funciones.descomponer("placa", assetAMI), color: funciones.descomponer("color", assetAMI), anio: funciones.descomponer("anio", assetAMI));

                        string modeloText;
                        if (Strings.Len(funciones.descomponer("modelo", assetAMI)) > 40)
                            modeloText = (funciones.descomponer("modelo", assetAMI)).Substring(0, 40);
                        else
                            modeloText = funciones.descomponer("modelo", assetAMI);
                        string placaText = funciones.descomponer("placa", assetAMI);
                        string marcaText = funciones.descomponer("marca", assetAMI);
                        string colorText = funciones.descomponer("color", assetAMI);
                        string tipoText = funciones.descomponer("tipo", assetAMI);
                        string chasisText = funciones.descomponer("chasis", assetAMI);
                        asset = new assetEntidad
                        {
                            name = placaText,
                            assettype = tipoText,
                            codigosys = p.codigosys, //CODIGO SYS O NETSUITE
                            codigonetsuite = p.codigovehiculo,
                            customname = "",
                            active = "True",
                            description = "PLC.:" + placaText + "; MAR.:" + marcaText + "; MOD.:" + modeloText + "; COL.:" + colorText,
                            productexpiredate = p.cobertura,
                            product = producto,
                            chasis = chasisText,
                        };
                        //informacion del vehiculo atributos
                        cnstGenrl = BienSQL.CnstVehiculoParametros(opcion: "103", cliente: "", vehiculo: p.codigosys,
                                                                   parametroproducto: "GPT", tipo: tipoText, marca: marcaText,
                                                                   modelo: modeloText, chasis: chasisText, motor: funciones.descomponer("motor", assetAMI),
                                                                   placa: placaText, color: colorText, anio: funciones.descomponer("anio", assetAMI));
                        if (cnstGenrl.Tables.Count > 0)
                        {
                            attributeEntidadCollection = new attributeEntidadCollection();
                            attributeCollection = new attributeEntidad();
                            for (int i = 0; i <= cnstGenrl.Tables[0].Rows.Count - 1; i++)
                            {
                                attributeEntidad attribute = new attributeEntidad();
                                attribute.AttributeId = (int)cnstGenrl.Tables[0].Rows[i]["ID"];
                                attribute.Valor = (string)cnstGenrl.Tables[0].Rows[i]["VALOR"];
                                attributeEntidadCollection.Add(attribute);
                            }
                            attributeCollection.attributeEntidadCollection = attributeEntidadCollection;
                        }
                    }
                }
                if (bandera)
                {
                    var client = new RestClient(rutalogin);
                    var request = new RestRequest("", Method.Post);
                    request.AddParameter("username", monitoreo.login);
                    request.AddParameter("password", monitoreo.password);
                    RestResponse response = client.Execute(request);
                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(response.Content);
                    token = funciones.descomponer("token", jsonString);
                    // Validaciones
                    if (p.codigocliente == "" | p.clientemonitoreo == "" | p.codigovehiculo == "" | p.serie == "")
                    {
                        tabla.Rows.Add("-1", "Verificar datos no enviados ");
                        bandera = false;
                    }
                    else if (accion == "" | accion == "000")
                    {
                        tabla.Rows.Add("-1", "Verificar la acción comercial del producto ");
                        bandera = false;
                    }
                }
                // bloque del cliente y cliente monitoreo
                if (bandera)
                {
                    var customersearch = new RestClient();
                    var requestecustomersearch = new RestRequest();
                    RestResponse responsecustomersearch;
                    var customer = new RestClient();
                    var requeste = new RestRequest();
                    RestResponse responsecustomer;
                    string textocustomer;
                    string accountType = "1";
                    if (p.codigocliente != p.clientemonitoreo)
                    {
                        // cliente propietario distinto
                        // consulta si existe el customer si se encuentra inactivo
                        customersearch = new RestClient(rutacustomerAdd + "?username=" + propietario.username + "&is_active=" + false + "&customer__company_code=" + propietario.company_code + "&customer__identity_document_number=" + propietario.internal_identifier + " ");
                        requestecustomersearch = new RestRequest("", Method.Get);
                        requestecustomersearch.AddHeader("cache-control", "no-cache");
                        requestecustomersearch.AddHeader("authorization", "HunterAPI " + token);
                        responsecustomersearch = customersearch.Execute(requestecustomersearch);
                        valoretorno = "0";
                        valoretorno = funciones.descomponer("id", responsecustomersearch.Content);
                        if (valoretorno != "0")
                        {
                            customer = new RestClient(rutacustomerAdd + valoretorno + "/ ");
                            requeste = new RestRequest("", Method.Patch);
                            textocustomer = Utilidades.SerializarCustomer(propietario,  valoretorno, accountType);
                            requeste.AddHeader("content-type", "application/json");
                            requeste.AddHeader("authorization", "HunterAPI " + token);
                            requeste.AddParameter("application/json", textocustomer, ParameterType.RequestBody);
                            responsecustomer = customer.Execute(requeste);
                            if (Convert.ToInt32(responsecustomer.StatusCode) == 200)
                            {
                                customerID = funciones.descomponer("id", responsecustomer.Content);
                                ownerID = customerID;
                                bandera = true;
                            }
                            else if (Convert.ToInt32(responsecustomer.StatusCode) == 400)
                            {
                                string mensaje = funciones.DevuelveMensaje(responsecustomer.Content);
                                tabla.Rows.Add("-1", mensaje + " " + responsecustomer.StatusCode + " customer Actualiza Owner");
                                bandera = false;
                            }
                            else
                            {
                                tabla.Rows.Add("-1", "Error al procesar customer " + responsecustomer.StatusCode + " customer Actualiza Owner");
                                bandera = false;
                            }
                        }
                        else
                        {
                            // consulta si existe el customer si se encuentra activo
                            customersearch = new RestClient(rutacustomerAdd + "?username=" + propietario.username + "&is_active=" + true + "&customer__company_code=" + propietario.company_code + "&customer__identity_document_number=" + propietario.internal_identifier + " ");
                            requestecustomersearch = new RestRequest("", Method.Get);
                            requestecustomersearch.AddHeader("cache-control", "no-cache");
                            requestecustomersearch.AddHeader("authorization", "HunterAPI " + token);
                            responsecustomersearch = customersearch.Execute(requestecustomersearch);
                            valoretorno = "0";
                            valoretorno = funciones.descomponer("id", responsecustomersearch.Content);
                            if (valoretorno == "0")
                            {
                                customer = new RestClient(rutacustomer);
                                requeste = new RestRequest("", Method.Post);
                                requeste.AddHeader("cache-control", "no-cache");
                                requeste.AddHeader("authorization", "HunterAPI " + token);
                                requeste.AddParameter("username", propietario.username);
                                requeste.AddParameter("first_name", propietario.first_name);
                                requeste.AddParameter("last_name", propietario.last_name);
                                requeste.AddParameter("email", propietario.email);
                                requeste.AddParameter("customer.company_code", propietario.company_code);
                                requeste.AddParameter("customer.identity_document_type", propietario.identity_customer_type);
                                requeste.AddParameter("customer.identity_document_number", propietario.internal_identifier);
                                requeste.AddParameter("customer.business_name", propietario.business_name);
                                requeste.AddParameter("customer.phone_number", propietario.telefono);
                                requeste.AddParameter("customer.emergency_phone_number", propietario.emergency_phone_number);
                                requeste.AddParameter("customer.assistance_phone_number", propietario.assistance_phone_number);
                                requeste.AddParameter("customer.technical_support_email", propietario.technical_support_email);
                                requeste.AddParameter("customer.account_type", accountType);
                                requeste.AddParameter("is_active", propietario.active);
                                responsecustomer = customer.Execute(requeste);
                                if (Convert.ToInt32(responsecustomer.StatusCode) == 201)
                                {
                                    customerID = funciones.descomponer("id", responsecustomer.Content);
                                    ownerID = customerID;
                                    bandera = true;
                                }
                                else if (Convert.ToInt32(responsecustomer.StatusCode) == 400)
                                {
                                    string mensaje = funciones.DevuelveMensaje(responsecustomer.Content);
                                    tabla.Rows.Add("-1", mensaje + " " + responsecustomer.StatusCode + " customer Owner");
                                    bandera = false;
                                }
                                else
                                {
                                    tabla.Rows.Add("-1", "Error al procesar customer " + responsecustomer.StatusCode + " customer Owner");
                                    bandera = false;
                                }
                            }
                            else
                            {
                                customersearch = new RestClient(rutacustomerAdd + "?username=" + propietario.username + "&customer__company_code=" + propietario.company_code + "&customer__identity_document_number=" + propietario.internal_identifier + " ");
                                requestecustomersearch = new RestRequest("", Method.Get);
                                requestecustomersearch.AddHeader("cache-control", "no-cache");
                                requestecustomersearch.AddHeader("authorization", "HunterAPI " + token);
                                responsecustomersearch = customersearch.Execute(requestecustomersearch);
                                valoretorno = "0";
                                valoretorno = funciones.descomponer("id", responsecustomersearch.Content);
                                if (valoretorno != "0")
                                {
                                    customer = new RestClient(rutacustomerAdd + valoretorno + "/ ");
                                    requeste = new RestRequest("", Method.Patch);
                                    textocustomer = Utilidades.SerializarCustomer(propietario,  valoretorno, accountType);
                                    requeste.AddHeader("content-type", "application/json");
                                    requeste.AddHeader("authorization", "HunterAPI " + token);
                                    requeste.AddParameter("application/json", textocustomer, ParameterType.RequestBody);                                    
                                    responsecustomer = customer.Execute(requeste);
                                    if (Convert.ToInt32(responsecustomer.StatusCode) == 200)
                                    {
                                        customerID = funciones.descomponer("id", responsecustomer.Content);
                                        ownerID = customerID;
                                        bandera = true;
                                    }
                                    else if (Convert.ToInt32(responsecustomer.StatusCode) == 400)
                                    {
                                        string mensaje = funciones.DevuelveMensaje(responsecustomer.Content);
                                        tabla.Rows.Add("-1", mensaje + " " + responsecustomer.StatusCode + " customer Actualiza owner");
                                        bandera = false;
                                    }
                                    else
                                    {
                                        tabla.Rows.Add("-1", "Error al procesar customer " + responsecustomer.StatusCode + " customer Actualiza owner");
                                        bandera = false;
                                    }
                                }
                                else
                                {
                                    tabla.Rows.Add("-1", "User Name duplicado customer Actualiza owner");
                                    bandera = false;
                                }
                            }
                        }
                        accountType = "2";
                    }
                    
                    // customer
                    if (bandera)
                    {
                        // consulta si existe el customer si se encuentra inactivo
                        customersearch = new RestClient(rutacustomerAdd + "?username=" + monitoreo.username + "&is_active=" + false + "&customer__company_code=" + monitoreo.company_code + "&customer__identity_document_number=" + monitoreo.internal_identifier + " ");
                        requestecustomersearch = new RestRequest("", Method.Get);
                        requestecustomersearch.AddHeader("cache-control", "no-cache");
                        requestecustomersearch.AddHeader("authorization", "HunterAPI " + token);
                        responsecustomersearch = customersearch.Execute(requestecustomersearch);
                        valoretorno = "0";
                        valoretorno = funciones.descomponer("id", responsecustomersearch.Content);
                        if (valoretorno != "0")
                        {
                            customer = new RestClient(rutacustomerAdd + valoretorno + "/ ");
                            requeste = new RestRequest("",Method.Patch);
                            textocustomer = Utilidades.SerializarCustomer(monitoreo,  valoretorno, accountType);
                            requeste.AddHeader("content-type", "application/json");
                            requeste.AddHeader("authorization", "HunterAPI " + token);
                            requeste.AddParameter("application/json", textocustomer, ParameterType.RequestBody);
                            responsecustomer = customer.Execute(requeste);
                            if (Convert.ToInt32(responsecustomer.StatusCode) == 200)
                            {
                                customerID = funciones.descomponer("id", responsecustomer.Content);
                                if (ownerID == customerID)
                                {
                                    tabla.Rows.Add("-1", "Verificar el mail del cliente propietario - monitoreo no pueden ser iguales");
                                    bandera = false;
                                }
                                else
                                {
                                    if (ownerID == "0")
                                        ownerID = customerID;
                                    bandera = true;
                                }
                            }
                            else if (Convert.ToInt32(responsecustomer.StatusCode) == 400)
                            {
                                string mensaje = funciones.DevuelveMensaje(responsecustomer.Content);
                                tabla.Rows.Add("-1", mensaje + " " + responsecustomer.StatusCode + " customer Actualiza");
                                bandera = false;
                            }
                            else
                            {
                                tabla.Rows.Add("-1", "Error al procesar customer " + responsecustomer.StatusCode + " customer Actualiza");
                                bandera = false;
                            }
                        }
                        else
                        {
                            // consulta si existe el customer si se encuentra activo
                            customersearch = new RestClient(rutacustomerAdd + "?username=" + monitoreo.username + "&is_active=" + true + "&customer__company_code=" + monitoreo.company_code + "&customer__identity_document_number=" + monitoreo.internal_identifier + " ");
                            requestecustomersearch = new RestRequest("", Method.Get);
                            requestecustomersearch.AddHeader("cache-control", "no-cache");
                            requestecustomersearch.AddHeader("authorization", "HunterAPI " + token);
                            responsecustomersearch = customersearch.Execute(requestecustomersearch);
                            valoretorno = "0";
                            valoretorno = funciones.descomponer("id", responsecustomersearch.Content);
                            if (valoretorno == "0")
                            {
                                customer = new RestClient(rutacustomer);
                                requeste = new RestRequest("", Method.Post);
                                requeste.AddHeader("cache-control", "no-cache");
                                requeste.AddHeader("authorization", "HunterAPI " + token);
                                requeste.AddParameter("username", monitoreo.username);
                                requeste.AddParameter("first_name", monitoreo.first_name);
                                requeste.AddParameter("last_name", monitoreo.last_name);
                                requeste.AddParameter("email", monitoreo.email);
                                requeste.AddParameter("customer.company_code", monitoreo.company_code);
                                requeste.AddParameter("customer.identity_document_type", monitoreo.identity_customer_type);
                                requeste.AddParameter("customer.identity_document_number", monitoreo.internal_identifier);
                                requeste.AddParameter("customer.business_name", monitoreo.business_name);
                                requeste.AddParameter("customer.phone_number", propietario.telefono);
                                requeste.AddParameter("customer.emergency_phone_number", monitoreo.emergency_phone_number);
                                requeste.AddParameter("customer.assistance_phone_number", monitoreo.assistance_phone_number);
                                requeste.AddParameter("customer.technical_support_email", monitoreo.technical_support_email);
                                requeste.AddParameter("customer.account_type", accountType);
                                requeste.AddParameter("is_active", monitoreo.active);
                                responsecustomer = customer.Execute(requeste);
                                if (Convert.ToInt32(responsecustomer.StatusCode) == 201)
                                {
                                    customerID = funciones.descomponer("id", responsecustomer.Content);
                                    if (ownerID == customerID)
                                    {
                                        tabla.Rows.Add("-1", "Verificar el mail del cliente propietario - monitoreo no pueden ser iguales");
                                        bandera = false;
                                    }
                                    else
                                    {
                                        if (ownerID == "0")
                                            ownerID = customerID;
                                        bandera = true;
                                    }
                                }
                                else if (Convert.ToInt32(responsecustomer.StatusCode) == 400)
                                {
                                    string mensaje = funciones.DevuelveMensaje(responsecustomer.Content);
                                    tabla.Rows.Add("-1", mensaje + " " + responsecustomer.StatusCode + " customer");
                                    bandera = false;
                                }
                                else
                                {
                                    tabla.Rows.Add("-1", "Error al procesar customer " + responsecustomer.StatusCode + " customer");
                                    bandera = false;
                                }
                            }
                            else
                            {
                                customersearch = new RestClient(rutacustomerAdd + "?username=" + monitoreo.username + "&customer__company_code=" + monitoreo.company_code + "&customer__identity_document_number=" + monitoreo.internal_identifier + " ");
                                requestecustomersearch = new RestRequest("", Method.Get);
                                requestecustomersearch.AddHeader("cache-control", "no-cache");
                                requestecustomersearch.AddHeader("authorization", "HunterAPI " + token);
                                responsecustomersearch = customersearch.Execute(requestecustomersearch);
                                valoretorno = "0";
                                valoretorno = funciones.descomponer("id", responsecustomersearch.Content);
                                if (valoretorno != "0")
                                {
                                    customer = new RestClient(rutacustomerAdd + valoretorno + "/ ");
                                    requeste = new RestRequest("", Method.Patch);
                                    textocustomer = Utilidades.SerializarCustomer(monitoreo,  valoretorno, accountType);
                                    requeste.AddHeader("content-type", "application/json");
                                    requeste.AddHeader("authorization", "HunterAPI " + token);
                                    requeste.AddParameter("application/json", textocustomer, ParameterType.RequestBody);
                                    responsecustomer = customer.Execute(requeste);
                                    if (Convert.ToInt32(responsecustomer.StatusCode) == 200)
                                    {
                                        customerID = funciones.descomponer("id", responsecustomer.Content);
                                        if (ownerID == customerID)
                                        {
                                            tabla.Rows.Add("-1", "Verificar el mail del cliente propietario - monitoreo no pueden ser iguales");
                                            bandera = false;
                                        }
                                        else
                                        {
                                            if (ownerID == "0")
                                                ownerID = customerID;
                                            bandera = true;
                                        }
                                    }
                                    else if (Convert.ToInt32(responsecustomer.StatusCode) == 400)
                                    {
                                        string mensaje = funciones.DevuelveMensaje(responsecustomer.Content);
                                        tabla.Rows.Add("-1", mensaje + " " + responsecustomer.StatusCode + " customer Actualiza");
                                        bandera = false;
                                    }
                                    else
                                    {
                                        tabla.Rows.Add("-1", "Error al procesar customer " + responsecustomer.StatusCode + " customer Actualiza");
                                        bandera = false;
                                    }
                                }
                                else
                                {
                                    tabla.Rows.Add("-1", "User Name duplicado customer Actualiza");
                                    bandera = false;
                                }
                            }
                        }
                    }
                
                }
                
                // bloque del device
                if (bandera)
                {
                    var devicesearch = new RestClient();
                    var requestedevicesearch = new RestRequest();
                    RestResponse responsedevicesearch;
                    var clientedevices = new RestClient();
                    var requestedevice = new RestRequest();
                    RestResponse responsedevice;
                    // consulta si existe el device y si se encuentra inactivo
                    devicesearch = new RestClient(rutadeviceAdd + "?active=" + false + "&id=" + device.vid + "&company_code=" + device.company_code + " ");
                    requestedevicesearch = new RestRequest("",Method.Get);
                    requestedevicesearch.AddHeader("cache-control", "no-cache");
                    requestedevicesearch.AddHeader("authorization", "HunterAPI " + token);
                    responsedevicesearch = devicesearch.Execute(requestedevicesearch);
                    valoretorno = "0";
                    valoretorno = funciones.descomponer("id", responsedevicesearch.Content);
                    if (valoretorno != "0")
                    {
                        clientedevices = new RestClient(rutadeviceAdd + valoretorno + "/ ");
                        requestedevice = new RestRequest("",Method.Patch);
                        requestedevice.AddHeader("cache-control", "no-cache");
                        requestedevice.AddHeader("authorization", "HunterAPI " + token);
                        requestedevice.AddParameter("report_from", device.report_from);
                        requestedevice.AddParameter("model", device.model);
                        requestedevice.AddParameter("company_code", device.company_code);
                        requestedevice.AddParameter("id", device.vid);
                        requestedevice.AddParameter("active", device.active);
                        responsedevice = clientedevices.Execute(requestedevice);
                        if (Convert.ToInt32(responsedevice.StatusCode) == 200)
                            bandera = true;
                        else if (Convert.ToInt32(responsedevice.StatusCode) == 400)
                        {
                            string mensaje = funciones.DevuelveMensaje(responsedevice.Content);
                            tabla.Rows.Add("-1", mensaje + " " + responsedevice.StatusCode + " device Actualiza");
                            bandera = false;
                        }
                        else
                        {
                            tabla.Rows.Add("-1", "Error al procesar devices " + responsedevice.StatusCode + " device Actualiza");
                            bandera = false;
                        }
                    }
                    else
                    {
                        devicesearch = new RestClient(rutadeviceAdd + "?active=" + device.active + "&id=" + device.vid + "&company_code=" + device.company_code + " ");
                        requestedevicesearch = new RestRequest("",Method.Get);
                        requestedevicesearch.AddHeader("cache-control", "no-cache");
                        requestedevicesearch.AddHeader("authorization", "HunterAPI " + token);
                        responsedevicesearch = devicesearch.Execute(requestedevicesearch);
                        valoretorno = "0";
                        valoretorno = funciones.descomponer("id", responsedevicesearch.Content);
                        if (valoretorno == "0")
                        {
                            clientedevices = new RestClient(rutadevice);
                            requestedevice = new RestRequest("", Method.Post);
                            requestedevice.AddHeader("cache-control", "no-cache");
                            requestedevice.AddHeader("authorization", "HunterAPI " + token);
                            requestedevice.AddParameter("report_from", device.report_from);
                            requestedevice.AddParameter("model", device.model);
                            requestedevice.AddParameter("company_code", device.company_code);
                            requestedevice.AddParameter("id", device.vid);
                            requestedevice.AddParameter("active", device.active);
                            responsedevice = clientedevices.Execute(requestedevice);
                            if (Convert.ToInt32(responsedevice.StatusCode) == 201)
                                bandera = true;
                            else if (Convert.ToInt32(responsedevice.StatusCode) == 400)
                            {
                                string mensaje = funciones.DevuelveMensaje(responsedevice.Content);
                                tabla.Rows.Add("-1", mensaje + " " + responsedevice.StatusCode + " device");
                                bandera = false;
                            }
                            else
                            {
                                tabla.Rows.Add("-1", "Error al procesar devices " + responsedevice.StatusCode + " device");
                                bandera = false;
                            }
                        }
                        else
                            bandera = true;
                    }
                }
                // bloque del vehiculo
                if (bandera)
                {
                    var assetSearch = new RestClient();
                    var requesteAssetSearch = new RestRequest();
                    RestResponse responseAssetSearch;
                    string texto;
                    var clienteasset = new RestClient();
                    var requesteasset = new RestRequest();
                    RestResponse responseasset;
                    assetSearch = new RestClient(rutaassetAdd + "?attributes__attribute__name=COD_SYS" + "&attributes__value=" + asset.codigosys + " ");
                    requesteAssetSearch = new RestRequest("", Method.Get);
                    requesteAssetSearch.AddHeader("cache-control", "no-cache");
                    requesteAssetSearch.AddHeader("authorization", "HunterAPI " + token);
                    responseAssetSearch = assetSearch.Execute(requesteAssetSearch);
                    valoretorno = "0";
                    valoretorno = funciones.descomponer("id", responseAssetSearch.Content);
                    if (valoretorno != "0")
                    {
                        texto = Utilidades.SerializarAssetInstalacion(asset, attributeEntidadCollection, valoretorno, ownerID, p.sensors);
                        clienteasset = new RestClient(rutaassetAdd + valoretorno + "/ ");
                        requesteasset = new RestRequest("", Method.Patch);
                        requesteasset.AddHeader("content-type", "application/json");
                        requesteasset.AddHeader("authorization", "HunterAPI " + token);
                        requesteasset.AddParameter("application/json", texto, ParameterType.RequestBody);
                        responseasset = clienteasset.Execute(requesteasset);
                        if (Convert.ToInt32(responseasset.StatusCode) == 200 )
                        {
                            assetID = funciones.descomponer("id", responseasset.Content);
                            bandera = true;
                        }
                        else if (Convert.ToInt32(responseasset.StatusCode) == 400)
                        {
                            string mensaje = funciones.DevuelveMensaje(responseasset.Content);
                            tabla.Rows.Add("-1", mensaje + " " + responseasset.StatusCode + " asset Actualiza");
                            bandera = false;
                        }
                        else
                        {
                            tabla.Rows.Add("-1", "Error al procesar asset " + responseasset.StatusCode + " asset Actualiza");
                            bandera = false;
                        }
                    }
                    else
                    {
                        // consulta si existe el asset y si esta activo
                        assetSearch = new RestClient(rutaassetAdd + "?attributes__attribute__name=COD_SYS" + "&attributes__value=" + asset.codigosys + " ");
                        requesteAssetSearch = new RestRequest("", Method.Get);
                        requesteAssetSearch.AddHeader("cache-control", "no-cache");
                        requesteAssetSearch.AddHeader("authorization", "HunterAPI " + token);
                        responseAssetSearch = assetSearch.Execute(requesteAssetSearch);
                        valoretorno = "0";
                        valoretorno = funciones.descomponer("id", responseAssetSearch.Content);
                        if (valoretorno == "0")
                        {
                            texto = Utilidades.SerializarAssetInstalacion(asset, attributeEntidadCollection, valoretorno, customerID, p.sensors);
                            clienteasset = new RestClient(rutaasset);
                            requesteasset = new RestRequest("", Method.Post);
                            requesteasset.AddHeader("content-type", "application/json");
                            requesteasset.AddHeader("authorization", "HunterAPI " + token);
                            requesteasset.AddParameter("application/json", texto, ParameterType.RequestBody);
                            responseasset = clienteasset.Execute(requesteasset);
                            if (Convert.ToInt32(responseasset.StatusCode) == 201)
                            {
                                assetID = funciones.descomponer("id", responseasset.Content);
                                bandera = true;
                            }
                            else if (Convert.ToInt32(responseasset.StatusCode) == 400)
                            {
                                string mensaje = funciones.DevuelveMensaje(responseasset.Content);
                                tabla.Rows.Add("-1", mensaje + " " + responseasset.StatusCode + " asset");
                                bandera = false;
                            }
                            else
                            {
                                tabla.Rows.Add("-1", "Error al procesar asset " + responseasset.StatusCode + " asset");
                                bandera = false;
                            }
                        }
                        else
                        {
                            assetID = valoretorno;
                            bandera = true;
                        }
                    }
                }
                // bloque  de  vehiculo - usuario
                if (bandera)
                {
                    // consulta si existe la relacion vehiculo - usuario
                    var assetUserSearch = new RestClient(rutaassetAdd + "?user__email=" + monitoreo.email + "&attributes__attribute__name=COD_SYS" + "&attributes__value=" + asset.codigosys + " ");
                    var requesteassetUserSearch = new RestRequest("",Method.Get);
                    requesteassetUserSearch.AddHeader("cache-control", "no-cache");
                    requesteassetUserSearch.AddHeader("authorization", "HunterAPI " + token);
                    RestResponse responseUserSearch = assetUserSearch.Execute(requesteassetUserSearch);
                    valoretorno = "0";
                    valoretorno = funciones.descomponer("id", responseUserSearch.Content);
                    if (valoretorno == "0")
                    {
                        var clienteassetuser = new RestClient(rutaassetAdd + assetID + "/user-add/ ");
                        var requesteassetuser = new RestRequest("", Method.Patch);
                        requesteassetuser.AddHeader("cache-control", "no-cache");
                        requesteassetuser.AddHeader("authorization", "HunterAPI " + token);
                        requesteassetuser.AddParameter("id", assetID);
                        requesteassetuser.AddParameter("user_id", customerID);
                        RestResponse responseassetuser = clienteassetuser.Execute(requesteassetuser);
                        if (Convert.ToInt32(responseassetuser.StatusCode) == 200)
                            bandera = true;
                        else if (Convert.ToInt32(responseassetuser.StatusCode) == 400)
                        {
                            string mensaje = funciones.DevuelveMensaje(responseassetuser.Content);
                            tabla.Rows.Add("-1", mensaje + " " + responseassetuser.StatusCode + " asset-usuario customerID");
                            bandera = false;
                        }
                        else
                        {
                            tabla.Rows.Add("-1", "Error al procesar assetuser " + responseassetuser.StatusCode + " asset-usuario customerID");
                            bandera = false;
                        }
                    }
                    else
                        bandera = true;
                        // se agrega el usuario propietario
                        if (ownerID != customerID)
                        {
                            var assetUserPropieSearch = new RestClient(rutaassetAdd + "?user__email=" + propietario.email + "&attributes__attribute__name=COD_SYS" + "&attributes__value=" + asset.codigosys + " ");
                            var requesteassetUserPropieSearch = new RestRequest("", Method.Get);
                            requesteassetUserPropieSearch.AddHeader("cache-control", "no-cache");
                            requesteassetUserPropieSearch.AddHeader("authorization", "HunterAPI " + token);
                            RestResponse responseUserPropieSearch = assetUserPropieSearch.Execute(requesteassetUserPropieSearch);
                            valoretorno = "0";
                            valoretorno = funciones.descomponer("id", responseUserPropieSearch.Content);
                            if (valoretorno == "0")
                            {
                                var clienteassetuser = new RestClient(rutaassetAdd + assetID + "/user-add/ ");
                                var requesteassetuser = new RestRequest("", Method.Patch);
                                requesteassetuser.AddHeader("cache-control", "no-cache");
                                requesteassetuser.AddHeader("authorization", "HunterAPI " + token);
                                requesteassetuser.AddParameter("id", assetID);
                                requesteassetuser.AddParameter("user_id", ownerID);
                                RestResponse responseassetuser = clienteassetuser.Execute(requesteassetuser);
                                if (Convert.ToInt32(responseassetuser.StatusCode) == 200)
                                    bandera = true;
                                else if (Convert.ToInt32(responseassetuser.StatusCode) == 400)
                                {
                                    string mensaje = funciones.DevuelveMensaje(responseassetuser.Content);
                                    tabla.Rows.Add("-1", mensaje + " " + responseassetuser.StatusCode + " asset-usuario ownerID");
                                    bandera = false;
                                }
                                else
                                {
                                    tabla.Rows.Add("-1", "Error al procesar assetuser " + responseassetuser.StatusCode + " asset-usuario ownerID");
                                    bandera = false;
                                }
                            }
                            else
                                bandera = true;
                        }
                }
               
                // bloque  de  vehiculo - device
                if (bandera)
                {
                    // consulta si existe la relacion vehiculo - device
                    var assetDevSearch = new RestClient(rutaassetAdd + "?devices__device__id=" + device.vid + "&attributes__attribute__name=COD_SYS" + "&attributes__value=" + asset.codigosys + " ");
                    var requesteassetDevSearch = new RestRequest("",Method.Get);
                    requesteassetDevSearch.AddHeader("cache-control", "no-cache");
                    requesteassetDevSearch.AddHeader("authorization", "HunterAPI " + token);
                    RestResponse responseassetDevSearch = assetDevSearch.Execute(requesteassetDevSearch);
                    valoretorno = "0";
                    valoretorno = funciones.descomponer("id", responseassetDevSearch.Content);
                    if (valoretorno == "0")
                    {
                        var clienteassetdev = new RestClient(rutaassetAdd + assetID + "/device-add/ ");
                        var requesteassetdev = new RestRequest("", Method.Patch);
                        requesteassetdev.AddHeader("cache-control", "no-cache");
                        requesteassetdev.AddHeader("authorization", "HunterAPI " + token);
                        requesteassetdev.AddParameter("id", assetID);
                        requesteassetdev.AddParameter("device_id", device.vid);
                        RestResponse responseassetdev = clienteassetdev.Execute(requesteassetdev);
                        if (Convert.ToInt32(responseassetdev.StatusCode) == 200)
                            bandera = true;
                        else if (Convert.ToInt32(responseassetdev.StatusCode) == 400)
                        {
                            tabla.Rows.Add("-1",  responseassetdev.StatusCode + " asset-device");
                            bandera = false;
                        }
                        else
                        {
                            tabla.Rows.Add("-1", "Error al procesar assetdevice " + responseassetdev.StatusCode + " asset-device");
                            bandera = false;
                        }
                    }
                    else
                        bandera = true;
                }
                // bloque  de  vehiculo - comando
                if (bandera)
                {
                    if (p.comando != "")
                    {
                        var commandSearch = new RestClient();
                        var requesteCommandSearch = new RestRequest();
                        RestResponse responseCommandSearch;
                        var clientecommand = new RestClient();
                        var requestecommand = new RestRequest();
                        RestResponse responsecommand;
                        foreach (var comando in comandos)
                        {
                            commandSearch = new RestClient(rutacommandAdd + "?asset=" + assetID + "&command=" + comando + "&status=" + 0 + " ");
                            requesteCommandSearch = new RestRequest("", Method.Get);
                            requesteCommandSearch.AddHeader("cache-control", "no-cache");
                            requesteCommandSearch.AddHeader("authorization", "HunterAPI " + token);
                            responseCommandSearch = commandSearch.Execute(requesteCommandSearch);
                            valoretorno = "0";
                            valoretorno = funciones.descomponer("id", responseCommandSearch.Content);
                            if (valoretorno != "0")
                            {
                                clientecommand = new RestClient(rutacommandAdd + valoretorno + "/ ");
                                requestecommand = new RestRequest("", Method.Patch);
                                requestecommand.AddHeader("cache-control", "no-cache");
                                requestecommand.AddHeader("authorization", "HunterAPI " + token);
                                requestecommand.AddParameter("status", 1);
                                requestecommand.AddParameter("id", valoretorno);
                                requestecommand.AddParameter("command", comando);
                                requestecommand.AddParameter("asset", assetID);
                                responsecommand = clientecommand.Execute(requestecommand);
                                if (Convert.ToInt32(responsecommand.StatusCode) == 200)
                                    bandera = true;
                                else if (Convert.ToInt32(responsecommand.StatusCode) == 400)
                                {
                                    string mensaje = funciones.DevuelveMensaje(responsecommand.Content);
                                    tabla.Rows.Add("-1", mensaje + " " + responsecommand.StatusCode + " asset-comando Actualiza");
                                    bandera = false;
                                    break;
                                }
                                else
                                {
                                    tabla.Rows.Add("-1", "Error al procesar asset command " + responsecommand.StatusCode + " asset-comando Actualiza");
                                    bandera = false;
                                    break;
                                }
                            }
                            else
                            {
                                // consulta si existe la relacion vehiculo - comando - activos
                                commandSearch = new RestClient(rutacommandAdd + "?asset=" + assetID + "&command=" + comando + "&status=" + 1 + " ");
                                requesteCommandSearch = new RestRequest("", Method.Get);
                                requesteCommandSearch.AddHeader("cache-control", "no-cache");
                                requesteCommandSearch.AddHeader("authorization", "HunterAPI " + token);
                                responseCommandSearch = commandSearch.Execute(requesteCommandSearch);
                                valoretorno = "0";
                                valoretorno = funciones.descomponer("id", responseCommandSearch.Content);
                                if (valoretorno == "0")
                                {
                                    clientecommand = new RestClient(rutacommand);
                                    requestecommand = new RestRequest("", Method.Post);
                                    requestecommand.AddHeader("cache-control", "no-cache");
                                    requestecommand.AddHeader("authorization", "HunterAPI " + token);
                                    requestecommand.AddParameter("status", 1);
                                    requestecommand.AddParameter("command", comando);
                                    requestecommand.AddParameter("asset", assetID);
                                    responsecommand = clientecommand.Execute(requestecommand);
                                    if (Convert.ToInt32(responsecommand.StatusCode) == 201)
                                        bandera = true;
                                    else if (Convert.ToInt32(responsecommand.StatusCode)  == 400)
                                    {
                                        string mensaje = funciones.DevuelveMensaje(responsecommand.Content);
                                        tabla.Rows.Add("-1", mensaje + " " + responsecommand.StatusCode + " asset-comando");
                                        bandera = false;
                                        break;
                                    }
                                    else
                                    {
                                        tabla.Rows.Add("-1", "Error al procesar asset command " + responsecommand.StatusCode + " asset-comando");
                                        bandera = false;
                                        break;
                                    }
                                }
                                else
                                    bandera = true;
                            }
                        }
                    }
                }
                // bloque  de  comando
                if (bandera)
                {
                    if (p.comando != "")
                    {
                        var userCommandSearch = new RestClient();
                        var requesteuserCommandSearch = new RestRequest();
                        RestResponse responseuserCommandSearch;
                        var clienteusercommand = new RestClient();
                        var requesteusercommand = new RestRequest();
                        RestResponse responseusercommand;
                        foreach (var comando in comandos)
                        {
                            // consulta si existe el comando por vehiculo - comando - usuario - estado falso
                            userCommandSearch = new RestClient(rutausercommandAdd + "?asset=" + assetID + "&command=" + comando + "&user=" + customerID + "&can_execute=" + false + " ");
                            requesteuserCommandSearch = new RestRequest("", Method.Get);
                            requesteuserCommandSearch.AddHeader("cache-control", "no-cache");
                            requesteuserCommandSearch.AddHeader("authorization", "HunterAPI " + token);
                            responseuserCommandSearch = userCommandSearch.Execute(requesteuserCommandSearch);
                            valoretorno = "0";
                            valoretorno = funciones.descomponer("id", responseuserCommandSearch.Content);
                            if (valoretorno != "0")
                            {
                                clienteusercommand = new RestClient(rutausercommandAdd + valoretorno + "/ ");
                                requesteusercommand = new RestRequest("", Method.Patch);
                                requesteusercommand.AddHeader("cache-control", "no-cache");
                                requesteusercommand.AddHeader("authorization", "HunterAPI " + token);
                                requesteusercommand.AddParameter("id", valoretorno);
                                requesteusercommand.AddParameter("command", comando);
                                requesteusercommand.AddParameter("user", customerID);
                                requesteusercommand.AddParameter("asset", assetID);
                                requesteusercommand.AddParameter("can_execute", true);
                                responseusercommand = clienteusercommand.Execute(requesteusercommand);
                                if (Convert.ToInt32(responseusercommand.StatusCode) == 200)
                                    bandera = true;
                                else if (Convert.ToInt32(responseusercommand.StatusCode) == 400)
                                {
                                    string mensaje = funciones.DevuelveMensaje(responseusercommand.Content);
                                    tabla.Rows.Add("-1", mensaje + " " + responseusercommand.StatusCode + " comando-usuario Actualiza");
                                    bandera = false;
                                    break;
                                }
                                else
                                {
                                    tabla.Rows.Add("-1", "Error al procesar user asset command " + responseusercommand.StatusCode + " comando-usuario Actualiza");
                                    bandera = false;
                                    break;
                                }
                            }
                            else
                            {
                                // consulta si existe el comando por vehiculo - comando - usuario - estado
                                userCommandSearch = new RestClient(rutausercommandAdd + "?asset=" + assetID + "&command=" + comando + "&user=" + customerID + "&can_execute=" + true + " ");
                                requesteuserCommandSearch = new RestRequest("", Method.Get);
                                requesteuserCommandSearch.AddHeader("cache-control", "no-cache");
                                requesteuserCommandSearch.AddHeader("authorization", "HunterAPI " + token);
                                responseuserCommandSearch = userCommandSearch.Execute(requesteuserCommandSearch);
                                valoretorno = "0";
                                valoretorno = funciones.descomponer("id", responseuserCommandSearch.Content);
                                if (valoretorno == "0")
                                {
                                    clienteusercommand = new RestClient(rutausercommand);
                                    requesteusercommand = new RestRequest("", Method.Post);
                                    requesteusercommand.AddHeader("cache-control", "no-cache");
                                    requesteusercommand.AddHeader("authorization", "HunterAPI " + token);
                                    requesteusercommand.AddParameter("command", comando);
                                    requesteusercommand.AddParameter("user", customerID);
                                    requesteusercommand.AddParameter("asset", assetID);
                                    requesteusercommand.AddParameter("can_execute", true);
                                    responseusercommand = clienteusercommand.Execute(requesteusercommand);
                                    if (Convert.ToInt32(responseusercommand.StatusCode) == 201)
                                        bandera = true;
                                    else if (Convert.ToInt32(responseusercommand.StatusCode) == 400)
                                    {
                                        string mensaje = funciones.DevuelveMensaje(responseusercommand.Content);
                                        tabla.Rows.Add("-1", mensaje + " " + responseusercommand.StatusCode + " comando-usuario");
                                        bandera = false;
                                        break;
                                    }
                                    else
                                    {
                                        tabla.Rows.Add("-1", "Error al procesar user asset command " + responseusercommand.StatusCode + " comando-usuario");
                                        bandera = false;
                                        break;
                                    }
                                }
                                else
                                    bandera = true;
                            }
                        }
                    }
                }
            }

            // ingreso del log del proceso de AMI
            if (bandera)
            {
                string resultadoLog = ConsultaAMI.RegistroLog(p.codigocliente, ownerID, p.clientemonitoreo, customerID, p.codigosys, assetID, gencomando, p.serie, producto, p.ordenservicio, "0", p.usuario, envio, usermonitoreo, tiporegistro, userpropietario, p.origen, accion, "", "", "", asociadoAmi, asociadoID, asociadousers, "", "", "", "", p.parametroproducto, p.cobertura, valorvid, p.producto, concesionario);
                if (resultadoLog == "OK")
                {
                    bandera = true;
                }
                else
                {
                    tabla.Rows.Add("-1", "Error en el Ingreso del Log");
                    bandera = false;
                }
            }
            string jsonData;
            if (bandera)
            {
                tabla.Rows.Add(1, "Guardado Exitosamente");
            }
            jsonData = (string)Utilidades.DataTableToJSON(tabla);

            return jsonData;

        }


        [HttpPost]
        [Authorize]
        [SwaggerOperation(Summary = "", Description = "Cambio de propietario hacia la plataforma AMI")]
        [Route("CambioPropietario")]
        public ActionResult<string> CambioPropietario(CambioPropietarioAMIDTO p)
        {
            // ruta ami 
            conexionAMI ruta = new conexionAMI();
            string rutalogin = "";
            string rutaassetAdd = "";
            string rutacustomerAdd = "";
            string rutacommandAdd = "";
            string rutausercommandAdd = "";
            string rutacustomer = "";
            string rutausercommand = "";
            // variables
            bool bandera = true;
            char delimiter = ',';
            string gencomando = "";
            string comando = "";
            string[] comandos = comando.Split(delimiter);
            string customerID = "";
            string assetID = "";
            string token = "";
            string valoretorno = "";
            string userpropietario = "";
            string accion = "";
            string envio = "";
            string ownerID = "0";
            string valorvid = "";
            string customeroldID = "0";
            string userold = "";
            string cobertura = "";
            string producto = "";
            DataSet infoDatos = new DataSet();
            string log_usr = "";
            string log_pwd = "";
            string concesionario = "";
            Int32 status = 0;
            bool execute = true;
            DataTable tabla = new DataTable();
            tabla.Columns.Add("CODIGO", typeof(int));
            tabla.Columns.Add("MENSAJE", typeof(string));
            // busqueda de ruta por el parametro
            if (p.parametroproducto == "" | string.IsNullOrEmpty(p.parametroproducto))
                p.parametroproducto = "GPT";
            if (p.parametroproducto == "GPT")
            {
                rutalogin = ruta.GetRuta("1");
                rutaassetAdd = ruta.GetRuta("2");
                rutacustomerAdd = ruta.GetRuta("3");
                rutacommandAdd = ruta.GetRuta("5");
                rutausercommandAdd = ruta.GetRuta("6");
                rutacustomer = ruta.GetRuta("13");
                rutausercommand = ruta.GetRuta("16");
            }
            else if (p.parametroproducto == "GPA")
            {
                rutalogin = ruta.GetRuta("41");
                rutaassetAdd = ruta.GetRuta("42");
                rutacustomerAdd = ruta.GetRuta("43");
                rutacommandAdd = ruta.GetRuta("45");
                rutausercommandAdd = ruta.GetRuta("46");
                rutacustomer = ruta.GetRuta("53");
                rutausercommand = ruta.GetRuta("56");
            }
            else
            {
                bandera = false;
                tabla.Rows.Add("-1", "No existe el parametro del producto");
            }
                   
            // PROCESO DE AMI Y LATAM
            if (bandera)
            {
                customerEntidad monitoreo = null;
                customerEntidad propietario = null;
                customerEntidad clientenuevo = null;
                customerEntidad clienteanterior = null;
                //customer cliente = null;
                deviceEntidad device = null;
                assetEntidad asset = null;
                attributeEntidadCollection attributeEntidadCollection = null;
                attributeEntidad attributeCollection = null;
                string monitoreoAMI = "";
                string clientenuevoAMI = "";
                string clienteanteriorAMI = "";
                //string clienteAMI = "";
                string propietarioAMI = "";
                string deviceAMI = "";
                string assetAMI = "";
                // datos
                if (bandera)
                {
                    DataSet cnstGenrl = new DataSet();
                    cnstGenrl = BienSQL.CnstVehiculo(opcion: "100", cliente: "", vehiculo: "", parametroproducto: "GPT");
                    string support = (string)cnstGenrl.Tables[0].Rows[0]["support"];
                    log_usr = (string)cnstGenrl.Tables[0].Rows[0]["usuario"];
                    log_pwd = (string)cnstGenrl.Tables[0].Rows[0]["pass"];
                    string ami_telefono = (string)cnstGenrl.Tables[0].Rows[0]["telefono"];
                    string ami_celular = (string)cnstGenrl.Tables[0].Rows[0]["celular"];
                    string ruccompania = (string)cnstGenrl.Tables[0].Rows[0]["ruccompania"];
                    //monitoreo
                    if (bandera)
                    {
                        // informacion del cliente Monitoreo
                        monitoreoAMI = ConsultaAMI.ProcesoAMIConsulta("204", p.codigovehiculo, p.producto, p.ordenservicio, p.itemNS, "");
                        accion = funciones.descomponer("ACCION", monitoreoAMI);
                        // VERIFICA SI HAY QUE ENVIAR DE NUEVO EL EMAIL DE ACTIVACION 
                        envio = "N";
                        userpropietario = funciones.descomponer("USERNAME", monitoreoAMI);
                        monitoreo = new customerEntidad
                        {
                            username = funciones.descomponer("USERNAME", monitoreoAMI),
                            first_name = funciones.descomponer("FIRST_NAME", monitoreoAMI),
                            last_name = funciones.descomponer("LAST_NAME", monitoreoAMI),
                            email = funciones.descomponer("EMAIL", monitoreoAMI),
                            login = log_usr,
                            password = log_pwd,
                            internal_identifier = funciones.descomponer("ID_CLIENTE", monitoreoAMI),
                            active = "True",
                            telefono = funciones.descomponer("TELEFONO_CLIENTE", monitoreoAMI),
                            identity_customer_type = funciones.descomponer("TIPO", monitoreoAMI),
                            company_code = ruccompania,
                            business_name = "",
                            emergency_phone_number = ami_telefono,
                            assistance_phone_number = ami_celular,
                            technical_support_email = support,
                        };
                    }
                    //cliente nuevo
                    if (bandera)
                    {
                        // informacion del cliente nuevo
                        clientenuevoAMI = ConsultaAMI.ProcesoAMIConsulta("204", p.codigovehiculo, p.producto, p.ordenservicio, p.itemNS, "");
                        clientenuevo = new customerEntidad
                        {
                            username = funciones.descomponer("USERNAME", clientenuevoAMI),
                            first_name = funciones.descomponer("FIRST_NAME", clientenuevoAMI),
                            last_name = funciones.descomponer("LAST_NAME", clientenuevoAMI),
                            email = funciones.descomponer("EMAIL", clientenuevoAMI),
                            login = log_usr,
                            password = log_pwd,
                            internal_identifier = funciones.descomponer("ID_CLIENTE", clientenuevoAMI),
                            active = "True",
                            telefono = funciones.descomponer("TELEFONO_CLIENTE", clientenuevoAMI),
                            identity_customer_type = funciones.descomponer("TIPO", clientenuevoAMI),
                            company_code = ruccompania,
                            business_name = "",
                            emergency_phone_number = ami_telefono,
                            assistance_phone_number = ami_celular,
                            technical_support_email = support,
                        };
                    }
                    //cliente anterior
                    if (bandera)
                    {
                        clienteanteriorAMI = ConsultaAMI.ProcesoAMIConsulta("205", p.codigovehiculo, p.producto, p.ordenservicio, p.itemNS, "");
                        // VERIFICA SI HAY QUE ENVIAR DE NUEVO EL EMAIL DE ACTIVACION 
                        //userold = "jrodriguez@uejavier.com";
                        userold = funciones.descomponer("USERNAME", clienteanteriorAMI);
                        clienteanterior = new customerEntidad
                        {
                            //username = "jrodriguez@uejavier.com",
                            username = funciones.descomponer("USERNAME", clienteanteriorAMI),
                            first_name = funciones.descomponer("FIRST_NAME", clienteanteriorAMI),
                            last_name = funciones.descomponer("LAST_NAME", clienteanteriorAMI),
                            email = funciones.descomponer("EMAIL", clienteanteriorAMI),
                            login = log_usr,
                            password = log_pwd,
                            internal_identifier = funciones.descomponer("ID_CLIENTE", clienteanteriorAMI),
                            active = "True",
                            telefono = funciones.descomponer("TELEFONO_CLIENTE", clienteanteriorAMI),
                            identity_customer_type = funciones.descomponer("TIPO", clienteanteriorAMI),
                            company_code = ruccompania,
                            business_name = "",
                            emergency_phone_number = ami_telefono,
                            assistance_phone_number = ami_celular,
                            technical_support_email = support,
                        };
                    }
                    //device
                    if (bandera)
                    {
                        deviceAMI = ConsultaAMI.ProcesoAMIConsulta("202", p.codigovehiculo, p.producto, p.ordenservicio, p.itemNS, "");
                        valorvid = funciones.descomponer("vid", deviceAMI);
                        device = new deviceEntidad
                        {
                            id = funciones.descomponer("serie", deviceAMI),
                            vid = funciones.descomponer("vid", deviceAMI),
                            company_code = ruccompania,
                            active = "True",
                            model = funciones.descomponer("id_telematic_modelo", deviceAMI),
                            report_from = funciones.descomponer("id_telematic_servidor", deviceAMI),
                            numero_celular = funciones.descomponer("noCelularSIM", deviceAMI),
                        };

                        if (valorvid == "0")
                        {
                            tabla.Rows.Add("-1", "Verificar el valor del VID ");
                            bandera = false;
                        }
                        if (device.report_from == "")
                        {
                            tabla.Rows.Add("-1", "Verificar el Servidor del Dispositivo ");
                            bandera = false;
                        }
                        if (device.model == "")
                        {
                            tabla.Rows.Add("-1", "Verificar el Modelo del Dispositivo ");
                            bandera = false;
                        }
                    }

                    if (bandera)
                    {
                        // informacion del vehiculo
                        assetAMI = ConsultaAMI.ProcesoAMIConsulta("203", p.codigovehiculo, p.producto, p.ordenservicio, p.itemNS, "");
                        concesionario = funciones.descomponer("concesionarios", assetAMI);   
                        comando= funciones.descomponer("comandoami", assetAMI);
                        gencomando = funciones.descomponer("comandoami", assetAMI);
                        comandos = gencomando.Split(delimiter);
                        asset = new assetEntidad
                        {
                            name = funciones.descomponer("placa", assetAMI),
                            assettype = funciones.descomponer("tipo", assetAMI),
                            codigosys = p.codigosys, //CODIGO SYS O NETSUITE
                            codigonetsuite = p.codigovehiculo,
                            customname = "",
                            active = "True",
                            description = "PLC.:" + funciones.descomponer("placa", assetAMI) + ";MAR.:" + funciones.descomponer("marca", assetAMI) + ";MOD.:" + funciones.descomponer("modelo", assetAMI) + ";COL.:" + funciones.descomponer("color", assetAMI),
                            //productexpiredate = p.cobertura,
                            // product = producto,
                            productexpiredate = "",
                            product ="",
                            chasis = funciones.descomponer("chasis", assetAMI),
                        };
                        //informacion del vehiculo atributos
                        cnstGenrl = BienSQL.CnstVehiculoParametros(opcion: "103", cliente: "", vehiculo: p.codigosys,
                                                                   parametroproducto: "GPT", tipo: funciones.descomponer("tipo", assetAMI), marca: funciones.descomponer("marca", assetAMI),
                                                                   modelo: funciones.descomponer("modelo", assetAMI), chasis: funciones.descomponer("chasis", assetAMI), motor: funciones.descomponer("motor", assetAMI),
                                                                   placa: funciones.descomponer("placa", assetAMI), color: funciones.descomponer("color", assetAMI), anio: funciones.descomponer("anio", assetAMI));
                        if (cnstGenrl.Tables.Count > 0)
                        {
                            attributeEntidadCollection = new attributeEntidadCollection();
                            attributeCollection = new attributeEntidad();
                            for (int i = 0; i <= cnstGenrl.Tables[0].Rows.Count - 1; i++)
                            {
                                attributeEntidad attribute = new attributeEntidad();
                                attribute.AttributeId = (int)cnstGenrl.Tables[0].Rows[i]["ID"];
                                attribute.Valor = (string)cnstGenrl.Tables[0].Rows[i]["VALOR"];
                                attributeEntidadCollection.Add(attribute);
                            }
                            attributeCollection.attributeEntidadCollection = attributeEntidadCollection;
                        }
                    }
                }
                //login
                if (bandera)
                {
                    var client = new RestClient(rutalogin);
                    var request = new RestRequest("", Method.Post);
                    request.AddParameter("username", monitoreo.login);
                    request.AddParameter("password", monitoreo.password);
                    //IRestResponse response = client.Execute(request);
                    //token = funciones.Desencriptar(response.Content, "token");
                    RestResponse response = client.Execute(request);
                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(response.Content);
                    token = funciones.descomponer("token", jsonString);
                    if (accion=="010")
                    {
                        status = 1;
                        execute = true;
                    }
                    if (accion == "002")
                    {
                        status = 0;
                        execute = false;
                    }
                }                
                // bloque del cliente y cliente monitoreo
                if (bandera)
                {
                    var customersearch = new RestClient();
                    var requestecustomersearch = new RestRequest();
                    RestResponse responsecustomersearch;
                    var customer = new RestClient();
                    var requeste = new RestRequest();
                    RestResponse responsecustomer;
                    string textocustomer;
                    string accountType = "1";
                    if (p.codigocliente != p.clientemonitoreo)
                    {
                        // consulta si existe el customer si se encuentra inactivo
                        customersearch = new RestClient(rutacustomerAdd + "?username=" + propietario.username + "&is_active=" + false + "&customer__company_code=" + propietario.company_code + "&customer__identity_document_number=" + propietario.internal_identifier + " ");
                        requestecustomersearch = new RestRequest("", Method.Get);
                        requestecustomersearch.AddHeader("cache-control", "no-cache");
                        requestecustomersearch.AddHeader("authorization", "HunterAPI " + token);
                        responsecustomersearch = customersearch.Execute(requestecustomersearch);
                        valoretorno = "0";
                        valoretorno = funciones.descomponer("id", responsecustomersearch.Content);
                        if (valoretorno != "0")
                        {
                            customer = new RestClient(rutacustomerAdd + valoretorno + "/ ");
                            requeste = new RestRequest("", Method.Patch);
                            textocustomer = Utilidades.SerializarCustomer(propietario, valoretorno, accountType);
                            requeste.AddHeader("content-type", "application/json");
                            requeste.AddHeader("authorization", "HunterAPI " + token);
                            requeste.AddParameter("application/json", textocustomer, ParameterType.RequestBody);
                            responsecustomer = customer.Execute(requeste);
                            if (Convert.ToInt32(responsecustomer.StatusCode) == 200)
                            {
                                customerID = funciones.descomponer("id", responsecustomer.Content);
                                ownerID = customerID;
                                bandera = true;
                            }
                            else if (Convert.ToInt32(responsecustomer.StatusCode) == 400)
                            {
                                string mensaje = funciones.DevuelveMensaje(responsecustomer.Content);
                                tabla.Rows.Add("-1", mensaje + " " + responsecustomer.StatusCode + " customer Actualiza Owner");
                                bandera = false;
                            }
                            else
                            {
                                tabla.Rows.Add("-1", "Error al procesar customer " + responsecustomer.StatusCode + " customer Actualiza Owner");
                                bandera = false;
                            }
                        }
                        else
                        {
                            // consulta si existe el customer si se encuentra activo
                            customersearch = new RestClient(rutacustomerAdd + "?username=" + propietario.username + "&is_active=" + true + "&customer__company_code=" + propietario.company_code + "&customer__identity_document_number=" + propietario.internal_identifier + " ");
                            requestecustomersearch = new RestRequest("", Method.Get);
                            requestecustomersearch.AddHeader("cache-control", "no-cache");
                            requestecustomersearch.AddHeader("authorization", "HunterAPI " + token);
                            responsecustomersearch = customersearch.Execute(requestecustomersearch);
                            valoretorno = "0";
                            valoretorno = funciones.descomponer("id", responsecustomersearch.Content);
                            if (valoretorno == "0")
                            {
                                customer = new RestClient(rutacustomer);
                                requeste = new RestRequest("", Method.Post);
                                requeste.AddHeader("cache-control", "no-cache");
                                requeste.AddHeader("authorization", "HunterAPI " + token);
                                requeste.AddParameter("username", propietario.username);
                                requeste.AddParameter("first_name", propietario.first_name);
                                requeste.AddParameter("last_name", propietario.last_name);
                                requeste.AddParameter("email", propietario.email);
                                requeste.AddParameter("customer.company_code", propietario.company_code);
                                requeste.AddParameter("customer.identity_document_type", propietario.identity_customer_type);
                                requeste.AddParameter("customer.identity_document_number", propietario.internal_identifier);
                                requeste.AddParameter("customer.business_name", propietario.business_name);
                                requeste.AddParameter("customer.phone_number", propietario.telefono);
                                requeste.AddParameter("customer.emergency_phone_number", propietario.emergency_phone_number);
                                requeste.AddParameter("customer.assistance_phone_number", propietario.assistance_phone_number);
                                requeste.AddParameter("customer.technical_support_email", propietario.technical_support_email);
                                requeste.AddParameter("customer.account_type", accountType);
                                requeste.AddParameter("is_active", propietario.active);
                                responsecustomer = customer.Execute(requeste);
                                if (Convert.ToInt32(responsecustomer.StatusCode) == 201)
                                {
                                    customerID = funciones.descomponer("id", responsecustomer.Content);
                                    ownerID = customerID;
                                    bandera = true;
                                }
                                else if (Convert.ToInt32(responsecustomer.StatusCode) == 400)
                                {
                                    string mensaje = funciones.DevuelveMensaje(responsecustomer.Content);
                                    tabla.Rows.Add("-1", mensaje + " " + responsecustomer.StatusCode + " customer Owner");
                                    bandera = false;
                                }
                                else
                                {
                                    tabla.Rows.Add("-1", "Error al procesar customer " + responsecustomer.StatusCode + " customer Owner");
                                    bandera = false;
                                }
                            }
                            else
                            {
                                customersearch = new RestClient(rutacustomerAdd + "?username=" + propietario.username + "&customer__company_code=" + propietario.company_code + "&customer__identity_document_number=" + propietario.internal_identifier + " ");
                                requestecustomersearch = new RestRequest("", Method.Get);
                                requestecustomersearch.AddHeader("cache-control", "no-cache");
                                requestecustomersearch.AddHeader("authorization", "HunterAPI " + token);
                                responsecustomersearch = customersearch.Execute(requestecustomersearch);
                                valoretorno = "0";
                                valoretorno = funciones.descomponer("id", responsecustomersearch.Content);
                                if (valoretorno != "0")
                                {
                                    customer = new RestClient(rutacustomerAdd + valoretorno + "/ ");
                                    requeste = new RestRequest("", Method.Patch);
                                    textocustomer = Utilidades.SerializarCustomer(propietario, valoretorno, accountType);
                                    requeste.AddHeader("content-type", "application/json");
                                    requeste.AddHeader("authorization", "HunterAPI " + token);
                                    requeste.AddParameter("application/json", textocustomer, ParameterType.RequestBody);
                                    responsecustomer = customer.Execute(requeste);
                                    if (Convert.ToInt32(responsecustomer.StatusCode) == 200)
                                    {
                                        customerID = funciones.descomponer("id", responsecustomer.Content);
                                        ownerID = customerID;
                                        bandera = true;
                                    }
                                    else if (Convert.ToInt32(responsecustomer.StatusCode) == 400)
                                    {
                                        string mensaje = funciones.DevuelveMensaje(responsecustomer.Content);
                                        tabla.Rows.Add("-1", mensaje + " " + responsecustomer.StatusCode + " customer Actualiza owner");
                                        bandera = false;
                                    }
                                    else
                                    {
                                        tabla.Rows.Add("-1", "Error al procesar customer " + responsecustomer.StatusCode + " customer Actualiza owner");
                                        bandera = false;
                                    }
                                }
                                else
                                {
                                    tabla.Rows.Add("-1", "User Name duplicado customer Actualiza owner");
                                    bandera = false;
                                }
                            }
                        }
                        accountType = "2";
                    }
                    // customer
                    if (bandera)
                    {
                        // consulta si existe el customer si se encuentra inactivo
                        customersearch = new RestClient(rutacustomerAdd + "?username=" + monitoreo.username + "&is_active=" + false + "&customer__company_code=" + monitoreo.company_code + "&customer__identity_document_number=" + monitoreo.internal_identifier + " ");
                        requestecustomersearch = new RestRequest("", Method.Get);
                        requestecustomersearch.AddHeader("cache-control", "no-cache");
                        requestecustomersearch.AddHeader("authorization", "HunterAPI " + token);
                        responsecustomersearch = customersearch.Execute(requestecustomersearch);
                        valoretorno = "0";
                        valoretorno = funciones.descomponer("id", responsecustomersearch.Content);
                        if (valoretorno != "0")
                        {
                            customer = new RestClient(rutacustomerAdd + valoretorno + "/ ");
                            requeste = new RestRequest("", Method.Patch);
                            textocustomer = Utilidades.SerializarCustomer(monitoreo, valoretorno, accountType);
                            requeste.AddHeader("content-type", "application/json");
                            requeste.AddHeader("authorization", "HunterAPI " + token);
                            requeste.AddParameter("application/json", textocustomer, ParameterType.RequestBody);
                            responsecustomer = customer.Execute(requeste);
                            if (Convert.ToInt32(responsecustomer.StatusCode) == 200)
                            {
                                customerID = funciones.descomponer("id", responsecustomer.Content);
                                if (ownerID == customerID)
                                {
                                    tabla.Rows.Add("-1", "Verificar el mail del cliente propietario - monitoreo no pueden ser iguales");
                                    bandera = false;
                                }
                                else
                                {
                                    if (ownerID == "0")
                                        ownerID = customerID;
                                    bandera = true;
                                }
                            }
                            else if (Convert.ToInt32(responsecustomer.StatusCode) == 400)
                            {
                                string mensaje = funciones.DevuelveMensaje(responsecustomer.Content);
                                tabla.Rows.Add("-1", mensaje + " " + responsecustomer.StatusCode + " customer Actualiza");
                                bandera = false;
                            }
                            else
                            {
                                tabla.Rows.Add("-1", "Error al procesar customer " + responsecustomer.StatusCode + " customer Actualiza");
                                bandera = false;
                            }
                        }
                        else
                        {
                            // consulta si existe el customer si se encuentra activo
                            customersearch = new RestClient(rutacustomerAdd + "?username=" + monitoreo.username + "&is_active=" + true + "&customer__company_code=" + monitoreo.company_code + "&customer__identity_document_number=" + monitoreo.internal_identifier + " ");
                            requestecustomersearch = new RestRequest("", Method.Get);
                            requestecustomersearch.AddHeader("cache-control", "no-cache");
                            requestecustomersearch.AddHeader("authorization", "HunterAPI " + token);
                            responsecustomersearch = customersearch.Execute(requestecustomersearch);
                            valoretorno = "0";
                            valoretorno = funciones.descomponer("id", responsecustomersearch.Content);
                            if (valoretorno == "0")
                            {
                                customer = new RestClient(rutacustomer);
                                requeste = new RestRequest("", Method.Post);
                                requeste.AddHeader("cache-control", "no-cache");
                                requeste.AddHeader("authorization", "HunterAPI " + token);
                                requeste.AddParameter("username", monitoreo.username);
                                requeste.AddParameter("first_name", monitoreo.first_name);
                                requeste.AddParameter("last_name", monitoreo.last_name);
                                requeste.AddParameter("email", monitoreo.email);
                                requeste.AddParameter("customer.company_code", monitoreo.company_code);
                                requeste.AddParameter("customer.identity_document_type", monitoreo.identity_customer_type);
                                requeste.AddParameter("customer.identity_document_number", monitoreo.internal_identifier);
                                requeste.AddParameter("customer.business_name", monitoreo.business_name);
                                requeste.AddParameter("customer.phone_number", monitoreo.telefono);
                                requeste.AddParameter("customer.emergency_phone_number", monitoreo.emergency_phone_number);
                                requeste.AddParameter("customer.assistance_phone_number", monitoreo.assistance_phone_number);
                                requeste.AddParameter("customer.technical_support_email", monitoreo.technical_support_email);
                                requeste.AddParameter("customer.account_type", accountType);
                                requeste.AddParameter("is_active", monitoreo.active);
                                responsecustomer = customer.Execute(requeste);
                                if (Convert.ToInt32(responsecustomer.StatusCode) == 201)
                                {
                                    customerID = funciones.descomponer("id", responsecustomer.Content);
                                    if (ownerID == customerID)
                                    {
                                        tabla.Rows.Add("-1", "Verificar el mail del cliente propietario - monitoreo no pueden ser iguales");
                                        bandera = false;
                                    }
                                    else
                                    {
                                        if (ownerID == "0")
                                            ownerID = customerID;
                                        bandera = true;
                                    }
                                }
                                else if (Convert.ToInt32(responsecustomer.StatusCode) == 400)
                                {
                                    string mensaje = funciones.DevuelveMensaje(responsecustomer.Content);
                                    tabla.Rows.Add("-1", mensaje + " " + responsecustomer.StatusCode + " customer");
                                    bandera = false;
                                }
                                else
                                {
                                    tabla.Rows.Add("-1", "Error al procesar customer " + responsecustomer.StatusCode + " customer");
                                    bandera = false;
                                }
                            }
                            else
                            {
                                customersearch = new RestClient(rutacustomerAdd + "?username=" + monitoreo.username + "&customer__company_code=" + monitoreo.company_code + "&customer__identity_document_number=" + monitoreo.internal_identifier + " ");
                                requestecustomersearch = new RestRequest("", Method.Get);
                                requestecustomersearch.AddHeader("cache-control", "no-cache");
                                requestecustomersearch.AddHeader("authorization", "HunterAPI " + token);
                                responsecustomersearch = customersearch.Execute(requestecustomersearch);
                                valoretorno = "0";
                                valoretorno = funciones.descomponer("id", responsecustomersearch.Content);
                                if (valoretorno != "0")
                                {
                                    customer = new RestClient(rutacustomerAdd + valoretorno + "/ ");
                                    requeste = new RestRequest("", Method.Patch);
                                    textocustomer = Utilidades.SerializarCustomer(monitoreo, valoretorno, accountType);
                                    requeste.AddHeader("content-type", "application/json");
                                    requeste.AddHeader("authorization", "HunterAPI " + token);
                                    requeste.AddParameter("application/json", textocustomer, ParameterType.RequestBody);
                                    responsecustomer = customer.Execute(requeste);
                                    if (Convert.ToInt32(responsecustomer.StatusCode) == 200)
                                    {
                                        customerID = funciones.descomponer("id", responsecustomer.Content);
                                        if (ownerID == customerID)
                                        {
                                            tabla.Rows.Add("-1", "Verificar el mail del cliente propietario - monitoreo no pueden ser iguales");
                                            bandera = false;
                                        }
                                        else
                                        {
                                            if (ownerID == "0")
                                                ownerID = customerID;
                                            bandera = true;
                                        }
                                    }
                                    else if (Convert.ToInt32(responsecustomer.StatusCode) == 400)
                                    {
                                        string mensaje = funciones.DevuelveMensaje(responsecustomer.Content);
                                        tabla.Rows.Add("-1", mensaje + " " + responsecustomer.StatusCode + " customer Actualiza");
                                        bandera = false;
                                    }
                                    else
                                    {
                                        tabla.Rows.Add("-1", "Error al procesar customer " + responsecustomer.StatusCode + " customer Actualiza");
                                        bandera = false;
                                    }
                                }
                                else
                                {
                                    tabla.Rows.Add("-1", "User Name duplicado customer Actualiza");
                                    bandera = false;
                                }
                            }
                        }
                    }

                    // bloque de cliente anterior
                    if (bandera)
                    {
                        customersearch = new RestClient(rutacustomerAdd + "?username=" + clienteanterior.username + "&customer__company_code=" + clienteanterior.company_code + " ");
                        requestecustomersearch = new RestRequest("", Method.Get);
                        requestecustomersearch.AddHeader("cache-control", "no-cache");
                        requestecustomersearch.AddHeader("authorization", "HunterAPI " + token);
                        responsecustomersearch = customersearch.Execute(requestecustomersearch);
                        valoretorno = "0";
                        valoretorno = funciones.descomponer("id", responsecustomersearch.Content);
                        if (valoretorno == "0")
                        {
                            tabla.Rows.Add("-1", "Error al consultar el customer anterior, no existe, Cambio de propietario ");
                            bandera = false;
                        }
                        else
                        {
                            customeroldID = valoretorno;
                            bandera = true;
                        }
                    }
                }

                // bloque  de  vehiculo
                if (bandera)
                {
                    string texto = "";
                    var clienteasset = new RestClient();
                    var requesteasset = new RestRequest();
                    RestResponse responseasset;
                    // consulta si existe la relacion vehiculo - usuario
                    var assetSearch = new RestClient(rutaassetAdd + "?attributes__attribute__name=COD_SYS" + "&attributes__value=" + asset.codigosys + " ");
                    var requesteAssetSearch = new RestRequest("", Method.Get);
                    requesteAssetSearch.AddHeader("cache-control", "no-cache");
                    requesteAssetSearch.AddHeader("authorization", "HunterAPI " + token);
                    RestResponse responseAssetSearch = assetSearch.Execute(requesteAssetSearch);
                    valoretorno = "0";
                    valoretorno = funciones.descomponer("id", responseAssetSearch.Content);
                    if (valoretorno != "0")
                    {
                        cobertura = funciones.descomponer("product_expire_date", responseAssetSearch.Content).Substring(0, 10);
                        producto = funciones.descomponer("product", responseAssetSearch.Content);
                        //producto = "SUZUKI TRACKING BY HUNTER";
                        //asset.product = producto;
                        texto = Utilidades.SerializarAssetCambio(asset, attributeEntidadCollection, valoretorno, ownerID);
                        clienteasset = new RestClient(rutaassetAdd + valoretorno + "/ ");
                        requesteasset = new RestRequest("",Method.Patch);
                        requesteasset.AddHeader("content-type", "application/json");
                        requesteasset.AddHeader("authorization", "HunterAPI " + token);
                        requesteasset.AddParameter("application/json", texto, ParameterType.RequestBody);
                        responseasset = clienteasset.Execute(requesteasset);
                        if (Convert.ToInt32(responseasset.StatusCode) == 200)
                        {
                            assetID = funciones.descomponer("id", responseasset.Content);
                            bandera = true;                           
                        }
                        else if (Convert.ToInt32(responseasset.StatusCode) == 400)
                        {
                            string mensaje = funciones.DevuelveMensaje(responseasset.Content);
                            tabla.Rows.Add("-1", mensaje + " " + responseasset.StatusCode + " asset Actualiza");
                            bandera = false;
                        }
                    }
                    else
                    {
                        tabla.Rows.Add("-1", "Error al consultar el asset, no existe " + ", Cambio de propietario ");
                        bandera = false;
                    }
                }

                //bloque  de  vehiculo remover - usuario anterior
                if (bandera)
                {
                    RestResponse responseUserSearch;
                    RestResponse responseassetuser;
                    var assetUserSearch = new RestClient(rutaassetAdd + "?attributes__attribute__name=COD_SYS" + "&attributes__value=" + asset.codigosys + " ");
                    var requesteassetUserSearch = new RestRequest("", Method.Get);
                    requesteassetUserSearch.AddHeader("cache-control", "no-cache");
                    requesteassetUserSearch.AddHeader("authorization", "HunterAPI " + token);
                    responseUserSearch = assetUserSearch.Execute(requesteassetUserSearch);
                    string cadena = "0";
                    cadena = funciones.descomponer("user", responseUserSearch.Content);
                    cadena = cadena.Replace("[", "").Replace("]", "");
                    //cadena = "46052";
                    if (cadena != "0")
                    {
                        string[] datousers = cadena.Split(delimiter);
                        foreach (var usercustomers in datousers)
                        {
                            var clienteassetuser = new RestClient(rutaassetAdd + assetID + "/user-remove/ ");
                            var requesteassetuser = new RestRequest("", Method.Patch);
                            requesteassetuser.AddHeader("cache-control", "no-cache");
                            requesteassetuser.AddHeader("authorization", "HunterAPI " + token);
                            requesteassetuser.AddParameter("id", assetID);
                            requesteassetuser.AddParameter("user_id", usercustomers);
                            responseassetuser = clienteassetuser.Execute(requesteassetuser);
                            if (Convert.ToInt32(responseassetuser.StatusCode) == 200)
                            {
                                bandera = true;
                            }
                            else if (Convert.ToInt32(responseassetuser.StatusCode) == 400)
                            {
                                string mensaje = funciones.DevuelveMensaje(responseassetuser.Content);
                                tabla.Rows.Add("-1", mensaje + " " + responseassetuser.StatusCode + " asset-usuario Chequeo ");
                                bandera = false;
                            }
                            else
                            {
                                tabla.Rows.Add("-1", "Error al procesar assetuser " + responseassetuser.StatusCode + " asset-usuario Chequeo ");
                                bandera = false;
                            }
                        }
                    }
                    else
                    {
                        tabla.Rows.Add("-1", "Error al consultar el asset, no tienen relacionado el device " + ", Cambio de propietario ");
                        bandera = false;
                    }
                }
               
                //bloque  de  vehiculo adicionar - usuario nuevo
                if (bandera)
                {
                    RestResponse responseUserSearch;
                    RestResponse responseassetuser;
                    var assetUserSearch = new RestClient(rutacustomerAdd + "?attributes__attribute__name=COD_SYS" + "&attributes__value=" + asset.codigosys + " ");
                    var requesteassetUserSearch = new RestRequest("", Method.Get);
                    requesteassetUserSearch.AddHeader("cache-control", "no-cache");
                    requesteassetUserSearch.AddHeader("authorization", "HunterAPI " + token);
                    responseUserSearch = assetUserSearch.Execute(requesteassetUserSearch);
                    valoretorno = "0";
                    valoretorno = funciones.descomponer("id", responseUserSearch.Content);
                    if (valoretorno != "0")
                    {
                        var clienteassetuser = new RestClient(rutaassetAdd + assetID + "/user-add/ ");
                        var requesteassetuser = new RestRequest("", Method.Patch);
                        requesteassetuser.AddHeader("cache-control", "no-cache");
                        requesteassetuser.AddHeader("authorization", "HunterAPI " + token);
                        requesteassetuser.AddParameter("id", assetID);
                        requesteassetuser.AddParameter("user_id", customerID);
                        responseassetuser = clienteassetuser.Execute(requesteassetuser);
                        if (Convert.ToInt32(responseassetuser.StatusCode) == 200)
                        {
                            bandera = true;
                        }
                        else if (Convert.ToInt32(responseassetuser.StatusCode) == 400)
                        {
                            string mensaje = funciones.DevuelveMensaje(responseassetuser.Content);
                            tabla.Rows.Add("-1", mensaje + " " + responseassetuser.StatusCode + " asset-usuario");
                            bandera = false;
                        }
                        else
                        {
                            tabla.Rows.Add("-1", "Error al procesar assetuser " + responseassetuser.StatusCode + " asset-usuario ");
                            bandera = false;
                        }
                    }
                    else
                    {
                        tabla.Rows.Add("-1", "Error al consultar el asset, no tienen relacionado el device " + ", Cambio de propietario ");
                        bandera = false;
                    }
                }
                //bloque  de  vehiculo - comando               
                if (bandera)
                {
                    if (comando != "")
                    {
                        var commandSearch = new RestClient();
                        var requesteCommandSearch = new RestRequest();
                        RestResponse responseCommandSearch;
                        var clientecommand = new RestClient();
                        var requestecommand = new RestRequest();
                        RestResponse responsecommand;
                        foreach (var datcomando in comandos)
                        {
                            commandSearch = new RestClient(rutacommandAdd + "?asset=" + assetID + "&command=" + comando  + " ");
                            requesteCommandSearch = new RestRequest("", Method.Get);
                            requesteCommandSearch.AddHeader("cache-control", "no-cache");
                            requesteCommandSearch.AddHeader("authorization", "HunterAPI " + token);
                            responseCommandSearch = commandSearch.Execute(requesteCommandSearch);
                            valoretorno = "0";
                            valoretorno = funciones.descomponer("id", responseCommandSearch.Content);
                            if (valoretorno != "0")
                            {
                                clientecommand = new RestClient(rutacommandAdd + valoretorno + "/ ");
                                requestecommand = new RestRequest("", Method.Patch);
                                requestecommand.AddHeader("cache-control", "no-cache");
                                requestecommand.AddHeader("authorization", "HunterAPI " + token);
                                requestecommand.AddParameter("status", 1);
                                requestecommand.AddParameter("id", valoretorno);
                                requestecommand.AddParameter("command", comando);
                                requestecommand.AddParameter("asset", assetID);
                                responsecommand = clientecommand.Execute(requestecommand);
                                if (Convert.ToInt32(responsecommand.StatusCode) == 200)
                                    bandera = true;
                                else if (Convert.ToInt32(responsecommand.StatusCode) == 400)
                                {
                                    string mensaje = funciones.DevuelveMensaje(responsecommand.Content);
                                    tabla.Rows.Add("-1", mensaje + " " + responsecommand.StatusCode + " asset-comando Actualiza");
                                    bandera = false;
                                    break;
                                }
                                else
                                {
                                    tabla.Rows.Add("-1", "Error al procesar asset command " + responsecommand.StatusCode + " asset-comando Actualiza");
                                    bandera = false;
                                    break;
                                }
                            }
                            else
                            {
                                tabla.Rows.Add("-1", "Error no existe procesar asset command ");
                                bandera = false;
                                break;
                            }
                        }
                    }
                }
                
                //bloque  de  comando
                if (bandera)
                {
                    if (comando != "")
                    {
                        var userCommandSearch = new RestClient();
                        var requesteuserCommandSearch = new RestRequest();
                        RestResponse responseuserCommandSearch;
                        var clienteusercommand = new RestClient();
                        var requesteusercommand = new RestRequest();
                        RestResponse responseusercommand;
                        foreach (var datcomando in comandos)
                        {
                            //consulta si existe el comando por vehiculo - comando - usuario old - estado falso
                            userCommandSearch = new RestClient(rutausercommandAdd + "?asset=" + assetID + "&command=" + comando + "&user=" + customeroldID + "&can_execute=" + true + " ");
                            requesteuserCommandSearch = new RestRequest("", Method.Get);
                            requesteuserCommandSearch.AddHeader("cache-control", "no-cache");
                            requesteuserCommandSearch.AddHeader("authorization", "HunterAPI " + token);
                            responseuserCommandSearch = userCommandSearch.Execute(requesteuserCommandSearch);
                            valoretorno = "0";
                            valoretorno = funciones.descomponer("id", responseuserCommandSearch.Content);
                            if (valoretorno != "0")
                            {
                                clienteusercommand = new RestClient(rutausercommandAdd + valoretorno + "/ ");
                                requesteusercommand = new RestRequest("", Method.Patch);
                                requesteusercommand.AddHeader("cache-control", "no-cache");
                                requesteusercommand.AddHeader("authorization", "HunterAPI " + token);
                                requesteusercommand.AddParameter("id", valoretorno);
                                requesteusercommand.AddParameter("command", comando);
                                requesteusercommand.AddParameter("user", customeroldID);
                                requesteusercommand.AddParameter("asset", assetID);
                                requesteusercommand.AddParameter("can_execute", false);
                                responseusercommand = clienteusercommand.Execute(requesteusercommand);
                                if (Convert.ToInt32(responseusercommand.StatusCode) == 200)
                                    bandera = true;
                                else if (Convert.ToInt32(responseusercommand.StatusCode) == 400)
                                {
                                    string mensaje = funciones.DevuelveMensaje(responseusercommand.Content);
                                    tabla.Rows.Add("-1", mensaje + " " + responseusercommand.StatusCode + " comando-usuario old Actualiza");
                                    bandera = false;
                                    break;
                                }
                                else
                                {
                                    tabla.Rows.Add("-1", "Error al procesar user asset command " + responseusercommand.StatusCode + " comando-usuario old Actualiza");
                                    bandera = false;
                                    break;
                                }
                            }
                            //consulta si existe el comando por vehiculo - comando - usuario  - estado 
                            userCommandSearch = new RestClient(rutausercommandAdd + "?asset=" + assetID + "&command=" + comando + "&user=" + customerID + " ");
                            requesteuserCommandSearch = new RestRequest("", Method.Get);
                            requesteuserCommandSearch.AddHeader("cache-control", "no-cache");
                            requesteuserCommandSearch.AddHeader("authorization", "HunterAPI " + token);
                            responseuserCommandSearch = userCommandSearch.Execute(requesteuserCommandSearch);
                            valoretorno = "0";
                            valoretorno = funciones.descomponer("id", responseuserCommandSearch.Content);
                            if (valoretorno != "0")
                            {
                                clienteusercommand = new RestClient(rutausercommandAdd + valoretorno + "/ ");
                                requesteusercommand = new RestRequest("", Method.Patch);
                                requesteusercommand.AddHeader("cache-control", "no-cache");
                                requesteusercommand.AddHeader("authorization", "HunterAPI " + token);
                                requesteusercommand.AddParameter("id", valoretorno);
                                requesteusercommand.AddParameter("command", comando);
                                requesteusercommand.AddParameter("user", customerID);
                                requesteusercommand.AddParameter("asset", assetID);
                                requesteusercommand.AddParameter("can_execute", execute);
                                responseusercommand = clienteusercommand.Execute(requesteusercommand);
                                if (Convert.ToInt32(responseusercommand.StatusCode) == 200)
                                    bandera = true;
                                else if (Convert.ToInt32(responseusercommand.StatusCode) == 400)
                                {
                                    string mensaje = funciones.DevuelveMensaje(responseusercommand.Content);
                                    tabla.Rows.Add("-1", mensaje + " " + responseusercommand.StatusCode + " comando-usuario Actualiza");
                                    bandera = false;
                                    break;
                                }
                                else
                                {
                                    tabla.Rows.Add("-1", "Error al procesar user asset command " + responseusercommand.StatusCode + " comando-usuario Actualiza");
                                    bandera = false;
                                    break;
                                }
                            }
                            else
                            {
                                userCommandSearch = new RestClient(rutausercommandAdd + "?asset=" + assetID + "&command=" + comando + "&user=" + customeroldID + " ");
                                requesteuserCommandSearch = new RestRequest("", Method.Get);
                                requesteuserCommandSearch.AddHeader("cache-control", "no-cache");
                                requesteuserCommandSearch.AddHeader("authorization", "HunterAPI " + token);
                                responseuserCommandSearch = userCommandSearch.Execute(requesteuserCommandSearch);
                                valoretorno = "0";
                                valoretorno = funciones.descomponer("id", responseuserCommandSearch.Content);
                                if (valoretorno != "0")
                                {
                                    clienteusercommand = new RestClient(rutausercommandAdd + valoretorno + "/ ");
                                    requesteusercommand = new RestRequest("", Method.Patch);
                                    requesteusercommand.AddHeader("cache-control", "no-cache");
                                    requesteusercommand.AddHeader("authorization", "HunterAPI " + token);
                                    requesteusercommand.AddParameter("id", valoretorno);
                                    requesteusercommand.AddParameter("command", comando);
                                    requesteusercommand.AddParameter("user", customerID);
                                    requesteusercommand.AddParameter("asset", assetID);
                                    requesteusercommand.AddParameter("can_execute", execute);
                                    responseusercommand = clienteusercommand.Execute(requesteusercommand);
                                    if (Convert.ToInt32(responseusercommand.StatusCode) == 200)
                                        bandera = true;
                                    else if (Convert.ToInt32(responseusercommand.StatusCode) == 400)
                                    {
                                        string mensaje = funciones.DevuelveMensaje(responseusercommand.Content);
                                        tabla.Rows.Add("-1", mensaje + " " + responseusercommand.StatusCode + " comando-usuario");
                                        bandera = false;
                                        break;
                                    }
                                    else
                                    {
                                        tabla.Rows.Add("-1", "Error al procesar user asset command " + responseusercommand.StatusCode + " comando-usuario");
                                        bandera = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    tabla.Rows.Add("-1", "Error al procesar user asset command no existe comando-usuario");
                                    bandera = false;
                                    break;
                                }
                            }
                        }
                    }
                }
               
                // ingreso del log del proceso de AMI
                if (bandera)
                {
                    string resultadoLog = ConsultaAMI.RegistroLog(p.codigocliente, ownerID, p.clientemonitoreo, customerID, asset.codigosys, assetID, gencomando, device.id, producto, p.ordenservicio, "0", p.usuario, envio, monitoreo.username, p.tiporegistro, userpropietario, p.origen, accion, p.clienteold, customeroldID, userold, "", "", "", "", "", "", "", p.parametroproducto, cobertura, valorvid, p.producto, concesionario);
                    if (resultadoLog == "OK")
                    {
                        bandera = true;
                    }
                    else
                    {
                        tabla.Rows.Add("-1", "Error en el Ingreso del Log");
                        bandera = false;
                    }
                }
            }
            string jsonData;
            if (bandera)
            {
                tabla.Rows.Add(1, "Guardado Exitosamente");
            }
            jsonData = (string)Utilidades.DataTableToJSON(tabla);

            return jsonData;

        }


        [HttpPost]
        [Authorize]
        [SwaggerOperation(Summary = "", Description = "Actualiza la cobertura del vehiculo")]
        [Route("ActualizacionCoberturaAsset")]
        public ActionResult<string> ActualizacionCoberturaAsset(GeneralAMIDTO p)
        {  
            //conexion ruta 
            string rutalogin = "";
            string rutaassetAdd = "";
            string rutacustomerAdd = "";
            //variables
            bool bandera = true;
            DataSet infoDatos = new DataSet();
            string token = "";
            string valoretorno = "";
            string customerID = "";
            string verificaasociado = "N";
            string tiporegistro = "COB";
            string assetID = "";
            string sensors = "0";
            string log_usr = "";
            string log_pwd = "";
            string producto = "";
            string concesionario = "";
            DataTable tabla = new DataTable();
            tabla.Columns.Add("CODIGO", typeof(int));
            tabla.Columns.Add("MENSAJE", typeof(string));
            conexionAMI ruta = new conexionAMI();
            if (p.parametroproducto == "GPT")
            {
                rutalogin = ruta.GetRuta("1");
                rutaassetAdd = ruta.GetRuta("2");
                rutacustomerAdd = ruta.GetRuta("3");    
            }
            else
            {
                bandera = false;
                tabla.Rows.Add("-1", "No existe el parametro del producto");
            }
            customerEntidad cliente = null;
            assetEntidad asset = null;
            deviceEntidad device = null;
            attributeEntidadCollection attributeEntidadCollection = null;
            attributeEntidad attributeCollection = null;
            string busquedaAmiVehiculo = "";
            string busquedaAmiCliente = "";
            string accion = "";
            string clienteAMI = "";
            string assetAMI = "";
            string deviceAMI = "";
            if (bandera)
            {
                DataSet cnstGenrl = new DataSet();
                cnstGenrl = BienSQL.CnstVehiculo(opcion: "100", cliente: "", vehiculo: "", parametroproducto: "GPT");
                string support = (string)cnstGenrl.Tables[0].Rows[0]["support"];
                log_usr = (string)cnstGenrl.Tables[0].Rows[0]["usuario"];
                log_pwd = (string)cnstGenrl.Tables[0].Rows[0]["pass"];
                string ami_telefono = (string)cnstGenrl.Tables[0].Rows[0]["telefono"];
                string ami_celular = (string)cnstGenrl.Tables[0].Rows[0]["celular"];
                string ruccompania = (string)cnstGenrl.Tables[0].Rows[0]["ruccompania"];
                //informacion del cliente 
                if (bandera)
                {
                    clienteAMI = ConsultaAMI.ProcesoAMIConsulta("204", p.codigovehiculo, p.producto, p.ordenservicio, p.itemNS, "");
                    accion = funciones.descomponer("ACCION", clienteAMI);
                    busquedaAmiCliente= funciones.descomponer("ID_TELEMATIC", clienteAMI);
                    cliente = new customerEntidad
                    {
                        username = funciones.descomponer("USERNAME", clienteAMI),
                        //username = "dranancy@hotmail.com",
                        first_name = funciones.descomponer("FIRST_NAME", clienteAMI),
                        last_name = funciones.descomponer("LAST_NAME", clienteAMI),
                        email = funciones.descomponer("EMAIL", clienteAMI),
                        login = log_usr,
                        password = log_pwd,
                        internal_identifier = funciones.descomponer("ID_CLIENTE", clienteAMI),
                        active = "True",
                        telefono = funciones.descomponer("TELEFONO_CLIENTE", clienteAMI),
                        identity_customer_type = funciones.descomponer("TIPO", clienteAMI),
                        company_code = ruccompania,
                        business_name = "",
                        emergency_phone_number = ami_telefono,
                        assistance_phone_number = ami_celular,
                        technical_support_email = support,
                    };
                }
                //device
                if (bandera)
                {
                    deviceAMI = ConsultaAMI.ProcesoAMIConsulta("202", p.codigovehiculo, p.producto, p.ordenservicio, p.itemNS, "");
                    //valorvid = funciones.descomponer("vid", deviceAMI);
                   // p.cobertura = funciones.descomponer("cobertura", deviceAMI);
                    p.cobertura = funciones.descomponer("cobertura", deviceAMI) + "T23:59:59";
                    device = new deviceEntidad
                    {
                        id = funciones.descomponer("serie", deviceAMI),
                        vid = funciones.descomponer("vid", deviceAMI),
                        company_code = ruccompania,
                        active = "True",
                        model = funciones.descomponer("id_telematic_modelo", deviceAMI),
                        report_from = funciones.descomponer("id_telematic_servidor", deviceAMI),
                        numero_celular = funciones.descomponer("noCelularSIM", deviceAMI),
                    };

                    if (device.vid == "0")
                    {
                        tabla.Rows.Add("-1", "Verificar el valor del VID ");
                        bandera = false;
                    }
                    if (device.report_from == "")
                    {
                        tabla.Rows.Add("-1", "Verificar el Servidor del Dispositivo ");
                        bandera = false;
                    }
                    if (device.model == "")
                    {
                        tabla.Rows.Add("-1", "Verificar el Modelo del Dispositivo ");
                        bandera = false;
                    }
                }
                //informacion del vehiculo
                if (bandera)
                {
                    assetAMI = ConsultaAMI.ProcesoAMIConsulta("203", p.codigovehiculo, p.producto, p.ordenservicio, p.itemNS, "");
                    busquedaAmiVehiculo = funciones.descomponer("ID_TELEMATIC", assetAMI);
                    if (funciones.descomponer("familiadescripcion", assetAMI) == "HUNTER GPS HMT")
                    {
                        producto = "H.MONITOREO AMI";
                    }
                    else if (funciones.descomponer("familiadescripcion", assetAMI) == "AMI CHANGAN BY HUNTER")
                    {
                        producto = "H.MONITOREO AMI";
                    }
                    else if (funciones.descomponer("familiadescripcion", assetAMI) == "ANDOR BY HUNTER")
                    {
                        producto = "ANDOR LINK BY HUNTER";
                    }
                    else
                    {
                        producto = funciones.descomponer("familiadescripcion", assetAMI);
                    }
                    //para cambio de producto
                    concesionario = funciones.descomponer("concesionarios", assetAMI);
                    cnstGenrl = BienSQL.CnstProducto(opcion: "104", concesionario: concesionario, producto: p.producto);
                    if (cnstGenrl.Tables[0].Rows.Count > 0)
                    {
                        producto = (string)cnstGenrl.Tables[0].Rows[0]["NOMBRE_PRODUCTO"];
                    }
                    //trae los codigo del cliente y de vehiculo
                    cnstGenrl = BienSQL.CnstVehiculoParametros(opcion: "105", cliente: cliente.internal_identifier, vehiculo: p.codigosys, parametroproducto: "GPT", tipo:"", marca: "", modelo: "", chasis:"", motor:"", placa:"", color:"", anio:"");
                    if (cnstGenrl.Tables.Count > 0)
                    {
                        if (busquedaAmiCliente == "")
                        {
                            busquedaAmiCliente = (string)cnstGenrl.Tables[0].Rows[0]["CODIGO_AMI_OWNER"];
                        }

                        if (busquedaAmiVehiculo == "")
                        {
                            busquedaAmiVehiculo = (string)cnstGenrl.Tables[0].Rows[0]["CODIGO_AMI_VEHICULO"];
                        }
                    }
                    asset = new assetEntidad
                    {
                        name = funciones.descomponer("placa", assetAMI),
                        assettype = funciones.descomponer("tipo", assetAMI),
                        codigosys = p.codigosys, //CODIGO SYS O NETSUITE
                        codigonetsuite = p.codigovehiculo,
                        customname = "",
                        active = "True",
                        description = "PLC.:" + funciones.descomponer("placa", assetAMI) + ";MAR.:" + funciones.descomponer("marca", assetAMI) + ";MOD.:" + funciones.descomponer("modelo", assetAMI) + ";COL.:" + funciones.descomponer("color", assetAMI),
                        productexpiredate = p.cobertura,
                        product = producto,
                        chasis = funciones.descomponer("chasis", assetAMI),
                    };
                    //informacion del vehiculo atributos
                    cnstGenrl = BienSQL.CnstVehiculoParametros(opcion: "103", cliente: "", vehiculo: p.codigosys,
                                                               parametroproducto: "GPT", tipo: funciones.descomponer("tipo", assetAMI), marca: funciones.descomponer("marca", assetAMI),
                                                               modelo: funciones.descomponer("modelo", assetAMI), chasis: funciones.descomponer("chasis", assetAMI), motor: funciones.descomponer("motor", assetAMI),
                                                               placa: funciones.descomponer("placa", assetAMI), color: funciones.descomponer("color", assetAMI), anio: funciones.descomponer("anio", assetAMI));
                    if (cnstGenrl.Tables.Count > 0)
                    {
                        attributeEntidadCollection = new attributeEntidadCollection();
                        attributeCollection = new attributeEntidad();
                        for (int i = 0; i <= cnstGenrl.Tables[0].Rows.Count - 1; i++)
                        {
                            attributeEntidad attribute = new attributeEntidad();
                            attribute.AttributeId = (int)cnstGenrl.Tables[0].Rows[i]["ID"];
                            attribute.Valor = (string)cnstGenrl.Tables[0].Rows[i]["VALOR"];
                            attributeEntidadCollection.Add(attribute);
                        }
                        attributeCollection.attributeEntidadCollection = attributeEntidadCollection;
                    }
                }
                //login
                if (bandera)
                {
                    var client = new RestClient(rutalogin);
                    var request = new RestRequest("", Method.Post);
                    request.AddParameter("username", cliente.login);
                    request.AddParameter("password", cliente.password);
                    RestResponse response = client.Execute(request);
                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(response.Content);
                    token = funciones.descomponer("token", jsonString);
                }
            }        
            //cliente propietario
            if (bandera)
            {
                var customersearch = new RestClient();
                var requestecustomersearch = new RestRequest();
                RestResponse responsecustomersearch;
               // customersearch = new RestClient(rutacustomerAdd + "?username=" + cliente.username + "&assets__id=" + busquedaAmiVehiculo + "&id=" + busquedaAmiCliente + " ");
                customersearch = new RestClient(rutacustomerAdd + "?assets__id=" + busquedaAmiVehiculo + "&id=" + busquedaAmiCliente + " ");
                requestecustomersearch = new RestRequest("", Method.Get);
                requestecustomersearch.AddHeader("cache-control", "no-cache");
                requestecustomersearch.AddHeader("authorization", "HunterAPI " + token);
                responsecustomersearch = customersearch.Execute(requestecustomersearch);
                valoretorno = "0";
                valoretorno = funciones.descomponer("id", responsecustomersearch.Content);
                if (valoretorno != "0")
                {
                    if (valoretorno == busquedaAmiCliente)
                    {
                        customerID = valoretorno;
                        bandera = true;
                    }
                    else
                    {
                        tabla.Rows.Add("-1", "busqueda incorrecta" + " Customer Incorrecto");
                        bandera = false;
                    }
                }
                else
                {
                    tabla.Rows.Add("-1", "Error al buscar el Customer del cliente ");
                    bandera = false;
                }
            }           
            //bloque vehiculo
            if (bandera)
            {
                var assetSearch = new RestClient();
                var requesteAssetSearch = new RestRequest();
                RestResponse responseAssetSearch;
                var clienteasset = new RestClient();
                var requesteasset = new RestRequest();
                RestResponse responseasset;
                string texto  = "";
                assetSearch = new RestClient(rutaassetAdd + "?attributes__attribute__name=COD_SYS" + "&attributes__value=" + asset.codigosys + " ");
                requesteAssetSearch = new RestRequest("", Method.Get);
                requesteAssetSearch.AddHeader("cache-control", "no-cache");
                requesteAssetSearch.AddHeader("authorization", "HunterAPI " + token);
                responseAssetSearch = assetSearch.Execute(requesteAssetSearch);
                valoretorno = "0";
                valoretorno = funciones.descomponer("id", responseAssetSearch.Content);
                if (valoretorno != "0")
                {
                    if (valoretorno== busquedaAmiVehiculo)
                    {
                        sensors = funciones.descomponer("doors_sensors", responseAssetSearch.Content);
                        texto = Utilidades.SerializarAssetInstalacion(asset, attributeEntidadCollection, valoretorno, customerID, sensors);
                        clienteasset = new RestClient(rutaassetAdd + valoretorno + "/ ");
                        requesteasset = new RestRequest("", Method.Patch);
                        requesteasset.AddHeader("content-type", "application/json");
                        requesteasset.AddHeader("authorization", "HunterAPI " + token);
                        requesteasset.AddParameter("application/json", texto, ParameterType.RequestBody);
                        responseasset = clienteasset.Execute(requesteasset);
                        if (Convert.ToInt32(responseasset.StatusCode) == 200)
                        {
                            assetID = funciones.descomponer("id", responseasset.Content);
                            bandera = true;
                        }
                        else if (Convert.ToInt32(responseasset.StatusCode) == 400)
                        {
                            string mensaje = funciones.DevuelveMensaje(responseasset.Content);
                            tabla.Rows.Add("-1", mensaje + " " + responseasset.StatusCode + " asset Actualiza");
                            bandera = false;
                        }
                        else
                        {
                            tabla.Rows.Add("-1", "Error al procesar asset " + responseasset.StatusCode + " asset Actualiza");
                            bandera = false;
                        }
                    }
                    else
                    {
                        tabla.Rows.Add("-1", "Error al consultar el asset, no existe" + ", Actualizando ");
                        bandera = false;
                    }
                }
            }
            // ingreso del log del proceso de AMI
            if (bandera)
            {
                string resultadoLog = ConsultaAMI.RegistroLog(p.clientepropietario, customerID, "0", "0", asset.codigosys, assetID, "", "", producto, p.ordenservicio, "0", p.usuario, "N", "", tiporegistro, cliente.username, p.origen, accion, "", "", "", "", "", "", "", "", "", "", p.parametroproducto, p.cobertura, "", p.producto, concesionario);
                if (resultadoLog == "OK")
                {
                    //mensaje = "OK";
                    bandera = true;
                }
                else
                {
                    tabla.Rows.Add("-1", "Error en el Ingreso del Log");
                    bandera = false;
                }
            }
            string jsonData;
            if (bandera)
            {
                tabla.Rows.Add(1, "Guardado Exitosamente");
            }
            jsonData = (string)Utilidades.DataTableToJSON(tabla);

            return jsonData;

        }


        [HttpPost]
        [Authorize]
        [SwaggerOperation(Summary = "", Description = "Actualiza la cobertura para el proceso de Netsuite")]
        [Route("ActualizacionCoberturaNetsuite")]
        public ActionResult<string> ActualizacionCoberturaNetsuite(GeneralAMIDTO p)
        {
            //conexion ruta 
            string rutalogin = "";
            string rutaassetAdd = "";
            string rutacustomerAdd = "";
            //variables
            bool bandera = true;
            DataSet infoDatos = new DataSet();
            string token = "";
            string valoretorno = "";
            string customerID = "";
            string verificaasociado = "N";
            string tiporegistro = "COB";
            string assetID = "";
            string sensors = "0";
            string log_usr = "";
            string log_pwd = "";
            string producto = "";
            string concesionario = "";
            DataTable tabla = new DataTable();
            tabla.Columns.Add("CODIGO", typeof(int));
            tabla.Columns.Add("MENSAJE", typeof(string));
            conexionAMI ruta = new conexionAMI();
            if (p.parametroproducto == "GPT")
            {
                rutalogin = ruta.GetRuta("1");
                rutaassetAdd = ruta.GetRuta("2");
                rutacustomerAdd = ruta.GetRuta("3");
            }
            else
            {
                bandera = false;
                tabla.Rows.Add("-1", "No existe el parametro del producto");
            }
            customerEntidad cliente = null;
            assetEntidad asset = null;
            deviceEntidad device = null;
            attributeEntidadCollection attributeEntidadCollection = null;
            attributeEntidad attributeCollection = null;
            string busquedaAmiVehiculo = "";
            string busquedaAmiCliente = "";
            string accion = "";
            string clienteAMI = "";
            string assetAMI = "";
            string deviceAMI = "";
            if (bandera)
            {
                DataSet cnstGenrl = new DataSet();
                cnstGenrl = BienSQL.CnstVehiculo(opcion: "100", cliente: "", vehiculo: "", parametroproducto: "GPT");
                string support = (string)cnstGenrl.Tables[0].Rows[0]["support"];
                log_usr = (string)cnstGenrl.Tables[0].Rows[0]["usuario"];
                log_pwd = (string)cnstGenrl.Tables[0].Rows[0]["pass"];
                string ami_telefono = (string)cnstGenrl.Tables[0].Rows[0]["telefono"];
                string ami_celular = (string)cnstGenrl.Tables[0].Rows[0]["celular"];
                string ruccompania = (string)cnstGenrl.Tables[0].Rows[0]["ruccompania"];
                //informacion del cliente 
                if (bandera)
                {
                    clienteAMI = ConsultaAMI.ProcesoAMIConsulta("201", p.codigovehiculo, p.producto, p.ordenservicio, p.itemNS, "");
                    accion = funciones.descomponer("ACCION", clienteAMI);
                    busquedaAmiCliente = funciones.descomponer("ID_TELEMATIC", clienteAMI);
                    cliente = new customerEntidad
                    {
                        username = funciones.descomponer("USERNAME", clienteAMI),
                        //username = "dranancy@hotmail.com",
                        first_name = funciones.descomponer("FIRST_NAME", clienteAMI),
                        last_name = funciones.descomponer("LAST_NAME", clienteAMI),
                        email = funciones.descomponer("EMAIL", clienteAMI),
                        login = log_usr,
                        password = log_pwd,
                        internal_identifier = funciones.descomponer("ID_CLIENTE", clienteAMI),
                        active = "True",
                        telefono = funciones.descomponer("TELEFONO_CLIENTE", clienteAMI),
                        identity_customer_type = funciones.descomponer("TIPO", clienteAMI),
                        company_code = ruccompania,
                        business_name = "",
                        emergency_phone_number = ami_telefono,
                        assistance_phone_number = ami_celular,
                        technical_support_email = support,
                    };
                }
                //device
                if (bandera)
                {
                    deviceAMI = ConsultaAMI.ProcesoAMIConsulta("202", p.codigovehiculo, p.producto, p.ordenservicio, p.itemNS, "");
                    //valorvid = funciones.descomponer("vid", deviceAMI);
                    //p.cobertura = funciones.descomponer("cobertura", deviceAMI)+ "T23:59:59";
                    device = new deviceEntidad
                    {
                        id = funciones.descomponer("serie", deviceAMI),
                        vid = funciones.descomponer("vid", deviceAMI),
                        company_code = ruccompania,
                        active = "True",
                        model = funciones.descomponer("id_telematic_modelo", deviceAMI),
                        report_from = funciones.descomponer("id_telematic_servidor", deviceAMI),
                        numero_celular = funciones.descomponer("noCelularSIM", deviceAMI),
                    };

                    if (device.vid == "0")
                    {
                        tabla.Rows.Add("-1", "Verificar el valor del VID ");
                        bandera = false;
                    }
                    if (device.report_from == "")
                    {
                        tabla.Rows.Add("-1", "Verificar el Servidor del Dispositivo ");
                        bandera = false;
                    }
                    if (device.model == "")
                    {
                        tabla.Rows.Add("-1", "Verificar el Modelo del Dispositivo ");
                        bandera = false;
                    }
                }
                //informacion del vehiculo
                if (bandera)
                {
                    assetAMI = ConsultaAMI.ProcesoAMIConsulta("203", p.codigovehiculo, p.producto, p.ordenservicio, p.itemNS, "");
                    busquedaAmiVehiculo = funciones.descomponer("ID_TELEMATIC", assetAMI);
                    if (funciones.descomponer("familiadescripcion", assetAMI) == "HUNTER GPS HMT")
                    {
                        producto = "H.MONITOREO AMI";
                    }
                    else if (funciones.descomponer("familiadescripcion", assetAMI) == "AMI CHANGAN BY HUNTER")
                    {
                        producto = "H.MONITOREO AMI";
                    }
                    else if (funciones.descomponer("familiadescripcion", assetAMI) == "ANDOR BY HUNTER")
                    {
                        producto = "ANDOR LINK BY HUNTER";
                    }
                    else
                    {
                        producto = funciones.descomponer("familiadescripcion", assetAMI);
                    }
                    //para cambio de producto
                    concesionario = funciones.descomponer("concesionarios", assetAMI);
                    cnstGenrl = BienSQL.CnstProducto(opcion: "104", concesionario: concesionario, producto: p.producto);
                    if (cnstGenrl.Tables[0].Rows.Count > 0)
                    {
                        producto = (string)cnstGenrl.Tables[0].Rows[0]["NOMBRE_PRODUCTO"];
                    }
                    //trae los codigo del cliente y de vehiculo
                    cnstGenrl = BienSQL.CnstVehiculoParametros(opcion: "105", cliente: cliente.internal_identifier, vehiculo: p.codigosys, parametroproducto: "GPT", tipo: "", marca: "", modelo: "", chasis: "", motor: "", placa: "", color: "", anio: "");
                    if (cnstGenrl.Tables.Count > 0)
                    {
                        if (busquedaAmiCliente == "")
                        {
                            busquedaAmiCliente = (string)cnstGenrl.Tables[0].Rows[0]["CODIGO_AMI_OWNER"];
                        }

                        if (busquedaAmiVehiculo == "")
                        {
                            busquedaAmiVehiculo = (string)cnstGenrl.Tables[0].Rows[0]["CODIGO_AMI_VEHICULO"];
                        }
                    }
                    asset = new assetEntidad
                    {
                        name = funciones.descomponer("placa", assetAMI),
                        assettype = funciones.descomponer("tipo", assetAMI),
                        codigosys = p.codigosys, //CODIGO SYS O NETSUITE
                        codigonetsuite = p.codigovehiculo,
                        customname = "",
                        active = "True",
                        description = "PLC.:" + funciones.descomponer("placa", assetAMI) + ";MAR.:" + funciones.descomponer("marca", assetAMI) + ";MOD.:" + funciones.descomponer("modelo", assetAMI) + ";COL.:" + funciones.descomponer("color", assetAMI),
                        productexpiredate = p.cobertura,
                        product = producto,
                        chasis = funciones.descomponer("chasis", assetAMI),
                    };
                    //informacion del vehiculo atributos
                    cnstGenrl = BienSQL.CnstVehiculoParametros(opcion: "103", cliente: "", vehiculo: p.codigosys,
                                                               parametroproducto: "GPT", tipo: funciones.descomponer("tipo", assetAMI), marca: funciones.descomponer("marca", assetAMI),
                                                               modelo: funciones.descomponer("modelo", assetAMI), chasis: funciones.descomponer("chasis", assetAMI), motor: funciones.descomponer("motor", assetAMI),
                                                               placa: funciones.descomponer("placa", assetAMI), color: funciones.descomponer("color", assetAMI), anio: funciones.descomponer("anio", assetAMI));
                    if (cnstGenrl.Tables.Count > 0)
                    {
                        attributeEntidadCollection = new attributeEntidadCollection();
                        attributeCollection = new attributeEntidad();
                        for (int i = 0; i <= cnstGenrl.Tables[0].Rows.Count - 1; i++)
                        {
                            attributeEntidad attribute = new attributeEntidad();
                            attribute.AttributeId = (int)cnstGenrl.Tables[0].Rows[i]["ID"];
                            attribute.Valor = (string)cnstGenrl.Tables[0].Rows[i]["VALOR"];
                            attributeEntidadCollection.Add(attribute);
                        }
                        attributeCollection.attributeEntidadCollection = attributeEntidadCollection;
                    }
                }
                //login
                if (bandera)
                {
                    var client = new RestClient(rutalogin);
                    var request = new RestRequest("", Method.Post);
                    request.AddParameter("username", cliente.login);
                    request.AddParameter("password", cliente.password);
                    RestResponse response = client.Execute(request);
                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(response.Content);
                    token = funciones.descomponer("token", jsonString);
                }
            }
            //cliente propietario
            if (bandera)
            {
                var customersearch = new RestClient();
                var requestecustomersearch = new RestRequest();
                RestResponse responsecustomersearch;
                // customersearch = new RestClient(rutacustomerAdd + "?username=" + cliente.username + "&assets__id=" + busquedaAmiVehiculo + "&id=" + busquedaAmiCliente + " ");
                customersearch = new RestClient(rutacustomerAdd + "?assets__id=" + busquedaAmiVehiculo + "&id=" + busquedaAmiCliente + " ");
                requestecustomersearch = new RestRequest("", Method.Get);
                requestecustomersearch.AddHeader("cache-control", "no-cache");
                requestecustomersearch.AddHeader("authorization", "HunterAPI " + token);
                responsecustomersearch = customersearch.Execute(requestecustomersearch);
                valoretorno = "0";
                valoretorno = funciones.descomponer("id", responsecustomersearch.Content);
                if (valoretorno != "0")
                {
                    if (valoretorno == busquedaAmiCliente)
                    {
                        customerID = valoretorno;
                        bandera = true;
                    }
                    else
                    {
                        tabla.Rows.Add("-1", "busqueda incorrecta" + " Customer Incorrecto");
                        bandera = false;
                    }
                }
                else
                {
                    tabla.Rows.Add("-1", "Error al buscar el Customer del cliente ");
                    bandera = false;
                }
            }
            //bloque vehiculo
            if (bandera)
            {
                var assetSearch = new RestClient();
                var requesteAssetSearch = new RestRequest();
                RestResponse responseAssetSearch;
                var clienteasset = new RestClient();
                var requesteasset = new RestRequest();
                RestResponse responseasset;
                string texto = "";
                assetSearch = new RestClient(rutaassetAdd + "?attributes__attribute__name=COD_SYS" + "&attributes__value=" + asset.codigosys + " ");
                requesteAssetSearch = new RestRequest("", Method.Get);
                requesteAssetSearch.AddHeader("cache-control", "no-cache");
                requesteAssetSearch.AddHeader("authorization", "HunterAPI " + token);
                responseAssetSearch = assetSearch.Execute(requesteAssetSearch);
                valoretorno = "0";
                valoretorno = funciones.descomponer("id", responseAssetSearch.Content);
                if (valoretorno != "0")
                {
                    if (valoretorno == busquedaAmiVehiculo)
                    {
                        sensors = funciones.descomponer("doors_sensors", responseAssetSearch.Content);
                        texto = Utilidades.SerializarAssetInstalacion(asset, attributeEntidadCollection, valoretorno, customerID, sensors);
                        clienteasset = new RestClient(rutaassetAdd + valoretorno + "/ ");
                        requesteasset = new RestRequest("", Method.Patch);
                        requesteasset.AddHeader("content-type", "application/json");
                        requesteasset.AddHeader("authorization", "HunterAPI " + token);
                        requesteasset.AddParameter("application/json", texto, ParameterType.RequestBody);
                        responseasset = clienteasset.Execute(requesteasset);
                        if (Convert.ToInt32(responseasset.StatusCode) == 200)
                        {
                            assetID = funciones.descomponer("id", responseasset.Content);
                            bandera = true;
                        }
                        else if (Convert.ToInt32(responseasset.StatusCode) == 400)
                        {
                            string mensaje = funciones.DevuelveMensaje(responseasset.Content);
                            tabla.Rows.Add("-1", mensaje + " " + responseasset.StatusCode + " asset Actualiza");
                            bandera = false;
                        }
                        else
                        {
                            tabla.Rows.Add("-1", "Error al procesar asset " + responseasset.StatusCode + " asset Actualiza");
                            bandera = false;
                        }
                    }
                    else
                    {
                        tabla.Rows.Add("-1", "Error al consultar el asset, no existe" + ", Actualizando ");
                        bandera = false;
                    }
                }
            }
            // ingreso del log del proceso de AMI
            if (bandera)
            {
                string resultadoLog = ConsultaAMI.RegistroLog(p.clientepropietario, customerID, "0", "0", asset.codigosys, assetID, "", "", producto, p.ordenservicio, "0", p.usuario, "N", "", tiporegistro, cliente.username, p.origen, accion, "", "", "", "", "", "", "", "", "", "", p.parametroproducto, p.cobertura, "", p.producto, concesionario);
                if (resultadoLog == "OK")
                {
                    //mensaje = "OK";
                    bandera = true;
                }
                else
                {
                    tabla.Rows.Add("-1", "Error en el Ingreso del Log");
                    bandera = false;
                }
            }
            string jsonData;
            if (bandera)
            {
                tabla.Rows.Add(1, "Guardado Exitosamente");
            }
            jsonData = (string)Utilidades.DataTableToJSON(tabla);

            return jsonData;

        }


        [HttpPost]
        [Authorize]
        [SwaggerOperation(Summary = "", Description = "Desinstala el dispositivo del vehiculo")]
        [Route("Desinstalacion")]
        public ActionResult<string> Desinstalacion(GeneralAMIDTO p)
        {
            // ruta ami 
            conexionAMI ruta = new conexionAMI();
            string rutalogin = "";
            string rutaassetAdd = "";
            string rutacustomerAdd = "";
            string rutadeviceAdd = "";
            string rutacommandAdd = "";
            string rutausercommandAdd = "";
            // variables
            bool bandera = true;
            bool banderawesafe = false;
            char delimiter = ',';
            string comando = "";
            string[] comandos = comando.Split(delimiter);
            string customerID = "";
            string cliente = "";
            string token = "";
            string valoretorno = "";
            string assetID = "";
            string ownerID = "";
            string tiporegistro = "DES";
            DataSet infoDatos = new DataSet();
            string log_usr = "";
            string log_pwd = "";
            string producto = "";
            string concesionario = "";
            string cadena = "";
            string cobertura = "";
            DataTable tabla = new DataTable();
            tabla.Columns.Add("CODIGO", typeof(int));
            tabla.Columns.Add("MENSAJE", typeof(string));
            // busqueda de ruta por el parametro
            if (p.parametroproducto == "" | string.IsNullOrEmpty(p.parametroproducto))
                p.parametroproducto = "GPT";
            if (p.parametroproducto == "GPT")
            {
                rutalogin = ruta.GetRuta("1");
                rutaassetAdd = ruta.GetRuta("2");
                rutacustomerAdd = ruta.GetRuta("3");
                rutadeviceAdd = ruta.GetRuta("4");
                rutacommandAdd = ruta.GetRuta("5");
                rutausercommandAdd = ruta.GetRuta("6");
            }            
            else
            {
                bandera = false;
                tabla.Rows.Add("-1", "No existe el parametro del producto");
            }
            
            // PROCESO DE WESAFE
            if (bandera & banderawesafe == true)
            {
                // proceso de wesafe
            }
            customerEntidad monitoreo = null;
            customerEntidad propietario = null;
            //customer cliente = null;
            deviceEntidad device = null;
            assetEntidad asset = null;
            // PROCESO DE AMI Y LATAM
            if (bandera & banderawesafe == false)
            {
                attributeEntidadCollection attributeEntidadCollection = null;
                attributeEntidad attributeCollection = null;
                string monitoreoAMI = "";
                string propietarioAMI = "";
                string deviceAMI = "";
                string assetAMI = "";
                // datos
                if (bandera)
                {
                    DataSet cnstGenrl = new DataSet();
                    cnstGenrl = BienSQL.CnstVehiculo(opcion: "100", cliente: "", vehiculo: "", parametroproducto: "GPT");
                    string support = (string)cnstGenrl.Tables[0].Rows[0]["support"];
                    log_usr = (string)cnstGenrl.Tables[0].Rows[0]["usuario"];
                    log_pwd = (string)cnstGenrl.Tables[0].Rows[0]["pass"];
                    string ami_telefono = (string)cnstGenrl.Tables[0].Rows[0]["telefono"];
                    string ami_celular = (string)cnstGenrl.Tables[0].Rows[0]["celular"];
                    string ruccompania = (string)cnstGenrl.Tables[0].Rows[0]["ruccompania"];
                    // informacion del cliente Monitoreo
                    if (bandera)
                    {
                        monitoreoAMI = ConsultaAMI.ProcesoAMIConsulta("200", p.codigovehiculo, p.producto, p.ordenservicio, p.itemNS, "");
                        monitoreo = new customerEntidad
                        {
                            username = funciones.descomponer("USERNAME", monitoreoAMI),
                            //username = "roberto@urbanofilms.com",
                            first_name = funciones.descomponer("FIRST_NAME", monitoreoAMI),
                            last_name = funciones.descomponer("LAST_NAME", monitoreoAMI),
                            email = funciones.descomponer("EMAIL", monitoreoAMI),
                            login = log_usr,
                            password = log_pwd,
                            internal_identifier = funciones.descomponer("ID_CLIENTE", monitoreoAMI),
                            active = "True",
                            telefono = funciones.descomponer("TELEFONO_CLIENTE", monitoreoAMI),
                            identity_customer_type = funciones.descomponer("TIPO", monitoreoAMI),
                            company_code = ruccompania,
                            business_name = "",
                            emergency_phone_number = ami_telefono,
                            assistance_phone_number = ami_celular,
                            technical_support_email = support,
                        };
                    }
                    // informacion del cliente
                    if (bandera)
                    {
                        propietarioAMI = ConsultaAMI.ProcesoAMIConsulta("201", p.codigovehiculo, p.producto, p.ordenservicio, p.itemNS, "");
                        propietario = new customerEntidad
                        {
                            username = funciones.descomponer("USERNAME", propietarioAMI),
                            //username = "roberto@urbanofilms.com",
                            first_name = funciones.descomponer("FIRST_NAME", propietarioAMI),
                            last_name = funciones.descomponer("LAST_NAME", propietarioAMI),
                            email = funciones.descomponer("EMAIL", propietarioAMI),
                            login = log_usr,
                            password = log_pwd,
                            internal_identifier = funciones.descomponer("ID_CLIENTE", propietarioAMI),
                            active = "True",
                            telefono = funciones.descomponer("TELEFONO_CLIENTE", propietarioAMI),
                            identity_customer_type = funciones.descomponer("TIPO", propietarioAMI),
                            company_code = ruccompania,
                            business_name = "",
                            emergency_phone_number = ami_telefono,
                            assistance_phone_number = ami_celular,
                            technical_support_email = support,
                        };
                    }
                    //device
                    if (bandera)
                    {
                        deviceAMI = ConsultaAMI.ProcesoAMIConsulta("202", p.codigovehiculo, p.producto, p.ordenservicio, p.itemNS, "");
                        //valorvid = funciones.descomponer("vid", deviceAMI);
                        device = new deviceEntidad
                        {
                            id = funciones.descomponer("serie", deviceAMI),
                            vid = funciones.descomponer("vid", deviceAMI),
                            company_code = ruccompania,
                            active = "True",
                            model = funciones.descomponer("id_telematic_modelo", deviceAMI),
                            report_from = funciones.descomponer("id_telematic_servidor", deviceAMI),
                            numero_celular = funciones.descomponer("noCelularSIM", deviceAMI),
                        };

                        if (device.vid == "0")
                        {
                            tabla.Rows.Add("-1", "Verificar el valor del VID ");
                            bandera = false;
                        }
                        if (device.report_from == "")
                        {
                            tabla.Rows.Add("-1", "Verificar el Servidor del Dispositivo ");
                            bandera = false;
                        }
                        if (device.model == "")
                        {
                            tabla.Rows.Add("-1", "Verificar el Modelo del Dispositivo ");
                            bandera = false;
                        }
                    }
                    // informacion del vehiculo
                    if (bandera)
                    {
                        assetAMI = ConsultaAMI.ProcesoAMIConsulta("203", p.codigovehiculo, p.producto, p.ordenservicio, p.itemNS, "");
                        // string codigosys = "";
                        if (funciones.descomponer("familiadescripcion", assetAMI) == "HUNTER GPS HMT")
                        {
                            producto = "H.MONITOREO AMI";
                        }
                        else if (funciones.descomponer("familiadescripcion", assetAMI) == "AMI CHANGAN BY HUNTER")
                        {
                            producto = "H.MONITOREO AMI";
                        }
                        else if (funciones.descomponer("familiadescripcion", assetAMI) == "ANDOR BY HUNTER")
                        {
                            producto = "ANDOR LINK BY HUNTER";
                        }
                        else
                        {
                            producto = funciones.descomponer("familiadescripcion", assetAMI);
                        }
                        concesionario = funciones.descomponer("concesionarios", assetAMI);

                        cnstGenrl = BienSQL.CnstProducto(opcion: "104", concesionario: concesionario, producto: p.producto);
                        if (cnstGenrl.Tables[0].Rows.Count > 0)
                        {
                            producto = (string)cnstGenrl.Tables[0].Rows[0]["NOMBRE_PRODUCTO"];
                        }
                        comando = (string)funciones.descomponer("comandoami", assetAMI);
                        comandos = comando.Split(delimiter);
                        asset = new assetEntidad
                        {
                            name = funciones.descomponer("placa", assetAMI),
                            assettype = funciones.descomponer("tipo", assetAMI),
                            // codigosys= codigosys, //CODIGO SYS O NETSUITE
                            codigosys = p.codigosys, //CODIGO SYS O NETSUITE
                            codigonetsuite = p.codigovehiculo,
                            customname = "",
                            active = "True",
                            description = "PLC.:" + funciones.descomponer("placa", assetAMI) + ";MAR.:" + funciones.descomponer("marca", assetAMI) + ";MOD.:" + funciones.descomponer("modelo", assetAMI) + ";COL.:" + funciones.descomponer("color", assetAMI),
                            productexpiredate = p.cobertura,
                            product = producto,
                            chasis = funciones.descomponer("chasis", assetAMI),
                        };                      
                    }
                }
                //login
                if (bandera)
                {
                    var client = new RestClient(rutalogin);
                    var request = new RestRequest("", Method.Post);
                    request.AddParameter("username", monitoreo.login);
                    request.AddParameter("password", monitoreo.password);
                    RestResponse response = client.Execute(request);
                    var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(response.Content);
                    token = funciones.descomponer("token", jsonString);                 
                }
                //bloque cliente monitoreo
                if (bandera)
                {
                    var customersearch = new RestClient();
                    var requestecustomersearch = new RestRequest();
                    RestResponse responsecustomersearch;
                    customersearch = new RestClient(rutacustomerAdd + "?username=" + monitoreo.username + "&is_active=" + true + "&customer__company_code=" + monitoreo.company_code + " ");
                    requestecustomersearch = new RestRequest("", Method.Get);
                    requestecustomersearch.AddHeader("cache-control", "no-cache");
                    requestecustomersearch.AddHeader("authorization", "HunterAPI " + token);
                    responsecustomersearch = customersearch.Execute(requestecustomersearch);
                    valoretorno = "0";
                    valoretorno = funciones.descomponer("id", responsecustomersearch.Content);
                    if (valoretorno != "0")
                    {
                        customerID = valoretorno;
                        bandera = true;
                    }
                    else
                    {
                        tabla.Rows.Add("-1", "Error al buscar el Customer " );
                        bandera = false;
                    }
                }
                //bloque cliente propietario
                if (bandera)
                {
                    var customersearch = new RestClient();
                    var requestecustomersearch = new RestRequest();
                    RestResponse responsecustomersearch;
                    customersearch = new RestClient(rutacustomerAdd + "?username=" + propietario.username + "&is_active=" + true + "&customer__company_code=" + propietario.company_code + " ");
                    requestecustomersearch = new RestRequest("", Method.Get);
                    requestecustomersearch.AddHeader("cache-control", "no-cache");
                    requestecustomersearch.AddHeader("authorization", "HunterAPI " + token);
                    responsecustomersearch = customersearch.Execute(requestecustomersearch);
                    valoretorno = "0";
                    valoretorno = funciones.descomponer("id", responsecustomersearch.Content);
                    if (valoretorno != "0")
                    {
                        ownerID = valoretorno;
                        bandera = true;
                    }
                    else
                    {
                        tabla.Rows.Add("-1", "Error al buscar el Customer Propietario ");
                        bandera = false;
                    }
                }
                // bloque del device
                if (bandera)
                {
                    var devicesearch = new RestClient();
                    var requestedevicesearch = new RestRequest();
                    RestResponse responsedevicesearch;
                    var clientedevices = new RestClient();
                    var requestedevice = new RestRequest();
                    RestResponse responsedevice;
                    // consulta si existe el device y si se encuentra activo
                    devicesearch = new RestClient(rutadeviceAdd + "?active=" + true + "&id=" + device.vid + "&company_code=" + device.company_code + " ");
                    requestedevicesearch = new RestRequest("", Method.Get);
                    requestedevicesearch.AddHeader("cache-control", "no-cache");
                    requestedevicesearch.AddHeader("authorization", "HunterAPI " + token);
                    responsedevicesearch = devicesearch.Execute(requestedevicesearch);
                    valoretorno = "0";
                    valoretorno = funciones.descomponer("id", responsedevicesearch.Content);
                    if (valoretorno != "0")
                    {
                        clientedevices = new RestClient(rutadeviceAdd + valoretorno + "/ ");
                        requestedevice = new RestRequest("", Method.Patch);
                        requestedevice.AddHeader("cache-control", "no-cache");
                        requestedevice.AddHeader("authorization", "HunterAPI " + token);
                        requestedevice.AddParameter("id", valoretorno);
                        requestedevice.AddParameter("active", false);
                        responsedevice = clientedevices.Execute(requestedevice);
                        if (Convert.ToInt32(responsedevice.StatusCode) == 200)
                            bandera = true;
                            //validaciondesintalacion = "N";
                        else if (Convert.ToInt32(responsedevice.StatusCode) == 400)
                        {
                            string mensaje = funciones.DevuelveMensaje(responsedevice.Content);
                            tabla.Rows.Add("-1", mensaje + " " + responsedevice.StatusCode + " device Actualiza");
                            bandera = false;
                        }
                        else
                        {
                            tabla.Rows.Add("-1", "Error al procesar devices " + responsedevice.StatusCode + " device Actualiza");
                            bandera = false;
                        }
                    }  
                }
                // bloque del vehiculo
                if (bandera)
                {
                    var assetSearch = new RestClient();
                    var requesteAssetSearch = new RestRequest();
                    RestResponse responseAssetSearch;
                    string texto;
                    var clienteasset = new RestClient();
                    var requesteasset = new RestRequest();
                    RestResponse responseasset;
                    assetSearch = new RestClient(rutaassetAdd + "?active=" + true + "&devices__device__company_code=" + device.company_code + "&attributes__attribute__name=COD_SYS" + "&attributes__value=" + asset.codigosys + " ");
                    requesteAssetSearch = new RestRequest("", Method.Get);
                    requesteAssetSearch.AddHeader("cache-control", "no-cache");
                    requesteAssetSearch.AddHeader("authorization", "HunterAPI " + token);
                    responseAssetSearch = assetSearch.Execute(requesteAssetSearch);
                    valoretorno = "0";
                    valoretorno = funciones.descomponer("id", responseAssetSearch.Content);
                    if (valoretorno != "0")
                    {
                        cobertura = funciones.descomponer("product_expire_date", responseAssetSearch.Content).Substring(0, 10);
                        producto = funciones.descomponer("product", responseAssetSearch.Content).Substring(0, 10);
                        clienteasset = new RestClient(rutaassetAdd + valoretorno + "/ ");
                        requesteasset = new RestRequest("", Method.Patch);
                        requesteasset.AddHeader("cache-control", "no-cache");
                        requesteasset.AddHeader("authorization", "HunterAPI " + token);
                        requesteasset.AddParameter("id", valoretorno);
                        requesteasset.AddParameter("active", false);
                        responseasset = clienteasset.Execute(requesteasset);
                        if (Convert.ToInt32(responseasset.StatusCode) == 200)
                        {
                            assetID = funciones.descomponer("id", responseasset.Content);
                            bandera = true;
                        }
                        else if (Convert.ToInt32(responseasset.StatusCode) == 400)
                        {
                            string mensaje = funciones.DevuelveMensaje(responseasset.Content);
                            tabla.Rows.Add("-1", mensaje + " " + responseasset.StatusCode + " asset Actualiza");
                            bandera = false;
                        }
                        else
                        {
                            tabla.Rows.Add("-1", "Error al procesar asset " + responseasset.StatusCode + " asset Actualiza");
                            bandera = false;
                        }
                    }
                    else
                    {
                        //tabla.Rows.Add("-1", "Error al buscar el asset");
                        //bandera = false;
                        assetSearch = new RestClient();
                        requesteAssetSearch = new RestRequest();
                        clienteasset = new RestClient();
                        requesteasset = new RestRequest();
                        assetSearch = new RestClient(rutaassetAdd + "?active=" + false + "&devices__device__company_code=" + device.company_code + "&attributes__attribute__name=COD_SYS" + "&attributes__value=" + asset.codigosys + " ");
                        requesteAssetSearch = new RestRequest("", Method.Get);
                        requesteAssetSearch.AddHeader("cache-control", "no-cache");
                        requesteAssetSearch.AddHeader("authorization", "HunterAPI " + token);
                        responseAssetSearch = assetSearch.Execute(requesteAssetSearch);
                        valoretorno = "0";
                        valoretorno = funciones.descomponer("id", responseAssetSearch.Content);
                        if (valoretorno != "0")
                        {
                            assetID = valoretorno;
                            bandera = true;
                        }
                        else
                        {
                            //tabla.Rows.Add("-1", "Error al buscar el asset");
                            //bandera = false;
                            assetSearch = new RestClient(rutaassetAdd + "?active=" + false +  "&attributes__attribute__name=COD_SYS" + "&attributes__value=" + asset.codigosys + " ");
                            requesteAssetSearch = new RestRequest("", Method.Get);
                            requesteAssetSearch.AddHeader("cache-control", "no-cache");
                            requesteAssetSearch.AddHeader("authorization", "HunterAPI " + token);
                            responseAssetSearch = assetSearch.Execute(requesteAssetSearch);
                            valoretorno = "0";
                            valoretorno = funciones.descomponer("id", responseAssetSearch.Content);
                            if (valoretorno != "0")
                            {
                                assetID = valoretorno;
                                bandera = true;
                            }
                            else
                            {
                                tabla.Rows.Add("-1", "Error al buscar el asset");
                                bandera = false;
                            }
                        }
                    }
                }
                // bloque  de  vehiculo - usuario
                if (bandera)
                {
                    RestResponse responseassetuser;
                    var assetUserSearch = new RestClient(rutaassetAdd + "?devices__device__company_code=" + device.company_code + "&attributes__attribute__name=COD_SYS" + "&attributes__value=" + asset.codigosys + " ");
                    var requesteassetUserSearch = new RestRequest("", Method.Get);
                    requesteassetUserSearch.AddHeader("cache-control", "no-cache");
                    requesteassetUserSearch.AddHeader("authorization", "HunterAPI " + token);
                    RestResponse responseUserSearch = assetUserSearch.Execute(requesteassetUserSearch);
                    cadena = "0";
                    cadena = funciones.descomponer("user", responseUserSearch.Content);
                    cadena = cadena.Replace("[", "").Replace("]", "");
                    if (cadena != "0")
                    {
                        string[] datousers = cadena.Split(delimiter);
                        foreach (var usercustomers in datousers)
                        {
                            var clienteassetuser = new RestClient(rutaassetAdd + assetID + "/user-remove/ ");
                            var requesteassetuser = new RestRequest("", Method.Patch);
                            requesteassetuser.AddHeader("cache-control", "no-cache");
                            requesteassetuser.AddHeader("authorization", "HunterAPI " + token);
                            requesteassetuser.AddParameter("id", assetID);
                            requesteassetuser.AddParameter("user_id", usercustomers);
                            responseassetuser = clienteassetuser.Execute(requesteassetuser);
                            if (Convert.ToInt32(responseassetuser.StatusCode) == 200)
                            {
                                bandera = true;
                            }
                            else if (Convert.ToInt32(responseassetuser.StatusCode) == 400)
                            {
                                string mensaje = funciones.DevuelveMensaje(responseassetuser.Content);
                                tabla.Rows.Add("-1", mensaje + " " + responseassetuser.StatusCode + " asset-usuario Chequeo ");
                                bandera = false;
                            }
                            else
                            {
                                tabla.Rows.Add("-1", "Error al procesar assetuser " + responseassetuser.StatusCode + " asset-usuario Chequeo ");
                                bandera = false;
                            }
                        }                        
                    }
                    else
                    {
                        bandera = true;
                    }                    
                }
                // bloque  de  vehiculo - device
                if (bandera)
                {
                    var assetDevSearch = new RestClient(rutaassetAdd + "?devices__device__id=" + device.vid + "&devices__device__company_code=" + device.company_code + "&attributes__attribute__name=COD_SYS" + "&attributes__value=" + asset.codigosys + " ");
                    var requesteassetDevSearch = new RestRequest("", Method.Get);
                    requesteassetDevSearch.AddHeader("cache-control", "no-cache");
                    requesteassetDevSearch.AddHeader("authorization", "HunterAPI " + token);
                    RestResponse responseassetDevSearch = assetDevSearch.Execute(requesteassetDevSearch);
                    valoretorno = "0";
                    valoretorno = funciones.descomponer("id", responseassetDevSearch.Content);
                    if (valoretorno != "0")
                    {
                        if (valoretorno == assetID)
                        {
                            var clienteassetdev = new RestClient(rutaassetAdd + assetID + "/device-remove/ ");
                            var requesteassetdev = new RestRequest("", Method.Patch);
                            requesteassetdev.AddHeader("cache-control", "no-cache");
                            requesteassetdev.AddHeader("authorization", "HunterAPI " + token);
                            requesteassetdev.AddParameter("id", assetID);
                            requesteassetdev.AddParameter("device_id", device.vid);
                            RestResponse responseassetdev = clienteassetdev.Execute(requesteassetdev);
                            if (Convert.ToInt32(responseassetdev.StatusCode) == 200)
                                bandera = true;
                            else if (Convert.ToInt32(responseassetdev.StatusCode) == 400)
                            {
                                tabla.Rows.Add("-1", responseassetdev.StatusCode + " asset-device");
                                bandera = false;
                            }
                            else
                            {
                                tabla.Rows.Add("-1", "Error al procesar assetdevice " + responseassetdev.StatusCode + " asset-device");
                                bandera = false;
                            }
                        }
                        else
                        {
                            tabla.Rows.Add("-1", "Error al procesar asset Id distinto remove device");
                            bandera = false;
                        }
                    }
                    else   
                    {
                        bandera = true;
                    }
                }
                // bloque  de  vehiculo - comando
                if (bandera)
                {
                    var commandSearch = new RestClient();
                    var requesteCommandSearch = new RestRequest();
                    RestResponse responseCommandSearch;
                    var clientecommand = new RestClient();
                    var requestecommand = new RestRequest();
                    RestResponse responsecommand;
                    foreach (var vehcomando in comandos)
                    {
                        // consulta si existe la relacion vehiculo - comando - activos
                        commandSearch = new RestClient(rutacommandAdd + "?asset=" + assetID + "&command=" + comando + "&status=" + 1 + " ");
                        requesteCommandSearch = new RestRequest("", Method.Get);
                        requesteCommandSearch.AddHeader("cache-control", "no-cache");
                        requesteCommandSearch.AddHeader("authorization", "HunterAPI " + token);
                        responseCommandSearch = commandSearch.Execute(requesteCommandSearch);
                        valoretorno = "0";
                        valoretorno = funciones.descomponer("id", responseCommandSearch.Content);
                        if (valoretorno != "0")
                        {
                            clientecommand = new RestClient(rutacommandAdd + valoretorno + "/ ");
                            requestecommand = new RestRequest("", Method.Patch);
                            requestecommand.AddHeader("cache-control", "no-cache");
                            requestecommand.AddHeader("authorization", "HunterAPI " + token);
                            requestecommand.AddParameter("id", valoretorno);
                            requestecommand.AddParameter("status", 0);
                            responsecommand = clientecommand.Execute(requestecommand);
                            if (Convert.ToInt32(responsecommand.StatusCode) == 200)
                                bandera = true;
                            else if (Convert.ToInt32(responsecommand.StatusCode) == 400)
                            {
                                string mensaje = funciones.DevuelveMensaje(responsecommand.Content);
                                tabla.Rows.Add("-1", mensaje + " " + responsecommand.StatusCode + " asset-comando ");
                                bandera = false;
                                break;
                            }
                            else
                            {
                                tabla.Rows.Add("-1", "Error al procesar asset command " + responsecommand.StatusCode + " asset-comando");
                                bandera = false;
                                break;
                            }
                        } 
                    }                   
                }
                // bloque  de  comando
                if (bandera)
                {
                    var userCommandSearch = new RestClient();
                    var requesteuserCommandSearch = new RestRequest();
                    RestResponse responseuserCommandSearch;
                    var clienteusercommand = new RestClient();
                    var requesteusercommand = new RestRequest();
                    RestResponse responseusercommand;
                    foreach (var vehcomando in comandos)
                    {
                        // consulta si existe el comando por vehiculo - comando - usuario - estado 
                        userCommandSearch = new RestClient(rutausercommandAdd + "?asset=" + assetID + "&command=" + comando + "&user=" + customerID + "&can_execute=" + true + " ");
                        requesteuserCommandSearch = new RestRequest("", Method.Get);
                        requesteuserCommandSearch.AddHeader("cache-control", "no-cache");
                        requesteuserCommandSearch.AddHeader("authorization", "HunterAPI " + token);
                        responseuserCommandSearch = userCommandSearch.Execute(requesteuserCommandSearch);
                        valoretorno = "0";
                        valoretorno = funciones.descomponer("id", responseuserCommandSearch.Content);
                        if (valoretorno != "0")
                        {
                            clienteusercommand = new RestClient(rutausercommandAdd + valoretorno + "/ ");
                            requesteusercommand = new RestRequest("", Method.Patch);
                            requesteusercommand.AddHeader("cache-control", "no-cache");
                            requesteusercommand.AddHeader("authorization", "HunterAPI " + token);
                            requesteusercommand.AddParameter("id", valoretorno);
                            requesteusercommand.AddParameter("can_execute", false);
                            responseusercommand = clienteusercommand.Execute(requesteusercommand);
                            if (Convert.ToInt32(responseusercommand.StatusCode) == 200)
                                bandera = true;
                            else if (Convert.ToInt32(responseusercommand.StatusCode) == 400)
                            {
                                string mensaje = funciones.DevuelveMensaje(responseusercommand.Content);
                                tabla.Rows.Add("-1", mensaje + " " + responseusercommand.StatusCode + " comando-usuario ");
                                bandera = false;
                                break;
                            }
                            else
                            {
                                tabla.Rows.Add("-1", "Error al procesar user asset command " + responseusercommand.StatusCode + " comando-usuario ");
                                bandera = false;
                                break;
                            }
                        }
                    }   
                }
            }
            //ingreso del log del proceso de AMI
            if (bandera)
            {
                string resultadoLog = ConsultaAMI.RegistroLog(p.clientepropietario, ownerID, p.clientemonitoreo, customerID, p.codigosys, assetID, p.comando, device.id, producto, p.ordenservicio, "0", p.usuario, "N", monitoreo.username  , tiporegistro, propietario.username, p.origen, "002", "", "", "", "", "", "", "", "", "", "", p.parametroproducto, p.cobertura, device.vid, producto, concesionario);
                if (resultadoLog == "OK")
                {
                    //mensaje = "OK";
                    bandera = true;
                }
                else
                {
                    tabla.Rows.Add("-1", "Error en el Ingreso del Log");
                    bandera = false;
                }
            }
            string jsonData;
            if (bandera)
            {
                tabla.Rows.Add(1, "Guardado Exitosamente");
            }
            jsonData = (string)Utilidades.DataTableToJSON(tabla);
            return jsonData;
        }


        [HttpPost]
        [Authorize]
        [SwaggerOperation(Summary = "", Description = "Actualiza datos del cliente")]
        [Route("ActualizacionCustomer")]
        public ActionResult<string> ActualizacionCustomer(GeneralAMIDTO p)
        {
            //conexion ruta 
            string rutalogin = "";
            string rutacustomerAdd = "";
            //variables
            bool bandera = true;
            string token = "";
            string valoretorno = "";
            string tiporegistro = "CUS";
            string busquedaAmi = "0";
            string busquedaAmiOwner = "0";
            string log_usr = "";
            string log_pwd = "";
            DataTable tabla = new DataTable();
            tabla.Columns.Add("CODIGO", typeof(int));
            tabla.Columns.Add("MENSAJE", typeof(string));
            conexionAMI ruta = new conexionAMI();
            if (p.parametroproducto == "GPT")
            {
                rutalogin = ruta.GetRuta("1");
                rutacustomerAdd = ruta.GetRuta("3");
            }
            else
            {
                bandera = false;
                tabla.Rows.Add("-1", "No existe el parametro del producto");
            }
            customerEntidad monitoreo = null;
            customerEntidad propietario = null;
            string monitoreoAMI = "";
            string propietarioAMI = "";
            string accountType = "1";
            // datos
            if (bandera)
            {
                DataSet cnstGenrl = new DataSet();
                cnstGenrl = BienSQL.CnstVehiculo(opcion: "100", cliente: "", vehiculo: "", parametroproducto: "GPT");
                string support = (string)cnstGenrl.Tables[0].Rows[0]["support"];
                log_usr = (string)cnstGenrl.Tables[0].Rows[0]["usuario"];
                log_pwd = (string)cnstGenrl.Tables[0].Rows[0]["pass"];
                string ami_telefono = (string)cnstGenrl.Tables[0].Rows[0]["telefono"];
                string ami_celular = (string)cnstGenrl.Tables[0].Rows[0]["celular"];
                string ruccompania = (string)cnstGenrl.Tables[0].Rows[0]["ruccompania"];
                // informacion del cliente Monitoreo
                //if (bandera)
                //{
                //    monitoreoAMI = ConsultaAMI.ProcesoAMIConsulta("206", "", "", "", "", p.clienteid);
                //    busquedaAmi = funciones.descomponer("ID_TELEMATIC", monitoreoAMI);
                //    monitoreo = new customerEntidad
                //    {
                //        username = funciones.descomponer("USERNAME", monitoreoAMI),
                //        first_name = funciones.descomponer("FIRST_NAME", monitoreoAMI),
                //        last_name = funciones.descomponer("LAST_NAME", monitoreoAMI),
                //        email = funciones.descomponer("EMAIL", monitoreoAMI),
                //        login = log_usr,
                //        password = log_pwd,
                //        internal_identifier = funciones.descomponer("ID_CLIENTE", monitoreoAMI),
                //        active = "True",
                //        telefono = funciones.descomponer("TELEFONO_CLIENTE", monitoreoAMI),
                //        identity_customer_type = funciones.descomponer("TIPO", monitoreoAMI),
                //        company_code = ruccompania,
                //        business_name = "",
                //        emergency_phone_number = ami_telefono,
                //        assistance_phone_number = ami_celular,
                //        technical_support_email = support,
                //    };
                //}

                // informacion del cliente
                if (bandera)
                {
                    propietarioAMI = ConsultaAMI.ProcesoAMIConsulta("206", "", "", "", "", p.clienteid);
                    busquedaAmiOwner = funciones.descomponer("ID_TELEMATIC", propietarioAMI);
                    propietario = new customerEntidad
                    {
                        username = funciones.descomponer("USERNAME", propietarioAMI),
                        first_name = funciones.descomponer("FIRST_NAME", propietarioAMI),
                        last_name = funciones.descomponer("LAST_NAME", propietarioAMI),
                        email = funciones.descomponer("EMAIL", propietarioAMI),
                        login = log_usr,
                        password = log_pwd,
                        internal_identifier = funciones.descomponer("ID_CLIENTE", propietarioAMI),
                        active = "True",
                        telefono = funciones.descomponer("TELEFONO_CLIENTE", propietarioAMI),
                        identity_customer_type = funciones.descomponer("TIPO", propietarioAMI),
                        company_code = ruccompania,
                        business_name = "",
                        emergency_phone_number = ami_telefono,
                        assistance_phone_number = ami_celular,
                        technical_support_email = support,
                    };
                }

                //login
                if (bandera)
                {
                    if (propietario.username=="")
                    {
                        bandera = false;
                        tabla.Rows.Add("-1", "El cliente Propietario USERNAME no existe en AMI");
                    }
                    else if (monitoreo.username == "")
                    {
                        bandera = false;
                        tabla.Rows.Add("-1", "El monitoreo USERNAME no existe en AMI");
                    }
                    else if (busquedaAmi == "" && busquedaAmiOwner == "" )
                    {
                        bandera = false;
                        tabla.Rows.Add("-1", "No existe el cliente en AMI");
                    }
                    else
                    {
                        var client = new RestClient(rutalogin);
                        var request = new RestRequest("", Method.Post);
                        request.AddParameter("username", propietario.login);
                        request.AddParameter("password", propietario.password);
                        RestResponse response = client.Execute(request);
                        var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(response.Content);
                        token = funciones.descomponer("token", jsonString);
                    }  
                }
            }
            //bloque del cliente y cliente monitoreo
            if (bandera)
            {
                var customersearch = new RestClient();
                var requestecustomersearch = new RestRequest();
                RestResponse responsecustomersearch;
                var customer = new RestClient();
                var requeste = new RestRequest();
                RestResponse responsecustomer;
                if (busquedaAmi != "0")
                {
                    customersearch = new RestClient(rutacustomerAdd + "?customer__identity_document_number=" + monitoreo.internal_identifier + "&customer__company_code=" + monitoreo.company_code + "&id=" + busquedaAmi + " ");
                }else  if (busquedaAmiOwner != "0")
                {
                    customersearch = new RestClient(rutacustomerAdd + "?customer__identity_document_number=" + propietario.internal_identifier + "&customer__company_code=" + propietario.company_code + "&id=" + busquedaAmiOwner + " ");
                }
                requestecustomersearch = new RestRequest("", Method.Get);
                requestecustomersearch.AddHeader("cache-control", "no-cache");
                requestecustomersearch.AddHeader("authorization", "HunterAPI " + token);
                responsecustomersearch = customersearch.Execute(requestecustomersearch);
                valoretorno = "0";
                valoretorno = funciones.descomponer("id", responsecustomersearch.Content);
                if (valoretorno != "0")
                {
                    if (valoretorno == busquedaAmi)
                    {
                        //    customer = new RestClient(rutacustomerAdd + valoretorno + "/ ");
                        //    requeste = new RestRequest("",Method.Patch);
                        //    requeste.AddHeader("cache-control", "no-cache");
                        //    requeste.AddHeader("authorization", "HunterAPI " + token);
                        //    requeste.AddParameter("id", valoretorno);
                        //    requeste.AddParameter("username", monitoreo.username);
                        //    requeste.AddParameter("first_name", monitoreo.first_name);
                        //    requeste.AddParameter("last_name", monitoreo.last_name);
                        //    requeste.AddParameter("email", monitoreo.email);
                        //    requeste.AddParameter("is_active", monitoreo.active);
                        //    requeste.AddParameter("customer.company_code", monitoreo.company_code);
                        //    requeste.AddParameter("customer.identity_document_type", monitoreo.identity_customer_type);
                        //    requeste.AddParameter("customer.identity_document_number", monitoreo.internal_identifier);
                        //    requeste.AddParameter("customer.business_name", "");
                        //    requeste.AddParameter("customer.phone_number", monitoreo.assistance_phone_number);
                        //    requeste.AddParameter("customer.emergency_phone_number", monitoreo.emergency_phone_number);
                        //    requeste.AddParameter("customer.assistance_phone_number", monitoreo.assistance_phone_number);
                        //    requeste.AddParameter("customer.technical_support_email", monitoreo.technical_support_email);
                        //    requeste.AddParameter("customer.account_type", accountType);
                        //    responsecustomer = customer.Execute(requeste);
                        //    if (Convert.ToInt32(responsecustomer.StatusCode) == 200)
                        //    {
                        //        bandera = true;
                        //    }
                        //    else if (Convert.ToInt32(responsecustomer.StatusCode) == 400)
                        //    {
                        //        string mensaje = funciones.DevuelveMensaje(responsecustomer.Content);
                        //        tabla.Rows.Add("-1", mensaje + " " + responsecustomer.StatusCode + " Customer Actualizando");
                        //        bandera = false;
                        //    }
                        //    else
                        //    {
                        //        tabla.Rows.Add("-1", "Error al procesar customer " + responsecustomer.StatusCode + " Customer Actualizando");
                        //        bandera = false;
                        //    }
                    } else if ( valoretorno == busquedaAmiOwner)
                    {
                        customer = new RestClient(rutacustomerAdd + valoretorno + "/ ");
                        requeste = new RestRequest("", Method.Patch);
                        requeste.AddHeader("cache-control", "no-cache");
                        requeste.AddHeader("authorization", "HunterAPI " + token);
                        requeste.AddParameter("id", valoretorno);
                        requeste.AddParameter("username", propietario.username);
                        requeste.AddParameter("first_name", propietario.first_name);
                        requeste.AddParameter("last_name", propietario.last_name);
                        requeste.AddParameter("email", propietario.email);
                        requeste.AddParameter("is_active", propietario.active);
                        requeste.AddParameter("customer.company_code", propietario.company_code);
                        requeste.AddParameter("customer.identity_document_type", propietario.identity_customer_type);
                        requeste.AddParameter("customer.identity_document_number", propietario.internal_identifier);
                        requeste.AddParameter("customer.business_name", "");
                        requeste.AddParameter("customer.phone_number", propietario.assistance_phone_number);
                        requeste.AddParameter("customer.emergency_phone_number", propietario.emergency_phone_number);
                        requeste.AddParameter("customer.assistance_phone_number", propietario.assistance_phone_number);
                        requeste.AddParameter("customer.technical_support_email", propietario.technical_support_email);
                        requeste.AddParameter("customer.account_type", accountType);
                        responsecustomer = customer.Execute(requeste);
                        if (Convert.ToInt32(responsecustomer.StatusCode) == 200)
                        {
                            bandera = true;
                        }
                        else if (Convert.ToInt32(responsecustomer.StatusCode) == 400)
                        {
                            string mensaje = funciones.DevuelveMensaje(responsecustomer.Content);
                            tabla.Rows.Add("-1", mensaje + " " + responsecustomer.StatusCode + " Customer Actualizando Propietario");
                            bandera = false;
                        }
                        else
                        {
                            tabla.Rows.Add("-1", "Error al procesar customer " + responsecustomer.StatusCode + " Customer Actualizando Propietario");
                            bandera = false;
                        }
                    }
                    else
                    {
                        tabla.Rows.Add("-1", "busqueda incorrecta" + " Customer Actualizando");
                        bandera = false;
                    }
                }
                else
                {
                    tabla.Rows.Add("-1", "No se encuentra customer para Actualizarlo");
                    bandera = false;
                }
            }
            // ingreso del log del proceso de AMI
            if (bandera)
            {
                string resultadoLog = "";
                if (busquedaAmi != "0")
                {
                    resultadoLog = ConsultaAMI.RegistroLog("0", "0", monitoreo.internal_identifier, busquedaAmi, "", "", "", "", "", "0", "0", p.usuario, "N", monitoreo.username, tiporegistro, "", p.origen, "", "", "", "", "", "", "", "", "", "", "", p.parametroproducto, "1900-01-01", "", p.producto, "");
                }
                else if (busquedaAmiOwner != "0")
                {
                    resultadoLog = ConsultaAMI.RegistroLog(propietario.internal_identifier, busquedaAmiOwner, "0", "0", "", "", "", "", "", "0", "0", p.usuario, "N", propietario.username, tiporegistro, "", p.origen, "", "", "", "", "", "", "", "", "", "", "", p.parametroproducto, "1900-01-01", "", p.producto, "");
                }

                if (resultadoLog == "OK")
                {
                    bandera = true;
                }
                else
                {
                    tabla.Rows.Add("-1", "Error en el Ingreso del Log");
                    bandera = false;
                }
            }
            string jsonData;
            if (bandera)
            {
                tabla.Rows.Add(1, "Guardado Exitosamente");
            }
            jsonData = (string)Utilidades.DataTableToJSON(tabla);

            return jsonData;

        }


        [HttpPost]
        [Authorize]
        [SwaggerOperation(Summary = "", Description = "Actualiza datos del Vehiculo")]
        [Route("ActualizacionAsset")]
        public ActionResult<string> ActualizacionAsset(GeneralAMIDTO p)
        {
            //conexion ruta 
            string rutalogin = "";
            string rutaassetAdd = "";
            string rutacustomerAdd = "";
            //variables
            bool bandera = true;
            //DataSet infoDatos = new DataSet();
            string token = "";
            string valoretorno = "";
            string customerID = "";
            string assetID = "";
            string tiporegistro = "VEH";
            string log_usr = "";
            string log_pwd = "";
            string producto = "";
            string cobertura = "";
            string concesionario = "";
            DataTable tabla = new DataTable();
            tabla.Columns.Add("CODIGO", typeof(int));
            tabla.Columns.Add("MENSAJE", typeof(string));
            conexionAMI ruta = new conexionAMI();
            if (p.parametroproducto == "GPT")
            {
                rutalogin = ruta.GetRuta("1");
                rutaassetAdd = ruta.GetRuta("2");
                rutacustomerAdd = ruta.GetRuta("3");
            }
            else
            {
                bandera = false;
                tabla.Rows.Add("-1", "No existe el parametro del producto");
            }
            customerEntidad cliente = null;
            assetEntidad asset = null;
            attributeEntidadCollection attributeEntidadCollection = null;
            attributeEntidad attributeCollection = null;
            string busquedaAmiVehiculo = "";
            string busquedaAmiCliente = "";
            //string accion = "";
            string clienteAMI = "";
            string assetAMI = "";
            // datos
            if (bandera)
            {
                DataSet cnstGenrl = new DataSet();
                cnstGenrl = BienSQL.CnstVehiculo(opcion: "100", cliente: "", vehiculo: "", parametroproducto: "GPT");
                string support = (string)cnstGenrl.Tables[0].Rows[0]["support"];
                log_usr = (string)cnstGenrl.Tables[0].Rows[0]["usuario"];
                log_pwd = (string)cnstGenrl.Tables[0].Rows[0]["pass"];
                string ami_telefono = (string)cnstGenrl.Tables[0].Rows[0]["telefono"];
                string ami_celular = (string)cnstGenrl.Tables[0].Rows[0]["celular"];
                string ruccompania = (string)cnstGenrl.Tables[0].Rows[0]["ruccompania"];

                // informacion del cliente
                if (bandera)
                {
                    //consulta del cliente por codigo interno de netsuite
                    clienteAMI = ConsultaAMI.ProcesoAMIConsulta("206", "", "", "", "", p.clienteid);
                    busquedaAmiCliente = funciones.descomponer("ID_TELEMATIC", clienteAMI);
                    cliente = new customerEntidad
                    {
                        username = funciones.descomponer("USERNAME", clienteAMI),
                        first_name = funciones.descomponer("FIRST_NAME", clienteAMI),
                        last_name = funciones.descomponer("LAST_NAME", clienteAMI),
                        email = funciones.descomponer("EMAIL", clienteAMI),
                        login = log_usr,
                        password = log_pwd,
                        internal_identifier = funciones.descomponer("ID_CLIENTE", clienteAMI),
                        active = "True",
                        telefono = funciones.descomponer("TELEFONO_CLIENTE", clienteAMI),
                        identity_customer_type = funciones.descomponer("TIPO", clienteAMI),
                        company_code = ruccompania,
                        business_name = "",
                        emergency_phone_number = ami_telefono,
                        assistance_phone_number = ami_celular,
                        technical_support_email = support,
                    };
                }

                // informacion vehiculo y atributos
                if (bandera)
                {
                    // informacion del vehiculo por codigo interno de netsuite
                    assetAMI = ConsultaAMI.ProcesoAMIConsulta("207", p.vehiculoid, "", "", "", "");
                    busquedaAmiVehiculo = funciones.descomponer("ID_TELEMATIC", assetAMI);
                    //if (funciones.descomponer("familiadescripcion", assetAMI) == "HUNTER GPS HMT")
                    //{
                    //    producto = "H.MONITOREO AMI";
                    //}
                    //else if (funciones.descomponer("familiadescripcion", assetAMI) == "AMI CHANGAN BY HUNTER")
                    //{
                    //    producto = "H.MONITOREO AMI";
                    //}
                    //else if (funciones.descomponer("familiadescripcion", assetAMI) == "ANDOR BY HUNTER")
                    //{
                    //    producto = "ANDOR LINK BY HUNTER";
                    //}
                    //else
                    //{
                    //    producto = funciones.descomponer("familiadescripcion", assetAMI);
                    //}
                    //concesionario = funciones.descomponer("concesionarios", assetAMI);
                    //cnstGenrl = BienSQL.CnstProducto(opcion: "104", concesionario: concesionario);
                    //if (cnstGenrl.Tables[0].Rows.Count > 0)
                    //{
                    //    producto = (string)cnstGenrl.Tables[0].Rows[0]["NOMBRE_PRODUCTO"];
                    //}
                    asset = new assetEntidad
                    {
                        name = funciones.descomponer("placa", assetAMI),
                        assettype = funciones.descomponer("tipo", assetAMI),
                        // codigosys= codigosys, //CODIGO SYS O NETSUITE
                        codigosys = p.codigosys, //CODIGO SYS O NETSUITE
                        codigonetsuite = p.codigovehiculo,
                        customname = "",
                        active = "True",
                        description = "PLC.:" + funciones.descomponer("placa", assetAMI) + ";MAR.:" + funciones.descomponer("marca", assetAMI) + ";MOD.:" + funciones.descomponer("modelo", assetAMI) + ";COL.:" + funciones.descomponer("color", assetAMI),
                        productexpiredate = p.cobertura,
                        product = producto,
                        chasis = funciones.descomponer("chasis", assetAMI),
                    };
                    //informacion del vehiculo atributos
                    cnstGenrl = BienSQL.CnstVehiculoParametros(opcion: "103", cliente: "", vehiculo: p.codigosys,
                                                               parametroproducto: "GPT", tipo: funciones.descomponer("tipo", assetAMI), marca: funciones.descomponer("marca", assetAMI),
                                                               modelo: funciones.descomponer("modelo", assetAMI), chasis: funciones.descomponer("chasis", assetAMI), motor: funciones.descomponer("motor", assetAMI),
                                                               placa: funciones.descomponer("placa", assetAMI), color: funciones.descomponer("color", assetAMI), anio: funciones.descomponer("anio", assetAMI));
                    if (cnstGenrl.Tables.Count > 0)
                    {
                        attributeEntidadCollection = new attributeEntidadCollection();
                        attributeCollection = new attributeEntidad();
                        for (int i = 0; i <= cnstGenrl.Tables[0].Rows.Count - 1; i++)
                        {
                            attributeEntidad attribute = new attributeEntidad();
                            attribute.AttributeId = (int)cnstGenrl.Tables[0].Rows[i]["ID"];
                            attribute.Valor = (string)cnstGenrl.Tables[0].Rows[i]["VALOR"];
                            attributeEntidadCollection.Add(attribute);
                        }
                        attributeCollection.attributeEntidadCollection = attributeEntidadCollection;
                    }
                }

                //login
                if (bandera)
                {
                    if (cliente.username == "")
                    {
                        bandera = false;
                        tabla.Rows.Add("-1", "El cliente USERNAME no existe en AMI");
                    }
                    else if (busquedaAmiVehiculo == "")
                    {
                        bandera = false;
                        tabla.Rows.Add("-1", "No existe el vehiculo en AMI");
                    }
                    else if (busquedaAmiCliente == "")
                    {
                        bandera = false;
                        tabla.Rows.Add("-1", "No existe el cliente en AMI");
                    }
                    else
                    {
                        var client = new RestClient(rutalogin);
                        var request = new RestRequest("", Method.Post);
                        request.AddParameter("username", cliente.login);
                        request.AddParameter("password", cliente.password);
                        RestResponse response = client.Execute(request);
                        var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(response.Content);
                        token = funciones.descomponer("token", jsonString);
                    }
                }

            }

            //Bloque cliente propietario
            if (bandera)
            {
                var customersearch = new RestClient();
                var requestecustomersearch = new RestRequest();
                RestResponse responsecustomersearch;
                //var customer = new RestClient();
                //var requeste = new RestRequest();
                //RestResponse responsecustomer;
                customersearch = new RestClient(rutacustomerAdd + "?username=" + cliente.username + "&is_active=" + true + "&id=" + busquedaAmiCliente + " ");
                requestecustomersearch = new RestRequest("", Method.Get);
                requestecustomersearch.AddHeader("cache-control", "no-cache");
                requestecustomersearch.AddHeader("authorization", "HunterAPI " + token);
                responsecustomersearch = customersearch.Execute(requestecustomersearch);
                valoretorno = "0";
                valoretorno = funciones.descomponer("id", responsecustomersearch.Content);
                if (valoretorno != "0")
                {
                    if (valoretorno == busquedaAmiCliente)
                    {
                        customerID = valoretorno;
                        bandera = true;
                    }
                    else
                    {
                        tabla.Rows.Add("-1", "busqueda incorrecta" + " Customer Incorrecto");
                        bandera = false;
                    }
                } else
                {
                    tabla.Rows.Add("-1", "Error al buscar el Customer del cliente ");
                    bandera = false;
                }

            }
            //bloque del vehiculo
            if (bandera)
            {
                var assetSearch = new RestClient();
                var requesteAssetSearch = new RestRequest();
                RestResponse responseAssetSearch;
                var clienteasset = new RestClient();
                var requesteasset = new RestRequest();
                RestResponse responseasset;
                string texto = "";
                assetSearch = new RestClient(rutaassetAdd + "?attributes__attribute__name=COD_SYS" + "&attributes__value=" + asset.codigosys + " ");
                requesteAssetSearch = new RestRequest("", Method.Get);
                requesteAssetSearch.AddHeader("cache-control", "no-cache");
                requesteAssetSearch.AddHeader("authorization", "HunterAPI " + token);
                responseAssetSearch = assetSearch.Execute(requesteAssetSearch);
                valoretorno = "0";
                valoretorno = funciones.descomponer("id", responseAssetSearch.Content);
                if (valoretorno != "0")
                {
                    if (valoretorno == busquedaAmiVehiculo)
                    {
                        //customerID = valoretorno;
                        //bandera = true;
                        cobertura = funciones.descomponer("product_expire_date", responseAssetSearch.Content ).Substring(0, 10);
                        producto = funciones.descomponer("product",responseAssetSearch.Content );

                        texto = Utilidades.SerializarAssetActualizar(asset, attributeEntidadCollection, valoretorno, customerID);
                        clienteasset = new RestClient(rutaassetAdd + valoretorno + "/ ");
                        requesteasset = new RestRequest("", Method.Patch);
                        requesteasset.AddHeader("content-type", "application/json");
                        requesteasset.AddHeader("authorization", "HunterAPI " + token);
                        requesteasset.AddParameter("application/json", texto, ParameterType.RequestBody);
                        responseasset = clienteasset.Execute(requesteasset);
                        if (Convert.ToInt32(responseasset.StatusCode) == 200)
                        {
                            assetID = funciones.descomponer("id", responseasset.Content);
                            bandera = true;
                        } else if (Convert.ToInt32(responseasset.StatusCode) == 400)
                        {
                            string mensaje = funciones.DevuelveMensaje(responseasset.Content);
                            tabla.Rows.Add("-1", mensaje + " " + responseasset.StatusCode + " asset Actualiza");
                            bandera = false;
                        }
                        else
                        {
                            tabla.Rows.Add("-1", "Error al procesar asset " + responseasset.StatusCode + " asset Actualiza");
                            bandera = false;
                        }
                    }
                    else
                    {
                        tabla.Rows.Add("-1", "busqueda incorrecta" + " Customer Incorrecto");
                        bandera = false;
                    }
                }
                else
                {
                    tabla.Rows.Add("-1", "Error al consultar el asset, no existe" + ", Actualizando ");
                    bandera = false;
                }
            }

            // ingreso del log del proceso de AMI
            if (bandera)
            {
                string resultadoLog = "";               
                resultadoLog = ConsultaAMI.RegistroLog(cliente.internal_identifier, customerID, "0", "0", asset.codigosys, assetID, "", "", producto, "0", "0", p.usuario, "N", "", tiporegistro, cliente.username, p.origen, "", "", "", "", "", "", "", "", "", "", "", p.parametroproducto, cobertura, "", p.producto, "");
                if (resultadoLog == "OK")
                {
                    bandera = true;
                }
                else
                {
                    tabla.Rows.Add("-1", "Error en el Ingreso del Log");
                    bandera = false;
                }
            }

            string jsonData;
            if (bandera)
            {
                tabla.Rows.Add(1, "Guardado Exitosamente");
            }
            jsonData = (string)Utilidades.DataTableToJSON(tabla);

            return jsonData;

        }


        [HttpPost]
        [Authorize]
        [SwaggerOperation(Summary = "", Description = "Actualiza Comandos")]
        [Route("ActualizarComando")]
        public ActionResult<string> ActualizarComando(GeneralAMIDTO p)
        {
            //conexion ruta 
            string rutalogin = "";
            string rutaassetAdd = "";
            string rutacustomerAdd = "";
            string rutacommandAdd = "";
            string rutausercommandAdd = "";
            string rutacommand = "";
            string rutausercommand = "";

            //variables
            bool bandera = true;
            //DataSet infoDatos = new DataSet();
            string token = "";
            string valoretorno = "";
            string customerID = "";
            char delimiter = ',';
            string gencomando = p.comando;
            string[] comandos = p.comando.Split(delimiter);
            string busquedaAmiVehiculo = "";
            string busquedaAmiCliente = "";
            string tiporegistro = "COM";
            string assetID = "";
            string log_usr = "";
            string log_pwd = "";
            string producto = "";
            string cobertura = "";
            Int32 status = 0;
            bool execute = true;
            DataTable tabla = new DataTable();
            tabla.Columns.Add("CODIGO", typeof(int));
            tabla.Columns.Add("MENSAJE", typeof(string));
            conexionAMI ruta = new conexionAMI();
            if (p.parametroproducto == "GPT")
            {
                rutalogin = ruta.GetRuta("1");
                rutaassetAdd = ruta.GetRuta("2");
                rutacustomerAdd = ruta.GetRuta("3");
                rutacommandAdd = ruta.GetRuta("5");
                rutausercommandAdd = ruta.GetRuta("6");
                rutacommand = ruta.GetRuta("15");
                rutausercommand = ruta.GetRuta("16");
            }
            else
            {
                bandera = false;
                tabla.Rows.Add("-1", "No existe el parametro del producto");
            }
            customerEntidad cliente = null;
            assetEntidad asset = null;
        
            string clienteAMI = "";
            string assetAMI = "";

            // datos
            if (bandera)
            {
                DataSet cnstGenrl = new DataSet();
                cnstGenrl = BienSQL.CnstVehiculo(opcion: "100", cliente: "", vehiculo: "", parametroproducto: "GPT");
                string support = (string)cnstGenrl.Tables[0].Rows[0]["support"];
                log_usr = (string)cnstGenrl.Tables[0].Rows[0]["usuario"];
                log_pwd = (string)cnstGenrl.Tables[0].Rows[0]["pass"];
                string ami_telefono = (string)cnstGenrl.Tables[0].Rows[0]["telefono"];
                string ami_celular = (string)cnstGenrl.Tables[0].Rows[0]["celular"];
                string ruccompania = (string)cnstGenrl.Tables[0].Rows[0]["ruccompania"];
               

                // informacion del cliente
                if (bandera)
                {
                    clienteAMI = ConsultaAMI.ProcesoAMIConsulta("206", "", "", "", "", p.clienteid);
                    busquedaAmiCliente = funciones.descomponer("ID_TELEMATIC", clienteAMI);
                    cliente = new customerEntidad
                    {
                        username = funciones.descomponer("USERNAME", clienteAMI),
                        first_name = funciones.descomponer("FIRST_NAME", clienteAMI),
                        last_name = funciones.descomponer("LAST_NAME", clienteAMI),
                        email = funciones.descomponer("EMAIL", clienteAMI),
                        login = log_usr,
                        password = log_pwd,
                        internal_identifier = funciones.descomponer("ID_CLIENTE", clienteAMI),
                        active = "True",
                        telefono = funciones.descomponer("TELEFONO_CLIENTE", clienteAMI),
                        identity_customer_type = funciones.descomponer("TIPO", clienteAMI),
                        company_code = ruccompania,
                        business_name = "",
                        emergency_phone_number = ami_telefono,
                        assistance_phone_number = ami_celular,
                        technical_support_email = support,
                    };
                }


                // informacion vehiculo y atributos
                if (bandera)
                {
                    // informacion del vehiculo por codigo interno de netsuite
                    assetAMI = ConsultaAMI.ProcesoAMIConsulta("207", p.vehiculoid, "", "", "", "");
                    busquedaAmiVehiculo = funciones.descomponer("ID_TELEMATIC", assetAMI);
                   
                    asset = new assetEntidad
                    {
                        name = funciones.descomponer("placa", assetAMI),
                        assettype = funciones.descomponer("tipo", assetAMI),
                        // codigosys= codigosys, //CODIGO SYS O NETSUITE
                        codigosys = p.codigosys, //CODIGO SYS O NETSUITE
                        codigonetsuite = p.codigovehiculo,
                        customname = "",
                        active = "True",
                        description = "PLC.:" + funciones.descomponer("placa", assetAMI) + ";MAR.:" + funciones.descomponer("marca", assetAMI) + ";MOD.:" + funciones.descomponer("modelo", assetAMI) + ";COL.:" + funciones.descomponer("color", assetAMI),
                        productexpiredate = p.cobertura,
                        product = producto,
                        chasis = funciones.descomponer("chasis", assetAMI),
                    };
                    
                }


                //login
                if (bandera)
                {
                    if (cliente.username == "")
                    {
                        bandera = false;
                        tabla.Rows.Add("-1", "El cliente cliente USERNAME no existe en AMI");
                    }
                    else if (busquedaAmiVehiculo == "" && busquedaAmiCliente == "")
                    {
                        bandera = false;
                        tabla.Rows.Add("-1", "No existe el cliente en AMI");
                    }
                    else
                    {
                        var client = new RestClient(rutalogin);
                        var request = new RestRequest("", Method.Post);
                        request.AddParameter("username", cliente.login);
                        request.AddParameter("password", cliente.password);
                        RestResponse response = client.Execute(request);
                        var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(response.Content);
                        token = funciones.descomponer("token", jsonString);
                        if (p.accion == "010")
                        {
                            status = 1;
                            execute = true;
                        }
                        if (p.accion == "002")
                        {
                            status = 0;
                            execute = false;
                        }
                    }
                }
            }

            //bloque del cliente propietario
            if (bandera)
            {
                var customersearch = new RestClient();
                var requestecustomersearch = new RestRequest();
                RestResponse responsecustomersearch;
                var customer = new RestClient();
                var requeste = new RestRequest();
                RestResponse responsecustomer;
                customersearch = new RestClient(rutacustomerAdd + "?username=" + cliente.username + "&is_active=" + true + "&id=" + busquedaAmiCliente + " ");
                requestecustomersearch = new RestRequest("", Method.Get);
                requestecustomersearch.AddHeader("cache-control", "no-cache");
                requestecustomersearch.AddHeader("authorization", "HunterAPI " + token);
                responsecustomersearch = customersearch.Execute(requestecustomersearch);
                valoretorno = "0";
                valoretorno = funciones.descomponer("id", responsecustomersearch.Content);
                if (valoretorno != "0")
                {
                    if (valoretorno == busquedaAmiCliente)
                    {
                        customerID = valoretorno;
                        bandera = true;
                    }
                    else
                    {
                        tabla.Rows.Add("-1", "busqueda incorrecta" + " Customer Incorrecto");
                        bandera = false;
                    }
                }
                else
                {
                    tabla.Rows.Add("-1", "Error al buscar el Customer del cliente ");
                    bandera = false;
                }
            }

            //bloque del vehiculo
            if (bandera)
            {
                var assetSearch = new RestClient();
                var requesteAssetSearch = new RestRequest();
                RestResponse responseAssetSearch;
                var clienteasset = new RestClient();
                var requesteasset = new RestRequest();
                RestResponse responseasset;
                string texto = "";
                assetSearch = new RestClient(rutaassetAdd + "?attributes__attribute__name=COD_SYS" + "&attributes__value=" + asset.codigosys + " ");
                requesteAssetSearch = new RestRequest("", Method.Get);
                requesteAssetSearch.AddHeader("cache-control", "no-cache");
                requesteAssetSearch.AddHeader("authorization", "HunterAPI " + token);
                responseAssetSearch = assetSearch.Execute(requesteAssetSearch);
                valoretorno = "0";
                valoretorno = funciones.descomponer("id", responseAssetSearch.Content);
                if (valoretorno != "0")
                {
                    if (valoretorno == busquedaAmiVehiculo)
                    {
                        assetID = valoretorno;
                        bandera = true;
                        cobertura = funciones.descomponer("product_expire_date", responseAssetSearch.Content).Substring(0, 10);
                        producto = funciones.descomponer("product", responseAssetSearch.Content);

                       
                    }
                    else
                    {
                        tabla.Rows.Add("-1", "busqueda incorrecta" + " Asset Incorrecto");
                        bandera = false;
                    }
                }
                else
                {
                    tabla.Rows.Add("-1", "Error al consultar el asset, no existe" + ", Verificando ");
                    bandera = false;
                }
            }

            //bloque  de  vehiculo - comando               
            if (bandera)
            {
                if (p.comando != "")
                {
                    var commandSearch = new RestClient();
                    var requesteCommandSearch = new RestRequest();
                    RestResponse responseCommandSearch;
                    var clientecommand = new RestClient();
                    var requestecommand = new RestRequest();
                    RestResponse responsecommand;
                    foreach (var datcomando in comandos)
                    {
                        commandSearch = new RestClient(rutacommandAdd + "?asset=" + assetID + "&command=" + datcomando + "&status=" + 0 +  " ");
                        requesteCommandSearch = new RestRequest("", Method.Get);
                        requesteCommandSearch.AddHeader("cache-control", "no-cache");
                        requesteCommandSearch.AddHeader("authorization", "HunterAPI " + token);
                        responseCommandSearch = commandSearch.Execute(requesteCommandSearch);
                        valoretorno = "0";
                        valoretorno = funciones.descomponer("id", responseCommandSearch.Content);
                        if (valoretorno != "0")
                        {
                            clientecommand = new RestClient(rutacommandAdd + valoretorno + "/ ");
                            requestecommand = new RestRequest("", Method.Patch);
                            requestecommand.AddHeader("cache-control", "no-cache");
                            requestecommand.AddHeader("authorization", "HunterAPI " + token);
                            requestecommand.AddParameter("status", status);
                            requestecommand.AddParameter("id", valoretorno);
                            requestecommand.AddParameter("command", datcomando);
                            requestecommand.AddParameter("asset", assetID);
                            responsecommand = clientecommand.Execute(requestecommand);
                            if (Convert.ToInt32(responsecommand.StatusCode) == 200)
                                bandera = true;
                            else if (Convert.ToInt32(responsecommand.StatusCode) == 400)
                            {
                                string mensaje = funciones.DevuelveMensaje(responsecommand.Content);
                                tabla.Rows.Add("-1", mensaje + " " + responsecommand.StatusCode + " asset-comando Actualiza");
                                bandera = false;
                                break;
                            }
                            else
                            {
                                tabla.Rows.Add("-1", "Error al procesar asset command " + responsecommand.StatusCode + " asset-comando Actualiza");
                                bandera = false;
                                break;
                            }
                        }
                        else
                        {
                            commandSearch = new RestClient(rutacommandAdd + "?asset=" + assetID + "&command=" + datcomando + "&status=" + 1 + " ");
                            requesteCommandSearch = new RestRequest("", Method.Get);
                            requesteCommandSearch.AddHeader("cache-control", "no-cache");
                            requesteCommandSearch.AddHeader("authorization", "HunterAPI " + token);
                            responseCommandSearch = commandSearch.Execute(requesteCommandSearch);
                            valoretorno = "0";
                            valoretorno = funciones.descomponer("id", responseCommandSearch.Content);
                            if (valoretorno != "0")
                            {
                                clientecommand = new RestClient(rutacommandAdd + valoretorno + "/ ");
                                requestecommand = new RestRequest("", Method.Post);
                                requestecommand.AddHeader("cache-control", "no-cache");
                                requestecommand.AddHeader("authorization", "HunterAPI " + token);
                                requestecommand.AddParameter("status", status);
                                requestecommand.AddParameter("id", valoretorno);
                                requestecommand.AddParameter("command", datcomando);
                                requestecommand.AddParameter("asset", assetID);
                                responsecommand = clientecommand.Execute(requestecommand);
                                if (Convert.ToInt32(responsecommand.StatusCode) == 201)
                                    bandera = true;
                                else if (Convert.ToInt32(responsecommand.StatusCode) == 400)
                                {
                                    string mensaje = funciones.DevuelveMensaje(responsecommand.Content);
                                    tabla.Rows.Add("-1", mensaje + " " + responsecommand.StatusCode + " asset-comando");
                                    bandera = false;
                                    break;
                                }
                                else
                                {
                                    tabla.Rows.Add("-1", "Error al procesar asset command " + responsecommand.StatusCode + " asset-comando ");
                                    bandera = false;
                                    break;
                                }
                            }
                            else
                            {
                                clientecommand = new RestClient(rutacommandAdd + valoretorno + "/ ");
                                requestecommand = new RestRequest("", Method.Patch);
                                requestecommand.AddHeader("cache-control", "no-cache");
                                requestecommand.AddHeader("authorization", "HunterAPI " + token);
                                requestecommand.AddParameter("status", status);
                                requestecommand.AddParameter("id", valoretorno);
                                requestecommand.AddParameter("command", datcomando);
                                requestecommand.AddParameter("asset", assetID);
                                responsecommand = clientecommand.Execute(requestecommand);
                                if (Convert.ToInt32(responsecommand.StatusCode) == 200)
                                    bandera = true;
                                else if (Convert.ToInt32(responsecommand.StatusCode) == 400)
                                {
                                    string mensaje = funciones.DevuelveMensaje(responsecommand.Content);
                                    tabla.Rows.Add("-1", mensaje + " " + responsecommand.StatusCode + " asset-comando");
                                    bandera = false;
                                    break;
                                }
                                else
                                {
                                    tabla.Rows.Add("-1", "Error al procesar asset command " + responsecommand.StatusCode + " asset-comando ");
                                    bandera = false;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            //bloque  de  comando
            if (bandera)
            {
                if (p.comando != "")
                {
                    var userCommandSearch = new RestClient();
                    var requesteuserCommandSearch = new RestRequest();
                    RestResponse responseuserCommandSearch;
                    var clienteusercommand = new RestClient();
                    var requesteusercommand = new RestRequest();
                    RestResponse responseusercommand;
                    foreach (var datcomando in comandos)
                    {
                        //consulta si existe el comando por vehiculo - comando - usuario old - estado falso
                        userCommandSearch = new RestClient(rutausercommandAdd + "?asset=" + assetID + "&command=" + datcomando + "&user=" + customerID + "&can_execute=" + false + " ");
                        requesteuserCommandSearch = new RestRequest("", Method.Get);
                        requesteuserCommandSearch.AddHeader("cache-control", "no-cache");
                        requesteuserCommandSearch.AddHeader("authorization", "HunterAPI " + token);
                        responseuserCommandSearch = userCommandSearch.Execute(requesteuserCommandSearch);
                        valoretorno = "0";
                        valoretorno = funciones.descomponer("id", responseuserCommandSearch.Content);
                        if (valoretorno != "0")
                        {
                            clienteusercommand = new RestClient(rutausercommandAdd + valoretorno + "/ ");
                            requesteusercommand = new RestRequest("", Method.Patch);
                            requesteusercommand.AddHeader("cache-control", "no-cache");
                            requesteusercommand.AddHeader("authorization", "HunterAPI " + token);
                            requesteusercommand.AddParameter("id", valoretorno);
                            requesteusercommand.AddParameter("command", datcomando);
                            requesteusercommand.AddParameter("user", customerID);
                            requesteusercommand.AddParameter("asset", assetID);
                            requesteusercommand.AddParameter("can_execute", execute);
                            responseusercommand = clienteusercommand.Execute(requesteusercommand);
                            if (Convert.ToInt32(responseusercommand.StatusCode) == 200)
                                bandera = true;
                            else if (Convert.ToInt32(responseusercommand.StatusCode) == 400)
                            {
                                string mensaje = funciones.DevuelveMensaje(responseusercommand.Content);
                                tabla.Rows.Add("-1", mensaje + " " + responseusercommand.StatusCode + " comando-usuario Actualiza");
                                bandera = false;
                                break;
                            }
                            else
                            {
                                tabla.Rows.Add("-1", "Error al procesar user asset command " + responseusercommand.StatusCode + " comando-usuario Actualiza");
                                bandera = false;
                                break;
                            }
                        }
                        else
                        {
                            //consulta si existe el comando por vehiculo - comando - usuario  - estado 
                            userCommandSearch = new RestClient(rutausercommandAdd + "?asset=" + assetID + "&command=" + datcomando + "&user=" + customerID + "&can_execute=" + true + " ");
                            requesteuserCommandSearch = new RestRequest("", Method.Get);
                            requesteuserCommandSearch.AddHeader("cache-control", "no-cache");
                            requesteuserCommandSearch.AddHeader("authorization", "HunterAPI " + token);
                            responseuserCommandSearch = userCommandSearch.Execute(requesteuserCommandSearch);
                            valoretorno = "0";
                            valoretorno = funciones.descomponer("id", responseuserCommandSearch.Content);
                            if (valoretorno != "0")
                            {
                                clienteusercommand = new RestClient(rutausercommand);
                                requesteusercommand = new RestRequest("", Method.Post);
                                requesteusercommand.AddHeader("cache-control", "no-cache");
                                requesteusercommand.AddHeader("authorization", "HunterAPI " + token);
                                requesteusercommand.AddParameter("command", datcomando);
                                requesteusercommand.AddParameter("user", customerID);
                                requesteusercommand.AddParameter("asset", assetID);
                                requesteusercommand.AddParameter("can_execute", execute);
                                responseusercommand = clienteusercommand.Execute(requesteusercommand);
                                if (Convert.ToInt32(responseusercommand.StatusCode) == 201)
                                    bandera = true;
                                else if (Convert.ToInt32(responseusercommand.StatusCode) == 400)
                                {
                                    string mensaje = funciones.DevuelveMensaje(responseusercommand.Content);
                                    tabla.Rows.Add("-1", mensaje + " " + responseusercommand.StatusCode + " comando-usuario");
                                    bandera = false;
                                    break;
                                }
                                else
                                {
                                    tabla.Rows.Add("-1", "Error al procesar user asset command " + responseusercommand.StatusCode + " comando-usuario");
                                    bandera = false;
                                    break;
                                }
                            }
                            else
                            {
                                clienteusercommand = new RestClient(rutausercommandAdd + valoretorno + "/ ");
                                requesteusercommand = new RestRequest("", Method.Patch);
                                requesteusercommand.AddHeader("cache-control", "no-cache");
                                requesteusercommand.AddHeader("authorization", "HunterAPI " + token);
                                requesteusercommand.AddParameter("id", valoretorno);
                                requesteusercommand.AddParameter("command", datcomando);
                                requesteusercommand.AddParameter("user", customerID);
                                requesteusercommand.AddParameter("asset", assetID);
                                requesteusercommand.AddParameter("can_execute", execute);
                                responseusercommand = clienteusercommand.Execute(requesteusercommand);
                                if (Convert.ToInt32(responseusercommand.StatusCode) == 200)
                                    bandera = true;
                                else if (Convert.ToInt32(responseusercommand.StatusCode) == 400)
                                {
                                    string mensaje = funciones.DevuelveMensaje(responseusercommand.Content);
                                    tabla.Rows.Add("-1", mensaje + " " + responseusercommand.StatusCode + " comando-usuario");
                                    bandera = false;
                                    break;
                                }
                                else
                                {
                                    tabla.Rows.Add("-1", "Error al procesar user asset command " + responseusercommand.StatusCode + " comando-usuario");
                                    bandera = false;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            // ingreso del log del proceso de AMI
            if (bandera)
            {
                string resultadoLog = ConsultaAMI.RegistroLog(p.clienteid, customerID, "0", "0", asset.codigosys, assetID, gencomando, "", producto, p.ordenservicio, "0", p.usuario, "N", "", tiporegistro, cliente.username, p.origen, p.accion, "", "", "", "", "", "", "", "", "", "", p.parametroproducto, cobertura, "", p.producto, "");
                if (resultadoLog == "OK")
                {
                    bandera = true;
                }
                else
                {
                    tabla.Rows.Add("-1", "Error en el Ingreso del Log");
                    bandera = false;
                }
            }

            string jsonData;
            if (bandera)
            {
                tabla.Rows.Add(1, "Guardado Exitosamente");
            }
            jsonData = (string)Utilidades.DataTableToJSON(tabla);

            return jsonData;

        }


        [HttpPost]
        [Authorize]
        [SwaggerOperation(Summary = "", Description = "Actualiza Dispositivo")]
        [Route("ActualizacionDevice")]
        public ActionResult<string> ActualizacionDevice(GeneralAMIDTO p)
        {
            //conexion ruta 
            string rutalogin = "";
            string rutaassetAdd = "";
            string rutadeviceAdd = "";
            string rutadevice = "";
            //variables
            bool bandera = true;
            //DataSet infoDatos = new DataSet();
            string token = "";
            string valoretorno = "";
            string assetID = "";
            //string verificaasociado = "N";
            string tiporegistro = "DEV";
            string busquedaAmiCliente = "";
            string busquedaAmiVehiculo = "";
            //string sensors = "0";
            string log_usr = "";
            string log_pwd = "";
            string producto = "";
            string cobertura = "";
            string valorvid = "";
            DataTable tabla = new DataTable();
            tabla.Columns.Add("CODIGO", typeof(int));
            tabla.Columns.Add("MENSAJE", typeof(string));
            conexionAMI ruta = new conexionAMI();
            if (p.parametroproducto == "GPT")
            {
                rutalogin = ruta.GetRuta("1");
                rutaassetAdd = ruta.GetRuta("2");
                rutadeviceAdd = ruta.GetRuta("4");
                rutadevice = ruta.GetRuta("14");
            }
            else
            {
                bandera = false;
                tabla.Rows.Add("-1", "No existe el parametro del producto");
            }

            customerEntidad cliente = null;
            assetEntidad asset = null;
            deviceEntidad device = null;
            //attributeEntidadCollection attributeEntidadCollection = null;
            //attributeEntidad attributeCollection = null;
            //string busquedaAmiVehiculo = "";
           
            string deviceAMI = "";
            string clienteAMI = "";
            string assetAMI = "";

            // datos
            if (bandera)
            {
                DataSet cnstGenrl = new DataSet();
                cnstGenrl = BienSQL.CnstVehiculo(opcion: "100", cliente: "", vehiculo: "", parametroproducto: "GPT");
                string support = (string)cnstGenrl.Tables[0].Rows[0]["support"];
                log_usr = (string)cnstGenrl.Tables[0].Rows[0]["usuario"];
                log_pwd = (string)cnstGenrl.Tables[0].Rows[0]["pass"];
                string ami_telefono = (string)cnstGenrl.Tables[0].Rows[0]["telefono"];
                string ami_celular = (string)cnstGenrl.Tables[0].Rows[0]["celular"];
                string ruccompania = (string)cnstGenrl.Tables[0].Rows[0]["ruccompania"];


                // informacion del cliente
                if (bandera)
                {
                    clienteAMI = ConsultaAMI.ProcesoAMIConsulta("206", "", "", "", "", p.clienteid);
                    busquedaAmiCliente = funciones.descomponer("ID_TELEMATIC", clienteAMI);
                    cliente = new customerEntidad
                    {
                        username = funciones.descomponer("USERNAME", clienteAMI),
                        first_name = funciones.descomponer("FIRST_NAME", clienteAMI),
                        last_name = funciones.descomponer("LAST_NAME", clienteAMI),
                        email = funciones.descomponer("EMAIL", clienteAMI),
                        login = log_usr,
                        password = log_pwd,
                        internal_identifier = funciones.descomponer("ID_CLIENTE", clienteAMI),
                        active = "True",
                        telefono = funciones.descomponer("TELEFONO_CLIENTE", clienteAMI),
                        identity_customer_type = funciones.descomponer("TIPO", clienteAMI),
                        company_code = ruccompania,
                        business_name = "",
                        emergency_phone_number = ami_telefono,
                        assistance_phone_number = ami_celular,
                        technical_support_email = support,
                    };
                }

                // informacion vehiculo
                if (bandera)
                {
                    // informacion del vehiculo por codigo interno de netsuite
                    assetAMI = ConsultaAMI.ProcesoAMIConsulta("207", p.vehiculoid, "", "", "", "");
                    busquedaAmiVehiculo = funciones.descomponer("ID_TELEMATIC", assetAMI);
                    asset = new assetEntidad
                    {
                        name = funciones.descomponer("placa", assetAMI),
                        assettype = funciones.descomponer("tipo", assetAMI),
                        // codigosys= codigosys, //CODIGO SYS O NETSUITE
                        codigosys = p.codigosys, //CODIGO SYS O NETSUITE
                        codigonetsuite = p.codigovehiculo,
                        customname = "",
                        active = "True",
                        description = "PLC.:" + funciones.descomponer("placa", assetAMI) + ";MAR.:" + funciones.descomponer("marca", assetAMI) + ";MOD.:" + funciones.descomponer("modelo", assetAMI) + ";COL.:" + funciones.descomponer("color", assetAMI),
                        productexpiredate = p.cobertura,
                        product = producto,
                        chasis = funciones.descomponer("chasis", assetAMI),
                    };
                }

                //proceso device
                if (bandera)
                {
                    deviceAMI = ConsultaAMI.ProcesoAMIConsulta("202", p.codigovehiculo, p.producto, p.ordenservicio, p.itemNS, "");
                    valorvid = funciones.descomponer("vid", deviceAMI);
                    device = new deviceEntidad
                    {
                        id = p.serie,
                        vid = funciones.descomponer("vid", deviceAMI),
                        company_code = ruccompania,
                        active = "True",
                        model = funciones.descomponer("id_telematic_modelo", deviceAMI),
                        report_from = funciones.descomponer("id_telematic_servidor", deviceAMI),
                        numero_celular = funciones.descomponer("noCelularSIM", deviceAMI),
                    };

                    if (valorvid == "0")
                    {
                        tabla.Rows.Add("-1", "Verificar el valor del VID ");
                        bandera = false;
                    }
                    if (device.report_from == "")
                    {
                        tabla.Rows.Add("-1", "Verificar el Servidor del Dispositivo ");
                        bandera = false;
                    }
                    if (device.model == "")
                    {
                        tabla.Rows.Add("-1", "Verificar el Modelo del Dispositivo ");
                        bandera = false;
                    }
                }


                //login
                if (bandera)
                {
                    if (cliente.username == "")
                    {
                        bandera = false;
                        tabla.Rows.Add("-1", "El cliente cliente USERNAME no existe en AMI");
                    }
                    else if (busquedaAmiVehiculo == "" && busquedaAmiCliente == "")
                    {
                        bandera = false;
                        tabla.Rows.Add("-1", "No existe el cliente en AMI");
                    }
                    else
                    {
                        var client = new RestClient(rutalogin);
                        var request = new RestRequest("", Method.Post);
                        request.AddParameter("username", cliente.login);
                        request.AddParameter("password", cliente.password);
                        RestResponse response = client.Execute(request);
                        var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(response.Content);
                        token = funciones.descomponer("token", jsonString);
                    }
                }
            }

            //bloque del vehiculo
            if (bandera)
            {
                var assetSearch = new RestClient();
                var requesteAssetSearch = new RestRequest();
                RestResponse responseAssetSearch;
                assetSearch = new RestClient(rutaassetAdd + "?attributes__attribute__name=COD_SYS="  + "&attributes__value=" + asset.codigosys + " ");
                requesteAssetSearch = new RestRequest("", Method.Get);
                requesteAssetSearch.AddHeader("cache-control", "no-cache");
                requesteAssetSearch.AddHeader("authorization", "HunterAPI " + token);
                responseAssetSearch = assetSearch.Execute(requesteAssetSearch);
                valoretorno = "0";
                valoretorno = funciones.descomponer("id", responseAssetSearch.Content);
                if (valoretorno != "0")
                {
                    assetID = valoretorno;
                    cobertura = funciones.descomponer("product_expire_date", responseAssetSearch.Content).Substring(0, 10);
                    producto = funciones.descomponer("product", responseAssetSearch.Content);
                }
                else
                {
                    tabla.Rows.Add("-1", "Error al consultar el asset, no existe" + ", Verificar ");
                    bandera = false;
                }
            }

            //bloque del anterior device
            if (bandera)
            {
                var devicesearch = new RestClient();
                var requestedevicesearch = new RestRequest();
                RestResponse responsedevicesearch;
                var clientedevices = new RestClient();
                var requestedevice = new RestRequest();
                RestResponse responsedevice;
                devicesearch = new RestClient(rutadeviceAdd + "?id=" + p.vidanterior + "&company_code=" + device.company_code + " ");
                requestedevicesearch = new RestRequest("", Method.Get);
                requestedevicesearch.AddHeader("cache-control", "no-cache");
                requestedevicesearch.AddHeader("authorization", "HunterAPI " + token);
                responsedevicesearch = devicesearch.Execute(requestedevicesearch);
                valoretorno = "0";
                valoretorno = funciones.descomponer("id", responsedevicesearch.Content);
                if (valoretorno != "0")
                {
                    clientedevices = new RestClient(rutadeviceAdd +valoretorno + "/ ");
                    requestedevice = new RestRequest("", Method.Patch);
                    requestedevice.AddHeader("cache-control", "no-cache");
                    requestedevice.AddHeader("authorization", "HunterAPI " + token);
                    requestedevice.AddParameter("report_from", device.report_from);
                    requestedevice.AddParameter("model", device.model);
                    requestedevice.AddParameter("company_code", device.company_code);
                    requestedevice.AddParameter("id", p.vidanterior);
                    requestedevice.AddParameter("active", false);
                    responsedevice = clientedevices.Execute(requestedevice);
                    if (Convert.ToInt32(responsedevice.StatusCode) == 200)
                        bandera = true;
                    else if (Convert.ToInt32(responsedevice.StatusCode) == 400)
                    {
                        string mensaje = funciones.DevuelveMensaje(responsedevice.Content);
                        tabla.Rows.Add("-1", mensaje + " " + responsedevice.StatusCode + " device anterior Actualiza");
                        bandera = false;
                    }
                    else
                    {
                        tabla.Rows.Add("-1", "Error al procesar devices " + responsedevice.StatusCode + " device anterior Actualiza");
                        bandera = false;
                    }
                }
                else
                {
                    tabla.Rows.Add("-1", "Error al buscar el devices anterior ");
                    bandera = false;
                }
                
            }

            //device nuevo
            if (bandera)
            {
                var devicesearch = new RestClient();
                var requestedevicesearch = new RestRequest();
                RestResponse responsedevicesearch;
                var clientedevices = new RestClient();
                var requestedevice = new RestRequest();
                RestResponse responsedevice;
                devicesearch = new RestClient(rutadeviceAdd + "?id=" + device.vid + "&company_code=" + device.company_code + " ");
                requestedevicesearch = new RestRequest("", Method.Get);
                requestedevicesearch.AddHeader("cache-control", "no-cache");
                requestedevicesearch.AddHeader("authorization", "HunterAPI " + token);
                responsedevicesearch = devicesearch.Execute(requestedevicesearch);
                valoretorno = "0";
                valoretorno = funciones.descomponer("id", responsedevicesearch.Content);
                if (valoretorno != "0")
                {
                    clientedevices = new RestClient(rutadeviceAdd + valoretorno + "/ ");
                    requestedevice = new RestRequest("", Method.Patch);
                    requestedevice.AddHeader("cache-control", "no-cache");
                    requestedevice.AddHeader("authorization", "HunterAPI " + token);
                    requestedevice.AddParameter("report_from", device.report_from);
                    requestedevice.AddParameter("model", device.model);
                    requestedevice.AddParameter("company_code", device.company_code);
                    requestedevice.AddParameter("id", device.vid);
                    requestedevice.AddParameter("active", device.active);
                    responsedevice = clientedevices.Execute(requestedevice);
                    if (Convert.ToInt32(responsedevice.StatusCode) == 200)
                        bandera = true;
                    else if (Convert.ToInt32(responsedevice.StatusCode) == 400)
                    {
                        string mensaje = funciones.DevuelveMensaje(responsedevice.Content);
                        tabla.Rows.Add("-1", mensaje + " " + responsedevice.StatusCode + " device nuevo Actualiza");
                        bandera = false;
                    }
                    else
                    {
                        tabla.Rows.Add("-1", "Error al procesar devices " + responsedevice.StatusCode + " device nuevo Actualiza");
                        bandera = false;
                    }
                    //assetID = valoretorno;
                    //cobertura = funciones.descomponer("product_expire_date", responseAssetSearch.Content).Substring(0, 10);
                    //producto = funciones.descomponer("product", responseAssetSearch.Content);
                }
                else
                {
                    devicesearch = new RestClient(rutadeviceAdd + "?id=" + device.vid + "&company_code=" + device.company_code + " ");
                    requestedevicesearch = new RestRequest("", Method.Get);
                    requestedevicesearch.AddHeader("cache-control", "no-cache");
                    requestedevicesearch.AddHeader("authorization", "HunterAPI " + token);
                    responsedevicesearch = devicesearch.Execute(requestedevicesearch);
                    valoretorno = "0";
                    valoretorno = funciones.descomponer("id", responsedevicesearch.Content);
                    if (valoretorno == "0")
                    {
                        clientedevices = new RestClient(rutadevice);
                        requestedevice = new RestRequest("", Method.Post);
                        requestedevice.AddHeader("cache-control", "no-cache");
                        requestedevice.AddHeader("authorization", "HunterAPI " + token);
                        requestedevice.AddParameter("report_from", device.report_from);
                        requestedevice.AddParameter("model", device.model);
                        requestedevice.AddParameter("company_code", device.company_code);
                        requestedevice.AddParameter("id", device.vid);
                        requestedevice.AddParameter("active", device.active);
                        responsedevice = clientedevices.Execute(requestedevice);
                        if (Convert.ToInt32(responsedevice.StatusCode) == 201)
                            bandera = true;
                        else if (Convert.ToInt32(responsedevice.StatusCode) == 400)
                        {
                            string mensaje = funciones.DevuelveMensaje(responsedevice.Content);
                            tabla.Rows.Add("-1", mensaje + " " + responsedevice.StatusCode + " device ");
                            bandera = false;
                        }
                        else
                        {
                            tabla.Rows.Add("-1", "Error al procesar devices nuevo " + responsedevice.StatusCode + " device ");
                            bandera = false;
                        }
                    }
                    else
                    {
                        bandera = true;
                    }
                }
            }

            //bloque  de  vehiculo - remove device
            if (bandera)
            {
                var assetDevSearch = new RestClient();
                var requesteassetDevSearch = new RestRequest();
                RestResponse responseassetDevSearch;
                var clienteassetdev = new RestClient();
                var requesteassetdev = new RestRequest();
                RestResponse responseassetdev;
                ////anterior device
                assetDevSearch = new RestClient(rutaassetAdd + "?devices__device__id=" + p.vidanterior + "&devices__device__company_code=" + device.company_code + "&attributes__attribute__name=COD_SYS" + "&attributes__value=" + asset.codigosys + " ");
                requesteassetDevSearch = new RestRequest("", Method.Get);
                requesteassetDevSearch.AddHeader("cache-control", "no-cache");
                requesteassetDevSearch.AddHeader("authorization", "HunterAPI " + token);
                responseassetDevSearch = assetDevSearch.Execute(requesteassetDevSearch);
                valoretorno = "0";
                valoretorno = funciones.descomponer("id", responseassetDevSearch.Content);
                if (valoretorno != "0")
                {
                    if (valoretorno == assetID)
                    {
                        clienteassetdev = new RestClient(rutaassetAdd + valoretorno + "/device-remove/ ");
                        requesteassetdev = new RestRequest("", Method.Patch);
                        requesteassetdev.AddHeader("cache-control", "no-cache");
                        requesteassetdev.AddHeader("authorization", "HunterAPI " + token);
                        requesteassetdev.AddParameter("id", valoretorno);
                        requesteassetdev.AddParameter("device_id", p.vidanterior);

                        responseassetdev = clienteassetdev.Execute(requesteassetdev);

                        if (Convert.ToInt32(responseassetdev.StatusCode) == 200)
                            bandera = true;
                        else if (Convert.ToInt32(responseassetdev.StatusCode) == 400)
                        {
                            string mensaje = funciones.DevuelveMensaje(responseassetdev.Content);
                            tabla.Rows.Add("-1", mensaje + " " + responseassetdev.StatusCode + " remove asset-device ");
                            bandera = false;
                        }
                        else
                        {
                            tabla.Rows.Add("-1", "Error al procesar assetdevice " + responseassetdev.StatusCode + " remove asset-device ");
                            bandera = false;
                        }
                    }
                    else
                    {
                        tabla.Rows.Add("-1", "Error al procesar asset Id distinto remove device ");
                        bandera = false;
                    }

                }
                else
                {
                    bandera = true;
                }
            }

            //bloque  de  vehiculo - add device
            if (bandera)
            {
                var assetDevSearch = new RestClient();
                var requesteassetDevSearch = new RestRequest();
                RestResponse responseassetDevSearch;
                var clienteassetdev = new RestClient();
                var requesteassetdev = new RestRequest();
                RestResponse responseassetdev;
                assetDevSearch = new RestClient(rutaassetAdd + "?devices__device__id=" + device.vid   + "&attributes__attribute__name=COD_SYS" + "&attributes__value=" + asset.codigosys + " ");
                requesteassetDevSearch = new RestRequest("", Method.Get);
                requesteassetDevSearch.AddHeader("cache-control", "no-cache");
                requesteassetDevSearch.AddHeader("authorization", "HunterAPI " + token);
                responseassetDevSearch = assetDevSearch.Execute(requesteassetDevSearch);
                valoretorno = "0";
                valoretorno = funciones.descomponer("id", responseassetDevSearch.Content);
                if (valoretorno == "0")
                {
                    clienteassetdev = new RestClient(rutaassetAdd + assetID + "/device-add/ ");
                    requesteassetdev = new RestRequest("", Method.Patch);
                    requesteassetdev.AddHeader("cache-control", "no-cache");
                    requesteassetdev.AddHeader("authorization", "HunterAPI " + token);
                    requesteassetdev.AddParameter("id", assetID);
                    requesteassetdev.AddParameter("device_id", device.vid);
                    responseassetdev = clienteassetdev.Execute(requesteassetdev);
                    if (Convert.ToInt32(responseassetdev.StatusCode) == 200)
                        bandera = true;
                    else if (Convert.ToInt32(responseassetdev.StatusCode) == 400)
                    {
                        string mensaje = funciones.DevuelveMensaje(responseassetdev.Content);
                        tabla.Rows.Add("-1", mensaje + " " + responseassetdev.StatusCode + " asset-device ");
                        bandera = false;
                    }
                    else
                    {
                        tabla.Rows.Add("-1", "Error al procesar assetdevice " + responseassetdev.StatusCode + " asset-device ");
                        bandera = false;
                    }
                }
                else
                {
                    bandera = true;
                }
            }

            // ingreso del log del proceso de AMI
            if (bandera)
            {
                string resultadoLog = ConsultaAMI.RegistroLog(p.clienteid, busquedaAmiCliente, "0", "0", asset.codigosys, assetID, "", p.serie, producto, "0", "0", p.usuario, "N", "", tiporegistro, cliente.username, p.origen, "", "", "", "", "", "", "", "", "", "", "", p.parametroproducto, cobertura, valorvid, "", "");
                if (resultadoLog == "OK")
                {
                    bandera = true;
                }
                else
                {
                    tabla.Rows.Add("-1", "Error en el Ingreso del Log");
                    bandera = false;
                }
            }

            string jsonData;
            if (bandera)
            {
                tabla.Rows.Add(1, "Guardado Exitosamente");
            }
            jsonData = (string)Utilidades.DataTableToJSON(tabla);

            return jsonData;

        }


        [HttpPost]
        [Authorize]
        [SwaggerOperation(Summary = "", Description = "Actualiza el producto del vehiculo")]
        [Route("UpgradeAsset")]
        public ActionResult<string> UpgradeAsset(GeneralAMIDTO p)
        {
            //conexion ruta 
            string rutalogin = "";
            string rutaassetAdd = "";
            string rutacustomerAdd = "";
            //variables
            bool bandera = true;
            //DataSet infoDatos = new DataSet();
            string token = "";
            string valoretorno = "";
            string customerID = "";
            string accion = "006";
            string tiporegistro = "UPG";
            string assetID = "";
            string sensors = "0";
            string concesionario = "";
            string log_usr = "";
            string log_pwd = "";
            string producto = "";
            string cobertura = "";
            DataTable tabla = new DataTable();
            tabla.Columns.Add("CODIGO", typeof(int));
            tabla.Columns.Add("MENSAJE", typeof(string));
            conexionAMI ruta = new conexionAMI();
            if (p.parametroproducto == "GPT")
            {
                rutalogin = ruta.GetRuta("1");
                rutaassetAdd = ruta.GetRuta("2");
                rutacustomerAdd = ruta.GetRuta("3");
            }
            else
            {
                bandera = false;
                tabla.Rows.Add("-1", "No existe el parametro del producto");
            }
            customerEntidad cliente = null;
            assetEntidad asset = null;
            attributeEntidadCollection attributeEntidadCollection = null;
            attributeEntidad attributeCollection = null;
            string busquedaAmiVehiculo = "";
            string busquedaAmiCliente = "";
            
            string clienteAMI = "";
            string assetAMI = "";

            // datos
            if (bandera)
            {
                DataSet cnstGenrl = new DataSet();
                cnstGenrl = BienSQL.CnstVehiculo(opcion: "100", cliente: "", vehiculo: "", parametroproducto: "GPT");
                string support = (string)cnstGenrl.Tables[0].Rows[0]["support"];
                log_usr = (string)cnstGenrl.Tables[0].Rows[0]["usuario"];
                log_pwd = (string)cnstGenrl.Tables[0].Rows[0]["pass"];
                string ami_telefono = (string)cnstGenrl.Tables[0].Rows[0]["telefono"];
                string ami_celular = (string)cnstGenrl.Tables[0].Rows[0]["celular"];
                string ruccompania = (string)cnstGenrl.Tables[0].Rows[0]["ruccompania"];


                // informacion del cliente
                if (bandera)
                {
                    clienteAMI = ConsultaAMI.ProcesoAMIConsulta("206", "", "", "", "", p.clienteid);
                    busquedaAmiCliente = funciones.descomponer("ID_TELEMATIC", clienteAMI);
                    cliente = new customerEntidad
                    {
                        username = funciones.descomponer("USERNAME", clienteAMI),
                        first_name = funciones.descomponer("FIRST_NAME", clienteAMI),
                        last_name = funciones.descomponer("LAST_NAME", clienteAMI),
                        email = funciones.descomponer("EMAIL", clienteAMI),
                        login = log_usr,
                        password = log_pwd,
                        internal_identifier = funciones.descomponer("ID_CLIENTE", clienteAMI),
                        active = "True",
                        telefono = funciones.descomponer("TELEFONO_CLIENTE", clienteAMI),
                        identity_customer_type = funciones.descomponer("TIPO", clienteAMI),
                        company_code = ruccompania,
                        business_name = "",
                        emergency_phone_number = ami_telefono,
                        assistance_phone_number = ami_celular,
                        technical_support_email = support,
                    };
                }

                // informacion del vehiculo y atributos
                if (bandera)
                {
                    // informacion del vehiculo
                    assetAMI = ConsultaAMI.ProcesoAMIConsulta("203", p.codigovehiculo, p.producto, p.ordenservicio, p.itemNS, "");
                    if (funciones.descomponer("familiadescripcion", assetAMI) == "HUNTER GPS HMT")
                    {
                        producto = "H.MONITOREO AMI";
                    }
                    else if (funciones.descomponer("familiadescripcion", assetAMI) == "AMI CHANGAN BY HUNTER")
                    {
                        producto = "H.MONITOREO AMI";
                    }
                    else if (funciones.descomponer("familiadescripcion", assetAMI) == "ANDOR BY HUNTER")
                    {
                        producto = "ANDOR LINK BY HUNTER";
                    }
                    else
                    {
                        producto = funciones.descomponer("familiadescripcion", assetAMI);
                    }
                    concesionario = funciones.descomponer("concesionarios", assetAMI);
                    cnstGenrl = BienSQL.CnstProducto(opcion: "104", concesionario: concesionario, producto: p.producto);
                    if (cnstGenrl.Tables[0].Rows.Count > 0)
                    {
                        producto = (string)cnstGenrl.Tables[0].Rows[0]["NOMBRE_PRODUCTO"];
                    }
                    asset = new assetEntidad
                    {
                        name = funciones.descomponer("placa", assetAMI),
                        assettype = funciones.descomponer("tipo", assetAMI),
                        // codigosys= codigosys, //CODIGO SYS O NETSUITE
                        codigosys = p.codigosys, //CODIGO SYS O NETSUITE
                        codigonetsuite = p.codigovehiculo,
                        customname = "",
                        active = "True",
                        description = "PLC.:" + funciones.descomponer("placa", assetAMI) + ";MAR.:" + funciones.descomponer("marca", assetAMI) + ";MOD.:" + funciones.descomponer("modelo", assetAMI) + ";COL.:" + funciones.descomponer("color", assetAMI),
                        productexpiredate = p.cobertura,
                        product = producto,
                        chasis = funciones.descomponer("chasis", assetAMI),
                    };
                    //informacion del vehiculo atributos
                    cnstGenrl = BienSQL.CnstVehiculoParametros(opcion: "103", cliente: "", vehiculo: p.codigosys,
                                                               parametroproducto: "GPT", tipo: funciones.descomponer("tipo", assetAMI), marca: funciones.descomponer("marca", assetAMI),
                                                               modelo: funciones.descomponer("modelo", assetAMI), chasis: funciones.descomponer("chasis", assetAMI), motor: funciones.descomponer("motor", assetAMI),
                                                               placa: funciones.descomponer("placa", assetAMI), color: funciones.descomponer("color", assetAMI), anio: funciones.descomponer("anio", assetAMI));
                    if (cnstGenrl.Tables.Count > 0)
                    {
                        attributeEntidadCollection = new attributeEntidadCollection();
                        attributeCollection = new attributeEntidad();
                        for (int i = 0; i <= cnstGenrl.Tables[0].Rows.Count - 1; i++)
                        {
                            attributeEntidad attribute = new attributeEntidad();
                            attribute.AttributeId = (int)cnstGenrl.Tables[0].Rows[i]["ID"];
                            attribute.Valor = (string)cnstGenrl.Tables[0].Rows[i]["VALOR"];
                            attributeEntidadCollection.Add(attribute);
                        }
                        attributeCollection.attributeEntidadCollection = attributeEntidadCollection;
                    }
                }

              
                //login
                if (bandera)
                {
                    if (cliente.username == "")
                    {
                        bandera = false;
                        tabla.Rows.Add("-1", "El cliente cliente USERNAME no existe en AMI");
                    }
                    else if (busquedaAmiVehiculo == "" && busquedaAmiCliente == "")
                    {
                        bandera = false;
                        tabla.Rows.Add("-1", "No existe el cliente en AMI");
                    }
                    else
                    {
                        var client = new RestClient(rutalogin);
                        var request = new RestRequest("", Method.Post);
                        request.AddParameter("username", cliente.login);
                        request.AddParameter("password", cliente.password);
                        RestResponse response = client.Execute(request);
                        var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(response.Content);
                        token = funciones.descomponer("token", jsonString);
                    }
                }
            }


            // Bloque cliente propietario
            if (bandera)
            {
                var customersearch = new RestClient();
                var requestecustomersearch = new RestRequest();
                RestResponse responsecustomersearch;
                customersearch = new RestClient(rutacustomerAdd + "?username=" + cliente.username + "&is_active=" + true + "&id=" + busquedaAmiCliente + " ");
                requestecustomersearch = new RestRequest("", Method.Get);
                requestecustomersearch.AddHeader("cache-control", "no-cache");
                requestecustomersearch.AddHeader("authorization", "HunterAPI " + token);
                responsecustomersearch = customersearch.Execute(requestecustomersearch);
                valoretorno = "0";
                valoretorno = funciones.descomponer("id", responsecustomersearch.Content);
                if (valoretorno != "0")
                {
                    if (valoretorno == busquedaAmiCliente)
                    {
                        customerID = valoretorno;
                        bandera = true;
                    }
                    else
                    {
                        tabla.Rows.Add("-1", "busqueda incorrecta " + " Customer Incorrecto");
                        bandera = false;
                    }
                }
                else
                {
                    tabla.Rows.Add("-1", "Error al buscar el Customer del cliente ");
                    bandera = false;
                }
            }
           
            //bloque del vehiculo
            if (bandera)
            {
                var assetSearch = new RestClient();
                var requesteAssetSearch = new RestRequest();
                RestResponse responseAssetSearch;
                var clienteasset = new RestClient();
                var requesteasset = new RestRequest();
                RestResponse responseasset;
                string texto = "";
                assetSearch = new RestClient(rutaassetAdd + "?attributes__attribute__name=COD_SYS=" + "&attributes__value=" + asset.codigosys + " ");
                requesteAssetSearch = new RestRequest("", Method.Get);
                requesteAssetSearch.AddHeader("cache-control", "no-cache");
                requesteAssetSearch.AddHeader("authorization", "HunterAPI " + token);
                responseAssetSearch = assetSearch.Execute(requesteAssetSearch);
                valoretorno = "0";
                valoretorno = funciones.descomponer("id", responseAssetSearch.Content);
                if (valoretorno != "0")
                {
                    if (valoretorno == busquedaAmiVehiculo)
                    {
                        asset.productexpiredate = funciones.descomponer("product_expire_date", responseAssetSearch.Content).Substring(0, 10);
                        asset.product = funciones.descomponer("product", responseAssetSearch.Content);
                        texto = Utilidades.SerializarAssetInstalacion(asset, attributeEntidadCollection, valoretorno, customerID, sensors);
                        clienteasset = new RestClient(rutaassetAdd + valoretorno + "/ ");
                        requesteasset = new RestRequest("",Method.Patch);
                        requesteasset.AddHeader("content-type", "application/json");
                        requesteasset.AddHeader("authorization", "HunterAPI " + token);
                        requesteasset.AddParameter("application/json", texto, ParameterType.RequestBody);
                        responseasset = clienteasset.Execute(requesteasset);
                        if (Convert.ToInt32(responseasset.StatusCode) == 200)
                        {
                            assetID = funciones.descomponer("id", responseasset.Content);
                            bandera = true;
                        }
                        else if (Convert.ToInt32(responseasset.StatusCode) == 400)
                        {
                            string mensaje = funciones.DevuelveMensaje(responseasset.Content);
                            tabla.Rows.Add("-1", mensaje + " " + responseasset.StatusCode + " asset Upgrade");
                            bandera = false;
                        }
                        else
                        {
                            tabla.Rows.Add("-1", "Error al procesar asset " + responseasset.StatusCode + " asset Upgrade");
                            bandera = false;
                        }

                    }
                    else
                    {
                        tabla.Rows.Add("-1", "busqueda incorrecta" + " Asset Incorrecto");
                        bandera = false;
                    }
                }
                else
                {
                    tabla.Rows.Add("-1", "Error al consultar el asset, no existe" + ", Upgrade ");
                    bandera = false;
                }
            }

            // ingreso del log del proceso de AMI
            if (bandera)
            {
                string resultadoLog = ConsultaAMI.RegistroLog(p.clienteid, customerID, "0", "0", asset.codigosys, assetID, "", "", producto, p.ordenservicio, "0", p.usuario, "N", "", tiporegistro, cliente.username, p.origen, accion, "", "", "", "", "", "", "", "", "", "", p.parametroproducto, cobertura, "", "", "");


                if (resultadoLog == "OK")
                {
                    bandera = true;
                }
                else
                {
                    tabla.Rows.Add("-1", "Error en el Ingreso del Log");
                    bandera = false;
                }
            }
            
            string jsonData;
            if (bandera)
            {
                tabla.Rows.Add(1, "Guardado Exitosamente");
            }
            jsonData = (string)Utilidades.DataTableToJSON(tabla);

            return jsonData;

        }


        [HttpPost]
        [Authorize]
        [SwaggerOperation(Summary = "", Description = "Chequeo del Dispositivo")]
        [Route("ChequeoDispositivo")]
        public ActionResult<string> ChequeoDispositivo(GeneralAMIDTO p)
        {
            //conexion ruta 
            string rutalogin = "";
            string rutaassetAdd = "";
            string rutadeviceAdd = "";
            //variables
            bool bandera = true;
            DataSet infoDatos = new DataSet();
            string token = "";
            string valoretorno = "";
            string assetID = "";
            string cobertura = "";
            string tiporegistro = "CHE";
            string destino = "";
            string vid = "";
            string log_usr = "";
            string log_pwd = "";
            string valorvid = "";
            string clienteAMI = "";
            string propietarioAMI = "";
            string assetAMI = "";
            string busquedaAmiVehiculo = "";
            string busquedaAmiCliente = "";
            string deviceAMI = "";
            string producto = "";
            string accion = "002";
            DataTable tabla = new DataTable();
            tabla.Columns.Add("CODIGO", typeof(int));
            tabla.Columns.Add("MENSAJE", typeof(string));
            conexionAMI ruta = new conexionAMI();
            if (p.parametroproducto == "GPT")
            {
                rutalogin = ruta.GetRuta("1");
                rutaassetAdd = ruta.GetRuta("2");
                rutadeviceAdd = ruta.GetRuta("4");
            }
            else
            {
                bandera = false;
                tabla.Rows.Add("-1", "No existe el parametro del producto");
            }
            customerEntidad cliente = null;
            assetEntidad asset = null;
            deviceEntidad device = null;

            //datoa generales
            if (bandera)
            {
              
                DataSet cnstGenrl = new DataSet();
                cnstGenrl = BienSQL.CnstVehiculo(opcion: "100", cliente: "", vehiculo: "", parametroproducto: "GPT");
                string support = (string)cnstGenrl.Tables[0].Rows[0]["support"];
                log_usr = (string)cnstGenrl.Tables[0].Rows[0]["usuario"];
                log_pwd = (string)cnstGenrl.Tables[0].Rows[0]["pass"];
                string ami_telefono = (string)cnstGenrl.Tables[0].Rows[0]["telefono"];
                string ami_celular = (string)cnstGenrl.Tables[0].Rows[0]["celular"];
                string ruccompania = (string)cnstGenrl.Tables[0].Rows[0]["ruccompania"];

                //informacion del cliente 
                if (bandera)
                {
                    clienteAMI = ConsultaAMI.ProcesoAMIConsulta("204", p.codigovehiculo, p.producto, p.ordenservicio, p.itemNS, "");
                    //accion = funciones.descomponer("ACCION", clienteAMI);
                    busquedaAmiCliente = funciones.descomponer("ID_TELEMATIC", clienteAMI);
                    cliente = new customerEntidad
                    {
                        username = funciones.descomponer("USERNAME", clienteAMI),
                        first_name = funciones.descomponer("FIRST_NAME", clienteAMI),
                        last_name = funciones.descomponer("LAST_NAME", clienteAMI),
                        email = funciones.descomponer("EMAIL", clienteAMI),
                        login = log_usr,
                        password = log_pwd,
                        internal_identifier = funciones.descomponer("ID_CLIENTE", clienteAMI),
                        active = "True",
                        telefono = funciones.descomponer("TELEFONO_CLIENTE", clienteAMI),
                        identity_customer_type = funciones.descomponer("TIPO", clienteAMI),
                        company_code = ruccompania,
                        business_name = "",
                        emergency_phone_number = ami_telefono,
                        assistance_phone_number = ami_celular,
                        technical_support_email = support,
                    };
                }

                //proceso device
                if (bandera)
                {
                    deviceAMI = ConsultaAMI.ProcesoAMIConsulta("202", p.codigovehiculo, p.producto, p.ordenservicio, p.itemNS, "");
                    valorvid = funciones.descomponer("vid", deviceAMI);
                    device = new deviceEntidad
                    {
                        id = p.serie,
                        vid = funciones.descomponer("vid", deviceAMI),
                        company_code = ruccompania,
                        active = "True",
                        model = funciones.descomponer("id_telematic_modelo", deviceAMI),
                        report_from = funciones.descomponer("id_telematic_servidor", deviceAMI),
                        numero_celular = funciones.descomponer("noCelularSIM", deviceAMI),
                    };

                    if (valorvid == "0")
                    {
                        tabla.Rows.Add("-1", "Verificar el valor del VID ");
                        bandera = false;
                    }
                    if (device.report_from == "")
                    {
                        tabla.Rows.Add("-1", "Verificar el Servidor del Dispositivo ");
                        bandera = false;
                    }
                    if (device.model == "")
                    {
                        tabla.Rows.Add("-1", "Verificar el Modelo del Dispositivo ");
                        bandera = false;
                    }
                }

                // informacion vehiculo
                if (bandera)
                {
                    // informacion del vehiculo por codigo interno de netsuite
                    assetAMI = ConsultaAMI.ProcesoAMIConsulta("207", p.vehiculoid, "", "", "", "");
                    busquedaAmiVehiculo = funciones.descomponer("ID_TELEMATIC", assetAMI);
                    asset = new assetEntidad
                    {
                        name = funciones.descomponer("placa", assetAMI),
                        assettype = funciones.descomponer("tipo", assetAMI),
                        // codigosys= codigosys, //CODIGO SYS O NETSUITE
                        codigosys = p.codigosys, //CODIGO SYS O NETSUITE
                        codigonetsuite = p.codigovehiculo,
                        customname = "",
                        active = "True",
                        description = "PLC.:" + funciones.descomponer("placa", assetAMI) + ";MAR.:" + funciones.descomponer("marca", assetAMI) + ";MOD.:" + funciones.descomponer("modelo", assetAMI) + ";COL.:" + funciones.descomponer("color", assetAMI),
                        productexpiredate = p.cobertura,
                        product = p.producto,
                        chasis = funciones.descomponer("chasis", assetAMI),
                    };
                }
                
            }

            //validaciones  
            if (bandera)
            {
                var client = new RestClient(rutalogin);
                var request = new RestRequest("", Method.Post);
                request.AddParameter("username", cliente.login);
                request.AddParameter("password", cliente.password);
                RestResponse response = client.Execute(request);
                var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(response.Content);
                token = funciones.descomponer("token", jsonString);
                // Validaciones
                if ( p.clientemonitoreo == "" | p.codigovehiculo == "" )
                {
                    tabla.Rows.Add("-1", "Verificar datos no enviados ");
                    bandera = false;
                }
                else if (busquedaAmiVehiculo == "" )
                {
                    tabla.Rows.Add("-1", "No existe el vehiculo en AMI");
                    bandera = false;
                }
                else if (busquedaAmiCliente == "")
                {
                    tabla.Rows.Add("-1", "No existe el cliente en AMI");
                    bandera = false;
                }
            }

            // bloque del vehiculo
            if (bandera)
            {
                var assetSearch = new RestClient();
                var requesteAssetSearch = new RestRequest();
                RestResponse responseAssetSearch;
                string texto;
                var clienteasset = new RestClient();
                var requesteasset = new RestRequest();
                RestResponse responseasset;
                assetSearch = new RestClient(rutaassetAdd + "?attributes__attribute__name=COD_SYS" + "&attributes__value=" + asset.codigosys + " ");
                requesteAssetSearch = new RestRequest("", Method.Get);
                requesteAssetSearch.AddHeader("cache-control", "no-cache");
                requesteAssetSearch.AddHeader("authorization", "HunterAPI " + token);
                responseAssetSearch = assetSearch.Execute(requesteAssetSearch);
                valoretorno = "0";
                valoretorno = funciones.descomponer("id", responseAssetSearch.Content);
                if (valoretorno != "0")
                {
                    producto = funciones.descomponer("product", responseAssetSearch.Content);
                    cobertura = funciones.descomponer("product_expire_date", responseAssetSearch.Content);
                    assetID = valoretorno;      
                }
                else
                {
                    tabla.Rows.Add("-1", "Error al consultar el asset, no existe" + ", Verificar ");
                    bandera = false;
                }
            }

            // bloque del device
            if (bandera)
            {
                var devicesearch = new RestClient();
                var requestedevicesearch = new RestRequest();
                RestResponse responsedevicesearch;
                var clientedevices = new RestClient();
                var requestedevice = new RestRequest();
                RestResponse responsedevice;
                // consulta si existe el device y si se encuentra inactivo
                devicesearch = new RestClient(rutadeviceAdd + "?active=" + false + "&id=" + device.vid + "&company_code=" + device.company_code + " ");
                requestedevicesearch = new RestRequest("", Method.Get);
                requestedevicesearch.AddHeader("cache-control", "no-cache");
                requestedevicesearch.AddHeader("authorization", "HunterAPI " + token);
                responsedevicesearch = devicesearch.Execute(requestedevicesearch);
                valoretorno = "0";
                valoretorno = funciones.descomponer("id", responsedevicesearch.Content);
                if (valoretorno != "0")
                {
                    clientedevices = new RestClient(rutadeviceAdd + valoretorno + "/ ");
                    requestedevice = new RestRequest("", Method.Patch);
                    requestedevice.AddHeader("cache-control", "no-cache");
                    requestedevice.AddHeader("authorization", "HunterAPI " + token);
                    requestedevice.AddParameter("report_from", device.report_from);
                    requestedevice.AddParameter("model", device.model);
                    requestedevice.AddParameter("company_code", device.company_code);
                    requestedevice.AddParameter("id", device.vid);
                    requestedevice.AddParameter("active", false);
                    responsedevice = clientedevices.Execute(requestedevice);
                    if (Convert.ToInt32(responsedevice.StatusCode) == 200)
                        bandera = true;
                    else if (Convert.ToInt32(responsedevice.StatusCode) == 400)
                    {
                        string mensaje = funciones.DevuelveMensaje(responsedevice.Content);
                        tabla.Rows.Add("-1", mensaje + " " + responsedevice.StatusCode + " device anterior Actualiza");
                        bandera = false;
                    }
                    else
                    {
                        tabla.Rows.Add("-1", "Error al procesar devices " + responsedevice.StatusCode + " device anterior Actualiza");
                        bandera = false;
                    }
                }
                
            }

            // bloque  de  vehiculo - device
            if (bandera)
            {
                var assetDevSearch = new RestClient(rutaassetAdd + "?devices__device__id=" + device.vid + "&devices__device__company_code=" + device.company_code + "&attributes__attribute__name=COD_SYS" + "&attributes__value=" + asset.codigosys + " ");
                var requesteassetDevSearch = new RestRequest("", Method.Get);
                requesteassetDevSearch.AddHeader("cache-control", "no-cache");
                requesteassetDevSearch.AddHeader("authorization", "HunterAPI " + token);
                RestResponse responseassetDevSearch = assetDevSearch.Execute(requesteassetDevSearch);
                valoretorno = "0";
                valoretorno = funciones.descomponer("id", responseassetDevSearch.Content);
                if (valoretorno != "0")
                {
                    if (valoretorno == assetID)
                    {
                        var clienteassetdev = new RestClient(rutaassetAdd + assetID + "/device-remove/ ");
                        var requesteassetdev = new RestRequest("", Method.Patch);
                        requesteassetdev.AddHeader("cache-control", "no-cache");
                        requesteassetdev.AddHeader("authorization", "HunterAPI " + token);
                        requesteassetdev.AddParameter("id", assetID);
                        requesteassetdev.AddParameter("device_id", device.vid);
                        RestResponse responseassetdev = clienteassetdev.Execute(requesteassetdev);

                        if (Convert.ToInt32(responseassetdev.StatusCode) == 200)
                            bandera = true;
                        else if (Convert.ToInt32(responseassetdev.StatusCode) == 400)
                        {
                            tabla.Rows.Add("-1", responseassetdev.StatusCode + " remove asset-device");
                            bandera = false;
                        }
                        else
                        {
                            tabla.Rows.Add("-1", "Error al procesar assetdevice " + responseassetdev.StatusCode + "remove asset-device");
                            bandera = false;
                        }
                    }
                    else
                    {
                        tabla.Rows.Add("-1", "Error al procesar asset Id distinto remove device");
                        bandera = false;
                    }
                }
                else
                {
                    bandera = true;
                }
            }

            //ingreso del log del proceso de AMI
            if (bandera)
            {
                string resultadoLog = ConsultaAMI.RegistroLog(cliente.internal_identifier, busquedaAmiCliente, "0", "0", p.codigosys, assetID, "", device.id, producto, p.ordenservicio, "0", p.usuario, "N", "", tiporegistro, cliente.username, p.origen, accion, "", "", "", "", "", "", "", "", "", "", p.parametroproducto, p.cobertura, device.vid, p.producto, "");
                if (resultadoLog == "OK")
                {
                    //mensaje = "OK";
                    bandera = true;
                }
                else
                {
                    tabla.Rows.Add("-1", "Error en el Ingreso del Log");
                    bandera = false;
                }
            }
            
            string jsonData;
            if (bandera)
            {
                tabla.Rows.Add(1, "Guardado Exitosamente");
            }
            jsonData = (string)Utilidades.DataTableToJSON(tabla);

            return jsonData;

        }


        [HttpPost]
        [Authorize]
        [SwaggerOperation(Summary = "", Description = "Proceso de corte de la SIMCARD")]
        [Route("CorteSimCard")]
        public ActionResult<string> CorteSimCard(GeneralAMIDTO p)
        {
            //conexion ruta 
            string rutalogin = "";
            string rutaassetAdd = "";
            string rutacustomerAdd = "";
            string rutadeviceAdd = "";
            //variables
            bool bandera = true;
            //DataSet infoDatos = new DataSet();
            string token = "";
            string valoretorno = "";
            string customerID = "";
            string ownerID = "";
            string tiporegistro = "COR";
            string assetID = "";
            string accion = "002";
            string log_usr = "";
            string log_pwd = "";
            string gencomando = p.comando;
            //string producto = "";
            string busquedaAmiVehiculo = "";
            DataTable tabla = new DataTable();
            tabla.Columns.Add("CODIGO", typeof(int));
            tabla.Columns.Add("MENSAJE", typeof(string));
            conexionAMI ruta = new conexionAMI();
            if (p.parametroproducto == "GPT")
            {
                rutalogin = ruta.GetRuta("1");
                rutaassetAdd = ruta.GetRuta("2");
                rutacustomerAdd = ruta.GetRuta("3");
                rutadeviceAdd = ruta.GetRuta("4");
            }
            else
            {
                bandera = false;
                tabla.Rows.Add("-1", "No existe el parametro del producto");
            }

            customerEntidad monitoreo = null;
            assetEntidad asset = null;
            customerEntidad propietario = null;
            //attributeEntidadCollection attributeEntidadCollection = null;
            //attributeEntidad attributeCollection = null;
            //string busquedaAmiVehiculo = "";
            //string busquedaAmiCliente = "";
            //string accion = "";
            string monitoreoAMI = "";
            string assetAMI = "";
            string propietarioAMI = "";

            // datos
            if (bandera)
            {
                DataSet cnstGenrl = new DataSet();
                cnstGenrl = BienSQL.CnstVehiculo(opcion: "100", cliente: "", vehiculo: "", parametroproducto: "GPT");
                string support = (string)cnstGenrl.Tables[0].Rows[0]["support"];
                log_usr = (string)cnstGenrl.Tables[0].Rows[0]["usuario"];
                log_pwd = (string)cnstGenrl.Tables[0].Rows[0]["pass"];
                string ami_telefono = (string)cnstGenrl.Tables[0].Rows[0]["telefono"];
                string ami_celular = (string)cnstGenrl.Tables[0].Rows[0]["celular"];
                string ruccompania = (string)cnstGenrl.Tables[0].Rows[0]["ruccompania"];


                // informacion del cliente
                if (bandera)
                {
                    monitoreoAMI = ConsultaAMI.ProcesoAMIConsulta("206", "", "", "", "", p.clientemonitoreo);
                    //busquedaAmiCliente = funciones.descomponer("ID_TELEMATIC", clienteAMI);
                    monitoreo = new customerEntidad
                    {
                        username = funciones.descomponer("USERNAME", monitoreoAMI),
                        first_name = funciones.descomponer("FIRST_NAME", monitoreoAMI),
                        last_name = funciones.descomponer("LAST_NAME", monitoreoAMI),
                        email = funciones.descomponer("EMAIL", monitoreoAMI),
                        login = log_usr,
                        password = log_pwd,
                        internal_identifier = funciones.descomponer("ID_CLIENTE", monitoreoAMI),
                        active = "True",
                        telefono = funciones.descomponer("TELEFONO_CLIENTE", monitoreoAMI),
                        identity_customer_type = funciones.descomponer("TIPO", monitoreoAMI),
                        company_code = ruccompania,
                        business_name = "",
                        emergency_phone_number = ami_telefono,
                        assistance_phone_number = ami_celular,
                        technical_support_email = support,
                    };
                }

                // informacion del propietario
                if (bandera)
                {
                    propietarioAMI = ConsultaAMI.ProcesoAMIConsulta("206", "", "", "", "", p.clientepropietario);
                    //busquedaAmiCliente = funciones.descomponer("ID_TELEMATIC", clienteAMI);
                    propietario = new customerEntidad
                    {
                        username = funciones.descomponer("USERNAME", propietarioAMI),
                        first_name = funciones.descomponer("FIRST_NAME", propietarioAMI),
                        last_name = funciones.descomponer("LAST_NAME", propietarioAMI),
                        email = funciones.descomponer("EMAIL", propietarioAMI),
                        login = log_usr,
                        password = log_pwd,
                        internal_identifier = funciones.descomponer("ID_CLIENTE", propietarioAMI),
                        active = "True",
                        telefono = funciones.descomponer("TELEFONO_CLIENTE", propietarioAMI),
                        identity_customer_type = funciones.descomponer("TIPO", propietarioAMI),
                        company_code = ruccompania,
                        business_name = "",
                        emergency_phone_number = ami_telefono,
                        assistance_phone_number = ami_celular,
                        technical_support_email = support,
                    };
                }


                // informacion vehiculo
                if (bandera)
                {
                    // informacion del vehiculo por codigo interno de netsuite
                    assetAMI = ConsultaAMI.ProcesoAMIConsulta("207", p.vehiculoid, "", "", "", "");
                    busquedaAmiVehiculo = funciones.descomponer("ID_TELEMATIC", assetAMI);
                    asset = new assetEntidad
                    {
                        name = funciones.descomponer("placa", assetAMI),
                        assettype = funciones.descomponer("tipo", assetAMI),
                        // codigosys= codigosys, //CODIGO SYS O NETSUITE
                        codigosys = p.codigosys, //CODIGO SYS O NETSUITE
                        codigonetsuite = p.codigovehiculo,
                        customname = "",
                        active = "True",
                        description = "PLC.:" + funciones.descomponer("placa", assetAMI) + ";MAR.:" + funciones.descomponer("marca", assetAMI) + ";MOD.:" + funciones.descomponer("modelo", assetAMI) + ";COL.:" + funciones.descomponer("color", assetAMI),
                        productexpiredate = p.cobertura,
                        product = p.producto,
                        chasis = funciones.descomponer("chasis", assetAMI),
                    };
                }

                //login
                if (bandera)
                {
                    if (propietario.username == "")
                    {
                        bandera = false;
                        tabla.Rows.Add("-1", "El cliente cliente USERNAME no existe en AMI");
                    }
                    //else if (busquedaAmiVehiculo == "" && busquedaAmiCliente == "")
                    //{
                    //    bandera = false;
                    //    tabla.Rows.Add("-1", "No existe el cliente en AMI");
                    //}
                    else
                    {
                        var client = new RestClient(rutalogin);
                        var request = new RestRequest("", Method.Post);
                        request.AddParameter("username", propietario.login);
                        request.AddParameter("password", propietario.password);
                        RestResponse response = client.Execute(request);
                        var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(response.Content);
                        token = funciones.descomponer("token", jsonString);
                    }
                }
            }


            //bloque cliente monitoreo
            if (bandera)
            {
                var customersearch = new RestClient();
                var requestecustomersearch = new RestRequest();
                RestResponse responsecustomersearch;
                customersearch = new RestClient(rutacustomerAdd + "?username=" + monitoreo.username + "&is_active=" + true +"&customer__company_code=" + monitoreo.company_code + " ");
                requestecustomersearch = new RestRequest("", Method.Get);
                requestecustomersearch.AddHeader("cache-control", "no-cache");
                requestecustomersearch.AddHeader("authorization", "HunterAPI " + token);
                responsecustomersearch = customersearch.Execute(requestecustomersearch);
                valoretorno = "0";
                valoretorno = funciones.descomponer("id", responsecustomersearch.Content);
                if (valoretorno != "0")
                {
                    customerID = valoretorno;
                    bandera = true;
                }
                else
                {
                    tabla.Rows.Add("-1", "Error al buscar el Customer  ");
                    bandera = false;
                }
            }

            // bloque cliente monitoreo propietario
            if (bandera)
            {
                var customersearch = new RestClient();
                var requestecustomersearch = new RestRequest();
                RestResponse responsecustomersearch;
                customersearch = new RestClient(rutacustomerAdd + "?username=" + propietario.username + "&is_active=" + true + "&customer__company_code=" + propietario.company_code + " ");
                requestecustomersearch = new RestRequest("", Method.Get);
                requestecustomersearch.AddHeader("cache-control", "no-cache");
                requestecustomersearch.AddHeader("authorization", "HunterAPI " + token);
                responsecustomersearch = customersearch.Execute(requestecustomersearch);
                valoretorno = "0";
                valoretorno = funciones.descomponer("id", responsecustomersearch.Content);
                if (valoretorno != "0")
                {
                    ownerID = valoretorno;
                    bandera = true;
                }
                else
                {
                    tabla.Rows.Add("-1", "Error al buscar el Customer Propietario ");
                    bandera = false;
                }
            }

            // bloque del device
            if (bandera)
            {
                var devicesearch = new RestClient();
                var requestedevicesearch = new RestRequest();
                RestResponse responsedevicesearch;
                var clientedevices = new RestClient();
                var requestedevice = new RestRequest();
                RestResponse responsedevice;
                devicesearch = new RestClient(rutadeviceAdd + "?active=" + true + "&id=" + p.vid + "&company_code=" + propietario.company_code + " ");
                requestedevicesearch = new RestRequest("", Method.Get);
                requestedevicesearch.AddHeader("cache-control", "no-cache");
                requestedevicesearch.AddHeader("authorization", "HunterAPI " + token);
                responsedevicesearch = devicesearch.Execute(requestedevicesearch);
                valoretorno = "0";
                valoretorno = funciones.descomponer("id", responsedevicesearch.Content);
                if (valoretorno != "0")
                {
                    clientedevices = new RestClient(rutadeviceAdd + valoretorno + "/ ");
                    requestedevice = new RestRequest("", Method.Patch);
                    requestedevice.AddHeader("cache-control", "no-cache");
                    requestedevice.AddHeader("authorization", "HunterAPI " + token);
                    requestedevice.AddParameter("id", valoretorno);
                    requestedevice.AddParameter("active", false);
                    responsedevice = clientedevices.Execute(requestedevice);
                    if (Convert.ToInt32(responsedevice.StatusCode) == 200)
                        bandera = true;
                    else if (Convert.ToInt32(responsedevice.StatusCode) == 400)
                    {
                        string mensaje = funciones.DevuelveMensaje(responsedevice.Content);
                        tabla.Rows.Add("-1", mensaje + " " + responsedevice.StatusCode + " device ");
                        bandera = false;
                    }
                    else
                    {
                        tabla.Rows.Add("-1", "Error al procesar devices " + responsedevice.StatusCode + " device ");
                        bandera = false;
                    }
                }
            }

            // ingreso del log del proceso de AMI
            if (bandera)
            {
                string resultadoLog = ConsultaAMI.RegistroLog(p.clientepropietario, ownerID, p.clientemonitoreo, customerID, asset.codigosys, assetID, gencomando, p.serie, p.producto, "0", "0", p.usuario, "N", monitoreo.username, tiporegistro, propietario.username, p.origen, accion, "", "", "", "", "", "", "", "", "", "", p.parametroproducto, p.cobertura, p.vid, "", "");

                if (resultadoLog == "OK")
                {
                    bandera = true;
                }
                else
                {
                    tabla.Rows.Add("-1", "Error en el Ingreso del Log");
                    bandera = false;
                }
            }
            string jsonData;
            if (bandera)
            {
                tabla.Rows.Add(1, "Guardado Exitosamente");
            }
            jsonData = (string)Utilidades.DataTableToJSON(tabla);

            return jsonData;

        }


        [HttpPost]
        [Authorize]
        [SwaggerOperation(Summary = "", Description = "Reconexion de la SIMCARD")]
        [Route("ReconexionSimCard")]
        public ActionResult<string> ReconexionSimCard(GeneralAMIDTO p)
        {
            //conexion ruta 
            string rutalogin = "";
            string rutaassetAdd = "";
            string rutacustomerAdd = "";
            string rutadeviceAdd = "";
            string rutacommandAdd = "";
            string rutausercommandAdd = "";
            string rutadevice = "";
            //variables
            bool bandera = true;
            //DataSet infoDatos = new DataSet();
            string token = "";
            string valoretorno = "";
            string customerID = "";
            string assetID = "";
            string ownerID = "";
            string accion = "";
            string tiporegistro = "REC";
            string sensors = "0";
            string log_usr = "";
            string log_pwd = "";
            string valorvid = "";
            string mensajetipo = "Reconexion Sim Card";
            string producto = "";
            string concesionario = "";
            char delimiter = ',';
            string gencomando = p.comando;
            string[] comandos = p.comando.Split(delimiter);
            DataTable tabla = new DataTable();
            tabla.Columns.Add("CODIGO", typeof(int));
            tabla.Columns.Add("MENSAJE", typeof(string));
            conexionAMI ruta = new conexionAMI();
            if (p.parametroproducto == "GPT")
            {
                rutalogin = ruta.GetRuta("1");
                rutaassetAdd = ruta.GetRuta("2");
                rutacustomerAdd = ruta.GetRuta("3");
                rutadeviceAdd = ruta.GetRuta("4");
                rutacommandAdd = ruta.GetRuta("5");
                rutausercommandAdd = ruta.GetRuta("6");
                rutadevice = ruta.GetRuta("14");
            }
            else
            {
                bandera = false;
                tabla.Rows.Add("-1", "No existe el parametro del producto");
            }
            customerEntidad monitoreo = null;
            customerEntidad propietario = null;
            assetEntidad asset = null;
            deviceEntidad device = null;
            attributeEntidadCollection attributeEntidadCollection = null;
            attributeEntidad attributeCollection = null;
            //string busquedaAmiVehiculo = "";
            //string busquedaAmiCliente = "";
            string propietarioAMI = "";
            string monitoreoAMI = "";
            string deviceAMI = "";
            string assetAMI = "";

            // datos
            if (bandera)
            {
                DataSet cnstGenrl = new DataSet();
                cnstGenrl = BienSQL.CnstVehiculo(opcion: "100", cliente: "", vehiculo: "", parametroproducto: "GPT");
                string support = (string)cnstGenrl.Tables[0].Rows[0]["support"];
                log_usr = (string)cnstGenrl.Tables[0].Rows[0]["usuario"];
                log_pwd = (string)cnstGenrl.Tables[0].Rows[0]["pass"];
                string ami_telefono = (string)cnstGenrl.Tables[0].Rows[0]["telefono"];
                string ami_celular = (string)cnstGenrl.Tables[0].Rows[0]["celular"];
                string ruccompania = (string)cnstGenrl.Tables[0].Rows[0]["ruccompania"];


                // informacion del cliente
                if (bandera)
                {
                    monitoreoAMI = ConsultaAMI.ProcesoAMIConsulta("206", "", "", "", "", p.clientemonitoreo);
                    //busquedaAmiCliente = funciones.descomponer("ID_TELEMATIC", clienteAMI);
                    monitoreo = new customerEntidad
                    {
                        username = funciones.descomponer("USERNAME", monitoreoAMI),
                        first_name = funciones.descomponer("FIRST_NAME", monitoreoAMI),
                        last_name = funciones.descomponer("LAST_NAME", monitoreoAMI),
                        email = funciones.descomponer("EMAIL", monitoreoAMI),
                        login = log_usr,
                        password = log_pwd,
                        internal_identifier = funciones.descomponer("ID_CLIENTE", monitoreoAMI),
                        active = "True",
                        telefono = funciones.descomponer("TELEFONO_CLIENTE", monitoreoAMI),
                        identity_customer_type = funciones.descomponer("TIPO", monitoreoAMI),
                        company_code = ruccompania,
                        business_name = "",
                        emergency_phone_number = ami_telefono,
                        assistance_phone_number = ami_celular,
                        technical_support_email = support,
                    };
                }

                // informacion del propietario
                if (bandera)
                {
                    propietarioAMI = ConsultaAMI.ProcesoAMIConsulta("206", "", "", "", "", p.clientepropietario);
                    //busquedaAmiCliente = funciones.descomponer("ID_TELEMATIC", clienteAMI);
                    propietario = new customerEntidad
                    {
                        username = funciones.descomponer("USERNAME", propietarioAMI),
                        first_name = funciones.descomponer("FIRST_NAME", propietarioAMI),
                        last_name = funciones.descomponer("LAST_NAME", propietarioAMI),
                        email = funciones.descomponer("EMAIL", propietarioAMI),
                        login = log_usr,
                        password = log_pwd,
                        internal_identifier = funciones.descomponer("ID_CLIENTE", propietarioAMI),
                        active = "True",
                        telefono = funciones.descomponer("TELEFONO_CLIENTE", propietarioAMI),
                        identity_customer_type = funciones.descomponer("TIPO", propietarioAMI),
                        company_code = ruccompania,
                        business_name = "",
                        emergency_phone_number = ami_telefono,
                        assistance_phone_number = ami_celular,
                        technical_support_email = support,
                    };
                }

                //proceso device
                if (bandera)
                {
                    deviceAMI = ConsultaAMI.ProcesoAMIConsulta("202", p.codigovehiculo, p.producto, p.ordenservicio, p.itemNS, "");
                    valorvid = funciones.descomponer("vid", deviceAMI);
                    device = new deviceEntidad
                    {
                        id = p.serie,
                        vid = funciones.descomponer("vid", deviceAMI),
                        company_code = ruccompania,
                        active = "True",
                        model = funciones.descomponer("id_telematic_modelo", deviceAMI),
                        report_from = funciones.descomponer("id_telematic_servidor", deviceAMI),
                        numero_celular = funciones.descomponer("noCelularSIM", deviceAMI),
                    };

                    if (valorvid == "0")
                    {
                        tabla.Rows.Add("-1", "Verificar el valor del VID ");
                        bandera = false;
                    }
                    if (device.report_from == "")
                    {
                        tabla.Rows.Add("-1", "Verificar el Servidor del Dispositivo ");
                        bandera = false;
                    }
                    if (device.model == "")
                    {
                        tabla.Rows.Add("-1", "Verificar el Modelo del Dispositivo ");
                        bandera = false;
                    }
                }

                // informacion del vehiculo y atributos
                if (bandera)
                {
                    // informacion del vehiculo
                    assetAMI = ConsultaAMI.ProcesoAMIConsulta("203", p.codigovehiculo, p.producto, p.ordenservicio, p.itemNS, "");
                    if (funciones.descomponer("familiadescripcion", assetAMI) == "HUNTER GPS HMT")
                    {
                        producto = "H.MONITOREO AMI";
                    }
                    else if (funciones.descomponer("familiadescripcion", assetAMI) == "AMI CHANGAN BY HUNTER")
                    {
                        producto = "H.MONITOREO AMI";
                    }
                    else if (funciones.descomponer("familiadescripcion", assetAMI) == "ANDOR BY HUNTER")
                    {
                        producto = "ANDOR LINK BY HUNTER";
                    }
                    else
                    {
                        producto = funciones.descomponer("familiadescripcion", assetAMI);
                    }
                    concesionario = funciones.descomponer("concesionarios", assetAMI);
                    cnstGenrl = BienSQL.CnstProducto(opcion: "104", concesionario: concesionario, producto: p.producto);
                    if (cnstGenrl.Tables[0].Rows.Count > 0)
                    {
                        producto = (string)cnstGenrl.Tables[0].Rows[0]["NOMBRE_PRODUCTO"];
                    }
                    asset = new assetEntidad
                    {
                        name = funciones.descomponer("placa", assetAMI),
                        assettype = funciones.descomponer("tipo", assetAMI),
                        // codigosys= codigosys, //CODIGO SYS O NETSUITE
                        codigosys = p.codigosys, //CODIGO SYS O NETSUITE
                        codigonetsuite = p.codigovehiculo,
                        customname = "",
                        active = "True",
                        description = "PLC.:" + funciones.descomponer("placa", assetAMI) + ";MAR.:" + funciones.descomponer("marca", assetAMI) + ";MOD.:" + funciones.descomponer("modelo", assetAMI) + ";COL.:" + funciones.descomponer("color", assetAMI),
                        productexpiredate = p.cobertura,
                        product = producto,
                        chasis = funciones.descomponer("chasis", assetAMI),
                    };
                    //informacion del vehiculo atributos
                    cnstGenrl = BienSQL.CnstVehiculoParametros(opcion: "103", cliente: "", vehiculo: p.codigosys,
                                                               parametroproducto: "GPT", tipo: funciones.descomponer("tipo", assetAMI), marca: funciones.descomponer("marca", assetAMI),
                                                               modelo: funciones.descomponer("modelo", assetAMI), chasis: funciones.descomponer("chasis", assetAMI), motor: funciones.descomponer("motor", assetAMI),
                                                               placa: funciones.descomponer("placa", assetAMI), color: funciones.descomponer("color", assetAMI), anio: funciones.descomponer("anio", assetAMI));
                    if (cnstGenrl.Tables.Count > 0)
                    {
                        attributeEntidadCollection = new attributeEntidadCollection();
                        attributeCollection = new attributeEntidad();
                        for (int i = 0; i <= cnstGenrl.Tables[0].Rows.Count - 1; i++)
                        {
                            attributeEntidad attribute = new attributeEntidad();
                            attribute.AttributeId = (int)cnstGenrl.Tables[0].Rows[i]["ID"];
                            attribute.Valor = (string)cnstGenrl.Tables[0].Rows[i]["VALOR"];
                            attributeEntidadCollection.Add(attribute);
                        }
                        attributeCollection.attributeEntidadCollection = attributeEntidadCollection;
                    }
                }

                //login
                if (bandera)
                {
                    if (propietario.username == "")
                    {
                        bandera = false;
                        tabla.Rows.Add("-1", "El cliente cliente USERNAME no existe en AMI");
                    }
                    //else if (busquedaAmiVehiculo == "" && busquedaAmiCliente == "")
                    //{
                    //    bandera = false;
                    //    tabla.Rows.Add("-1", "No existe el cliente en AMI");
                    //}
                    else
                    {
                        var client = new RestClient(rutalogin);
                        var request = new RestRequest("", Method.Post);
                        request.AddParameter("username", propietario.login);
                        request.AddParameter("password", propietario.password);
                        RestResponse response = client.Execute(request);
                        var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(response.Content);
                        token = funciones.descomponer("token", jsonString);
                    }
                }
            }

            //customer propietario
            if (bandera)
            {
                var customersearch = new RestClient();
                var requestecustomersearch = new RestRequest();
                RestResponse responsecustomersearch;
                //var customer = new RestClient();
                //var requeste = new RestRequest();
                //RestResponse responsecustomer;
                //string textocustomer;
                //string accountType = "1";
                if (p.clientepropietario != p.clientemonitoreo)
                {
                    customersearch = new RestClient(rutacustomerAdd + "?username=" + propietario.username + "&is_active=" + true + "&customer__company_code=" + propietario.company_code + "&customer__identity_document_number=" + propietario.internal_identifier + " ");
                    requestecustomersearch = new RestRequest("", Method.Get);
                    requestecustomersearch.AddHeader("cache-control", "no-cache");
                    requestecustomersearch.AddHeader("authorization", "HunterAPI " + token);
                    responsecustomersearch = customersearch.Execute(requestecustomersearch);
                    valoretorno = "0";
                    valoretorno = funciones.descomponer("id", responsecustomersearch.Content);
                    if (valoretorno != "0")
                    {
                        customerID = valoretorno;
                        ownerID = customerID;
                    }
                    else
                    {
                        tabla.Rows.Add("-1", "No se encontró el UserName propietario customer para Actualizar " + mensajetipo);
                        bandera = false;

                    }
                }
            }

            //customer monitoreo
            if (bandera)
            {
                var customersearch = new RestClient();
                var requestecustomersearch = new RestRequest();
                RestResponse responsecustomersearch;
                customersearch = new RestClient(rutacustomerAdd + "?username=" + monitoreo.username + "&is_active=" + true + "&customer__company_code=" + propietario.company_code + "&customer__identity_document_number=" + propietario.internal_identifier + " ");
                requestecustomersearch = new RestRequest("", Method.Get);
                requestecustomersearch.AddHeader("cache-control", "no-cache");
                requestecustomersearch.AddHeader("authorization", "HunterAPI " + token);
                responsecustomersearch = customersearch.Execute(requestecustomersearch);
                valoretorno = "0";
                valoretorno = funciones.descomponer("id", responsecustomersearch.Content);
                if (valoretorno != "0")
                {
                    customerID = valoretorno;
                    ownerID = customerID;
                }
                else
                {
                    tabla.Rows.Add("-1", "No se encontró el UserName de monitoreo customer para Actualizar  " + mensajetipo);
                    bandera = false;

                }
            }

            //bloque del device
            if (bandera)
            {
                var devicesearch = new RestClient();
                var requestedevicesearch = new RestRequest();
                RestResponse responsedevicesearch;
                var clientedevices = new RestClient();
                var requestedevice = new RestRequest();
                RestResponse responsedevice;
                // consulta si existe el device y si se encuentra inactivo
                devicesearch = new RestClient(rutadeviceAdd + "?active=" + false + "&id=" + device.vid + "&company_code=" + device.company_code + " ");
                requestedevicesearch = new RestRequest("", Method.Get);
                requestedevicesearch.AddHeader("cache-control", "no-cache");
                requestedevicesearch.AddHeader("authorization", "HunterAPI " + token);
                responsedevicesearch = devicesearch.Execute(requestedevicesearch);
                valoretorno = "0";
                valoretorno = funciones.descomponer("id", responsedevicesearch.Content);
                if (valoretorno != "0")
                {
                    clientedevices = new RestClient(rutadeviceAdd + valoretorno + "/ ");
                    requestedevice = new RestRequest("", Method.Patch);
                    requestedevice.AddHeader("cache-control", "no-cache");
                    requestedevice.AddHeader("authorization", "HunterAPI " + token);
                    requestedevice.AddParameter("report_from", device.report_from);
                    requestedevice.AddParameter("model", device.model);
                    requestedevice.AddParameter("company_code", device.company_code);
                    requestedevice.AddParameter("id", device.vid);
                    requestedevice.AddParameter("active", device.active);
                    responsedevice = clientedevices.Execute(requestedevice);
                    if (Convert.ToInt32(responsedevice.StatusCode) == 200)
                        bandera = true;
                    else if (Convert.ToInt32(responsedevice.StatusCode) == 400)
                    {
                        string mensaje = funciones.DevuelveMensaje(responsedevice.Content);
                        tabla.Rows.Add("-1", mensaje + " " + responsedevice.StatusCode + " device Actualiza " + mensajetipo);
                        bandera = false;
                    }
                    else
                    {
                        tabla.Rows.Add("-1", "Error al procesar devices " + responsedevice.StatusCode + " device Actualiza " + mensajetipo);
                        bandera = false;
                    }
                }
                else
                {
                    devicesearch = new RestClient(rutadeviceAdd + "?active=" + device.active + "&id=" + device.vid + "&company_code=" + device.company_code + " ");
                    requestedevicesearch = new RestRequest("", Method.Get);
                    requestedevicesearch.AddHeader("cache-control", "no-cache");
                    requestedevicesearch.AddHeader("authorization", "HunterAPI " + token);
                    responsedevicesearch = devicesearch.Execute(requestedevicesearch);
                    valoretorno = "0";
                    valoretorno = funciones.descomponer("id", responsedevicesearch.Content);
                    if (valoretorno == "0")
                    {
                        clientedevices = new RestClient(rutadevice);
                        requestedevice = new RestRequest("", Method.Post);
                        requestedevice.AddHeader("cache-control", "no-cache");
                        requestedevice.AddHeader("authorization", "HunterAPI " + token);
                        requestedevice.AddParameter("report_from", device.report_from);
                        requestedevice.AddParameter("model", device.model);
                        requestedevice.AddParameter("company_code", device.company_code);
                        requestedevice.AddParameter("id", device.vid);
                        requestedevice.AddParameter("active", device.active);
                        responsedevice = clientedevices.Execute(requestedevice);
                        if (Convert.ToInt32(responsedevice.StatusCode) == 201)
                            bandera = true;
                        else if (Convert.ToInt32(responsedevice.StatusCode) == 400)
                        {
                            string mensaje = funciones.DevuelveMensaje(responsedevice.Content);
                            tabla.Rows.Add("-1", mensaje + " " + responsedevice.StatusCode + " device");
                            bandera = false;
                        }
                        else
                        {
                            tabla.Rows.Add("-1", "Error al procesar devices " + responsedevice.StatusCode + " device");
                            bandera = false;
                        }
                    }
                }
            }

            //bloque del vehiculo
            if (bandera)
            {
                var assetSearch = new RestClient();
                var requesteAssetSearch = new RestRequest();
                RestResponse responseAssetSearch;
                string texto;
                var clienteasset = new RestClient();
                var requesteasset = new RestRequest();
                RestResponse responseasset;
                assetSearch = new RestClient(rutaassetAdd + "?attributes__attribute__name=COD_SYS" + "&attributes__value=" + asset.codigosys + " ");
                requesteAssetSearch = new RestRequest("", Method.Get);
                requesteAssetSearch.AddHeader("cache-control", "no-cache");
                requesteAssetSearch.AddHeader("authorization", "HunterAPI " + token);
                responseAssetSearch = assetSearch.Execute(requesteAssetSearch);
                valoretorno = "0";
                valoretorno = funciones.descomponer("id", responseAssetSearch.Content);
                if (valoretorno != "0")
                {
                    sensors = funciones.descomponer("doors_sensors", responseAssetSearch.Content);
                    texto = Utilidades.SerializarAssetInstalacion(asset, attributeEntidadCollection, valoretorno, ownerID, sensors);
                    clienteasset = new RestClient(rutaassetAdd + valoretorno + "/ ");
                    requesteasset = new RestRequest("", Method.Patch);
                    requesteasset.AddHeader("content-type", "application/json");
                    requesteasset.AddHeader("authorization", "HunterAPI " + token);
                    requesteasset.AddParameter("application/json", texto, ParameterType.RequestBody);
                    responseasset = clienteasset.Execute(requesteasset);
                    if (Convert.ToInt32(responseasset.StatusCode) == 200)
                    {
                        assetID = funciones.descomponer("id", responseasset.Content);
                        bandera = true;
                    }
                    else if (Convert.ToInt32(responseasset.StatusCode) == 400)
                    {
                        string mensaje = funciones.DevuelveMensaje(responseasset.Content);
                        tabla.Rows.Add("-1", mensaje + " " + responseasset.StatusCode + " asset Actualizar " + mensajetipo);
                        bandera = false;
                    }
                    else
                    {
                        tabla.Rows.Add("-1", "Error al procesar asset " + responseasset.StatusCode + " asset Actualizar " + mensajetipo);
                        bandera = false;
                    }
                }
                else
                {
                    tabla.Rows.Add("-1", "No se encontró el asset para Actualizar  " + mensajetipo);
                    bandera = false;
                }
            }

            //bloque  de  vehiculo - usuario
            if (bandera)
            {
                // consulta si existe la relacion vehiculo - usuario
                var assetUserSearch = new RestClient(rutaassetAdd + "?user__email=" + monitoreo.email + "&attributes__attribute__name=COD_SYS" + "&attributes__value=" + asset.codigosys + " ");
                var requesteassetUserSearch = new RestRequest("", Method.Get);
                requesteassetUserSearch.AddHeader("cache-control", "no-cache");
                requesteassetUserSearch.AddHeader("authorization", "HunterAPI " + token);
                RestResponse responseUserSearch = assetUserSearch.Execute(requesteassetUserSearch);
                valoretorno = "0";
                valoretorno = funciones.descomponer("id", responseUserSearch.Content);
                if (valoretorno == "0")
                {
                    var clienteassetuser = new RestClient(rutaassetAdd + assetID + "/user-add/ ");
                    var requesteassetuser = new RestRequest("", Method.Patch);
                    requesteassetuser.AddHeader("cache-control", "no-cache");
                    requesteassetuser.AddHeader("authorization", "HunterAPI " + token);
                    requesteassetuser.AddParameter("id", assetID);
                    requesteassetuser.AddParameter("user_id", customerID);
                    RestResponse responseassetuser = clienteassetuser.Execute(requesteassetuser);
                    if (Convert.ToInt32(responseassetuser.StatusCode) == 200)
                        bandera = true;
                    else if (Convert.ToInt32(responseassetuser.StatusCode) == 400)
                    {
                        string mensaje = funciones.DevuelveMensaje(responseassetuser.Content);
                        tabla.Rows.Add("-1", mensaje + " " + responseassetuser.StatusCode + " asset-usuario " + mensajetipo);
                        bandera = false;
                    }
                    else
                    {
                        tabla.Rows.Add("-1", "Error al procesar assetuser " + responseassetuser.StatusCode + " asset-usuario " + mensajetipo);
                        bandera = false;
                    }
                }
                else
                {
                    bandera = true;
                }
            }

            // bloque  de vehiculo - device
            if (bandera)
            {
                // consulta si existe la relacion vehiculo - device
                var assetDevSearch = new RestClient(rutaassetAdd + "?devices__device__id=" + device.vid + "&attributes__attribute__name=COD_SYS" + "&attributes__value=" + asset.codigosys + " ");
                var requesteassetDevSearch = new RestRequest("", Method.Get);
                requesteassetDevSearch.AddHeader("cache-control", "no-cache");
                requesteassetDevSearch.AddHeader("authorization", "HunterAPI " + token);
                RestResponse responseassetDevSearch = assetDevSearch.Execute(requesteassetDevSearch);
                valoretorno = "0";
                valoretorno = funciones.descomponer("id", responseassetDevSearch.Content);
                if (valoretorno == "0")
                {
                    var clienteassetdev = new RestClient(rutaassetAdd + assetID + "/device-add/ ");
                    var requesteassetdev = new RestRequest("", Method.Patch);
                    requesteassetdev.AddHeader("cache-control", "no-cache");
                    requesteassetdev.AddHeader("authorization", "HunterAPI " + token);
                    requesteassetdev.AddParameter("id", assetID);
                    requesteassetdev.AddParameter("device_id", device.vid);
                    RestResponse responseassetdev = clienteassetdev.Execute(requesteassetdev);
                    if (Convert.ToInt32(responseassetdev.StatusCode) == 200)
                        bandera = true;
                    else if (Convert.ToInt32(responseassetdev.StatusCode) == 400)
                    {
                        tabla.Rows.Add("-1", responseassetdev.StatusCode + " asset-device " + mensajetipo);
                        bandera = false;
                    }
                    else
                    {
                        tabla.Rows.Add("-1", "Error al procesar assetdevice " + responseassetdev.StatusCode + " asset-device " + mensajetipo);
                        bandera = false;
                    }
                }
                else
                {
                    bandera = true;
                }
            }

            // bloque  de  vehiculo - comando
            if (bandera)
            {
                if (p.comando != "")
                {
                    var commandSearch = new RestClient();
                    var requesteCommandSearch = new RestRequest();
                    RestResponse responseCommandSearch;
                    var clientecommand = new RestClient();
                    var requestecommand = new RestRequest();
                    RestResponse responsecommand;
                    foreach (var comando in comandos)
                    {
                        commandSearch = new RestClient(rutacommandAdd + "?asset=" + assetID + "&command=" + comando + "&status=" + 0 + " ");
                        requesteCommandSearch = new RestRequest("", Method.Get);
                        requesteCommandSearch.AddHeader("cache-control", "no-cache");
                        requesteCommandSearch.AddHeader("authorization", "HunterAPI " + token);
                        responseCommandSearch = commandSearch.Execute(requesteCommandSearch);
                        valoretorno = "0";
                        valoretorno = funciones.descomponer("id", responseCommandSearch.Content);
                        if (valoretorno != "0")
                        {
                            clientecommand = new RestClient(rutacommandAdd + valoretorno + "/ ");
                            requestecommand = new RestRequest("", Method.Patch);
                            requestecommand.AddHeader("cache-control", "no-cache");
                            requestecommand.AddHeader("authorization", "HunterAPI " + token);
                            requestecommand.AddParameter("status", 1);
                            requestecommand.AddParameter("id", valoretorno);
                            requestecommand.AddParameter("command", comando);
                            requestecommand.AddParameter("asset", assetID);
                            responsecommand = clientecommand.Execute(requestecommand);
                            if (Convert.ToInt32(responsecommand.StatusCode) == 200)
                                bandera = true;
                            else if (Convert.ToInt32(responsecommand.StatusCode) == 400)
                            {
                                string mensaje = funciones.DevuelveMensaje(responsecommand.Content);
                                tabla.Rows.Add("-1", mensaje + " " + responsecommand.StatusCode + " asset-comando Actualizar " + mensajetipo);
                                bandera = false;
                                break;
                            }
                            else
                            {
                                tabla.Rows.Add("-1", "Error al procesar asset command " + responsecommand.StatusCode + " asset-comando Actualizar " + mensajetipo);
                                bandera = false;
                                break;
                            }
                        }
                        else
                        {
                            // consulta si existe la relacion vehiculo - comando - activos
                            commandSearch = new RestClient(rutacommandAdd + "?asset=" + assetID + "&command=" + comando + "&status=" + 1 + " ");
                            requesteCommandSearch = new RestRequest("", Method.Get);
                            requesteCommandSearch.AddHeader("cache-control", "no-cache");
                            requesteCommandSearch.AddHeader("authorization", "HunterAPI " + token);
                            responseCommandSearch = commandSearch.Execute(requesteCommandSearch);
                            valoretorno = "0";
                            valoretorno = funciones.descomponer("id", responseCommandSearch.Content);
                            if (valoretorno == "0")
                            {
                                tabla.Rows.Add("-1", "No se encontró asset command para Actualizar, asset-comando " + mensajetipo);
                                bandera = false;
                                break;
                               
                            }
                        }
                    }
                }
            }

            // bloque  de  comando
            if (bandera)
            {
                if (p.comando != "")
                {
                    var userCommandSearch = new RestClient();
                    var requesteuserCommandSearch = new RestRequest();
                    RestResponse responseuserCommandSearch;
                    var clienteusercommand = new RestClient();
                    var requesteusercommand = new RestRequest();
                    RestResponse responseusercommand;
                    foreach (var comando in comandos)
                    {
                        // consulta si existe el comando por vehiculo - comando - usuario - estado falso
                        userCommandSearch = new RestClient(rutausercommandAdd + "?asset=" + assetID + "&command=" + comando + "&user=" + customerID + "&can_execute=" + false + " ");
                        requesteuserCommandSearch = new RestRequest("", Method.Get);
                        requesteuserCommandSearch.AddHeader("cache-control", "no-cache");
                        requesteuserCommandSearch.AddHeader("authorization", "HunterAPI " + token);
                        responseuserCommandSearch = userCommandSearch.Execute(requesteuserCommandSearch);
                        valoretorno = "0";
                        valoretorno = funciones.descomponer("id", responseuserCommandSearch.Content);
                        if (valoretorno != "0")
                        {
                            clienteusercommand = new RestClient(rutausercommandAdd + valoretorno + "/ ");
                            requesteusercommand = new RestRequest("", Method.Patch);
                            requesteusercommand.AddHeader("cache-control", "no-cache");
                            requesteusercommand.AddHeader("authorization", "HunterAPI " + token);
                            requesteusercommand.AddParameter("id", valoretorno);
                            requesteusercommand.AddParameter("command", comando);
                            requesteusercommand.AddParameter("user", customerID);
                            requesteusercommand.AddParameter("asset", assetID);
                            requesteusercommand.AddParameter("can_execute", true);
                            responseusercommand = clienteusercommand.Execute(requesteusercommand);
                            if (Convert.ToInt32(responseusercommand.StatusCode) == 200)
                                bandera = true;
                            else if (Convert.ToInt32(responseusercommand.StatusCode) == 400)
                            {
                                string mensaje = funciones.DevuelveMensaje(responseusercommand.Content);
                                tabla.Rows.Add("-1", mensaje + " " + responseusercommand.StatusCode + " comando-usuario Actualizar " + mensajetipo);
                                bandera = false;
                                break;
                            }
                            else
                            {
                                tabla.Rows.Add("-1", "Error al procesar user asset command " + responseusercommand.StatusCode + " comando-usuario Actualizar " + mensajetipo);
                                bandera = false;
                                break;
                            }
                        }
                        else
                        {
                            // consulta si existe el comando por vehiculo - comando - usuario - estado
                            userCommandSearch = new RestClient(rutausercommandAdd + "?asset=" + assetID + "&command=" + comando + "&user=" + customerID + "&can_execute=" + true + " ");
                            requesteuserCommandSearch = new RestRequest("", Method.Get);
                            requesteuserCommandSearch.AddHeader("cache-control", "no-cache");
                            requesteuserCommandSearch.AddHeader("authorization", "HunterAPI " + token);
                            responseuserCommandSearch = userCommandSearch.Execute(requesteuserCommandSearch);
                            valoretorno = "0";
                            valoretorno = funciones.descomponer("id", responseuserCommandSearch.Content);
                            if (valoretorno == "0")
                            {
                                tabla.Rows.Add("-1", "No se encontró asset command para Actualizar, comando-usuario " + mensajetipo);
                                bandera = false;
                                break;
                            }
                        }
                    }
                }
            }
            // ingreso del log del proceso de AMI
            if (bandera)
            {
                string resultadoLog = ConsultaAMI.RegistroLog(p.clientepropietario, ownerID, p.clientemonitoreo, customerID, asset.codigosys, assetID, gencomando, p.serie, p.producto, p.ordenservicio, "0", p.usuario, "N", monitoreo.username, tiporegistro, propietario.username, p.origen, accion, "", "", "", "", "", "", "", "", "", "", p.parametroproducto, p.cobertura, p.vid, "", "");
                if (resultadoLog == "OK")
                {
                    bandera = true;
                }
                else
                {
                    tabla.Rows.Add("-1", "Error en el Ingreso del Log");
                    bandera = false;
                }
            }

            string jsonData;
            if (bandera)
            {
                tabla.Rows.Add(1, "Guardado Exitosamente");
            }
            jsonData = (string)Utilidades.DataTableToJSON(tabla);

            return jsonData;

        }


    }
}
