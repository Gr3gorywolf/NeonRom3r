using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.Threading;
using Firebase.Database;
using Android.Content.PM;
using Android.Runtime;
using System.Collections.Generic;
using System.IO;
using System;
using Newtonsoft.Json;
namespace neonrommer
{
    [Activity(Label = "NeonRom3r", Theme = "@style/Theme.DesignDemo", MainLauncher = true, ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class actsplashcreen : Activity
    {
        ImageView logo;
        string verstring = "";
        int  segs = 0;
        bool envioklk = false;
        string directoriocache = Android.OS.Environment.ExternalStorageDirectory + "/.romercache";
        // bool terminada = false;
        Android.Animation.ObjectAnimator animacion;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.splashcreen);

            ServicePointManager.ServerCertificateValidationCallback += new System.Net.Security.RemoteCertificateValidationCallback(ValidateRemoteCertificate);
          

            logo = FindViewById<ImageView>(Resource.Id.portada2);

            animar3(logo);

            new Thread(() =>
            {
                if (CheckInternetConnection())
                {
                    List<string> arraydatos = new List<string>();
                    arraydatos.Add(Android.Manifest.Permission.ReadExternalStorage);
                    arraydatos.Add(Android.Manifest.Permission.WriteExternalStorage);
                    if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
                    {
                        RunOnUiThread(() =>
                        {
                            RequestPermissions(arraydatos.ToArray(), 0);
                        });
                    }
                    else
                    {
                        enviarvainilla();
                    }


                    verstring = new WebClient().DownloadString("https://gr3gorywolf.github.io/getromdownload/version.gr3v");
                    new Thread(() => { poner(); }).Start();

                }
                else
                {
                    RunOnUiThread(() =>
                    {
                        if (File.Exists(miselaneousmethods.archivoregistro))
                            new AlertDialog.Builder(this).SetTitle("No se pudo conectar a el servidor").SetMessage("Puede que no haya conexion a internet o los servidores estén en mantenimiento. Usted puede entrar a la app en el modo offline en el cual podrá usar solo ciertas funciones").SetCancelable(false).SetNegativeButton("Cerrar", ok).SetPositiveButton("Abrir en modo offline", off).Create().Show();
                        else
                            new AlertDialog.Builder(this).SetTitle("No se pudo conectar a el servidor").SetMessage("Puede que no haya conexion a internet o los servidores estén en mantenimiento. El modo offline se habilitará cuando usted tenga descargado almenos 1 rom").SetCancelable(true).SetPositiveButton("Ok", ok).Create().Show();
                    });
                    }
            }).Start();

            // Create your application here
        }
        public void ok(object sender, EventArgs e)
        {

            this.Finish();
        }
        public void off(object sender, EventArgs e)
        {
            
            Intent intento = new Intent(this, typeof(MainActivity));
            intento.PutExtra("online", false);
            StartActivity(intento);
            this.Finish();
        }
        public async void enviarvainilla() {

            if (!File.Exists(directoriocache + "/paths.json"))
            {

                Dictionary<string, string> dic = new Dictionary<string, string>();
                string downloadpath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDownloads);
                foreach (var axd in miselaneousmethods.consolelist)
                {
                    dic.Add(axd, downloadpath);

                }
                if (!Directory.Exists(directoriocache))
                    Directory.CreateDirectory(directoriocache);

                var xdd = File.CreateText(directoriocache + "/paths.json");
                xdd.Write(JsonConvert.SerializeObject(dic));
                xdd.Close();

              

            }


            if (!File.Exists(directoriocache + "/version.gr3d")) {
                string teto = "Descargada";
                var firebase = new FirebaseClient("<your firebase url>");
            await firebase.Child("Descargas/"+ miselaneousmethods.getrandomserial()).PutAsync(JsonConvert.SerializeObject(teto));

               
                envioklk = true;


            }
            else {
                envioklk = true;
                }

            if (!Directory.Exists(directoriocache)) {
                Directory.CreateDirectory(directoriocache);
                }
            var axdxx = File.CreateText(directoriocache + "/appver");
            axdxx.Write("1");
            axdxx.Close();

        }

        public void poner() {
         
            while (true) {

                if (verstring != "" && segs>5 && envioklk) {


                    RunOnUiThread(() =>
                    {


                        Intent intento = new Intent(this, typeof(MainActivity));
                        intento.PutExtra("ver", verstring);
                        intento.PutExtra("online", true);
                        StartActivity(intento);
                        this.Finish();
                    });
                    break;
                }
                segs++;
                Thread.Sleep(1000);
            }

        }

       
        private static bool ValidateRemoteCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyErrors)
        {
            return true;
        }
        protected override void OnDestroy()
        {

            animacion.Cancel();
            base.OnDestroy();
        }
        public void animar3(View imagen)
        {
            imagen.SetLayerType(LayerType.Hardware, null);
           animacion= Android.Animation.ObjectAnimator.OfFloat(imagen, "alpha", 0.2f, 1f);
            animacion.SetDuration(1000);
            animacion.RepeatCount = int.MaxValue-30;
            animacion.RepeatMode = Android.Animation.ValueAnimatorRepeatMode.Reverse;
            animacion.Start();
            animacion.AnimationEnd += delegate
            {
               
            };

        }
        public bool CheckInternetConnection()
        {
            string CheckUrl = "https://emulator.games/";

            try
            {
                HttpWebRequest iNetRequest = (HttpWebRequest)WebRequest.Create(CheckUrl);

                iNetRequest.Timeout =35000;

                WebResponse iNetResponse = iNetRequest.GetResponse();

                // Console.WriteLine ("...connection established..." + iNetRequest.ToString ());
                iNetResponse.Close();

                return true;

            }
            catch (WebException)
            {

                // Console.WriteLine (".....no connection..." + ex.ToString ());

                return false;
            }
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {

            foreach(var permi in grantResults) {
                if (permi == Permission.Denied)
                {
                    new AlertDialog.Builder(this).SetTitle("Error").SetMessage("No se han aceptado los permisos esto podria causar problemas en la aplicacion por favor aceptelos").SetPositiveButton("Ok", ok).Create().Show();

                }
                else {
                    enviarvainilla();
                }
              
                }
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }



    }

    

}