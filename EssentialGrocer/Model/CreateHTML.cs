using EssentialGrocer.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;


namespace EssentialGrocer
{
    class CreateHTML


    {
        
        public static string CreateThisHTML()
        {

           
            XDocument MasterListX = GroceryManager.ShoppingListX;
            var q = from b in MasterListX.Descendants("product")
                    select new Grocery
                    {
                        UPC_Code = (string)b.Element("UPC_Code").Value,
                        Description = (string)b.Element("Description").Value,
                        Isle = (string)b.Element("Isle").Value,
                    };

            q = q.OrderBy(p => p.Description).OrderBy(p => p.Isle);


            string traverseData = "";

            //traverseData = "<div>This was sent via Essential Grocer, a Windows 10 app,,,<br /> This is your shopping List,,,</div>";
            traverseData = @"<div><table Style=""Width:82%;""><tr><th>Category</th><th>Items</th><td></td></tr>";
            string holdAisle = "";
            bool oneTwo = false;
            foreach(Grocery j in q)
            { 
                if (j.Isle != holdAisle)
                {
                    holdAisle = j.Isle;
                    if (oneTwo)  traverseData += @"<td></td></tr>";
                    traverseData += @"<tr><th>" + j.Isle + "</th><td></td><td></td></tr>";
                    oneTwo = true;
                }
                if (oneTwo)
                {
                    traverseData += @"<tr><td></td><td>[]&nbsp;&nbsp;" + j.Description + "</td>";
                    
                } else
                {
                    traverseData += @"<td>[]&nbsp;&nbsp;" + j.Description + "</td></tr>";
                }

                oneTwo = !oneTwo;



         }
            if (oneTwo) traverseData += "<td></td></tr></table></div>";
            else traverseData += "</table></div>";


            traverseData += "<div><br/><br/><br/>This was sent by Essential Grocer, a Windows 10 app, By Essential Software/John Leone (c) 2016 Available in the Windows Store. <br/><br/> Open Source Code available at github.com/gibbloggen/essentialgrocer<br/><br/></div>";

            return (traverseData);

        
        
        
        }
    }
}
