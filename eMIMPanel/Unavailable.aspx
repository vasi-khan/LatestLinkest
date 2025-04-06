<%@ Page Title="" Language="C#" MasterPageFile="~/Site3.Master" AutoEventWireup="true" CodeBehind="Unavailable.aspx.cs" Inherits="eMIMPanel.Unavailable" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<!-- Primary Meta Tags -->
<title>MIM Admin - Coming Soon</title>
<meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
<meta name="title" content="MIM Admin Sign up">
<meta name="author" content="Yusuf">

<link rel="canonical" href="" />

<!--  Social tags -->
<meta name="keywords" content="MIM Dashboard">

<!-- Schema.org markup for Google+ -->
<meta itemprop="name" content="#">
<meta itemprop="description" content="#">
<meta itemprop="image" content="#">


<!-- Favicon -->
<link rel="apple-touch-icon" sizes="57x57" href="img/favicon/apple-icon-57x57.png">
<link rel="apple-touch-icon" sizes="60x60" href="img/favicon/apple-icon-60x60.png">
<link rel="apple-touch-icon" sizes="72x72" href="img/favicon/apple-icon-72x72.png">
<link rel="apple-touch-icon" sizes="76x76" href="img/favicon/apple-icon-76x76.png">
<link rel="apple-touch-icon" sizes="114x114" href="img/favicon/apple-icon-114x114.png">
<link rel="apple-touch-icon" sizes="120x120" href="img/favicon/apple-icon-120x120.png">
<link rel="apple-touch-icon" sizes="144x144" href="img/favicon/apple-icon-144x144.png">
<link rel="apple-touch-icon" sizes="152x152" href="img/favicon/apple-icon-152x152.png">
<link rel="apple-touch-icon" sizes="180x180" href="img/favicon/apple-icon-180x180.png">
<link rel="icon" type="image/png" sizes="192x192"  href="img/favicon/android-icon-192x192.png">
<link rel="icon" type="image/png" sizes="32x32" href="img/favicon/favicon-32x32.png">
<link rel="icon" type="image/png" sizes="96x96" href="img/favicon/favicon-96x96.png">
<link rel="icon" type="image/png" sizes="16x16" href="img/favicon/favicon-16x16.png">
<link rel="manifest" href="img/favicon/manifest.json">
<meta name="msapplication-TileColor" content="#ffffff">
<meta name="msapplication-TileImage" content="img/favicon/ms-icon-144x144.png">
<meta name="theme-color" content="#29648a">

<!-- Fontawesome -->
  <link rel="stylesheet" href="vendor/fontawesome-free/css/all.min.css">
         
  <link href="https://fonts.googleapis.com/css2?family=Playball&display=swap" rel="stylesheet">

<!-- Pixel CSS -->
<link type="text/css" href="css/neumorphism.css" rel="stylesheet" />

    
<style>
.thank-content h2 {
    font-family: 'Playball', cursive;
    font-size: 6em;
    color: #27648a;
}
.thank-content h2 {
    position: relative;
    color: rgba(0, 0, 0, .3);
    font-size: 4em
}
.thank-content h2:before {
    content: attr(data-text);
    position: absolute;
    overflow: hidden;
    max-width: 7em;
    white-space: nowrap;
    color:#27648a;
    animation: loading 4s linear;
}
@keyframes loading {
    0% {
        max-width: 0;
    }
}

/* Success Check  */

.check_mark { /* Check background */
  width: 88px;
  height: 88px;
	border-radius: 50%;
  margin: 0 auto;
	background-color: #00DACA;
}

.sa-icon {
  width: 80px;
  height: 80px;
	border: 4px solid #00DACA;
  -webkit-border-radius: 40px;
  border-radius: 40px;
  border-radius: 50%;
  /* margin: 20px auto; */
  padding: 0;
  position: relative;
  box-sizing: content-box;
	overflow: hidden;
}

.sa-icon.sa-success {
  border-color: #00DACA;
}

.sa-icon.sa-success::before,
.sa-icon.sa-success::after {
  content: '';
  -webkit-border-radius: 40px;
  border-radius: 40px;
  border-radius: 50%;
  position: absolute;
  width: 60px;
  height: 120px;
  background: #00DACA;
  -webkit-transform: rotate(45deg);
  transform: rotate(45deg);
}

.sa-icon.sa-success::before {
  -webkit-border-radius: 120px 0 0 120px;
  border-radius: 120px 0 0 120px;
  top: -7px;
  left: -33px;
  -webkit-transform: rotate(-45deg);
  transform: rotate(-45deg);
  -webkit-transform-origin: 60px 60px;
  transform-origin: 60px 60px;
}

.sa-icon.sa-success::after {
  -webkit-border-radius: 0 120px 120px 0;
  border-radius: 0 120px 120px 0;
  top: -11px;
  left: 30px;
  -webkit-transform: rotate(-45deg);
  transform: rotate(-45deg);
  -webkit-transform-origin: 0px 60px;
  transform-origin: 0px 60px;
}

.sa-icon.sa-success .sa-placeholder {
  width: 80px;
  height: 80px;
  // border: 4px solid rgba(76, 175, 80, .5);
	// border: 4px solid #00DACA;
	// border: 4px solid rgba(255, 255, 255, .7);
  -webkit-border-radius: 40px;
  border-radius: 40px;
  border-radius: 50%;
  box-sizing: content-box;
  position: absolute;
  left: -4px;
  top: -4px;
  z-index: 2;
	/*********************************************/
}

.sa-icon.sa-success .sa-fix {
  width: 5px;
  height: 90px;
  background-color: #00DACA;
  position: absolute;
  left: 28px;
  top: 8px;
  z-index: 1;
  -webkit-transform: rotate(-45deg);
  transform: rotate(-45deg);
}

.sa-icon.sa-success.animate::after {
  -webkit-animation: rotatePlaceholder 4.25s ease-in;
  animation: rotatePlaceholder 4.25s ease-in;
}

.sa-icon.sa-success {
  border-color: transparent\9;
}
.sa-icon.sa-success .sa-line.sa-tip {
  -ms-transform: rotate(45deg) \9;
}
.sa-icon.sa-success .sa-line.sa-long {
  -ms-transform: rotate(-45deg) \9;
}

.animateSuccessTip {
  -webkit-animation: animateSuccessTip 0.75s;
  animation: animateSuccessTip 0.75s;
}

.animateSuccessLong {
  -webkit-animation: animateSuccessLong 0.75s;
  animation: animateSuccessLong 0.75s;
}

@-webkit-keyframes animateSuccessLong {
  0% {
    width: 0;
    right: 46px;
    top: 54px;
  }
  65% {
    width: 0;
    right: 46px;
    top: 54px;
  }
  84% {
    width: 55px;
    right: 0px;
    top: 35px;
  }
  100% {
    width: 47px;
    right: 8px;
    top: 38px;
  }
}
@-webkit-keyframes animateSuccessTip {
  0% {
    width: 0;
    left: 1px;
    top: 19px;
  }
  54% {
    width: 0;
    left: 1px;
    top: 19px;
  }
  70% {
    width: 50px;
    left: -8px;
    top: 37px;
  }
  84% {
    width: 17px;
    left: 21px;
    top: 48px;
  }
  100% {
    width: 25px;
    left: 14px;
    top: 45px;
  }
}
@keyframes animateSuccessTip {
  0% {
    width: 0;
    left: 1px;
    top: 19px;
  }
  54% {
    width: 0;
    left: 1px;
    top: 19px;
  }
  70% {
    width: 50px;
    left: -8px;
    top: 37px;
  }
  84% {
    width: 17px;
    left: 21px;
    top: 48px;
  }
  100% {
    width: 25px;
    left: 14px;
    top: 45px;
  }
}

@keyframes animateSuccessLong {
  0% {
    width: 0;
    right: 46px;
    top: 54px;
  }
  65% {
    width: 0;
    right: 46px;
    top: 54px;
  }
  84% {
    width: 55px;
    right: 0px;
    top: 35px;
  }
  100% {
    width: 47px;
    right: 8px;
    top: 38px;
  }
}

.sa-icon.sa-success .sa-line {
  height: 5px;
  background-color: white;
  display: block;
  border-radius: 2px;
  position: absolute;
  z-index: 2;
	/******************/
	/******************/
}

.sa-icon.sa-success .sa-line.sa-tip {
  width: 25px;
  left: 14px;
  top: 46px;
  -webkit-transform: rotate(45deg);
  transform: rotate(45deg);
}

.sa-icon.sa-success .sa-line.sa-long {
  width: 47px;
  right: 8px;
  top: 38px;
  -webkit-transform: rotate(-45deg);
  transform: rotate(-45deg);
}

@-webkit-keyframes rotatePlaceholder {
  0% {
    transform: rotate(-45deg);
    -webkit-transform: rotate(-45deg);
  }
  5% {
    transform: rotate(-45deg);
    -webkit-transform: rotate(-45deg);
  }
  12% {
    transform: rotate(-405deg);
    -webkit-transform: rotate(-405deg);
  }
  100% {
    transform: rotate(-405deg);
    -webkit-transform: rotate(-405deg);
  }
}
@keyframes rotatePlaceholder {
  0% {
    transform: rotate(-45deg);
    -webkit-transform: rotate(-45deg);
  }
  5% {
    transform: rotate(-45deg);
    -webkit-transform: rotate(-45deg);
  }
  12% {
    transform: rotate(-405deg);
    -webkit-transform: rotate(-405deg);
  }
  100% {
    transform: rotate(-405deg);
    -webkit-transform: rotate(-405deg);
  }
}


</style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <main>

      <div class="container-fluid">
        <nav aria-label="breadcrumb" class="my-3">
            <ol class="breadcrumb breadcrumb-info">
                <li class="breadcrumb-item"><a href="#">Home</a></li>
                <li class="breadcrumb-item active" aria-current="page">Coming Soon</li>
            </ol>
        </nav>
        <!-- Content Row -->
          <div class="row">
            <div class="col-lg-12 col-xl-12"> 
              <!-- Basic Card Example -->
              <div class="card bg-primary border-light shadow-soft mb-4">
                  <div class="card-header py-3 bg-primary d-flex justify-content-between flex-wrap align-content-center">
                  <h6 class="m-0 font-weight-bold font-weight-bold my-auto"><i class="fas fa-hourglass-half"></i> Coming Soon</h6>
                </div>
                <div class="card-body">
                  <div class="d-flex flex-column justify-content-center align-items-center">
                     <div class="thank-content">
                         <h2 class="h1" data-text="Coming Soon">Coming Soon</h2> 
                         <p class="text-center">We are working on awesome site</p>
                      </div>
                   </div>
                </div>
              </div>
            </div>
          </div>           
      </div>
    </main>

<!-- Bootstrap core JavaScript--> 
<script src="vendor/jquery/jquery-3.5.1.min.js"></script> 
<script src="vendor/bootstrap/js/bootstrap.bundle.min.js"></script> 


</asp:Content>
