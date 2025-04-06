<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="credit-debit-logs_U.aspx.cs" Inherits="eMIMPanel.credit_debit_logs_U" MaintainScrollPositionOnPostback="true" %>

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

            <!-- Content Row -->
            <div class="row">

                <!-- Area Chart -->
                <div class="col-xl-12 col-lg-12">
                    <!-- Basic Card Example -->
                    <div class="card bg-primary border-light shadow-soft mb-4">
                        <div class="card-header py-3 bg-primary d-flex justify-content-between flex-wrap align-content-center">
                            <div class="right-view" style="width:100%">
                                <div class="row">
                                    <div class="col-12">
                                        <div class="card card-body mb-4 bg-primary border-light shadow-soft">
                                            <fieldset class="form-group mb-0">
                                                <div class="form-row">
                                                    <div class="col-sm-2">
                                                        <a href="#" class="btn text-dark btn-icon-split mr-3 btn-block text-left">
                                                            <span class="text-dark">
                                                                <asp:CheckBox ID="chkSMS" runat="server" aria-label="Checkbox for following text input" />
                                                            </span>
                                                            <span class="text font-weight-bold">SMS</span>
                                                            <i class="fas fa-comment-alt"></i>
                                                        </a>
                                                    </div>
                                                    <div class="col-sm-2">
                                                        <a href="#" class="btn text-dark btn-icon-split mr-3 btn-block text-left">
                                                            <span class="text-dark">
                                                                <asp:CheckBox ID="chkURL" runat="server" aria-label="Checkbox for following text input" />
                                                            </span>
                                                            <span class="text font-weight-bold">URL</span>
                                                            <i class="fas fa-link"></i>
                                                        </a>
                                                    </div>
                                                    <div class="col-sm-2">
                                                        <a href="#" class="btn text-dark btn-icon-split mr-3 btn-block text-left">
                                                            <span class="text-dark">
                                                               <asp:CheckBox ID="chkClick" runat="server" aria-label="Checkbox for following text input" />
                                                            </span>
                                                            <span class="text font-weight-bold">Clicks</span>
                                                            <i class="fas fa-mouse-pointer"></i>
                                                        </a>
                                                        <asp:CheckBox ID="chkAll" runat="server" visible="false" />
                                                    </div>
                                                    <%--<div class="col-sm-1">
                                                        <a href="#" class="btn text-dark btn-icon-split mr-3 btn-block text-left">
                                                            <span class="text-dark">
                                                                <asp:CheckBox ID="chkAll" runat="server" aria-label="Checkbox for following text input" />
                                                            </span>
                                                            <span class="text font-weight-bold">All</span>
                                                            <i class="fas fa-compass"></i>
                                                        </a>
                                                    </div>--%>
                                                </div>
                                            </fieldset>
                                            <br />
                                            <fieldset class="form-group mb-0">
                                                <div class="form-row">
                                                    <div class="col-2">
                                                        <asp:TextBox ID="txtFrm" runat="server" onchange="javascript:text_changed_from();" class="form-control datepicker" placeholder="From Date" autocomplete="off"></asp:TextBox>
                                                        <asp:HiddenField ID="hdntxtFrm" runat="server" />
                                                    </div>
                                                    <div class="col-2">
                                                        <asp:TextBox ID="txtTo" runat="server" onchange="javascript:text_changed_to();" class="form-control datepicker" placeholder="To Date" autocomplete="off"></asp:TextBox>
                                                        <asp:HiddenField ID="hdntxtTo" runat="server" />
                                                    </div>
                                                    <div class="col-sm-3">
                                                        <a href="#" class="btn text-dark btn-icon-split mr-3 btn-block text-left">
                                                            <span class="text-dark">
                                                                <asp:CheckBox ID="chkIntoCreditDebit" runat="server" aria-label="Checkbox for following text input" />
                                                            </span>
                                                            <span>Included Auto Credit Debit</span>
                                                        </a>
                                                    </div>
                                                     <div class="col-sm-4">
                                                        <asp:LinkButton runat="server" ID="LinkButton2" OnClick="btnUpdate_Click" class="btn btn-mini">
                                                        Show <i class="fas fa-eye" aria-hidden="true"></i>
                                                        </asp:LinkButton>
                                                    </div>
                                                </div>
                                            </fieldset>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="card-body">
                            <div class="table-responsive">
                                <asp:GridView ID="grv" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                                    runat="server" Width="100%" CellPadding="10" OnRowDataBound="grv_RowDataBound" BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive nowrap dataTable-view">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr No">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="Action">
                                            <ItemTemplate>
                                                 <asp:LinkButton ID="btnXL" runat="server" OnClick="btnXL_Click" class="btn btn-datatable btn-icon btn-transparent-dark px-2 py-0" data-toggle="tooltip" data-placement="top" title="Details"><i class="far fa-info-circle"></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="From Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFrmDt" runat="server" Text='<%#Eval("fromdate")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="To Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblToDt" runat="server" Text='<%#Eval("todate")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Product">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProduct" runat="server" Text='<%#Eval("product")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="Credit">
                                            <ItemTemplate>
                                                <asp:Label ID="lblcr" runat="server" class="text-success font-weight-bold" Text='<%#Eval("cr")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Debit">
                                            <ItemTemplate>
                                                <asp:Label ID="lbldr" runat="server" class="text-danger font-weight-bold" Text='<%#Eval("dr")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                       <%-- <asp:TemplateField HeaderText="Remarks">
                                            <ItemTemplate>
                                                <asp:Label ID="lblRemark" runat="server" Text='<%#Eval("remarks")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                    </Columns>
                                </asp:GridView>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
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
                                        <asp:Label ID="lblTranDate" runat="server" Text='<%#Eval("tdate")%>'></asp:Label>
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
                                        <asp:Label ID="lblRemarks" runat="server" Text='<%#Eval("REMARKS")%>'></asp:Label>
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
            document.getElementById("ContentPlaceHolder1_hdntxtFrm").value = d;
        }
        function text_changed_to() {
            var d = document.getElementById("ContentPlaceHolder1_txtTo").value
            document.getElementById("ContentPlaceHolder1_hdntxtTo").value = d;
        }
    </script>
</asp:Content>
