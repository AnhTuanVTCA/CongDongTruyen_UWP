using AppX.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AppX
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ContentPage : Page
    {
        public ContentPage()
        {
            this.InitializeComponent();
            fragmentTop.btnPane.Click += BtnPane_Click;


        }

        String nextPage;
        String prePage;
        HtmlDocument htmlDoc;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            String url = ((Chapter)e.Parameter).chapterUrl;
            getData(url);

        }

        void getInfo(HtmlDocument htmlDoc)
        {
            HtmlNode _nodTitle = htmlDoc.DocumentNode.SelectSingleNode(@"//h1[@class='title']");
            tblTitle.Text = _nodTitle.InnerText;
            HtmlNode _nodChapter = htmlDoc.DocumentNode.SelectSingleNode(@"//div[@class='author']");
            tblChapter.Text = _nodChapter.SelectSingleNode("h3").InnerText;

        }

        void getData(String url) {
            ProgressRing progressRing = new ProgressRing();
            progressRing.IsActive = true;
            HtmlAgilityPack.HtmlWeb htmlWeb = new HtmlAgilityPack.HtmlWeb();
            htmlDoc = htmlWeb.Load(url);
            getInfo(htmlDoc);
            HtmlNode _nod = htmlDoc.DocumentNode.SelectSingleNode(@"//div[@id='detailcontent']");
            String html = _nod.InnerHtml.Replace("\t","");
            StringBuilder sb = new StringBuilder(html);
            sb.Replace("<br>", "\n");
            sb.Replace("&quot;", "\"");
            tblContent.Text = sb.ToString();
            checkPageState(htmlDoc);
            progressRing.IsActive = false;
        }

        private void previous_Click(object sender, RoutedEventArgs e)
        {
            getData(prePage);
            scrollviewer.ChangeView(null, 0, null);
        }

        private void next_Click(object sender, RoutedEventArgs e)
        {
            getData(nextPage);
            scrollviewer.ChangeView(null, 0, null);
        }

        void checkPageState(HtmlDocument htmlDoc)
        {
            HtmlNode _nodPre = htmlDoc.DocumentNode.SelectSingleNode(@"//a[@id='prechap']");
            if (_nodPre != null)
            {
                prePage = _nodPre.GetAttributeValue("href", null);
                previous.Visibility = Visibility.Visible;
            }
            else previous.Visibility = Visibility.Collapsed;

            HtmlNode _nodNext = htmlDoc.DocumentNode.SelectSingleNode(@"//a[@id='nextchap']");
            if (_nodNext != null)
            {
                nextPage = _nodNext.GetAttributeValue("href", null);
                next.Visibility = Visibility.Visible;
            }
            else next.Visibility = Visibility.Collapsed;
        }

        private void zoomout_Click(object sender, RoutedEventArgs e)
        {
            if (tblContent.FontSize >= 10)
            {
                tblContent.FontSize -= 2;
            }
        }

        private void zoomin_Click(object sender, RoutedEventArgs e)
        {
            tblContent.FontSize += 2;
        }

        private void BtnPane_Click(object sender, RoutedEventArgs e)
        {
            svLeft.IsPaneOpen = !svLeft.IsPaneOpen;
        }
    }
}
