<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignUp1.aspx.cs" Inherits="eMIMPanel.SignUp1" %>

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
    <link
      href="https://fonts.googleapis.com/css?family=Nunito:200,200i,300,300i,400,400i,600,600i,700,700i,800,800i,900,900i"
      rel="stylesheet">

    <!-- Custom styles for this template-->
    <!--  <link href="css/sb-admin-2.min.css" rel="stylesheet">-->

    <!-- <link rel="stylesheet" href="css/neumorphism.css"> -->
    <link rel="stylesheet" href="./assets/icon/logo/style.css">
    <style>
      .form-control {
        display: block;
        width: 100%;
        height: calc(1.1rem + 1.2rem + 0.0625rem);
        padding: 0.6rem 0rem;
        background-color: white;
        font-size: 0.8;
        font-weight: 300;
        line-height: 1.5;
        color: gray;
        border: none;
        background-clip: padding-box;
        border-bottom: 0.0625rem solid #D1D9E6;
        transition: all 0.3s ease-in-out;
        margin-top: -0.8rem;
      }

      .form-control:focus {
        color: #44476A;
        background-color: white;
        border-color: #D1D9E6;
        outline: 0;
        box-shadow: inset 2px 2px 5px #b8b9be, inset -3px -3px 7px #FFFFFF, none;
      }

      label {
        display: inline-block;
        margin-bottom: -0.1rem;
        color: gray;
      }

      .btn-success:not(:disabled):not(.disabled):active,
      .btn-success:not(:disabled):not(.disabled).active,
      .show>.btn-success.dropdown-toggle {
        color: #18634b;
        background-color: white;
        border-color: #18634b;
      }

      .btn {
        position: relative;
        transition: all 0.2s ease;
        letter-spacing: 0.025em;
        font-size: 1rem;
        border-color: #18634b;
        box-shadow: none;
      }

      .btn-success:hover {
        background-color: white;
        color: #18634b;
        transition: all 0.5s linear;
      }

      input,
      button,
      select,
      optgroup,
      textarea {
        margin: 0;
        font-family: inherit;
        font-size: 0.8rem;
        line-height: inherit;

      }

      input[type=text]:focus {
        background-color: white;
      }

      .form-control:disabled,
      .form-control[readonly] {
        background-color: white;
        opacity: .6;
      }

      /* Media Query */

      @media(max-width:700px) {
        .img-d {
          display: none;

        }
      }
    </style>
  </head>

  <body>

    <form id="form1" runat="server">
      <asp:ScriptManager runat="server" ID="sm1"></asp:ScriptManager>

      <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>

          <div class="container-fluid">
            <div class="row  ">
              <div class="col-md-7 img-d" style="background-color: whitesmoke;">
                <div class="row">
                  <div class="col-md-12 ">
                    <img src="./assets/icon/logo/Linkext-logo.png" alt=""
                      style="max-width: 25%; margin-top: 1.2rem; margin-left: 1.2rem;">
                  </div>
                  <div class="col-md-12- text-center ">
                    <img src="./assets/icon/logo/aa.png" alt="" style="    max-width: 75%;
                  margin-top: 1rem;    position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
                  
              ">
                  </div>
                </div>
              </div>
              <div class="col-md-5 px-lg-5 " style="background-color:white ;">
                <div class="row mt-5 px-5 ">
                  <div class="col-lg-12 mt-4">
                    <h1 class="h4 text-gray-900 " style="font-size: 1.8rem; font-weight: 500; ">Sign Up
                    </h1>
                    <div class="form-group mt-4">
                      <label for="exampleInputPassword1">Name</label>
                      <asp:TextBox ID="txtName" autocomplete="off" runat="server" class="form-control" placeholder="">
                      </asp:TextBox>
                      <%--<input type="text" class="form-control" id="exampleInputPassword1" placeholder="  ">--%>
                    </div>
                    <div class="form-group ">

                      <asp:DropDownList runat="server" ID="ddlCountryCode" CssClass="form-control">
                      </asp:DropDownList>

                    </div>
                    <div class="form-group ">
                      <label for="exampleInputPassword1">Mobile</label>
                      <asp:TextBox ID="txtMobile" autocomplete="off" onkeypress="return onlyNumberKey(event)"
                        MaxLength="14" runat="server" class="form-control" aria-describedby="emailHelp" placeholder="">
                      </asp:TextBox>
                    </div>
                    <div class="form-group ">
                      <label for="exampleInputPassword1">E-mail</label>
                      <asp:TextBox ID="txtEmailId" autocomplete="off" runat="server" class="form-control"
                        aria-describedby="emailHelp" placeholder=""></asp:TextBox>
                    </div>
                    <div class="form-group ">
                      <label for="exampleInputPassword1">Company Name</label>
                      <asp:TextBox ID="txtCompany" autocomplete="off" runat="server" class="form-control"
                        aria-describedby="emailHelp" placeholder=""></asp:TextBox>
                    </div>
                    <div class="form-group mb-4">
                      <label for="exampleInputPassword1">Designation</label>
                      <asp:TextBox ID="txtDesignation" autocomplete="off" runat="server" class="form-control"
                        aria-describedby="emailHelp" placeholder=""></asp:TextBox>
                    </div>
                    <div class="form-group text-center  ">
                      <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click"
                        class="btn btn-success btn-sm  px-4" style="border-radius: 2rem; " Text="Sign up" />

                      <button type="button" id="btnPoP" class="btn btn-primary float-end"
                        style="float:right;display:none;" data-toggle="modal" data-target="#exampleModal">
                        Button
                      </button>
                      <button type="button" id="btnPoP1" class="btn btn-primary float-end"
                        style="float:right;display:none;" data-toggle="modal" data-target="#exampleModal1">
                        Button
                      </button>
                      <%-- <button type="button" class="btn btn-success btn-sm  px-4" style="border-radius: 2rem; "
                        data-target="#exampleModal" data-toggle="modal">Sign
                        up</button>--%>


                        <!-- Modal -->
                        <div class="modal fade" id="exampleModal" tabindex="-1" role="dialog"
                          aria-labelledby="exampleModalLabel" aria-hidden="true">
                          <div class="modal-dialog" role="document">
                            <div class="modal-content">
                              <div class="modal-header">
                                <h5 class="modal-title" id="exampleModalLabel"> <span id="sptitle" runat="server">
                                    Warning</span> </h5>
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                  <span aria-hidden="true">&times;</span>
                                </button>
                              </div>
                              <div class="modal-body">

                                <span id="msg" runat="server">
                                  THANK YOU
                                </span>

                              </div>
                              <div class="modal-footer">
                                <button type="button" class="btn btn-danger btn-sm  "
                                  data-dismiss="modal">Close</button>
                              </div>
                            </div>
                          </div>
                        </div>
                        <!-- Modal -->
                        <div class="modal fade" id="exampleModal1" tabindex="-1" role="dialog"
                          aria-labelledby="exampleModalLabel" aria-hidden="true">
                          <div class="modal-dialog" role="document">
                            <div class="modal-content">
                              <div class="modal-header">
                                <h5 class="modal-title">
                                  Success </h5>
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                  <span aria-hidden="true">&times;</span>
                                </button>
                              </div>
                              <div class="modal-body">
                                <img src="assets/thankYou.jpg">
                              </div>
                              <div class="modal-footer">
                                <button type="button" class="btn btn-danger btn-sm  "
                                  data-dismiss="modal">Close</button>
                              </div>
                            </div>
                          </div>
                        </div>


                    </div>
                  </div>
                  <div class="col-md-12 text-center mt-2">
                    <p style="font-size: 0.75rem; ">By clicking this button, you agree to My Inbox Media's <a
                        href="https://www.myinboxmedia.com/terms-of-service.php" style="color: rgb(79, 79, 251);">Terms
                        of Services </a></p>
                  </div>
                  <div class="col-md-12 text-center mt-1 lead">
                    <p style="font-size: 0.8rem; margin-top: -1rem;"><a href="https://linkext.io/Login.aspx">Already
                        have an account?</a></p>
                  </div>

                </div>
              </div>
            </div>
          </div>
        </ContentTemplate>

      </asp:UpdatePanel>
    </form>




  </body>
  <script src="vendor/jquery/jquery.min.js"></script>
  <script src="vendor/bootstrap/js/bootstrap.bundle.min.js"></script>

  <!-- Core plugin JavaScript-->
  <script src="vendor/jquery-easing/jquery.easing.min.js"></script>

  <!-- Custom scripts for all pages-->
  <script src="js/sb-admin-2.min.js"></script>
  <script>
    function onlyNumberKey(evt) {
      // Only ASCII character in that range allowed
      var ASCIICode = (evt.which) ? evt.which : evt.keyCode
      if (ASCIICode > 31 && (ASCIICode < 48 || ASCIICode > 57))
        return false;
      return true;
    }
    function showpopmsg() {
      $("#btnPoP").click();
    }
    function showpopmsg1() {
      $("#btnPoP1").click();
    }
  </script>

  </html>