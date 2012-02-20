using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.christoc.modules.ladder.Components;
using com.christoc.modules.ladder.Data;
using DotNetNuke.Entities.Users;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;

namespace com.christoc.modules.ladder.Controls
{
    public partial class PlayerList : ladderModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    Localization.LocalizeGridView(ref gvListOfNonPlayers, LocalResourceFile);

                    BindPlayerList();
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void BindPlayerList()
        {
            gvListOfNonPlayers.DataSource = DataProvider.Instance().GetNonPlayers(PortalId);
            gvListOfNonPlayers.DataBind();
        }


        protected void gbListOfNonPlayers_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            //TODO: we need to bind the user info so that we can allow the add button to be clicked.
            //todo: we need to designate current game
            //todo: we need to designate last game

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var lbAdd = e.Row.FindControl("lbAdd") as LinkButton;

                var curUserId = DataBinder.Eval(e.Row.DataItem, "UserId");
                if (lbAdd != null)
                {
                    lbAdd.CommandName = "AddPlayer";
                    lbAdd.CommandArgument = curUserId.ToString();
                }
            }
        }

        protected void gbListOfNonPlayers_OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
            var userid = e.CommandArgument;

            var newP = new Player {UserId = Convert.ToInt32(userid), PortalId = PortalId};
            var pc = new PlayerController();
            pc.Save(newP);
            //todo: refresh the grid to remove the recently added player
            
        }


        //
        //add a player
        //if (e.Row.DataItem.GetType() == UserInfo.GetType())
        //{
        //    
        //}
    }
}