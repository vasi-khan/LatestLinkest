<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true"  EnableEventValidation="false" CodeBehind="DNDSummary.aspx.cs" Inherits="eMIMPanel.DNDSummary" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

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

            <!-- Nav Start -->
            <nav aria-label="breadcrumb" class="my-3">
                <ol class="breadcrumb breadcrumb-info">
                    <li class="breadcrumb-item"><a href="#">Home</a></li>
                    <li class="breadcrumb-item active" aria-current="page">DND Summary</li>
                </ol>
            </nav>
            <!-- Nav End -->

            <!-- Start Row -->
            <div class="row">
                <div class="col-12">
                    <div class="card card-body mb-4 bg-primary border-light shadow-soft">
                        <div class="form-row">
                            <asp:RadioButton ID="rbHonda" runat="server" AutoPostBack="true" GroupName="FilterdateTemp" Checked="true" Style="margin-left: 20px;" OnCheckedChanged="rbHonda_CheckedChanged" />&nbsp;&nbsp;Honda
                            <asp:RadioButton ID="rbOthers" runat="server" AutoPostBack="true" OnCheckedChanged="rbOthers_CheckedChanged" Style="margin-left: 20px;" GroupName="FilterdateTemp" />&nbsp;&nbsp;Others
                        </div>
                        <div class="form-row">
                            <div class="right-view">
                                <div class="row">
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtFrm" runat="server" onchange="javascript:text_changed_from();" class="form-control datepicker" placeholder="From Date" autocomplete="off"></asp:TextBox>
                                        <asp:HiddenField ID="hdntxtFrm" runat="server" />
                                    </div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtTo" runat="server" onchange="javascript:text_changed_to();" class="form-control datepicker my-3 my-lg-0" placeholder="To Date" autocomplete="off"></asp:TextBox>
                                        <asp:HiddenField ID="hdntxtTo" runat="server" />
                                    </div>
                                    <div class="col-md-3" id="divOther" runat="server">
                                        <asp:TextBox ID="txtDltNO" runat="server" CssClass="form-control" placeholder="DLT No"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:LinkButton runat="server" ID="aaaa" OnClick="btnUpdate_Click" class="btn btn-block">
                                                    Show <i class="fas fa-eye" aria-hidden="true"></i>
                                        </asp:LinkButton>
                                    </div>
                                    <asp:HiddenField ID="h1" runat="server" />
                                    <asp:HiddenField ID="h2" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>

                <div class="card mb-4 bg-primary border-light shadow-soft">
                    <div class="card-header d-flex justify-content-between align-items-center border-bottom">
                        <div class="flex-fill m-0 font-weight-bold">SMS Summary</div>
                        <div class="flex-fill text-right">
                            <asp:LinkButton runat="server" ID="lnkDownload" OnClick="lnkDownload_Click" class="btn btn-mini">
                                Download <i class="fas fa-download" aria-hidden="true"></i>
                             </asp:LinkButton>
                        </div>
                    </div>

                    <div class="card-body">
                        <div id="collapseOne" class="collapse show" aria-labelledby="headingOne" data-parent="#accordionExample" runat="server">
                            <!--  -->
                            <div class="table-responsive">
                                <asp:GridView ID="grv" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false" ShowFooter="true"
                                    runat="server" Width="100%" CellPadding="10" BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive wrap dataTable-view">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr No">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SenderID Wise">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LinkButton2" runat="server" OnClick="btnView_Click" class="btn btn-datatable btn-icon btn-transparent-dark px-2 py-0" data-toggle="tooltip" data-placement="top" title="Details"><i class="far fa-file-alt"></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="TemplateID Wise" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTemplateId" runat="server" Text='<%#Eval("senderid")%>'></asp:Label>
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
                                                <asp:HiddenField ID="hdndate" runat="server" Value='<%#Eval("smsDate1")%>'></asp:HiddenField>
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
                                    <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                </asp:GridView>
                            </div>
                            <!--  -->
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
            <!-- End Row -->

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
