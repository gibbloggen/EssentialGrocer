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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace EssentialGrocer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ManageGroups : Page
    {
        public ManageGroups()
        {
            this.InitializeComponent();
        }
        private void MainPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            //JL
            double j = e.NewSize.Height;
            if (true)
            {
                // GottenGroceriesList.Height = j - 100;
                // GettingGroceriesList.Height = j - 100;
                // MyRightMenuSorta.Height = j - 100;
                MySplitView.Height = j - 37;
            }


            return;

        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CategoryButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ListBox_Tapped(object sender, TappedRoutedEventArgs e)
        {

        }

        private void ListBox_Tapped_1(object sender, TappedRoutedEventArgs e)
        {

        }
    }
   
}
