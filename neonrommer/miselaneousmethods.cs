using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace neonrommer
{
    class miselaneousmethods
    {


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
        public static void ponerfuente2(AssetManager mgr, TextView texto)
        {

            /////////////////usa una fuente externa en formato ttfs y la convierte a nativa y se la aplica a cualquier textview
            Typeface tipo = Typeface.CreateFromAsset(mgr, "GeoOblique.ttf");
            texto.SetTypeface(tipo, TypefaceStyle.Normal);

        }

    }
}