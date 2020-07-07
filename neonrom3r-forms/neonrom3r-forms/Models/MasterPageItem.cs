using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace neonrom3r.forms.Models
{

    public class MasterPageItem
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Icon { get; set; }
        public Type TargetType { get; set; }
    }
}