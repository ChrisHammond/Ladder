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
using System.Collections;
using System.Web.UI.WebControls;
using Christoc.Com.Modules.Ladder.Components;

using DotNetNuke.Services.Exceptions;

namespace Christoc.Com.Modules.Ladder.Controls
{
    public partial class ManageTeam : LadderModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    //get a list of players that are available
                    var pc = new PlayerController();
                    var availPlayerList = new ArrayList();
                    var teamPlayerList = new ArrayList();

                    foreach (var player in pc.GetPlayers(PortalId))
                    {
                        availPlayerList.Add(player);
                    }

                    var tc = new TeamController();
                    if (TeamId > 0)
                    {
                        var curTeam = tc.GetTeam(TeamId);

                        //populate team name
                        txtTeamName.Text = curTeam.Name;

                        //populate list of players
                        foreach (var p in curTeam.Players)
                        {
                            teamPlayerList.Add(p);
                        }
                    }

                    dlPlayers.Available = availPlayerList;
                    dlPlayers.Assigned = teamPlayerList;
                    dlPlayers.DataBind();
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        protected void LbSaveClick(object sender, EventArgs e)
        {
            if (Settings.Contains("MaxPerTeam"))
            {
                if (dlPlayers.Assigned.Count < Convert.ToInt32(Settings["MaxPerTeam"])+1)
                {
                    
                    //TODO: don't allow more than two players on a team

                    //save the team

                    var curTeam = new Team();

                    var tc = new TeamController();
                    if (TeamId > 0)
                    {
                        curTeam = tc.GetTeam(TeamId);
                    }
                    curTeam.Name = txtTeamName.Text;

                    //populate list of players
                    var pc = new PlayerController();
                    foreach (var player in dlPlayers.Assigned)
                    {
                        var li = (ListItem)player;

                        var curPlayer = pc.GetPlayer(Convert.ToInt32(li.Value));
                        curTeam.Players.Add(curPlayer);
                    }
                    curTeam.PortalId = PortalId;
                    tc.SaveTeam(curTeam);
                    Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(TabId));
                }
            }
        }

        protected void LbCancelClick(object sender, EventArgs e)
        {
            //todo:where should we go? currently redirecting to the tabid
            Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(TabId));
        }
    }
}