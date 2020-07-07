using DLToolkit.Forms.Controls;
using neonrom3r.forms.DependencyServices;
using neonrom3r.forms.Utils;
using neonrom3r.forms.Views;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace neonrom3r.forms
{
    public partial class App : Application
    {
        public static App AppInstance = null;
        public App(string AppVersion = null)
        {
           
            InitializeComponent();
            FlowListView.Init();
            AppInstance = this;
            int localVer = -1;
            int serverVer = AppVersion != null ? int.Parse(AppVersion) : 0;
            if (File.Exists(Constants.VersionFile))
            {
                localVer = int.Parse(File.ReadAllText(Constants.VersionFile).Trim());
            }
            if(serverVer > localVer)
            {
                MainPage = new NavigationPage(new UpdaterPage(serverVer));
            }
            else
            {
                LoadApp();
            }  
        }

        public void LoadApp()
        {
            MainPage = new Views.MainMenu.MainMenu();
        }

        protected override void OnStart()
        {
           
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
