<%@ Page Title="" Language="C#" MasterPageFile="~/Site2.Master" AutoEventWireup="true" CodeBehind="SMSLinkToWABA.aspx.cs" Inherits="eMIMPanel.SMSLinkToWABA" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </asp:ToolkitScriptManager>
    <main>
        <div class="container-fluid">
            <nav aria-label="breadcrumb" class="my-3">
                <ol class="breadcrumb breadcrumb-info">
                    <li class="breadcrumb-item"><a href="#">Home</a></li>
                    <li class="breadcrumb-item"><a href="#">Link WABA Account</a></li>
                </ol>
            </nav>
            <!-- Content Row -->
            <div class="row">
                <div class="col-12">
                    <div class="card mb-4 bg-primary border-light shadow-soft">
                        <div class="card-body pt-0 pt-lg-4">
                            <form>
                                <div class="form-group row" runat="server" id="lblMessage" visible="false">
                                    <div class="col-sm-8">
                                        <asp:Label ID="lblMsg" runat="server" CssClass="font-weight-bold">Your WhatsApp Account is not linked to Linkext.
                                            <asp:LinkButton ID="lnkbtnLink" runat="server" OnClientClick="return confirm('Are you sure you want to proceed?');" OnClick="lnkbtnLink_Click"
                                                CssClass="font-weight-bold" Style="text-decoration: underline; color: blue">Click here</asp:LinkButton>
                                            to Link
                                        </asp:Label>
                                    </div>
                                </div>
                            </form>
                        </div>
                    </div>

                    <div class="card mb-4 bg-primary border-light shadow-soft" runat="server" id="divShortURL" visible="false">
                        <div class="card-body pt-0 pt-lg-4">
                            <div class="form-group row">
                                <div class="col-sm-6">
                                    <div class="custom-control custom-radio custom-control-inline pl-2">
                                        <asp:RadioButton class="mr-2" ID="rdbSelectShortURL" runat="server" Checked="true" GroupName="mobile" AutoPostBack="true" OnCheckedChanged="rdbUpload_CheckedChanged" />
                                        <label>Select Short URL</label>
                                    </div>
                                    <div class="custom-control custom-radio custom-control-inline pl-2">
                                        <asp:RadioButton class="mr-2" ID="rdbCreateShortURL" runat="server" GroupName="mobile" AutoPostBack="true" OnCheckedChanged="rdbUpload_CheckedChanged" />
                                        <label>Create Short URL</label>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group row" runat="server" id="divSelectShortURL">
                                <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">Short URL :</label>
                                <div class="col-sm-5">
                                    <asp:DropDownList ID="ddlURL" runat="server" AutoPostBack="true" class="drop-select form-control mb-3"></asp:DropDownList>
                                </div>
                            </div>
                            <div runat="server" id="divCreateShortURL" visible="false">
                                <div class="form-group row">
                                    <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">Enter Long URL :</label>
                                    <div class="col-sm-5">
                                        <asp:TextBox ID="txtLongURL" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group row">
                                    <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">Short URL Name :</label>
                                    <div class="col-sm-2">
                                        <asp:TextBox ID="txtShortURLname" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-3">
                                        <asp:LinkButton ID="lnkCreateShortURL" runat="server" OnClick="lnkCreateShortURL_Click" OnClientClick="return ConfirmBal();"
                                            class="btn btn-primary text-secondary font-weight-bold btn-block"> 
                                                        <span>Create Short URL</span>
                                        </asp:LinkButton>
                                    </div>
                                </div>
                                <div class="form-group row">
                                    <label for="inputEmail3" class="col-sm-2 col-form-label font-weight-bold">Short URL :</label>
                                    <div class="col-sm-5">
                                        <asp:Label ID="lblShortURL" runat="server"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <%-----------------Whatsapp Template--------------%>
                    <div class="card mb-4 bg-primary border-light shadow-soft" runat="server" id="divWABAtemplate" visible="false">
                        <div class="card-body pt-0 pt-lg-4">
                            <div class="form-group row">
                                <%--<div class="col-sm-2"></div>--%>
                                <div class="col-sm-5">
                                    <label class="form-check-label font-weight-bold" for="gridCheck2">Select WABA Template to be Triggered on Link Click</label>
                                </div>
                            </div>

                            <div id="divuploadImage" runat="server">
                                <div class="  row t-pt-10 px-4 mt-1">
                                    <div class="col-md-4 px-4">
                                        <div class="mb-3" style="pointer-events: all;">
                                            <asp:RadioButtonList class="form-radio" ID="rbFileOrURL"
                                                OnSelectedIndexChanged="rbFileOrURL_SelectedIndexChanged"
                                                AutoPostBack="true" RepeatDirection="Horizontal"
                                                runat="server">
                                                <asp:ListItem class="form-check-label sm-text"
                                                    Selected="True" Value="0">&nbsp;&nbsp;Upload File &nbsp;&nbsp; &nbsp;
                                                </asp:ListItem>
                                                <asp:ListItem class="form-check-label sm-text" Value="1">
                                                                &nbsp;Enter URL </asp:ListItem>
                                            </asp:RadioButtonList>
                                        </div>
                                    </div>
                                    <div class="col-md-8" id="dvMediafile" runat="server">
                                        <div class="input-group mb-3">
                                            <label for="inputEmail31" class="input-group-text sm-text">
                                                Upload Media File&nbsp;</label>
                                            <asp:FileUpload ID="uImage"
                                                accept=".mp4,.3gpp,.pdf,.jpg,.jpeg,.png,.aac,.amr,.mp3,.mp4,.opus"
                                                runat="server" class="form-control sm-text"
                                                ClientIDMode="Static"
                                                onchange="if( SMSfileUpload60() ) { console.log('formsubmit'); this.form.submit(); }" />
                                            <small id="textHelp2"
                                                class="form-text text-muted mb-3 mb-lg-0">&nbsp; Upload Limit
                                                        <strong id="lblfilesize" runat="server">5 MB.</strong>
                                            </small>
                                        </div>
                                    </div>
                                    <div class="col-md-4" id="divmediafile" runat="server" visible="false">
                                        <div>
                                            <asp:Label ID="lblImage" class="form-check-label sm-text"
                                                runat="server"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="col-md-6" id="dventerurl" runat="server" visible="false">
                                        <div class="input-group mb-3">
                                            <span class="input-group-text sm-text">Enter File URL </span>
                                            <asp:TextBox class="form-control sm-text"
                                                placeholder="Enter File URL" ID="txtFileUrl" runat="server"
                                                ToolTip="File URL" onkeyup="return changeImage();" />&nbsp;
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2"
                                                        runat="server" SetFocusOnError="true"
                                                        ControlToValidate="txtFileUrl"
                                                        ErrorMessage="Please Enter Valid Url !!" Visible="True"
                                                        Font-Size="Smaller" ForeColor="Red"
                                                        ValidationExpression="http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&amp;=]*)?"
                                                        Display="Dynamic"></asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                    <div class="col-md-5" id="dvcaption" runat="server" visible="false">
                                        <div class="input-group mb-3">
                                            <span class="input-group-text sm-text">Caption </span>
                                            <asp:TextBox class="form-control sm-text" placeholder="Caption"
                                                ID="txtCaption" runat="server" ToolTip="Caption" onkeyup="onchangecaption();" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row px-4 py-2">
                                <div class="col-md-6">
                                    <div class="row py-3" id="divTimeout" runat="server" visible="false">
                                        <div class="col-md-6 " id="divexp" runat="server" visible="false">
                                            <label>Expiration Time</label>
                                            <asp:TextBox ID="txtexptime" runat="server" placeholder="Offer Time Out in Mili Second"
                                                MaxLength="50" class="form-control sm-text" type="datetime-local" oninput="bindexp()"></asp:TextBox>
                                        </div>
                                        <div class="col-md-6" id="divoofrcode" runat="server" visible="false">
                                            <label>Offer Code</label>
                                            <asp:TextBox ID="txtOfferCode" runat="server" placeholder="Offer Code"
                                                MaxLength="15" class="form-control sm-text" oninput="bindofc()"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div id="divTempId" runat="server">
                                                <div class="col-md-12 ">
                                                    <div class="form-group row" runat="server" id="div8" style="pointer-events: all;">
                                                        <label for="inputEmail3" class="col-sm-4 col-form-label font-weight-bold">Template ID :</label>
                                                        <div class="col-sm-8">
                                                            <asp:DropDownList ID="ddlTempID" runat="server" OnSelectedIndexChanged="ddlTemplate_SelectedIndexChanged" AutoPostBack="true" class="drop-select form-control mb-3"></asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-12" id="divSendQR" runat="server" visible="false">
                                            <div class="col-md-12 ">
                                                <div class="input-group mb-3">
                                                    <asp:CheckBox ID="ChkSendQR" AutoPostBack="true" runat="server" />
                                                    <label class="col-sm-4 col-form-label font-weight-bold">
                                                        Send QR</label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-12" id="divCatalogProductId" runat="server" visible="false">
                                            <div class="input-group mb-3">
                                                <label class="col-sm-4 col-form-label font-weight-bold">Product ID</label>
                                                <asp:TextBox ID="txtCatalogProductId" runat="server"
                                                    placeholder="Catalog Product Id" class="form-control sm-text"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-12 text-center mx-auto">
                                            <div id="divsmstext" runat="server" class=" ">
                                                <div id="divemptydiv" runat="server">
                                                </div>
                                                <div id="divtxt" runat="server">
                                                    <div>
                                                        <asp:HiddenField ID="hfVar1" Value="{Var1}" runat="server" />
                                                        <asp:HiddenField ID="hfVar2" Value="{Var2}" runat="server" />
                                                        <asp:HiddenField ID="hfVar3" Value="{Var3}" runat="server" />
                                                        <asp:HiddenField ID="hfVar4" Value="{Var4}" runat="server" />
                                                        <asp:HiddenField ID="hfVar5" Value="{Var5}" runat="server" />
                                                        <asp:HiddenField ID="hfVar6" Value="{Var6}" runat="server" />
                                                        <asp:HiddenField ID="hfVar7" Value="{Var7}" runat="server" />
                                                        <asp:HiddenField ID="hfVar8" Value="{Var8}" runat="server" />
                                                        <asp:HiddenField ID="hfVar9" Value="{Var9}" runat="server" />
                                                        <asp:HiddenField ID="hfVar10" Value="{Var10}" runat="server" />
                                                        <asp:HiddenField ID="hdnLto" Value="HDN" runat="server" />

                                                        <asp:HiddenField ID="hdnTemplateVarText" runat="server" Value="" />
                                                        <asp:HiddenField ID="HiddenField1" runat="server" Value="" />
                                                        <asp:HiddenField ID="HiddenField2" runat="server" Value="" />
                                                        <asp:HiddenField ID="HiddenField3" runat="server" Value="" />
                                                        <asp:HiddenField ID="HiddenField4" runat="server" Value="" />
                                                        <asp:HiddenField ID="HiddenField5" runat="server" Value="" />
                                                        <asp:HiddenField ID="HiddenField6" runat="server" Value="" />
                                                        <asp:HiddenField ID="HiddenField7" runat="server" Value="" />
                                                        <asp:HiddenField ID="HiddenField8" runat="server" Value="" />
                                                        <asp:HiddenField ID="HiddenField9" runat="server" Value="" />
                                                        <asp:HiddenField ID="HiddenField10" runat="server" Value="" />

                                                        <asp:HiddenField ID="vrCount" runat="server" />
                                                        <asp:HiddenField ID="vrCountCarousel1" runat="server" />
                                                        <asp:HiddenField ID="vrCountCarousel2" runat="server" />
                                                        <asp:HiddenField ID="vrCountCarousel3" runat="server" />
                                                        <asp:HiddenField ID="vrCountCarousel4" runat="server" />
                                                        <asp:HiddenField ID="vrCountCarousel5" runat="server" />
                                                        <asp:HiddenField ID="vrCountCarousel6" runat="server" />
                                                        <asp:HiddenField ID="vrCountCarousel7" runat="server" />
                                                        <asp:HiddenField ID="vrCountCarousel8" runat="server" />
                                                        <asp:HiddenField ID="vrCountCarousel9" runat="server" />
                                                        <asp:HiddenField ID="vrCountCarousel10" runat="server" />

                                                        <asp:TextBox ID="TextBox1" runat="server" Visible="false"
                                                            placeholder="{Var1}" onkeyup="SMStextChange1(); return true;"
                                                            class="form-control sm-text"></asp:TextBox>

                                                        <asp:TextBox ID="TextBox2" runat="server" Visible="false"
                                                            placeholder="{Var2}" onkeyup="SMStextChange1(); return true;"
                                                            class="form-control sm-text"></asp:TextBox>

                                                        <asp:TextBox ID="TextBox3" runat="server" Visible="false"
                                                            placeholder="{Var3}" onkeyup="SMStextChange1(); return true;"
                                                            class="form-control sm-text"></asp:TextBox>

                                                        <asp:TextBox ID="TextBox4" runat="server" Visible="false"
                                                            placeholder="{Var4}" onkeyup="SMStextChange1(); return true;"
                                                            class="form-control sm-text"></asp:TextBox>

                                                        <asp:TextBox ID="TextBox5" runat="server" Visible="false"
                                                            placeholder="{Var5}" onkeyup="SMStextChange1(); return true;"
                                                            class="form-control sm-text"></asp:TextBox>

                                                        <asp:TextBox ID="TextBox6" runat="server" Visible="false"
                                                            placeholder="{Var6}" onkeyup="SMStextChange1(); return true;"
                                                            class="form-control sm-text"></asp:TextBox>

                                                        <asp:TextBox ID="TextBox7" runat="server" Visible="false"
                                                            placeholder="{Var7}" onkeyup="SMStextChange1(); return true;"
                                                            class="form-control sm-text"></asp:TextBox>

                                                        <asp:TextBox ID="TextBox8" runat="server" Visible="false"
                                                            placeholder="{Var8}" onkeyup="SMStextChange1(); return true;"
                                                            class="form-control sm-text"></asp:TextBox>

                                                        <asp:TextBox ID="TextBox9" runat="server" Visible="false"
                                                            placeholder="{Var9}" onkeyup="SMStextChange1(); return true;"
                                                            class="form-control sm-text"></asp:TextBox>

                                                        <asp:TextBox ID="TextBox10" runat="server"
                                                            Visible="false" placeholder="{Var10}"
                                                            onkeyup="SMStextChange1(); return true;"
                                                            class="form-control sm-text"></asp:TextBox>
                                                    </div>
                                                    <br />

                                                    <div id="divCardMain" runat="server" visible="false">
                                                        <div id="CarouselControls" class="carousel slide" data-ride="carousel" data-interval="false" style="overflow: hidden">
                                                            <div class="carousel-inner">
                                                                <div id="divCard1" runat="server" style="border: dotted;" visible="false" class="carousel-item active">
                                                                    <h5 style="padding-bottom: 20px;">Card 1</h5>
                                                                    <div class="input-group mb-3">
                                                                        <label for="inputEmail31" class="input-group-text sm-text">
                                                                            Upload File&nbsp;</label>
                                                                        <asp:FileUpload ID="fileuploadCarouselMedia1" accept=".mp4,.jpg,.jpeg,.png,"
                                                                            runat="server" class="form-control sm-text" onchange="fileUpload(1);" ClientIDMode="Static" />
                                                                    </div>
                                                                    <asp:HiddenField ID="HiddenFieldCard11" Value="{Var1}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard12" Value="{Var2}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard13" Value="{Var3}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard14" Value="{Var4}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard15" Value="{Var5}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard16" Value="{Var6}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard17" Value="{Var7}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard18" Value="{Var8}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard19" Value="{Var9}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard110" Value="{Var10}" runat="server" />

                                                                    <asp:TextBox ID="txtVarCard11" runat="server" Visible="false"
                                                                        placeholder="{Var1}" onkeyup="SMStextChange1(11); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard12" runat="server" Visible="false"
                                                                        placeholder="{Var2}" onkeyup="SMStextChange1(12); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard13" runat="server" Visible="false"
                                                                        placeholder="{Var3}" onkeyup="SMStextChange1(13); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard14" runat="server" Visible="false"
                                                                        placeholder="{Var4}" onkeyup="SMStextChange1(14); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard15" runat="server" Visible="false"
                                                                        placeholder="{Var5}" onkeyup="SMStextChange1(15); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard16" runat="server" Visible="false"
                                                                        placeholder="{Var6}" onkeyup="SMStextChange1(16); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard17" runat="server" Visible="false"
                                                                        placeholder="{Var7}" onkeyup="SMStextChange1(17); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard18" runat="server" Visible="false"
                                                                        placeholder="{Var8}" onkeyup="SMStextChange1(18); return true;"
                                                                        class="form-control sm-text"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard19" runat="server" Visible="false"
                                                                        placeholder="{Var9}" onkeyup="SMStextChange1(19); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard110" runat="server"
                                                                        Visible="false" placeholder="{Var10}"
                                                                        onkeyup="SMStextChange1(110); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>
                                                                </div>
                                                                <div id="divCard2" runat="server" style="border: dotted;" visible="false" class="carousel-item">
                                                                    <h5 style="padding-bottom: 20px;">Card 2</h5>
                                                                    <div class="input-group mb-3">
                                                                        <label for="inputEmail31" class="input-group-text sm-text">
                                                                            Upload File&nbsp;</label>
                                                                        <asp:FileUpload ID="fileuploadCarouselMedia2" accept=".mp4,.jpg,.jpeg,.png,"
                                                                            runat="server" class="form-control sm-text" onchange="fileUpload(2);" ClientIDMode="Static" />
                                                                    </div>
                                                                    <asp:HiddenField ID="HiddenFieldCard21" Value="{Var1}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard22" Value="{Var2}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard23" Value="{Var3}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard24" Value="{Var4}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard25" Value="{Var5}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard26" Value="{Var6}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard27" Value="{Var7}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard28" Value="{Var8}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard29" Value="{Var9}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard210" Value="{Var10}" runat="server" />

                                                                    <asp:TextBox ID="txtVarCard21" runat="server" Visible="false"
                                                                        placeholder="{Var1}" onkeyup="SMStextChange1(21); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard22" runat="server" Visible="false"
                                                                        placeholder="{Var2}" onkeyup="SMStextChange1(22); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard23" runat="server" Visible="false"
                                                                        placeholder="{Var3}" onkeyup="SMStextChange1(23); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard24" runat="server" Visible="false"
                                                                        placeholder="{Var4}" onkeyup="SMStextChange1(24); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard25" runat="server" Visible="false"
                                                                        placeholder="{Var5}" onkeyup="SMStextChange1(25); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard26" runat="server" Visible="false"
                                                                        placeholder="{Var6}" onkeyup="SMStextChange1(26); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard27" runat="server" Visible="false"
                                                                        placeholder="{Var7}" onkeyup="SMStextChange1(27); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard28" runat="server" Visible="false"
                                                                        placeholder="{Var8}" onkeyup="SMStextChange1(28); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard29" runat="server" Visible="false"
                                                                        placeholder="{Var9}" onkeyup="SMStextChange1(29); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard210" runat="server"
                                                                        Visible="false" placeholder="{Var10}"
                                                                        onkeyup="SMStextChange1(210); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>
                                                                </div>
                                                                <div id="divCard3" runat="server" style="border: dotted;" visible="false" class="carousel-item">
                                                                    <h5 style="padding-bottom: 20px;">Card 3</h5>
                                                                    <div class="input-group mb-3">
                                                                        <label for="inputEmail31" class="input-group-text sm-text">
                                                                            Upload File&nbsp;</label>
                                                                        <asp:FileUpload ID="fileuploadCarouselMedia3" accept=".mp4,.jpg,.jpeg,.png,"
                                                                            runat="server" class="form-control sm-text" onchange="fileUpload(3);" ClientIDMode="Static" />
                                                                    </div>
                                                                    <asp:HiddenField ID="HiddenFieldCard31" Value="{Var1}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard32" Value="{Var2}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard33" Value="{Var3}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard34" Value="{Var4}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard35" Value="{Var5}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard36" Value="{Var6}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard37" Value="{Var7}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard38" Value="{Var8}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard39" Value="{Var9}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard310" Value="{Var10}" runat="server" />

                                                                    <asp:TextBox ID="txtVarCard31" runat="server" Visible="false"
                                                                        placeholder="{Var1}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard32" runat="server" Visible="false"
                                                                        placeholder="{Var2}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard33" runat="server" Visible="false"
                                                                        placeholder="{Varck3}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard34" runat="server" Visible="false"
                                                                        placeholder="{Var4}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard35" runat="server" Visible="false"
                                                                        placeholder="{Var5}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard36" runat="server" Visible="false"
                                                                        placeholder="{Var6}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard37" runat="server" Visible="false"
                                                                        placeholder="{Var7}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard38" runat="server" Visible="false"
                                                                        placeholder="{Var8}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard39" runat="server" Visible="false"
                                                                        placeholder="{Var9}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard310" runat="server"
                                                                        Visible="false" placeholder="{Var10}"
                                                                        onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>
                                                                </div>
                                                                <div id="divCard4" runat="server" style="border: dotted;" visible="false" class="carousel-item">
                                                                    <h5 style="padding-bottom: 20px;">Card 4</h5>
                                                                    <div class="input-group mb-3">
                                                                        <label for="inputEmail31" class="input-group-text sm-text">
                                                                            Upload File&nbsp;</label>
                                                                        <asp:FileUpload ID="fileuploadCarouselMedia4" accept=".mp4,.jpg,.jpeg,.png,"
                                                                            runat="server" class="form-control sm-text" onchange="fileUpload(4);" ClientIDMode="Static" />
                                                                    </div>
                                                                    <asp:HiddenField ID="HiddenFieldCard41" Value="{Var1}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard42" Value="{Var2}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard43" Value="{Var3}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard44" Value="{Var4}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard45" Value="{Var5}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard46" Value="{Var6}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard47" Value="{Var7}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard48" Value="{Var8}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard49" Value="{Var9}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard410" Value="{Var10}" runat="server" />

                                                                    <asp:TextBox ID="txtVarCard41" runat="server" Visible="false"
                                                                        placeholder="{Var1}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard42" runat="server" Visible="false"
                                                                        placeholder="{Var2}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard43" runat="server" Visible="false"
                                                                        placeholder="{Var3}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard44" runat="server" Visible="false"
                                                                        placeholder="{Var4}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard45" runat="server" Visible="false"
                                                                        placeholder="{Var5}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard46" runat="server" Visible="false"
                                                                        placeholder="{Var6}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard47" runat="server" Visible="false"
                                                                        placeholder="{Var7}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard48" runat="server" Visible="false"
                                                                        placeholder="{Var8}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard49" runat="server" Visible="false"
                                                                        placeholder="{Var9}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard410" runat="server"
                                                                        Visible="false" placeholder="{Var10}"
                                                                        onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>
                                                                </div>
                                                                <div id="divCard5" runat="server" style="border: dotted;" visible="false" class="carousel-item">
                                                                    <h5 style="padding-bottom: 20px;">Card 5</h5>
                                                                    <div class="input-group mb-3">
                                                                        <label for="inputEmail31" class="input-group-text sm-text">
                                                                            Upload File&nbsp;</label>
                                                                        <asp:FileUpload ID="fileuploadCarouselMedia5" accept=".mp4,.jpg,.jpeg,.png,"
                                                                            runat="server" class="form-control sm-text" ClientIDMode="Static" onchange="fileUpload(5);" />
                                                                    </div>
                                                                    <asp:HiddenField ID="HiddenFieldCard51" Value="{Var1}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard52" Value="{Var2}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard53" Value="{Var3}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard54" Value="{Var4}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard55" Value="{Var5}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard56" Value="{Var6}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard57" Value="{Var7}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard58" Value="{Var8}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard59" Value="{Var9}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard510" Value="{Var10}" runat="server" />

                                                                    <asp:TextBox ID="txtVarCard51" runat="server" Visible="false"
                                                                        placeholder="{Var1}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard52" runat="server" Visible="false"
                                                                        placeholder="{Var2}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard53" runat="server" Visible="false"
                                                                        placeholder="{Var3}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard54" runat="server" Visible="false"
                                                                        placeholder="{Var4}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard55" runat="server" Visible="false"
                                                                        placeholder="{Var5}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard56" runat="server" Visible="false"
                                                                        placeholder="{Var6}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard57" runat="server" Visible="false"
                                                                        placeholder="{Var7}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard58" runat="server" Visible="false"
                                                                        placeholder="{Var8}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard59" runat="server" Visible="false"
                                                                        placeholder="{Var9}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard510" runat="server"
                                                                        Visible="false" placeholder="{Var10}"
                                                                        onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>
                                                                </div>
                                                                <div id="divCard6" runat="server" style="border: dotted;" visible="false" class="carousel-item">
                                                                    <h5 style="padding-bottom: 20px;">Card 6</h5>
                                                                    <div class="input-group mb-3">
                                                                        <label for="inputEmail31" class="input-group-text sm-text">
                                                                            Upload File&nbsp;</label>
                                                                        <asp:FileUpload ID="fileuploadCarouselMedia6" accept=".mp4,.jpg,.jpeg,.png,"
                                                                            runat="server" class="form-control sm-text" ClientIDMode="Static" onchange="fileUpload(6);" />
                                                                    </div>
                                                                    <asp:HiddenField ID="HiddenFieldCard61" Value="{Var1}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard62" Value="{Var2}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard63" Value="{Var3}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard64" Value="{Var4}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard65" Value="{Var5}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard66" Value="{Var6}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard67" Value="{Var7}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard68" Value="{Var8}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard69" Value="{Var9}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard610" Value="{Var10}" runat="server" />

                                                                    <asp:TextBox ID="txtVarCard61" runat="server" Visible="false"
                                                                        placeholder="{Var1}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard62" runat="server" Visible="false"
                                                                        placeholder="{Var2}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard63" runat="server" Visible="false"
                                                                        placeholder="{Var3}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard64" runat="server" Visible="false"
                                                                        placeholder="{Var4}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard65" runat="server" Visible="false"
                                                                        placeholder="{Var5}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard66" runat="server" Visible="false"
                                                                        placeholder="{Var6}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard67" runat="server" Visible="false"
                                                                        placeholder="{Var7}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard68" runat="server" Visible="false"
                                                                        placeholder="{Var8}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard69" runat="server" Visible="false"
                                                                        placeholder="{Var9}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard610" runat="server"
                                                                        Visible="false" placeholder="{Var10}"
                                                                        onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>
                                                                </div>
                                                                <div id="divCard7" runat="server" style="border: dotted;" visible="false" class="carousel-item">
                                                                    <h5 style="padding-bottom: 20px;">Card 7</h5>
                                                                    <div class="input-group mb-3">
                                                                        <label for="inputEmail31" class="input-group-text sm-text">
                                                                            Upload File&nbsp;</label>
                                                                        <asp:FileUpload ID="fileuploadCarouselMedia7" accept=".mp4,.jpg,.jpeg,.png,"
                                                                            runat="server" class="form-control sm-text" ClientIDMode="Static" onchange="fileUpload(7);" />
                                                                    </div>
                                                                    <asp:HiddenField ID="HiddenFieldCard71" Value="{Var1}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard72" Value="{Var2}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard73" Value="{Var3}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard74" Value="{Var4}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard75" Value="{Var5}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard76" Value="{Var6}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard77" Value="{Var7}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard78" Value="{Var8}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard79" Value="{Var9}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard710" Value="{Var10}" runat="server" />

                                                                    <asp:TextBox ID="txtVarCard71" runat="server" Visible="false"
                                                                        placeholder="{Var1}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard72" runat="server" Visible="false"
                                                                        placeholder="{Var2}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard73" runat="server" Visible="false"
                                                                        placeholder="{Var3}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard74" runat="server" Visible="false"
                                                                        placeholder="{Var4}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard75" runat="server" Visible="false"
                                                                        placeholder="{Var5}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard76" runat="server" Visible="false"
                                                                        placeholder="{Var6}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard77" runat="server" Visible="false"
                                                                        placeholder="{Var7}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard78" runat="server" Visible="false"
                                                                        placeholder="{Var8}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard79" runat="server" Visible="false"
                                                                        placeholder="{Var9}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard710" runat="server"
                                                                        Visible="false" placeholder="{Var10}"
                                                                        onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>
                                                                </div>
                                                                <div id="divCard8" runat="server" style="border: dotted;" visible="false" class="carousel-item">
                                                                    <h5 style="padding-bottom: 20px;">Card 8</h5>
                                                                    <div class="input-group mb-3">
                                                                        <label for="inputEmail31" class="input-group-text sm-text">
                                                                            Upload File&nbsp;</label>
                                                                        <asp:FileUpload ID="fileuploadCarouselMedia8" accept=".mp4,.jpg,.jpeg,.png,"
                                                                            runat="server" class="form-control sm-text" ClientIDMode="Static" onchange="fileUpload(8);" />
                                                                    </div>
                                                                    <asp:HiddenField ID="HiddenFieldCard81" Value="{Var1}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard82" Value="{Var2}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard83" Value="{Var3}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard84" Value="{Var4}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard85" Value="{Var5}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard86" Value="{Var6}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard87" Value="{Var7}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard88" Value="{Var8}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard89" Value="{Var9}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard810" Value="{Var10}" runat="server" />

                                                                    <asp:TextBox ID="txtVarCard81" runat="server" Visible="false"
                                                                        placeholder="{Var1}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard82" runat="server" Visible="false"
                                                                        placeholder="{Var2}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard83" runat="server" Visible="false"
                                                                        placeholder="{Var3}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard84" runat="server" Visible="false"
                                                                        placeholder="{Var4}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard85" runat="server" Visible="false"
                                                                        placeholder="{Var5}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard86" runat="server" Visible="false"
                                                                        placeholder="{Var6}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard87" runat="server" Visible="false"
                                                                        placeholder="{Var7}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard88" runat="server" Visible="false"
                                                                        placeholder="{Var8}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard89" runat="server" Visible="false"
                                                                        placeholder="{Var9}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard810" runat="server"
                                                                        Visible="false" placeholder="{Var10}"
                                                                        onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>
                                                                </div>
                                                                <div id="divCard9" runat="server" style="border: dotted;" visible="false" class="carousel-item">
                                                                    <h5 style="padding-bottom: 20px;">Card 9</h5>
                                                                    <div class="input-group mb-3">
                                                                        <label for="inputEmail31" class="input-group-text sm-text">
                                                                            Upload File&nbsp;</label>
                                                                        <asp:FileUpload ID="fileuploadCarouselMedia9" accept=".mp4,.jpg,.jpeg,.png,"
                                                                            runat="server" class="form-control sm-text" ClientIDMode="Static" onchange="fileUpload(9);" />
                                                                    </div>
                                                                    <asp:HiddenField ID="HiddenFieldCard91" Value="{Var1}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard92" Value="{Var2}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard93" Value="{Var3}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard94" Value="{Var4}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard95" Value="{Var5}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard96" Value="{Var6}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard97" Value="{Var7}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard98" Value="{Var8}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard99" Value="{Var9}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard910" Value="{Var10}" runat="server" />

                                                                    <asp:TextBox ID="txtVarCard91" runat="server" Visible="false"
                                                                        placeholder="{Var1}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard92" runat="server" Visible="false"
                                                                        placeholder="{Var2}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard93" runat="server" Visible="false"
                                                                        placeholder="{Var3}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard94" runat="server" Visible="false"
                                                                        placeholder="{Var4}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard95" runat="server" Visible="false"
                                                                        placeholder="{Var5}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard96" runat="server" Visible="false"
                                                                        placeholder="{Var6}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard97" runat="server" Visible="false"
                                                                        placeholder="{Var7}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard98" runat="server" Visible="false"
                                                                        placeholder="{Var8}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard99" runat="server" Visible="false"
                                                                        placeholder="{Var9}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard910" runat="server"
                                                                        Visible="false" placeholder="{Var10}"
                                                                        onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>
                                                                </div>
                                                                <div id="divCard10" runat="server" style="border: dotted;" visible="false" class="carousel-item">
                                                                    <h5 style="padding-bottom: 20px;">Card 10</h5>
                                                                    <div class="input-group mb-3">
                                                                        <label for="inputEmail31" class="input-group-text sm-text">
                                                                            Upload File&nbsp;</label>
                                                                        <asp:FileUpload ID="fileuploadCarouselMedia10" accept=".mp4,.jpg,.jpeg,.png,"
                                                                            runat="server" class="form-control sm-text" ClientIDMode="Static" onchange="fileUpload(10);" />
                                                                    </div>
                                                                    <asp:HiddenField ID="HiddenFieldCard101" Value="{Var1}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard102" Value="{Var2}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard103" Value="{Var3}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard104" Value="{Var4}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard105" Value="{Var5}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard106" Value="{Var6}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard107" Value="{Var7}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard108" Value="{Var8}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard109" Value="{Var9}" runat="server" />
                                                                    <asp:HiddenField ID="HiddenFieldCard1010" Value="{Var10}" runat="server" />

                                                                    <asp:TextBox ID="txtVarCard101" runat="server" Visible="false"
                                                                        placeholder="{Var1}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard102" runat="server" Visible="false"
                                                                        placeholder="{Var2}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard103" runat="server" Visible="false"
                                                                        placeholder="{Var3}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard104" runat="server" Visible="false"
                                                                        placeholder="{Var4}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard105" runat="server" Visible="false"
                                                                        placeholder="{Var5}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard106" runat="server" Visible="false"
                                                                        placeholder="{Var6}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard107" runat="server" Visible="false"
                                                                        placeholder="{Var7}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard108" runat="server" Visible="false"
                                                                        placeholder="{Var8}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard109" runat="server" Visible="false"
                                                                        placeholder="{Var9}" onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>

                                                                    <asp:TextBox ID="txtVarCard1010" runat="server"
                                                                        Visible="false" placeholder="{Var10}"
                                                                        onkeyup="SMStextChange1(); return true;"
                                                                        class="form-control sm-text" ClientIDMode="Static"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <a class="carousel-control-prev" href="#CarouselControls" role="button" onclick="changeBothCarousels(-1)" data-slide="prev" style="height: 10px; margin-top: 3%;">
                                                                <span class="carousel-control-prev-icon" aria-hidden="true" style="background-color: black;"></span>
                                                                <span class="sr-only">Previous</span>
                                                            </a>
                                                            <a class="carousel-control-next" href="#CarouselControls" role="button" onclick="changeBothCarousels(1)" data-slide="next" style="height: 10px; margin-top: 3%;">
                                                                <span class="carousel-control-next-icon" aria-hidden="true" style="background-color: black;"></span>
                                                                <span class="sr-only">Next</span>
                                                            </a>
                                                        </div>
                                                    </div>
                                                </div>
                                                <br />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="row">
                                        <div class="col-md-12 " id="divMsg" runat="server" style="pointer-events: all;">
                                            <asp:TextBox ID="txtPreview" runat="server" placeholder="Whatsapp Text"
                                                TextMode="MultiLine" MaxLength="12288" class="form-control sm-text"
                                                Rows="5" onkeyup="smscnt1(); return true;"></asp:TextBox>
                                            <asp:TextBox ID="txtMsg" runat="server" TextMode="MultiLine"
                                                MaxLength="12288" class="form-control sm-text d-none" Rows="5"
                                                onkeyup="smscnt(); return true;"></asp:TextBox>
                                            <div id="divOptOut" runat="server" style="display: block;"
                                                visible="false">
                                                <asp:CheckBox ID="chkOptOut" runat="server" Text="Include - " />
                                                <asp:Label ID="lblOptOut" runat="server" Text="DND7726">
                                                </asp:Label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="col-sm-6"></div>
                                <div class="col-sm-2">
                                    <asp:LinkButton ID="LnkbtnSubmitTemplate" OnClick="LnkbtnSubmitTemplate_Click" runat="server" class="btn btn-primary text-secondary font-weight-bold btn-block"> 
                                                        <span>Submit</span>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                    <%-----------------End--------------%>

                    <div class="col-lg-12URL order-2 order-lg-1" id="divgridviewdetails" runat="server" visible="false">
                        <div class="card bg-primary border-light shadow-soft mb-4">
                            <div class="card-header py-3 bg-primary">
                                <h6 class="m-0 font-weight-bold">Short URL List
                                </h6>
                            </div>
                            <div class="card-body py-1">
                                <div class="row">
                                    <!-- Checker Card  -->
                                    <div class="col-12 mb-4 ">
                                        <div class="card bg-primary shadow-inset border-light h-100 py-2">
                                            <div class="card-body">
                                                <div class="row no-gutters align-items-center">
                                                    <div class="col mr-2">
                                                        <div class="h6 mb-0 text-gray">
                                                            <div class="table-responsive">
                                                                <asp:GridView ID="grdshorturl" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                                                                    runat="server" CellPadding="10" BorderColor="#ede8e8" OnRowCommand="grdshorturl_RowCommand"
                                                                    Class="table table-striped table-bordered dt-responsive nowrap dataTable-view">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderStyle-Width="5%" HeaderText="Sl. No.">
                                                                            <ItemTemplate>
                                                                                <%#Container.DataItemIndex+1 %>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Template Name">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblWABATemplateName" runat="server" Text='<%#Eval("WABATemplateName")%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Short URL">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblShortURL" runat="server" Text='<%#Eval("ShortURL")%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Creation Date">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblInsertDate" runat="server" Text='<%#Eval("InsertDate")%>'></asp:Label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Action">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lnkUnlinkTemplate" runat="server" OnClientClick="return confirm('Are you sure you want to unlink this record?');" 
                                                                                    CommandName="Unlink" CommandArgument='<%# Eval("ID") %>'>
                                                                                  <i class="fa fa-unlink"></i>
                                                                                </asp:LinkButton>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </main>

    <script type="text/javascript">
        function bindexp() {
            var offerCodeInput = document.getElementById("ContentPlaceHolder1_txtexptime");

            if (offerCodeInput) {
                var selectedDateTime = offerCodeInput.value;

                if (selectedDateTime) {
                    var date = new Date(selectedDateTime);
                    var formattedDate = date.toLocaleString('en-US', { month: 'long', day: 'numeric' });

                    var offerCodeLabel = document.getElementById("lblexptime");
                    if (offerCodeLabel) {
                        offerCodeLabel.innerText = 'Offer ends on ' + formattedDate;
                    }
                }
            }
        }

        function limitofrbind(ofrtxt = '', btntxt = '') {
            var expLabel = document.getElementById("lblexp");
            if (expLabel) {
                expLabel.innerText = ofrtxt;
            }
            var btn1 = document.getElementById("lblButton9");
            btn1.style.display = btn1.style.display === 'none' ? '' : 'none';
            var btn2 = document.getElementById("lblButton10");
            btn2.style.display = btn2.style.display === 'none' ? '' : 'none';

            var divlto = document.getElementById("divlto");
            divlto.style.display = divlto.style.display === 'none' ? '' : 'none';

            var lblbtn = document.getElementById("lblButton10");
            if (lblbtn) {
                lblbtn.innerText = btntxt;
            }

            $('#ContentPlaceHolder1_divdocument').hide();
            $('#ContentPlaceHolder1_divlbldoc').hide();
            $('#ContentPlaceHolder1_videofile').hide();
            $('#ContentPlaceHolder1_divvideomsg').hide();
            $('#ContentPlaceHolder1_spHeaderText').hide();
            $('#ContentPlaceHolder1_spfootertext').hide();
            $('#ContentPlaceHolder1_mobilemsgpreview').hide();
            $('#ContentPlaceHolder1_carouselExampleIndicators').hide();
        }

        function setCharAt(str, chr, rep) {
            var index = -1;
            index = str.lastIndexOf(chr);
            var len = chr.length;
            if (index > str.length - 1) return str;
            return str.substring(0, index) + rep + str.substring(index
                + len);
        }

        function SMSfileUpload60() {
            var uploadControl = document.getElementById('<%= uImage.ClientID %>');
            var myfile = uploadControl.value;
            console.log(myfile);
            var Extension = myfile.substring(myfile.lastIndexOf('.') + 1).toLowerCase();
            console.log(Extension);
            if ((Extension != "jpg" && Extension != "png" && Extension != "jpeg" && Extension != "pdf" && Extension != "mp4")) {
                alert("File Should be jpg, pdf, mp4 or png format !!");
                return false;
            }
            else {
                //showLoading();
                console.log("ret true");
                return true;
            }
        }

        function SMStextChange1(CardNo = '') {
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
            var txt1valcard11; var txt1valcard21; var txt1valcard31; var txt1valcard41; var txt1valcard51; var txt1valcard61; var txt1valcard71; var txt1valcard81; var txt1valcard91; var txt1valcard101;
            var txt1valcard12; var txt1valcard22; var txt1valcard32; var txt1valcard42; var txt1valcard52; var txt1valcard62; var txt1valcard72; var txt1valcard82; var txt1valcard92; var txt1valcard102;
            var txt1valcard13; var txt1valcard23; var txt1valcard33; var txt1valcard43; var txt1valcard53; var txt1valcard63; var txt1valcard73; var txt1valcard83; var txt1valcard93; var txt1valcard103;
            var txt1valcard14; var txt1valcard24; var txt1valcard34; var txt1valcard44; var txt1valcard54; var txt1valcard64; var txt1valcard74; var txt1valcard84; var txt1valcard94; var txt1valcard104;
            var txt1valcard15; var txt1valcard25; var txt1valcard35; var txt1valcard45; var txt1valcard55; var txt1valcard65; var txt1valcard75; var txt1valcard85; var txt1valcard95; var txt1valcard105;
            var txt1valcard16; var txt1valcard26; var txt1valcard36; var txt1valcard46; var txt1valcard56; var txt1valcard66; var txt1valcard76; var txt1valcard86; var txt1valcard96; var txt1valcard106;
            var txt1valcard17; var txt1valcard27; var txt1valcard37; var txt1valcard47; var txt1valcard57; var txt1valcard67; var txt1valcard77; var txt1valcard87; var txt1valcard97; var txt1valcard107;
            var txt1valcard18; var txt1valcard28; var txt1valcard38; var txt1valcard48; var txt1valcard58; var txt1valcard68; var txt1valcard78; var txt1valcard88; var txt1valcard98; var txt1valcard108;
            var txt1valcard19; var txt1valcard29; var txt1valcard39; var txt1valcard49; var txt1valcard59; var txt1valcard69; var txt1valcard79; var txt1valcard89; var txt1valcard99; var txt1valcard109;
            var txt1valcard110; var txt1valcard210; var txt1valcard310; var txt1valcard410; var txt1valcard510; var txt1valcard610; var txt1valcard710; var txt1valcard810; var txt1valcard910; var txt1valcard1010;

            var myPrevioew = document.getElementById("<%=hdnTemplateVarText.ClientID%>").value;
            var origText = document.getElementById("<%=vrCount.ClientID%>").value;
            var PlaceVal = document.getElementById("<%=hfVar1.ClientID%>").value;

            var vrCountCarousel1 = document.getElementById("<%=vrCountCarousel1.ClientID%>").value;
            var vrCountCarousel2 = document.getElementById("<%=vrCountCarousel2.ClientID%>").value;
            var vrCountCarousel3 = document.getElementById("<%=vrCountCarousel3.ClientID%>").value;
            var vrCountCarousel4 = document.getElementById("<%=vrCountCarousel4.ClientID%>").value;
            var vrCountCarousel5 = document.getElementById("<%=vrCountCarousel5.ClientID%>").value;
            var vrCountCarousel6 = document.getElementById("<%=vrCountCarousel6.ClientID%>").value;
            var vrCountCarousel7 = document.getElementById("<%=vrCountCarousel7.ClientID%>").value;
            var vrCountCarousel8 = document.getElementById("<%=vrCountCarousel8.ClientID%>").value;
            var vrCountCarousel9 = document.getElementById("<%=vrCountCarousel9.ClientID%>").value;
            var vrCountCarousel10 = document.getElementById("<%=vrCountCarousel10.ClientID%>").value;

            var myPrevioewcard11 = document.getElementById("<%=HiddenField1.ClientID%>").value;
            var myPrevioewcard12 = document.getElementById("<%=HiddenField2.ClientID%>").value;
            var myPrevioewcard13 = document.getElementById("<%=HiddenField3.ClientID%>").value;
            var myPrevioewcard14 = document.getElementById("<%=HiddenField4.ClientID%>").value;
            var myPrevioewcard15 = document.getElementById("<%=HiddenField5.ClientID%>").value;
            var myPrevioewcard16 = document.getElementById("<%=HiddenField6.ClientID%>").value;
            var myPrevioewcard17 = document.getElementById("<%=HiddenField7.ClientID%>").value;
            var myPrevioewcard18 = document.getElementById("<%=HiddenField8.ClientID%>").value;
            var myPrevioewcard19 = document.getElementById("<%=HiddenField9.ClientID%>").value;
            var myPrevioewcard110 = document.getElementById("<%=HiddenField10.ClientID%>").value;
            var btntype = document.getElementById("<%=hdnLto.ClientID%>").value;

            var textBox = document.getElementById("txtVarCard" + CardNo);
            if (CardNo != "") {
                if (vrCountCarousel1 == "1") {
                    var newPreviewcard1;
                    var txt1valcard11;
                    if (textBox.value == "") {
                        txt1valcard11 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    } else {
                        txt1valcard11 = textBox.value;
                    }
                    newPreviewcard1 = setCharAt(myPrevioewcard11, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard11);
                    carouselcard1(newPreviewcard1, CardNo);
                }
                else if (vrCountCarousel1 == "2") {
                    var newPreviewcard1;
                    var txt1valcard11;
                    if (textBox.value == "") {
                        txt1valcard11 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard11 = textBox.value;
                    }
                    if (textBox.value == "") {
                        txt1valcard12 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard12 = textBox.value;
                    }
                    newPreviewcard1 = setCharAt(myPrevioewcard11, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard11);
                    newPreviewcard1 = setCharAt(newPreviewcard1, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard12);
                    carouselcard1(newPreviewcard1, CardNo);
                }
                else if (vrCountCarousel1 == "3") {
                    var newPreviewcard1;
                    var txt1valcard11;
                    if (textBox.value == "") {
                        txt1valcard11 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard11 = textBox.value;
                    }
                    if (textBox.value == "") {
                        txt1valcard12 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard12 = dtextBox.value;
                    }
                    if (textBox.value == "") {
                        txt1valcard13 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard13 = textBox.value;
                    }

                    newPreviewcard1 = setCharAt(myPrevioewcard11, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard11);
                    newPreviewcard1 = setCharAt(newPreviewcard1, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard12);
                    newPreviewcard1 = setCharAt(newPreviewcard1, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard13);
                    carouselcard1(newPreviewcard1, CardNo);
                }
                else if (vrCountCarousel1 == "4") {
                    var newPreviewcard1;
                    var txt1valcard11;
                    if (textBox.value == "") {
                        txt1valcard11 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard11 = textBox.value;
                    }
                    if (textBox.value == "") {
                        txt1valcard12 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard12 = textBox.value;
                    }
                    if (textBox.value == "") {
                        txt1valcard13 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard13 = textBox.value;
                    }
                    if (textBox.value == "") {
                        txt1valcard14 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard14 = textBox.value;
                    }

                    newPreviewcard1 = setCharAt(myPrevioewcard11, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard11);
                    newPreviewcard1 = setCharAt(newPreviewcard1, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard12);
                    newPreviewcard1 = setCharAt(newPreviewcard1, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard13);
                    newPreviewcard1 = setCharAt(newPreviewcard1, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard14);
                    carouselcard1(newPreviewcard1, CardNo);
                }
                else if (vrCountCarousel1 == "5") {
                    var newPreviewcard1;
                    var txt1valcard11;
                    if (textBox.value == "") {
                        txt1valcard11 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard11 = textBox.value;
                    }
                    if (textBox.value == "") {
                        txt1valcard12 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard12 = textBox.value;
                    }
                    if (textBox.value == "") {
                        txt1valcard13 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard13 = textBox.value;
                    }
                    if (textBox.value == "") {
                        txt1valcard14 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard14 = textBox.value;
                    }
                    if (textBox.value == "") {
                        txt1valcard15 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard15 = textBox.value;
                    }

                    newPreviewcard1 = setCharAt(myPrevioewcard11, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard11);
                    newPreviewcard1 = setCharAt(newPreviewcard1, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard12);
                    newPreviewcard1 = setCharAt(newPreviewcard1, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard13);
                    newPreviewcard1 = setCharAt(newPreviewcard1, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard14);
                    newPreviewcard1 = setCharAt(newPreviewcard1, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard15);
                    carouselcard1(newPreviewcard1, CardNo);
                }
                else if (vrCountCarousel1 == "6") {
                    var newPreviewcard1;
                    var txt1valcard11;
                    if (textBox.value == "") {
                        txt1valcard11 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard11 = textBox.value;
                    }
                    if (textBox.value == "") {
                        txt1valcard12 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard12 = documenttextBox.value;
                    }
                    if (textBox.value == "") {
                        txt1valcard13 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard13 = textBox.value;
                    }
                    if (textBox.value == "") {
                        txt1valcard14 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard14 = textBox.value;
                    }
                    if (textBox.value == "") {
                        txt1valcard15 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard15 = textBox.value;
                    }
                    if (textBox.value == "") {
                        txt1valcard16 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard16 = textBox.value;
                    }

                    newPreviewcard1 = setCharAt(myPrevioewcard11, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard11);
                    newPreviewcard1 = setCharAt(newPreviewcard1, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard12);
                    newPreviewcard1 = setCharAt(newPreviewcard1, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard13);
                    newPreviewcard1 = setCharAt(newPreviewcard1, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard14);
                    newPreviewcard1 = setCharAt(newPreviewcard1, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard15);
                    newPreviewcard1 = setCharAt(newPreviewcard1, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard16);
                    carouselcard1(newPreviewcard1, CardNo);
                }
                else if (vrCountCarousel1 == "7") {
                    var newPreviewcard1;
                    var txt1valcard11;
                    if (textBox.value == "") {
                        txt1valcard11 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard11 = textBox.value;
                    }
                    if (textBox.value == "") {
                        txt1valcard12 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard12 = textBox.value;
                    }
                    if (textBox.value == "") {
                        txt1valcard13 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard13 = textBox.value;
                    }
                    if (textBox.value == "") {
                        txt1valcard14 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard14 = textBox.value;
                    }
                    if (textBox.value == "") {
                        txt1valcard15 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard15 = textBox.value;
                    }
                    if (textBox.value == "") {
                        txt1valcard16 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard16 = textBox.value;
                    }
                    if (textBox.value == "") {
                        txt1valcard17 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard17 = textBox.value;
                    }

                    newPreviewcard1 = setCharAt(myPrevioewcard11, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard11);
                    newPreviewcard1 = setCharAt(newPreviewcard1, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard12);
                    newPreviewcard1 = setCharAt(newPreviewcard1, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard13);
                    newPreviewcard1 = setCharAt(newPreviewcard1, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard14);
                    newPreviewcard1 = setCharAt(newPreviewcard1, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard15);
                    newPreviewcard1 = setCharAt(newPreviewcard1, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard16);
                    newPreviewcard1 = setCharAt(newPreviewcard1, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard17);
                    carouselcard1(newPreviewcard1, CardNo);
                }
                else if (vrCountCarousel1 == "8") {
                    var newPreviewcard1;
                    var txt1valcard11;
                    if (textBox.value == "") {
                        txt1valcard11 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard11 = textBox.value;
                    }
                    if (textBox.value == "") {
                        txt1valcard12 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard12 = textBox.value;
                    }
                    if (textBox.value == "") {
                        txt1valcard13 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard13 = textBox.value;
                    }
                    if (textBox.value == "") {
                        txt1valcard14 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard14 = textBox.value;
                    }
                    if (textBox.value == "") {
                        txt1valcard15 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard15 = textBox.value;
                    }
                    if (textBox.value == "") {
                        txt1valcard16 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard16 = textBox.value;
                    }
                    if (textBox.value == "") {
                        txt1valcard17 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard17 = textBox.value;
                    }

                    if (textBox.value == "") {
                        txt1valcard18 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard18 = textBox.value;
                    }

                    newPreviewcard1 = setCharAt(myPrevioewcard11, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard11);
                    newPreviewcard1 = setCharAt(newPreviewcard1, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard12);
                    newPreviewcard1 = setCharAt(newPreviewcard1, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard13);
                    newPreviewcard1 = setCharAt(newPreviewcard1, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard14);
                    newPreviewcard1 = setCharAt(newPreviewcard1, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard15);
                    newPreviewcard1 = setCharAt(newPreviewcard1, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard16);
                    newPreviewcard1 = setCharAt(newPreviewcard1, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard17);
                    newPreviewcard1 = setCharAt(newPreviewcard1, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard18);
                    carouselcard1(newPreviewcard1, CardNo);
                }
                else if (vrCountCarousel1 == "9") {
                    var newPreviewcard1;
                    var txt1valcard11;
                    if (textBox.value == "") {
                        txt1valcard11 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard11 = textBox.value;
                    }
                    if (textBox.value == "") {
                        txt1valcard12 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard12 = textBox.value;
                    }
                    if (textBox.value == "") {
                        txt1valcard13 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard13 = textBox.value;
                    }
                    if (textBox.value == "") {
                        txt1valcard14 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard14 = textBox.value;
                    }
                    if (textBox.value == "") {
                        txt1valcard15 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard15 = textBox.value;
                    }
                    if (textBox.value == "") {
                        txt1valcard16 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard16 = textBox.value;
                    }
                    if (textBox.value == "") {
                        txt1valcard17 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard17 = textBox.value;
                    }
                    if (textBox.value == "") {
                        txt1valcard18 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard18 = textBox.value;
                    }
                    if (textBox.value == "") {
                        txt1valcard19 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard19 = textBox.value;
                    }

                    newPreviewcard1 = setCharAt(myPrevioewcard11, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard11);
                    newPreviewcard1 = setCharAt(newPreviewcard1, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard12);
                    newPreviewcard1 = setCharAt(newPreviewcard1, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard13);
                    newPreviewcard1 = setCharAt(newPreviewcard1, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard14);
                    newPreviewcard1 = setCharAt(newPreviewcard1, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard15);
                    newPreviewcard1 = setCharAt(newPreviewcard1, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard16);
                    newPreviewcard1 = setCharAt(newPreviewcard1, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard17);
                    newPreviewcard1 = setCharAt(newPreviewcard1, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard18);
                    newPreviewcard1 = setCharAt(newPreviewcard1, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard19);
                    carouselcard1(newPreviewcard1, CardNo);
                }

                else if (vrCountCarousel1 == "10") {
                    var newPreviewcard1;
                    var txt1valcard11;
                    if (textBox.value == "") {
                        txt1valcard11 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard11 = textBox.value;
                    }
                    if (textBox.value == "") {
                        txt1valcard12 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard12 = textBox.value;
                    }
                    if (textBox.value == "") {
                        txt1valcard13 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard13 = textBox.value;
                    }
                    if (textBox.value == "") {
                        txt1valcard14 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard14 = textBox.value;
                    }
                    if (textBox.value == "") {
                        txt1valcard15 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard15 = textBox.value;
                    }
                    if (textBox.value == "") {
                        txt1valcard16 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard16 = textBox.value;
                    }
                    if (textBox.value == "") {
                        txt1valcard17 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard17 = textBox.value;
                    }
                    if (textBox.value == "") {
                        txt1valcard18 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard18 = textBox.value;
                    }
                    if (textBox.value == "") {
                        txt1valcard19 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard19 = textBox.value;
                    }
                    if (textBox.value == "") {
                        txt1valcard110 = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1valcard110 = textBox.value;
                    }

                    newPreviewcard1 = setCharAt(myPrevioewcard11, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard11);
                    newPreviewcard1 = setCharAt(newPreviewcard1, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard12);
                    newPreviewcard1 = setCharAt(newPreviewcard1, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard13);
                    newPreviewcard1 = setCharAt(newPreviewcard1, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard14);
                    newPreviewcard1 = setCharAt(newPreviewcard1, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard15);
                    newPreviewcard1 = setCharAt(newPreviewcard1, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard16);
                    newPreviewcard1 = setCharAt(newPreviewcard1, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard17);
                    newPreviewcard1 = setCharAt(newPreviewcard1, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard18);
                    newPreviewcard1 = setCharAt(newPreviewcard1, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard19);
                    newPreviewcard1 = setCharAt(newPreviewcard1, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1valcard110);
                    carouselcard1(newPreviewcard1, CardNo);
                }
            }
            else {
                if (origText == "1") {
                    if (document.getElementById("<%=TextBox1.ClientID%>").value == "") {
                        txt1val = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}";
                    }
                    else {
                        txt1val = document.getElementById("<%=TextBox1.ClientID%>").value;
                    }
                    var newPreview = setCharAt(myPrevioew, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1val);

                }

                else if (origText == "2") {
                    if (document.getElementById("<%=TextBox1.ClientID%>").value == "") {
                        txt1val = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}"
                    }
                    else txt1val = document.getElementById("<%=TextBox1.ClientID%>").value;
                    if (document.getElementById("<%=TextBox2.ClientID%>").value == "") {
                        txt2val = document.getElementById("<%=hfVar2.ClientID%>").value == "{Var2}" ? "{{2}}" : "{{" + document.getElementById("<%=hfVar2.ClientID%>").value + "}}"
                    } else txt2val = document.getElementById("<%=TextBox2.ClientID%>").value;

                    var newPreview = setCharAt(myPrevioew, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1val);
                    var newPreview = setCharAt(newPreview, document.getElementById("<%=hfVar2.ClientID%>").value == "{Var2}" ? "{{2}}" : "{{" + document.getElementById("<%=hfVar2.ClientID%>").value + "}}", txt2val);
                    console.log(newPreview);
                }

                else if (origText == "3") {

                    if (document.getElementById("<%=TextBox1.ClientID%>").value == "") {
                        txt1val = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}"
                    }
                    else txt1val = document.getElementById("<%=TextBox1.ClientID%>").value;
                    if (document.getElementById("<%=TextBox2.ClientID%>").value == "") {
                        txt2val = document.getElementById("<%=hfVar2.ClientID%>").value == "{Var2}" ? "{{2}}" : "{{" + document.getElementById("<%=hfVar2.ClientID%>").value + "}}"
                    } else txt2val = document.getElementById("<%=TextBox2.ClientID%>").value;
                    if (document.getElementById("<%=TextBox3.ClientID%>").value == "") {
                        txt3val = document.getElementById("<%=hfVar3.ClientID%>").value == "{Var3}" ? "{{3}}" : "{{" + document.getElementById("<%=hfVar3.ClientID%>").value + "}}"
                    } else txt3val = document.getElementById("<%=TextBox3.ClientID%>").value;

                    var newPreview = setCharAt(myPrevioew, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1val);
                    var newPreview = setCharAt(newPreview, document.getElementById("<%=hfVar2.ClientID%>").value == "{Var2}" ? "{{2}}" : "{{" + document.getElementById("<%=hfVar2.ClientID%>").value + "}}", txt2val);
                    var newPreview = setCharAt(newPreview, document.getElementById("<%=hfVar3.ClientID%>").value == "{Var3}" ? "{{3}}" : "{{" + document.getElementById("<%=hfVar3.ClientID%>").value + "}}", txt3val);

                    console.log(newPreview);

                }
                else if (origText == "4") {
                    if (document.getElementById("<%=TextBox1.ClientID%>").value == "") {
                        txt1val = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}"
                    }
                    else txt1val = document.getElementById("<%=TextBox1.ClientID%>").value;
                    if (document.getElementById("<%=TextBox2.ClientID%>").value == "") {
                        txt2val = document.getElementById("<%=hfVar2.ClientID%>").value == "{Var2}" ? "{{2}}" : "{{" + document.getElementById("<%=hfVar2.ClientID%>").value + "}}"
                    } else txt2val = document.getElementById("<%=TextBox2.ClientID%>").value;
                    if (document.getElementById("<%=TextBox3.ClientID%>").value == "") {
                        txt3val = document.getElementById("<%=hfVar3.ClientID%>").value == "{Var3}" ? "{{3}}" : "{{" + document.getElementById("<%=hfVar3.ClientID%>").value + "}}"
                    } else txt3val = document.getElementById("<%=TextBox3.ClientID%>").value;
                    if (document.getElementById("<%=TextBox4.ClientID%>").value == "")
                        txt4val = document.getElementById("<%=hfVar4.ClientID%>").value == "{Var4}" ? "{{4}}" : "{{" + document.getElementById("<%=hfVar4.ClientID%>").value + "}}"
                    else txt4val = document.getElementById("<%=TextBox4.ClientID%>").value;

                    var newPreview = setCharAt(myPrevioew, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1val);
                    var newPreview = setCharAt(newPreview, document.getElementById("<%=hfVar2.ClientID%>").value == "{Var2}" ? "{{2}}" : "{{" + document.getElementById("<%=hfVar2.ClientID%>").value + "}}", txt2val);
                    var newPreview = setCharAt(newPreview, document.getElementById("<%=hfVar3.ClientID%>").value == "{Var3}" ? "{{3}}" : "{{" + document.getElementById("<%=hfVar3.ClientID%>").value + "}}", txt3val);
                    var newPreview = setCharAt(newPreview, document.getElementById("<%=hfVar4.ClientID%>").value == "{Var4}" ? "{{4}}" : "{{" + document.getElementById("<%=hfVar4.ClientID%>").value + "}}", txt4val);
                    //caro(newPreview);
                    console.log(newPreview);
                }
                else if (origText == "5") {
                    if (document.getElementById("<%=TextBox1.ClientID%>").value == "")
                        txt1val = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}"
                    else txt1val = document.getElementById("<%=TextBox1.ClientID%>").value;
                    if (document.getElementById("<%=TextBox2.ClientID%>").value == "")
                        txt2val = document.getElementById("<%=hfVar2.ClientID%>").value == "{Var2}" ? "{{2}}" : "{{" + document.getElementById("<%=hfVar2.ClientID%>").value + "}}"
                    else txt2val = document.getElementById("<%=TextBox2.ClientID%>").value;
                    if (document.getElementById("<%=TextBox3.ClientID%>").value == "")
                        txt3val = document.getElementById("<%=hfVar3.ClientID%>").value == "{Var3}" ? "{{3}}" : "{{" + document.getElementById("<%=hfVar3.ClientID%>").value + "}}"
                    else txt3val = document.getElementById("<%=TextBox3.ClientID%>").value;
                    if (document.getElementById("<%=TextBox4.ClientID%>").value == "")
                        txt4val = document.getElementById("<%=hfVar4.ClientID%>").value == "{Var4}" ? "{{4}}" : "{{" + document.getElementById("<%=hfVar4.ClientID%>").value + "}}"
                    else txt4val = document.getElementById("<%=TextBox4.ClientID%>").value;
                    if (document.getElementById("<%=TextBox5.ClientID%>").value == "")
                        txt5val = document.getElementById("<%=hfVar5.ClientID%>").value == "{Var5}" ? "{{5}}" : "{{" + document.getElementById("<%=hfVar5.ClientID%>").value + "}}"
                    else txt5val = document.getElementById("<%=TextBox5.ClientID%>").value;

                    var newPreview = setCharAt(myPrevioew, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1val);
                    var newPreview = setCharAt(newPreview, document.getElementById("<%=hfVar2.ClientID%>").value == "{Var2}" ? "{{2}}" : "{{" + document.getElementById("<%=hfVar2.ClientID%>").value + "}}", txt2val);
                    var newPreview = setCharAt(newPreview, document.getElementById("<%=hfVar3.ClientID%>").value == "{Var3}" ? "{{3}}" : "{{" + document.getElementById("<%=hfVar3.ClientID%>").value + "}}", txt3val);
                    var newPreview = setCharAt(newPreview, document.getElementById("<%=hfVar4.ClientID%>").value == "{Var4}" ? "{{4}}" : "{{" + document.getElementById("<%=hfVar4.ClientID%>").value + "}}", txt4val);
                    var newPreview = setCharAt(newPreview, document.getElementById("<%=hfVar5.ClientID%>").value == "{Var5}" ? "{{5}}" : "{{" + document.getElementById("<%=hfVar5.ClientID%>").value + "}}", txt5val);
                    carouselcard1(newPreview);
                    console.log(newPreview);
                }
                else if (origText <= 6) {
                    if (document.getElementById("<%=TextBox1.ClientID%>").value == "")
                        txt1val = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}"
                    else txt1val = document.getElementById("<%=TextBox1.ClientID%>").value;
                    if (document.getElementById("<%=TextBox2.ClientID%>").value == "")
                        txt2val = document.getElementById("<%=hfVar2.ClientID%>").value == "{Var2}" ? "{{2}}" : "{{" + document.getElementById("<%=hfVar2.ClientID%>").value + "}}"
                    else txt2val = document.getElementById("<%=TextBox2.ClientID%>").value;
                    if (document.getElementById("<%=TextBox3.ClientID%>").value == "")
                        txt3val = document.getElementById("<%=hfVar3.ClientID%>").value == "{Var3}" ? "{{3}}" : "{{" + document.getElementById("<%=hfVar3.ClientID%>").value + "}}"
                    else txt3val = document.getElementById("<%=TextBox3.ClientID%>").value;
                    if (document.getElementById("<%=TextBox4.ClientID%>").value == "")
                        txt4val = document.getElementById("<%=hfVar4.ClientID%>").value == "{Var4}" ? "{{4}}" : "{{" + document.getElementById("<%=hfVar4.ClientID%>").value + "}}"
                    else txt4val = document.getElementById("<%=TextBox4.ClientID%>").value;
                    if (document.getElementById("<%=TextBox5.ClientID%>").value == "")
                        txt5val = document.getElementById("<%=hfVar5.ClientID%>").value == "{Var5}" ? "{{5}}" : "{{" + document.getElementById("<%=hfVar5.ClientID%>").value + "}}"
                    else txt5val = document.getElementById("<%=TextBox5.ClientID%>").value;
                    if (document.getElementById("<%=TextBox6.ClientID%>").value == "")
                        txt6val = document.getElementById("<%=hfVar6.ClientID%>").value == "{Var6}" ? "{{6}}" : "{{" + document.getElementById("<%=hfVar6.ClientID%>").value + "}}"
                    else txt6val = document.getElementById("<%=TextBox6.ClientID%>").value;

                    var newPreview = setCharAt(myPrevioew, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1val);
                    var newPreview = setCharAt(newPreview, document.getElementById("<%=hfVar2.ClientID%>").value == "{Var2}" ? "{{2}}" : "{{" + document.getElementById("<%=hfVar2.ClientID%>").value + "}}", txt2val);
                    var newPreview = setCharAt(newPreview, document.getElementById("<%=hfVar3.ClientID%>").value == "{Var3}" ? "{{3}}" : "{{" + document.getElementById("<%=hfVar3.ClientID%>").value + "}}", txt3val);
                    var newPreview = setCharAt(newPreview, document.getElementById("<%=hfVar4.ClientID%>").value == "{Var4}" ? "{{4}}" : "{{" + document.getElementById("<%=hfVar4.ClientID%>").value + "}}", txt4val);
                    var newPreview = setCharAt(newPreview, document.getElementById("<%=hfVar5.ClientID%>").value == "{Var5}" ? "{{5}}" : "{{" + document.getElementById("<%=hfVar5.ClientID%>").value + "}}", txt5val);
                    var newPreview = setCharAt(newPreview, document.getElementById("<%=hfVar6.ClientID%>").value == "{Var6}" ? "{{6}}" : "{{" + document.getElementById("<%=hfVar6.ClientID%>").value + "}}", txt6val);
                    carouselcard1(newPreview);
                    console.log(newPreview);
                }

                else if (origText <= 7) {
                    if (document.getElementById("<%=TextBox1.ClientID%>").value == "")
                        txt1val = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}"
                    else txt1val = document.getElementById("<%=TextBox1.ClientID%>").value;
                    if (document.getElementById("<%=TextBox2.ClientID%>").value == "")
                        txt2val = document.getElementById("<%=hfVar2.ClientID%>").value == "{Var2}" ? "{{2}}" : "{{" + document.getElementById("<%=hfVar2.ClientID%>").value + "}}"
                    else txt2val = document.getElementById("<%=TextBox2.ClientID%>").value;
                    if (document.getElementById("<%=TextBox3.ClientID%>").value == "")
                        txt3val = document.getElementById("<%=hfVar3.ClientID%>").value == "{Var3}" ? "{{3}}" : "{{" + document.getElementById("<%=hfVar3.ClientID%>").value + "}}"
                    else txt3val = document.getElementById("<%=TextBox3.ClientID%>").value;
                    if (document.getElementById("<%=TextBox4.ClientID%>").value == "")
                        txt4val = document.getElementById("<%=hfVar4.ClientID%>").value == "{Var4}" ? "{{4}}" : "{{" + document.getElementById("<%=hfVar4.ClientID%>").value + "}}"
                    else txt4val = document.getElementById("<%=TextBox4.ClientID%>").value;
                    if (document.getElementById("<%=TextBox5.ClientID%>").value == "")
                        txt5val = document.getElementById("<%=hfVar5.ClientID%>").value == "{Var5}" ? "{{5}}" : "{{" + document.getElementById("<%=hfVar5.ClientID%>").value + "}}"
                    else txt5val = document.getElementById("<%=TextBox5.ClientID%>").value;
                    if (document.getElementById("<%=TextBox6.ClientID%>").value == "")
                        txt6val = document.getElementById("<%=hfVar6.ClientID%>").value == "{Var6}" ? "{{6}}" : "{{" + document.getElementById("<%=hfVar6.ClientID%>").value + "}}"
                    else txt6val = document.getElementById("<%=TextBox6.ClientID%>").value;
                    if (document.getElementById("<%=TextBox7.ClientID%>").value == "")
                        txt7val = document.getElementById("<%=hfVar7.ClientID%>").value == "{Var7}" ? "{{7}}" : "{{" + document.getElementById("<%=hfVar7.ClientID%>").value + "}}"
                    else txt7val = document.getElementById("<%=TextBox7.ClientID%>").value;

                    var newPreview = setCharAt(myPrevioew, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1val);
                    var newPreview = setCharAt(newPreview, document.getElementById("<%=hfVar2.ClientID%>").value == "{Var2}" ? "{{2}}" : "{{" + document.getElementById("<%=hfVar2.ClientID%>").value + "}}", txt2val);
                    var newPreview = setCharAt(newPreview, document.getElementById("<%=hfVar3.ClientID%>").value == "{Var3}" ? "{{3}}" : "{{" + document.getElementById("<%=hfVar3.ClientID%>").value + "}}", txt3val);
                    var newPreview = setCharAt(newPreview, document.getElementById("<%=hfVar4.ClientID%>").value == "{Var4}" ? "{{4}}" : "{{" + document.getElementById("<%=hfVar4.ClientID%>").value + "}}", txt4val);
                    var newPreview = setCharAt(newPreview, document.getElementById("<%=hfVar5.ClientID%>").value == "{Var5}" ? "{{5}}" : "{{" + document.getElementById("<%=hfVar5.ClientID%>").value + "}}", txt5val);
                    var newPreview = setCharAt(newPreview, document.getElementById("<%=hfVar6.ClientID%>").value == "{Var6}" ? "{{6}}" : "{{" + document.getElementById("<%=hfVar6.ClientID%>").value + "}}", txt6val);
                    var newPreview = setCharAt(newPreview, document.getElementById("<%=hfVar7.ClientID%>").value == "{Var7}" ? "{{7}}" : "{{" + document.getElementById("<%=hfVar7.ClientID%>").value + "}}", txt7val);
                    //caro(newPreview);
                    console.log(newPreview);
                }

                else if (origText <= 8) {
                    if (document.getElementById("<%=TextBox1.ClientID%>").value == "")
                        txt1val = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}"
                    else txt1val = document.getElementById("<%=TextBox1.ClientID%>").value;
                    if (document.getElementById("<%=TextBox2.ClientID%>").value == "")
                        txt2val = document.getElementById("<%=hfVar2.ClientID%>").value == "{Var2}" ? "{{2}}" : "{{" + document.getElementById("<%=hfVar2.ClientID%>").value + "}}"
                    else txt2val = document.getElementById("<%=TextBox2.ClientID%>").value;
                    if (document.getElementById("<%=TextBox3.ClientID%>").value == "")
                        txt3val = document.getElementById("<%=hfVar3.ClientID%>").value == "{Var3}" ? "{{3}}" : "{{" + document.getElementById("<%=hfVar3.ClientID%>").value + "}}"
                    else txt3val = document.getElementById("<%=TextBox3.ClientID%>").value;
                    if (document.getElementById("<%=TextBox4.ClientID%>").value == "")
                        txt4val = document.getElementById("<%=hfVar4.ClientID%>").value == "{Var4}" ? "{{4}}" : "{{" + document.getElementById("<%=hfVar4.ClientID%>").value + "}}"
                    else txt4val = document.getElementById("<%=TextBox4.ClientID%>").value;
                    if (document.getElementById("<%=TextBox5.ClientID%>").value == "")
                        txt5val = document.getElementById("<%=hfVar5.ClientID%>").value == "{Var5}" ? "{{5}}" : "{{" + document.getElementById("<%=hfVar5.ClientID%>").value + "}}"
                    else txt5val = document.getElementById("<%=TextBox5.ClientID%>").value;
                    if (document.getElementById("<%=TextBox6.ClientID%>").value == "")
                        txt6val = document.getElementById("<%=hfVar6.ClientID%>").value == "{Var6}" ? "{{6}}" : "{{" + document.getElementById("<%=hfVar6.ClientID%>").value + "}}"
                    else txt6val = document.getElementById("<%=TextBox6.ClientID%>").value;
                    if (document.getElementById("<%=TextBox7.ClientID%>").value == "")
                        txt7val = document.getElementById("<%=hfVar7.ClientID%>").value == "{Var7}" ? "{{7}}" : "{{" + document.getElementById("<%=hfVar7.ClientID%>").value + "}}"
                    else txt7val = document.getElementById("<%=TextBox7.ClientID%>").value;
                    if (document.getElementById("<%=TextBox8.ClientID%>").value == "")
                        txt8val = document.getElementById("<%=hfVar8.ClientID%>").value == "{Var8}" ? "{{8}}" : "{{" + document.getElementById("<%=hfVar8.ClientID%>").value + "}}"
                    else txt8val = document.getElementById("<%=TextBox8.ClientID%>").value;

                    var newPreview = setCharAt(myPrevioew, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1val);
                    var newPreview = setCharAt(newPreview, document.getElementById("<%=hfVar2.ClientID%>").value == "{Var2}" ? "{{2}}" : "{{" + document.getElementById("<%=hfVar2.ClientID%>").value + "}}", txt2val);
                    var newPreview = setCharAt(newPreview, document.getElementById("<%=hfVar3.ClientID%>").value == "{Var3}" ? "{{3}}" : "{{" + document.getElementById("<%=hfVar3.ClientID%>").value + "}}", txt3val);
                    var newPreview = setCharAt(newPreview, document.getElementById("<%=hfVar4.ClientID%>").value == "{Var4}" ? "{{4}}" : "{{" + document.getElementById("<%=hfVar4.ClientID%>").value + "}}", txt4val);
                    var newPreview = setCharAt(newPreview, document.getElementById("<%=hfVar5.ClientID%>").value == "{Var5}" ? "{{5}}" : "{{" + document.getElementById("<%=hfVar5.ClientID%>").value + "}}", txt5val);
                    var newPreview = setCharAt(newPreview, document.getElementById("<%=hfVar6.ClientID%>").value == "{Var6}" ? "{{6}}" : "{{" + document.getElementById("<%=hfVar6.ClientID%>").value + "}}", txt6val);
                    var newPreview = setCharAt(newPreview, document.getElementById("<%=hfVar7.ClientID%>").value == "{Var7}" ? "{{7}}" : "{{" + document.getElementById("<%=hfVar7.ClientID%>").value + "}}", txt7val);
                    var newPreview = setCharAt(newPreview, document.getElementById("<%=hfVar8.ClientID%>").value == "{Var8}" ? "{{8}}" : "{{" + document.getElementById("<%=hfVar8.ClientID%>").value + "}}", txt8val);
                    //caro(newPreview);
                    console.log(newPreview);
                }

                else if (origText <= 9) {
                    if (document.getElementById("<%=TextBox1.ClientID%>").value == "")
                        txt1val = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}"
                    else txt1val = document.getElementById("<%=TextBox1.ClientID%>").value;
                    if (document.getElementById("<%=TextBox2.ClientID%>").value == "")
                        txt2val = document.getElementById("<%=hfVar2.ClientID%>").value == "{Var2}" ? "{{2}}" : "{{" + document.getElementById("<%=hfVar2.ClientID%>").value + "}}"
                    else txt2val = document.getElementById("<%=TextBox2.ClientID%>").value;
                    if (document.getElementById("<%=TextBox3.ClientID%>").value == "")
                        txt3val = document.getElementById("<%=hfVar3.ClientID%>").value == "{Var3}" ? "{{3}}" : "{{" + document.getElementById("<%=hfVar3.ClientID%>").value + "}}"
                    else txt3val = document.getElementById("<%=TextBox3.ClientID%>").value;
                    if (document.getElementById("<%=TextBox4.ClientID%>").value == "")
                        txt4val = document.getElementById("<%=hfVar4.ClientID%>").value == "{Var4}" ? "{{4}}" : "{{" + document.getElementById("<%=hfVar4.ClientID%>").value + "}}"
                    else txt4val = document.getElementById("<%=TextBox4.ClientID%>").value;
                    if (document.getElementById("<%=TextBox5.ClientID%>").value == "")
                        txt5val = document.getElementById("<%=hfVar5.ClientID%>").value == "{Var5}" ? "{{5}}" : "{{" + document.getElementById("<%=hfVar5.ClientID%>").value + "}}"
                    else txt5val = document.getElementById("<%=TextBox5.ClientID%>").value;
                    if (document.getElementById("<%=TextBox6.ClientID%>").value == "")
                        txt6val = document.getElementById("<%=hfVar6.ClientID%>").value == "{Var6}" ? "{{6}}" : "{{" + document.getElementById("<%=hfVar6.ClientID%>").value + "}}"
                    else txt6val = document.getElementById("<%=TextBox6.ClientID%>").value;
                    if (document.getElementById("<%=TextBox7.ClientID%>").value == "")
                        txt7val = document.getElementById("<%=hfVar7.ClientID%>").value == "{Var7}" ? "{{7}}" : "{{" + document.getElementById("<%=hfVar7.ClientID%>").value + "}}"
                    else txt7val = document.getElementById("<%=TextBox7.ClientID%>").value;
                    if (document.getElementById("<%=TextBox8.ClientID%>").value == "")
                        txt8val = document.getElementById("<%=hfVar8.ClientID%>").value == "{Var8}" ? "{{8}}" : "{{" + document.getElementById("<%=hfVar8.ClientID%>").value + "}}"
                    else txt8val = document.getElementById("<%=TextBox8.ClientID%>").value;
                    if (document.getElementById("<%=TextBox9.ClientID%>").value == "")
                        txt9val = document.getElementById("<%=hfVar9.ClientID%>").value == "{Var9}" ? "{{9}}" : "{{" + document.getElementById("<%=hfVar9.ClientID%>").value + "}}"
                    else txt9val = document.getElementById("<%=TextBox9.ClientID%>").value;

                    var newPreview = setCharAt(myPrevioew, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1val);
                    var newPreview = setCharAt(newPreview, document.getElementById("<%=hfVar2.ClientID%>").value == "{Var2}" ? "{{2}}" : "{{" + document.getElementById("<%=hfVar2.ClientID%>").value + "}}", txt2val);
                    var newPreview = setCharAt(newPreview, document.getElementById("<%=hfVar3.ClientID%>").value == "{Var3}" ? "{{3}}" : "{{" + document.getElementById("<%=hfVar3.ClientID%>").value + "}}", txt3val);
                    var newPreview = setCharAt(newPreview, document.getElementById("<%=hfVar4.ClientID%>").value == "{Var4}" ? "{{4}}" : "{{" + document.getElementById("<%=hfVar4.ClientID%>").value + "}}", txt4val);
                    var newPreview = setCharAt(newPreview, document.getElementById("<%=hfVar5.ClientID%>").value == "{Var5}" ? "{{5}}" : "{{" + document.getElementById("<%=hfVar5.ClientID%>").value + "}}", txt5val);
                    var newPreview = setCharAt(newPreview, document.getElementById("<%=hfVar6.ClientID%>").value == "{Var6}" ? "{{6}}" : "{{" + document.getElementById("<%=hfVar6.ClientID%>").value + "}}", txt6val);
                    var newPreview = setCharAt(newPreview, document.getElementById("<%=hfVar7.ClientID%>").value == "{Var7}" ? "{{7}}" : "{{" + document.getElementById("<%=hfVar7.ClientID%>").value + "}}", txt7val);
                    var newPreview = setCharAt(newPreview, document.getElementById("<%=hfVar8.ClientID%>").value == "{Var8}" ? "{{8}}" : "{{" + document.getElementById("<%=hfVar8.ClientID%>").value + "}}", txt8val);
                    var newPreview = setCharAt(newPreview, document.getElementById("<%=hfVar9.ClientID%>").value == "{Var9}" ? "{{9}}" : "{{" + document.getElementById("<%=hfVar9.ClientID%>").value + "}}", txt9val);
                    //caro(newPreview);
                    console.log(newPreview);
                }

                else if (origText <= 10) {
                    if (document.getElementById("<%=TextBox1.ClientID%>").value == "")
                        txt1val = document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}"
                    else txt1val = document.getElementById("<%=TextBox1.ClientID%>").value;
                    if (document.getElementById("<%=TextBox2.ClientID%>").value == "")
                        txt2val = document.getElementById("<%=hfVar2.ClientID%>").value == "{Var2}" ? "{{2}}" : "{{" + document.getElementById("<%=hfVar2.ClientID%>").value + "}}"
                    else txt2val = document.getElementById("<%=TextBox2.ClientID%>").value;
                    if (document.getElementById("<%=TextBox3.ClientID%>").value == "")
                        txt3val = document.getElementById("<%=hfVar3.ClientID%>").value == "{Var3}" ? "{{3}}" : "{{" + document.getElementById("<%=hfVar3.ClientID%>").value + "}}"
                    else txt3val = document.getElementById("<%=TextBox3.ClientID%>").value;
                    if (document.getElementById("<%=TextBox4.ClientID%>").value == "")
                        txt4val = document.getElementById("<%=hfVar4.ClientID%>").value == "{Var4}" ? "{{4}}" : "{{" + document.getElementById("<%=hfVar4.ClientID%>").value + "}}"
                    else txt4val = document.getElementById("<%=TextBox4.ClientID%>").value;
                    if (document.getElementById("<%=TextBox5.ClientID%>").value == "")
                        txt5val = document.getElementById("<%=hfVar5.ClientID%>").value == "{Var5}" ? "{{5}}" : "{{" + document.getElementById("<%=hfVar5.ClientID%>").value + "}}"
                    else txt5val = document.getElementById("<%=TextBox5.ClientID%>").value;
                    if (document.getElementById("<%=TextBox6.ClientID%>").value == "")
                        txt6val = document.getElementById("<%=hfVar6.ClientID%>").value == "{Var6}" ? "{{6}}" : "{{" + document.getElementById("<%=hfVar6.ClientID%>").value + "}}"
                    else txt6val = document.getElementById("<%=TextBox6.ClientID%>").value;
                    if (document.getElementById("<%=TextBox7.ClientID%>").value == "")
                        txt7val = document.getElementById("<%=hfVar7.ClientID%>").value == "{Var7}" ? "{{7}}" : "{{" + document.getElementById("<%=hfVar7.ClientID%>").value + "}}"
                    else txt7val = document.getElementById("<%=TextBox7.ClientID%>").value;
                    if (document.getElementById("<%=TextBox8.ClientID%>").value == "")
                        txt8val = document.getElementById("<%=hfVar8.ClientID%>").value == "{Var8}" ? "{{8}}" : "{{" + document.getElementById("<%=hfVar8.ClientID%>").value + "}}"
                    else txt8val = document.getElementById("<%=TextBox8.ClientID%>").value;
                    if (document.getElementById("<%=TextBox9.ClientID%>").value == "")
                        txt9val = document.getElementById("<%=hfVar9.ClientID%>").value == "{Var9}" ? "{{9}}" : "{{" + document.getElementById("<%=hfVar9.ClientID%>").value + "}}"
                    else txt9val = document.getElementById("<%=TextBox9.ClientID%>").value;
                    if (document.getElementById("<%=TextBox10.ClientID%>").value == "")
                        txt10val = document.getElementById("<%=hfVar10.ClientID%>").value == "{Var10}" ? "{{10}}" : "{{" + document.getElementById("<%=hfVar10.ClientID%>").value + "}}"
                    else txt10val = document.getElementById("<%=TextBox10.ClientID%>").value

                    var newPreview = setCharAt(myPrevioew, document.getElementById("<%=hfVar1.ClientID%>").value == "{Var1}" ? "{{1}}" : "{{" + document.getElementById("<%=hfVar1.ClientID%>").value + "}}", txt1val);
                    var newPreview = setCharAt(newPreview, document.getElementById("<%=hfVar2.ClientID%>").value == "{Var2}" ? "{{2}}" : "{{" + document.getElementById("<%=hfVar2.ClientID%>").value + "}}", txt2val);
                    var newPreview = setCharAt(newPreview, document.getElementById("<%=hfVar3.ClientID%>").value == "{Var3}" ? "{{3}}" : "{{" + document.getElementById("<%=hfVar3.ClientID%>").value + "}}", txt3val);
                    var newPreview = setCharAt(newPreview, document.getElementById("<%=hfVar4.ClientID%>").value == "{Var4}" ? "{{4}}" : "{{" + document.getElementById("<%=hfVar4.ClientID%>").value + "}}", txt4val);
                    var newPreview = setCharAt(newPreview, document.getElementById("<%=hfVar5.ClientID%>").value == "{Var5}" ? "{{5}}" : "{{" + document.getElementById("<%=hfVar5.ClientID%>").value + "}}", txt5val);
                    var newPreview = setCharAt(newPreview, document.getElementById("<%=hfVar6.ClientID%>").value == "{Var6}" ? "{{6}}" : "{{" + document.getElementById("<%=hfVar6.ClientID%>").value + "}}", txt6val);
                    var newPreview = setCharAt(newPreview, document.getElementById("<%=hfVar7.ClientID%>").value == "{Var7}" ? "{{7}}" : "{{" + document.getElementById("<%=hfVar7.ClientID%>").value + "}}", txt7val);
                    var newPreview = setCharAt(newPreview, document.getElementById("<%=hfVar8.ClientID%>").value == "{Var8}" ? "{{8}}" : "{{" + document.getElementById("<%=hfVar8.ClientID%>").value + "}}", txt8val);
                    var newPreview = setCharAt(newPreview, document.getElementById("<%=hfVar9.ClientID%>").value == "{Var9}" ? "{{9}}" : "{{" + document.getElementById("<%=hfVar9.ClientID%>").value + "}}", txt9val);
                    var newPreview = setCharAt(newPreview, document.getElementById("<%=hfVar10.ClientID%>").value == "{Var10}" ? "{{10}}" : "{{" + document.getElementById("<%=hfVar10.ClientID%>").value + "}}", txt10val);
                    //caro(newPreview);
                    console.log(newPreview);
                }

                $(".emojionearea-editor").text(newPreview);
                $("#ContentPlaceHolder1_txtPreview").text(newPreview);
                $("#ContentPlaceHolder1_txtPreview").val(newPreview);
                $("#ContentPlaceHolder1_txtMsg").text(newPreview);
                hdnTemplateVarText = newPreview;
                //onChangePreview('', '', '', btntype);
            }
        }

        function ConfirmBal() {
            console.log("reached");
            var curr =  ' <%= Session["CURRENCY"] %>';
            var rate = ' <%= Session["URLRATE"] %>';
            console.log(rate);
            var x = confirm(" " + rate + " " + curr + " will be deducted from the Balance. Do you want to continue ?");
            console.log(x);
            if (x == true) return true; else return false;

        }
    </script>
</asp:Content>
