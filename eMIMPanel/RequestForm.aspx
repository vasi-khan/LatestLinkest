<%@ Page Language="C#" MasterPageFile="~/BDMaster.Master" AutoEventWireup="true" CodeBehind="RequestForm.aspx.cs" Inherits="eMIMPanel.RequestForm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <link rel="canonical" href="">


    <link href="OffLineCDN/bootstrap.min.css" rel="stylesheet" />
    <!-- Bootstrap core CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-EVSTQN3/azprG1Anm3QDgpJLIm9Nao0Yz1ztcQTwFspd3yD65VohhpuuCOmLASjC" crossorigin="anonymous">

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/16.0.8/css/intlTelInput.css" />
      

    <!-- Favicons -->
    <link rel="apple-touch-icon" href="assets/favicons/apple-touch-icon.png" sizes="180x180">
    <link rel="icon" href="assets/favicons/favicon-32x32.png" sizes="32x32" type="image/png">
    <link rel="icon" href="assets/favicons/favicon-16x16.png" sizes="16x16" type="image/png">
    <link rel="manifest" href="assets/favicons/manifest.json">
    <link rel="mask-icon" href="assets/favicons/safari-pinned-tab.svg" color="#7952b3">
    <link rel="icon" href="assets/favicons/favicon.ico">
    <meta name="theme-color" content="#7952b3">


    <style>
        active{
    color: #fff;
    background-color: #0d6efd;
    border-color: #0d6efd;
}
        .iti {
            width: 100%;
        }
        
        input#txtPhone2 {
            height: calc(3.5rem + 2px);
            line-height: 1.25;
        }
    </style>

   <style type="text/css">
    .modal
    {
        position: fixed;
        top: 0;
        left: 0;
        /*background-color: black;*/
        z-index: 99;
        opacity: 0.8;
        filter: alpha(opacity=80);
        -moz-opacity: 0.8;
        min-height: 100%;
        width: 100%;
    }
    .loading
    {
        /*font-family: Arial;
        font-size: 10pt;
        border: 5px solid #67CFF5;*/
        width: 200px;
        height: 100px;
        display: none;
        position: fixed;
        /*background-color: White;*/
        z-index: 999;
    }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    
       <div class="container body-content">
           <asp:ScriptManager ID="sm" runat="server" EnablePageMethods="true"></asp:ScriptManager>

            <asp:HiddenField ID="hfiso2" runat="server" Value="" />
            <asp:HiddenField ID="hfCountryCode" runat="server" Value="" />

            <div class="d-flex justify-content-end">
                 <div class="form-floating my-2">
                <asp:DropDownList ID="ddLang" runat="server" CssClass="form-select-sm" AutoPostBack="true" OnSelectedIndexChanged="ddLang_SelectedIndexChanged">  
                    <asp:ListItem Value="en-US" Text="English" />  
                    <asp:ListItem Value="ar-EG" Text="Arabic" />  
                </asp:DropDownList> 
<%--                  <label for="ddLang" id="lbllang" runat="server">Language</label>--%>
             </div>
                 <div class="form-floating my-2">
                            <a href="TL_Login.aspx" class="text-decoration-none ps-3 pt-3 small">Sign Out <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-box-arrow-in-right" viewBox="0 0 16 16">
  <path fill-rule="evenodd" d="M6 3.5a.5.5 0 0 1 .5-.5h8a.5.5 0 0 1 .5.5v9a.5.5 0 0 1-.5.5h-8a.5.5 0 0 1-.5-.5v-2a.5.5 0 0 0-1 0v2A1.5 1.5 0 0 0 6.5 14h8a1.5 1.5 0 0 0 1.5-1.5v-9A1.5 1.5 0 0 0 14.5 2h-8A1.5 1.5 0 0 0 5 3.5v2a.5.5 0 0 0 1 0v-2z"/>
  <path fill-rule="evenodd" d="M11.854 8.354a.5.5 0 0 0 0-.708l-3-3a.5.5 0 1 0-.708.708L10.293 7.5H1.5a.5.5 0 0 0 0 1h8.793l-2.147 2.146a.5.5 0 0 0 .708.708l3-3z"/>
</svg></a>

                     </div>
            </div>
              
    <asp:UpdatePanel runat="server" ID="UP1">

        <ContentTemplate>
    <main>
        <div class="container py-lg-1">
            
            <div class="row g-lg-5 h-100 align-items-center justify-content-center">
                <div class="col-md-5 col-lg-6 order-md-last">

                    <img src="assets/img/support-img-01.svg" alt="" class="img-fluid">
                </div>
                <div class="col-md-7 col-lg-6">

                    <div class="tab-content" id="myTabContent" runat="server">
                        <div class="tab-pane fade show active" id="home" runat="server" role="tabpanel" aria-labelledby="home-tab">
                            <!--  -->
                            <h4 class="mb-lg-3 mt-lg-4" id="Campaign_Request_Form" runat="server">Request Form</h4>
                            <hr class="my-4">
                            <form class="needs-validation" novalidate>
                                <div class="form-floating mb-3">
                                    <asp:DropDownList ID="ddlEmployee" CssClass="form-select" Enabled="false" runat="server">
                                        <asp:ListItem Text="select executive" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Employee 1" Value="1"></asp:ListItem>
                                    </asp:DropDownList>
                                   
                                    <label for="floatingSelect" id="Sales_Executive" runat="server">Sales Executive</label>
                                </div>
                               <div class="form-floating mb-3">
                                    <asp:TextBox ID="txtCompany" runat="server" class="form-control" PlaceHolder="Company Name" ></asp:TextBox>
                                    <label for="txtCompany" id="Company_Name" runat="server">Company Name</label>
                                </div>
                                <div class="form-floating mb-3">
                                    <asp:TextBox ID="txtcustomername" runat="server" class="form-control" PlaceHolder="Client Name" ></asp:TextBox>
                                    <label for="txtcustomername" id="Client_Name" runat="server">Client Name</label>
                                </div>
                               
                                <div class="form-floating mb-3">
                                     <input type="tel" class="form-control" onkeypress="return onlyNumberKey(event)" id="txtPhone2" runat="server" placeholder="Mobile Number" maxlength="10" >
                               
                                </div>
                                <div class="form-floating mb-3">
                                    <asp:TextBox ID="txtEmailId" runat="server" class="form-control" PlaceHolder="Email Address" ></asp:TextBox>
                                    <label for="txtEmailId" id="Email_Address" runat="server">Email Address</label>
                                </div>
                         <div class="row">
                            <div class="col">
                                <div class="form-floating mb-3">
                                     <asp:DropDownList runat="server" ID="ddlProduct" OnSelectedIndexChanged="ddlProduct_SelectedIndexChanged" AutoPostBack="true" onchange="HideTextBox()" CssClass="form-select">
                                        <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="SMS" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Email" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Whatsapp" Value="3"></asp:ListItem>
                                    </asp:DropDownList>
                               
                                    <label for="ddlProduct" id="Product_Group" runat="server">Product Group</label>
                                </div>
                            </div>
                            <div class="col">
                                <div class="form-floating mb-3">
                                      <asp:DropDownList runat="server" ID="ddlproductsubgroup" CssClass="form-select">
                                        <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                       <%-- <asp:ListItem Text="GSM" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Oprator" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="SMPP" Value="3"></asp:ListItem>--%>
                                    </asp:DropDownList>
                               
                                    <label for="ddlproductsubgroup" id="Product_Sub_Group" runat="server">Product Sub Group</label>
                                </div>
                            </div>
                            <!--  -->
                         
                        </div>
                                <div class="row">
                               <div class="col">
                                <div class="form-floating mb-3">
                                     <asp:DropDownList runat="server" ID="ddltranstype" CssClass="form-select">
                                        <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Recharge" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Campaign" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                 
                                    <label for="ddltranstype" id="Transaction_Type" runat="server">Transaction Type</label>
                                </div>
                            </div>
                            <div class="col">
                                <div class="form-floating mb-3">
                                     <asp:DropDownList runat="server" ID="ddlOrderType" CssClass="form-select">
                                        <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Cash" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="PO" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Emails" Value="3"></asp:ListItem>
                                    </asp:DropDownList>
                                    <label for="ddlOrderType" id="Order_Type" runat="server">Order Type</label>
                                </div>
                            </div>
                          
                        </div>
                                 <div class="row" id="dvuserid" runat="server" visible="false">
                                  <div class="col-sm-8">
                                <div class="form-floating mb-3" >
                                    <asp:TextBox ID="txtUserId" runat="server" class="form-control" PlaceHolder="UserId " ></asp:TextBox>
                                    <label for="txtUserId" id="UserId" runat="server">UserId (If new client then type new .)</label>
                                </div>

                                      </div>
                                     <div class="col-sm-4">
                                <div class="form-floating mb-3">
                                    <label> &nbsp;</label>
                                    <asp:Button runat="server" ID="btnGo" CssClass="btn btn-primary" OnClick="btnGo_Click" Text="Go" />
                                    </div>

                                      </div>
                                </div>
                                
                                 <div class="mb-3 row">
                                    <label for="inputPassword" class="col-6 col-sm-6 col-form-label" id="Credentials_to_be_used" runat="server">Credentials to be used</label>
                                    <div class="col-6 col-sm-6">
                                        <asp:RadioButtonList runat="server" ID="rbcredentials" onchange="showhidediv()"  RepeatDirection="Horizontal">
                                            <asp:ListItem Text="Client" Selected="True" class="btn btn-outline-primary me-3" Value="Client"></asp:ListItem>
                                            <asp:ListItem Text="MIM" class="btn btn-outline-primary me-3" Value="MIM"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                                <div class="form-floating mb-3" id="dvPEID" runat="server" visible="true">
                                    <asp:TextBox ID="txtPEID" runat="server" class="form-control" PlaceHolder="PEID" ></asp:TextBox>
                                    <label for="txtPEID" runat="server" id="PEID">PEID</label>
                                </div>
                                <div class="form-floating mb-3" id="dvTemplateId" runat="server" visible="true">
                                    <asp:TextBox ID="txtTemplateId" runat="server" class="form-control" PlaceHolder="Template Id" ></asp:TextBox>
                                    <label for="txtTemplateId" id="Template_Id" runat="server">Template Id</label>
                                </div>
                                 <div class="form-floating mb-3" id="dvSenderId" runat="server" visible="true">
                                    <asp:TextBox ID="txtsenderid" runat="server" class="form-control" PlaceHolder="Sender Id" ></asp:TextBox>
                                    <label for="txtsenderid" id="Sender_Id" runat="server">Sender Id</label>
                                </div>
                                 
                                <div class="form-floating mb-3" >
                                    <asp:TextBox ID="txtmsg" TextMode="MultiLine" runat="server" class="form-control" PlaceHolder="SMS Text" ></asp:TextBox>
                                    <label for="txtmsg" id="SMS_Text" runat="server">SMS Text</label>
                                </div>
                        
                              <div class="row mb-3">
                            <div class="col">
                                <div class="form-floating">
                                    <asp:TextBox ID="txtQuantity" runat="server" onkeypress="return onlyNumberKey(event)" class="form-control" PlaceHolder="Quantity" ></asp:TextBox>

                                  <label for="txtQuantity" id="Quantity" runat="server">Quantity</label>
                                </div>
                            </div>
                            <div class="col">
                                <div class="form-floating">
                                    <asp:TextBox ID="txtRate" runat="server" onkeypress="return onlydecimalKey(event)" MaxLength="10" class="form-control" PlaceHolder="Rate" ></asp:TextBox>

                                    <label for="txtRate" id="Rate" runat="server">Rate</label>
                                </div>
                            </div>
                        </div>
                                <div class="mb-3">
                                    <label for="formFileLg" class="form-label" id="filesize" runat="server">Upload File (Size can not be above of 5 mb.)</label>
                                     <asp:FileUpload CssClass="form-control form-control-lg" runat="server" ID="FU1" />
                                    <%-- <asp:RegularExpressionValidator ID="RegularExpressionValidator7"  
                     runat="server" ControlToValidate="FU1"  
                     ErrorMessage="Only .jpeg / .jpg / .png / .pdf" ForeColor="Red"  
                     ValidationExpression="/^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))+(.jpeg|.jpg|.png|.pdf)$/"  
                     ValidationGroup="PartnerProfileUpdate" SetFocusOnError="true"></asp:RegularExpressionValidator>  --%>
                                </div>
                             
                                <hr class="my-4">
                                <asp:Button runat="server" ID="btnSave" Text="Submit" OnClick="btnSave_Click" CssClass="w-100 btn btn-primary btn-lg"/>
                            </form>
                            <!--  -->
                        </div>
                        
                    </div>
                    </div>
            </div>
        </div>
    </main>
<div class="loading" align="center">
    <img src="img/loading.gif" alt="" />
</div>
    
    </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />
            <%--<asp:PostBackTrigger ControlID="ddlProduct" />--%>
            
        </Triggers>
    </asp:UpdatePanel>
    
              <asp:UpdateProgress ID="UpdateProgress1" runat="Server" AssociatedUpdatePanelID="UP1">
    <ProgressTemplate>
        <span style="background-color:#66997A;"> <%--<img src="img/images.jpg" alt="Please wait"  width="100px"/>--%> Please wait ...</span>
    </ProgressTemplate>
</asp:UpdateProgress>

        </div>
         
   <!-------------->
     <link href="OffLineCDN/CSS/bootstrap5.css" rel="stylesheet" />
    <link href="OffLineCDN/CSS/intITellInput.css" rel="stylesheet" />
    <link href="OffLineCDN/bootstrap.min.css" rel="stylesheet" />
      <script src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.3/jquery.min.js"></script>

      <script src="https://code.jquery.com/jquery-3.6.0.min.js" integrity="sha256-/xUj+3OJU5yExlq6GSYGSHk7tPXikynS7ogEvDej/m4=" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.9.2/dist/umd/popper.min.js" integrity="sha384-IQsoLXl5PILFhosVNubq5LC7Qb9DXgDA9i+tQ8Zj3iwWAwPtgFTxbJ8NT4GN1R8p" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.min.js" integrity="sha384-cVKIPhGWiC2Al4u+LWgxfKTRIcfu0JTxR+EQDz/bgldoEyl4H0zUF0QKbrJ0EcQF" crossorigin="anonymous"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/16.0.8/js/intlTelInput-jquery.min.js"></script>
    <script src="OffLineCDN/bootstrap5.min.js"></script>
    <script src="OffLineCDN/proper.min.js"></script>

    <script src="OffLineCDN/jquery.min.js"></script>
    <script src="OffLineCDN/intTellinput-jquery.min.js.js"></script>
    <script type="text/javascript">
       $(function () {
          //alert($('#<%= hfiso2.ClientID %>').val());
             $('#txtPhone2').intlTelInput({
                 autoHideDialCode: true,
                 autoPlaceholder: "ON",
                 dropdownContainer: document.body,
                 formatOnDisplay: true,
                 hiddenInput: "full_number",
                 initialCountry:  $('#<%= hfiso2.ClientID %>').val()==""? "IN": $('#<%= hfiso2.ClientID %>').val(),
                 nationalMode: true,
                 placeholderNumberType: "MOBILE",
                 preferredCountries: ["IN", "AE"],
                 separateDialCode: true
             });


             
           <%--  $('#<%= rbcredentials.ClientID %>').on('onchange', function () {
                    var iso2 = $("#txtPhone2").intlTelInput("getSelectedCountryData").iso2;
                 var code = $("#txtPhone2").intlTelInput("getSelectedCountryData").dialCode;
                 $('#<%= hfCountryCode.ClientID %>').val(code);
                 $('#<%= hfiso2.ClientID %>').val(iso2);
             });--%>
             $('#<%= btnSave.ClientID %>').on('click', function () {
                

                  if ($('#<%= ddlEmployee.ClientID %>').val()=="0") {
                     alert("Please select any sales executive.");
                     $('#<%= ddlEmployee.ClientID %>').focus();
                     return false;
                 }

                  if ($('#<%= txtCompany.ClientID %>').val().trim()=="") {
                     alert("Please select any company name.");
                     $('#<%= txtCompany.ClientID %>').focus();
                     return false;
                 }

                   if ($('#<%= txtcustomername.ClientID %>').val().trim()=="") {
                     alert("Please enter client name.");
                     $('#<%= txtcustomername.ClientID %>').focus();
                     return false;
                 }

                 var iso2 = $("#txtPhone2").intlTelInput("getSelectedCountryData").iso2;
                 var code = $("#txtPhone2").intlTelInput("getSelectedCountryData").dialCode;
                 $('#<%= hfCountryCode.ClientID %>').val(code);
                 $('#<%= hfiso2.ClientID %>').val(iso2);
                 
                 var mobentr = $('#txtPhone2').val().length;
                 var moblen = $('#<%= hfCountryCode.ClientID %>').val().length + mobentr;
                 if (moblen!=12) {
                     alert("Mobile no should be of 12 digit.");
                     $('#<%= txtPhone2.ClientID %>').focus();
                     return false;
                 }
                 if ($('#<%= txtEmailId.ClientID %>').val()=="") {
                     alert("Please enter email id.");
                     $('#<%= txtEmailId.ClientID %>').focus();
                     return false;
                 }

                 if ($('#<%= txtEmailId.ClientID %>').val().includes("@"))
                 {
                     
                 }
                  else {
                        alert("Please enter email address in xxxxx@yyyy.zzz format.");
                          $('#<%= txtEmailId.ClientID %>').focus();
                         return false;
                 }

                  if ($('#<%= txtEmailId.ClientID %>').val().includes("."))
                 {
                     
                 }
                  else {
                        alert("Please enter email address in xxxxx@yyyy.zzz format.");
                          $('#<%= txtEmailId.ClientID %>').focus();
                         return false;
                 }

                  if ($('#<%= ddlProduct.ClientID %>').val()=="0") {
                     alert("Please select any product group.");
                     $('#<%= ddlProduct.ClientID %>').focus();
                     return false;
                 }
                   if ($('#<%= ddlProduct.ClientID %>').val()=="1" && $('#<%= ddlproductsubgroup.ClientID %>').val()=="0") {
                     alert("Please select any product sub group.");
                     $('#<%= ddlproductsubgroup.ClientID %>').focus();
                     return false;
                 }
                   if ($('#<%= ddlProduct.ClientID %>').val()=="8" && $('#<%= ddlproductsubgroup.ClientID %>').val()=="0") {
                     alert("Please select any product sub group.");
                     $('#<%= ddlproductsubgroup.ClientID %>').focus();
                     return false;
                 }
                   if ($('#<%= ddltranstype.ClientID %>').val()=="0") {
                     alert("Please select any transaction type.");
                     $('#<%= ddltranstype.ClientID %>').focus();
                     return false;
                 }
                   if ($('#<%= ddlOrderType.ClientID %>').val()=="0") {
                     alert("Please select any order type.");
                     $('#<%= ddlOrderType.ClientID %>').focus();
                     return false;
                 }
                   if ($("input[name='rbcredentials']:checked").val()=="Client" && $('#<%= txtPEID.ClientID %>').val()=="" && $('#<%= ddLang.ClientID %>').val()!="ar-EG") {
                     alert("Please enter peid.");
                     $('#<%= txtPEID.ClientID %>').focus();
                     return false;
                 }
                  if ($('#<%= ddlProduct.ClientID %>').val()=="1" && $('#<%= txtUserId.ClientID %>').val()=="") {
                     alert("Please enter userid.");
                     $('#<%= txtUserId.ClientID %>').focus();
                     return false;
                 }
                   if ($("input[name='rbcredentials']:checked").val()=="Client" && $('#<%= txtsenderid.ClientID %>').val()=="") {
                     alert("Please enter sender id.");
                     $('#<%= txtsenderid.ClientID %>').focus();
                     return false;
                 }
                   if ($("input[name='rbcredentials']:checked").val()=="Client" && $('#<%= txtTemplateId.ClientID %>').val()=="" && $('#<%= ddLang.ClientID %>').val()!="ar-EG") {
                     alert("Please enter template id.");
                     $('#<%= txtTemplateId.ClientID %>').focus();
                     return false;
                 }
                   if ($('#<%= txtmsg.ClientID %>').val()=="") {
                     alert("Please enter sms text.");
                     $('#<%= txtmsg.ClientID %>').focus();
                     return false;
                 }
                   if ($('#<%= txtQuantity.ClientID %>').val()=="") {
                     alert("Please enter quantity.");
                     $('#<%= txtQuantity.ClientID %>').focus();
                     return false;
                 }
                  if (parseFloat($('#<%= txtQuantity.ClientID %>').val())<=0) {
                     alert("Please enter quantity.");
                     $('#<%= txtQuantity.ClientID %>').focus();
                     return false;
                 }
                   if ($('#<%= txtRate.ClientID %>').val()=="") {
                     alert("Please enter rate.");
                     $('#<%= txtRate.ClientID %>').focus();
                     return false;
                 }
                  if (parseFloat($('#<%= txtRate.ClientID %>').val())<=0) {
                     alert("Please enter rate.");
                     $('#<%= txtRate.ClientID %>').focus();
                     return false;
                 }
                 ShowProgress();

                <%--  $('#txtPhone2').intlTelInput({
                 autoHideDialCode: true,
                 autoPlaceholder: "ON",
                 dropdownContainer: document.body,
                 formatOnDisplay: true,
                 hiddenInput: "full_number",
                 initialCountry: $('#<%= hfCountryCode.ClientID %>').val()=="" ? "IN": $('#<%= hfCountryCode.ClientID %>').val(),
                 nationalMode: true,
                 placeholderNumberType: "MOBILE",
                 preferredCountries: ["IN", "AE"],
                 separateDialCode: true
             });--%>
             });
             
             $('#rbcredentials').find('span').removeClass('btn btn-outline-primary me-3');
             $('#rbcredentials').find('input').addClass('btn-check');
             $('#rbcredentials').find('label').addClass('btn btn-outline-primary me-3');
         });
    </script>
      <script>
          function ConfirmFunction() {
              if (confirm("Are you sure ") == true) {
                  return true;
              }
              else {
                  return false;
              }
          }
          function onlydecimalKey(evt) {
            
        // Only ASCII character in that range allowed
        var ASCIICode = (evt.which) ? evt.which : evt.keyCode
        if (ASCIICode > 31 && (ASCIICode == 47 || ASCIICode < 46 || ASCIICode > 57))
            return false;
        return true;
          }
          function onlyNumberKey(evt) {
                  var iso2 = $("#txtPhone2").intlTelInput("getSelectedCountryData").iso2;
                 var code = $("#txtPhone2").intlTelInput("getSelectedCountryData").dialCode;
                 $('#<%= hfCountryCode.ClientID %>').val(code);
                 $('#<%= hfiso2.ClientID %>').val(iso2);
        // Only ASCII character in that range allowed
        var ASCIICode = (evt.which) ? evt.which : evt.keyCode
        if (ASCIICode > 31 && (ASCIICode < 48 || ASCIICode > 57))
            return false;
        return true;
    }
      </script>
    <script>
          function showhidediv()
        {
                 var iso2 = $("#txtPhone2").intlTelInput("getSelectedCountryData").iso2;
                 var code = $("#txtPhone2").intlTelInput("getSelectedCountryData").dialCode;
                 $('#<%= hfCountryCode.ClientID %>').val(code);
              $('#<%= hfiso2.ClientID %>').val(iso2);

              var pval = $('#<%= rbcredentials.ClientID %>').val();
             var pval = $("input[name='rbcredentials']:checked").val();
             if(pval == 'MIM')  //it depends on which value Selection do u want to hide or show your textbox
             {
                  $('#dvPEID').hide();
                 $('#dvSenderId').hide();
                 $('#dvTemplateId').hide();
                
             }
             else
             {
                 $('#dvPEID').show();
                 $('#dvSenderId').show();
                 $('#dvTemplateId').show();
             }
        }
        function HideTextBox()
        {
                 var iso2 = $("#txtPhone2").intlTelInput("getSelectedCountryData").iso2;
                 var code = $("#txtPhone2").intlTelInput("getSelectedCountryData").dialCode;
                 $('#<%= hfCountryCode.ClientID %>').val(code);
                 $('#<%= hfiso2.ClientID %>').val(iso2);
            var pval = $('#<%= ddlProduct.ClientID %>').val();
             if(pval == '1')  //it depends on which value Selection do u want to hide or show your textbox
             {
                 $('#dvuserid').show();
             }
             else
             {
                  $('#dvuserid').hide();
            }

            showhidediv();
        }
 </script>
    <%--<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js" type="text/javascript"></script>--%>
    <script type="text/javascript">

         function loadscr(countrycode) {
            $('#txtPhone2').intlTelInput({
                 autoHideDialCode: true,
                 autoPlaceholder: "ON",
                 dropdownContainer: document.body,
                 formatOnDisplay: true,
                 hiddenInput: "full_number",
                 initialCountry: countrycode,
                 nationalMode: true,
                 placeholderNumberType: "MOBILE",
                 preferredCountries: ["IN", "AE"],
                 separateDialCode: true
             });

                 $('#rbcredentials').find('span').removeClass('btn btn-outline-primary me-3');
             $('#rbcredentials').find('input').addClass('btn-check');
             $('#rbcredentials').find('label').addClass('btn btn-outline-primary me-3');
             
       }
    function ShowProgress() {
        setTimeout(function () {
            var modal = $('<div />');
            modal.addClass("modal");
            $('body').append(modal);
            var loading = $(".loading");
            loading.show();
            var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
            var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
            loading.css({ top: top, left: left });
        }, 200);
    }


    
</script>
       

</asp:Content>
 

