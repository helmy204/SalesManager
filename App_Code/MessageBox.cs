using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;


namespace System.Data.ElkhateebDynamicLinq
{
    /// <summary>
    /// Summary description for Helper
    /// </summary>
    public static partial class MessageBox
    {
        static MessageBox()
        {
            //
            // TODO: Add constructor logic here
            //
            if (HttpContext.Current.Session["culture"] == null)
                HttpContext.Current.Session["culture"] = "en-US";
        }
        public static void Show(this UpdatePanel updatepanel, string message)
        {
            ScriptManager.RegisterStartupScript(updatepanel, updatepanel.GetType(), "MessageBox", "alert('" + message + "')", true);
        }
        public static void Show(this Page page, string message)
        {
            ScriptManager.RegisterClientScriptBlock(page, page.GetType(), "MessageBox", "alert('" + message + "')", true);

        }
        public static void SucceedMessage(this Page page)
        {
            string message = (HttpContext.Current.Session["culture"] == null || HttpContext.Current.Session["culture"].ToString() == "en-US") ? "process Completed successfully." : "العمليه تمت بنجاح";
            Show(page, message);
        }
        public static void FailMessage(this Page page)
        {
            string message = (HttpContext.Current.Session["culture"] ==null || HttpContext.Current.Session["culture"].ToString() == "en-US") ? "process Failed." : "خطأ , فى اتمام العمليه من فضلك حاول مره اخرى";
            Show(page, message);
        }
    }
}
