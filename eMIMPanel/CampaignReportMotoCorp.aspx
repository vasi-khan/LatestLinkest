<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="CampaignReportMotoCorp.aspx.cs" Inherits="eMIMPanel.CampaignReportMotoCorp" %>

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
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <cc1:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </cc1:ToolkitScriptManager>
    <link rel="stylesheet" href="https://unpkg.com/bootstrap-table@1.15.4/dist/bootstrap-table.min.css" />
    <link rel="stylesheet" href="https://unpkg.com/bootstrap-table@1.15.4/dist/bootstrap-table.min.js" />
    <link rel="stylesheet" href="https://unpkg.com/bootstrap-table@1.15.4/dist/bootstrap-table-locale-all.min.js" />
    <style>
        .table thead th {
            vertical-align: middle;
            border-bottom: 0.125rem solid #6e6e6f;
            text-align: center;
        }

        .table th, .table td {
            padding: 8px;
            font-size: 12px;
            border: 1px solid #9a9a9a;
            text-align: center;
        }

        #ContentPlaceHolder1_divResult thead {
            background: #d2d2d2;
        }

        #ContentPlaceHolder1_divResult tbody tr {
            background: #f1f1f1;
        }

            #ContentPlaceHolder1_divResult tbody tr:nth-child(2n) {
                background: #ffffff;
            }

            #ContentPlaceHolder1_divResult tbody tr.collapse.show table tbody tr:nth-child(odd) {
                background: #f1f1f1;
            }

            #ContentPlaceHolder1_divResult tbody tr.collapse.show table tbody tr:nth-child(even) {
                background: #e8e8e8;
            }
    </style>
    <main>
        <div class="container-fluid">
            <nav aria-label="breadcrumb" class="my-3">
                <ol class="breadcrumb breadcrumb-info">
                    <li class="breadcrumb-item"><a href="#">Home</a></li>
                    <li class="breadcrumb-item"><a href="#">Reports</a></li>
                    <li class="breadcrumb-item active" aria-current="page">Campaign Report</li>
                </ol>
            </nav>
            <div class="row">
                <div class="col-12">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <div class="card card-body mb-4 bg-primary border-light shadow-soft">
                                <div class="right-view">
                                    <div class="row">
                                        <div class="col-md-3">
                                            <div class="flex-fill font-weight-bold mr-lg-3">From Date</div>
                                            <asp:TextBox ID="txtFrm" runat="server" onchange="javascript:text_changed_from();" class="form-control datepicker" placeholder="From Date" autocomplete="off"></asp:TextBox>
                                            <asp:HiddenField ID="hdntxtFrm" runat="server" />
                                        </div>
                                        <div class="col-md-3">
                                            <div class="flex-fill font-weight-bold mr-lg-3">To Date</div>
                                            <asp:TextBox ID="txtTo" runat="server" onchange="javascript:text_changed_to();" class="form-control datepicker" placeholder="To Date" autocomplete="off"></asp:TextBox>
                                            <asp:HiddenField ID="hdntxtTo" runat="server" />
                                        </div>
                                        <div class="col-md-3">
                                            <div class="flex-fill font-weight-bold mr-lg-3">Campaign</div>
                                            <asp:DropDownList ID="ddlCamp" runat="server" class="custom-select" ClientIDMode="Static">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="flex-fill font-weight-bold mr-lg-3">Events</div>
                                            <asp:DropDownList ID="ddlEvents" runat="server" class="custom-select my-3 my-lg-0" ClientIDMode="Static">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-3">
                                            <div class="flex-fill font-weight-bold mr-lg-3">Group Location</div>
                                            <div class="flex-fill">
                                                <asp:DropDownList ID="ddlCategory" runat="server" class="custom-select my-3 my-lg-0" AutoPostBack="true" ClientIDMode="Static" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="flex-fill font-weight-bold mr-lg-3">Location</div>
                                            <asp:DropDownList ID="ddlLocation" runat="server" class="custom-select" AutoPostBack="true" ClientIDMode="Static" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="flex-fill font-weight-bold mr-lg-3">SubLocation</div>
                                            <asp:DropDownList ID="ddlSubLocation" runat="server" class="custom-select" AutoPostBack="true" ClientIDMode="Static" OnSelectedIndexChanged="ddlSubLocation_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="flex-fill font-weight-bold mr-lg-3">Dealer</div>
                                            <asp:DropDownList ID="ddlDealerCode" runat="server" class="custom-select my-3 my-lg-0" ClientIDMode="Static">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="right-view" style="text-align: right;">
                                        <div class="flex-fill font-weight-bold mr-lg-3">
                                            <br />
                                        </div>
                                        <asp:LinkButton runat="server" ID="lnkShow" OnClick="lnkShow_Click" class="btn btn-block" Width="10%">
                                                    Show <i class="fas fa-eye" aria-hidden="true"></i>
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                            <!-- Card End -->
                            <!--  -->
                            <div class="card mb-4 bg-primary border-light shadow-soft">
                                <div class="card-header pt-3 pb-0 bg-primary d-flex justify-content-between flex-wrap align-items-center">
                                    <div class="flex-fill">
                                        <h6 class="font-weight-bold mb-0"><i class="fas fa-chart-line"></i>Campaign Report</h6>
                                    </div>
                                    <div class="downB">
                                        <asp:LinkButton runat="server" ID="lnkDownload" OnClick="lnkDownload_Click" class="btn btn-block">
                                                    Download <i class="fas fa-download" aria-hidden="true"></i>
                                        </asp:LinkButton>
                                    </div>
                                </div>

                                <div class="card-body">
                                    <div class="table-responsive">
                                        <div id="divResult" runat="server">
                                            <!-- <asp:Label ID="lblResult" Style="overflow-y: scroll" runat="server"></asp:Label> -->
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <!--  -->
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="lnkDownload" />
                        </Triggers>
                    </asp:UpdatePanel>
                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="0">
                        <ProgressTemplate>
                            <div align="center" style="position: absolute; left: 40%; top: 50%; vertical-align: middle;">
                                <img src="Img/LOADING.GIF" />
                            </div>
                            <div align="center" style="position: absolute; left: 0%; top: 0%; vertical-align: middle;">
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </div>
            </div>
        </div>
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
        <script type="text/javascript">
            $(function () {
                // popover
                $('.notePopover').popover();
                var $table = $('#table');
                $(".collapse td.inside-table").siblings().remove();
                $("table tr [data-toggle=collapse]").on('click', function (e) {
                    var rowIndex = ($(this).closest('td').parent()[0].sectionRowIndex);
                    if ($($(this).closest('td').parent()[0]).hasClass('active')) {
                        $('table tr').removeClass('active');
                        $('.collapse').removeClass('show');
                    } else {
                        $($(this).closest('td').parent()[0]).addClass('active');
                    }
                    console.log(rowIndex);
                });
            });
        </script>
    </main>
    <script type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
            function EndRequestHandler(sender, args) {
                $('.datepicker').datepicker({ dateFormat: 'dd-mm-yy' });
            }
        });
        function loadscrq() {
            $('.datepicker').datepicker({ dateFormat: 'dd-mm-yy' });
        }
    </script>
    <script type="text/javascript">
        function ResendSmsProcess(fileid) {
            window.location.href = "send-sms-u-B4Send.aspx?Id=" + fileid;
        }
    </script>
    <script type="text/javascript">
        function ResendSmsDuplicateProcess(fileid) {
            window.location.href = "send-sms-u-B4Send.aspx?FId=" + fileid;
        }

    </script>
    <!-- Bootstrap core JavaScript-->
    <script src="vendor/jquery/jquery-3.5.1.min.js"></script>
    <!-- Select2 CSS -->
    <link href="css/select2.min.css" rel="stylesheet" />
    <!-- Select2 -->
    <script src="js/select2.min.js"></script>
    <script>
        $("#ddlCamp").select2({
            allowClear: true
        });

        $("#ddlEvents").select2({
            allowClear: true
        });

        $("#ddlCategory").select2({
            allowClear: true
        });

        $("#ddlLocation").select2({
            allowClear: true
        });

        $("#ddlSubLocation").select2({
            allowClear: true
        });

        $("#ddlDealerCode").select2({
            allowClear: true
        });
    </script>
</asp:Content>
