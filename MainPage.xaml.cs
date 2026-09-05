using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Web.Syndication;

namespace GTAVNews
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                var client = new SyndicationClient();
                var feed = await client.RetrieveFeedAsync(new Uri("https://www.reddit.com/r/GTA/top/.rss"));
                NewsListView.ItemsSource = feed.Items;
            }
            catch { }
        }
    }
}