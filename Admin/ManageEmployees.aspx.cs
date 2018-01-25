using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.ElkhateebDynamicLinq;
public partial class Admin_ManageEmployees : System.Web.UI.Page
{
    #region Objects
    Employee objEmployee = new Employee();
    spEmployeesResult objspEmployeesResult = new spEmployeesResult();
    Job objJob = new Job();
    #endregion

    #region Methods
    bool EmployeeNameExist(string EmployeeName)
    {
        var q = objEmployee.Filter(p => p.EmployeeName.ToLower() == EmployeeName.ToLower()).ToList();
        if (q.Count >0)//exist
            return true;
        else
            return false ;//not exist
    }
    bool EmployeeNameExist(int EmployeeID,string EmployeeName)
    {
        var q = objEmployee.Filter(p => p.EmployeeID !=EmployeeID && p.EmployeeName == EmployeeName).ToList();
        if (q.Count > 0)//exist
            return true;
        else
            return false;//not exist
    }
    void Clear()
    {
        txtID.Text = txtEmployeeName.Text =txtAddress.Text=txtEmail.Text=txtMobile.Text=txtTelphone.Text= string.Empty;
        ddlJob.SelectedIndex = 0;
        gvEmployees.SelectedIndex = -1;
    }
    void FillddlJob()
    {
        ddlJob.DataSource = objJob.Get();
        ddlJob.DataTextField = "JobName";
        ddlJob.DataValueField = "JobID";
        ddlJob.DataBind();
    }
    void FillgvEmployees()
    {
        object[] parameter = null;
        gvEmployees.DataSource = objspEmployeesResult.Get(parameter).ToList();
        gvEmployees.DataBind();
    }
    bool Insert()
    {
        objEmployee = new Employee();
        objEmployee.EmployeeName = txtEmployeeName.Text.Trim();
        objEmployee.Adress = txtAddress.Text.Trim();
        objEmployee.Email = txtEmail.Text.Trim();
        objEmployee.Telphone = txtTelphone.Text.Trim();
        objEmployee.Mobile = txtMobile.Text.Trim();
        objEmployee.JobID = (ddlJob.SelectedIndex > 0) ? (Nullable<int>) int.Parse( ddlJob.SelectedValue) : null;
       
       return  (Convert .ToInt32( objEmployee.Insert())>0);
    }
    bool Update()
    {
        objEmployee = new Employee();
        objEmployee.EmployeeID = Convert.ToInt32(txtID.Text.Trim());
        objEmployee.EmployeeName = txtEmployeeName.Text.Trim();
        objEmployee.JobID = (ddlJob.SelectedIndex > 0) ? (Nullable<int>)int.Parse(ddlJob.SelectedValue) : null;
        objEmployee.Adress = txtAddress.Text.Trim();
        objEmployee.Email = txtEmail.Text.Trim();
        objEmployee.Telphone = txtTelphone.Text.Trim();
        objEmployee.Mobile = txtMobile.Text.Trim();
        return objEmployee.Update(p => p.EmployeeID == objEmployee.EmployeeID);
    }
    void Dir(Employee entity)
    {
        txtID.Text = entity.EmployeeID.ToString();
        txtEmployeeName.Text = entity.EmployeeName;
        ddlJob.SelectedValue = (entity.JobID !=null )?entity.JobID.ToString():"0";

        txtAddress.Text = entity.Adress;
        txtEmail.Text=entity .Email ;
         txtTelphone.Text=entity .Telphone;
         txtMobile.Text = entity.Mobile;
       
    }
    void GetEmployeeByName(string EmployeeName)
    {
        object[] parameter = null;
        gvEmployees.DataSource = objspEmployeesResult.Filter(parameter, p => p.EmployeeName.ToLower() == EmployeeName.ToLower()).ToList();
      gvEmployees.DataBind();
    }
    #endregion

    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.Title = "Manage Employees";
            FillddlJob();
            FillgvEmployees();
        }

    }
    protected void imgbtnSave_Click(object sender, ImageClickEventArgs e)
    {
        System.Threading.Thread.Sleep(2000);
        if (gvEmployees.SelectedIndex == -1)
        {
            if (EmployeeNameExist(txtEmployeeName.Text.Trim()))
            {
                MessageBox.Show(this.Page ,"Employee Name exist before");
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
        else if (gvEmployees.SelectedIndex > -1)
        {
            if (EmployeeNameExist(Convert.ToInt32(txtID.Text.Trim()),txtEmployeeName.Text.Trim()))
            {
                MessageBox.Show(this.Page, "Employee Name exist before");
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
        FillgvEmployees();
        //FillddlJob();
        Clear();
    }
    protected void imgbtnClear_Click(object sender, ImageClickEventArgs e)
    {
        FillgvEmployees();
        Clear();
        

    }
    protected void gvEmployees_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int EmployeeID = Convert.ToInt32(e.CommandArgument);
        if (e.CommandName == "select")
        {
            Dir(objEmployee.Single(p => p.EmployeeID == EmployeeID));
        }
        else if (e.CommandName == "del")
        {
            try
            {
                System.Threading.Thread.Sleep(2000);
                if (objEmployee.Delete(p => p.EmployeeID == EmployeeID))
                {
                    mdl_Sucess.Show();
                    FillgvEmployees();
                    //FillddlJob();
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
    protected void gvEmployees_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvEmployees.PageIndex = e.NewPageIndex;
        FillgvEmployees();
    }
    #endregion


    protected void imgbtnSearch_Click(object sender, ImageClickEventArgs e)
    {
        if(!string.IsNullOrEmpty(txtSearchValue.Text))
        {
            GetEmployeeByName(txtSearchValue.Text.Trim());
        }
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
        Employee objEmployee = new Employee();
        return objEmployee.Get(p => p.EmployeeName.ToLower().StartsWith(prefixText.ToLower()), p => p.EmployeeName).ToArray();
    }
}
