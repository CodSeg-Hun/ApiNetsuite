using Newtonsoft.Json.Linq;

namespace ApiNetsuite.Clases
{
    public class conexion
    {


        private string HMACSHA256SignatureType = "HMAC-SHA256";
        private string OAuthVersion = "1.0";
        // Dim oauthConsumerKey = "b61ef7d183239d254c09cfa1051fa362559767dfa52a5b7effa9fb3a78771373"
        // Dim oauthToken = "9c680c18a3da6257c5d61469756e1086c468495349fbb35f5cc9cd9e45581ddc"
        // Dim oauthConsumerSecret = "aed86ff1499f8ab02c760eff7d35723a402e9e20e968311e4b81a37e7fd6041d"
        // Dim oauthTokenSecret = "2bcacc32e946368f417895e452fb7ff4b7337941ac6ea51f4f8e5eaa767ef721"
        private string httpMethodPost = "POST";
        private string httpMethodGet = "GET";
        private string httpMethodPut = "PUT";
        private string httpMethodDelete = "DELETE";

        //produccion
        private string realm = "7451241";
        private string realmRuta = "7451241";
        private string oauthConsumerKey = "ce63cf522b9a8693077aec7daa3f7f3b11cb7bccd42a109e2db4ddd9f2071f94";
        private string oauthToken = "fd0f8baa2fbea3f7f095d8a560e6838ac8008f74381615a113ab025d750f2d80";
        private string oauthConsumerSecret = "3875e1166246fda15fb8916ed35f9429bf5e4c65aa884a2d1b2a4d05d0c52228";
        private string oauthTokenSecret = "4ecdff3d28ccedb08c69cace6f5efa0e53740a0c5a29dc18eb48c20e9daf0598";

        //desarrollo
        //private string realm = "7451241_SB1";
        //private string realmRuta = "7451241-sb1";
        //private string oauthConsumerKey = "6b13c81a90ae5552c619cfd0cc0fa18ce87513633f86ec06303cb47c4bca7041";
        //private string oauthToken = "d9548c00a52db100579a1817d5bfb1f0c227e45b1b6450927e93b80913211118";
        //private string oauthConsumerSecret = "3c63eef239e04e7f9f609d8d39a250feb26edd2bee55ee2e187da7259650ff53";
        //private string oauthTokenSecret = "54d16874de656a0855983739f5a21543577c9d1e525325df718b2c4c6af6fa59";




        public string ObtenerDatos(string opcion)
        {
            string cadena = "";
            if (opcion == "1")
                cadena = HMACSHA256SignatureType;
            else if (opcion == "2")
                cadena = OAuthVersion;
            else if (opcion == "3")
                cadena = oauthConsumerKey;
            else if (opcion == "4")
                cadena = oauthToken;
            else if (opcion == "5")
                cadena = oauthConsumerSecret;
            else if (opcion == "6")
                cadena = oauthTokenSecret;
            else if (opcion == "7")
                cadena = httpMethodPost;
            else if (opcion == "8")
                cadena = httpMethodGet;
            else if (opcion == "9")
                cadena = realm;
            else if (opcion == "10")
                cadena = httpMethodPut;
            else if (opcion == "11")
                cadena = httpMethodDelete;
            return cadena;
        }

        public string ObtenerRuta(string script, string deploy)
        {
            string generalruta = "https://"+ realmRuta + ".restlets.api.netsuite.com/app/site/hosting/restlet.nl?script=";
            string cadena = generalruta + script + "&deploy=" + deploy;
            return cadena;
        }

    }



}
