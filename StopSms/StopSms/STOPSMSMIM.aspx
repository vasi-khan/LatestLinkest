<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="STOPSMSMIM.aspx.cs" Inherits="StopSms.STOPSMSMIM" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
        <!-- Required meta tags -->
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <!-- Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet"
        integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous" />
    <style>
        .container {
            width: 40%;
        }

        .hidden {
            display: none;
        }

        @media(max-width:600px) {
            .container {
                width: 100%;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="container mt-5   ">
            <div class="card  shadow border-0  ">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div class="card-body ">
                            <div class="row gy-3 px-lg-5 px-sm-1">
                                <div class="col-md-12">

                                    <h5 class="card-title text-center">STOP RCM COMMUNICATION</h5>
                                </div>
                                <div class="col-md-4 col-6 ">
                                    <label>Mobile Number </label>
                                </div>
                                <div class="col-md-8 col-6">
                                    <asp:TextBox ID="txtMobile"  runat="server" onkeypress="return OnlyNumbers(event)"></asp:TextBox>
                                </div>
                                <div class="col-md-12 text-center">
                                    <%--<button class="btn btn-sm btn-warning px-3 text-uppercase">Generate OTP</button>--%>
                                    <asp:Button runat="server" Text="Generate OTP" OnClientClick="enablebutton();" class="btn btn-sm btn-warning px-3 text-uppercase" ID="btnGo" OnClick="btnGo_Click" />
                                </div>
                                <div class="col-md-4 col-6 ">
                                    <label>Enter OTP : </label>
                                </div>
                                <div class="col-md-6 col-6">
                                    <%--<input type="text" class="form-control form-control-sm" id="">--%>
                                    <asp:TextBox ID="txtotp" runat="server" class="form-control form-control-sm"></asp:TextBox>
                                </div>

                                <div class="col-md-12 text-center">
                                    <%--<button class="btn btn-sm btn-success px-3 text-uppercase">Verify</button>--%>
                                    <asp:Button runat="server" Text="Verify OTP" class="btn btn-sm btn-success px-3 text-uppercase" ID="btnSubmitOTP" OnClick="btnSubmitOTP_Click"></asp:Button>

                                    <%--<button class="btn btn-sm btn-danger px-3 text-uppercase">Resend OTP</button>--%>
                                    <asp:Button ID="btnResendSMS" runat="server" Text="Resend OTP" disabled="disabled" />
                                </div>

                                <div class="col-md-12 text-center">
                                    <%--<button class="btn btn-sm btn-outline-success px-3 text-uppercase" onclick="showDiv2()">unsubscribe from sms</button>--%>
                                    <asp:Button ID="btnUnSubscribe" Visible="false" class="btn btn-sm btn-outline-success px-3 text-uppercase"
                                        OnClientClick="showDiv2();" runat="server" Text="unsubscribe from RCM commumnication" OnClick="btnUnSubscribe_Click" />

                                </div>

                            </div>

                            <div class="row text-center mt-5 mb-5" id="box2" runat="server" style="display: none">
                                <div class="col-md-12">
                                    <h5>Thank You. You have been successfully unsubscribed from MyInboxMedia Google RCM Communication</h5>
                                    <br />
                                    <h6>To subscribe againg click <a href="https://myinboxmedia.com/account-creation.php">here</a></h6> 
                                    <%--<p>You have been successfully receiving further SMS</p>--%>
                                </div>
                            </div>


                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnResendSMS" />
                    </Triggers>
                </asp:UpdatePanel>

            </div>
        </div>

        <script
            src="https://code.jquery.com/jquery-3.6.4.min.js"
            integrity="sha256-oP6HI9z1XaZNBrJURtCoUT5SUnxFr8s3BzRl+cbzUq8="
            crossorigin="anonymous"></script>
        <script>
            //function showDiv2() {
            //    document.getElementById('box2').classList.remove('hidden');
            //}


            function enablebutton() {
                if ($('#txtMobile').val() != "") {
                    var fewSeconds = 10;
                    setTimeout(function () {
                        $('#btnResendSMS').removeAttr('disabled');
                        //$('#btnResendSMS').attr('style', 'background-color: white; color:#0c5b5b; border: 1px solid #0c5b5b');  // js
                    }, fewSeconds * 1000);
                }
            }
        </script>
        <!-- Optional JavaScript; choose one of the two! -->

        <!-- Option 1: Bootstrap Bundle with Popper -->
        
        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.bundle.min.js"
            integrity="sha384-MrcW6ZMFYlzcLA8Nl+NtUVF0sA7MsXsP1UyJoMp4YLEuNSfAP+JcXn/tWtIaxVXM"
            crossorigin="anonymous"></script>
        <script type="text/javascript">  
            function OnlyNumbers(evt) {
                var charCode = (evt.which) ? evt.which : event.keyCode
                if (charCode > 31 && ((charCode >= 48 && charCode <= 57) || charCode == 46)) {

                }
                else {
                    alert('Please Enter Numeric values.');
                    return false;
                }
                var phn = document.getElementById("txtMobile").value;
                var strlength = phn.length;
                var str = phn.substring(0, 1);
                if (str == 0) {
                    if (strlength < 10) {
                        return true;
                    }
                    else {
                        return false;
                    }
                }
                else {
                    if (strlength < 10) {
                        return true;
                    }
                    else {
                        return false;
                    }
                }
            }
            </script>

        <!-- Option 2: Separate Popper and Bootstrap JS -->
        <!--
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.9.2/dist/umd/popper.min.js" integrity="sha384-IQsoLXl5PILFhosVNubq5LC7Qb9DXgDA9i+tQ8Zj3iwWAwPtgFTxbJ8NT4GN1R8p" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.min.js" integrity="sha384-cVKIPhGWiC2Al4u+LWgxfKTRIcfu0JTxR+EQDz/bgldoEyl4H0zUF0QKbrJ0EcQF" crossorigin="anonymous"></script>
    -->
    </form>
</body>
</html>
