<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="sms-whatsapp_u.aspx.cs" Inherits="eMIMPanel.sms_whatsapp_u" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="updFormArea" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSend" />
        </Triggers>
        <ContentTemplate>
            <main>
                <div class="container-fluid">
                    <nav aria-label="breadcrumb" class="my-3">
                        <ol class="breadcrumb breadcrumb-info">
                            <li class="breadcrumb-item"><a href="#">Home</a></li>
                            <li class="breadcrumb-item"><a href="#">Linkext</a></li>
                            <li class="breadcrumb-item active" aria-current="page">SMS - Whatsapp</li>
                        </ol>
                    </nav>

                    <!-- Content Row -->
                    <div class="row justify-content-center">
                        <div class="col-12 col-md-12 col-lg-12">
                            <!--Accordion-->
                            <div class="accordion shadow-soft rounded">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div class="card card-sm card-body bg-primary border-light mb-0">
                                            <a href="#panel-4" data-target="#panel-4" class="accordion-panel-header" data-toggle="collapse" role="button" aria-expanded="false" aria-controls="panel-1">
                                                <span class="icon-title h6 mb-0 font-weight-bold"><span class="fas fa-comment-alt"></span>SMS - Whatsapp</span>
                                                <span class="icon"><span class="fas fa-plus"></span></span>
                                            </a>
                                            <div class="collapse show" id="panel-4">
                                                <div class="pt-3">
                                                    <form>
                                                        <fieldset class="form-group">
                                                            <div class="row">
                                                                <legend class="col-form-label col-sm-2 pt-0 font-weight-bold">Country Code:</legend>
                                                                <div class="col-sm-6">
                                                                    <asp:DropDownList ID="ddlCCode" runat="server" class="custom-select" AutoPostBack="true" OnSelectedIndexChanged="ddlCCode_SelectedIndexChanged"></asp:DropDownList>
                                                                </div>
                                                            </div>
                                                            <div id="divSender" runat="server">
                                                                <div class="row mt-2">
                                                                    <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">Sender ID</label>
                                                                    <div class="col-sm-5">
                                                                        <asp:DropDownList ID="ddlSender" runat="server" class="custom-select">
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                </div>
                                                            </div>

                                                            <div class="row mt-2">
                                                                <legend class="col-form-label col-sm-2 pt-0 font-weight-bold">Mobile Number:</legend>
                                                                <div class="col-sm-10">
                                                                    <asp:TextBox ID="txtMobile" runat="server" type="text" MaxLength="10" class="form-control" aria-describedby="textHelp1" placeholder="Enter Mobile Number"></asp:TextBox>
                                                                    <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterMode="ValidChars"
                                                                        TargetControlID="txtMobile" ValidChars="0123456789">
                                                                    </asp:FilteredTextBoxExtender>
                                                                    <small id="textHelp1" class="form-text text-muted">Without Country Code</small>
                                                                </div>
                                                            </div>
                                                        </fieldset>
                                                        <div class="form-group row">
                                                            <div class="col-sm-2 col-lg-2"></div>
                                                            <div class="col-sm-3 col-lg-3">
                                                                <asp:LinkButton ID="btnSend" runat="server" class="btn btn-primary my-2 my-lg-0 btn-block" OnClick="btnSend_Click"><i class="fas fa-thumbs-up"></i>Verify</asp:LinkButton>
                                                            </div>
                                                            <div id="divVerified" runat="server" style="display: none;">
                                                                <h4 style="color: Green;"><span class="fas fa-check" />Verified</h4>
                                                            </div>
                                                        </div>
                                                        <div id="divOTP" runat="server" style="display: none;">
                                                            <fieldset class="form-group">
                                                                <div class="row">
                                                                    <legend class="col-form-label col-sm-2 pt-0 font-weight-bold">OTP</legend>
                                                                    <div class="col-sm-10">
                                                                        <asp:TextBox runat="server" type="text" class="form-control" ID="txtOTP" aria-describedby="genHelp" placeholder="Enter OTP"></asp:TextBox>
                                                                        <small id="genHelp" class="form-text text-muted">Please Check Phone SMS</small>
                                                                    </div>
                                                                </div>
                                                            </fieldset>
                                                            <div class="form-group row justify-content-end">
                                                                <div class="col-sm-5">
                                                                    <asp:LinkButton ID="btnOTPSubmit" runat="server" OnClick="btnOTPSubmit_Click" class="btn btn-primary my-2 my-lg-0"><i class="fas fa-thumbs-up"></i>Submit</asp:LinkButton>
                                                                    <%--<asp:LinkButton ID="btnResendOTP" runat="server" OnClick="btnResedOTP_Click" class="btn btn-primary my-2 my-lg-0"><i class="fas fa-thumbs-up"></i>Submit</asp:LinkButton>--%>
                                                                </div>
                                                                <div id="divReSendOTP" runat="server" style="display: none;" class="col-sm-5">
                                                                    <asp:LinkButton ID="btnResendOTP" runat="server" OnClick="btnResendOTP_Click" class="btn btn-primary my-2 my-lg-0"><i class="fas fa-thumbs-up"></i>Resend OTP</asp:LinkButton>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </form>
                                                </div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel2"
                                    DisplayAfter="0">
                                    <ProgressTemplate>
                                        <div align="center" style="position: absolute; left: 40%; top: 50%; vertical-align: middle;">
                                            <img src="Img/loading.gif" />
                                        </div>
                                        <div align="center" style="position: absolute; left: 0%; top: 0%; vertical-align: middle;">
                                        </div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                                <div id="divBelow" runat="server" style="display: none;">
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <div class="card card-sm card-body bg-primary border-light mb-0">
                                                <a href="#panel-5" data-target="#panel-5" class="accordion-panel-header" data-toggle="collapse" role="button" aria-expanded="false" aria-controls="panel-1">
                                                    <span class="icon-title h6 mb-0 font-weight-bold"><span class="fas fa-fw fa-link"></span>Create Short URL:</span>
                                                    <span class="icon"><span class="fas fa-plus"></span></span>
                                                </a>
                                                <div class="collapse show" id="panel-5">
                                                    <div class="pt-3">
                                                        <%-- <form>--%>
                                                        <fieldset class="form-group">
                                                            <div class="row">
                                                                <legend class="col-form-label col-sm-2 pt-0 font-weight-bold">Message:</legend>
                                                                <div class="col-sm-10">
                                                                    <asp:TextBox ID="txtMsg" runat="server" class="form-control" TextMode="MultiLine" Rows="3" placeholder="Enter Text Message"></asp:TextBox>
                                                                    <small id="textHelp2" class="form-text text-muted">Enter Required Pretyped Text</small>
                                                                </div>
                                                            </div>
                                                        </fieldset>
                                                        <fieldset class="form-group">
                                                            <div class="row">
                                                                <legend class="col-form-label col-sm-2 pt-0 font-weight-bold">Keyword</legend>
                                                                <div class="col-sm-10">
                                                                    <asp:TextBox ID="txtShortURL" runat="server" type="text" class="form-control" aria-describedby="textHelp3" placeholder="Enter word of your own choice or leave blank for autogenrate"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </fieldset>
                                                        <div class="form-group row justify-content-end">
                                                            <div class="col-sm-10">
                                                                <asp:LinkButton ID="btnSMSUrl" runat="server" class="btn btn-primary text-secondary" OnClick="btnSMSUrl_Click">Generate SMS Short URL</asp:LinkButton>
                                                                <asp:LinkButton ID="btnWAUrl" runat="server" class="btn btn-primary text-success" OnClick="btnWAUrl_Click">Generate WhatsApp Short URL</asp:LinkButton>
                                                            </div>
                                                            <div class="col-sm-10" style="font-size: 14px; margin-top: 10px;">
                                                                <asp:Label ID="lblShortURL" runat="server" Text="" Style="font-size: 20px; color: darkblue;"></asp:Label>
                                                            </div>
                                                        </div>

                                                        <%--  </form>--%>
                                                    </div>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <div class="card card-sm card-body bg-primary border-light mb-0">
                                        <a href="#panel-6" data-target="#panel-6" class="accordion-panel-header" data-toggle="collapse" role="button" aria-expanded="false" aria-controls="panel-1">
                                            <span class="icon-title h6 mb-0 font-weight-bold"><span class="fas fa-qrcode"></span>Create QR:</span>
                                            <span class="icon"><span class="fas fa-plus"></span></span>
                                        </a>
                                        <div class="collapse show" id="panel-6">
                                            <div class="pt-3">
                                                <%-- <form>--%>
                                                <%--<fieldset class="form-group">
                                                    <div class="row">
                                                        <legend class="col-form-label col-sm-2 pt-0 font-weight-bold">Message:</legend>
                                                        <div class="col-sm-10">
                                                            <asp:TextBox ID="txtMsg2" runat="server" class="form-control" TextMode="MultiLine" Rows="5" placeholder="Enter Text Message"></asp:TextBox>
                                                            <small id="textHelp" class="form-text text-muted">Enter Required Pretyped Text</small>
                                                        </div>
                                                    </div>
                                                </fieldset>--%>
                                                <div class="form-group row justify-content-end">
                                                    <div class="col-sm-10 ">
                                                        <asp:LinkButton ID="btnSMSQR" runat="server" class="btn btn-primary text-secondary" OnClick="btnSMSQR_Click">
                                                            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-message-square">
                                                                <path d="M21 15a2 2 0 0 1-2 2H7l-4 4V5a2 2 0 0 1 2-2h14a2 2 0 0 1 2 2z"></path>
                                                            </svg>
                                                            Generate SMS QRCode</asp:LinkButton>

                                                        <asp:LinkButton ID="btnWAQR" runat="server" class="btn btn-primary text-success" OnClick="btnWAQR_Click">
                                                            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-message-circle">
                                                                <path d="M21 11.5a8.38 8.38 0 0 1-.9 3.8 8.5 8.5 0 0 1-7.6 4.7 8.38 8.38 0 0 1-3.8-.9L3 21l1.9-5.7a8.38 8.38 0 0 1-.9-3.8 8.5 8.5 0 0 1 4.7-7.6 8.38 8.38 0 0 1 3.8-.9h.5a8.48 8.48 0 0 1 8 8v.5z"></path>
                                                            </svg>
                                                            Generate WhatsApp QRCode</asp:LinkButton>
                                                    </div>
                                                </div>
                                                <%--</form>--%>
                                            </div>
                                        </div>
                                    </div>

                                    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel3"
                                        DisplayAfter="0">
                                        <ProgressTemplate>
                                            <div align="center" style="position: absolute; left: 40%; top: 50%; vertical-align: middle;">
                                                <img src="Img/loading.gif" />
                                            </div>
                                            <div align="center" style="position: absolute; left: 0%; top: 0%; vertical-align: middle;">
                                            </div>
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                    <div class="card card-sm card-body bg-primary border-light mb-0">
                                        <a href="#panel-7" data-target="#panel-7" class="accordion-panel-header" data-toggle="collapse" role="button" aria-expanded="false" aria-controls="panel-1">
                                            <span class="icon-title h6 mb-0 font-weight-bold"><span class="fas fa-qrcode"></span>QR Code:</span>
                                            <span class="icon"><span class="fas fa-plus"></span></span>
                                        </a>
                                        <div class="collapse show" id="panel-7">
                                            <div class="pt-3">
                                                <h3 class="text-center font-weight-bold mb-3 h4">QRCode for
                    <asp:Label ID="lblQR4" runat="server"></asp:Label>
                                                </h3>
                                                <div class="row justify-content-center">
                                                    <div class="card card-body shadow border-0 col-lg-4 p-2">
                                                        <asp:Image ID="imgQR" runat="server" />
                                                        <%--<canvas id="qr" class="w-100"></canvas>--%>
                                                    </div>
                                                </div>
                                                <div class="text-center mt-3">
                                                    <asp:LinkButton ID="btnDownload" runat="server" class="btn btn-primary text-success" OnClick="btnDwQR_Click">
                                                        <span class="icon text-success"><i class="fas fa-download"></i></span>
                                                        <span class="text">Download</span>
                                                    </asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <!--End of Accordion-->
                        </div>
                    </div>
                </div>
            </main>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script src="https://files.codepedia.info/files/uploads/iScripts/html2canvas.js"></script>
    <%--<script src="Scripts/html2canvas.min.js" type="text/javascript"></script>--%>

    <script type="text/javascript">

        //function ConvertToImage(btnExport) {
        //    html2canvas($("#dvTable")[0]).then(function (canvas) {
        //        var base64 = canvas.toDataURL();
        //        $("[id*=hfImageData]").val(base64);
        //        __doPostBack(btnExport.name, "");
        //    });
        //    return true;
        //}


        //$(document).ready(function () {
        //    var element = $("#htmlcontentholder"); // global variable
        //    var getCanvas; // global variable

        //    html2canvas(element, {
        //        onrendered: function (canvas) {
        //            $("#previewImage").append(canvas);
        //            getCanvas = canvas;
        //        }
        //    });

        //    $("#btnConvertHtml2Image").on('click', function () {
        //        var imgageData = getCanvas.toDataURL("image/png");
        //        // Now browser starts downloading it instead of just showing it
        //        var newData = imgageData.replace(/^data:image\/png/, "data:application/octet-stream");
        //        $("#btnConvertHtml2Image").attr("download", "QR.png").attr("href", newData);
        //    });
        //});
  //      function dwQR() {

  //           var c = document.getElementById("qr");
  //var ctx = c.getContext("2d");
  //var img = document.getElementById("imgQR");
  //ctx.drawImage(img, 10, 10, 150, 180);

  //          html2canvas(document.querySelector("#dvTable")).then(canvas => {
  //              //document.body.appendChild(canvas);
  //              var a = document.createElement('a');
  //              //// toDataURL defaults to png, so we need to request a jpeg, then convert for file download.
  //              a.href = canvas.toDataURL("image/png").replace("image/png", "image/octet-stream");
  //              a.download = 'QRcode.png';
  //              a.click();
  //          });
  //      }
    </script>
</asp:Content>
