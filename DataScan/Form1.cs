using DataScan.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
namespace DataScan
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            initPath();
        }
        public string path = "";

        public void initPath()
        {
            XElement allData = XElement.Load("Settings.xml");
            XElement pat = null;
            pat = allData.Descendants("path").FirstOrDefault();
            path = pat.Value.ToString();
            folderPathTxt.Text = this.path;
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
            if (this.path == "")
            {
                MessageBox.Show("Please set folder path first using settings!");
                return;
            }
            if (file == "BW")
            {
                try
                {
                    IEnumerable<BW> data = reader.GetBW<BW>(fileName);
                    frm.bw = data;
                    frm.path = this.path;
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
                    frm.path = this.path;
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

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SettingsPanel.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.folderBrowserDialog1.ShowDialog();
            string path = this.folderBrowserDialog1.SelectedPath;
            folderPathTxt.Text = path;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            DirectoryInfo di = new DirectoryInfo(this.folderBrowserDialog1.SelectedPath);
            FileInfo[] TXTFiles = di.GetFiles("*.xml");
            if (TXTFiles.Length == 4)
            {
                FileInfo BWDataXML = TXTFiles.Where(x => x.Name == "BWDataXML.xml").FirstOrDefault();
                if (BWDataXML == null)
                {
                    MessageBox.Show("Xml Files are missing in the selected Folder!");
                    return;
                }
                FileInfo BWXml = TXTFiles.Where(x => x.Name == "BWXml.xml").FirstOrDefault();
                if (BWXml == null)
                {
                    MessageBox.Show("Xml Files are missing in the selected Folder!");
                    return;
                }
                FileInfo ColorDataXML = TXTFiles.Where(x => x.Name == "ColorDataXML.xml").FirstOrDefault();
                if (ColorDataXML == null)
                {
                    MessageBox.Show("Xml Files are missing in the selected Folder!");
                    return;
                }
                FileInfo ColorXml = TXTFiles.Where(x => x.Name == "ColorXml.xml").FirstOrDefault();
                if (ColorXml == null)
                {
                    MessageBox.Show("Xml Files are missing in the selected Folder!");
                    return;
                }

            }
            else
            {
                MessageBox.Show("Xml Files are missing in the selected Folder!");
                return;
            }
            XDocument documentc = XDocument.Load("Settings.xml");
            XElement et = documentc.Descendants("path").FirstOrDefault();
            et.Value = this.folderBrowserDialog1.SelectedPath;
            documentc.Save("Settings.xml");
            MessageBox.Show("Path has been saved!");
            this.path = this.folderBrowserDialog1.SelectedPath;
            folderPathTxt.Text = this.path;
            this.SettingsPanel.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.SettingsPanel.Hide();
        }
    }
}
