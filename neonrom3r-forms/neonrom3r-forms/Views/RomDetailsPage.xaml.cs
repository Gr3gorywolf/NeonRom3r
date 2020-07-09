using neonrom3r.forms.Models;
using neonrom3r.forms.Utils;
using neonrom3r.forms.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace neonrom3r.forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RomDetailsPage : Rg.Plugins.Popup.Pages.PopupPage
    {


        public RomDetailsViewModel ViewModel;
        public RomDetailsPage(RomItem rom)
        {
            InitializeComponent();
            this.ViewModel = new RomDetailsViewModel(rom);
            CloseWhenBackgroundIsClicked = false;
            this.BindingContext = this.ViewModel;
        }

        protected override void OnAppearing()
        {
            new Thread(new ThreadStart(() =>
            {
                ViewModel.LoadRom.Execute(null);
            })).Start() ;
            Utils.AnimationsHelper.SlideUpModal(stkContent);
            btnClose.Clicked += async delegate
             {
                 await AnimationsHelper.SlideDownModal(stkContent);
                 Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync(false);
             };
            base.OnAppearing();
        }
    }
}