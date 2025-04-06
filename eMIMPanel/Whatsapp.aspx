<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" EnableEventValidation="false" CodeBehind="Whatsapp.aspx.cs" Inherits="eMIMPanel.Whatsapp" %>

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
            width: 50%;
            /*height:90%;*/
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

        /*  */
    </style>
    <link href="ms-Dropdown-master/css/msdropdown/dd.css" rel="stylesheet" />
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>
    <script type="text/javascript" src="ms-Dropdown-master/js/jquery/jquery-1.9.0.min.js"></script>
    <script type="text/javascript" src="ms-Dropdown-master/js/msdropdown/jquery.dd.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </asp:ToolkitScriptManager>
    <%-- <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>--%>
    <main>
        <div class="container-fluid">

            <%--pnlPopUp_WA Panel With Design--%>
            <asp:Panel ID="pnlPopUp_WA" runat="server" CssClass="modalPopup" Style="display: none;">
                <div class="modal-content">
                    <div class="modal-header">
                        <asp:Label ID="lblHeading" runat="server" CssClass="modal-title" Text="Preview"></asp:Label>
                    </div>
                    <div class="modal-body">
                        <div class="row">

                            <div class="col-sm-7" runat="server" visible="true" id="previewWA">
                                <div class="resp-container">

                                    <iframe class="resp-iframe" id="myIframe" src="wa_/WAPreview.aspx" height="600px" frameborder="0" width="100%"></iframe>
                                </div>
                                <asp:TextBox runat="server" ID="txtpreview" ReadOnly="true" TextMode="MultiLine" class="form-control d-none" Rows="4" placeholder="Message Preview"></asp:TextBox>
                            </div>
                            <div class="col-sm-7" runat="server" id="previewRCS" visible="false">
                                <div class="resp-container">
                                    <iframe id="myIframe1" src="rcs/RCSPreview.aspx" height="600px" frameborder="0" width="100%"></iframe>
                                </div>
                                <asp:TextBox runat="server" ID="TextBox1" ReadOnly="true" TextMode="MultiLine" class="form-control d-none" Rows="4" placeholder="Message Preview"></asp:TextBox>
                            </div>

                            <div class="col-sm-5">
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="d-flex justify-content-between">
                                            <p class="my-2">
                                                <span class="font-weight-bold">Recipient :</span>
                                                <asp:Label ID="lblRecipientcnt" runat="server" Text="0"></asp:Label>
                                            </p>
                                            <p class="my-2">
                                                <span class="font-weight-bold">Message :</span>
                                                <asp:Label ID="lblMessagecnt" runat="server" Text="0"></asp:Label>
                                            </p>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-sm-12">
                                        <asp:LinkButton ID="lnkbtnSend" runat="server" class="btn btn-primary text-secondary btn-block mt-5" OnClientClick="return ConfirmSubmit();" OnClick="lnkbtnSend_Click" data-toggle="tooltip" data-placement="top">
                                                        <span class="text-secondary"> <i class="fas fa-paper-plane"></i> Send</span>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="btnCancel" runat="server" class="btn btn-primary text-danger btn-block mt-3" data-toggle="tooltip" data-placement="top">
                                                        <span class="text-danger"> <i class="fas fa-times"></i> Cancel</span>
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>

                        </div>

                    </div>
                    <div class="modal-footer">
                        <%--<button id="btnCancel" runat="server" class="btn btn-primary">Cancel</button>--%>
                    </div>
                </div>
            </asp:Panel>

            <asp:Button runat="server" CssClass="d-none" ID="btnshow" />
            <%--pnlPopUp_WA Modal Popup Extender For pnlPopUp_WA--%>
            <asp:ModalPopupExtender ID="pnlPopUp_WA_ModalPopupExtender" runat="server" PopupControlID="pnlPopUp_WA" TargetControlID="btnshow" BehaviorID="mpeAddUpdateEmployee" CancelControlID="btnCancel" BackgroundCssClass="modalBackground">
            </asp:ModalPopupExtender>

            <div class="row">
                <div class="col-lg-10 col-xl-11">
                    <!-- Start Card -->
                    <div class="card bg-primary border-light shadow-soft mb-4">


                        <div class="card-header py-3 bg-primary justify-content-between flex-wrap align-content-center">

                            <div class="row">
                                <div class="col-sm-9">
                                    <h6 class="m-0 font-weight-bold font-weight-bold my-auto"><i class="fas fa-comment-alt"></i>Whatsapp/RCS</h6>

                                </div>
                                <div class="col-sm-3">
                                    <h6 class="m-0 font-weight-bold font-weight-bold my-auto"><i class="fas fa-wallet"></i>Balance : <span id="lblbal" runat="server"></span></h6>

                                </div>

                            </div>

                        </div>

                        <div class="card-body pt-0">

                            <div class="form-group row">
                                <div class="col-sm-4">
                                    <div class="btn-group btn-group-toggle btn-block mb-3 mb-lg-0" data-toggle="buttons">
                                        <label class="btn text-secondary active">
                                            <img src="assets/icon/whatsapp-business.svg" alt="whatsapp-business" width="28px">
                                            <%--<input type="radio" name="videoUp1" value="videoUp1" checked>--%>
                                            <asp:RadioButton ID="rdbWhatsapp" runat="server" AutoPostBack="true" GroupName="vi" Checked="true" />
                                            Whatsapp
                                        </label>
                                        <label class="btn text-secondary">
                                            <img src="assets/icon/google_icon.svg" alt="google_icon.svg" width="20px">
                                            <%--<input type="radio" name="ImgUp1" value="ImgUp1">--%>
                                            <asp:RadioButton ID="rdbRCS" runat="server" AutoPostBack="true" GroupName="vi" />
                                            RCS
                                        </label>
                                    </div>
                                </div>


                                <label for="exampleFormControlTextarea1" class="col-sm-2 col-form-label font-weight-bold">Total Numbers</label>
                                <div class="col-sm-6">

                                    <asp:TextBox ID="txtMobNum" runat="server" class="form-control" TextMode="MultiLine" Rows="5" MaxLength="2147483647" onkeyup="integersOnly(this); return true;"></asp:TextBox>

                                    <div class="d-flex">
                                        <span class="text-danger small mt-2 mb-0 small"><b>Enter Mobile Numbers With Country Code (Maximum 5 Numbers)</b></span>
                                    </div>
                                </div>
                            </div>

                            <div id="div1" runat="server" class="form-group row">
                                <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Template</label>
                                <div class="col-sm-4">
                                    <asp:DropDownList ID="ddlwatempate" OnSelectedIndexChanged="ddlwatempate_SelectedIndexChanged" AutoPostBack="true" runat="server" class="custom-select"></asp:DropDownList>
                                </div>

                                <div class="col-sm-6">
                                    <asp:TextBox runat="server" ID="txtTemplatePreview" TextMode="MultiLine" class="form-control" Rows="4" placeholder="Preview"></asp:TextBox>
                                </div>
                            </div>

                            <hr />

                            <%--  <div class="row">
                                <div class="col-md-3">
                                    Schedule Date : 
                            <asp:Label ID="lblScheduleDate" Visible="false" runat="server" ClientIDMode="Static"></asp:Label>
                                    <asp:TextBox ID="txtScheduleDate" runat="server" onchange="javascript:text_changed_from();" class="form-control" TextMode="Date" placeholder="Scheduled Date"></asp:TextBox>
                                    <asp:HiddenField ID="hdnScheduleDate" runat="server" />

                                </div>
                                <div class="col-md-2">
                                    Time (HH:MM) :
                            <asp:TextBox ID="txtTime" ToolTip="Enter time in HH:MM format. Entered time will not be deleted. You can overwrite the time." onkeypress="return false" onkeyup="return ValTime();" OnPaste="return false" runat="server" MaxLength="6" Width="35%" Enabled="true"
                                class="form-control"></asp:TextBox>
                                    <asp:MaskedEditExtender ID="MaskedEditExtendertxtTime" runat="server" TargetControlID="txtTime" Mask="99:99" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" OnInvalidCssClass="MaskedEditError" MaskType="Time" AcceptAMPM="false" ErrorTooltipEnabled="True" />
                                </div>

                            </div>
                            <hr />--%>

                            <div class="row">
                                <div class="col-sm-6">



                                    <asp:TextBox class="form-control" ID="txtCampNm" runat="server" placeholder="Campaign Name" ToolTip="Campaign Name" />


                                </div>
                                <div class="col-sm-3">
                                    <asp:LinkButton ID="lnkbtnContinue" OnClick="lnkbtnContinue_Click" runat="server" class="btn btn-primary text-warning btn-block">
                                                        <span class="text-warning"> <i class="fas fa-arrow-alt-circle-right"></i> Continue To Preview</span>
                                    </asp:LinkButton>
                                </div>
                                <div class="col-sm-3">
                                    <asp:LinkButton ID="lnkbtnCancel" runat="server" class="btn btn-primary text-danger btn-block my-3 my-lg-0" data-toggle="tooltip" data-placement="top" OnClick="lnkbtnCancel_Click">
                                                        <span class="text-danger"> <i class="fas fa-times"></i> Cancel</span>
                                    </asp:LinkButton>
                                </div>
                            </div>



                        </div>

                    </div>
                </div>
            </div>

        </div>
    </main>
    <%--   </ContentTemplate>
    </asp:UpdatePanel>--%>

    <!-- Select2 CSS -->
    <!-- jQuery -->

    <!-- Select2 -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js"></script>
    <script>
        function integersOnly(obj) {
            obj.value = obj.value.replace(/[^0-9,\r\n]/g, '');
        }
        //$("#ddlwatempate").select2({
        //    allowClear: true
        //});
    </script>
</asp:Content>
