<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="HeroMonthlyReportMotoCorp.aspx.cs" Inherits="eMIMPanel.HeroMonthlyReportMotoCorp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .select2-container--default .select2-selection--single {
            background-color: #e6e7ee !important;
            border-radius: 4px;
            border: 1px solid #aaa !important;
            box-shadow: inset 2px 2px 5px #b8b9be, inset -3px -3px 7px #ffffff !important;
        }

        .select2-search--dropdown .select2-search__field {
            padding: 4px;
            width: 100%;
            box-sizing: border-box;
            background: #e6e7ee !important;
            border: 1px solid red;
        }
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
    <script>
        function Confirm() {
            if (confirm(' Are you sure to download.')) {
                return true;
            }
            else {
                return false;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <cc1:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </cc1:ToolkitScriptManager>
    <main>
        <nav aria-label="breadcrumb" class="my-3">
            <ol class="breadcrumb breadcrumb-info">
                <li class="breadcrumb-item"><a href="#">Home</a></li>
                <li class="breadcrumb-item"><a href="#">Reports</a></li>
                <li class="breadcrumb-item active" aria-current="page">Monthly Reports</li>
            </ol>
        </nav>

        <!-- Start Row -->
        <div class="row">
            <div class="col-xl-12 col-lg-12">
                <!-- Basic Card Example -->
                <div class="card bg-primary border-light shadow-soft mb-4">
                    <div class="card-header py-3 bg-primary d-flex flex-column flex-lg-row justify-content-lg-between flex-wrap align-content-lg-center">
                        <h6 class="m-0 font-weight-bold my-auto">Monthly Reports</h6>
                    </div>
                    <div class="row">
                        <div class="col-md-3">
                            <div class="flex-fill font-weight-bold mr-lg-3">Year</div>
                            <div class="flex-fill">
                                <asp:DropDownList ID="ddlYear" runat="server" class="custom-select" ClientIDMode="Static">
                                    <asp:ListItem Value="0">--Select Year--</asp:ListItem>
                                    <asp:ListItem Value="2021">2021</asp:ListItem>
                                    <asp:ListItem Value="2022">2022</asp:ListItem>
                                    <asp:ListItem Value="2023">2023</asp:ListItem>
                                    <asp:ListItem Value="2024">2024</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="flex-fill font-weight-bold mr-lg-3">Month</div>
                            <div class="flex-fill">
                                <asp:DropDownList ID="ddlMonth" runat="server" class="custom-select" ClientIDMode="Static">
                                    <asp:ListItem Value="0">--Select Month--</asp:ListItem>
                                    <asp:ListItem Value="1">January</asp:ListItem>
                                    <asp:ListItem Value="2">February</asp:ListItem>
                                    <asp:ListItem Value="3">March</asp:ListItem>
                                    <asp:ListItem Value="4">April</asp:ListItem>
                                    <asp:ListItem Value="5">May</asp:ListItem>
                                    <asp:ListItem Value="6">June</asp:ListItem>
                                    <asp:ListItem Value="7">July</asp:ListItem>
                                    <asp:ListItem Value="8">August</asp:ListItem>
                                    <asp:ListItem Value="9">September</asp:ListItem>
                                    <asp:ListItem Value="10">October</asp:ListItem>
                                    <asp:ListItem Value="11">November</asp:ListItem>
                                    <asp:ListItem Value="12">December</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-3">
                            <div class="flex-fill font-weight-bold mr-lg-3">Group Location</div>
                            <div class="flex-fill">
                                <asp:DropDownList ID="ddlCategory" runat="server" class="custom-select" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="flex-fill font-weight-bold mr-lg-3">Location</div>
                            <div class="flex-fill">
                                <asp:DropDownList ID="ddlLocation" runat="server" class="custom-select" AutoPostBack="true" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged">
                                    <asp:ListItem Value="0">--All--</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="flex-fill font-weight-bold mr-lg-3">SubLocation</div>
                            <div class="flex-fill">
                                <asp:DropDownList ID="ddlSubLocation" runat="server" class="custom-select" AutoPostBack="true" OnSelectedIndexChanged="ddlSubLocation_SelectedIndexChanged">
                                    <asp:ListItem Value="0">--All--</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="flex-fill font-weight-bold mr-lg-3">Dealer</div>
                            <div class="flex-fill">
                                <asp:DropDownList ID="ddlDealerCode" runat="server" class="custom-select" ClientIDMode="Static">
                                    <asp:ListItem Value="0">--All--</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="flex-fill mr-lg-3">
                                <br />
                            </div>
                            <div class="right-view" style="text-align: right;">
                                <asp:LinkButton runat="server" ID="lnkDownload" OnClick="btnUpdate_Click" class="btn btn-block" Width="15%"> Download <i class="fa-fax fa-download" aria-hidden="true"></i></asp:LinkButton>
                            </div>
                        </div>
                    </div>
                    <br />
                </div>
            </div>
        </div>
        <!-- End Row -->
    </main>

    <!-- Select2 CSS -->
    <link href="css/select2.min.css" rel="stylesheet" />
    <!-- Select2 -->
    <script src="js/select2.min.js"></script>
    <script>
        $("#ddlTempIdAndName").select2({
            allowClear: true
        });

        $("#ddlTempIdAndName").select2({
            allowClear: true
        });

        $("#ddlCamp").select2({
            allowClear: true
        });

        $("#ContentPlaceHolder1_ddlCategory").select2({
            allowClear: true
        });

        $("#ContentPlaceHolder1_ddlLocation").select2({
            allowClear: true
        });

        $("#ContentPlaceHolder1_ddlSubLocation").select2({
            allowClear: true
        });

        $("#ddlDealerCode").select2({
            allowClear: true
        });
    </script>

    <!-- Bootstrap core JavaScript-->
    <script src="vendor/jquery/jquery-3.5.1.min.js"></script>

    <script>
        $('#rpttable').dataTable({
            destroy: true,
            aaData: response.data
        });
    </script>

    <script type="text/javascript">  
        function view(senderid, fileid, reqsrc) {
            console.log(senderid);
            console.log(fileid);
            $('#rpttable').dataTable().fnDestroy();   //Change on 05 Aug 2022

            var today = new Date();
            var dd = String(today.getDate()).padStart(2, '0');
            var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
            var yyyy = today.getFullYear();

            var user = '<% =Session["UserID"].ToString() %>';
            var startDate = yyyy + '-' + mm + '-' + dd;
            var endDate = startDate + ' 23:59:59';
            var mob = $('#ContentPlaceHolder1_txtMobileNo').val();
            console.log(startDate);
            console.log(endDate);
            console.log(user);
            window.open("../ViewDetail.aspx?A=" + startDate + '$' + endDate + '$' + user + '$' + fileid + '$' + senderid + '$' + reqsrc + '$' + mob, '_blank');
            console.log("done");
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
            document.getElementById("ContentPlaceHolder1_hdntxtFrm").value = d;
        }
        function text_changed_to() {
            var d = document.getElementById("ContentPlaceHolder1_txtTo").value
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
