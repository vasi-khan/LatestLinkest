<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="AccountSystemIdforPanel.aspx.cs" Inherits="eMIMPanel.AccountSystemIdforPanel" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .modalBackground {
            background-color: Black;
            filter: alpha(opacity=90);
            opacity: 0.8;
        }

        .modalPopup {
            border-radius: 5px;
            background-color: #FFFFFF;
            width: 50%;
            /*height: 140px;*/
            /* border-width: 3px;
                border-style: solid;
                border-color: black;
                padding-top: 10px;
                padding-left: 10px; */
        }

        .hide {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="upd1">
        <ContentTemplate>
            <div class="container-fluid">
                <nav aria-label="breadcrumb" class="my-3">
                    <ol class="breadcrumb breadcrumb-info">
                        <li class="breadcrumb-item"><a href="index2.aspx">Home</a></li>
                        <li class="breadcrumb-item active" aria-current="page">Account System Id For Panel</li>
                    </ol>
                </nav>

                <div class="row">
                    <div class="col-xl-12 col-lg-12">
                        <div class="card bg-primary border-light shadow-soft mb-4">

                            <div class="card-body px-4">
                                <asp:Panel ID="pnlMain" runat="server">
                                    <div class="row">
                                        <div class="col-md-2">
                                            <label>User Account Type</label>
                                        </div>
                                        <div class="col-md-1 ">
                                            <asp:RadioButton ID="rbAPI" runat="server" AutoPostBack="true" GroupName="type" Text="API" OnCheckedChanged="rbAPI_CheckedChanged" />
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RadioButton ID="rbPanel" runat="server" AutoPostBack="true" GroupName="type" Text="PANEL" OnCheckedChanged="rbAPI_CheckedChanged" />
                                        </div>
                                    </div>
                                    <div runat="server" class="row" id="divAPI" visible="true">
                                        <div class="col-md-2">
                                            <label>API Account Type</label>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RadioButton ID="rbTran" runat="server" AutoPostBack="true" GroupName="A" Text="Tran" />
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RadioButton ID="rbPromo" runat="server" AutoPostBack="true" GroupName="A" Text="Promo" />
                                        </div>
                                        <div class="col-md-10"></div>
                                        <div class="col-md-3">
                                            <label>User ID</label>
                                            <asp:TextBox class="form-control" runat="server" ID="txtUserIdAPI" AutoPostBack="true" MaxLength="25" placeholder="UAE USERID" OnTextChanged="txtUserIdAPI_TextChanged"></asp:TextBox>
                                            <asp:Label class="text-danger" ID="lblUserIdAPICheck" runat="server"></asp:Label>
                                        </div>
                                        <div class="col-md-3">
                                            <label>Provider</label>
                                            <asp:DropDownList CssClass="form-control" ID="ddlProviderAPI" runat="server"></asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="row" runat="server" id="divPanel" visible="false">
                                        <div class="col-md-3">
                                            <label>User ID</label>
                                            <asp:TextBox class="form-control" runat="server" ID="txtUserIdPanel" MaxLength="25" AutoPostBack="true" placeholder="USERID" OnTextChanged="txtUserIdPanel_TextChanged"></asp:TextBox>
                                            <asp:Label class="text-danger" ID="lblUserIdPanel" runat="server"></asp:Label>
                                        </div>
                                        <div class="col-md-3">
                                            <label>Provider</label>
                                            <asp:DropDownList ID="ddlProviderPanel" CssClass="form-control" runat="server"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-3">
                                            <label>Manual ACID</label>
                                            <asp:DropDownList ID="ddlManualAcid" CssClass="form-control" runat="server"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="row py-2">
                                        <div class="col-md-1">
                                            <asp:Button ID="btnSave" runat="server" Text="SAVE" class="btn btn-success btn-sm " OnClick="btnSave_Click" />
                                        </div>
                                        <div class="col-md-1">
                                            <asp:Button ID="btnClear" runat="server" Text="RESET" class="btn btn-danger btn-sm " OnClick="btnClear_Click" />
                                        </div>
                                    </div>
                                </asp:Panel>

                                <asp:Button ID="btnpop" runat="server" Text="show" Style="display: none" />
                                <!-- ModalPopupExtender -->
                                <asp:ModalPopupExtender ID="mp1" ClientIDMode="Static" runat="server" PopupControlID="Panel1" CancelControlID="btnclose" TargetControlID="btnpop" BackgroundCssClass="modalBackground">
                                </asp:ModalPopupExtender>
                                <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" align="center" Style="display: none">
                                    <div class="card">
                                        <div class="card-body">
                                            <!--  -->
                                            <div class="row">
                                                <div class="col-sm-12">
                                                    <asp:TextBox CssClass="form-control" ID="txtPWD" type="password" placeholder="Profile Password" runat="server"></asp:TextBox>
                                                    <asp:Label ID="lblPWDCheck" runat="server"></asp:Label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="card-footer">
                                            <asp:LinkButton runat="server" ID="btnSuccess" Text="Check" CssClass="btn btn-Success" OnClick="btnSuccess_Click" />
                                            <asp:LinkButton runat="server" ID="btnclose" Text="Close" CssClass="btn btn-danger" OnClick="btnclose_Click" />
                                        </div>
                                </asp:Panel>
                            </div>

                            <div class="mt-5" runat="server" id="divgrdtran">
                                <asp:GridView ID="grdAPITran" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                                    runat="server" Width="100%" CellPadding="10"
                                    BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive dataTable-view">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="5%" HeaderText="Sr No">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="User Id" HeaderStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label ID="lbluserid" runat="server" Text='<%#Eval("userid")%>' Width="100%"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status" HeaderStyle-Width="25%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblactive" runat="server" Text='<%#Eval("active")%>' Width="100%"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Creation Time" HeaderStyle-Width="60%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblinsertdate" runat="server" Text='<%#Eval("insertdate")%>' Width="100%"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Provider" HeaderStyle-Width="60%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblprovider" runat="server" Text='<%#Eval("ACCOUNT")%>' Width="100%"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action" HeaderStyle-Width="20%">
                                            <ItemTemplate>
                                                <asp:HiddenField Value='<%#Eval("userid") %>' ID="HD_usid" runat="server" />
                                                <asp:HiddenField Value='<%#Eval("insertdate") %>' ID="HD_dtime" runat="server" />
                                                <asp:HiddenField Value='<%#Eval("ACCOUNT") %>' ID="HD_provider" runat="server" />
                                                <asp:LinkButton ID="lnkDelete" runat="server" class="mx-1 btn btn-primary text-danger"
                                                    OnClientClick="return confirm('Are you sure You want to Delete this ?');" OnClick="lnkDelete_Click"
                                                    data-toggle="tooltip" data-placement="top" title="" data-original-title="Declined"> 
                                                         <span class="text-danger"> <i class="fas fa-trash"></i> </span></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <table style="width: 100%;">
                                            <tr>
                                                <td align="center">
                                                    <asp:Label ID="lblEmpty" Text="No Data Found !!" Style="color: Red;" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </div>

                            <div class="mt-5" runat="server" id="divgrdpromo">
                                <asp:GridView ID="GridAPIPROMO" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                                    runat="server" Width="100%" CellPadding="10"
                                    BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive dataTable-view">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="5%" HeaderText="Sr No">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="User Id" HeaderStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label ID="lbluserid" runat="server" Text='<%#Eval("userid")%>' Width="100%"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sender Id" HeaderStyle-Width="25%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblsenderid" runat="server" Text='<%#Eval("senderid")%>' Width="100%"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Creation Time" HeaderStyle-Width="60%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblinsertdate" runat="server" Text='<%#Eval("insertdate")%>' Width="100%"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Provider" HeaderStyle-Width="60%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblprovider" runat="server" Text='<%#Eval("ACCOUNT")%>' Width="100%"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action" HeaderStyle-Width="20%">
                                            <ItemTemplate>
                                                <asp:HiddenField Value='<%#Eval("userid") %>' ID="HD_usid" runat="server" />
                                                <asp:HiddenField Value='<%#Eval("insertdate") %>' ID="HD_dtime" runat="server" />
                                                <asp:HiddenField Value='<%#Eval("ACCOUNT") %>' ID="HD_provider" runat="server" />
                                                <asp:LinkButton ID="lnkAPIPromoDelete" runat="server" class="mx-1 btn btn-primary text-danger"
                                                    OnClientClick="return confirm('Are you sure You want to Delete this ?');" OnClick="lnkAPIPromoDelete_Click"
                                                    data-toggle="tooltip" data-placement="top" title="" data-original-title="Declined"> 
                                                         <span class="text-danger"> <i class="fas fa-trash"></i> </span></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <table style="width: 100%;">
                                            <tr>
                                                <td align="center">
                                                    <asp:Label ID="lblEmpty" Text="No Data Found !!" Style="color: Red;" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </div>

                            <div class="mt-5" runat="server" id="divpanelgrd">
                                <asp:GridView ID="grdPanel" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                                    runat="server" Width="100%" CellPadding="10"
                                    BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive dataTable-view">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr No">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="User Id" HeaderStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label ID="lbluserid" runat="server" Text='<%#Eval("userid")%>' Width="100%"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Smpp Account Id" HeaderStyle-Width="25%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblsmppaccountid" runat="server" Text='<%#Eval("Provider")%>' Width="100%"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status" HeaderStyle-Width="20%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblactive" runat="server" Text='<%#Eval("ACTIVE")%>' Width="100%"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Country Code" HeaderStyle-Width="20%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblcountrycode" runat="server" Text='<%#Eval("CountryCode")%>' Width="100%"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Manual AcId" HeaderStyle-Width="60%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblManualAcId" runat="server" Text='<%#Eval("ManualAcId")%>' Width="100%"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action" HeaderStyle-Width="20%">
                                            <ItemTemplate>
                                                <asp:HiddenField Value='<%#Eval("userid") %>' ID="HD_usid" runat="server" />
                                                <asp:HiddenField Value='<%#Eval("smppaccountid") %>' ID="HD_smppaccountid" runat="server" />
                                                <asp:HiddenField Value='<%#Eval("countrycode") %>' ID="HD_countrycode" runat="server" />
                                                <asp:HiddenField Value='<%#Eval("ManualAcId_id") %>' ID="HD_ManualAcId" runat="server" />
                                                <asp:LinkButton ID="lnkpanelDelete" runat="server" class="mx-1 btn btn-primary text-danger"
                                                    OnClientClick="return confirm('Are you sure You want to Delete this ?');" OnClick="lnkpanelDelete_Click"
                                                    data-toggle="tooltip" data-placement="top" title="" data-original-title="Declined"> 
                                                         <span class="text-danger"> <i class="fas fa-trash"></i> </span></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <table style="width: 100%;">
                                            <tr>
                                                <td align="center">
                                                    <asp:Label ID="lblEmpty" Text="No Data Found !!" Style="color: Red;" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="rbPanel" />
            <asp:PostBackTrigger ControlID="rbAPI" />
            <asp:PostBackTrigger ControlID="rbTran" />
            <asp:PostBackTrigger ControlID="rbPromo" />
            <asp:PostBackTrigger ControlID="ddlProviderAPI" />
            <asp:PostBackTrigger ControlID="txtUserIdPanel" />
        </Triggers>
    </asp:UpdatePanel>

    <script src="OffLineCDN/JS%20File/jquery-3.6.0.min.js"></script>
    <script>
        // Datatable Script 
        $(document).ready(function () {
            $('.dataTable-view').DataTable();
            $('.dataTable-view1').DataTable({ paging: false });

        });
    </script>
</asp:Content>
