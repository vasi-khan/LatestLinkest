<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="rich_media_url_u1.aspx.cs" Inherits="eMIMPanel.rich_media_url_u1" %>

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
                            <h6 class="font-weight-bold my-lg-auto mb-3"><i class="fas fa-ad"></i>Rich Media URL</h6>
                        </div>
                        <div class="card-body">
                            <fieldset class="form-group">
                                <div class="row">
                                    <legend class="col-form-label col-sm-2 pt-0 font-weight-bold">Upload</legend>
                                    <div class="col-sm-5">
                                        <div class="custom-file">
                                            <asp:FileUpload ID="FileUpload1" runat="server" class="custom-file-input" ClientIDMode="Static" onchange="showLoading(); this.form.submit()" />
                                            <label class="custom-file-label" for="customFile">Video file upload</label>
                                        </div>
                                        <small id="textHelp" class="form-text text-muted">Upload Limit <strong>25 MB.</strong> &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblUploading" runat="server"></asp:Label> </small>
                                    </div>
                                    <div class="col-sm-5">
                                        <div class="custom-file">
                                            <asp:FileUpload ID="FileUpload2" runat="server" class="custom-file-input" ClientIDMode="Static" onchange="showLoading2(); this.form.submit()" />
                                            <label class="custom-file-label" for="customFile">Brand Logo Upload</label>
                                        </div>
                                        <small id="textHelp2" class="form-text text-muted">Upload Limit <strong>200 Kb.</strong> &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblUploading2" runat="server"></asp:Label> </small>
                                    </div>
                                </div>
                            </fieldset>
                            <fieldset class="form-group mb-4">
                                <div class="row">
                                    <legend class="col-form-label col-sm-2 pt-0 font-weight-bold">Call To Action (CTA)</legend>
                                    <div class="col-sm-5">
                                        <div class="input-group">
                                            <div class="input-group-prepend">
                                                <div class="input-group-text">
                                                    <asp:CheckBox ID="chkVisit" runat="server" aria-label="Checkbox for following text input" />
                                                </div>
                                            </div>
                                            <asp:TextBox ID="txtVisitButtonName" runat="server" aria-label="First name" class="form-control" placeholder="Enter Button Name"></asp:TextBox>
                                            <asp:TextBox ID="txtVisitUs" runat="server" class="form-control" aria-describedby="genHelp" placeholder="Enter a link" MaxLength="300"></asp:TextBox>
                                        </div>
                                        <small id="genHelp" class="form-text text-muted">Button Name Example - Visit Us</small>
                                    </div>
                                    <div class="col-sm-5">
                                        <div class="input-group">
                                            <div class="input-group-prepend">
                                                <div class="input-group-text">
                                                    <asp:CheckBox ID="chkCall" runat="server" aria-label="Checkbox for following text input" />
                                                </div>
                                            </div>
                                            <asp:TextBox ID="txtCallButtonName" runat="server" aria-label="First name" class="form-control" placeholder="Enter Button Name"></asp:TextBox>
                                            <asp:TextBox ID="txtCallUs" runat="server" class="form-control" aria-describedby="genHelp" placeholder="Enter Mobile No" MaxLength="13"></asp:TextBox>
                                        </div>
                                        <small id="genHelp3" class="form-text text-muted">Button Name Example - Call Us</small>
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
                            <div class="form-group row justify-content-end">
                                <div class="col-sm-10 ">
                                    <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" class="btn btn-primary text-secondary"><i class="fas fa-fw fa-link text-secondary"></i>Make Short</asp:LinkButton>
                                </div>
                            </div>
                            <fieldset class="form-group">
                                <div class="row">
                                    <legend class="col-form-label col-sm-2 pt-0 font-weight-bold">Generated URL</legend>
                                    <div class="col-sm-10">
                                        <asp:TextBox ID="lblShortURL" class="form-control" runat="server" aria-describedby="textHelp" placeholder="Generated URL"></asp:TextBox>
                                        <small id="textHelp3" class="form-text text-muted">Short URL: <strong>http://emim.in/word</strong></small>
                                    </div>
                                </div>
                            </fieldset>
                            <div class="form-group row justify-content-end">
                                <div class="col-sm-10 ">
                                    <button type="submit" class="btn btn-primary text-success font-weight-bold"><i class="far fa-copy"></i>Copy</button>
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
