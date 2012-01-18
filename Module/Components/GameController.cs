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