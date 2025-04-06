<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="qry28.aspx.cs" Inherits="eMIMPanel.qry28" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        
 <div>
            <asp:TextBox ID="txtQ" runat="server" Text="" width="100%" TextMode="MultiLine" Rows="10">

            </asp:TextBox>

        </div>
        <div>
            <asp:Button ID="btnok"  runat="server" Style="font-size:xx-large;" OnClick="btnok_Click" Text="Execute" />
        </div>
        <div>
            <asp:GridView ID="grd" runat="server" >

            </asp:GridView>
        </div>
    </form>
</body>
</html>
