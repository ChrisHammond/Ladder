<%@ Control language="C#" Inherits="com.christoc.modules.LadderTester.View" AutoEventWireup="false"  Codebehind="View.ascx.cs" %>

Enter JSON
<br /><asp:TextBox ID="txtGameJson" runat="server" TextMode="MultiLine" Columns="50" Rows="15" />
<br />
<asp:linkButton ID="lbSubmit" runat="server" onclick="lbSubmit_Click" />

<br />
Result: <asp:TextBox ID="txtResult" runat="server"/>