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
using com.christoc.modules.ladder.Data;

namespace DotNetNuke.Modules.ladder.Components
{
    public class GameController
    {

        /* create game workflow */

        //save game
        public Game SaveGame(Game g)
        {
            g = g.GameId > 0 ? UpdateGame(g) : CreateGame(g);

            foreach (var t in g.Teams)
            {
                var tc = new TeamController();
                tc.Save(t);
            }

            return g;
        }

        private static Game CreateGame(Game g)
        {
            g.GameId = DataProvider.Instance().AddGame(g);

            return g;

        }

        //update game
        private static Game UpdateGame(Game g)
        {
            DataProvider.Instance().UpdateGame(g);
            return g;
        }


        //todo: get game

        //todo: add teams to game



        // gameId
        // populate game data, populate collection of teams


        //get games by date

        //get games by team

        //get games by player

        //get games by....




    }
}