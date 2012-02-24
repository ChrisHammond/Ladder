<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewGame.ascx.cs" Inherits="com.christoc.modules.ladder.Controls.ViewGame" %>

<asp:Panel ID="pnlAdmin" runat="server">
    <asp:LinkButton ID="lbDeleteGame" runat="server" resourcekey="lbDeleteGame" 
        onclick="LbDeleteGameClick" />
        <asp:hyperlink ID="lbManageGame" runat="server" resourcekey="lbManageGame" />
</asp:Panel>

<asp:Label ID="lblGameStart" runat="server" CssClass="GameTime" />

<div class="TeamDiv">
    <asp:HyperLink ID="lblTeam1Link" runat="server" CssClass="Team1Name"/>
    <asp:Label ID="lblTeam1Score" runat="server" class="Team1Score" />
    <div class="TeamDetails">
        <asp:PlaceHolder id="phTeam1Details" runat="server" />
    </div>
</div>

<div class="TeamDiv">
    <asp:HyperLink ID="lblTeam2Link" runat="server" CssClass="Team2Name"/>
    <asp:Label ID="lblTeam2Score" runat="server" class="Team2Score" />
    <div class="TeamDetails">
        <asp:PlaceHolder id="phTeam2Details" runat="server" />
    </div>
</div>
