<%@ Control Language="C#" Inherits="com.christoc.modules.ladder.View" AutoEventWireup="false"
    CodeBehind="View.ascx.cs" %>
<asp:GridView ID="gvGames" runat="server" AutoGenerateColumns="true" />
<asp:Repeater ID="rptGames" runat="server" OnItemDataBound="RptGamesOnItemDataBound">
    <HeaderTemplate>
        <ul class="ladder_game">
    </HeaderTemplate>
    <ItemTemplate>
        <li class="GameItem">
            <asp:Label ID="lblGameStart" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"PlayedDate").ToString() %>'
                CssClass="GameTime" />
            <div class="TeamDiv">
                <asp:Label ID="lblTeam1Name" runat="server" class="Team1Name" />
                <asp:Label ID="lblTeam1Score" runat="server" class="Team1Score" />
            </div>
            <div class="TeamDiv">
                <asp:Label ID="lblTeam2Name" runat="server" class="Team2Name" />
                <asp:Label ID="lblTeam2Score" runat="server" class="Team2Score" />
            </div>
        </li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater>
