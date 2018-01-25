using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.ElkhateebDynamicLinq;
public partial class Admin_ManageSuppliers : System.Web.UI.Page
{
    #region Objects
    Supplier objSupplier = new Supplier();
    static double oldStartBalance;
    #endregion

    #region Methods
    bool SupplierNameExist(string SupplierName)
    {
        var q = objSupplier.Filter(p => p.SupplierName.ToLower() == SupplierName.ToLower()).ToList();
        if (q.Count >0)//exist
            return true;
        else
            return false ;//not exist
    }
    bool SupplierNameExist(int SupplierID,string SupplierName)
    {
        var q = objSupplier.Filter(p => p.SupplierID !=SupplierID && p.SupplierName == SupplierName).ToList();
        if (q.Count > 0)//exist
            return true;
        else
            return false;//not exist
    }
    void Clear()
    {
        txtID.Text = txtSupplierName.Text =txtAddress.Text=txtMobile.Text=txtTelphone.Text=txtFax.Text = string.Empty;
        txtBalance.Text = txtStartBalance.Text = "0";
       
        gvSuppliers.SelectedIndex = -1;
    }
 
    void FillgvSuppliers()
    {
       
        gvSuppliers.DataSource = objSupplier.Get().ToList();
        gvSuppliers.DataBind();
    }
    bool Insert()
    {
        objSupplier = new Supplier();
        objSupplier.SupplierName = txtSupplierName.Text.Trim();
        objSupplier.Address = txtAddress.Text.Trim();
        
        objSupplier.Telphone = txtTelphone.Text.Trim();
        objSupplier.Mobile = txtMobile.Text.Trim();
        objSupplier.StartBalance=Convert.ToDouble(txtStartBalance.Text.Trim());
        objSupplier.Balance += objSupplier.StartBalance;
       
       return  (Convert .ToInt32( objSupplier.Insert())>0);
    }
    bool Update()
    {
        objSupplier = new Supplier();
        objSupplier.SupplierID = Convert.ToInt32(txtID.Text.Trim());
        objSupplier.SupplierName = txtSupplierName.Text.Trim();
        
        objSupplier.Address = txtAddress.Text.Trim();
      
        objSupplier.Telphone = txtTelphone.Text.Trim();
        objSupplier.Mobile = txtMobile.Text.Trim();
        objSupplier.Telphone = txtTelphone.Text.Trim();
        objSupplier.Mobile = txtMobile.Text.Trim();
        objSupplier.StartBalance = Convert.ToDouble(txtStartBalance.Text.Trim());
        objSupplier.Balance = (Convert.ToDouble(txtBalance.Text.Trim()) - oldStartBalance) + objSupplier.StartBalance;
        return objSupplier.Update(p => p.SupplierID == objSupplier.SupplierID);
    }
    void Dir(Supplier entity)
    {
        txtID.Text = entity.SupplierID.ToString();
        txtSupplierName.Text = entity.SupplierName;
        

        txtAddress.Text = entity.Address;
        
         txtTelphone.Text=entity .Telphone;
         txtMobile.Text = entity.Mobile;
         txtBalance.Text = entity.Balance.ToString();
         txtStartBalance.Text = entity.StartBalance.ToString();
         oldStartBalance = entity.StartBalance;
    }
    void GetSupplierByName(string SupplierName)
    {
        
        gvSuppliers.DataSource = objSupplier.Filter( p => p.SupplierName.ToLower() == SupplierName.ToLower()).ToList();
      gvSuppliers.DataBind();
    }
    #endregion

    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.Title = "Manage Suppliers";
            
            FillgvSuppliers();
        }

    }
    protected void imgbtnSave_Click(object sender, ImageClickEventArgs e)
    {
        System.Threading.Thread.Sleep(2000);
        if (gvSuppliers.SelectedIndex == -1)
        {
            if (SupplierNameExist(txtSupplierName.Text.Trim()))
            {
                MessageBox.Show(this.Page ,"Supplier Name exist before");
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
        else if (gvSuppliers.SelectedIndex > -1)
        {
            if (SupplierNameExist(Convert.ToInt32(txtID.Text.Trim()),txtSupplierName.Text.Trim()))
            {
                MessageBox.Show(this.Page, "Supplier Name exist before");
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
        FillgvSuppliers();
        
        Clear();
    }
    protected void imgbtnClear_Click(object sender, ImageClickEventArgs e)
    {
        FillgvSuppliers();
        Clear();
        

    }
    protected void gvSuppliers_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int SupplierID = Convert.ToInt32(e.CommandArgument);
        if (e.CommandName == "select")
        {
            Dir(objSupplier.Single(p => p.SupplierID == SupplierID));
        }
        else if (e.CommandName == "del")
        {
            try
            {
                System.Threading.Thread.Sleep(2000);
                if (objSupplier.Delete(p => p.SupplierID == SupplierID))
                {
                    mdl_Sucess.Show();
                    FillgvSuppliers();
                   
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
    protected void gvSuppliers_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvSuppliers.PageIndex = e.NewPageIndex;
        FillgvSuppliers();
    }
    #endregion


    protected void imgbtnSearch_Click(object sender, ImageClickEventArgs e)
    {
        if(!string.IsNullOrEmpty(txtSearchValue.Text))
        {
            GetSupplierByName(txtSearchValue.Text.Trim());
        }
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        Supplier objSupplier = new Supplier();
        return objSupplier.Get(p => p.SupplierName.ToLower().StartsWith(prefixText.ToLower()), p => p.SupplierName).ToArray();
    }
}
