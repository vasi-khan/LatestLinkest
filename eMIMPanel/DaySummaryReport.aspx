<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="DaySummaryReport.aspx.cs" Inherits="eMIMPanel.DaySummaryReport" %>

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
                    <li class="breadcrumb-item active" aria-current="page">Day Summary</li>
                </ol>
            </nav>

            <!--  -->
            <%--<div class="card card-body bg-primary border-light shadow-soft mb-4">
            <div class="col-lg-6 col-xl-4 mb-2 mb-lg-0 mt-3">
                <div class="form-group">
                    <asp:LinkButton ID="btnsearch" runat="server" class="btn btn-primary text-success btn-block" OnClick="btnsearch_Click"><i class="fas fa-download fa-sm text-success"></i> Download Delivery Report</asp:LinkButton>
                </div>
            </div>
            </div>--%>
            <div class="row">
                <div class="col-12">
                    <div class="card card-body bg-primary border-light shadow-soft mb-4">
                        <div class="row">
                            <div class="col-md-3">
                                <%--<asp:TextBox ID="txtFrm" runat="server" onchange="javascript:text_changed_from();" class="form-control datepicker" placeholder="From Date" autocomplete="off"></asp:TextBox>
                                <asp:HiddenField ID="hdntxtFrm" runat="server" />--%>
                                <asp:DropDownList ID="ddlFromMonth" runat="server" class="form-control" placeholder="From Month">
                                    <asp:ListItem Selected="True" Value="0">---From Month---</asp:ListItem>
                                    <asp:ListItem Value="January">January</asp:ListItem>
                                    <asp:ListItem Value="February">February</asp:ListItem>
                                    <asp:ListItem Value="March">March</asp:ListItem>
                                    <asp:ListItem Value="April">April</asp:ListItem>
                                    <asp:ListItem Value="May">May</asp:ListItem>
                                    <asp:ListItem Value="June">June</asp:ListItem>
                                    <asp:ListItem Value="July">July</asp:ListItem>
                                    <asp:ListItem Value="August">August</asp:ListItem>
                                    <asp:ListItem Value="September">September</asp:ListItem>
                                    <asp:ListItem Value="October">October</asp:ListItem>
                                    <asp:ListItem Value="November">November</asp:ListItem>
                                    <asp:ListItem Value="December">December</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-3">
                                <%--<asp:TextBox ID="txtTo" runat="server" onchange="javascript:text_changed_to();" class="form-control datepicker my-3 my-lg-0" placeholder="To Date" autocomplete="off"></asp:TextBox>
                                <asp:HiddenField ID="hdntxtTo" runat="server" />--%>

                                <asp:DropDownList ID="ddlToMonth" runat="server" class="form-control" placeholder="To Month">
                                    <asp:ListItem Selected="True" Value="0">---To Month---</asp:ListItem>
                                    <asp:ListItem Value="January">January</asp:ListItem>
                                    <asp:ListItem Value="February">February</asp:ListItem>
                                    <asp:ListItem Value="March">March</asp:ListItem>
                                    <asp:ListItem Value="April">April</asp:ListItem>
                                    <asp:ListItem Value="May">May</asp:ListItem>
                                    <asp:ListItem Value="June">June</asp:ListItem>
                                    <asp:ListItem Value="July">July</asp:ListItem>
                                    <asp:ListItem Value="August">August</asp:ListItem>
                                    <asp:ListItem Value="September">September</asp:ListItem>
                                    <asp:ListItem Value="October">October</asp:ListItem>
                                    <asp:ListItem Value="November">November</asp:ListItem>
                                    <asp:ListItem Value="December">December</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-3">
                                <asp:DropDownList ID="ddlYear" runat="server" class="form-control" placeholder="Year">
                                </asp:DropDownList>
                                <%--<asp:TextBox ID="txtmobile" runat="server" class="form-control my-3 my-lg-0" MaxLength="15" onkeypress="return ValidateNumericInput(event.key)" placeholder="Mobile No"></asp:TextBox>--%>
                            </div>
                             <div class="col-md-1">
                                DND <asp:CheckBox ID="ChkDND" runat="server" />
                            </div>
                            <div class="col-md-2">
                                Group On DLTNo <asp:CheckBox Checked="true" ID="ChkGroupOnDlt" runat="server" />
                            </div>

                            <%-- <div class="col-md-3">
                                <asp:LinkButton runat="server" ID="LinkButton2" OnClick="btnUpdate_Click" class="btn btn-block">
                                        Show <i class="fas fa-eye" aria-hidden="true"></i>
                                </asp:LinkButton>
                            </div>
                            <asp:HiddenField ID="h1" runat="server" />
                            <asp:HiddenField ID="h2" runat="server" />--%>
                        </div>
                        <div class="row mt-2">
                            <div class="col-md-3">
                                <asp:TextBox ID="txtUserID" runat="server" class="form-control my-3 my-lg-0" MaxLength="15" placeholder="User ID"></asp:TextBox>
                            </div>
                            <div class="col-md-3">
                                <asp:TextBox ID="txtDltno" runat="server" class="form-control my-3 my-lg-0" MaxLength="100" placeholder="DLT No"></asp:TextBox>
                            </div>
                            <div class="col-md-3">
                                <asp:LinkButton runat="server" ID="LinkButton2" OnClick="btnUpdate_Click" class="btn btn-block">
                                        Show <i class="fas fa-eye" aria-hidden="true"></i>
                                </asp:LinkButton>
                            </div>
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
                            <%--<h6 class="m-0 font-weight-bold my-auto">Day Summary Reports</h6>--%>
                            <div class="flex-fill">
                                <h6 class="font-weight-bold mb-0"><i class="fas fa-chart-line"></i>Day Summary Reports</h6>
                            </div>
                            <div class="downB">
                                <asp:LinkButton runat="server" ID="lnkDownload" Visible="false" class="btn btn-block" OnClick="lnkDownload_Click">
                                                    Download <i class="fas fa-download" aria-hidden="true"></i>
                                </asp:LinkButton>
                            </div>
                        </div>
                        <div class="card-body">
                            <div class="table-responsive">
                                <asp:GridView ID="grv" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false" ShowFooter="true"
                                    runat="server" Width="100%" CellPadding="10" BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive nowrap dataTable-view">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr.No.">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="User ID">
                                            <ItemTemplate>
                                                <asp:Label ID="lblUserId" runat="server" Text='<%#Eval("ProfileID")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Company Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblsender" runat="server" Text='<%#Eval("COMPNAME")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Country Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblcc" runat="server" Text='<%#Eval("defaultCountry")%>'></asp:Label>
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
                                        <asp:TemplateField HeaderText="DND" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl5" runat="server" Text='<%#Eval("DND")%>'></asp:Label>
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
        function ValidateNumericInput(Input) {
            var regex = /^[0-9]+$/
            return regex.test(Input);
        }

    </script>
</asp:Content>
