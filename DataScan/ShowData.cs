using DataScan.Core;
using Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
namespace DataScan
{
    public partial class ShowData : Form
    {
        public IEnumerable<BW> bw;
        public IEnumerable<ColorExcel> colorExcel;

        public List<BW> conflictbw = new List<BW>();
        public List<ColorExcel> conflictcolorExcel = new List<ColorExcel>();

        public IList<BW> exportBW = new List<BW>();

        public IList<ColorExcel> exportColor = new List<ColorExcel>();

        public string type = "";

        public bool dateColflict = false;

        public ShowData()
        {
            InitializeComponent();
        }
        public void ShowDa()
        {


            this.InitGrid();

            this.Show();
        }

        public void BWInit()
        {
            int row = 0;
            XElement allData = XElement.Load("BWXml.xml");
            IEnumerable<XElement> authors = null;
            if (allData != null)
            {
                authors = allData.Descendants("Data");
            }

            foreach (var b in bw)
            {
                this.dataGridView1.Rows.Add(b.Date.ToShortDateString(), b.SerialNo, b.EquipmentNumber, b.Total);

                if (authors != null)
                {
                    XElement e = authors.Where(x => x.Element("EquipmentNumber").Value == b.EquipmentNumber).FirstOrDefault();
                    if (e != null)
                    {
                        int total = Convert.ToInt32(e.Element("Total").Value);
                        if (Convert.ToInt32(b.Total) < total)
                        {

                            this.dataGridView1.Rows[row].Cells[3].Style.BackColor = Color.Red;
                            conflictbw.Add(b);
                        }
                        else if (Convert.ToInt32(b.Total) > total)
                        {
                            this.dataGridView1.Rows[row].Cells[3].Style.BackColor = Color.Red;
                            conflictbw.Add(b);
                        }
                        DateTime d = Convert.ToDateTime(e.Element("Date").Value.ToString());

                        DateTime date = DateTime.ParseExact(d.ToShortDateString(), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        DateTime newdate = DateTime.ParseExact(b.Date.ToShortDateString(), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        int result = DateTime.Compare(newdate, date);
                        if (result <= 0)
                        {
                            dateColflict = true;
                            this.dataGridView1.Rows[row].Cells[0].Style.BackColor = Color.Green;
                        }
                        //DateTime date = DateTime.ParseExact(e.Element("Date").Value, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        //// DateTime date = Convert.ToDateTime().;
                        //int result = DateTime.Compare(b.Date, date);
                        //if (result <= 0)
                        //{
                        //    dateColflict = true;
                        //    this.dataGridView1.Rows[row].Cells[0].Style.BackColor = Color.Green;
                        //}
                    }
                    else
                    {
                        DateTime date = DateTime.ParseExact(b.Date.ToShortDateString(), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        XDocument document = XDocument.Load("BWXml.xml");
                        document.Root.Add
                               (
                                  new XElement
                                    (
                                            "Data", new XElement("Date", date.ToShortDateString()), new XElement("SerialNumber", b.SerialNo), new XElement("EquipmentNumber", b.EquipmentNumber), new XElement("Total", b.Total)
                                    )
                              );
                        document.Save("BWXml.xml");

                    }
                }
                row++;
            }
        }

        public void ColorInit()
        {
            int row = 0;
            dataGridView1.Columns.Add("Black", "Black");
            dataGridView1.Columns.Add("Color", "Color");


            XElement allData = XElement.Load("ColorXml.xml");
            IEnumerable<XElement> authors = null;
            if (allData != null)
            {
                authors = allData.Descendants("Data");
            }

            foreach (var b in colorExcel)
            {
                this.dataGridView1.Rows.Add(b.Date, b.SerialNo, b.EquipmentNumber, b.Total, b.Black, b.Color);

                if (authors != null)
                {
                    XElement e = authors.Where(x => x.Element("EquipmentNumber").Value == b.EquipmentNumber).FirstOrDefault();
                    if (e != null)
                    {
                        int total = Convert.ToInt32(e.Element("Total").Value);
                        if (Convert.ToInt32(b.Total) < total)
                        {
                            this.dataGridView1.Rows[row].Cells[3].Style.BackColor = Color.Red;
                            conflictcolorExcel.Add(b);
                        }
                        else if (Convert.ToInt32(b.Total) > total)
                        {
                            this.dataGridView1.Rows[row].Cells[3].Style.BackColor = Color.Red;
                            conflictcolorExcel.Add(b);
                        }
                        DateTime d = Convert.ToDateTime(e.Element("Date").Value.ToString());

                        DateTime date = DateTime.ParseExact(d.ToShortDateString(), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        DateTime newdate = DateTime.ParseExact(b.Date.ToShortDateString(), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        int result = DateTime.Compare(newdate, date);
                        if (result <= 0)
                        {
                            dateColflict = true;
                            this.dataGridView1.Rows[row].Cells[0].Style.BackColor = Color.Green;
                        }
                    }
                    else
                    {
                        DateTime date = DateTime.ParseExact(b.Date.ToShortDateString(), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        XDocument document = XDocument.Load("ColorXml.xml");
                        document.Root.Add
                               (
                                  new XElement
                                    (
                                            "Data", new XElement("Date", date.ToShortDateString()), new XElement("SerialNumber", b.SerialNo), new XElement("EquipmentNumber", b.EquipmentNumber), new XElement("Total", b.Total)
                                    )
                              );
                        document.Save("ColorXml.xml");

                    }
                }
                row++;
            }
        }

        public void InitGrid()
        {

            switch (type)
            {
                case "BW":
                    BWInit();
                    break;
                case "Color":
                    ColorInit();
                    break;
            }
            //if (type == "BW")
            //{
            //    BWInit();
            //}
            //if (type == "Color")
            //{
            //    ColorInit();
            //}

            if (!dateColflict)
            {
                if (conflictbw.Count > 0 || conflictcolorExcel.Count > 0)
                {
                    ConfirmBtn.Enabled = true;
                    Exportbtn.Enabled = false;
                }
                else
                {

                    ConfirmBtn.Enabled = true;
                    Exportbtn.Enabled = false;

                }
            }
            else
            {
                ConfirmBtn.Enabled = false;
                Exportbtn.Enabled = false;

            }
        }

        public void BWConfirm()
        {
            foreach (DataGridViewRow r in dataGridView1.Rows)
            {
                DateTime d = Convert.ToDateTime(r.Cells[0].Value.ToString());
                DateTime date = DateTime.ParseExact(d.ToShortDateString(), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                string SerialNumber = r.Cells[1].Value.ToString();
                string EquipmentNumber = r.Cells[2].Value.ToString();
                string Total = r.Cells[3].Value.ToString();

                BW be = new BW();

                be.Date = date;
                be.SerialNo = SerialNumber;
                be.EquipmentNumber = EquipmentNumber;
                be.Total = Total;

                exportBW.Add(be);

                //   DateTime date = DateTime.ParseExact(b.Date.ToString(), "dd-MM-yyyy", CultureInfo.InvariantCulture);
                XDocument document = XDocument.Load("BWDataXML.xml");
                document.Root.Add
                       (
                          new XElement
                            (
                                    "Data", new XElement("Date", date.ToShortDateString()), new XElement("SerialNumber", SerialNumber), new XElement("EquipmentNumber", EquipmentNumber), new XElement("Total", Total)
                            )
                      );
                document.Save("BWDataXML.xml");
                BW b = conflictbw.Where(x => x.EquipmentNumber == EquipmentNumber).FirstOrDefault();
                if (b != null)
                {
                    XDocument documentc = XDocument.Load("BWXml.xml");
                    XElement et = documentc.Descendants("Data").Where(x => x.Element("EquipmentNumber").Value == b.EquipmentNumber).FirstOrDefault();
                    et.SetElementValue("Date", date);
                    et.SetElementValue("Total", Total);
                    documentc.Save("BWXml.xml");
                }
                else
                {
                    XDocument documentc = XDocument.Load("BWXml.xml");
                    XElement et = documentc.Descendants("Data").Where(x => x.Element("EquipmentNumber").Value == EquipmentNumber).FirstOrDefault();
                    et.SetElementValue("Date", date);
                    documentc.Save("BWXml.xml");
                }
            }
        }
        public void ColorConfirm()
        {
            foreach (DataGridViewRow r in dataGridView1.Rows)
            {
                DateTime d = Convert.ToDateTime(r.Cells[0].Value.ToString());
                DateTime date = DateTime.ParseExact(d.ToShortDateString(), "dd-MM-yyyy", CultureInfo.InvariantCulture);  // Convert.ToDateTime(r.Cells[0].Value);
                string SerialNumber = r.Cells[1].Value.ToString();
                string EquipmentNumber = r.Cells[2].Value.ToString();
                string Total = r.Cells[3].Value.ToString();
                string Black = r.Cells[4].Value.ToString();
                string Color = r.Cells[5].Value.ToString();

                ColorExcel be = new ColorExcel();

                be.Date = date;
                be.SerialNo = SerialNumber;
                be.EquipmentNumber = EquipmentNumber;
                be.Total = Total;
                be.Black = Black;
                be.Color = Color;
                exportColor.Add(be);


                XDocument document = XDocument.Load("ColorDataXML.xml");
                document.Root.Add
                       (
                          new XElement
                            (
                                    "Data", new XElement("Date", date.ToShortDateString()), new XElement("SerialNumber", SerialNumber), new XElement("EquipmentNumber", EquipmentNumber), new XElement("Total", Total), new XElement("Black", Black), new XElement("Color", Color)
                            )
                      );
                document.Save("ColorDataXML.xml");
                BW b = conflictbw.Where(x => x.EquipmentNumber == EquipmentNumber).FirstOrDefault();
                if (b != null)
                {
                    XDocument documentc = XDocument.Load("ColorXml.xml");
                    XElement et = documentc.Descendants("Data").Where(x => x.Element("EquipmentNumber").Value == b.EquipmentNumber).FirstOrDefault();
                    et.SetElementValue("Date", date);
                    et.SetElementValue("Total", Total);
                    documentc.Save("ColorXml.xml");
                }
                else
                {
                    XDocument documentc = XDocument.Load("ColorXml.xml");
                    XElement et = documentc.Descendants("Data").Where(x => x.Element("EquipmentNumber").Value == EquipmentNumber).FirstOrDefault();
                    et.SetElementValue("Date", date);
                    documentc.Save("ColorXml.xml");
                }
            }
        }
        private void ConfirmBtn_Click(object sender, EventArgs e)
        {
            ConfirmBtn.Enabled = false;
            Exportbtn.Enabled = true;

            switch (type)
            {
                case "BW":
                    BWConfirm();
                    break;
                case "Color":
                    ColorConfirm();
                    break;
            }
        }
        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
            TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }

        private void Exportbtn_Click(object sender, EventArgs e)
        {
            this.saveFileDialog1.AddExtension = false;
            this.saveFileDialog1.DefaultExt = ".xlsx";
            this.saveFileDialog1.ShowDialog();
            Cursor.Current = Cursors.WaitCursor;
            string directory = this.saveFileDialog1.FileName;
            if (directory == "")
            {
                return;
            }
            DataTable table = null;
            switch (type)
            {
                case "BW":
                    table = ConvertToDataTable<BW>(exportBW);
                    break;
                case "Color":
                    table = ConvertToDataTable<ColorExcel>(exportColor);
                    break;
            }

            ExcelUtlity util = new ExcelUtlity();
            try
            {
                string location = directory;
                bool result = util.WriteDataTableToExcel(table, type, location, "");
                if (result)
                {
                    MessageBox.Show("Excel Exported at : " + location);
                }
            }
            catch
            {
                MessageBox.Show("oops !Some error occured.");
            }
            Cursor.Current = Cursors.Default;
        }

        private void Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
