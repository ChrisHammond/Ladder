using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.christoc.modules.ladder.Data;
using DotNetNuke.Common.Utilities;

namespace DotNetNuke.Modules.ladder.Components
{
    public class TeamController
    {
        public Team GetTeam(int teamId)
        {
            var t = CBO.FillObject(DataProvider.Instance().GetTeam(teamId));
            return null;
        }

        public List<Team> GetTeams()
        {

        }

    }
}