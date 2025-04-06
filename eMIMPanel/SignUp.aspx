<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SignUp.aspx.cs" Inherits="eMIMPanel.SignUp" %>

  <!DOCTYPE html>

  <html xmlns="http://www.w3.org/1999/xhtml">

  <head runat="server">
    <title>Sign Up</title>
    <!-- Custom fonts for this template-->
    <link href="vendor/fontawesome-free/css/all.min.css" rel="stylesheet" type="text/css">
    <link
      href="https://fonts.googleapis.com/css?family=Nunito:200,200i,300,300i,400,400i,600,600i,700,700i,800,800i,900,900i"
      rel="stylesheet">

    <!-- Custom styles for this template-->
    <!--  <link href="css/sb-admin-2.min.css" rel="stylesheet">-->
    <link rel="stylesheet" href="css/neumorphism.css">
    <style>
      .mystyle {
        display: block;
        padding-right: 17px;
      }
    </style>
  </head>

  <body class="bg-gradient-primary">

    <form id="form1" runat="server">
      <asp:ScriptManager runat="server" ID="sm1"></asp:ScriptManager>

      <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate>
          <div class="container" style="max-width:1400px ;">

            <!-- Outer Row -->
            <div class="row justify-content-center">

              <div class="col-xl-6 col-lg-6 col-md-6">

                <div class="card bg-primary border-light shadow-soft o-hidden my-5">
                  <div class="card-body p-0">
                    <div class="row">
                      <div class="col-md-12 text-center ">
                        <img src="./assets/icon/logo/Linkext-logo.png" alt=""
                          style="max-width: 30%; margin-top:1.6rem; margin-left: 1.5rem;">
                      </div>
                    </div>
                    <!-- Nested Row within Card Body -->
                    <div class="row  mt-3 text-center" style="padding: 0 3.5rem;">


                      <div class="col-lg-12 mt-1">
                        <h1 class="h4 text-gray-900 mb-4" style="font-size: 1.7rem;">SIGN UP!</h1>
                      </div>
                      <div class="col-md-4 mt-2 text-left">
                        Name
                      </div>
                      <div class="col-md-8 mt-2">
                        <div class="form-group">
                          <asp:TextBox ID="txtName" runat="server" class="form-control form-control-user"
                            aria-describedby="emailHelp" placeholder=""></asp:TextBox>
                        </div>
                      </div>
                      <div class="col-md-4 mt-2 text-left">
                        Country
                      </div>
                      <div class="col-md-8 mt-2">
                        <div class="form-group">
                          <asp:DropDownList runat="server" ID="ddlCountryCode" CssClass="custom-select">
                          </asp:DropDownList>
                        </div>
                      </div>
                      <div class="col-md-4 text-left">
                        Mobile No.
                      </div>
                      <div class="col-md-8 ">
                        <div class="form-group">
                          <asp:TextBox ID="txtMobile" onkeypress="return onlyNumberKey(event)" MaxLength="14"
                            runat="server" class="form-control form-control-user" aria-describedby="emailHelp"
                            placeholder=""></asp:TextBox>
                        </div>
                      </div>
                      <div class="col-md-4 text-left">
                        E-mail
                      </div>
                      <div class="col-md-8 ">
                        <div class="form-group">
                          <asp:TextBox ID="txtEmailId" runat="server" class="form-control form-control-user"
                            aria-describedby="emailHelp" placeholder=""></asp:TextBox>

                        </div>
                      </div>
                      <div class="col-md-4 text-left">
                        Company Name
                      </div>
                      <div class="col-md-8 ">
                        <div class="form-group">
                          <asp:TextBox ID="txtCompany" runat="server" class="form-control form-control-user"
                            aria-describedby="emailHelp" placeholder=""></asp:TextBox>
                        </div>
                      </div>
                      <div class="col-md-4 text-left">
                        Designation
                      </div>
                      <div class="col-md-8">
                        <div class="form-group">
                          <asp:TextBox ID="txtDesignation" runat="server" class="form-control form-control-user"
                            aria-describedby="emailHelp" placeholder=""></asp:TextBox>
                        </div>
                      </div>
                      <div class="col-md-12 mt-3 mb-4">
                        <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" class="btn btn-primary"
                          style="padding:0.5rem 4.2rem; " Text="Submit" />
                        <!-- Button trigger modal -->
                        <button type="button" id="btnPoP" class="btn btn-primary float-end"
                          style="float:right;display:none;" data-toggle="modal" data-target="#exampleModal">
                          Button
                        </button>


                        <!-- Modal -->
                        <div class="modal fade" id="exampleModal" tabindex="-1" role="dialog"
                          aria-labelledby="exampleModalLabel" aria-hidden="true">
                          <div class="modal-dialog" role="document">
                            <div class="modal-content">
                              <div class="modal-header">
                                <h5 class="modal-title" id="exampleModalLabel"> <span id="sptitle" runat="server">
                                    Tittle</span> </h5>
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
                                <button type="button" class="btn btn-primary" data-dismiss="modal">Close</button>
                              </div>
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>
                    <!-- <div class="col-md-12 ">
                      <img src="./assets/icon/logo/MiM-Logo-v5.png" alt=""
                        style="max-width: 25%; float: right; margin-bottom: 1rem;">
                    </div> -->
                  </div>
                </div>
              </div>
            </div>

          </div>

        </ContentTemplate>

      </asp:UpdatePanel>

    </form>
    <!-- Bootstrap core JavaScript-->
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
        //var element = document.getElementById("exampleModal");
        //element.classList.add("mystyle");
        $("#btnPoP").click();
      }
    </script>
  </body>

  </html>