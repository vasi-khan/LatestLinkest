<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="eMIMPanel.Login" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="MSCaptcha" Namespace="MSCaptcha" TagPrefix="cc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
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

        #captchaContainer {
            text-align: center; /* Ensure the CAPTCHA is centered or properly aligned */
        }

        img.captchaImage {
            border: 1px solid #ddd; /* Optional: Add a border to ensure visibility */
        }
    </style>

    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no" />
    <meta name="description" content="" />
    <meta name="author" content="" />
    <title>Linkext - Login</title>
    <meta property="og:title" content="Linkext" />
    <meta property="og:type" content="SMS Panel" />
    <meta property="og:url" content="https://linkext.io" />
    <meta property="og:image" content="img/Linkext-logo.png" />
    <meta property="og:description" content="Application-to-Person bulk SMS tool has a dynamic Link Text feature capable of carrying voluminous data – text, videos, images, graphics, infographics, presentations, excel, tutorials, and more." />

    <!-- Custom fonts for this template-->
    <link href="vendor/fontawesome-free/css/all.min.css" rel="stylesheet" type="text/css" />
    <link href="https://fonts.googleapis.com/css?family=Nunito:200,200i,300,300i,400,400i,600,600i,700,700i,800,800i,900,900i" rel="stylesheet" />

    <!-- Custom styles for this template-->
    <!--  <link href="css/sb-admin-2.min.css" rel="stylesheet">-->

    <link rel="stylesheet" href="css/neumorphism.css" />
    <!-- Bootstrap core JavaScript-->
    <script src="vendor/jquery/jquery.min.js"></script>
    <script src="vendor/bootstrap/js/bootstrap.bundle.min.js"></script>

    <!-- Core plugin JavaScript-->
    <script src="vendor/jquery-easing/jquery.easing.min.js"></script>

    <!-- Custom scripts for all pages-->
    <script src="js/sb-admin-2.min.js"></script>
</head>

<body>
    <!-- Section -->
    <section class="min-vh-100 d-flex bg-primary align-items-center">
        <div class="container">
            <div class="row justify-content-center">
                <div class="col-12 col-md-8 col-lg-6 justify-content-center">
                    <div class="card bg-primary shadow-soft border-light p-4">
                        <div class="card-header text-center pb-0">
                            <img src="img/Linkext-logo.png" alt="Linkext Logo" width="170px" class="mb-3">
                            <h2 class="h4">Sign in</h2>
                        </div>
                        <div class="card-body">
                            <form id="form1" runat="server">
                                <asp:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
                                </asp:ToolkitScriptManager>
                                <!-- Form -->
                                <div class="form-group">
                                    <label for="exampleInputIcon3">Account ID</label>
                                    <div class="input-group mb-4">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text"><span class="fas fa-envelope"></span></span>
                                        </div>
                                        <asp:TextBox ID="txtUserID" runat="server" class="form-control form-control-user" aria-describedby="emailHelp" placeholder="Enter Account Id"></asp:TextBox>
                                    </div>
                                </div>
                                <!-- End of Form -->
                                <div class="form-group">
                                    <!-- Form -->
                                    <div class="form-group">
                                        <label for="exampleInputPassword6">Password</label>
                                        <div class="input-group mb-4">
                                            <div class="input-group-prepend">
                                                <span class="input-group-text"><span class="fas fa-unlock-alt"></span></span>
                                            </div>
                                            <asp:TextBox ID="txtPassword" runat="server" class="form-control form-control-user" TextMode="Password" ClientIDMode="Static" placeholder="Password"></asp:TextBox>
                                        </div>
                                    </div>
                                    <!-- End of Form -->
                                </div>
                                <div class="form-group">
                                    <!-- Form -->
                                    <div class="form-group">
                                        <div class="input-group mb-4">
                                            <cc1:CaptchaControl ID="captcha1" runat="server" CaptchaBackgroundNoise="Extreme" CaptchaLength="5"
                                                CaptchaHeight="45" CaptchaWidth="200" CaptchaMinTimeout="5" CaptchaMaxTimeout="240"
                                                FontColor="#D20B0C" NoiseColor="#B1B1B1" />
                                            <asp:ImageButton ImageUrl="~/img/refresh.png" OnClientClick="refreshCaptcha();return false;" runat="server" CausesValidation="false" />
                                            <asp:TextBox ID="txtCaptcha" runat="server" class="form-control form-control-user" placeholder="Enter Captcha"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="d-block d-sm-flex justify-content-between align-items-center mb-4">
                                        <div><a href="forget.aspx" class="small text-right">Forget password?</a></div>
                                    </div>
                                </div>
                                <asp:Button ID="btnLogin" runat="server" class="btn btn-primary btn-user btn-block" Text="Login" OnClick="btnLogin_Click" />
                                <br />

                                <%------OTP Div Start-------%>
                                <div class="form-group" id="divOTP" runat="server" visible="false">
                                    <label for="exampleInputIcon3">
                                        An OTP has been sent to the registered mobile number
                                        <asp:Label ID="lblSentOtpMobNo" runat="server"></asp:Label>
                                        please enter the OTP to verfily your account access</label>
                                    <div class="input-group mb-4">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text"><span class="fas fa-check"></span></span>
                                        </div>
                                        <asp:TextBox ID="txtOTPEnter" runat="server" class="form-control form-control-user" MaxLength="6" aria-describedby="emailHelp" placeholder="Enter OTP"></asp:TextBox>
                                    </div>
                                    <div class="form-group" style="text-align: center">
                                        <asp:Button ID="btnOTPSubmit" runat="server" class="btn btn-primary" Text="Submit" OnClick="btnOTPSubmit_Click" />
                                        <asp:Button ID="btnOTPResent" runat="server" class="btn btn-primary" Text="Resent" OnClick="btnOTPResent_Click" />
                                    </div>
                                </div>
                                <%-----------OTP Div END-------------%>
                                <script type="text/javascript">
                                    function MyFunction() {
                                        document.getElementById('<%=btnOTPResent.ClientID %>').disabled = true;
                                        setTimeout(function () { document.getElementById('<%=btnOTPResent.ClientID %>').disabled = false; }, 10000);
                                    }
                                </script>
                                <script type="text/javascript">
                                    function Confirm() {
                                        $('#logoutModal').modal('show');
                                    }
                                    function HideModal() {
                                        $('#logoutModal').modal('hide');
                                        window.location.href = "Login.aspx";
                                    }
                                    function refreshCaptcha() {
                                        var img = document.getElementById('<%=captcha1.ClientID %>');
                                        img.src = img.src.split('?')[0] + '?' + new Date().getTime(); // Append a timestamp to force reload
                                    }
                                </script>
                                <script type="text/javascript">
                                    function Confirm1() {
                                        $('#mt1').modal('show');
                                    }
                                    function HideModal1() {
                                        $('#mt1').modal('hide');
                                    }
                                </script>
                                <!-- Logout Modal-->
                                <div class="modal fade" id="logoutModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                                    <div class="modal-dialog" role="document">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <h5 class="modal-title" id="exampleModalLabel">Login Alert?</h5>
                                                <button class="close" type="button" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span> </button>
                                            </div>
                                            <div class="modal-body">A session for this User is already found as logged in. Do you want to terminate the existing session and continue with login ?</div>
                                            <div class="modal-footer">
                                                <button class="btn btn-primary text-facebook" type="button" data-dismiss="modal" onclick="HideModal()">Cancel</button>
                                                <asp:LinkButton ID="btnLogout" runat="server" class="btn btn-primary text-danger" OnClick="btnLogout_Click">Ok</asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="modal fade" id="mt1" tabindex="-1" role="dialog" aria-labelledby="exmt1" aria-hidden="true">
                                    <div class="modal-dialog" role="document">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <asp:Label runat="server" ID="lblHeader" CssClass="h5"></asp:Label>
                                                <button class="close" type="button" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span> </button>
                                            </div>
                                            <div class="modal-body">
                                                <asp:Label ID="lblAlert" runat="server"></asp:Label>
                                            </div>
                                            <div class="modal-footer">
                                                <div class="card-footer">
                                                    <asp:LinkButton runat="server" ID="btnYes" Text="Yes" CssClass="btn btn-Success" OnClick="btnYes_Click" />
                                                    <asp:LinkButton runat="server" ID="btnNo" Text="No" CssClass="btn btn-danger" OnClick="btnNo_Click" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </form>
                            <div class="d-block d-sm-flex justify-content-center align-items-center mt-4">
                                <span class="font-weight-normal">
                                    <a href="privacy-policy.html" class="font-weight-bold">Privacy Policy</a>
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</body>
</html>
