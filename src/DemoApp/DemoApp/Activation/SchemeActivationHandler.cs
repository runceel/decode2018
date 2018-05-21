using System;
using System.Threading.Tasks;

using DemoApp.Services;
using DemoApp.Views;

using Windows.ApplicationModel.Activation;

namespace DemoApp.Activation
{
    // TODO WTS: Open package.appxmanifest and change the declaration for the scheme (from the default of 'wtsapp') to what you want for your app.
    // More details about this functionality can be found at https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/features/uri-scheme.md
    // TODO WTS: Change the image in Assets/Logo.png to one for display if the OS asks the user which app to launch.
    internal class SchemeActivationHandler : ActivationHandler<ProtocolActivatedEventArgs>
    {
        // By default, this handler expects URIs of the format 'wtsapp:sample?secret={value}'
        protected override async Task HandleInternalAsync(ProtocolActivatedEventArgs args)
        {
            var path = args.Uri.AbsolutePath.ToLowerInvariant();
            if (path.Equals(""))
            {
                var id = "";

                try
                {
                    if (args.Uri.Query != null)
                    {
                        // The following will extract the secret value and pass it to the page. Alternatively, you could pass all or some of the Uri.
                        var decoder = new Windows.Foundation.WwwFormUrlDecoder(args.Uri.Query);

                        id = decoder.GetFirstValueByName("id");
                    }
                }
                catch (Exception)
                {
                    NavigationService.Navigate(typeof(Views.MainPage));
                    return;
                }

                // It's also possible to have logic here to navigate to different pages. e.g. if you have logic based on the URI used to launch
                NavigationService.Navigate(typeof(Views.ContentPage), id);
            }
            else if (path == "rome")
            {
                var text = "";
                try
                {
                    if (args.Uri.Query != null)
                    {
                        // The following will extract the secret value and pass it to the page. Alternatively, you could pass all or some of the Uri.
                        var decoder = new Windows.Foundation.WwwFormUrlDecoder(args.Uri.Query);

                        text = decoder.GetFirstValueByName("text");
                    }
                }
                catch (Exception)
                {
                    NavigationService.Navigate(typeof(Views.MainPage));
                    return;
                }

                // It's also possible to have logic here to navigate to different pages. e.g. if you have logic based on the URI used to launch
                NavigationService.Navigate(typeof(Views.RomePage), text);

            }
            else if (args.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                // If the app isn't running and not navigating to a specific page based on the URI, navigate to the home page
                NavigationService.Navigate(typeof(Views.MainPage));
            }

            await Task.CompletedTask;
        }

        protected override bool CanHandleInternal(ProtocolActivatedEventArgs args)
        {
            // If your app has multiple handlers of ProtocolActivationEventArgs
            // use this method to determine which to use. (possibly checking args.Uri.Scheme)
            return true;
        }
    }
}
