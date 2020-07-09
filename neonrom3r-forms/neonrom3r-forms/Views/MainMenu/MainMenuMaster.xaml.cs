using neonrom3r.forms.Models;
using neonrom3r.forms.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace neonrom3r.forms.Views.MainMenu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenuMaster : ContentPage
    {
        public ListView ListView;
        public MainMenuMaster()
        {
            InitializeComponent();
            BindingContext = new MainMenuMasterViewModel();
            ListView = PagesList;
        }
        protected override void OnAppearing()
        {
            /*  var pages = new List<MasterPageItem>();
              pages.Add();
              PagesList.ItemsSource = null;
              PagesList.ItemsSource = pages;*/
            base.OnAppearing();
        }

        public class MainMenuMasterViewModel : INotifyPropertyChanged
        {


            public ObservableCollection<MasterPageItem> MenuItems { get; set; }

            public void ReloadItems()
            {
                var items = new ObservableCollection<MasterPageItem>(new[]
                {
                    new MasterPageItem(){Id = 0,Title = "Downloaded roms",Icon = "download.png", TargetType = typeof(DownloadedRoms)},
                    new MasterPageItem(){Id = 1,Title = "Emulators",Icon = "arcade.png", TargetType = typeof(EmulatorsPage)},
                    new MasterPageItem(){Id = 2,Title = "Roms",Icon = "gamepad.png", TargetType = typeof(RomsPage)},
                    new MasterPageItem(){Id = 3,Title = "Leave a feedback",Icon = "messagealert.png", TargetType = typeof(FeedbackPage)},
                    new MasterPageItem(){Id = 4,Title = "Settings",Icon = "messagealert.png", TargetType = typeof(SettingsPage)},

                });
                MenuItems = items;
                OnPropertyChanged(nameof(MenuItems));
            }

            public MainMenuMasterViewModel()
            {
                ReloadItems();
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}