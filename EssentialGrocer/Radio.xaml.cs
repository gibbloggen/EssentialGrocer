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


            /*
            RichTextBlock gutOne = new RichTextBlock();
            gutOne.Foreground = new SolidColorBrush(Windows.UI.Colors.Black);
            gutOne.FontSize = 18;
            // gutOne.OverflowContentTarget = FirstLinkedContainer;
            gutOne.FontFamily = new FontFamily("Courier New");
            //gutOne.Tag = "Grid.Row=\"1\" Grid.ColumnSpan=\"2\" ";
            gutOne.VerticalAlignment = VerticalAlignment.Top;
            gutOne.HorizontalAlignment = HorizontalAlignment.Left;

            Paragraph para = new Paragraph();
            // para.Margin = new Thickness(5);

            Run ran = new Run();
            ran.Text = "     Meats";
            para.Inlines.Add(ran);
            gutOne.Blocks.Add(para);

            Paragraph para2 = new Paragraph();
            //  para2.Margin = new Thickness(65);

            Run ran2 = new Run();
            ran2.Text = "          [] Pork Butt                                                             [] Hamburger";
            para2.Inlines.Add(ran2);




            gutOne.Blocks.Add(para2);


            ContentStack.Children.Add(gutOne);*/

        }
       /* public void FillGroceries()
        {
            Grocery j = new Grocery();
            j.Isle = "Produce";
            j.Item = "Apple";
            Grocs.Add(j);

            j = new Grocery();
            j.Isle = "Produce";
            j.Item = "Avacodo";
            Grocs.Add(j);

            j = new Grocery();
            j.Isle = "Produce";
            j.Item = "Bannana";
            Grocs.Add(j);

            j = new Grocery();
            j.Isle = "Produce";
            j.Item = "Cantaloupe";
            Grocs.Add(j);

            j = new Grocery();
            j.Isle = "Produce";
            j.Item = "Corn";
            Grocs.Add(j);

            j = new Grocery();
            j.Isle = "Produce";
            j.Item = "Edamame";
            Grocs.Add(j);

            j = new Grocery();
            j.Isle = "Produce";
            j.Item = "Kale";
            Grocs.Add(j);


            j = new Grocery();
            j.Isle = "Dairy";
            j.Item = "Butter";
            Grocs.Add(j);


            j = new Grocery();
            j.Isle = "Dairy";
            j.Item = "Cottage Cheese";
            Grocs.Add(j);

            j = new Grocery();
            j.Isle = "Dairy";
            j.Item = "Chocolate Milk";
            Grocs.Add(j);


            j = new Grocery();
            j.Isle = "Dairy";
            j.Item = "Half-N-Half";
            Grocs.Add(j);

            j = new Grocery();
            j.Isle = "Dairy";
            j.Item = "Heavy Cream";
            Grocs.Add(j);


            j = new Grocery();
            j.Isle = "Dairy";
            j.Item = "Eggs";
            Grocs.Add(j);


            j = new Grocery();
            j.Isle = "Dairy";
            j.Item = "Skim Milk";
            Grocs.Add(j);

            j = new Grocery();
            j.Isle = "Dairy";
            j.Item = "Sour Cream";
            Grocs.Add(j);

            j = new Grocery();
            j.Isle = "Dairy";
            j.Item = "Yogurt";
            Grocs.Add(j);




            j = new Grocery();
            j.Isle = "Paper";
            j.Item = "Napkins";
            Grocs.Add(j);



        }*/
        private void MakeThePrintOut()
        {


            RichTextBlock gutOne = initBlock();
            PopulateBlock(gutOne);
            ContentStack.Children.Add(gutOne);
            /* RichTextBlock gutOne = new RichTextBlock();
             gutOne.Foreground = new SolidColorBrush(Windows.UI.Colors.Black);
             gutOne.FontSize = 18;
             gutOne.OverflowContentTarget = FirstLinkedContainer;
             gutOne.FontFamily = new FontFamily("Courier New");
             gutOne.VerticalAlignment = VerticalAlignment.Top;
             gutOne.HorizontalAlignment = HorizontalAlignment.Left;

     */




            /*

                        Paragraph para = new Paragraph();
                        // para.Margin = new Thickness(5);

                        Run ran = new Run();
                        ran.Text = "     Meats";
                        para.Inlines.Add(ran);
                        gutOne.Blocks.Add(para);

                        Paragraph para2 = new Paragraph();
                        //  para2.Margin = new Thickness(65);

                        Run ran2 = new Run();
                        ran2.Text = "        [] Pork Butt                 [] Hamburger";
                        para2.Inlines.Add(ran2);




                        gutOne.Blocks.Add(para2);

                */

            // TextContent. = gutOne;
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
        private void PopulateBlock( RichTextBlock Blocker)
        {


            bool firstItem = true;
            int firstLength = 0;
            Paragraph paraItem = null;
            Run itemRun = null;

            string CurrentIsle = "None";

            foreach( Grocery j in Grocs)
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
                    paraRan.Text = "     " + j.Isle;
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
                } else
                {
                    firstItem = true;
                    string s = new string(' ', 30 - firstLength);
                    itemRun.Text += s + "[] " +  j.Description;
                    paraItem.Inlines.Add(itemRun);
                    Blocker.Blocks.Add(paraItem);

                }

               
                //  para.Inlines.Add(ran);
                // Blocker.Blocks.Add(para);

              /*  Paragraph para2 = new Paragraph();
                    //  para2.Margin = new Thickness(65);

                    Run ran2 = new Run();
                    ran2.Text = "        [] Pork Butt                 [] Hamburger";
                    para2.Inlines.Add(ran2);

    */


                  //  Blocker.Blocks.Add(para2);




                }
            if (!firstItem)
            {
                paraItem.Inlines.Add(itemRun);
                Blocker.Blocks.Add(paraItem);
            }
        }






           
        }

    }

