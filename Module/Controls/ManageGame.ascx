<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageGame.ascx.cs"
    Inherits="Christoc.Com.Modules.Ladder.Controls.ManageGame" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/labelcontrol.ascx" %>



<div class="dnnForm dnnFoosManageGame dnnClear" id="dnnFoosManageGame">
    <div class="dnnFormExpandContent"><a href=""><%=LocalizeString("ExpandAll")%></a></div>

    <h2 id="H1" class="dnnFormSectionHead">
    <a href="" class="dnnSectionExpanded">
        <%=LocalizeString("GeneralGameSettings")%></a></h2>
<fieldset>
    <div class="dnnFormItem">
        <dnn:label ID="lblGameDate" runat="server" ControlName="txtGameDate" />
        <asp:TextBox ID="txtGameDate" runat="server" />
    </div>

</fieldset>


<h2 id="dnnSitePanel-Team1" class="dnnFormSectionHead">
    <a href="" class="dnnSectionExpanded">
        <%=LocalizeString("Team1")%></a></h2>
<fieldset>
    <div class="dnnFormItem">
        <dnn:label ID="lblTeam1" runat="server" ControlName="ddlTeam1" />
        <asp:DropDownList ID="ddlTeam1" runat="server" DataTextField="Name" DataValueField="TeamId" />
    </div>
    <div class="dnnFormItem">
        <dnn:label ID="lblTeam1Score" runat="server" ControlName="txtTeam1Score" />
        <asp:TextBox ID="txtTeam1Score" runat="server" />
    </div>
    <div class="dnnFormItem">
        <dnn:label ID="lblTeam1IsHome" runat="server" ControlName="chkTeam1IsHome" />
        <asp:CheckBox ID="chkTeam1IsHome" runat="server" />
    </div>
</fieldset>
<h2 id="dnnSitePanel-Team2" class="dnnFormSectionHead">
    <a href="" class="dnnSectionExpanded">
        <%=LocalizeString("Team2")%></a></h2>
<fieldset>
    <div class="dnnFormItem">
        <dnn:label ID="lblTeam2" runat="server"  ControlName="ddlTeam2"/>
        <asp:DropDownList ID="ddlTeam2" runat="server" DataTextField="Name" DataValueField="TeamId" />
    </div>
    <div class="dnnFormItem">
        <dnn:label ID="lblTeam2Score" runat="server" ControlName="txtTeam2Score" />
        <asp:TextBox ID="txtTeam2Score" runat="server" />
    </div>
    <div class="dnnFormItem">
        <dnn:label ID="lblTeam2IsHome" runat="server" ControlName="chkTeam2IsHome" />
        <asp:CheckBox ID="chkTeam2IsHome" runat="server" />
    </div>
</fieldset>
<asp:LinkButton ID="lbSaveGame" runat="server" resourcekey="lbSaveGame" CssClass="dnnPrimaryAction"
    OnClick="lbSaveGame_Click" />
<asp:LinkButton ID="lbCancel" runat="server" resourcekey="lbCancel" CssClass="dnnSecondaryAction"
    OnClick="lbCancel_Click" />

    </div>


<script language="javascript" type="text/javascript">
    /*globals jQuery, window, Sys */
    (function ($, Sys) {
        function dnnFoosSettings() {
            $('#dnnFoosManageGame').dnnPanels();
            $('#dnnFoosManageGame .dnnFormExpandContent a').dnnExpandAll({ expandText: '<%=Localization.GetString("ExpandAll", LocalResourceFile)%>', collapseText: '<%=Localization.GetString("CollapseAll", LocalResourceFile)%>', targetArea: '#dnnFoosManageGame' });
        }

        $(document).ready(function () {
            dnnFoosSettings();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () {
                dnnFoosSettings();
            });
        });

    } (jQuery, window.Sys));
</script>