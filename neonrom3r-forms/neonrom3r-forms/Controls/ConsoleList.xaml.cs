using neonrom3r.forms.Models;
using neonrom3r.forms.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace neonrom3r.forms.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]


    public partial class ConsoleListPage : StackLayout
    {
        public delegate void OnSelected(ConsoleItem SelectedConsole);
        public OnSelected onSelected = null;
        public ConsoleListPage()
        {
            InitializeComponent();
            lstConsole.FlowItemsSource = new ConsolesHelper().GetConsoles();
            lstConsole.SelectionMode = ListViewSelectionMode.Single;
            lstConsole.FlowItemTapped += (send, args) =>
             {
                 onSelected?.Invoke((ConsoleItem)args.Item);
             };
        }
    }
}