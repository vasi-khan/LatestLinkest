<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="RCS_Summary_U.aspx.cs" EnableEventValidation="false" Inherits="eMIMPanel.RCS_Summary_U" %>

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
            <nav aria-label="breadcrumb" class="my-3">
                <ol class="breadcrumb breadcrumb-info">
                    <li class="breadcrumb-item"><a href="#">Home</a></li>
                    <li class="breadcrumb-item"><a href="#">Reports</a></li>
                    <li class="breadcrumb-item active" aria-current="page">RCS Summary</li>
                </ol>
            </nav>
            <div class="row">
                <div class="col-12">
                    <div class="card card-body mb-4 bg-primary border-light shadow-soft">

                        <div class="form-row">
                            <div class="right-view">

                                <div class="row">
                                    <%-- <div class="col-md-2">
                                        <asp:Label ID="lblProfile" runat="server" Text="User_ID"></asp:Label>
                                    </div>
                                    --%>
                                    <div class="col-md-3">
                                        <%--   <asp:Label ID="Label1" runat="server" Text="User_ID"></asp:Label>--%>
                                        <%--<asp:TextBox ID="txtProfile" runat="server" class="form-control" placeholder="User ID" ToolTip="User ID" autocomplete="off"></asp:TextBox>--%>
                                        <asp:DropDownList ID="ddlCamp" runat="server" class="custom-select my-3 my-lg-0">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2" style="display: none">
                                        <%--    <asp:Label ID="lblSenderId" runat="server" Text="Sender_ID"></asp:Label>--%>
                                        <asp:TextBox ID="txtSenderId" runat="server" class="form-control" placeholder="Sender ID" ToolTip="Sender ID" autocomplete="off"></asp:TextBox>

                                    </div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtFrm" runat="server" onchange="javascript:text_changed_from();" class="form-control datepicker" placeholder="From Date" autocomplete="off"></asp:TextBox>
                                        <asp:HiddenField ID="hdntxtFrm" runat="server" />
                                    </div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtTo" runat="server" onchange="javascript:text_changed_to();" class="form-control datepicker" placeholder="To Date" autocomplete="off"></asp:TextBox>
                                        <asp:HiddenField ID="hdntxtTo" runat="server" />
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
                                <span class="icon-title h6 mb-0 font-weight-bold"><span class="fas fa-chart-line"></span>RCS Summary</span>
                                <span class="icon"><span class="fas fa-plus"></span></span>
                            </a>

                            <!-- <div class="card-header" id="headingOne">
                                <h2 class="mb-0">
                                    <button class="btn btn-link btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseOne" aria-expanded="true" aria-controls="collapseOne">
                                        Analytics
                                    </button>
                                </h2>
                            </div> -->
                            <div class="row justify-content-center">
                                <asp:LinkButton runat="server" ID="lnkDownload" OnClick="lnkDownload_Click" class="btn btn-mini">
                                Download <i class="fas fa-download" aria-hidden="true"></i>
                                </asp:LinkButton>
                            </div>


                            <div id="collapseOne" class="collapse show" aria-labelledby="headingOne" data-parent="#accordionExample" runat="server">
                                <div class="card-body px-0">
                                    <div class="row">
                                        <!-- Area Chart -->
                                        <div class="col-xl-12 col-lg-12">
                                            <!-- Loader -->
                                            <%--<div class="o-page-loader">
                                                <div class="o-page-loader--content">
                                                    <div class="o-page-loader--spinner"></div>
                                                    <div class="o-page-loader--message">
                                                        <span>Loading...</span>
                                                    </div>
                                                </div>
                                            </div>--%>
                                            <!--End Loader -->

                                            <div class="table-responsive">
                                                <asp:GridView ID="grv" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false" ShowFooter="true"
                                                    runat="server" Width="100%" CellPadding="10" BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive wrap dataTable-view">
                                                    <Columns>
                                                        <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr No">
                                                            <ItemTemplate>
                                                                <%#Container.DataItemIndex+1 %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Action">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="LinkButton2" runat="server" OnClick="btnView_Click" class="btn btn-datatable btn-icon btn-transparent-dark px-2 py-0" data-toggle="tooltip" data-placement="top" title="Details"><i class="far fa-file-alt"></i></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDate" runat="server" Text='<%#Eval("CreatedDate")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="User ID">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblUserId" runat="server" Text='<%#Eval("username")%>'></asp:Label>
                                                                <asp:HiddenField ID="hdndate" runat="server" Value='<%#Eval("CreatedDate")%>'></asp:HiddenField>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <%--<asp:TemplateField HeaderText="Sender ID">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSenderId" runat="server" Text='<%#Eval("senderid")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>--%>
                                                        <asp:TemplateField HeaderText="Submitted">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl1" runat="server" Text='<%#Eval("SUBMITTED")%>'></asp:Label>

                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Delivered">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl2" runat="server" Text='<%#Eval("DELIVERED")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Failed">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl3" runat="server" Text='<%#Eval("FAIL")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Rejected">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblurlid" runat="server" Text='<%#Eval("REJECTED")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Unknown">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl8" runat="server" Text='<%#Eval("unknown")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Seen">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl9" runat="server" Text='<%#Eval("Seen")%>'></asp:Label>
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

                            <!-- <div class="card-header" id="headingTwo">
                                <h2 class="mb-0">
                                    <button class="btn btn-link btn-block text-left collapsed" type="button" data-toggle="collapse" data-target="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo">
                                        Analytics Detail
                                    </button>
                                </h2>
                            </div> -->

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
                    <asp:Label ID="lblHeading" runat="server" CssClass="modal-title" Text="User ID Wise Details"></asp:Label>
                    <asp:LinkButton runat="server" ID="Lnkdwnload" OnClick="Lnkdownload_Click1" class="btn btn-mini row justify-content-center">
                                Download <i class="fas fa-download" aria-hidden="true"></i>
                    </asp:LinkButton>
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
                                        <asp:Label ID="lblDate" runat="server" Text='<%#Eval("CreatedDate")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="User ID">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUserId" runat="server" Text='<%#Eval("username")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="Sender ID">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSenderId" runat="server" Text='<%#Eval("senderid")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Msg ID">
                                    <ItemTemplate>
                                        <asp:Label ID="lblmsgid" runat="server" Text='<%#Eval("messageId")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Mobile No">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl1" runat="server" Text='<%#Eval("MobNo")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sent Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl2" runat="server" Text='<%#Eval("SendDate")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Delivery Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl3" runat="server" Text='<%#Eval("DeliveryDate")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Msg Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl4" runat="server" Text='<%#Eval("RCSType")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Msg Text">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl5" runat="server" Text='<%#Eval("MsgText")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl6" runat="server" Text='<%#Eval("sentstatus")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Delivery Response">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl7" runat="server" Text='<%#Eval("DeliveryResponse")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="SeenTime">
                                 <ItemTemplate>
                                     <asp:Label ID="lbl8" runat="server" Text='<%#Eval("seentime")%>'></asp:Label>
                                 </ItemTemplate>
                             </asp:TemplateField>
                             <asp:TemplateField HeaderText="Seen Status">
                                 <ItemTemplate>
                                     <asp:Label ID="lbl9" runat="server" Text='<%#Eval("seenstatus")%>'></asp:Label>
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

    <%--<script>
        (function (loader) {

            window.addEventListener('beforeunload', function (e) {
                activateLoader();
            });

            window.addEventListener('load', function (e) {
                deactivateLoader();
            });

            function activateLoader() {
                loader.style.display = 'block';
                loader.style.opacity = 1;
            }

            function deactivateLoader() {
                /**
                 * ensures that the loading animation plays for at least a second to give the
                 * appearance of seamless loading on pages that execute and load extremely
                 * quickly (i.e., intranet pages)
                 */
                setTimeout(function () {
                    deactivate();
                }, 1000);

                function deactivate() {
                    loader.style.opacity = 0;
                    loader.addEventListener('transitionend', function () {
                        loader.style.display = 'none';
                    }, false);
                }
            }

        })(document.querySelector('.o-page-loader'));
</script>--%>
</asp:Content>
