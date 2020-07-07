using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace neonrom3r.forms.Utils
{
   public class AnimationsHelper
    {
        public static async Task SlideUpModal(VisualElement element)
        {

            await element.TranslateTo(0, Convert.ToInt32(Application.Current.MainPage.Height * 0.75), 0);
            await element.TranslateTo(0, 0, 200);
        }

        public static async Task SlideDownModal(VisualElement element)
        {
            await element.TranslateTo(0, 0, 0);
            await element.TranslateTo(0, Convert.ToInt32(Application.Current.MainPage.Height * 0.75), 200);
           
        }
    }
}
