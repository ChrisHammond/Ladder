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
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;

namespace com.christoc.modules.ladder.Components
{
    public class Player : IHydratable
    {
        public int PlayerId { get; set; }
        public int UserId { get; set; }
        public int Rank { get; set; }
        public int Games { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
                

        #region IHydratable Members

        void IHydratable.Fill(System.Data.IDataReader dr)
        {
            PlayerId = Null.SetNullInteger(dr["playerId"]);
            UserId = Null.SetNullInteger(dr["userId"]);
            Rank = Null.SetNullInteger(dr["rank"]);
            Games = Null.SetNullInteger(dr["games"]);
            Wins = Null.SetNullInteger(dr["wins"]);
            Losses = Null.SetNullInteger(dr["losses"]);

        }

        int IHydratable.KeyID
        {
            get
            {
                return PlayerId;
            }
            set
            {
                PlayerId = value;
            }
        }

        #endregion
    }
}