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
    class RomDetailsViewModel : INotifyPropertyChanged
    {
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
        public RomItem RomItem { 
            get {
                return _romItem;
            }
            set
            {
                _romItem = value;
                OnPropertyChanged(nameof(RomItem));
            } 
        }

        private Command _loadRomCommand = null;
        public Command LoadRomCommand => _loadRomCommand ?? (_loadRomCommand = new Command(async () =>
        {
            await LoadRom();
        }));
        private async Task LoadRom()
        {
            IsLoading = true;
            this.Rom = await new RomsHelpers().FetchRomInfo(RomItem);
            IsLoading = false;
        }

    }
}
