<%@ Control language="C#" Inherits="com.christoc.modules.LadderTester.View" AutoEventWireup="false"  Codebehind="View.ascx.cs" %>
Server URL <asp:TextBox ID="txtServerUrl" runat="server" /> <br />
GameID <asp:TextBox ID="txtGameId" runat="server" /> <br />
Home Team Score <asp:TextBox ID="txtHomeTeam" runat="server" /> <br />
Away Team Score <asp:TextBox ID="txtAwayTeam" runat="server" /> <br />

Field Identifier <asp:TextBox id="txtFieldIdentifier" runat="server" /> <br />
<br />
<asp:linkButton ID="lbSubmit" runat="server" onclick="lbSubmit_Click" Text="submit" CssClass="dnnPrimaryAction" /><br />
Generated JSON
<br /><asp:TextBox ID="txtGameJson" runat="server" TextMode="MultiLine" Columns="50" Rows="15" />

<br />
Result: <asp:TextBox ID="txtResult" runat="server"/>