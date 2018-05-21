using AdaptiveCards;
using DemoApp.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp.Models
{
    public class AppContent : Observable
    {
        private string _id;
        public string Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _content;
        public string Content
        {
            get { return _content; }
            set { SetProperty(ref _content, value); }
        }

        public AppContent Clone()
        {
            return JsonConvert.DeserializeObject<AppContent>(
                JsonConvert.SerializeObject(this));
        }
    }

    public static class AppContentExtensions
    {
        public static AdaptiveCard ToAdaptiveCard(this AppContent self) => new AdaptiveCard
        {
            Version = "1.0",
            Body =
                {
                    new AdaptiveColumnSet
                    {
                        Columns =
                        {
                            new AdaptiveColumn
                            {
                                Width = "auto",
                                Items =
                                {
                                    new AdaptiveImage
                                    {
                                        Url = new Uri("https://user-images.githubusercontent.com/79868/40269931-ed9022be-5bbf-11e8-83b9-e8cba533e405.jpg"),
                                        Style = AdaptiveImageStyle.Person,
                                        Size = AdaptiveImageSize.Small,
                                    }
                                }
                            },
                            new AdaptiveColumn
                            {
                                Width = "stretch",
                                Items =
                                {
                                    new AdaptiveTextBlock
                                    {
                                        Weight = AdaptiveTextWeight.Bolder,
                                        Size = AdaptiveTextSize.Large,
                                        Wrap = false,
                                        MaxLines = 1,
                                        Text = self.Title ?? "",
                                    },
                                }
                            }
                        }
                    },
                    new AdaptiveTextBlock
                    {
                        Weight = AdaptiveTextWeight.Default,
                        Size = AdaptiveTextSize.Default,
                        Wrap = true,
                        MaxLines = 3,
                        Text = self.Content ?? "",
                    }
                }
        };
    }
}
