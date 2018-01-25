using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AjaxControlToolkit;
using System.IO;





namespace System.Data.ElkhateebDynamicLinq
{
    /// <summary>
    /// Summary description for ExtenssionMethods
    /// </summary>
    public static class ExtenssionMethods
    {
        //public ExtenssionMethods()
        //{
        //    //
        //    // TODO: Add constructor logic here
        //    //
        //}
        public static bool UploadFile(this AsyncFileUpload FileUpload, string extendname, string path, out string FileName)
        {

            string file = extendname + FileUpload.FileName;
            FileUpload.SaveAs(HttpContext.Current.Server.MapPath(path + file));
            FileName = file;
            return true;
        }
        public static bool RemoveFile(this string Path)
        {
            if (File.Exists(Path))
            {
                File.Delete(Path);
            }
            return true;
        }
    }
}