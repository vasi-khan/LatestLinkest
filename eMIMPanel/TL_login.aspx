<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TL_login.aspx.cs" Inherits="eMIMPanel.TL_login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="vendor/fontawesome-free/css/all.css" rel="stylesheet" />
    <link href="vendor/fontawesome-free/css/all.min.css" rel="stylesheet" />
    <link href="vendor/fontawesome-free/css/googlepise.css" rel="stylesheet" />
    <link href="css/sb-admin-2.min.css" rel="stylesheet" />
</head>
<body class="bg-gradient-primary">
    <form id="form1" runat="server">
        
        <div>
            <div style="text-align: center; margin-top: 20px;">
                <%--<img src="img/logo.png" alt="mim logo" width="200px" />--%>
            </div>
            <div class="container">
                
            <div class="d-flex justify-content-end">
                 <div class="form-floating my-2">
                <asp:DropDownList ID="ddLang" runat="server" CssClass="form-select-sm" AutoPostBack="true" OnSelectedIndexChanged="ddLang_SelectedIndexChanged">  
                    <asp:ListItem Value="en-US" Text="English" />  
                    <asp:ListItem Value="ar-EG" Text="Arabic" />  
                </asp:DropDownList> 
<%--                  <label for="ddLang" id="lbllang" runat="server">Language</label>--%>
             </div>
            </div>

                
                <!-- Outer Row -->
                <div class="row justify-content-center">
                    <div class="col-xl-6 col-lg-6 col-md-6">
                        <div class="card o-hidden border-0 shadow-lg my-5">
                            <div class="card-body p-0">
                                <!-- Nested Row within Card Body -->
                                <div class="row">
                                    <!-- <div class="col-lg-6 d-none d-lg-block bg-login-image"></div>-->
                                    <div class="col-lg-12">
                                        <div class="p-5">
                                            <div class="text-center">
                                                <h1 class="h4 text-gray-900 mb-4" id="lblheading" runat="server"> Request Form Sign In</h1>
                                            </div>

                                            <div class="form-group">
                                                <asp:TextBox class="form-control form-control-user" ID="txtEmail" runat="server"
                                                    placeholder="Enter User ID..."></asp:TextBox>
                                            </div>
                                            <div class="form-group">
                                                <asp:TextBox TextMode="Password" class="form-control form-control-user" ID="txtPassword" runat="server"
                                                    placeholder="Password"></asp:TextBox>
                                            </div>
                                            <%--<div class="form-group">
                                            <div class="custom-control custom-checkbox small">
                                                <input type="checkbox" class="custom-control-input" id="customCheck">
                                                <label class="custom-control-label" for="customCheck">
                                                    Remember Me</label>
                                            </div>
                                        </div>--%>
                                            <asp:Button ID="btnLogin" runat="server" Text="Login" class="btn btn-primary btn-user btn-block"  OnClick="btnLogin_Click" />
                                            <hr>
                                            <%--<div class="form-group">
                                                <asp:TextBox class="form-control form-control-user" ID="txtOTP" runat="server" Visible="false" MaxLength="6"
                                                    placeholder="Enter Receive OTP..."></asp:TextBox>

                                            </div>
                                            <asp:Button ID="btnSubmit1" Visible="false" runat="server" Text="Submit" class="btn btn-primary btn-user btn-block" OnClick="btnSubmit1_Click" />
                                            <hr>--%>
                                            <%--<div class="text-center">
                                            <a class="small" href="forgot-password.html">Forgot Password?</a>
                                        </div>
                                        <div class="text-center">
                                            <a class="small" href="register.html">Create an Account!</a>
                                        </div>--%>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- Bootstrap core JavaScript-->
        <script src="vendor/jquery/jquery.min.js"></script>
        <script src="vendor/bootstrap/js/bootstrap.bundle.min.js"></script>
        <!-- Core plugin JavaScript-->
        <script src="vendor/jquery-easing/jquery.easing.min.js"></script>
        <!-- Custom scripts for all pages-->
        <script src="js/sb-admin-2.min.js"></script>


    </form>
</body>
</html>
