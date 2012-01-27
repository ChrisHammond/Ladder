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
using System.Collections.Generic;
using com.christoc.modules.ladder.Data;
using DotNetNuke.Common.Utilities;

namespace com.christoc.modules.ladder.Components
{
    public class TeamController
    {

        public Team SaveTeam(Team t)
        {
            //todo: we need to look to see if the team already exists, if so update their info on the object before saving.
            var lookup = GetTeamByName(t.Name);
            if(lookup!=null)
            {
                t.TeamId = lookup.TeamId;
                t.Losses += lookup.Losses;
                t.Wins += lookup.Wins;
                t.Games = lookup.Wins;
                t.FirstPlayed = lookup.FirstPlayed;
                t.LastPlayed = lookup.LastPlayed;
            }
            
            //if the team has an ID we are updating them, otherwise we are creating them
            if(t.TeamId>0)
                UpdateTeam(t);
            else
                t = CreateTeam(t);           
            return t;

            //todo: save players?
        }
        //create team
        private Team CreateTeam(Team t)
        {
            t.Wins = t.Losses = 0;
            
            t.CreatedByUserId = 1;
            t.LastUpdatedByUserId = 1;
            t.CreatedDate = DateTime.Now;
            t.LastUpdatedDate = DateTime.Now;
            t.FirstPlayed = DateTime.Now;
            t.LastPlayed = DateTime.Now;
            //create the team and updated with the new TeamId
            t.TeamId = DataProvider.Instance().CreateTeam(t);
            //since Team is created add the players
            AddPlayers(t);
            return t;
        }

        //update team

        public void UpdateTeam(Team t)
        {
            t.LastUpdatedByUserId = 1;

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

        public Team GetTeamByName(string teamName)
        {
            var t = CBO.FillObject<Team>(DataProvider.Instance().GetTeam(teamName));
            if(t!=null)
                t.Players = GetPlayers(t.TeamId);            //populate collection of players for team
            return t;
            
        }

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
            return CBO.FillCollection<Team>(DataProvider.Instance().GetTeams());
        }

        //get record for a team
    }
}