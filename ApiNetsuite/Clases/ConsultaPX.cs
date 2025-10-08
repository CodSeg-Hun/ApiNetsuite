using ApiNetsuite.DTO.Bien;
using ApiNetsuite.DTO.Cliente;
using ApiNetsuite.DTO.Estado;
using ApiNetsuite.Repositorio;
using System;
using System.Data;


namespace ApiNetsuite.Clases
{
    public class ConsultaPX
    {
        public static string CambioConAseFin(BienDTO p)
        {
            string resultado ;
            string mensaje = "OK";
            try
            {
                DataSet cnstGenrl = new DataSet();
                DataSet infoDatos = new DataSet();
                infoDatos = BienSQL.CambioConAseFin(opcion: "106", vehiculo: p.codvehiculo.ToUpper(), placa: p.placa.ToUpper(), idmarca: p.idmarca, marca: p.marca.ToUpper(),
                                                    idmodelo: p.idmodelo.ToUpper(), modelo: p.modelo.ToUpper(), chasis: p.chasis.ToUpper(), motor: p.motor.ToUpper(), color: p.color.ToUpper(), anio: p.anio,
                                                    tipo: p.tipo.ToUpper(), idconcesionario: p.idconcesionario, concesionariodesc: p.concesionariodesc, concesionariodir: p.concesionariodire,
                                                    idfinanciera: p.idfinanciera, financieradesc: p.financieradesc, financieradir: p.financieradire, idaseguradora: p.idaseguradora,
                                                    aseguradoradesc: p.aseguradoradesc, aseguradoradir: p.aseguradoradire, "CambioConAseFin", p.numeroorden, p.idusuario, p.ejecutiva, p.estadocartera, p.sucursal);
                if (infoDatos.Tables[0].Rows.Count > 0)
                {
                    var respuesta = (string)infoDatos.Tables[0].Rows[0]["CODIGO"];
                    if (respuesta == "1")
                    {
                        mensaje = "OK";
                    }
                    else
                    {
                        mensaje = (string)infoDatos.Tables[0].Rows[0]["MENSAJE"];
                    }
                }
                resultado = mensaje;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            return resultado;
        }

        public static string CambioPropietario(PropietarioDTO p)
        {
            string resultado;
            string mensaje = "OK";
            try
            {
                DataSet cnstGenrl = new DataSet();
                DataSet infoDatos = new DataSet();
                infoDatos = BienSQL.CambioPropietario(opcion: "105", vehiculo: p.codvehiculo.ToUpper(), placa: p.placa.ToUpper(), idmarca: p.idmarca, marca: p.marca.ToUpper(),
                                                    idmodelo: p.idmodelo.ToUpper(), modelo: p.modelo.ToUpper(), chasis: p.chasis.ToUpper(), motor: p.motor.ToUpper(), color: p.color.ToUpper(), anio: p.anio,
                                                    tipo: p.tipo.ToUpper(), numero_documento: p.numero_documento, primer_nombre: p.primer_nombre, segundo_nombre: p.segundo_nombre, apellido_paterno:p.apellido_paterno, apellido_materno: p.apellido_materno,
                                                    direccion:p.direccion, telefono_convencional:p.telefono_convencional, telefono_celular:p.telefono_celular, email: p.email,  "CambioPropietario", numeroorden: p.numeroorden, idusuario: p.idusuario);
                if (infoDatos.Tables[0].Rows.Count > 0)
                {
                    var respuesta = (string)infoDatos.Tables[0].Rows[0]["CODIGO"];
                    if (respuesta == "1")
                    {
                        mensaje = "OK";
                    }
                    else
                    {
                        mensaje = (string)infoDatos.Tables[0].Rows[0]["MENSAJE"];
                    }
                }
                resultado = mensaje;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            return resultado;
        }


        public static string CambioCliente(ClienteDTO p)
        {
            string resultado = "";
            string mensaje = "OK";
            try
            {
                DataSet cnstGenrl = new DataSet();
                string numero_documento = p.id_cliente;
                string primer_nombre = p.primer_nombre;
                string segundo_nombre = p.segundo_nombre;
                string apellido_paterno = p.apellido_paterno;
                string apellido_materno = p.apellido_materno;
                string direccion = p.direccion;
                string telefono_convencional = p.telefono_convencional;
                string telefono_celular = p.telefono_celular;
                string email = p.email;
                string usuario_id = p.id_usuario;
                string numeroorden = p.numeroorden;
                cnstGenrl = ClienteSQL.CnstClienteParametrosPX("107", numero_documento, primer_nombre, segundo_nombre, apellido_paterno, apellido_materno,
                                                                direccion, telefono_convencional, telefono_celular, email, usuario_id, numeroorden, p.ejecutiva, p.sucursal);
                if (cnstGenrl.Tables[0].Rows.Count > 0)
                {
                    string codigo = (string)cnstGenrl.Tables[0].Rows[0]["CODIGO"];
                    if (codigo == "1")
                    {
                        mensaje = "OK";
                    }
                    else
                    {
                        mensaje = (string)cnstGenrl.Tables[0].Rows[0]["DESCRIPCION"];
                    }
                }
                else
                {
                    mensaje = "Consulta no se puede hacer";
                }
                resultado = mensaje;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            return resultado;
        }


        public static string CambioEstado(EstadoDTO p)
        {
            string resultado = "";
            string mensaje = "OK";
            try
            {
                DataSet cnstGenrl = new DataSet();
                DataSet infoDatos = new DataSet();
                cnstGenrl = EstadoSQL.CnstConsultar(opcion: "100");
                if (cnstGenrl.Tables[0].Rows.Count > 0)
                {
                    infoDatos = EstadoSQL.CnstEstado(opcion: "109", vehiculo: p.codVehiculo, placa: p.placaVehiculo.ToUpper(), idmarca: p.idMarcaVehiculo, marca: p.marcaVehiculo.ToUpper(),
                                                        idmodelo: p.idModeloVehiculo, modelo: p.modeloVehiculo.ToUpper(), chasis: p.chasisVehiculo.ToUpper(), motor: p.motorVehiculo.ToUpper(),
                                                        color: p.colorVehiculo.ToUpper(), anio: p.anioVehiculo, tipo: p.tipoVehiculo.ToUpper(), idfamiliaProducto: p.idfamiliaProdructo.ToUpper(),
                                                        familiaProducto: p.familiaProdructo.ToUpper(), idmarcaDispositivo: p.idmarcaDispositivo, marcaDispositivo: p.marcaDispositivo.ToUpper(),
                                                        idmodeloDispositivo: p.idmodeloDispositivo, modeloDispositivo: p.modeloDispositivo.ToUpper(),
                                                        serieDispositivo: p.serieDispositivo.ToUpper(), vid: p.vidDispositivo.ToUpper(), imeiDispositivo: p.imeiDispositivo.ToUpper(),
                                                        direccionMac: p.direccionMacSim.ToUpper(), serieSim: p.serieSim.ToUpper(), numeroCelularSim: p.numeroCelularSim.ToUpper(), operadoraSim: p.operadoraSim.ToUpper(),
                                                        estadoFamiliaProducto: p.estadoFamiliaProducto.ToUpper(), parametroproducto: "PX", nemonicoUsuario: p.nemonicoUsuario.ToUpper(),
                                                        numeroorden:p.numeroorden);
                    if (infoDatos.Tables[0].Rows.Count > 0)
                    {
                        string codigo = (string)infoDatos.Tables[0].Rows[0]["CODIGO"];
                        if (codigo == "1")
                        {
                            mensaje = "OK";
                        }
                        else
                        {
                            mensaje = (string)infoDatos.Tables[0].Rows[0]["DESCRIPCION"];
                        }
                    }
                    else
                    {
                        mensaje = "No se Proceso";
                    }
                }
                else
                {
                    mensaje = "Consulta no se puede hacer";
                }
                resultado = mensaje;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            return resultado;
        }

    }
}
