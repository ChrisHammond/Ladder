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
using com.christoc.modules.ladder.Components;
using DotNetNuke.Services.Exceptions;

namespace com.christoc.modules.ladder.Controls
{
    public partial class ManageGame : LadderModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //TODO: allow for creating a game
            //TODO: fix expand panels on view control

            try
            {
                //LoadTeamLists
                if (!Page.IsPostBack)
                {
                    LoadAvailableTeams();
                    //load the game
                    var gc = new GameController();
                    if (1 == 1)
                    {
                        var currentGame = gc.GetGame(GameId);
                        if (currentGame != null)
                        {
                            ddlTeam1.Items.FindByValue(currentGame.Teams[0].TeamId.ToString()).Selected = true;
                            txtTeam1Score.Text = currentGame.Teams[0].Score.ToString();
                            chkTeam1IsHome.Checked = currentGame.Teams[0].HomeTeam;
                            ddlTeam2.Items.FindByValue(currentGame.Teams[1].TeamId.ToString()).Selected = true;
                            txtTeam2Score.Text = currentGame.Teams[1].Score.ToString();
                            chkTeam2IsHome.Checked = currentGame.Teams[1].HomeTeam;

                            txtGameDate.Text = currentGame.PlayedDate.ToString();
                        }
                        else
                        {
                            txtGameDate.Text = DateTime.Now.ToString();
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void LoadAvailableTeams()
        {
            //get a list of all teams
            var tc = new TeamController();
            ddlTeam1.DataSource = ddlTeam2.DataSource = tc.GetTeams(PortalId);
            ddlTeam1.DataBind();
            ddlTeam2.DataBind();

        }

        protected void lbCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(GetGameLink(GameId));
        }

        protected void lbSaveGame_Click(object sender, EventArgs e)
        {
            //save the game

            var gc = new GameController();

            var currentGame = gc.GetGame(GameId) ?? new Game();

            currentGame.Teams.Clear();
            var tc = new TeamController();
            var team1 = tc.GetTeam(Convert.ToInt32(ddlTeam1.SelectedValue));

            team1.Score = Convert.ToInt32(txtTeam1Score.Text);
            team1.HomeTeam = chkTeam1IsHome.Checked;

            var team2 = tc.GetTeam(Convert.ToInt32(ddlTeam2.SelectedValue));

            team2.Score = Convert.ToInt32(txtTeam2Score.Text);
            team2.HomeTeam = chkTeam2IsHome.Checked;

            currentGame.Teams.Clear();
            currentGame.Teams.Add(team1);
            currentGame.Teams.Add(team2);

            //check if we're creating a new game and assign the current user as the Creator

            if (currentGame.CreatedByUserId < 0)
                currentGame.CreatedByUserId = UserId;

            currentGame.PlayedDate = Convert.ToDateTime(txtGameDate.Text);

            currentGame.LastUpdatedByUserId = UserId;
            currentGame.PortalId = PortalId;
            currentGame.Save();

            Response.Redirect(GetGameLink(currentGame.GameId));
        }
    }
}