using System;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;

namespace DotNetNuke.Modules.ladder.Components
{
    public class Game : IHydratable
    {
        public int GameId { get; set; }
        public DateTime PlayedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public int PortalId { get; set; }
        public int ModuleId { get; set; }


        public int CreatedByUserId { get; set; }
        public int LastUpdatedByUserId { get; set; }

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
            GameId = Null.SetNullInteger(dr["GameId"]);
            ModuleId = Null.SetNullInteger(dr["ModuleId"]);
            PlayedDate = Null.SetNullDateTime(dr["PlayedDate"]);
            CreatedDate = Null.SetNullDateTime(dr["CreatedDate"]);
            LastUpdatedDate = Null.SetNullDateTime(dr["LastUpdatedDate"]);

            CreatedByUserId = Null.SetNullInteger(dr["CreatedByUserId"]);
            LastUpdatedByUserId = Null.SetNullInteger(dr["LastUpdatedByUserId"]);
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