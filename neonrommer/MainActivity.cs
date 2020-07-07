using System;
using System.Collections.Generic;
using System.Linq;
using Android.Graphics;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.IO;
using System.Net;
using System.Threading;
using Android.Graphics.Drawables;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using Android.Glide;
using Newtonsoft.Json;
using directorychooser;

namespace neonrommer
{
	[Activity(Label = "NeonRom3r", Theme = "@style/Theme.DesignDemo", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
	public class MainActivity : AppCompatActivity
	{
        ///////estatuscodes
        //////0-rom console selected
        //////1-rom downloads
        //////2-roms consoles chooser
        //////3-emulators consoles choser
        //////4-emulator consoles selected
        ///5-pathchanging
        int selectedconsole = 0;
        int estado = 0;
        int idimagen = 0;
        public ListView listin;
        Android.Support.V4.Widget.DrawerLayout sidem;
        NavigationView itemsm;
      
      
        public int[] resourceids = { Resource.Drawable.GB, Resource.Drawable.GBC, Resource.Drawable.GBA, Resource.Drawable.NES, Resource.Drawable.SNES, Resource.Drawable.N64, Resource.Drawable.PSX, Resource.Drawable.Genesis,Resource.Drawable.Dreamcast,Resource.Drawable.NDS };
      Dictionary<string, List<Models.emuladores>> diccionarioemuladores = new Dictionary<string, List<Models.emuladores>>();
        Android.Support.V7.Widget.SearchView searchview;
        public List<int> romcounts = new List<int>();
        public bool online = true;
        public List<string> listillaxx = new List<string>();
        string directoriocache = Android.OS.Environment.ExternalStorageDirectory + "/.romercache";
        List<Models.romsinfos> listaelementos = new List<Models.romsinfos>();
#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
        public ProgressDialog dialogoprogreso;
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos
        List<string> downloadlinks = new List<string>();
        public static bool presionadounavez = false;
        public static MainActivity actividad;
        protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.activity_main);
            StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
            StrictMode.SetVmPolicy(builder.Build());
            builder.DetectFileUriExposure();
            ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(ValidateRemoteCertificate);
            // se selecciona la version de el protocolo de seguridad a la versiond e tls 1.2
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.my_toolbar);
            SetSupportActionBar(toolbar);
            sidem = FindViewById<Android.Support.V4.Widget.DrawerLayout>(Resource.Id.drawer_layout);
            itemsm = FindViewById<NavigationView>(Resource.Id.content_frame);
            searchview = FindViewById<Android.Support.V7.Widget.SearchView>(Resource.Id.searchView);
            listin = FindViewById<ListView>(Resource.Id.listView1);
            actividad = this;
            //////////////////////////////////////////////////
           SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.hambur);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.Title = "";
            SupportActionBar.SetBackgroundDrawable(new ColorDrawable(Color.ParseColor("#2b2e30")));
            online = Intent.GetBooleanExtra("online", false);
       

            TextView titleTextView = null;

            try
            {
                var f = toolbar.Class.GetDeclaredField("mTitleTextView");
                f.Accessible = true;
                titleTextView = (TextView)f.Get(toolbar);

                titleTextView.Ellipsize=Android.Text.TextUtils.TruncateAt.Marquee;
                titleTextView.Focusable = true ;
                titleTextView.FocusableInTouchMode=true;
                titleTextView.RequestFocus();
                titleTextView.SetSingleLine(true);
                titleTextView.Selected=true;
                titleTextView.SetMarqueeRepeatLimit(-1);

            }
            catch 
            {
            }
           
            if (!Directory.Exists(directoriocache)) {

                Directory.CreateDirectory(directoriocache);
            }

            listin.ItemClick += async (aa, aaa)  => {
                var texto = aaa.View.FindViewById<TextView>(Resource.Id.textView).Text;
                if (estado == 0)
                {

                    var link = listaelementos.First(aax => aax.Name == texto).InfoLink;
                    Intent intento = new Intent(this, typeof(actbiginfo));
                    intento.PutExtra("link", link);
                    intento.PutExtra("consoleindex", selectedconsole);
                    intento.PutExtra("placeholder", idimagen);
                    StartActivity(intento);
                    Console.WriteLine("");
                   
                }
                else
                if (estado == 1)
                {

                    var seleccion = listaelementos.First(aax => aax.Name == texto);
                    new Thread(() =>
                    {

                        miselaneousmethods.abriremulador(this,   miselaneousmethods.consolelist[miselaneousmethods.consolelistformal.ToList().IndexOf(seleccion.Region)]   , Android.Net.Uri.Parse(seleccion.InfoLink), dialogoprogreso);

                    }).Start();
                }
                else
                if (estado == 2)
                {
                    var consoleindex = miselaneousmethods.consolelist.ToList().IndexOf(listaelementos.First(aax => aax.Name == texto).InfoLink);

                    selectedconsole = consoleindex;
                    SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.home);
                    new Thread(() =>
                    {

                        cargarconsola(consoleindex);
                       

                    }).Start();

                }
                else
                     if (estado == 3)
                {
                    var seleccion = listaelementos.First(aax => aax.Name == texto);
                    var indice = miselaneousmethods.consolelistformal.ToList().IndexOf(seleccion.Name);
                    if (diccionarioemuladores.ContainsKey(miselaneousmethods.consolelist[indice]))
                    {
                        new Thread(() =>
                    {

                        cargaremulador(indice);

                    }).Start();
                    }
                    else {
                        Toast.MakeText(this, "No se han añadido emuladores para esta consola aún", ToastLength.Long).Show();
                    }
                }
                if (estado == 4)
                {
                    var seleccion = listaelementos.First(aax => aax.Name == texto);
                    var uri = Android.Net.Uri.Parse(seleccion.InfoLink);
                    var intent = new Intent(Intent.ActionView, uri);
                    StartActivity(intent);
                }
                if (estado == 5) {

                    var seleccion = listaelementos.First(aax => aax.Name == texto);
                    SimpleFileDialog sfd = new SimpleFileDialog(this, SimpleFileDialog.FileSelectionMode.FolderChooseRoot);
                   var rutula  = await sfd.GetFileOrDirectoryAsync(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath);
                    miselaneousmethods.cambiarruta(this, miselaneousmethods.consolelist[miselaneousmethods.consolelistformal.ToList().IndexOf(seleccion.Name)], rutula);
                    new Thread(() =>
                    {
                        cargarrutas();
                    }).Start();
                }

            };
            new Thread(() =>
            {
                ///idimagen = Resource.Drawable.gameboy;
                ///

                if (online) { 
                llenarlistas();
                cargarconsolas();
          
                }
                else {
                    cargardescargas();
                    }
                //  cargarconsola(0);
            }).Start();
            new Thread(() =>
            {
                alertifnewdowload();
            }).Start();
            new Thread(() => infoserverstatus()).Start();
            ////////////////////////////////
            itemsm.NavigationItemSelected += (sender, e) =>
            {
                e.MenuItem.SetChecked(true);
                e.MenuItem.SetChecked(true);
                var nombreconsola = e.MenuItem.TitleFormatted.ToString().Trim();
                SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.hambur);
                if (nombreconsola == "Acerca de")
                {

                    StartActivity(new Intent(this, typeof(actacercade)));
                }
                else
                if (nombreconsola == "Deja un feedback")
                {
                    if (online)
                        StartActivity(new Intent(this, typeof(actsugerencias)));
                    else
                        Toast.MakeText(this, "Solo disponible en modo online", ToastLength.Long).Show();
                }
                if (nombreconsola == "Roms descargados")
                {

                  
                        if (File.Exists(miselaneousmethods.archivoregistro))
                    {
                        searchview.SetQuery("", true);
                        new Thread(() =>
                        {
                            cargardescargas();
                        }).Start();
                    }
                    else
                    {
                        Toast.MakeText(this, "Usted no ha descargado ningun rom", ToastLength.Long).Show();
                    }
                    
                }
                else
                if (nombreconsola == "Roms") {

                    if (online) {
                        searchview.SetQuery("", true);
                        new Thread(() =>
                        {
                            cargarconsolas();
                        }).Start();
                    }                   
                    else
                        Toast.MakeText(this, "Solo disponible en modo online", ToastLength.Long).Show();
                 

                }
                if (nombreconsola == "Emuladores")
                {

                    if (online)
                    {
                        searchview.SetQuery("", true);
                        new Thread(() =>
                        {
                           cargaremuladores();
                        }).Start();
                    }
                    else
                     Toast.MakeText(this, "Solo disponible en modo online", ToastLength.Long).Show();
                }
                if (nombreconsola == "Carpetas de descargas")
                {
                    searchview.SetQuery("", true);
                    new Thread(() =>
                        {
                            cargarrutas();
                        }).Start();
                   
                

                }
                e.MenuItem.SetChecked(false);
                e.MenuItem.SetChecked(false);
                sidem.CloseDrawers();
              

            };
            //////////////////////////////////////////////////////////////////////////////filter
            ///
        
            searchview.QueryTextSubmit += (aa, aaa)=>
            {
                Console.WriteLine("klkprro");
                if (estado == 0 || estado == 4)
                {
                    adaptadorroms adaptadolllll = null;
                        adaptadolllll = new adaptadorroms(this, listaelementos.Where(aaxx => aaxx.Name.ToLower().Contains(aaa.NewText.ToLower())).ToList(), idimagen);
                    RunOnUiThread(() => listin.Adapter = adaptadolllll);

                }
               
            };
            searchview.QueryTextChange += (aa, aaa) =>
            {
                try
                {
                    if (aaa.NewText != null)
                    {
                        adaptadorroms adaptadolllll = null;
                        adaptadorromsdownloaded adaptadolllll2 = null;
                        if (estado == 0 || estado == 4) {
                            if(romcounts[selectedconsole]<=500)
                               adaptadolllll = new adaptadorroms(this, listaelementos.Where(aaxx => aaxx.Name.ToLower().Contains(aaa.NewText.ToLower())).ToList(), idimagen);
                            else
                                if (aaa.NewText.Length <=1)
                            {

                                adaptadolllll = new adaptadorroms(this, listaelementos.Where(aaxx => aaxx.Name.ToLower().Contains("")).ToList(), idimagen);
                            }
                        }
                        else
                        if (estado == 1)
                        {

                            var selector = listaelementos.Where(aaxx => aaxx.Name.ToLower().Contains(aaa.NewText.ToLower()) || aaxx.Region.ToLower().Contains(aaa.NewText.ToLower())).ToList();
                            var links = new List<string>();
                            foreach (var axx in selector)
                            {
                                links.Add(downloadlinks[listaelementos.IndexOf(axx)]);
                            }

                            adaptadolllll2 = new adaptadorromsdownloaded(this, selector, Resource.Drawable.download, null, links);

                            RunOnUiThread(() => listin.Adapter = adaptadolllll2);
                            //   links.Clear();
                        }

                        else
                        {
                            var selector = listaelementos.Where(aaxx => aaxx.Name.ToLower().Contains(aaa.NewText.ToLower())).ToList();
                            var portraits = new List<int>();
                            foreach (var axx in selector)
                            {
                                portraits.Add(resourceids[listaelementos.IndexOf(axx)]);
                            }

                            adaptadolllll = new adaptadorroms(this, listaelementos.Where(aaxx => aaxx.Name.ToLower().Contains(aaa.NewText.ToLower())).ToList(), idimagen, portraits.ToArray(), true);



                        }

                        if (estado != 1 && romcounts[selectedconsole]<=500) { 
                       RunOnUiThread(() => listin.Adapter = adaptadolllll);
                        }

                    }
                    else
                    {
                        if (estado == 0 || estado == 4)
                        {
                            if (romcounts[selectedconsole] >= 500) { 
                                adaptadorroms adaptadolllll = null;
                            adaptadolllll = new adaptadorroms(this, listaelementos.Where(aaxx => aaxx.Name.ToLower().Contains("")).ToList(), idimagen);
                            }

                        }
                    }
                }
                catch {

                }
                /*var adaptadol2 = new adapterlistaoffline(this, nombreses.Where(a => a.ToLower().Contains(aaa.NewText.ToLower())).ToList(), portadases, "", nombreses, diccionario, patheses);

                RunOnUiThread(() => lista.Adapter = adaptadol2);*/
            };


        }


        public static MainActivity gettearinstancia() {

            return actividad;
        }
        public void creararchivosyagregar(string[] consolelist) {
           RunOnUiThread(()=> dialogoprogreso.Dismiss());
            setdialog("Descargando actualización", "Por favor espere...");
           
            miselaneousmethods.RocketUpdate();
            foreach (var elemzz in consolelist)
            {
                listillaxx.Add(File.ReadAllText( miselaneousmethods.cachepath+"/"+ elemzz + ".json"));
            }
           
            for (int i = 0; i < consolelist.Length; i++)
            {
                romcounts.Add(JsonConvert.DeserializeObject<List<Models.romsinfos>>(listillaxx[i]).Count);             
            }
            var emuladores = File.ReadAllText(miselaneousmethods.cachepath + "/emulators.json" );
            diccionarioemuladores = JsonConvert.DeserializeObject<Dictionary<string, List<Models.emuladores>>>(emuladores);
            var archi = File.CreateText(directoriocache + "/version.gr3d");
            archi.Write(Intent.GetStringExtra("ver"));
            archi.Close();
        }

        public void llenarlistas() {
            setdialog("Cargando roms", "Por favor espere...");
            if (listillaxx.Count == 0 && !File.Exists(directoriocache + "/version.gr3d"))
            {

                creararchivosyagregar(miselaneousmethods.consolelist);
            }
            else
           if (listillaxx.Count == 0)
            {
                var verlocal = int.Parse(File.ReadAllText(directoriocache + "/version.gr3d").Trim());
                var verserver = int.Parse(Intent.GetStringExtra("ver"));
                if (verserver > verlocal)
                {
                    creararchivosyagregar(miselaneousmethods.consolelist);
                }
                else
                {

                    foreach (var elemzz in miselaneousmethods.consolelist)
                    {
                        var texto = File.ReadAllText(directoriocache + "/" + elemzz + ".json");
                        listillaxx.Add(texto);
                        romcounts.Add(JsonConvert.DeserializeObject<List<Models.romsinfos>>(texto).Count);

                    }


                    diccionarioemuladores = JsonConvert.DeserializeObject<Dictionary<string, List<Models.emuladores>>>(File.ReadAllText(directoriocache+"/emulators.json"));

                }
            }
            RunOnUiThread(() => { dialogoprogreso.Dismiss(); });

        }



        public void cargaremulador(int console) {



            RunOnUiThread(() => {

                searchview.SetQuery("", true);
                SupportActionBar.Title = miselaneousmethods.consolelistformal[console];
            });
            try
            {
               // Glide.Get(this).ClearMemory();
            }
            catch { }
            listaelementos.Clear();

            setdialog("Cargando lista de emuladores", "Por favor espere...");


            foreach (var ax in diccionarioemuladores[miselaneousmethods.consolelist[console]])
            {

                Models.romsinfos modelito = new Models.romsinfos
                {
                    Name = ax.nombre,
                    InfoLink = ax.link,
                    Portrait = ax.imagen,
                    Region = ax.compactible
                };

                /*    if (modelito.InfoLink.Contains("https://play.google.com/store/apps/details?id="))
                    {*/
                try {
                    if (miselaneousmethods.existepaquete(this, modelito.InfoLink.Split('=')[1]))
                        modelito.Region += " - instalado";
                }
                catch (Exception)
                {

                }
              //  }

                    listaelementos.Add(modelito);
                modelito = new Models.romsinfos();
            }
          

       
            estado = 4;
         
            idimagen = resourceids[console];
         ///   listaelementos = listaelementos.OrderBy(c => c.nombre).ToList();

            var adaptadolllll = new adaptadorroms(this, listaelementos, resourceids[console]);
            RunOnUiThread(() => {
                listin.Adapter = adaptadolllll;
                dialogoprogreso.Dismiss();
                SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.home);
            });

            GC.Collect(0);
        }

        public void cargarrutas() {
            try
            {
                var rutas = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(miselaneousmethods.cachepath + "/paths.json"));
                listaelementos.Clear();
                for (int i = 0; i < listillaxx.Count; i++)
                {
                    Models.romsinfos modelito = new Models.romsinfos
                    {
                        Name = miselaneousmethods.consolelistformal[i],
                        InfoLink = miselaneousmethods.consolelist[i],
                        Portrait = "android.resource//" + PackageName + "/" + resourceids[i],
                        Region = rutas[miselaneousmethods.consolelist[i]]
                    };

                    listaelementos.Add(modelito);

                };
                estado = 5;
                var adaptadolllll = new adaptadorroms(this, listaelementos, idimagen, resourceids);
                RunOnUiThread(() =>
                {
                    SupportActionBar.Title = "Carpetas de descargas";
                    listin.Adapter = adaptadolllll;
                    dialogoprogreso.Dismiss();
                    SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.hambur);
                });


            }
            catch (Exception) {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                string downloadpath = System.IO.Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDownloads);
                foreach (var axd in miselaneousmethods.consolelist)
                {
                    dic.Add(axd, downloadpath);

                }
                if (!Directory.Exists(directoriocache))
                    Directory.CreateDirectory(directoriocache);

                var xdd = File.CreateText(directoriocache + "/paths.json");
                xdd.Write(JsonConvert.SerializeObject(dic));
                xdd.Close();
                cargarrutas();

            }
            GC.Collect(0);

        }
        public void cargaremuladores() {

            listaelementos.Clear();
            for (int i = 0; i < listillaxx.Count; i++)
            {
                Models.romsinfos modelito = new Models.romsinfos
                {
                    Name = miselaneousmethods.consolelistformal[i],
                    InfoLink = miselaneousmethods.consolelist[i],
                    Portrait = "android.resource//" + PackageName + "/" + resourceids[i]
                };
                if (diccionarioemuladores.ContainsKey(miselaneousmethods.consolelist[i]))
                    modelito.Region = diccionarioemuladores[miselaneousmethods.consolelist[i]].Count + " Emuladores";
                else
                    modelito.Region = "0 Emuladores";

                listaelementos.Add(modelito);

            };
            estado = 3;
            var adaptadolllll = new adaptadorroms(this, listaelementos, idimagen, resourceids);
            RunOnUiThread(() => {
                SupportActionBar.Title = "Todos los emuladores";
                listin.Adapter = adaptadolllll;
                dialogoprogreso.Dismiss();
                SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.hambur);
            });

            GC.Collect(0);


        }
        public void cargardescargas() {
            ///    miselaneousmethods.actualizararchivos(this);
            ///    

            bool noencontro = false;
            listaelementos.Clear();
            setdialog("Cargando roms", "Por favor espere...");
            downloadlinks.Clear();

           
            foreach (var klk in miselaneousmethods.getteardatosregistry())
            {
                Models.romsinfos modelito = new Models.romsinfos
                {
                    Name = klk.nombre,
                    InfoLink = klk.path,
                    Portrait = miselaneousmethods.imgpath + "/" + klk.nombre + ".jpg",
                    Region = klk.consola
                };

                if (File.Exists(modelito.InfoLink)) { 
                    downloadlinks.Add(klk.linkdescarga);
                listaelementos.Add(modelito);
                }
                else
                    noencontro = true;

               
           
            }
            downloadlinks.Reverse();
            listaelementos.Reverse();
            var adaptadolllll = new adaptadorromsdownloaded(this, listaelementos, Resource.Drawable.download,null,downloadlinks);
            RunOnUiThread(() => {
                SupportActionBar.Title = "Roms descargados";
                listin.Adapter = adaptadolllll;
                dialogoprogreso.Dismiss();
            });
            if (noencontro)
            {
                RunOnUiThread(() => {
                
                        new Android.App.AlertDialog.Builder(this).SetTitle("No se pudieron encontrar algunos roms").SetMessage("Al parecer algunos roms fueron movidos o eliminados de sus rutas originales si desea actualizar sus rutas por favor haga toque escanear").SetCancelable(false).SetNegativeButton("Cancelar", cancell).SetPositiveButton("Escanear", scan).Create().Show();
                   


                });
            }

            GC.Collect(0);
            ///////estatuscodes
            //////0-roms search
            //////1-rom downloads
            //////2-romselection
            estado = 1;
      
        }
        public void page(object sender, EventArgs e)
        {
          
            var uri = Android.Net.Uri.Parse("https://gr3gorywolf.github.io/neonrom3r-webpage/");
            var intent = new Intent(Intent.ActionView, uri);
            StartActivity(intent);
        }
        public void cancell(object sender, EventArgs e)
        {

        }
        public void scan(object sender, EventArgs e)
        {

            new Thread(() =>
            {
                miselaneousmethods.actualizararchivos(this, dialogoprogreso);
                cargardescargas();

            }).Start();
        }
        public void cargarconsolas() {
            listaelementos.Clear();
            for (int i = 0; i < listillaxx.Count; i++) {
                Models.romsinfos modelito = new Models.romsinfos
                {
                    Name = miselaneousmethods.consolelistformal[i],
                    InfoLink = miselaneousmethods.consolelist[i],
                    Portrait = "android.resource//" + PackageName + "/" + resourceids[i],
                    Region = romcounts[i].ToString() + " Roms"
                };
                listaelementos.Add(modelito);

            };
            estado = 2;
            var adaptadolllll = new adaptadorroms(this, listaelementos, idimagen,resourceids);
            RunOnUiThread(() => {
                SupportActionBar.Title= "Todas las consolas";
                listin.Adapter = adaptadolllll;
                dialogoprogreso.Dismiss();
                SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.hambur);
            });
       
            GC.Collect(0);
        }

        public void cargarconsola(int console) {


           
               
            RunOnUiThread(() => {

                searchview.SetQuery("", true);
                SupportActionBar.Title = miselaneousmethods.consolelistformal[console];
            });
            try
            {
               // Glide.Get(this).ClearMemory();
            }
            catch {}
            listaelementos.Clear();
           
            setdialog("Cargando roms", "Por favor espere...");
            
            
           

            var todo = "";
        

                todo = listillaxx[console];

            listaelementos = JsonConvert.DeserializeObject<List<Models.romsinfos>>(todo);
            //////0-roms search
            //////1-rom downloads
            //////2-romselection
            estado = 0;
            todo = "";
            idimagen = resourceids[console];
            listaelementos = listaelementos.OrderBy(c => c.Name).ToList();

            var adaptadolllll = new adaptadorroms(this, listaelementos,resourceids[console]);
            RunOnUiThread(() => {
                listin.Adapter = adaptadolllll;
                dialogoprogreso.Dismiss();
                SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.home);
                if (romcounts[selectedconsole] >= 500)
                    Toast.MakeText(this, "Busqueda dinamica deshabilitada. Pulse enter para la busqueda", ToastLength.Long).Show();
            });
        
            GC.Collect(0);

        }
        private static bool ValidateRemoteCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyErrors)
        {
            return true;
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {

        

            return true;
        }

        public override bool OnPrepareOptionsMenu(IMenu menu)
        {
          

            return base.OnPrepareOptionsMenu(menu);
        }


        public void escanearroms() {

        

        }
        public void setdialog(string titulo,string mensaje) {
            RunOnUiThread(() =>
            {

                #pragma warning disable CS0618 // El tipo o el miembro están obsoletos
                dialogoprogreso = new ProgressDialog(this);
                #pragma warning restore CS0618 // El tipo o el miembro están obsoletos

                dialogoprogreso.SetCanceledOnTouchOutside(false);
                dialogoprogreso.SetCancelable(false);
                dialogoprogreso.SetTitle(titulo);
                dialogoprogreso.SetMessage(mensaje);
                dialogoprogreso.Show();
            });
        }
        public override void OnBackPressed()
        {

            if (sidem.IsDrawerOpen(Android.Support.V4.View.GravityCompat.Start))
            {
                sidem.CloseDrawers();
            }
            else {
                if (estado == 0)
                {
                    new Thread(() =>
                    {
                        selectedconsole = 0;
                        cargarconsolas();
                    }).Start();
                }
                else
                if (estado == 4)
                {
                    new Thread(() =>
                    {

                        cargaremuladores();
                    }).Start();
                }
                else {
                    if (presionadounavez)
                    {
                        Finish();
                    }
                    else {
                        presionadounavez = true;
                        Toast.MakeText(this, "Pulse otra vez para salir", ToastLength.Short).Show();
                        new Handler().PostDelayed(new RunnableHelper(), 2000);
                    }


                }

            }

           // base.OnBackPressed();
        }
         public class RunnableHelper : Java.Lang.Object, Java.Lang.IRunnable

        {
              public void Run()
          {
             presionadounavez = false;
            }
 }


        public void infoserverstatus() {
            string CheckUrl = "https://the-eye.eu/";

            try
            {
                HttpWebRequest iNetRequest = (HttpWebRequest)WebRequest.Create(CheckUrl);

                iNetRequest.Timeout = 35000;

                WebResponse iNetResponse = iNetRequest.GetResponse();

                // Console.WriteLine ("...connection established..." + iNetRequest.ToString ());
                iNetResponse.Close();

             
            }
            catch (WebException)
            {

                // Console.WriteLine (".....no connection..." + ex.ToString ());

                RunOnUiThread(() => {
                    new Android.App.AlertDialog.Builder(this)
                   .SetTitle("Atención")
                   .SetMessage("Error al conectar al servidor de descargas..\n#nota\nPosiblemente tenga errores al descargar roms")
                   .SetCancelable(false)
                   .SetPositiveButton("Entendido!", (aa,fsdf)=> { })
                   .Create()
                   .Show();

                });
            }

        }

        public void alertifnewdowload() {
            try { 
            var text = new WebClient().DownloadString("https://raw.githubusercontent.com/Gr3gorywolf/NeonRom3r/master/Updates/ver");
            var text2 = File.ReadAllText(miselaneousmethods.cachepath + "/appver");
            if (int.Parse(text.Trim()) > int.Parse(text2.Trim())) {
                RunOnUiThread(() =>
                {
                   
               
                    new Android.App.AlertDialog.Builder(this)
                    .SetTitle("Atención")
                    .SetMessage("Existe una  versión mas reciente de la aplicación por favor pulse ok para ir a  la pagina de descarga")
                    .SetCancelable(false).SetNegativeButton("Cancelar", cancell)
                    .SetPositiveButton("Ok", page)
                    .Create()
                    .Show();


                });

            }
            }
            catch(Exception e) {
                Console.WriteLine(e.Message);
            }

        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    if (estado == 2)
                        sidem.OpenDrawer(Android.Support.V4.View.GravityCompat.Start);
                    else
                    if (estado == 0)
                    {
                        SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.hambur);
                        selectedconsole = 0;
                        cargarconsolas();
                        
                    }
                    else
                    if (estado == 4)
                    {
                        SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.hambur);
                        cargaremuladores();
                    }
                    else {
                      
                        sidem.OpenDrawer(Android.Support.V4.View.GravityCompat.Start);
                    }
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

    
	}
}

