using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using neonrom3r.forms.DependencyServices;
using neonrommer.DependencyService;
using Xamarin.Forms;

[assembly: Dependency(typeof(AlertsDependency))]
namespace neonrommer.DependencyService
{
    
    public class AlertsDependency : IAlertService
    {
        public void ShowToast(string message)
        {
            Toast.MakeText(FormsActivity.GetInstance(), message, ToastLength.Long).Show();
        }
    }
}