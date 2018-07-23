using AppX.Models;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AppX
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DetailPage : Page
    {
        String name;
        String imgUrl;
        String mainUrl;
        List<Novel> listNovel = new List<Novel>();
        ObservableCollection<Chapter> chapter = new ObservableCollection<Chapter>();
        Novel parentNovel;
        HtmlDocument htmlDoc;

        public DetailPage()
        {
            this.InitializeComponent();
            fragmentTop.btnPane.Click += BtnPane_Click;
            btnFavorite.favoriteBtn.Click += favoriteBtn_Click;
        }

        enum Mode { Refresh, Update };
        int currentPage;
        void getData(String url, Mode mode)
        {
            if (mode == Mode.Refresh)
            {
                chapter.Clear();

            }
            HtmlAgilityPack.HtmlWeb htmlWeb = new HtmlAgilityPack.HtmlWeb();
            htmlDoc = htmlWeb.Load(url);
            getInfo(htmlDoc);
            getSummary(htmlDoc);
            HtmlNode _nod = htmlDoc.DocumentNode.SelectSingleNode(@"//table[@class='table table-striped']");
            HtmlNodeCollection _mainNode = _nod.SelectNodes("tr");

            foreach (HtmlNode node in _mainNode)
            {
                HtmlNode chap = node.SelectSingleNode("td[2]");
                HtmlNode n = node.SelectSingleNode("td[3]");
                if (n != null)
                {
                    String displayName = chap.SelectSingleNode("strong").InnerText + " : " + n.SelectSingleNode("a").InnerText;
                    String chapterUrl = n.SelectSingleNode("a").GetAttributeValue("href", null);
                    chapter.Add(new Chapter(displayName, chapterUrl));
                }
            }


            currentPage = Convert.ToInt16(htmlDoc.DocumentNode.SelectSingleNode(@"//a[@title='current-page']").InnerText);
            lvChapter.ItemsSource = chapter;
            checkNextPage(htmlDoc);
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            name = ((Novel)e.Parameter).name;
            imgUrl = ((Novel)e.Parameter).imgUrl;
            mainUrl = ((Novel)e.Parameter).mainUrl;
            parentNovel = (Novel)e.Parameter;
            
            getData(mainUrl,Mode.Refresh);
            await checkFavorite();
            action = checkAction();
            updateUI(action);
            svLeft.IsPaneOpen = false;

            tblName.Text = name;
            imgMain.Source = new BitmapImage(new Uri(imgUrl, UriKind.RelativeOrAbsolute));

        }

        bool action;
        void updateUI(bool action)
        {
            if (action)
            {
                btnFavorite.siFavorite.Symbol = Symbol.SolidStar;
                btnFavorite.tblFavorite.Text = "Đã Thích";
            }
            else
            {
                btnFavorite.siFavorite.Symbol = Symbol.Like;
                btnFavorite.tblFavorite.Text = "Thích";
            }
        }
        async Task checkFavorite()
        {
            StorageFolder installedFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile sfile = await installedFolder.GetFileAsync("ListFavorite.txt");
            String data = await FileIO.ReadTextAsync(sfile);
            if (data.Equals("") || data == null)
            {

            }
            else
            {
                listNovel = JsonConvert.DeserializeObject<List<Novel>>(data);
            }
        }

        int index;
        bool checkAction()
        {
            foreach (Novel novel in listNovel)
            {
                if (parentNovel.mainUrl.Equals(novel.mainUrl))
                {
                    index = listNovel.IndexOf(novel);
                    return true;break; 
                }
            }
            return false;
        }

        void getInfo(HtmlDocument htmlDoc)
        {
            HtmlNode _nod = htmlDoc.DocumentNode.SelectSingleNode(@"//div[@class='author']");
            tblAuthor.Text = _nod.SelectSingleNode("p[1]").InnerText;
            tblGenre.Text = _nod.SelectSingleNode("p[2]").InnerText;
            tblSource.Text = _nod.SelectSingleNode("p[3]").InnerText;
        }

        void getSummary(HtmlDocument htmlDoc)
        {
            HtmlNode _nod = htmlDoc.DocumentNode.SelectSingleNode(@"//div[@class='summary']");
            String data = _nod.SelectSingleNode("p[2]").InnerHtml;
            StringBuilder sb = new StringBuilder(data);
            sb.Replace("\t", "");
            sb.Replace("<br>", " . ");
            sb.Replace("<span>", "");
            sb.Replace("</span>", "");
            sb.Replace("&quot;", "\"");
            tblSum.Text = "\"" + sb.ToString() + "\"";
        }


        private void lvChapter_ItemClick(object sender, ItemClickEventArgs e)
        {
            Chapter paramChapter = (Chapter)e.ClickedItem;
            paramChapter.parentNovel = parentNovel;
            Frame.Navigate(typeof(ContentPage), paramChapter);
            
        }

        void checkNextPage(HtmlDocument htmlDoc)
        {
            HtmlNode previousNod = htmlDoc.DocumentNode.SelectSingleNode(@"//a[@title='current-page']");
            HtmlNode a = previousNod.ParentNode.NextSibling;
            if (!a.Name.Equals("li"))
            {
                
                
            }            
        }

        private void favoriteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!action)
            {
                addFavorite(parentNovel);
            }

            else
            {
                removeFavorite(parentNovel);
            }
            action = checkAction();
            updateUI(action);
        }

        private async void removeFavorite(Novel parentNovel)
        {
            listNovel.RemoveAt(index);
            String data = JsonConvert.SerializeObject(listNovel);
            StorageFolder installedFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile sfile = await installedFolder.GetFileAsync("ListFavorite.txt");
            await FileIO.WriteTextAsync(sfile, data);
            MessageDialog md = new MessageDialog("Đã xóa khỏi mục ưa thích");
            await md.ShowAsync();

        }

        private async void addFavorite(Novel parentNovel)
        {
            listNovel.Add(parentNovel);
            String data = JsonConvert.SerializeObject(listNovel);
            StorageFolder installedFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile sfile = await installedFolder.GetFileAsync("ListFavorite.txt");
            await FileIO.WriteTextAsync(sfile, data);
            MessageDialog md = new MessageDialog("Đã thêm vào mục ưa thích");
            await md.ShowAsync();
        }

        private void BtnPane_Click(object sender, RoutedEventArgs e)
        {
            svLeft.IsPaneOpen = !svLeft.IsPaneOpen;
        }

        private void lvChapter_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var panel = (ItemsWrapGrid)lvChapter.ItemsPanelRoot;
            panel.ItemWidth = e.NewSize.Width;
        }

        public ScrollViewer GetScrollViewer(DependencyObject depObj)
        {
            if (depObj is ScrollViewer) return depObj as ScrollViewer;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);
                var result = GetScrollViewer(child);
                if (result != null) return result;
            }
            return null;
        }

        private void LvHomePage_Loaded(object sender, RoutedEventArgs e)
        {
            ScrollViewer sv = GetScrollViewer(lvChapter);
            sv.ViewChanged += sv_ViewChanged;
        }

        private void sv_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            ScrollViewer sv = GetScrollViewer(lvChapter);
            var verticalOffsetvalue = sv.VerticalOffset;

            if (verticalOffsetvalue == sv.ScrollableHeight)
            {
                getData(mainUrl + "/" + (currentPage + 1), Mode.Update);
            }
        }


        bool spanned;

        private void tblSum_Tapped(object sender, TappedRoutedEventArgs e)
        {
            
            if (!spanned)
            {
                tblAuthor.Visibility = Visibility.Collapsed;
                tblGenre.Visibility = Visibility.Collapsed;
                tblSource.Visibility = Visibility.Collapsed;
                tblSum.TextTrimming = TextTrimming.None;
                spanned = true;
            }
            else
            {                
                tblAuthor.Visibility = Visibility.Visible;
                tblGenre.Visibility = Visibility.Visible;
                tblSource.Visibility = Visibility.Visible;
                tblSum.TextTrimming = TextTrimming.Clip;
                spanned = false;
            }

        }
    }
}
