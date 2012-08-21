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
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Users;

namespace Christoc.Com.Modules.Ladder.Components
{
    [Serializable()]
    public class Game : IHydratable
    {
        public int GameId { get; set; }
        public DateTime PlayedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public int PortalId { get; set; }

        public List<Team> Teams { get; set; }

        public int CreatedByUserId { get; set; }
        public int LastUpdatedByUserId { get; set; }

        public string FieldIdentifier { get; set; }

        //Read Only Props
        ///<summary>
        /// The username of the user who created the article
        ///</summary>
        public string CreatedByUser
        {
            get
            {
                return CreatedByUserId != 0 ? UserController.GetUserById(PortalId, CreatedByUserId).Username : Null.NullString;
            }
        }

        ///<summary>
        /// The username of the user who last updated the article
        ///</summary>
        public string LastUpdatedByUser
        {
            get
            {
                return LastUpdatedByUserId != 0 ? UserController.GetUserById(PortalId, LastUpdatedByUserId).Username : Null.NullString;
            }
        }


        public Game()
        {
            Teams = new List<Team>();
            PlayedDate = LastUpdatedDate = CreatedDate = DateTime.Now;
            LastUpdatedByUserId = CreatedByUserId = -1;
            
        }

        public Game Save()
        {
            var gc = new GameController();
            return gc.SaveGame(this);
        }



        #region IHydratable Members

        void IHydratable.Fill(System.Data.IDataReader dr)
        {
            GameId = Null.SetNullInteger(dr["GameId"]);
            PlayedDate = Null.SetNullDateTime(dr["PlayedDate"]);
            CreatedDate = Null.SetNullDateTime(dr["CreatedDate"]);
            LastUpdatedDate = Null.SetNullDateTime(dr["LastUpdatedDate"]);
            CreatedByUserId = Null.SetNullInteger(dr["CreatedByUserId"]);
            LastUpdatedByUserId = Null.SetNullInteger(dr["LastUpdatedByUserId"]);
            FieldIdentifier = Null.SetNullString(dr["FieldIdentifier"]);
            PortalId = Null.SetNullInteger(dr["PortalId"]);
        }

        int IHydratable.KeyID
        {
            get
            {
                return GameId;
            }
            set
            {
                GameId = value;
            }
        }

        #endregion
    }
}