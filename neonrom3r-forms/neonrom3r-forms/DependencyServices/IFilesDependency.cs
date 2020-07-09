using neonrom3r.forms.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace neonrom3r.forms.DependencyServices
{
    public interface IFilesDependency
    {
        string GetRootPath();
        bool DownloadUpdateBundle(string cachePath);

        /// <summary>
        /// Executes rom downloading
        /// </summary>
        /// <param name="rom"></param>
        /// <param name="downloadPath"></param>
        /// <returns>Downloaded file path or NULL if there is a error</returns>
        string DownloadRom(Rom item,string path = null);
    }
}
