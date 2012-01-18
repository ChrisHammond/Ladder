using System;
using System.Collections.Generic;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;

namespace DotNetNuke.Modules.ladder.Components
{
    public class Team : IHydratable
    {
        public int TeamId { get; set; }
        public string Name { get; set; }
        public DateTime FirstPlayed { get; set; }
        public DateTime LastPlayed { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public int Games { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }


        public int CreatedByUserId { get; set; }
        public int LastUpdatedByUserId { get; set; }
        public int PortalId { get; set; }

        public List<Player> Players { get; set; }

        //Read Only Props
        ///<summary>
        /// The username of the user who created the article
        ///</summary>
        public string CreatedByUser
        {
            get
            {
                return CreatedByUserId != 0 ? Entities.Users.UserController.GetUserById(PortalId, CreatedByUserId).Username : Null.NullString;
            }
        }

        ///<summary>
        /// The username of the user who last updated the article
        ///</summary>
        public string LastUpdatedByUser
        {
            get
            {
                return LastUpdatedByUserId != 0 ? Entities.Users.UserController.GetUserById(PortalId, LastUpdatedByUserId).Username : Null.NullString;
            }
        }

        #region IHydratable Members

        void IHydratable.Fill(System.Data.IDataReader dr)
        {
            TeamId = Null.SetNullInteger(dr["TeamId"]);
            Name = Null.SetNullString(dr["Name"]);
            FirstPlayed = Null.SetNullDateTime(dr["FirstPlayed"]);
            LastPlayed = Null.SetNullDateTime(dr["LastPlayed"]);
            CreatedDate = Null.SetNullDateTime(dr["CreatedDate"]);
            LastUpdatedDate = Null.SetNullDateTime(dr["LastUpdatedDate"]);
            Games = Null.SetNullInteger(dr["Games"]);
            Wins = Null.SetNullInteger(dr["Wins"]);
            Losses = Null.SetNullInteger(dr["Losses"]);

            CreatedByUserId = Null.SetNullInteger(dr["CreatedByUserId"]);
            LastUpdatedByUserId = Null.SetNullInteger(dr["LastUpdatedByUserId"]);
        }

        int IHydratable.KeyID
        {
            get
            {
                return TeamId;
            }
            set
            {
                TeamId = value;
            }
        }

        #endregion
    }
}