<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ManageEmployees.aspx.cs" Inherits="Admin_ManageEmployees" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <script language="javascript" type="text/javascript" >
    //--- To Show Modal Popup of ValidationSummary
    function ShowModalDialog(group) {
    var g='';
    g=group ;
    var vs = document.getElementById ('<%=ValidationSummary3.ClientID %>');
         vs.validationGroup=g ;
        var x = $find('<%=ModalExtnd1.ClientID %>');
        
        Page_ClientValidate(g );
        if (!Page_IsValid)
            x.show();


    }
</script>

    <asp:Panel ID="pnlControl" runat="server" GroupingText="Employees Control" 
        DefaultButton="imgbtnSave">
        <asp:UpdatePanel ID="upnlControl" runat="server">
        <ContentTemplate>
        <table width="100%">
        <tr>
        <td style="width: 147px" class="tableCell">
            <asp:Label ID="lblEmployeeID" runat="server" Text="Employee ID : "></asp:Label>
            </td>
        <td>
            <asp:TextBox ID="txtID" runat="server" Enabled="False" ReadOnly="True"></asp:TextBox>
            </td>
        </tr>
        <tr>
        <td style="width: 147px" class="tableCell">
            <asp:Label ID="lblEmployeeName" runat="server" Text="Employee Name : "></asp:Label>
            </td>
        <td>
            <asp:TextBox ID="txtEmployeeName" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvEmployeeName" runat="server" 
                ControlToValidate="txtEmployeeName" Display="Dynamic" 
                ErrorMessage="Enter Employee Name ." ValidationGroup="i">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
        <td style="width: 147px" class="tableCell">
            <asp:Label ID="lblJob" runat="server" Text="Job : "></asp:Label>
            </td>
        <td>
            <asp:DropDownList ID="ddlJob" runat="server" 
                AppendDataBoundItems="True">
                <asp:ListItem Value="0">Select</asp:ListItem>
            </asp:DropDownList>
            </td>
        </tr>
            <tr>
                <td class="tableCell" style="width: 147px">
                    <asp:Label ID="lblEmail" runat="server" Text="Email : "></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                        ControlToValidate="txtEmail" Display="Dynamic" 
                        ErrorMessage="Enter Valid Email ." ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                        ValidationGroup="i">*</asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td class="tableCell" style="width: 147px">
                    <asp:Label ID="lblAdress" runat="server" Text="Adress : "></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtAddress" runat="server" TextMode="MultiLine"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tableCell" style="width: 147px">
                    <asp:Label ID="lblTelphone" runat="server" Text="Telphone : "></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtTelphone" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tableCell" style="width: 147px">
                    <asp:Label ID="lblMobile" runat="server" Text="Mobile : "></asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtMobile" runat="server"></asp:TextBox>
                </td>
            </tr>
        <tr>
        <td style="width: 147px"></td>
        <td>
       
    <asp:ImageButton ID="imgbtnSave" runat="server" Height="35px" 
        ImageAlign="Middle" ImageUrl="~/img/1270384072_filesave.png"   
                OnClientClick ="ShowModalDialog('i')" AlternateText="Save" ToolTip="Save" 
                onclick="imgbtnSave_Click"/>
  
    <asp:ImageButton ID="imgbtnClear" runat="server" Height="35px" 
        ImageAlign="Middle" ImageUrl="~/img/Reset.png" 
                CausesValidation="false" AlternateText="Clear" ToolTip="Clear" 
                onclick="imgbtnClear_Click"  />
        </td>
        </tr>
        </table>
        </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <asp:Panel ID="pnlShow" runat="server" GroupingText="Employees Show">
    <asp:UpdatePanel ID="upnlShow" runat="server">
    
        <ContentTemplate>
      
            <asp:TextBox ID="txtSearchValue" runat="server"></asp:TextBox>
            <cc1:AutoCompleteExtender ID="txtSearchValue_AutoCompleteExtender" 
                runat="server" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" 
                ServiceMethod="GetCompletionList" ServicePath="" 
                TargetControlID="txtSearchValue" UseContextKey="True">
            </cc1:AutoCompleteExtender>
            <cc1:TextBoxWatermarkExtender ID="txtSearchEmployee_TextBoxWatermarkExtender" 
                runat="server" Enabled="True" TargetControlID="txtSearchValue" 
                WatermarkCssClass="WaterMark" WatermarkText="Find Employee ">
            </cc1:TextBoxWatermarkExtender>
            <asp:ImageButton ID="imgbtnSearch" runat="server" Height="27px" 
                ImageAlign="Middle" ImageUrl="~/img/Search.png" 
                AlternateText="Search" CausesValidation="False" ToolTip="Search" 
                onclick="imgbtnSearch_Click" />
              
            <asp:GridView ID="gvEmployees" runat="server" AllowPaging="True"  
                AutoGenerateColumns="False"  DataKeyNames="EmployeeID" Width="100%" 
                onpageindexchanging="gvEmployees_PageIndexChanging" 
                onrowcommand="gvEmployees_RowCommand">
                
                <Columns>
                    <asp:BoundField DataField="EmployeeID" HeaderText="ID" ReadOnly="True" 
                        SortExpression="EmployeeID" />
                    <asp:BoundField DataField="EmployeeName" HeaderText="Employee" 
                        ReadOnly="True" SortExpression="EmployeeName" />
                    <asp:BoundField DataField="JobName" HeaderText="Job" 
                        ReadOnly="True" SortExpression="JobName" />
                        <asp:BoundField DataField="Email" HeaderText="Email" 
                        ReadOnly="True" SortExpression="Email" />
                        <asp:BoundField DataField="Adress" HeaderText="Address" 
                        ReadOnly="True" SortExpression="Adress" />
                        <asp:BoundField DataField="Telphone" HeaderText="Telphone" 
                        ReadOnly="True" SortExpression="Telphone" />
                        <asp:BoundField DataField="Mobile" HeaderText="Mobile" 
                        ReadOnly="True" SortExpression="Mobile" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="imgbtnEdit" runat="server" Height="35px" ImageAlign="Middle" ImageUrl="~/img/1270383845_edit.png" CommandName="select" CausesValidation="false" CommandArgument='<%#Eval("EmployeeID") %>' AlternateText="Edit" ToolTip="Edit"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField>
                        <ItemTemplate>
                           <asp:ImageButton ID="imgbtnDelete" runat="server" Height="35px" ImageAlign="Middle" ImageUrl="~/img/delete.png" CommandArgument='<%#Eval("EmployeeID") %>' CommandName="del" CausesValidation="false" AlternateText="Delete" ToolTip="Delete" OnClientClick="return confirm('Are you sure ?');"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
               
            </asp:GridView>
         
        </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>

<%--End of Sucess ModalPopup --%>

    <%--End of Sucess ModalPopup --%>
<cc1:ModalPopupExtender ID="ModalExtnd1" runat="server" 
    TargetControlID="lblHidden"
    PopupControlID="SummaryDiv" 
    BackgroundCssClass="modalBackground" DynamicServicePath="" Enabled="True" CancelControlID="btnclose" >
</cc1:ModalPopupExtender>
   
<div id="SummaryDiv" runat="server" align="center" class="confirm" style="display:none" >
    <table align="center" class="confirmtable" >
        <tr>
        <td valign="top">
            <img align="middle" src="../img/warning.gif" />
            <asp:Label ID="LabelMsg" runat="server" Text="PLZ,Enter The following :-" 
                ></asp:Label>
            &nbsp;&nbsp;</td>
        </tr>
        <tr>
        <td colspan="2" align="left" >
             <asp:ValidationSummary ID="ValidationSummary3" runat="server" 
                   />
            </td>
        </tr>
        <tr>
        <td colspan="2">
        <asp:Button ID="btnclose" runat="server" Text="Close" 
                 />
        
            <%--End of Sucess ModalPopup --%>
         </td>
        </tr>
    </table>      
</div>
<asp:Label ID="lblHidden" runat="server" Text="hidden" CssClass="hidelbl"      ></asp:Label>
        
    <%--End of Sucess ModalPopup --%> <%--End of Sucess ModalPopup --%> <%--End of Sucess ModalPopup --%>
    <cc1:ModalPopupExtender ID="mdl_Fail" runat="server" 
        TargetControlID="lbl_Fail"
        PopupControlID="FailDiv" 
        BackgroundCssClass="modalBackground" DynamicServicePath="" Enabled="True"  CancelControlID ="btnclose_f">
    </cc1:ModalPopupExtender>
    <div id="FailDiv" runat="server" align="center" class="confirm" style="display:none">
        <table align="center" class="confirmtable" id="faliur" runat="server" >
            <tr>
            <td valign="top">
                <img align="middle" src="../img/achtung.gif" />
                <asp:Label ID="Label20" runat="server" Font-Bold="True" ForeColor="Red" 
                    Text="Error,plz try again. "   ></asp:Label>
                &nbsp;&nbsp;</td>
            </tr>
            
            <tr>
            <td colspan="2">
            <asp:Button ID="btnclose_f" runat="server" Text="Close" 
                     />
            </td> 
            </tr>
        </table>      
    </div>
    <asp:Label ID="lbl_Fail" runat="server" Text="hidden" CssClass="hidelbl"         ></asp:Label>
    
    <%--End of Sucess ModalPopup --%>

<%--End of Sucess ModalPopup --%>
    <cc1:ModalPopupExtender ID="mdl_Sucess" runat="server" 
        TargetControlID="lbl_Sucess"
        PopupControlID="SucessDiv" 
        BackgroundCssClass="modalBackground" DynamicServicePath="" Enabled="True" CancelControlID ="btnclose_s" >
    </cc1:ModalPopupExtender>
    <div id="SucessDiv" runat="server" align="center" class="confirm" style="display:none">
        <table align="center" class="confirmtable" id="Sucess" runat="server" >
            <tr>
            <td valign="top">
                <img align="middle" src="../img/accept.png" />
                <asp:Label ID="Label5" runat="server" Font-Bold="True" ForeColor="Green" 
                    Text="Process Complete Successfully. "            ></asp:Label>
                &nbsp;&nbsp;</td>
            </tr>
            
            <tr>
            <td colspan="2">
            <asp:Button ID="btnclose_s" Text="Close" runat="server" 
                     />
                
                    
                    </td>
            </tr>
        </table>      
    </div>
    <asp:Label ID="lbl_Sucess" runat="server" Text="hidden" CssClass="hidelbl"  ></asp:Label>




</asp:Content>


