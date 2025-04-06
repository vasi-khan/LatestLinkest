<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="account-list1.aspx.cs" Inherits="eMIMPanel.account_list1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>MIM Account List</title>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main>
        <div class="container-fluid">
            <nav aria-label="breadcrumb" class="my-3">
                <ol class="breadcrumb breadcrumb-info">
                    <li class="breadcrumb-item"><a href="#">Home</a></li>
                    <li class="breadcrumb-item active" aria-current="page">Account List</li>
                </ol>
            </nav>
            <form>
                <!-- Content Row -->
                <div class="row">

                    <!-- Area Chart -->
                    <div class="col-xl-12 col-lg-12">
                        <!-- Basic Card Example -->
                        <div class="card bg-primary border-light shadow-soft mb-6">
                            <div class="card-header py-3 bg-primary d-flex justify-content-between flex-wrap align-content-center">
                                <h6 class="m-0 font-weight-bold my-auto">Account List</h6>
                                <div class="dateRangeView">
                                    <a class="btn btn-primary text-dark btn-block" id="reportrange" role="button" aria-pressed="true">
                                        <i class="fas fa-calendar mr-2 text-dark"></i>
                                        <span class="text-dark"></span><i class="ml-1 fas fa-chevron-down" data-feather="chevron-down"></i>
                                    </a>
                                    <a href="#" class="btn text-success mr-3">
                                        <span class="text-success">
                                            <i class="fas fa-step-forward"></i>
                                        </span>
                                        <asp:Button ID="btnShow" runat="server" Text="Show" OnClientClick="getdt();" OnClick="btnUpdate_Click" />
                                        
                                    </a>
                                    <asp:HiddenField ID="h1" runat="server" />
                                    <asp:HiddenField ID="h2" runat="server" />

                                </div>

                                <%--<asp:TextBox id="reportrange" runat="server" CssClass="DatepickerInput" Text=" " style="width:100%"></asp:TextBox>--%>
                            </div>
                            <div class="card-body">
                                <asp:GridView ID="grv" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                                    runat="server" Width="100%" CellPadding="10"  OnPageIndexChanging="grv_PageIndexChanging"
                                    BorderColor="#ede8e8"  Class="table table-striped table-bordered dt-responsive nowrap dataTable-view"> 
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr No">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Company Name" HeaderStyle-Width="20%">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl1" runat="server" Text='<%#Eval("compname")%>' Style="word-break:break-all;"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sender ID">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl2" Text='<%#Eval("sender")%>' runat="server"></asp:Label>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Mobile No">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl3" runat="server" Text='<%#Eval("mobile")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Email ID" HeaderStyle-Width="20%">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl4" runat="server" Text='<%#Eval("Email")%>' Style="word-break:break-all;"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Balance">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl5" runat="server" Text='<%#Eval("bal")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Created By">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl6" runat="server" Text='<%#Eval("createdby")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="Action">
                                                <ItemTemplate>
                                                    
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                    </Columns>
                                    <%--<EditRowStyle BackColor="#7C6F57" />
                                    <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#dbd7d7" Font-Bold="True" ForeColor="Black" Height="50px" />
                                    <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                    <RowStyle />
                                    <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                    <SortedAscendingCellStyle BackColor="#F8FAFA" />
                                    <SortedAscendingHeaderStyle BackColor="#246B61" />
                                    <SortedDescendingCellStyle BackColor="#D4DFE1" />
                                    <SortedDescendingHeaderStyle BackColor="#15524A" />--%>
                                    <EmptyDataTemplate>
                                        <table style="width: 100%;">
                                            <tr>
                                                <td align="center">
                                                    <asp:Label ID="lblEmpty" Text="No Data Found!!!" Style="color: Red;" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                </asp:GridView>

                            </div>
                        </div>
                    </div>
                </div>
            </form>
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

        function getdt() {
            var s1 = $('#reportrange').data('daterangepicker').startDate.format('YYYY-MM-DD');
            var s2 = $('#reportrange').data('daterangepicker').endDate.format('YYYY-MM-DD');

            document.getElementById('<%= h1.ClientID %>').value = s1;
            document.getElementById('<%= h2.ClientID %>').value = s2;
        }

        $('#reportrange').daterangepicker();
        $('#reportrange').on('apply.daterangepicker', function (ev, picker) {
            console.log(picker.startDate.format('YYYY-MM-DD'));
            console.log(picker.endDate.format('YYYY-MM-DD'));
        });
    </script>

</asp:Content>

