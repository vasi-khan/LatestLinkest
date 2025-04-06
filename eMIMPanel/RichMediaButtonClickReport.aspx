<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="RichMediaButtonClickReport.aspx.cs"
    Inherits="eMIMPanel.RichMediaButtonClickReport" %>

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
                    <li class="breadcrumb-item"><a href="#">Linkext</a></li>
                    <li class="breadcrumb-item active" aria-current="page">Rich Media Click Report</li>
                </ol>
            </nav>
            <div class="row">
                <div class="col-12">
                    <div class="card card-body mb-4 bg-primary border-light shadow-soft">
                        <div class="row">
                            <div class="col-md-2">
                                <asp:Label runat="server"> Rich Media Creation Date</asp:Label>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtFrm" runat="server" onchange="javascript:text_changed_from();"
                                    class="form-control datepicker" placeholder="From Date" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                                <asp:HiddenField ID="hdntxtFrm" runat="server" ClientIDMode="Static" />
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtTo" runat="server" onchange="javascript:text_changed_to();" class="form-control datepicker"
                                    placeholder="To Date" autocomplete="off" ClientIDMode="Static"></asp:TextBox>
                                <asp:HiddenField ID="hdntxtTo" runat="server" ClientIDMode="Static" />
                            </div>
                            <div class="col-md-1">
                                <asp:Button runat="server" OnClientClick="return CheckDates();" Text="GO" class="btn btn-success" ID="btnGO" OnClick="btnGO_Click" />
                            </div>
                            <div class="col-md-3">
                                <asp:DropDownList ID="ddlShortUrl" runat="server" ClientIDMode="Static" class="custom-select">
                                    <asp:ListItem Text="--ALL--" Value="-1"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:LinkButton runat="server" OnClientClick="return CheckDates1();" ID="btnUpdate" OnClick="btnUpdate_Click" class="btn btn-block">
                                            Show <i class="fas fa-eye" aria-hidden="true"></i>
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>
                    <!-- Card End -->

                    <div class="accordion shadow-soft rounded" id="accordionExample">
                        <div class="card card-sm card-body bg-primary border-light mb-0">
                            <div class="flex-fill m-0 font-weight-bold">
                                <a href="#collapseOne" id="headingOne" data-target="#collapseOne" class="accordion-panel-header" data-toggle="collapse" role="button" aria-expanded="true" aria-controls="collapseOne">
                                    <span class="icon-title h6 mb-0 font-weight-bold"><span class="fas fa-chart-line"></span>Rich Media Click</span>
                                </a>
                            </div>
                            <div class="flex-fill text-right">
                                <asp:LinkButton runat="server" ID="lnkDownload" class="btn btn-mini" OnClick="lnkDownload_Click">
                                    Download <i class="fas fa-download" aria-hidden="true"></i>
                                </asp:LinkButton>
                            </div>
                            <div id="collapseOne" class="collapse show" aria-labelledby="headingOne" data-parent="#accordionExample" runat="server">
                                <div class="card-body">
                                    <div class="table-responsive">
                                        <div id="divResult" runat="server">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="accordion shadow-soft rounded" id="accordionExample1" style="display:none;">
                        <div class="card card-sm card-body bg-primary border-light mb-0">
                            <div class="flex-fill m-0 font-weight-bold">
                                <a href="#collapseOne" id="headingOne1" data-target="#collapseOne" class="accordion-panel-header" data-toggle="collapse" role="button" aria-expanded="true" aria-controls="collapseOne">
                                    <span class="icon-title h6 mb-0 font-weight-bold"><span class="fas fa-chart-line"></span>Rich Media Click Detail</span>
                                </a>
                            </div>
                            <div class="flex-fill text-right">
                                <asp:LinkButton runat="server" ID="lnkbtnDownloadDetails" class="btn btn-mini" OnClick="lnkbtnDownloadDetails_Click">
                                    Download <i class="fas fa-download" aria-hidden="true"></i>
                                </asp:LinkButton>
                            </div>
                            <div id="Div1" class="collapse show" aria-labelledby="headingOne1" data-parent="#accordionExample1" runat="server">
                                <div class="card-body">
                                    <div class="table-responsive">
                                        <div id="divResult2" runat="server">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </main>
    <script type="text/javascript"> 
        function text_changed_from() {
            var d = document.getElementById("txtFrm").value
            console.log(d);
            document.getElementById("hdntxtFrm").value = d;
        }
        function text_changed_to() {
            var d = document.getElementById("txtTo").value
            console.log(d);
            document.getElementById("hdntxtTo").value = d;
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
    <script>
        function CheckDates() {
            var fromDate = $("#<%= txtFrm.ClientID %>").datepicker("getDate");
            var toDate = $("#<%= txtTo.ClientID %>").datepicker("getDate");

            // Check if either From Date or To Date is empty or null
            if ((fromDate == null || toDate == null) || (fromDate === "" || toDate === "")) {
                alert("From date and To Date cannot be empty");
                return false;
            }

            // Check if From Date is greater than To Date
            if (fromDate > toDate) {
                alert("From Date cannot be greater than To Date");
                return false;
            }
            return true;
        }
    </script>
    <script type="text/javascript">
        function ShowDetails(ButtonName, UId) {
            $.ajax({
                type: 'POST',
                url: '<%=ResolveUrl("RichMediaButtonClickReport.aspx/ShowDetails") %>',
                data: JSON.stringify({ ButtonName: ButtonName, UId: UId }),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (response) {
                    var d = response.d;
                    console.log(d);
                    var divResult = document.getElementById("ContentPlaceHolder1_divResult2");
                    divResult.innerHTML = "";
                    if (d && d.trim() !== '') {
                        // Bind HTML table to divResult2
                        if (divResult) {
                            divResult.innerHTML = d;
                        } else {
                            console.error('Element with id="divResult2" not found.');
                        }
                    } else {
                        console.error('Response data (d) is empty or not valid.');
                    }
                    $("#accordionExample1").removeAttr("style");
                },
                error: function (xhr, status, error) {
                    $("#accordionExample1").attr("style", "display:none");
                }
            });
        }
    </script>
</asp:Content>
