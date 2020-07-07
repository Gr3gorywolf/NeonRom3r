using System.Threading;
using Android.App;
using Android.Content;
using Android.Glide;
//using Android.Glide.Request;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

using System.IO;
using static Android.Icu.Text.DateFormat;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;

namespace neonrommer
{
    [Activity(Label = "NeonRommer", Theme = "@style/Theme.DesignDemo", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class actbiginfo : AppCompatActivity
    {
        public bool initialized = false;
        public string romid;
        public string nombre;
        string imagen;
        string consolaa;
        public TextView consola;
        public TextView estrella;
        public TextView size;
        public TextView region;
        public TextView descargas;
        public TextView titulo;
        public ImageView portada;
        public string imgcache = Android.OS.Environment.ExternalStorageDirectory + "/.romercache/catched.jpg";
        public Dictionary<string, string> dicciopath = new Dictionary<string, string>();
#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
        public ProgressDialog dialogoprogreso;
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos
      public static  actbiginfo contexto;
        public string downloadlink = "";
        public string[] consolelistformal = { "Gameboy", "Gameboy Color", "Gameboy Advance", "Nintendo", "Super Nintendo", "Nintendo 64", "Playstation", "Sega Genesis", "Sega Dreamcast", "Nintendo DS" };
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.layoutbiginfo);
            consola = FindViewById<TextView>(Resource.Id.consola);
            estrella = FindViewById<TextView>(Resource.Id.estrella);
            size = FindViewById<TextView>(Resource.Id.tamano);
            descargas= FindViewById<TextView>(Resource.Id.descargar);
            portada = FindViewById<ImageView>(Resource.Id.portada);
            region = FindViewById<TextView>(Resource.Id.region);
            titulo = FindViewById<TextView>(Resource.Id.toolbar_title);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.my_toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.home);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.Title = "";
            SupportActionBar.SetBackgroundDrawable(new ColorDrawable(Android.Graphics.Color.ParseColor("#2b2e30")));
            miselaneousmethods.ponerfuente(Assets, consola);
            miselaneousmethods.ponerfuente(Assets, estrella);
            miselaneousmethods.ponerfuente(Assets, size);
            miselaneousmethods.ponerfuente(Assets, descargas);
            miselaneousmethods.ponerfuente(Assets, region);
            miselaneousmethods.ponerfuente(Assets, titulo);
            if (File.Exists(imgcache)) {
                File.Delete(imgcache);
            }


            new Thread(() =>
            {
                ponerinfo(Intent.GetStringExtra("link"));

            }).Start();

           
            // Create your application here
            contexto = this;
        }
        public static actbiginfo gettearinstancia() {
            return contexto;
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            if (!initialized) {
                MenuInflater.Inflate(Resource.Menu.menutoolbar, menu);
                initialized = false;
            }
        
            return base.OnCreateOptionsMenu(menu);
        }

        /*   public override bool OnOptionsItemSelected(IMenuItem item)
           {
               string textToShow;

              /* if (item.ItemId == Resource.Id.)
                   textToShow = "Learn more about us on our website :)";
               else
                   textToShow = "Overfloooow";

               Android.Widget.Toast.MakeText(this, item.TitleFormatted + ": " + textToShow,
                   Android.Widget.ToastLength.Long).Show();

               return base.OnOptionsItemSelected(item);
           }*/
        public async void compartir() {
            RunOnUiThread(() =>
            {

#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
                dialogoprogreso = new ProgressDialog(this);
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos
                dialogoprogreso.SetCanceledOnTouchOutside(false);
                dialogoprogreso.SetCancelable(false);
                dialogoprogreso.SetTitle("Obteniendo link de descarga del rom  y portada...");
                dialogoprogreso.SetMessage("Por favor espere");
                dialogoprogreso.Show();
            });

            var link = downloadlink;
          RunOnUiThread(()=> {
              dialogoprogreso.Dismiss();
           //   var imagenuri = Android.Net.Uri.Parse(imagen);
            Intent intentsend = new Intent();
              intentsend.SetAction(Intent.ActionSend);
              /*   intentsend.PutExtra(Intent.ExtraTitle, "Link de descarga para el rom:" + nombre);
                 intentsend.PutExtra(Intent.ExtraSubject, "Link de descarga para el rom:" + nombre);*/
              intentsend.PutExtra(Intent.ExtraText, "Link de descarga para el rom:" + nombre +"\n"+ link.Replace(" ","%20")+"\n Compartido desde:NeonRom3r");
              intentsend.SetType("text/plain");
              StartActivity(Intent.CreateChooser(intentsend, "Compartir a travez de?"));
          }
            );
        }

        public void descargar()
        {

            RunOnUiThread(() =>
            {

#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
                dialogoprogreso = new ProgressDialog(this);
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos
                dialogoprogreso.SetCanceledOnTouchOutside(false);
                dialogoprogreso.SetCancelable(false);
                dialogoprogreso.SetTitle("Obteniendo link de descarga del rom...");
                dialogoprogreso.SetMessage("Por favor espere");
                dialogoprogreso.Show();
            });


            var ruta = dicciopath[miselaneousmethods.consolelist[Intent.GetIntExtra("consoleindex",0)]];
            if (!Directory.Exists(miselaneousmethods.cachepath))
                Directory.CreateDirectory(miselaneousmethods.cachepath);
            var link = downloadlink;
            var manige = DownloadManager.FromContext(this);
            var requ = new DownloadManager.Request(Android.Net.Uri.Parse(link.Replace(" ","%20")));
            requ.SetDescription("Espere por favor");
            requ.SetNotificationVisibility(DownloadVisibility.VisibleNotifyCompleted);
            requ.SetTitle(nombre);
            Android.Net.Uri destino =
                Android.Net.Uri.FromFile(new Java.IO.File(ruta + "/" +Path.GetFileName( System.Net.WebUtility.UrlDecode(downloadlink).Replace(" ",""))));          
            requ.SetDestinationUri(destino);
            requ.SetVisibleInDownloadsUi(true);
            manige.Enqueue(requ);
            dialogoprogreso.Dismiss();
            RunOnUiThread(() => Toast.MakeText(this, "Descarga iniciada!!", ToastLength.Long).Show());
            miselaneousmethods.guardarenregistry(nombre, destino.Path, imagen, miselaneousmethods.consolelistformal[Intent.GetIntExtra("consoleindex", 0)], link.Replace(" ", "%20"));
          //  miselaneousmethods.guardarenregistrydescargas(nombre, destino.Path, imagen, consola.Text, link.Replace(" ", "%20"));
            new Thread(() =>
            {
                try
                {
                    using (WebClient cliente = new WebClient())
                    {

                        if (!Directory.Exists(miselaneousmethods.imgpath))
                            Directory.CreateDirectory(miselaneousmethods.imgpath);
                        cliente.DownloadFile(imagen, miselaneousmethods.imgpath + "/" + nombre + ".jpg");
                    }
                }
                catch (Exception) { }
            }).Start();

    

        }

        public void ponerinfo(string linkinfo) {
            RunOnUiThread(() =>
            {

#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
                dialogoprogreso = new ProgressDialog(this);
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos
                dialogoprogreso.SetCanceledOnTouchOutside(false);
                dialogoprogreso.SetCancelable(false);
                dialogoprogreso.SetTitle("Obteniendo información del rom...");
                dialogoprogreso.SetMessage("Por favor espere");
                dialogoprogreso.Show();
            });
            try
            {
                dicciopath = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(miselaneousmethods.cachepath + "/paths.json"));
         
                var objeto = JsonConvert.DeserializeObject<Models.rominfo>(new WebClient().DownloadString(linkinfo));
                downloadlink = objeto.DownloadLink;
                nombre = objeto.Name;
                imagen = objeto.Portrait;
                RunOnUiThread(() =>
                {
                    consolaa = objeto.Console;
                    consola.Text = objeto.Console;


                    region.Text = objeto.Region;

                    size.Text = objeto.Size;
                    titulo.Text = objeto.Name;
                    titulo.Selected = true;

                  /* Glide.With(this)
                    .Load(objeto.Portrait)
                     .Apply(RequestOptions.NoTransformation().Placeholder(Intent.GetIntExtra("placeholder", 0)))
                     .Into(portada);*/
                    dialogoprogreso.Dismiss();
                });
            }
            catch (Exception) {

                RunOnUiThread(() =>
                {

                    dialogoprogreso.Dismiss();
                    Toast.MakeText(this, "Error al cargar este rom", ToastLength.Long).Show() ;
                });
            }



        }
        protected override void OnDestroy()
        {

           // Glide.Get(this).ClearMemory();
            System.GC.Collect(0);
            base.OnDestroy();
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    this.Finish();
                    return true;
                case Resource.Id.descargartool:
                    new Thread(()=> { descargar(); }).Start();
                    return true;
                case Resource.Id.compartir:
                    new Thread(() => { compartir(); }).Start();
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}