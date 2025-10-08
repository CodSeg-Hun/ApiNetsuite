using ApiNetsuite.Modelo;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;

namespace ApiNetsuite.Clases
{
    public class conexionPX
    {
        public string GetRuta(string opcion)
        {
            string ruta = "";
           
            // ******************************************************************************************
            // produccion
            // ******************************************************************************************
            if (opcion == "1")
                ruta = "https://tristan.24hm.net/API_PX_LATAM/WSPX.asmx"; //produccion
               // ruta = "https://tyr.24hm.net/API_PX/WSPX.asmx";  //desarrollo
            else if (opcion == "2")
                ruta = "http://tempuri.org/AutenticacionUsuarioPx";
            else if (opcion == "3")
                ruta = "http://tempuri.org/InsertaOrdenV2";
            else if (opcion == "4")
                ruta = "http://tempuri.org/ConsultaVehiculo";
            else if (opcion == "5")
                ruta = "http://tempuri.org/ConsultaOrden";
            else if (opcion == "6")
                ruta = "http://tempuri.org/ConsultaPropietario";
            else if (opcion == "7")
                ruta = "http://tempuri.org/ConsultaDispositivo";
            else if (opcion == "8")
                ruta = "http://tempuri.org/ConsultaComandos";
            else if (opcion == "9")
                ruta = "http://tempuri.org/EjecutaComandos";
            else if (opcion == "10")
                ruta = "http://tempuri.org/ContactoEmergencia";
            return ruta;
        }


        public string GetToken(string usuario, string pass, string strtoken)
        {
            string token = "";
            //conecion PX
            conexionPX ruta = new conexionPX();
            string rutaurl = ruta.GetRuta("1"); ;
            string rutaAutenticacion = ruta.GetRuta("2");
            // XML de la solicitud SOAP
            string soapRequest = $@"<?xml version=""1.0"" encoding=""utf-8""?><soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema""  xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/""><soap:Header><SeguridadPx xmlns=""http://tempuri.org/""><StrToken>{strtoken}</StrToken><AutenticacionToken>string</AutenticacionToken><UserName>{usuario}</UserName><Password>{pass}</Password></SeguridadPx></soap:Header><soap:Body><AutenticacionUsuarioPx xmlns=""http://tempuri.org/"" /></soap:Body></soap:Envelope>";
            // Código para enviar la solicitud HTTP
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(rutaurl);
            request.Headers.Add("SOAPAction", rutaAutenticacion);
            request.Headers.Add("Content-Security-Policy", "default-src 'self' https: data: 'unsafe-inline' 'unsafe-eval';");
            request.ContentType = "text/xml; charset=utf-8";
            request.Method = "POST";
            // Escribir el contenido de la solicitud
            using (Stream stream = request.GetRequestStream())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(soapRequest);
                stream.Write(bytes, 0, bytes.Length);
            }
            // Obtener la respuesta
            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    //string responseXml = reader.ReadToEnd();
                    //Console.WriteLine(responseXml);
                    string responseXml = reader.ReadToEnd();
                    // Analizar el XML de la respuesta para extraer AutenticacionUsuarioPxResult
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(responseXml);
                    // Namespace manager para manejar prefijos de namespaces
                    XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmlDoc.NameTable);
                    nsmgr.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");
                    nsmgr.AddNamespace("tempuri", "http://tempuri.org/");
                    // Buscar el nodo AutenticacionUsuarioPxResult
                    XmlNode resultNode = xmlDoc.SelectSingleNode("//tempuri:AutenticacionUsuarioPxResult", nsmgr);
                    if (resultNode != null)
                    {
                        string result = resultNode.InnerText;
                        token= result;                       
                    }
                    else
                    {
                        token = "-2";
                    }
                }
            }
            return token;
        }



    }
}
