using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace neonrommer
{
    [Activity(Label = "Neon Rom3r", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize, Theme = "@style/Theme.UserDialog")]
    public class actacercade : Activity
    {
        public TextView texto;
        public ImageView logo;
        Android.Animation.ObjectAnimator animacion;



        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
           SetContentView(Resource.Layout.acercade);
            texto = FindViewById<TextView>(Resource.Id.consola);
            logo = FindViewById<ImageView>(Resource.Id.portada2);
            miselaneousmethods.ponerfuente2(Assets,texto);
            animar3(logo);
            SetFinishOnTouchOutside(true);

            // Create your application here
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
        protected override void OnDestroy()
        {


            animacion.Cancel();
            base.OnDestroy();
        }
    }
}