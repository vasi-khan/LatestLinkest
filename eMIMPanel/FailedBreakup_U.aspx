<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" EnableEventValidation="false" AutoEventWireup="true"
    CodeBehind="FailedBreakup_U.aspx.cs" Inherits="eMIMPanel.FailedBreakup_U" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </asp:ToolkitScriptManager>
    <main>
        <div class="container-fluid">
            <nav aria-label="breadcrumb" class="my-3">
                <ol class="breadcrumb breadcrumb-info">
                    <li class="breadcrumb-item"><a href="#">Home</a></li>
                    <li class="breadcrumb-item"><a href="#">Reports</a></li>
                    <li class="breadcrumb-item active" aria-current="page">SMS Failure Breakup</li>
                </ol>
            </nav>
            <div class="row">
                <div class="col-xl-12 col-lg-12">
                    <div class="card card-body bg-primary border-light shadow-soft mb-4">
                        <div class="row align-items-center">
                            <div class="col-md-2">
                                <asp:TextBox ID="txtFrm" runat="server" onchange="javascript:text_changed_from();" class="form-control datepicker" placeholder="From Date" autocomplete="off"></asp:TextBox>
                                <asp:HiddenField ID="hdntxtFrm" runat="server" />
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtTo" runat="server" onchange="javascript:text_changed_to();" class="form-control datepicker mt-3 my-lg-0" placeholder="To Date" autocomplete="off"></asp:TextBox>
                                <asp:HiddenField ID="hdntxtTo" runat="server" />
                            </div>
                            <div class="col-md-3">
                                <asp:DropDownList ID="ddlCamp" runat="server" class="custom-select my-3 my-lg-0">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:LinkButton runat="server" OnClientClick="return CheckDates();" ID="btnShow" OnClick="btnUpdate_Click" class="btn btn-block mt-3 my-lg-0">
                                            Show <i class="fas fa-eye" aria-hidden="true"></i>
                                </asp:LinkButton>
                            </div>
                            <div class="col-md-2">
                                <asp:LinkButton ID="lnkDownload" runat="server" OnClick="lnkDownload_Click" class="btn btn-block mt-3 my-lg-0">
                                            Download <i class="fas fa-download" aria-hidden="true"></i>
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <!-- Content Row -->
            <div class="row">
                <div class="col-xl-12 col-lg-12">
                    <!-- Start Card-->
                    <div class="card bg-primary border-light shadow-soft mb-4">
                        <div class="card-header py-3 bg-primary d-flex justify-content-between flex-wrap align-content-center">
                            <h6 class="m-0 font-weight-bold my-auto"><i class="fas fa-ban pr-2"></i>SMS Failure Breakup</h6>
                        </div>
                        <div class="card-body">
                            <div class="table-responsive">
                                <asp:GridView ID="grv" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                                    runat="server" Width="100%" CellPadding="10" BorderColor="#ede8e8"
                                    Class="table table-striped table-bordered dt-responsive nowrap dataTable-view">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr No">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                                <asp:HiddenField ID="hdnFrom" Value='<%#Eval("From")%>' runat="server" />
                                                <asp:HiddenField ID="hdnTo" Value='<%#Eval("To")%>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Error Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblErrorCode" runat="server" Text='<%#Eval("ErrorCode")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Error Description">
                                            <ItemTemplate>
                                                <asp:Label ID="lblErrorDescription" runat="server" Text='<%#Eval("ErrorDescription")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Mobile No Count">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMobileNoCount" runat="server" Text='<%#Eval("MobileNoCount")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="TOTAL">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTOTAL" runat="server" Text='<%#Eval("TOTAL")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                    <!-- End Card -->
                </div>
            </div>
            <!-- End Row -->
        </div>
    </main>
    <script>
        $(function () {
            var threeYearsAgo = new Date();
            threeYearsAgo.setFullYear(threeYearsAgo.getFullYear() - 3);

            $(".datepicker").datepicker({
                endDate: new Date(),
                todayHighlight: true,
                autoclose: true,
                startDate: threeYearsAgo,
                format: 'yyyy-mm-dd'
            }).datepicker("setDate", "0");
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

            if ((fromDate == null || toDate == null) || (fromDate === "" || toDate === "")) {
                alert("From date and To Date cannot be empty");
                return false;
            }

            if (fromDate > toDate) {
                alert("From Date cannot be greater than To Date");
                return false;
            }
            return true;
        }
    </script>
</asp:Content>
