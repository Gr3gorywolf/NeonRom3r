using neonrom3r.forms.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace neonrom3r.forms.Utils
{
   public class RomsHelpers
    {
       public List<RomItem> GetRoms(ConsoleItem console)
        {
            var listPath = $"{Constants.CachePath}/{console.Slug}.json";
            var content = File.ReadAllText(listPath);
            var roms = JsonConvert.DeserializeObject<List<RomItem>>(content);
            return roms;
        }

        public async Task<Rom> FetchRomInfo(RomItem rom)
        {
            var data = new WebClient().DownloadString(new Uri(rom.InfoLink));
            return JsonConvert.DeserializeObject<Rom>(data);
        }
    }
}
