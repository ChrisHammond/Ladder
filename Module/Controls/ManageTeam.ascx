<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageTeam.ascx.cs"
    Inherits="com.christoc.modules.ladder.Controls.ManageTeam" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelcontrol.ascx" %>
<%@ Register TagPrefix="dnn" TagName="duallist" Src="~/controls/duallistcontrol.ascx" %>
<div class="dnnForm ladderManageTeam dnnClear" id="ladderManageTeam">
    <div class="dnnFormExpandContent">
        <a href="">
            <%=LocalizeString("ExpandAll")%></a></div>
    <h2 id="dnnSitePanel-BasicSettings" class="dnnFormSectionHead">
        <a href="" class="dnnSectionExpanded">
            <%=LocalizeString("TeamSettings")%></a></h2>
    <fieldset>
        <div class="dnnFormItem">
            <dnn:label id="lblTeamName" controlname="lblTeamName" runat="server" />
            <asp:TextBox ID="txtTeamName" runat="server" Columns="50" /><asp:RequiredFieldValidator
                ID="rfvTeamName" runat="server" ControlToValidate="txtTeamName" CssClass="NormalRed" />
        </div>
        <div class="dnnFormItem">
            <dnn:label id="lblPlayers" controlname="lblPlayers" runat="server" />
            <dnn:duallist runat="server" id="dlPlayers" />

        </div>

    </fieldset>

</div>
