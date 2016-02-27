
/*
            Program: Essential Grocer
            Author:  John Leone
            Email:   gibbloggen@gmail.com
            Date:    2-20-2016
            License: MIT License
            Purpose: A Universal Windows app for both win 10 desktop and mobile



The MIT License (MIT) 
Copyright (c) <2016> <John Leone, gibbloggen@gmail.com>

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.





*/



using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using EssentialGrocer.Model;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace EssentialGrocer
{
    
    /// <summary>
    /// This is the currently only page of the program.
    /// It contains 2 hamburger menues one top left, other top right.
    /// Also, has the main grocery list on the right, and
    /// the current list for that shopping on the left.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static double HoldWidth = 0;
        //Observable Collections here, so that when I update them
        //the change is showing on the list on the screen.
        private ObservableCollection<Grocery> GroceriesToGet;
        private ObservableCollection<Grocery> Groceries;

        public MainPage()
        {
            this.InitializeComponent();

           // this.SizeChanged += MainPage_SizeChanged;

            Groceries = new ObservableCollection<Grocery>();
            GroceryManager.GetGroceriesByAisle("Produce", Groceries);
            //GroceriesToGet = new ObservableCollection<Grocery>();
            GroceriesToGet = new ObservableCollection<Grocery>();
            //Window.Current.Bounds.Height

         //MySplitView.IsPaneOpen =  GroceryManager.CheckWindowSize(Window.Current);
         //CategorySplitView.IsPaneOpen = GroceryManager.CheckWindowSize(Window.Current);
        }






        private void GroceryStore_ItemClick(object sender, ItemClickEventArgs e)
        {
            var PutItOnTheList = (Grocery)e.ClickedItem;
            var GrocProduct = new Grocery();
            GrocProduct.UPC_Code = PutItOnTheList.UPC_Code;
            GrocProduct.Description = PutItOnTheList.Description;
            GrocProduct.Isle = PutItOnTheList.Isle;
            GroceriesToGet.Add(GrocProduct);
            GroceriesToGet.Sort(p => p.Description);
            
            
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

        //This is the button on the Left containing general operations
        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            if (GroceryManager.CheckWindowSize(Window.Current))
                { MySplitView.IsPaneOpen = true;
            }
            else
            {
                MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;

            }
        }



        private async void ListBox_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (NewList.IsSelected)
            {
                GroceriesToGet.Clear();
                MySplitView.IsPaneOpen = !GroceryManager.CheckWindowSize(Window.Current);

            }
            else if (OpenList.IsSelected)
            {

                GroceryManager Grok = new GroceryManager();
                FileOpenPicker openPicker = new FileOpenPicker();
                openPicker.ViewMode = PickerViewMode.List;
                openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                openPicker.FileTypeFilter.Add(".xml");
                StorageFile file = await openPicker.PickSingleFileAsync();
                if (file != null)
                {
                    GroceryManager.GetGroceriesFromSavedList(file, GroceriesToGet, true);
                }

                MySplitView.IsPaneOpen = !GroceryManager.CheckWindowSize(Window.Current);

            }
            else if (SaveList.IsSelected)
            {
                
                XDocument doccer = new XDocument();
                GroceryManager v = new GroceryManager();
                v.GetXMLForSaving(GroceriesToGet, doccer);
                FileSavePicker savePicker = new FileSavePicker();
                savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
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
                      // Let Windows know that we're finished changing the file so the other app can update the remote version of the file.
                    // Completing updates may require Windows to ask for user input.
                    FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
                    if (status == FileUpdateStatus.Complete)
                    {
                   
                    }
                    else
                    {
                      
                    }
                }
                else
                {
                }
            }

            MySplitView.IsPaneOpen = !GroceryManager.CheckWindowSize(Window.Current);


        }


     private void CategoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (MyRightMenuSorta.Visibility == Visibility.Collapsed) MyRightMenuSorta.Visibility = Visibility.Visible;
            else MyRightMenuSorta.Visibility = Visibility.Collapsed;
            return;


        }


        private void CategoryListBox_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (Produce.IsSelected)
            {

                GroceryManager.GetGroceriesByAisle("Produce", Groceries);
                if (GroceryManager.CheckWindowSize(Window.Current)) MyRightMenuSorta.Visibility = Visibility.Collapsed;


            }
            else if (Bakery.IsSelected)
            {
                GroceryManager.GetGroceriesByAisle("Bakery", Groceries);
                if( GroceryManager.CheckWindowSize(Window.Current)) MyRightMenuSorta.Visibility = Visibility.Collapsed;
            }
            else if (Dairy.IsSelected)
            {
                GroceryManager.GetGroceriesByAisle("Dairy", Groceries);
                if (GroceryManager.CheckWindowSize(Window.Current)) MyRightMenuSorta.Visibility = Visibility.Collapsed;

            }
            else if (Beverages.IsSelected)
            {
                GroceryManager.GetGroceriesByAisle("Beverages", Groceries);
                if (GroceryManager.CheckWindowSize(Window.Current)) MyRightMenuSorta.Visibility = Visibility.Collapsed;
            }
            else if (Cheese.IsSelected)
            {
                GroceryManager.GetGroceriesByAisle("Cheese", Groceries);
                if (GroceryManager.CheckWindowSize(Window.Current)) MyRightMenuSorta.Visibility = Visibility.Collapsed;

            }
            else if (Deli.IsSelected)
            {
                GroceryManager.GetGroceriesByAisle("Deli", Groceries);
                if (GroceryManager.CheckWindowSize(Window.Current)) MyRightMenuSorta.Visibility = Visibility.Collapsed;
            }
            else if (Fish.IsSelected)
            {
                GroceryManager.GetGroceriesByAisle("Fish", Groceries);
                if (GroceryManager.CheckWindowSize(Window.Current)) MyRightMenuSorta.Visibility = Visibility.Collapsed;

            } else if (Meat.IsSelected)
            {
                GroceryManager.GetGroceriesByAisle("Meat", Groceries);
                if (GroceryManager.CheckWindowSize(Window.Current)) MyRightMenuSorta.Visibility = Visibility.Collapsed;
            }
         
        }

        private void MainPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
          
            if (e.NewSize.Width > 700)
            {
               MySplitView.IsPaneOpen = true;
               //CategorySplitView.IsPaneOpen = true;
              
            }
            else
            {
                MySplitView.IsPaneOpen = false;
                //CategorySplitView.IsPaneOpen = false;
                
           }
            return;

        }
    }

}
