using neonrom3r.forms.DependencyServices;
using neonrom3r.forms.Models;
using neonrom3r.forms.Utils;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace neonrom3r.forms.ViewModels
{
    public class RomDetailsViewModel : INotifyPropertyChanged
    {
        public RomDetailsViewModel(RomItem item)
        {
            this.RomItem = item;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private Rom _rom = null;
        public Rom Rom
        {
            get
            {
                return _rom;
            }
            set
            {
                this._rom = value;
                OnPropertyChanged(nameof(Rom));
            }
        }

        private bool _isLoading = false;
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        private RomItem _romItem = null;
        private RomItem RomItem { 
            get {
                return _romItem;
            }
            set
            {
                _romItem = value;
                OnPropertyChanged(nameof(RomItem));
            } 
        }

      
      public Command LoadRom
        {
            get
            {
                return new Command(async() =>
                {
                    IsLoading = true;
                    this.Rom = await new RomsHelpers().FetchRomInfo(RomItem);
                    IsLoading = false;
                });
            }
        }

       public Command DownloadRom
        {
            get
            {
                return new Command(() =>
                {
                    var downloadedPath = DependencyService.Get<IFilesDependency>().DownloadRom(Rom);
                    if(downloadedPath != null)
                    {
                        new RomsHelpers().RegisterDownloadedRom(Rom, downloadedPath);
                    }
                    else
                    {
                        DependencyService.Get<IAlertService>().ShowToast("Rom download failed!");
                    }
                });
            }
        }

        public Command ShareRom
        {
            get
            {
                return new Command(() =>
                {
                    Xamarin.Essentials.Share.RequestAsync($"Download link for rom: {Rom.Name}\n {Rom.DownloadLink}\n Shared from:NeonRom3r");
                });
            }
        }
    }
}
