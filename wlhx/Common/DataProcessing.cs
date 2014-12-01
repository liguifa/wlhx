using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing.Printing;
using System.Data;
using System.IO;

namespace wlhx.Common
{
    public class DataProcessing
    {
        public static JsonStatus PrintDataTable()
        {
            JsonStatus js = new JsonStatus();
            //PrintDocment objPrintDocment = new PrintDocment();
            //objPrintDocment.PrintPage += new PrintPageEventHandler
            return js;
        }

        public static JsonStatus OutExecl(string path, DataTable dt, string[] titles,string fileName)
        {
            JsonStatus js = new JsonStatus();
            try
            {
                object missing = System.Reflection.Missing.Value;
                Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                app.Application.Workbooks.Add(true);
                Microsoft.Office.Interop.Excel.Workbook book = (Microsoft.Office.Interop.Excel.Workbook)app.ActiveWorkbook;
                Microsoft.Office.Interop.Excel.Worksheet sheet = (Microsoft.Office.Interop.Excel.Worksheet)book.ActiveSheet;
                sheet.Cells[1, 1] = "序号";
                for (int i = 0; i < titles.Length; i++)
                {
                    sheet.Cells[1, i + 2] = titles[i];
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sheet.Cells[i + 2, 1] = i + 1;
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        sheet.Cells[i + 2, j + 2] = dt.Rows[i][j];
                    }
                }
                string fn=DateTime.Now.ToString("yyyyMMddhhmmss")+"@" + fileName;
                string filename = path+"/" + fn+".xlsx";
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }
                book.SaveCopyAs(filename);
                //关闭文件
                book.Close(false, missing, missing);
                //退出excel
                app.Quit();
                js.status = "1";
                js.msg = fn;
            }
            catch(Exception e)
            {
                js.status = "0";
                js.msg = "导出失败！未知错误！"+e.Message;
            }
            return js;
        }

        public JsonStatus ClearSystemIn(string path)
        {
            JsonStatus js = new JsonStatus();
            try
            {
                Directory.Delete(path, true);
                Directory.CreateDirectory(path);
                js.status = "1";
                js.msg = "清除成功！";
            }
            catch
            {
                js.status = "0";
                js.msg = "清除失败！未知错误！";
            }
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return js;
        }
    }
}