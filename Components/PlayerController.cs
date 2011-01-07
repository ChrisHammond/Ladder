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
    public class PlayerController
    {

        //create player
        public Player CreatePlayer(int playerId)
        {
            var p = new Player
                        {
                            UserId = DataProvider.Instance().CreatePlayer(playerId),
                            Rank = 0,
                            Games = 0,
                            Losses = 0,
                            Wins = 0
                        };
            return p;
        }

        //update player
        public void UpdatePlayer(Player p)
        {
            DataProvider.Instance().UpdatePlayer(p);
        }

        //get player info
        public Player GetPlayer(int playerId)
        {
            return CBO.FillObject<Player>(DataProvider.Instance().GetPlayer(playerId));
        }

    }
}