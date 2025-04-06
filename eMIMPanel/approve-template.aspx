<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="approve-template.aspx.cs" Inherits="eMIMPanel.approve_template" %>

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
                    <li class="breadcrumb-item active" aria-current="page">Approve Template</li>
                </ol>
            </nav>

            <div class="row">
                <div class="col">
                    <div class="card card-body bg-primary border-light shadow-soft mb-4">
                        <!--  -->
                        <div class="row">
                            <div class="col-12 col-md-3">
                                <asp:TextBox ID="txtFrm"  runat="server"  onchange="javascript:text_changed_from();" class="form-control datepicker" placeholder="From Date" autocomplete="off"></asp:TextBox>
                                <asp:HiddenField ID="hdntxtFrm" runat="server" />
                            </div>
                            <div class="col-12 col-md-3">
                               <asp:TextBox ID="txtTo" runat="server"  onchange="javascript:text_changed_to();" class="form-control datepicker my-3 my-lg-0" placeholder="To Date" autocomplete="off"></asp:TextBox>
                                <asp:HiddenField ID="hdntxtTo" runat="server" />
                            </div>
                            <div class="col-12 col-md-3">
                                 <asp:LinkButton runat="server" ID="btnShow" OnClick="btnUpdate_Click" class="btn btn-block">
                                    Show <i class="fas fa-eye" aria-hidden="true"></i>
                                </asp:LinkButton>
                            </div>

                            <asp:HiddenField ID="h1" runat="server" />
                            <asp:HiddenField ID="h2" runat="server" />
                        </div>
                        <!--  -->
                    </div>
                </div>
            </div>
            <!-- Content Row -->
            <div class="row">
                <div class="col-xl-12 col-lg-12">
                    <!-- Start Card-->
                    <div class="card bg-primary border-light shadow-soft mb-4">
                        <div class="card-header py-3 bg-primary d-flex justify-content-between flex-wrap align-content-center border-bottom">
                            <h6 class="m-0 font-weight-bold font-weight-bold my-auto"><i class="far fa-id-card"></i> Approve Template</h6>
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
                                            <asp:LinkButton ID="btnView1" runat="server" class="mx-1 btn btn-primary text-secondary" OnClick="btnView_Click"
                                                data-toggle="tooltip" data-placement="top" title="" data-original-title="View"> <span class="text-secondary"> 
                                                    <i class="fas fa-eye"></i> </span></asp:LinkButton>

                                            <asp:LinkButton ID="LinkButton1" runat="server" class="mx-1 btn btn-primary text-success"
                                                OnClientClick="return confirm('Do you want to Approve this Template ?');" OnClick="btnApprove_Click"
                                                data-toggle="tooltip" data-placement="top" title="" data-original-title="Approved"> 
                                                <span class="text-success"> <i class="fas fa-thumbs-up"></i> </span></asp:LinkButton>

                                            <asp:LinkButton ID="LinkButton2" runat="server" class="mx-1 btn btn-primary text-danger"
                                                OnClientClick="return confirm('Do you want to Reject this Template ?');" OnClick="btnReject_Click"
                                                data-toggle="tooltip" data-placement="top" title="" data-original-title="Declined"> 
                                                    <span class="text-danger"> <i class="fas fa-thumbs-down"></i> </span></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Company Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl1" runat="server" Text='<%#Eval("compname")%>'></asp:Label>
                                            <asp:HiddenField ID="hdnUserId" runat="server" Value='<%#Eval("username")%>' />
                                            <asp:HiddenField ID="hdnfilepath" runat="server" Value='<%#Eval("filepath")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Full Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl2" runat="server" Text='<%#Eval("fullname")%>'></asp:Label>
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
                                    <asp:TemplateField HeaderText="Template ID">
                                        <ItemTemplate>
                                            <asp:Label ID="lbltemplateID" runat="server" Text='<%#Eval("templateid")%>'></asp:Label>
                                             <asp:HiddenField ID="hdnSenderId" runat="server" Value='<%#Eval("SenderId")%>' />
                                             <asp:HiddenField ID="hdnTemplateName" runat="server" Value='<%#Eval("TempName")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Template">
                                        <ItemTemplate>
                                            <p class="show-read-more m-0">
                                                <asp:Label ID="lbltemplate" runat="server" Text='<%#Eval("template")%>'></asp:Label>
                                            </p>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SenderId">
                                        <ItemTemplate>
                                            
                                                <asp:Label ID="lblsenderid" runat="server" Text='<%#Eval("senderId")%>'></asp:Label>
                                            
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                </Columns>
                            </asp:GridView>
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
