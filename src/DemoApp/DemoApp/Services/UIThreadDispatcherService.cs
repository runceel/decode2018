using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace DemoApp.Services
{
    public class UIThreadDispatcherService
    {
        private CoreDispatcher Dispatcher { get; set; }

        public void Initialize()
        {
            Dispatcher = Window.Current.Dispatcher;
        }

        public async Task DispatchAsync(Action action)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action());
        }
    }
}
