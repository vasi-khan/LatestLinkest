<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="send-sms_u30.aspx.cs"  Inherits="eMIMPanel.send_sms_u30" MaintainScrollPositionOnPostback="true" %>
<%@ MasterType VirtualPath="~/Site2.Master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <style type="text/css">
        /*CSS Classes For Design Modal*/
        .modalPopup {
            min-height: 75px;
            position: fixed;
            z-index: 2000;
            padding: 0;
            background-color: #fff;
            border-radius: 6px;
            background-clip: padding-box;
            border: 1px solid rgba(0, 0, 0, 0.2);
            min-width: 290px;
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
    </style>
    <link href="css/dialog.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </asp:ToolkitScriptManager>

    <%--<asp:UpdatePanel ID="updFormArea" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnUploadMedia" />
        </Triggers>
        <ContentTemplate>--%>
    <main>

        <div class="container-fluid">
            <nav aria-label="breadcrumb" class="my-3">
                <ol class="breadcrumb breadcrumb-info">
                    <li class="breadcrumb-item"><a href="#">Home</a></li>
                    <li class="breadcrumb-item active" aria-current="page">Send SMS</li>
                </ol>
            </nav>
            <!-- Content Row -->
            <div class="row">
                <div class="col-lg-9 col-xl-9">
                    <!-- Basic Card Example -->
                    <div class="card bg-primary border-light shadow-soft mb-4">
                        <div class="card-header py-3 bg-primary d-flex justify-content-between flex-wrap align-content-center">
                            <h6 class="m-0 font-weight-bold font-weight-bold my-auto"><i class="fas fa-comment-alt"></i>Send SMS</h6>
                        </div>
                        <div class="card-body">
                            <%--<form>--%>
                            <div id="divFileLoader" runat="server" style="display: none; text-align: center" class="form-group row">
                                <h3>File uploading. Please wait...</h3>
                                <img src="img/loading.gif" />
                            </div>
                            <div class="form-group row">
                                <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">SMS Type</label>
                                <div class="col-sm-5">
                                    <asp:DropDownList ID="ddlSMSType" runat="server" class="custom-select" AutoPostBack="true" OnSelectedIndexChanged="ddlSMSType_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <%--<asp:ListItem Text="Premium" Value="1" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Link Text" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Campaign" Value="4"></asp:ListItem>--%>

                                    <%--<asp:ListItem Text="OTP" Value="3"></asp:ListItem>--%>
                                </div>
                                <div class="col-sm-5">
                                    <asp:Label ID="lblRate" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group row">
                                <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">Sender ID</label>
                                <div class="col-sm-5">
                                    <asp:DropDownList ID="ddlSender" runat="server" class="custom-select">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <fieldset class="form-group">
                                <div class="row">
                                    </div>
                                    <legend class="col-form-label col-sm-2 pt-0 font-weight-bold">Mobile Numbers</legend>
                                    <div class="col-sm-10">
                                        <div class="custom-control custom-radio custom-control-inline pl-2">
                                            <asp:RadioButton class="mr-2" ID="rdbUpload" runat="server" GroupName="mobile" AutoPostBack="true" OnCheckedChanged="rdbUpload_CheckedChanged" />
                                            <%--<input type="radio" id="customRadioInline01" name="customRadioInline2" class="custom-control-input">--%>
                                            <label>
                                                Upload Files <i class="fas fa-file-csv text-primary"></i>
                                                <br>
                                                <small>(XLS/CSV/TXT)</small></label>
                                        </div>
                                        <div class="custom-control custom-radio custom-control-inline pl-2">
                                            <asp:RadioButton class="mr-2" ID="rdbPersonal" runat="server" GroupName="mobile" AutoPostBack="true" OnCheckedChanged="rdbUpload_CheckedChanged" />
                                            <label>Personalized SMS Uplaod XLS <i class="far fa-file-excel fa-1x text-success"></i></label>
                                        </div>
                                        <div class="custom-control custom-radio custom-control-inline pl-2">
                                            <asp:RadioButton class="mr-2" ID="rdbEntry" runat="server" Checked="true" GroupName="mobile" AutoPostBack="true" OnCheckedChanged="rdbUpload_CheckedChanged" />
                                            <label>Enter Number <i class="far fa-keyboard text-dark"></i></label>
                                        </div>
                                        <div class="custom-control custom-radio custom-control-inline pl-2">
                                            <asp:RadioButton class="mr-2" ID="rdbImport" runat="server" GroupName="mobile" AutoPostBack="true" OnCheckedChanged="rdbUpload_CheckedChanged" />
                                            <label>Import From Group <i class="fas fa-users text-info"></i></label>
                                        </div>
                                    </div>
                                </div>
                            </fieldset>
                            
                                <div id="divFileUpload" runat="server" class="form-group row d-none">  
                                    <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold" style="margin-top:-10px;">Mobile Numbers File&nbsp;</label>
                                    <div class="col-md-6">
                                        <div class="custom-file">
                                            <asp:FileUpload ID="FileUpload1" runat="server" class="custom-file-input" ClientIDMode="Static" onchange="if( SMSfileUpload() ) { console.log('formsubmit'); this.form.submit(); }" />
                                            <label class="custom-file-label" for="customFile">Choose file</label>
                                            <p class="text-danger small mt-2 mb-0">Maximum File Size 6 MB</p>
                                        </div>
                                    </div>
                                    <div class="4">
                                        <asp:Label ID="lblUploading" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div  id="divFileUpload3" runat="server"  class="form-group row d-none">
                                    <label for="inputEmail31" class="col-sm-2 col-form-label font-weight-bold" style="margin-top:-10px;">Exclude Mobile Numbers File&nbsp;</label>
                                    <div class="col-md-6">
                                        <div class="custom-file">
                                            <asp:FileUpload ID="FileUpload3" runat="server" class="custom-file-input" ClientIDMode="Static" onchange="if( SMSfileUpload3() ) { console.log('formsubmit'); this.form.submit(); }" />
                                            <label class="custom-file-label" for="customFile">Choose file</label>
                                            <p class="text-danger small mt-2 mb-0">Maximum File Size 6 MB</p>
                                        </div>
                                    </div>
                                    <div class="4">
                                        <asp:Label ID="lblUploading3" runat="server" ></asp:Label>
                                    </div>
                                </div>

                              <div id="divCamp" runat="server" class="form-group row d-none">
                                <label for="exampleFormControlTextarea1" class="col-sm-2 col-form-label font-weight-bold">Campaign Name</label>
                                <div class="col-md-6">
                                    <asp:TextBox class="form-control" ID="txtCampNm" runat="server" placeholder="Campaign Name" ToolTip="Campaign Name" />
                                </div>
                            </div>
                            
                            <div class="form-group row">
                                <label for="exampleFormControlTextarea1" class="col-sm-2 col-form-label font-weight-bold">Total Numbers</label>
                                <div class="col-sm-10">
                                    <div id="divNum" runat="server" style="pointer-events: all;">
                                        <asp:TextBox ID="txtMobNum" runat="server" class="form-control" TextMode="MultiLine" Rows="5" MaxLength="2147483647"  onkeyup="integersOnly(this); mobnumbcnt(); return true;"></asp:TextBox>
                                        <%--<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterMode="ValidChars"
                                                    TargetControlID="txtMobNum" ValidChars="0123456789, ">
                                                </asp:FilteredTextBoxExtender>--%>
                                    </div>
                                    <%--<textarea class="form-control" id="exampleFormControlTextarea1" rows="5"></textarea>--%>
                                    <%--<div class="form-check custom-control-inline">--%>

                                    <%--<input class="form-check-input" type="checkbox" id="gridCheck2">--%>
                                    <div class="d-flex">
                                        <p class="my-2 mr-3">
                                            <asp:CheckBox ID="chkRemoveDuplicates" runat="server" />
                                            <label class="form-check-label" for="gridCheck2">Remove Duplicates </label>
                                        </p>
                                        <p class="my-2">
                                            <span class="font-weight-bold"  style="font-size:smaller;">Number Count:</span>
                                            <asp:Label ID="lblMobileCnt" runat="server"  style="font-size:smaller;" ></asp:Label>
                                        </p>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <p class="my-2" >
                                            <span class="font-weight-bold" style="font-size:smaller;">Excude Number Count:</span>
                                            <asp:Label ID="lblExcludeCnt" runat="server"  style="font-size:smaller;" ></asp:Label>
                                        </p>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <p class="my-2">
                                            <span class="font-weight-bold" style="font-size:smaller;">SMS to be Sent On:</span>
                                            <asp:Label ID="lblActualMobCnt" runat="server"  style="font-size:smaller;" ></asp:Label>
                                        </p>
                                    </div>
                                    <%--</div>--%>
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="col">
                                    <div class="row">
                                        <label for="inputEmail3" class="col-lg-4 col-form-label font-weight-bold">Select Language</label>
                                        <div class="col-lg-8">
                                            <asp:DropDownList ID="ddlLanguage" runat="server" class="custom-select">
                                                <asp:ListItem Text="Hindi" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="English" Value="2" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Urdu" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="Arbic" Value="4"></asp:ListItem>
                                                <asp:ListItem Text="Marathi" Value="5"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="col">
                                    <div class="row">
                                        <div class="col-lg-6">
                                            <%--<a class="btn btn-primary" href="#" role="button" data-toggle="modal" data-target="#modal-insertLink"><i class="fas fa-fw fa-link text-secondary"></i>Insert Short URL</a>--%>
                                            <asp:LinkButton ID="LinkButton1" Visible="false" runat="server" class="mx-1 btn btn-primary text-secondary" OnClick="btnInsURL_Click"> 
                                                    <span class="text-secondary" style="font-size:smaller;"> <i class="fas fa-fw fa-link text-secondary"></i>Insert Short URL </span>
                                            </asp:LinkButton>
                                        </div>
                                        <div class="col-lg-6">
                                            <asp:LinkButton ID="LinkButton4" runat="server" class="mx-0 btn btn-primary text-secondary small" OnClick="btnInsTemplate_Click"> 
                                                    <span class="text-secondary" style="font-size:smaller;"> <i class="fas fa-fw fa-redo text-secondary"></i>Insert Template </span>
                                            </asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div id="divTemplateList" runat="server" style="display: none;">
                                <div id="div1" runat="server" class="form-group row">
                                    <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Select Template</label>
                                    <div class="col-sm-10">
                                        <asp:DropDownList ID="ddlTemplate" runat="server" class="custom-select" OnSelectedIndexChanged="ddlTemplate_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    </div>
                                </div>

                                <div id="div3" runat="server" class="form-group row">
                                    <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold"></label>
                                    <div class="col-sm-4">
                                        <label for="exampleFormControlTextarea2" class="col-form-label font-weight-bold">Template Fields</label>
                                    </div>
                                    <div class="col-sm-4">
                                        <label for="exampleFormControlTextarea2" class="col-form-label font-weight-bold">Uploaded XLS Fields</label>
                                    </div>
                                </div>

                                <div id="div2" runat="server" class="form-group row">
                                    <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold"></label>
                                    <div class="col-sm-4">
                                        <asp:ListBox ID="LstTemplateFld" Rows="5" runat="server" class="custom-select"></asp:ListBox>
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
                                <div id="div4" runat="server" class="form-group row">
                                    <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold"></label>
                                    <div class="col-sm-8">
                                        <label for="exampleFormControlTextarea2" class="col-form-label font-weight-bold">Mapped Fields</label>
                                    </div>
                                </div>
                                <div id="div5" runat="server" class="form-group row">
                                    <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold"></label>
                                    <div class="col-sm-8">
                                        <asp:ListBox ID="lstMappedFields" Rows="5" runat="server" class="custom-select"></asp:ListBox>
                                    </div>
                                    <div class="col-sm-2">
                                        <asp:LinkButton ID="btnUnMap" runat="server" class="mx-0 btn btn-primary text-secondary small" OnClick="btnUnMap_Click"> 
                                                    <span class="text-secondary" style="font-size:smaller; margin-top:20px;"> <i class="text-danger"></i>Break</span>
                                        </asp:LinkButton>
                                    </div>
                                </div>
                                <div id="div7" runat="server" class="form-group row">
                                    <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold"></label>
                                    <div class="col-sm-8">
                                        <label for="exampleFormControlTextarea2" class="col-form-label font-weight-bold">Message Preview</label>
                                    </div>
                                </div>
                                <div id="div6" runat="server" class="form-group row">
                                    <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold"></label>
                                    <div class="col-sm-10">
                                        <asp:TextBox ID="txtPreview" runat="server" TextMode="MultiLine" class="form-control" Rows="4"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group row">
                                <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">SMS Text</label>
                                <div class="col-sm-10">
                                    <div id="divMsg" runat="server" style="pointer-events: all;">
                                        <asp:TextBox ID="txtMsg" runat="server" TextMode="MultiLine" class="form-control" Rows="7" onkeyup="smscnt(); return true;"></asp:TextBox>
                                    </div>
                                    <%--<textarea class="form-control" id="exampleFormControlTextarea2" rows="7"></textarea>--%>
                                    <div class="d-flex justify-content-start">
                                        <p class="mb-1 mr-5">
                                            <span class="font-weight-bold">Used :</span>
                                            <asp:Label ID="lblused" runat="server" Text="0"></asp:Label>
                                        </p>
                                        <p class="mb-1 mr-5">
                                            <span class="font-weight-bold">SMS Count :</span>
                                            <asp:Label ID="lblsmscnt" runat="server" Text="0"></asp:Label>
                                        </p>
                                        <p class="mb-1">
                                            <%--<span class="font-weight-bold">Unicode :</span> --%>
                                            <asp:Label ID="lblUniCode" runat="server" Text="" Style="color: red; font-weight: bolder;"></asp:Label>
                                        </p>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group row">
													<label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Template ID</label>
													<div class="col-sm-10">
														<div id="div8" runat="server" style="pointer-events: all;">
															<asp:DropDownList ID="ddlTempID" runat="server" class="custom-select" OnSelectedIndexChanged="ddlTempID_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
														</div>
														<div class="d-flex justify-content-start">
															<p class="mb-1 mr-5">
																<span class="font-weight-bold">Template SMS: </span>
																<%--<asp:Label ID="lblTempSMS" runat="server" Text=""></asp:Label>--%>
															</p>
														</div>
													</div>
												</div>
												
												<div class="form-group row" style="margin-top:-20px;">
													<label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold"></label>
													<div class="col-sm-10">
														<div id="div9" runat="server" >
															<asp:TextBox ID="lblTempSMS" runat="server" TextMode="MultiLine" class="form-control" Rows="3" disabled="disabled"></asp:TextBox>
														</div>
													</div>
												</div>
												
                            <!-- Loader -->
                            <%-- <div class="o-page-loader">
                                        <div class="o-page-loader--content">
                                            <div class="o-page-loader--spinner"></div>
                                            <div class="o-page-loader--message">
                                                <span>Loading...</span>
                                            </div>
                                        </div>
                                    </div>--%>
                            <!--End Loader -->

                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="form-group row justify-content-end">
                                        <div class="col-sm-10 ">
                                            <asp:LinkButton ID="LinkButton2" runat="server" class="mx-1 btn btn-primary text-secondary"
                                                OnClientClick="return ConfirmSubmit();"
                                                OnClick="btnSend_Click" data-toggle="tooltip" data-placement="top"> 
                                                        <span class="text-secondary"> <i class="fas fa-paper-plane"></i>Send</span></asp:LinkButton>

                                            <asp:LinkButton ID="LinkButton3" runat="server" class="mx-1 btn btn-primary text-danger"
                                                OnClick="btnCancel_Click" data-toggle="tooltip" data-placement="top"> 
                                                        <span class="text-danger"> <i class="fas fa-times"></i>Cancel</span></asp:LinkButton>


                                            <asp:LinkButton ID="LinkButton5" runat="server" class="mx-1 btn btn-primary text-warning"
                                                OnClick="btnSchedule_Click" data-toggle="tooltip" data-placement="top"> 
                                                        <span class="text-warning"> <i class="fas fa-clock"></i>Schedule</span></asp:LinkButton>

                                        </div>
                                    </div>

                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1"
                                DisplayAfter="0">
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
                                        <asp:Label ID="lblStat" runat="server" Visible="false"></asp:Label>
                                    </h2>
                                </div>
                            </div>
                            <%--</form>--%>
                        </div>
                    </div>
                </div>
            </div>
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
    <asp:Panel ID="pnlPopUp_SCHEDULE" runat="server" CssClass="modalPopup"
        Style="display: none;">
        <div style="overflow-y: auto; overflow-x: hidden; max-height: 450px;">
            <div class="modal-header">
                <asp:Label ID="Label2" runat="server" CssClass="modal-title" Text="Schedule SMS"></asp:Label>
            </div>
            <div class="modal-body">
                <div>
                    <div>
                        Schedule Date :
                            <asp:TextBox ID="txtScheduleDate" runat="server" onchange="javascript:text_changed_from();" class="form-control datepicker" placeholder="Scheduled Date"></asp:TextBox>
                        <asp:HiddenField ID="hdnScheduleDate" runat="server" />
                        <%-- <asp:CalendarExtender ID="txtScheduleDate_Ext" runat="server" Format="dd/MMM/yyyy" TargetControlID="txtScheduleDate">
                            </asp:CalendarExtender>--%>
                    </div>
                    <div>
                        Time (HH:MM) :
                                <asp:TextBox ID="txtTime" ToolTip="Enter time in HH:MM format. Entered time will not be deleted. You can overwrite the time."
                                    onkeypress="return false" onkeyup="return ValTime();" OnPaste="return false"
                                    runat="server" MaxLength="6" Width="35%" Enabled="true" class="form-control"></asp:TextBox>
                        <asp:MaskedEditExtender ID="MaskedEditExtendertxtTime" runat="server"
                            TargetControlID="txtTime" Mask="99:99" MessageValidatorTip="true"
                            OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Time"
                            AcceptAMPM="false" ErrorTooltipEnabled="True" />

                        <%-- <asp:TextBox ID="txtTime11" runat="server" class="form-control"></asp:TextBox>--%>
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
                        <button id="btnCancel1" runat="server" class="btn btn-primary">Cancel</button>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:LinkButton ID="lnkSCH" runat="server"></asp:LinkButton>
    <asp:ModalPopupExtender ID="pnlPopUp_SCHEDULE_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground"
        PopupControlID="pnlPopUp_SCHEDULE" TargetControlID="lnkSCH" CancelControlID="btnCancel1">
    </asp:ModalPopupExtender>
    <%--  Schedule POPUP --%>


    <%-- Group POPUP --%>
    <asp:Panel ID="pnlPopUp_GROUP" runat="server" CssClass="modalPopup" Style="display: none;">
        <div style="overflow-y: auto; overflow-x: hidden; max-height: 450px;">
            <div class="modal-header">
                <asp:Label ID="Label1" runat="server" CssClass="modal-title" Text="Group"></asp:Label>
            </div>
            <div class="modal-body">
                <div>
                    <asp:DropDownList ID="ddlGrp" runat="server" class="custom-select">
                    </asp:DropDownList>
                </div>
            </div>
            <div align="center" class="modal-footer">
                <div class="row">
                    <div class="col-md-12">
                        <asp:Button ID="btnAddGroup" runat="server" Text="Select Group" class="btn btn-primary" OnClick="btnAddGroup_Click" />
                        <button id="btnCancel2" runat="server" class="btn btn-primary">Cancel</button>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:LinkButton ID="lnkGroup" runat="server"></asp:LinkButton>
    <asp:ModalPopupExtender ID="pnlPopUp_GROUP_ModalPopupExtender" runat="server" BackgroundCssClass="modalBackground"
        PopupControlID="pnlPopUp_GROUP" TargetControlID="lnkGroup" CancelControlID="btnCancel2">
    </asp:ModalPopupExtender>
    <%-- Group POPUP --%>


    <%-- URL POPUP --%>
    <%--lnkFake Link Button for mpeAddUpdateEmployee ModalPopup as TargetControlID--%>
    <asp:LinkButton ID="lnkURL" runat="server"></asp:LinkButton>

    <%--pnlPopUp_URL Panel With Design--%>
    <asp:Panel ID="pnlPopUp_URL" runat="server" CssClass="modalPopup" Style="display: none;">
        <div class="modal-content">
            <div class="modal-header">
                <asp:Label ID="lblHeading" runat="server" CssClass="modal-title" Text="Load Short URL"></asp:Label>
            </div>
            <div class="modal-body">
                <div class="mb-3">
                    <asp:Button ID="btnExisting" Text="Exsting" runat="server" OnClick="btnExisting_Click" CssClass="btn btn-primary" />
                    <asp:Button ID="btnNew" Text="Add New" runat="server" OnClick="btnNew_Click" CssClass="btn btn-primary" />
                    <asp:Button ID="btnMedia" Text="Add Media" runat="server" OnClick="btnMedia_Click" CssClass="btn btn-primary" />
                </div>

                <div class="">
                    <asp:Panel ID="pnlExisting" runat="server" Visible="false">
                        <div>
                            <asp:Label ID="Label3" runat="server" Text="Short URL : "></asp:Label>

                            <asp:DropDownList ID="ddlURL" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlURL_OnSelectedIndexChanged" class="drop-select form-control mb-3"></asp:DropDownList>

                            <asp:Button ID="btnShowLongURL" runat="server" Text="Show Long URL" class="btn btn-primary" OnClick="btnShowLongURL_Click" />

                            <div>
                                <asp:Label ID="lblLongUrl" runat="server" Style="font-size: 14px;" Text=""></asp:Label>
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
                                    <%--<input type="file" class="custom-file-input" id="customFile">--%>
                                    <label class="custom-file-label" for="customFile">Choose file</label>
                                    <%--<asp:Label ID="lblfn" runat="server" class="custom-file-label" for="customFile" Text="Choose file"></asp:Label>--%>
                                </div>
                                <%--<div class="custom-file">
                                            <asp:FileUpload ID="FileUpload3" runat="server" />
                                            <asp:Button ID="btnUploadMedia" runat="server" Text="Upload" class="btn btn-primary " OnClick="btnUpload_Click" />
                                        </div>--%>
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
    <asp:ModalPopupExtender ID="pnlPopUp_URL_ModalPopupExtender" runat="server" PopupControlID="pnlPopUp_URL"
        TargetControlID="lnkURL" BehaviorID="mpeAddUpdateEmployee" CancelControlID="btnCancel"
        BackgroundCssClass="modalBackground">
    </asp:ModalPopupExtender>
    <%-- URL POPUP --%>
    <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
    <script src="js/dialog.js"></script>
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
            document.getElementById('<%= lblUploading.ClientID %>').innerHTML = "Uploading. Please Wait ... ";

        }
        function SMSfileUpload3() {
            var uploadControl = document.getElementById('<%= FileUpload3.ClientID %>');
            var myfile = uploadControl.value;
            console.log(myfile);
            var Extension = myfile.substring(myfile.lastIndexOf('.') + 1).toLowerCase();
            console.log(Extension);
            if (Extension == "txt") {
                if (uploadControl.files[0].size > 6291456) {
                    alert("Upload text file of size upto 6 MB only.");
                    return false;
                }
                else {
                    showLoading();
                    console.log("ret true");
                    return true;
                }
            }
            if (Extension == "xls" || Extension == "xlsx") {
                if (uploadControl.files[0].size > 5242880) {
                    alert("Upload Excel file of size upto 5 MB only.");
                    return false;
                }
                else {
                    showLoading();
                    console.log("ret true");
                    return true;
                }
            }
        }
        function SMSfileUpload() {
            var uploadControl = document.getElementById('<%= FileUpload1.ClientID %>');
            var myfile = uploadControl.value;
            console.log(myfile);
            var Extension = myfile.substring(myfile.lastIndexOf('.') + 1).toLowerCase();
            console.log(Extension);
            if (Extension == "txt") {
                if (uploadControl.files[0].size > 6291456) {
                    alert("Upload text file of size upto 6 MB only.");
                    return false;
                }
                else {
                    showLoading();
                    console.log("ret true");
                    return true;
                }
            }
            if (Extension == "xls" || Extension == "xlsx") {
                if (uploadControl.files[0].size > 5242880) {
                    alert("Upload Excel file of size upto 5 MB only.");
                    return false;
                }
                else {
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
            if ( (charCode > 48 && charCode < 57) || charCode == 44 || charCode == 13) {
                return true;
            }
            else
            {
                return false;
            }

        }
        function integersOnly(obj) {
            obj.value = obj.value.replace(/[^0-9,\r\n]/g,'');
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
                }
                else
                    document.getElementById('<%= lblMobileCnt.ClientID %>').innerHTML = "";
                if (s != '' && x == 0)
                    document.getElementById('<%= lblMobileCnt.ClientID %>').innerHTML = "1";
            }
            else {
                document.getElementById('<%= lblMobileCnt.ClientID %>').innerHTML = "";
            }
        }

        function ConfirmSubmit() {
            var s = document.getElementById("<%=txtMsg.ClientID%>").value;
            var tt = 0;
            tt = s.split(/\r\n|\r|\n/).length;
            if (tt >= 1) tt = tt - 1;
            if (s != '') {
                var i = 0;
                var ln = s.length + tt;
                for (var k = 0, n = s.length; k < n; k++) {
                    if (s.charAt(k) == "~" || s.charAt(k) == "|" ) { ln = ln + 1; }
                }

                console.log(document.getElementById('<%= ddlSMSType.ClientID %>').val);
                console.log(document.getElementById('<%= ddlSMSType.ClientID %>').value);
                if (document.getElementById('<%= ddlSMSType.ClientID %>').value == "2") ln = ln + 2;
                i = getLength(ln);

                var y = 0;
                for (var k = 0, n = s.length; k < n; k++) {
                    if (s.charCodeAt(k) > 255) { y = 1; }
                }
                if (y == 1) {
                    ln = s.length;
                    i = getUniCodeLength(ln);
                }
                if (i > 1) {
                    if (confirm('This message will cost \n\n' + i + ' SMS per message. \n\nDo you want to continue ? ')) {
                        console.log('ok clicked.');
                        if (confirm('This message will cost \n\n' + i + ' SMS per message. \n\n Are you sure you want to continue ? ')) {
                            return true;
                        }
                        else {
                            document.getElementById('<%= divSchedule.ClientID %>').style.display = 'none';
                            return false;
                        }
                    }
                    else {
                        document.getElementById('<%= divSchedule.ClientID %>').style.display = 'none';
                        return false;
                    }
                }
                else
                    return true;
            }
            else
                return true;
        }

        function ConfirmSubmit2() {
            var s = document.getElementById("<%=txtMsg.ClientID%>").value;
            var tt = 0;
            tt = s.split(/\r\n|\r|\n/).length;
            if (tt >= 1) tt = tt - 1;
            if (s != '') {
                var i = 0;
                var ln = s.length + tt;
                for (var k = 0, n = s.length; k < n; k++) {
                    if (s.charAt(k) == "~" || s.charAt(k) == "|" ) { ln = ln + 1; }
                }

                console.log(document.getElementById('<%= ddlSMSType.ClientID %>').val);
                console.log(document.getElementById('<%= ddlSMSType.ClientID %>').value);
                if (document.getElementById('<%= ddlSMSType.ClientID %>').value == "2") ln = ln + 2;
                i = getLength(ln);

                var y = 0;
                for (var k = 0, n = s.length; k < n; k++) {
                    if (s.charCodeAt(k) > 255) { y = 1; }
                }
                if (y == 1) {
                    ln = s.length;
                    i = getUniCodeLength(ln);
                }
                if (i > 1) {
                    if (confirm('This message will cost ' + i + ' SMS per message. Do you want to continue ? ')) {
                        if (confirm('This message will cost ' + i + ' SMS per message. \n\n Are you sure you want to continue ? ')) {
                            return true;
                        }
                        else return false;
                    }
                    else return false;
                }
                else
                    return true;
            }
            else
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
            tt = s.split(/\r\n|\r|\n/).length;
            if (tt >= 1) tt = tt - 1;
            if (s != '') {
                var i = 0;
                var ln = s.length + tt;
                for (var k = 0, n = s.length; k < n; k++) {
                    if (s.charAt(k) == "~" || s.charAt(k) == "|" ) { ln = ln + 1; }
                }
                
                console.log(document.getElementById('<%= ddlSMSType.ClientID %>').val);
                console.log(document.getElementById('<%= ddlSMSType.ClientID %>').value);

                if (document.getElementById('<%= ddlSMSType.ClientID %>').value == "2") ln = ln + 2;
                i = getLength(ln);
                document.getElementById('<%= lblUniCode.ClientID %>').innerHTML = "";

                document.getElementById('<%= lblsmscnt.ClientID %>').innerHTML = "No. of Char: " + ln + ". <br />No. of SMS: " + i.toString();
                var y = 0;
                for (var k = 0, n = s.length; k < n; k++) {
                    if (s.charCodeAt(k) > 255) { y = 1; }
                }
                if (y == 1) {
                    ln = s.length;
                    i = getUniCodeLength(ln);
                    document.getElementById('<%= lblUniCode.ClientID %>').innerHTML = "UNICODE : YES";

                    document.getElementById('<%= lblsmscnt.ClientID %>').innerHTML = "No. of Char: " + ln + ". <br />No. of SMS: " + i.toString();
                }
            }
            else
                document.getElementById('<%= lblsmscnt.ClientID %>').innerHTML = "";
        }

    </script>

    <%-- <script>
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
    </script>--%>
</asp:Content>
