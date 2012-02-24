<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageGame.ascx.cs"
    Inherits="com.christoc.modules.ladder.Controls.ManageGame" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelcontrol.ascx" %>
<h2 id="dnnSitePanel-Team1" class="dnnFormSectionHead">
    <a href="" class="dnnSectionExpanded">
        <%=LocalizeString("Team1")%></a></h2>
<fieldset>
    <div class="dnnFormItem">
        <dnn:label ID="lblTeam1" runat="server" />
        <asp:DropDownList ID="ddlTeam1" runat="server" DataTextField="Name" DataValueField="TeamId" />
    </div>
    <div class="dnnFormItem">
        <dnn:label ID="lblTeam1Score" runat="server" />
        <asp:TextBox ID="txtTeam1Score" runat="server" />
    </div>
    <div class="dnnFormItem">
        <dnn:label ID="lblTeam1IsHome" runat="server" />
        <asp:CheckBox ID="chkTeam1IsHome" runat="server" />
    </div>
</fieldset>
<h2 id="dnnSitePanel-Team2" class="dnnFormSectionHead">
    <a href="" class="dnnSectionExpanded">
        <%=LocalizeString("Team2")%></a></h2>
<fieldset>
    <div class="dnnFormItem">
        <dnn:label ID="lblTeam2" runat="server" />
        <asp:DropDownList ID="ddlTeam2" runat="server" DataTextField="Name" DataValueField="TeamId" />
    </div>
    <div class="dnnFormItem">
        <dnn:label ID="lblTeam2Score" runat="server" />
        <asp:TextBox ID="txtTeam2Score" runat="server" />
    </div>
    <div class="dnnFormItem">
        <dnn:label ID="lblTeam2IsHome" runat="server" />
        <asp:CheckBox ID="chkTeam2IsHome" runat="server" />
    </div>
</fieldset>
<asp:LinkButton ID="lbSaveGame" runat="server" resourcekey="lbSaveGame" CssClass="dnnPrimaryAction"
    OnClick="lbSaveGame_Click" />
<asp:LinkButton ID="lbCancel" runat="server" resourcekey="lbCancel" CssClass="dnnSecondaryAction"
    OnClick="lbCancel_Click" />
