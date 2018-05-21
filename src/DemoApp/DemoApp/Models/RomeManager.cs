using DemoApp.Helpers;
using DemoApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp.Models
{
    public class RomeManager : Observable
    {
        private RomeConnectionManager RomeConnectionManager { get; } = Singleton<RomeConnectionManager>.Instance;

        private string _text;

        public string Text
        {
            get { return this._text; }
            set { this.SetProperty(ref this._text, value); }
        }

        public RomeManager()
        {
            RomeConnectionManager.RemoteMessageReceived += RemoteMessageReceived;
        }

        public async Task SendTextAsync()
        {
            await RomeConnectionManager.SendAsync(Text);
        }

        private async void RemoteMessageReceived(object sender, RemoteMessageReceivedEventArgs e)
        {
            await Singleton<UIThreadDispatcherService>.Instance.DispatchAsync(() =>
            {
                Text = e.Message;
            });
        }
    }
}
