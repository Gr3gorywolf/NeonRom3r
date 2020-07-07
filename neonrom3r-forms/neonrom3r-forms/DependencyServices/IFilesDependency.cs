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
    }
}
