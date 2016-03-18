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
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;
using Windows.Storage.Streams;

namespace EssentialGrocer.Model
{
    public static class sortableObservable
    {

        /* A paste in from http://stackoverflow.com/questions/1945461/how-do-i-sort-an-observable-collection */

        public static void Sort<TSource, TKey>(this ObservableCollection<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source == null) return;

            Comparer<TKey> comparer = Comparer<TKey>.Default;

            for (int i = source.Count - 1; i >= 0; i--)
            {
                for (int j = 1; j <= i; j++)
                {
                    TSource o1 = source[j - 1];
                    TSource o2 = source[j];
                    if (comparer.Compare(keySelector(o1), keySelector(o2)) > 0)
                    {
                        source.Remove(o1);
                        source.Insert(j, o1);
                    }
                }
            }
        }

    }
    public class Grocery
    {
        private string upc_Code = "00000000";
        public string UPC_Code
        {
            get { return upc_Code; }
            set { upc_Code = value; }
        }
        private string description = "GotNothing";
        public string Description
        {
            get { return description; }

            set { description = value; }
        }
        private string isle = "Clueless";
        public string Isle
        {
            get { return isle; }
            set { isle = value; }
        }

    }



    /// <summary>
    /// Main Class that does the bulk of the work with the Grocery Lists manipulation
    /// Of note, a lot of async stuff, and am learning it as I go.
    /// </summary>
    public class GroceryManager

    {
        static string groceryDataFileName = "GroceryData.xml";
        static string groceryListTempFileName = "GE_Temp.xml";

        static int AmIRunning = 0;
        static int AmIRunningToGet = 0;
        public static ObservableCollection<Grocery> observableGroceries;

        /*   public ObservableCollection<Grocery> observableGroceries
       {
           get { return MainPage.Groceries; }
           set { MainPage.Groceries = value; }
       }*/

        private static XDocument masterListX = null;
        public static XDocument MasterListX
        {
            get { return masterListX; }
            set { masterListX = value; }
        }


        private static XDocument shoppingListX = null;
        public static XDocument ShoppingListX
        {
            get { return shoppingListX; }
            set { shoppingListX = value; }
        }


        public XDocument GetXMLForSavingMaster()
        {
            return(MasterListX);
       
        }

        // GroceryManager.UpdateToGetList(GroceriesToGet);

        public async static void UpdateToGetList(ObservableCollection<Grocery> ToGetList)
        {
            Stream ToGetDataStream = null;
            Boolean Gotten = false;
            XDocument SaveThatTemp = new XDocument();
            // static int i;

            if (AmIRunningToGet == 0)
            {
                do
                {

                    if (AmIRunningToGet++ > 2) Gotten = true;
                    try
                    {
                        ToGetDataStream = await Windows.Storage.ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync(groceryListTempFileName, CreationCollisionOption.ReplaceExisting);

                        Gotten = true;
                        System.Diagnostics.Debug.WriteLine("Got it after:" + AmIRunning.ToString() + "tries");
                    }
                    catch
                    {
                        //System.Diagnostics.Debug.WriteLine("Couldn't get the darn thing open for Write");
                        int j = 7;
                        continue;

                    }

                } while (!Gotten);


                GetXMLForSaving(ToGetList, SaveThatTemp);
                SaveThatTemp.Save(ToGetDataStream);
                //added with Beta 2 issue
                ToGetDataStream.Flush();
                ToGetDataStream.Dispose();




                AmIRunningToGet = 0;

                System.Diagnostics.Debug.WriteLine("Got it after:" + AmIRunning.ToString() + "tries");

            }
            else System.Diagnostics.Debug.WriteLine("Passed One Call");




        }
        /// <summary>
        /// This initialization routine took some doing, I had to realize that because it was async, it was not sending anything back, 
        /// nor is it stopping the calling routine, hence async :-)  So, I am working with putting anything I want to follow these calculations
        /// either called in this method beyond the await, if it has to be done after the async calulations, or, in this case, data reads.
        /// </summary>
        public async static void AsynchInitializingGroceryCollection(ObservableCollection<Grocery> ProduceCategory)
        {

            string IsleFor = "Produce";
            Stream GroceryDataStream = null;
            try
            {
                GroceryDataStream = await Windows.Storage.ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(groceryDataFileName);
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("Couldn't get the darn thing open");
                try
                {
                    string fileName = groceryDataFileName;
                    string sourcePath = @".\Model";
                    string targetPath = ApplicationData.Current.LocalFolder.Path;
                    string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
                    string destFile = System.IO.Path.Combine(targetPath, fileName);
                    if (!System.IO.Directory.Exists(targetPath))
                    {
                        System.Diagnostics.Debug.WriteLine("No Path at local folder" + targetPath);

                    }
                    System.IO.File.Copy(sourceFile, destFile, true);
                    GroceryDataStream = await Windows.Storage.ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(groceryDataFileName);

                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("Couldn't get the darn thing reset");

                }
            }



            //This redundent code with GetGroceryByIsle, for now I am using it to handle the asyncrhonization.
            //That the observableGroceries is static throughout the class, will give that flexibility

            // Stream grocDataStream = await Windows.Storage.ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(grocData);
            MasterListX = XDocument.Load(GroceryDataStream);


            //added with Beta 2 issue

            GroceryDataStream.Flush();
            GroceryDataStream.Dispose();

            //GetGroceriesByAisle("Produce", PassedObservableGroceries);

            //ObservableCollection<Grocery> localGroceryAccess = Application.Groceries;

           
           ProduceCategory.Clear();

            var q = from b in MasterListX.Descendants("product")
                    select new
                    {
                        UPC_Code = (string)b.Element("UPC_Code").Value,
                        Description = (string)b.Element("Description").Value,
                        Isle = (string)b.Element("Isle").Value,
                    };

            q = q.Where(p => p.Isle == IsleFor).OrderBy(p => p.Description);
            q = q.OrderBy(p => p.Description);
            foreach (var grocery in q)
                if (grocery.Isle == IsleFor)
                    ProduceCategory.Add(new Grocery { UPC_Code = grocery.UPC_Code, Description = grocery.Description, Isle = grocery.Isle });
                   








        }


        public async static void AsynchInitializingGroceryToGet(ObservableCollection<Grocery> TheOldListCategory)
        {

            bool newFile = false;
            Stream ShoppingDataStream = null;
            try
            {
                ShoppingDataStream = await Windows.Storage.ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(groceryListTempFileName);
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("Couldn't get the temp List open");
                try
                {



                    ShoppingDataStream = await Windows.Storage.ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync(groceryListTempFileName,CreationCollisionOption.ReplaceExisting);
                    newFile = true;
/*
                    string fileName = groceryListTempFileName;
                    string sourcePath = @".\Model";
                    string targetPath = ApplicationData.Current.LocalFolder.Path;
                    string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
                    string destFile = System.IO.Path.Combine(targetPath, fileName);
                    if (!System.IO.Directory.Exists(targetPath))
                    {
                        System.Diagnostics.Debug.WriteLine("No Path at local folder" + targetPath);

                    }
                    System.IO.File.Copy(sourceFile, destFile, true);
                    GroceryDataStream = await Windows.Storage.ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(groceryDataFileName);
                    */
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("Couldn't get the darn thing reset");

                }
            }

            if (ShoppingDataStream.Length == 0) newFile = true;

            //This redundent code with GetGroceryByIsle, for now I am using it to handle the asyncrhonization.
            //That the observableGroceries is static throughout the class, will give that flexibility

            // Stream grocDataStream = await Windows.Storage.ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(grocData);
            if(!newFile)
              ShoppingListX = XDocument.Load(ShoppingDataStream);


            //added with Beta 2 issue

            ShoppingDataStream.Flush();
            ShoppingDataStream.Dispose();

            //GetGroceriesByAisle("Produce", PassedObservableGroceries);

            //ObservableCollection<Grocery> localGroceryAccess = Application.Groceries;


            //ProduceCategory.Clear();
            if (!newFile)
            {
                var q = from b in ShoppingListX.Descendants("product")
                        select new
                        {
                            UPC_Code = (string)b.Element("UPC_Code").Value,
                            Description = (string)b.Element("Description").Value,
                            Isle = (string)b.Element("Isle").Value,
                        };

                //q = q.Where(p => p.Isle == IsleFor).OrderBy(p => p.Description);
                q = q.OrderBy(p => p.Description);
                foreach (var grocery in q)
                {
                    TheOldListCategory.Add(new Grocery { UPC_Code = grocery.UPC_Code, Description = grocery.Description, Isle = grocery.Isle });

                }
            }
        }



        private async static void SaveMasterList()
        {
            Stream GroceryDataStream = null;
            Boolean Gotten = false;
           // static int i;

            if (AmIRunning == 0) {
                do
                {

                    if (AmIRunning++ > 2) Gotten = true;
                    try
                    {
                        GroceryDataStream = await Windows.Storage.ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync(groceryDataFileName, CreationCollisionOption.ReplaceExisting);
                       
                        Gotten = true;
                        System.Diagnostics.Debug.WriteLine("Got it after:" + AmIRunning.ToString() + "tries");
                    }
                    catch
                    {
                        //System.Diagnostics.Debug.WriteLine("Couldn't get the darn thing open for Write");
                        int j = 7;
                        continue;

                    }

                } while (!Gotten);

                MasterListX.Save(GroceryDataStream);
                //added with Beta 2 issue
                GroceryDataStream.Flush();
                GroceryDataStream.Dispose();




                AmIRunning = 0;

                System.Diagnostics.Debug.WriteLine("Got it after:" + AmIRunning.ToString() + "tries");

            }
            else System.Diagnostics.Debug.WriteLine("Passed One Call");

        } 

        public static Boolean CheckWindowSize(Windows.UI.Xaml.Window CurrentWndow, int DesiredWidth = 1200)
        {
            if (CurrentWndow.Bounds.Width < DesiredWidth) return true; else return (false);



        }


        public static Boolean AddToList(ObservableCollection<Grocery> Groceries, string IsleCategory, string ItemDescription)
        {


            // ObservableCollection<Grocery> GroceriesMaster = new ObservableCollection<Grocery>();


            switch (IsleCategory)
            {

                case "Add To Bakery":
                    UpdatingMasterList(ItemDescription, "Bakery");
                    GetGroceriesByAisle("Bakery", Groceries);
                    System.Diagnostics.Debug.WriteLine("Got Bakery");
                    break;


                case "Add To Bake Spice":

                    UpdatingMasterList(ItemDescription, "BakeSpice");
                    GetGroceriesByAisle("BakeSpice", Groceries);

                    System.Diagnostics.Debug.WriteLine("Got Bake Spice");
                    break;


                case "Add To Produce":

                    UpdatingMasterList(ItemDescription, "Produce");
                    GetGroceriesByAisle("Produce", Groceries);

                    System.Diagnostics.Debug.WriteLine("Got Produce");
                    break;

                case "Add To Dairy":

                    UpdatingMasterList(ItemDescription, "Dairy");
                    GetGroceriesByAisle("Dairy", Groceries);

                    System.Diagnostics.Debug.WriteLine("Got Dairy");
                    break;

                case "Add To Beverages":

                    UpdatingMasterList(ItemDescription, "Beverages");
                    GetGroceriesByAisle("Beverages", Groceries);

                    System.Diagnostics.Debug.WriteLine("Got Beverages");
                    break;

                case "Add To Cheese":

                    UpdatingMasterList(ItemDescription, "Cheese");
                    GetGroceriesByAisle("Cheese", Groceries);

                    System.Diagnostics.Debug.WriteLine("Got Cheese");
                    break;

                case "Add To Deli":

                    UpdatingMasterList(ItemDescription, "Deli");
                    GetGroceriesByAisle("Deli", Groceries);

                    System.Diagnostics.Debug.WriteLine("Got Deli");
                    break;

                case "Add To Fish":

                    UpdatingMasterList(ItemDescription, "Fish");
                    GetGroceriesByAisle("Fish", Groceries);

                    System.Diagnostics.Debug.WriteLine("Got Fish");
                    break;

                case "Add To Meat":

                    UpdatingMasterList(ItemDescription, "Meat");
                    GetGroceriesByAisle("Meat", Groceries);

                    System.Diagnostics.Debug.WriteLine("Got Meat");
                    break;

                case "Add To Frozen":

                    UpdatingMasterList(ItemDescription, "Frozen");
                    GetGroceriesByAisle("Frozen", Groceries);

                    System.Diagnostics.Debug.WriteLine("Got Frozen");
                    break;

                case "Add To Canned Jarred":

                    UpdatingMasterList(ItemDescription, "CannedJarred");
                    GetGroceriesByAisle("CannedJarred", Groceries);

                    System.Diagnostics.Debug.WriteLine("Got Canned Jarred");
                    break;

                case "Add To Noodles Pasta":

                    UpdatingMasterList(ItemDescription, "NoodlesPasta");
                    GetGroceriesByAisle("NoodlesPasta", Groceries);

                    System.Diagnostics.Debug.WriteLine("Got Noodles Pasta");
                    break;

                case "Add To Condiments":

                    UpdatingMasterList(ItemDescription, "Condiments");
                    GetGroceriesByAisle("Condiments", Groceries);

                    System.Diagnostics.Debug.WriteLine("Got Condiments");
                    break;

                case "Add To Cereal":

                    UpdatingMasterList(ItemDescription, "Cereal");
                    GetGroceriesByAisle("Cereal", Groceries);

                    System.Diagnostics.Debug.WriteLine("Got Cereal");
                    break;





            }

            SaveMasterList();

            return true;


        }

        /*public async static void pGetGroceriesMaster(ObservableCollection<Grocery> observableGroceries)
        {

            //his opening bit is to make sure that the GroceryData.xml file is in the Local State folder
            //in the users profile.  During Development the path is c:/users/gibbl/appdata/local/Packages/<gibberish>/LocalState
            //If it isn't in that folder, then it copies a new one from the source code.
            Windows.Storage.StorageFile GroceryData = null;

            if (observableGroceries.Count != 0) observableGroceries.Clear();

            try
            {
                GroceryData = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync("GroceryData.xml");
            }
            catch
            {
                System.Diagnostics.Debug.WriteLine("Couldn't get the darn thing open");
                try
                {
                    string fileName = "GroceryData.xml";
                    string sourcePath = @".\Model";
                    string targetPath = ApplicationData.Current.LocalFolder.Path;
                    string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
                    string destFile = System.IO.Path.Combine(targetPath, fileName);
                    if (!System.IO.Directory.Exists(targetPath))
                    {
                        System.Diagnostics.Debug.WriteLine("No Path at local folder" + targetPath);

                    }
                    System.IO.File.Copy(sourceFile, destFile, true);
                    GroceryData = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync("GroceryData.xml");

                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("Couldn't get the darn thing reset");

                }
            }

            // Need to add the SuperMarket Tag, I think,,,

            Stream jc = await GroceryData.OpenStreamForReadAsync();

            XDocument GrocXML = XDocument.Load(jc);
            //XDocument GrocXML =  XDocument.Load("GroceryData.xml");

            var j = new ObservableCollection<Grocery>();

            var q = from b in GrocXML.Descendants("product")
                    select new
                    {
                        UPC_Code = (string)b.Element("UPC_Code").Value,
                        Description = (string)b.Element("Description").Value,
                        Isle = (string)b.Element("Isle").Value,
                    };

           // q = q.OrderBy(p => p.Description);
            //q = q.OrderBy(p => p.Description);
            foreach (var grocery in q)
               // if (grocery.Isle == IsleFor)
                    observableGroceries.Add(new Grocery { UPC_Code = grocery.UPC_Code, Description = grocery.Description, Isle = grocery.Isle });

            //observableGroceries = j;
            return;


        }*/

        public static void UpdatingMasterList(string describer, string IsleEr)
        {

            // string grocData = "GroceryData.xml";
            //  Stream  grocDataStream = await Windows.Storage.ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(grocData);

            //  XDocument masterListX = XDocument.Load(grocDataStream);
            // grocDataStream = null;
            XElement newProduct = masterListX.Element("SuperMarket");

            Random q = new Random();
            Int32 numberPlease = q.Next(99999999);

            newProduct.Add(new XElement("product",
                new XElement("Isle", IsleEr),
                new XElement("Description", describer),
                new XElement("UPC_Code", numberPlease)));

            int r = 1;

            //Stream grocDataStreamWrite = await Windows.Storage.ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync(grocData, CreationCollisionOption.ReplaceExisting);
            //masterListX.Save(grocDataStreamWrite);





            // return;

            /*   foreach (Grocery j in GroceriesMaster)
               {
                   XElement k = new XElement("Product",
                       new XElement("UPC_Code", j.UPC_Code),
                       new XElement("Description", j.Description),
                       new XElement("Isle", j.Isle));

                   Saviour.Element("SuperMarket").Add(k);
               }

               Random q = new Random();
               Int32 numberPlease = q.Next(99999999);

               XElement v = new XElement("Product",
                   new XElement("UPC_Code", numberPlease),
                   new XElement("Description", describer),
                   new XElement("Isle", IsleEr));

               Saviour.Element("SuperMarket").Add(v);



               string fileName = "GroceryData.xml";
               // string sourcePath = @".\Model";
               string targetPath = ApplicationData.Current.LocalFolder.Path;
               //string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
               string destFile = System.IO.Path.Combine(targetPath, fileName);

               if (File.Exists(destFile)) {
                   File.Delete(destFile);
   }
               FileStream jazz = File.Create(destFile);
               Saviour.Save(jazz);


               return;*/
        }


        //This is a brute force method of creating xml file to save on the system.
        //Can probably be done more Elegantly, but it works.
        public static void GetXMLForSaving(ObservableCollection<Grocery> GatherGroceries, XDocument Saviour)
        {

            XElement z = new XElement("SavedList");
            Saviour.Add(z);


            foreach (Grocery j in GatherGroceries)
            {
                XElement k = new XElement("product",
                    new XElement("UPC_Code", j.UPC_Code),
                    new XElement("Description", j.Description),
                    new XElement("Isle", j.Isle));

                Saviour.Element("SavedList").Add(k);
            }
            return;
        }

        // This function was just used for development, will be deleted in future versions.
        public static List<Grocery> GetGroceries()
        {
            var Groceries = new List<Grocery>();

            Groceries.Add(new Grocery { UPC_Code = "1111111", Description = "Hot Dogs", Isle = "Produce" });
            Groceries.Add(new Grocery { UPC_Code = "1111112", Description = "Pastrami", Isle = "Produce" });
            Groceries.Add(new Grocery { UPC_Code = "1111113", Description = "Coleslaw", Isle = "Produce" });
            Groceries.Add(new Grocery { UPC_Code = "1111114", Description = "Pickles", Isle = "Produce" });
            Groceries.Add(new Grocery { UPC_Code = "1111115", Description = "Seltzer", Isle = "Produce" });

            return Groceries;
        }



        // This procedure orders the main grocery list in such a way that it works with the various filters.
        // The sort is by isle and then by product alpabetically.

        public static void GetGroceriesByAisle(string IsleFor, ObservableCollection<Grocery> bringingItOver)
        {

            //his opening bit is to make sure that the GroceryData.xml file is in the Local State folder
            //in the users profile.  During Development the path is c:/users/gibbl/appdata/local/Packages/<gibberish>/LocalState
            //If it isn't in that folder, then it copies a new one from the source code.

            // ObservableCollection<Grocery> localGroceryAccess = Application.Groceries;


            // Stream jc = await GroceryData.OpenStreamForReadAsync();

            // XDocument GrocXML = XDocument.Load(jc);
            //XDocument GrocXML =  XDocument.Load("GroceryData.xml");

            bringingItOver.Clear();

            var q = from b in MasterListX.Descendants("product")
                    select new
                    {
                        UPC_Code = (string)b.Element("UPC_Code").Value,
                        Description = (string)b.Element("Description").Value,
                        Isle = (string)b.Element("Isle").Value,
                    };

            q = q.Where(p => p.Isle == IsleFor).OrderBy(p => p.Description);
            q = q.OrderBy(p => p.Description);
            foreach (var grocery in q)
                if (grocery.Isle == IsleFor)
                    bringingItOver.Add(new Grocery { UPC_Code = grocery.UPC_Code, Description = grocery.Description, Isle = grocery.Isle });


            return;

        }

        //this is just for testing, will be deleted in future releases.
        public string getJ()
        {
            XDocument GrocXML = XDocument.Load(@".\Model\GroceryData.xml");
            return GrocXML.ToString();
        }



        // This routine appends the xml file to the existing grocery list, so that you can add either recipe lists, or 
        // a list that is one of your standards.

        // It parses traverses the file and puts it in a long string, then XDocument.Parse turns it into xml

        public static async void GetGroceriesFromSavedList(StorageFile TheAddition, ObservableCollection<Grocery> ObservableGroceries, bool Append)
        {
            Stream w = await TheAddition.OpenStreamForReadAsync();

            char line;
            string anything = "";
            for (int i = 0; i < w.Length; i++)
            {
                line = (char)w.ReadByte();
                anything += line;
            };


            XDocument GrocXML = XDocument.Parse(anything);


            if (Append) { }
            else {
                ObservableGroceries = new ObservableCollection<Grocery>();
            }
            var q = from b in GrocXML.Descendants("Product")
                    select new
                    {
                        UPC_Code = (string)b.Element("UPC_Code").Value,
                        Description = (string)b.Element("Description").Value,
                        Isle = (string)b.Element("Isle").Value,
                    };

            ObservableCollection<Grocery> j = new ObservableCollection<Grocery>();
            foreach (var grocery in q)
                j.Add(new Grocery { UPC_Code = grocery.UPC_Code, Description = grocery.Description, Isle = grocery.Isle });

            var z = j.OrderBy(p => p.Description);

            foreach (Grocery g in z)
                ObservableGroceries.Add(new Grocery { UPC_Code = g.UPC_Code, Description = g.Description, Isle = g.Isle });
            return;

        }




        // This was a paste from somewhere.  I believe it is going to be another delete,,,
        public static async Task<byte[]> ReadFile(StorageFile file)

        {

            byte[] fileBytes = null;

            using (IRandomAccessStreamWithContentType stream = await file.OpenReadAsync())

            {

                fileBytes = new byte[stream.Size];

                using (DataReader reader = new DataReader(stream))

                {

                    await reader.LoadAsync((uint)stream.Size);

                    reader.ReadBytes(fileBytes);

                }

            }



            return fileBytes;

        }


        // This works, but is dependent on the file being in Model folder of the bin.
        // This is going to be updated to have this list an others in the app folder for the user.
        public static ObservableCollection<Grocery> GetGroceriesFromXML()

        {


            XDocument GrocXML = XDocument.Load(@".\Model\GroceryData.xml");


            var j = new ObservableCollection<Grocery>();

            var q = from b in GrocXML.Descendants("product")
                    select new
                    {
                        UPC_Code = (string)b.Element("UPC_Code").Value,
                        Description = (string)b.Element("Description").Value,
                        Isle = (string)b.Element("Isle").Value,
                    };

            foreach (var grocery in q)
                j.Add(new Grocery { UPC_Code = grocery.UPC_Code, Description = grocery.Description, Isle = grocery.Isle });



            return j;

        }


        // I think this was just for testing, will delete in next cleanup.
        public class GroceryManagerObservable
        {

            public static ObservableCollection<Grocery> GetThemGroceries()

            {
                var Groceries = new ObservableCollection<Grocery>();

                Groceries.Add(new Grocery { UPC_Code = "1111111", Description = "Hot Dogs", Isle = "Produce" });
                Groceries.Add(new Grocery { UPC_Code = "1111112", Description = "Pastrami", Isle = "Produce" });
                Groceries.Add(new Grocery { UPC_Code = "1111113", Description = "Coleslaw", Isle = "Produce" });
                Groceries.Add(new Grocery { UPC_Code = "1111114", Description = "Pickles", Isle = "Produce" });
                Groceries.Add(new Grocery { UPC_Code = "1111115", Description = "Seltzer", Isle = "Produce" });

                return Groceries;
            }
        }
    }
}
