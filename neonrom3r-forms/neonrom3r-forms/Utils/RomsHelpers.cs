using neonrom3r.forms.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
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

        public List<RomRegistry> GetDownloadedRoms()
        {
            if (File.Exists(Constants.DownloadsFile))
            {
                var fileContents = File.ReadAllText(Constants.DownloadsFile);
                try
                {
                    return JsonConvert.DeserializeObject<List<RomRegistry>>(fileContents);
                }
                catch (Exception ex)
                {
                    return new List<RomRegistry>();
                }
            }
            else
            {
                return new List<RomRegistry>();
            }
        }

        public void RegisterDownloadedRom(Rom rom,string location)
        {
            var downloaded = GetDownloadedRoms();
            var romRegistry = new RomRegistry(rom)
            {
                FilePath = location
            };
            if (!downloaded.Contains(romRegistry))
            {
                downloaded.Add(romRegistry);
            }
            if (!Directory.Exists(Constants.CachePath))
            {
                Directory.CreateDirectory(Constants.CachePath);
            }
            var file = File.CreateText(Constants.DownloadsFile);
            file.Write(JsonConvert.SerializeObject(downloaded));
            file.Close();
            if (!Directory.Exists(Constants.CatchedPortraitsPath)) {
                Directory.CreateDirectory(Constants.CatchedPortraitsPath);
            };
            new Thread(new ThreadStart(() =>
            {
                try
                {
                    new WebClient().DownloadFile(rom.Portrait, $"{Constants.CatchedPortraitsPath}/{rom.Name}.png");
                }
                catch(Exception ex) {}
            })).Start();
        }
    }
}
