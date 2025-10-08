using Nancy.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using static System.Net.Mime.MediaTypeNames;
//using System.Web.Script.Serialization;

namespace ApiNetsuite.Clases
{
    public class funciones
    {

        //public static object Desencriptar(string data, string campo)
        //{

        //    //var serializer = new JavaScriptSerializer();
        //    //Dictionary<string, object> result = serializer.DeserializeObject(data) as Dictionary<string, object>;
        //    //return result[campo];


        //}
        //private string descomponer(string etiqueta, string trama)
        //{
        //    string resultado = "";
        //    if (trama.IndexOf(etiqueta) > 0)
        //    {
        //        resultado = trama.Substring(trama.IndexOf(etiqueta));
        //        resultado = resultado.Substring(resultado.IndexOf(":") + 1, i_if(resultado.IndexOf(",") < 0, resultado.Length - 1, resultado.IndexOf(",")) - (resultado.IndexOf(":") + 1));
        //        resultado = resultado.Replace("\"", "").Replace("{", "").Replace("}", "");
        //        resultado = resultado.Trim();
        //    }
        //    return resultado;
        //}

        //public static object Desencriptar(string data, string campo)
        //{
        //    var serializer = new JavaScriptSerializer();
        //    Dictionary<string, object> result = serializer.DeserializeObject(data) as Dictionary<string, object>;
        //    return result[campo];
        //}

        internal static string descomponer(string etiqueta, string trama)
        {
            string resultado = "0";
            if (trama.IndexOf(etiqueta) > 0)
            {
                resultado = trama.Substring(trama.IndexOf(etiqueta));
                resultado = resultado.Substring(resultado.IndexOf(":") + 1, i_if(resultado.IndexOf(",") < 0, resultado.Length - 1, resultado.IndexOf(",")) - (resultado.IndexOf(":") + 1));
                resultado = resultado.Replace("\"", "").Replace("{", "").Replace("}", "").Replace("\\", "");
                resultado = resultado.Trim();
            }
            return resultado;
        }

     

        public static string DevuelveMensaje(string data)
        {
            var serializer = new JavaScriptSerializer();
            Dictionary<string, object> result = serializer.DeserializeObject(data) as Dictionary<string, object>;
            string mensaje = data;
            mensaje = mensaje.Replace("\"", "").Replace("{", "").Replace(":", "").Replace("}", "").Replace("\\", "").Replace("[", "").Replace("]", "");
            return mensaje;
        }





    internal static int i_if(Boolean expresion, int c1, int c2)
        {
            int resultado = -1;
            if (expresion)
            {
                resultado = c1;
            }
            else
            {
                resultado = c2;
            }
            return resultado;
        }

        internal static object Desencriptar(string v, string assetAMI)
        {
            throw new NotImplementedException();
        }
    }
}
