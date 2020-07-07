using System;
using System.Collections.Generic;
using System.Text;

namespace neonrom3r.forms.Models
{
    public class ConsoleItem
    {
       public string Name { get; set; }
       public string Slug { get; set; }

        public string Image { get {
                return $"{this.Slug}.png";
            }
        }
    }
}
