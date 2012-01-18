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
using com.christoc.modules.ladder.Data;
using DotNetNuke.Common.Utilities;

namespace DotNetNuke.Modules.ladder.Components
{
    public class TeamController
    {

        public Team Save(Team t)
        {
            if(t.TeamId>0)
                UpdateTeam(t);
            else
            {
                t = CreateTeam(t);
            }
            return t;
        }
        //create team
        private Team CreateTeam(Team t)
        {
            //create the team and updated with the new TeamId
            t.TeamId = DataProvider.Instance().CreateTeam(t);
            //since Team is created add the players
            AddPlayers(t);
            return t;
        }

        //update team

        public void UpdateTeam(Team t)
        {
            DataProvider.Instance().UpdateTeam(t);
            AddPlayers(t);
        }

        //get players for a team
        public List<Player> GetPlayers(int teamId)
        {
            var pl = new List<Player>();
            var dr = DataProvider.Instance().GetTeamPlayers(teamId);

            while (dr.Read())
            {
                var pc = new PlayerController();
                pl.Add(pc.GetPlayer(Convert.ToInt32(dr["PlayerId"])));
            }
            return pl;
        }

        //add player
        public void AddTeamPlayer(int teamId, int playerId)
        {
            //the SQL makes sure not to add the same player twice
            DataProvider.Instance().AddTeamPlayer(teamId, playerId);
        }

        public void AddPlayers(Team t)
        {            
            foreach (Player p in t.Players)
            {
                AddTeamPlayer(t.TeamId,p.PlayerId);
            }
        }


        //todo: delete team player

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