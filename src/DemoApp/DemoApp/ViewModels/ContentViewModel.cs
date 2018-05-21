using System;

using DemoApp.Helpers;
using DemoApp.Models;
using DemoApp.Services;

namespace DemoApp.ViewModels
{
    public class ContentViewModel : Observable
    {
        public DocumentManager DocumentManager { get; } = Singleton<DocumentManager>.Instance;

        private AppContent _appContent;
        public AppContent AppContent
        {
            get { return _appContent; }
            set { SetProperty(ref _appContent, value); }
        }

        private RelayCommand _saveCommand;

        public RelayCommand SaveCommand => _saveCommand ??
            (_saveCommand = new RelayCommand(SaveExecute));

        public object NavigationManager { get; private set; }

        public ContentViewModel()
        {
        }

        private async void SaveExecute()
        {
            await DocumentManager.AddOrUpdateDocumentAsync(AppContent);
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }
    }
}
