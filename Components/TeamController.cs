/*
' Copyright (c) 2010  Christoc.com
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.christoc.modules.ladder.Data;
using DotNetNuke.Common.Utilities;

namespace DotNetNuke.Modules.ladder.Components
{
    public class TeamController
    {

        //create team
        public Team CreateTeam(Team t)
        {
            //create the team and updated with the new TeamId
            t.TeamId = DataProvider.Instance().CreateTeam(t);
            return t;
        }

        //update team

        public void UpdateTeam(Team t)
        {
            DataProvider.Instance().UpdateTeam(t);
        }

        //get players

        public List<Player> GetPlayers(int teamId)
        {
            var pl = new List<Player>();
            var dr = DataProvider.Instance().GetTeamPlayers(teamId);
            
            while(dr.Read())
            {
                var pc = new PlayerController();
                pl.Add(pc.GetPlayer(Convert.ToInt32(dr["PlayerId"])));
            }
            return pl;
        }

        //add player
        public void AddPlayer(int teamId, int playerId)
        {
            DataProvider.Instance().AddTeamPlayer(teamId, playerId);
        }

        //delete team player

        public Team GetTeam(int teamId)
        {
            var t = CBO.FillObject<Team>(DataProvider.Instance().GetTeam(teamId));

            //populate collection of players for team
            t.Players = GetPlayers(t.TeamId);
            return t;
        }

        //get all teams
        public List<Team> GetTeams()
        {
            return null;
        }

        //get record for a team


    }
}