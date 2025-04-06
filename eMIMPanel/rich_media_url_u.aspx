<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="rich_media_url_u.aspx.cs" Inherits="eMIMPanel.rich_media_url_u" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <cc1:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </cc1:ToolkitScriptManager>
    <main>
        <div class="container-fluid">
            <nav aria-label="breadcrumb" class="my-3">
                <ol class="breadcrumb breadcrumb-info">
                    <li class="breadcrumb-item"><a href="#">Home</a></li>
                    <li class="breadcrumb-item"><a href="#">Linkext</a></li>
                    <li class="breadcrumb-item active" aria-current="page">Rich Media URL</li>
                </ol>
            </nav>

            <!-- Content Row -->
            <div class="row">
                <div class="col-xl-12 col-lg-12">
                    <!-- Basic Card Example -->
                    <div class="card bg-primary border-light shadow-soft mb-4">
                        <div class="card-header py-3 bg-primary d-flex justify-content-between flex-wrap align-content-center">
                            <h6 class="font-weight-bold my-lg-auto mb-0"><i class="fas fa-ad"></i>Rich Media URL</h6>
                        </div>
                        <div class="card-body pt-0">
                            <fieldset class="form-group">
                                <div class="row">
                                    <legend class="col-form-label col-sm-2 pt-0 font-weight-bold">Upload</legend>
                                    <div class="col-12 col-sm-2">
                                        <div class="btn-group btn-group-toggle btn-block mb-3 mb-lg-0" data-toggle="buttons">
                                            <label class="btn text-secondary active">
                                                <i class="fas fa-video"></i>
                                                <asp:RadioButton ID="rdbVideo" runat="server" AutoPostBack="true" GroupName="vi" Checked="true" OnCheckedChanged="rdbVideo_CheckedChanged" />
                                                Video
                                            </label>
                                            <label class="btn text-secondary">
                                                <i class="fas fa-image"></i>
                                                <asp:RadioButton ID="rdbImg" runat="server" AutoPostBack="true" GroupName="vi" OnCheckedChanged="rdbVideo_CheckedChanged" />
                                                Image
                                            </label>
                                        </div>
                                    </div>
                                    <div class="col-12 col-sm-3">
                                        <div class="custom-file">
                                            <asp:FileUpload ID="FileUpload1" runat="server" class="custom-file-input" ClientIDMode="Static" onchange="showLoading(); this.form.submit()" />
                                            <label class="custom-file-label" for="customFile">
                                                <asp:Label ID="lblVI" runat="server" Text="Video"></asp:Label>
                                                file upload</label>
                                        </div>
                                        <small id="textHelp" class="form-text text-muted mb-3 mb-lg-0">Upload Limit <strong>
                                            <asp:Label ID="lblFileSize" runat="server"></asp:Label>
                                            MB.</strong> &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblUploading" runat="server"></asp:Label>
                                        </small>
                                    </div>
                                    <div class="col-3 col-sm-1">
                                        <div class="input-group-text">
                                            <asp:CheckBox ID="chkLogo" runat="server" AutoPostBack="true" OnCheckedChanged="chkLogo_CheckedChanged" aria-label="Checkbox for following text input" />
                                        </div>
                                    </div>
                                    <div class="col-9 col-sm-4" id="divUploadLogo" runat="server" style="pointer-events: none;">
                                        <div class="custom-file">
                                            <asp:FileUpload ID="FileUpload2" runat="server" class="custom-file-input" ClientIDMode="Static" onchange="showLoading2(); this.form.submit()" />
                                            <label class="custom-file-label" for="customFile">Brand Logo Upload</label>
                                        </div>
                                        <small id="textHelp2" class="form-text text-muted mb-3 mb-lg-0">Upload Limit <strong>200 Kb.</strong> &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblUploading2" runat="server"></asp:Label>
                                        </small>
                                    </div>
                                </div>
                            </fieldset>
                            <fieldset class="form-group mb-1 mb-lg-4">
                                <div class="row">
                                    <legend class="col-form-label col-sm-2 pt-0 font-weight-bold">Call To Action (CTA)</legend>
                                    <div class="col-sm-5">
                                        <div class="input-group">
                                            <div class="input-group-prepend">
                                                <div class="input-group-text">
                                                    <asp:CheckBox ID="chkVisit" runat="server" AutoPostBack="true" OnCheckedChanged="chkButton_CheckedChanged" aria-label="Checkbox for following text input" />
                                                </div>
                                            </div>
                                            <asp:TextBox ID="txtVisitButtonName" runat="server" aria-label="First name" class="form-control" placeholder="Button Name"></asp:TextBox>
                                            <asp:TextBox ID="txtVisitUs" runat="server" class="form-control" aria-describedby="genHelp" placeholder="Enter a link" MaxLength="300"></asp:TextBox>
                                        </div>
                                        <small id="genHelp" class="form-text text-muted mb-3 mb-lg-0">Button Name Example - Visit Us</small>
                                    </div>
                                    <div class="col-sm-5">
                                        <div class="input-group">
                                            <div class="input-group-prepend">
                                                <div class="input-group-text">
                                                    <asp:CheckBox ID="chkCall" runat="server" AutoPostBack="true" OnCheckedChanged="chkButton_CheckedChanged" aria-label="Checkbox for following text input" />
                                                </div>
                                            </div>
                                            <asp:TextBox ID="txtCallButtonName" runat="server" aria-label="First name" class="form-control" placeholder="Button Name"></asp:TextBox>
                                            <asp:TextBox ID="txtCallUs" runat="server" class="form-control" aria-describedby="genHelp" placeholder="Enter Mobile No" MaxLength="15"></asp:TextBox>
                                        </div>
                                        <small id="genHelp3" class="form-text text-muted mb-3 mb-lg-0">Button Name Example - Call Us</small>
                                    </div>
                                </div>
                            </fieldset>
                            <fieldset class="form-group mb-1 mb-lg-4">
                                <div class="row">
                                    <legend class="col-form-label col-sm-2 pt-0 font-weight-bold"></legend>
                                    <div class="col-sm-5">
                                        <div class="input-group">
                                            <div class="input-group-prepend">
                                                <div class="input-group-text">
                                                    <asp:CheckBox ID="chkWA" runat="server" AutoPostBack="true" OnCheckedChanged="chkButton_CheckedChanged" aria-label="Checkbox for following text input" />
                                                </div>
                                            </div>
                                            <asp:TextBox ID="txtWAButtonName" runat="server" aria-label="First name" class="form-control" placeholder="Button Name"></asp:TextBox>
                                            <asp:TextBox ID="txtWA" runat="server" class="form-control" aria-describedby="genHelp" placeholder="Enter Whatsapp No" MaxLength="15"></asp:TextBox>
                                        </div>
                                        <small id="gen2Help" class="form-text text-muted mb-3 mb-lg-0">Button Name Example - Whatsapp</small>
                                    </div>
                                    <div class="col-sm-5">
                                        <div class="input-group">
                                            <div class="input-group-prepend">
                                                <div class="input-group-text">
                                                    <asp:CheckBox ID="chkFB" runat="server" AutoPostBack="true" OnCheckedChanged="chkButton_CheckedChanged" aria-label="Checkbox for following text input" />
                                                </div>
                                            </div>
                                            <asp:TextBox ID="txtFBButtonName" runat="server" aria-label="First name" class="form-control" placeholder="Button Name"></asp:TextBox>
                                            <asp:TextBox ID="txtFB" runat="server" class="form-control" aria-describedby="genHelp" placeholder="Enter FB messenger" MaxLength="300"></asp:TextBox>
                                        </div>
                                        <small id="gen3Help" class="form-text text-muted mb-3 mb-lg-0">Button Name Example - FB messenger</small>
                                    </div>
                                </div>
                            </fieldset>
                            <fieldset class="form-group">
                                <div class="row">
                                    <legend class="col-form-label col-sm-2 pt-0 font-weight-bold">Keyword:</legend>
                                    <div class="col-sm-10">
                                        <asp:TextBox ID="txtShortURL" runat="server" class="form-control" aria-describedby="genHelp" placeholder="Enter word of your own choice" MaxLength="20"></asp:TextBox>
                                        <small id="genHelp2" class="form-text text-muted">(Leave blank for <strong>autogenerate word</strong> for short URL)</small>
                                    </div>
                                </div>
                            </fieldset>
                            <div class="form-group row">
                                <div class="col-md-2">
                                </div>
                                <div class="col-sm-3">
                                    <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" class="btn btn-primary text-secondary btn-block"><i class="fas fa-fw fa-link text-secondary"></i> Make Short</asp:LinkButton>
                                </div>
                                <div class="col-sm-3">
                                    <legend class="col-form-label col-sm-12 pt-0 font-weight-bold">Reference Page Name:</legend>
                                </div>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txtPageName" runat="server" class="form-control" aria-describedby="genHelp" placeholder="Enter Reference Page Name" MaxLength="15"></asp:TextBox>
                                </div>
                            </div>
                            <fieldset class="form-group">
                                <div class="row">
                                    <legend class="col-form-label col-sm-2 pt-0 font-weight-bold">Generated URL</legend>
                                    <div class="col-sm-10">
                                        <asp:TextBox ID="lblShortURL" class="form-control" runat="server" aria-describedby="textHelp" placeholder="Generated URL"></asp:TextBox>
                                        <small id="textHelp3" class="form-text text-muted">Short URL: <strong>http://m1m.io/word</strong></small>
                                    </div>
                                </div>
                            </fieldset>
                            <div class="row">
                                <div class="col-md-2"></div>
                                <div class="col-sm-10">
                                    <asp:Label ID="lblExp" runat="server" class="text-danger font-weight-bold text-center"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group row mt-2">
                                <div class="col-md-2"></div>
                                <div class="col-sm-3">
                                    <asp:LinkButton ID="LinkButton1" runat="server" class="btn btn-primary text-success font-weight-bold btn-block">
                                                    <i class="far fa-copy"></i>Copy</asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </main>
    <script type="text/javascript"> 

        function showLoading() {
            document.getElementById('<%= lblUploading.ClientID %>').innerHTML = "Uploading. Please Wait ... ";
        }

        function showLoading2() {
            document.getElementById('<%= lblUploading2.ClientID %>').innerHTML = "Uploading. Please Wait ... ";
        }
    </script>
</asp:Content>
