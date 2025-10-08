using ApiNetsuite.DTO;
using ApiNetsuite.Modelo;
using Microsoft.VisualBasic;
using Nancy.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace ApiNetsuite
{
    public static class Utilidades
    {

        public static bool EsJsonObjeto(string texto)
        {
            try
            {
                var token = JToken.Parse(texto);
                return token.Type == JTokenType.Object; // true si es { ... }
            }
            catch
            {
                return false; // No es JSON válido
            }
        }


        public static string Catalogo(string texto, string valorDevolver = "C")
        {
            string resultado = "";
            string[] cadenas = texto.ToString().Split(';');
            switch (valorDevolver)
            {
                case "C":
                    {
                        resultado = cadenas[0];
                        break;
                    }
                case "D":
                    {
                        resultado = cadenas[1];
                        break;
                    }
                default:
                    {
                        //        throw new Exception("No existe programada esa función ");
                        break;
                    }
            }
            return resultado;
        }


        public static UsuarioDTO convertirDTO(this UsuarioAPI u)
        {
            if (u != null)
            {
                return new UsuarioDTO
                {
                    Token = u.Token,
                    Usuario = u.Usuario
                };
            }

            return null;
        }


        public static string SerializarAssetActualizar(assetEntidad Asset, attributeEntidadCollection attribute, string valor, string owner)
        {
            try
            {
                string data = "{" + Constants.vbLf + "  \"id\":" + "\"" + valor + "\"" + "," + Constants.vbLf + "  \"name\": " + "\"" + Asset.name + "\"" + "," + Constants.vbLf + "  \"active\": true," + Constants.vbLf;
                string data1 = "   \"attributes\": [" + Constants.vbLf;
                string data2 = "";
                Int32 c = 0;
                foreach (attributeEntidad attribute2 in attribute)
                    {
                        c += 1;
                       if (c == 1)
                           data2 = "    {" + Constants.vbLf + "    " + Constants.vbTab + "  \"attribute\": " + "\"" + attribute2.AttributeId + "\"" + "," + Constants.vbLf + "    " + Constants.vbTab + "  \"value\": " + "\"" + attribute2.Valor + "\"" + Constants.vbLf + "    }";
                       if (c > 1)
                            data2 = data2 + ",    {" + Constants.vbLf + "    " + Constants.vbTab + "  \"attribute\": " + "\"" + attribute2.AttributeId + "\"" + "," + Constants.vbLf + "    " + Constants.vbTab + "  \"value\": " + "\"" + attribute2.Valor + "\"" + Constants.vbLf + "    }";
                    }
                string data3 = "  ],";
                string data4 = "  \"asset_type\": " + "\"" + Asset.assettype + "\"" + "," + Constants.vbLf + "  \"custom_name\": " + "\"" + Asset.customname + "\"" + "," + Constants.vbLf + "  \"owner\": " + "\"" + owner + "\"" + "," + Constants.vbLf;
                string data5 = "  \"description\": " + "\"" + Asset.description + "\"" + Constants.vbLf + "}";
                // Dim data5 As String = "  ""description"": " & """" & entidad.description & """" & vbLf & "," & "  ""product"":" & """" & entidad.product & """" & vbLf & "}"
                string resultado = data + data1 + data2 + data3 + data4 + data5;
                return resultado;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        public static string SerializarContrato(string cedula, string documento)
        {
            try
            {
                string data = "";
                data = "{" + Constants.vbLf + "  \"NumeroIdentificacion\":" + "\"" + cedula + "\"" + "," + Constants.vbLf + "  \"TipoDocumento\": " + "\"" + documento + "\"" + Constants.vbLf + "   }  ";
                string resultado = data;
                return resultado;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        public static string SerializarDatos(string campo1, string campo2, string campo3, string campo4, string campo5, 
                                             string campo6, string campo7, string campo8, string campo9, string campo10, 
                                             string origen)
        {
            try
            {
                string data = "";
                string data1 = "";
                string data2 = "";
                string data3 = "";
                string resultado = "";
                if (origen.Equals("login"))
                {
                    data = "{" + Constants.vbLf + "  \"usuario\":" + "\"" + campo1 + "\"" + "," + Constants.vbLf;
                    data1 = "  \"contrasenia\": " + "\"" + campo2 + "\"" + Constants.vbLf + "}";
                    resultado = data + data1;
                }
                if (origen.Equals("seguridad"))
                {
                    data = "{" + Constants.vbLf + "  \"ipAddress\":" + "\"" + campo1 + "\"" + "," + Constants.vbLf;
                    data1 = "  \"nombreServicio\": " + "\"" + campo2 + "\"" + Constants.vbLf + "}";
                    resultado = data + data1;
                }
                if (origen.Equals("contabilidad"))
                {
                    data = "{" + Constants.vbLf + "  \"vinVehiculo\":" + "\"" + campo1 + "\"" + "," + Constants.vbLf + "  \"numeroDocumento\": " + "\"" + campo2 + "\"" + "," + Constants.vbLf + "  \"tipoDocumento\": " + "\"" + campo3 + "\"" + "," + Constants.vbLf;
                    data1 = "  \"trabajoRealizadoProveedor\": " + "" + campo4 + "" + "," + Constants.vbLf + "  \"fechaTrabajoRealizadoProveedor\": " + "\"" + campo5 + "\"" + "," + Constants.vbLf + "  \"ordenTrabajoProveedor\": " + "\"" + campo6 + "\"" + "," + Constants.vbLf;
                    data2 = "  \"lugarTrabajoProveedor\": " + "\"" + campo7 + "\"" + "," + Constants.vbLf + "  \"observacionProveedor\": " + "\"" + campo8 + "\"" + "," + Constants.vbLf + "  \"aniosAdicionales\": " + "\"" + campo9 + "\"" + "," + Constants.vbLf;
                    data3 = "  \"facturaAniosAdicionales\": " + "\"" + campo10 + "\"" + Constants.vbLf + "}";
                    resultado = data + data1 + data2 + data3;
                }
                if (origen.Equals("actualizar"))
                {
                    data = "{" + Constants.vbLf + "  \"vinVehiculo\":" + "\"" + campo1 + "\"" + "," + Constants.vbLf;
                    data1 = "  \"estadoDispositivo\":" + "" + campo2 + "" + Constants.vbLf + "}";
                    resultado = data + data1;
                }
                return resultado;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        public static string SerializarInstalacion(string sponsor, string tipoplan, string cedula, string celular, string mail, string fechainicio, string fechafin, beneficiarioEntidadCollection beneficiario)
        {
            try
            {
                string data = "";
                string data1 = "";
                string data2 = "";
                string data3 = "";
                string data4 = "";
                data = "{" + Constants.vbLf + "  \"Sponsor\":" + "\"" + sponsor + "\"" + "," + Constants.vbLf + "  \"TipoPlan\": " + "\"" + tipoplan + "\"" + "," + Constants.vbLf;
                data1 = "  \"NumeroIdentificacion\": " + "\"" + cedula + "\"" + "," + Constants.vbLf + "  \"Mail\": " + "\"" + mail + "\"" + "," + Constants.vbLf;
                data2 = "  \"Celular\": " + "\"" + celular + "\"" + "," + Constants.vbLf + "  \"FechaInicio\": " + "\"" + fechainicio + "\"" + "," + Constants.vbLf + "  \"FechaFin\": " + "\"" + fechafin + "\"" + "," + Constants.vbLf;
                Int32 c = 0;
                if (beneficiario.Count == 0)
                    data3 = "  \"ListaBeneficiarioGenerico\": [";
                foreach (beneficiarioEntidad beneficiario2 in beneficiario)
                {
                    c += 1;
                    if (c == 1)
                        data3 = "  \"ListaBeneficiarioGenerico\": [" + Constants.vbLf + "{" + Constants.vbLf + "  \"NumeroIdentificacion\":" + "\"" + beneficiario2.Identificacion + "\"" + "," + Constants.vbLf + "  \"Mail\": " + "\"" + beneficiario2.Mail + "\"" + "," + Constants.vbLf + "  \"Celular\":" + "\"" + beneficiario2.Celular + "\"" + "," + Constants.vbLf + "  \"Relacion\": " + "\"" + beneficiario2.Relacion + "\"" + Constants.vbLf + "   }  ";
                    if (c > 1)
                        data3 = data3 + ", {" + Constants.vbLf + "  \"NumeroIdentificacion\":" + "\"" + beneficiario2.Identificacion + "\"" + "," + Constants.vbLf + "  \"Mail\": " + "\"" + beneficiario2.Mail + "\"" + "," + Constants.vbLf + "  \"Celular\":" + "\"" + beneficiario2.Celular + "\"" + "," + Constants.vbLf + "  \"Relacion\": " + "\"" + beneficiario2.Relacion + "\"" + Constants.vbLf + "   }  ";
                }
                data4 = " ] }";
                string resultado = data + data1 + data2 + data3 + data4;
                return resultado;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        public static string SerializarAnulacion(string contrato, string region, string producto, string sponsor, string tipoplan)
        {
            try
            {
                string data = "";
                string data1 = "";
                string data2 = "";
                string data3 = "";
                string data4 = "";
                data = "{" + Constants.vbLf + "  \"ContratoNumero\":" + "\"" + contrato + "\"" + "," + Constants.vbLf;
                data1 = "  \"Region\": " + "\"" + region + "\"" + "," + Constants.vbLf;
                data2 = "  \"CodigoProducto\": " + "\"" + producto + "\"" + "," + Constants.vbLf;
                data3 = "  \"Sponsor\": " + "\"" + sponsor + "\"" + "," + Constants.vbLf;
                data4 = "  \"TipoPlan\":" + "\"" + tipoplan + "\"" + Constants.vbLf + "   }  ";
                string resultado = data + data1 + data2 + data3 + data4;
                return resultado;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        public static string SerializarRenovacion(string contrato, string region, string producto, string fechainicio, string fechafin, string sponsor)
        {
            try
            {
                string data = "";
                string data1 = "";
                string data2 = "";
                string data3 = "";
                string data4 = "";
                string data5 = "";
                data = "{" + Constants.vbLf + "  \"ContratoNumero\":" + "\"" + contrato + "\"" + "," + Constants.vbLf;
                data1 = "  \"Region\": " + "\"" + region + "\"" + "," + Constants.vbLf;
                data2 = "  \"CodigoProducto\": " + "\"" + producto + "\"" + "," + Constants.vbLf;
                data3 = "  \"FechaInicio\": " + "\"" + fechainicio + "\"" + "," + Constants.vbLf;
                data4 = "  \"fechafin\": " + "\"" + fechafin + "\"" + "," + Constants.vbLf;
                data5 = "  \"Sponsor\":" + "\"" + sponsor + "\"" + Constants.vbLf + "   }  ";
                string resultado = data + data1 + data2 + data3 + data4 + data5;
                return resultado;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        public static string SerializarBeneficiario(string cedula, string mail, string celular, string relacion, string sponsor, string tipoplan, string contrato, string region, string producto)
        {
            try
            {
                string data = "";
                string data1 = "";
                string data2 = "";
                string data3 = "";
                string data4 = "";
                data = "{" + Constants.vbLf + "  \"NumeroIdentificacion\":" + "\"" + cedula + "\"" + "," + Constants.vbLf + "  \"Mail\": " + "\"" + mail + "\"" + "," + Constants.vbLf;
                data1 = "  \"Celular\": " + "\"" + celular + "\"" + "," + Constants.vbLf + "  \"Relacion\": " + "\"" + relacion + "\"" + "," + Constants.vbLf;
                data2 = "  \"Sponsor\": " + "\"" + sponsor + "\"" + "," + Constants.vbLf;
                data3 = "  \"TipoPlan\": " + "\"" + tipoplan + "\"" + "," + Constants.vbLf + "  \"ContratoNumero\": " + "\"" + contrato + "\"" + "," + Constants.vbLf;
                data4 = "  \"Region\":" + "\"" + region + "\"" + "," + Constants.vbLf + "  \"Producto\": " + "\"" + producto + "\"" + Constants.vbLf + "   }  ";
                string resultado = data + data1 + data2 + data3 + data4;
                return resultado;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        public static string SerializarCustomer(customerEntidad monitoreo,  string valor, string accountType)
        {
            try
            {
                //customer cliente,
                string data = "";
                if (valor == "0")
                    data = "{" + Constants.vbLf + "  \"username\":" + "\"" + monitoreo.username + "\"" + "," + Constants.vbLf + "  \"first_name\": " + "\"" + monitoreo.first_name + "\"" + "," + Constants.vbLf + "  \"is_active\": " + "\"" + monitoreo.active + "\"" + "," + Constants.vbLf;
                else
                    data = "{" + Constants.vbLf + "  \"id\":" + "\"" + valor + "\"" + "," + Constants.vbLf + "  \"username\":" + "\"" + monitoreo.username + "\"" + "," + Constants.vbLf + "  \"first_name\": " + "\"" + monitoreo.first_name + "\"" + "," + Constants.vbLf + "  \"is_active\": " + "\"" + monitoreo.active + "\"" + "," + Constants.vbLf;
                string data1 = "  \"last_name\": " + "\"" + monitoreo.last_name + "\"" + "," + Constants.vbLf + "  \"customer\": {" + Constants.vbLf + "  \"company_code\": " + "\"" + monitoreo.company_code + "\"" + "," + Constants.vbLf;
                // *Dim data2 As String = "  ""identity_document_type"": " & """" & cliente.identity_document_type & """" & "," & vbLf & "  ""identity_document_number"": " & """" & cliente.identity_document_number & """" & ","
                string data2 = "  \"identity_document_type\": " + "\"" + monitoreo.identity_customer_type + "\"" + "," + Constants.vbLf + "  \"identity_document_number\": " + "\"" + monitoreo.internal_identifier + "\"" + ",";
                string data3 = "  \"business_name\": " + "\"" + monitoreo.business_name + "\"" + "," + Constants.vbLf + "  \"phone_number\": " + "\"" + monitoreo.telefono + "\"" + "," + Constants.vbLf + "  \"emergency_phone_number\": " + "\"" + monitoreo.emergency_phone_number + "\"" + ",";
                string data4 = "  \"assistance_phone_number\": " + "\"" + monitoreo.assistance_phone_number + "\"" + "," + Constants.vbLf + "  \"technical_support_email\": " + "\"" + monitoreo.technical_support_email + "\"" + "} ," + Constants.vbLf;
                string data5 = "  \"email\": " + "\"" + monitoreo.email + "\"" + "," + Constants.vbLf + "  \"account_type\": " + "\"" + accountType + "\"" + "}";
                string resultado = data + data1 + data2 + data3 + data4 + data5;
                return resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static string SerializarAssetCambio(assetEntidad entidad, attributeEntidadCollection attribute, string valor, string owner)
        {
            try
            {
                string data = "";
                if (valor == "0")
                    // data = "{" & vbLf & "  ""product"":" & """" & entidad.product & """" & "," & vbLf & "  ""name"": " & """" & entidad.name & """" & "," & vbLf & "  ""active"": true," & vbLf
                    data = "{" + Constants.vbLf + "   \"name\": " + "\"" + entidad.name + "\"" + "," + Constants.vbLf + "  \"active\": true," + Constants.vbLf;
                else
                    // data = "{" & vbLf & "  ""id"":" & """" & valor & """" & "," & vbLf & "  ""product"":" & """" & entidad.product & """" & "," & vbLf & "  ""name"": " & """" & entidad.name & """" & "," & vbLf & "  ""active"": true," & vbLf
                    data = "{" + Constants.vbLf + "  \"id\":" + "\"" + valor + "\"" + "," + Constants.vbLf + "  \"name\": " + "\"" + entidad.name + "\"" + "," + Constants.vbLf + "  \"active\": true," + Constants.vbLf;
                // Dim data1 As String = "  ""product_expire_date"": " & """" & entidad.product_expire_date & """" & "," & vbLf & "  ""attributes"": [" & vbLf
                string data1 = "   \"attributes\": [" + Constants.vbLf;
                string data2 = "";
                Int32 c = 0;
                foreach (attributeEntidad attribute2 in attribute)
                {
                    c += 1;
                    if (c == 1)
                        data2 = "    {" + Constants.vbLf + "    " + Constants.vbTab + "  \"attribute\": " + "\"" + attribute2.AttributeId + "\"" + "," + Constants.vbLf + "    " + Constants.vbTab + "  \"value\": " + "\"" + attribute2.Valor + "\"" + Constants.vbLf + "    }";
                    if (c > 1)
                        data2 = data2 + ",    {" + Constants.vbLf + "    " + Constants.vbTab + "  \"attribute\": " + "\"" + attribute2.AttributeId + "\"" + "," + Constants.vbLf + "    " + Constants.vbTab + "  \"value\": " + "\"" + attribute2.Valor + "\"" + Constants.vbLf + "    }";
                }
                string data3 = "  ],";
                // Dim data4 As String = "  ""asset_type"": " & """" & entidad.asset_type & """" & "," & vbLf & "  ""custom_name"": " & """" & entidad.custom_name & """" & ","
                // Dim data4 As String = "  ""asset_type"": " & """" & entidad.asset_type & """" & "," & vbLf & "  ""custom_name"": " & """" & entidad.custom_name & """" & "," & vbLf & "  ""owner"": " & """" & owner & """" & ","
                string data4 = "  \"asset_type\": " + "\"" + entidad.assettype + "\"" + "," + Constants.vbLf + "  \"custom_name\": " + "\"" + entidad.customname + "\"" + "," + Constants.vbLf + "  \"owner\": " + "\"" + owner + "\"" + Constants.vbLf + "}";
                // Dim data5 As String = "  ""description"": " & """" & entidad.description & """" & vbLf & "}"
                string resultado = data + data1 + data2 + data3 + data4;
                return resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static string SerializarAssetInstalacion(assetEntidad entidad, attributeEntidadCollection attribute, string valor, string owner, string sensors)
        {
            try
            {
                string data = "";
                if (valor == "0")
                    data = "{" + Constants.vbLf + "  \"product\":" + "\"" + entidad.product + "\"" + "," + Constants.vbLf + "  \"name\": " + "\"" + entidad.name + "\"" + "," + Constants.vbLf + "  \"active\": true," + Constants.vbLf;
                else
                    data = "{" + Constants.vbLf + "  \"id\":" + "\"" + valor + "\"" + "," + Constants.vbLf + "  \"product\":" + "\"" + entidad.product + "\"" + "," + Constants.vbLf + "  \"name\": " + "\"" + entidad.name + "\"" + "," + Constants.vbLf + "  \"active\": true," + Constants.vbLf;
                string data1 = "  \"product_expire_date\": " + "\"" + entidad.productexpiredate + "\"" + "," + Constants.vbLf + "  \"attributes\": [" + Constants.vbLf;
                string data2 = "";
                Int32 c = 0;
                foreach (attributeEntidad attribute2 in attribute)
                {
                    c += 1;
                    if (c == 1)
                        data2 = "    {" + Constants.vbLf + "    " + Constants.vbTab + "  \"attribute\": " + "\"" + attribute2.AttributeId + "\"" + "," + Constants.vbLf + "    " + Constants.vbTab + "  \"value\": " + "\"" + attribute2.Valor + "\"" + Constants.vbLf + "    }";
                    if (c > 1)
                        data2 = data2 + ",    {" + Constants.vbLf + "    " + Constants.vbTab + "  \"attribute\": " + "\"" + attribute2.AttributeId + "\"" + "," + Constants.vbLf + "    " + Constants.vbTab + "  \"value\": " + "\"" + attribute2.Valor + "\"" + Constants.vbLf + "    }";
                }
                string data3 = "  ],";
                // Dim data4 As String = "  ""asset_type"": " & """" & entidad.asset_type & """" & "," & vbLf & "  ""custom_name"": " & """" & entidad.custom_name & """" & ","
                string data4 = "  \"asset_type\": " + "\"" + entidad.assettype + "\"" + "," + Constants.vbLf + "  \"custom_name\": " + "\"" + entidad.customname + "\"" + "," + Constants.vbLf + "  \"owner\": " + "\"" + owner + "\"" + ",";
                string data5 = "  \"doors_sensors\": " + "\"" + sensors + "\"" + "," + Constants.vbLf + "  \"state\": 1," + Constants.vbLf + "  \"description\": " + "\"" + entidad.description + "\"" + Constants.vbLf + "}";
                string resultado = data + data1 + data2 + data3 + data4 + data5;
                return resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static string SerializarAuthenticate( string username, string password)
        {
            try
            {
                string data = "";
                data = "{" + Constants.vbLf + "  \"username\":" + "\"" + username + "\"" + "," + Constants.vbLf + "  \"password\": " + "\"" + password + "\"" + "}";
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        // Clave de encriptación fija
        private const string EncryptionKey = "MAKV2SPBNI99212";
        // Método público para encriptar una lista de strings
        public static List<string> EncryptStrings(List<string> strings)
        {
            List<string> encryptedStrings = new List<string>();
            foreach (string str in strings)
            {
                encryptedStrings.Add(Encrypt(str));
            }
            return encryptedStrings;
        }


        // Método privado para encriptar un único string
        public static string Encrypt(string clearText)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6E, 0x20, 0x4D, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public static object DataTableToJSON(DataTable table)
        {
            var list = new List<Dictionary<string, object>>();
            foreach (DataRow row in table.Rows)
            {
                var dict = new Dictionary<string, object>();

                foreach (DataColumn col in table.Columns)
                {
                    dict[col.ColumnName] = (Convert.ToString(row[col]));
                }
                list.Add(dict);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(list);
        }


    }
}
