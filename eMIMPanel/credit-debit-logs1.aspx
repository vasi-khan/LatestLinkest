<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="credit-debit-logs1.aspx.cs" Inherits="eMIMPanel.credit_debit_logs1" MaintainScrollPositionOnPostback="true" %>

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
                    <li class="breadcrumb-item active" aria-current="page">Credit Debit Logs</li>
                </ol>
            </nav>
            <!--  -->
            <div class="row">
                <div class="col-12">
                    <div class="card card-body bg-primary border-light shadow-soft mb-4">
                        <div class="row">
                            <div class="col-12 col-md-2">
                                <asp:TextBox ID="txtFrm" runat="server" onchange="javascript:text_changed_from();" class="form-control datepicker" placeholder="From Date" autocomplete="off"></asp:TextBox>
                                <asp:HiddenField ID="hdntxtFrm" runat="server" />
                            </div>
                            <div class="col-12 col-md-2">
                                <asp:TextBox ID="txtTo" runat="server" onchange="javascript:text_changed_to();" class="form-control datepicker my-3 my-lg-0" placeholder="To Date" autocomplete="off"></asp:TextBox>
                                <asp:HiddenField ID="hdntxtTo" runat="server" />
                            </div>
                            <div class="col-12 col-md-2">
                                <asp:TextBox ID="txtUserId" runat="server" class="form-control my-3 my-lg-0" placeholder="Userid" autocomplete="off" MaxLength="30"></asp:TextBox>
                            </div>
                            <div class="col-12 col-md-2">
                                <asp:CheckBox ID="chkIntoCreditDebit" runat="server" Text="Included Auto Credit Debit" />
                            </div>
                            <div class="col-6 col-md-2">
                                <asp:LinkButton runat="server" ID="LinkButton2" OnClick="btnUpdate_Click" class="btn btn-block">
                                            Show <i class="fas fa-eye" aria-hidden="true"></i>
                                </asp:LinkButton>

                            </div>
                            <div class="col-6 col-md-2">
                                <asp:LinkButton runat="server" ID="lnkDownload" OnClick="lnkDownload_Click" class="btn btn-block">
                                     <i class="fas fa-download" aria-hidden="true"></i> Download
                                </asp:LinkButton>
                            </div>
                            <asp:HiddenField ID="h1" runat="server" />
                            <asp:HiddenField ID="h2" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <!--  -->

            <!-- Content Row -->
            <div class="row">
                <div class="col-xl-12 col-lg-12">
                    <!-- Card-->
                    <div class="card bg-primary border-light shadow-soft mb-4">
                        <div class="card-header py-3 bg-primary d-flex justify-content-between flex-wrap align-content-center border-bottom">
                            <h6 class="m-0 font-weight-bold my-auto">Credit Debit Logs</h6>
                        </div>
                        <div class="card-body">
                            <div class="table-responsive">
                                <asp:GridView ID="grv" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                                    runat="server" Width="100%" CellPadding="10" BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive nowrap dataTable-view">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr No">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnView" runat="server" class="mx-1 btn btn-primary text-success"
                                                    OnClick="btnView_Click" data-toggle="tooltip" data-placement="top" title=""
                                                    data-original-title="View"> 
                                                        <span class="text-success"> <i class="fas fa-eye"></i> </span></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <%--<asp:TemplateField  HeaderText="SMS_Credit">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnViewSMS" runat="server" class="mx-1 btn btn-primary text-success"
                                                    OnClick="btnViewSMS_Click" data-toggle="tooltip" data-placement="top" title=""
                                                    data-original-title="View"> 
                                                        <span class="text-success"> <i class="fas fa-eye"></i> </span></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>

                                        <asp:TemplateField HeaderText="User ID">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUserId" runat="server" Text='<%#Eval("username")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Company Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCompName" runat="server" Text='<%#Eval("compname")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sender ID">
                                            <ItemTemplate>
                                                <asp:Label ID="lblsender" runat="server" Text='<%#Eval("senderid")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Mobile No">
                                            <ItemTemplate>
                                                <asp:Label ID="lblmobile" runat="server" Text='<%#Eval("mobile1")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Email Id">
                                            <ItemTemplate>
                                                <asp:Label ID="lblemail" runat="server" Text='<%#Eval("email")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lbltdate" runat="server" Text='<%#Eval("tdate")%>'></asp:Label>
                                                <asp:HiddenField ID="hdnfrmdt" runat="server" Value='<%#Eval("frmdate")%>' />
                                                <asp:HiddenField ID="hdntodt" runat="server" Value='<%#Eval("todate")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Credit">
                                            <ItemTemplate>
                                                <asp:Label ID="lblcr" runat="server" Text='<%#Eval("cr")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Debit">
                                            <ItemTemplate>
                                                <asp:Label ID="lbldr" runat="server" Text='<%#Eval("dr")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Balance">
                                            <ItemTemplate>
                                                <asp:Label ID="lblbalance" runat="server" Text='<%#Eval("balance")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%-- <asp:TemplateField HeaderText="Remarks">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("Remarks")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                    </Columns>
                                </asp:GridView>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <!--  -->

        </div>
    </main>


    <!-- Modal Popup for Detail -->

    <%--lnkDetail Link Button for ModalPopup as TargetControlID--%>
    <asp:LinkButton ID="lnkDetail" runat="server"></asp:LinkButton>

    <%--pnlPopUp_Detail Panel With Design--%>
    <asp:Panel ID="pnlPopUp_Detail" runat="server" CssClass="modal modalPopup" Style="display: none;">
        <div class="modal-dialog modal-xl modal-dialog-centered modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header">
                    <asp:Label ID="lblHeading" runat="server" CssClass="modal-title" Text="Credit Debit Log Details"></asp:Label>
                </div>
                <div class="modal-body">
                    <div class="">
                        <asp:GridView ID="grvDtl" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                            runat="server" Width="100%" CellPadding="10"
                            BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive nowrap dataTable-view">
                            <Columns>
                                <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr No">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Transaction Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTranDate" runat="server" Text='<%#Eval("trandate")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Transaction Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lbltrantype" runat="server" Text='<%#Eval("trantype")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAmount" runat="server" Text='<%#Eval("Amount")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remarks">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("Remarks")%>'></asp:Label>
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
    <!-- Modal Popup for Detail -->


    <!-- Modal Popup for Detail -->

    <%--lnkDetail Link Button for ModalPopup as TargetControlID--%>
    <asp:LinkButton ID="lnkDetail1" runat="server"></asp:LinkButton>

    <%--pnlPopUp_Detail Panel With Design--%>
    <asp:Panel ID="pnlPopUp_Detail1" runat="server" CssClass="modal modalPopup" Style="display: none;">
        <div class="modal-dialog modal-xl modal-dialog-centered modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header">
                    <asp:Label ID="lblheading11" runat="server" CssClass="modal-title" Text="SMS Credit Log Details"></asp:Label>
                </div>
                <div class="modal-body">
                    <div class="">
                        <asp:GridView ID="grvDtl1" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                            runat="server" Width="100%" CellPadding="10"
                            BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive nowrap dataTable-view">
                            <Columns>
                                <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr No">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Transaction Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTranDate" runat="server" Text='<%#Eval("trandate")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Transaction Type">
                                    <ItemTemplate>
                                        <asp:Label ID="lbltrantype" runat="server" Text='<%#Eval("trantype")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="SMS">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAmount" runat="server" Text='<%#Eval("Amount")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>


                            </Columns>


                        </asp:GridView>
                    </div>
                </div>
                <div class="modal-footer">
                    <button id="btnCancel1" runat="server" class="btn btn-primary">Close</button>
                </div>
            </div>
        </div>
    </asp:Panel>

    <%--pnlPopUp_Detail Modal Popup Extender For pnlPopUp_Detail--%>
    <cc1:ModalPopupExtender ID="pnlPopUp_Detail_ModalPopupExtender1" runat="server" PopupControlID="pnlPopUp_Detail1"
        TargetControlID="lnkDetail1" BehaviorID="mpeAddUpdateEmployee1" CancelControlID="btnCancel1"
        BackgroundCssClass="modalBackground">
    </cc1:ModalPopupExtender>
    <!-- Modal Popup for Detail -->


    <!-- Bootstrap core JavaScript-->
    <script src="vendor/jquery/jquery-3.5.1.min.js"></script>
    <script src="vendor/bootstrap/js/bootstrap.bundle.min.js"></script>

    <!-- Core plugin JavaScript-->
    <script src="vendor/jquery-easing/jquery.easing.min.js"></script>

    <!--  Date-->
    <script src="vendor/datepicker/moment.min.js"></script>
    <script src="vendor/datepicker/daterangepicker.min.js"></script>

    <!-- Custom scripts for all pages-->
    <script src="js/sb-admin-2.min.js"></script>

    <!-- Page level plugins -->
    <!-- <script src="vendor/chart.js/Chart.min.js"></script>  -->

    <!-- Page level custom scripts -->
    <!-- <script src="js/demo/chart-pie-demo.js"></script> 
                                            <script src="js/demo/chart-bar-demo.js"></script>  -->

    <!-- Page level plugins -->
    <script src="vendor/datatables/jquery.dataTables.min.js"></script>
    <script src="vendor/datatables/dataTables.bootstrap4.min.js"></script>
    <script src="vendor/datatables/dataTables.responsive.min.js"></script>
    <script src="vendor/datatables/responsive.bootstrap4.min.js"></script>

    <!--  Select-->
    <script src="vendor/select/bootstrap-select.min.js"></script>

    <!-- Page level custom scripts -->
    <script src="js/demo/datatables-demo.js"></script>
    <script src="js/demo/date-range-picker-demo.js"></script>

    <script type="text/javascript">  
        $(document).ready(function () {
            t();
        });


        function b() {
            //if ($.fn.datatable.isdatatable('#rpttable')) {
            //    $('#rpttable').datatable().destroy();
            //}
            //$('#rpttable tbody').empty();
            t();
        }

        function t() {
            var startDate = $('#reportrange').data('daterangepicker').startDate.format('YYYY-MM-DD');
            var endDate = $('#reportrange').data('daterangepicker').endDate.format('YYYY-MM-DD');
            //alert(startDate);
            //alert(endDate);




        }
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
            var fromDate = $("#<%= txtFrm.ClientID %>").datepicker("getDate");
            var toDate = $("#<%= txtTo.ClientID %>").datepicker("getDate");
            // Check if From Date is greater than To Date
            if (fromDate > toDate) {
                alert("To Date cannot be lesser than From Date !");
                document.getElementById("ContentPlaceHolder1_txtTo").value = '';
                document.getElementById("ContentPlaceHolder1_txtTo").value = '';
                return false;
            }
            return true;
        }

    </script>
</asp:Content>
