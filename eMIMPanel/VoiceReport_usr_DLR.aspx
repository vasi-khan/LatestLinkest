<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="VoiceReport_usr_DLR.aspx.cs"  Inherits="eMIMPanel.VoiceReport_usr_DLR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <cc1:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </cc1:ToolkitScriptManager>
    <main>
        <div class="container-fluid">
            <nav aria-label="breadcrumb" class="my-3">
                <ol class="breadcrumb breadcrumb-info">
                    <li class="breadcrumb-item"><a href="#">Home</a></li>
                    <li class="breadcrumb-item"><a href="#">Reports</a></li>
                    <li class="breadcrumb-item active" aria-current="page"> Download Voice Report</li>
                </ol>
            </nav>


                                     <div class="row">
                                        <div class="col-md-2">
                                            <label class="col-form-label" >Date From </label>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="txtFrm1" runat="server" onchange="javascript:text_changed_from1();" class="form-control" placeholder="From Date"  TextMode="Date"  autocomplete="off" ></asp:TextBox>
                                            
                                            <asp:HiddenField ID="hdntxtFrm1" runat="server" />
                                        </div>
                                   <div class="col-md-2">
                                  <label class="col-form-label" >Date To </label>
                              </div>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="txtTo1" runat="server" onchange="javascript:text_changed_to1();" class="form-control" placeholder="To Date" TextMode="Date"  autocomplete="off"></asp:TextBox>
                                            <asp:HiddenField ID="hdntxtTo1" runat="server" />
                                        </div>
                                   <div class="form-group col-lg-2  col-xl-2 mt-auto ">
                                <asp:LinkButton ID="btnshow" runat="server" OnClick="btnshow_Click" class="btn btn-primary text-success btn-block"><i class="fas fa-search fa-sm text-success"></i>Show</asp:LinkButton>
                            </div>
                                    </div>
                       





            <div class="card card-body mb-4 bg-primary border-light shadow-soft">
                       
                                    <div runat="server" id="divexportexcel" style="text-align:right;">
                                        <asp:Button ID="btnExport" runat="server" Text="Export to XLS" class="btn btn-primary text-success font-weight-bold" OnClick="btnExport_Click"  style="margin-bottom:1rem"/>
                                    </div>
                           
                        <div class="table-responsive">
                            <asp:GridView ID="grv2" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                                runat="server" Width="100%" CellPadding="10" BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive nowrap dataTable-view" >
                                <Columns>
                                    <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr No">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Mobile No">
                                        <ItemTemplate>
                                            <asp:Label ID="txtmob" runat="server" Text='<%#Eval("mobileno")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Send Date">
                                        <ItemTemplate>
                                            <asp:Label ID="grdsenddate" runat="server" Text='<%#Eval("sendtime")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Voice Text">
                                        <ItemTemplate>
                                            <asp:Label ID="grdvoicetext" runat="server" Text='<%#Eval("messagetext")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                </Columns>
                            </asp:GridView>

                        </div>
                    </div>
            </div>
    </main>


<%--    <script src="CDN/datatableResponsive.js"></script>
                <script src="CDN/dataTables.bootstrap4.min.js"></script>
                <script src="CDN/dataTables.responsive.min.js"></script>
                <script src="CDN/datatable.min.js"></script>--%>
   <%-- <script src="DataTable/dataTables.responsive.min.js"></script>
    <script src="DataTable/jquery.dataTables.min.js"></script>
    <script src="DataTable/dataTables.bootstrap4.min.js"></script>--%>

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
        function text_changed_from1() {
            var d = document.getElementById("ContentPlaceHolder1_txtFrm1").value
            console.log(d);
            document.getElementById("ContentPlaceHolder1_hdntxtFrm1").value = d;
        }
        function text_changed_to1() {
            var d = document.getElementById("ContentPlaceHolder1_txtTo1").value
            console.log(d);
            document.getElementById("ContentPlaceHolder1_hdntxtTo1").value = d;
        }

    </script>
</asp:Content>
