<%@ Page Title="Manage Sales" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="ManageSales.aspx.cs" Inherits="Admin_ManageSales" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script language="javascript" type="text/javascript">
        //--- To Show Modal Popup of ValidationSummary
        function ShowModalDialog(group) {
            var g = '';
            g = group;
            var vs = document.getElementById('<%=ValidationSummary3.ClientID %>');
            vs.validationGroup = g;
            var x = $find('<%=ModalExtnd1.ClientID %>');

            Page_ClientValidate(g);
            if (!Page_IsValid) {
                x.show();
                return false;
            }
            return true;

        }
    </script>
    <asp:Panel ID="pnlControl" runat="server" GroupingText="Sales Control" DefaultButton="imgbtnSave">
        <asp:UpdatePanel ID="upnlControl" runat="server">
            <ContentTemplate>
                <table width="100%">
                    <tr>
                        <td class="tableCell" colspan="2">
                            <asp:Label ID="lblSalesID" runat="server" Text="Sales Number : "></asp:Label>
                        </td>
                        <td style="width: 213px" class="style3" colspan="2">
                            <asp:TextBox ID="txtID" runat="server" Enabled="False" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tableCell" colspan="2">
                            <asp:Label ID="lblCustomer" runat="server" Text="Customer : "></asp:Label>
                        </td>
                        <td style="width: 213px" class="style3" colspan="2">
                            <asp:DropDownList ID="ddlCustomer" runat="server" AppendDataBoundItems="True" 
                                AutoPostBack="True" onselectedindexchanged="ddlCustomer_SelectedIndexChanged">
                                <asp:ListItem Value="0">Select</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlCustomer"
                                Display="Dynamic" ErrorMessage="Enter Customer ." InitialValue="0" 
                                ValidationGroup="none">*</asp:RequiredFieldValidator>
                            <br />
                            <asp:TextBox ID="txtCustomerName" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator45" runat="server" 
                                ControlToValidate="txtCustomerName" Display="Dynamic" 
                                ErrorMessage="Enter Customer ." ValidationGroup="i">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="tableCell" colspan="2">
                            <asp:Label ID="lblEmployee" runat="server" Text="Employee : "></asp:Label>
                        </td>
                        <td style="width: 213px" class="style3" colspan="2">
                            <asp:DropDownList ID="ddlEmployee" runat="server" AppendDataBoundItems="True">
                                <asp:ListItem Value="0">Select</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tableCell" colspan="2">
                            <asp:Label ID="lblSalesDateM" runat="server" Text="SalesDate M : "></asp:Label>
                        </td>
                        <td style="width: 213px" class="style3" colspan="2">
                            <asp:TextBox ID="txtSalesDateM" runat="server" AutoPostBack="True" OnTextChanged="txtSalesDateM_TextChanged"></asp:TextBox>
                            <cc1:CalendarExtender ID="txtSalesDateM_CalendarExtender" runat="server" Enabled="True"
                                TargetControlID="txtSalesDateM">
                            </cc1:CalendarExtender>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtSalesDateM"
                                Display="Dynamic" ErrorMessage="Enter SalesDate ." ValidationGroup="i">*</asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="tableCell" colspan="2">
                            <asp:Label ID="lblSalesDateH" runat="server" Text="SalesDate H : "></asp:Label>
                        </td>
                        <td style="width: 213px" class="style3" colspan="2">
                            <asp:TextBox ID="txtSalesDateH" runat="server" Enabled="False"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tableCell" colspan="2">
                            <asp:Label ID="lblNotes" runat="server" Text="Notes  : "></asp:Label>
                        </td>
                        <td style="width: 213px" class="style3" colspan="2">
                            <asp:TextBox ID="txtNotes" runat="server" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <asp:Panel ID="pnlProduct" runat="server" GroupingText="Products" DefaultButton="imgbtnAdd">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:GridView ID="gvProducts" runat="server" DataKeyNames="SalesDetailID,SalesID,ProductID,UnitID"
                                                AutoGenerateColumns="false" OnRowCommand="gvProducts_RowCommand" OnRowDataBound="gvProducts_RowDataBound"
                                                Width="100%" OnDataBound="gvProducts_DataBound">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="SalesDetailID" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSalesDetailID" Text='<%# Eval("SalesDetailID") %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="SalesID" Visible="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSalesID" Text='<%# Eval("SalesID") %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="S.N.">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblS" Text='<%# Container.DataItemIndex + 1 %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Product">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="ddlProduct" runat="server" Width="100px">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlProduct"
                                                                Display="Dynamic" ErrorMessage="Enter Product" InitialValue="0" ValidationGroup="i">*</asp:RequiredFieldValidator>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:TemplateField HeaderText="Quantity">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtQuantity" Text='<%# Eval("Quantity") %>' runat="server" AutoPostBack="True"
                                                                OnTextChanged="txtQuantity_TextChanged" Width="90px"></asp:TextBox>
                                                            <asp:HiddenField ID="hfoldQty" runat="server" Value='<%# Eval("oldQty") %>'  />
                                                            <cc1:FilteredTextBoxExtender ID="txtQuantity_FilteredTextBoxExtender" runat="server"
                                                                Enabled="True" FilterType="Numbers" TargetControlID="txtQuantity">

                                                            </cc1:FilteredTextBoxExtender>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtQuantity"
                                                                Display="Dynamic" ErrorMessage="Enter Quantity." ValidationGroup="i">*</asp:RequiredFieldValidator>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>--%>
                                                    
                                                      <asp:TemplateField HeaderText="S.Qty.">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtSQTY" runat="server" AutoPostBack="True" 
                                                                Text='<%# Eval("SQTY") %>' Width="70px" ontextchanged="txtSQTY_TextChanged" ></asp:TextBox>
                                                                          <asp:RequiredFieldValidator ID="RequiredFieldValidator44" runat="server" ControlToValidate="txtSQTY"
                                                                Display="Dynamic" ErrorMessage="Enter S.QTY." ValidationGroup="i">*</asp:RequiredFieldValidator>

                                                                 <asp:RegularExpressionValidator ID="RegularExpressionValidator111" runat="server" ControlToValidate="txtSQTY"
                                                                Display="Dynamic" ErrorMessage="Enter Valid S.QTY." ValidationExpression="[-+]?[0-9]*\.?[0-9]+">*</asp:RegularExpressionValidator>
                                                                <asp:HiddenField ID="hfoldQty" runat="server" Value='<%# Eval("oldQty") %>'  />

                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                     <asp:TemplateField HeaderText="Unit">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="ddlUnits" runat="server"  
                                                                onselectedindexchanged="ddlUnits_SelectedIndexChanged">
                                                            </asp:DropDownList>

                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator33" runat="server" ControlToValidate="ddlUnits"
                                                                Display="Dynamic" ErrorMessage="Enter Unit" InitialValue="0" ValidationGroup="i">*</asp:RequiredFieldValidator>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Unit Price">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtSUnitPrice" Text='<%# Eval("SUnitPrice") %>' runat="server" AutoPostBack="True"
                                                                OnTextChanged="txtSUnitPrice_TextChanged" Width="90px"></asp:TextBox>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtSUnitPrice"
                                                                Display="Dynamic" ErrorMessage="Enter Unit Price." ValidationGroup="i">*</asp:RequiredFieldValidator>
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtSUnitPrice"
                                                                Display="Dynamic" ErrorMessage="Enter Valid Price." ValidationExpression="[-+]?[0-9]*\.?[0-9]+">*</asp:RegularExpressionValidator>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total Price">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtTotalPrice" runat="server" Enabled="False" Width="90px" Text='<%# Eval("TotalPrice") %>'></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="imgbtnDelete" runat="server" AlternateText="Delete" CausesValidation="false"
                                                                CommandArgument='<%#Eval("SalesDetailID") %>' CommandName="del" Height="30px"
                                                                ImageAlign="Middle" ImageUrl="~/img/delete.png" OnClientClick="return confirm('Are you sure ?');"
                                                                ToolTip="Delete" Width="35px" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tableCell">
                                            <table style="background-color: #5D7B9D">
                                                <tr>
                                                    <td class="tableCell">
                                                        <asp:ImageButton ID="imgbtnAdd" runat="server" AlternateText="Add" ValidationGroup="i"
                                                            CommandName="Add" Height="30px" ImageAlign="Middle" ImageUrl="~/img/add.png"
                                                            ToolTip="Add" OnClick="imgbtnAdd_Click" />
                                                    </td>
                                                    
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                          
                    <tr>
                        <td style="width: 475px"  >
                          <asp:Label ID="lblIsCash1" runat="server" Text="Is Cash ? "></asp:Label></td>
                        <td style="width: 97px">
                            <asp:CheckBox ID="cboxIsCash" runat="server" AutoPostBack="True" 
                                oncheckedchanged="cboxIsCash_CheckedChanged" Checked="True" 
                                Enabled="False" />
                        </td>
                        <td style="width: 166px" class="style3" >
                        <asp:Label ID="lblSales" runat="server" Text="Total Sales : "></asp:Label>
                        </td>
                        <td class="style3" style="width: 213px">
                         <asp:Label ID="lblTotalSalesPrice" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 475px">
                            <asp:Label ID="Label25" runat="server" Text="Customer Balance:"></asp:Label>
                        </td>
                        <td style="width: 97px">
                            <asp:Label ID="lblCustomerBalance" runat="server">0</asp:Label>
                        </td>
                        <td class="style3" style="width: 166px">
                            <asp:Label ID="Label21" runat="server" Text="Payment:"></asp:Label>
                        </td>
                        <td class="style3" style="width: 213px">
                            <asp:TextBox ID="txtPayment" runat="server" AutoPostBack="True" 
                                ontextchanged="txtPayment_TextChanged" Width="50px">0</asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 475px">
                            <asp:Label ID="Label28" runat="server" Text="new Customer Balance:"></asp:Label>
                        </td>
                        <td style="width: 97px">
                            <asp:Label ID="lblNewCustomerBalance" runat="server" Text="0"></asp:Label>
                        </td>
                        <td class="style3" style="width: 166px">
                            <asp:Label ID="Label27" runat="server" Text="Rest:"></asp:Label>
                        </td>
                        <td class="style3" style="width: 213px">
                            <asp:Label ID="lblRest" runat="server" Text="0"></asp:Label>
                            <asp:HiddenField ID="hfOldRest" runat="server" Value="0" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                        </td>
                        <td class="style3" style="width: 213px" colspan="2">
                            <asp:ImageButton ID="imgbtnSave" runat="server" AlternateText="Save" 
                                Height="35px" ImageAlign="Middle" ImageUrl="~/img/1270384072_filesave.png" 
                                OnClick="imgbtnSave_Click" OnClientClick="return ShowModalDialog('i')" 
                                ToolTip="Save" />
                            <asp:ImageButton ID="imgbtnClear" runat="server" AlternateText="Clear" 
                                CausesValidation="false" Height="35px" ImageAlign="Middle" 
                                ImageUrl="~/img/Reset.png" OnClick="imgbtnClear_Click" ToolTip="Clear" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <asp:Panel ID="pnlShow" runat="server" GroupingText="Sales Show">
        <asp:UpdatePanel ID="upnlShow" runat="server">
            <ContentTemplate>
                <asp:TextBox ID="txtSearchValue" runat="server"></asp:TextBox>
                <cc1:AutoCompleteExtender ID="txtSearchValue_AutoCompleteExtender" runat="server"
                    DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionList"
                    ServicePath="" TargetControlID="txtSearchValue" UseContextKey="True">
                </cc1:AutoCompleteExtender>
                <cc1:TextBoxWatermarkExtender ID="txtSearchSales_TextBoxWatermarkExtender" runat="server"
                    Enabled="True" TargetControlID="txtSearchValue" WatermarkCssClass="WaterMark"
                    WatermarkText="Find Sales">
                </cc1:TextBoxWatermarkExtender>
                <asp:ImageButton ID="imgbtnSearch" runat="server" Height="27px" ImageAlign="Middle"
                    ImageUrl="~/img/Search.png" AlternateText="Search" CausesValidation="False"
                    ToolTip="Search" OnClick="imgbtnSearch_Click" />
                <asp:GridView ID="gvSales" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                    DataKeyNames="SalesID" Width="100%" OnPageIndexChanging="gvSales_PageIndexChanging"
                    OnRowCommand="gvSales_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="SalesID" HeaderText="ID" ReadOnly="True" SortExpression="SalesID" />
                        <asp:BoundField DataField="CustomerName" HeaderText="Customer" ReadOnly="True" SortExpression="CustomerName" />
                        <asp:TemplateField HeaderText="SalesDate M">
                            <ItemTemplate>
                                <asp:Label ID="lblSalesDateM" Text='<%# Convert.ToDateTime(Eval("SalesDateM")).ToString("MM/dd/yyyy") %>'
                                    runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="SalesDateH" HeaderText="SalesDate H" ReadOnly="True" SortExpression="SalesDateH" />
                        <asp:BoundField DataField="Notes" HeaderText="Notes" ReadOnly="True" SortExpression="Notes" />
                        <asp:CheckBoxField DataField="IsCash" HeaderText="Is Cash" ReadOnly="True" SortExpression="IsCash" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnEdit" runat="server" Height="35px" ImageAlign="Middle"
                                    ImageUrl="~/img/1270383845_edit.png" CommandName="select" CausesValidation="false"
                                    CommandArgument='<%#Eval("SalesID") %>' AlternateText="Edit" ToolTip="Edit" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="imgbtnDelete" runat="server" AlternateText="Delete" CausesValidation="false"
                                    CommandArgument='<%#Eval("SalesID") %>' CommandName="del" Height="35px" ImageAlign="Middle"
                                    ImageUrl="~/img/delete.png" OnClientClick="return confirm('Are you sure ?');"
                                    ToolTip="Delete" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <%--End of Sucess ModalPopup --%><%--End of Sucess ModalPopup --%>
    <cc1:ModalPopupExtender ID="ModalExtnd1" runat="server" TargetControlID="lblHidden"
        PopupControlID="SummaryDiv" BackgroundCssClass="modalBackground" DynamicServicePath=""
        Enabled="True" CancelControlID="btnclose">
    </cc1:ModalPopupExtender>
    <div id="SummaryDiv" runat="server" align="center" class="confirm" style="display: none">
        <table align="center" class="confirmtable">
            <tr>
                <td valign="top">
                    <img align="middle" src="../img/warning.gif" />
                    <asp:Label ID="LabelMsg" runat="server" Text="PLZ,Enter The following :-"></asp:Label>
                    &nbsp;&nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="2" align="left">
                    <asp:ValidationSummary ID="ValidationSummary3" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnclose" runat="server" Text="Close" />
                    <%--End of Sucess ModalPopup --%>
                </td>
            </tr>
        </table>
    </div>
    <asp:Label ID="lblHidden" runat="server" Text="hidden" CssClass="hidelbl"></asp:Label>
    <%--End of Sucess ModalPopup --%><%--End of Sucess ModalPopup --%><%--End of Sucess ModalPopup --%>
    <cc1:ModalPopupExtender ID="mdl_Fail" runat="server" TargetControlID="lbl_Fail" PopupControlID="FailDiv"
        BackgroundCssClass="modalBackground" DynamicServicePath="" Enabled="True" CancelControlID="btnclose_f">
    </cc1:ModalPopupExtender>
    <div id="FailDiv" runat="server" align="center" class="confirm" style="display: none">
        <table align="center" class="confirmtable" id="faliur" runat="server">
            <tr>
                <td valign="top">
                    <img align="middle" src="../img/achtung.gif" />
                    <asp:Label ID="Label20" runat="server" Font-Bold="True" ForeColor="Red" Text="Error,plz try again. "></asp:Label>
                    &nbsp;&nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnclose_f" runat="server" Text="Close" />
                </td>
            </tr>
        </table>
    </div>
    <asp:Label ID="lbl_Fail" runat="server" Text="hidden" CssClass="hidelbl"></asp:Label>
    <%--End of Sucess ModalPopup --%><%--End of Sucess ModalPopup --%>
    <cc1:ModalPopupExtender ID="mdl_Sucess" runat="server" TargetControlID="lbl_Sucess"
        PopupControlID="SucessDiv" BackgroundCssClass="modalBackground" DynamicServicePath=""
        Enabled="True" CancelControlID="btnclose_s">
    </cc1:ModalPopupExtender>
    <div id="SucessDiv" runat="server" align="center" class="confirm" style="display: none">
        <table align="center" class="confirmtable" id="Sucess" runat="server">
            <tr>
                <td valign="top">
                    <img align="middle" src="../img/accept.png" />
                    <asp:Label ID="Label5" runat="server" Font-Bold="True" ForeColor="Green" Text="Process Complete Successfully. "></asp:Label>
                    &nbsp;&nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="btnclose_s" Text="Close" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    <asp:Label ID="lbl_Sucess" runat="server" Text="hidden" CssClass="hidelbl"></asp:Label>
</asp:Content>
