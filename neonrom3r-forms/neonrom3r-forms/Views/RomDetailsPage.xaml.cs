using neonrom3r.forms.Models;
using neonrom3r.forms.Utils;
using neonrom3r.forms.ViewModels;
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
    public partial class RomDetailsPage : ContentPage
    {
       
       
        public RomDetailsPage(RomItem rom)
        {
            InitializeComponent();
            stkContent.BindingContext = this;
            this.BindingContext = new RomDetailsViewModel();
           
            
        } 

    }
}