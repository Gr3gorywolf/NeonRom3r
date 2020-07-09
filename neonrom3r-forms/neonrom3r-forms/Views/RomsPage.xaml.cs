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
            stkRoms.IsVisible = false;
            consConsoles.onSelected += (ConsoleItem cons) =>
            {
                Navigation.PushAsync(new RomsPage(cons));
            };
        }
        public RomsPage(ConsoleItem console)
        {
            InitializeComponent();
            consConsoles.IsVisible = false;
            Title = $"{console.Name} Roms";
            var romList = new RomsHelpers().GetRoms(console);
            srcSearch.TextChanged += (obj, send) =>
            {
                lstRoms.ItemsSource = romList.Where(ax => ax.Name.ToLower().Contains(send.NewTextValue.ToLower())).ToList();
            };
            lstRoms.ItemsSource = romList;
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