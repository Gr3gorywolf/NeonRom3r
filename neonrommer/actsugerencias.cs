using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Firebase.Database;
using Newtonsoft.Json;
using System.Threading;
namespace neonrommer
{
    [Activity(Label = "NeonRommer", Theme = "@style/Theme.DesignDemo", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class actsugerencias : AppCompatActivity
    {
        public ImageView logo;
        public EditText nombre;
        public EditText titulo;
        public EditText mensaje;
        public Button button;
#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
        ProgressDialog dialogoprogreso;
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos
        Android.Animation.ObjectAnimator animacion;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Sugerencias);
            logo = FindViewById<ImageView>(Resource.Id.portada2);
            nombre = FindViewById<EditText>(Resource.Id.Nombre);
            titulo = FindViewById<EditText>(Resource.Id.titulo);
            mensaje = FindViewById<EditText>(Resource.Id.Mensaje);
           button = FindViewById<Button>(Resource.Id.enviar);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.my_toolbar);
            SetSupportActionBar(toolbar);
            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.home);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            SupportActionBar.Title = "Envie una opinion/sugerencia";
            SupportActionBar.SetBackgroundDrawable(new Android.Graphics.Drawables.ColorDrawable(Android.Graphics.Color.ParseColor("#2b2e30")));
            animar3(logo);

            button.Click += delegate
            {
                new Thread(() => { enviar(); }).Start();
              
            };

        // Create your application here
    }



        public async void enviar() {

            RunOnUiThread(() =>
            {

                #pragma warning disable CS0618 // El tipo o el miembro están obsoletos
                dialogoprogreso = new ProgressDialog(this);
                #pragma warning restore CS0618 // El tipo o el miembro están obsoletos
                dialogoprogreso.SetCanceledOnTouchOutside(false);
                dialogoprogreso.SetCancelable(false);
                dialogoprogreso.SetTitle("Enviando sugerencia...");
                dialogoprogreso.SetMessage("Por favor espere");
                dialogoprogreso.Show();
            });
            if (nombre.Text.Trim().Length >= 5 && titulo.Text.Trim().Length >= 5 && mensaje.Text.Trim().Length >= 5)
            {
                var datos = new Dictionary<string, string>();
         
                  datos.Add("Nombre", nombre.Text);
                  datos.Add("Titulo", titulo.Text);
                  datos.Add("Mensaje", mensaje.Text);
          
                string datastr = JsonConvert.SerializeObject(datos);

                var firebase = new FirebaseClient("https://neonrom3r-suggestions.firebaseio.com");


                await firebase.Child("Sugerencias/"+ miselaneousmethods.getrandomserial()).PutAsync(datastr);
             
                RunOnUiThread(() => {
                    dialogoprogreso.Dismiss();
                    Toast.MakeText(this, "Gracias por enviar su sugerencia la tendre pendiente", ToastLength.Long).Show();
                    nombre.Text = "";
                    titulo.Text = "";
                    mensaje.Text = "";

                });

            }
            else {
           RunOnUiThread(()=>
           {
               Toast.MakeText(this, "Cada campo debe contener almenos 5 caracteres", ToastLength.Long).Show();
               dialogoprogreso.Dismiss();
           }
               );
            }

        }

        public void animar3(View imagen)
        {
            imagen.SetLayerType(LayerType.Hardware, null);
            animacion = Android.Animation.ObjectAnimator.OfFloat(imagen, "alpha", 0.2f, 1f);
            animacion.SetDuration(1000);
            animacion.RepeatCount = int.MaxValue - 30;
            animacion.RepeatMode = Android.Animation.ValueAnimatorRepeatMode.Reverse;
            animacion.Start();
            animacion.AnimationEnd += delegate
            {

            };

        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    this.Finish();
                    return true;
               
            }
            return base.OnOptionsItemSelected(item);
        }
        protected override void OnDestroy()
        {

            animacion.Cancel();
            base.OnDestroy();
        }


    }
}