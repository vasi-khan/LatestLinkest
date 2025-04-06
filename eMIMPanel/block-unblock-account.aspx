<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="block-unblock-account.aspx.cs" Inherits="eMIMPanel.block_unblock_account" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <%-- <form runat="server">--%>
        <cc1:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
        </cc1:ToolkitScriptManager>
        <main>
                    <div class="container-fluid">
                        <nav aria-label="breadcrumb" class="my-3">
                            <ol class="breadcrumb breadcrumb-info">
                                <li class="breadcrumb-item"><a href="#">Home</a></li>
                                <li class="breadcrumb-item active" aria-current="page">Block & Unblock Account</li>
                            </ol>
                        </nav>
                        <!-- Content Row -->
                        <div class="row">

                            <!-- Area Chart -->
                            <div class="col-xl-12 col-lg-12">
                                <!-- Basic Card Example -->
                                <div class="card bg-primary border-light shadow-soft mb-4">
                                    <div class="card-header py-3 bg-primary d-flex justify-content-between flex-wrap align-content-center">
                                        <h6 class="m-0 font-weight-bold">Block & Unblock A/C </h6>
                                        <div class="right-view">
                                            <%--<a href="#" class="mx-2 btn btn-primary text-secondary" data-toggle="tooltip" data-placement="top" title="Last Refresh : 12:05"><i class="fas fa-sync"></i></a>--%>
                                            <div class="row">

                                                <div class="col-md-4">
                                                    <asp:RadioButton ID="rdbBlocked" runat="server" class="form-control" Text="Blocked" GroupName="blk" Checked="true"></asp:RadioButton>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:RadioButton ID="rdbUnblocked" runat="server" class="form-control" Text="UnBlocked" GroupName="blk"></asp:RadioButton>
                                                </div>
                                                <%--<a class="btn btn-primary text-dark btn-block" id="reportrange" role="button" aria-pressed="true">
                                                <i class="fas fa-calendar mr-2 text-dark"></i>
                                                <span class="text-dark"></span><i class="ml-1 fas fa-chevron-down" data-feather="chevron-down"></i>
                                            </a>--%>
                                                <div class="col-md-4">
                                                     <asp:LinkButton runat="server" ID="LinkButton3" OnClick="btnUpdate_Click" class="btn btn-mini">
                                                    Show <i class="fas fa-eye" aria-hidden="true"></i>
                                                </asp:LinkButton>
                                                    <%--<a href="#" class="btn text-success mr-3">
                                                        <span class="text-success">
                                                            <i class="fas fa-sync"></i>
                                                        </span>
                                                        <asp:Button ID="btnShow" runat="server" Text="Show" OnClick="btnUpdate_Click" />

                                                    </a>--%>
                                                </div>

                                                <asp:HiddenField ID="h1" runat="server" />
                                                <asp:HiddenField ID="h2" runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="card-body">
                                        <asp:GridView ID="grv" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                                            runat="server" Width="100%" CellPadding="10" BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive nowrap dataTable-view">
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr No">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Action">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="LinkButton1"  Visible='<%# Eval("Status").ToString() == "BLOCKED" ? true : false %>'
                                                            runat="server" class="mx-1 btn btn-primary text-success"
                                                        OnClientClick="return confirm('Do you want to Unblock this User ID ?');" OnClick="btnApprove_Click"
                                                        data-toggle="tooltip" data-placement="top" title="" data-original-title="Approved"> 
                                                        <span class="text-success"> <i class="fas fa-unlock"></i> </span></asp:LinkButton>

                                                    <asp:LinkButton ID="LinkButton2" Visible='<%# Eval("Status").ToString() == "UNBLOCKED" ? true : false %>'
                                                        runat="server"  class="mx-1 btn btn-primary text-danger"
                                                        OnClientClick="return confirm('Do you want to Block this User ID ?');" OnClick="btnReject_Click"
                                                        data-toggle="tooltip" data-placement="top" title="" data-original-title="Declined"> 
                                                         <span class="text-danger"> <i class="fas fa-lock"></i> </span></asp:LinkButton>

                                                        <%--<asp:Button ID="btnApprove" Visible='<%# Eval("Status").ToString() == "BLOCKED" ? true : false %>' runat="server" 
                                                            class="mx-1 btn btn-primary text-success" Text="UnBlock" 
                                                            OnClientClick="return confirm('Do you want to Unblock this User ID ?');" OnClick="btnApprove_Click" 
                                                            data-toggle="tooltip" data-placement="top" title="" data-original-title="UbBlock" />
                                                        <asp:Button ID="btnReject" Visible='<%# Eval("Status").ToString() == "UNBLOCKED" ? true : false %>' runat="server" 
                                                            class="mx-1 btn btn-primary text-danger" Text="Block" OnClientClick="return confirm('Do you want to Block this User ID ?');" 
                                                            OnClick="btnReject_Click" data-toggle="tooltip" data-placement="top" title="" data-original-title="Block" />--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Company Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl1" runat="server" Text='<%#Eval("compname")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="User ID">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbluserid" runat="server" Text='<%#Eval("username")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Mobile No">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl3" runat="server" Text='<%#Eval("mobile")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Email ID">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl4" runat="server" Text='<%#Eval("Email")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sender ID">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblsender" runat="server" Text='<%#Eval("sender")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
        </main>
    <%--</form>--%>
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

</script>
</asp:Content>
