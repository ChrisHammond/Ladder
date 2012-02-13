using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.christoc.modules.ladder.Components;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Utilities;

namespace com.christoc.modules.ladder.Controls
{
    public partial class ViewGame : ladderModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                pnlAdmin.Visible = IsEditable;
                ClientAPI.AddButtonConfirm(lbDeleteGame, Localization.GetString("DeleteConfirm", LocalResourceFile));

                var gc = new GameController();
                var currentGame = gc.GetGame(GameId);
                if(currentGame!=null)
                {
                    lblGameStart.Text = currentGame.PlayedDate.ToShortDateString();
                    lblTeam1Link.NavigateUrl = GetTeamLink(currentGame.Teams[0].TeamId);
                    lblTeam1Link.Text = currentGame.Teams[0].Name;
                    lblTeam1Score.Text = currentGame.Teams[0].Score.ToString();
                    lblTeam2Link.NavigateUrl = GetTeamLink(currentGame.Teams[1].TeamId);
                    lblTeam2Link.Text = currentGame.Teams[1].Name;
                    lblTeam2Score.Text = currentGame.Teams[1].Score.ToString();
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this,exc);
            }
        }

        protected void lbDeleteGame_Click(object sender, EventArgs e)
        {
            if(GameId>0)
            {
                var gc = new GameController();
                gc.DeleteGame(GameId);
                Response.Redirect(DotNetNuke.Common.Globals.NavigateURL());
            }
        }
    }
}