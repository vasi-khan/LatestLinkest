<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="sms-reports.aspx.cs" Inherits="eMIMPanel.sms_reports" %>

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
                    <li class="breadcrumb-item active" aria-current="page">SMS Reports</li>
                </ol>
            </nav>

            <!--  -->
            <div class="card card-body bg-primary border-light shadow-soft mb-4">
                <div class="col-lg-6 col-xl-4 mb-2 mb-lg-0 mt-3">
                    <div class="form-group">
                        <asp:LinkButton ID="btnsearch" runat="server" class="btn btn-primary text-success btn-block" OnClick="btnsearch_Click"><i class="fas fa-download fa-sm text-success"></i> Download Delivery Report</asp:LinkButton>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-12">
                    <div class="card card-body bg-primary border-light shadow-soft mb-4">
                        <div class="row">
                            <div class="col-md-3">
                                <asp:TextBox ID="txtFrm" runat="server" onchange="javascript:text_changed_from(); " class="form-control datepicker" placeholder="From Date" autocomplete="off"></asp:TextBox>
                                <asp:HiddenField ID="hdntxtFrm" runat="server" />
                            </div>
                            <div class="col-md-3">
                                <asp:TextBox ID="txtTo" runat="server" onchange="javascript:text_changed_to();" class="form-control datepicker my-3 my-lg-0" placeholder="To Date" autocomplete="off"></asp:TextBox>
                                <asp:HiddenField ID="hdntxtTo" runat="server" />
                            </div>
                            <div class="col-md-3">
                                <asp:TextBox ID="txtmobile" runat="server" class="form-control my-3 my-lg-0" MaxLength="15" onkeypress="return ValidateNumericInput(event.key)" placeholder="Mobile No"></asp:TextBox>
                            </div>

                            <div class="col-md-3">
                                <asp:LinkButton runat="server" ID="LinkButton2" OnClick="btnUpdate_Click" class="btn btn-block">
                                        Show <i class="fas fa-eye" aria-hidden="true"></i>
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
                    <!-- Start Card -->
                    <div class="card bg-primary border-light shadow-soft mb-4">
                        <div class="card-header py-3 bg-primary d-flex justify-content-between flex-wrap align-content-center border-bottom">
                            <h6 class="m-0 font-weight-bold my-auto">SMS Reports</h6>
                        </div>
                        <div class="card-body">
                            <div class="table-responsive">
                                <asp:GridView ID="grv" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false" ShowFooter="true"
                                    runat="server" Width="100%" CellPadding="10" BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive nowrap dataTable-view">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr No">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="LinkButton1" runat="server" class="mx-1 btn btn-primary text-success"
                                                    OnClick="btnView_Click" data-toggle="tooltip" data-placement="top" title=""
                                                    data-original-title="View"> 
                                                <span class="text-success"> <i class="fas fa-file-csv"></i> </span></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="User ID">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUserId" runat="server" Text='<%#Eval("username")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sender ID">
                                            <ItemTemplate>
                                                <asp:Label ID="lblsender" runat="server" Text='<%#Eval("senderid")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Submitted">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl1" runat="server" Text='<%#Eval("submitted")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Delivered">
                                            <ItemTemplate>
                                                <asp:Label ID="lblname" runat="server" Text='<%#Eval("delivered")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Failed">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl3" runat="server" Text='<%#Eval("failed")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Unknown">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl5" runat="server" Text='<%#Eval("unknown")%>'></asp:Label>
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
            <!--  -->
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
            alert(startDate);
            //alert(endDate);


            $.ajax({
                type: "POST",
                dataType: "json",
                url: "WebService.asmx/GetSMSReport",
                data: { dater: startDate + '$' + endDate },
                success: function (data) {
                    var datatableVariable = $('#rpttable').DataTable({
                        data: data,
                        columns: [
                            { 'data': 'sln' },
                            { 'data': 'UserName' },
                            { 'data': 'SenderID' },
                            { 'data': 'Submitted' },
                            { 'data': 'Delivered' },
                            { 'data': 'Failed' },
                            { 'data': 'Unknown' }
                        ]
                    });

                }
            });

        }
    </script>

    <script type="text/javascript"> 
        function ValidateNumericInput(Input) {
            var regex = /^[0-9]+$/
            return regex.test(Input);
        }

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
