<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="account-list0.aspx.cs" Inherits="eMIMPanel.account_list0" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>MIM Account List</title>
 
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main>
        <div class="container-fluid">
            <nav aria-label="breadcrumb" class="my-3">
                <ol class="breadcrumb breadcrumb-info">
                    <li class="breadcrumb-item"><a href="#">Home</a></li>
                    <li class="breadcrumb-item active" aria-current="page">Account List</li>
                </ol>
            </nav>

            <!-- Content Row -->
            <div class="row">

                <!-- Area Chart -->
                <div class="col-xl-12 col-lg-12">
                    <!-- Basic Card Example -->
                    <div class="card bg-primary border-light shadow-soft mb-4">
                        <div class="card-header py-3 bg-primary d-flex justify-content-between flex-wrap align-content-center">
                            <h6 class="m-0 font-weight-bold my-auto">Account List</h6>
                            <div class="dateRangeView">
                                <a class="btn btn-primary text-dark btn-block" id="reportrange" role="button" aria-pressed="true">
                                    <i class="fas fa-calendar mr-2 text-dark"></i>
                                    <span class="text-dark"></span><i class="ml-1 fas fa-chevron-down" data-feather="chevron-down"></i>
                                </a>
                            </div>
                        </div>
                        <div class="card-body">
                            <div class="table-responsive">
                                <table class="table table-striped table-bordered dt-responsive nowrap" id="CustomerTable" width="100%" cellspacing="0">
                                    <thead>
                                        <tr>
                                            <th>Sr. No</th>
                                            <th>Company Name</th>
                                            <th>Sender Id</th>
                                            <th>Mobile No</th>
                                            <th>Email Id</th>
                                            <th>Balance</th>
                                            <th>Created By</th>
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
            
            if ($.fn.DataTable.isDataTable('#CustomerTable')) {
                $('#CustomerTable').DataTable().destroy();
            }
            $('#CustomerTable tbody').empty();

         $.ajax({  
             type: "POST",  
             dataType: "json",  
             url: "WebService.asmx/GetCustomers",  
             success: function (data) {  
                 var datatableVariable = $('#CustomerTable').DataTable({  
                     data: data,  
                     columns: [  
                         
                         { 'data': 'CompName' },  
                         { 'data': 'SenderID' },  
                         { 'data': 'Mobile1' },  
                         { 'data': 'Email' }, 
                         { 'data': 'Balance' },  
                         { 'data': 'CreatedBy' },  
                         { 'data': 'Actions' }]  
                 });  
                  
             }  
         });  
  
     });  
 </script>  

</asp:Content>

