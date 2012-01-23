<%@ Control Language="C#" AutoEventWireup="false" Inherits="com.christoc.modules.ladder.Settings" Codebehind="Settings.ascx.cs" %>


<%@ Register TagName="label" TagPrefix="dnn" Src="~/controls/labelcontrol.ascx" %>
<h2 id="dnnSitePanel-BasicSettings" class="dnnFormSectionHead">
    <a href="" class="dnnSectionExpanded">
        <%=LocalizeString("BasicSettings")%></a></h2>
<fieldset>
    <div class="dnnFormItem">
        <dnn:label ID="Setting1" runat="server" ControlName="txtSetting1" />
        <asp:TextBox ID="txtSetting1" runat="server" />
    </div>
</fieldset>