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
using emulatorgamessuperscrapper;
using Android.Glide;
namespace neonrommer
{
	[Activity(Label = "NeonRom3r", Theme = "@style/Theme.DesignDemo", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
	public class MainActivity : AppCompatActivity
	{

        int idimagen = 0;
        public ListView listin;
        Android.Support.V4.Widget.DrawerLayout sidem;
        NavigationView itemsm;
        Android.Support.V7.Widget.SearchView searchview;
        public List<string> listillaxx = new List<string>();
        string directoriocache = Android.OS.Environment.ExternalStorageDirectory + "/.romercache";
        List<Models.romsinfos> listaelementos = new List<Models.romsinfos>();
#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
        public ProgressDialog dialogoprogreso;
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos
        protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.activity_main);
        
            ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(ValidateRemoteCertificate);
            // se selecciona la version de el protocolo de seguridad a la versiond e tls 1.2
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.my_toolbar);
            SetSupportActionBar(toolbar);
            sidem = FindViewById<Android.Support.V4.Widget.DrawerLayout>(Resource.Id.drawer_layout);
            itemsm = FindViewById<NavigationView>(Resource.Id.content_frame);
            searchview = FindViewById<Android.Support.V7.Widget.SearchView>(Resource.Id.searchView);
            listin = FindViewById<ListView>(Resource.Id.listView1);
            //////////////////////////////////////////////////
           SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.hambur);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.Title = "";
            SupportActionBar.SetBackgroundDrawable(new ColorDrawable(Color.ParseColor("#2b2e30")));
           
            if (!Directory.Exists(directoriocache)) {

                Directory.CreateDirectory(directoriocache);
            }

            listin.ItemClick += (aa, aaa) => {
            
                var texto = aaa.View.FindViewById<TextView>(Resource.Id.textView).Text ;
                var link = listaelementos.First(aax => aax.nombre == texto).link;
                Intent intento = new Intent(this, typeof(actbiginfo));
                intento.PutExtra("link", link);
                intento.PutExtra("placeholder", idimagen);
                StartActivity(intento);
                Console.WriteLine("");

            };
            new Thread(() =>
            {
                idimagen = Resource.Drawable.gameboy;
                cargarconsola(0);
            }).Start();


            ////////////////////////////////
            itemsm.NavigationItemSelected += (sender, e) =>
            {
                e.MenuItem.SetChecked(true);
                e.MenuItem.SetChecked(true);
                var nombreconsola = e.MenuItem.TitleFormatted.ToString().Trim();
                var numerocoonsola = 0;
                if (nombreconsola == "Todos")
                {
                    idimagen = Resource.Drawable.gamepadvariant;
                    numerocoonsola = 100;
                }
                if (nombreconsola == "Gameboy")
                {
                    idimagen = Resource.Drawable.gameboy;
                    numerocoonsola = 0;
                }
                else
                if (nombreconsola == "Gameboy Color")
                {
                    idimagen = Resource.Drawable.game;
                    numerocoonsola = 1;
                }
                else
                if (nombreconsola == "Gameboy Advance")
                {
                    idimagen = Resource.Drawable.GBA;
                    numerocoonsola = 2;
                }
                else
               if (nombreconsola == "Nintendo")
                {
                    idimagen = Resource.Drawable.nintendogamesconsole;
                    numerocoonsola = 3;
                }
                else
               if (nombreconsola == "Super nintendo")
                {
                    idimagen = Resource.Drawable.supernintendo;
                    numerocoonsola = 4;
                }
                else
               if (nombreconsola == "Nintendo 64")
                {
                    idimagen = Resource.Drawable.nintendo64console;
                    numerocoonsola = 5;
                }
                else
               if (nombreconsola == "Playstation")
                {
                    idimagen = Resource.Drawable.playstation;
                    numerocoonsola = 6;
                }
                else
               if (nombreconsola == "Sega megadrive") {
                    idimagen = Resource.Drawable.segamegadrive;
                    numerocoonsola = 7;
                }
                else
                if (nombreconsola == "Acerca de")
                {
                    StartActivity(new Intent(this, typeof(actacercade)));
                }
                else
                if (nombreconsola == "Deja un feedback")
                {
                    StartActivity(new Intent(this, typeof(actsugerencias)));
                }

                e.MenuItem.SetChecked(false);
                e.MenuItem.SetChecked(false);
                sidem.CloseDrawers();
                if (nombreconsola != "Acerca de" && nombreconsola != "Deja un feedback") {
                new Thread(() =>
                {
                    cargarconsola(numerocoonsola);
                }).Start();
                }

            };
            searchview.QueryTextChange += (aa, aaa) =>
            {
                try
                {
                    if (aaa.NewText != null)
                    {
                        var adaptadolllll = new adaptadorroms(this, listaelementos.Where(aaxx => aaxx.nombre.ToLower().Contains(aaa.NewText.ToLower())).ToList(), idimagen);
                        RunOnUiThread(() => listin.Adapter = adaptadolllll);
                    }
                }
                catch {

                }
                /*var adaptadol2 = new adapterlistaoffline(this, nombreses.Where(a => a.ToLower().Contains(aaa.NewText.ToLower())).ToList(), portadases, "", nombreses, diccionario, patheses);

                RunOnUiThread(() => lista.Adapter = adaptadol2);*/
            };


        }



        public void creararchivosyagregar(string[] consolelist) {
            var archi = File.CreateText(directoriocache + "/version.gr3d");
            archi.Write(Intent.GetStringExtra("ver"));
            archi.Close();
            foreach (var elemzz in consolelist)
            {

                listillaxx.Add(new WebClient().DownloadString("https://gr3gorywolf.github.io/getromdownload/" + elemzz + ".gr3dump"));

            }
            for (int i = 0; i < consolelist.Length; i++)
            {

                var archil = File.CreateText(directoriocache + "/" + consolelist[i] + ".gr3d");
                archil.Write(listillaxx[i]);
                archil.Close();
            }

        }
        public void cargarconsola(int console) {


            string[] consolelist = { "gameboy", "gameboycolor", "gba", "nintendo", "supernintendo", "nintendo64", "playstation", "segagenesis" };
            try {
                RunOnUiThread(() =>
                {
                    searchview.SetQuery("", true);
                });
            }
            catch { }

            try {

                Glide.Get(this).ClearMemory();
              
            }
            catch {

            }
          
            listaelementos.Clear();
            RunOnUiThread(() =>
            {      
#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
                dialogoprogreso = new ProgressDialog(this);
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos
                dialogoprogreso.SetCanceledOnTouchOutside(false);
                dialogoprogreso.SetCancelable(false);
                dialogoprogreso.SetTitle("Buscando roms...");
                dialogoprogreso.SetMessage("Por favor espere");
                dialogoprogreso.Show();
            });
            
            if (listillaxx.Count == 0 && !File.Exists(directoriocache+"/version.gr3d")) {
           
                creararchivosyagregar(consolelist);
            }
            else
            if(listillaxx.Count==0)
            {
                var verlocal = int.Parse(File.ReadAllText(directoriocache + "/version.gr3d").Trim());
                var verserver = int.Parse(Intent.GetStringExtra("ver"));
                if (verserver > verlocal)
                {
                    creararchivosyagregar(consolelist);
                }
                else
                {

                    foreach (var elemzz in consolelist)
                    {
                        listillaxx.Add(File.ReadAllText(directoriocache + "/" + elemzz + ".gr3d"));
                    }

                }
            }

            var todo = "";
            if (console == 100)
            {
                foreach (var ax in listillaxx)
                {


                    var nombreses = ax.Split(new[] { "+++++" }, StringSplitOptions.None)[0].Split(new[] { "****" }, StringSplitOptions.None);
                    var linkeses = ax.Split(new[] { "+++++" }, StringSplitOptions.None)[1].Split(new[] { "****" }, StringSplitOptions.None);
                    var portadases = ax.Split(new[] { "+++++" }, StringSplitOptions.None)[2].Split(new[] { "****" }, StringSplitOptions.None);
                    var descargases = ax.Split(new[] { "+++++" }, StringSplitOptions.None)[3].Split(new[] { "****" }, StringSplitOptions.None);

                    for (int i = 0; i < nombreses.Length; i++)
                    {
                        Models.romsinfos modelito = new Models.romsinfos();
                        modelito.nombre = nombreses[i];
                        modelito.link = linkeses[i];
                        modelito.imagen = portadases[i];
                        modelito.descargas = descargases[i];
                        listaelementos.Add(modelito);
                        modelito = new Models.romsinfos();
                    }

                    nombreses = null;
                    linkeses = null;
                    portadases = null;
                    descargases = null;
                }

            }
            else {

                todo = listillaxx[console];
                var nombreses = todo.Split(new[] { "+++++" }, StringSplitOptions.None)[0].Split(new[] { "****" }, StringSplitOptions.None);
                var linkeses = todo.Split(new[] { "+++++" }, StringSplitOptions.None)[1].Split(new[] { "****" }, StringSplitOptions.None);
                var portadases = todo.Split(new[] { "+++++" }, StringSplitOptions.None)[2].Split(new[] { "****" }, StringSplitOptions.None);
                var descargases = todo.Split(new[] { "+++++" }, StringSplitOptions.None)[3].Split(new[] { "****" }, StringSplitOptions.None);

                for (int i = 0; i < nombreses.Length; i++)
                {
                    Models.romsinfos modelito = new Models.romsinfos();
                    modelito.nombre = nombreses[i];
                    modelito.link = linkeses[i];
                    modelito.imagen = portadases[i];
                    modelito.descargas = descargases[i];
                    listaelementos.Add(modelito);
                    modelito = new Models.romsinfos();
                }
                nombreses = null;
                linkeses = null;
                portadases = null;
                descargases = null;

            }

          

            todo = "";

          //  listaelementos = listaelementos.OrderBy(c => c.nombre).ToList();
            var adaptadolllll = new adaptadorroms(this, listaelementos,idimagen);
            RunOnUiThread(() => listin.Adapter = adaptadolllll);
           RunOnUiThread(()=> dialogoprogreso.Dismiss());
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

        public void descargar(string id,string nombre) {

            
            var ruta = Android.OS.Environment.ExternalStorageDirectory + "/neonromerdownloads";
            if (!Directory.Exists(ruta))
                Directory.CreateDirectory(ruta);
            var link = new superscraper().getdownloadlink(id).Result;
            var manige = DownloadManager.FromContext(this);
            var requ = new DownloadManager.Request(Android.Net.Uri.Parse(link));
            requ.SetDescription("Espere por favor");
            requ.SetNotificationVisibility(DownloadVisibility.VisibleNotifyCompleted);
            requ.SetTitle(nombre);
            var destino = Android.Net.Uri.FromFile(new Java.IO.File(ruta+"/"+nombre+".zip"));
            requ.SetDestinationUri(destino);
            requ.SetVisibleInDownloadsUi(true);
            manige.Enqueue(requ);
            RunOnUiThread(() => Toast.MakeText(this, "Descarga iniciada!!", ToastLength.Long).Show());

        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    sidem.OpenDrawer(Android.Support.V4.View.GravityCompat.Start);
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

    
	}
}

