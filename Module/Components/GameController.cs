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
    public class GameController
    {

        /* create game workflow */

        //save game
        public Game SaveGame(Game g)
        {
            bool newGame = true;
            if(g.GameId>0)
                newGame = false;

            g = g.GameId > 0 ? UpdateGame(g) : CreateGame(g);

            foreach (var t in g.Teams)
            {
                var tc = new TeamController();
                //configure the portalId
                t.PortalId = g.PortalId;
                t.LastPlayed = DateTime.Now;
                t.TeamId = tc.SaveTeam(t).TeamId;
                //add GameTeam relationship to store the Scores

                //TODO: figure out how to flag a WIN and HOME team
                if(newGame)
                    DataProvider.Instance().AddGameTeam(g.GameId,t.TeamId,t.Score,false,t.HomeTeam);
                else
                    DataProvider.Instance().UpdateGameTeam(g.GameId, t.TeamId, t.Score, false, t.HomeTeam);

            }
            
            return g;
        }

        private Game CreateGame(Game g)
        {
            g.GameId = DataProvider.Instance().AddGame(g);

            return g;

        }

        //update game
        private Game UpdateGame(Game g)
        {
            DataProvider.Instance().UpdateGame(g);
            return g;
        }


        public Game GetGame (int gameId)
        {
            //TODO: get the team info
            return CBO.FillObject<Game>(DataProvider.Instance().GetGame(gameId));
        }


        //TODO: we're going to need to filter by date range, team, etc
        
        public List<Game> GetGames (int PortalId, bool populateAll)
        {
            //check if we should populate teams and players
            if(populateAll)
            {
                var listOfGames = CBO.FillCollection<Game>(DataProvider.Instance().GetGames(PortalId));

                var outputGames = new List<Game>();

                foreach (var log in listOfGames)
                {
                    //get the teams
                    var tc = new TeamController();
                    foreach( Team t in tc.GetTeamsByGame(log.GameId))
                    {
                        log.Teams.Add(t);
                    }
                    outputGames.Add(log);    
                    //get the players
                }

                return listOfGames;
            }
            return CBO.FillCollection<Game>(DataProvider.Instance().GetGames(PortalId));
        }
        
        // gameId
        // populate game data, populate collection of teams


        //get games by date

        //get games by team

        //get games by player

        //get games by....




    }
}