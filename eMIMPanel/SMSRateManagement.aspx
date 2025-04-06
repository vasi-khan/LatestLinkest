<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="SMSRateManagement.aspx.cs" Inherits="eMIMPanel.SMSRateManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <main>
        <div class="container-fluid">
            <!--  -->
            <nav aria-label="breadcrumb" class="my-3">
                <ol class="breadcrumb breadcrumb-info">
                    <li class="breadcrumb-item"><a href="#">Home</a></li>
                    <li class="breadcrumb-item"><a href="#">Balance Management</a></li>
                    <li class="breadcrumb-item active" aria-current="page">SMS Rate Management</li>
                </ol>
            </nav>
            <!--  -->

            <div class="row">
                <div class="col-12">
                    <!--  -->
                    <div class="card card-body mb-4 bg-primary border-light shadow-soft">
                        <div class="form-group row">
                            <div class="col-md-2 mb-3">
                                <asp:Button ID="btnBack" runat="server" Text="Go Back" PostBackUrl="~/balance-management0.aspx" />
                            </div>

                            <div class="col-md-4 mb-3">
                                <asp:DropDownList ID="ddlCountryCode" runat="server" class="drop-select form-control" data-live-search="true">
                                    <asp:ListItem Value="93" Text="Afghanistan (+93)"></asp:ListItem>
                                    <asp:ListItem Value="355" Text="Albania (+355)"></asp:ListItem>
                                    <asp:ListItem Value="213" Text="Algeria (+213)"></asp:ListItem>
                                    <asp:ListItem Value="376" Text="Andorra (+376)"></asp:ListItem>
                                    <asp:ListItem Value="244" Text="Angola (+244)"></asp:ListItem>
                                    <asp:ListItem Value="54" Text="Argentina (+54)"></asp:ListItem>
                                    <asp:ListItem Value="374" Text="Armenia (+374)"></asp:ListItem>
                                    <asp:ListItem Value="297" Text="Aruba (+297)"></asp:ListItem>
                                    <asp:ListItem Value="61" Text="Australia (+61)"></asp:ListItem>
                                    <asp:ListItem Value="43" Text="Austria (+43)"></asp:ListItem>
                                    <asp:ListItem Value="994" Text="Azerbaijan (+994)"></asp:ListItem>
                                    <asp:ListItem Value="973" Text="Bahrain (+973)"></asp:ListItem>
                                    <asp:ListItem Value="880" Text="Bangladesh (+880)"></asp:ListItem>
                                    <asp:ListItem Value="375" Text="Belarus (+375)"></asp:ListItem>
                                    <asp:ListItem Value="32" Text="Belgium (+32)"></asp:ListItem>
                                    <asp:ListItem Value="501" Text="Belize (+501)"></asp:ListItem>
                                    <asp:ListItem Value="229" Text="Benin (+229)"></asp:ListItem>
                                    <asp:ListItem Value="975" Text="Bhutan (+975)"></asp:ListItem>
                                    <asp:ListItem Value="591" Text="Bolivia (+591)"></asp:ListItem>
                                    <asp:ListItem Value="387" Text="Bosnia and Herzegovina (+387)"></asp:ListItem>
                                    <asp:ListItem Value="267" Text="Botswana (+267)"></asp:ListItem>
                                    <asp:ListItem Value="55" Text="Brazil (+55)"></asp:ListItem>
                                    <asp:ListItem Value="673" Text="Brunei (+673)"></asp:ListItem>
                                    <asp:ListItem Value="359" Text="Bulgaria (+359)"></asp:ListItem>
                                    <asp:ListItem Value="226" Text="Burkina Faso (+226)"></asp:ListItem>
                                    <asp:ListItem Value="257" Text="Burundi (+257)"></asp:ListItem>
                                    <asp:ListItem Value="855" Text="Cambodia (+855)"></asp:ListItem>
                                    <asp:ListItem Value="237" Text="Cameroon (+237)"></asp:ListItem>
                                    <asp:ListItem Value="238" Text="Cape Verde (+238)"></asp:ListItem>
                                    <asp:ListItem Value="236" Text="Central African Republic (+236)"></asp:ListItem>
                                    <asp:ListItem Value="235" Text="Chad (+235)"></asp:ListItem>
                                    <asp:ListItem Value="56" Text="Chile (+56)"></asp:ListItem>
                                    <asp:ListItem Value="86" Text="China (+86)"></asp:ListItem>
                                    <asp:ListItem Value="57" Text="Colombia (+57)"></asp:ListItem>
                                    <asp:ListItem Value="269" Text="Comoros (+269)"></asp:ListItem>
                                    <asp:ListItem Value="242" Text="Congo (+242)"></asp:ListItem>
                                    <asp:ListItem Value="682" Text="Cook Islands (+682)"></asp:ListItem>
                                    <asp:ListItem Value="506" Text="Costa Rica (+506)"></asp:ListItem>
                                    <asp:ListItem Value="385" Text="Croatia (+385)"></asp:ListItem>
                                    <asp:ListItem Value="53" Text="Cuba (+53)"></asp:ListItem>
                                    <asp:ListItem Value="357" Text="Cyprus (+357)"></asp:ListItem>
                                    <asp:ListItem Value="420" Text="Czech Republic (+420)"></asp:ListItem>
                                    <asp:ListItem Value="243" Text="Democratic Republic of Congo (+243)"></asp:ListItem>
                                    <asp:ListItem Value="45" Text="Denmark (+45)"></asp:ListItem>
                                    <asp:ListItem Value="253" Text="Djibouti (+253)"></asp:ListItem>
                                    <asp:ListItem Value="593" Text="Ecuador (+593)"></asp:ListItem>
                                    <asp:ListItem Value="20" Text="Egypt (+20)"></asp:ListItem>
                                    <asp:ListItem Value="503" Text="El Salvador (+503)"></asp:ListItem>
                                    <asp:ListItem Value="240" Text="Equatorial Guinea (+240)"></asp:ListItem>
                                    <asp:ListItem Value="372" Text="Estonia (+372)"></asp:ListItem>
                                    <asp:ListItem Value="251" Text="Ethiopia (+251)"></asp:ListItem>
                                    <asp:ListItem Value="500" Text="Falkland (Malvinas) Islands (+500)"></asp:ListItem>
                                    <asp:ListItem Value="298" Text="Faroe Islands (+298)"></asp:ListItem>
                                    <asp:ListItem Value="679" Text="Fiji (+679)"></asp:ListItem>
                                    <asp:ListItem Value="358" Text="Finland (+358)"></asp:ListItem>
                                    <asp:ListItem Value="33" Text="France (+33)"></asp:ListItem>
                                    <asp:ListItem Value="594" Text="French Guiana (+594)"></asp:ListItem>
                                    <asp:ListItem Value="241" Text="Gabon (+241)"></asp:ListItem>
                                    <asp:ListItem Value="220" Text="Gambia (+220)"></asp:ListItem>
                                    <asp:ListItem Value="995" Text="Georgia (+995)"></asp:ListItem>
                                    <asp:ListItem Value="49" Text="Germany (+49)"></asp:ListItem>
                                    <asp:ListItem Value="233" Text="Ghana (+233)"></asp:ListItem>
                                    <asp:ListItem Value="350" Text="Gibraltar (+350)"></asp:ListItem>
                                    <asp:ListItem Value="30" Text="Greece (+30)"></asp:ListItem>
                                    <asp:ListItem Value="299" Text="Greenland (+299)"></asp:ListItem>
                                    <asp:ListItem Value="590" Text="Guadeloupe (+590)"></asp:ListItem>
                                    <asp:ListItem Value="502" Text="Guatemala (+502)"></asp:ListItem>
                                    <asp:ListItem Value="224" Text="Guinea (+224)"></asp:ListItem>
                                    <asp:ListItem Value="245" Text="Guinea-Bissau (+245)"></asp:ListItem>
                                    <asp:ListItem Value="592" Text="Guyana (+592)"></asp:ListItem>
                                    <asp:ListItem Value="509" Text="Haiti (+509)"></asp:ListItem>
                                    <asp:ListItem Value="504" Text="Honduras (+504)"></asp:ListItem>
                                    <asp:ListItem Value="852" Text="Hong Kong (+852)"></asp:ListItem>
                                    <asp:ListItem Value="36" Text="Hungary (+36)"></asp:ListItem>
                                    <asp:ListItem Value="354" Text="Iceland (+354)"></asp:ListItem>
                                    <asp:ListItem Value="91" Text="India (+91)" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="62" Text="Indonesia (+62)"></asp:ListItem>
                                    <asp:ListItem Value="98" Text="Iran (+98)"></asp:ListItem>
                                    <asp:ListItem Value="964" Text="Iraq (+964)"></asp:ListItem>
                                    <asp:ListItem Value="353" Text="Ireland (+353)"></asp:ListItem>
                                    <asp:ListItem Value="972" Text="Israel (+972)"></asp:ListItem>
                                    <asp:ListItem Value="39" Text="Italy (+39)"></asp:ListItem>
                                    <asp:ListItem Value="225" Text="Ivory Coast (+225)"></asp:ListItem>
                                    <asp:ListItem Value="81" Text="Japan (+81)"></asp:ListItem>
                                    <asp:ListItem Value="962" Text="Jordan (+962)"></asp:ListItem>
                                    <asp:ListItem Value="254" Text="Kenya (+254)"></asp:ListItem>
                                    <asp:ListItem Value="686" Text="Kiribati (+686)"></asp:ListItem>
                                    <asp:ListItem Value="965" Text="Kuwait (+965)"></asp:ListItem>
                                    <asp:ListItem Value="996" Text="Kyrgyzstan (+996)"></asp:ListItem>
                                    <asp:ListItem Value="856" Text="Laos (+856)"></asp:ListItem>
                                    <asp:ListItem Value="371" Text="Latvia (+371)"></asp:ListItem>
                                    <asp:ListItem Value="961" Text="Lebanon (+961)"></asp:ListItem>
                                    <asp:ListItem Value="266" Text="Lesotho (+266)"></asp:ListItem>
                                    <asp:ListItem Value="231" Text="Liberia (+231)"></asp:ListItem>
                                    <asp:ListItem Value="218" Text="Libya (+218)"></asp:ListItem>
                                    <asp:ListItem Value="423" Text="Liechtenstein (+423)"></asp:ListItem>
                                    <asp:ListItem Value="370" Text="Lithuania (+370)"></asp:ListItem>
                                    <asp:ListItem Value="352" Text="Luxembourg (+352)"></asp:ListItem>
                                    <asp:ListItem Value="853" Text="Macau (+853)"></asp:ListItem>
                                    <asp:ListItem Value="389" Text="Macedonia (+389)"></asp:ListItem>
                                    <asp:ListItem Value="261" Text="Madagascar (+261)"></asp:ListItem>
                                    <asp:ListItem Value="265" Text="Malawi (+265)"></asp:ListItem>
                                    <asp:ListItem Value="60" Text="Malaysia (+60)"></asp:ListItem>
                                    <asp:ListItem Value="960" Text="Maldives (+960)"></asp:ListItem>
                                    <asp:ListItem Value="223" Text="Mali (+223)"></asp:ListItem>
                                    <asp:ListItem Value="356" Text="Malta (+356)"></asp:ListItem>
                                    <asp:ListItem Value="596" Text="Martinique (+596)"></asp:ListItem>
                                    <asp:ListItem Value="222" Text="Mauritania (+222)"></asp:ListItem>
                                    <asp:ListItem Value="230" Text="Mauritius (+230)"></asp:ListItem>
                                    <asp:ListItem Value="52" Text="Mexico (+52)"></asp:ListItem>
                                    <asp:ListItem Value="373" Text="Moldova (+373)"></asp:ListItem>
                                    <asp:ListItem Value="377" Text="Monaco (+377)"></asp:ListItem>
                                    <asp:ListItem Value="976" Text="Mongolia (+976)"></asp:ListItem>
                                    <asp:ListItem Value="382" Text="Montenegro (+382)"></asp:ListItem>
                                    <asp:ListItem Value="212" Text="Morocco (+212)"></asp:ListItem>
                                    <asp:ListItem Value="258" Text="Mozambique (+258)"></asp:ListItem>
                                    <asp:ListItem Value="95" Text="Myanmar (+95)"></asp:ListItem>
                                    <asp:ListItem Value="264" Text="Namibia (+264)"></asp:ListItem>
                                    <asp:ListItem Value="977" Text="Nepal (+977)"></asp:ListItem>
                                    <asp:ListItem Value="31" Text="Netherlands (+31)"></asp:ListItem>
                                    <asp:ListItem Value="599" Text="Netherlands Antilles (+599)"></asp:ListItem>
                                    <asp:ListItem Value="64" Text="New Zealand (+64)"></asp:ListItem>
                                    <asp:ListItem Value="505" Text="Nicaragua (+505)"></asp:ListItem>
                                    <asp:ListItem Value="227" Text="Niger (+227)"></asp:ListItem>
                                    <asp:ListItem Value="234" Text="Nigeria (+234)"></asp:ListItem>
                                    <asp:ListItem Value="47" Text="Norway (+47)"></asp:ListItem>
                                    <asp:ListItem Value="968" Text="Oman (+968)"></asp:ListItem>
                                    <asp:ListItem Value="92" Text="Pakistan (+92)"></asp:ListItem>
                                    <asp:ListItem Value="680" Text="Palau (+680)"></asp:ListItem>
                                    <asp:ListItem Value="507" Text="Panama (+507)"></asp:ListItem>
                                    <asp:ListItem Value="675" Text="Papua New Guinea (+675)"></asp:ListItem>
                                    <asp:ListItem Value="595" Text="Paraguay (+595)"></asp:ListItem>
                                    <asp:ListItem Value="51" Text="Peru (+51)"></asp:ListItem>
                                    <asp:ListItem Value="63" Text="Philippines (+63)"></asp:ListItem>
                                    <asp:ListItem Value="48" Text="Poland (+48)"></asp:ListItem>
                                    <asp:ListItem Value="351" Text="Portugal (+351)"></asp:ListItem>
                                    <asp:ListItem Value="974" Text="Qatar (+974)"></asp:ListItem>
                                    <asp:ListItem Value="262" Text="Reunion (+262)"></asp:ListItem>
                                    <asp:ListItem Value="40" Text="Romania (+40)"></asp:ListItem>
                                    <asp:ListItem Value="7" Text="Russian Federation (+7)"></asp:ListItem>
                                    <asp:ListItem Value="250" Text="Rwanda (+250)"></asp:ListItem>
                                    <asp:ListItem Value="685" Text="Samoa (+685)"></asp:ListItem>
                                    <asp:ListItem Value="378" Text="San Marino (+378)"></asp:ListItem>
                                    <asp:ListItem Value="239" Text="Sao Tome and Principe (+239)"></asp:ListItem>
                                    <asp:ListItem Value="966" Text="Saudi Arabia (+966)"></asp:ListItem>
                                    <asp:ListItem Value="221" Text="Senegal (+221)"></asp:ListItem>
                                    <asp:ListItem Value="381" Text="Serbia (+381)"></asp:ListItem>
                                    <asp:ListItem Value="248" Text="Seychelles (+248)"></asp:ListItem>
                                    <asp:ListItem Value="232" Text="Sierra Leone (+232)"></asp:ListItem>
                                    <asp:ListItem Value="65" Text="Singapore (+65)"></asp:ListItem>
                                    <asp:ListItem Value="421" Text="Slovakia (+421)"></asp:ListItem>
                                    <asp:ListItem Value="386" Text="Slovenia (+386)"></asp:ListItem>
                                    <asp:ListItem Value="677" Text="Solomon Islands (+677)"></asp:ListItem>
                                    <asp:ListItem Value="252" Text="Somalia (+252)"></asp:ListItem>
                                    <asp:ListItem Value="27" Text="South Africa (+27)"></asp:ListItem>
                                    <asp:ListItem Value="82" Text="South Korea (+82)"></asp:ListItem>
                                    <asp:ListItem Value="211" Text="South Sudan (+211)"></asp:ListItem>
                                    <asp:ListItem Value="34" Text="Spain (+34)"></asp:ListItem>
                                    <asp:ListItem Value="94" Text="Sri Lanka (+94)"></asp:ListItem>
                                    <asp:ListItem Value="249" Text="Sudan (+249)"></asp:ListItem>
                                    <asp:ListItem Value="597" Text="Suriname (+597)"></asp:ListItem>
                                    <asp:ListItem Value="268" Text="Swaziland (+268)"></asp:ListItem>
                                    <asp:ListItem Value="46" Text="Sweden (+46)"></asp:ListItem>
                                    <asp:ListItem Value="41" Text="Switzerland (+41)"></asp:ListItem>
                                    <asp:ListItem Value="963" Text="Syria (+963)"></asp:ListItem>
                                    <asp:ListItem Value="886" Text="Taiwan (+886)"></asp:ListItem>
                                    <asp:ListItem Value="992" Text="Tajikistan (+992)"></asp:ListItem>
                                    <asp:ListItem Value="255" Text="Tanzania (+255)"></asp:ListItem>
                                    <asp:ListItem Value="66" Text="Thailand (+66)"></asp:ListItem>
                                    <asp:ListItem Value="228" Text="Togo (+228)"></asp:ListItem>
                                    <asp:ListItem Value="676" Text="Tonga (+676)"></asp:ListItem>
                                    <asp:ListItem Value="216" Text="Tunisia (+216)"></asp:ListItem>
                                    <asp:ListItem Value="90" Text="Turkey (+90)"></asp:ListItem>
                                    <asp:ListItem Value="256" Text="Uganda (+256)"></asp:ListItem>
                                    <asp:ListItem Value="380" Text="Ukraine (+380)"></asp:ListItem>
                                    <asp:ListItem Value="971" Text="United Arab Emirates (+971)"></asp:ListItem>
                                    <asp:ListItem Value="44" Text="United Kingdom (+44)"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="United States of America (+1)"></asp:ListItem>
                                    <asp:ListItem Value="598" Text="Uruguay (+598)"></asp:ListItem>
                                    <asp:ListItem Value="998" Text="Uzbekistan (+998)"></asp:ListItem>
                                    <asp:ListItem Value="678" Text="Vanuatu (+678)"></asp:ListItem>
                                    <asp:ListItem Value="58" Text="Venezuela (+58)"></asp:ListItem>
                                    <asp:ListItem Value="84" Text="Vietnam (+84)"></asp:ListItem>
                                    <asp:ListItem Value="967" Text="Yemen (+967)"></asp:ListItem>
                                    <asp:ListItem Value="260" Text="Zambia (+260)"></asp:ListItem>
                                    <asp:ListItem Value="263" Text="Zimbabwe (+263)"></asp:ListItem>

                                </asp:DropDownList>
                                <div class="invalid-feedback">
                                    Please select a valid state.                             
                                </div>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">UserId:</label>
                            <div class="col-sm-4">
                                <asp:Label ID="lblUserId" runat="server" class="form-control" />
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-2">
                            </div>
                            <div class="col-sm-2">
                                Old Rate
                            </div>
                            <div class="col-sm-2">
                                New Rate
                            </div>
                        </div>
                        <div class="form-group row">
                            <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Premium</label>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txts1" runat="server" class="form-control" placeholder="Old Price" disabled="true" />
                            </div>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txtns1" runat="server" class="form-control" placeholder="Price" ReadOnly="true" />
                            </div>
                        </div>
                        <div class="form-group row">
                            <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Link Text</label>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txts2" runat="server" class="form-control" placeholder="Old Price" disabled="true" />
                            </div>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txtns2" runat="server" class="form-control" placeholder="Price" ReadOnly="true" />
                            </div>
                        </div>
                        <div class="form-group row">
                            <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Campaign</label>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txts3" runat="server" class="form-control" placeholder="Old Price" disabled="true" />
                            </div>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txtns3" runat="server" class="form-control" placeholder="Price" ReadOnly="true" />
                            </div>
                        </div>
                        <div class="form-group row">
                            <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">OTP</label>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txts4" runat="server" class="form-control" placeholder="Old Price" disabled="true" />
                            </div>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txtns4" runat="server" class="form-control" placeholder="Price" ReadOnly="true" />
                            </div>
                        </div>
                        <div class="form-group row">
                            <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">URL</label>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txtUrlRate" runat="server" class="form-control" placeholder="Old Price" disabled="true" />
                            </div>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txtUrlRateN" runat="server" class="form-control" placeholder="Price" ReadOnly="true" />
                            </div>
                        </div>
                        <div class="form-group row">
                            <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label"><b>DLT Charges</b><span style="font-size: x-small;"> (included in SMS Rate)</span></label>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txtd1" runat="server" class="form-control" placeholder="Old Price" disabled="true" />
                            </div>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txtnd1" runat="server" class="form-control" placeholder="Price" ReadOnly="true" />
                            </div>
                        </div>

                        <%--<div class="form-group row">
                            <label for="exampleFormControlTextarea2" class="col-sm-2 col-form-label font-weight-bold">Form</label>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txtFormRate" runat="server" class="form-control" placeholder="1000" disabled="true" />
                            </div>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txtFormRateN" runat="server" class="form-control" placeholder="Price" disabled="true" />
                            </div>
                        </div>--%>

                        <div class="form-group row">
                            <div class="col-sm-2">
                            </div>

                            <div class="col-sm-2">
                                <asp:Button ID="btnAdd" runat="server" Text="Add" class="btn btn-mini" OnClick="btnAdd_Click" />

                            </div>
                            <div class="col-sm-2">
                                <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-mini" OnClick="btnSubmit_Click" />
                            </div>
                        </div>
                    </div>

                    <!-- Card End -->
                    <!--  -->
                </div>
            </div>

            <!--  -->
            <div class="row">
                <div class="col-12">
                    <div class="accordion shadow-soft rounded" id="accordionExample">
                        <div class="card card-sm card-body bg-primary border-light mb-0">

                            <a href="#collapseOne" id="headingOne" data-target="#collapseOne" class="accordion-panel-header" data-toggle="collapse" role="button" aria-expanded="true" aria-controls="collapseOne">
                                <span class="icon-title h6 mb-0 font-weight-bold"><span class="fas fa-chart-line"></span>Rate Management List</span>
                                <span class="icon"><span class="fas fa-plus"></span></span>
                            </a>

                            <div id="collapseOne" class="collapse show" aria-labelledby="headingOne" data-parent="#accordionExample" runat="server">
                                <div class="card-body px-0">
                                    <div class="row">
                                        <!-- Area Chart -->
                                        <div class="col-xl-12 col-lg-12">
                                            <div class="table-responsive">
                                                <asp:GridView ID="grv" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                                                    runat="server" Width="100%" CellPadding="10" BorderColor="#ede8e8" class="table table-striped table-bordered dt-responsive wrap dataTable-view">
                                                    <Columns>
                                                        <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr No">
                                                            <ItemTemplate>
                                                                <%#Container.DataItemIndex+1 %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Country">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCountry" runat="server" Text='<%#Eval("countrycode")%>'></asp:Label>
                                                                <asp:Label ID="lblCN" runat="server" Text='<%#Eval("countryName")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Premimum">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPremimum" runat="server" Text='<%#Eval("RATE_NORMALSMS")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Link Text">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLinkText" runat="server" Text='<%#Eval("RATE_SMARTSMS")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Campaign">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCampaign" runat="server" Text='<%#Eval("RATE_CAMPAIGN")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="OTP">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblOTP" runat="server" Text='<%#Eval("RATE_OTP")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="URL">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblURL" runat="server" Text='<%#Eval("urlrate")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="DLT">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDLT" runat="server" Text='<%#Eval("DLTCHARGE")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Action">
                                                            <ItemTemplate>

                                                                <asp:LinkButton ID="btnEdit" runat="server" OnClick="btnEdit_Click"> <i class="fa fa-edit"></i> </asp:LinkButton>
                                                                <asp:LinkButton ID="btnRemove" runat="server" OnClientClick="return confirm('Are you sure you want to delete this record?');" OnClick="btnRemove_Click"> <i class="fa fa-trash"></i> </asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>


                                                </asp:GridView>

                                            </div>
                                        </div>
                                    </div>
                                    <div class="row mt-5">
                                        <div class="col text-center">
                                            <asp:Button ID="btnSave" runat="server" Text="Save" class="btn btn-primary" OnClick="btnSave_Click" />
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

</asp:Content>
