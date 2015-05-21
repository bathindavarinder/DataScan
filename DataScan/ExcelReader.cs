using Excel;
//using Excel; 
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace DataScan.Core
{

    public class BW
    {
        public DateTime Date { get; set; }
        public string SerialNo { get; set; }
        public string EquipmentNumber{ get; set; }
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

            IEnumerable<T> emps = rows.Select(dataRow => new T()
            {
                Date = Convert.ToDateTime(dataRow["Date"].ToString()),
                SerialNo = dataRow["SerialNo"].ToString(),
                EquipmentNumber = dataRow["EquipmentNumber"].ToString(),
                Total = dataRow["Total"].ToString() 
               
            }).ToList();

            return emps;

        }
        public IEnumerable<T> GetColor<T>(string filename) where T : ColorExcel, new()
        {
            IExcelDataReader excelReader = getexceldatareader(filename, true);

            IEnumerable<DataRow> rows = GetDataRow(excelReader);

            IEnumerable<T> emps = rows.Select(dataRow => new T()
            {
                Date = Convert.ToDateTime(dataRow["Date"].ToString()),
                SerialNo = dataRow["SerialNo"].ToString(),
                EquipmentNumber = dataRow["EquipmentNumber"].ToString(),
                Total = dataRow["Total"].ToString(),
                Black = dataRow["Black"].ToString(),
                Color = dataRow["Color"].ToString()  
            }).ToList();

            return emps;

        }

        //public IEnumerable<T> GetData<T>(string filename) where T : AssignProjectModel, new()
        //{
        //    IExcelDataReader excelReader = getexceldatareader(filename, true);

        //    IEnumerable<DataRow> rows = GetDataRow(excelReader);

        //    IEnumerable<T> emps = rows.Select(dataRow => new T()
        //    {
                
        //        projectid = Convert.ToInt32(dataRow["ProjectID"].ToString()),
        //        projectName = dataRow["Project"].ToString(),
        //        StartDate = Convert.ToDateTime(dataRow["Start Date"].ToString()),
        //        EndDate = Convert.ToDateTime(dataRow["End Date"].ToString()),
        //        VirtualPay = Convert.ToDecimal(dataRow["Virtual Pay"].ToString()),
        //        OvertimePay = Convert.ToDecimal(dataRow["Overtime Rate"].ToString()),
        //        HolidayPay = Convert.ToDecimal(dataRow["Sunday/Holiday Allowance"].ToString()),
        //        Levy = Convert.ToInt32(dataRow["Levy"].ToString()),
        //        FINIC = dataRow["FINIC"].ToString(),
        //    }).ToList();

        //    return emps;

        //}

        public IEnumerable<DataRow> GetDataRow(IExcelDataReader excelReader)
        {
            DataSet result = excelReader.AsDataSet();
            DataTable ws = result.Tables[0];

            return from DataRow row in ws.Rows where  row[0].ToString() != ""
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
