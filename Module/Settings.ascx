<%@ Control Language="C#" AutoEventWireup="false" Inherits="com.christoc.modules.ladder.Settings" Codebehind="Settings.ascx.cs" %>


<%@ Register TagName="label" TagPrefix="dnn" Src="~/controls/labelcontrol.ascx" %>
<h2 id="dnnSitePanel-BasicSettings" class="dnnFormSectionHead">
    <a href="" class="dnnSectionExpanded">
        <%=LocalizeString("BasicSettings")%></a></h2>
<fieldset>
    <div class="dnnFormItem">
        <dnn:label ID="lblMaxPerTeam" runat="server" ControlName="txtMaxPerTeam" />
        <asp:TextBox ID="txtMaxPerTeam" runat="server" />
    </div>
</fieldset>