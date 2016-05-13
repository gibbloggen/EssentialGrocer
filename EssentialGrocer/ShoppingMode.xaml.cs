using EssentialGrocer.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class ShoppingMode : Page
    {
        private ObservableCollection<Grocery> GettingGroceries;
        private ObservableCollection<Grocery> GottenGroceries;

        public ShoppingMode()
        {
            this.InitializeComponent();

            GettingGroceries = new ObservableCollection<Grocery>();
            GottenGroceries = new ObservableCollection<Grocery>();
            GroceryManager.AsynchInitializingGroceryToGet(GettingGroceries);
           

        }
        






        private void GottenGroceries_ItemClick(object sender, ItemClickEventArgs e)
        {
            Grocery NotGotten = (Grocery)e.ClickedItem;
            foreach (Grocery product in GottenGroceries)
            {
                if (product.UPC_Code == NotGotten.UPC_Code)
                {
                    GottenGroceries.Remove(product);
                    break;
                }
            }
            GettingGroceries.Add(NotGotten);
            GettingGroceries.Sort(p => p.Description);
        }

        private void GettingGroceries_ItemClick(object sender, ItemClickEventArgs e)
        {
            Grocery Gotten = (Grocery)e.ClickedItem;
            foreach (Grocery product in GettingGroceries)
            {
                if (product.UPC_Code == Gotten.UPC_Code)
                {
                    GettingGroceries.Remove(product);
                    break;
                }
            }
           GottenGroceries.Add(Gotten);
            GottenGroceries.Sort(p => p.Description);

        }

        private void MainPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            //JL
            double j = e.NewSize.Height;
            if (true)
            {
                GottenGroceriesList.Height = j - 100;
                GettingGroceriesList.Height = j - 100;
               // MyRightMenuSorta.Height = j - 100;
                MySplitView.Height = j - 37;
            }
            //else MySplitView.Height = 1000;


            /* SetterBase q = new SetterBase(    ;
           q.SetValue(GroceryStore.Height., e.NewSize.Height - 145);
           Phone.Setters.Add("SetterBase item")

           if (e.NewSize.Width > 700)
           {
               MySplitView.IsPaneOpen = true;


           }
           else
           {
               MySplitView.IsPaneOpen = false;


           }*/
            return;

        }
        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            if (GroceryManager.CheckWindowSize(Window.Current))
            {
                MySplitView.IsPaneOpen = true;
            }
            else
            {
                MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;

            }
        }

        private void CategoryButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ListBox_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (ListCreation.IsSelected)
            {
                Frame BackToCreation = (Frame)Parent;
                BackToCreation.Content = new WorkHorse();
                

            }
        }
    }
}
