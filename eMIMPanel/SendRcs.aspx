<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="SendRcs.aspx.cs" Inherits="eMIMPanel.SendRcs"
    MaintainScrollPositionOnPostback="true" EnableEventValidation="false"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <style type="text/css">
        /*CSS Classes For Design Modal*/

.select2-container--default .select2-selection--single {
    background-color: #e6e7ee!important;
   
    border-radius: 4px;
    border: 1px solid #aaa!important;
    box-shadow: inset 2px 2px 5px #b8b9be, inset -3px -3px 7px #ffffff!important;
}

.select2-search--dropdown .select2-search__field{padding:4px;width:100%;box-sizing:border-box;background:#e6e7ee!important;border:1px solid red;}
.select2-dropdown{background-color:#e6e7ee!important;border:1px solid #aaa;}

        .modalPopup {
            min-height: 75px;
            position: fixed;
            z-index: 2000;
            padding: 0;
            background-color: #fff;
            border-radius: 6px;
            background-clip: padding-box;
            border: 1px solid rgba(0, 0, 0, 0.2);
            min-width: 295px;
            box-shadow: 0 5px 10px rgba(0, 0, 0, 0);
        }


        .modalPopup1 {
            min-height: 75px;
            position: fixed;
            z-index: 2000;
            padding: 0;
            background-color: #fff;
            border-radius: 6px;
            background-clip: padding-box;
            border: 1px solid rgba(0, 0, 0, 0.2);
            min-width: 800px;
            box-shadow: 0 5px 10px rgba(0, 0, 0, 0);
        }

        .modalBackground {
            position: fixed;
            top: 0;
            left: 0;
            background-color: #000;
            opacity: 0.5;
            z-index: 1800;
            min-height: 100%;
            width: 100%;
            overflow: hidden;
            filter: alpha(opacity=50);
            display: inline-block;
            z-index: 1000;
        }

        /*  */

        .select2-container .select2-selection--single .select2-selection__rendered {
            padding-left: 0;
            padding-right: 0;
            height: auto;
            margin-top: -3px
        }

        .select2-container--default .select2-selection--single,
        .select2-selection .select2-selection--single {
            border: 1px solid #d2d6de;
            border-radius: 5px !important;
            padding: 6px 12px;
            height: 40px !important
        }

            .select2-container--default .select2-selection--single .select2-selection__arrow {
                height: 26px;
                position: absolute;
                top: 6px !important;
                right: 1px;
                width: 20px
            }

        /*  */
    </style>
    <script type="text/javascript">
        function Confirm() {
            
            var confirm_value = document.createElement("INPUT");
            confirm_value.nodeType = "hidden";
            confirm_value.nodeName = "confirm_value";
            if (confirm("Messate Text Changed")) {
                confirm_value.value = "Yes";
            }
            else {
                confirm_value.value = "No";
            }
            document.forms[0].appendChild(confirm_value);
        }
    </script>
    <link href="ms-Dropdown-master/css/msdropdown/dd.css" rel="stylesheet" />
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>
    <script type="text/javascript" src="ms-Dropdown-master/js/jquery/jquery-1.9.0.min.js"></script>
    <script type="text/javascript" src="ms-Dropdown-master/js/msdropdown/jquery.dd.js"></script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </asp:ToolkitScriptManager>
     <main>
        <div class="container-fluid">
            <!-- Content Row -->
            <div class="row">
                <div class="col-lg-10 col-xl-11">
                    <!-- Start Card -->
                    <div class="card bg-primary border-light shadow-soft mb-4">
                        <div class="card-header py-3 bg-primary d-flex justify-content-between flex-wrap align-content-center">
                            <h6 class="m-0 font-weight-bold font-weight-bold my-auto">
                               <img src="assets/icon/google_icon.svg" alt="google_icon.svg" width="18px">
                                Send RCS</h6>
                        </div>
                        <div class="card-body pt-0">
                            <!-- Start Card -->
                            <div id="divFileLoader" runat="server" style="display: none; text-align: center" class="form-group row">
                                <h3>File uploading. Please wait...</h3>
                                <img src="img/loading.gif" />
                            </div>
                            <div id="div10" runat="server" class="form-group row">
                                <label for="inputEmail33" class="col-sm-2 col-form-label font-weight-bold" style="margin-top: -10px;">Country Code</label>
                                <div class="col-md-5">
                                    <div id="divmobile" runat="server" class="form-label-group">
                                        <%-- <asp:DropDownList ID="ddlCCode" runat="server" class="custom-select" AutoPostBack="true" OnSelectedIndexChanged="ddlCCode_SelectedIndexChanged"></asp:DropDownList>--%>

                                        <asp:DropDownList ID="ddlCCode" runat="server" class="custom-select" AutoPostBack="true" OnSelectedIndexChanged="ddlCCode_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="5">
                                </div>
                            </div>
                         
                             <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                           <div class="form-group row" >
                                <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">RCS Type</label>
                                <div class="col-sm-5">
                                    <asp:DropDownList ID="ddlRCSType" runat="server" class="custom-select" AutoPostBack="true" OnSelectedIndexChanged="ddlRCSType_SelectedIndexChanged">
                                    <asp:ListItem Value="1" Selected="True">Text</asp:ListItem>
                                                <asp:ListItem  Value="2">Image</asp:ListItem>
                                                <asp:ListItem Value="3">Video</asp:ListItem>
                                        <asp:ListItem  Value="4">Card</asp:ListItem>
                                        <asp:ListItem  Value="5">Carousel</asp:ListItem>
                                        
                                        </asp:DropDownList>

                                </div>
                                <div class="col-sm-5">
                                    <asp:Label ID="lblRCSRate" runat="server" ></asp:Label>
                                </div>
                            </div>

                            <div class="form-group row" hidden="hidden">
                                <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">Sender ID</label>
                                <div class="col-sm-5">
                                    <asp:DropDownList ID="ddlSender" runat="server" class="custom-select" AutoPostBack="true" OnSelectedIndexChanged="ddlSender_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group row">
                                <legend class="col-form-label col-sm-2 pt-0 font-weight-bold">Mobile Numbers</legend>
                                <div class="col-sm-10">
                                    <div class="custom-control custom-radio custom-control-inline pl-2">
                                        <asp:RadioButton class="mr-2" ID="rdbUpload" runat="server" GroupName="mobile" AutoPostBack="true" OnCheckedChanged="rdbUpload_CheckedChanged" />
                                        <%--<input type="radio" id="customRadioInline01" name="customRadioInline2" class="custom-control-input">--%>
                                        <label>
                                            Upload Files <i class="fas fa-file-csv text-primary"></i>
                                            <br>
                                            <small>(CSV/TXT)</small></label></div>
                                  
                                    <div class="custom-control custom-radio custom-control-inline pl-2">
                                        <asp:RadioButton class="mr-2" ID="rdbEntry" runat="server" Checked="true" GroupName="mobile" AutoPostBack="true" OnCheckedChanged="rdbUpload_CheckedChanged" />
                                        <label>Enter Number <i class="far fa-keyboard text-dark"></i></label>
                                    </div>
                                   
                                </div>
                            </div>
                                    <div id="divFileUpload" runat="server" class="form-group row d-none">
                                <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold" style="margin-top: -10px;">Mobile Numbers File&nbsp;</label>
                                <div class="col-md-6">
                                    <div class="custom-file">
                                        <asp:FileUpload ID="FileUpload1" runat="server" class="custom-file-input" ClientIDMode="Static" onchange="if( SMSfileUpload() ) { console.log('formsubmit'); this.form.submit(); }" />
                                        <label class="custom-file-label" for="customFile">Choose file</label>
                                        <p class="text-danger small mt-2 mb-0">Maximum File Size 6 MB <b>(Mobile Numbers without Country Code)</b></p>
                                    </div>
                                </div>
                                <div class="4">
                                    <asp:Label ID="lblUploading" runat="server"></asp:Label>
                                </div>
                            </div>
                                    <div id="divCamp" runat="server" class="form-group row d-none">
                                <label for="exampleFormControlTextarea1" class="col-sm-2 col-form-label font-weight-bold">Campaign Name</label>
                                <div class="col-md-6">
                                    <asp:TextBox class="form-control" ID="txtCampNm" runat="server" placeholder="Campaign Name" ToolTip="Campaign Name" />
                                </div>
                            </div>
                            <div class="form-group row" id="Tnumber" runat="server">
                                <label for="exampleFormControlTextarea1" id="lbln" runat="server" class="col-sm-2 col-form-label font-weight-bold">Total Numbers</label>
                                <div class="col-sm-10">
                                    <div id="divNum" runat="server" style="pointer-events: all;">
                                        <asp:TextBox ID="txtMobNum" runat="server" class="form-control" TextMode="MultiLine" Rows="5" MaxLength="2147483647" onkeyup="integersOnly(this); mobnumbcnt(); return true;"></asp:TextBox>
                                        <%--<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterMode="ValidChars"
                                                        TargetControlID="txtMobNum" ValidChars="0123456789, ">
                                            </asp:FilteredTextBoxExtender>--%>
                                   

                                    <div class="d-flex">
                                        <span class="text-danger small mt-2 mb-0 small"><b>Enter Mobile Numbers without Country Code</b></span>
                                    </div>
                               </div>
                                    
                                    <%--</div>--%>
                                </div>
                            </div>
                                    <div class="form-group row">
                                          <label for="exampleFormControlTextarea1" id="Label2" runat="server" class="col-sm-2 col-form-label font-weight-bold"></label>
                                       
                                        <div class="col-md-4">
                                             <p class="my-2 mr-3">
                                            <asp:CheckBox ID="chkAllowDuplicates" runat="server" />
                                            <label class="form-check-label font-weight-bold" for="gridCheck2">Allow Duplicates </label>
                                        </p>
                                        </div>
                                       <div class="col-md-4">
                                        <p class="my-2">
                                            <span class="font-weight-bold">Number Count:</span>
                                            <asp:Label ID="lblMobileCnt" runat="server" class="small"></asp:Label>
                                        </p>
                                           <p class="my-2" hidden="hidden">
                                            <span class="font-weight-bold small">Excude Number Count:</span>
                                            <asp:Label ID="lblExcludeCnt" runat="server" class="small"></asp:Label>
                                        </p>
                                        <p class="my-2" hidden="hidden">
                                            <span class="font-weight-bold small">RCS to be Sent On:</span>
                                            <asp:Label ID="lblActualMobCnt" runat="server" class="small"></asp:Label>
                                        </p>
                                           </div>
                                        
                                    
                                        </div>
                            <div class="form-group row" style="display:none;">
                                <div class="col-lg-6">
                                    <div class="row">
                                        <label for="inputEmail3" class="col-lg-4 col-form-label font-weight-bold">Select Language</label>
                                        <div class="col-lg-8">
                                            <asp:DropDownList ID="ddlLanguage" runat="server" class="custom-select">
                                                <%--<asp:ListItem Text="Hindi" Value="1"></asp:ListItem>--%>
                                                <asp:ListItem Text="English" Value="2" Selected="True"></asp:ListItem>
                                                <%--<asp:ListItem Text="Urdu" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="Arbic" Value="4"></asp:ListItem>
                                                <asp:ListItem Text="Marathi" Value="5"></asp:ListItem>--%>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                               
                            </div>
                            
                               <div class="form-group row" id="divselect" runat="server">
                                <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Select </label>
                                <div class="col-sm-10">
                                    <div id="div3" runat="server" style="pointer-events: all;">
                                        <div class="row">
                                            <div class="col-lg-12">
                                                <asp:RadioButtonList class="mr-2 font-weight-bold" Width="180px" ID="rbtnSelect" OnSelectedIndexChanged="rbtnSelect_SelectedIndexChanged" AutoPostBack="true" RepeatDirection="Horizontal" runat="server">
                                                    <asp:ListItem Selected="True" Value="0">Manual</asp:ListItem>
                                                     <asp:ListItem Value="1">Template</asp:ListItem>
                                                </asp:RadioButtonList>
                                               
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                              
                            <div id="divuploadImage" runat="server" class="form-group row">
                                <label for="inputEmail31" class="col-sm-2 col-form-label font-weight-bold" style="margin-top: -10px;">Upload Image&nbsp;</label>
                                <div class="col-md-6">
                                    <div>
                                        <asp:FileUpload ID="uImage" runat="server" class="custom-file-input" ClientIDMode="Static" onchange="if( SMSfileUpload60() ) { console.log('formsubmit'); this.form.submit(); }" />
                                        <label class="custom-file-label" for="customFile">Choose file</label>
                                         <small id="textHelp2" class="form-text text-muted mb-3 mb-lg-0">Upload Limit <strong>2 MB.</strong> &nbsp;&nbsp;&nbsp;&nbsp;</small>
                                    </div>
                                </div>
                                <div class="4">
                                    
                                    <asp:Label ID="lblImage" runat="server"></asp:Label>
                                </div>
                            </div>
                             <div id="divuploadVideo" runat="server" class="form-group row">
                                <label for="inputEmail31" class="col-sm-2 col-form-label font-weight-bold" style="margin-top: -10px;">Upload Video&nbsp;</label>
                                <div class="col-md-6">
                                    <div>
                                        <asp:FileUpload ID="uVideo" runat="server" class="custom-file-input" ClientIDMode="Static" onchange="if( SMSfileUpload61() ) { console.log('formsubmit'); this.form.submit(); }" />
                                        <label class="custom-file-label" for="customFile">Choose file</label>
                                        
                                    </div>
                                </div>
                                <div class="4">
                                    <asp:Label ID="lblVideo" runat="server"></asp:Label>
                                </div>
                            </div>

                           

                            

                           

                            <div class="form-group row d-none" id="divTempId" runat="server">
                                <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Template ID</label>
                                <div class="col-sm-10">
                                    <div id="div8" runat="server" style="pointer-events: all;">
                                        <div class="row">
                                            <div class="col-lg-12">
                                                <asp:DropDownList ID="ddlTempID" runat="server" class="custom-select"  OnSelectedIndexChanged="ddlTempID_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                                
                            <div id="divsmstext"   runat="server" class="form-group row">
                                <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">RCS Text</label>
                                <div class="col-sm-10">
                                    <div id="divMsg" runat="server" style="pointer-events: all;">
                                        <asp:TextBox ID="txtMsg" runat="server" TextMode="MultiLine" MaxLength="12288" class="form-control"  Rows="7" onkeyup="smscnt(); return true;"></asp:TextBox>
                                        <div id="divOptOut" runat="server" style="display: block;">
                                            <asp:CheckBox ID="chkOptOut" runat="server" Text="Include - " />
                                            <asp:Label ID="lblOptOut" runat="server" Text="DND7726"></asp:Label>
                                        </div>
                                    </div>
                                    <%--<textarea class="form-control" id="exampleFormControlTextarea2" rows="7"></textarea>--%>
                                    <div class="d-flex justify-content-between">
                                        <p class="my-2">
                                            <span class="font-weight-bold">Used :</span>
                                            <asp:Label ID="lblused" runat="server" Text="0"></asp:Label>
                                        </p>
                                        <p class="my-2">
                                            <span class="font-weight-bold">RCS Count :</span>
                                            <asp:Label ID="lblsmscnt" runat="server" Text="0"></asp:Label>
                                        </p>
                                        <p class="my-2">
                                            <%--<span class="font-weight-bold">Unicode :</span> --%>
                                            <asp:Label ID="lblUniCode" runat="server" Text="" Style="color: red; font-weight: bolder;display:none"></asp:Label>
                                        </p>
                                    </div>
                                </div>
                            </div>
                              
                            <div class="form-group row" id="Div1" runat="server" >
                                <label for="exampleFormControlTextarea1" id="Label1" runat="server" class="col-sm-2 col-form-label font-weight-bold"></label>
                                <div class="col-sm-10">
                             <div class="d-flex flex-md-row flex-wrap justify-content-between">
                                        <div class="col-md-4">
                                             <p class="my-2 mr-3">
                                            <asp:CheckBox ID="cbfailover" runat="server" Checked="false" OnCheckedChanged="cbfailover_CheckedChanged" AutoPostBack="true" />
                                            <label class="form-check-label font-weight-bold" for="gridCheck2">Activate FailOver</label>
                                        </p>
                                        </div>
                                        
                                        </div>
                                    </div>                              
                                         
                                    </div>
                          

                             <div class="col-sm-6 form-group row d-none"  id="smstempId" runat="server">
                                  <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold"></label>

                                <label for="exampleFormControlTextarea2" class="col-sm-4 col-form-label font-weight-bold">SMS Template ID
                                  <asp:DropDownList ID="ddlsmsTemplateId" OnSelectedIndexChanged="ddlsmsTemplateId_SelectedIndexChanged" AutoPostBack="true" runat="server" class="custom-select" ></asp:DropDownList>
                                 <div id="divtxt" runat="server">

                                 
                                        <asp:TextBox ID="TextBox1" runat="server" MaxLength="30" Visible="false" placeholder="{Var1}" onkeyup="SMStextChange1(); return true;" class="form-control" ></asp:TextBox>
                                        
                                        <asp:TextBox ID="TextBox2" runat="server" MaxLength="30" Visible="false" placeholder="{Var2}"  onkeyup="SMStextChange1(); return true;" class="form-control" ></asp:TextBox>
                                            
                                        <asp:TextBox ID="TextBox3" runat="server"  MaxLength="30" Visible="false" placeholder="{Var3}"   onkeyup="SMStextChange1(); return true;" class="form-control" ></asp:TextBox>
                                          
                                        <asp:TextBox ID="TextBox4" runat="server"  MaxLength="30" Visible="false" placeholder="{Var4}"  onkeyup="SMStextChange1(); return true;"  class="form-control" ></asp:TextBox>
                                                       
                                        <asp:TextBox ID="TextBox5" runat="server"  MaxLength="30" Visible="false" placeholder="{Var5}"  onkeyup="SMStextChange1(); return true;"  class="form-control" ></asp:TextBox>
                                                         
                                        <asp:TextBox ID="TextBox6" runat="server"  MaxLength="30" Visible="false" placeholder="{Var6}"   onkeyup="SMStextChange1(); return true;" class="form-control" ></asp:TextBox>
                                                   
                                        <asp:TextBox ID="TextBox7" runat="server"  MaxLength="30" Visible="false" placeholder="{Var7}"   onkeyup="SMStextChange1(); return true;" class="form-control" ></asp:TextBox>
                                                                     
                                        <asp:TextBox ID="TextBox8" runat="server"  MaxLength="30" Visible="false"  placeholder="{Var8}"  onkeyup="SMStextChange1(); return true;" class="form-control" ></asp:TextBox>
                                                                       
                                        <asp:TextBox ID="TextBox9" runat="server"  MaxLength="30" Visible="false" placeholder="{Var9}"   onkeyup="SMStextChange1(); return true;" class="form-control" ></asp:TextBox>
                                                                             
                                        <asp:TextBox ID="TextBox10" runat="server"  MaxLength="30" Visible="false" placeholder="{Var10}"  onkeyup="SMStextChange1(); return true;" class="form-control" ></asp:TextBox>
                                          </div>               
                                    </label>

                                  <div class="col-sm-6 form-group row" id="divTempsms" visible="false" runat="server">
                                      <%--<div class="col-sm-12 ">--%>
                                <label for="exampleFormControlTextarea2" class="col-sm-12 col-form-label font-weight-bold">Preview: 
                                    <asp:TextBox ID="txtsms" runat="server" TextMode="MultiLine" onkeyup="smscnt1(); return true;" class="form-control" Rows="10"></asp:TextBox>
                                    <div class="d-flex justify-content-between">
                                        <p class="my-2">
                                            <span class="font-weight-bold">Used :</span>
                                            <asp:Label ID="lblsmdused" runat="server" Text="0"></asp:Label>
                                        </p>
                                        <p class="my-2">
                                            <span class="font-weight-bold">SMS Count :</span>
                                            <asp:Label ID="lblsmscount" runat="server" Text="0"></asp:Label>
                                        </p>
                                        <p class="my-2">
                                            <%--<span class="font-weight-bold">Unicode :</span> --%>
                                            <asp:Label ID="Label5" runat="server" Text="" Style="color: red; font-weight: bolder;"></asp:Label>
                                        </p>
                                    </div>
                                        <asp:HiddenField ID="hdnTemplateVarText" runat="server" />
                                         <asp:HiddenField ID="vrCount" runat="server" />
                                </label>
                                         <%-- </div>--%>
                                  
                            </div> 
                            </div>
<div>
        
                                  
                                 
</div>
                              
                              <div id="divsms" runat="server" class="form-group row d-none">
                                <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">SMS Text</label>
                                <div class="col-sm-10">
                                    <div id="div4" runat="server" style="pointer-events: all;">
                                        
                                        <asp:TextBox ID="lblTempSMS" runat="server" TextMode="MultiLine" MaxLength="2000" class="form-control" Rows="10" disabled="disabled"></asp:TextBox>
                                    </div>
                                    <%--<textarea class="form-control" id="exampleFormControlTextarea2" rows="7"></textarea>--%>
                                    
                                </div>
                            </div>

                                    </ContentTemplate>
                                
                                 
                                </asp:UpdatePanel>
                            
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional" >
                                <ContentTemplate>
                                    <div class="form-group row justify-content-end">
                                        <div class="col-sm-10">
                                            <div class="row">
                                                <div class="col-lg-3">
                                                    <asp:LinkButton ID="LinkButton2" runat="server" class="btn btn-primary text-secondary btn-block" OnClientClick="return ConfirmSubmit();" OnClick="btnSend_Click" data-toggle="tooltip" data-placement="top">
                                                        <span class="text-secondary"> <i class="fas fa-paper-plane"></i> Send</span>
                                                    </asp:LinkButton>
                                                </div>
                                                <div class="col-lg-3">
                                                    <asp:LinkButton ID="LinkButton3" runat="server" class="btn btn-primary text-danger btn-block my-3 my-lg-0" OnClick="btnCancel_Click" data-toggle="tooltip" data-placement="top">
                                                        <span class="text-danger"> <i class="fas fa-times"></i> Cancel</span>
                                                    </asp:LinkButton>
                                                </div>
                                               
                                            </div>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            

                            
                            <!--footer-->
                            <div id="divFooter" runat="server" visible="false">
                                <div class="card card-body mb-4 bg-primary border-light shadow-inset">
                                    <div class=" justify-content-between align-items-center text-center">
                                        <div class="text-danger">
                                            <strong>
                                                <asp:Label ID="lblNotice" runat="server"></asp:Label></strong>
                                        </div>

                                    </div>
                                </div>
                            </div>

                            <!--End-->

                            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1" DisplayAfter="0">
                                <ProgressTemplate>
                                    <div align="center" style="position: absolute; left: 40%; top: 50%; vertical-align: middle;">
                                        <img src="Img/LOADING.GIF" />
                                    </div>
                                    <div align="center" style="position: absolute; left: 0%; top: 0%; vertical-align: middle;">
                                    </div>
                                </ProgressTemplate>
                            </asp:UpdateProgress>

                            <div class="form-group row justify-content-end">
                                <div class="col-sm-10 ">
                                    <h2>
                                        <asp:Label ID="lblStat" runat="server" Visible="false"></asp:Label></h2>
                                </div>
                            </div>

                        </div>

                    </div>
                    <!-- End Card -->
                </div>
            </div>
            <!-- End Row -->
        </div>
    </main>

    <div id="dialogoverlay"></div>

    <div id="dialogbox">
        <div>
            <div id="dialogboxhead"></div>
            <div id="dialogboxbody"></div>
            <div id="dialogboxfoot"></div>
        </div>
    </div>
     
    <script type="text/javascript">

       

        function text_changed_from1() {
            var d = document.getElementById("ContentPlaceHolder1_txtScheduleDate1").value
            console.log(d);
            document.getElementById("ContentPlaceHolder1_hdnScheduleDate1").value = d;
        }
         

        function text_changed_from2() {
            var d = document.getElementById("ContentPlaceHolder1_txtScheduleDate2").value
            console.log(d);
            document.getElementById("ContentPlaceHolder1_hdnScheduleDate2").value = d;
        }
         

        function text_changed_from3() {
            var d = document.getElementById("ContentPlaceHolder1_txtScheduleDate3").value
            console.log(d);
            document.getElementById("ContentPlaceHolder1_hdnScheduleDate3").value = d;
        }
        

        function text_changed_from4() {
            var d = document.getElementById("ContentPlaceHolder1_txtScheduleDate4").value
            console.log(d);
            document.getElementById("ContentPlaceHolder1_hdnScheduleDate4").value = d;
        }
        
        function text_changed_from5() {
            var d = document.getElementById("ContentPlaceHolder1_txtScheduleDate5").value
            console.log(d);
            document.getElementById("ContentPlaceHolder1_hdnScheduleDate5").value = d;
        }
        

        function text_changed_from6() {
            var d = document.getElementById("ContentPlaceHolder1_txtScheduleDate6").value
            console.log(d);
            document.getElementById("ContentPlaceHolder1_hdnScheduleDate6").value = d;
        }
        

        function text_changed_from7() {
            var d = document.getElementById("ContentPlaceHolder1_txtScheduleDate7").value
            console.log(d);
            document.getElementById("ContentPlaceHolder1_hdnScheduleDate7").value = d;
        }
         
        function text_changed_from8() {
            var d = document.getElementById("ContentPlaceHolder1_txtScheduleDate8").value
            console.log(d);
            document.getElementById("ContentPlaceHolder1_hdnScheduleDate8").value = d;
        }
         

        function text_changed_from9() {
            var d = document.getElementById("ContentPlaceHolder1_txtScheduleDate9").value
            console.log(d);
            document.getElementById("ContentPlaceHolder1_hdnScheduleDate9").value = d;
        }
        

        function text_changed_from10() {
            var d = document.getElementById("ContentPlaceHolder1_txtScheduleDate10").value
            console.log(d);
            document.getElementById("ContentPlaceHolder1_hdnScheduleDate10").value = d;
        }

         
        function text_changed_from11() {
            var d = document.getElementById("ContentPlaceHolder1_txtScheduleDate11").value
            console.log(d);
            document.getElementById("ContentPlaceHolder1_hdnScheduleDate11").value = d;
        }
         

        function text_changed_from12() {
            var d = document.getElementById("ContentPlaceHolder1_txtScheduleDate12").value
            console.log(d);
            document.getElementById("ContentPlaceHolder1_hdnScheduleDate12").value = d;
        }
         

        function text_changed_from13() {
            var d = document.getElementById("ContentPlaceHolder1_txtScheduleDate13").value
            console.log(d);
            document.getElementById("ContentPlaceHolder1_hdnScheduleDate13").value = d;
        }
         
        function text_changed_from14() {
            var d = document.getElementById("ContentPlaceHolder1_txtScheduleDate14").value
            console.log(d);
            document.getElementById("ContentPlaceHolder1_hdnScheduleDate14").value = d;
        }


    </script>
    <script type="text/javascript">
        

        function text_changed_from() {
            var d = document.getElementById("ContentPlaceHolder1_txtScheduleDate").value
            console.log(d);
            document.getElementById("ContentPlaceHolder1_hdnScheduleDate").value = d;
        }

        function showLoading() {
            document.getElementById('<%= divFileLoader.ClientID %>').style.display = 'block';
            document.getElementById('<%= lblUploading.ClientID %>').innerHTML = "Uploading. Please Wait ... ";

        }

        
        

        function SMSfileUpload() {
            debugger
            var uploadControl = document.getElementById('<%= FileUpload1.ClientID %>');
            var myfile = uploadControl.value;
            console.log(myfile);
            var Extension = myfile.substring(myfile.lastIndexOf('.') + 1).toLowerCase();
            console.log(Extension);

           // if (Extension != "txt" && Extension != "csv" &&Extension != "xls" &&Extension != "xlsx") {
            if (Extension != "txt" && Extension != "csv" ) {
                alert("File Shoul be txt,csv,xls or png format !!");
                return false;
            }

            if (Extension == "txt") {
                if (uploadControl.files[0].size > 2 * 6291456) {
                    alert("Upload text file of size upto 6 MB only.");
                    return false;
                } else {
                    showLoading();
                    console.log("ret true");
                    return true;
                }
            }
            if (Extension == "csv") {
                if (uploadControl.files[0].size > 20971520) {
                    alert("Upload csv file of size upto 20 MB only."); /*3 * 6291456*/
                    return false;
                } else {
                    showLoading();
                    console.log("ret true");
                    return true;
                }
            }

            if (Extension == "xls" || Extension == "xlsx") {
                if (uploadControl.files[0].size > 5242880) {
                    alert("Upload Excel file of size upto 5 MB only.");
                    return false;
                } else {
                    showLoading();
                    console.log("ret true");
                    return true;
                }
            }
        }

        function SMSfileUpload60() {
             
            var uploadControl = document.getElementById('<%= uImage.ClientID %>');
            var myfile = uploadControl.value;
             
            console.log(myfile);
            var Extension = myfile.substring(myfile.lastIndexOf('.') + 1).toLowerCase();
            console.log(Extension);

            
                if ((Extension != "jpg" && Extension != "png")) {
                    alert("File Shoul be jpg or png format !!");
                    return false;
                }
                else
                {
                    showLoading();
                    console.log("ret true");
                    return true;
            }
            

             
            
           
        }

         function SMSfileUpload61() {
            var uploadControl = document.getElementById('<%= uVideo.ClientID %>');
            var myfile = uploadControl.value;
            console.log(myfile);
            var Extension = myfile.substring(myfile.lastIndexOf('.') + 1).toLowerCase();
            console.log(Extension);

             if ((Extension != "mp4")) {
                    alert("File Shoul be mp4 !!");
                    return false;
                }
                    showLoading();
                    console.log("ret true");
                    return true;
                         
           
        }
        
        function isnumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if ((charCode > 48 && charCode < 57) || charCode == 44 || charCode == 13) {
                return true;
            } else {
                return false;
            }

        }

        function integersOnly(obj) {
            obj.value = obj.value.replace(/[^0-9,\r\n]/g, '');
        }

        function mobnumbcnt() {
            var s = document.getElementById("<%=txtMobNum.ClientID%>").value;
            if (s != '') {
                var str = s;
                var x = 0;
                for (var i = 0; i < str.length; i++) {
                    if (str.charAt(i) == ',') x++;
                }
                for (var i = 0; i < str.length; i++) {
                    if (str.charAt(i) == '\n') x++;
                }

                if (x > 0) {
                    document.getElementById('<%= lblMobileCnt.ClientID %>').innerHTML = (x + 1).toString();
                } else
                    document.getElementById('<%= lblMobileCnt.ClientID %>').innerHTML = "";
                if (s != '' && x == 0)
                    document.getElementById('<%= lblMobileCnt.ClientID %>').innerHTML = "1";
            } else {
                document.getElementById('<%= lblMobileCnt.ClientID %>').innerHTML = "";
            }
        }

       

      

        function getLength1(ln) {
            var i = 0;
            if (ln >= 1) i = 1;
            if (ln > 1024) i = 2;
            if (ln > 2048) i = 3;
            if (ln > 3072) i = 4;
            if (ln > 4096) i = 5;
            if (ln > 5120) i = 6;
            if (ln > 6144) i = 7;
            if (ln > 7168) i = 8;
            if (ln > 8192) i = 9;
            if (ln > 9216) i = 10;
            if (ln > 10240) i = 11;
            if (ln > 11264) i = 12;
            return i;
        }

        function getUniCodeLength1(ln) {
            var i = 0;
            if (ln >= 1) i = 1;
            if (ln > 1024) i = 2;
            if (ln > 3072) i = 3;
            if (ln > 4096) i = 4;
            if (ln > 5120) i = 5;
            if (ln > 6144) i = 6;
            if (ln > 7168) i = 7;
            if (ln > 8192) i = 8;
            if (ln > 9216) i = 9;
            if (ln > 10240) i = 10;
            return i;
        }

        function smscnt() {
            debugger
            var s = document.getElementById("<%=txtMsg.ClientID%>").value;
            var tt = 0;
            //tt = s.split(/\r\n|\r|\n/).length;
            if (tt >= 1) tt = tt - 1;
            if (s != '') {
                var i = 0;
                var ln = s.length + tt;
                for (var k = 0, n = s.length; k < n; k++) {
                    if (s.charAt(k) == "~" || s.charAt(k) == "|" || s.charAt(k) == "{" || s.charAt(k) == "}" || s.charAt(k) == "[" || s.charAt(k) == "]" || s.charAt(k) == "^" || s.charAt(k) == "\\") {
                        ln = ln + 1;
                    }
                }
                <%--var ou = document.getElementById("<%=lblOptOut.ClientID%>").innerText;
                console.log(ou);
                ln = ln + ou.length;--%>

                   i = getLength1(ln);
                <%--document.getElementById('<%= lblUniCode.ClientID %>').innerHTML = "";--%>

                //document.getElementById('<%= lblsmscount.ClientID %>').innerHTML = "No. of Char: " + ln + ". <br />No. of SMS: " + i.toString();
                document.getElementById('<%= lblsmscnt.ClientID %>').innerHTML =  i.toString();
                 var char = ln;
                    var char1 = 1024;
                    var tvat = char % char1;
                if (i <= 1) {
               
                    document.getElementById('<%= lblused.ClientID %>').innerHTML = "" + ln + " / 1024 ";
                 }
                else
                {
                    if(tvat==0)
                        document.getElementById('<%= lblused.ClientID %>').innerHTML = "1024 / 1024 ";
                    else
                        document.getElementById('<%= lblused.ClientID %>').innerHTML = "" + tvat + " / 1024 ";
                    }
                <%--i = getLength(ln);
                document.getElementById('<%= lblUniCode.ClientID %>').innerHTML = "";

                 //document.getElementById('<%= lblsmscnt.ClientID %>').innerHTML = "No. of Char : " + ln + ". <br />No. of RCS : " + i.toString();
                document.getElementById('<%= lblsmscnt.ClientID %>').innerHTML =  i.toString();
                document.getElementById('<%= lblused.ClientID %>').innerHTML = "" + ln - (i.toString() - 1) * 1024 + " / 1024 ";--%>



                var y = 0;
                for (var k = 0, n = s.length; k < n; k++) {
                    if (s.charCodeAt(k) > 126) {
                        y = 1;
                    }
                }
                if (y == 1) {
                    ln = s.length;
                    i = getUniCodeLength1(ln);
                    document.getElementById('<%= lblUniCode.ClientID %>').innerHTML = "UNICODE : YES";
                    //var Used = 0;
                    //Used = i - 1;
                    //Used = Used * 1024;
                    //document.getElementById('<%= lblsmscnt.ClientID %>').innerHTML = "No. of Char : " + ln + ". <br />No. of RCS: " + i.toString();
                    document.getElementById('<%= lblsmscnt.ClientID %>').innerHTML = i.toString();
                    <%--document.getElementById('<%= lblused.ClientID %>').innerHTML = "" + ln - Used + " / 1024 ";--%>

                    var char = ln;
                    var char1 = 1024;
                    var tvat = char % char1;
                if (i <= 1) {
               
                    document.getElementById('<%= lblused.ClientID %>').innerHTML = "" + ln + " / 1024 ";
                 }
                else
                {
                    if(tvat==0)
                        document.getElementById('<%= lblused.ClientID %>').innerHTML = "1024 / 1024 ";
                    else
                        document.getElementById('<%= lblused.ClientID %>').innerHTML = "" + tvat + " / 1024 ";
                    }
                }
            } else
                {
                document.getElementById('<%= lblsmscnt.ClientID %>').innerHTML = "0";
                document.getElementById('<%= lblused.ClientID %>').innerHTML = "0";
              }
        }
         function setCharAt(str, chr, rep) {
            var index = -1;
            index =str.lastIndexOf(chr);
            var len = chr.length;
            if (index > str.length - 1) return str;
            return str.substring(0, index) + rep + str.substring(index
                + len);
        }

        function SetMSG() {
        
}
        function SMStextChange1() {
            debugger
           


 var txt1val;
            var txt2val;
            var txt3val;
            var txt4val;
            var txt5val;
            var txt6val;
            var txt7val;
            var txt8val;
            var txt9val;
             var txt10val;
            var myPrevioew = document.getElementById("<%=hdnTemplateVarText.ClientID%>").value;
            var origText = document.getElementById("<%=vrCount.ClientID%>").value;
         
            if (origText == "1") {
                if (document.getElementById("<%=TextBox1.ClientID%>").value == "") txt1val = '{1}'; else txt1val = document.getElementById("<%=TextBox1.ClientID%>").value;
                var newPreview = setCharAt(myPrevioew, "{1}", txt1val);
                console.log(newPreview);
            }
            else if (origText == "2") {
                if (document.getElementById("<%=TextBox1.ClientID%>").value == "") txt1val = '{1}'; else txt1val = document.getElementById("<%=TextBox1.ClientID%>").value;
                if (document.getElementById("<%=TextBox2.ClientID%>").value == "") txt2val = '{2}'; else txt2val = document.getElementById("<%=TextBox2.ClientID%>").value;
                var newPreview = setCharAt(myPrevioew, "{1}", txt1val);
                var newPreview = setCharAt(newPreview, "{2}", txt2val);
                console.log(newPreview);
            }
            else if (origText == "3") {
                if (document.getElementById("<%=TextBox1.ClientID%>").value == "") txt1val = '{1}'; else txt1val = document.getElementById("<%=TextBox1.ClientID%>").value;
                if (document.getElementById("<%=TextBox2.ClientID%>").value == "") txt2val = '{2}'; else txt2val = document.getElementById("<%=TextBox2.ClientID%>").value;
                if (document.getElementById("<%=TextBox3.ClientID%>").value == "") txt3val = '{3}'; else txt3val = document.getElementById("<%=TextBox3.ClientID%>").value;
                var newPreview = setCharAt(myPrevioew, "{1}", txt1val);
                var newPreview = setCharAt(newPreview, "{2}", txt2val);
                var newPreview = setCharAt(newPreview, "{3}", txt3val);
                console.log(newPreview);
            }
            else if (origText == "4") {
                if (document.getElementById("<%=TextBox1.ClientID%>").value == "") txt1val = '{1}'; else txt1val = document.getElementById("<%=TextBox1.ClientID%>").value;
                if (document.getElementById("<%=TextBox2.ClientID%>").value == "") txt2val = '{2}'; else txt2val = document.getElementById("<%=TextBox2.ClientID%>").value;
                if (document.getElementById("<%=TextBox3.ClientID%>").value == "") txt3val = '{3}'; else txt3val = document.getElementById("<%=TextBox3.ClientID%>").value;
                if (document.getElementById("<%=TextBox4.ClientID%>").value == "") txt4val = '{4}'; else txt4val = document.getElementById("<%=TextBox4.ClientID%>").value;
                var newPreview = setCharAt(myPrevioew, "{1}", txt1val);
                var newPreview = setCharAt(newPreview, "{2}", txt2val);
                var newPreview = setCharAt(newPreview, "{3}", txt3val);
                var newPreview = setCharAt(newPreview, "{4}", txt4val);
                console.log(newPreview);
            }
            else if (origText == "5") {
                if (document.getElementById("<%=TextBox1.ClientID%>").value == "") txt1val = '{1}'; else txt1val = document.getElementById("<%=TextBox1.ClientID%>").value;
                if (document.getElementById("<%=TextBox2.ClientID%>").value == "") txt2val = '{2}'; else txt2val = document.getElementById("<%=TextBox2.ClientID%>").value;
                if (document.getElementById("<%=TextBox3.ClientID%>").value == "") txt3val = '{3}'; else txt3val = document.getElementById("<%=TextBox3.ClientID%>").value;
                if (document.getElementById("<%=TextBox4.ClientID%>").value == "") txt4val = '{4}'; else txt4val = document.getElementById("<%=TextBox4.ClientID%>").value;
                if (document.getElementById("<%=TextBox5.ClientID%>").value == "") txt5val = '{5}'; else txt5val = document.getElementById("<%=TextBox5.ClientID%>").value;
                var newPreview = setCharAt(myPrevioew, "{1}", txt1val);
                var newPreview = setCharAt(newPreview, "{2}", txt2val);
                var newPreview = setCharAt(newPreview, "{3}", txt3val);
                var newPreview = setCharAt(newPreview, "{4}", txt4val);
                var newPreview = setCharAt(newPreview, "{5}", txt5val);
                console.log(newPreview);
            }
            else if (origText <= 6) {
                if (document.getElementById("<%=TextBox1.ClientID%>").value == "") txt1val = '{1}'; else txt1val = document.getElementById("<%=TextBox1.ClientID%>").value;
            if (document.getElementById("<%=TextBox2.ClientID%>").value == "") txt2val = '{2}'; else txt2val = document.getElementById("<%=TextBox2.ClientID%>").value;
            if (document.getElementById("<%=TextBox3.ClientID%>").value == "") txt3val = '{3}'; else txt3val = document.getElementById("<%=TextBox3.ClientID%>").value;
            if (document.getElementById("<%=TextBox4.ClientID%>").value == "") txt4val = '{4}'; else txt4val = document.getElementById("<%=TextBox4.ClientID%>").value;
            if (document.getElementById("<%=TextBox5.ClientID%>").value == "") txt5val = '{5}'; else txt5val = document.getElementById("<%=TextBox5.ClientID%>").value;
            if (document.getElementById("<%=TextBox6.ClientID%>").value == "") txt6val = '{6}'; else txt6val = document.getElementById("<%=TextBox6.ClientID%>").value;
                var newPreview = setCharAt(myPrevioew, "{1}", txt1val);
                var newPreview = setCharAt(newPreview, "{2}", txt2val);
                var newPreview = setCharAt(newPreview, "{3}", txt3val);
                var newPreview = setCharAt(newPreview, "{4}", txt4val);
                var newPreview = setCharAt(newPreview, "{5}", txt5val);
                var newPreview = setCharAt(newPreview, "{6}", txt6val);
                console.log(newPreview);
            }

            else if (origText <= 7) {
                 if (document.getElementById("<%=TextBox1.ClientID%>").value == "") txt1val = '{1}'; else txt1val = document.getElementById("<%=TextBox1.ClientID%>").value;
            if (document.getElementById("<%=TextBox2.ClientID%>").value == "") txt2val = '{2}'; else txt2val = document.getElementById("<%=TextBox2.ClientID%>").value;
            if (document.getElementById("<%=TextBox3.ClientID%>").value == "") txt3val = '{3}'; else txt3val = document.getElementById("<%=TextBox3.ClientID%>").value;
            if (document.getElementById("<%=TextBox4.ClientID%>").value == "") txt4val = '{4}'; else txt4val = document.getElementById("<%=TextBox4.ClientID%>").value;
            if (document.getElementById("<%=TextBox5.ClientID%>").value == "") txt5val = '{5}'; else txt5val = document.getElementById("<%=TextBox5.ClientID%>").value;
            if (document.getElementById("<%=TextBox6.ClientID%>").value == "") txt6val = '{6}'; else txt6val = document.getElementById("<%=TextBox6.ClientID%>").value;
            if (document.getElementById("<%=TextBox7.ClientID%>").value == "") txt7val = '{7}'; else txt7val = document.getElementById("<%=TextBox7.ClientID%>").value;
                var newPreview = setCharAt(myPrevioew, "{1}", txt1val);
                var newPreview = setCharAt(newPreview, "{2}", txt2val);
                var newPreview = setCharAt(newPreview, "{3}", txt3val);
                var newPreview = setCharAt(newPreview, "{4}", txt4val);
                var newPreview = setCharAt(newPreview, "{5}", txt5val);
                var newPreview = setCharAt(newPreview, "{6}", txt6val);
                var newPreview = setCharAt(newPreview, "{7}", txt7val);
                console.log(newPreview);
               
            }

            else if (origText <= 8) {
                    if (document.getElementById("<%=TextBox1.ClientID%>").value == "") txt1val = '{1}'; else txt1val = document.getElementById("<%=TextBox1.ClientID%>").value;
            if (document.getElementById("<%=TextBox2.ClientID%>").value == "") txt2val = '{2}'; else txt2val = document.getElementById("<%=TextBox2.ClientID%>").value;
            if (document.getElementById("<%=TextBox3.ClientID%>").value == "") txt3val = '{3}'; else txt3val = document.getElementById("<%=TextBox3.ClientID%>").value;
            if (document.getElementById("<%=TextBox4.ClientID%>").value == "") txt4val = '{4}'; else txt4val = document.getElementById("<%=TextBox4.ClientID%>").value;
            if (document.getElementById("<%=TextBox5.ClientID%>").value == "") txt5val = '{5}'; else txt5val = document.getElementById("<%=TextBox5.ClientID%>").value;
            if (document.getElementById("<%=TextBox6.ClientID%>").value == "") txt6val = '{6}'; else txt6val = document.getElementById("<%=TextBox6.ClientID%>").value;
            if (document.getElementById("<%=TextBox7.ClientID%>").value == "") txt7val = '{7}'; else txt7val = document.getElementById("<%=TextBox7.ClientID%>").value;
            if (document.getElementById("<%=TextBox8.ClientID%>").value == "") txt8val = '{8}'; else txt8val = document.getElementById("<%=TextBox8.ClientID%>").value;
                 var newPreview = setCharAt(myPrevioew, "{1}", txt1val);
                var newPreview = setCharAt(newPreview, "{2}", txt2val);
                var newPreview = setCharAt(newPreview, "{3}", txt3val);
                var newPreview = setCharAt(newPreview, "{4}", txt4val);
                var newPreview = setCharAt(newPreview, "{5}", txt5val);
                var newPreview = setCharAt(newPreview, "{6}", txt6val);
                var newPreview = setCharAt(newPreview, "{7}", txt7val);
                var newPreview = setCharAt(newPreview, "{8}", txt8val);
                console.log(newPreview);
            }

            else if (origText <= 9) {
                  if (document.getElementById("<%=TextBox1.ClientID%>").value == "") txt1val = '{1}'; else txt1val = document.getElementById("<%=TextBox1.ClientID%>").value;
            if (document.getElementById("<%=TextBox2.ClientID%>").value == "") txt2val = '{2}'; else txt2val = document.getElementById("<%=TextBox2.ClientID%>").value;
            if (document.getElementById("<%=TextBox3.ClientID%>").value == "") txt3val = '{3}'; else txt3val = document.getElementById("<%=TextBox3.ClientID%>").value;
            if (document.getElementById("<%=TextBox4.ClientID%>").value == "") txt4val = '{4}'; else txt4val = document.getElementById("<%=TextBox4.ClientID%>").value;
            if (document.getElementById("<%=TextBox5.ClientID%>").value == "") txt5val = '{5}'; else txt5val = document.getElementById("<%=TextBox5.ClientID%>").value;
            if (document.getElementById("<%=TextBox6.ClientID%>").value == "") txt6val = '{6}'; else txt6val = document.getElementById("<%=TextBox6.ClientID%>").value;
            if (document.getElementById("<%=TextBox7.ClientID%>").value == "") txt7val = '{7}'; else txt7val = document.getElementById("<%=TextBox7.ClientID%>").value;
            if (document.getElementById("<%=TextBox8.ClientID%>").value == "") txt8val = '{8}'; else txt8val = document.getElementById("<%=TextBox8.ClientID%>").value;
            if (document.getElementById("<%=TextBox9.ClientID%>").value == "") txt9val = '{9}'; else txt9val = document.getElementById("<%=TextBox9.ClientID%>").value;
                var newPreview = setCharAt(myPrevioew, "{1}", txt1val);
                var newPreview = setCharAt(newPreview, "{2}", txt2val);
                var newPreview = setCharAt(newPreview, "{3}", txt3val);
                var newPreview = setCharAt(newPreview, "{4}", txt4val);
                var newPreview = setCharAt(newPreview, "{5}", txt5val);
                var newPreview = setCharAt(newPreview, "{6}", txt6val);
                var newPreview = setCharAt(newPreview, "{7}", txt7val);
                var newPreview = setCharAt(newPreview, "{8}", txt8val);
                var newPreview = setCharAt(newPreview, "{9}", txt9val);
                console.log(newPreview);
            }

            else if (origText <= 10) {
                 if (document.getElementById("<%=TextBox1.ClientID%>").value == "") txt1val = '{1}'; else txt1val = document.getElementById("<%=TextBox1.ClientID%>").value;
            if (document.getElementById("<%=TextBox2.ClientID%>").value == "") txt2val = '{2}'; else txt2val = document.getElementById("<%=TextBox2.ClientID%>").value;
            if (document.getElementById("<%=TextBox3.ClientID%>").value == "") txt3val = '{3}'; else txt3val = document.getElementById("<%=TextBox3.ClientID%>").value;
            if (document.getElementById("<%=TextBox4.ClientID%>").value == "") txt4val = '{4}'; else txt4val = document.getElementById("<%=TextBox4.ClientID%>").value;
            if (document.getElementById("<%=TextBox5.ClientID%>").value == "") txt5val = '{5}'; else txt5val = document.getElementById("<%=TextBox5.ClientID%>").value;
            if (document.getElementById("<%=TextBox6.ClientID%>").value == "") txt6val = '{6}'; else txt6val = document.getElementById("<%=TextBox6.ClientID%>").value;
            if (document.getElementById("<%=TextBox7.ClientID%>").value == "") txt7val = '{7}'; else txt7val = document.getElementById("<%=TextBox7.ClientID%>").value;
            if (document.getElementById("<%=TextBox8.ClientID%>").value == "") txt8val = '{8}'; else txt8val = document.getElementById("<%=TextBox8.ClientID%>").value;
            if (document.getElementById("<%=TextBox9.ClientID%>").value == "") txt9val = '{9}'; else txt9val = document.getElementById("<%=TextBox9.ClientID%>").value;
            if (document.getElementById("<%=TextBox10.ClientID%>").value == "") txt10val = '{10}'; else txt10val = document.getElementById("<%=TextBox10.ClientID%>").value
                  var newPreview = setCharAt(myPrevioew, "{1}", txt1val);
                var newPreview = setCharAt(newPreview, "{2}", txt2val);
                var newPreview = setCharAt(newPreview, "{3}", txt3val);
                var newPreview = setCharAt(newPreview, "{4}", txt4val);
                var newPreview = setCharAt(newPreview, "{5}", txt5val);
                var newPreview = setCharAt(newPreview, "{6}", txt6val);
                var newPreview = setCharAt(newPreview, "{7}", txt7val);
                var newPreview = setCharAt(newPreview, "{8}", txt8val);
                var newPreview = setCharAt(newPreview, "{9}", txt9val);
                var newPreview = setCharAt(newPreview, "{10}", txt10val);
                console.log(newPreview);
            }

            document.getElementById("<%=txtsms.ClientID%>").value = newPreview;
            hdnTemplateVarText = newPreview;
            smscnt1();
            <%--if (document.getElementById("<%=TextBox1.ClientID%>").value == "") {
                document.getElementById("<%=lblTempSMS.ClientID%>").innerHTML = newPreview;
            }--%>
        }
          function getLength(ln) {
            var i = 0;
            if (ln >= 1) i = 1;
            if (ln > 160) i = 2;
            if (ln > 306) i = 3;
            if (ln > 459) i = 4;
            if (ln > 612) i = 5;
            if (ln > 765) i = 6;
            if (ln > 918) i = 7;
            if (ln > 1071) i = 8;
            if (ln > 1224) i = 9;
            if (ln > 1377) i = 10;
            if (ln > 1530) i = 11;
            if (ln > 1683) i = 12;
            return i;
        }

        function getUniCodeLength(ln) {
            var i = 0;
            if (ln >= 1) i = 1;
            if (ln > 70) i = 2;
            if (ln > 134) i = 3;
            if (ln > 201) i = 4;
            if (ln > 268) i = 5;
            if (ln > 335) i = 6;
            if (ln > 402) i = 7;
            if (ln > 469) i = 8;
            if (ln > 536) i = 9;
            if (ln > 603) i = 10;
            return i;
        }

        function smscnt1() {
            
            var s = document.getElementById("<%=txtsms.ClientID%>").value;
            var tt = 0;
            //tt = s.split(/\r\n|\r|\n/).length;
            if (tt >= 1) tt = tt - 1;
            if (s != '') {
                var i = 0;
                var ln = s.length + tt;
                for (var k = 0, n = s.length; k < n; k++) {
                    if (s.charAt(k) == "~" || s.charAt(k) == "|" || s.charAt(k) == "{" || s.charAt(k) == "}" || s.charAt(k) == "[" || s.charAt(k) == "]" || s.charAt(k) == "^" || s.charAt(k) == "\\") {
                        ln = ln + 1;
                    }
                }
               <%-- var ou = document.getElementById("<%=lblOptOut.ClientID%>").innerText;
                console.log(ou);
                ln = ln + ou.length;--%>

                
                
                i = getLength(ln);
                <%--document.getElementById('<%= lblUniCode.ClientID %>').innerHTML = "";--%>

                //document.getElementById('<%= lblsmscount.ClientID %>').innerHTML = "No. of Char: " + ln + ". <br />No. of SMS: " + i.toString();
                document.getElementById('<%= lblsmscount.ClientID %>').innerHTML =  i.toString();
                 var char = ln;
                    var char1 = 153;
                    var tvat = char % char1;
                if (i <= 1) {
               
                    document.getElementById('<%= lblsmdused.ClientID %>').innerHTML = "" + ln + " / 160 ";
                 }
                else
                {
                    if(tvat==0)
                        document.getElementById('<%= lblsmdused.ClientID %>').innerHTML = "153 / 153 ";
                    else
                        document.getElementById('<%= lblsmdused.ClientID %>').innerHTML = "" + tvat + " / 153 ";
                    }
                var y = 0;
                for (var k = 0, n = s.length; k < n; k++) {
                    if (s.charCodeAt(k) > 126) {
                        y = 1;
                    }
                }
                if (y == 1) {
                    ln = s.length;
                    i = getUniCodeLength(ln);
                    <%--document.getElementById('<%= lblUniCode.ClientID %>').innerHTML = "UNICODE : YES";--%>

                   // document.getElementById('<%= lblsmscount.ClientID %>').innerHTML = "No. of Char: " + ln + ". <br />No. of SMS: " + i.toString();
                     document.getElementById('<%= lblsmscount.ClientID %>').innerHTML =  i.toString();
                    var char = ln;
                    var char1 = 63;
                    var tvat = char % char1;
                    if (i <= 1) {
               
                    document.getElementById('<%= lblsmdused.ClientID %>').innerHTML = "" + ln + " / 70 ";
                 }
                else
                {
                    if(tvat==0)
                        document.getElementById('<%= lblsmdused.ClientID %>').innerHTML = "63 / 63 ";
                    else
                        document.getElementById('<%= lblsmdused.ClientID %>').innerHTML = "" + tvat + " / 63 ";
                    }
                    
                }
            } else
                document.getElementById('<%= lblsmscount.ClientID %>').innerHTML = "";
        }
    </script>
     <link href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/css/select2.min.css" rel="stylesheet" />
    <!-- jQuery -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
    <!-- Select2 -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.6-rc.0/js/select2.min.js"></script>
   <%-- <script>
        $("#ddlTempID").select2({
            allowClear: true
        });

       
    </script>--%>
</asp:Content>
