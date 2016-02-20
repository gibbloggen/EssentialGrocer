
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using EssentialGrocer.Model;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace EssentialGrocer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // private List<Grocery> Groceries;
        //private List<Grocery> GroceriesToGet;
        private ObservableCollection<Grocery> GroceriesToGet;
        private ObservableCollection<Grocery> Groceries;

        public MainPage()
        {
            this.InitializeComponent();
            Groceries = new ObservableCollection<Grocery>();
            //Groceries = GroceryManager.GetGroceriesFromXML();
            GroceryManager.GetGroceriesByAisle("Produce", Groceries);
            //Groceries = GroceryManager.GetGroceries();
            //GroceriesToGet = GroceryManager.GetGroceries();
            //GroceriesToGet = GroceryManager.GroceryManagerObservable.GetThemGroceries();
            GroceriesToGet = new ObservableCollection<Grocery>();
        }

        private void GroceryStore_ItemClick(object sender, ItemClickEventArgs e)
        {
            var PutItOnTheList = (Grocery)e.ClickedItem;
            //GroceriesToGet.Add(new Grocery { UPC_Code = PutItOnTheList.UPC_Code, Product = PutItOnTheList.Product });
            var blah = new Grocery();
            blah.UPC_Code = PutItOnTheList.UPC_Code;
            blah.Description = PutItOnTheList.Description;
            blah.Isle = PutItOnTheList.Isle;
            GroceriesToGet.Add(blah);
            // GroceryStoreToGetList.ItemsSource = GroceriesToGet;
            // GroceryStoreToGetList.DataContext = GroceriesToGet

        }

        private void GroceryStoreToGet_ItemClick(object sender, ItemClickEventArgs e)
        {


        }

        private void GroceryStoreToGetList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var RemoveItFromTheList = (Grocery)e.ClickedItem;

            foreach (Grocery product in GroceriesToGet)
            {
                if (product.UPC_Code == RemoveItFromTheList.UPC_Code)
                {
                    GroceriesToGet.Remove(product);
                    break;
                }
            }



        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;
        }



        private async void ListBox_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (NewList.IsSelected)
            {


                //Groceries= GroceryManager.GetGroceriesFromXML();
                // Groceries = GroceryManager.GroceryManagerObservable.GetThemGroceries();
                GroceriesToGet.Clear();
                MySplitView.IsPaneOpen = false;


            }
            else if (OpenList.IsSelected)
            {

                GroceryManager Grok = new GroceryManager();

                // Clear previous returned file name, if it exists, between iterations of this scenario
                //tputTextBlock.Text = "";

                FileOpenPicker openPicker = new FileOpenPicker();
                openPicker.ViewMode = PickerViewMode.List;
                openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                openPicker.FileTypeFilter.Add(".xml");
                StorageFile file = await openPicker.PickSingleFileAsync();
                if (file != null)
                {




                    GroceryManager.GetGroceriesFromSavedList(file, GroceriesToGet, true);

                }

                MySplitView.IsPaneOpen = false;
                /* some code to open a list */
            }
            else if (SaveList.IsSelected)
            {
                StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                await localFolder.CreateFileAsync("FindAnything.txt");
                //OutputTextBlock.Text = "Hello World, How the heck are you????";
                XDocument doccer = new XDocument();
                GroceryManager v = new GroceryManager();
                v.GetXMLForSaving(GroceriesToGet, doccer);
                FileSavePicker savePicker = new FileSavePicker();
                savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                // Dropdown of file types the user can save the file as
                savePicker.FileTypeChoices.Add("XML markup", new List<string>() { ".xml" });
                // Default file name if the user does not type one in or select a file to replace
                savePicker.SuggestedFileName = "New Document";
                StorageFile file = await savePicker.PickSaveFileAsync();
                if (file != null)
                {
                    // Prevent updates to the remote version of the file until we finish making changes and call CompleteUpdatesAsync.
                    CachedFileManager.DeferUpdates(file);
                    // write to file
                    await FileIO.WriteTextAsync(file, doccer.ToString());
                    //await FileIO.WriteLinesAsync(file, GroceriesToGet.ToArray<Grocery>);
                    // await FileIO.WriteTextAsync(file, file.Name);
                    //await FileIO.WriteTextAsync(file, "ain't got no shit");
                    //await FileIO.AppendTextAsync(file, "ain't got no shit");
                    //IEnumerable<String> j = (IEnumerable<String>)("Ain't Got no Shit")
                    //await FileIO.AppendLinesAsync(file, "Ain't got no shit????");
                    // Let Windows know that we're finished changing the file so the other app can update the remote version of the file.
                    // Completing updates may require Windows to ask for user input.
                    FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
                    if (status == FileUpdateStatus.Complete)
                    {
                        //OutputTextBlock.Text = "File " + file.Name + " was saved.";
                    }
                    else
                    {
                        //OutputTextBlock.Text = "File " + file.Name + " couldn't be saved.";
                    }
                }
                else
                {
                    //OutputTextBlock.Text = "Operation cancelled.";
                }
            }


            /*
            GroceryManager j = new GroceryManager();
             j.SaveGroceryToGetList(GroceriesToGet, "FirstList");
            MySplitView.IsPaneOpen = false;
            /* some code to open a list */
        }


        private void CategoryButton_Click(object sender, RoutedEventArgs e)
        {
            CategorySplitView.IsPaneOpen = !CategorySplitView.IsPaneOpen;





        }


        private void CategoryListBox_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (Produce.IsSelected)
            {

                GroceryManager.GetGroceriesByAisle("Produce", Groceries);
                CategorySplitView.IsPaneOpen = false;


            }
            else if (Bakery.IsSelected)
            {
                GroceryManager.GetGroceriesByAisle("Bakery", Groceries);
                CategorySplitView.IsPaneOpen = false;
            }
            else if (Dairy.IsSelected)
            {
                GroceryManager.GetGroceriesByAisle("Dairy", Groceries);
                CategorySplitView.IsPaneOpen = false;

            }

        }
    }

}
