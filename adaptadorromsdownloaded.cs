using System.Text;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;
using Android.Graphics;
using System.Net;
using System.Threading;
//using Square.Picasso;
using Android.Glide;
using Android.Glide.Request;
using System;

namespace neonrommer
{
    class adaptadorromsdownloaded : BaseAdapter
    {

        Context context;
        public List<Models.romsinfos> lista = new List<Models.romsinfos>();
        public List<string> down = new List<string>();
        public int[] consolesources = null;
        int pos = 0;
        int src = 0;
        public adaptadorromsdownloaded(Context context, List<Models.romsinfos> list,int srcid,int[]consolesourcess=null,List<string>downloadlinks=null)
        {
            lista = list;
            src = srcid;
            consolesources = consolesourcess;
            down = downloadlinks;
            this.context = context;
        }


        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {

           
            var view = convertView;
            adaptadorromsdownloadedViewHolder holder = null;
           
                if (view != null)
                holder = view.Tag as adaptadorromsdownloadedViewHolder;
           
                if (holder == null)
            {
                holder = new adaptadorromsdownloadedViewHolder();
                var inflater = context.GetSystemService(Context.LayoutInflaterService).JavaCast<LayoutInflater>();
                //replace with your item and your holder items
                //comment back in
                view = inflater.Inflate(Resource.Layout.layoutbuscadorconmenu, parent, false);
                holder.Title = view.FindViewById<TextView>(Resource.Id.textView);
                holder.Title2 = view.FindViewById<TextView>(Resource.Id.textView2);
                holder.portrait= view.FindViewById<ImageView>(Resource.Id.imageView);
                holder.menutool = view.FindViewById<ImageView>(Resource.Id.imageView2);
                miselaneousmethods.ponerfuente(context.Assets, holder.Title);
                miselaneousmethods.ponerfuente(context.Assets, holder.Title2);
                int idd = 0;
                if (consolesources != null)
                {
                    idd = consolesources[position];
                }
                else
                {
                    idd = src;
                }
                holder.menutool.Click += (aax, asdd)=>
                {


                    PopupMenu menu = new PopupMenu(context, holder.menutool);
                    menu.Inflate(Resource.Menu.menupopup);
                    menu.Show();
               
                    menu.MenuItemClick += (s1, arg1) =>
                    {


                        pos = (int)(((ImageView)aax).GetTag(Resource.Id.imageView2));
                     
                        /*   intentsend.PutExtra(Intent.ExtraTitle, "Link de descarga para el rom:" + nombre);
                           intentsend.PutExtra(Intent.ExtraSubject, "Link de descarga para el rom:" + nombre);*/

                        AlertDialog.Builder ad = new AlertDialog.Builder(context);
                        ad.SetCancelable(false);
                        ad.SetTitle("Que desea compartir?");
                        ad.SetMessage("Desea compartir el archivo descargado o el link de descarga");
                        ad.SetPositiveButton("Archivo", ok);
                        ad.SetNegativeButton("Link", no);
                        ad.Show();

                       
                    };


                    
                    };
                Glide.With(context)
                .Load(lista[position].imagen)
                .Apply(RequestOptions.NoTransformation().SkipMemoryCache(true).Override(75, 75).Placeholder(idd))
                .Into(holder.portrait);
                holder.portrait.SetTag(Resource.Id.imageView, lista[position].imagen);
                view.Tag = holder;
               
            }
            else {
                holder.animar3(view);
            }





            if (holder.portrait.GetTag(Resource.Id.imageView).ToString() != lista[position].imagen)
            {
                try
                {
                    int idd = 0;
                    if (consolesources != null)
                    {
                        idd = consolesources[position];
                    }
                    else {
                        idd = src;
                    }
                        Glide.With(context)
                      .Load(lista[position].imagen)
                  
                     
                       .Apply(RequestOptions.NoTransformation().SkipMemoryCache(true).Override(75,75)
                     
                       .Placeholder(idd))

                       .Into(holder.portrait);
                   
                }
                catch (Exception) { }

            }




            //fill in your items
            //holder.Title.Text = "new text here";
            holder.Title.Text = lista[position].nombre;
            holder.Title2.Text = lista[position].descargas;
            holder.portrait.SetTag(Resource.Id.imageView, lista[position].imagen);
            holder.menutool.SetTag(Resource.Id.imageView2,position);
            return view;

        }

        //Fill in cound here, currently 0

        void ok (object sender, EventArgs e) {
            Intent intentsend = new Intent();
            StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
            StrictMode.SetVmPolicy(builder.Build());
            builder.DetectFileUriExposure();
            intentsend.SetAction(Intent.ActionSend);
          
            intentsend.PutExtra(Intent.ExtraStream, Android.Net.Uri.Parse("file://" + lista[pos].link));
            intentsend.SetType("application/zip");
          
            context.StartActivity(Intent.CreateChooser(intentsend, "Compartir a?"));

        }
        void no(object sender, EventArgs e)
        {
            Intent intentsend = new Intent();
            intentsend.SetAction(Intent.ActionSend);
            intentsend.PutExtra(Intent.ExtraText, "Link de descarga para el rom:" + lista[pos].nombre + "\n" + down[pos].Replace(" ", "%20") + "\n Compartido desde:NeonRom3r");
            intentsend.SetType("text/plain");
            context.StartActivity(Intent.CreateChooser(intentsend, "Compartir a travez de?"));

        }
        public override int Count
        {
            get
            {
                return lista.Count ;
            }
        }

    }

    class adaptadorromsdownloadedViewHolder : Java.Lang.Object
    {
        //Your adapter views to re-use
        //public TextView Title { get; set; }
        public TextView Title { get; set; }
        public TextView Title2 { get; set; }
        public ImageView menutool { get; set; }
        public ImageView portrait { get; set; }
        public void animar3(View imagen)
        {
            imagen.SetLayerType(LayerType.Hardware, null);
            Android.Animation.ObjectAnimator animacion = Android.Animation.ObjectAnimator.OfFloat(imagen, "alpha", 0f, 1f);
            animacion.SetDuration(150);
            animacion.Start();
            animacion.AnimationEnd += delegate
            {
                imagen.SetLayerType(LayerType.None, null);
            };

        }

    }
}