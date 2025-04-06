<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="click-reports_u.aspx.cs" Inherits="eMIMPanel.click_reports_u" %>

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
                            <li class="breadcrumb-item active" aria-current="page">Click Reports</li>
                        </ol>
                    </nav>

                    <!-- Content Row -->
                    <div class="row" style="display: none;">
                        <div class="col-md-12">
                            <div class="card bg-primary border-light shadow-soft mb-4">
                                <div class="card-body">
                                    <form>
                                        <div class="form-row">
                                            <div class="form-group col-md-2">
                                                <label for="#">To Date</label>
                                                <input type="date" class="form-control" placeholder="To Date" textmode="Date">
                                            </div>
                                            <div class="form-group col-md-2">
                                                <label for="#">From Date</label>
                                                <input type="date" class="form-control" placeholder="From Date">
                                            </div>
                                            <div class="form-group col-md-3">
                                                <label for="#">Links</label>
                                                <select class="drop-select form-control" data-live-search="true">
                                                    <option selected="">Select</option>
                                                    <option data-tokens="10" value="1">10</option>
                                                    <option data-tokens="20" value="2">20</option>
                                                    <option data-tokens="30" value="3">50</option>
                                                </select>
                                            </div>
                                            <div class="form-group col-md-2">
                                                <label for="#">Search(Country, City)</label>
                                                <input type="text" class="form-control" placeholder="">
                                            </div>
                                            <div class="form-group col-md-2 mt-auto">
                                                <a class="btn btn-primary btn-block text-success" href="#" role="button"><i class="fas fa-search text-success"></i>Search </a>
                                            </div>
                                            <div class="form-group col-md-1 mt-lg-auto">
                                                <a href="#"><i class="fas fa-file-excel fa-2x text-success"></i></a>
                                                <a href="#"><i class="fas fa-file-csv fa-2x text-danger"></i></a>
                                                <a href="#"><i class="far fa-file-alt fa-2x text-secondary"></i></a>
                                            </div>
                                        </div>
                                    </form>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Content Row -->
                    <div class="row">
                        <div class="col-xl-12 col-lg-12">
                            <!-- Basic Card Example -->
                            <div class="card bg-primary border-light shadow-soft mb-4">
                                <div class="card-header py-3 bg-primary d-flex justify-content-between flex-wrap align-content-center">
                                    <h6 class="m-0 font-weight-bold my-auto"><i class="fas fa-hand-pointer"></i>URL Click</h6>
                                    <div class="right-view">
                                        <div class="row">

                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtFrm"  runat="server"  onchange="javascript:text_changed_from();" class="form-control datepicker" placeholder="From Date" autocomplete="off"></asp:TextBox>
                                                <asp:HiddenField ID="hdntxtFrm" runat="server" />
                                                <%--<cc1:CalendarExtender ID="txtFrm_Ext" runat="server" Format="dd/MMM/yyyy" TargetControlID="txtFrm">
                                                </cc1:CalendarExtender>--%>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtTo" runat="server"  onchange="javascript:text_changed_to();"  class="form-control datepicker" placeholder="To Date" autocomplete="off"></asp:TextBox>
                                                <asp:HiddenField ID="hdntxtTo" runat="server" />
                                               <%-- <cc1:CalendarExtender ID="txtTo_Ext" runat="server" Format="dd/MMM/yyyy" TargetControlID="txtTo">
                                                </cc1:CalendarExtender>--%>
                                            </div>

                                            <div class="col-md-4">
                                                 <asp:LinkButton runat="server" OnClientClick="return CheckDates();" ID="btnShow" OnClick="btnUpdate_Click" class="btn btn-mini">
                                                    Show <i class="fas fa-eye" aria-hidden="true"></i>
                                                </asp:LinkButton>
                                            </div>

                                            <asp:HiddenField ID="h1" runat="server" />
                                            <asp:HiddenField ID="h2" runat="server" />
                                        </div>
                                    </div>
                                </div>
                                <div class="card-body">
                                    <div class="table-responsive">
                                        
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
                                                                <asp:LinkButton ID="LinkButton1" runat="server" OnClick="btnXL_Click" class="btn btn-datatable btn-icon btn-transparent-dark px-2 py-0" data-toggle="tooltip" data-placement="top" title="XLS"><i class="far fa-file-excel"></i></asp:LinkButton>
                                                                <asp:LinkButton ID="LinkButton2" runat="server" OnClick="btnView_Click" class="btn btn-datatable btn-icon btn-transparent-dark px-2 py-0" data-toggle="tooltip" data-placement="top" title="Details"><i class="far fa-file-alt"></i></asp:LinkButton>

                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Long URL">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbllongurl" runat="server" Text='<%#Eval("LongURL")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Small URL">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl1" runat="server" Text='<%#Eval("smallURL")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Creation Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl2" runat="server" Text='<%#Eval("CreationDate")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Tot URL Sent">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl3" runat="server" Text='<%#Eval("No_of_url_sent")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="No of Hits">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl4" runat="server" Text='<%#Eval("No_Of_Hits")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="URL ID">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblurlid" runat="server" Text='<%#Eval("URLID")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                       
                                                    </Columns>

                                                   <%-- <EmptyDataTemplate>
                                                        <table style="width: 100%;">
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:Label ID="lblEmpty" Text="No Data Found!!!" Style="color: Red;" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </EmptyDataTemplate>--%>
                                                </asp:GridView>
                                        
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </main>

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


   
</asp:Content>
