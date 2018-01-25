using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.ElkhateebDynamicLinq;
public partial class Admin_ManageCustomers : System.Web.UI.Page
{
    #region Objects
    Customer objCustomer = new Customer();
    static double oldStartBalance;
    #endregion

    #region Methods
    bool CustomerNameExist(string CustomerName)
    {
        var q = objCustomer.Filter(p => p.CustomerName.ToLower() == CustomerName.ToLower()).ToList();
        if (q.Count >0)//exist
            return true;
        else
            return false ;//not exist
    }
    bool CustomerNameExist(int CustomerID,string CustomerName)
    {
        var q = objCustomer.Filter(p => p.CustomerID !=CustomerID && p.CustomerName == CustomerName).ToList();
        if (q.Count > 0)//exist
            return true;
        else
            return false;//not exist
    }
    void Clear()
    {
        txtID.Text = txtCustomerName.Text =txtAddress.Text=txtMobile.Text=txtTelphone.Text=txtFax.Text= string.Empty;
        txtBalance.Text = txtStartBalance.Text = "0";
       
        gvCustomers.SelectedIndex = -1;
    }
 
    void FillgvCustomers()
    {
       
        gvCustomers.DataSource = objCustomer.Get().ToList();
        gvCustomers.DataBind();
    }
    bool Insert()
    {
        objCustomer = new Customer();
        objCustomer.CustomerName = txtCustomerName.Text.Trim();
        objCustomer.Address = txtAddress.Text.Trim();
        
        objCustomer.Telphone = txtTelphone.Text.Trim();
        objCustomer.Mobile = txtMobile.Text.Trim();
        objCustomer.StartBalance=Convert.ToDouble(txtStartBalance.Text.Trim());
        objCustomer.Balance += objCustomer.StartBalance;
       
       return  (Convert .ToInt32( objCustomer.Insert())>0);
    }
    bool Update()
    {
        objCustomer = new Customer();
        objCustomer.CustomerID = Convert.ToInt32(txtID.Text.Trim());
        objCustomer.CustomerName = txtCustomerName.Text.Trim();
        
        objCustomer.Address = txtAddress.Text.Trim();
      
        objCustomer.Telphone = txtTelphone.Text.Trim();
        objCustomer.Mobile = txtMobile.Text.Trim();
        objCustomer.Telphone = txtTelphone.Text.Trim();
        objCustomer.Mobile = txtMobile.Text.Trim();
        objCustomer.StartBalance = Convert.ToDouble(txtStartBalance.Text.Trim());
        objCustomer.Balance = (Convert.ToDouble(txtBalance.Text.Trim()) -oldStartBalance) + objCustomer.StartBalance;
        return objCustomer.Update(p => p.CustomerID == objCustomer.CustomerID);
    }
    void Dir(Customer entity)
    {
        txtID.Text = entity.CustomerID.ToString();
        txtCustomerName.Text = entity.CustomerName;
        

        txtAddress.Text = entity.Address;
        
         txtTelphone.Text=entity .Telphone;
         txtMobile.Text = entity.Mobile;
         txtBalance.Text = entity.Balance.ToString();
         txtStartBalance.Text = entity.StartBalance.ToString();
         oldStartBalance  = entity.StartBalance;
    }
    void GetCustomerByName(string CustomerName)
    {
        
        gvCustomers.DataSource = objCustomer.Filter( p => p.CustomerName.ToLower() == CustomerName.ToLower()).ToList();
      gvCustomers.DataBind();
    }
    #endregion

    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.Title = "Manage Customers";
            
            FillgvCustomers();
            
        }

    }
    protected void imgbtnSave_Click(object sender, ImageClickEventArgs e)
    {
        System.Threading.Thread.Sleep(1000);
        if (gvCustomers.SelectedIndex == -1)
        {
            if (CustomerNameExist(txtCustomerName.Text.Trim()))
            {
                MessageBox.Show(this.Page ,"Customer Name exist before");
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
        else if (gvCustomers.SelectedIndex > -1)
        {
            if (CustomerNameExist(Convert.ToInt32(txtID.Text.Trim()),txtCustomerName.Text.Trim()))
            {
                MessageBox.Show(this.Page, "Customer Name exist before");
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
        FillgvCustomers();
        
        Clear();
    }
    protected void imgbtnClear_Click(object sender, ImageClickEventArgs e)
    {
        FillgvCustomers();
        Clear();
        

    }
    protected void gvCustomers_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int CustomerID = Convert.ToInt32(e.CommandArgument);
        if (e.CommandName == "select")
        {
            Dir(objCustomer.Single(p => p.CustomerID == CustomerID));
        }
        else if (e.CommandName == "del")
        {
            try
            {
                System.Threading.Thread.Sleep(2000);
                if (objCustomer.Delete(p => p.CustomerID == CustomerID))
                {
                    mdl_Sucess.Show();
                    FillgvCustomers();
                   
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
    protected void gvCustomers_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvCustomers.PageIndex = e.NewPageIndex;
        FillgvCustomers();
    }
    #endregion


    protected void imgbtnSearch_Click(object sender, ImageClickEventArgs e)
    {
        if(!string.IsNullOrEmpty(txtSearchValue.Text))
        {
            GetCustomerByName(txtSearchValue.Text.Trim());
        }
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        Customer objCustomer = new Customer();
        return objCustomer.Get(p => p.CustomerName.ToLower().StartsWith(prefixText.ToLower()), p => p.CustomerName).ToArray();
    }
}
