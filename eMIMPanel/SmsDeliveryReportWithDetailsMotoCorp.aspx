<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="SmsDeliveryReportWithDetailsMotoCorp.aspx.cs" Inherits="eMIMPanel.SmsDeliveryReportWithDetailsMotoCorp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        /*CSS Classes For Design Modal*/
        .modal.modalPopup {
            top: 0 !important;
            left: 0 !important;
            display: block;
        }

        .modalBackground {
            background-color: #000;
            opacity: 0.5;
        }
    </style>
    <style>
        .ajax__combobox_itemlist {
            top: 39px !important;
            left: 0px !important;
        }
    </style>
     <style type="text/css">
          .select2-container--default .select2-selection--single {
            background-color: #e6e7ee !important;
            border-radius: 4px;
            border: 1px solid #aaa !important;
            box-shadow: inset 2px 2px 5px #b8b9be, inset -3px -3px 7px #ffffff !important;
        }

        .select2-search--dropdown .select2-search__field {
            padding: 4px;
            width: 100%;
            box-sizing: border-box;
            background: #e6e7ee !important;
            border: 1px solid red;
        }
    </style>
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
                    <li class="breadcrumb-item active" aria-current="page">Download Delivery Report</li>
                </ol>
            </nav>
            <!-- Content Row -->
            <div class="row" style="display: none;">
                <div class="col-12">
                    <div class="card bg-primary border-light shadow-soft mb-4">
                        <div class="card-header py-3 bg-primary">
                            <h6 class="m-0 font-weight-bold"><i class="fas fa-info-circle"></i>Info</h6>
                        </div>
                        <div class="card-body py-2">
                            <div class="row">
                                <!-- Card-->
                                <div class="col-12 col-md-6 col-sm-6 col-lg-6 col-xl-4 mb-2 count-view">
                                    <a href="sms-reports.html">
                                        <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                            <div class="card-body">
                                                <div class="row no-gutters align-items-center">
                                                    <div class="col mr-2">
                                                        <div class="text-xs font-weight-bold text-warning text-uppercase mb-1">Submitted</div>
                                                        <div class="h5 mb-0 font-weight-bold text-gray-800">
                                                            <asp:Label ID="lblTodaySubmitted" runat="server"></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="col-auto">
                                                        <div class="icon icon-shape shadow-soft rounded-circle">
                                                            <span class="fas fa-check-circle fa-2x text-warning"></span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </a>
                                </div>
                                <!-- End Card -->
                                <!-- Card-->
                                <div class="col-12 col-md-6 col-sm-6 col-lg-6 col-xl-4 mb-2 count-view">
                                    <a href="sms-reports.html">
                                        <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                            <div class="card-body">
                                                <div class="row no-gutters align-items-center">
                                                    <div class="col mr-2">
                                                        <div class="text-xs font-weight-bold text-success text-uppercase mb-1">Delivered</div>
                                                        <div class="row no-gutters align-items-center">
                                                            <div class="col-auto">
                                                                <div class="h5 mb-0 mr-3 font-weight-bold text-gray-900">
                                                                    <asp:Label ID="lblTodayDelivered" runat="server"></asp:Label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-auto">
                                                        <div class="icon icon-shape shadow-soft rounded-circle">
                                                            <span class="fas fa-check-double fa-2x text-success"></span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </a>
                                </div>
                                <!-- End Card -->

                                <!-- Card-->
                                <div class="col-12 col-md-6 col-sm-6 col-lg-6 col-xl-4 mb-4 count-view">
                                    <a href="sms-reports.html">
                                        <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                            <div class="card-body">
                                                <div class="row no-gutters align-items-center">
                                                    <div class="col mr-2">
                                                        <div class="text-xs font-weight-bold text-danger text-uppercase mb-1">Failed</div>
                                                        <div class="h5 mb-0 font-weight-bold text-danger-800">
                                                            <asp:Label ID="lblTodayFailed" runat="server"></asp:Label>
                                                        </div>
                                                    </div>
                                                    <div class="col-auto">
                                                        <div class="icon icon-shape shadow-soft rounded-circle">
                                                            <span class="fas fa-thumbs-down fa-2x text-danger"></span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </a>
                                </div>
                                <!-- End Card -->
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-12">
                    <div class="card card-body mb-4 bg-primary border-light shadow-soft">
                        <div class="form-row">
                            <asp:RadioButton ID="rbTdy" runat="server" AutoPostBack="true" Text="Today" GroupName="Filter" Checked="true" Style="margin-left: 20px" OnCheckedChanged="rbTdy_CheckedChanged" />
                            <asp:RadioButton ID="rbHis" runat="server" AutoPostBack="true" OnCheckedChanged="rbHis_CheckedChanged" Text="Old" Style="margin-left: 20px; margin-right: 50px" GroupName="Filter" />
                        </div>
                        <div class="form-row">
                            <asp:RadioButton ID="rbSbmtd" runat="server" AutoPostBack="true" Text="Submitted" GroupName="Filter1" Checked="true" Style="margin-left: 20px" />
                            <asp:RadioButton ID="rbDlvr" runat="server" AutoPostBack="true" Text="Delivery" Style="margin-left: 20px" GroupName="Filter1" />
                            <asp:RadioButton ID="rbFailed" runat="server" AutoPostBack="true" Text="Failed" Style="margin-left: 20px; margin-right: 50px" GroupName="Filter1" />
                            <asp:CheckBox ID="chkwithoutDealer" runat="server" Style="margin-left: 20px; margin-right: 50px" Text="Without Dealer Wise" AutoPostBack="true" OnCheckedChanged="chkwithoutDealer_CheckedChanged" />
                            <div class="form-group col-lg-2  col-xl-2 mt-auto ">
                                <asp:LinkButton ID="btnsearch" runat="server" OnClick="btnsearch_Click" class="btn btn-primary text-success btn-block"><i class="fas fa-search fa-sm text-success"></i>Search</asp:LinkButton>
                            </div>
                        </div>
                        <div id="divOld" runat="server" class="form-row mt-2 d-none">
                            <div class="form-row">
                                <div class="form-group col-lg-4 col-xl-4">
                                    <label for="#">Download Delivery Report</label>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtFrm1" runat="server" onchange="javascript:text_changed_from1();" class="form-control" placeholder="From Date" TextMode="Date" autocomplete="off"></asp:TextBox>
                                            <asp:HiddenField ID="hdntxtFrm1" runat="server" />
                                        </div>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtTo1" runat="server" onchange="javascript:text_changed_to1();" class="form-control" placeholder="To Date" TextMode="Date" autocomplete="off"></asp:TextBox>
                                            <asp:HiddenField ID="hdntxtTo1" runat="server" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group col-lg-3 col-xl-3">
                                    SenderID wise
                                <div class="input-group">
                                    <div class="input-group-prepend">
                                        <div class="input-group-text">
                                            <asp:RadioButton ID="rdbSender" GroupName="rdb" Checked="true" runat="server" aria-label="Radiobutton for following text input" />
                                        </div>
                                    </div>
                                    <asp:DropDownList ID="ddlSender" runat="server" class="custom-select">
                                    </asp:DropDownList>
                                </div>
                                </div>
                                <div class="form-group col-lg-3 col-xl-3">
                                    Campaign wise
                                <div class="input-group">
                                    <div class="input-group-prepend">
                                        <div class="input-group-text">
                                            <asp:RadioButton ID="rdbCamp" GroupName="rdb" runat="server" aria-label="Radiobutton for following text input" />
                                        </div>
                                    </div>
                                    <asp:DropDownList ID="ddlCamp" runat="server" class="custom-select"></asp:DropDownList>
                                </div>
                                </div>
                            </div>
                            <div class="form-row">
                                <div class="col-md-3">
                                    <div class="flex-fill font-weight-bold mr-lg-3">Template ID(Name)</div>
                                    <div class="input-group" style="width: max-content;">
                                        <asp:ComboBox runat="server" ID="ddltemplate" Style="font-family: 'Courier New';" placeholder="Enter Template id" AutoPostBack="true" AutoCompleteMode="SuggestAppend">
                                            <asp:ListItem Value="-1" Text="--Select--" Selected="True"></asp:ListItem>
                                        </asp:ComboBox>
                                        <asp:LinkButton runat="server" ID="Button3" OnClick="Button3_Click"><i  class="fa fa-search mt-3 px-2"></i>
                                        </asp:LinkButton>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="flex-fill font-weight-bold mr-lg-3">Search mobile number</div>
                                    <asp:TextBox ID="txtmob" runat="server" class="form-control" placeholder="XX-XXX-XXXX" MaxLength="10"></asp:TextBox>
                                    <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterMode="ValidChars"
                                        TargetControlID="txtmob" ValidChars="0123456789">
                                    </cc1:FilteredTextBoxExtender>
                                </div>
                                <div class="col-md-3">
                                    <div class="flex-fill font-weight-bold mr-lg-3">
                                        <br />
                                    </div>
                                    <asp:RadioButtonList runat="server" RepeatDirection="Horizontal" ID="rdblistselect" Style="margin-left: 20px">
                                        <asp:ListItem Value="D" Selected="True">Date Wise</asp:ListItem>
                                        <asp:ListItem Value="S">Single</asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </div>
                        </div>
                        <div class="form-row" runat="server" id="dealer">
                                <div class="col-md-3">
                                    <div class="flex-fill font-weight-bold mr-lg-3">Group Location</div>
                                    <div class="flex-fill">
                                        <asp:DropDownList ID="ddlCategory" runat="server" class="custom-select" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="flex-fill font-weight-bold mr-lg-3">Location</div>
                                    <asp:DropDownList ID="ddlLocation" runat="server" class="custom-select" AutoPostBack="true" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged">
                                        <asp:ListItem Value="0">--All--</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-3">
                                    <div class="flex-fill font-weight-bold mr-lg-3">SubLocation</div>
                                    <asp:DropDownList ID="ddlSubLocation" runat="server" class="custom-select" AutoPostBack="true" OnSelectedIndexChanged="ddlSubLocation_SelectedIndexChanged">
                                        <asp:ListItem Value="0">--All--</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-3" >
                                    <div class="flex-fill font-weight-bold mr-lg-3">Dealer</div>
                                    <asp:DropDownList ID="ddlDealerCode" runat="server" class="custom-select" ClientIDMode="Static">
                                        <asp:ListItem Value="0">--All--</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                    </div>
                    <div class="card card-body mb-4 bg-primary border-light shadow-soft">
                        <div class="table-responsive">
                            <asp:GridView ID="grv2" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false" OnRowDataBound="grv2_RowDataBound"
                                runat="server" Width="100%" CellPadding="10" BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive nowrap dataTable-view">
                                <Columns>
                                    <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr No">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="Downloadxlx" class="btn btn-datatable btn-icon btn-transparent-dark px-2 py-0" data-toggle="tooltip" data-placement="top" title="download" OnClick="Downloadxlx_Click" Visible="false"><i class="fas fa-file-excel fa-sm text-success"></i></asp:LinkButton>
                                            &nbsp;
                                            <asp:LinkButton ID="LinkButton1" runat="server" OnClick="btnDownLoad_Click" class="btn btn-datatable btn-icon btn-transparent-dark px-2 py-0" data-toggle="tooltip" data-placement="top" title="download"><i class="fas fa-file-csv fa-sm text-success"></i></asp:LinkButton>
                                            <asp:LinkButton ID="btnViewFile" runat="server" OnClick="btnViewFile_Click" class="btn btn-datatable btn-icon btn-transparent-dark px-2 py-0" data-toggle="tooltip" data-placement="top" title="View"><i class="far fa-eye fa-sm text-success"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Submit Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblsubmitdt" runat="server" Text='<%#Eval("submitdate")%>'></asp:Label>
                                            <asp:HiddenField ID="hdnto" runat="server" Value='<%#Eval("userid")%>'></asp:HiddenField>
                                            <asp:HiddenField ID="hdnFrmDW" runat="server" Value='<%#Eval("fromdate")%>'></asp:HiddenField>
                                            <asp:HiddenField ID="hdnToDW" runat="server" Value='<%#Eval("todate")%>'></asp:HiddenField>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Group Location">
                                        <ItemTemplate>
                                            <asp:Label ID="lblCategory" runat="server" Text='<%#Eval("CategoryName").ToString()=="0"?"ALL":Eval("CategoryName")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Location">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLocation" runat="server" Text='<%#Eval("LocationName").ToString()=="0"?"ALL":Eval("LocationName")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SubLocation">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSubLocation" runat="server" Text='<%#Eval("SubLocationName").ToString()=="0"?"ALL":Eval("SubLocationName")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Dealer Code">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDealerCode" runat="server" Text='<%#Eval("DLRCODE")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Dealer Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDealerName" runat="server" Text='<%#Eval("DLRName")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sender ID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblsender" runat="server" Text='<%#Eval("sender")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Submitted">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl3" runat="server" Text='<%#Eval("submitted")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delivered">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl4" runat="server" Text='<%#Eval("delivered")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Failed">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl5" runat="server" Text='<%#Eval("failed")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Unknown">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl6" runat="server" Text='<%#Eval("unknown")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>

                            <asp:GridView ID="grv3" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false" OnRowDataBound="grv2_RowDataBound"
                                runat="server" Width="100%" CellPadding="10" BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive nowrap dataTable-view">
                                <Columns>
                                    <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr No">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="Downloadxlx" class="btn btn-datatable btn-icon btn-transparent-dark px-2 py-0" data-toggle="tooltip" data-placement="top" title="download" OnClick="Downloadxlx_Click" Visible="false"><i class="fas fa-file-excel fa-sm text-success"></i></asp:LinkButton>
                                            &nbsp;
                                            <asp:LinkButton ID="LinkButton1" runat="server" OnClick="btnDownLoad_Click" class="btn btn-datatable btn-icon btn-transparent-dark px-2 py-0" data-toggle="tooltip" data-placement="top" title="download"><i class="fas fa-file-csv fa-sm text-success"></i></asp:LinkButton>
                                            <asp:LinkButton ID="btnViewFile" runat="server" OnClick="btnViewFile_Click" class="btn btn-datatable btn-icon btn-transparent-dark px-2 py-0" data-toggle="tooltip" data-placement="top" title="View"><i class="far fa-eye fa-sm text-success"></i></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Submit Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblsubmitdt" runat="server" Text='<%#Eval("date")%>'></asp:Label>
                                            <asp:HiddenField ID="hdnto" runat="server" Value='<%#Eval("userid")%>'></asp:HiddenField>
                                            <asp:HiddenField ID="hdnFrmDW" runat="server" Value='<%#Eval("fromdate")%>'></asp:HiddenField>
                                            <asp:HiddenField ID="hdnToDW" runat="server" Value='<%#Eval("todate")%>'></asp:HiddenField>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                   
                                    <asp:TemplateField HeaderText="Sender ID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblsender" runat="server" Text='<%#Eval("sender")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Submitted">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl3" runat="server" Text='<%#Eval("submitted")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Delivered">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl4" runat="server" Text='<%#Eval("delivered")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Failed">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl5" runat="server" Text='<%#Eval("failed")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Unknown">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl6" runat="server" Text='<%#Eval("unknown")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>

                    
                </div>
            </div>

            <!-- Content Row -->
            <div class="row d-none">
                <div class="col-xl-12 col-lg-12">
                    <!-- Basic Card Example -->
                    <div class="card bg-primary border-light shadow-soft mb-4">
                        <div class="card-header py-3 bg-primary d-flex justify-content-between flex-wrap align-content-center">
                            <h6 class="m-0 font-weight-bold my-auto">SMS Reports</h6>
                            <div class="right-view">
                                <asp:LinkButton runat="server" ID="LinkButton1" OnClick="btnUpdate_Click" class="btn btn-mini"> Show <i class="fas fa-eye" aria-hidden="true"></i></asp:LinkButton>
                                <asp:HiddenField ID="h1" runat="server" />
                                <asp:HiddenField ID="h2" runat="server" />
                            </div>
                        </div>
                        <div class="accordion" id="accordionExample">
                            <div class="card">
                                <div class="card-header" id="headingOne">
                                    <h2 class="mb-0">
                                        <button class="btn btn-link btn-block text-left" type="button" data-toggle="collapse" data-target="#collapseOne" aria-expanded="true" aria-controls="collapseOne">
                                            SMS Summary
                                        </button>
                                    </h2>
                                </div>

                                <div id="collapseOne" class="collapse show" aria-labelledby="headingOne" data-parent="#accordionExample" runat="server">
                                    <div class="card-body">
                                        <div class="row">
                                            <!-- Area Chart -->
                                            <div class="col-xl-12 col-lg-12">
                                                <!-- Loader -->
                                                <div class="o-page-loader">
                                                    <div class="o-page-loader--content">
                                                        <div class="o-page-loader--spinner"></div>
                                                        <div class="o-page-loader--message">
                                                            <span>Loading...</span>
                                                        </div>
                                                    </div>
                                                </div>
                                                <!--End Loader -->
                                                <div class="table-responsive">

                                                    <asp:GridView UseAccessibleHeader="true" ID="grv" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
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
                                                                    <asp:LinkButton ID="LinkButton2" OnClientClick=<%# "view('" + Eval("sender") + "','" + Eval("fileid") + "'); return false;" %> runat="server" class="btn btn-datatable btn-icon btn-transparent-dark px-2 py-0" data-toggle="tooltip" data-placement="top" title="Details"><i class="far fa-file-alt"></i></asp:LinkButton>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Submit Date Time" HeaderStyle-CssClass="text-wrap">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblmobile" runat="server" CssClass="text-wrap" Text='<%#Eval("submitdate")%>'></asp:Label>
                                                                    <asp:HiddenField ID="hdnFileId" runat="server" Value='<%#Eval("fileid")%>'></asp:HiddenField>
                                                                    <asp:HiddenField ID="hdnUserId" runat="server" Value='<%#Eval("userid")%>'></asp:HiddenField>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Source">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl0" runat="server" Text='<%#Eval("reqsrc")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Sender ID">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblsender" runat="server" Text='<%#Eval("sender")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Submitted">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl3" runat="server" Text='<%#Eval("submitted")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Delivered">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl4" runat="server" Text='<%#Eval("delivered")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Failed">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl5" runat="server" Text='<%#Eval("failed")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Unknown">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl6" runat="server" Text='<%#Eval("unknown")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="File Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl1" runat="server" Text='<%#Eval("filenm")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Message">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl7" runat="server" Text='<%#Eval("msg")%>'></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="card">
                                <div class="card-header" id="headingTwo">
                                    <h2 class="mb-0">
                                        <button class="btn btn-link btn-block text-left collapsed" type="button" data-toggle="collapse" data-target="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo">
                                            SMS Detail
                                        </button>
                                    </h2>
                                </div>
                                <div id="collapseTwo" class="collapse" aria-labelledby="headingTwo" data-parent="#accordionExample" runat="server">
                                    <div class="card-body">
                                        <div style="text-align: right;">
                                            <asp:LinkButton runat="server" ID="LinkButton3" OnClick="btnCloseDetail_Click" class="btn btn-mini">Close Detail</asp:LinkButton>
                                        </div>
                                        <div class="table-responsive">
                                            <table class="table table-striped table-bordered dt-responsive nowrap" id="rpttable" width="100%" cellspacing="0">
                                                <thead>
                                                    <tr>
                                                        <th>Sr. No</th>
                                                        <th>MSG ID</th>
                                                        <th>Mobile No</th>
                                                        <th>Sender Id</th>
                                                        <th>Submit Date Time</th>
                                                        <th>Status Date Time</th>
                                                        <th>Message</th>
                                                        <th>Status</th>
                                                        <th>Server Response</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="card-body">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </main>

    <%--lnkDetail Link Button for ModalPopup as TargetControlID--%>
    <asp:LinkButton ID="lnkDetail" runat="server"></asp:LinkButton>

    <!--================Start Section TEMPLATE FIND POPUP Show in Find==================================-->
    <asp:Panel ID="TEMPLATEFINDPOPUP" class="modalPopup1" runat="server" Style="display: none; width: -webkit-fill-available;">
        <div class=" modal-dialog  modal-xl ">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title mt-2" runat="server" id="HH">FIND TEMPLATE</h5>
                    <asp:LinkButton runat="server" ID="LinkButton4" class="close">
                                                         <span aria-hidden="true" style="padding:6px">&times;</span>
                    </asp:LinkButton>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-3 col-6">
                            <div class="form-group">
                                <label runat="server" id="lblTempId">TEMPLATE ID</label>
                                <div class="input-group">
                                    <asp:TextBox ID="txtTemplateId" runat="server" class="form-control" MaxLength="15" placeholder="Template Id" onkeypress="return /^[a-zA-Z0-9\s]{0,20}$/.test(this.value+event.key);"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-3 col-6">
                            <div class="form-group">
                                <label runat="server" id="lblTempName">TEMPLATE NAME</label>
                                <div class="input-group">
                                    <asp:TextBox ID="txtTempName" runat="server" class="form-control" MaxLength="15" placeholder="Template Name" onkeypress="return /^[a-zA-Z0-9\s]{0,20}$/.test(this.value+event.key);"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-1 col-2">
                            <br />
                            <asp:Button runat="server" ID="Filtertemp" Text="GO" class="btn btn-outline-primary" OnClick="Filtertemp_Click" />
                        </div>
                    </div>

                    <!--grid view Find-->
                    <div class="row mt-3">
                        <div class="table-responsive" style="height: 300px; overflow-y: scroll;">
                            <asp:GridView ID="TEMPLATE_FIND" runat="server" AutoGenerateColumns="false" HeaderStyle-BackColor="#F5F7F8" HeaderStyle-ForeColor="#707070" class="table table-striped table-bordered w-100 text-nowrap display">
                                <Columns>
                                    <asp:TemplateField HeaderText="TEMPLATE ID" ItemStyle-Width="30%">
                                        <ItemTemplate>
                                            <%#Eval("templateid") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="TEMPLATE NAME" ItemStyle-Width="30%">
                                        <ItemTemplate>
                                            <%#Eval("tempname") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SHOW" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:HiddenField Value='<%#Eval("templateid") %>' ID="HD_templateid" runat="server" />
                                            <asp:LinkButton ID="btn_Viewrecord" runat="server" OnClick="btn_Viewrecord_Click"><%--OnClick="btn_Viewrecord_Click1"--%>
                                                                      <center>  <i class="fa fa-solid fa-eye"> </i></center>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <div class="modal-footer p-2">
                    <asp:Button ID="Button4" runat="server" class="btn btn-outline-danger " Text="Close" UseSubmitBehavior="false"></asp:Button>
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:LinkButton runat="server" ID="LinkButton2"></asp:LinkButton>

    <asp:ModalPopupExtender runat="server" ID="modal_Find_TEMPLATE" TargetControlID="LinkButton2" PopupControlID="TEMPLATEFINDPOPUP" BackgroundCssClass="modalBackground">
    </asp:ModalPopupExtender>
    <!--====================END Section TEMPLATE_FIND FIND POPUP Show in Find==============================-->

    <%--pnlPopUp_Detail Panel With Design--%>
    <asp:Panel ID="pnlPopUp_Detail" runat="server" CssClass="modal modalPopup" Style="display: none;">
        <div class="modal-dialog modal-xl modal-dialog-centered modal-dialog-scrollable">
            <div class="modal-content">
                <div class="modal-header">
                    <asp:Label ID="lblHeading" runat="server" CssClass="modal-title" Text="Sender ID Wise Details"></asp:Label>
                </div>
                <div class="modal-body">
                    <div class="">
                        <asp:GridView ID="grvModal" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                            runat="server" Width="100%" CellPadding="10"
                            BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive nowrap dataTable-view">
                            <Columns>
                                <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr No">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDate" runat="server" Text='<%#Eval("SMSDATE")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Message Id">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUserId" runat="server" Text='<%#Eval("Messageid")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Mobile No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMobileNo" runat="server" Text='<%#Eval("MobileNo")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sender ID">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSenderId" runat="server" Text='<%#Eval("Sender")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sent Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblsentDate" runat="server" Text='<%#Eval("SentDate")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Delivered Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDeliveredDate" runat="server" Text='<%#Eval("DeliveredDate")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Message">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMessage" runat="server" Text='<%#Eval("Message")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Message State">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMessageState" runat="server" Text='<%#Eval("MessageState")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Response">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMessage" runat="server" Text='<%#Eval("RESPONSE")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div class="modal-footer">
                    <button id="btnCancel" runat="server" class="btn btn-primary">Close</button>
                </div>
            </div>
        </div>
    </asp:Panel>

    <%--pnlPopUp_Detail Modal Popup Extender For pnlPopUp_Detail--%>
    <cc1:ModalPopupExtender ID="pnlPopUp_Detail_ModalPopupExtender" runat="server" PopupControlID="pnlPopUp_Detail"
        TargetControlID="lnkDetail" BehaviorID="mpeAddUpdateEmployee" CancelControlID="btnCancel"
        BackgroundCssClass="modalBackground">
    </cc1:ModalPopupExtender>

    <!-- Bootstrap core JavaScript-->
    <script src="vendor/jquery/jquery-3.5.1.min.js"></script>
    <script src="vendor/bootstrap/js/bootstrap.bundle.min.js"></script>
    <script src="vendor/datepicker/bootstrap-datepicker.js"></script>


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
        $(function () {
            var pDate = new Date();
            pDate.setDate(pDate.getDate() - 45);
            var month1 = ('0' + (pDate.getMonth() + 1)).slice(-2);
            var day1 = ('0' + pDate.getDate()).slice(-2);
            var year1 = pDate.getFullYear();
            var date1 = year1 + '-' + month1 + '-' + day1;


            var today = new Date();
            today.setDate(today.getDate() - 1);
            var month = ('0' + (today.getMonth() + 1)).slice(-2);
            var day = ('0' + today.getDate()).slice(-2);
            var year = today.getFullYear();
            var date = year + '-' + month + '-' + day;

            $('[id*=txtFrm1]').attr('max', date);
            $('[id*=txtFrm1]').attr('min', date1);

            $('[id*=txtTo1]').attr('max', date);
            $('[id*=txtTo1]').attr('min', date1);

        });
    </script>

    <%--    <script>
            $(function () {
               $('.datepicker').datepicker({ defaultDate: -1, minDate:'-1d' });
            });
        </script>--%>

    <%--<script type="text/javascript">
        $(document).ready(function () {
            $('.datepicker').datepicker({ defaultDate: -1, minDate:'-1d' });
        });
    </script>--%>

    <%--<script type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

            function EndRequestHandler(sender, args) {
                $('.datepicker').datepicker({ dateFormat: 'dd-mm-yy' });

            }

        });
    </script>--%>
    <script type="text/javascript">  
        function view(senderid, fileid) {
            console.log(senderid);
            console.log(fileid);


            var today = new Date();
            var dd = String(today.getDate()).padStart(2, '0');
            var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
            var yyyy = today.getFullYear();

            var user = '<% =Session["UserID"].ToString() %>';
            var startDate = yyyy + '-' + mm + '-' + dd;
            var endDate = startDate + ' 23:59:59';

            console.log(startDate);
            console.log(endDate);
            console.log(user);

            $.ajax({
                type: "POST",
                dataType: "json",
                url: "WebService.asmx/GetSMSReportDetailUserNew",
                data: { dater: startDate + '$' + endDate + '$' + user + '$' + fileid + '$' + senderid },
                success: function (data) {
                    var datatableVariable = $('#rpttable').DataTable({
                        data: data,
                        columns: [
                            { 'data': 'sln' },
                            { 'data': 'msgid' },
                            { 'data': 'mobile' },
                            { 'data': 'senderid' },
                            { 'data': 'senttime' },
                            { 'data': 'dlrtime' },
                            { 'data': 'msgtext' },
                            { 'data': 'dlrstat' },
                            { 'data': 'dlrresp' }
                        ]
                    });
                }
            });
            console.log("done");
            document.getElementById('ContentPlaceHolder1_collapseOne').className = 'collapse';
            document.getElementById('ContentPlaceHolder1_collapseTwo').className = 'collapse show';
            console.log("done222");
        }

    </script>

    <%--<script type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

            function EndRequestHandler(sender, args) {
                $('.datepicker').datepicker({ dateFormat: 'dd-mm-yy' });
            }

        });
    </script>--%>

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

    <script>
        (function (loader) {

            window.addEventListener('beforeunload', function (e) {
                activateLoader();
            });

            window.addEventListener('load', function (e) {
                deactivateLoader();
            });

            function activateLoader() {
                loader.style.display = 'block';
                loader.style.opacity = 1;
            }

            function deactivateLoader() {
                /**
                 * ensures that the loading animation plays for at least a second to give the
                 * appearance of seamless loading on pages that execute and load extremely
                 * quickly (i.e., intranet pages)
                 */
                setTimeout(function () {
                    deactivate();
                }, 1000);

                function deactivate() {
                    loader.style.opacity = 0;
                    loader.addEventListener('transitionend', function () {
                        loader.style.display = 'none';
                    }, false);
                }
            }

        })(document.querySelector('.o-page-loader'));
    </script>

     <!-- Bootstrap core JavaScript-->
    <script src="vendor/jquery/jquery-3.5.1.min.js"></script>
    <!-- Select2 CSS -->
    <link href="css/select2.min.css" rel="stylesheet" />
    <!-- Select2 -->
    <script src="js/select2.min.js"></script>
    <script>
        $("#ContentPlaceHolder1_ddlCategory").select2({
            allowClear: true
        });

        $("#ContentPlaceHolder1_ddlLocation").select2({
            allowClear: true
        });

        $("#ContentPlaceHolder1_ddlSubLocation").select2({
            allowClear: true
        });

        $("#ddlDealerCode").select2({
            allowClear: true
        });

    </script>
</asp:Content>