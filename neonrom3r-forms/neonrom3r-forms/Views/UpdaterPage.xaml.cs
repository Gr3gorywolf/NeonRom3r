using neonrom3r.forms.DependencyServices;
using neonrom3r.forms.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace neonrom3r.forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdaterPage : ContentPage
    {
        int ServerVersion = 0;
        public UpdaterPage(int serverVersion)
        {
            InitializeComponent();
            this.ServerVersion = serverVersion;
            var thread = new Thread(new ThreadStart(() =>
            {
                RunUpdate();
            }));
            thread.Start();

        }

        public void RunUpdate()
        {
            var isSuccess = DependencyService.Get<IFilesDependency>().DownloadUpdateBundle(Constants.CachePath);
            if (!isSuccess)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                  var result = await  DisplayAlert("Error al descargar la actualizacion", "Por favor verifique que tenga una conexion a internet activa y vuelva a intentarlo", "Reintentar", "Cerrar aplicacion");
                    if (result)
                    {
                        RunUpdate();
                    }
                    else
                    {
                        System.Environment.Exit(0);
                    }
                });
            }
            else
            {
                var writer = File.CreateText(Constants.CachePath + "/version.gr3d");
                writer.Write(ServerVersion);
                writer.Close();
                Device.BeginInvokeOnMainThread(() =>
                {
                    App.AppInstance.LoadApp();
                });
            } 
        }
    }
}