<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="Users_Update_Setting.aspx.cs" Inherits="eMIMPanel.Users_Update_Setting" MaintainScrollPositionOnPostback="true" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/Site2.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <!-- Select2 CSS -->
    <link href="css/select2.min.css" rel="stylesheet" />

    <link href="ms-Dropdown-master/css/msdropdown/dd.css" rel="stylesheet" />
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>
    <script type="text/javascript" src="ms-Dropdown-master/js/jquery/jquery-1.9.0.min.js"></script>
    <script type="text/javascript" src="ms-Dropdown-master/js/msdropdown/jquery.dd.js"></script>

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

        .modalBackground {
            background-color: #000;
            opacity: 0.5;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="updFormArea" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
    <main>
        <div class="container-fluid">
            <!-- Content Row -->
            <div class="row">
                <div class="col-lg-10 col-xl-11">
                    <!-- Start Card -->
                    <div class="card bg-primary border-light shadow-soft mb-4">
                        <div class="card-header py-3 bg-primary d-flex justify-content-between flex-wrap align-content-center">
                            <h6 class="m-0 font-weight-bold font-weight-bold my-auto"><i class="fas fa-cog"></i>&nbsp;&nbsp;2FA for Linkext</h6>
                        </div>
                        <div class="card-body pt-0">
                            <!-- Start Card -->
                            <div class="form-group row">
                                <div class="col-sm-2">
                                </div>
                                <div class="col-sm-5">
                                    <asp:CheckBox ID="chkEnableOTP" runat="server" AutoPostBack="true" OnCheckedChanged="chkEnableOTP_CheckedChanged" />&nbsp;&nbsp;<label for="inputEmail3" class="col-form-label font-weight-bold">Enable 2FA for Linkext Access</label>
                                </div>
                                   <div class="col-sm-5" id="divlogintype" runat="server" visible="false">
                                       <asp:RadioButton ID="rdbOtpSMS" runat="server" Checked="true" GroupName="OTPTYPE" AutoPostBack="true" OnCheckedChanged="rdbOtpSMS_CheckedChanged"/>&nbsp;&nbsp;<label for="inputEmail3" class="col-form-label font-weight-bold">OTP on SMS</label>&nbsp;&nbsp;&nbsp;&nbsp;
                                       <asp:RadioButton ID="rdbOtpWABA" runat="server" GroupName="OTPTYPE" AutoPostBack="true" OnCheckedChanged="rdbOtpWABA_CheckedChanged" />&nbsp;&nbsp;<label for="inputEmail3" class="col-form-label font-weight-bold">OTP on WhatsApp</label>
                                   </div>
                            </div>
                            <div id="divSenderId" runat="server" visible="false" class="form-group row">
                                <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">Sender ID</label>
                                <div class="col-sm-5">
                                    <asp:DropDownList ID="ddlSender" runat="server" class="custom-select">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group row" id="divTempId" runat="server" visible="false" >
                                <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Template ID</label>
                                <div class="col-sm-10">
                                    <div id="div8" runat="server" style="pointer-events: all;">
                                        <div class="row">
                                            <div class="col-lg-12">
                                                <asp:DropDownList ID="ddlTempID" runat="server" ClientIDMode="Static" class="custom-select" OnSelectedIndexChanged="ddlTempID_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group row" id="divTempsms" runat="server" visible="false">
                                <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Text SMS: </label>
                                <div class="col-sm-10">
                                    <div id="div9" runat="server">
                                        <asp:TextBox ID="lblTempSMS" runat="server" TextMode="MultiLine" class="form-control" Rows="3" disabled="disabled"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group row" id="divPreview" runat="server" visible="false">
                                <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Text Preview: </label>
                                <div class="col-sm-10">
                                    <div id="div2" runat="server">
                                        <asp:TextBox ID="txtpreview" runat="server" TextMode="MultiLine" class="form-control" Rows="3" disabled="disabled"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="form-group row justify-content-end">
                                        <div class="col-sm-10">
                                            <div class="row">
                                                <div class="col-lg-3">
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:LinkButton ID="lnkUpdateDate" runat="server" class="btn btn-primary text-secondary btn-block" OnClick="lnkUpdateDate_Click">
                                                        <span class="text-secondary"> <i class="fas fa-save"></i> Update</span>
                                                    </asp:LinkButton>
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:LinkButton ID="LinkButton3" runat="server" class="btn btn-primary text-danger btn-block my-3 my-lg-0" OnClick="btnCancel_Click" data-toggle="tooltip" data-placement="top">
                                                        <span class="text-danger"> <i class="fas fa-times"></i> Cancel</span>
                                                    </asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
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
                        </div>
                    </div>
                    <!-- End Card -->

                    <!-- Start Card Report on Email -->
                    <div class="card bg-primary border-light shadow-soft mb-4">
                        <div class="card-header py-3 bg-primary d-flex justify-content-between flex-wrap align-content-center">
                            <h6 class="m-0 font-weight-bold font-weight-bold my-auto"><i class="fas fa-cog"></i>&nbsp;&nbsp;Report on Email</h6>
                        </div>
                        <div class="card-body pt-0">
                            <!-- Start Card -->
                            <div class="form-group row">
                                <div class="col-sm-2">
                                </div>
                                <div class="col-sm-10">
                                    <asp:CheckBox ID="chkEnableSendEmail" runat="server" />&nbsp;&nbsp;<label for="inputEmail3" class="col-form-label font-weight-bold">Send Previous Day SMS Report on Email</label>
                                </div>
                            </div>
                            <div class="form-group row">
                                <label for="inputEmail3" class="col-sm-12 col-form-label font-weight-bold">Report will be sent on following email id</label>
                            </div>
                            <div class="form-group row">
                                <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">To Email:</label>
                                <div class="col-sm-10">
                                       <asp:TextBox ID="txtEmailTo" runat="server" class="form-control" disabled="disabled"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group row">
                                <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">CC Email: </label>
                                <div class="col-sm-10">
                                      <asp:TextBox ID="txtEmailCC" runat="server" TextMode="MultiLine" class="form-control" Rows="2" disabled="disabled"></asp:TextBox>
                                </div>
                            </div>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="form-group row justify-content-end">
                                        <div class="col-sm-10">
                                            <div class="row">
                                                <div class="col-lg-3">
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:LinkButton ID="lnkbtnEmailUpdate" runat="server" class="btn btn-primary text-secondary btn-block" OnClick="lnkbtnEmailUpdate_Click">
                                                        <span class="text-secondary"> <i class="fas fa-save"></i> Update</span>
                                                    </asp:LinkButton>
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:LinkButton ID="lnkbtnEmailCancel" runat="server" class="btn btn-primary text-danger btn-block my-3 my-lg-0" OnClick="btnCancel_Click" data-toggle="tooltip" data-placement="top">
                                                        <span class="text-danger"> <i class="fas fa-times"></i> Cancel</span>
                                                    </asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                            <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="0">
                                <ProgressTemplate>
                                    <div align="center" style="position: absolute; left: 40%; top: 50%; vertical-align: middle;">
                                        <img src="Img/LOADING.GIF" />
                                    </div>
                                    <div align="center" style="position: absolute; left: 0%; top: 0%; vertical-align: middle;">
                                    </div>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </div>
                    </div>
                    <!-- End Card -->
                </div>
            </div>
            <!-- End Row -->
        </div>
    </main>
    </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="chkEnableOTP" />
        </Triggers>
  </asp:UpdatePanel>

    
    <!-- Select2 -->
    <script src="js/select2.min.js"></script>
    <script>
            $("#ddlTempID").select2({
                allowClear: true
            });

            $("#ddlTempID").select2({
                allowClear: true
            });
    </script>
</asp:Content>
