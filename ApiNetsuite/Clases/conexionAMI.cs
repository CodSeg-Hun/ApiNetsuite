namespace ApiNetsuite.Clases
{
    public class conexionAMI
    {
        public string GetRuta(string opcion)
        {
            string ruta = "";
            // ******************************************************************************************
            // desarrollo
            // '******************************************************************************************

            //if (opcion == "1")
            //    ruta = "https://test-telematicsapi.hunterlabs.io/api-token-auth/ ";
            //else if (opcion == "2")
            //    ruta = "https://test-telematicsapi.hunterlabs.io/asset/";
            //else if (opcion == "3")
            //    ruta = "https://test-telematicsapi.hunterlabs.io/customer/user/";
            //else if (opcion == "4")
            //    ruta = "https://test-telematicsapi.hunterlabs.io/device/";
            //else if (opcion == "5")
            //    ruta = "https://test-telematicsapi.hunterlabs.io/asset-command/";
            //else if (opcion == "6")
            //    ruta = "https://test-telematicsapi.hunterlabs.io/user-asset-command/";
            //else if (opcion == "7")
            //    ruta = "https://test-telematicsapi.hunterlabs.io/protector-entity/";
            //else if (opcion == "8")
            //    ruta = "https://test-telematicsapi.hunterlabs.io/protector-asset-laststate/";
            //else if (opcion == "12")
            //    ruta = "https://test-telematicsapi.hunterlabs.io/asset/ ";
            //else if (opcion == "13")
            //    ruta = "https://test-telematicsapi.hunterlabs.io/customer/user/ ";
            //else if (opcion == "14")
            //    ruta = "https://test-telematicsapi.hunterlabs.io/device/ ";
            //else if (opcion == "15")
            //    ruta = "https://test-telematicsapi.hunterlabs.io/asset-command/ ";
            //else if (opcion == "16")
            //    ruta = "https://test-telematicsapi.hunterlabs.io/user-asset-command/ ";
            //else if (opcion == "17")
            //    ruta = "https://test-telematicsapi.hunterlabs.io/protector-entity/ ";
            //else if (opcion == "18")
            //    ruta = "https://test-telematicsapi.hunterlabs.io/command/run ";
            //else if (opcion == "31")
            //    ruta = "https://test-driverscore.hunterlabs.io/api-token-auth/ ";
            //else if (opcion == "32")
            //    ruta = "https://test-driverscore.hunterlabs.io/asset/";
            //else if (opcion == "33")
            //    ruta = "https://test-driverscore.hunterlabs.io/asset/ ";
            //else if (opcion == "34")
            //    ruta = "https://test-driverscore.hunterlabs.io/score/group/customer/";
            //else if (opcion == "35")
            //    ruta = "https://test-driverscore.hunterlabs.io/score/trip/customer/get-score/ ";
            //else if (opcion == "36")
            //    ruta = "https://test-telematicsapi.hunterlabs.io/trip/";
            //else if (opcion == "41")
            //    ruta = "https://test-motorfyapi.hunterlabs.io/api-token-auth/ ";
            //else if (opcion == "42")
            //    ruta = "https://test-motorfyapi.hunterlabs.io/asset/";
            //else if (opcion == "43")
            //    ruta = "https://test-motorfyapi.hunterlabs.io/customer/user/";
            //else if (opcion == "44")
            //    ruta = "https://test-motorfyapi.hunterlabs.io/device/";
            //else if (opcion == "45")
            //    ruta = "https://test-motorfyapi.hunterlabs.io/asset-command/";
            //else if (opcion == "46")
            //    ruta = "https://test-motorfyapi.hunterlabs.io/user-asset-command/";
            //else if (opcion == "47")
            //    ruta = "https://test-motorfyapi.hunterlabs.io/protector-entity/";
            //else if (opcion == "48")
            //    ruta = "https://test-motorfyapi.hunterlabs.io/protector-asset-laststate/";
            //else if (opcion == "49")
            //    ruta = "https://test-motorfyapi.hunterlabs.io/command/run ";
            //else if (opcion == "52")
            //    ruta = "https://test-motorfyapi.hunterlabs.io/asset/ ";
            //else if (opcion == "53")
            //    ruta = "https://test-motorfyapi.hunterlabs.io/customer/user/ ";
            //else if (opcion == "54")
            //    ruta = "https://test-motorfyapi.hunterlabs.io/device/ ";
            //else if (opcion == "55")
            //    ruta = "https://test-motorfyapi.hunterlabs.io/asset-command/ ";
            //else if (opcion == "56")
            //    ruta = "https://test-motorfyapi.hunterlabs.io/user-asset-command/ ";
            //else if (opcion == "57")
            //    ruta = "https://test-motorfyapi.hunterlabs.io/protector-entity/ ";
            //else if (opcion == "60")
            //    ruta = "https://facetrackerapi.hunterlabs.io/api/token-auth ";
            //else if (opcion == "61")
            //    ruta = "https://facetrackerapi.hunterlabs.io/api/companies/list";
            //else if (opcion == "62")
            //    ruta = "https://facetrackerapi.hunterlabs.io/api/companies/new";
            //else if (opcion == "63")
            //    ruta = "https://facetrackerapi.hunterlabs.io/api/companies/update/";
            //else if (opcion == "64")
            //    ruta = "https://facetrackerapi.hunterlabs.io/api/devices/list";
            //else if (opcion == "65")
            //    ruta = "https://facetrackerapi.hunterlabs.io/api/devices/update/";
            //else if (opcion == "66")
            //    ruta = "https://facetrackerapi.hunterlabs.io/api/devices/new";
            //else if (opcion == "67")
            //    ruta = "https://facetrackerapi.hunterlabs.io/api/devices/delete/";
            //else if (opcion == "68")
            //    ruta = "https://facetrackerapi.hunterlabs.io/api/companies/get/";
            //else if (opcion == "69")
            //    ruta = "https://facetrackerapi.hunterlabs.io/api/devices/get/";
            // ******************************************************************************************
            // produccion
            // ******************************************************************************************
            if (opcion == "1")
                ruta = "https://telematicsapi.hunterlabs.io/api-token-auth/ ";
            else if (opcion == "2")
                ruta = "https://telematicsapi.hunterlabs.io/asset/";
            else if (opcion == "3")
                ruta = "https://telematicsapi.hunterlabs.io/customer/user/";
            else if (opcion == "4")
                ruta = "https://telematicsapi.hunterlabs.io/device/";
            else if (opcion == "5")
                ruta = "https://telematicsapi.hunterlabs.io/asset-command/";
            else if (opcion == "6")
                ruta = "https://telematicsapi.hunterlabs.io/user-asset-command/";
            else if (opcion == "7")
                ruta = "https://telematicsapi.hunterlabs.io/protector-entity/";
            else if (opcion == "8")
                ruta = "https://telematicsapi.hunterlabs.io/protector-asset-laststate/";
            else if (opcion == "12")
                ruta = "https://telematicsapi.hunterlabs.io/asset/ ";
            else if (opcion == "13")
                ruta = "https://telematicsapi.hunterlabs.io/customer/user/ ";
            else if (opcion == "14")
                ruta = "https://telematicsapi.hunterlabs.io/device/ ";
            else if (opcion == "15")
                ruta = "https://telematicsapi.hunterlabs.io/asset-command/ ";
            else if (opcion == "16")
                ruta = "https://telematicsapi.hunterlabs.io/user-asset-command/ ";
            else if (opcion == "17")
                ruta = "https://telematicsapi.hunterlabs.io/protector-entity/ ";
            else if (opcion == "18")
                ruta = "https://telematicsapi.hunterlabs.io/command/run/ ";
            else if (opcion == "31")
                ruta = "https://test-driverscore.hunterlabs.io/api-token-auth/ ";
            else if (opcion == "32")
                ruta = "https://test-driverscore.hunterlabs.io/asset/";
            else if (opcion == "33")
                ruta = "https://test-driverscore.hunterlabs.io/asset/ ";
            else if (opcion == "34")
                ruta = "https://test-driverscore.hunterlabs.io/score/group/customer/";
            else if (opcion == "35")
                ruta = "https://test-driverscore.hunterlabs.io/score/trip/customer/get-score/ ";
            else if (opcion == "36")
                ruta = "https://telematicsapi.hunterlabs.io/trip/";
            else if (opcion == "41")
                ruta = "https://motorfyapi.hunterlabs.io/api-token-auth/ ";
            else if (opcion == "42")
                ruta = "https://motorfyapi.hunterlabs.io/asset/";
            else if (opcion == "43")
                ruta = "https://motorfyapi.hunterlabs.io/customer/user/";
            else if (opcion == "44")
                ruta = "https://motorfyapi.hunterlabs.io/device/";
            else if (opcion == "45")
                ruta = "https://motorfyapi.hunterlabs.io/asset-command/";
            else if (opcion == "46")
                ruta = "https://motorfyapi.hunterlabs.io/user-asset-command/";
            else if (opcion == "47")
                ruta = "https://motorfyapi.hunterlabs.io/protector-entity/";
            else if (opcion == "48")
                ruta = "https://motorfyapi.hunterlabs.io/protector-asset-laststate/";
            else if (opcion == "49")
                ruta = "https://telematicsapi.hunterlabs.io/command/run/ ";
            else if (opcion == "52")
                ruta = "https://motorfyapi.hunterlabs.io/asset/ ";
            else if (opcion == "53")
                ruta = "https://motorfyapi.hunterlabs.io/customer/user/ ";
            else if (opcion == "54")
                ruta = "https://motorfyapi.hunterlabs.io/device/ ";
            else if (opcion == "55")
                ruta = "https://motorfyapi.hunterlabs.io/asset-command/ ";
            else if (opcion == "56")
                ruta = "https://motorfyapi.hunterlabs.io/user-asset-command/ ";
            else if (opcion == "57")
                ruta = "https://motorfyapi.hunterlabs.io/protector-entity/ ";
            else if (opcion == "60")
                ruta = "https://api.wesafebyhunter.com/api/token-auth ";
            else if (opcion == "61")
                ruta = "https://api.wesafebyhunter.com/api/companies/list";
            else if (opcion == "62")
                ruta = "https://api.wesafebyhunter.com/api/companies/new";
            else if (opcion == "63")
                ruta = "https://api.wesafebyhunter.com/api/companies/update/";
            else if (opcion == "64")
                ruta = "https://api.wesafebyhunter.com/api/devices/list";
            else if (opcion == "65")
                ruta = "https://api.wesafebyhunter.com/api/devices/update/";
            else if (opcion == "66")
                ruta = "https://api.wesafebyhunter.com/api/devices/new";
            else if (opcion == "67")
                ruta = "https://api.wesafebyhunter.com/api/devices/delete/";
            else if (opcion == "68")
                ruta = "https://api.wesafebyhunter.com/api/companies/get/";
            else if (opcion == "69")
                ruta = "https://api.wesafebyhunter.com/api/devices/get/";
            else if (opcion == "70")
                ruta = "https://api.wesafebyhunter.com/api/token-auth/";
            else if (opcion == "71")
                ruta = "https://api.wesafebyhunter.com/api/business/companies/get/";
            else if (opcion == "72")
                ruta = "https://api.wesafebyhunter.com/api/business/companies/";
            else if (opcion == "73")
                ruta = "https://api.wesafebyhunter.com/api/business/license-plan-company-links";
            return ruta;
        }
    
    }
}
