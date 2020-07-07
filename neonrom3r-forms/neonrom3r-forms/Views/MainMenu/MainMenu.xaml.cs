using neonrom3r.forms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace neonrom3r.forms.Views.MainMenu
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenu : MasterDetailPage
    {
        static MainMenu instance;
        public Type CurrentDetailType;
        public static MainMenu GetInstance()
        {
            return instance;
        }
        public MainMenu()
        {
            InitializeComponent();
           
            MasterPage.PagesList.ItemSelected += ListView_ItemSelected;
        }




        public async Task SwapDetail(Page newPage, bool forceSwap = false)
        {
            MasterPage.PagesList.SelectedItem = null;
            ///Se encarga de evitar que se abra dos veces la misma pagina
            this.IsPresented = false;
            if (CurrentDetailType != newPage.GetType())
            {
                CurrentDetailType = newPage.GetType();
            }
            else
            {
                if (!forceSwap)
                    return;
            }
            Detail = new NavigationPage(newPage);
            return;
        }



        private async void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageItem;
            if (item == null)
                return;
            await SwapDetail((Page)Activator.CreateInstance(item.TargetType));
        }
    }
}