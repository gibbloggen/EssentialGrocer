using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;
using Windows.Storage.Streams;

namespace EssentialGrocer.Model
{
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

    public class GroceryManager
    {

        public void GetXMLForSaving(ObservableCollection<Grocery> GatherGroceries, XDocument Saviour)
        {

            XElement z = new XElement("SavedList");
            Saviour.Add(z);


            foreach (Grocery j in GatherGroceries)
            {
                XElement k = new XElement("Product",
                    new XElement("UPC_Code", j.UPC_Code),
                    new XElement("Description", j.Description),
                    new XElement("Isle", j.Isle));

                Saviour.Element("SavedList").Add(k);
            }
            return;
        }

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


        public async static void GetGroceriesByAisle(string IsleFor, ObservableCollection<Grocery> observableGroceries)
        {
            if (observableGroceries.Count != 0) observableGroceries.Clear();
            Windows.Storage.StorageFile fd = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync("GroceryData.xml");

            Stream jc = await fd.OpenStreamForReadAsync();

            XDocument merde = XDocument.Load(jc);
            //XDocument merde =  XDocument.Load("GroceryData.xml");

            var j = new ObservableCollection<Grocery>();

            var q = from b in merde.Descendants("product")
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
                    observableGroceries.Add(new Grocery { UPC_Code = grocery.UPC_Code, Description = grocery.Description, Isle = grocery.Isle });




        }

        public string getJ()
        {
            XDocument merde = XDocument.Load(@".\Models\GroceryData.xml");
            return merde.ToString();
        }
        public static async void GetGroceriesFromSavedList(StorageFile TheAddition, ObservableCollection<Grocery> ObservableGroceries, bool Append)

        {

            Stream w = await TheAddition.OpenStreamForReadAsync();

            char line;
            string anything = "";
            for (int i = 0; i < w.Length; i++)
            {
                line = (char)w.ReadByte();
                anything += line;
            }


;



            XDocument merde = XDocument.Parse(anything);


            if (Append) { }
            else {
                ObservableGroceries = new ObservableCollection<Grocery>();
            }
            var q = from b in merde.Descendants("Product")
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

        public static ObservableCollection<Grocery> GetGroceriesFromXML()

        {


            XDocument merde = XDocument.Load(@".\Models\GroceryData.xml");


            var j = new ObservableCollection<Grocery>();

            var q = from b in merde.Descendants("product")
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
