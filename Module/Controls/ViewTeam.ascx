<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewTeam.ascx.cs" Inherits="com.christoc.modules.ladder.Controls.ViewTeam" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelcontrol.ascx" %>

<asp:Panel ID="pnlAdmin" runat="server" Enabled="false">
    <asp:HyperLink ID="hlManageTeam" runat="server" resourcekey="hlManageTeam" />
</asp:Panel>


<h2><asp:Label ID="lblTeamName" runat="server" /></h2>

<dnn:label id="lblWins" runat="server" /><asp:Label ID="lblWinsValue" runat="server" />
<dnn:label id="lblLosses" runat="server" /><asp:Label ID="lblLossesValue" runat="server" />




