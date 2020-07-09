using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace neonrom3r.forms.DependencyServices
{
    public interface IAlertService
    {
         void ShowToast(string message);
    }
}
