using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace EssentialGrocer
{
    using EssentialGrocer.Model;
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ManageGroups : Page
    {
        public ObservableCollection<string> CurrentGroupsCollection;   // = new ObservableCollection<string>();
        public ObservableCollection<string> CurrentGroupsHolder;  // = new ObservableCollection<string>();

        public ManageGroups()
        {
            this.InitializeComponent();
            CurrentGroupsCollection = GroceryManager.GetGroups();
            CurrentGroupsHolder = GroceryManager.GetGroups();

            /*
            CurrentGroupsCollection.Add("Produce");
            CurrentGroupsCollection.Add("Meat");
            CurrentGroupsCollection.Add("Deli");
            CurrentGroupsCollection.Add("Cheese");
            CurrentGroupsCollection.Add("Jarred/Canned");
            CurrentGroupsCollection.Add("Pasta/Noodles");

            CurrentGroupsHolder.Add("Produce");
            CurrentGroupsHolder.Add("Meat");
            CurrentGroupsHolder.Add("Deli");
            CurrentGroupsHolder.Add("Cheese");
            CurrentGroupsHolder.Add("Jarred/Canned");
            CurrentGroupsHolder.Add("Pasta/Noodles");*/
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

        private async void ListBox_Tapped_1(object sender, TappedRoutedEventArgs e)
        {
            if (CancelChanges.IsSelected)
            {
                Frame BackToCreation = (Frame)Parent;
                BackToCreation.Content = new WorkHorse();
            }
            else if (SaveChangedGroups.IsSelected)
            {
                // Thanks to Vitalii Vzsylenko http://stackoverflow.com/questions/22909329/universal-apps-messagebox-the-name-messagebox-does-not-exist-in-the-current
                var RUSuredialog = new MessageDialog("You are about to change the grouping of your Master List, there is no going back!  Are You Sure?");
                RUSuredialog.Title = "Are You Sure!";
                RUSuredialog.Commands.Add(new UICommand { Label = "Ok", Id = 1 });
                RUSuredialog.Commands.Add(new UICommand { Label = "Cancel", Id = 0 });
                var res = await RUSuredialog.ShowAsync();

                if ((int)res.Id == 0)
                { return; }
                else {

                

                Frame BackToCreation = (Frame)Parent;
                BackToCreation.Content = new WorkHorse();
            }
            }
            else if (AddGroup.IsSelected)
            {
                /*
                if (StoreItemDescription.Text.Length == 0) return;
                GroceryManager.AddToList(Groceries, AddCategoryText.Text, StoreItemDescription.Text);
                StoreItemDescription.Text = "";
                AddToTheList.Hide();
*/
            }
            else if (UndoChanges.IsSelected)
            {
                CurrentGroupsCollection.Clear();
                foreach (string j in CurrentGroupsHolder) CurrentGroupsCollection.Add(j);



                /*
                if (StoreItemDescription.Text.Length == 0) return;
                GroceryManager.AddToList(Groceries, AddCategoryText.Text, StoreItemDescription.Text);
                StoreItemDescription.Text = "";
                AddToTheList.Hide();
*/
            }
        }

        private void CurrentGroups_ItemClick(object sender, ItemClickEventArgs e)
        {
            //Added UndoGrocery Here, so if you accidently delete, you can now put it back on.
            //UndoGrocery = (string)e.ClickedItem;

            foreach (string groupee in CurrentGroupsCollection)
            {
                if (groupee == e.ClickedItem.ToString())
                {
                    CurrentGroupsCollection.Remove(groupee);
                    break;
                }
            }
           // CurrentGroupsCollection.UpdateToGetList(GroceriesToGet);

        }

        private void MakeAddGroupFlyout(object sender, TappedRoutedEventArgs e)
        {

        }

        private void AddToGroupList(object sender, TappedRoutedEventArgs e)
        {
            if (StoreGroupDescription.Text.Length == 0) return;
            CurrentGroupsCollection.Add(StoreGroupDescription.Text);
            // CurrentGroupsCollection = new ObservableCollection<string>(CurrentGroupsCollection.OrderBy(p => p));
            //CurrentGroupsCollection.OrderBy(p => p);


            CurrentGroupsCollection = GroceryManager.OrderThoseGroups(CurrentGroupsCollection);

            StoreGroupDescription.Text = "";
            AddToTheList.Hide();
        }

        private void CancelGroupFlyout(object sender, TappedRoutedEventArgs e)
        {
            StoreGroupDescription.Text = "";
            AddToTheList.Hide();
        }
    }
   
}
