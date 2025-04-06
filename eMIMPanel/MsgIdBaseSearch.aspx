<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="MsgIdBaseSearch.aspx.cs" Inherits="eMIMPanel.MsgIdBaseSearch" %>

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
                    <li class="breadcrumb-item active" aria-current="page">Messages Id Base Search</li>
                </ol>
            </nav>
            <!-- Content Row -->
            <div class="row">
                <div class="col-12">
                    <div class="card card-body mb-4 bg-primary border-light shadow-soft">
                        <div class="card-body">
                            <div class="right-view">
                                <div class="row">
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtMessageId" runat="server" class="form-control text-action" placeholder="Enter Message Id"></asp:TextBox>
                                    </div>
                                    <div class="col-md-9">
                                        <asp:LinkButton runat="server" ID="aaaa" OnClick="btnUpdate_Click" class="btn btn-mini">
                                                    Show <i class="fas fa-eye" aria-hidden="true"></i>
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="card card-body mb-4 bg-primary border-light shadow-soft">
                        <div class="card-header py-3 bg-primary d-flex justify-content-between flex-wrap align-content-center">
                            <h6 class="m-0 font-weight-bold font-weight-bold my-auto">Provider Details</h6>
                        </div>
                        <div class="card-body pt-0">
                            <div class="form-group row">
                                <label for="inputEmail33" class="col-sm-2 col-form-label font-weight-bold" style="margin-top: -10px;">Provider</label>
                                <div class="col-md-5">
                                    <div class="form-label-group">
                                        <asp:Label ID="lblProvider" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="5">
                                </div>
                            </div>
                            <div class="form-group row">
                                <label for="inputEmail33" class="col-sm-2 col-form-label font-weight-bold" style="margin-top: -10px;">SMPP A/C Id</label>
                                <div class="col-md-5">
                                    <div class="form-label-group">
                                        <asp:Label ID="lblSMPPACID" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="5">
                                </div>
                            </div>
                            <div class="form-group row">
                                <label for="inputEmail33" class="col-sm-2 col-form-label font-weight-bold" style="margin-top: -10px;">SMPP A/C System Id</label>
                                <div class="col-md-5">
                                    <div class="form-label-group">
                                        <asp:Label ID="lblSMPPSystemID" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="5">
                                </div>
                            </div>
                            <div class="form-group row">
                                <label for="inputEmail33" class="col-sm-2 col-form-label font-weight-bold" style="margin-top: -10px;">Message Id</label>
                                <div class="col-md-5">
                                    <div class="form-label-group">
                                        <asp:Label ID="lblMessageID" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="5">
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="card card-body mb-4 bg-primary border-light shadow-soft">
                        <div class="card-header py-3 bg-primary d-flex justify-content-between flex-wrap align-content-center">
                            <h6 class="m-0 font-weight-bold font-weight-bold my-auto">Submission Details</h6>
                        </div>
                        <div class="card-body pt-0">
                            <div class="form-group row">
                                <label for="inputEmail33" class="col-sm-2 col-form-label font-weight-bold" style="margin-top: -10px;">Profile Id:</label>
                                <div class="col-md-5">
                                    <div class="form-label-group">
                                        <asp:Label ID="lblProfileId" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="5">
                                </div>
                            </div>
                            <div class="form-group row">
                                <label for="inputEmail33" class="col-sm-2 col-form-label font-weight-bold" style="margin-top: -10px;">Client Name:</label>
                                <div class="col-md-5">
                                    <div class="form-label-group">
                                        <asp:Label ID="lblClientName" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="5">
                                </div>
                            </div>
                            <div class="form-group row">
                                <label for="inputEmail33" class="col-sm-2 col-form-label font-weight-bold" style="margin-top: -10px;">Sender Id</label>
                                <div class="col-md-5">
                                    <div class="form-label-group">
                                        <asp:Label ID="lblSenderId" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="5">
                                </div>
                            </div>
                            <div class="form-group row">
                                <label for="inputEmail33" class="col-sm-2 col-form-label font-weight-bold" style="margin-top: -10px;">Message Text</label>
                                <div class="col-md-5">
                                    <div class="form-label-group">
                                        <asp:Label ID="lblMessText" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="5">
                                </div>
                            </div>
                            <div class="form-group row">
                                <label for="inputEmail33" class="col-sm-2 col-form-label font-weight-bold" style="margin-top: -10px;">Received Time</label>
                                <div class="col-md-5">
                                    <div class="form-label-group">
                                        <asp:Label ID="lblReceivedTime" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="5">
                                </div>
                            </div>
                            <div class="form-group row">
                                <label for="inputEmail33" class="col-sm-2 col-form-label font-weight-bold" style="margin-top: -10px;">Sent Time</label>
                                <div class="col-md-5">
                                    <div class="form-label-group">
                                        <asp:Label ID="lblSentTime" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="5">
                                </div>
                            </div>
                            <div class="form-group row">
                                <label for="inputEmail33" class="col-sm-2 col-form-label font-weight-bold" style="margin-top: -10px;">Template Id:</label>
                                <div class="col-md-5">
                                    <div class="form-label-group">
                                        <asp:Label ID="lblTemplateId" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="5">
                                </div>
                            </div>
                            <div class="form-group row">
                                <label for="inputEmail33" class="col-sm-2 col-form-label font-weight-bold" style="margin-top: -10px;">Template Text:</label>
                                <div class="col-md-5">
                                    <div class="form-label-group">
                                        <asp:Label ID="lblTemplateText" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="5">
                                </div>
                            </div>
                            <div class="form-group row">
                                <label for="inputEmail33" class="col-sm-2 col-form-label font-weight-bold" style="margin-top: -10px;">Submission Type:</label>
                                <div class="col-md-5">
                                    <div class="form-label-group">
                                        <asp:Label ID="lblSubmissionType" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="5">
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="card card-body mb-4 bg-primary border-light shadow-soft">
                        <div class="card-header py-3 bg-primary d-flex justify-content-between flex-wrap align-content-center">
                            <h6 class="m-0 font-weight-bold font-weight-bold my-auto">Delivery Details</h6>
                        </div>
                        <div class="card-body pt-0">
                            <div class="form-group row">
                                <label for="inputEmail33" class="col-sm-2 col-form-label font-weight-bold" style="margin-top: -10px;">Status:</label>
                                <div class="col-md-5">
                                    <div class="form-label-group">
                                        <asp:Label ID="lblStatus" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="5">
                                </div>
                            </div>
                            <div class="form-group row">
                                <label for="inputEmail33" class="col-sm-2 col-form-label font-weight-bold" style="margin-top: -10px;">DLR Time:</label>
                                <div class="col-md-5">
                                    <div class="form-label-group">
                                        <asp:Label ID="lblDLRTime" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="5">
                                </div>
                            </div>
                            <div class="form-group row">
                                <label for="inputEmail33" class="col-sm-2 col-form-label font-weight-bold" style="margin-top: -10px;">Error Code:</label>
                                <div class="col-md-5">
                                    <div class="form-label-group">
                                        <asp:Label ID="lblErrorCode" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="5">
                                </div>
                            </div>
                            <div class="form-group row">
                                <label for="inputEmail33" class="col-sm-2 col-form-label font-weight-bold" style="margin-top: -10px;">Error Desc:</label>
                                <div class="col-md-5">
                                    <div class="form-label-group">
                                        <asp:Label ID="lblErrorDesc" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div class="5">
                                </div>
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
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

            function EndRequestHandler(sender, args) {
                $('.datepicker').datepicker({ dateFormat: 'dd-mm-yy' });
            }

        });
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
