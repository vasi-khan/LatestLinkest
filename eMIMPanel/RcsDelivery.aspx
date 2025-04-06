<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="RcsDelivery.aspx.cs" Inherits="eMIMPanel.RcsDelivery" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
                    <li class="breadcrumb-item active" aria-current="page">Download Delivery Report</li>
                </ol>
            </nav>
            <!-- Content Row -->
            <div class="row" style="display: none;">
                <div class="col-12">
                    <div class="card bg-primary border-light shadow-soft mb-4">
                        <div class="card-header py-3 bg-primary">
                            <h6 class="m-0 font-weight-bold"><i class="fas fa-info-circle"></i>Info</h6>
                        </div>
                        <div class="card-body py-2">
                            <div class="row">
                                <!-- Card-->
                                <div class="col-12 col-md-6 col-sm-6 col-lg-6 col-xl-4 mb-2 count-view">
                                    <a href="sms-reports.html">
                                        <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                            <div class="card-body">
                                                <div class="row no-gutters align-items-center">
                                                    <div class="col mr-2">
                                                        <div class="text-xs font-weight-bold text-warning text-uppercase mb-1">Submitted</div>
                                                        <div class="h5 mb-0 font-weight-bold text-gray-800">
                                                            <asp:Label ID="lblTodaySubmitted" runat="server"></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="col-auto">
                                                        <div class="icon icon-shape shadow-soft rounded-circle">
                                                            <span class="fas fa-check-circle fa-2x text-warning"></span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </a>
                                </div>
                                <!-- End Card -->
                                <!-- Card-->
                                <div class="col-12 col-md-6 col-sm-6 col-lg-6 col-xl-4 mb-2 count-view">
                                    <a href="sms-reports.html">
                                        <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                            <div class="card-body">
                                                <div class="row no-gutters align-items-center">
                                                    <div class="col mr-2">
                                                        <div class="text-xs font-weight-bold text-success text-uppercase mb-1">Delivered</div>
                                                        <div class="row no-gutters align-items-center">
                                                            <div class="col-auto">
                                                                <div class="h5 mb-0 mr-3 font-weight-bold text-gray-900">
                                                                    <asp:Label ID="lblTodayDelivered" runat="server"></asp:Label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-auto">
                                                        <div class="icon icon-shape shadow-soft rounded-circle">
                                                            <span class="fas fa-check-double fa-2x text-success"></span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </a>
                                </div>
                                <!-- End Card -->

                                <!-- Card-->
                                <div class="col-12 col-md-6 col-sm-6 col-lg-6 col-xl-4 mb-4 count-view">
                                    <a href="sms-reports.html">
                                        <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                            <div class="card-body">
                                                <div class="row no-gutters align-items-center">
                                                    <div class="col mr-2">
                                                        <div class="text-xs font-weight-bold text-danger text-uppercase mb-1">Failed</div>
                                                        <div class="h5 mb-0 font-weight-bold text-danger-800">
                                                            <asp:Label ID="lblTodayFailed" runat="server"></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="col-auto">
                                                        <div class="icon icon-shape shadow-soft rounded-circle">
                                                            <span class="fas fa-thumbs-down fa-2x text-danger"></span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </a>
                                </div>
                                <!-- End Card -->
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-12">
                    <div class="card card-body mb-4 bg-primary border-light shadow-soft">

                        <div class="form-row">
                            <div class="form-group col-lg-4 col-xl-4">
                                <label for="#">Download Delivery Report</label>
                                <div class="row">
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtFrm1" runat="server" onchange="javascript:text_changed_from1();" class="form-control datepicker" placeholder="From Date" autocomplete="off"></asp:TextBox>
                                        <asp:HiddenField ID="hdntxtFrm1" runat="server" />
                                    </div>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtTo1" runat="server" onchange="javascript:text_changed_to1();" class="form-control datepicker" placeholder="To Date" autocomplete="off"></asp:TextBox>
                                        <asp:HiddenField ID="hdntxtTo1" runat="server" />
                                    </div>
                                </div>
                            </div>

                            <div class="form-group col-lg-3 col-xl-3 d-none">
                                SenderID wise
                                <div class="input-group">
                                    <div class="input-group-prepend">
                                        <div class="input-group-text">
                                            <asp:RadioButton ID="rdbSender" GroupName="rdb" Checked="true" runat="server" aria-label="Radiobutton for following text input" />
                                        </div>
                                    </div>
                                    <asp:DropDownList ID="ddlSender" runat="server" class="custom-select">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group col-lg-3 col-xl-3">
                                Campaign wise
                                <div class="input-group">
                                    <div class="input-group-prepend">
                                        <div class="input-group-text">
                                            <asp:RadioButton ID="rdbCamp" GroupName="rdb" runat="server" aria-label="Radiobutton for following text input" />
                                        </div>
                                    </div>
                                    <asp:DropDownList ID="ddlCamp" runat="server" class="custom-select"></asp:DropDownList>
                                </div>
                            </div>

                            <div class="form-group col-lg-3 col-xl-3" style="display: none;">
                                <label for="#">Search mobile number :</label>
                                <asp:TextBox ID="txtmob" runat="server" class="form-control" placeholder="XX-XXX-XXXX" MaxLength="10"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterMode="ValidChars"
                                    TargetControlID="txtmob" ValidChars="0123456789">
                                </cc1:FilteredTextBoxExtender>
                            </div>
                            <div class="form-group col-lg-2  col-xl-2 mt-auto">
                                <asp:LinkButton ID="btnsearch" runat="server" OnClick="btnsearch_Click" class="btn btn-primary text-success btn-block"><i class="fas fa-search fa-sm text-success"></i>Search</asp:LinkButton>
                            </div>
                            
                        </div>

                    </div>
                    <div class="card card-body mb-4 bg-primary border-light shadow-soft">
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
                            <asp:GridView ID="grv2" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false" ShowFooter="true"
                                runat="server" Width="100%" CellPadding="10" BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive nowrap dataTable-view">
                                <Columns>
                                    <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr No">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" OnClick="btnDownLoad_Click" class="btn btn-datatable btn-icon btn-transparent-dark px-2 py-0" data-toggle="tooltip" data-placement="top" title="download"><i class="fas fa-download fa-sm text-success"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Submit Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblsubmitdt" runat="server" Text='<%#Eval("submitdate")%>'></asp:Label>
                                            <%--<asp:HiddenField ID="hdnfrom" runat="server" Value='<%#Eval("fileid")%>'></asp:HiddenField>--%>
                                            <asp:HiddenField ID="hdnto" runat="server" Value='<%#Eval("username")%>'></asp:HiddenField>

                                            <asp:HiddenField ID="hdnFrmDW" runat="server" Value='<%#Eval("fromdate")%>'></asp:HiddenField>
                                            <asp:HiddenField ID="hdnToDW" runat="server" Value='<%#Eval("todate")%>'></asp:HiddenField>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                 <%--   <asp:TemplateField HeaderText="Sender ID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblsender" runat="server" Text='<%#Eval("sender")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Submitted">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl3" runat="server" Text='<%#Eval("submitted")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delivered">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl5" runat="server" Text='<%#Eval("delivered")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Failed">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl6" runat="server" Text='<%#Eval("failed")%>'></asp:Label>
                                        </ItemTemplate>                                        
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Rejected">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl4" runat="server" Text='<%#Eval("rejected")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Unknown">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl7" runat="server" Text='<%#Eval("unknown")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Seen Count">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl8" runat="server" Text='<%#Eval("seen")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                  <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                            </asp:GridView>

                        </div>
                    </div>
                </div>
            </div>

            <div class="row justify-content-center d-none" >
                <div class="col-12">
                    <!--Accordion-->
                    <div class="accordion shadow-soft rounded mb-4">
                        <div class="card card-sm card-body bg-primary border-light mb-0">
                            <a href="#panel-4" data-target="#panel-4" class="accordion-panel-header" data-toggle="collapse" role="button" aria-expanded="false" aria-controls="panel-1">
                                <span class="icon-title h6 mb-0 font-weight-bold"><span class="fas fa-chevron-down"></span>Todays sent count info</span>
                                <span class="icon"><span class="fas fa-plus"></span></span>
                            </a>
                            <div class="collapse" id="panel-4">
                                <div class="pt-3">
                                    <!-- Card-1 -->
                                    <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                        <div class="card-body py-2">
                                            <p><strong>Delivery Status :</strong></p>
                                            <ul class="mb-0">
                                                <li>Delivered : Delivered to Handset </li>
                                                <li>Undelivered : Not Delivered </li>
                                                <li>Unknown : SMS may be delivered but report not received from telecom operator(Du/Etisalat) </li>
                                            </ul>
                                        </div>
                                    </div>
                                    <!-- Card-2 -->
                                    <div class="card mb-0 border-left-danger">
                                        <div class="card-body py-2">
                                            <p><span class="font-weight-bold text-danger">Note :</span> We show exactly those reports which are being provided by different operators. At times the SMS gets delivered but the report shows unknown. These are those SMS whose delivery reports are not provided by the operator but they may have been actually delivered.</p>
                                            <ul class="mb-0">
                                                <li>System will send DELIVERED/UNDELIVERED/UNKNOWN status only. Failure details is not supported as per system design. Single customer complain should be raised by customer to support. </li>
                                                <li>NO DLR received: Confirm that full packet is sent or reaching server or not. </li>
                                                <li>Confirm Validity period is 12 hours. </li>
                                                <li>Verify schedule delivery time and other configuration. </li>
                                                <li>In case successful DLR but no content showing in handset: It’s the handset which sent ACK to the network regarding SMS receipt. </li>
                                                <li>Ensure to remove inactive/invalid MSISDNs from recipients list. </li>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!--End of Accordion-->
                </div>
            </div>

            <!-- Content Row -->
            <div class="row d-none">
                <div class="col-xl-12 col-lg-12">
                    <!-- Basic Card Example -->
                    <div class="card bg-primary border-light shadow-soft mb-4">
                        <div class="card-header py-3 bg-primary d-flex justify-content-between flex-wrap align-content-center">
                            <h6 class="m-0 font-weight-bold my-auto">SMS Reports</h6>
                            <div class="right-view">
                                <asp:LinkButton runat="server" ID="LinkButton1" OnClick="btnUpdate_Click" class="btn btn-mini"> Show <i class="fas fa-eye" aria-hidden="true"></i></asp:LinkButton>
                                <asp:HiddenField ID="h1" runat="server" />
                                <asp:HiddenField ID="h2" runat="server" />
                            </div>
                        </div>
                        <div class="accordion" id="accordionExample">
                            <div class="card">
                                <div class="card-header" id="headingOne">
                                    <h2 class="mb-0">
                                        <button class="btn btn-link btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseOne" aria-expanded="true" aria-controls="collapseOne">
                                            SMS Summary
                                        </button>
                                    </h2>
                                </div>

                                <div id="collapseOne" class="collapse show" aria-labelledby="headingOne" data-parent="#accordionExample" runat="server">
                                    <div class="card-body">
                                        <div class="row">
                                            <!-- Area Chart -->
                                            <div class="col-xl-12 col-lg-12">
                                                <!-- Loader -->
                                                <div class="o-page-loader">
                                                    <div class="o-page-loader--content">
                                                        <div class="o-page-loader--spinner"></div>
                                                        <div class="o-page-loader--message">
                                                            <span>Loading...</span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <!--End Loader -->
                                                <div class="table-responsive">

                                                    <asp:GridView UseAccessibleHeader="true" ID="grv" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                                                        runat="server" Width="100%" CellPadding="10" BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive nowrap dataTable-view">
                                                        <Columns>
                                                            <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr No">
                                                                <ItemTemplate>
                                                                    <%#Container.DataItemIndex+1 %>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Action">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="LinkButton1" runat="server" OnClick="btnXL_Click" class="btn btn-datatable btn-icon btn-transparent-dark px-2 py-0" data-toggle="tooltip" data-placement="top" title="XLS"><i class="far fa-file-excel"></i></asp:LinkButton>
                                                                    <asp:LinkButton ID="LinkButton2" OnClientClick=<%# "view('" + Eval("sender") + "','" + Eval("fileid") + "'); return false;" %> runat="server" class="btn btn-datatable btn-icon btn-transparent-dark px-2 py-0" data-toggle="tooltip" data-placement="top" title="Details"><i class="far fa-file-alt"></i></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Submit Date Time" HeaderStyle-CssClass="text-wrap">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblmobile" runat="server" CssClass="text-wrap" Text='<%#Eval("submitdate")%>'></asp:Label>
                                                                    <asp:HiddenField ID="hdnFileId" runat="server" Value='<%#Eval("fileid")%>'></asp:HiddenField>
                                                                    <asp:HiddenField ID="hdnUserId" runat="server" Value='<%#Eval("userid")%>'></asp:HiddenField>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Source">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl0" runat="server" Text='<%#Eval("reqsrc")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Sender ID">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblsender" runat="server" Text='<%#Eval("sender")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Submitted">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl3" runat="server" Text='<%#Eval("submitted")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Delivered">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl4" runat="server" Text='<%#Eval("delivered")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Failed">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl5" runat="server" Text='<%#Eval("failed")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Unknown">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl6" runat="server" Text='<%#Eval("unknown")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="File Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl1" runat="server" Text='<%#Eval("filenm")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Message">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl7" runat="server" Text='<%#Eval("msg")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="card">
                                <div class="card-header" id="headingTwo">
                                    <h2 class="mb-0">
                                        <button class="btn btn-link btn-block text-left collapsed" type="button" data-toggle="collapse" data-target="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo">
                                            SMS Detail
                                        </button>
                                    </h2>
                                </div>
                                <div id="collapseTwo" class="collapse" aria-labelledby="headingTwo" data-parent="#accordionExample" runat="server">
                                    <div class="card-body">
                                        <div style="text-align: right;">
                                            <asp:LinkButton runat="server" ID="LinkButton3" OnClick="btnCloseDetail_Click" class="btn btn-mini">Close Detail</asp:LinkButton>
                                        </div>
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
                                            <table class="table table-striped table-bordered dt-responsive nowrap" id="rpttable" width="100%" cellspacing="0">
                                                <thead>
                                                    <tr>
                                                        <th>Sr. No</th>
                                                        <th>MSG ID</th>
                                                        <th>Mobile No</th>
                                                        <th>Sender Id</th>
                                                        <th>Submit Date Time</th>
                                                        <th>Status Date Time</th>
                                                        <th>Message</th>
                                                        <th>Status</th>
                                                        <th>Server Response</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <div class="card-body">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </main>
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
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

            function EndRequestHandler(sender, args) {
                $('.datepicker').datepicker({ dateFormat: 'dd-mm-yy' });
            }

        });
    </script>
    <script type="text/javascript">  
        function view(senderid, fileid) {
            console.log(senderid);
            console.log(fileid);


            var today = new Date();
            var dd = String(today.getDate()).padStart(2, '0');
            var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
            var yyyy = today.getFullYear();

            var user = '<% =Session["UserID"].ToString() %>';
            var startDate = yyyy + '-' + mm + '-' + dd;
            var endDate = startDate + ' 23:59:59';

            console.log(startDate);
            console.log(endDate);
            console.log(user);

            $.ajax({
                type: "POST",
                dataType: "json",
                url: "WebService.asmx/GetSMSReportDetailUserNew",
                data: { dater: startDate + '$' + endDate + '$' + user + '$' + fileid + '$' + senderid },
                success: function (data) {
                    var datatableVariable = $('#rpttable').DataTable({
                        data: data,
                        columns: [
                            { 'data': 'sln' },
                            { 'data': 'msgid' },
                            { 'data': 'mobile' },
                            { 'data': 'senderid' },
                            { 'data': 'senttime' },
                            { 'data': 'dlrtime' },
                            { 'data': 'msgtext' },
                            { 'data': 'dlrstat' },
                            { 'data': 'dlrresp' }
                        ]
                    });
                }
            });
            console.log("done");
            document.getElementById('ContentPlaceHolder1_collapseOne').className = 'collapse';
            document.getElementById('ContentPlaceHolder1_collapseTwo').className = 'collapse show';
            console.log("done222");
        }

    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

            function EndRequestHandler(sender, args) {
                $('.datepicker').datepicker({ dateFormat: 'dd-mm-yy' });
            }

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
        function text_changed_from1() {
            var d = document.getElementById("ContentPlaceHolder1_txtFrm1").value
            console.log(d);
            document.getElementById("ContentPlaceHolder1_hdntxtFrm1").value = d;
        }
        function text_changed_to1() {
            var d = document.getElementById("ContentPlaceHolder1_txtTo1").value
            console.log(d);
            document.getElementById("ContentPlaceHolder1_hdntxtTo1").value = d;
        }

    </script>

    <script>
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
    </script>
</asp:Content>

