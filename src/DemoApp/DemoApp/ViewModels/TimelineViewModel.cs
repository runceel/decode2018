using System;

using DemoApp.Helpers;
using DemoApp.Models;
using DemoApp.Services;
using DemoApp.Views;

namespace DemoApp.ViewModels
{
    public class TimelineViewModel : Observable
    {
        public DocumentManager DocumentManager { get; } = Singleton<DocumentManager>.Instance;

        private string _inputTitle;
        public string InputTitle
        {
            get { return _inputTitle; }
            set { SetProperty(ref _inputTitle, value); AddContentCommand.OnCanExecuteChanged(); }
        }

        private RelayCommand _addContentCommand;
        public RelayCommand AddContentCommand => _addContentCommand ??
            (_addContentCommand = new RelayCommand(AddContentExecute, CanAddContentExecute));


        private RelayCommand _clearCommand;
        public RelayCommand ClearCommand => _clearCommand ??
            (_clearCommand = new RelayCommand(ClearExecute));

        public TimelineViewModel()
        {
        }

        private async void AddContentExecute()
        {
            var item = await DocumentManager.CreateNewAsync(InputTitle);
            NavigationService.Navigate(typeof(ContentPage), item.Id);
        }

        private bool CanAddContentExecute()
        {
            return !string.IsNullOrWhiteSpace(InputTitle);
        }

        private async void ClearExecute()
        {
            await DocumentManager.ClearAsync();
        }

    }
}
