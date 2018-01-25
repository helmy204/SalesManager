using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Web.UI.WebControls;
using System.Data;
using System.ComponentModel;

/// <summary>
/// Summary description for CustomsConverter
/// </summary>
public static  class CustomsConverter
{
     static  CustomsConverter()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public static string GetHijriDate(string GregorianDate)
    {
        CultureInfo culture = new CultureInfo("ar-SA");
        char[] split = new char[3];
        split[0] = '/';
        string[] date = GregorianDate.Split(split);
        DateTime dt = new DateTime(Convert.ToInt32(date[2]), Convert.ToInt32(date[0]), Convert.ToInt32(date[1]));
        return dt.ToString("MM/dd/yyyy", culture);

    }

    public static Table ConvertToHtmlTable(this DataTable dt,string reportName)
    {
        int totalrows = dt.Rows.Count;
        int totalColumns = dt.Columns.Count;



        Table Table1 = new Table();
        Table1.CssClass = "mytable";
        Table1.CellSpacing = 0;


        TableRow trcation = new TableRow();
        TableCell tdcaption = new TableCell();
        tdcaption.ColumnSpan = totalColumns;
        tdcaption.CssClass = "xcaption";
        trcation.CssClass = "xcaption";
        tdcaption.Text = "<h1>"+reportName +"</h1>";

        trcation.Cells.Add(tdcaption);
        Table1.Rows.Add(trcation);


        TableHeaderRow headerrow = new TableHeaderRow();
        headerrow.CssClass = "mytable";
        for (int j = 0; j < totalColumns; j++)
        {
            TableHeaderCell col = new TableHeaderCell();
            col.CssClass = "mytable";
            col.Text = dt.Columns[j].Caption;


            headerrow.Cells.Add(col);
        }

        Table1.Rows.Add(headerrow);


        for (int i = 0; i < totalrows; ++i)
        {
            TableRow tr = new TableRow();
            tr.CssClass = "mytable";
            for (int j = 0; j < totalColumns; ++j)
            {
                TableCell col = new TableCell();
                col.Text = dt.Rows[i][j].ToString();
                col.CssClass = "mytable";
                tr.Cells.Add(col);
            }
            Table1.Rows.Add(tr);
        }
        return Table1;

    }

    public static  DataTable ToDataTable<T>(IList<T> list)
    { 
        PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
        DataTable table = new DataTable(); 
        for (int i = 0; i < props.Count; i++) 
        { 
            PropertyDescriptor prop = props[i];
            table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
        } 
        object[] values = new object[props.Count];
        foreach (T item in list)
        { 
            for (int i = 0; i < values.Length; i++) 
                values[i] = props[i].GetValue(item) ?? DBNull.Value; table.Rows.Add(values);
        }
        return table;
    }


}
