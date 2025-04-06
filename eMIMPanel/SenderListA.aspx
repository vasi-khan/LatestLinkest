<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="SenderListA.aspx.cs" Inherits="eMIMPanel.SenderListA" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main>
        <div class="container-fluid">
            <nav aria-label="breadcrumb" class="my-3">
                <ol class="breadcrumb breadcrumb-info">
                    <li class="breadcrumb-item"><a href="#">Home</a></li>
                    <li class="breadcrumb-item active" aria-current="page">SenderId List</li>
                </ol>
            </nav>

            <!-- Content Row -->
            <div class="row">

                <!-- Area Chart -->
                <div class="col-xl-12 col-lg-12">
                    <!-- Basic Card Example -->
                    <div class="card bg-primary border-light shadow-soft mb-4">
                        <div class="card-header py-3 bg-primary d-flex justify-content-between flex-wrap align-content-center">
                            <h6 class="m-0 font-weight-bold my-auto">SenderId List</h6>
                            <div class="right-view">
                                <div class="row">
                                    <div class="col-md-5">
                                        <asp:TextBox ID="txtUserId" runat="server" class="form-control" placeholder="User Id"></asp:TextBox>
                                    </div>
                                    <div class="col-md-auto">
                                        <asp:LinkButton runat="server" ID="LinkButton2" OnClick="btnUpdate_Click" class="btn btn-mini">
                                                    Show <i class="fas fa-eye" aria-hidden="true"></i>
                                        </asp:LinkButton>
                                    </div>
                                    <div class="col-md-auto">
                                        <asp:LinkButton runat="server" ID="lnkDelete" OnClick="lnkDelete_Click" ToolTip="Delete" class="btn btn-mini" OnClientClick='return confirm("Are you sure you want to delete this record?");'>
                                   <i class="fas fa-trash" aria-hidden="true"></i>
                                        </asp:LinkButton>
                                    </div>
                                    <div class="col-md-auto">
                                        <asp:LinkButton runat="server" ID="lnkDownload" OnClick="lnkDownload_Click" ToolTip="Download" class="btn btn-mini">
                                   <i class="fas fa-download" aria-hidden="true"></i>
                                        </asp:LinkButton>
                                    </div>

                                </div>
                            </div>
                        </div>
                        <div class="card-body">
                            <asp:GridView ID="grv" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                                runat="server" Width="100%" CellPadding="10" BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive nowrap dataTable-view">
                                <Columns>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkAllSelect" runat="server" onclick="CheckAll(this);" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr No">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="User ID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUserId" runat="server" Text='<%#Eval("userid")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sender ID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblsender" runat="server" Text='<%#Eval("senderid")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Country Code">
                                        <ItemTemplate>
                                            <asp:Label ID="lblccode" runat="server" Text='<%#Eval("countrycode")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
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
        </div>
    </main>
    <script type="text/javascript"> 
        function CheckAll(Checkbox) {
            var GridVwHeaderCheckbox = document.getElementById("<%=grv.ClientID %>");
            for (i = 1; i < GridVwHeaderCheckbox.rows.length; i++) {
                GridVwHeaderCheckbox.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
            }
        }
    </script>
    <script type="text/javascript">  
        function ConfirmSubmit() { 
            if (confirm('Are you sure you want to delete it?. Do you want to continue ?')) {
                if (confirm('Are you sure you want to delete it?')) {
                    return true;
                } else return false;
            } else return false; 
    </script>
</asp:Content>
