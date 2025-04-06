<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="send-sms-u-B4Send.aspx.cs" Inherits="eMIMPanel.send_sms_u_B4Send" MaintainScrollPositionOnPostback="true" EnableEventValidation="false" %>

<%@ MasterType VirtualPath="~/Site2.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <style type="text/css">
        /*CSS Classes For Design Modal*/
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

        .select2-dropdown {
            background-color: #e6e7ee !important;
            border: 1px solid #aaa;
        }

        .modalPopup {
            min-height: 75px;
            position: fixed;
            z-index: 2000;
            padding: 0;
            background-color: #fff;
            border-radius: 6px;
            background-clip: padding-box;
            border: 1px solid rgba(0, 0, 0, 0.2);
            min-width: 295px;
            box-shadow: 0 5px 10px rgba(0, 0, 0, 0);
        }

        .modalPopup1 {
            min-height: 75px;
            position: fixed;
            z-index: 2000;
            padding: 0;
            background-color: #fff;
            border-radius: 6px;
            background-clip: padding-box;
            border: 1px solid rgba(0, 0, 0, 0.2);
            min-width: 800px;
            box-shadow: 0 5px 10px rgba(0, 0, 0, 0);
        }

        .modalBackground {
            position: fixed;
            top: 0;
            left: 0;
            background-color: #000;
            opacity: 0.5;
            z-index: 1800;
            min-height: 100%;
            width: 100%;
            overflow: hidden;
            filter: alpha(opacity=50);
            display: inline-block;
            z-index: 1000;
        }

        /*  */

        .select2-container .select2-selection--single .select2-selection__rendered {
            padding-left: 0;
            padding-right: 0;
            height: auto;
            margin-top: -3px
        }

        .select2-container--default .select2-selection--single,
        .select2-selection .select2-selection--single {
            border: 1px solid #d2d6de;
            border-radius: 5px !important;
            padding: 6px 12px;
            height: 40px !important
        }

            .select2-container--default .select2-selection--single .select2-selection__arrow {
                height: 26px;
                position: absolute;
                top: 6px !important;
                right: 1px;
                width: 20px
            }


        /* Base styles for Bootstrap tooltip */
        .tooltip {
            opacity: 1 !important;
        }

            /* Custom wide-tooltip class */
            .tooltip.wide-tooltip .tooltip-inner {
                max-width: none;
                width: 300px;
                background-color: #f0f0f0;
                color: #333;
                border-radius: 12px;
                padding: 10px;
                box-shadow: 5px 5px 10px #bebebe, -5px -5px 10px #ffffff;
                padding-right: 35px !important;
                width: auto;
                white-space: nowrap;
                text-align: left;
            }

            .tooltip.wide-tooltip .tooltip-arrow {
                border-color: #f0f0f0;
            }


        #info {
            position: relative;
            left: -10px;
        }
    </style>
    <style type="text/css">
        .callout {
            position: fixed;
            bottom: 35px;
            right: 20px;
            margin-left: 25px;
            max-width: 400px;
        }

        .callout-header {
            padding: 47px 41px 6px 12px;
            background: #555;
            font-size: 25px;
            color: white;
        }

        .callout-container {
            padding: 15px;
            background-color: #ccc;
            color: black
        }

        .closebtn {
            position: absolute;
            right: 15px;
            color: red;
            font-size: 35px;
            cursor: pointer;
        }

            .closebtn:hover {
                color: lightgrey;
            }
    </style>
    <link href="ms-Dropdown-master/css/msdropdown/dd.css" rel="stylesheet" />
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>
    <script type="text/javascript" src="ms-Dropdown-master/js/jquery/jquery-1.9.0.min.js"></script>
    <script type="text/javascript" src="ms-Dropdown-master/js/msdropdown/jquery.dd.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </asp:ToolkitScriptManager>
    <main>
        <div class="container-fluid">
            <!-- Content Row -->
            <div class="row">
                <div class="col-lg-10 col-xl-11">
                    <!-- Start Card -->
                    <div class="card bg-primary border-light shadow-soft mb-4">
                        <div class="card-header py-3 bg-primary d-flex justify-content-between flex-wrap align-content-center">
                            <h6 class="m-0 font-weight-bold font-weight-bold my-auto"><i class="fas fa-comment-alt"></i>Send SMS </h6>
                        </div>
                        <div class="card-body pt-0">
                            <!-- Start Card -->
                            <div id="divFileLoader" runat="server" style="display: none; text-align: center" class="form-group row">
                                <h3>File uploading. Please wait...</h3>
                                <img src="img/loading.gif" />
                            </div>
                            <div id="div10" runat="server" class="form-group row">
                                <label for="inputEmail33" class="col-sm-2 col-form-label font-weight-bold" style="margin-top: -10px;">Country Code</label>
                                <div class="col-md-5">
                                    <div id="divmobile" runat="server" class="form-label-group">
                                        <asp:DropDownList ID="ddlCCode" runat="server" class="custom-select" AutoPostBack="true" OnSelectedIndexChanged="ddlCCode_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="5">
                                </div>
                            </div>
                            <div class="form-group row">
                                <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">SMS Type</label>
                                <div class="col-sm-5">
                                    <asp:DropDownList ID="ddlSMSType" runat="server" class="custom-select" AutoPostBack="true" OnSelectedIndexChanged="ddlSMSType_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-sm-5 pt-3">
                                    <i class="fas fa-info-circle custom-tooltip" id="info" data-toggle="tooltip" data-html="true" data-custom-class="wide-tooltip" title="Premium - For General SMS without Tracking Link.<br/><br/>Link Text - For SMS having link for tracking.<br/><br/>Promotional - For Promotional SMS (Numeric SenderID) with or without Link"></i>
                                    <asp:Label ID="lblRate" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group row">
                                <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">Sender ID</label>
                                <div class="col-sm-5">
                                    <asp:DropDownList ID="ddlSender" runat="server" class="custom-select" AutoPostBack="true" OnSelectedIndexChanged="ddlSender_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group row" id="DivDealerName" runat="server" visible="false">
                                <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">Dealer Name</label>
                                <div class="col-sm-5">
                                    <asp:DropDownList ID="ddldlrname" runat="server" class="custom-select">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group row" id="divcompName" runat="server" visible="false">
                                <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">Insurance Company Name</label>
                                <div class="col-sm-5">
                                    <asp:DropDownList ID="ddlcompname" runat="server" class="custom-select">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group row">
                                <legend class="col-form-label col-sm-2 pt-0 font-weight-bold">Mobile Numbers</legend>
                                <div class="col-sm-10">
                                    <div class="custom-control custom-radio custom-control-inline pl-2">
                                        <asp:RadioButton class="mr-2" ID="rdbEntry" runat="server" Checked="true" GroupName="mobile" AutoPostBack="true" OnCheckedChanged="rdbUpload_CheckedChanged" />
                                        <label>Enter Number <i class="far fa-keyboard text-dark"></i></label>
                                    </div>
                                    <div class="custom-control custom-radio custom-control-inline pl-2">
                                        <asp:RadioButton class="mr-2" ID="rdbUpload" runat="server" GroupName="mobile" AutoPostBack="true" OnCheckedChanged="rdbUpload_CheckedChanged" />
                                        <label>
                                            Upload Files <i class="fas fa-file-csv text-primary"></i>
                                            <br>
                                            <small>(CSV/TXT)</small></label>
                                    </div>
                                    <div class="custom-control custom-radio custom-control-inline pl-2">
                                        <asp:RadioButton class="mr-2" ID="rdbPersonal" runat="server" GroupName="mobile" AutoPostBack="true" OnCheckedChanged="rdbUpload_CheckedChanged" />
                                        <label>Personalized SMS Upload CSV <i class="fa fa-file-csv fa-1x text-success"></i></label>
                                    </div>
                                    <div class="custom-control custom-radio custom-control-inline pl-2">
                                        <asp:RadioButton class="mr-2" ID="rdbImport" runat="server" GroupName="mobile" AutoPostBack="true" OnCheckedChanged="rdbUpload_CheckedChanged" />
                                        <label>
                                            <asp:LinkButton ID="lnkbtnimport" runat="server" Enabled="false" Text="Import From Group" OnClick="lnkbtnimport_Click">
                                            </asp:LinkButton><i class="fas fa-users text-info"></i></label>
                                    </div>
                                </div>
                            </div>
                            <div id="divFileUpload" runat="server" class="form-group row d-none">
                                <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold" style="margin-top: -10px;">Mobile Numbers File&nbsp;</label>
                                <div class="col-md-6">
                                    <div class="custom-file">
                                        <asp:FileUpload ID="FileUpload1" runat="server" accept=".csv,.txt" class="custom-file-input" ClientIDMode="Static" onchange="if( SMSfileUpload() ) { console.log('formsubmit'); this.form.submit(); }" />
                                        <label class="custom-file-label" for="customFile">Choose file</label>
                                        <p class="text-danger small mt-2 mb-0">Maximum File Size 6 MB</p>
                                    </div>
                                </div>
                                <div class="4">
                                    <asp:Label ID="lblUploading" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div id="divFileUpload3" runat="server" class="form-group row d-none">
                                <label for="inputEmail31" class="col-sm-2 col-form-label font-weight-bold" style="margin-top: -10px;">Exclude Mobile Numbers File&nbsp;</label>
                                <div class="col-md-6">
                                    <div class="custom-file">
                                        <asp:FileUpload ID="FileUpload3" runat="server" class="custom-file-input" ClientIDMode="Static" onchange="if( SMSfileUpload3() ) { console.log('formsubmit'); this.form.submit(); }" />
                                        <label class="custom-file-label" for="customFile">Choose file</label>
                                        <p class="text-danger small mt-2 mb-0">Maximum File Size 6 MB</p>
                                    </div>
                                </div>
                                <div class="4">
                                    <asp:Label ID="lblUploading3" runat="server"></asp:Label>
                                </div>
                            </div>

                            <div id="divCamp" runat="server" class="form-group row d-none">
                                <label for="exampleFormControlTextarea1" class="col-sm-2 col-form-label font-weight-bold">Campaign Name</label>
                                <div class="col-md-6">
                                    <asp:TextBox class="form-control" ID="txtCampNm" MaxLength="50" runat="server" placeholder="Campaign Name" ToolTip="Campaign Name" />
                                </div>
                                <label id="lblEvents" runat="server" visible="false" for="exampleFormControlTextarea1" class="col-sm-1 col-form-label font-weight-bold">Events</label>
                                <div class="col-md-3" id="DivEvents" runat="server" visible="false">
                                    <asp:DropDownList ID="ddlEvents" runat="server" class="custom-select">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group row" id="divEnterMobNum" runat="server">
                                <label for="exampleFormControlTextarea1" class="col-sm-2 col-form-label font-weight-bold">Total Numbers</label>
                                <div class="col-sm-10">
                                    <div id="divNum" runat="server" style="pointer-events: all;">
                                        <asp:TextBox ID="txtMobNum" runat="server" class="form-control" TextMode="MultiLine" Rows="5"
                                            MaxLength="2147483647" onkeyup="integersOnly(this); mobnumbcnt(); return true;" placeholder="Enter Mobile Numbers"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="col-sm-2">
                                </div>
                                <div class="col-sm-10">
                                    <div class="d-flex">
                                        <p class="my-2 mr-3">
                                            <asp:CheckBox ID="chkAllowDuplicates" runat="server" onchange="updateCounts();" />
                                            <label class="form-check-label" for="gridCheck2">Allow Duplicates </label>
                                        </p>
                                    </div>
                                    <div class="d-flex flex-md-row flex-wrap justify-content-between">
                                        <p class="my-2">
                                            <span class="font-weight-bold small">Uploaded:</span>
                                            <asp:Label ID="lblMobTotalCount" runat="server" class="small" Font-Size="Large"></asp:Label>
                                        </p>
                                        <p class="my-2">
                                            <span class="font-weight-bold small">Duplicate:</span>
                                            <asp:LinkButton ID="lblMobDuplicateCount" runat="server" ForeColor="Blue" Font-Size="Large" ClientIDMode="Static" OnClick="lblMobDuplicateCountNoti_Click"></asp:LinkButton>
                                        </p>
                                        <p class="my-2">
                                            <span class="font-weight-bold small">Incorrect:</span>
                                            <asp:LinkButton ID="lblMobIncorrectCount" runat="server" ForeColor="Blue" Font-Size="Large" OnClick="lblMobIncorrectCountNoti_Click"></asp:LinkButton>
                                        </p>
                                        <p class="my-2">
                                            <span class="font-weight-bold small">Excluded Numbers:</span>
                                            <asp:Label ID="lblExcludeCnt" runat="server" class="small" Font-Size="Large"></asp:Label>
                                        </p>
                                        <p class="my-2">
                                            <span class="font-weight-bold small">SMS to be Sent On:</span>
                                            <asp:Label ID="lblMobileCnt" runat="server" class="small" ClientIDMode="Static" Font-Size="Large"></asp:Label>
                                        </p>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="col-lg-6">
                                </div>
                                <div class="col-lg-6">
                                    <div class="row">
                                        <div class="col-lg-6">
                                            <asp:LinkButton ID="LinkButton1" Visible="false" runat="server" class="btn btn-primary text-secondary small btn-block my-3 my-lg-0"
                                                OnClick="btnInsURL_Click">
                                             <span class="text-secondary small"> <i class="fas fa-fw fa-link text-secondary"></i> Insert Short URL </span>
                                            </asp:LinkButton>
                                        </div>
                                        <div class="col-lg-6">
                                            <asp:LinkButton ID="btnInsTemplate" runat="server" class="btn btn-primary text-secondary small btn-block my-3 my-lg-0"
                                                ToolTip="Click only for Personalized SMS" OnClick="btnInsTemplate_Click" Visible="false">
                                            <span class="text-secondary small"> <i class="fas fa-fw fa-redo text-secondary"></i> Insert Template </span>
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div id="divTemplateList" runat="server" style="display: none;">
                                <div id="div1" runat="server" class="form-group row">
                                    <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Select Template</label>
                                    <div class="col-sm-7">
                                        <asp:DropDownList ID="ddlTemplate" runat="server" ClientIDMode="Static" class="custom-select"
                                            OnSelectedIndexChanged="ddlTemplate_SelectedIndexChanged" AutoPostBack="true" Width="97%">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-sm-3" style="margin: auto;">
                                        <asp:CheckBox ID="chkAutoMapping" runat="server" Enabled="false" AutoPostBack="true" OnCheckedChanged="chkAutoMapping_CheckedChanged" />
                                        <label class="form-check-label" for="gridCheck2">Auto Map Template Variables</label>
                                    </div>
                                </div>

                                <div id="divBindHeader" runat="server" class="form-group row">
                                    <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold"></label>
                                    <div class="col-sm-4">
                                        <label for="exampleFormControlTextarea2" class="col-form-label font-weight-bold">Template Fields</label>
                                    </div>
                                    <div class="col-sm-4">
                                        <label for="exampleFormControlTextarea2" class="col-form-label font-weight-bold">Uploaded XLS Fields</label>
                                    </div>
                                </div>

                                <div id="divBindBody" runat="server" class="form-group row">
                                    <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold"></label>
                                    <div class="col-sm-4">
                                        <asp:UpdatePanel ID="updFormArea" runat="server" UpdateMode="Conditional">
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="LstTemplateFld" />
                                            </Triggers>
                                            <ContentTemplate>
                                                <asp:ListBox ID="LstTemplateFld" Rows="5" runat="server" class="custom-select"></asp:ListBox>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                    <div class="col-sm-4">
                                        <asp:ListBox ID="LstXLSFld" Rows="5" runat="server" class="custom-select"></asp:ListBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:LinkButton ID="btnMap" runat="server" class="mx-0 btn btn-primary text-secondary small" OnClick="btnMap_Click">
                                             <span class="text-secondary" style="font-size:smaller;"> <i class="text-secondary"></i>Bind</span>
                                        </asp:LinkButton>
                                    </div>
                                </div>

                                <div id="divBreak" runat="server" class="form-group row">
                                    <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Mapped Fields</label>
                                    <div class="col-sm-8">
                                        <asp:ListBox ID="lstMappedFields" Rows="5" runat="server" class="custom-select"></asp:ListBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:LinkButton ID="btnUnMap" runat="server" class="mx-0 btn btn-primary text-secondary small" OnClick="btnUnMap_Click">
                                            <span class="text-secondary" style="font-size:smaller; margin-top:20px;"> <i class="text-danger"></i>Break</span>
                                        </asp:LinkButton>
                                    </div>
                                </div>

                                <div id="div6" runat="server" class="form-group row">
                                    <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Message Preview</label>
                                    <div class="col-sm-10">
                                        <asp:Literal ID="txtPreview" runat="server"></asp:Literal>
                                        <asp:HiddenField ID="IsPersalisedSMS" runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                            </div>

                            <div class="form-group row">
                                <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">SMS Text</label>
                                <div class="col-sm-10">
                                    <div id="divMsg" runat="server" style="pointer-events: all;">
                                        <asp:TextBox ID="txtMsg" runat="server" TextMode="MultiLine" class="form-control" Rows="5" onkeyup="smscnt(); return true;"></asp:TextBox>
                                        <div id="divOptOut" runat="server" style="display: block;">
                                            <asp:CheckBox ID="chkOptOut" runat="server" Text="Include - " AutoPostBack="true" OnCheckedChanged="chkOptOut_CheckedChanged" onchange="smscnt(); return true;" />
                                            <asp:Label ID="lblOptOut" runat="server" Text="DND7726"></asp:Label>
                                        </div>
                                        <div id="divUAEDNDUser" runat="server" style="display: none;">
                                            <asp:Label ID="Label4" runat="server" Text="Use : DND7726"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="d-flex justify-content-between">
                                        <p class="my-2">
                                            <span class="font-weight-bold">Used :</span>
                                            <asp:Label ID="lblused" runat="server" Text="0"></asp:Label>
                                        </p>
                                        <p class="my-2">
                                            <span class="font-weight-bold">SMS Count :</span>
                                            <asp:Label ID="lblsmscnt" runat="server" Text="0"></asp:Label>
                                        </p>
                                        <p class="my-2">
                                            <asp:Label ID="lblUniCode" runat="server" Text="" Style="color: red; font-weight: bolder;"></asp:Label>
                                        </p>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group row" id="divTempId" runat="server">
                                <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Template ID</label>
                                <div class="col-sm-10">
                                    <div id="div8" runat="server" style="pointer-events: all;">
                                        <div class="row">
                                            <div class="col-lg-12">
                                                <asp:DropDownList ID="ddlTempID" runat="server" ClientIDMode="Static" class="custom-select"
                                                    OnSelectedIndexChanged="ddlTempID_SelectedIndexChanged" AutoPostBack="true" Width="97%">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group row" id="divTempsms" runat="server">
                                <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Template SMS </label>
                                <div class="col-sm-10">
                                    <div id="div9" runat="server">
                                        <asp:Literal ID="lblTempSMS" runat="server"></asp:Literal>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group row" id="divscratchcard" runat="server" visible="false">
                                <div class="col-sm-2"></div>
                                <div class="col-sm-10">
                                    <div class="d-flex flex-md-row flex-wrap justify-content-between">
                                        <p class="my-2 mr-3">
                                            <asp:CheckBox ID="chkScratchCard" runat="server" />
                                            <label class="form-check-label" for="chkScratchCard">Send Scratch Card </label>
                                        </p>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group row">
                                <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Captcha </label>
                                <div class="col-sm-10">
                                    <div class="form-group">
                                        <div class="input-group mb-4">
                                            <cc1:CaptchaControl ID="captcha1" runat="server" CaptchaBackgroundNoise="Extreme" CaptchaLength="5"
                                                CaptchaHeight="45" CaptchaWidth="200" CaptchaMinTimeout="5" CaptchaMaxTimeout="240"
                                                FontColor="#D20B0C" NoiseColor="#B1B1B1" />
                                            <asp:ImageButton ImageUrl="~/img/refresh.png" OnClientClick="refreshCaptcha();return false;" runat="server" CausesValidation="false" />
                                            <asp:TextBox ID="txtCaptcha" runat="server" class="form-control form-control-user" placeholder="Enter Captcha"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="form-group row justify-content-end">
                                        <div class="col-sm-10">
                                            <div class="row">
                                                <div class="col-lg-3">
                                                    <asp:LinkButton ID="LinkButton2" runat="server" class="btn btn-primary text-secondary btn-block" OnClientClick="return ConfirmSubmit();" OnClick="btnSend_Click" data-toggle="tooltip" data-placement="top">
                                                        <span class="text-secondary"> <i class="fas fa-paper-plane"></i> Send</span>
                                                    </asp:LinkButton>
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:LinkButton ID="LinkButton3" runat="server" class="btn btn-primary text-danger btn-block my-3 my-lg-0" OnClick="btnCancel_Click" data-toggle="tooltip" data-placement="top">
                                                        <span class="text-danger"> <i class="fas fa-times"></i> Cancel</span>
                                                    </asp:LinkButton>
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:LinkButton ID="LinkButton5" runat="server" class="btn btn-primary text-warning btn-block" OnClick="btnSchedule_Click" data-toggle="tooltip" data-placement="top">
                                                        <span class="text-warning"> <i class="fas fa-clock"></i> Schedule</span>
                                                    </asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <!--footer-->
                            <div id="divFooter" runat="server">
                                <div class="card card-body mb-4 bg-primary border-light shadow-inset">
                                    <div class=" justify-content-between align-items-center text-center">
                                        <div class="text-danger">
                                            <strong>
                                                <asp:Label ID="lblNotice" runat="server"></asp:Label></strong>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!--End-->

                            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="0">
                                <ProgressTemplate>
                                    <div align="center" style="position: absolute; left: 40%; top: 50%; vertical-align: middle;">
                                        <img src="Img/LOADING.GIF" />
                                    </div>
                                    <div align="center" style="position: absolute; left: 0%; top: 0%; vertical-align: middle;">
                                    </div>
                                </ProgressTemplate>
                            </asp:UpdateProgress>

                            <div class="form-group row justify-content-end">
                                <div class="col-sm-10 ">
                                    <h2>
                                        <asp:Label ID="lblStat" runat="server" Visible="false"></asp:Label></h2>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- End Card -->

                    <!--Start Notifacation Alert -->
                    <div id="notificationAlert" runat="server" class="callout" style="display: none;">
                        <asp:Button ID="btnNotificationAlert" runat="server" CssClass="closebtn" Text="x" OnClick="btnNotificationAlert_Click" />
                        <div class="callout-header">Upload File Notification</div>
                        <div class="callout-container">
                            <table style="width: 85%;">
                                <tr>
                                    <td style="text-align: left">Uploaded: 
                                    </td>
                                    <td style="text-align: right">
                                        <asp:LinkButton ID="lblMobTotalCountNoti" runat="server"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left">Duplicate: 
                                    </td>
                                    <td style="text-align: right">
                                        <asp:LinkButton ID="lblMobDuplicateCountNoti" runat="server" ForeColor="Blue" OnClick="lblMobDuplicateCountNoti_Click" Font-Underline="true"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left">Incorrect:
                                    </td>
                                    <td style="text-align: right">
                                        <asp:LinkButton ID="lblMobIncorrectCountNoti" runat="server" ForeColor="Blue" OnClick="lblMobIncorrectCountNoti_Click" Font-Underline="true"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left">SMS to be Sent On:
                                    </td>
                                    <td style="text-align: right">
                                        <asp:LinkButton ID="lblMobileCntNotI" runat="server"></asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <!--END Notifacation Alert -->
                </div>
            </div>
            <!-- End Row -->
        </div>
    </main>

    <div id="dialogoverlay"></div>

    <div id="dialogbox">
        <div>
            <div id="dialogboxhead"></div>
            <div id="dialogboxbody"></div>
            <div id="dialogboxfoot"></div>
        </div>
    </div>

    <%--  Schedule POPUP --%>
    <asp:Panel ID="pnlPopUp_SCHEDULE" runat="server" CssClass="modalPopup1" Style="display: none;">
        <div style="overflow-y: auto; overflow-x: hidden; max-height: 80%;">
            <div class="modal-header">
                <asp:Label ID="Label2" runat="server" CssClass="modal-title" Text="Schedule SMS"></asp:Label>
            </div>
            <div class="modal-body" style="overflow-y: auto; height: 70vh;">
                <div>
                    <asp:HiddenField ID="hdnScheduleCount" runat="server" Value="1" ClientIDMode="Static" />
                    <div class="row">
                        <div class="col-md-5">
                            Schedule Date 1: 
                            <asp:Label ID="lblScheduleDate" Visible="false" runat="server" ClientIDMode="Static"></asp:Label>
                            <asp:TextBox ID="txtScheduleDate" runat="server" onchange="javascript:text_changed_from();" class="form-control datepicker " placeholder="Scheduled Date"></asp:TextBox>
                            <asp:HiddenField ID="hdnScheduleDate" runat="server" />
                        </div>
                        <div class="col-md-4">
                            Time (HH:MM) :
                                            <asp:TextBox ID="txtTime" ToolTip="Enter time in HH:MM format. Entered time will not be deleted. You can overwrite the time." onkeypress="return false" onkeyup="return ValTime();" OnPaste="return false" runat="server" MaxLength="6" Width="35%" Enabled="true"
                                                class="form-control"></asp:TextBox>
                            <asp:MaskedEditExtender ID="MaskedEditExtendertxtTime" runat="server" TargetControlID="txtTime" Mask="99:99" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Time" AcceptAMPM="false" ErrorTooltipEnabled="True" />
                        </div>
                        <div class="col-md-2">
                            <button type="button" onclick="SetSchedule1()" class="btn btn-primary btn-icon-split"><span class="text-success"><i class="fas fa-plus"></i></span></button>
                        </div>
                    </div>
                    <div class="row" style="display: none" id="divSchedule1" runat="server" clientidmode="Static">
                        <asp:HiddenField ID="hdnSchedule1" runat="server" ClientIDMode="Static" />
                        <div class="col-md-5">
                            Schedule Date 2:
                            <asp:Label ID="lblScheduleDate1" Visible="false" runat="server" ClientIDMode="Static"></asp:Label>
                            <asp:TextBox ID="txtScheduleDate1" runat="server" onchange="javascript:text_changed_from1();" class="form-control datepicker " placeholder="Scheduled Date"></asp:TextBox>
                            <asp:HiddenField ID="hdnScheduleDate1" runat="server" />
                        </div>
                        <div class="col-md-4">
                            Time (HH:MM) :
                                            <asp:TextBox ID="txtTime1" ToolTip="Enter time in HH:MM format. Entered time will not be deleted. You can overwrite the time." onkeypress="return false" onkeyup="return ValTime1();" OnPaste="return false" runat="server" MaxLength="6" Width="35%" Enabled="true"
                                                class="form-control"></asp:TextBox>
                            <asp:MaskedEditExtender ID="MaskedEditExtendertxtTime1" runat="server" TargetControlID="txtTime1" Mask="99:99" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Time" AcceptAMPM="false" ErrorTooltipEnabled="True" />
                        </div>
                        <div class="col-md-2">
                            <button type="button" onclick="SetSchedule2()" class="btn btn-primary btn-icon-split"><span class="text-success"><i class="fas fa-plus"></i></span></button>
                        </div>
                    </div>
                    <div class="row" style="display: none" id="divSchedule2" runat="server" clientidmode="Static">
                        <asp:HiddenField ID="hdnSchedule2" runat="server" ClientIDMode="Static" />
                        <div class="col-md-5">
                            Schedule Date 3:<asp:Label ID="lblScheduleDate2" Visible="false" runat="server" ClientIDMode="Static"></asp:Label>
                            <asp:TextBox ID="txtScheduleDate2" runat="server" onchange="javascript:text_changed_from2();" class="form-control datepicker " placeholder="Scheduled Date"></asp:TextBox>
                            <asp:HiddenField ID="hdnScheduleDate2" runat="server" />
                        </div>
                        <div class="col-md-4">
                            Time (HH:MM) :
                                            <asp:TextBox ID="txtTime2" ToolTip="Enter time in HH:MM format. Entered time will not be deleted. You can overwrite the time." onkeypress="return false" onkeyup="return ValTime2();" OnPaste="return false" runat="server" MaxLength="6" Width="35%" Enabled="true"
                                                class="form-control"></asp:TextBox>
                            <asp:MaskedEditExtender ID="MaskedEditExtendertxtTime2" runat="server" TargetControlID="txtTime2" Mask="99:99" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Time" AcceptAMPM="false" ErrorTooltipEnabled="True" />
                        </div>
                        <div class="col-md-2">
                            <button type="button" onclick="SetSchedule3()" class="btn btn-primary btn-icon-split"><span class="text-success"><i class="fas fa-plus"></i></span></button>
                        </div>
                    </div>
                    <div class="row" style="display: none" id="divSchedule3" runat="server" clientidmode="Static">
                        <asp:HiddenField ID="hdnSchedule3" runat="server" ClientIDMode="Static" />
                        <div class="col-md-5">
                            Schedule Date 4:<asp:Label ID="lblScheduleDate3" Visible="false" runat="server" ClientIDMode="Static"></asp:Label>
                            <asp:TextBox ID="txtScheduleDate3" runat="server" onchange="javascript:text_changed_from3();" class="form-control datepicker" placeholder="Scheduled Date"></asp:TextBox>
                            <asp:HiddenField ID="hdnScheduleDate3" runat="server" />
                        </div>
                        <div class="col-md-4">
                            Time (HH:MM) :
                                            <asp:TextBox ID="txtTime3" ToolTip="Enter time in HH:MM format. Entered time will not be deleted. You can overwrite the time." onkeypress="return false" onkeyup="return ValTime3();" OnPaste="return false" runat="server" MaxLength="6" Width="35%" Enabled="true"
                                                class="form-control"></asp:TextBox>
                            <asp:MaskedEditExtender ID="MaskedEditExtendertxtTime3" runat="server" TargetControlID="txtTime3" Mask="99:99" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Time" AcceptAMPM="false" ErrorTooltipEnabled="True" />
                        </div>
                        <div class="col-md-2">
                            <button type="button" onclick="SetSchedule4()" class="btn btn-primary btn-icon-split"><span class="text-success"><i class="fas fa-plus"></i></span></button>
                        </div>
                    </div>
                    <div class="row" style="display: none" id="divSchedule4" runat="server" clientidmode="Static">
                        <asp:HiddenField ID="hdnSchedule4" runat="server" ClientIDMode="Static" />
                        <div class="col-md-5">
                            Schedule Date 5:<asp:Label ID="lblScheduleDate4" runat="server" ClientIDMode="Static"></asp:Label>
                            <asp:TextBox ID="txtScheduleDate4" runat="server" onchange="javascript:text_changed_from4();" class="form-control datepicker" placeholder="Scheduled Date"></asp:TextBox>
                            <asp:HiddenField ID="hdnScheduleDate4" runat="server" />
                        </div>
                        <div class="col-md-4">
                            Time (HH:MM) :
                                            <asp:TextBox ID="txtTime4" ToolTip="Enter time in HH:MM format. Entered time will not be deleted. You can overwrite the time." onkeypress="return false" onkeyup="return ValTime4();" OnPaste="return false" runat="server" MaxLength="6" Width="35%" Enabled="true"
                                                class="form-control"></asp:TextBox>
                            <asp:MaskedEditExtender ID="MaskedEditExtendertxtTime4" runat="server" TargetControlID="txtTime4" Mask="99:99" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Time" AcceptAMPM="false" ErrorTooltipEnabled="True" />
                        </div>
                        <div class="col-md-2">
                            <button type="button" onclick="SetSchedule5()" class="btn btn-primary btn-icon-split"><span class="text-success"><i class="fas fa-plus"></i></span></button>
                        </div>
                    </div>
                    <div class="row" style="display: none" id="divSchedule5" runat="server" clientidmode="Static">
                        <asp:HiddenField ID="hdnSchedule5" runat="server" ClientIDMode="Static" />
                        <div class="col-md-5">
                            Schedule Date 6:<asp:Label ID="lblScheduleDate5" runat="server" ClientIDMode="Static"></asp:Label>
                            <asp:TextBox ID="txtScheduleDate5" runat="server" onchange="javascript:text_changed_from5();" class="form-control datepicker" placeholder="Scheduled Date"></asp:TextBox>
                            <asp:HiddenField ID="hdnScheduleDate5" runat="server" />
                        </div>
                        <div class="col-md-4">
                            Time (HH:MM) :
                                            <asp:TextBox ID="txtTime5" ToolTip="Enter time in HH:MM format. Entered time will not be deleted. You can overwrite the time." onkeypress="return false" onkeyup="return ValTime5();" OnPaste="return false" runat="server" MaxLength="6" Width="35%" Enabled="true"
                                                class="form-control"></asp:TextBox>
                            <asp:MaskedEditExtender ID="MaskedEditExtendertxtTime5" runat="server" TargetControlID="txtTime5" Mask="99:99" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Time" AcceptAMPM="false" ErrorTooltipEnabled="True" />
                        </div>
                        <div class="col-md-2">
                            <button type="button" onclick="SetSchedule6()" class="btn btn-primary btn-icon-split"><span class="text-success"><i class="fas fa-plus"></i></span></button>
                        </div>
                    </div>
                    <div class="row" style="display: none" id="divSchedule6" runat="server" clientidmode="Static">
                        <asp:HiddenField ID="hdnSchedule6" runat="server" ClientIDMode="Static" />
                        <div class="col-md-5">
                            Schedule Date 7:<asp:Label ID="lblScheduleDate6" Visible="false" runat="server" ClientIDMode="Static"></asp:Label>
                            <asp:TextBox ID="txtScheduleDate6" runat="server" onchange="javascript:text_changed_from6();" class="form-control datepicker" placeholder="Scheduled Date"></asp:TextBox>
                            <asp:HiddenField ID="hdnScheduleDate6" runat="server" />
                        </div>
                        <div class="col-md-4">
                            Time (HH:MM) :
                                            <asp:TextBox ID="txtTime6" ToolTip="Enter time in HH:MM format. Entered time will not be deleted. You can overwrite the time." onkeypress="return false" onkeyup="return ValTime6();" OnPaste="return false" runat="server" MaxLength="6" Width="35%" Enabled="true"
                                                class="form-control"></asp:TextBox>
                            <asp:MaskedEditExtender ID="MaskedEditExtendertxtTime6" runat="server" TargetControlID="txtTime6" Mask="99:99" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Time" AcceptAMPM="false" ErrorTooltipEnabled="True" />
                        </div>
                        <div class="col-md-2">
                            <button type="button" onclick="SetSchedule7()" class="btn btn-primary btn-icon-split"><span class="text-success"><i class="fas fa-plus"></i></span></button>
                        </div>
                    </div>
                    <div class="row" style="display: none" id="divSchedule7" runat="server" clientidmode="Static">
                        <asp:HiddenField ID="hdnSchedule7" runat="server" ClientIDMode="Static" />
                        <div class="col-md-5">
                            Schedule Date 8:<asp:Label ID="lblScheduleDate7" Visible="false" runat="server" ClientIDMode="Static"></asp:Label>
                            <asp:TextBox ID="txtScheduleDate7" runat="server" onchange="javascript:text_changed_from7();" class="form-control  datepicker" placeholder="Scheduled Date"></asp:TextBox>
                            <asp:HiddenField ID="hdnScheduleDate7" runat="server" />
                        </div>
                        <div class="col-md-4">
                            Time (HH:MM) :
                                            <asp:TextBox ID="txtTime7" ToolTip="Enter time in HH:MM format. Entered time will not be deleted. You can overwrite the time." onkeypress="return false" onkeyup="return ValTime7();" OnPaste="return false" runat="server" MaxLength="6" Width="35%" Enabled="true"
                                                class="form-control"></asp:TextBox>
                            <asp:MaskedEditExtender ID="MaskedEditExtendertxtTime7" runat="server" TargetControlID="txtTime7" Mask="99:99" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Time" AcceptAMPM="false" ErrorTooltipEnabled="True" />
                        </div>
                        <div class="col-md-2">
                            <button type="button" onclick="SetSchedule8()" class="btn btn-primary btn-icon-split"><span class="text-success"><i class="fas fa-plus"></i></span></button>
                        </div>
                    </div>
                    <div class="row" style="display: none" id="divSchedule8" runat="server" clientidmode="Static">
                        <asp:HiddenField ID="hdnSchedule8" runat="server" ClientIDMode="Static" />
                        <div class="col-md-5">
                            Schedule Date 9:<asp:Label ID="lblScheduleDate8" Visible="false" runat="server" ClientIDMode="Static"></asp:Label>
                            <asp:TextBox ID="txtScheduleDate8" runat="server" onchange="javascript:text_changed_from8();" class="form-control datepicker" placeholder="Scheduled Date"></asp:TextBox>
                            <asp:HiddenField ID="hdnScheduleDate8" runat="server" />
                        </div>
                        <div class="col-md-4">
                            Time (HH:MM) :
                                            <asp:TextBox ID="txtTime8" ToolTip="Enter time in HH:MM format. Entered time will not be deleted. You can overwrite the time." onkeypress="return false" onkeyup="return ValTime8();" OnPaste="return false" runat="server" MaxLength="6" Width="35%" Enabled="true"
                                                class="form-control"></asp:TextBox>
                            <asp:MaskedEditExtender ID="MaskedEditExtendertxtTime8" runat="server" TargetControlID="txtTime8" Mask="99:99" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Time" AcceptAMPM="false" ErrorTooltipEnabled="True" />
                        </div>
                        <div class="col-md-2">
                            <button type="button" onclick="SetSchedule9()" class="btn btn-primary btn-icon-split"><span class="text-success"><i class="fas fa-plus"></i></span></button>
                        </div>
                    </div>
                    <div class="row" style="display: none" id="divSchedule9" runat="server" clientidmode="Static">
                        <asp:HiddenField ID="hdnSchedule9" runat="server" ClientIDMode="Static" />
                        <div class="col-md-5">
                            Schedule Date 10:<asp:Label ID="lblScheduleDate9" Visible="false" runat="server" ClientIDMode="Static"></asp:Label>
                            <asp:TextBox ID="txtScheduleDate9" runat="server" onchange="javascript:text_changed_from9();" class="form-control datepicker" placeholder="Scheduled Date"></asp:TextBox>
                            <asp:HiddenField ID="hdnScheduleDate9" runat="server" />
                        </div>
                        <div class="col-md-4">
                            Time (HH:MM) :
                                            <asp:TextBox ID="txtTime9" ToolTip="Enter time in HH:MM format. Entered time will not be deleted. You can overwrite the time." onkeypress="return false" onkeyup="return ValTime9();" OnPaste="return false" runat="server" MaxLength="6" Width="35%" Enabled="true"
                                                class="form-control"></asp:TextBox>
                            <asp:MaskedEditExtender ID="MaskedEditExtendertxtTime9" runat="server" TargetControlID="txtTime9" Mask="99:99" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Time" AcceptAMPM="false" ErrorTooltipEnabled="True" />
                        </div>
                        <div class="col-md-2">
                            <button type="button" onclick="SetSchedule10()" class="btn btn-primary btn-icon-split"><span class="text-success"><i class="fas fa-plus"></i></span></button>
                        </div>
                    </div>
                    <div class="row" style="display: none" id="divSchedule10" runat="server" clientidmode="Static">
                        <asp:HiddenField ID="hdnSchedule10" runat="server" ClientIDMode="Static" />
                        <div class="col-md-5">
                            Schedule Date 11:<asp:Label ID="lblScheduleDate10" Visible="false" runat="server" ClientIDMode="Static"></asp:Label>
                            <asp:TextBox ID="txtScheduleDate10" runat="server" onchange="javascript:text_changed_from10();" class="form-control datepicker" placeholder="Scheduled Date"></asp:TextBox>
                            <asp:HiddenField ID="hdnScheduleDate10" runat="server" />
                        </div>
                        <div class="col-md-4">
                            Time (HH:MM) :
                                            <asp:TextBox ID="txtTime10" ToolTip="Enter time in HH:MM format. Entered time will not be deleted. You can overwrite the time." onkeypress="return false" onkeyup="return ValTime10();" OnPaste="return false" runat="server" MaxLength="6" Width="35%" Enabled="true"
                                                class="form-control"></asp:TextBox>
                            <asp:MaskedEditExtender ID="MaskedEditExtendertxtTime10" runat="server" TargetControlID="txtTime10" Mask="99:99" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Time" AcceptAMPM="false" ErrorTooltipEnabled="True" />
                        </div>
                        <div class="col-md-2">
                            <button type="button" onclick="SetSchedule11()" class="btn btn-primary btn-icon-split"><span class="text-success"><i class="fas fa-plus"></i></span></button>
                        </div>
                    </div>
                    <div class="row" style="display: none" id="divSchedule11" runat="server" clientidmode="Static">
                        <asp:HiddenField ID="hdnSchedule11" runat="server" ClientIDMode="Static" />
                        <div class="col-md-5">
                            Schedule Date 12:<asp:Label ID="lblScheduleDate11" runat="server" ClientIDMode="Static"></asp:Label>
                            <asp:TextBox ID="txtScheduleDate11" runat="server" onchange="javascript:text_changed_from11();" class="form-control datepicker" placeholder="Scheduled Date"></asp:TextBox>
                            <asp:HiddenField ID="hdnScheduleDate11" runat="server" />
                        </div>
                        <div class="col-md-4">
                            Time (HH:MM) :
                                            <asp:TextBox ID="txtTime11" ToolTip="Enter time in HH:MM format. Entered time will not be deleted. You can overwrite the time." onkeypress="return false" onkeyup="return ValTime11();" OnPaste="return false" runat="server" MaxLength="6" Width="35%" Enabled="true"
                                                class="form-control"></asp:TextBox>
                            <asp:MaskedEditExtender ID="MaskedEditExtendertxtTime11" runat="server" TargetControlID="txtTime11" Mask="99:99" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Time" AcceptAMPM="false" ErrorTooltipEnabled="True" />
                        </div>
                        <div class="col-md-2">
                            <button type="button" onclick="SetSchedule12()" class="btn btn-primary btn-icon-split"><span class="text-success"><i class="fas fa-plus"></i></span></button>
                        </div>
                    </div>
                    <div class="row" style="display: none" id="divSchedule12" runat="server" clientidmode="Static">
                        <asp:HiddenField ID="hdnSchedule12" runat="server" ClientIDMode="Static" />
                        <div class="col-md-5">
                            Schedule Date 13:<asp:Label ID="lblScheduleDate12" Visible="false" runat="server" ClientIDMode="Static"></asp:Label>
                            <asp:TextBox ID="txtScheduleDate12" runat="server" onchange="javascript:text_changed_from12();" class="form-control datepicker" placeholder="Scheduled Date"></asp:TextBox>
                            <asp:HiddenField ID="hdnScheduleDate12" runat="server" />
                        </div>
                        <div class="col-md-4">
                            Time (HH:MM) :
                                            <asp:TextBox ID="txtTime12" ToolTip="Enter time in HH:MM format. Entered time will not be deleted. You can overwrite the time." onkeypress="return false" onkeyup="return ValTime12();" OnPaste="return false" runat="server" MaxLength="6" Width="35%" Enabled="true"
                                                class="form-control"></asp:TextBox>
                            <asp:MaskedEditExtender ID="MaskedEditExtendertxtTime12" runat="server" TargetControlID="txtTime12" Mask="99:99" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Time" AcceptAMPM="false" ErrorTooltipEnabled="True" />
                        </div>
                        <div class="col-md-2">
                            <button type="button" onclick="SetSchedule13()" class="btn btn-primary btn-icon-split"><span class="text-success"><i class="fas fa-plus"></i></span></button>
                        </div>
                    </div>
                    <div class="row" style="display: none" id="divSchedule13" runat="server" clientidmode="Static">
                        <asp:HiddenField ID="hdnSchedule13" runat="server" ClientIDMode="Static" />
                        <div class="col-md-5">
                            Schedule Date 14:<asp:Label ID="lblScheduleDate13" Visible="false" runat="server" ClientIDMode="Static"></asp:Label>
                            <asp:TextBox ID="txtScheduleDate13" runat="server" onchange="javascript:text_changed_from13();" class="form-control datepicker" placeholder="Scheduled Date"></asp:TextBox>
                            <asp:HiddenField ID="hdnScheduleDate13" runat="server" />
                        </div>
                        <div class="col-md-4">
                            Time (HH:MM) :
                                            <asp:TextBox ID="txtTime13" ToolTip="Enter time in HH:MM format. Entered time will not be deleted. You can overwrite the time." onkeypress="return false" onkeyup="return ValTime13();" OnPaste="return false" runat="server" MaxLength="6" Width="35%" Enabled="true"
                                                class="form-control"></asp:TextBox>
                            <asp:MaskedEditExtender ID="MaskedEditExtendertxtTime13" runat="server" TargetControlID="txtTime13" Mask="99:99" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Time" AcceptAMPM="false" ErrorTooltipEnabled="True" />
                        </div>
                        <div class="col-md-2">
                            <button type="button" onclick="SetSchedule14()" class="btn btn-primary btn-icon-split"><span class="text-success"><i class="fas fa-plus"></i></span></button>
                        </div>
                    </div>
                    <div class="row" style="display: none" id="divSchedule14" runat="server" clientidmode="Static">
                        <asp:HiddenField ID="hdnSchedule14" runat="server" ClientIDMode="Static" />
                        <div class="col-md-5">
                            Schedule Date 15:<asp:Label ID="lblScheduleDate14" Visible="false" runat="server" ClientIDMode="Static"></asp:Label>
                            <asp:TextBox ID="txtScheduleDate14" runat="server" onchange="javascript:text_changed_from14();" class="form-control datepicker" placeholder="Scheduled Date"></asp:TextBox>
                            <asp:HiddenField ID="hdnScheduleDate14" runat="server" />
                        </div>
                        <div class="col-md-4">
                            Time (HH:MM) :
                                            <asp:TextBox ID="txtTime14" ToolTip="Enter time in HH:MM format. Entered time will not be deleted. You can overwrite the time." onkeypress="return false" onkeyup="return ValTime14();" OnPaste="return false" runat="server" MaxLength="6" Width="35%" Enabled="true"
                                                class="form-control"></asp:TextBox>
                            <asp:MaskedEditExtender ID="MaskedEditExtendertxtTime14" runat="server" TargetControlID="txtTime14" Mask="99:99" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Time" AcceptAMPM="false" ErrorTooltipEnabled="True" />
                        </div>
                        <div id="divSchedule" runat="server" style="display: none; text-align: center" class="form-group row">
                            <div>Scheduling SMS. Please wait...</div>
                            <div>
                                <img src="img/loading.gif" width="125px" height="100px" />
                            </div>
                        </div>
                    </div>
                </div>
                <div align="center" class="modal-footer">
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Button ID="btnScheduleSMS" runat="server" Text="Schedule SMS" class="btn btn-primary" OnClientClick="showLoading1(); return ConfirmSubmit();" OnClick="btnScheduleSMS_Click" />
                            <asp:Button ID="btnCancelSch" runat="server" Text="Cancel" class="btn btn-primary" OnClick="btnCancelSch_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:LinkButton ID="lnkSCH" runat="server"></asp:LinkButton>
    <asp:ModalPopupExtender ID="pnlPopUp_SCHEDULE_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground" PopupControlID="pnlPopUp_SCHEDULE" TargetControlID="lnkSCH">
    </asp:ModalPopupExtender>
    <%--  Schedule POPUP --%>

    <%-- Group POPUP --%>
    <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>--%>
    <asp:Panel ID="pnlPopUp_GROUP" runat="server" CssClass="modalPopup" Style="display: none;">
        <div style="overflow-y: auto; overflow-x: hidden; max-height: 450px;">
            <div class="modal-header">
                <div>
                    <asp:Label ID="Label1" runat="server" CssClass="modal-title" Style="font-size: x-large; font-weight: bold;" Text="Group"></asp:Label>&nbsp;
                </div>
                <div>
                    Total Groups :
                    <asp:Label ID="lbltotgroups" runat="server" CssClass="modal-title"></asp:Label>&nbsp;
                Selected Groups :
                    <asp:Label ID="lblselectedgrp" runat="server" CssClass="modal-title"></asp:Label>&nbsp;&nbsp;
                </div>
                <button type="button" id="btnclose" class="close" data-dismiss="modal" aria-hidden="true">Ã—</button>
            </div>
            <div class="modal-body">
                <div class="checkbox checkbox-primary">
                    <div id="divid" runat="server" style="height: 250px; overflow: scroll">
                        <asp:CheckBoxList ID="chkbxgroup" ClientIDMode="Static" CssClass="styled" runat="server" RepeatLayout="OrderedList">
                        </asp:CheckBoxList>
                    </div>
                </div>
            </div>
            <div align="center" class="modal-footer">
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnAddGroup" runat="server" Text="Submit" class="btn btn-primary" OnClick="btnAddGroup_Click" />&nbsp;
                        <asp:Button ID="btnCleargroup" CssClass="btn btn-primary" runat="server" Text="Clear" OnClick="btnCleargroup_Click" />
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:LinkButton ID="lnkGroup" runat="server"></asp:LinkButton>
    <asp:ModalPopupExtender ID="pnlPopUp_GROUP_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground" PopupControlID="pnlPopUp_GROUP" TargetControlID="lnkGroup" CancelControlID="btnclose">
    </asp:ModalPopupExtender>
    <%-- Group POPUP --%>

    <%-- URL POPUP --%>
    <asp:LinkButton ID="lnkURL" runat="server"></asp:LinkButton>

    <%--pnlPopUp_URL Panel With Design--%>
    <asp:Panel ID="pnlPopUp_URL" runat="server" CssClass="modalPopup" Style="display: none;">
        <div class="modal-content">
            <div class="modal-header">
                <asp:Label ID="lblHeading" runat="server" CssClass="modal-title" Text="Load Short URL"></asp:Label>
            </div>
            <div class="modal-body">
                <div class="mb-3">
                    <asp:Button ID="btnExisting" Text="Existing" runat="server" OnClick="btnExisting_Click" CssClass="btn btn-primary" />
                    <asp:Button ID="btnNew" Text="Add New" runat="server" OnClick="btnNew_Click" CssClass="btn btn-primary" />
                    <asp:Button ID="btnMedia" Text="Add Media" runat="server" OnClick="btnMedia_Click" CssClass="btn btn-primary" />
                </div>

                <div class="">
                    <asp:Panel ID="pnlExisting" runat="server" Visible="false">
                        <div>
                            <asp:Label ID="Label3" runat="server" Text="Short URL : "></asp:Label>
                            <asp:DropDownList ID="ddlURL" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlURL_OnSelectedIndexChanged" class="drop-select form-control mb-3"></asp:DropDownList>
                            <asp:Button ID="btnShowLongURL" runat="server" Text="Show Long URL" class="btn btn-primary" OnClick="btnShowLongURL_Click" />
                            <div class="d-flex" style="width: 500px!important">
                                <asp:Label ID="lblLongUrl" CssClass="text-break" runat="server" Style="font-size: 14px;" Text=""></asp:Label>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
                <div class="">
                    <asp:Panel ID="pnlNew" runat="server" Visible="false">
                        <div>
                            <br />
                            <div class="form-group">
                                <div class="form-group">
                                    <asp:TextBox ID="txtAddName" runat="server" class="form-control" aria-describedby="textHelp" placeholder="Add Name"></asp:TextBox>
                                </div>
                                <div class="form-group mb-0">
                                    <asp:TextBox ID="txtLongURL" runat="server" class="form-control" aria-describedby="textHelp" placeholder="Enter Long URL"></asp:TextBox>
                                    <small id="textHelp" class="form-text text-muted">Short URL: <strong>http://emim.in/word</strong></small>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlMedia" runat="server" Visible="false">
                        <br />
                        <div class="form-group row">
                            <div class="col-md-12">
                                <div class="custom-file">
                                    <asp:FileUpload ID="FileUpload2" runat="server" class="custom-file-input" ClientIDMode="Static" onchange="this.form.submit()" />
                                    <label class="custom-file-label" for="customFile">Choose file</label>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
            <div class="modal-footer">
                <asp:Button ID="btnInsertURLinMsg" runat="server" Text="Insert Short URL in Message" class="btn btn-primary" OnClick="btnInsertURLinMsg_Click" />
                <button id="btnCancel" runat="server" class="btn btn-primary">Cancel</button>
            </div>
        </div>
    </asp:Panel>

    <%--pnlPopUp_URL Modal Popup Extender For pnlPopUp_URL--%>
    <asp:ModalPopupExtender ID="pnlPopUp_URL_ModalPopupExtender" runat="server" PopupControlID="pnlPopUp_URL" TargetControlID="lnkURL" BehaviorID="mpeAddUpdateEmployee" CancelControlID="btnCancel" BackgroundCssClass="modalBackground">
    </asp:ModalPopupExtender>
    <%-- URL POPUP --%>
    <script src="js/dialog.js"></script>
    <script type="text/javascript">
        $(function () {
            $('#txtPhone').intlTelInput({
                autoHideDialCode: true,
                autoPlaceholder: "ON",
                dropdownContainer: document.body,
                formatOnDisplay: true,
                hiddenInput: "full_number",
                initialCountry: "IN",
                nationalMode: true,
                placeholderNumberType: "MOBILE",
                preferredCountries: ["IN", "AE", "US"],
                separateDialCode: true
            });
            $('#btnOTP').on('click', function () {
                var code = $("#txtPhone").intlTelInput("getSelectedCountryData").dialCode;
                $('#hdnCC').val(code);
            });
        });
    </script>
    <script type="text/javascript">
        function SetSchedule1() {
            document.getElementById('divSchedule1').style.display = "";
            document.getElementById('hdnSchedule1').value = 1;
            document.getElementById('hdnScheduleCount').value = 2;
        }

        function SetSchedule2() {
            document.getElementById('divSchedule2').style.display = "";
            document.getElementById('hdnSchedule2').value = 1;
            document.getElementById('hdnScheduleCount').value = 3;
        }

        function SetSchedule3() {
            document.getElementById('divSchedule3').style.display = "";
            document.getElementById('hdnSchedule3').value = 1;
            document.getElementById('hdnScheduleCount').value = 4;
        }

        function SetSchedule4() {
            document.getElementById('divSchedule4').style.display = "";
            document.getElementById('hdnSchedule4').value = 1;
            document.getElementById('hdnScheduleCount').value = 5;
        }

        function SetSchedule5() {
            document.getElementById('divSchedule5').style.display = "";
            document.getElementById('hdnSchedule5').value = 1;
            document.getElementById('hdnScheduleCount').value = 6;
        }

        function SetSchedule6() {
            document.getElementById('divSchedule6').style.display = "";
            document.getElementById('hdnSchedule6').value = 1;
            document.getElementById('hdnScheduleCount').value = 7;
        }

        function SetSchedule7() {
            document.getElementById('divSchedule7').style.display = "";
            document.getElementById('hdnSchedule7').value = 1;
            document.getElementById('hdnScheduleCount').value = 8;
        }

        function SetSchedule8() {
            document.getElementById('divSchedule8').style.display = "";
            document.getElementById('hdnSchedule8').value = 1;
            document.getElementById('hdnScheduleCount').value = 9;
        }

        function SetSchedule9() {
            document.getElementById('divSchedule9').style.display = "";
            document.getElementById('hdnSchedule9').value = 1;
            document.getElementById('hdnScheduleCount').value = 10;
        }

        function SetSchedule10() {
            document.getElementById('divSchedule10').style.display = "";
            document.getElementById('hdnSchedule10').value = 1;
            document.getElementById('hdnScheduleCount').value = 11;
        }

        function SetSchedule11() {
            document.getElementById('divSchedule11').style.display = "";
            document.getElementById('hdnSchedule11').value = 1;
            document.getElementById('hdnScheduleCount').value = 12;
        }

        function SetSchedule12() {
            document.getElementById('divSchedule12').style.display = "";
            document.getElementById('hdnSchedule12').value = 1;
            document.getElementById('hdnScheduleCount').value = 13;
        }

        function SetSchedule13() {
            document.getElementById('divSchedule13').style.display = "";
            document.getElementById('hdnSchedule13').value = 1;
            document.getElementById('hdnScheduleCount').value = 14;
        }

        function SetSchedule14() {
            document.getElementById('divSchedule14').style.display = "";
            document.getElementById('hdnSchedule14').value = 1;
            document.getElementById('hdnScheduleCount').value = 15;
        }

        function ValTime1() {
            var flag = true;
            var MM = $("<%=txtTime1.ClientID%>").val();
            HH = MM.split(':')[0];
            MM = MM.split(':')[1];

            if (HH > 23) {
                alert('Please Enter HH less than 24')
                flag = false;
            }

            if (MM > 59) {
                alert('Please Enter MM less than 60')
                flag = false;
            }
        }

        function text_changed_from1() {
            var d = document.getElementById("ContentPlaceHolder1_txtScheduleDate1").value
            console.log(d);
            document.getElementById("ContentPlaceHolder1_hdnScheduleDate1").value = d;
        }

        function ValTime2() {
            var flag = true;
            var MM = $("<%=txtTime2.ClientID%>").val();
            HH = MM.split(':')[0];
            MM = MM.split(':')[1];

            if (HH > 23) {
                alert('Please Enter HH less than 24')
                flag = false;
            }

            if (MM > 59) {
                alert('Please Enter MM less than 60')
                flag = false;
            }
        }

        function text_changed_from2() {
            var d = document.getElementById("ContentPlaceHolder1_txtScheduleDate2").value
            console.log(d);
            document.getElementById("ContentPlaceHolder1_hdnScheduleDate2").value = d;
        }

        function ValTime3() {
            var flag = true;
            var MM = $("<%=txtTime3.ClientID%>").val();
            HH = MM.split(':')[0];
            MM = MM.split(':')[1];

            if (HH > 23) {
                alert('Please Enter HH less than 24')
                flag = false;
            }

            if (MM > 59) {
                alert('Please Enter MM less than 60')
                flag = false;
            }

        }

        function text_changed_from3() {
            var d = document.getElementById("ContentPlaceHolder1_txtScheduleDate3").value
            console.log(d);
            document.getElementById("ContentPlaceHolder1_hdnScheduleDate3").value = d;
        }

        function ValTime4() {
            var flag = true;
            var MM = $("<%=txtTime4.ClientID%>").val();
            HH = MM.split(':')[0];
            MM = MM.split(':')[1];

            if (HH > 23) {
                alert('Please Enter HH less than 24')
                flag = false;
            }

            if (MM > 59) {
                alert('Please Enter MM less than 60')
                flag = false;
            }

        }

        function text_changed_from4() {
            var d = document.getElementById("ContentPlaceHolder1_txtScheduleDate4").value
            console.log(d);
            document.getElementById("ContentPlaceHolder1_hdnScheduleDate4").value = d;
        }

        function ValTime5() {
            var flag = true;
            var MM = $("<%=txtTime5.ClientID%>").val();
            HH = MM.split(':')[0];
            MM = MM.split(':')[1];

            if (HH > 23) {
                alert('Please Enter HH less than 24')
                flag = false;
            }

            if (MM > 59) {
                alert('Please Enter MM less than 60')
                flag = false;
            }
        }

        function text_changed_from5() {
            var d = document.getElementById("ContentPlaceHolder1_txtScheduleDate5").value
            console.log(d);
            document.getElementById("ContentPlaceHolder1_hdnScheduleDate5").value = d;
        }

        function ValTime6() {
            var flag = true;
            var MM = $("<%=txtTime6.ClientID%>").val();
            HH = MM.split(':')[0];
            MM = MM.split(':')[1];

            if (HH > 23) {
                alert('Please Enter HH less than 24')
                flag = false;
            }

            if (MM > 59) {
                alert('Please Enter MM less than 60')
                flag = false;
            }
        }

        function text_changed_from6() {
            var d = document.getElementById("ContentPlaceHolder1_txtScheduleDate6").value
            console.log(d);
            document.getElementById("ContentPlaceHolder1_hdnScheduleDate6").value = d;
        }

        function ValTime7() {
            var flag = true;
            var MM = $("<%=txtTime7.ClientID%>").val();
            HH = MM.split(':')[0];
            MM = MM.split(':')[1];

            if (HH > 23) {
                alert('Please Enter HH less than 24')
                flag = false;
            }

            if (MM > 59) {
                alert('Please Enter MM less than 60')
                flag = false;
            }

        }

        function text_changed_from7() {
            var d = document.getElementById("ContentPlaceHolder1_txtScheduleDate7").value
            console.log(d);
            document.getElementById("ContentPlaceHolder1_hdnScheduleDate7").value = d;
        }
        function ValTime8() {
            var flag = true;
            var MM = $("<%=txtTime8.ClientID%>").val();
            HH = MM.split(':')[0];
            MM = MM.split(':')[1];

            if (HH > 23) {
                alert('Please Enter HH less than 24')
                flag = false;
            }

            if (MM > 59) {
                alert('Please Enter MM less than 60')
                flag = false;
            }
        }

        function text_changed_from8() {
            var d = document.getElementById("ContentPlaceHolder1_txtScheduleDate8").value
            console.log(d);
            document.getElementById("ContentPlaceHolder1_hdnScheduleDate8").value = d;
        }

        function ValTime9() {
            var flag = true;
            var MM = $("<%=txtTime9.ClientID%>").val();
            HH = MM.split(':')[0];
            MM = MM.split(':')[1];

            if (HH > 23) {
                alert('Please Enter HH less than 24')
                flag = false;
            }

            if (MM > 59) {
                alert('Please Enter MM less than 60')
                flag = false;
            }
        }

        function text_changed_from9() {
            var d = document.getElementById("ContentPlaceHolder1_txtScheduleDate9").value
            console.log(d);
            document.getElementById("ContentPlaceHolder1_hdnScheduleDate9").value = d;
        }

        function ValTime10() {
            var flag = true;
            var MM = $("<%=txtTime10.ClientID%>").val();
            HH = MM.split(':')[0];
            MM = MM.split(':')[1];

            if (HH > 23) {
                alert('Please Enter HH less than 24')
                flag = false;
            }

            if (MM > 59) {
                alert('Please Enter MM less than 60')
                flag = false;
            }
        }

        function text_changed_from10() {
            var d = document.getElementById("ContentPlaceHolder1_txtScheduleDate10").value
            console.log(d);
            document.getElementById("ContentPlaceHolder1_hdnScheduleDate10").value = d;
        }

        function ValTime11() {
            var flag = true;
            var MM = $("<%=txtTime11.ClientID%>").val();
            HH = MM.split(':')[0];
            MM = MM.split(':')[1];

            if (HH > 23) {
                alert('Please Enter HH less than 24')
                flag = false;
            }

            if (MM > 59) {
                alert('Please Enter MM less than 60')
                flag = false;
            }
        }

        function text_changed_from11() {
            var d = document.getElementById("ContentPlaceHolder1_txtScheduleDate11").value
            console.log(d);
            document.getElementById("ContentPlaceHolder1_hdnScheduleDate11").value = d;
        }

        function ValTime12() {
            var flag = true;
            var MM = $("<%=txtTime12.ClientID%>").val();
            HH = MM.split(':')[0];
            MM = MM.split(':')[1];

            if (HH > 23) {
                alert('Please Enter HH less than 24')
                flag = false;
            }

            if (MM > 59) {
                alert('Please Enter MM less than 60')
                flag = false;
            }
        }

        function text_changed_from12() {
            var d = document.getElementById("ContentPlaceHolder1_txtScheduleDate12").value
            console.log(d);
            document.getElementById("ContentPlaceHolder1_hdnScheduleDate12").value = d;
        }

        function ValTime13() {
            var flag = true;
            var MM = $("<%=txtTime13.ClientID%>").val();
            HH = MM.split(':')[0];
            MM = MM.split(':')[1];

            if (HH > 23) {
                alert('Please Enter HH less than 24')
                flag = false;
            }

            if (MM > 59) {
                alert('Please Enter MM less than 60')
                flag = false;
            }
        }

        function text_changed_from13() {
            var d = document.getElementById("ContentPlaceHolder1_txtScheduleDate13").value
            console.log(d);
            document.getElementById("ContentPlaceHolder1_hdnScheduleDate13").value = d;
        }

        function ValTime14() {
            var flag = true;
            var MM = $("<%=txtTime14.ClientID%>").val();
            HH = MM.split(':')[0];
            MM = MM.split(':')[1];

            if (HH > 23) {
                alert('Please Enter HH less than 24')
                flag = false;
            }

            if (MM > 59) {
                alert('Please Enter MM less than 60')
                flag = false;
            }
        }

        function text_changed_from14() {
            var d = document.getElementById("ContentPlaceHolder1_txtScheduleDate14").value
            console.log(d);
            document.getElementById("ContentPlaceHolder1_hdnScheduleDate14").value = d;
        }
    </script>
    <script type="text/javascript">
        function ValTime() {
            var flag = true;
            var MM = $("<%=txtTime.ClientID%>").val();
            HH = MM.split(':')[0];
            MM = MM.split(':')[1];

            if (HH > 23) {
                alert('Please Enter HH less than 24')
                flag = false;
            }

            if (MM > 59) {
                alert('Please Enter MM less than 60')
                flag = false;
            }
        }

        function text_changed_from() {
            var d = document.getElementById("ContentPlaceHolder1_txtScheduleDate").value
            console.log(d);
            document.getElementById("ContentPlaceHolder1_hdnScheduleDate").value = d;
        }

        function showLoading() {
            document.getElementById('<%= divFileLoader.ClientID %>').style.display = 'block';
        }

        function SMSfileUpload3() {
            var uploadControl = document.getElementById('<%= FileUpload3.ClientID %>');
            var myfile = uploadControl.value;
            console.log(myfile);

            var fake_path = document.getElementById('FileUpload3').value
            var arr = fake_path.split("\\").pop().split(".");
            var fname = arr[0];
            if (fname.length > 50) {
                alert("Filename should be atleast 50 character !!");
                return false;
            }
            var Extension = myfile.substring(myfile.lastIndexOf('.') + 1).toLowerCase();
            console.log(Extension);
            if (Extension == "txt") {
                if (uploadControl.files[0].size > 6291456) {
                    alert("Upload csv file of size upto 6 MB only.");
                    return false;
                } else {
                    showLoading();
                    console.log("ret true");
                    return true;
                }
            }
            if (Extension == "csv") {
                if (uploadControl.files[0].size > 20971520) {
                    alert("Upload CSV file of size upto 12 MB only.");
                    return false;
                } else {
                    showLoading();
                    console.log("ret true");
                    return true;
                }
            }
            if (Extension == "xls" || Extension == "xlsx") {
                if (uploadControl.files[0].size > 5242880) {
                    alert("Upload Excel file of size upto 5 MB only.");
                    return false;
                } else {
                    showLoading();
                    console.log("ret true");
                    return true;
                }
            }
        }

        function SMSfileUpload() {
            var uploadControl = document.getElementById('<%= FileUpload1.ClientID %>');
            var myfile = uploadControl.value;

            var fake_path = document.getElementById('FileUpload1').value
            var arr = fake_path.split("\\").pop().split(".");
            var fname = arr[0];
            if (fname.length > 50) {
                alert("Filename should be atleast 50 character !!");
                return false;
            }
            console.log(myfile);
            var Extension = myfile.substring(myfile.lastIndexOf('.') + 1).toLowerCase();
            console.log(Extension);

            if (Extension == "txt") {
                if (uploadControl.files[0].size > 2 * 6291456) {
                    //alert("Upload text file of size upto 6 MB only.");
                    return true;
                } else {
                    showLoading();
                    console.log("ret true");
                    return true;
                }
            }
            if (Extension == "csv") {
                if (uploadControl.files[0].size > 20971520) {
                    //alert("Upload csv file of size upto 20 MB only."); /*3 * 6291456*/
                    return true;
                } else {
                    showLoading();
                    console.log("ret true");
                    return true;
                }
            }

            if (Extension == "xls" || Extension == "xlsx") {
                if (uploadControl.files[0].size > 5242880) {
                    //alert("Upload Excel file of size upto 5 MB only.");
                    return true;
                } else {
                    showLoading();
                    console.log("ret true");
                    return true;
                }
            }
        }

        function showLoading1() {
            document.getElementById('<%= divSchedule.ClientID %>').style.display = 'block';
        }

        function isnumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if ((charCode > 48 && charCode < 57) || charCode == 44 || charCode == 13) {
                return true;
            } else {
                return false;
            }
        }

        function integersOnly(obj) {
            obj.value = obj.value.replace(/[^0-9,\r\n]/g, '');
        }

        function mobnumbcnt() {
            var s = document.getElementById("<%=txtMobNum.ClientID%>").value;
            if (s != '') {
                var str = s;
                var x = 0;
                for (var i = 0; i < str.length; i++) {
                    if (str.charAt(i) == ',') x++;
                }
                for (var i = 0; i < str.length; i++) {
                    if (str.charAt(i) == '\n') x++;
                }

                if (x > 0) {
                    document.getElementById('<%= lblMobileCnt.ClientID %>').innerHTML = (x + 1).toString();
                } else
                    document.getElementById('<%= lblMobileCnt.ClientID %>').innerHTML = "8";
                if (s != '' && x == 0)
                    document.getElementById('<%= lblMobileCnt.ClientID %>').innerHTML = "1";
            } else {
                document.getElementById('<%= lblMobileCnt.ClientID %>').innerHTML = "";
            }
        }

        function ConfirmSubmit() {

            var s = document.getElementById("<%=txtMsg.ClientID%>").value;
            var IsPersalisedSMSValue = document.getElementById("IsPersalisedSMS").value;
            if (IsPersalisedSMSValue == "1")
                s = document.getElementById("<%=txtPreview.ClientID%>").value;
            var tt = 0;
            //tt = s.split(/\r\n|\r|\n/).length;
            if (tt >= 1) tt = tt - 1;

            var i = 0;
            var ln = 0;
            if (s != '') {

                ln = s.length + tt;
                for (var k = 0, n = s.length; k < n; k++) {
                    if (s.charAt(k) == "~" || s.charAt(k) == "|" || s.charAt(k) == "{" || s.charAt(k) == "}" || s.charAt(k) == "[" || s.charAt(k) == "]" || s.charAt(k) == "^" || s.charAt(k) == "\\") {
                        ln = ln + 1;
                    }
                }

                var ou = document.getElementById("<%=lblOptOut.ClientID%>").innerText;

                console.log(ou);

                var ength = 0;
                if (ou != "") { ength = 1; console.log(ength); }
                else { ength = 0; }

                //ln = ln + ou.length + ength;

                //Abhishek -11-01-2023 Start
                if (document.getElementById("<%=chkOptOut.ClientID%>").checked && UserID.toUpperCase() != "MIM2400057") {

                    ln = ln + ou.length + ength;
                }
                else {
                    ln = ln;
                }
                //Abhishek -11-01-2023 END

                console.log(document.getElementById('<%= ddlSMSType.ClientID %>').val);
                console.log(document.getElementById('<%= ddlSMSType.ClientID %>').value);
                if (document.getElementById('<%= ddlSMSType.ClientID %>').value == "2") ln = ln + 2;
                i = getLength(ln);

                var y = 0;
                for (var k = 0, n = s.length; k < n; k++) {
                    if (s.charCodeAt(k) > 126) {
                        y = 1;

                    }
                }

                if (y == 1) {

                    ln = s.length;

                    for (var k = 0, n = s.length; k < n; k++) {
                        if (s.charAt(k) == "~" || s.charAt(k) == "|" || s.charAt(k) == "{" || s.charAt(k) == "}" || s.charAt(k) == "[" || s.charAt(k) == "]" || s.charAt(k) == "^" || s.charAt(k) == "\\") {
                            ln = ln + 1;
                        }
                    }

                    var ou = document.getElementById("<%=lblOptOut.ClientID%>").innerText;

                    console.log(ou);

                    var ength = 0;
                    if (ou != "") { ength = 1; console.log(ength); }
                    else { ength = 0; }

                    if (document.getElementById("<%=chkOptOut.ClientID%>").checked) {

                        ln = ln + ou.length + ength;
                    }
                    else {
                        ln = ln;
                    }
                    i = getUniCodeLength(ln);
                }

                if (i > 1) {
                    if (confirm('This message will cost \n\n' + i + ' SMS per message. \n\nDo you want to continue ? ')) {
                        console.log('ok clicked.');
                        if (confirm('This message will cost \n\n' + i + ' SMS per message. \n\n Are you sure you want to continue ? ')) {
                            return true;
                        } else {
                            document.getElementById('<%= divSchedule.ClientID %>').style.display = 'none';
                            return false;
                        }
                    } else {
                        document.getElementById('<%= divSchedule.ClientID %>').style.display = 'none';
                        return false;
                    }
                } else
                    return true;
            } else
                return true;
        }

        function ConfirmSubmit2() {
            var s = document.getElementById("<%=txtMsg.ClientID%>").value;
            var tt = 0;
            //tt = s.split(/\r\n|\r|\n/).length;
            if (tt >= 1) tt = tt - 1;
            if (s != '') {
                var i = 0;
                var ln = s.length + tt;
                for (var k = 0, n = s.length; k < n; k++) {
                    if (s.charAt(k) == "~" || s.charAt(k) == "|" || s.charAt(k) == "{" || s.charAt(k) == "}" || s.charAt(k) == "[" || s.charAt(k) == "]" || s.charAt(k) == "^" || s.charAt(k) == "\\") {
                        ln = ln + 1;
                    }
                }

                var ou = document.getElementById("<%=lblOptOut.ClientID%>").innerText;
                console.log(ou);


                var ength = 0;
                if (ou != "") { ength = 1; console.log(ength); }
                else { ength = 0; }
                //ln = ln + ou.length + ength;
                var UserID = '<%=Session["userid"].ToString()%>';
                if (document.getElementById("<%=chkOptOut.ClientID%>").checked && UserID.toUpperCase() != "MIM2400057") {
                    ln = ln + ou.length + ength;
                }
                else {
                    ln = ln;
                }

                console.log(document.getElementById('<%= ddlSMSType.ClientID %>').val);
                console.log(document.getElementById('<%= ddlSMSType.ClientID %>').value);
                if (document.getElementById('<%= ddlSMSType.ClientID %>').value == "2") ln = ln + 2;
                i = getLength(ln);

                var y = 0;
                for (var k = 0, n = s.length; k < n; k++) {
                    if (s.charCodeAt(k) > 126) {
                        y = 1;
                    }
                }
                if (y == 1) {
                    ln = s.length;

                    for (var k = 0, n = s.length; k < n; k++) {
                        if (s.charAt(k) == "~" || s.charAt(k) == "|" || s.charAt(k) == "{" || s.charAt(k) == "}" || s.charAt(k) == "[" || s.charAt(k) == "]" || s.charAt(k) == "^" || s.charAt(k) == "\\") {
                            ln = ln + 1;
                        }
                    }

                    var ou = document.getElementById("<%=lblOptOut.ClientID%>").innerText;
                    console.log(ou);

                    var ength = 0;
                    if (ou != "") { ength = 1; console.log(ength); }
                    else { ength = 0; }
                    //ln = ln + ou.length + ength;

                    if (document.getElementById("<%=chkOptOut.ClientID%>").checked) {

                        ln = ln + ou.length + ength;
                    }
                    else {
                        ln = ln;
                    }
                    //ln = ln + ou.length + ength;
                    i = getUniCodeLength(ln);
                }
                if (i > 1) {
                    if (confirm('This message will cost ' + i.toString() + ' SMS per message. Do you want to continue ? ')) {
                        if (confirm('This message will cost ' + i.toString() + ' SMS per message. \n\n Are you sure you want to continue ? ')) {
                            return true;
                        } else return false;
                    } else return false;
                } else
                    return true;
            } else
                return true;
        }

        function getLength(ln) {
            var i = 0;
            if (ln >= 1) i = 1;
            if (ln > 160) i = 2;
            if (ln > 306) i = 3;
            if (ln > 459) i = 4;
            if (ln > 612) i = 5;
            if (ln > 765) i = 6;
            if (ln > 918) i = 7;
            if (ln > 1071) i = 8;
            if (ln > 1224) i = 9;
            if (ln > 1377) i = 10;
            if (ln > 1530) i = 11;
            if (ln > 1683) i = 12;

            return i;
        }

        function getUniCodeLength(ln) {
            var i = 0;
            if (ln >= 1) i = 1;
            if (ln > 70) i = 2;
            if (ln > 134) i = 3;
            if (ln > 201) i = 4;
            if (ln > 268) i = 5;
            if (ln > 335) i = 6;
            if (ln > 402) i = 7;
            if (ln > 469) i = 8;
            if (ln > 536) i = 9;
            if (ln > 603) i = 10;
            return i;
        }

        function smscnt() {
            var s = document.getElementById("<%=txtMsg.ClientID%>").value;
            var tt = 0;
            var prelen = 0;
            var ength = 0;
            var ln = 0;
            //tt = s.split(/\r\n|\r|\n/).length;
            if (tt >= 1) tt = tt - 1;
            if (s != '') {
                var i = 0;
                ln = s.length + tt;
                for (var k = 0, n = s.length; k < n; k++) {
                    if (s.charAt(k) == "~" || s.charAt(k) == "|" || s.charAt(k) == "{" || s.charAt(k) == "}" || s.charAt(k) == "[" || s.charAt(k) == "]" || s.charAt(k) == "^" || s.charAt(k) == "\\") {
                        ln = ln + 1;
                    }
                    prelen = ln;
                }
                var ou = document.getElementById("<%=lblOptOut.ClientID%>").innerText;
                var chk = document.getElementById("<%=chkOptOut.ClientID%>");

                console.log(ou);

                //chk.addEventListener("change", () => {
                if (document.getElementById("<%=chkOptOut.ClientID%>").checked) {
                    var UserID = '<%=Session["userid"].ToString()%>';
                    console.log(UserID);
                    if (UserID.toUpperCase() != "MIM2400057") {
                        if (ou.val != "") { ength = 1; console.log(ength); }
                        ln = ln + ou.length + ength;
                        console.log('tot len' + ln);
                    }
                    else { console.log('Done len' + UserID); }
                }
                else {
                    console.log('tot len' + ln);
                    ln = prelen;
                }
                console.log(document.getElementById('<%= ddlSMSType.ClientID %>').val);
                console.log(document.getElementById('<%= ddlSMSType.ClientID %>').value);
                <%--var Domain = '<%=Session["Domain"].ToString()%>';--%>
                <%--alert('<%=Session["Domain"].ToString()%>');--%>
                var domain = '<%=Session["Domain"].ToString()%>';
                if (document.getElementById('<%= ddlSMSType.ClientID %>').value != "1" && domain != "" && s.toLowerCase().includes(domain)) ln = ln + 2;
                <%--if (document.getElementById('<%= ddlSMSType.ClientID %>').value == "2" ) ln = ln + 2;--%>
                i = getLength(ln);
                document.getElementById('<%= lblUniCode.ClientID %>').innerHTML = "";
                console.log('len=' + ln);
                document.getElementById('<%= lblsmscnt.ClientID %>').innerHTML = "No. of Char: " + ln + ". <br />No. of SMS: " + i.toString();
                var y = 0;
                for (var k = 0, n = s.length; k < n; k++) {
                    if (s.charCodeAt(k) > 126) {
                        y = 1;
                    }
                }
                if (y == 1) {
                    ln = s.length;

                    for (var k = 0, n = s.length; k < n; k++) {
                        if (s.charAt(k) == "~" || s.charAt(k) == "|" || s.charAt(k) == "{" || s.charAt(k) == "}" || s.charAt(k) == "[" || s.charAt(k) == "]" || s.charAt(k) == "^" || s.charAt(k) == "\\") {
                            ln = ln + 1;
                        }
                        prelen = ln;
                    }

                    //ln = ln + ou.length + ength;

                    if (document.getElementById("<%=chkOptOut.ClientID%>").checked) {

                        if (ou.val != "") { ength = 1; console.log(ength); }
                        ln = ln + ou.length + ength;
                        console.log('tot len' + ln);
                    }
                    else {
                        console.log('tot len' + ln);
                        ln = prelen;
                    }
                    if (document.getElementById('<%= ddlSMSType.ClientID %>').value != "1" && domain != "" && s.toLowerCase().includes(domain)) ln = ln + 2;
                    i = getUniCodeLength(ln);

                    document.getElementById('<%= lblUniCode.ClientID %>').innerHTML = "UNICODE : YES";

                    document.getElementById('<%= lblsmscnt.ClientID %>').innerHTML = "No. of Char: " + ln + ". <br />No. of SMS: " + i.toString();
                }
            } else
                document.getElementById('<%= lblsmscnt.ClientID %>').innerHTML = "";
        }
    </script>

    <!-- Select2 CSS -->
    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <!-- jQuery -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
    <!-- Select2 -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js"></script>
    <script>
        $("#ddlTempID").select2({
            allowClear: true
        });

        $("#ddlTemplate").select2({
            allowClear: true
        });
    </script>

    <script type="text/javascript">
        window.onload = function () {
            var chk = document.getElementById("<%=chkbxgroup.ClientID %>");
            var checkboxes = chk.getElementsByTagName("INPUT");
            for (var i = 0; i < checkboxes.length; i++) {
                checkboxes[i].onchange = function () {
                    var message = "";
                    var cnt = 0;
                    for (var i = 0; i < checkboxes.length; i++) {
                        if (checkboxes[i].checked) {
                            cnt++;
                        }
                    }
                    document.getElementById('<%= lblselectedgrp.ClientID %>').innerHTML = cnt;
                };
            }
        };
    </script>
    <script>
        $(function () {
            var nowdate = new Date();
            // sixMonthAgo.setMonth(sixMonthAgo.getMonth() - 6);
            var onemoremonth = new Date();
            onemoremonth.setMonth(onemoremonth.getMonth() + 1);
            $(".datepicker").datepicker({

                todayHighlight: true,
                autoclose: true,
                startDate: nowdate,

                format: 'yyyy-mm-dd',
                endDate: onemoremonth
                //autoUpdateInput: false
            });
        });

        $(document).ready(function () {
            $('[data-toggle="tooltip"]').tooltip({
                template: '<div class="tooltip wide-tooltip" role="tooltip"><div class="tooltip-arrow"></div><div class="tooltip-inner"></div></div>'
            });
        });
    </script>

    <script type="text/javascript">
        function CloseNotification() {
            var notification = document.getElementById("ContentPlaceHolder1_notificationAlert");
            notification.style.display = "none";
        }

        function updateCounts() {
            var duplicateCountElement = document.getElementById("<%= lblMobDuplicateCount.ClientID %>");
            var MobileCnt = document.getElementById("<%= lblMobileCnt.ClientID %>");
            var checkbox = document.getElementById("<%= chkAllowDuplicates.ClientID %>");

            if (checkbox.checked) {
                var duplicateCount = parseInt(duplicateCountElement.innerText);
                var MobileCount = parseInt(MobileCnt.innerText);

                // Update text of both elements
                document.getElementById("<%= lblMobileCnt.ClientID %>").innerText = (duplicateCount + MobileCount).toString();
                document.getElementById("<%= lblMobileCntNotI.ClientID %>").innerText = (duplicateCount + MobileCount).toString();
            }
            else {
                var duplicateCount = parseInt(duplicateCountElement.innerText);
                var MobileCount = parseInt(MobileCnt.innerText);

                // Update text of both elements
                document.getElementById("<%= lblMobileCnt.ClientID %>").innerText = (MobileCount - duplicateCount).toString();
                document.getElementById("<%= lblMobileCntNotI.ClientID %>").innerText = (MobileCount - duplicateCount).toString();
            }
        }
</script>
    <script type="text/javascript">
        function refreshCaptcha() {
            var img = document.getElementById('<%=captcha1.ClientID %>');
            img.src = img.src.split('?')[0] + '?' + new Date().getTime(); // Append a timestamp to force reload
        }
    </script>
</asp:Content>
