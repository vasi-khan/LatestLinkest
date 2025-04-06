<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="DealerwiseReport.aspx.cs" Inherits="eMIMPanel.DealerwiseReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <cc1:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </cc1:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <main>
                <div class="container-fluid">
                    <nav aria-label="breadcrumb" class="my-3">
                        <ol class="breadcrumb breadcrumb-info">
                            <li class="breadcrumb-item"><a href="#">Home</a></li>
                            <li class="breadcrumb-item"><a href="#">Reports</a></li>
                            <li class="breadcrumb-item active" aria-current="page">Dealer wise Reports</li>
                        </ol>
                    </nav>

                    <div class="row">
                        <div class="col-xl-12 col-lg-12">
                            <div class="card card-body bg-primary border-light shadow-soft mb-4 py-3 px-4">
                                <div class="row align-items-center p-0">
                                    <div class="col-md-2">

                                        <asp:RadioButton ID="rdbdealerwise" Checked="true" runat="server" Text="" GroupName="1" />
                                        &nbsp;  
                                    <label for="ContentPlaceHolder1_rdbdealerwise" class="col-form-label font-weight-bold">Dealer wise</label>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:RadioButton ID="rdbDealerCampaignwise" runat="server" Text="" GroupName="1" />
                                        &nbsp; 
                                     <label for="ContentPlaceHolder1_rdbDealerCampaignwise" class="col-form-label font-weight-bold">Dealer wise campaign wise</label>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:RadioButton ID="rdbdealerDatewise" runat="server" Text="" GroupName="1" />
                                        <label for="ContentPlaceHolder1_rdbdealerDatewise" class="col-form-label font-weight-bold">Dealer and Date wise</label>
                                    </div>
                                </div>
                                <div class="row align-items-center">
                                    <div class="col-md-3">
                                        <label class="col-form-label font-weight-bold">From Date</label>
                                        <asp:TextBox ID="txtFrm" runat="server" onchange="javascript:text_changed_from();" class="form-control datepicker" placeholder="From Date" autocomplete="off"></asp:TextBox>
                                        <asp:HiddenField ID="hdntxtFrm" runat="server" />

                                    </div>
                                    <div class="col-md-3">
                                        <label class="col-form-label font-weight-bold">To Date</label>
                                        <asp:TextBox ID="txtTo" runat="server" onchange="javascript:text_changed_to();" class="form-control datepicker mt-3 my-lg-0" placeholder="To Date" autocomplete="off"></asp:TextBox>
                                        <asp:HiddenField ID="hdntxtTo" runat="server" />
                                    </div>


                                    <div class="col-md-3">
                                        <label class="col-form-label font-weight-bold">Dealer Code</label>
                                        <asp:TextBox ID="txtDealerCode" runat="server" class="form-control" autocomplete="off"></asp:TextBox>
                                    </div>



                                    <div class="col-md-2">
                                        <label>&nbsp;</label>
                                        <asp:LinkButton runat="server" ID="btnShow" class="btn btn-block mt-3 my-lg-0" OnClick="btnShow_Click">
                                             Show <i class="fas fa-eye" aria-hidden="true"></i>
                                        </asp:LinkButton>
                                    </div>

                                    <asp:HiddenField ID="h1" runat="server" />
                                    <asp:HiddenField ID="h2" runat="server" />


                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="card mb-4 bg-primary border-light shadow-soft">
                        <div class="card-header pt-3 pb-0 bg-primary d-flex justify-content-between flex-wrap align-items-center">
                            <div class="flex-fill">
                                <h6 class="font-weight-bold mb-0"><i class="fas fa-chart-line"></i>Dealer wise Report</h6>
                            </div>
                            <div class="downB">
                                <asp:LinkButton runat="server" ID="lnkDownload" Visible="false" class="btn btn-block" OnClick="lnkDownload_Click">
                                                    Download <i class="fas fa-download" aria-hidden="true"></i>
                                </asp:LinkButton>
                            </div>
                        </div>

                        <div class="card-body">
                            <div class="table-responsive">
                                <asp:GridView ID="grvDtl" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                                    runat="server" Width="100%" CellPadding="10"
                                    BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive nowrap dataTable-view">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr No">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Dealer Code">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDlrCode" runat="server" Text='<%#Eval("DlrCode")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Campaign Name">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCampaignName" runat="server" Text='<%#Eval("CampaignName")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Send Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSentDate" runat="server" Text='<%#Eval("SENTDATE")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SUBMITTED">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSUBMITTED" runat="server" Text='<%#Eval("SUBMITTED")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Delivered">
                                            <ItemTemplate>
                                                <asp:Label ID="lbldelivered" runat="server" Text='<%#Eval("delivered")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Failed">
                                            <ItemTemplate>
                                                <asp:Label ID="lblfailed" runat="server" Text='<%#Eval("failed")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="unknown">
                                            <ItemTemplate>
                                                <asp:Label ID="lblunknown" runat="server" Text='<%#Eval("unknown")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>


                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
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
            </main>

            <script type="text/javascript">
                $(document).ready(function () {
                    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

                    function EndRequestHandler(sender, args) {
                        $('.datepicker').datepicker({ dateFormat: 'dd-mm-yy' });
                    }

                });
                function loadscrq() {
                    $('.datepicker').datepicker({ dateFormat: 'dd-mm-yy' });
                }
            </script>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="lnkDownload" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="0">
        <ProgressTemplate>
            <div align="center" style="position: absolute; left: 40%; top: 50%; vertical-align: middle;">
                <img src="Img/LOADING.GIF" />
            </div>
            <div align="center" style="position: absolute; left: 0%; top: 0%; vertical-align: middle;">
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
