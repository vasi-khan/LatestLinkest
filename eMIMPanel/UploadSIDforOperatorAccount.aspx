<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="UploadSIDforOperatorAccount.aspx.cs" Inherits="eMIMPanel.UploadSIDforOperatorAccount" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        /*CSS Classes For Design Modal*/


        .modal.modalPopup {
            top: 0 !important;
            left: 0 !important;
            display: block;
        }

        .modalBackground {
            background-color: #000;
            opacity: 0.5;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="container-fluid">
                <nav aria-label="breadcrumb" class="my-3">
                    <ol class="breadcrumb breadcrumb-info">
                        <li class="breadcrumb-item"><a href="index2.aspx">Home</a></li>
                        <li class="breadcrumb-item active" aria-current="page">Upload SenderId for Account</li>
                    </ol>
                </nav>

                <div class="row">
                    <div class="col-xl-12 col-lg-12">
                        <div class="card bg-primary border-light shadow-soft mb-4">
                            <div class="card-body px-4">
                                <asp:Panel ID="pnlMain" runat="server">
                                    <div class="row">
                                        <div class="col-md-3">
                                            <label>Provider</label>
                                            <asp:DropDownList CssClass="form-control" ID="ddlProvider" runat="server"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-3">
                                            <label>SystemID</label>
                                            <asp:DropDownList CssClass="form-control" ID="ddlSystemId" runat="server"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-4">
                                            <label class="mr-3">SenderId Upload</label>
                                            <div class=" d-flex">
                                                <asp:FileUpload ID="FileUploadSenderID" runat="server" accept=".txt" class="form-control rounded-start rounded-0" ClientIDMode="Static" />
                                                <asp:LinkButton class="btn btn-success mt-0  text-center rounded-end rounded-0" ClientIDMode="Static" ID="btnUploadSenderId" runat="server" OnClick="btnUploadSenderId_Click">
                                           Go
                                                </asp:LinkButton>
                                            </div>
                                            <asp:Label ID="lblUploading" runat="server"></asp:Label>
                                        </div>
                                        <div class="col-md-1 py-4">
                                            <asp:Button ID="btnSubmit" CssClass="btn btn-success btn-sm" runat="server" Text="SUMBIT" OnClick="btnSubmit_Click" />
                                        </div>
                                        <div class="col-md-1 py-4">
                                            <asp:Button ID="btnReset" CssClass="btn btn-danger btn-sm" runat="server" Text="RESET" OnClick="btnReset_Click" />
                                        </div>
                                    </div>
                                    <div class="mt-5" runat="server" id="divgrdtran">
                                        <asp:GridView ID="grd" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                                            runat="server" Width="100%" CellPadding="10"
                                            BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive dataTable-view">
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-Width="15%" HeaderText="Sr No">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Provider" HeaderStyle-Width="35%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblProvider" runat="server" Text='<%#Eval("Provider")%>' Width="100%"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="SystemId" HeaderStyle-Width="35%">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSystemId" runat="server" Text='<%#Eval("SystemId")%>' Width="100%"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="SenderId" HeaderStyle-Width="30%">
                                                    <ItemTemplate>
                                                        <asp:HiddenField Value='<%#Eval("Provider") %>' ID="HD_Provider" runat="server" />
                                                        <asp:HiddenField Value='<%#Eval("SystemId") %>' ID="HD_SystemId" runat="server" />
                                                        <%--<asp:HiddenField Value='<%#Eval("InsertDateTime") %>' ID="HD_InsertDateTime" runat="server" />--%>
                                                        <asp:LinkButton ID="lnkInfo" runat="server" class="mx-1 btn btn-primary text-danger" OnClick="lnkinfo_Click">
                                                            <asp:Label ID="SID" runat="server" Text='<%#Eval("SID")%>' Width="100%"></asp:Label>
                                                        </asp:LinkButton>
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
                                    <asp:Panel ID="POPUPCOIT" class="modalsparePopup5" runat="server" Style="display: none">
                                        <div class=" modal-dialog-centered modal-dialog-scrollable modal-lg">
                                            <div class="modal-content  ">
                                                <div class="modal-header">
                                                    <h5 class="modal-title" runat="server" id="H5">SENDERID</h5>
                                                    <asp:LinkButton runat="server" ID="btnClose" AutoPostBAck="true" class="close">
                                                         <span  style="padding:6px">&times;</span>
                                                    </asp:LinkButton>

                                                </div>
                                                <div class="modal-body">
                                                    <div class="card">
                                                        <div class="row">
                                                            <div class="col-md-4">
                                                                <div class="form-group">
                                                                    <label>Provider :</label>
                                                                    <asp:Label runat="server" ID="lblProvider"></asp:Label>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <div class="form-group">
                                                                    <label>SystemId :</label>
                                                                    <asp:Label runat="server" ID="lblSystemId"></asp:Label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-6">
                                                                <label>SenderId :</label>
                                                                <asp:TextBox runat="server" CssClass="form-control" ID="txtSender"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-2 py-4">
                                                                <asp:Button runat="server" ID="btnGo" CssClass="btn btn-success" Text="GO" OnClick="btnGo_Click" />
                                                            </div>
                                                        </div>
                                                    </div>


                                                    <!--grid view Find-->
                                                    <div class=" mx-5 mb-3">
                                                        <div class="table-responsive" style="height: 350px; overflow-y: scroll;">
                                                            <asp:GridView ID="GridV" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                                                                runat="server" Width="100%" CellPadding="10"
                                                                BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive dataTable-view">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Sender Id" ItemStyle-Width="60%">
                                                                        <ItemTemplate>
                                                                            <%#Eval("SID") %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Inactive" ItemStyle-Width="10%">
                                                                        <ItemTemplate>
                                                                            <asp:HiddenField Value='<%#Eval("Id") %>' ID="HD_ID" runat="server" />
                                                                            <asp:CheckBox ID="chkInactive" runat="server" AutoPostBack="true" Checked='<%# Convert.ToBoolean(Eval("Active")) == false ? true : false %>' OnCheckedChanged="chkInactive_CheckedChanged" />
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="modal-footer p-2">
                                                    <asp:Button ID="Button5" runat="server" class="btn btn-danger btn-sm mt-0" Text="Close" UseSubmitBehavior="false"></asp:Button>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                    <asp:LinkButton runat="server" ID="Lnkb"></asp:LinkButton>
                                    <asp:ModalPopupExtender runat="server" ID="ModalPopitm" ClientIDMode="Static" TargetControlID="Lnkb" PopupControlID="POPUPCOIT" CancelControlID="btnClose" BackgroundCssClass="modalBackground">
                                    </asp:ModalPopupExtender>

                                    <!-- ModalPopupExtender -->
                                    <asp:Button ID="btnpop" runat="server" Text="show" Style="display: none" />

                                    <asp:ModalPopupExtender ID="mp1" ClientIDMode="Static" runat="server" PopupControlID="Panel1" CancelControlID="btnclose" TargetControlID="btnpop" BackgroundCssClass="modalBackground">
                                    </asp:ModalPopupExtender>
                                    <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" align="center" Style="display: none">
                                        <div class="card">
                                            <div class="card-body">
                                                <!--  -->
                                                <div class="row">
                                                    <div class="col-sm-12">
                                                        <asp:Label ID="lblStatus" runat="server"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="card-footer">
                                                <asp:LinkButton runat="server" ID="btnYes" Text="Yes" CssClass="btn btn-Success" OnClick="btnYes_Click" />
                                                <asp:LinkButton runat="server" ID="btnNo" Text="No" CssClass="btn btn-danger" OnClick="btnNo_Click" />
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
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
