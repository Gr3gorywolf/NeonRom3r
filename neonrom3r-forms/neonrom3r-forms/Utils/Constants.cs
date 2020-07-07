using neonrom3r.forms.DependencyServices;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace neonrom3r.forms.Utils
{
    public class Constants
    {
        public static string CachePath
        {
            get
            {
                return $"{DependencyService.Get<IFilesDependency>().GetRootPath()}/.romercache";
            }
        }

        public static string CatchedPortraitsPath
        {
            get
            {
                return $"{CachePath}/portraits";
            }
        }

        public static string DownloadsFile
        {
            get
            {
                return $"{CachePath}/downloads.json";
            }
        }

        public static string VersionFile
        {
            get
            {
                return $"{CachePath}/version.gr3d";
            }
        }
    }
}
