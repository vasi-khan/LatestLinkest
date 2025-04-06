<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="sms-reports-detail_usr.aspx.cs" Inherits="eMIMPanel.click_reports_detail_usr" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <cc1:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </cc1:ToolkitScriptManager>
    <main>
        <asp:UpdatePanel ID="updFormArea" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="container-fluid">
                    <nav aria-label="breadcrumb" class="my-3">
                        <ol class="breadcrumb breadcrumb-info">
                            <li class="breadcrumb-item"><a href="#">Home</a></li>
                            <li class="breadcrumb-item"><a href="#">Linkext</a></li>
                            <li class="breadcrumb-item active" aria-current="page">SMS Report Details</li>
                        </ol>
                    </nav>
                    <div class="row">
                        <div class="col-12">
                            

                            <div class="card card-body mb-4 bg-primary border-light shadow-soft">
                              <asp:PlaceHolder ID = "PlaceHolder1" runat="server" />
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
            </ContentTemplate>
        </asp:UpdatePanel>
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
        $(document).ready(function() {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

            function EndRequestHandler(sender, args) {
                $('.datepicker').datepicker({ dateFormat: 'dd-mm-yy' });
            }

        });
    </script> 

    <script type="text/javascript">  
        $(document).ready(function () {

            var user = '<% =Session["rptUSERID"].ToString() %>';
            var fileid = '<% =Session["rptFILEID"].ToString() %>';
            var senderid = '<% =Session["rptSENDER"].ToString() %>';
            var startDate = '<% =Session["rptS1"].ToString() %>';
            var endDate = '<% =Session["rptS2"].ToString() %>';

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
        });
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
