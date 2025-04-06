<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="url-shortner_u.aspx.cs" Inherits="eMIMPanel.url_shortner_u" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </asp:ToolkitScriptManager>



    <main>
        <div class="container-fluid">
            <nav aria-label="breadcrumb" class="my-3">
                <ol class="breadcrumb breadcrumb-info">
                    <li class="breadcrumb-item"><a href="#">Home</a></li>
                    <li class="breadcrumb-item"><a href="#">Linkext</a></li>
                    <li class="breadcrumb-item active" aria-current="page">URL Shortner</li>
                </ol>
            </nav>

            <!-- Content Row -->
            <div class="row">
                <div class="col-xl-12 col-lg-12">
                    <!-- Basic Card Example -->
                    <div class="card bg-primary border-light shadow-soft mb-4">
                        <div class="card-header py-3 bg-primary d-flex justify-content-between flex-wrap align-content-center">
                            <h6 class="font-weight-bold my-lg-auto mb-0"><i class="fas fa-link"></i>URL Shortner</h6>
                        </div>
                        <div class="card-body pt-0">
                            <form>
                                <asp:UpdatePanel ID="updFormArea" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <fieldset class="form-group">
                                            <div class="row">
                                                <legend class="col-form-label col-sm-2 pt-0 font-weight-bold">Long URL:</legend>
                                                <div class="col-sm-10">
                                                    <asp:TextBox ID="txtLongURL" runat="server" class="form-control" aria-describedby="textHelp" placeholder="Enter Long URL"></asp:TextBox>
                                                    <%--<input type="text" class="form-control" id="exampleInputText1" aria-describedby="textHelp" placeholder="Enter Long URL">--%>
                                                    <%--<small id="textHelp" class="form-text text-muted">Short URL: <strong>http://emim.in/word</strong></small>--%>
                                                </div>
                                            </div>
                                        </fieldset>
                                        <fieldset class="form-group">
                                            <div class="row">
                                                <legend class="col-form-label col-sm-2 pt-0 font-weight-bold">Keyword:</legend>
                                                <div class="col-sm-10">
                                                    <%--<input type="text" class="form-control" id="exampleInputText2" aria-describedby="genHelp" placeholder="Enter word of your own choice">--%>
                                                    <asp:TextBox ID="txtShortURL" runat="server" type="text" class="form-control" aria-describedby="genHelp" placeholder="Enter word of your own choice"></asp:TextBox>
                                                    <small id="textHelp3" class="form-text text-muted">Short URL: <strong>http://m1m.io/word </strong>(Leave blank for <strong>autogenerate word</strong> for short URL)</small>
                                                </div>
                                            </div>
                                        </fieldset>
                                        <div class="form-group row ">
                                            <div class="col-sm-2 col-lg-2"></div>
                                            <div class="col-sm-3 col-lg-3">
                                                <%--<button type="submit" class="btn btn-primary text-secondary"><i class="fas fa-fw fa-link text-secondary"></i>Make Short</button>--%>
                                                <asp:LinkButton ID="LinkButton2" runat="server" class="btn btn-primary text-secondary btn-block" OnClientClick="return ConfirmBal();"
                                                    OnClick="btnSend_Click"><i class="fas fa-fw fa-link text-secondary"></i>Make Short</asp:LinkButton>
                                            </div>
                                        </div>
                                        <fieldset class="form-group">
                                            <div class="row">
                                                <legend class="col-form-label col-sm-2 pt-0 font-weight-bold">Generated URL</legend>
                                                <div class="col-sm-10">
                                                    <asp:TextBox ID="lblShortURL" runat="server" type="text" class="form-control" aria-describedby="textHelp" placeholder="Generated URL"></asp:TextBox>

                                                </div>
                                            </div>
                                        </fieldset>
                                           <fieldset class="form-group">
                                            <div class="row">
                                                <legend class="col-form-label col-sm-2 pt-0 font-weight-bold">Expiry of URL</legend>
                                                <div class="col-sm-10">                                                   
                                                    <asp:Label ID="lblExp" runat="server" class="form-control"></asp:Label>
                                                </div>
                                            </div>
                                        </fieldset>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="updFormArea"
                                    DisplayAfter="0">
                                    <ProgressTemplate>
                                        <div align="center" style="position: absolute; left: 40%; top: 50%; vertical-align: middle; z-index: 10000;">
                                            <img src="Img/LOADING.GIF" />
                                        </div>
                                        <div align="center" style="position: absolute; left: 0%; top: 0%; vertical-align: middle;">
                                        </div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>

                                <div class="form-group row">
                                    <div class="col-sm-2 col-lg-2"></div>
                                    <div class="col-sm-3 col-lg-3">
                                        <%--<button type="submit" class="btn btn-primary text-success font-weight-bold btn-block"><i class="far fa-copy"></i>Copy</button>--%>
                                        <asp:LinkButton ID="LinkButton1" runat="server" class="btn btn-primary text-success font-weight-bold btn-block">
                                                    <i class="far fa-copy"></i>Copy</asp:LinkButton>
                                    </div>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </main>


    <script type="text/javascript">
        function ConfirmBal() {
            console.log("reached");
            var curr =  ' <%= Session["CURRENCY"] %>';
            var rate = ' <%= Session["URLRATE"] %>';
            console.log(rate);
            var x = confirm(" " + rate + " " + curr + " will be deducted from the Balance. Do you want to continue ?");
            console.log(x);
            if (x == true) return true; else return false;

        }
    </script>
</asp:Content>
