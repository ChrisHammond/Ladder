<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewGame.ascx.cs" Inherits="com.christoc.modules.ladder.Controls.ViewGame" %>

<asp:Panel ID="pnlAdmin" runat="server">
    <asp:LinkButton ID="lbDeleteGame" runat="server" resourcekey="lbDeleteGame" 
        onclick="lbDeleteGame_Click" />
</asp:Panel>

<asp:Label ID="lblGameStart" runat="server" CssClass="GameTime" />

<div class="TeamDiv">
    <asp:Label ID="lblTeam1Name" runat="server" class="Team1Name" />
    <asp:Label ID="lblTeam1Score" runat="server" class="Team1Score" />
    <div class="TeamDetails">
        <asp:PlaceHolder id="phTeam1Details" runat="server" />
    </div>
</div>

<div class="TeamDiv">
    <asp:Label ID="lblTeam2Name" runat="server" class="Team2Name" />
    <asp:Label ID="lblTeam2Score" runat="server" class="Team2Score" />
    <div class="TeamDetails">
        <asp:PlaceHolder id="phTeam2Details" runat="server" />
    </div>
</div>
