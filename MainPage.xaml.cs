using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using Windows.Web.Syndication;

namespace GTAVNews
{
    public sealed partial class MainPage : Page
    {
        // Multiple RSS sources in case one fails
        private readonly List<string> _feedUrls = new List<string>
        {
            "https://www.rockstargames.com/feed",
            "https://feeds.feedburner.com/RockstarGames",
            "https://www.reddit.com/r/gtaonline/new/.rss?sort=new",
            "https://www.reddit.com/r/GTA/.rss"
        };

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            LoadNews();
        }

        private void RetryButton_Click(object sender, RoutedEventArgs e)
        {
            LoadNews();
        }

        private async void LoadNews()
        {
            // Show loading state
            LoadingPanel.Visibility = Visibility.Visible;
            ErrorPanel.Visibility = Visibility.Collapsed;
            NewsScrollViewer.Visibility = Visibility.Collapsed;
            NewsPanel.Children.Clear();
            SubtitleText.Text = "Loading latest news...";

            SyndicationFeed feed = null;
            string lastError = "";

            foreach (var url in _feedUrls)
            {
                try
                {
                    var client = new SyndicationClient();
                    client.SetRequestHeader("User-Agent", "GTAVNewsApp/1.0 (Windows UWP)");
                    client.SetRequestHeader("Accept", "application/rss+xml, application/atom+xml, */*");
                    feed = await client.RetrieveFeedAsync(new Uri(url));
                    if (feed != null && feed.Items != null && feed.Items.Count > 0)
                        break;
                }
                catch (Exception ex)
                {
                    lastError = ex.Message;
                    feed = null;
                }
            }

            LoadingPanel.Visibility = Visibility.Collapsed;

            if (feed == null || feed.Items == null || feed.Items.Count == 0)
            {
                // Show sample data so the app is never completely blank
                ShowSampleData();
                SubtitleText.Text = "Showing sample data (check your internet connection)";
                return;
            }

            SubtitleText.Text = $"{feed.Items.Count} articles from {(feed.Title?.Text ?? "GTA News")}";
            BuildNewsCards(feed.Items);
            NewsScrollViewer.Visibility = Visibility.Visible;
        }

        private void BuildNewsCards(IList<SyndicationItem> items)
        {
            Color[] accentColors = {
                Color.FromArgb(255, 230, 57, 70),
                Color.FromArgb(255, 255, 140, 0),
                Color.FromArgb(255, 76, 201, 240),
            };

            int i = 0;
            foreach (var item in items)
            {
                if (i >= 30) break;

                string title = item.Title?.Text ?? "No title";
                string summary = item.Summary?.Text ?? item.Content?.Text ?? "";

                // Strip basic HTML tags from summary
                summary = StripHtml(summary);
                if (summary.Length > 200) summary = summary.Substring(0, 200) + "…";

                string date = "";
                if (item.PublishedDate.Year > 2000)
                    date = item.PublishedDate.LocalDateTime.ToString("MMM dd, yyyy  HH:mm");

                var card = BuildCard(title, summary, date, accentColors[i % accentColors.Length]);
                NewsPanel.Children.Add(card);
                i++;
            }
        }

        private Border BuildCard(string title, string summary, string date, Color accent)
        {
            var accentBar = new Rectangle
            {
                Width = 4,
                Fill = new SolidColorBrush(accent),
                RadiusX = 2,
                RadiusY = 2,
                Margin = new Thickness(0, 4, 12, 4)
            };

            var titleBlock = new TextBlock
            {
                Text = title,
                FontSize = 18,
                FontWeight = Windows.UI.Text.FontWeights.SemiBold,
                Foreground = new SolidColorBrush(Color.FromArgb(255, 241, 241, 241)),
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 6)
            };

            var contentStack = new StackPanel { Orientation = Orientation.Vertical };
            contentStack.Children.Add(titleBlock);

            if (!string.IsNullOrWhiteSpace(summary))
            {
                contentStack.Children.Add(new TextBlock
                {
                    Text = summary,
                    FontSize = 14,
                    Foreground = new SolidColorBrush(Color.FromArgb(255, 160, 160, 176)),
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(0, 0, 0, 8)
                });
            }

            if (!string.IsNullOrWhiteSpace(date))
            {
                contentStack.Children.Add(new TextBlock
                {
                    Text = date,
                    FontSize = 11,
                    Foreground = new SolidColorBrush(Color.FromArgb(255, 100, 100, 120))
                });
            }

            var row = new StackPanel { Orientation = Orientation.Horizontal };
            row.Children.Add(accentBar);
            row.Children.Add(contentStack);

            return new Border
            {
                Background = new SolidColorBrush(Color.FromArgb(255, 22, 33, 62)),
                CornerRadius = new CornerRadius(8),
                Padding = new Thickness(20, 16, 20, 16),
                Child = row
            };
        }

        private void ShowSampleData()
        {
            var samples = new List<(string title, string summary, string date)>
            {
                ("GTA V Online: Latest Update", "Rockstar Games continues to deliver new content for GTA Online players worldwide.", "Sep 2026"),
                ("New Vehicles Added to Southern SA Auto", "Several new exotic and sports cars have been added to the in-game dealership.", "Sep 2026"),
                ("Double Money on Select Missions", "This week features double GTA$ and RP on Contact Missions and Adversary Modes.", "Sep 2026"),
                ("GTA 6 Hype Continues to Build", "The gaming community awaits the next installment in the Grand Theft Auto series.", "Sep 2026"),
                ("Rockstar Newswire Updates", "Check Rockstar's official Newswire for the latest GTA Online news and events.", "Sep 2026"),
            };

            Color[] colors = {
                Color.FromArgb(255, 230, 57, 70),
                Color.FromArgb(255, 255, 140, 0),
                Color.FromArgb(255, 76, 201, 240),
            };

            int i = 0;
            foreach (var (title, summary, date) in samples)
            {
                NewsPanel.Children.Add(BuildCard(title, summary, date, colors[i % colors.Length]));
                i++;
            }
            NewsScrollViewer.Visibility = Visibility.Visible;
        }

        private static string StripHtml(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";
            // Simple HTML tag removal
            var output = System.Text.RegularExpressions.Regex.Replace(input, "<.*?>", " ");
            output = System.Net.WebUtility.HtmlDecode(output);
            output = System.Text.RegularExpressions.Regex.Replace(output, @"\s+", " ").Trim();
            return output;
        }
    }
}