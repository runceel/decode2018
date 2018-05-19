using AdaptiveCards.Rendering.Uwp;
using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel.UserActivities;
using Windows.Security.Credentials;
using Windows.Storage;
using Windows.UI.Shell;
using Windows.Security.Authentication.Web.Core;
using System.Diagnostics;

namespace TimelineApp
{
    class DocumentManager : BindableBase
    {
        private UserActivitySession _currentActivity;

        public WebAccount Account { get; private set; }

        public static DocumentManager Instance { get; } = new DocumentManager();

        private ObservableCollection<AppContent> _items;
        public ObservableCollection<AppContent> Items
        {
            get { return _items; }
            private set { SetProperty(ref _items, value); }
        }

        public Task SetAccountAsync(WebAccount account)
        {
            Account = account;
            ApplicationData.Current.LocalSettings.Values["CurrentUserProviderId"] = account.WebAccountProvider.Id;
            ApplicationData.Current.LocalSettings.Values["CurrentUserId"] = account.Id;
            return Task.CompletedTask;
        }

        public async Task AddOrUpdateDocumentAsync(AppContent appContent)
        {
            CheckInitialized();
            var target = Items.FirstOrDefault(x => x.Id == appContent.Id);
            if (target != null)
            {
                target.Title = appContent.Title;
                target.Content = appContent.Content;
            }
            else
            {
                target = appContent;
                target.Id = Guid.NewGuid().ToString();
            }

            await CreateActivityAsync(target);
            await SaveItemsAsync();
        }

        private async Task CreateActivityAsync(AppContent target)
        {
            var channel = UserActivityChannel.TryGetForWebAccount(Account);
            var activity = await channel.GetOrCreateUserActivityAsync(target.Id);
            activity.ActivationUri = new Uri($"decodedemo:?id={target.Id}");

            var card = target.ToAdaptiveCard();
            Debug.WriteLine(card.ToJson());
            activity.VisualElements.Content = AdaptiveCardBuilder.CreateAdaptiveCardFromJson(card.ToJson());
            activity.VisualElements.DisplayText = target.Title;

            await activity.SaveAsync();
            _currentActivity?.Dispose();
            _currentActivity = activity.CreateSession();
        }

        public Task<AppContent> GetItemByIdAsync(string id)
        {
            CheckInitialized();
            return Task.FromResult(Items.FirstOrDefault(x => x.Id == id)?.Clone());
        }

        public async Task<AppContent> CreateNewAsync(string title)
        {
            CheckInitialized();
            var item = new AppContent { Id = Guid.NewGuid().ToString(), Title = title };
            Items.Add(item);
            await CreateActivityAsync(item);
            await SaveItemsAsync();
            return item.Clone();
        }

        private async Task SaveItemsAsync()
        {
            var file = await ApplicationData.Current.LocalFolder.TryGetItemAsync("items.json") as StorageFile;
            if (file == null)
            {
                file = await ApplicationData.Current.LocalFolder.CreateFileAsync("items.json");
            }

            await FileIO.WriteTextAsync(file, JsonConvert.SerializeObject(Items.ToArray()));
        }

        public async Task ClearAsync()
        {
            Items.Clear();
            await SaveItemsAsync();
        }

        public async Task LoadItemsAsync()
        {
            if (Items != null)
            {
                return;
            }

            Items = new ObservableCollection<AppContent>();
            var file = await ApplicationData.Current.LocalFolder.TryGetItemAsync("items.json") as StorageFile;
            if (file == null)
            {
                return;
            }

            using (var s = await file.OpenStreamForReadAsync())
            using (var sr = new StreamReader(s))
            {
                var items = JsonConvert.DeserializeObject<AppContent[]>(await sr.ReadToEndAsync());
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
        }

        public async Task LoadAccountAsync()
        {
            var providerId = ApplicationData.Current.LocalSettings.Values["CurrentUserProviderId"]?.ToString();
            var accountId = ApplicationData.Current.LocalSettings.Values["CurrentUserId"]?.ToString();

            if (providerId == null || accountId == null)
            {
                return;
            }

            var provider = await WebAuthenticationCoreManager.FindAccountProviderAsync(providerId);
            var account = await WebAuthenticationCoreManager.FindAccountAsync(provider, accountId);

            var request = new WebTokenRequest(provider, "wl.basic");

            var result = await WebAuthenticationCoreManager.GetTokenSilentlyAsync(request, account);
            if (result.ResponseStatus == WebTokenRequestStatus.Success)
            {
                Account = result.ResponseData[0].WebAccount;
            }
        }

        private void CheckInitialized()
        {
            if (Items == null)
            {
                throw new InvalidOperationException("Object isn't initialized.");
            }

            if (Account == null)
            {
                throw new InvalidOperationException("Account isn't inisizlied.");
            }
        }
    }
}
