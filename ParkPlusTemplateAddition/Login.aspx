<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ParkPlusTemplateAddition.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="assets/css/bootstrap5min.css" rel="stylesheet" />
    <script src="assets/js/bootstrap.min.js"></script>
    <script src="assets/js/jquery_slim_min.js"></script>
    <script src="assets/js/popper.min.js"></script>
</head>
<style>
    .container-fluid {
        width: 40%;
    }

    @media(max-width:600px) {
        .container-fluid {
            width: 80%;
        }
    }
</style>
<body>
    <form id="form1" runat="server">
        <div>
            <div class="container-fluid p-2 mt-5">
                <center>
                <img src="assets/icon/mim2.png" width="47%" />
                    </center>
                <div class="card border rounded ">
                    <div class="card-header bg-transparent border-0">
                        <h5 class="card-tittle text-center mt-3 font-weight-bold">LOGIN </h5>
                    </div>
                    <div class="card-body pt-0">
                        <div class="row p-3">
                            <div class="col-md-12">
                                <div class=" mb-3">
                                    <div class="d-flex">
                                        <%--<asp:Label ID="lblUserID" runat="server">USERNAME</asp:Label>
                                        &nbsp;&nbsp;&nbsp;&nbsp;--%>
                                        <asp:TextBox ID="txtuserid" runat="server" CssClass="form-control" MaxLength="20" Placeholder="USERNAME"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-12">
                                <div class=" mb-3">
                                    <div class="d-flex">
                                        <%--<asp:Label ID="lblPwd" runat="server">PASSWORD</asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;--%>
                                        <asp:TextBox type="password" ID="txtPwd" runat="server" CssClass="password-input form-control" MaxLength="25" Placeholder="PASSWORD"></asp:TextBox>
                                        <span class="eye-icon px-2 mt-1" onclick="togglePasswordVisibility()" style="margin-left: -2rem; color: gray;">
                                            <img id="eyeImg" src="assets/icon/hidden.png" style="height: 16px; width: 16px;" />
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-12 ">
                                <div class=" mb-3 ">
                                    <asp:Button ID="btnSubmit" runat="server" Text="SUBMIT" class="btn btn-primary btn-block" OnClick="btnSubmit_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script type="text/javascript">
        function togglePasswordVisibility() {
            var passwordInput = document.querySelector('.password-input');
            var eyeImg = document.getElementById('eyeImg');

            if (passwordInput.type === 'password') {
                passwordInput.type = 'text';
                eyeImg.src = 'assets/icon/show.png';
                eyeImg.alt = 'Hide Password';
            } else {
                passwordInput.type = 'password';
                eyeImg.src = 'assets/icon/hidden.png';
                eyeImg.alt = 'Show Password';
            }
        }
    </script>
</body>
</html>

