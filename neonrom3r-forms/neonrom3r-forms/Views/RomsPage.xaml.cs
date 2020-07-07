using neonrom3r.forms.Models;
using neonrom3r.forms.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace neonrom3r.forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RomsPage : ContentPage
    {
        public RomsPage()
        {
            InitializeComponent();
            consConsoles.onSelected += (ConsoleItem cons) =>
            {
                Navigation.PushAsync(new RomsPage(cons));
            };
            lstRoms.IsVisible = false;
           
        }
        public RomsPage(ConsoleItem console)
        {
            InitializeComponent();
            consConsoles.IsVisible = false;
            Title = $"Roms de {console.Name}";
            lstRoms.ItemsSource = new RomsHelpers().GetRoms(console);
            lstRoms.ItemSelected += (send, obj) =>
            {
                if(obj.SelectedItem != null)
                {
                    Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new RomDetailsPage((RomItem)obj.SelectedItem),false);
                }
                lstRoms.SelectedItem = null;
            };
        }
        protected override void OnAppearing()
        { 
            base.OnAppearing();
        }
    }
}