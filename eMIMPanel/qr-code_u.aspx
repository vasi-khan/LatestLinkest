<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="qr-code_u.aspx.cs" Inherits="eMIMPanel.qr_code_u" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="updFormArea" runat="server" UpdateMode="Conditional">
        <%--<Triggers>
            <asp:PostBackTrigger ControlID="btnSend" />
        </Triggers>--%>
        <ContentTemplate>


            <main>
                <div class="container-fluid">
                    <nav aria-label="breadcrumb" class="my-3">
                        <ol class="breadcrumb breadcrumb-info">
                            <li class="breadcrumb-item"><a href="#">Home</a></li>
                            <li class="breadcrumb-item"><a href="#">Linkext</a></li>
                            <li class="breadcrumb-item active" aria-current="page">QR Code</li>
                        </ol>
                    </nav>

                    <!-- Content Row -->
                    <div class="row">

                        <!-- QR Details -->
                        <div class="col-xl-6 col-lg-6 order-2 order-lg-1">
                            <!-- Basic Card Example -->
                            <div class="card bg-primary border-light shadow-soft mt-4 mt-lg-0 h-100">
                                <div class="card-header py-3 bg-gradient-primary">
                                    <h6 class="m-0 font-weight-bold text-dark"><i class="fas fa-qrcode"></i> QR Code Details</h6>
                                </div>
                                <div class="card-body pt-0">
                                    <form>
                                        <fieldset class="form-group mb-0 mb-lg-2">
                                            <div class="row">
                                                <legend class="col-form-label col-sm-3 pt-0 font-weight-bold">QR Code Size:</legend>
                                                <div class="col-sm-9">
                                                    <asp:RadioButtonList ID="rdbQRsize" runat="server" RepeatDirection="Horizontal">
                                                        <asp:ListItem Text="Large (3x3)&nbsp;" Value="L"></asp:ListItem>
                                                        <asp:ListItem Text="Medium (2x2)&nbsp;" Value="M" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="Small (1x1)" Value="S"></asp:ListItem>
                                                    </asp:RadioButtonList>

                                                    <%--<div class="custom-control custom-radio custom-control-inline">
                                                        <input type="radio" id="customRadioInline1" name="customRadioInline1" class="custom-control-input">
                                                        <label class="custom-control-label" for="customRadioInline1">Large (3x3) </label>
                                                </div>
                                                    <div class="custom-control custom-radio custom-control-inline">
                                                        <input type="radio" id="customRadioInline2" name="customRadioInline1" class="custom-control-input">
                                                        <label class="custom-control-label" for="customRadioInline2">Medium (2x2) </label>
                                                    </div>
                                                    <div class="custom-control custom-radio custom-control-inline">
                                                        <input type="radio" id="customRadioInline3" name="customRadioInline1" class="custom-control-input">
                                                        <label class="custom-control-label" for="customRadioInline3">Small (1x1) </label>
                                                    </div>--%>
                                            </div>
                                        </fieldset>
                                        <div class="form-group row">
                                            <label for="inputEmail3" class="col-sm-3 col-form-label font-weight-bold">Short URL</label>
                                            <div class="col-sm-9">
                                                <asp:DropDownList ID="ddlURL" runat="server" class="drop-select form-control" data-live-search="true"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group row">
                                            <label for="exampleFormControlTextarea2" class="col-sm-3 col-form-label font-weight-bold">QRCode Color</label>
                                            <div class="col-6 col-sm-4">
                                                <asp:TextBox ID="txtQRCodeColor" runat="server" class="form-control" Style="box-shadow: none;"></asp:TextBox>
                                            </div>
                                            <div class="col-6 col-sm-5">
                                                <asp:Button ID="btnPickQRCodeColor" runat="server" Text="Pick Color" class="btn bg-primary btn-block" />
                                                <asp:ColorPickerExtender
                                                    ID="txtQRCodeColor_ColorPickerExtender"
                                                    TargetControlID="txtQRCodeColor"
                                                    PopupButtonID="btnPickQRCodeColor"
                                                    PopupPosition="TopRight"
                                                    SampleControlID="txtQRCodeColor"
                                                    Enabled="True"
                                                    runat="server">
                                                </asp:ColorPickerExtender>
                                            </div>
                                        </div>
                                        <div class="form-group row">
                                            <label for="exampleFormControlTextarea2" class="col-sm-3 col-form-label font-weight-bold">Frame Color</label>
                                            <div class="col-6 col-sm-4">
                                                <asp:TextBox ID="txtFramColor" runat="server" class="form-control" Style="box-shadow: none;"></asp:TextBox>
                                            </div>
                                            <div class="col-6 col-sm-5">
                                                <asp:Button ID="btnPickFramColor" runat="server" Text="Pick Color" class="btn bg-primary btn-block" />
                                                <asp:ColorPickerExtender
                                                    ID="txtFramColor_ColorPickerExtender"
                                                    TargetControlID="txtFramColor"
                                                    PopupButtonID="btnPickFramColor"
                                                    PopupPosition="TopRight"
                                                    SampleControlID="txtFramColor"
                                                    Enabled="True"
                                                    runat="server">
                                                </asp:ColorPickerExtender>
                                            </div>
                                        </div>
                                        <div class="form-group row">
                                            <label for="exampleFormControlTextarea2" class="col-sm-3 col-form-label font-weight-bold">Text:</label>
                                            <div class="col-sm-9">
                                                <asp:TextBox ID="txtText" runat="server" class="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group row">
                                            <label for="exampleFormControlTextarea2" class="col-sm-3 col-form-label font-weight-bold">Upload Logo:</label>
                                            <div class="col-sm-9">
                                                <div class="custom-file">
                                                    <asp:FileUpload ID="FileUpload1" runat="server" class="custom-file-input" ClientIDMode="Static" onchange="this.form.submit()" />
                                                    <label class="custom-file-label" for="customFile">Choose file</label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group row justify-content-end mt-4 mb-0">
                                            <div class="col-sm-9">
                                                <!--  -->
                                                <div class="row">
                                                    <div class="col-lg-7">
                                                        <asp:LinkButton ID="btnSend" runat="server" class="btn btn-primary font-weight-bold btn-block mb-3 mb-lg-0"
                                                        OnClick="btnSend_Click"> <i class="fas fa-qrcode"></i> Generate QR Code</asp:LinkButton>
                                                    </div>
                                                    <div class="col-lg-5">
                                                        <asp:LinkButton ID="LinkButton1" runat="server" class="btn btn-primary font-weight-bold btn-block"
                                                             OnClick="btnCancel_Click"> <i class="fas fa-sync"></i> Reset</asp:LinkButton>
                                                    </div>
                                                </div>
                                               <!--  -->
                                            </div>
                                        </div>
                                    </form>
                                </div>
                            </div>
                        </div>
                        <!-- End QR Details -->

                        <!-- QR Genrated -->
                        <div class="col-xl-6 col-lg-6 order-1 order-lg-2">
                            <asp:HiddenField ID="hfImageData" runat="server" />
                            <div class="card bg-primary border-light shadow-soft mb-4 h-100">
                                <div class="card-header py-3">
                                    <h6 class="m-0 font-weight-bold text-dark"><i class="fas fa-qrcode"></i>QR Generated</h6>
                                </div>
                                <div class="card-body" style="text-align: center;">
                                    <%--<h3 class="text-center font-weight-bold mb-3 h4">QRCode View</h3>--%>
                                    <asp:Image ID="imgPicture" runat="server" />

                                    <div id="dvTable" runat="server" class="row justify-content-center">
                                        <div id="divQROutside" runat="server">
                                            <%--<canvas id="qr" runat="server" class="w-100" />--%>
                                            <div id="divQR" runat="server" style="background-color: white; text-align: center;">
                                                <asp:Image ID="imgQR" runat="server" />
                                            </div>
                                            <div style="text-align: center;">
                                                <asp:Label ID="lblTitle" runat="server" Style="font-size: 18px;"></asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="text-center mt-3">
                                        <asp:LinkButton ID="btnDownload" runat="server" class="btn btn-primary text-success"
                                            OnClick="Btndownload_click">
                                            <span class="icon text-white">
                                                <i class="fas fa-download text-success"></i>
                                            </span>
                                            <span class="text">Download</span>
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                         <!-- End QR Genrated -->

                    </div>

                </div>
            </main>
        </ContentTemplate>
    </asp:UpdatePanel>
     <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="updFormArea"
        DisplayAfter="0">
        <ProgressTemplate>
            <div align="center" style="position: absolute; left: 40%; top: 50%; vertical-align: middle; z-index:10000;">
                <img src="Img/LOADING.GIF" />
            </div>
            <div align="center" style="position: absolute; left: 0%; top: 0%; vertical-align: middle;">
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
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
        function test() {
            html2canvas(document.querySelector("#dvTable")).then(canvas => {
                //document.body.appendChild(canvas);
                var a = document.createElement('a');
                //// toDataURL defaults to png, so we need to request a jpeg, then convert for file download.
                a.href = canvas.toDataURL("image/png").replace("image/png", "image/octet-stream");
                a.download = 'QRcode.png';
                a.click();
            });
        }
    </script>
</asp:Content>
