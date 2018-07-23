using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace AppX
{
    public sealed partial class FragmentLeft : UserControl
    {

        public FragmentLeft()
        {
            this.InitializeComponent();
            List<MenuItem> list = new List<MenuItem>();

            list.Add(new MenuItem(1, "TRANG CHỦ", "http://congdongtruyen.com/all/"));
            list.Add(new MenuItem(2, "TRUYỆN TIÊN HIỆP", "http://congdongtruyen.com/tien-hiep/"));
            list.Add(new MenuItem(3, "TRUYỆN KIẾM HIỆP", "http://congdongtruyen.com/kiem-hiep/"));
            list.Add(new MenuItem(4, "TRUYỆN NGÔN TÌNH", "http://congdongtruyen.com/ngon-tinh/"));
            list.Add(new MenuItem(5, "TRUYỆN ĐÔ THỊ", "http://congdongtruyen.com/do-thi/"));
            list.Add(new MenuItem(6, "TRUYỆN XUYÊN KHÔNG", "http://congdongtruyen.com/xuyen-khong/"));
            list.Add(new MenuItem(7, "TRUYỆN ƯA THÍCH", ""));
            list.Add(new MenuItem(8, "VỀ TÁC GIẢ", ""));
            lvMenu.ItemsSource = list;
            

        }

        private void lvMenu_ItemClick(object sender, ItemClickEventArgs e)
        {
            MenuItem menuItem = (MenuItem)e.ClickedItem;
            if (menuItem.id<7)
            {
                ((Frame)Window.Current.Content).Navigate(typeof(MainPage),menuItem);
            }
            else
            {
                ((Frame)Window.Current.Content).Navigate(typeof(FavoritePage));
            }
        }
    }

    class MenuItem
    {
        public int id { get; set; }
        public String name { get; set; }
        public String path { get; set; }

        public MenuItem(int id, string name, string path)
        {
            this.id = id;
            this.name = name;
            this.path = path;
        }

        public MenuItem() { }
    }



}
