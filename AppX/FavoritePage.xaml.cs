using AppX.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace AppX
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FavoritePage : Page
    {
        List<Novel> listNovels = new List<Novel>();

        public FavoritePage()
        {
            this.InitializeComponent();
            fragmentTop.btnPane.Click += BtnPane_Click;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            getData();
        }

        private async void getData()
        {
            StorageFolder installedFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile sfile = await installedFolder.GetFileAsync("ListFavorite.txt");
            string data = await FileIO.ReadTextAsync(sfile);
            if (data.Equals("") || data == null)
            {

            }
            else
            {
                listNovels = JsonConvert.DeserializeObject<List<Novel>>(data);
                fragmentGridView.lvHomePage.ItemsSource = listNovels;
            }
        }

        private void BtnPane_Click(object sender, RoutedEventArgs e)
        {
            svLeft.IsPaneOpen = !svLeft.IsPaneOpen;
        }


    }
}
