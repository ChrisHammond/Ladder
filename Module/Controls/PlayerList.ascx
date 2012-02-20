﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PlayerList.ascx.cs" Inherits="com.christoc.modules.ladder.Controls.PlayerList" %>
<asp:GridView ID="gvListOfNonPlayers" runat="server" AutoGenerateColumns="false" OnRowDataBound="gbListOfNonPlayers_OnRowDataBound" OnRowCommand="gbListOfNonPlayers_OnRowCommand">
<Columns>
    <asp:BoundField DataField="UserId" HeaderText="UserId" />
    <asp:BoundField DataField="DisplayName" HeaderText="DisplayName" />
    <asp:TemplateField>
        <ItemTemplate>
            <asp:LinkButton ID="lbAdd" runat="server" resourcekey="Add" />
        </ItemTemplate>
    </asp:TemplateField>
</Columns>
</asp:GridView>