using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.ElkhateebDynamicLinq;
public partial class Admin_ManageProducts : System.Web.UI.Page
{
    #region Objects
    Product objProduct = new Product();
    spProductsResult objspProductsResult = new spProductsResult();
    Category objCategory = new Category();
    Unit objUnit = new Unit();
    #endregion

    #region Methods
    bool ProductNameexist(string ProductName)
    {
        var q = objProduct.Filter(p => p.ProductName.ToLower() == ProductName.ToLower()).ToList();
        if (q.Count >0)//exist
            return true;
        else
            return false ;//not exist
    }
    bool ProductNameexist(int ProductID,string ProductName)
    {
        var q = objProduct.Filter(p => p.ProductID !=ProductID && p.ProductName == ProductName).ToList();
        if (q.Count > 0)//exist
            return true;
        else
            return false;//not exist
    }
    void Clear()
    {
        txtID.Text = txtProductName.Text =txtDescription.Text=txtSQTY.Text =txtQuantity.Text=txtStartQuantity.Text = string.Empty;
        ddlCategory.SelectedIndex = 0;
        gvProducts.SelectedIndex = -1;
        hfOldStartQuantity.Value = "0";
    }
    void FillddlCategory()
    {
        var q = objCategory.Get().ToList();
        ddlCategory.DataSource = q;
        ddlCategory.DataTextField = "CategoryName";
        ddlCategory.DataValueField = "CategoryID";
        ddlCategory.DataBind();
    }
    void FillddlUnites()
    {
        ddlUnites.DataSource = objUnit.Get().ToList();
        ddlUnites.DataTextField = (Session["culture"] == "en-US") ? "UnitName_En" : "UnitName_Ar";
        ddlUnites.DataValueField = "UnitID";
        ddlUnites.DataBind();
    }
    void FillgvProducts()
    {
        object[] parameter = null;
        var q = objspProductsResult.Get(parameter).OrderByDescending(p => p.ProductID).ToList();
        gvProducts.DataSource = q;
        gvProducts.DataBind();

        if (Session["culture"] == "en-US")
        {
            var printlist = q.Select(p =>
                new
                    {
                        No = p.ProductID,
                        Product = p.ProductName,
                        SQTY = p.StartQuantity,
                        QTY = p.Quantity,
                        unit = SalesManager.getBasicUnitName

                    }
                    ).ToList();

            divprint.Controls.Add(printlist.ToDataTable().ConvertToHtmlTable("Products Quantities"));
        }
        else
        {
            var printlist = q.Select(p =>
               new
               {
                   رقم = p.ProductID,
                   المنتج = p.ProductName,
                  الكميه_الاوليه = p.StartQuantity,
                   الكميه = p.Quantity,
                   الوحده = SalesManager.getBasicUnitName

               }
                   ).ToList();

            divprint.Controls.Add(printlist.ToDataTable().ConvertToHtmlTable("كميات المنتجات"));
        }
    }
    bool Insert()
    {
        objProduct = new Product();
        objProduct.ProductName = txtProductName.Text.Trim();
        objProduct.CategoryID = (ddlCategory.SelectedIndex > 0) ? (Nullable<int>) Convert.ToInt32 ( ddlCategory.SelectedValue) : null;
        //objProduct.Price = Convert.ToDouble(txtPrice.Text.Trim());
        objProduct.SQty = Convert.ToDouble(txtSQTY.Text);
        objProduct.UnitID = Convert.ToInt32(ddlUnites.SelectedValue);
        objProduct.BasicUnitID = SalesManager.getBasicUnitID;


        objProduct.StartQuantity = Convert.ToDouble(txtStartQuantity.Text.Trim());
        objProduct.Quantity = objProduct.StartQuantity;
       
        objProduct.Description = txtDescription.Text.Trim();
       return  (Convert .ToInt32( objProduct.Insert())>0);
    }
    bool Update()
    {
        objProduct = new Product();
        objProduct.ProductID = Convert.ToInt32(txtID.Text.Trim());
        objProduct.ProductName = txtProductName.Text.Trim();
        objProduct.CategoryID = (ddlCategory.SelectedIndex > 0) ? (Nullable<int>)int.Parse(ddlCategory.SelectedValue) : null;
        //objProduct.Price = Convert.ToDouble(txtPrice.Text.Trim());

        objProduct.SQty = Convert.ToDouble(txtSQTY.Text);
        objProduct.UnitID = Convert.ToInt32(ddlUnites.SelectedValue);
        objProduct.BasicUnitID = SalesManager.getBasicUnitID;


        objProduct.StartQuantity = Convert.ToDouble(txtStartQuantity.Text.Trim());
        objProduct.Quantity = (Convert.ToDouble(txtQuantity.Text.Trim()) - Convert.ToDouble(hfOldStartQuantity.Value)) + objProduct.StartQuantity;

        objProduct.Description = txtDescription.Text.Trim();
        return objProduct.Update(p => p.ProductID == objProduct.ProductID);
    }
    void Dir(Product entity)
    {
        txtID.Text = entity.ProductID.ToString();
        txtProductName.Text = entity.ProductName;
        ddlCategory.SelectedValue = (entity.CategoryID !=null )?entity.CategoryID.ToString():"0";
        //txtPrice.Text = entity.Price.ToString();
        txtStartQuantity.Text=hfOldStartQuantity.Value = entity.StartQuantity.ToString();
        txtQuantity.Text = entity.Quantity.ToString();
        txtDescription.Text = entity.Description;

        txtSQTY.Text = entity.SQty.ToString();
        ddlUnites.SelectedValue = entity.UnitID.ToString();

    }
    void GetProductByName(string ProductName)
    {
        object[] parameter = null;
        gvProducts.DataSource = objspProductsResult.Filter(parameter, p => p.ProductName.ToLower() == ProductName.ToLower()).ToList();
      gvProducts.DataBind();
    }
    void getIntialQty()
    {
        if (!string.IsNullOrEmpty(txtSQTY.Text) && ddlUnites.SelectedIndex > 0)
        {
            double sqty = Convert.ToDouble(txtSQTY.Text);
            double intialqty = SalesManager.UnitConvert(Convert.ToInt32(ddlUnites.SelectedValue), sqty);
            txtStartQuantity.Text = intialqty.ToString();
            if (gvProducts.SelectedIndex == -1)
                txtQuantity.Text = txtStartQuantity.Text;

        }
    }
    void FillddlUnities()
    {

        var q = objUnit.Filter(p => p.Basic != true);
        ddlUnites.DataSource = q;
        ddlUnites.DataTextField = (Session["culture"] == "en-US") ? "UnitName_En" : "UnitName_Ar";
        ddlUnites.DataValueField = "UnitID";
        ddlUnites.DataBind();
    }

    #endregion

    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.Title = "Manage Products";
           
            FillddlCategory();
            FillgvProducts();
            FillddlUnities();
        }

    }
    protected void imgbtnSave_Click(object sender, ImageClickEventArgs e)
    {
        System.Threading.Thread.Sleep(2000);
        if (gvProducts.SelectedIndex == -1)
        {
            if (ProductNameexist(txtProductName.Text.Trim()))
            {
                MessageBox.Show(this.Page ,"Product Name exist before");
                return;
            }
            if (Insert())
            {
                mdl_Sucess.Show();
            }
            else
            {
                mdl_Fail.Show();
            }
        }
        else if (gvProducts.SelectedIndex > -1)
        {
            if (ProductNameexist(Convert.ToInt32(txtID.Text.Trim()),txtProductName.Text.Trim()))
            {
                MessageBox.Show(this.Page, "Product Name exist before");
                return;
            }
            if (Update())
            {
                mdl_Sucess.Show();
            }
            else
            {
                mdl_Fail.Show();
            }
        }
        FillgvProducts();
        //FillddlCategory();
        Clear();
    }
    protected void imgbtnClear_Click(object sender, ImageClickEventArgs e)
    {
        FillgvProducts();
        Clear();

    }
    protected void gvProducts_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int ProductID = Convert.ToInt32(e.CommandArgument);
        if (e.CommandName == "select")
        {
            Dir(objProduct.Single(p => p.ProductID == ProductID));
        }
        else if (e.CommandName == "del")
        {
            try
            {
                System.Threading.Thread.Sleep(2000);
                if (objProduct.Delete(p => p.ProductID == ProductID))
                {
                    mdl_Sucess.Show();
                    FillgvProducts();
                    //FillddlCategory();
                    Clear();
                }
                else
                {
                    mdl_Fail.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.Page, "this filed linked With other data");
            }

        }
    }
    protected void gvProducts_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvProducts.PageIndex = e.NewPageIndex;
        FillgvProducts();
    }
    protected void txtSQTY_TextChanged(object sender, EventArgs e)
    {
        getIntialQty();
        ddlUnites.Focus();
    }
    protected void ddlUnites_SelectedIndexChanged(object sender, EventArgs e)
    {
        getIntialQty();
        ddlUnites.Focus();
    }
    protected void imgbtnSearch_Click(object sender, ImageClickEventArgs e)
    {
        if (!string.IsNullOrEmpty(txtSearchValue.Text))
        {
            GetProductByName(txtSearchValue.Text.Trim());
        }
    }
    #endregion


    

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        Product objProduct = new Product();
        return objProduct.Get(p => p.ProductName.ToLower().StartsWith(prefixText.ToLower()), p => p.ProductName).ToArray();
    }

    
}
