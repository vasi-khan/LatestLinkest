<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="Notification.aspx.cs" Inherits="eMIMPanel.Notification" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        /*CSS Classes For Design Modal*/
        .modal.modalPopup {
            top: 0 !important;
            left: 0 !important;
            display: block;
        }

        .modalBackground {
            background-color: #000;
            opacity: 0.5;
        }
    </style>
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
                    <li class="breadcrumb-item active" aria-current="page">Notifications</li>
                </ol>
            </nav>



            <div class="row">
                <div class="col-xl-12 col-lg-12">
                    <!-- Basic Card Example -->
                    <div class="card bg-primary border-light shadow-soft mb-4">
                        <div class="card-header py-3 bg-primary d-flex justify-content-between flex-wrap align-content-center">
                            <h6 class="font-weight-bold my-lg-auto mb-3"><i class="fas fa-bell"></i>Notifications</h6>
                        </div>
                        <div class="card-body">
                            <form>

                                <fieldset class="form-group mb-4">
                                    <div class="row">
                                        <legend class="col-form-label col-sm-2 pt-0 font-weight-bold">VIA</legend>
                                        <div class="col-sm-4">
                                            <div class="input-group">
                                                <div class="input-group-prepend">
                                                    <div class="input-group-text">
                                                        <asp:CheckBox runat="server" ID="chmob" aria-label="Checkbox for following text input"></asp:CheckBox>
                                                    </div>
                                                </div>
                                                <asp:TextBox ID="txtMobile" runat="server" TextMode="MultiLine" Rows="3" onchange="javascript:text_changed_from();" class="form-control" placeholder="Enter Mobile No" autocomplete="off"></asp:TextBox>

                                            </div>
                                        <p class="m-0 mt-1">Max 5 Number allow with comma(,) seprated</p>
                                        </div>
                                        <div class="col-sm-6">
                                            <div class="input-group">
                                                <div class="input-group-prepend">
                                                    <div class="input-group-text">
                                                        <asp:CheckBox runat="server" ID="chmail" aria-label="Checkbox for following text input"></asp:CheckBox>

                                                    </div>
                                                </div>
                                                <asp:TextBox ID="txtEmail" runat="server" TextMode="MultiLine" Rows="3" onchange="javascript:text_changed_from();" class="form-control" placeholder="Enter Email ID" autocomplete="off"></asp:TextBox>

                                            </div>
                                            <p class="m-0 mt-1">Max 5 Email allow with comma(,) seprated</p>
                                        </div>
                                    </div>
                                </fieldset>
                                <fieldset class="form-group mb-4">
                                    <div class="row">
                                        <legend class="col-form-label col-sm-2 pt-0 font-weight-bold">Schedule</legend>
                                        <div class="col-sm-2">
                                            <a href="#" class="btn text-dark btn-icon-split mr-3 btn-block text-left">
                                                <span class="text-dark">
                                                    <asp:CheckBox runat="server" ID="chDaily" aria-label="Checkbox for following text input"></asp:CheckBox>

                                                </span>
                                                <span class="text font-weight-bold">Daily</span>
                                            </a>
                                        </div>
                                        <div class="col-sm-2">
                                            <a href="#" class="btn text-dark btn-icon-split mr-3 btn-block text-left">
                                                <span class="text-dark">
                                                    <asp:CheckBox runat="server" ID="chWeekly" aria-label="Checkbox for following text input"></asp:CheckBox>

                                                </span>
                                                <span class="text font-weight-bold">Weekly</span>
                                            </a>
                                        </div>
                                        <div class="col-sm-2">
                                            <a href="#" class="btn text-dark btn-icon-split mr-3 btn-block text-left">
                                                <span class="text-dark">
                                                    <asp:CheckBox runat="server" ID="chMonthly" aria-label="Checkbox for following text input"></asp:CheckBox>

                                                </span>
                                                <span class="text font-weight-bold">Monthly</span>
                                            </a>
                                        </div>
                                    </div>
                                </fieldset>
                                <div class="form-group row justify-content-end">
                                    <div class="col-sm-10 ">
                                        <asp:LinkButton runat="server" ID="aaaa" OnClick="btnUpdate_Click" class="btn btn-mini">
                                                    Update 
                                        </asp:LinkButton>
                                       <%-- <asp:LinkButton runat="server" ID="btntest" OnClick="btnTest_Click" class="btn btn-mini">Daily</asp:LinkButton>
                                        <asp:LinkButton runat="server" ID="LinkButton2" OnClick="btnWeekly_Click" class="btn btn-mini">Weekly</asp:LinkButton>
                                        <asp:LinkButton runat="server" ID="LinkButton3" OnClick="btnMonthly_Click" class="btn btn-mini">Monthly</asp:LinkButton>--%>
                                    </div>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
    </main>

    <script type="text/javascript"> 
        function text_changed_from() {
            var d = document.getElementById("ContentPlaceHolder1_txtFrm").value
            console.log(d);
            document.getElementById("ContentPlaceHolder1_hdntxtFrm").value = d;
        }
        function text_changed_to() {
            var d = document.getElementById("ContentPlaceHolder1_txtTo").value
            console.log(d);
            document.getElementById("ContentPlaceHolder1_hdntxtTo").value = d;
        }

    </script>

    <script>
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
    </script>
</asp:Content>
