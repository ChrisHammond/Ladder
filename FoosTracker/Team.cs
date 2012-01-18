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
using Microsoft.SPOT;

namespace FoosTracker
{
    class Team
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public int Score { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastUpdated { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }

        /*future properties */
        //team members

        
        public static Team CreateTeam(int teamId, string teamName)
        {
            var t = new Team();
            t.TeamId = teamId;
            t.TeamName = teamName;
            t.DateCreated = DateTime.Now;
            t.LastUpdated = DateTime.Now;
            t.Score = 0;
            t.Wins = 0;
            t.Losses = 0;
            return t;
        }

        public void AddScore()
        {
            Score += 1;
        }
        public void SubtractScore()
        {
            if(Score>0)
                Score -= 1;
        }
    }
}
