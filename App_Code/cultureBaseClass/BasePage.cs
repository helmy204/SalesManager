using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Threading;
using System.Globalization;
using System.Data.Linq;
using System.Linq;
public class BasePage : Page
{


    protected override void InitializeCulture()
    {
        if (Session["culture"] != null)
        {

            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Convert.ToString(Session["culture"]));
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Convert.ToString(Session["culture"]));
        }
     

        base.InitializeCulture();
    }

    
}
     
