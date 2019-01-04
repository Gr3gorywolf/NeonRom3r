using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;
using Newtonsoft.Json;
using SharpCompress;
using SharpCompress.Archives;
using System.IO.Compression;
using SharpCompress.Common;
using System.Net;

namespace neonrommer
{
    class miselaneousmethods
    {
        public static string archivodescargados = Android.OS.Environment.ExternalStorageDirectory + "/.romercache/downloadsp.json";
        public static string archivoregistro = Android.OS.Environment.ExternalStorageDirectory + "/.romercache/downloads.json";
        public static string cachepath= Android.OS.Environment.ExternalStorageDirectory + "/.romercache";
        public static string imgpath = Android.OS.Environment.ExternalStorageDirectory + "/.romercache/portraits";

        public static string[] consolelist = { "GB", "GBC", "GBA", "NES", "SNES", "N64", "PSX", "Genesis", "Dreamcast", "NDS" };
        public static string[] consolelistformal = { "Gameboy", "Gameboy Color", "Gameboy Advance", "Nintendo", "Super Nintendo", "Nintendo 64", "Playstation", "Sega Genesis", "Sega Dreamcast", "Nintendo DS" };
        public static string getrandomserial()
        {
            /////////////////////devuelve un serial random alfanumerico para luego crear ids unicos de firebase
            Random rondom = new Random();
            char[] array = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm' };
            return rondom.Next(1, 9).ToString() + array[rondom.Next(0, 12)].ToString()
                 +
                 rondom.Next(1, 9).ToString() + array[rondom.Next(0, 12)].ToString()
                  +
                 rondom.Next(1, 9).ToString() + array[rondom.Next(0, 12)].ToString()
                  +
                 rondom.Next(1, 9).ToString() + array[rondom.Next(0, 12)].ToString()
                  +
                 rondom.Next(1, 9).ToString() + array[rondom.Next(0, 12)].ToString();
        }


        public static void ponerfuente(AssetManager mgr,TextView texto) {

            /*
            Typeface tipo = Typeface.CreateFromAsset(mgr, "GeoOblique.ttf");
            texto.SetTypeface(tipo, TypefaceStyle.Normal);*/
            
            }

#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
        public static void actualizararchivos(Activity contexto,ProgressDialog dialogoprogreso)
        {
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos
            contexto.RunOnUiThread(() =>
            {


                dialogoprogreso = new ProgressDialog(contexto);


                dialogoprogreso.SetCanceledOnTouchOutside(false);
                dialogoprogreso.SetCancelable(false);
                dialogoprogreso.SetTitle("Actualizando directorios de los roms...");
                dialogoprogreso.SetMessage("Por favor espere");
                dialogoprogreso.Show();
            });

            var sdinterna = Android.OS.Environment.ExternalStorageDirectory.Path;
            var sdexterna = "";
            var xd2 = contexto.GetExternalMediaDirs();

            try
            {
                foreach (var xd in xd2)
                {
                    if (Android.OS.Environment.InvokeIsExternalStorageRemovable(xd))
                    {

                        sdexterna = xd.Path.Split(new[] { "Android" }, StringSplitOptions.None)[0];
                        break;
                    }

                }
            }
            catch (Exception) { }
            List<string> nombres = new List<string>();
            List<string> pathx = new List<string>();
            var datosregistry = getteardatosregistry();
            foreach (var xddd in datosregistry) {

                if (!File.Exists(xddd.path)) {
                    nombres.Add(System.IO.Path.GetFileName(xddd.path));
                pathx.Add("");
                }
            }


            bool romper = false;
            var interna = Directory.GetDirectories(sdinterna, "*", SearchOption.AllDirectories).ToList();
            if (sdexterna.Trim() != "") { 
            var externa = Directory.GetDirectories(sdexterna, "*", SearchOption.AllDirectories).ToList();
            

            foreach (var newdir in externa)
            {
                if (romper)
                    break;

                foreach (var patheses in nombres)
                {
                    if (File.Exists(newdir + "/" + patheses))
                    {
                        pathx[nombres.IndexOf(patheses)] = newdir + "/" + patheses;
                        if (pathx.IndexOf("") == -1)
                        {
                            romper = true;
                            break;
                        }
                    }
                }
                if (romper)
                    break;
            }
            }
            foreach (var newdir in interna) {
                if (romper)
                    break;

                foreach (var patheses in nombres) {
                    if (File.Exists(newdir + "/" + patheses)) {
                        pathx[nombres.IndexOf(patheses)] = newdir + "/" + patheses;
                        if (pathx.IndexOf("") == -1) {
                            romper = true; 
                            break;
                        }
                     }
            }
                if (romper)
                    break;
        }



            foreach (var nombrex in nombres) {

                if (datosregistry.Where(axxd => axxd.path.EndsWith(nombrex)).Count() > 0) {
                    if (pathx[nombres.IndexOf(nombrex)].Trim() != "")
                    {
                        datosregistry.Where(axxd => axxd.path.EndsWith(nombrex)).First().path = pathx[nombres.IndexOf(nombrex)];
                    }
                    else {
                        datosregistry.Remove(datosregistry.Where(axxd => axxd.path.EndsWith(nombrex)).First());
                    }
                }
            }



            if(!romper)
                
            if (!Directory.Exists(cachepath))
                Directory.CreateDirectory(cachepath);
            var es = File.CreateText(archivoregistro);
            es.Write(JsonConvert.SerializeObject(datosregistry));
            es.Close();

            contexto.RunOnUiThread(() => {
                dialogoprogreso.Dismiss();
                Toast.MakeText(contexto, "Las rutas de los roms fueron actualizadas", ToastLength.Long).Show();
                
                });

            Console.WriteLine("xd");

        }

#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
        public static string extraer(Activity contexto, ProgressDialog dialogoprogreso, string archivo, string console = "")
        {
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos

            string Innerarchivo = archivo;
            string directorio = "";
           contexto.RunOnUiThread(() =>
            {



#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
                dialogoprogreso = new ProgressDialog(contexto);
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos

                dialogoprogreso.SetCanceledOnTouchOutside(false);
                dialogoprogreso.SetCancelable(false);
                dialogoprogreso.SetTitle("Extrayendo rom...");
                dialogoprogreso.SetMessage("Esto solo pasara una vez con este rom");
                dialogoprogreso.Show();
            });
            if (!File.Exists(System.IO.Path.ChangeExtension(archivo, null)))
            {
                try
                {
                    IArchive entr = null;
                    string entrykey = "";
                    var archive = ArchiveFactory.Open(archivo);
                    directorio = System.IO.Path.GetDirectoryName(archivo);
                    if (console != "Dreamcast")
                    {
                        if (console == "PSX")
                        {
                            entr = archive.Entries.Where(ax => ax.Key.EndsWith(".bin")).FirstOrDefault().Archive;
                            entrykey = archive.Entries.Where(ax => ax.Key.EndsWith(".bin")).FirstOrDefault().Key;

                        }
                        else
                        {
                            entr = archive.Entries.ToList()[0].Archive;
                            entrykey = archive.Entries.ToList()[0].Key;
                        }


                        if (!File.Exists(directorio + "/" + entrykey))
                        {

                            entr.WriteToDirectory(directorio);
                        }
                    }
                    else {
                       directorio = Directory.CreateDirectory(System.IO.Path.GetDirectoryName(archivo) + "/" + System.IO.Path.GetFileNameWithoutExtension(archivo)).FullName;

                        if (directorio.EndsWith("/"))
                               directorio = directorio.Remove(directorio.Length - 1);


                        foreach (var entry in archive.Entries) { 
                             
                          

                            if (entry.Key.ToLower().EndsWith(".gdi")|| entry.Key.ToLower().EndsWith(".chd")|| entry.Key.ToLower().EndsWith(".cdi"))
                                entrykey = entry.Key;
                   

                            entry.WriteToDirectory(directorio);


                        }


                    }


                    // File.Delete(archivo);
                    var lector =JsonConvert.DeserializeObject<List< Models.registry>> (File.ReadAllText(cachepath + "/downloads.json"));
                       for(int i=0;i<lector.Count;i++)
                        {

                        if (lector[i].path == archivo) { 
                                lector[i].path = directorio+ "/" + entrykey;
                            break;
                        }

                    }


                      
                        var arc = File.CreateText(cachepath + "/downloads.json");
                        arc.Write(JsonConvert.SerializeObject(lector));
                        arc.Close();
                            
                 
                    Innerarchivo = directorio + "/" + entrykey;
                    File.Delete(archivo);
                    MainActivity.gettearinstancia().cargardescargas();
               
                }
                catch (Exception)
                {
                    if (!Innerarchivo.EndsWith(".zip") && !Innerarchivo.EndsWith("7z") && !Innerarchivo.EndsWith("rar"))
                    {
                        contexto.RunOnUiThread(() => { Toast.MakeText(contexto, "Archivo ya extraido", ToastLength.Long).Show(); });
                    }
                    else {
                        contexto.RunOnUiThread(() => { Toast.MakeText(contexto, "Error en la extraccion.Puede que no se pueda escribir en esa ruta", ToastLength.Long).Show(); });
                    }
                }
            }
           
            contexto.RunOnUiThread(() =>
            {
              
                dialogoprogreso.Dismiss();



            });




            return Innerarchivo;



        }

        public static  void RocketUpdate()
        {
            var cliente = new WebClient();

            if (!Directory.Exists(cachepath))
                Directory.CreateDirectory(cachepath);

            cliente.DownloadFile("https://raw.githubusercontent.com/Gr3gorywolf/NeonRom3r/master/Updates/update.zip", cachepath+"/update.zip");
            var archive = ArchiveFactory.Open(cachepath+"/update.zip");
         

            foreach (var entry in archive.Entries)
            {
                entry.WriteToDirectory(cachepath, new ExtractionOptions() { Overwrite = true });

            }
            File.Delete(cachepath + "/update.zip");

        }


        public static void cambiarruta(Activity actividad,string consolename,string nuevaruta) {
            bool valida = false;
            try
            {
                var archivo = File.CreateText(nuevaruta + "/archivo.tst");
                archivo.Close();
                File.Delete(nuevaruta + "/archivo.tst");
                valida = true;
            }
            catch (Exception) {
                actividad.RunOnUiThread(() =>
                {

                    Toast.MakeText(actividad,"Ruta invalida por favor seleccione otra",ToastLength.Long).Show();
                    valida = false;
                });
            }
            if (valida) {
            var datos = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText( cachepath + "/paths.json"));
            datos[consolename] = nuevaruta;
            var archi = File.CreateText(cachepath + "/paths.json");
            archi.Write(JsonConvert.SerializeObject(datos));
            archi.Close();
            actividad.RunOnUiThread(() =>
            {

                Toast.MakeText(actividad, "Ruta cambiada exitosamente", ToastLength.Long).Show();
              
            });
            }

        }
        public static void ponerfuente2(AssetManager mgr, TextView texto)
        {

            /////////////////usa una fuente externa en formato ttfs y la convierte a nativa y se la aplica a cualquier textview
            Typeface tipo = Typeface.CreateFromAsset(mgr, "GeoOblique.ttf");
            texto.SetTypeface(tipo, TypefaceStyle.Normal);

        }
        public static List<Models.registry> getteardatosregistry() {
            if (File.Exists(archivoregistro))
            {

               
                return JsonConvert.DeserializeObject<List<Models.registry>>(File.ReadAllText(archivoregistro));
            }
            else {
                return null;
            }

        }

       

        public static void reemplazarpath(string nombrerom, string consola, string newpath) {
            var datos = getteardatosregistry();
            if (datos == null)
                datos = new List<Models.registry>();
            var datoviejo = datos.Where(xv => xv.nombre == nombrerom && xv.consola == consola).First();
            var indice = datos.IndexOf(datoviejo);
            datoviejo.path = newpath;
            datos[indice] = datoviejo;
        }
        public static void guardarenregistry(string nombre,string path,string urlimagen,string consola,string linkdescarga) {
            var datos = getteardatosregistry();
            if (datos == null)
                datos = new List<Models.registry>();
            var xd = new Models.registry
            {
                nombre = nombre,
                path = path,
                portadalink = urlimagen,
                consola = consola,
                linkdescarga = linkdescarga
            };

            if (datos.Where(aaxx=>aaxx.nombre==nombre && aaxx.consola==consola && aaxx.path==path).Count()==0)
               datos.Add(xd);
        
          
            if (!Directory.Exists(cachepath))
                Directory.CreateDirectory(cachepath);
           var es= File.CreateText(archivoregistro);
            es.Write(JsonConvert.SerializeObject(datos));
            es.Close();
            datos.Clear();


        }



        public static List<Models.registry> getteardatosregistrydescargas()
        {
            if (File.Exists(archivodescargados))
            {


                return JsonConvert.DeserializeObject<List<Models.registry>>(File.ReadAllText(archivodescargados));
            }
            else
            {
                return null;
            }

        }
        public static void guardarenregistrydescargas(string nombre, string path, string urlimagen, string consola, string linkdescarga)
        {
            var datos = getteardatosregistrydescargas();
            if (datos == null)
                datos = new List<Models.registry>();
            var xd = new Models.registry
            {
                nombre = nombre,
                path = path,
                portadalink = urlimagen,
                consola = consola,
                linkdescarga = linkdescarga
            };

            if (datos.Where(aaxx => aaxx.nombre == nombre && aaxx.consola == consola && aaxx.path == path).Count()==0 )
                datos.Add(xd);


            if (!Directory.Exists(cachepath))
                Directory.CreateDirectory(cachepath);
            var es = File.CreateText(archivodescargados);
            es.Write(JsonConvert.SerializeObject(datos));
            es.Close();
            datos.Clear();


        }

        public static void abriremulador(Activity contexto,string consola,Android.Net.Uri urlarchivo, ProgressDialog dialogoprogreso) {


            /*
             * 
             * consolas
             * gameboy
             * gameboy color
             * gameboy advance
             * nintendo
             * super nintendo
             * nintendo 64
             * playstation
             * sega genesis
             * 
             * 
             * 
             * 
             * */
            Intent intento = new Intent(Intent.ActionView);
            bool existe = false;
            switch (consola)
            {
                case "GB": // myoldboy(gbc/gb) si es free sera gbcfree sino solo es gbc
                    if (existepaquete(contexto, "com.fastemulator.gbc"))
                    {
                        intento.SetClassName("com.fastemulator.gbc", "com.fastemulator.gbc.EmulatorActivity");
                        intento.SetDataAndType(urlarchivo, "application/x-gbc-rom");
                        existe = true;
                    }
                    else                 
                    if (existepaquete(contexto, "com.fastemulator.gbcfree"))
                    {
                        intento.SetClassName("com.fastemulator.gbcfree", "com.fastemulator.gbc.EmulatorActivity");
                        intento.SetDataAndType(urlarchivo, "application/x-gbc-rom");
                        existe = true;
                    }
                    break;
                case "GBC": // myoldboy(gbc/gb) si es free sera gbcfree sino solo es gbc
                    if (existepaquete(contexto, "com.fastemulator.gbc"))
                    {
                        intento.SetClassName("com.fastemulator.gbc", "com.fastemulator.gbc.EmulatorActivity");
                        intento.SetDataAndType(urlarchivo, "application/x-gbc-rom");
                        existe = true;

                    }                
                    else
                   if (existepaquete(contexto, "com.fastemulator.gbcfree"))
                    {
                        intento.SetClassName("com.fastemulator.gbcfree", "com.fastemulator.gbc.EmulatorActivity");
                        intento.SetDataAndType(urlarchivo, "application/x-gbc-rom");
                        existe = true;
                    }
                    break;
                case "GBA":// myboy(gba)
                    if (existepaquete(contexto, "com.fastemulator.gba"))
                    {
                        intento.SetClassName("com.fastemulator.gba", "com.fastemulator.gba.EmulatorActivity");
                        intento.SetDataAndType(urlarchivo, "application/x-gba-rom");
                        existe = true;
                    }
                    else
                      if (existepaquete(contexto, "com.fastemulator.gbafree"))
                    {
                        intento.SetClassName("com.fastemulator.gbafree", "com.fastemulator.gba.EmulatorActivity");
                        intento.SetDataAndType(urlarchivo, "application/x-gba-rom");
                        existe = true;
                    }
                             
                    break;
                case "NES":
                    if (existepaquete(contexto, "com.portableandroid.classicboyLite"))
                    {
                        intento.SetClassName("com.portableandroid.classicboyLite", "com.portableandroid.classicboy.EntryActivity");
                        intento.SetDataAndType(urlarchivo, "*/*");
                        existe = true;

                    }
                 
                    break;
                case "SNES": // SNNES(SNES) 

                    if (existepaquete(contexto, "com.explusalpha.Snes9xPlus"))
                    {
                        intento.SetClassName("com.explusalpha.Snes9xPlus", "com.imagine.BaseActivity");
                        intento.SetDataAndType(urlarchivo, "*/*");
                        existe = true;

                    }
                    else
                    if (existepaquete(contexto,"cl.godoiapps.snnes")) {

                   
                        intento.SetClassName("cl.godoiapps.snnes", "com.jlizama.snnes.EmulatorActivity");
                        intento.SetDataAndType(urlarchivo, "application/zip");
                       
                    }
                    else
                    if (existepaquete(contexto, "com.portableandroid.classicboyLite"))
                    {
                        intento.SetClassName("com.portableandroid.classicboyLite", "com.portableandroid.classicboy.EntryActivity");
                        intento.SetDataAndType(urlarchivo, "*/*");
                        existe = true;

                    }
                   
                        
                        
                    break;
                case "N64":// mupen64fz(n64)



                    var nombrearchivo = extraer(contexto, dialogoprogreso, urlarchivo.Path);

                    if (existepaquete(contexto, "org.mupen64plusae.v3.fzurita"))
                    {

                        intento.SetClassName("org.mupen64plusae.v3.fzurita", "paulscode.android.mupen64plusae.SplashActivity");
                        intento.SetDataAndType(Android.Net.Uri.Parse(nombrearchivo), "*/*");
                        existe = true;
                    }
                    else
                   
                    if (existepaquete(contexto, "paulscode.android.mupen64plus"))
                    {
                        intento.SetClassName("paulscode.android.mupen64plusae", "paulscode.android.mupen64plusae.MainActivity");
                        intento.SetDataAndType(Android.Net.Uri.Parse(nombrearchivo), "*/*");
                        existe = true;
                    }
                    else
                    if (existepaquete(contexto, "paulscode.android.mupen64plus.free"))
                    {
                        intento.SetClassName("paulscode.android.mupen64plusae", "paulscode.android.mupen64plusae.MainActivity");
                        intento.SetDataAndType(Android.Net.Uri.Parse(nombrearchivo), "*/*");
                        existe = true;
                    }

                    break;
                case "PSX":


                   var filename= extraer(contexto, dialogoprogreso, urlarchivo.Path,"PSX");
             
                    if (existepaquete(contexto, "com.portableandroid.classicboyLite"))
                    {
                        intento.SetClassName("com.portableandroid.classicboyLite", "com.portableandroid.classicboy.EntryActivity");
                        intento.SetDataAndType(Android.Net.Uri.Parse( filename), "*/*");
                        existe = true;
                    }
                    else
                    if (existepaquete(contexto, "com.emulator.fpse"))
                    {
                        intento.SetClassName("com.emulator.fpse", "com.emulator.fpse.Main");
                        intento.SetAction("android.intent.action.VIEW");
                        intento.SetDataAndType(Android.Net.Uri.Parse(filename), "*/*");
                        existe = true;
                    }
                   
                        break;
                case "Genesis":
                    if (existepaquete(contexto, "com.portableandroid.classicboyLite"))
                    {
                        intento.SetClassName("com.portableandroid.classicboyLite", "com.portableandroid.classicboy.EntryActivity");
                        intento.SetDataAndType(urlarchivo, "*/*");
                        existe = true;
                    }
                    break;
                case "Dreamcast":
                    var archi = extraer(contexto, dialogoprogreso, urlarchivo.Path, "Dreamcast");
                    if (existepaquete(contexto, "com.reicast.emulator"))
                    {
                        intento.SetClassName("com.reicast.emulator", "com.reicast.emulator.MainActivity");
                        intento.SetAction("android.intent.action.VIEW");
                        intento.SetDataAndType(Android.Net.Uri.Parse( archi), "*/*");
                        existe = true;
                    }
                    break;
                case "NDS":





                    var rompath = extraer(contexto,dialogoprogreso, urlarchivo.Path);
                    if (existepaquete(contexto, "com.NewDsemu.FreeDraStic"))
                    {
                        intento.SetClassName("com.NewDsemu.FreeDraStic", "com.dsemu.drastic.DraSticActivity");
                 

                        intento.SetDataAndType( Android.Net.Uri.Parse(rompath), "*/*");
                        existe = true;
                    }
                    break;
                 
            }

            contexto.RunOnUiThread(() =>
            {
                if (existe) { 
                Toast.MakeText(contexto, "Abriendo el rom", ToastLength.Long).Show();
                contexto.StartActivity(intento);
                }
                else
                Toast.MakeText(contexto, "No tiene instalado un emulador compatible en su dispositivo", ToastLength.Long).Show();
            });
       




            /*        
            Intent intento = new Intent(Intent.ActionView);
            intento.SetClassName("com.fastemulator.gba", "com.fastemulator.gba.EmulatorActivity");
            intento.SetDataAndType(Android.Net.Uri.Parse(Android.OS.Environment.ExternalStorageDirectory + "/test/juego.gba"), "application/x-gba-rom");
            StartActivity(intento);*/

            /*
                   
             Intent intento = new Intent(Intent.ActionView);
             intento.SetClassName("com.fastemulator.gbcfree", "com.fastemulator.gbc.EmulatorActivity");
             intento.SetDataAndType(Android.Net.Uri.Parse(Android.OS.Environment.ExternalStorageDirectory + "/test/juego.gbc"), "application/x-gbc-rom");
             StartActivity(intento);
             */

            
            //        Intent intento = new Intent(Intent.ActionView);
            //     intento.SetClassName("org.mupen64plusae.v3.fzurita", "paulscode.android.mupen64plusae.SplashActivity");
            //     intento.SetDataAndType(Android.Net.Uri.Parse(Android.OS.Environment.ExternalStorageDirectory + "/test/juego.z64"), "*/*");
            //     StartActivity(intento);

            /*
            
             Intent intento = new Intent(Intent.ActionView);
             intento.SetClassName("cl.godoiapps.snnes", "com.jlizama.snnes.EmulatorActivity");
             intento.SetDataAndType(Android.Net.Uri.Parse(Android.OS.Environment.ExternalStorageDirectory + "/test/juego.smc"), "application/zip");
             StartActivity(intento);
             */

        }

        /// <summary>
        /// Copies the contents of input to output. Doesn't close either stream.
        /// </summary>
        /// 
        public static bool existepaquete(Activity contexto,string nombrepaquete) {

            var pm = contexto.PackageManager.GetInstalledApplications(0);
            if (pm.Where(aaxx => aaxx.PackageName == nombrepaquete).Count()>0 ) {
                return true;
            }
            else {
                return false;
                    }

        }
        public static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        }
    }
}