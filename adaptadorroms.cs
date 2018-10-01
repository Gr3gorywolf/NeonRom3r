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
    class adaptadorroms : BaseAdapter
    {

        Context context;
        public List<Models.romsinfos> lista = new List<Models.romsinfos>();
       public int[] consolesources = null;
        int src = 0;
        public adaptadorroms(Context context, List<Models.romsinfos> list,int srcid,int[]consolesourcess=null)
        {
            lista = list;
            src = srcid;
            consolesources = consolesourcess;
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
            adaptadorromsViewHolder holder = null;
           
                if (view != null)
                holder = view.Tag as adaptadorromsViewHolder;
           
                if (holder == null)
            {
                holder = new adaptadorromsViewHolder();
                var inflater = context.GetSystemService(Context.LayoutInflaterService).JavaCast<LayoutInflater>();
                //replace with your item and your holder items
                //comment back in
                view = inflater.Inflate(Resource.Layout.layoutbuscadorconcarta, parent, false);
                holder.Title = view.FindViewById<TextView>(Resource.Id.textView);
                holder.Title2 = view.FindViewById<TextView>(Resource.Id.textView2);
                holder.portrait= view.FindViewById<ImageView>(Resource.Id.imageView);
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
           
            return view;

        }

        //Fill in cound here, currently 0
        public override int Count
        {
            get
            {
                return lista.Count ;
            }
        }

    }

    class adaptadorromsViewHolder : Java.Lang.Object
    {
        //Your adapter views to re-use
        //public TextView Title { get; set; }
        public TextView Title { get; set; }
        public TextView Title2 { get; set; }
    
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