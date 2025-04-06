<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="click-reports.aspx.cs" Inherits="eMIMPanel.click_reports" %>
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
                    <li class="breadcrumb-item active" aria-current="page">Click Reports</li>
                </ol>
            </nav>

            <!-- Content Row -->
            <div class="row">

                <!-- Area Chart -->
                <div class="col-xl-12 col-lg-12">
                    <!-- Basic Card Example -->
                    <div class="card bg-primary border-light shadow-soft mb-4">
                        <div class="card-header py-3 bg-primary d-flex justify-content-between flex-wrap align-content-center">
                            <h6 class="m-0 font-weight-bold my-auto">Click Reports</h6>
                            <div class="dateRangeView">
                                <a class="btn btn-primary text-dark btn-block" id="reportrange" role="button" aria-pressed="true">
                                    <i class="fas fa-calendar mr-2 text-dark"></i>
                                    <span class="text-dark"></span><i class="ml-1 fas fa-chevron-down" data-feather="chevron-down"></i>
                                </a>
                            </div>
                        </div>
                        <div class="card-body">
                            <div class="table-responsive">
                                <table class="table table-striped table-bordered dt-responsive wrap display table" id="rpttable" cellspacing="0">
                                    <thead>
                                        <tr>
                                            <th>Sr. No</th>
                                            <th>User ID</th>
                                            <th>Short URL</th>
                                            <th>Click Count</th>
                                            <th>Action</th>
                                        </tr>
                                    </thead>
                                   
                                </table>
                            </div>
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

            var startDate = $('#reportrange').data('daterangepicker').startDate.format('YYYY-MM-DD');
            var endDate = $('#reportrange').data('daterangepicker').endDate.format('YYYY-MM-DD');
            //if ($.fn.DataTable.isDataTable('#rpttable')) {
            //    $('#rpttable').DataTable().destroy();
            //}
            //$('#rpttable tbody').empty();
            var user = '<% =Session["User"].ToString() %>';
            var usertype = '<% =Session["UserType"].ToString() %>';
            var dlt = '<% =Session["DLT"].ToString() %>';
            var empcode = '<%=Session["EMPCODE"].ToString()%>';

            $.ajax({
                type: "POST",
                dataType: "json",
                url: "WebService.asmx/GetClickReport",
                data: { dater: startDate + '$' + endDate + '$' + user + '$' + usertype + '$' + dlt +'$'+ empcode},
                success: function (data) {
                    var datatableVariable = $('#rpttable').DataTable({
                        data: data,
                        columns: [
                            { 'data': 'sln' },
                            { 'data': 'username' },
                            { 'data': 'shorturl' },
                            { 'data': 'clickcount' },
                            {
                                'data': null, 'bSortable': false, 'mRender': function (data, type, full) {
                                    return '<a href="download-list.aspx?x='+ full.username + '$' + full.shorturl + '" target="_blank" class="mx-1 btn btn-primary text-secondary"> <i class="fas fa-file-download"></i></a>';
                                }
                            }
                            ]
                    });

                }
            });

        });
    </script>

</asp:Content>
