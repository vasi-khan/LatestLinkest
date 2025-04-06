<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ScheduleLogs_u_Admin.aspx.cs" Inherits="eMIMPanel.ScheduleLogs_u_Admin" %>

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
                    <li class="breadcrumb-item active" aria-current="page">Schedule Logs</li>
                </ol>
            </nav>
            <!-- Content Row -->
            <div class="row">
                <div class="col-12">
                    <div class="card card-body mb-4 bg-primary border-light shadow-soft">
                        <form>
                            <div class="row">

                                <div class="col-md-3">
                                    <asp:DropDownList ID="ddlUserId" runat="server" CssClass="custom-select"></asp:DropDownList>
                                </div>

                                <div class="col-md-3">
                                    <asp:TextBox ID="txtFrm" runat="server" onchange="javascript:text_changed_from();" class="form-control schedule-datepicker" placeholder="From Date" autocomplete="off"></asp:TextBox>
                                    <asp:HiddenField ID="hdntxtFrm" runat="server" />
                                    <%--<cc1:CalendarExtender ID="txtFrm_Ext" runat="server" Format="dd/MMM/yyyy" TargetControlID="txtFrm">
                                                </cc1:CalendarExtender>--%>
                                </div>
                                <div class="col-md-3">
                                    <asp:TextBox ID="txtTo" runat="server" onchange="javascript:text_changed_to();" class="form-control schedule-datepicker my-2 my-lg-0" placeholder="To Date" autocomplete="off"></asp:TextBox>
                                    <asp:HiddenField ID="hdntxtTo" runat="server" />
                                    <%-- <cc1:CalendarExtender ID="txtTo_Ext" runat="server" Format="dd/MMM/yyyy" TargetControlID="txtTo">
                                                </cc1:CalendarExtender>--%>
                                </div>

                                <div class="col-md-3">
                                    <asp:LinkButton runat="server" ID="btnShow" OnClick="btnUpdate_Click" class="btn btn-block">
                                                    Show <i class="fas fa-eye" aria-hidden="true"></i>
                                    </asp:LinkButton>
                                </div>

                                <asp:HiddenField ID="h1" runat="server" />
                                <asp:HiddenField ID="h2" runat="server" />
                            </div>
                        </form>
                    </div>
                </div>
            </div>

            <!-- Content Row -->
            <div class="row">
                <div class="col-xl-12 col-lg-12">
                    <!-- Basic Card Example -->
                    <div class="card bg-primary border-light shadow-soft mb-4">
                        <div class="card-header py-3 bg-primary d-flex justify-content-between flex-wrap align-content-center">
                            <h6 class="font-weight-bold my-lg-auto mb-3"><i class="far fa-clock"></i>Schedule Logs</h6>
                            <%--<div class="refresh-btn">
                                <a href="#" class="mx-2 btn btn-primary text-secondary" data-toggle="tooltip" data-placement="top" title="Last Refresh : 12:05"><i class="fas fa-sync"></i></a>
                            </div>--%>
                        </div>
                        <div class="card-body">
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
                                <asp:GridView ID="grv" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                                    runat="server" Width="100%" CellPadding="10"
                                    BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive nowrap dataTable-view">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr No">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                                <asp:HiddenField ID="hdnFileID" runat="server" Value='<%#Eval("FILEID")%>' />
                                                <asp:Label ID="lblEncryUsername" runat="server" Text='<%#Eval("EncryUserName")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="UserName">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUsername" runat="server" Text='<%#Eval("profileid")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Download">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnView" runat="server" OnClick="btnDw_Click" class="btn btn-primary text-secondary" data-toggle="tooltip" data-placement="top" title="download"><i class="fas fa-download"></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Delete">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnDel" runat="server" OnClientClick="return confirm('Do you want to Delete this Schedule ?');" OnClick="btnDel_Click" class="btn btn-primary text-danger" data-toggle="tooltip" data-placement="top" title="Delete"><i class="fas fa-trash-alt"></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Scheduled Time">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSch" runat="server" Text='<%#Eval("Schedule")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Number of Mobiles">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMobile" runat="server" Text='<%#Eval("mobiles")%>'></asp:Label>
                                                <asp:HiddenField ID="hdnSMSRate" runat="server" Value='<%#Eval("smsrate")%>' />
                                                <asp:HiddenField ID="hdnNoOfSMS" runat="server" Value='<%#Eval("noofsms")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="File Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFileName" runat="server" Text='<%#Eval("FileName")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Campaign Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblcampname" runat="server" Text='<%#Eval("campname")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Message Text">
                                            <ItemTemplate>
                                                <asp:Label ID="lblmsg" runat="server" Text='<%#Eval("msg")%>'></asp:Label>
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


    <script>
        $(function () {
            var sixmonthsago = new Date();
            sixmonthsago.setMonth(sixmonthsago.getMonth() - 6);
             var onemonthlater = new Date();
            onemonthlater.setDate(onemonthlater.getDate() +30);


            $(".schedule-datepicker").datepicker({
                endDate: onemonthlater,
                todayHighlight: true,
                autoclose: true,
                startDate: sixmonthsago,
                format: 'yyyy-mm-dd'
            });    // Set the current date
        });
    </script>
</asp:Content>
