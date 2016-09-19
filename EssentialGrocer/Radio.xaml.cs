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
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using EssentialGrocer.Model;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace EssentialGrocer
{

    
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Radio : Page
    {
        ObservableCollection<Grocery> Grocs = new ObservableCollection<Grocery>();
        public Radio(ObservableCollection<Grocery> newGroc)
        {
            this.InitializeComponent();

            Grocs = newGroc;
            //FillGroceries();

            MakeThePrintOut();


         
        }
       
        private void MakeThePrintOut()
        {


           
            PopulateBlock(TextContent);
            
            
        }
        private RichTextBlock initBlock()
        {

            RichTextBlock gutInitBlock = new RichTextBlock();
            gutInitBlock.Foreground = new SolidColorBrush(Windows.UI.Colors.Black);
            gutInitBlock.FontSize = 18;
            gutInitBlock.OverflowContentTarget = FirstLinkedContainer;
            gutInitBlock.FontFamily = new FontFamily("Courier New");
            gutInitBlock.VerticalAlignment = VerticalAlignment.Top;
            gutInitBlock.HorizontalAlignment = HorizontalAlignment.Left;
            return gutInitBlock;

        }
        private void PopulateBlock(RichTextBlock Blocker)
        {


            bool firstItem = true;
            int firstLength = 0;
            Paragraph paraItem = null;
            Run itemRun = null;

            string CurrentIsle = "None";

            foreach (Grocery j in Grocs)
            {
                if (j.Isle != CurrentIsle)
                {
                    if ((CurrentIsle != "None") && (!firstItem))
                    {
                        paraItem.Inlines.Add(itemRun);
                        Blocker.Blocks.Add(paraItem);

                    }
                    CurrentIsle = j.Isle;
                    firstItem = true;
                    Paragraph paraIsle = new Paragraph();
                    Run paraRan = new Run();
                    paraRan.Text = "  " + j.Isle;
                    paraIsle.Inlines.Add(paraRan);
                    Blocker.Blocks.Add(paraIsle);


                }
                if (firstItem)
                {
                    paraItem = new Paragraph();
                    itemRun = new Run();
                    itemRun.Text = "        [] " + j.Description;
                    firstLength = j.Description.Length;
                    firstItem = false;
                }
                else
                {
                    firstItem = true;
                    string s = new string(' ', 32 - firstLength);
                    itemRun.Text += s + "[] " + j.Description;
                    paraItem.Inlines.Add(itemRun);
                    Blocker.Blocks.Add(paraItem);

                }





            }
            if (!firstItem)
            {
                paraItem.Inlines.Add(itemRun);
                Blocker.Blocks.Add(paraItem);
            }
        }











    }

}

