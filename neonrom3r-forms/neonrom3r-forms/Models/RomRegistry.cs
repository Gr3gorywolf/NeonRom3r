using System;
using System.Collections.Generic;
using System.Text;

namespace neonrom3r.forms.Models
{
    public class RomRegistry : Rom
    {

        public RomRegistry(Rom parentRom)
        {
            this.Console = parentRom.Console;
            this.DownloadLink = parentRom.DownloadLink;
            this.Name = parentRom.Name;
            this.Portrait = parentRom.Portrait;
            this.Region = parentRom.Region;
            this.Size = parentRom.Size;
        }
        public string FilePath { get; set; }
    }
}
