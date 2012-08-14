<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GameList.ascx.cs" Inherits="Christoc.Com.Modules.Ladder.Controls.GameList" %>
<asp:Repeater ID="rptGames" runat="server" OnItemDataBound="RptGamesOnItemDataBound">
    <HeaderTemplate>
        <ul class="ladder_game">
    </HeaderTemplate>
    <ItemTemplate>
        <li class="GameItem">
            <asp:HyperLink ID="hlGameLink" runat="server" resourcekey="hlGameLink" />
            <asp:Label ID="lblGameStart" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"PlayedDate").ToString() %>'
                CssClass="GameTime" />
            <asp:Panel ID="pnlTeam1" CssClass="TeamDiv" runat="server">
                <asp:Label ID="lblTeam1Name" runat="server" class="Team1Name" />
                <asp:Label ID="lblTeam1Score" runat="server" class="Team1Score" />
            </asp:Panel>
            <asp:Panel ID="pnlTeam2" CssClass="TeamDiv" runat="server">
                <asp:Label ID="lblTeam2Name" runat="server" class="Team2Name" />
                <asp:Label ID="lblTeam2Score" runat="server" class="Team2Score" />
            </asp:Panel>
        </li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater>
