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
using System.Collections.Generic;
using com.christoc.modules.ladder.Data;
using DotNetNuke.Common.Utilities;

namespace com.christoc.modules.ladder.Components
{
    public class PlayerController
    {

        public Player Save(Player p)
        {
            if (p.PlayerId < 1)
                p = CreatePlayer(p.UserId);
            else
            {
                UpdatePlayer(p);
            }
            return p;
        }

        //create player
        public Player CreatePlayer(int playerId)
        {
            var p = new Player
                        {
                            PlayerId = DataProvider.Instance().CreatePlayer(playerId),
                            UserId= playerId,    
                            Rank = 0,
                            Games = 0,
                            Losses = 0,
                            Wins = 0
                        };
            UpdatePlayer(p);
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

        public List<Player> GetPlayers(int portalId)
        {
            return CBO.FillCollection<Player>(DataProvider.Instance().GetPlayers(portalId));
        }

        
    }
}