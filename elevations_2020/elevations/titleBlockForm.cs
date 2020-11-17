using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace elevations_tblocks
{
    public partial class titleblockform : Form
    {

        //List<string> fullList = new List<string>();

        public titleblockform(List<string> tblocks)
        {
            InitializeComponent();

            foreach (string item in tblocks)
            {
                //MessageBox.Show(item);
                listBox1.Items.Add(item);
                //fullList.Add(item);
            }
        }

        public List<string> tblockToUse { get; set; }

        //create tblock list from form
        public List<string> CreateTblockList()
        {
            List<string> tblockListWorking = new List<string>();
            if (listBox2.Items != null)
            {
                foreach (var item in listBox2.Items)
                {
                    string text = item.ToString();
                    //MessageBox.Show(text);
                    tblockListWorking.Add(text);
                }
                tblockToUse = tblockListWorking;
                return tblockToUse;
            }
            else
            {
                MessageBox.Show("List Box was empty");
                return tblockToUse;
            }
        }

        public String cleanNum { get; set; }
        public String cleanName { get; set; }

        public String cleantxtNum(string txtToClean)
        {
            cleanNum = txtToClean.Replace(" ", "");

            return cleanNum;
        }

        public String cleantxtName(string txtToClean)
        {
            cleanName = txtToClean.Trim();
            return cleanName;
        }


        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CreateTblockList();
            cleantxtNum(textBox1.Text);
            cleantxtName(textBox2.Text);
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void titleblockform_Load(object sender, EventArgs e)
        {

        }
    }
}
