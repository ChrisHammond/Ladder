using System;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;

namespace DotNetNuke.Modules.ladder.Components
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