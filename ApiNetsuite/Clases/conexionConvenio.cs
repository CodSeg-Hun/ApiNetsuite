namespace ApiNetsuite.Clases
{
    public class conexionConvenio
    {
        // ******************************************************************************************
        // conexion de Convenio
        // '******************************************************************************************
        public string GetRutaAmbacar(string opcion)
        {
            string ruta = "https://ambysoftapi.ambacar.ec/";
            //string ruta = "https://ambysoftapitest.ambacar.ec/";
            if (opcion == "1")
                ruta = ruta + "MS_SeguridadesCommand/api/Login/Login";
            else if (opcion == "2")
                ruta = ruta +  "MS_SeguridadesCommand/api/Token/Authenticate";
            else if (opcion == "3")
                ruta = ruta + "MS_VehiculosCommand/api/HunterDispositivo/ActualizarDispositivoHunter";
            else if (opcion == "4")
                ruta = ruta +  "MS_ContabilidadCommand/api/ServiciosHunter/ActualizarInformacionKardexContable";
           
            return ruta;
        }

        public string GetRutaIcesa(string opcion)
        {
            string ruta = "http://200.7.249.21:90/apiHunter/controllers/";
            if (opcion == "1")
                ruta = ruta + "motosHunter";
            else if (opcion == "2")
                ruta = ruta + "motosVenta";
            else if (opcion == "3")
                ruta = ruta + "motosVentaNotas";

            return ruta;
        }

        public string GetRutaHunterMed(string opcion)
        {
            string ruta = "";

            ////desarrollo
            //if (opcion == "1")
            //    ruta = "https://desarrollo.servicios.saludsa.com.ec/ServicioAutorizacion";
            //else if (opcion == "2")
            //    ruta = "https://desarrollo.servicios.saludsa.com.ec/ServicioDrSaludsaOnline";
            //else if (opcion == "3")
            //    ruta = "https://desarrollo.servicios.saludsa.com.ec/ServicioCatalogos";

                ////pruebas
                //if (opcion == "1")
                //    ruta = "https://pruebas.servicios.saludsa.com.ec/ServicioAutorizacion";
                //else if (opcion == "2")
                //    ruta = "http://pruebas.servicios.saludsa.com.ec/ServicioDrSaludsaOnline";
                //else if (opcion == "3")
                //    ruta = "http://pruebas.servicios.saludsa.com.ec/ServicioCatalogos";
            //else if (opcion == "4") //login-token
               // ruta = "https://servialamodesarrollo.southcentralus.cloudapp.azure.com:7007";

            //produccion
            if (opcion == "1")
                ruta = "https://servicios.saludsa.com.ec/ServicioAutorizacion";
            else if (opcion == "2")
                ruta =  "https://servicios.saludsa.com.ec/ServicioDrSaludsaOnline";
            else if (opcion == "3")
                ruta =  "https://servicios.saludsa.com.ec/ServicioCatalogos";
            else if (opcion == "4") //login-token
                ruta = "https://apis.servialamo.com";



            return ruta;
        }
    }
}
