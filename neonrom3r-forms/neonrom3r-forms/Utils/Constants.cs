using neonrom3r.forms.DependencyServices;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace neonrom3r.forms.Utils
{
    public class Constants
    {
        public  static string CachePath
        {
            get
            {
                return $"{DependencyService.Get<IFilesDependency>().GetRootPath()}/.romercache";
            }
        }

        public readonly static string CatchedPortraitsPath = $"{CachePath}/portraits";

        public readonly static string DownloadsFile = $"{CachePath}/downloads.json";

        public readonly static string VersionFile = $"{CachePath}/version.gr3d";
    }
}
