using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Newtonsoft.Json;
using neonrommer;
namespace emulatorgamessuperscrapper
{
    class superscrapper
    {
        private string desencriptar(string innertext) {

            ////////////////esto se encarga de extraer la info de la rom la cual es una cadena con el siguiente formato
            /////////ROMInformationFile Name:Pokemon - Ruby Version (V1.1)File Size:4.7MBRegion:USAConsole:Gameboy AdvanceDownloads:81,353
            ////////por esa razon es que se reemplazan esos valores por un separador para asi poder splitearlo y sacarlos mas facilmente
            return innertext.Replace("ROMInformation", "")
                .Replace("File Name:", "")
                .Replace("File Size:", "^^^???**//")
                .Replace("Region:", "^^^???**//")
                .Replace("Console:", "^^^???**//")
                .Replace("Downloads:", "^^^???**//");
            
        }


#pragma warning disable CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica
        public async Task<string> getdownloadlink(string romid)
#pragma warning restore CS1998 // El método asincrónico carece de operadores "await" y se ejecutará de forma sincrónica
        {

            ////////////////////////aqui se le agregan los headers a la webrequest para obtener el link de descarga directa desde el server
            string url = "https://emulator.games/get.php";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.KeepAlive = false;
            req.ProtocolVersion = HttpVersion.Version10;
            req.Method = "POST";
            ///////////////se le agrega el tipo de contenido que aceptara de los headers
            req.Accept = "application/json, text/javascript, */*; q=0.01";
            /////////////el servidor de emulator.games para devolverte el link tienes que enviarle un urlencoded con el id de el rom y el tipo que la mayoria de casos es "rom"
            /////////////se crea la cadena y se le extraen los bytes para asi mandarla a travez de un stream
            byte[] postBytes = Encoding.ASCII.GetBytes("set_id="+romid.Trim()+"&set_type=rom");
            //////////////se le agrega el tipo de contenido que enviara a los headers
            req.ContentType = "application/x-www-form-urlencoded";
            //////////se le obtiene el tama;o de los bytes que se enviaran 
            req.ContentLength = postBytes.Length;
            /////////////////se crea el objeto stream basado en el stream de la http request
            Stream requestStream = req.GetRequestStream();
            ///////////////se escriben los bytes en el stream 
            requestStream.Write(postBytes, 0, postBytes.Length);
            //////////////al cerrarlo los datos son enviados junto con la request
            requestStream.Close();

            ///////////////////////se obtiene la response la cual te dara un json con varios links pero el unico que nos importa es el numero 4
            HttpWebResponse response = (HttpWebResponse)req.GetResponse();
            Stream resStream = response.GetResponseStream();

            var sr = new StreamReader(response.GetResponseStream());
            ///////////////////se obtiene el json en string
            string responseText = sr.ReadToEnd();
            ///////////////////se parsea el json de string en objeto nativo para asi obtener los datos mas facilmente
            var result = JsonConvert.DeserializeObject<object[]>(responseText);

           ////////////////devuelve el elemento numero 4 de el array de objetos 
            return result[3].ToString();
        }
        public async Task<Models.rominfo> getrominfo(string link) {


            var doc2 = new HtmlAgilityPack.HtmlWeb();
            /////////////se busca la pagina de info de el rom 

            var htmlDoc2 =await doc2.LoadFromWebAsync(link);


            //////////////////esta pagina si es valida no puede contener 404 ya que el response llega ok pero no tiene nada en las tablas
            //////////////////lo cual podria provocar futuros crashes
            if (!htmlDoc2.Text.Contains("404 Page Not Found"))
            {
                //////////////se selecciona el 2do div de la pagina 
                var nodelo = htmlDoc2.DocumentNode.SelectNodes("//div")[1];
                ////////////dentro de este se obtiene un inner text de una tabla que hay dentro de ese div el cual contiene la info de el rom
                var listaelementos = desencriptar(nodelo.ChildNodes[2].ChildNodes[1].InnerText).Split(new[] { "^^^???**//" }, StringSplitOptions.None  );
                Models.rominfo info = new Models.rominfo();
                /////////////////////////////se busca directamente el elemento rom-link por su ide y se le agregan un par de cosas para hacerlo spliteable
                info.linkdescarga = htmlDoc2.GetElementbyId("rom-link").Attributes["href"].Value.Replace("&amp;", "").Replace("&","").Replace("token=", "&token=").Replace("id=", "&id=").Replace("name=", "&name=");
                ///////////////////////aqui se trata de buscar el id de el rom dentro de 2 parametros los cuales estan de la sig manera
                ///////////////////////&id=<id>&token=<token>
                info.id = info.linkdescarga.Split(new[] { "&id=" }, StringSplitOptions.None)[1].Split(new[] { "&token=" }, StringSplitOptions.None)[0].Replace("&","");
                //////////////////////////con los datos "desencriptados" se le agregan a la instancia de la clase de modelo
                info.nombre = listaelementos[0];
                info.size = listaelementos[1];
                info.region = listaelementos[2];
                info.consola = listaelementos[3];
                /////////////////////////se busca entre hijos la imagen y luego se ele extrae su href
                info.imagen = nodelo.ChildNodes[2].ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[0].Attributes["src"].Value;
                ////////////////////aqui se le extrae el info de descargas y votos si estos son existentes por eso estan dentro de un try catch
                try
                {
                    info.descargas = listaelementos[4];

                    info.votos = nodelo.ChildNodes[2].ChildNodes[0].ChildNodes[1].ChildNodes[1].ChildNodes[1].ChildNodes[0].ChildNodes[0].InnerText.Replace("Out of", " De ");
                }
                catch {
                    /////////////si no los encuentra se le ponen valores por defecto
                    info.descargas = "0";
                    info.votos = "0 de 5";
                }
              
            
                
                //  info.votos=
                return info;


            }
            else {
                return new Models.rominfo();
            }
        }

        public async Task< List<Models.romsinfos>> getwebdata(string consola,int  paginas,bool portadashd ) {

            List<Models.romsinfos> listaroms = new List<Models.romsinfos>();
           var doc = new HtmlAgilityPack.HtmlWeb();
            for(int i = 0; i < paginas; i++) {
                HtmlDocument htmlDoc;
                ///////////////// si la pagina es 0 o 1 se busca no se le agrega el subdirectorio page 
                if (i == 0 || i == 1)
                  /// se descarga la web y se genera un objeto de la clase htmldocument
                 htmlDoc =await doc.LoadFromWebAsync("https://emulator.games/roms/"+consola+"/");
                else
                  /// se descarga la web y se genera un objeto de la clase htmldocument
                 htmlDoc = await doc.LoadFromWebAsync("https://emulator.games/roms/" + consola + "/page/"+i+"/");
                if (!htmlDoc.Text.Contains("404 Page Not Found")) { 
                    //se busca la primera tabla existente en la pagina la cual contiene el info de todos los roms de esa pagina
            var node = htmlDoc.DocumentNode.SelectSingleNode("//table");
            int numerillo = 0;
                   //una vez se tiene la tabla se buscan los tr(table row) los cuales son las filas que contiene esa tabla
                    var nodu = node.ChildNodes["tbody"].SelectNodes("//tr");

            foreach (var nodel in nodu)
            {

                if (numerillo > 0)
                {

                    ////////////////////////cuando no hay una criteria valida el primer elemento de la table siempre dira esto
                    if (!nodel.InnerText.ToLower().Contains("search term not found"))
                    {
                        ///se busca entre los datos de el documento la informacion de los roms existentes para la consola seleccionada
                        ///y crea un objeto para luego agregarlo al array
                        Models.romsinfos elemento = new Models.romsinfos();
                                /////////////////////////a partir de aqui se navega entre hijos de los elementos para poder asi conseguir la info de ellos
                        elemento.nombre= nodel.ChildNodes[0].InnerText;
                                ////////////si la portada no es hd pondra la de default si no se buscara una hd en el server
                        if (!portadashd)
                        elemento.imagen= nodel.ChildNodes[0].ChildNodes[0].ChildNodes[0].Attributes["src"].Value;
                        else
                                
                        elemento.imagen = nodel.ChildNodes[0].ChildNodes[0].ChildNodes[0].Attributes["src"].Value.Replace("thumbnails/", "");
                        elemento.link= nodel.ChildNodes[0].ChildNodes[0].Attributes["href"].Value;
                        elemento.descargas= nodel.ChildNodes[1].InnerText;
                        listaroms.Add(elemento);
                        numerillo++;
                    }
                }

                else
                {
                    numerillo++;
                }



            }
                }
            }

            return listaroms;

        }

    }
}
