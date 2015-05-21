using Excel;
//using Excel; 
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace DataScan.Core
{

    public class BW
    {
        public DateTime Date { get; set; }
        public string SerialNo { get; set; }
        public string EquipmentNumber { get; set; }
        public string Total { get; set; }

    }

    public class ColorExcel
    {
        public DateTime Date { get; set; }
        public string SerialNo { get; set; }
        public string EquipmentNumber { get; set; }
        public string Total { get; set; }
        public string Black { get; set; }
        public string Color { get; set; }
    }

    public class ExcelReader
    {

        public IEnumerable<T> GetBW<T>(string filename) where T : BW, new()
        {
            IExcelDataReader excelReader = getexceldatareader(filename, true);

            IEnumerable<DataRow> rows = GetDataRow(excelReader);

            List<T> emps = new List<T>();

            foreach (DataRow dataRow in rows)
            {
                T ce = new T();

                string createddate = Convert.ToDateTime(dataRow["Date"].ToString()).ToString("dd-MM-yyyy");

                ce.Date = DateTime.ParseExact(createddate, "dd-MM-yyyy", CultureInfo.InvariantCulture);//Convert.ToDateTime(dataRow["Date"].ToString()),
                ce.SerialNo = dataRow["SerialNo"].ToString();
                ce.EquipmentNumber = dataRow["EquipmentNumber"].ToString();
                ce.Total = dataRow["Total"].ToString();
                
                emps.Add(ce);
            }
            //IEnumerable<T> emps = rows.Select(dataRow => new T()
            //{
            //    Date = DateTime.ParseExact(dataRow["Date"].ToString(), "dd-MM-yyyy", CultureInfo.InvariantCulture),//Convert.ToDateTime(dataRow["Date"].ToString()),
            //    SerialNo = dataRow["SerialNo"].ToString(),
            //    EquipmentNumber = dataRow["EquipmentNumber"].ToString(),
            //    Total = dataRow["Total"].ToString()

            //}).ToList();

            return emps;

        }
        public List<T> GetColor<T>(string filename) where T : ColorExcel, new()
        {
            IExcelDataReader excelReader = getexceldatareader(filename, true);

            IEnumerable<DataRow> rows = GetDataRow(excelReader);

            List<T> emps=new List<T>();

            foreach (DataRow dataRow in rows)
            {
                T ce = new T();

                string createddate = Convert.ToDateTime(dataRow["Date"].ToString()).ToString("dd-MM-yyyy");

                ce.Date = DateTime.ParseExact(createddate, "dd-MM-yyyy", CultureInfo.InvariantCulture);//Convert.ToDateTime(dataRow["Date"].ToString()),
                ce.SerialNo = dataRow["SerialNo"].ToString();
                ce.EquipmentNumber = dataRow["EquipmentNumber"].ToString();
                ce.Total = dataRow["Total"].ToString();
                ce.Black = dataRow["Black"].ToString();
                ce.Color = dataRow["Color"].ToString();
                emps.Add(ce);
            }

            //IEnumerable<T> emps = rows.Select(dataRow => new T()
            //{
            //    Date = DateTime.ParseExact(dataRow["Date"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture),//Convert.ToDateTime(dataRow["Date"].ToString()),
            //    SerialNo = dataRow["SerialNo"].ToString(),
            //    EquipmentNumber = dataRow["EquipmentNumber"].ToString(),
            //    Total = dataRow["Total"].ToString(),
            //    Black = dataRow["Black"].ToString(),
            //    Color = dataRow["Color"].ToString()
            //}).ToList();

            return emps;

        }


        public IEnumerable<DataRow> GetDataRow(IExcelDataReader excelReader)
        {
            DataSet result = excelReader.AsDataSet(true);
            DataTable ws = result.Tables[0];

             


            return from DataRow row in ws.Rows
                   where row[0].ToString() != ""
                   select row;

        }
        private IExcelDataReader getexceldatareader(string file, bool isfirstrowascolumnnames)
        {

            using (FileStream filestream = File.Open(file, FileMode.Open, FileAccess.Read))
            {
                dynamic datareader = "";

                if (file.EndsWith(".xls"))
                {
                    datareader = ExcelReaderFactory.CreateBinaryReader(filestream);
                }
                else if (file.EndsWith(".xlsx"))
                {
                    datareader = ExcelReaderFactory.CreateOpenXmlReader(filestream);
                }


                datareader.IsFirstRowAsColumnNames = isfirstrowascolumnnames;

                return datareader;
            }

        }


    }
}
