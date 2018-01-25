using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.ElkhateebDynamicLinq;
public partial class Admin_ManageJobs : System.Web.UI.Page
{
    #region Objects
    Job objJob = new Job();
    
    #endregion

    #region Methods
    bool JobNameExist(string JobName)
    {
        var q = objJob.Filter(p => p.JobName.ToLower() == JobName.ToLower()).ToList();
        if (q.Count > 0)//exist
            return true;
        else
            return false ;//not exist
    }
    bool JobNameExist(int JobID,string JobName)
    {
        var q = objJob.Filter(p => p.JobID !=JobID && p.JobName == JobName).ToList();
        if (q.Count > 0)//exist
            return true;
        else
            return false;//not exist
    }
    void Clear()
    {
        txtID.Text = txtJobName.Text = string.Empty;
       
        gvJobs.SelectedIndex = -1;
    }
  
    void FillgvJobs()
    {
        
        gvJobs.DataSource = objJob.Get().ToList();
        gvJobs.DataBind();
    }
    bool Insert()
    {
        objJob = new Job();
        objJob.JobName = txtJobName.Text.Trim();
        
       return  (Convert .ToInt32( objJob.Insert())>0);
    }
    bool Update()
    {
        objJob = new Job();
        objJob.JobID = Convert.ToInt32(txtID.Text.Trim());
        objJob.JobName = txtJobName.Text.Trim();
       
        return objJob.Update(p => p.JobID == objJob.JobID);
    }
    void Dir(Job entity)
    {
        txtID.Text = entity.JobID.ToString();
        txtJobName.Text = entity.JobName;
   
    }
    #endregion

    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.Title = "Manage Jobs";
           
            FillgvJobs();
        }

    }
    protected void imgbtnSave_Click(object sender, ImageClickEventArgs e)
    {
        System.Threading.Thread.Sleep(2000);
        if (gvJobs.SelectedIndex == -1)
        {
            if (JobNameExist(txtJobName.Text.Trim()))
            {
                MessageBox.Show(this.Page ,"Job Name exist before");
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
        else if (gvJobs.SelectedIndex > -1)
        {
            if (JobNameExist(Convert.ToInt32(txtID.Text.Trim()),txtJobName.Text.Trim()))
            {
                MessageBox.Show(this.Page, "Job Name exist before");
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
        FillgvJobs();
       
        Clear();
    }
    protected void imgbtnClear_Click(object sender, ImageClickEventArgs e)
    {
        FillgvJobs();
        Clear();

    }
    protected void gvJobs_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int JobID = Convert.ToInt32(e.CommandArgument);
        if (e.CommandName == "select")
        {
            Dir(objJob.Single(p => p.JobID == JobID));
        }
        else if (e.CommandName == "del")
        {
            try
            {
                System.Threading.Thread.Sleep(2000);
                if (objJob.Delete(p => p.JobID == JobID))
                {
                    mdl_Sucess.Show();
                    FillgvJobs();
                    
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
    protected void gvJobs_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvJobs.PageIndex = e.NewPageIndex;
        FillgvJobs();
    }
    #endregion


    protected void imgbtnSearch_Click(object sender, ImageClickEventArgs e)
    {
        if (!string.IsNullOrEmpty(txtSearchValue.Text))
        {
            GetJobByName(txtSearchValue.Text.Trim());
        }
    }

    private void GetJobByName(string JobName)
    {
       
        gvJobs.DataSource = objJob.Filter( p => p.JobName.ToLower() == JobName.ToLower()).ToList();
        gvJobs.DataBind();
    }

    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionList(string prefixText, int count, string contextKey)
    {
         Job objJob = new Job();
         return objJob.Get(p => p.JobName.ToLower().StartsWith(prefixText.ToLower()), p => p.JobName).ToArray();
    }
}
