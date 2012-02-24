/*
' Copyright (c) 2010-2012 Christoc.com
' 
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
' 
' The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
' 
*/

using System;
using System.Web.UI.WebControls;
using com.christoc.modules.ladder.Components;
using DotNetNuke.Services.Exceptions;

namespace com.christoc.modules.ladder.Controls
{
    public partial class GameList : LadderModuleBase
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