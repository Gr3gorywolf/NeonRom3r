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
    class Models
    {
        public class romsinfos
        {
            public string nombre { get; set; }
            public string imagen { get; set; }
            public string descargas { get; set; }
            public string link { get; set; }
        };
        public class rominfo
        {
            public string id { get; set; }
            public string nombre { get; set; }
            public string imagen { get; set; }
            public string descargas { get; set; }
            public string linkdescarga { get; set; }
            public string consola { get; set; }
            public string region { get; set; }
            public string votos { get; set; }
            public string size { get; set; }

        }

        public class emuladores {

            public string nombre { get; set; }
            public string imagen { get; set; }
            public string link { get; set; }
            public string compactible { get; set; }
        }
        public class registry
        {

            public string nombre { get; set; }
            public string path { get; set; }
            public string consola { get; set; }
            public string portadalink { get; set; }
            public string linkdescarga { get; set; }

        }

    }
}