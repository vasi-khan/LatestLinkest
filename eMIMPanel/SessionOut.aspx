<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SessionOut.aspx.cs" Inherits="eMIMPanel.SessionOut" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
      <meta charset="utf-8">
  <meta http-equiv="X-UA-Compatible" content="IE=edge">
  <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
  <meta name="description" content="">
  <meta name="author" content="">

  <title>Linkext-Login</title>

  <meta property="og:title" content="Linkext" />
  <meta property="og:type" content="SMS Panel" />
  <meta property="og:url" content="https://linkext.io" />
  <meta property="og:image" content="img/Linkext-logo.png" >
  <meta property="og:description" content="Application-to-Person bulk SMS tool has a dynamic Link Text feature capable of carrying voluminous data – text, videos, images, graphics, infographics, presentations, excel, tutorials, and more.">

  <!-- Custom fonts for this template-->
  <link href="vendor/fontawesome-free/css/all.min.css" rel="stylesheet" type="text/css">
  <link href="https://fonts.googleapis.com/css?family=Nunito:200,200i,300,300i,400,400i,600,600i,700,700i,800,800i,900,900i" rel="stylesheet">


    
    <link rel="stylesheet" href="css/neumorphism.css">
    
      <%-- <script>
         setTimeout(function(){
            //window.location.href = 'https://www.linkext.com/javascript/';
             window.location.assign('login.aspx');;
         }, 1000);
      </script>--%>
</head>
<body>
    <form id="form1" runat="server">
 
         <section class="min-vh-100 d-flex bg-primary align-items-center">
    <div class="container">
        <div class="row justify-content-center">
            <div class="col-12 col-md-8 col-lg-6 justify-content-center">
                <div class="card bg-primary shadow-soft border-light p-4">
                    
                    <div class="card-body">
                       <div class="d-block d-sm-flex justify-content-center align-items-center mt-4">
                            <div class="text-danger">
                                <span> Your session is timed out , please login again .</span>
                            </div>

                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>
</section>
      

   
    </form>

      <!-- Bootstrap core JavaScript-->
  <script src="vendor/jquery/jquery.min.js"></script>
  <script src="vendor/bootstrap/js/bootstrap.bundle.min.js"></script>

  <!-- Core plugin JavaScript-->
  <script src="vendor/jquery-easing/jquery.easing.min.js"></script>

  <!-- Custom scripts for all pages-->
  <script src="js/sb-admin-2.min.js"></script>
</body>
    
</html>
