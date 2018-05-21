using AdaptiveCards.Rendering.Uwp;
using DemoApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace DemoApp.Converters
{
    public class AdaptiveCardConverter : IValueConverter
    {
        private AdaptiveCardRenderer _renderer;
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (_renderer == null)
            {
                _renderer = new AdaptiveCardRenderer();
            }

            var json = ((AppContent)value).ToAdaptiveCard().ToJson();
            return _renderer.RenderAdaptiveCard(AdaptiveCard.FromJsonString(json).AdaptiveCard).FrameworkElement;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotSupportedException();
        }
    }
}
