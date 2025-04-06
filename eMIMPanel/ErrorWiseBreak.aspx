<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="ErrorWiseBreak.aspx.cs" Inherits="eMIMPanel.ErrorWiseBreak" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main class="px-3">
        <nav aria-label="breadcrumb" class="my-3">
            <ol class="breadcrumb breadcrumb-info">
                <li class="breadcrumb-item"><a href="index_u2.aspx">Home</a></li>
                <li class="breadcrumb-item active" aria-current="page">Error Wise Break</li>
            </ol>
        </nav>
        <div class="row">
            <div class="col-md-12">
                <div class="card  mb-4 bg-primary border-light shadow-soft">

                    <div class="card-body">
                        <div class="table-responsive">
                            <table class="table w-100 table-striped table-bordered">
                                <asp:GridView ID="grv" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                                    runat="server" Width="100%" CellPadding="10" BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive wrap dataTable-view">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr.No.">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Error Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblErrorcode" runat="server" Text='<%#Eval("code")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Error Description">
                                            <ItemTemplate>
                                                <asp:Label ID="lblErrorDesc" runat="server" Text='<%#Eval("descr")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Submitted">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCallStartDate" runat="server" Text='<%#Eval("submitted")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                </asp:GridView>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </main>
    <!---For Paging and Filter Start---->
        <script src="vendor/jquery/jquery-3.5.1.min.js"></script>
        <!-- Page level plugins -->
        <script src="vendor/datatables/jquery.dataTables.min.js"></script>
        <script src="vendor/datatables/dataTables.bootstrap4.min.js"></script>
        <script src="vendor/datatables/dataTables.responsive.min.js"></script>
        <script src="vendor/datatables/responsive.bootstrap4.min.js"></script>
        <!---For Paging and Filter End---->
        <!-- Page level custom scripts -->
        <script src="js/demo/datatables-demo.js"></script>

    <script>
            // Datatable Script 
            $(document).ready(function () {
                $('.dataTable-view').DataTable();
            });
        </script>
</asp:Content>
