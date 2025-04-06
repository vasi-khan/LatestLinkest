<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LogniC.aspx.cs" Inherits="eMIMPanel.LogniC" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>

  <meta charset="utf-8">
  <meta http-equiv="X-UA-Compatible" content="IE=edge">
  <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
  <meta name="description" content="">
  <meta name="author" content="">

  <title>MIM Admin - Login</title>

  <!-- Custom fonts for this template-->
  <link href="vendor/fontawesome-free/css/all.min.css" rel="stylesheet" type="text/css">
  <link href="https://fonts.googleapis.com/css?family=Nunito:200,200i,300,300i,400,400i,600,600i,700,700i,800,800i,900,900i" rel="stylesheet">

  <!-- Custom styles for this template-->
<!--  <link href="css/sb-admin-2.min.css" rel="stylesheet">-->
    
    <link rel="stylesheet" href="css/neumorphism.css">

</head>

<body class="bg-gradient-primary">
    <form id="form1" runat="server">
       <div class="container">

    <!-- Outer Row -->
    <div class="row justify-content-center">

      <div class="col-xl-6 col-lg-6 col-md-6">

        <div class="card bg-primary border-light shadow-soft o-hidden my-5">
          <div class="card-body p-0">
            <!-- Nested Row within Card Body -->
            <div class="row">
              <div class="col-lg-12">
                <div class="p-5">
                  <div class="text-center">
                    <h1 class="h4 text-gray-900 mb-4">Welcome Back!</h1>
                  </div>
                  <div class="user">
                    <div class="form-group">
                        <asp:TextBox ID="txtUserID" runat="server" class="form-control form-control-user"  aria-describedby="emailHelp" placeholder="Enter Email Address..."></asp:TextBox>
                      <%--<input type="email" class="form-control form-control-user" id="exampleInputEmail1" aria-describedby="emailHelp" placeholder="Enter Email Address...">--%>
                    </div>
                    <div class="form-group">
                        <asp:TextBox ID="txtPassword" runat="server" class="form-control form-control-user" textmode="Password" placeholder="Password"></asp:TextBox>
                      <%--<input type="password" class="form-control form-control-user" id="exampleInputPassword1" placeholder="Password">--%>
                    </div>
                    <%--<div class="form-group">
                      <div class="custom-control custom-checkbox small">
                        <input type="checkbox" class="custom-control-input" id="customCheck">
                        <label class="custom-control-label" for="customCheck">Remember Me</label>
                      </div>
                    </div>--%>
                      <asp:Button ID="btnLogin" runat="server" class="btn btn-primary btn-user btn-block" Text="Login" OnClick="btnLogin_Click" />
                    <%--<a href="index.html" class="btn btn-primary btn-user btn-block">
                      Login
                    </a>--%>
                  </div>
                  <hr>
                  <div class="text-center">
                    <a class="small" href="forgot-password.aspx">Forgot Password?</a>
                  </div>
                  <div class="text-center">
                    <a class="small" href="sign-up.aspx">Create an Account!</a>
                      <asp:Label ID="lblUrl" runat="server"></asp:Label>
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
