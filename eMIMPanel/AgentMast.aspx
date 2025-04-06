<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AgentMast.aspx.cs" Inherits="eMIMPanel.AgentMast" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.16.0/umd/popper.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="card-body col-6">

            <div class="row">
                <div class="col-6">
                    <label>Agent Name</label>
                    <asp:TextBox ID="txtName" runat="server" class="form-control">
                    </asp:TextBox>
                </div>
                <div class="col-6">
                    <label>Agent Email</label>
                    <asp:TextBox ID="txtAEmail" runat="server" class="form-control">
                    </asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="col-6">
                    <label>Agent Mobile1</label>
                    <asp:TextBox ID="txtMob1" runat="server" class="form-control" onkeypress="return isNumberKey(event)">
                    </asp:TextBox>
                </div>
                <div class="col-6">
                    <label>Agent Mobile2</label>
                    <asp:TextBox ID="txtMob2" runat="server" class="form-control" onkeypress="return isNumberKey(event)">
                    </asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="col-6">
                    <label>Agent Type</label>
                    <asp:RadioButtonList ID="rbtnlist" class="form-control" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Agent" Selected="True" Value="Agent"></asp:ListItem>
                        <asp:ListItem Text="Admin" Value="Admin"></asp:ListItem>
                    </asp:RadioButtonList>
                </div>

            </div>
            <div class="row">
                <div class="mt-3 col-6">
                    <asp:Button ID="btnSave" runat="server" Text="Save" class="form-control btn-success" OnClick="btnSave_Click"></asp:Button>
                </div>
            </div>

        </div>

        <script type="text/javascript">
            function isNumberKey(evt) {
                var charCode = (evt.which) ? evt.which : event.keyCode;
                if (charCode != 46 && charCode > 31 && (charCode < 48 || charCode > 57))
                    return false;
                return true;
            }
        </script>
    </form>

</body>
</html>
