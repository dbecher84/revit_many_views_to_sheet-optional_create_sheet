using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sheetcreation
{
    public partial class createSheet : Form
    {
        public createSheet()
        {
            InitializeComponent();
        }

        public string createNewSheet { get; set; }

        public string cNewSheet(string response)
        {
            if (response == "yes")
            {
                createNewSheet = "yes";
                return createNewSheet;
            }
            else
            {
                createNewSheet = "no";
                return createNewSheet;
            }
        }


        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            //No
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            //Yes
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                cNewSheet("no");
            }
            if (radioButton2.Checked)
            {
                cNewSheet("yes");
            }
        }

        private void createSheet_Load(object sender, EventArgs e)
        {

        }
    }
}
