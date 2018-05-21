using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.Foundation.Collections;

namespace DemoApp.Services
{
    public class RomeConnectionManager
    {
        private BackgroundTaskDeferral AppServiceDeferral { get; set; }
        private AppServiceConnection AppServiceConnection { get; set; }

        public void Initialize(IBackgroundTaskInstance taskInstance)
        {
            var appService = taskInstance.TriggerDetails as AppServiceTriggerDetails;
            if (appService == null)
            {
                return;
            }

            AppServiceDeferral = taskInstance.GetDeferral();
            taskInstance.Canceled += OnAppServicesCanceled;
            AppServiceConnection = appService.AppServiceConnection;
            AppServiceConnection.RequestReceived += OnAppServiceRequestReceived;
            AppServiceConnection.ServiceClosed += AppServiceConnection_ServiceClosed;
        }

        public void Disabled() => AppServiceConnection = null;

        public event EventHandler<RemoteMessageReceivedEventArgs> RemoteMessageReceived;

        public async Task SendAsync(string text)
        {
            if (AppServiceConnection == null)
            {
                return;
            }

            var message = new ValueSet();
            message["Request"] = text;
            await AppServiceConnection.SendMessageAsync(message);
        }

        private async void OnAppServiceRequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            var messageDeferral = args.GetDeferral();
            var message = args.Request.Message;
            var text = message["Request"] as string;

            RemoteMessageReceived?.Invoke(this, new RemoteMessageReceivedEventArgs(text));

            var returnMessage = new ValueSet();
            returnMessage.Add("Response", "True");
            await args.Request.SendResponseAsync(returnMessage);
            messageDeferral.Complete();
        }

        private void OnAppServicesCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            AppServiceDeferral.Complete();
            AppServiceDeferral = null;
            AppServiceConnection = null;
        }

        private void AppServiceConnection_ServiceClosed(AppServiceConnection sender, AppServiceClosedEventArgs args)
        {
            AppServiceDeferral.Complete();
            AppServiceDeferral = null;
            AppServiceConnection = null;
        }

    }

    public class RemoteMessageReceivedEventArgs : EventArgs
    {
        public string Message { get; }
        public RemoteMessageReceivedEventArgs(string message)
        {
            Message = message;
        }
    }
}
