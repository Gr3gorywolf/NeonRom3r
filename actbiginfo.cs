using System.Threading;
using Android.App;
using Android.Content;
using Android.Glide;
using Android.Glide.Request;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using emulatorgamessuperscraper;
using System.IO;
using static Android.Icu.Text.DateFormat;
using System.Net;
namespace neonrommer
{
    [Activity(Label = "NeonRommer", Theme = "@style/Theme.DesignDemo", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class actbiginfo : AppCompatActivity
    {
        public bool initialized = false;
        public string romid;
        public string nombre;
        string imagen;
        public TextView consola;
        public TextView estrella;
        public TextView size;
        public TextView region;
        public TextView descargas;
        public TextView titulo;
        public ImageView portada;
        public string imgcache = Android.OS.Environment.ExternalStorageDirectory + "/.romercache/catched.jpg";
#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
        public ProgressDialog dialogoprogreso;
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos
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

            var link = new superscraper().getdownloadlink(romid).Result;
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
            var ruta = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDownloads); 
            if (!Directory.Exists(ruta))
                Directory.CreateDirectory(ruta);
            var link = new superscraper().getdownloadlink(romid).Result;
            var manige = DownloadManager.FromContext(this);
            var requ = new DownloadManager.Request(Android.Net.Uri.Parse(link.Replace(" ","%20")));
            requ.SetDescription("Espere por favor");
            requ.SetNotificationVisibility(DownloadVisibility.VisibleNotifyCompleted);
            requ.SetTitle(nombre);
            var destino = Android.Net.Uri.FromFile(new Java.IO.File(ruta + "/" + nombre + ".zip"));
            requ.SetDestinationUri(destino);
            requ.SetVisibleInDownloadsUi(true);
            manige.Enqueue(requ);
            dialogoprogreso.Dismiss();
            RunOnUiThread(() => Toast.MakeText(this, "Descarga iniciada!!", ToastLength.Long).Show());

        }

        public void ponerinfo(string linkinfo) {
            RunOnUiThread(() =>
            {

#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
                dialogoprogreso = new ProgressDialog(this);
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos
                dialogoprogreso.SetCanceledOnTouchOutside(false);
                dialogoprogreso.SetCancelable(false);
                dialogoprogreso.SetTitle("Obteniendo informacion de el rom...");
                dialogoprogreso.SetMessage("Por favor espere");
                dialogoprogreso.Show();
            });
            superscraper escrapeador = new superscraper();
            var objeto =escrapeador.getrominfo(linkinfo).Result;
            romid = objeto.id;
            nombre = objeto.nombre;
            imagen = objeto.imagen;
            RunOnUiThread(() =>
            {
                consola.Text = objeto.consola;
                if (objeto.votos.Contains("5")) {
                    estrella.Text = objeto.votos;
                }
                else {
                    estrella.Text = "0 De 5";
                }
                
                region.Text = objeto.region;
                descargas.Text = objeto.descargas;
                size.Text = objeto.size;
                titulo.Text = objeto.nombre;
                titulo.Selected = true;
               
                Glide.With(this)
                .Load(objeto.imagen)
                 .Apply(RequestOptions.NoTransformation().Placeholder(Intent.GetIntExtra("placeholder",0)))
                 .Into(portada);
                dialogoprogreso.Dismiss();
            });
           
            

        }
        protected override void OnDestroy()
        {

            Glide.Get(this).ClearMemory();
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