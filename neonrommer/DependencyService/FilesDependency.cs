using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using neonrom3r.forms.DependencyServices;
using neonrom3r.forms.Models;
using SharpCompress.Archives;
using SharpCompress.Common;
using Android.Net;
using Android.OS;
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
                cliente.DownloadFile("https://raw.githubusercontent.com/Gr3gorywolf/NeonRom3r/master/Updates/update.zip", cachePath + "/update.zip");
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
       
        public string DownloadRom(Rom rom,string downloadPath = null)
        {
            var inst = FormsActivity.GetInstance();
            string defaultPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDownloads);
            var path = downloadPath != null? downloadPath : defaultPath;
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch(Exception ex)
                {
                    return null;
                }
            }
            
            var fileName = Path.GetFileName(WebUtility.UrlDecode(rom.DownloadLink).Replace(" ", ""));
            if (!Directory.Exists(GetRootPath()))
                Directory.CreateDirectory(GetRootPath());
            var link = rom.DownloadLink;
            var dManager = DownloadManager.FromContext(inst);
            var dRequest = new DownloadManager.Request(Android.Net.Uri.Parse(WebUtility.UrlDecode(link)));
            dRequest.SetDescription("Espere por favor");
            dRequest.SetNotificationVisibility(DownloadVisibility.VisibleNotifyCompleted);
            dRequest.SetTitle(rom.Name);
            var filePath = $"{path}/{fileName}";
            Android.Net.Uri destination = Android.Net.Uri.FromFile(new Java.IO.File(filePath));
            dRequest.SetDestinationUri(destination);
            dRequest.SetVisibleInDownloadsUi(true);
            dManager.Enqueue(dRequest);
            inst.RunOnUiThread(() => Toast.MakeText(inst, "Descarga iniciada!!", ToastLength.Long).Show());
            return filePath;
        }
    }
}