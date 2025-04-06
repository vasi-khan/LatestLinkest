<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Shapoorjibandra.aspx.cs" Inherits="eMIMPanel.Shapoorjibandra" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>


    <html xmlns="http://www.w3.org/1999/xhtml">

        <head runat="server">
            <meta charset="euc-jp">
            <!-- Required meta tags -->

            <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">

            <!-- Bootstrap CSS -->
            <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@4.5.3/dist/css/bootstrap.min.css" integrity="sha384-TX8t27EcRE3e/ihU7zmQxVncDAy5uIKz4rEkgIXeMed4M0jlfIDPvg6uqKI2xXr2" crossorigin="anonymous">

            <!-- <link rel="stylesheet" href=""> -->

            <title>Invitation</title>
        </head>

        <body class="bg-dark">

            <style>
                .bg-dark {
                    background: #e0cbbd !important;
                }
                /* .btn-primary {
                    color: #27648a;
                    background-color: #e6e7ee;
                    font-weight: 500;
                    border: none;
                } */
                
                .masthead {
                    /* height: 65vh;
                    min-height: 470px;
                    background-image: url('assets/birla-min.jpg'); */
                    /* background: #0e4127; */
                    background-size: cover;
                    background-position: center;
                    background-repeat: no-repeat;
                }
                
                .pro-detail h1 {
                    color: #00227c;
                    font-weight: 700;
                    font-size: 2em;
                    line-height: 1.4em;
                }
                
                .pro-detail h2 {
                    font-size: 1.2em;
                    margin-top: 1em;
                    line-height: 1.4em;
                    color: #5d5d5d;
                    font-weight: 300;
                }
                
                .pro-detail p {
                    font-size: 1.6em;
                    margin-top: .7em;
                    color: #343a3f;
                }
                
                p.price {
                    font-size: 2em;
                    font-weight: 700;
                    color: #00237c;
                }
                
                .table td,
                .table th {
                    font-size: 14px;
                    text-align: left;
                }
                
                .ad-detail .table .thead-dark th {
                    font-size: 12px;
                }
                /* .pro-detail {margin-top: 9em;} */
                /* // Extra small devices (portrait phones, less than 576px) */
                
                @media (max-width: 575.98px) {
                    /* .pro-detail {margin-top: 9em;} */
                    .pro-detail h1 {
                        font-size: 1.3em;
                    }
                    .pro-detail h2 {
                        font-size: 1em;
                    }
                    .pro-detail p {
                        font-size: 1.2em;
                    }
                    .pro-img {
                        margin-top: 4em;
                        margin-bottom: 0;
                    }
                    .pro-detail .btn-link {
                        font-size: 13px;
                    }
                }
                /* // Small devices (landscape phones, 576px and up) */
                
                @media (min-width: 576px) and (max-width: 767.98px) {
                    /* .pro-detail {margin-top: 9em;} */
                    .pro-detail h1 {
                        font-size: 1.3em;
                    }
                    .pro-detail h2 {
                        font-size: 1em;
                    }
                    .pro-detail p {
                        font-size: 1.2em;
                    }
                    .pro-img {
                        margin-top: 4em;
                        margin-bottom: 0;
                    }
                    .pro-detail .btn-link {
                        font-size: 13px;
                    }
                }
                /* // Medium devices (tablets, 768px and up) */
                
                @media (min-width: 768px) and (max-width: 991.98px) {}
                /* // Large devices (desktops, 992px and up) */
                
                @media (min-width: 992px) and (max-width: 1199.98px) {}
                /* // Extra large devices (large desktops, 1200px and up) */
                
                @media (min-width: 1200px) {
                    /* .pro-detail {margin-top: 9em;} */
                }
            </style>

            <!-- Navigation -->
            <nav class="navbar navbar-expand-lg navbar-light p-0" style="background:#f4e4d9">
                <div class="container justify-content-center">
                    <div class="row">
                        <div class="col">
                            <a class="navbar-brand m-0 w-100 p-0" href="#">
                                <img src="assets/sapor-img/shapoorji-logo.jpg" alt="shapoorji-logo" width="200px"></a>
                        </div>
                    </div>

                </div>
            </nav>

            <!-- Full Page Image Header with Vertically Centered Content -->
            <header class="masthead pt-2">
                <div class="container h-100">
                    <div class="row">
                        <div class="col-12">
                            <img src="assets/sapor-img/Project-Mailer.jpg" alt="Edge-Event-7" class="img-fluid w-100">
                        </div>
                    </div>
                </div>
            </header>


            <section>
                <div class="container">
                    <div class="row h-100 align-items-center mt-4">
                        <div class="col-lg-12">
                            <div class="card border-0 shadow ad-detail">
                                <div class="card-header" style="background: #433a33;color: #fff;">
                                    <h3 class="h5 text-center mb-0 font-weight-bold">Confirmation Form</h3>
                                </div>
                                <div class="card-body">
                                    <form id="form" runat="server" class="needs-validation" novalidate>
                                        <asp:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
                                        </asp:ToolkitScriptManager>
                                        <div id="divMain" runat="server" class="form-row" style="display: block;">
                                            <div class="col-md-12 mb-3">
                                                <label for="validationCustom01" class="sr-only">First name</label>
                                                <asp:TextBox ID="txtName" runat="server" class="form-control" placeholder="Full Name"></asp:TextBox>

                                            </div>
                                            <div class="col-md-12 mb-3">
                                                <label for="validationCustom02" class="sr-only">Phone Number</label>
                                                <asp:TextBox ID="txtPhone" runat="server" class="form-control" MaxLength="10" placeholder="Phone Number"></asp:TextBox>
                                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterMode="ValidChars" TargetControlID="txtPhone" ValidChars="0123456789">
                                                </asp:FilteredTextBoxExtender>

                                            </div>
                                            <div class="col-md-2 mb-3">
                                                <asp:Button ID="btnOTP" runat="server" class="btn btn-primary mb-1 btn-block" Text="Send OTP" OnClick="btnOTP_Click"></asp:Button>

                                            </div>
                                        </div>
                                        <div id="divOTP" runat="server" style="display: none">
                                            <div class="form-row">
                                                <div class="col-md-12 mb-3">
                                                    <label for="validationCustom01" class="sr-only">OTP</label>
                                                    <asp:TextBox ID="txtOTP" runat="server" class="form-control" placeholder="OTP"></asp:TextBox>

                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="form-check">
                                                    <%--<asp:CheckBox ID="chkAgree" runat="server" class="form-check-input" Text="Agree Invitation" />--%>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-6 mb-3">
                                                    <asp:Button ID="btnResend" runat="server" class="btn btn-success mb-1 btn-block" Text="Resend OTP" OnClick="btnResend_Click"></asp:Button>
                                                </div>
                                                <div class="col-md-6 mb-3">
                                                    <asp:Button ID="btnSubmit" runat="server" class="btn btn-primary mb-1 btn-block" Text="Submit Form" OnClick="btnSubmit_Click"></asp:Button>
                                                </div>
                                            </div>
                                        </div>
                                    </form>

                                    <div id="divSuccess" runat="server" class="alert alert-success text-center" role="alert" style="display: none;">
                                        <h4 class="alert-heading">Thank You</h4>
                                        <p>You have been successfully registered</p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>

            <footer class="mastfoot mt-auto fixed-bottom">
                <div class="bg-dark text-center small">
                    <p class="m-0 py-1 text-dark"><strong>Powered by :</strong> My Inbox Media &#174;</p>
                </div>
            </footer>


            <!-- Option 1: jQuery and Bootstrap Bundle (includes Popper) -->
            <script src="https://code.jquery.com/jquery-3.5.1.js"></script>
            <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.5.3/dist/js/bootstrap.bundle.min.js" integrity="sha384-ho+j7jyWK8fNQe+A12Hb8AhRq26LrZ/JpcUGGOn+Y7RsweNrtN/tE3MoK7ZeZDyx" crossorigin="anonymous"></script>

            <script>
                // Example starter JavaScript for disabling form submissions if there are invalid fields
                (function() {
                    'use strict';
                    window.addEventListener('load', function() {
                        // Fetch all the forms we want to apply custom Bootstrap validation styles to
                        var forms = document.getElementsByClassName('needs-validation');
                        // Loop over them and prevent submission
                        var validation = Array.prototype.filter.call(forms, function(form) {
                            form.addEventListener('submit', function(event) {
                                if (form.checkValidity() === false) {
                                    event.preventDefault();
                                    event.stopPropagation();
                                }
                                form.classList.add('was-validated');
                            }, false);
                        });
                    }, false);
                })();
            </script>

        </body>

        </html>