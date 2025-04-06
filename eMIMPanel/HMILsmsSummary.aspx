<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="HMILsmsSummary.aspx.cs" Inherits="eMIMPanel.HMILsmsSummary" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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
    <cc1:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </cc1:ToolkitScriptManager>
    <main>
        <div class="container-fluid">
            <nav aria-label="breadcrumb" class="my-3">
                <ol class="breadcrumb breadcrumb-info">
                    <li class="breadcrumb-item"><a href="#">Home</a></li>
                    <li class="breadcrumb-item active" aria-current="page">HMIL SMS Summary</li>
                </ol>
            </nav>
            <div class="row">
                <div class="col-12">
                    <div class="card card-body mb-4 bg-primary border-light shadow-soft">
                        <div class="form-row">
                            <div class="right-view">
                                <div class="row">
                                    <div class="col-md-2">
                                        <asp:TextBox ID="txtFrm" runat="server" onchange="javascript:text_changed_from();" class="form-control datepicker" placeholder="From Date" autocomplete="off"></asp:TextBox>
                                        <asp:HiddenField ID="hdntxtFrm" runat="server" />
                                    </div>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="txtTo" runat="server" onchange="javascript:text_changed_to();" class="form-control datepicker" placeholder="To Date" autocomplete="off"></asp:TextBox>
                                        <asp:HiddenField ID="hdntxtTo" runat="server" />
                                    </div>
                                    <div class="col-md-2">
                                        <asp:ListBox ID="lstHMIL" runat="server" SelectionMode="Multiple" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="lstHMIL_SelectedIndexChanged">
                                            <asp:ListItem Text="HYUNDAISALES" Value="HYUNDAISALES" />
                                            <asp:ListItem Text="HYUNDAISERVICE" Value="HYUNDAISERVICE" />
                                            <asp:ListItem Text="DEALERSALES" Value="DEALERSALES" />
                                            <asp:ListItem Text="DEALERSERVICE" Value="DEALERSERVICE" />
                                            <asp:ListItem Text="HASC" Value="HASC" />
                                        </asp:ListBox>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:DropDownList ID="ddlSenderID" runat="server" class="custom-select" AutoPostBack="true" OnSelectedIndexChanged="ddlSenderID_SelectedIndexChanged">
                                            <asp:ListItem Value="0">Select SenderID</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:DropDownList ID="ddlTemplate" runat="server" class="custom-select" ClientIDMode="Static">
                                            <asp:ListItem Value="0">Select Tempelte ID</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:LinkButton runat="server" ID="aaaa" OnClick="btnUpdate_Click" class="btn btn-mini">
                                                    Show <i class="fas fa-eye" aria-hidden="true"></i>
                                        </asp:LinkButton>
                                    </div>
                                    <asp:HiddenField ID="h1" runat="server" />
                                    <asp:HiddenField ID="h2" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- Card End -->

                    <div class="accordion shadow-soft rounded" id="accordionExample">
                        <div class="card card-sm card-body bg-primary border-light mb-0">
                            <a href="#collapseOne" id="headingOne" data-target="#collapseOne" class="accordion-panel-header" data-toggle="collapse" role="button" aria-expanded="true" aria-controls="collapseOne">
                                <span class="icon-title h6 mb-0 font-weight-bold"><span class="fas fa-chart-line"></span>SMS Summary</span>
                                <span class="icon"><span class="fas fa-plus"></span></span>
                            </a>
                            <div class="row justify-content" style="margin-left: 85%;">
                                <asp:LinkButton runat="server" ID="lnkDownload" OnClick="lnkDownload_Click" class="btn btn-mini">
                                Download <i class="fas fa-download" aria-hidden="true"></i>
                                </asp:LinkButton>
                            </div>

                            <div id="collapseOne" class="collapse show" aria-labelledby="headingOne" data-parent="#accordionExample" runat="server">
                                <div class="card-body px-0">
                                    <div class="row">
                                        <!-- Area Chart -->
                                        <div class="col-xl-12 col-lg-12">
                                            <div class="table-responsive">
                                                <asp:GridView ID="grv" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false" ShowFooter="true"
                                                    runat="server" Width="100%" CellPadding="10" BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive wrap dataTable-view">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sr No">
                                                            <ItemTemplate>
                                                                <%#Container.DataItemIndex+1 %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="User ID">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblUserId" runat="server" Text='<%#Eval("userid")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Dealer Code">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDealerCode" runat="server" Text='<%#Eval("FULLNAME")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Dealer Name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDealerName" runat="server" Text='<%#Eval("COMPNAME")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="From Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFromDate" runat="server" Text='<%#Eval("FromDATE")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="To Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblToDate" runat="server" Text='<%#Eval("ToDATE")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Submitted">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSubmitted" runat="server" Text='<%#Eval("Submitted")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Delivered">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDelivered" runat="server" Text='<%#Eval("Delivered")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Failed">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFailed" runat="server" Text='<%#Eval("Failed")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Unknown">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblUnknown" runat="server" Text='<%#Eval("Unknown")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Sender ID" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSenderID" runat="server" Text='<%#Eval("SenderId")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Template ID" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTempalteID" runat="server" Text='<%#Eval("TemplateID")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!--Start Accordian 2   -->
                        <div class="card card-sm card-body bg-primary border-light mb-0 d-none">

                            <a href="#collapseTwo" id="headingTwo" data-target="#collapseTwo" class="accordion-panel-header" data-toggle="collapse" role="button" aria-expanded="true" aria-controls="collapseTwo">
                                <span class="icon-title h6 mb-0 font-weight-bold"><span class="fas fa-chart-bar"></span>Analytics Detail</span>
                                <span class="icon"><span class="fas fa-plus"></span></span>
                            </a>
                            <div id="collapseTwo" class="collapse" aria-labelledby="headingTwo" data-parent="#accordionExample" runat="server">
                                <div class="card-body px-0">
                                    <div style="text-align: right;">
                                        <asp:LinkButton runat="server" ID="LinkButton1" OnClick="btnCloseDetail_Click" class="btn btn-mini">Close Detail</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- End Accordian 2 -->
                    </div>
                </div>
            </div>
        </div>
    </main>

    <%--lnkDetail Link Button for ModalPopup as TargetControlID--%>
    <asp:LinkButton ID="lnkDetail" runat="server"></asp:LinkButton>

    <%--pnlPopUp_Detail Panel With Design--%>
    <asp:Panel ID="pnlPopUp_Detail" runat="server" CssClass="modal modalPopup" Style="display: none;">
        <div class="modal-dialog modal-xl modal-dialog-centered modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header">
                    <asp:Label ID="lblHeading" runat="server" CssClass="modal-title" Text="Sender ID Wise Details"></asp:Label>
                </div>
                <div class="modal-body">
                    <div class="">
                        <asp:GridView ID="grv2" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                            runat="server" Width="100%" CellPadding="10"
                            BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive nowrap dataTable-view">
                            <Columns>
                                <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr No">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDate" runat="server" Text='<%#Eval("SMSDATE")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="User ID">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUserId" runat="server" Text='<%#Eval("userid")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sender ID">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSenderId" runat="server" Text='<%#Eval("senderid")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Submitted">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl1" runat="server" Text='<%#Eval("Submitted")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Delivered">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl2" runat="server" Text='<%#Eval("Delivered")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Failed">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl3" runat="server" Text='<%#Eval("Failed")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Unknown">
                                    <ItemTemplate>
                                        <asp:Label ID="lblurlid" runat="server" Text='<%#Eval("Unknown")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div class="modal-footer">
                    <button id="btnCancel" runat="server" class="btn btn-primary">Close</button>
                </div>
            </div>
        </div>
    </asp:Panel>

    <%--pnlPopUp_Detail Modal Popup Extender For pnlPopUp_Detail--%>
    <cc1:ModalPopupExtender ID="pnlPopUp_Detail_ModalPopupExtender" runat="server" PopupControlID="pnlPopUp_Detail"
        TargetControlID="lnkDetail" BehaviorID="mpeAddUpdateEmployee" CancelControlID="btnCancel"
        BackgroundCssClass="modalBackground">
    </cc1:ModalPopupExtender>
     <link href="css/bootstrap-multiselect.css" rel="stylesheet" />
    <script src="js/bootstrap-multiselect.js"></script>
    <script type="text/javascript">
        $(function () {
            $('[id*=lstHMIL]').multiselect({
                includeSelectAllOption: true
            });
        });
    </script>
    <script type="text/javascript"> 
        function text_changed_from() {
            var d = document.getElementById("ContentPlaceHolder1_txtFrm").value
            console.log(d);
            document.getElementById("ContentPlaceHolder1_hdntxtFrm").value = d;
        }
        function text_changed_to() {
            var d = document.getElementById("ContentPlaceHolder1_txtTo").value
            console.log(d);
            document.getElementById("ContentPlaceHolder1_hdntxtTo").value = d;
        }
    </script>
</asp:Content>