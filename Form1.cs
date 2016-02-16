using SimpleLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;

namespace SimpleStockAnalyze
{
    public partial class Main : Form
    {
        private Companies comp = new Companies();
        private NasdaqList tix = new NasdaqList();
        public string selected;

        private List<NasdaqList> nasdaqDisplayList = new List<NasdaqList>();
        BindingSource nasDAQBinding = new BindingSource();

        public Main()
        {
            InitializeComponent();
            GetNasdaqList();

            nasDAQBinding.DataSource = comp.Company.ToList();
            companyList.DataSource = nasDAQBinding;

            companyList.DisplayMember = "Display";
            companyList.ValueMember = "Display";
        }
        
        /// <summary>
        /// This downloads the text file that has all the NASDAQ company ticker names. 
        /// Once downloaded, the code reads the file and removes anything that isn't needed.
        /// </summary>
        public void GetNasdaqList()
        {
            string filePath = @"C:\\Users\\John\\Documents\\Visual Studio 2015\\Projects\\ConsoleApplication2\\ConsoleApplication2\\companylist.csv";

            using (StreamReader r = new StreamReader(filePath))
            {

                while ((!r.EndOfStream))
                {
                    var eachLine = r.ReadLine().Replace(@"""", "").Split(',');

                    comp.Company.Add(new NasdaqList
                    {
                        tixName = eachLine[0]
                    });
                }
                comp.Company.RemoveAt(0);
            }
        }

        /// <summary>
        /// This is the event for the search box. The purpose of this is to allow the user to search for a company's ticker name
        /// and adjust the visual list to match what the person is texting (as a suggestion box).
        /// </summary>
        private void searchBox_TextChanged(object sender, EventArgs e)
        {
            //Search box finally works! To search through the list of "y's" just keep pressing y to look through the list for all y's
            //TODO: auotcomplete list that updates with every change in user input
            int indexOfSelected = companyList.FindString(searchBox.Text);
            if (indexOfSelected != -1)
                companyList.SetSelected(indexOfSelected, true);
        }

        //When the Ticker name is selected and the view button is pressed...
        private void viewCompanyButton_Click(object sender, EventArgs e)
        {
            WebClient wc = new WebClient();

            selected = companyList.GetItemText(companyList.SelectedItem);

            //Display the tix name
            Stream stream = wc.OpenRead("http://download.finance.yahoo.com/d/quotes.csv?s=" + selected + "&f=n");
            using (StreamReader name = new StreamReader(stream))
            {
                String fullName = name.ReadToEnd().Replace(@"""","");
                CompanyTixValue.Text = string.Format(fullName);
            }

            //Last known trading price: 
            stream = wc.OpenRead("http://download.finance.yahoo.com/d/quotes.csv?s=" + selected + "&f=l1");
            using (StreamReader name = new StreamReader(stream))
            {
                String lastTrade = name.ReadToEnd().Replace(@"""", "");
                LastTradeValue.Text = string.Format("${0}", lastTrade);
            }

            //50d MA average:
            stream = wc.OpenRead("http://download.finance.yahoo.com/d/quotes.csv?s=" + selected + "&f=m3");
            using (StreamReader name = new StreamReader(stream))
            {
                String twoHundredMA = name.ReadToEnd().Replace(@"""", "");
                Last50MA.Text = string.Format("${0}", twoHundredMA);
            }

            //200d MA average: 
            stream = wc.OpenRead("http://download.finance.yahoo.com/d/quotes.csv?s=" + selected + "&f=m4");
            using (StreamReader name = new StreamReader(stream))
            {
                String twoHundredMA = name.ReadToEnd().Replace(@"""", "");
                Last200MA.Text = string.Format("${0}", twoHundredMA);
            }

            //For the visual part of the google url
            companyGoogleFinanceURL.Text = "www.google.com/finance/" + selected + "";
        }
    }
}
