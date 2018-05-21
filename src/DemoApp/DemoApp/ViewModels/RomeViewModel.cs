using System;

using DemoApp.Helpers;
using DemoApp.Models;

namespace DemoApp.ViewModels
{
    public class RomeViewModel : Observable
    {
        public RomeManager RomeManager { get; } = Singleton<RomeManager>.Instance;

        private RelayCommand _sendCommand;

        public RelayCommand SendCommand => _sendCommand ??
            (_sendCommand = new RelayCommand(SendExecute));

        public RomeViewModel()
        {
        }

        private async void SendExecute()
        {
            await RomeManager.SendTextAsync();
        }
    }
}
