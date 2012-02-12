using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.christoc.modules.ladder.Components;
using DotNetNuke.Services.Exceptions;

namespace com.christoc.modules.ladder.Controls
{
    public partial class GameList : ladderModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                var gc = new GameController();

                //load games into the gvGames grid view
                var listOfGames = gc.GetGames(PortalId, 5, true);
                //TODO: using game.Teams figure out how to display scores

                rptGames.DataSource = listOfGames;
                rptGames.DataBind();
                
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }
        protected void RptGamesOnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //todo: we need to designate current game
            //todo: we need to designate last game

            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                var lblTeam1Score = e.Item.FindControl("lblTeam1Score") as Label;
                var lblTeam2Score = e.Item.FindControl("lblTeam2Score") as Label;
                var lblTeam1Name = e.Item.FindControl("lblTeam1Name") as Label;
                var lblTeam2Name = e.Item.FindControl("lblTeam2Name") as Label;
                var hlGameLink = e.Item.FindControl("hlGameLink") as HyperLink;


                var curGame = (Game)e.Item.DataItem;
                if (lblTeam1Score != null) lblTeam1Score.Text = curGame.Teams[0].Score.ToString();
                if (lblTeam1Name != null) lblTeam1Name.Text = curGame.Teams[0].Name;
                if (lblTeam2Score != null) lblTeam2Score.Text = curGame.Teams[1].Score.ToString();
                if (lblTeam2Name != null) lblTeam2Name.Text = curGame.Teams[1].Name;
                if (hlGameLink != null) hlGameLink.NavigateUrl = GetGameLink(curGame.GameId);
            }
        }
    }

}