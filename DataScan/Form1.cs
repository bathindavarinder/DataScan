using DataScan.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace DataScan
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void openFileDialog1_FileOk_1(object sender, CancelEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string file = "";
            try
            {
                file = this.comboBox1.SelectedItem.ToString();
            }
            catch
            {
                MessageBox.Show("Please Select File Type");
            }
            if (file == "")
            {
                MessageBox.Show("Please Select File Type");
                return;
            }
            string fileName = this.openFileDialog1.FileName;

            if (fileName == "")
            {
                MessageBox.Show("Please Select File");
                return;
            }
            ExcelReader reader = new ExcelReader();
            ShowData frm = new ShowData();
            frm.type = file;

            if (file == "BW")
            {
                try
                {
                    IEnumerable<BW> data = reader.GetBW<BW>(fileName);
                    frm.bw = data;
                    frm.ShowDa();
                }
                catch (System.IO.IOException x)
                {
                    MessageBox.Show("Error : File is already opened. Please closed it first !");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There is some issue with data in the excel. Please check it once!");
                }

            }
            else if (file == "Color")
            {

                try
                {
                    IEnumerable<ColorExcel> data = reader.GetColor<ColorExcel>(fileName);
                    frm.colorExcel = data;
                    frm.ShowDa();
                }
                catch (System.IO.IOException x)
                {
                    MessageBox.Show("Error : File is already opened. Please closed it first !");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There is some issue with data in the excel. Please check it once!");
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk_2(object sender, CancelEventArgs e)
        {
            FileName.Text = this.openFileDialog1.FileName;
        }
    }
}
