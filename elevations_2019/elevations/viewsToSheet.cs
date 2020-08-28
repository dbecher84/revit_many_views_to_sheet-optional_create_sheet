using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace get_sheet
{
    public partial class viewsToSheet : Form
    {
        List<string> fullList = new List<string>();

        public viewsToSheet(List<string> list)
        {
            InitializeComponent();

            foreach (string item in list)
            {
                //MessageBox.Show(item);
                listBox1.Items.Add(item);
                fullList.Add(item);
            }
        }

        public List<string> sheetForViews { get; set; }

        //create view list to send for export
        public List<string> CreateSheetList()
        {
            List<string> sheetListWorking = new List<string>();
            if (listBox2.Items != null)
            {
                foreach (var item in listBox2.Items)
                {
                    string text = item.ToString();
                    //MessageBox.Show(text);
                    sheetListWorking.Add(text);
                }
                sheetForViews = sheetListWorking;
                return sheetForViews;
            }
            else
            {
                MessageBox.Show("List Box was empty");
                return sheetForViews;
            }
        }

        private void viewsToSheet_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //add item to second list on double click
        private void DoubleClick1(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                listBox2.Items.Clear();
                listBox2.Items.Add(listBox1.SelectedItem);
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //clear list 2
        private void ClearAll(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        //view list to filter list box 1 
        private void Filter_Load(object sender, EventArgs e)
        {
            string uText = textBox1.Text;
            string cText = uText.Trim();
            //MessageBox.Show(cText);
            List<string> filteredList = new List<string>();
            foreach (string item in listBox1.Items)
            {
                if (item.Contains(cText))
                {
                    filteredList.Add(item);
                }
            }
            listBox1.Items.Clear();
            foreach (string item in filteredList)
            {
                listBox1.Items.Add(item);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CreateSheetList();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            foreach (string item in fullList)
            {
                listBox1.Items.Add(item);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            
        }
    }
}
