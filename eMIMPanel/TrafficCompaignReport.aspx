<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="TrafficCompaignReport.aspx.cs" Inherits="eMIMPanel.TrafficCompaignReport" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
    <main>
         <!-- Content Row -->

        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <div class="container-fluid">
            <nav aria-label="breadcrumb" class="my-3">
                <ol class="breadcrumb breadcrumb-info">
                    <li class="breadcrumb-item"><a href="#">Home</a></li>
                    <li class="breadcrumb-item active" aria-current="page">Traffic Compaign Report</li>
                </ol>
            </nav>
            <div class="row">

                <!-- Area Chart -->
                <div class="col-xl-12 col-lg-12">
                    <!-- Basic Card Example -->
                    
                   
                <div class="card bg-primary border-light shadow-soft mb-4" >
                    <div class="row p-2" >
                       <%-- <div class="col-sm-1">
                            <label runat="server" class="form-label"> Customers</label>
                        </div>--%>
                        <div class="col-sm-2">
                            <asp:TextBox runat="server" ID="txtcust" CssClass="form-control" placeholder="Customer ID"></asp:TextBox>
                        </div>
                        <div class="col-sm-1">
                            
                            <asp:LinkButton runat="server" ID="btnFilter" OnClick="btnFilter_Click" class="btn btn-datatable btn-icon btn-transparent-dark px-2 py-2" data-toggle="tooltip" data-placement="top">
                                <i class="fa fa-search "></i>
                            </asp:LinkButton>
                        </div>
                    
                    <%--<div class="row">--%>
                        <div class="col-sm-5">
                            <span class="form-inline">
                                Show Companion of Last 
                                <asp:TextBox runat="server" ID="txtmonth" CssClass="form-control mx-2" Style="width:15%; "></asp:TextBox>
                                Months
                            </span>
                        </div>
                        <div class="col-sm-4">
                              <asp:Button runat="server" Text="Submit" class="btn btn-primary" ID="finalSubmit" OnClick="finalSubmit_Click"/>
                        <asp:Button runat="server" Text="Reset" class="btn btn-primary" ID="btnreset" OnClick="btnreset_Click"/>
                
                        </div>
                        <%--</div>--%>
                    <%--</div>--%>
                    <%--div class="row">--%>
                          <%--</%--div>--%>
                   </div>
                    </div>
                     <div class="card bg-primary border-light shadow-soft mb-4" >
                    <div class="table-responsive" style="height: 350px; overflow-y: scroll;">
                          <div  class="row justify-content-center">
                                <asp:LinkButton runat="server" ID="lnkDownload" OnClick="btnXL_Click" class="btn btn-mini">
                                Download <i class="fas fa-download" aria-hidden="true"></i>
                                </asp:LinkButton>                            </div>
                <%-- <asp:LinkButton ID="btnXL" runat="server" OnClick="btnXL_Click"  Visible="false" class="btn btn-datatable btn-icon btn-transparent-dark px-2 py-0" data-toggle="tooltip" data-placement="top" title="XLS"><i class="far fa-file-excel"></i></asp:LinkButton>--%>
                <asp:GridView runat="server" AutoGenerateColumns="true" Class="table table-striped table-bordered dt-responsive nowrap dataTable-view"  ID="grd" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" Width="100%" CellPadding="10" BorderColor="#ede8e8" >

                </asp:GridView>
                        </div>
                         </div>
                <!--==================Start Spare Part Details Filter=================-->
                                    <asp:Panel ID="POPUPCO" class="modalsparePopup" runat="server" Style="display: none; top:10%!important">

                                        <div class=" modal-dialog-centered modal-dialog-scrollable">
                                            <div class="modal-content modal-xl ">
                                                <div class="modal-header">
                                                    <h5 class="modal-title" runat="server" id="H3">Customer FIlter</h5>

                                                    <%--<button type="button" class="close" aria-label="Close">
                                                        <span aria-hidden="true">&times;</span>
                                                    </button>--%>
                                                  

                                                </div>
                                                <div class="row">
                                                 <div class="col-12">
                                            <div class="card card-body mb-4 bg-primary border-light shadow-soft">
                                                <div class="form-row">
                                            <div class="right-view">
                                                
                                                        <div class="row">
                                                                     <div class="col-md-3">
                                                                <div class="form-group">
                                                                    
                                                                    <div class="input-group">
                                                                        <asp:TextBox ID="txtprofileID" runat="server" class="form-control" MaxLength="15" placeholder="Enter Profile ID" onkeypress="return /^[a-zA-Z0-9\s]{0,20}$/.test(this.value+event.key);"></asp:TextBox>
                                                                       <%-- <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" FilterMode="ValidChars" ValidChars="ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789" runat="server" TargetControlID="txtRegnNo"></asp:FilteredTextBoxExtender>--%>
                                                                 </div>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-3bb">
                                                                <div class="form-group">
                                                                    
                                                                    <asp:TextBox runat="server" class="form-control" ID="txtfullname" placeholder="Enter Full Name" onkeypress="return /^[a-zA-Z0-9\s]{0,20}$/.test(this.value+event.key);"></asp:TextBox>

                                                                </div>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <div class="form-group">
                                                                   
                                                                    <div class="input-group">
                                                                        <asp:TextBox runat="server" class="form-control"  ID="txtcompname" placeholder="Enter Company Name"></asp:TextBox>
                                                                         <%--<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" FilterMode="ValidChars" ValidChars="ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789" runat="server" TargetControlID="txtEngineNo"></asp:FilteredTextBoxExtender>
                                                                    --%></div>
                                                                </div>
                                                            </div>
                                                           

                                                            <div class="col-md-1">
                                                                <asp:Button runat="server" ID="Filtergo" Text="Filter" class="btn btn-primary" OnClick="Filtergo_Click"  />
                                                            </div>
                                                          
                                                            

                                                        </div>
                                                    
                                                 </div>

                                                </div>
                                                </div>
                                                 </div>


                                                    <!--grid view Find-->
                                                     <div class="accordion shadow-soft rounded" id="accordionExample">
                        <div class="card card-sm card-body bg-primary border-light mb-0">
                                               <%--     <div class="card card-body mb-4 bg-primary border-light shadow-soft">

                        <div class="form-row">
                            <div class="right-view">--%>
                          
                            <div id="collapseOne" class="collapse show" aria-labelledby="headingOne" data-parent="#accordionExample" runat="server">
                                <div class="card-body px-0">
                                    <div class="row">
                                        <!-- Area Chart -->
                                        <div class="col-xl-12 col-lg-12">
                                                    
                                                        <div class="table-responsive" style="height: 350px; overflow-y: scroll;">
                                                            <asp:GridView ID="GrdViewCustInfo" runat="server" AutoGenerateColumns="false" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true"  Width="100%" CellPadding="10" BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive nowrap dataTable-view">
                                                                <Columns>                                                                   
                                                                    <asp:TemplateField HeaderText=" S No" ItemStyle-Width="30%">
                                                                        <ItemTemplate>
                                                                          <%#Container.DataItemIndex+1 %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Profile ID" ItemStyle-Width="30%" >
                                                                        <ItemTemplate>
                                                                            <asp:Label runat="server" ID="grdlblUsername" Text='<%#Eval("username") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Full Name" ItemStyle-Width="30%">
                                                                        <ItemTemplate>
                                                                            <%#Eval("FULLNAME") %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Comp Name" ItemStyle-Width="30%">
                                                                        <ItemTemplate>
                                                                            <%#Eval("COMPNAME") %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                
                                                                    <asp:TemplateField HeaderText="Show" ItemStyle-Width="10%">
                                                                        <ItemTemplate>
                                                                         
                                                                            <asp:LinkButton ID="btn_FindViewrecord" runat="server"  OnClick="btn_FindViewrecord_Click">
                                                                       
                                                                      <center>  <i class="fa fa-solid fa-eye"> </i></center>
                                                                            </asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>

                                                            </asp:GridView>
                                                        </div>
                                                    
                                            </div>
                                        </div>

                                </div>
                             
                                                
                                                <div class="modal-footer p-2">

                                                    <asp:Button ID="btbclose" runat="server" class=" btn btn-primary" Text="Close" UseSubmitBehavior="false" OnClick="btbclose_Click"></asp:Button>

                                                    <%-- <asp:Button ID="btnSubmit_Find" runat="server" Text="Submit" class="btn btn-primary" UseSubmitBehavior="False"  EventValidationTrue="True" OnClick="btnSubmit_Find_Click"></asp:Button> --%>
                                                </div>
                                   </div>
                            </div>
                                            </div>
                                          </div>
                                        </div>
                                        <%--</div>--%>
                                        <%--</div>--%>
                                    </asp:Panel>
                                    <asp:LinkButton runat="server" ID="LinkButton6"></asp:LinkButton>

                                    <asp:ModalPopupExtender runat="server" ID="ModalPopupex" TargetControlID="LinkButton6" PopupControlID="POPUPCO" BackgroundCssClass="modalBackground">
                                    </asp:ModalPopupExtender>
                                    <!--====================End Spare Part Details Filter=================-->

                </div>
                </div>
                
            </div>
        </main>
           
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
