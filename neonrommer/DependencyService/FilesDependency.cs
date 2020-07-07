using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using neonrom3r.forms.DependencyServices;
using SharpCompress.Archives;
using SharpCompress.Common;

[assembly: Xamarin.Forms.Dependency(typeof(neonrommer.DependencyService.FilesDependency))]
namespace neonrommer.DependencyService
{
    class FilesDependency : IFilesDependency
    {
        public string GetRootPath()
        {
            return Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
        }

       public  bool DownloadUpdateBundle(string cachePath)
        {
            var cliente = new WebClient();

            if (!Directory.Exists(cachePath))
                Directory.CreateDirectory(cachePath);

            try
            {
                cliente.DownloadFile(new Uri("https://raw.githubusercontent.com/Gr3gorywolf/NeonRom3r/master/Updates/update.zip"), cachePath + "/update.zip");
            }
            catch (Exception ex)
            {
                return false;
            }
            
            var archive = ArchiveFactory.Open(cachePath + "/update.zip");
            foreach (var entry in archive.Entries)
            {
                entry.WriteToDirectory(cachePath, new ExtractionOptions() { Overwrite = true });

            }
            File.Delete(cachePath + "/update.zip");
            return true;

        }

    }
}