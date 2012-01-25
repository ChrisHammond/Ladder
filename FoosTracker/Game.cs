using System;
using System.Collections;
using Microsoft.SPOT;

namespace com.christoc.netduino.FoosTracker
{
    class Game
    {
        public int GameId { get; set; }
        public DateTime PlayedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public int PortalId { get; set; }
        public int ModuleId { get; set; }

        public ArrayList Teams { get; set; }

        public int CreatedByUserId { get; set; }
        public int LastUpdatedByUserId { get; set; }

        public string FieldIdentifier { get; set; }
    }
}
