<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="send_rcs_user_template.aspx.cs" Inherits="eMIMPanel.send_rcs_user_template" %>

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

        .modal.modalPopupS {
            top: 10px !important;
            left: 0 !important;
            display: block;
        }

        .modalBackground {
            background-color: #000;
            opacity: 0.5;
        }

        .btn {
            margin: 0 20px 10px 20px
        }

        .card-title {
            margin-bottom: 0.25rem !important;
        }

        .card_bodytow {
            padding: 0.5rem !important;
        }
    </style>
    <style type="text/css">
        .labelcss {
            display: block;
            max-width: 200px;
            word-wrap: break-word;
        }
    </style>
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
            background-color: #000;
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
    <style>
        /* Carousel Css Start*/
        .btn-floating i {
            font-size: 1rem;
            line-height: 40px;
            display: inline-block;
            width: inherit;
            text-align: center;
            color: rgb(0, 0, 0);
        }

        .carousel-multi-item .controls-top {
            text-align: right;
            margin-bottom: 0.88rem;
        }

        .btn-floating {
            box-shadow: 0 5px 11px 0 rgb(0 0 0 / 18%), 0 4px 15px 0 rgb(0 0 0 / 15%);
            width: 37px;
            height: 37px;
            z-index: 1;
            display: inline-block;
            overflow: hidden;
            -webkit-transition: all .2s ease-in-out;
            transition: all .2s ease-in-out;
            margin: 10px;
            padding: 0;
            cursor: pointer;
        }

        .carousel-multi-item .controls-top .btn-floating {
            background: #e6e7ee;
        }

        .card-body {
            flex: 1 1 auto;
            padding: 1rem !important;
        }

        .card-text {
            height: 106px;
            overflow: hidden;
            position: relative;
            text-align: justify;
        }

        .card-title {
            margin-bottom: 0.25rem;
            overflow: hidden;
        }

        .card {
            border: 0rem solid rgba(243, 247, 250, 0.05) !important;
        }

        .btn-icon-split {
            margin: 0px 12px;
        }
        /* Carousel Css Close*/
    </style>
    <script type="text/javascript"> 
        function SMSfileUpload612() {

            var uploadControl = document.getElementById('<%= FileUpload3.ClientID %>');
            var myfile = uploadControl.value;
            console.log(myfile);
            var Extension = myfile.substring(myfile.lastIndexOf('.') + 1).toLowerCase();
            console.log(Extension);


            if ((Extension != "jpg" && Extension != "png")) {
                alert("File Shoul be jpg or png format !!");
                return false;
            } else {
                //showLoading();
                console.log("ret true");
                return true;
            }
        }
    </script>
    <script type="text/javascript">
        function Confirm() {
            var confirm_value = document.createElement("INPUT");
            confirm_value.type = "hidden";
            confirm_value.name = "confirm_value";
            if (confirm("ARE SURE YOU WANT TO POST THESE BOOKS NOW?")) {
                confirm_value.value = "Yes";
            } else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }
    </script>
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
            <nav aria-label="breadcrumb" class="my-3">
                <ol class="breadcrumb breadcrumb-info">
                    <li class="breadcrumb-item"><a href="#">Home</a></li>
                    <li class="breadcrumb-item active" aria-current="page">Add Template</li>
                </ol>
            </nav>
            <asp:UpdatePanel ID="upPnl" runat="server">
                <ContentTemplate>

                    <!-- Content Row -->

                    <div class="row">

                        <!-- Area Chart -->
                        <div class="col-xl-12 col-lg-12" id="divmain" runat="server">
                            <!-- Basic Card Example -->
                            <div class="card bg-primary border-light shadow-soft mb-4 d-none" id="divTemplate" runat="server">
                                <div class="card-header py-3 bg-primary">
                                    <div class="row">
                                        <div class="col-md-10">
                                            <h6 class="m-0 font-weight-bold"><i class="far fa-user-circle"></i>Add New Template</h6>
                                        </div>
                                    </div>
                                </div>


                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="card-body">
                                            <div>
                                                <fieldset class="form-group card bg-primary border-light shadow-soft mb-2" style="border-radius: 10px;">


                                                    <div class="col-md-12">

                                                        <div class="form-row">
                                                            <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Template Type </label>
                                                            <div class="col-md-6 mb-3">

                                                                <asp:DropDownList ID="ddlRCSType" runat="server" class="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlRCSType_SelectedIndexChanged1">
                                                                    <%--<asp:ListItem Text="Select RCS Type" Value="0"></asp:ListItem>--%>
                                                                    <asp:ListItem Text="TEXT" Value="1" Selected="True"></asp:ListItem>
                                                                    <asp:ListItem Text="IMAGE" Value="2"></asp:ListItem>
                                                                    <asp:ListItem Text="VIDEO" Value="3"></asp:ListItem>
                                                                    <asp:ListItem Text="CARD" Value="4"></asp:ListItem>
                                                                    <asp:ListItem Text="CAROUSEL" Value="5"></asp:ListItem>
                                                                </asp:DropDownList>

                                                            </div>
                                                        </div>

                                                        <div class="form-row">
                                                            <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Template Name </label>
                                                            <div class="col-md-6 mb-3">
                                                                <asp:TextBox class="form-control" ID="txttempname" runat="server" MaxLength="200" OnTextChanged="txttempname_TextChanged" AutoPostBack="true" placeholder="Template Name" ToolTip="Template Name" />
                                                                <div class="valid-feedback">
                                                                    Looks good!
                             
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </fieldset>
                                            </div>
                                            <div id="divTempTxt" runat="server">
                                                <fieldset class="form-group card bg-primary border-light shadow-soft mb-2" style="border-radius: 10px;">


                                                    <div class="col-md-12">
                                                        <div class="form-row">
                                                            <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Template Text </label>
                                                            <div class="col-sm-10">
                                                                <div style="pointer-events: all;">
                                                                    <asp:TextBox ID="txtTempText" runat="server" TextMode="MultiLine" class="form-control" MaxLength="12288" placeholder="Template Text" ToolTip="Template Text" Rows="7" onkeyup="smscnt(); return true;"></asp:TextBox>
                                                                    <div id="divOptOut" runat="server" style="display: none;">
                                                                        <asp:CheckBox ID="chkOptOut" runat="server" Text="Include - " />
                                                                        <asp:Label ID="lblOptOut" runat="server" Text="DND7726"></asp:Label>
                                                                    </div>
                                                                </div>
                                                                <%--<textarea class="form-control" id="exampleFormControlTextarea2" rows="7"></textarea>--%>
                                                                <div class="d-flex justify-content-between">
                                                                    <p class="my-2">
                                                                        <span class="font-weight-bold">Used :</span>
                                                                        <asp:Label ID="lblused" runat="server" Text="0"></asp:Label>
                                                                    </p>
                                                                    <p class="my-2">
                                                                        <span class="font-weight-bold">RCS Count :</span>
                                                                        <asp:Label ID="lblsmscnt" runat="server" Text="0"></asp:Label>
                                                                    </p>
                                                                    <p class="my-2">
                                                                        <%--<span class="font-weight-bold">Unicode :</span> --%>
                                                                        <asp:Label ID="lblUniCode" runat="server" Text="" Style="color: red; font-weight: bolder;"></asp:Label>
                                                                    </p>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </fieldset>
                                            </div>

                                            <div id="divimg" runat="server">
                                                <fieldset class="form-group card bg-primary border-light shadow-soft mb-2" style="border-radius: 10px;">


                                                    <div class="col-md-12">
                                                        <div class="form-row">
                                                            <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Image </label>
                                                            <div class="col-md-3 mb-3">
                                                                <div class="custom-file">
                                                                    <asp:FileUpload ID="FileUpload2" runat="server" class="custom-file-input" />
                                                                    <label class="custom-file-label" for="customFile">Upload Image</label>
                                                                </div>

                                                                <small id="textHelp2" class="form-text text-muted mb-3 mb-lg-0">Upload Limit <strong>2 MB.</strong> &nbsp;&nbsp;&nbsp;&nbsp;
                                                 <asp:Label ID="lblImagepath" runat="server"></asp:Label>
                                                                    <asp:Label ID="lblUploading2" runat="server"></asp:Label>
                                                                </small>
                                                            </div>


                                                            <div class="col-md-3 mb-3">
                                                                <div class="custom-file">
                                                                    <asp:LinkButton class="btn btn-primary btn-icon-split" ID="UploadImg" runat="server" OnClick="UploadImg_Click">
                                            <span class="text-success font-weight-bold">Upload</span>
                                                                    </asp:LinkButton>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-3 mb-3">
                                                                <asp:Label ID="lblupImg" Font-Bold="true" ForeColor="Green" runat="server"></asp:Label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </fieldset>
                                            </div>

                                            <div id="divVedio" runat="server">
                                                <fieldset class="form-group card bg-primary border-light shadow-soft mb-2" style="border-radius: 10px;">


                                                    <div class="col-md-12">
                                                        <div class="form-row">
                                                            <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Video </label>
                                                            <div class="col-md-4 mb-3">
                                                                <div class="custom-file">
                                                                    <asp:FileUpload ID="FileUpload1" runat="server" class="custom-file-input" />
                                                                    <label class="custom-file-label" for="customFile">
                                                                        <asp:Label ID="lblVI" runat="server" Text="Upload"></asp:Label>
                                                                        Video</label>
                                                                </div>
                                                                <small id="textHelp" class="form-text text-muted mb-3 mb-lg-0">Upload Limit <strong>
                                                                    <asp:Label ID="lblFileSize" runat="server"></asp:Label>
                                                                    10 MB.</strong> &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblUploading" Visible="false" runat="server"></asp:Label>
                                                                    <asp:Label ID="lblvideoname" runat="server"></asp:Label>
                                                                </small>
                                                            </div>
                                                            <div class="col-md-3 mb-3">
                                                                <div class="custom-file">
                                                                    <asp:LinkButton class="btn btn-primary btn-icon-split" ID="Lkbtnvideo" runat="server" OnClick="Lkbtnvideo_Click">
                                            <span class="text-success font-weight-bold">Upload</span>
                                                     
                                                                    </asp:LinkButton>
                                                                    <asp:Label ID="lblvideoS" Font-Bold="true" ForeColor="Green" runat="server"></asp:Label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </fieldset>
                                            </div>

                                            <div id="divcard" runat="server">
                                                <fieldset class="form-group card bg-primary border-light shadow-soft mb-2" style="border-radius: 10px;">


                                                    <div class="col-md-12">
                                                        <div class="form-row">
                                                            <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Orientation </label>
                                                            <div class="col-md-6 mb-3">
                                                                <asp:DropDownList ID="ddlOrientation" runat="server" class="form-control">
                                                                    <asp:ListItem Text="--Select Orientation--" Value="0" Selected="True"></asp:ListItem>
                                                                    <asp:ListItem Text="Horizontal" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="Vertical" Value="2"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>

                                                        <div class="form-row">
                                                            <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Alignment </label>
                                                            <div class="col-md-6 mb-3">
                                                                <asp:DropDownList ID="ddlAlignment" runat="server" class="form-control">
                                                                    <asp:ListItem Text="--Select Alignment--" Value="0" Selected="True"></asp:ListItem>
                                                                    <asp:ListItem Text="Left" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="Right" Value="2"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </fieldset>
                                                <fieldset class="form-group card bg-primary border-light shadow-soft mb-2" style="border-radius: 10px;">


                                                    <div class="col-md-12">
                                                        <legend class="col-form-label col-sm-3 pt-0 font-weight-bold">Content</legend>
                                                        <div class="form-row">
                                                            <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Title </label>
                                                            <div class="col-md-6 mb-3">
                                                                <!-- <label for="validationCustom03" class="font-weight-bold">Website : www.</label> -->
                                                                <asp:TextBox class="form-control" ID="txtcardtitle" MaxLength="200" runat="server" placeholder="Title" ToolTip="Title" onkeyup="displayNumberCount();" />
                                                                <%--<input type="text" class="form-control" id="validationCustom03" placeholder="Website : www.">--%>
                                                                <div class="valid-feedback">
                                                                    Looks good!
                             
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-row">
                                                            <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Description </label>
                                                            <div class="col-md-6 mb-3">
                                                                <!-- <label for="validationCustom03" class="font-weight-bold">Website : www.</label> -->
                                                                <asp:TextBox class="form-control" ID="txtdesc" runat="server" placeholder="Description" TextMode="MultiLine" MaxLength="2000"></asp:TextBox>
                                                                <%--<asp:TextBox class="form-control" ID="txtdesc" MaxLength="2000" runat="server" placeholder="Description" ToolTip="Description" TextMode="MultiLine" />--%>
                                                                <%--<input type="text" class="form-control" id="validationCustom03" placeholder="Website : www.">--%>
                                                                <div class="valid-feedback">
                                                                    Looks good!
                             
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-row">
                                                            <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Media Url Img </label>
                                                            <div class="col-md-6 mb-3">
                                                                <div class="custom-file">
                                                                    <asp:FileUpload ID="FileUpload5" runat="server" class="custom-file-input" />
                                                                    <label class="custom-file-label" for="customFile">Upload Image</label>

                                                                </div>
                                                                <small id="textHelp12" class="form-text text-muted mb-3 mb-lg-0">Upload Limit <strong>2 MB.</strong> &nbsp;&nbsp;&nbsp;&nbsp;</small>
                                                                <asp:Label ID="lblcardimage" Font-Bold="true" runat="server"></asp:Label>

                                                            </div>
                                                            <div class="col-md-3 mb-3">
                                                                <div class="custom-file">
                                                                    <asp:LinkButton class="btn btn-primary btn-icon-split" ID="cardbtnfile" runat="server" OnClick="cardbtnfile_Click">
                                            <span class="text-success font-weight-bold">Upload</span>
                                                                    </asp:LinkButton>
                                                                    <asp:Label ID="lblcardimageSucess" Font-Bold="true" ForeColor="Green" runat="server"></asp:Label>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="form-row">
                                                            <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Height </label>
                                                            <div class="col-md-6 mb-3">
                                                                <asp:DropDownList ID="ddlcardheight" runat="server" class="form-control">
                                                                    <asp:ListItem Text="Select Card Height" Value="0" Selected="True"></asp:ListItem>
                                                                    <asp:ListItem Text="Short" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="Medium" Value="2"></asp:ListItem>
                                                                    <asp:ListItem Text="Small" Value="3"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </fieldset>
                                                <div class="form-row">
                                                    <%--<label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Suggestion </label>--%>
                                                    <div class="col-md-6 mb-3" runat="server" style="display: none">
                                                        <!-- <label for="validationCustom03" class="font-weight-bold">Website : www.</label> -->
                                                        <asp:TextBox class="form-control" ID="TextBox4" runat="server" placeholder="Description" ToolTip="Description" TextMode="MultiLine" />
                                                        <%--<input type="text" class="form-control" id="validationCustom03" placeholder="Website : www.">--%>
                                                        <div class="valid-feedback">
                                                            Looks good!
                             
                                                        </div>
                                                    </div>
                                                </div>

                                                <fieldset class="form-group card bg-primary border-light shadow-soft mb-2" style="border-radius: 10px;">

                                                    <div class="col-md-12">
                                                        <legend class="col-form-label col-sm-3 pt-0 font-weight-bold">Card Suggestion</legend>
                                                        <div class="form-row">
                                                            <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Type </label>
                                                            <div class="col-md-6 mb-3">
                                                                <asp:DropDownList ID="ddlcardtype" runat="server" class="
                                                    form-control"
                                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlcardtype_SelectedIndexChanged">
                                                                    <asp:ListItem Text="--Select Card Suggestion Type--" Value="0" Selected="True"></asp:ListItem>
                                                                    <asp:ListItem Text="REPLY" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="OPEN_URL" Value="2"></asp:ListItem>
                                                                    <asp:ListItem Text="DIAL_PHONE" Value="3"></asp:ListItem>
                                                                    <asp:ListItem Text="SHOW_LOCATION" Value="4"></asp:ListItem>
                                                                    <asp:ListItem Text="REQUEST_LOCATION" Value="5"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="col-md-3 mb-3">
                                                                <div class="custom-file">

                                                                    <asp:Button ID="btnCarouselcardadd" runat="server" CssClass="btn btn-primary" OnClick="btnCarouselcardadd_Click" Text="Add" />

                                                                    <small id="textHelp17" class="form-text text-muted mb-3 mb-lg-0">
                                                                        <asp:Label ID="Label14" runat="server"></asp:Label></small>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="form-row" id="crdopenurl" runat="server">
                                                            <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Open Url </label>
                                                            <div class="col-md-6 mb-3">
                                                                <!-- <label for="validationCustom03" class="font-weight-bold">Website : www.</label> -->
                                                                <asp:TextBox class="form-control" ID="txtcardurl" runat="server" OnTextChanged="txtcardurl_TextChanged" AutoPostBack="true" placeholder="URL" ToolTip="URL" />

                                                                <%--  <asp:RegularExpressionValidator ID="rgurl" runat="server" ValidationExpression="^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)?[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$" SetFocusOnError="True" ControlToValidate="txtCcurl"
                                                     ErrorMessage="Enter a valid URL"></asp:RegularExpressionValidator>--%>
                                                                <%--<input type="text" class="form-control" id="validationCustom03" placeholder="Website : www.">--%>
                                                                <div class="valid-feedback">
                                                                    Looks good!
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-row" id="crdopenurl1" runat="server" style="display: none">
                                                            <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Open Url </label>
                                                            <div class="col-md-3 mb-3">
                                                                <div class="custom-file">
                                                                    <asp:FileUpload ID="FileUpload6" runat="server" class="custom-file-input" />
                                                                    <label class="custom-file-label" for="customFile">
                                                                        <asp:Label ID="Label7" runat="server" Text=""></asp:Label>Upload Url</label>
                                                                </div>
                                                                <small id="textHelp6" class="form-text text-muted mb-3 mb-lg-0">Upload Limit <strong>
                                                                    <asp:Label ID="Label8" runat="server"></asp:Label>
                                                                    MB.</strong> &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="Label9" runat="server"></asp:Label>
                                                                </small>
                                                            </div>
                                                            <div class="col-md-3 mb-3">
                                                                <div class="custom-file">
                                                                    <asp:LinkButton class="btn btn-primary btn-icon-split" ID="LinkButton6" runat="server">
                                            <span class="text-success font-weight-bold">Upload</span>
                                                                    </asp:LinkButton>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="form-row" id="crddialphone" runat="server">
                                                            <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Contact Number </label>
                                                            <div class="col-md-6 mb-3">
                                                                <!-- <label for="validationCustom03" class="font-weight-bold">Website : www.</label> -->
                                                                <asp:TextBox class="form-control" ID="txtcardphone" runat="server" placeholder="Contact Number" ToolTip="Contact No" MaxLength="10" onkeyup="integersOnly(this);" />
                                                                <%--<input type="text" class="form-control" id="validationCustom03" placeholder="Website : www.">--%>
                                                                <div class="valid-feedback">
                                                                    Looks good!
                                                                </div>
                                                                <asp:Label ID="Label10" runat="server" Text="Contact No (With Country Code)"></asp:Label>
                                                            </div>

                                                        </div>

                                                        <div id="crdshowlocation" runat="server">
                                                            <div class="form-row">
                                                                <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Latitude </label>
                                                                <div class="col-md-6 mb-3">
                                                                    <asp:TextBox class="form-control" ID="txtcrdlatitude" runat="server" OnTextChanged="txtcrdlatitude_TextChanged" placeholder="Latitude" ToolTip="Latitude" />

                                                                    <div class="valid-feedback">
                                                                        Looks good!
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="form-row">
                                                                <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Longitude </label>
                                                                <div class="col-md-6 mb-3">
                                                                    <asp:TextBox class="form-control" ID="txtcrdlongitude" OnTextChanged="txtcrdlongitude_TextChanged" runat="server" placeholder="Longitude" ToolTip="Longitude" />
                                                                    <div class="valid-feedback">
                                                                        Looks good!
                                                                    </div>
                                                                </div>

                                                            </div>
                                                            <div class="form-row" style="display: none">
                                                                <div class="col-md-3 mb-3">
                                                                    <div class="custom-file">
                                                                        <asp:LinkButton class="btn btn-primary btn-icon-split" ID="LinkButton7" runat="server" OnClientClick="getLocation();return false;">
                                                            <span class="text-success font-weight-bold">Submit</span>
                                                                        </asp:LinkButton>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-row" id="DIVTEXT" runat="server">
                                                            <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Text </label>
                                                            <div class="col-md-6 mb-3">
                                                                <!-- <label for="validationCustom03" class="font-weight-bold">Website : www.</label> -->
                                                                <asp:TextBox class="form-control" MaxLength="25" ID="txtcardtext" runat="server" placeholder="Text" ToolTip="Text" />
                                                                <%--<input type="text" class="form-control" id="validationCustom03" placeholder="Website : www.">--%>
                                                                <div class="valid-feedback">
                                                                    Looks good!
                             
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="col-md-12" id="suggestionCard" runat="server">
                                                        <div class="card-body">

                                                            <asp:GridView ID="gvCardsg" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false" OnRowDataBound="gvCardsg_RowDataBound"
                                                                OnRowCommand="gvCardsg_RowCommand" OnRowDeleting="gvCardsg_RowDeleting" runat="server" Width="100%" CellPadding="10" BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive dataTable-view">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Sr No">
                                                                        <ItemTemplate>
                                                                            <%#Container.DataItemIndex+1 %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="SuggestionType">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbldtype" runat="server" Text='<%#Eval("SuggetionType")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>



                                                                    <asp:TemplateField HeaderText="Text">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbl2" runat="server" Text='<%#Eval("SuggestionText")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Url">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbl3" runat="server" Text='<%#Eval("SuggestionUrl")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Phone">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCreatedDate" runat="server" Text='<%#Eval("SuggestionPhone")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Latitude">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblurlid" runat="server" Text='<%#Eval("SuggestionLatitude")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Longitude">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblurlids" runat="server" Text='<%#Eval("SuggestionLongitude")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="row">
                                                                        <ItemTemplate>

                                                                            <asp:LinkButton ID="lbtnDelete" runat="server" class="mx-1 btn btn-primary text-success" CommandName="delete" OnClientClick="return confirm('Are you sure you want to delete this record?');"
                                                                                data-toggle="tooltip" data-placement="top" title="" data-original-title="Delete"> 
                                                                            <span class="text-danger"> <i class="fas fa-times"></i> </span></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                </Columns>


                                                            </asp:GridView>


                                                        </div>
                                                    </div>
                                                </fieldset>
                                            </div>
                                            <div id="divcarousel" runat="server">
                                                <fieldset class="form-group card bg-primary border-light shadow-soft mb-2" style="border-radius: 10px;">

                                                    <div class="col-md-12">
                                                        <div class="form-row">
                                                            <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Card Width </label>
                                                            <div class="col-md-6 mb-3">
                                                                <asp:DropDownList ID="ddlwidth" runat="server" class="form-control">
                                                                    <asp:ListItem Text="--Select Card Width--" Value="0" Selected="True"></asp:ListItem>
                                                                    <asp:ListItem Text="Medium" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="Small" Value="2"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>

                                                        <div class="form-row">
                                                            <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Title </label>
                                                            <div class="col-md-6 mb-3">
                                                                <!-- <label for="validationCustom03" class="font-weight-bold">Website : www.</label> -->
                                                                <asp:TextBox class="form-control" ID="txttitle" MaxLength="200" runat="server" placeholder="Title" ToolTip="Title" />
                                                                <%--<input type="text" class="form-control" id="validationCustom03" placeholder="Website : www.">--%>
                                                                <div class="valid-feedback">
                                                                    Looks good!
                             
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-row">
                                                            <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Description </label>
                                                            <div class="col-md-6 mb-3">
                                                                <!-- <label for="validationCustom03" class="font-weight-bold">Website : www.</label> -->
                                                                <asp:TextBox class="form-control" ID="txtdescription" TextMode="MultiLine" MaxLength="2000" runat="server" placeholder="Description" ToolTip="Description" />
                                                                <%--<input type="text" class="form-control" id="validationCustom03" placeholder="Website : www.">--%>
                                                                <div class="valid-feedback">
                                                                    Looks good!
                             
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-row">
                                                            <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Media Url </label>
                                                            <div class="col-md-6 mb-3">
                                                                <div class="custom-file">
                                                                    <asp:FileUpload ID="FileUpload3" runat="server" class="custom-file-input" />
                                                                    <label class="custom-file-label" for="customFile">Upload Image</label>
                                                                </div>

                                                                <small id="textHelp3" class="form-text text-muted mb-3 mb-lg-0">Upload Limit <strong>2 MB.</strong> &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="Label1" runat="server"></asp:Label>
                                                                </small>
                                                            </div>


                                                            <div class="col-md-3 mb-3">
                                                                <div class="custom-file">
                                                                    <asp:LinkButton class="btn btn-primary btn-icon-split" ID="carofile" runat="server" OnClick="carofile_Click">
                                            <span class="text-success font-weight-bold">Upload</span>
                                                                    </asp:LinkButton>
                                                                    <small id="textHelp14" class="form-text text-muted mb-3 mb-lg-0">
                                                                        <asp:Label ID="Label6" runat="server"></asp:Label></small>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="form-row">
                                                            <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Height </label>
                                                            <div class="col-md-6 mb-3">
                                                                <asp:DropDownList ID="ddlheight" runat="server" class="form-control">
                                                                    <asp:ListItem Text="--Select Height--" Value="0" Selected="True"></asp:ListItem>
                                                                    <asp:ListItem Text="Short" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="Medium" Value="2"></asp:ListItem>
                                                                    <asp:ListItem Text="Small" Value="3"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                             <div class="col-md-3 mb-3" visible="false" id="divTc" runat="server" >
                                                                <div class="custom-file">
                                                                     
                                                                   <asp:Label ID="Label16" class="font-weight-bold" Text="Total Card Created :" runat="server"></asp:Label>
                                                           
                                                                   <asp:Label ID="lblcreatedcard" class="font-weight-bold" Visible="false" runat="server"></asp:Label>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-row">
                                                            <div class="col-md-6 mb-3" runat="server" style="display: none">
                                                                <!-- <label for="validationCustom03" class="font-weight-bold">Website : www.</label> -->
                                                                <asp:TextBox class="form-control" ID="TextBox1" runat="server" placeholder="Description" ToolTip="Description" TextMode="MultiLine" />
                                                                <%--<input type="text" class="form-control" id="validationCustom03" placeholder="Website : www.">--%>
                                                                <div class="valid-feedback">
                                                                    Looks good!
                             
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </fieldset>


                                                <fieldset class="form-group card bg-primary border-light shadow-soft mb-2" style="border-radius: 10px;">

                                                    <div class="col-md-12">

                                                        <legend class="col-form-label col-sm-3 pt-0 font-weight-bold">Carousel card Suggestions</legend>
                                                        <div class="form-row">
                                                            <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Type </label>
                                                            <div class="col-md-6 mb-3">
                                                                <asp:DropDownList ID="ddltypes" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddltypes_SelectedIndexChanged">
                                                                    <asp:ListItem Text="--Select Carousel card Suggestions--" Value="0" Selected="True"></asp:ListItem>
                                                                    <asp:ListItem Text="REPLY" Value="1" ></asp:ListItem>
                                                                    <asp:ListItem Text="OPEN_URL" Value="2"></asp:ListItem>
                                                                    <asp:ListItem Text="DIAL_PHONE" Value="3"></asp:ListItem>
                                                                    <asp:ListItem Text="SHOW_LOCATION" Value="4"></asp:ListItem>
                                                                    <asp:ListItem Text="REQUEST_LOCATION" Value="5"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="col-md-3 mb-3">
                                                                <div class="custom-file">

                                                                    <asp:Button ID="btnCarouselcaedrdadd" runat="server" CssClass="btn btn-primary" OnClick="btnCarouselcaedrdadd_Click" Text="Submit Suggestion" />

                                                                    <small id="textHelp15" class="form-text text-muted mb-3 mb-lg-0">
                                                                        <asp:Label ID="Label12" runat="server"></asp:Label></small>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="form-row" id="openurl1" runat="server" style="display: none">
                                                            <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Upload Url </label>
                                                            <div class="col-md-3 mb-3">
                                                                <div class="custom-file">
                                                                    <asp:FileUpload ID="FileUpload4" runat="server" class="custom-file-input" />
                                                                    <label class="custom-file-label" for="customFile">
                                                                        <asp:Label ID="Label2" runat="server" Text=""></asp:Label>Upload Url</label>
                                                                </div>
                                                                <small id="textHelp4" class="form-text text-muted mb-3 mb-lg-0">Upload Limit <strong>
                                                                    <asp:Label ID="Label3" runat="server"></asp:Label>
                                                                    MB.</strong> &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="Label4" runat="server"></asp:Label>
                                                                </small>
                                                            </div>
                                                            <div class="col-md-3 mb-3">
                                                                <div class="custom-file">
                                                                    <asp:LinkButton class="btn btn-primary btn-icon-split" ID="LinkButton3" runat="server">
                                            <span class="text-success font-weight-bold">Upload</span>
                                                                    </asp:LinkButton>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-row" id="openurl" runat="server">
                                                            <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Upload Url </label>
                                                            <div class="col-md-6 mb-3">
                                                                <!-- <label for="validationCustom03" class="font-weight-bold">Website : www.</label> -->
                                                                <asp:TextBox class="form-control" ID="txturl" OnTextChanged="txturl_TextChanged" runat="server" placeholder="URL" ToolTip="URL" />
                                                                <%--<input type="text" class="form-control" id="validationCustom03" placeholder="Website : www.">--%>
                                                                <div class="valid-feedback">
                                                                    Looks good!
                             
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="form-row" id="dialphone" runat="server">
                                                            <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Contact Number </label>
                                                            <div class="col-md-6 mb-3">
                                                                <!-- <label for="validationCustom03" class="font-weight-bold">Website : www.</label> -->
                                                                <asp:TextBox class="form-control" ID="txtphone" runat="server" placeholder="Contact Number" ToolTip="Contact No" MaxLength="10" onkeyup="integersOnly(this);" />
                                                                <%--<input type="text" class="form-control" id="validationCustom03" placeholder="Website : www.">--%>
                                                                <div class="valid-feedback">
                                                                    Looks good!
                                                                </div>
                                                                <asp:Label ID="Label5" runat="server" Text="Contact No (With Country Code)"></asp:Label>
                                                            </div>

                                                        </div>

                                                        <div id="divshowlocation" runat="server">

                                                            <div class="form-row">
                                                                <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Latitude </label>
                                                                <div class="col-md-6 mb-3">
                                                                    <asp:TextBox class="form-control" ID="txtlatitude" OnTextChanged="txtlatitude_TextChanged" runat="server" placeholder="Latitude" ToolTip="Latitude" />
                                                                    <div class="valid-feedback">
                                                                        Looks good!
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <div class="form-row">
                                                                <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Longitude </label>
                                                                <div class="col-md-6 mb-3">
                                                                    <asp:TextBox class="form-control" ID="txtlongitude" OnTextChanged="txtlongitude_TextChanged" runat="server" placeholder="Longitude" ToolTip="Longitude" />
                                                                    <div class="valid-feedback">
                                                                        Looks good!
                                                                    </div>
                                                                </div>
                                                                <asp:Label ID="lblmsg" runat="server" Text=""></asp:Label>
                                                            </div>
                                                            <div class="form-row" style="display: none">
                                                                <div class="col-md-3 mb-3">
                                                                    <div class="custom-file">
                                                                        <asp:LinkButton class="btn btn-primary btn-icon-split" ID="LinkButton4" runat="server" OnClientClick="getLocation();return false;">
                                            <span class="text-success font-weight-bold">Submit</span>
                                                                        </asp:LinkButton>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-row" id="dicccs" runat="server">
                                                            <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Text </label>
                                                            <div class="col-md-6 mb-3">
                                                                <!-- <label for="validationCustom03" class="font-weight-bold">Website : www.</label> -->
                                                                <asp:TextBox class="form-control" MaxLength="25" ID="txttext" runat="server" placeholder="Text" ToolTip="Text" />
                                                                <%--<input type="text" class="form-control" id="validationCustom03" placeholder="Website : www.">--%>
                                                                <div class="valid-feedback">
                                                                    Looks good!
                             
                                                                </div>
                                                            </div>

                                                           
                                                        </div>
                                                    </div>


                                                    <div class="col-md-12" id="suggectionCardCrasoul" runat="server">
                                                        <div class="card-body">
                                                            <asp:GridView ID="gvSType" HeaderStyle-Height="25px" OnRowDeleting="gvSType_RowDeleting" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false" OnRowDataBound="gvSType_RowDataBound"
                                                                runat="server" Width="100%" CellPadding="10" BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive dataTable-view">
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Sr No">
                                                                        <ItemTemplate>
                                                                            <%#Container.DataItemIndex+1 %>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="SuggestionType">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbldtype" runat="server" Text='<%#Eval("SuggetionType")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>



                                                                    <asp:TemplateField HeaderText="Text">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbl2" runat="server" Text='<%#Eval("SuggestionText")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>

                                                                    <asp:TemplateField HeaderText="Url">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lbl3" runat="server" Text='<%#Eval("SuggestionUrl")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Phone">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCreatedDate" runat="server" Text='<%#Eval("SuggestionPhone")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Latitude">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblurlid" runat="server" Text='<%#Eval("SuggestionLatitude")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Longitude">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblurlids" runat="server" Text='<%#Eval("SuggestionLongitude")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="row">
                                                                        <ItemTemplate>

                                                                            <asp:LinkButton ID="lbtnDelete" runat="server" class="mx-1 btn btn-primary text-success" CommandName="delete" OnClientClick="return confirm('Are you sure you want to delete this record?');"
                                                                                data-toggle="tooltip" data-placement="top" title="" data-original-title="Delete"> 
                                                                            <span class="text-danger"> <i class="fas fa-times"></i> </span></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>


                                                                </Columns>


                                                            </asp:GridView>
                                                        </div>
                                                    </div>

                                                </fieldset>
                                            </div>
                                            <%-----------------For Card------------------------%>
                                        </div>
                                        <div class="card bg-primary border-light shadow-soft mb-4" id="btngrid" visible="false" runat="server">

                                            <div class="card-body">
                                                <asp:Label ID="lockmsg" runat="server" Text=""></asp:Label>
                                                <div class="form-row" id="addnewhide" runat="server">
                                                    <div class="col-md-4" style="text-align: center;">
                                                        <asp:LinkButton class="btn btn-primary btn-icon-split  mt-2" ID="btnnewcard" OnClick="btnnewcard_Click" runat="server">Add New Card</asp:LinkButton>
                                                    </div>
                                                    <div class="col-md-4" style="text-align: center;" id="btnshow" runat="server">
                                    <asp:LinkButton class="btn btn-primary btn-icon-split  mt-2" ID="btnsubmit" OnClientClick="return confirm('Please note you will not be able to add more cards after clicking submit  Are you sure that you want to proceed with sumbit !!');" OnClick="btnsubmit_Click" runat="server" Visible="false">Submit Card</asp:LinkButton>

                                                    </div>
                                                    <div class="col-md-4" style="text-align: center;">
                                                        <asp:LinkButton runat="server" ID="lnkReset" class="btn btn-primary btn-icon-split  mt-2" OnClick="lnkReset_Click">Reset
                                                        </asp:LinkButton>
                                                    </div>
                                                </div>



                                            </div>
                                        </div>
                                        <div class="card bg-primary border-light shadow-soft mb-4 d-none" id="divcarouselsuggsction" runat="server">

                                            <fieldset class="form-group card bg-primary border-light shadow-soft mb-2" style="border-radius: 10px;">

                                                <div class="col-md-12">

                                                    <legend class="col-form-label col-sm-3 pt-0 font-weight-bold">Over All Suggestion</legend>
                                                    <div class="form-row">
                                                        <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Type </label>
                                                        <div class="col-md-6 mb-3">
                                                            <asp:DropDownList ID="ddlCarouselSuggetion" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlCarouselSuggetion_SelectedIndexChanged">
                                                                <asp:ListItem Text="--Select Carousel Suggetions--" Value="0" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="REPLY" Value="1" ></asp:ListItem>
                                                                <asp:ListItem Text="OPEN_URL" Value="2"></asp:ListItem>
                                                                <asp:ListItem Text="DIAL_PHONE" Value="3"></asp:ListItem>
                                                                <asp:ListItem Text="SHOW_LOCATION" Value="4"></asp:ListItem>
                                                                <asp:ListItem Text="REQUEST_LOCATION" Value="5"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-md-3 mb-3">
                                                            <div class="custom-file">

                                                                <asp:Button ID="btncarouslsgtion" runat="server" CssClass="btn btn-primary" OnClick="btncarouslsgtion_Click" Text="Submit Suggestion" />

                                                                <small id="textHelp16" class="form-text text-muted mb-3 mb-lg-0">
                                                                    <asp:Label ID="Label13" runat="server"></asp:Label></small>
                                                            </div>
                                                        </div>
                                                    </div>


                                                    <div class="form-row" id="sugurl" runat="server">
                                                        <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Upload Url </label>
                                                        <div class="col-md-6 mb-3">
                                                            <!-- <label for="validationCustom03" class="font-weight-bold">Website : www.</label> -->

                                                            <asp:TextBox class="form-control" ID="txtCcurl" OnTextChanged="txtCcurl_TextChanged" runat="server" placeholder="URL" ToolTip="URL" />
                                                            <%--<asp:RequiredFieldValidator ID="refsn" runat="server" ControlToValidate="txtCcurl" Text="*" ></asp:RequiredFieldValidator>
                                                <asp:RegularExpressionValidator ID="rgurl" runat="server" ValidationExpression="http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?" ControlToValidate="txtCcurl"
                                                     ErrorMessage="Enter a valid URL"></asp:RegularExpressionValidator>--%>
                                                            <%--<input type="text" class="form-control" id="validationCustom03" placeholder="Website : www.">--%>
                                                            <div class="valid-feedback">
                                                                Looks good!
                             
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="form-row" id="sgdialphone" runat="server">
                                                        <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Contact Number </label>
                                                        <div class="col-md-6 mb-3">
                                                            <!-- <label for="validationCustom03" class="font-weight-bold">Website : www.</label> -->
                                                            <asp:TextBox class="form-control" ID="tctccnumber" runat="server" placeholder="Contact Number" ToolTip="Contact No" MaxLength="10" onkeyup="integersOnly(this);" />
                                                            <%--<input type="text" class="form-control" id="validationCustom03" placeholder="Website : www.">--%>
                                                            <div class="valid-feedback">
                                                                Looks good!
                                                            </div>
                                                            <asp:Label ID="Label17" runat="server" Text="Contact No (With Country Code)"></asp:Label>
                                                        </div>

                                                    </div>

                                                    <div id="sgdivshowlocation" runat="server">

                                                        <div class="form-row">
                                                            <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Latitude </label>
                                                            <div class="col-md-6 mb-3">
                                                                <asp:TextBox class="form-control" ID="txtcLatitude" runat="server" OnTextChanged="txtcLatitude_TextChanged" placeholder="Latitude" ToolTip="Latitude" />
                                                                <div class="valid-feedback">
                                                                    Looks good!
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-row">
                                                            <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Longitude </label>
                                                            <div class="col-md-6 mb-3">
                                                                <asp:TextBox class="form-control" ID="txtcLongitude" runat="server" OnTextChanged="txtcLongitude_TextChanged" placeholder="Longitude" ToolTip="Longitude" />
                                                                <div class="valid-feedback">
                                                                    Looks good!
                                                                </div>
                                                            </div>
                                                            <asp:Label ID="Label18" runat="server" Text=""></asp:Label>
                                                        </div>

                                                    </div>
                                                    <div class="form-row" id="divcarsS" runat="server" >
                                                        <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Text </label>
                                                        <div class="col-md-6 mb-3">
                                                            <!-- <label for="validationCustom03" class="font-weight-bold">Website : www.</label> -->
                                                            <asp:TextBox class="form-control" MaxLength="25" ID="txtcctext" runat="server" placeholder="Text" ToolTip="Text" />
                                                            <%--<input type="text" class="form-control" id="validationCustom03" placeholder="Website : www.">--%>
                                                            <div class="valid-feedback">
                                                                Looks good!
                             
                                                            </div>
                                                        </div>



                                                    </div>
                                                </div>

                                                <div class="col-md-12" id="suggectionCrasoul" runat="server">
                                                    <div class="card-body">
                                                        <asp:GridView ID="gvcrauselS" HeaderStyle-Height="25px" OnRowDeleting="gvcrauselS_RowDeleting" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false" OnRowDataBound="gvcrauselS_RowDataBound"
                                                            runat="server" Width="100%" CellPadding="10" BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive dataTable-view">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Sr No">
                                                                    <ItemTemplate>
                                                                        <%#Container.DataItemIndex+1 %>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Suggestion Type">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbldtype" runat="server" Text='<%#Eval("SuggetionType")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>



                                                                <asp:TemplateField HeaderText="Text">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbl2" runat="server" Text='<%#Eval("SuggestionText")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Url">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbl3" runat="server" Text='<%#Eval("SuggestionUrl")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Phone">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCreatedDate" runat="server" Text='<%#Eval("SuggestionPhone")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Latitude">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblurlid" runat="server" Text='<%#Eval("SuggestionLatitude")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Longitude">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblurlids" runat="server" Text='<%#Eval("SuggestionLongitude")%>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="row">
                                                                    <ItemTemplate>

                                                                        <asp:LinkButton ID="lbtnDelete" runat="server" class="mx-1 btn btn-primary text-success" CommandName="delete" OnClientClick="return confirm('Are you sure you want to delete this record?');"
                                                                            data-toggle="tooltip" data-placement="top" title="" data-original-title="Delete"> 
                                                                            <span class="text-danger"> <i class="fas fa-times"></i> </span></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>

                                                            </Columns>


                                                        </asp:GridView>
                                                    </div>
                                                </div>


                                            </fieldset>
                                        </div>

                                    </div>


                                </div>
                            </div>


                        </div>


                    </div>


                </ContentTemplate>

                <Triggers>
                    <asp:PostBackTrigger ControlID="UploadImg" />
                </Triggers>
                <Triggers>
                    <asp:PostBackTrigger ControlID="Lkbtnvideo" />
                </Triggers>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnsubmit" />
                </Triggers>
                <Triggers>
                    <asp:PostBackTrigger ControlID="carofile" />
                </Triggers>
                <Triggers>
                    <asp:PostBackTrigger ControlID="cardbtnfile" />
                </Triggers>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnnewcard" />
                </Triggers>
                <Triggers>
                    <asp:PostBackTrigger ControlID="txtTempText" />
                </Triggers>






            </asp:UpdatePanel>
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <div class="row d-none" id="divSearch" runat="server">

                        <div class="col">
                            <div class="card card-body bg-primary border-light shadow-soft mb-4">
                                <!--  -->
                                <div class="row align-items-center">
                                    <div class="col-12 col-md-3">
                                        <asp:DropDownList ID="ddlSelectrcstype" runat="server" class="custom-select my-3 my-lg-0">
                                            <asp:ListItem Text="TEXT" Value="1" Selected="True">TEXT</asp:ListItem>
                                            <asp:ListItem Text="IMAGE" Value="2">IMAGE</asp:ListItem>
                                            <asp:ListItem Text="VIDEO" Value="3">VIDEO</asp:ListItem>
                                            <asp:ListItem Text="CARD" Value="4">CARD</asp:ListItem>
                                            <asp:ListItem Text="CAROUSEL" Value="5">CAROUSEL</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>

                                    <div class="col-6 col-md-2">
                                        <asp:LinkButton runat="server" ID="lnkShow" OnClick="lnkShow_Click" class="btn btn-block my-3 my-lg-0">
                                            Show <i class="fas fa-eye" aria-hidden="true"></i>
                                        </asp:LinkButton>

                                    </div>

                                </div>
                                <!--  -->
                            </div>
                        </div>

                        <div class="col-xl-12 col-lg-12">
                            <div class="card bg-primary border-light shadow-soft mb-4">
                        <div class="card-header py-3 bg-primary d-flex justify-content-between flex-wrap align-content-center">
                            <h6 class="m-0 font-weight-bold my-auto"> Template List</h6>
                        </div>
                        <div class="card-body">
                             <div class="table-responsive">
                            <asp:GridView ID="grv" HeaderStyle-Height="25px"  OnRowDeleting="grv_RowDeleting" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false" OnRowCommand="grv_RowCommand"
                                runat="server" Width="100%" CellPadding="10" BorderColor="#ede8e8" Class="table table-striped table-bordered dt-responsive dataTable-view">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr No">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="User ID">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUserId" runat="server" Text='<%#Eval("userid")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>



                                            <asp:TemplateField HeaderText="TemplateID">
                                                <ItemTemplate>
                                                    <asp:Label ID="lbl2" runat="server" Text='<%#Eval("TemplateID")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                             <asp:TemplateField HeaderText="TemplateName">
                                                <ItemTemplate>
                                                  <div id="divt" runat="server" style="width:200px; text-align:justify">
                                                 
                                                <%#Eval("TemplateName")%>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CreatedDate">
                                                <ItemTemplate>
                                                     <span class="text-success"></span>
                                                    <asp:Label ID="lblCreatedDate" runat="server" Text='<%#Eval("CreatedDate")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Total card">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblurlid" runat="server" Text='<%#Eval("tcard")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                              
                                            <asp:TemplateField HeaderText="Test">
                                                <ItemTemplate>
                                                      
                                                    <asp:LinkButton ID="lbtntestsend" runat="server" class="mx-1 btn btn-primary text-success btn-block"  CommandArgument='<%#Eval("templateid")%>' CommandName="popup" ToolTip="Send" >
                                                        <span class="text-success"> <i class="fas fa-paper-plane"></i></span>
                                                    </asp:LinkButton>
                                                         
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField  HeaderText="Action">
                                                <ItemTemplate>
                                                     <div id="divAction" runat="server" style="width:115px;">
                                                 
                                                    <asp:LinkButton ID="LinkButton1" runat="server" class="mx-1 btn btn-primary text-success" CommandName="View" ToolTip="Edit"
                                                        CommandArgument='<%#Eval("templateid")%>' data-toggle="tooltip" data-placement="top" title="" data-original-title="Edit"> 
                                                                            <span class="text-success"> <i class="fas fa-edit"></i> </span></asp:LinkButton>
                                                    <asp:LinkButton ID="lbtnDelete" runat="server" class="mx-1 btn btn-primary text-success" CommandName="delete" OnClientClick="return confirm('Are you sure you want to delete this record?');"
                                                        CommandArgument='<%#Eval("templateid")%>' data-toggle="tooltip" data-placement="top" title="" ToolTip="Delete" data-original-title="Delete"> 
                                                                            <span class="text-danger"> <i class="fas fa-times"></i> </span></asp:LinkButton>
                                                </div>
                                                         </ItemTemplate>
                                            </asp:TemplateField>


                                        </Columns>

                                        <EmptyDataTemplate>
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td align="center">
                                                        <asp:Label ID="lblEmpty" Text="No Data Found!!!" Style="color: Red;" runat="server"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                 </div>
                                  </div>
                              
                            </div>

                        </div>

                        <div id="divcardetail" runat="server" class="card card-body mb-4 bg-primary border-light shadow-soft d-none">

                            <div id="multi-item-example" class="carousel slide carousel-multi-item">
                                <div class="carousel-inner pt-4" role="listbox">


                                    <div class="carousel-item active" style="white-space: nowrap; overflow-x: auto;">

                                        <asp:ListView ID="lvCrousel" runat="server" OnItemCommand="lvCrousel_ItemCommand" OnItemDataBound="lvCrousel_ItemDataBound" OnItemEditing="lvCrousel_ItemEditing"
                                            OnItemDeleting="lvCrousel_ItemDeleting" RepeatLayout="Flow" RepeatDirection="Horizontal">

                                            <ItemTemplate>

                                                <div class="col-md-3 " style="display: inline-block">
                                                    <div class="card card-height border-light shadow-soft mb-2">
                                                        <asp:Image ID="Img" runat="server" class="card-img-top" />
                                                        <iframe id="ifVideo" runat="server" src='<%# Eval("fileurl") %>' visible="false"></iframe>
                                                        <div id="divCard1" runat="server" class="card-body card_height">
                                                            <h5 class="card-title">

                                                                <asp:Label ID="lblTitle" CssClass="labelcss" class="card-text" runat="server" Text='<%# Eval("Cardtitle") %>'></asp:Label>

                                                                <asp:Label ID="lbltname" Visible="false" runat="server" Text='<%# Eval("templatename") %>'></asp:Label>
                                                                <asp:Label ID="lblid" Visible="false" runat="server" Text='<%# Eval("id") %>'></asp:Label>
                                                                <asp:Label ID="lblrcstype" Visible="false" runat="server" Text='<%# Eval("rcstype") %>'></asp:Label>
                                                            </h5>

                                                            <p class="card-text" id="pText" runat="server">
                                                                <asp:Label ID="Label11" runat="server" Text='<%# Eval("carddesc") %>'></asp:Label>
                                                                <asp:Label ID="lbltempText" Visible="false" runat="server" Text='<%# Eval("templatetext") %>'></asp:Label>
                                                            </p>
                                                            <p style="text-align: center;height: 20px;overflow: hidden; display:none;">
                     <asp:Label ID="Label1"  runat="server" Text='<%# Eval("Suggestiontext") %>' ></asp:Label>
             </p>
                                                           
                                                            <div class="row">
                                                                <asp:LinkButton ID="lkbtnEdit" class="w-45 btn btn-primary btn-icon-split  mt-2" CommandArgument='<%# Eval("ID")+","+Eval("tempid") %>' CommandName="Edit" runat="server">
                           <span class="text-success"><i class="fas fa-edit"></i></span>Edit</asp:LinkButton>

                                                                <asp:LinkButton ID="lbtnDelete" class="w-40  btn btn-primary btn-icon-split  mt-2" CommandArgument='<%# Eval("ID")+","+Eval("tempid") %>' CommandName="Delete" runat="server">
                           <span class="text-success"><i class="fas fa-times"></i></span> Delete</asp:LinkButton>


                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>


                                            </ItemTemplate>

                                        </asp:ListView>
                                        <div class="col-md-3" style="display: inline-block">
                                            <div class="card card-height border-light shadow-soft mb-2" style="position: absolute; top: 100%; transform: translate(5%, 100%);">

                                                <div class="card-body" id="addCard" runat="server">

                                                    <div class="text-center">
                                                        <asp:LinkButton CommandArgument='<%# Eval("ID") %>' ID="lbtnAddnewcard" class="btn btn-primary btn-icon-split  mt-2 mb-3" OnClick="LinkButton5_Click" runat="server">
                   <span class="text-success"><i class="fas fa-plus"></i> &nbsp;Add New Card</span></asp:LinkButton>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>






                                    </div>



                                </div>
                            </div>
                        </div>

                    </div>



                    <fieldset class="form-group card bg-primary border-light shadow-soft mb-2" style="border-radius: 10px;">
                        <legend class="col-form-label col-sm-3 pt-0 font-weight-bold"></legend>

                        <div class="form-row">
                            <div class="col-md-4" style="text-align: center;">
                                                    
                                <asp:LinkButton class="btn btn-primary btn-icon-split  mt-2 mb-3" ID="lbtnAdd" OnClick="lbtnAdd_Click" runat="server">
                                            <span class="text-success"><i class="fas fa-save"></i></span>
                                            <span class="text-success font-weight-bold">Add Template</span>
                                </asp:LinkButton>

                            </div>
                            <div class="col-md-4" style="text-align: center;">
                                <asp:LinkButton class="btn btn-primary btn-icon-split  mt-2 mb-3" ID="lbtnFind" OnClick="lbtnFind_Click" runat="server">
                                            <span class="text-success"><i class="fas fa-search"></i></span>
                                            <span class="text-success font-weight-bold">Find Template</span>
                                </asp:LinkButton>
                            </div>

                            <div class="col-md-4" style="text-align: center;">
                                <asp:LinkButton class="btn btn-primary btn-icon-split  mt-2 mb-3" ID="btnSave" runat="server" OnClick="btnSave_Click">
                                    <span id="span1" runat="server" class="text-success"><i id="ispan" runat="server" class="fas fa-save"></i></span>
                                    <span id="span" runat="server" class="text-success font-weight-bold">Save Template</span>
                                </asp:LinkButton>
                            </div>
                        </div>
                    </fieldset>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="lbtnAdd" />
                </Triggers>
                <Triggers>
                    <asp:PostBackTrigger ControlID="lbtnFind" />
                </Triggers>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnSave" />
                </Triggers>
                 
            </asp:UpdatePanel>
             
            <asp:Panel ID="pnlPopUp_Detail1" runat="server" CssClass="modal modalPopupS" Style="display: none;">
                <div class="modal-dialog modal-md modal-dialog-centered modal-dialog-scrollable">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div class="modal-content">
                                <%--  <div class="modal-header">
                    <asp:Label ID="Label15" runat="server" CssClass="modal-title" Text="Sender ID Wise Details"></asp:Label>
                </div>--%>
                                <div class="modal-body">
                                    <div class="">
                                        &nbsp; 
                                        <asp:Label ID="lbltcount" runat="server"></asp:Label>
                                        Cards are already submitted by you and are not saved in template.<br />
                                        Do you want to continue with the submitted cards.<br />
                                        If you select 'NO' the submitted cards will be removed.

                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <asp:Button ID="btnTempcrauselsave" runat="server" class="btn btn-primary" Text="Yes" OnClick="btnTempcrauselsave_Click" />
                                    <asp:Button ID="btnTempcrauseldel" runat="server" class="btn btn-primary" Text="No" OnClick="btnTempcrauseldel_Click" />

                                </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnTempcrauselsave" />
                        </Triggers>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="btnTempcrauseldel" />
                        </Triggers>
                    </asp:UpdatePanel>
                </div>
            </asp:Panel>

            

        </div>
    </main>
  

        
    <asp:Panel ID="testpopup" runat="server" CssClass="modal modalPopupS" Style="display: none;">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>

            
                <div class="modal-dialog modal-md modal-dialog-centered modal-dialog-scrollable">
                    
                            <div class="modal-content">
                                  <div class="modal-header">
                   
                </div>
                                <div class="modal-body">
                                    <div class="row">
                                    <div class="col-md-4">
                                        &nbsp; 
                                        <asp:Label ID="Label15" class="card-text" runat="server">Enter Mobile</asp:Label> : 
                                        </div>
                                         <div class="col-md-4" style="text-align:left">
                                       <asp:TextBox ID="txtsendmobile"  class="form-control" runat="server"></asp:TextBox>

                                    </div>
                                          <div class="col-md-2">
                                               <asp:Button ID="btnSend" runat="server" class="btn btn-primary" Text="Send" OnClick="btnSend_Click" />
                                              </div>
                                    
                                        </div>
                                </div>
                                <div class="modal-footer">
                                    
                                    <asp:Button ID="btntest" runat="server" class="btn btn-primary" Text="Cancel" />

                                </div>
                            </div>
                        
                </div>
        </ContentTemplate>
            <Triggers>
                    <asp:PostBackTrigger ControlID="btnSend" />
                </Triggers>
             <Triggers>
                    <asp:PostBackTrigger ControlID="btntest" />
                </Triggers>
        </asp:UpdatePanel>
            </asp:Panel>
            
    <asp:LinkButton Text="" ID ="lnkFake" runat="server" />
    <asp:LinkButton Text="" ID ="lnkSend" runat="server" />
<cc1:ModalPopupExtender ID="pnlPopUp_Detail_ModalPopupExtender" runat="server" PopupControlID="pnlPopUp_Detail1" TargetControlID="lnkFake"
CancelControlID="btntest" BackgroundCssClass="modalBackground">
</cc1:ModalPopupExtender>

    <cc1:ModalPopupExtender ID="mpetestpopup" runat="server" PopupControlID="testpopup" TargetControlID="lnkSend"
CancelControlID="btntest" BackgroundCssClass="modalBackground">
</cc1:ModalPopupExtender>

    <%--pnlPopUp_Detail Modal Popup Extender For pnlPopUp_Detail--%>
   <%-- <asp:LinkButton ID="lnkDetail" runat="server"></asp:LinkButton>
    <cc1:ModalPopupExtender ID="mpetestpopup" runat="server" PopupControlID="testpopup"
        TargetControlID="lnkDetail" BehaviorID="mpeAddUpdateEmployee" CancelControlID="btnCancel"
        BackgroundCssClass="modalBackground">
    </cc1:ModalPopupExtender>--%>
     



    <script type="text/javascript" src="http://maps.googleapis.com/maps/api/js?sensor=false"></script>
    <script>
        function getLocation() {
            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(showPosition);
            } else {
                document.getElementById("lockmsg").value = "Geolocation is not supported by this browser.";
            }
        }
        function showPosition(position) {
            document.getElementById("txtcrdlatitude").value = position.coords.latitude
            document.getElementById("txtcrdlongitude").value = position.coords.longitude;


        }
    </script>
    <script type="text/javascript">

        function integersOnly(obj) {
            obj.value = obj.value.replace(/[^0-9,\r\n]/g, '');


        }
        function displayNumberCount() {
            document.getElementById('msg').innerHTML = document.getElementById('txttitle').value.length
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







        function getLength(ln) {
            var i = 0;
            if (ln >= 1) i = 1;
            if (ln > 1024) i = 2;
            if (ln > 2048) i = 3;
            if (ln > 3072) i = 4;
            if (ln > 4096) i = 5;
            if (ln > 5120) i = 6;
            if (ln > 6144) i = 7;
            if (ln > 7168) i = 8;
            if (ln > 8192) i = 9;
            if (ln > 9216) i = 10;
            if (ln > 10240) i = 11;
            if (ln > 11264) i = 12;
            return i;
        }

        function getUniCodeLength(ln) {
            var i = 0;
            if (ln >= 1) i = 1;
            if (ln > 1024) i = 2;
            if (ln > 3072) i = 3;
            if (ln > 4096) i = 4;
            if (ln > 5120) i = 5;
            if (ln > 6144) i = 6;
            if (ln > 7168) i = 7;
            if (ln > 8192) i = 8;
            if (ln > 9216) i = 9;
            if (ln > 10240) i = 10;
            return i;
        }
        function validUrl() {
            alert("File  !!");
        }
        function smscnt() {

            var s = document.getElementById("<%=txtTempText.ClientID%>").value;
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
                <%--var ou = document.getElementById("<%=lblOptOut.ClientID%>").innerText;
                console.log(ou);
                ln = ln + ou.length;--%>

                <%--console.log(document.getElementById('<%= ddlRCSType.ClientID %>').val);
                console.log(document.getElementById('<%= ddlRCSType.ClientID %>').value);--%>

                <%--if (document.getElementById('<%= ddlRCSType.ClientID %>').value == "2") ln = ln + 2;--%>
                i = getLength(ln);
                document.getElementById('<%= lblUniCode.ClientID %>').innerHTML = "";
                var Used = 0;
                Used = i - 1;
                Used = Used * 1024;
                <%--document.getElementById('<%= lblsmscnt.ClientID %>').innerHTML = "No. of Char : " + ln + ". <br />No. of RCS : " + i.toString();--%>
                document.getElementById('<%= lblsmscnt.ClientID %>').innerHTML = i.toString();
                document.getElementById('<%= lblused.ClientID %>').innerHTML = "" + ln - Used + " / 1024 ";
                var y = 0;
                for (var k = 0, n = s.length; k < n; k++) {
                    if (s.charCodeAt(k) > 126) {
                        y = 1;
                    }
                }
                if (y == 1) {
                    ln = s.length;
                    i = getUniCodeLength(ln);
                    document.getElementById('<%= lblUniCode.ClientID %>').innerHTML = "UNICODE : YES";
                    var Used = 0;
                    Used = i - 1;
                    Used = Used * 1024;
                    <%--document.getElementById('<%= lblsmscnt.ClientID %>').innerHTML = "No. of Char : " + ln + ". <br />No. of RCS: " + i.toString();--%>
                    document.getElementById('<%= lblsmscnt.ClientID %>').innerHTML = i.toString();
                    document.getElementById('<%= lblused.ClientID %>').innerHTML = "" + ln - Used + " / 1024 ";
                }
            }
            else {
                document.getElementById('<%= lblsmscnt.ClientID %>').innerHTML = "0";
                document.getElementById('<%= lblused.ClientID %>').innerHTML = "0";
            }

        }
    </script>
    <script type="text/javascript"> 
        function showLoading() {
            document.getElementById('<%= lblUploading.ClientID %>').innerHTML = "Uploading. Please Wait ... ";
        }

        function showLoading2() {
            document.getElementById('<%= lblUploading2.ClientID %>').innerHTML = "Uploading. Please Wait ... ";
        }
        function SMSfileUpload60() {
            alert("File  !!");
            var uploadControl = document.getElementById('<%= FileUpload3.ClientID %>');
            var myfile = uploadControl.value;
            console.log(myfile);
            var Extension = myfile.substring(myfile.lastIndexOf('.') + 1).toLowerCase();
            console.log(Extension);


            if ((Extension != "jpg" && Extension != "png")) {
                alert("File Shoul be jpg or png format !!");
                return false;
            } else {
                //showLoading();
                console.log("ret true");
                return true;
            }

            function url() {

                alert("this is test !!");
            }

        }
    </script>

</asp:Content>

