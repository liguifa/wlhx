using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace wlhx.BLL
{
    public class SaveFile
    {
        public string SaveImageTo(string path, HttpPostedFileBase imgFile)
        {
            string filename = imgFile.FileName;
            string fileExe = Path.GetExtension(filename);
            if (fileExe != ".gif" && fileExe != ".jpg" && fileExe != ".jpeg" && fileExe != ".png" && fileExe != ".bmp")
            {
               return showError("文件格式错误！");
                
            }
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + fileExe;
            string filePath = path +"\\"+ newFileName;
            imgFile.SaveAs(filePath);
           string url= "/upload/" + newFileName;
           return  "{\"error\":0,\"url\":\"" + url + "\"}";
            
        }
        private string showError(string message)
        {
           
            return "{\"error\":1,\"message\":\"" + message + "\"}";
        }

        public string SaveFileTo(string path, HttpPostedFileBase File)
        {
            if (File != null && !String.IsNullOrEmpty(path))
            {
                string filename = File.FileName;
                string fileExe = Path.GetExtension(filename);
                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + fileExe;
                string filePath = path + "\\" + newFileName;
                File.SaveAs(filePath);
                return "../../file/" + newFileName;
            }
            else
            {
                return "标题或文件不能为空.....";
            }
        }

        public  bool FileCheck(string filename)
        {
            string[] filenameArray = filename.Split('.');
            return filenameArray[filenameArray.Length - 1] == "xlsx" || filenameArray[filenameArray.Length - 1] == "xls" ? true : false;
        }

        public DataTable OpenFile(string filename)
        {
            try
            {
                string connectStr = ConfigurationManager.ConnectionStrings["ExcelConnectStr"].ToString().Replace("@", filename);
                OleDbConnection conn = new OleDbConnection(connectStr);
                conn.Open();
                OleDbDataAdapter da = new OleDbDataAdapter("select * from [Sheet1$]", conn);
                DataSet ds = new DataSet();
                da.Fill(ds, "table");
                conn.Close();
                return ds.Tables["table"];
            }
            catch
            {
                return new DataTable();
            }
        }

    }
}