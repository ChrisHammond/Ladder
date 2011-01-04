using System;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;

namespace DotNetNuke.Modules.ladder.Components
{
    public class Player : IHydratable
    {
        public int playerId { get; set; }
        public int userId { get; set; }
        public int rank { get; set; }
        public int games { get; set; }
        public int wins { get; set; }
        public int losses { get; set; }
                

        #region IHydratable Members

        void IHydratable.Fill(System.Data.IDataReader dr)
        {
            playerId = Null.SetNullInteger(dr["playerId"]);
            userId = Null.SetNullInteger(dr["userId"]);
            rank = Null.SetNullInteger(dr["rank"]);
            games = Null.SetNullInteger(dr["games"]);
            wins = Null.SetNullInteger(dr["wins"]);
            losses = Null.SetNullInteger(dr["losses"]);

        }

        int IHydratable.KeyID
        {
            get
            {
                return playerId;
            }
            set
            {
                playerId = value;
            }
        }

        #endregion
    }
}