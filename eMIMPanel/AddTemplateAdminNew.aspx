<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="AddTemplateAdminNew.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="eMIMPanel.AddTemplateAdminNew" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </asp:ToolkitScriptManager>

    <div class="container-fluid">
        <nav aria-label="breadcrumb" class="my-3">
            <ol class="breadcrumb breadcrumb-info">
                <li class="breadcrumb-item"><a href="#">Home</a></li>
                <li class="breadcrumb-item active" aria-current="page">Add Template</li>
            </ol>
        </nav>
        <!-- Content Row -->

        <div class="row">

            <!-- Area Chart -->
            <div class="col-xl-12 col-lg-12">
                <!-- Basic Card Example -->
                <div class="card bg-primary border-light shadow-soft mb-4">
                    <div class="card-header py-3 bg-primary">
                        <div class="row">
                            <div class="col-md-10">
                                <h6 class="m-0 font-weight-bold"><i class="far fa-user-circle"></i>Add New Template for Panel & API
                                         <asp:LinkButton class="btn btn-primary btn-icon-split" ID="lnkbtnBack" runat="server" PostBackUrl="~/AddTemplateAdmin.aspx">
                                            <span class="text-success"><i class="fas fa-backward"></i></span>     
                                             <span class="text-success font-weight-bold">Back</span> 
                                         </asp:LinkButton>
                                </h6>
                            </div>
                        </div>
                    </div>
                    <div class="card-body">
                        <asp:Panel ID="pnlMain" runat="server">
                            <div class="form-row">
                                <div class="col-md-3 mb-3">
                                    <asp:TextBox class="form-control" ID="txtUser" runat="server" placeholder="User Name" ToolTip="User Name" />
                                </div>
                                <div class="col-md-3 mb-3">
                                    <asp:TextBox class="form-control" ID="txtSenderId" runat="server" placeholder="Sender Id" ToolTip="Sender Id" />
                                    <div class="valid-feedback">
                                        Looks good!                             
                                    </div>
                                </div>
                                <div class="col-md-3 mb-3" style="text-align: right;">
                                    <asp:LinkButton class="btn btn-primary btn-icon-split" ID="lnkShow" runat="server" OnClick="lnkShow_Click">
                                            <span class="text-success"><i class="fas fa-eye"></i></span>
                                            <span class="text-success font-weight-bold">Show</span>
                                    </asp:LinkButton>
                                </div>
                            </div>

                            <div class="form-row">
                                <div class="col-md-6 mb-3">
                                    <asp:TextBox class="form-control" ID="txtTempId" runat="server" placeholder="Template Id" ToolTip="Template Id" />
                                    <div class="valid-feedback">
                                        Looks good!                             
                                    </div>
                                </div>
                                <div class="col-md-6 mb-3">
                                    <asp:TextBox class="form-control" ID="txtTempName" runat="server" placeholder="Template Name" ToolTip="Template Name" />
                                    <div class="valid-feedback">
                                        Looks good!                             
                                    </div>
                                </div>
                            </div>

                            <div class="form-row">
                                <div class="col-md-12 mb-12">
                                    <asp:TextBox class="form-control" ID="txtTemplateContent" runat="server" placeholder="Template Content" TextMode="MultiLine" Rows="5" ToolTip="Template Content" />
                                    <div class="valid-feedback">
                                        Looks good!                             
                                    </div>
                                </div>

                            </div>

                            <div class="form-row">
                                <div class="col-md-10" style="text-align: left;">
                                    <asp:LinkButton class="btn btn-primary btn-icon-split  mt-2" ID="btnSave" runat="server" OnClick="btnSave_Click">
                                            <span class="text-success"><i class="fas fa-save"></i></span>
                                            <span class="text-success font-weight-bold">Save</span>
                                    </asp:LinkButton>
                                </div>
                                <%--  <div class="col-md-2" style="text-align: right;">
                                    <asp:LinkButton runat="server" ID="lnkDownload" OnClick="lnkDownload_Click" class="btn btn-primary btn-icon-split  mt-2">
                                                    Download <i class="fas fa-download" aria-hidden="true"></i>
                                    </asp:LinkButton>
                                </div>--%>
                            </div>
                            <div class="mt-5">
                                <asp:GridView ID="grvTemplate" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                                    runat="server" Width="100%" CellPadding="10"
                                    BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive dataTable-view">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="5%" HeaderText="Sr No">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Template Id" HeaderStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTemplateId" runat="server" Width="100%" Text='<%#Eval("TemplateId")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Template Name" HeaderStyle-Width="15%">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTemplateName" runat="server" Text='<%#Eval("tempname")%>' Width="100%"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Template" HeaderStyle-Width="60%">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtTemplate" runat="server" TextMode="MultiLine" ReadOnly="true" Text='<%#Eval("Template")%>' Rows="3" Width="100%"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--   <asp:TemplateField HeaderText="Action" HeaderStyle-Width="10%">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDelete" runat="server" class="mx-1 btn btn-primary text-danger"
                                                    OnClientClick="return confirm('Do you want to Remove Template ?');" OnClick="lnkDelete_Click"
                                                    data-toggle="tooltip" data-placement="top" title="" data-original-title="Declined"> 
                                                         <span class="text-danger"> <i class="fas fa-trash"></i> </span></asp:LinkButton>
                                                <asp:LinkButton ID="lnkTest" runat="server" class="mx-1 btn btn-primary table-success"
                                                    OnClick="lnkTest_Click"
                                                    data-toggle="tooltip" data-placement="top" title="" data-original-title="Send SMS"> 
                                                         <span class="text-success"> <i class="fas fa-share"></i> </span></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <table style="width: 100%;">
                                            <tr>
                                                <td align="center">
                                                    <asp:Label ID="lblEmpty" Text="No Data Found!!!" Style="color: Red;" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </div>
                        </asp:Panel>

                    </div>
                </div>
            </div>

            <!-- Pie Chart -->
            <div class="col-xl-4 col-lg-5"></div>
        </div>

        <%-- Template Test POPUP For API --%>
        <asp:Panel ID="pnlPopUp_NUMBER" runat="server" CssClass="modalPopup" Style="display: none;">
            <div style="overflow-y: auto; overflow-x: hidden; max-height: 500px; width: 600px;">
                <div class="modal-header">
                    <asp:Label ID="Label132" runat="server" CssClass="modal-title" Text="Send Message"></asp:Label>
                </div>
                <div class="modal-body">
                    <div>
                        <div class="form-row">
                            <div class="col-md-6">
                                <asp:TextBox class="form-control" ID="txtTestUser" runat="server" placeholder="User Name" ToolTip="User" />
                            </div>
                            <div class="col-md-6">
                                <asp:TextBox class="form-control" ID="txtTestMobile" runat="server" placeholder="Mobile" ToolTip="Mobile" />
                            </div>
                        </div>

                        <div class="form-row mt-2">
                            <div class="col-md-6">
                                <asp:TextBox class="form-control" ID="txtTestSender" ReadOnly="true" runat="server" placeholder="Sender Id" ToolTip="Sender Id" />
                            </div>
                        </div>

                        <div class="form-row">
                            <div class="col-md-12 mb-12 mt-2">
                                <asp:TextBox class="form-control" ID="txtTestMessage" runat="server" placeholder="Message" TextMode="MultiLine" Rows="5" ToolTip="Message" />

                            </div>
                        </div>

                        <div class="form-row mt-2">
                            <div class="col-md-4">
                                <asp:Label class="form-control" ID="lblTotalMessage" runat="server" />
                            </div>
                            <div class="col-md-8">
                                <asp:Label class="form-control" ID="lblMessageLength" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
                <div align="center" class="modal-footer">
                    <div class="row">
                        <div class="col-md-6">
                            <asp:Button ID="btnSend" runat="server" Text="Send" class="btn btn-primary" OnClick="btnSend_Click" />

                        </div>
                        <div class="col-md-6">
                            <button id="btnCancel2" runat="server" class="btn btn-primary">Cancel</button>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
        <asp:LinkButton ID="lnkNumber" runat="server"></asp:LinkButton>
        <asp:ModalPopupExtender ID="pnlPopUp_NUMBER_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground"
            PopupControlID="pnlPopUp_NUMBER" TargetControlID="lnkNumber" CancelControlID="btnCancel2">
        </asp:ModalPopupExtender>

        <%-- Template Test POPUP For SMS PANEL --%>

        <asp:Panel ID="PanelLinkext" runat="server" CssClass="modalPopup" Style="display: none;">
            <div style="overflow-y: auto; overflow-x: hidden; max-height: 500px;">
                <div class="modal-header">
                    <asp:Label ID="Label1" runat="server" CssClass="modal-title" Text="Send Message"></asp:Label>
                </div>
                <div class="modal-body">
                    <div>
                        <div class="form-row">
                            <div class="col-md-6">
                                <asp:TextBox class="form-control" ID="txtLinkUser" runat="server" placeholder="User Name" ToolTip="User" />
                            </div>
                            <div class="col-md-6">
                                <asp:TextBox class="form-control" ID="txtLinkMobile" runat="server" placeholder="Mobile" ToolTip="Mobile" />
                            </div>
                        </div>

                        <div class="form-row mt-2">
                            <div class="col-md-6">
                                <asp:DropDownList class="form-control" ID="ddlLinkrSender" runat="server" placeholder="Sender Id" />
                            </div>
                        </div>

                        <div class="form-row">
                            <div class="col-md-12 mb-12 mt-2">
                                <asp:TextBox class="form-control" ID="txtLinkMessage" runat="server" placeholder="Message" TextMode="MultiLine" Rows="5" ToolTip="Message" />

                            </div>
                        </div>
                    </div>
                </div>
                <div align="center" class="modal-footer">
                    <div class="row">
                        <div class="col-md-6">
                            <asp:Button ID="btnLinkSend" runat="server" Text="Send" class="btn btn-primary" OnClick="btnLinkSend_Click" />

                        </div>
                        <div class="col-md-6">
                            <button id="Button2" runat="server" class="btn btn-primary">Cancel</button>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
        <asp:LinkButton ID="LinkButtonLinkext" runat="server"></asp:LinkButton>
        <asp:ModalPopupExtender ID="pnlPopUp_Linkext_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground"
            PopupControlID="PanelLinkext" TargetControlID="LinkButtonLinkext" CancelControlID="Button2">
        </asp:ModalPopupExtender>

        <style type="text/css">
            /*CSS Classes For Design Modal*/
            .modalPopup {
                min-height: 75px;
                position: fixed;
                z-index: 2000;
                padding: 0;
                background-color: #fff;
                border-radius: 6px;
                background-clip: padding-box;
                border: 1px solid rgba(0, 0, 0, 0.2);
                min-width: 290px;
                box-shadow: 0 5px 10px rgba(0, 0, 0, 0);
            }

            .modalBackground {
                position: fixed;
                top: 0;
                left: 0;
                background-color: #000;
                opacity: 0.5;
                z-index: 1800;
                min-height: 100%;
                width: 100%;
                overflow: hidden;
                filter: alpha(opacity=50);
                display: inline-block;
                z-index: 1000;
            }
        </style>
    </div>
</asp:Content>
