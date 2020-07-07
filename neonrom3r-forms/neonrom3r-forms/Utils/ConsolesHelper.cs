using neonrom3r.forms.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace neonrom3r.forms.Utils
{
    class ConsolesHelper
    {
        public  List<ConsoleItem> GetConsoles()
        {
           
            return new List<ConsoleItem>()
            {
                 #region Consoles
                new ConsoleItem()
                {
                    Name = "Gameboy",
                    Slug = "GB"
                },
                 new ConsoleItem()
                {
                    Name = "Gameboy Color",
                    Slug = "GBC"
                },
                  new ConsoleItem()
                {
                    Name = "Gameboy Advance",
                    Slug = "GBA"
                },
                   new ConsoleItem()
                {
                    Name = "Nintendo",
                    Slug = "NES"
                },
                    new ConsoleItem()
                {
                    Name = "Super Nintendo",
                    Slug = "SNES"
                }, new ConsoleItem()
                {
                    Name = "Nintendo 64",
                    Slug = "N64"
                }, new ConsoleItem()
                {
                    Name = "Playstation",
                    Slug = "PSX"
                }, new ConsoleItem()
                {
                    Name = "Sega Genesis",
                    Slug = "Genesis"
                }, new ConsoleItem()
                {
                    Name = "Sega Dreamcast",
                    Slug = "Dreamcast"
                }, new ConsoleItem()
                {
                    Name = "Nintendo DS",
                    Slug = "NDS"
                }
                  #endregion
            };
        }

    }
}
