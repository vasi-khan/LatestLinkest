<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="AccountList.aspx.cs" Inherits="eMIMPanel.AccountList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <cc1:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </cc1:ToolkitScriptManager>
    <main>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <div class="container-fluid">
                    <nav aria-label="breadcrumb" class="my-3">
                        <ol class="breadcrumb breadcrumb-info">
                            <li class="breadcrumb-item"><a href="#">Home</a></li>
                            <li class="breadcrumb-item active" aria-current="page">Account List</li>
                        </ol>
                    </nav>

                    <div class="row">
                        <div class="col-12">
                            <div class="card card-body bg-primary border-light shadow-soft mb-4">
                                <div class="row px-3">
                                    <div class="col-12 col-md-2 mt-2">
                                        <asp:RadioButton  runat="server" ID="rbl1" Checked="true" GroupName="ABC" />
                                        <spna class="px-2">All Account</spna>

                                    </div>
                                    <div class="col-12 col-md-4">
                                        <asp:RadioButton runat="server" ID="rbl2" GroupName="ABC" />
                                        <span>Inactive Account in Last  &nbsp;
                        <asp:TextBox runat="server" ID="txtrbl2" Width="20%" CssClass="btn btn-primary" ></asp:TextBox>
                                         &nbsp;    Days
                                        </span>
                                    </div>
                                    <div class="col-6 col-md-4">
                                        <asp:RadioButton runat="server" ID="rbl3" GroupName="ABC" />
                                        <span  class="px-2">Account Balance Below  &nbsp;
                         <asp:TextBox runat="server" ID="txtrbl3" Width="25%" CssClass="btn btn-primary"></asp:TextBox>
                                        </span>

                                    </div>

                                    <div class="col-6 col-md-2 text-right" >
                                        <asp:Button Text="SHOW" runat="server" ID="btnshow" ClientIDMode="Static" CssClass="btn-primary btn" OnClick="btnshow_Click" />
                                    </div>
                                    <%--<div class="col-12 col-md-12 py-3 mt-3">
                                        <div class="col-sm-12" style="text-align: center">
                                            <asp:Button Text="SHOW" runat="server" ID="btnshow" ClientIDMode="Static" CssClass="btn-primary btn" OnClick="btnshow_Click" />
                                        </div>
                                    </div>--%>


                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="card bg-primary border-light shadow-soft mb-4 " id="div2" runat="server" visible="false">

                        <div class="row  mb-2 p-3">
                            <%--<div  >--%>
                            <div class="col-10">
                                <%--<h6 style="font-weight:bold">Low Balance Accounts</h6>--%>
                                <span class="text font-weight-bold">
                                    <asp:Label ID="lblheading" runat="server" Style="font-weight: bold; " Text=""></asp:Label>
                                    <%--Total Accounts:--%>
                                    <asp:Label runat="server" ID="lbltotact" CssClass="form-label"></asp:Label>
                                </span>
                            </div>

                            <div class="col-2 text-right">
                                <asp:LinkButton runat="server" ID="btndownload" CssClass="btn btn-primary" Text="Download" OnClick="btndownload_Click"></asp:LinkButton>
                            </div>
                            <%--</div>--%>
                        </div>


                        <%--<div class="  ">--%>
                        <div class="table-responsive p-3 ">
                            <asp:GridView runat="server" Class="table table-striped table-bordered dt-responsive wrap dataTable-view" Style="vertical-align: middle;" ID="grdview"
                                Width="100%" AutoGenerateColumns="false">
                                <%--<asp:GridView ID="grdview" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false" ShowFooter="true"
                                                    runat="server" Width="100%" CellPadding="10" BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive wrap dataTable-view">--%>
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr.No.">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                   <asp:TemplateField HeaderText="USERID">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTranDate" runat="server" Text='<%#Eval("USERID")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                    <asp:TemplateField HeaderText="CURRENT BALANCE">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTranDate" runat="server" Text='<%#Eval("CURRENT BALANCE")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                    <asp:TemplateField HeaderText="COMPANY NAME">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTranDate" runat="server" Text='<%#Eval("COMPANY NAME")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="FULL NAME">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTranDate" runat="server" Text='<%#Eval("FULLNAME")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="MOBILE">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTranDate" runat="server" Text='<%#Eval("MOBILE")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="EMAIL">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTranDate" runat="server" Text='<%#Eval("EMAIL").ToString().Length>=15?Eval("EMAIL").ToString().Substring(0,15):Eval("EMAIL").ToString() %>' ToolTip='<%#Eval("EMAIL").ToString().Length>=15?Eval("EMAIL").ToString():"" %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText="A/C DATE">
                                    <ItemTemplate>  
                                        <asp:Label ID="lblTranDate" runat="server" Text='<%#Eval("ACCOUNT CREATEDON DATE")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>




                                        <asp:TemplateField HeaderText="LAST RECHARGE AMOUNT" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTranDate" runat="server" Text='<%#Eval("LASTRECHARGEAMOUNT")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                        <asp:TemplateField HeaderText="LAST RECHARGE DATE" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTranDate" runat="server" Text='<%#Eval("LASTRECHARGEDATE")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>


                                        
                                        <asp:TemplateField HeaderText="LAST LOGIN">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTranDate" runat="server" Text='<%#Eval("LAST LOGIN")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>


                                    <%--<asp:BoundField  HeaderText="USERNAME" DataField="username"   />
                <asp:BoundField  HeaderText="CAMPNAME" DataField="COMPNAME"   />
                <asp:BoundField  HeaderText="FULL NAME" DataField="FULLNAME"   />
                <asp:BoundField  HeaderText="MOBILE " DataField="MOBILE1"   />
                <asp:BoundField  HeaderText="EMAIL" DataField="EMAIL"   />
                <asp:BoundField  HeaderText="DLT NO" DataField="DLTNO"   />
                <asp:BoundField  HeaderText="ACOUNT CREATION DATE" DataField="ACCOUNTCREATEDON" />
                <asp:BoundField  HeaderText="BALANCE" DataField="balance"   />
                <asp:BoundField  HeaderText="Last LoginTime" DataField="lastlogintime"   />--%>
                                </Columns>
                                <EmptyDataTemplate>
                                    Data Not Found
                                </EmptyDataTemplate>
                            </asp:GridView>

                        </div>
                        <%--</div>--%>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnshow" />
                <asp:PostBackTrigger ControlID="txtrbl2" />
                <asp:PostBackTrigger ControlID="txtrbl3" />
            </Triggers>
        </asp:UpdatePanel>
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






    <%--<script src="DataTable/jquery.dataTables.min.js"></script>
        <link href="DataTable/dataTables.bootstrap4.min.css" rel="stylesheet" />
    <script src="DataTable/jquery.dataTables.js"></script>

    

    <link href="DataTable/dataTables.bootstrap4.css" rel="stylesheet" />
    <script src="DataTable/dataTables.bootstrap4.js"></script>


    <script src="DataTable/dataTables.responsive.min.js"></script>--%>
</asp:Content>
