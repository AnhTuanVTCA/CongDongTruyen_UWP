using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace AppX
{
    public sealed partial class FragmentTop : UserControl
    {

        public FragmentTop()
        {
            this.InitializeComponent();            
        }

        private void tbSearch_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                try
                {
                    search(tbSearch.Text);
                }
                catch (Exception)
                {
                    MessageDialog md = new MessageDialog("Nothing match your search");
                    md.ShowAsync();

                }
            }
        }

        void search(String keyword)
        {
            if (tbSearch.Text != null || !tbSearch.Equals(""))
            {
                String searchText = "http://congdongtruyen.com/actionsearch?txtsearch=" + keyword + "&go=";
                ((Frame)Window.Current.Content).Navigate(typeof(SearchPage), searchText);
            }

            else ((Frame)Window.Current.Content).Navigate(typeof(MainPage));
        }

        private void iLogo_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ((Frame)Window.Current.Content).Navigate(typeof(MainPage));
        }

    }


}
