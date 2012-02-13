using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.christoc.modules.ladder.Components;
using DotNetNuke.Services.Exceptions;

namespace com.christoc.modules.ladder.Controls
{
    public partial class ViewTeam : ladderModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if(TeamId>0)
                {

                    //TODO: link to the Manage Team controls

                    var tc = new TeamController();
                    var curTeam = tc.GetTeam(TeamId);

                    if(curTeam!=null)
                    {
                        lblTeamName.Text = curTeam.Name;
                        lblWinsValue.Text = curTeam.Wins.ToString();
                        lblLossesValue.Text = curTeam.Losses.ToString();
                    }
                }

                //todo: display last game
                //todo: link to players

            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this,exc);
            }
        }

        public int TeamId
        {
            get
            {
                var qs = Request.QueryString["tid"];
                if (qs != null)
                    return Convert.ToInt32(qs);
                return -1;
            }
        }
    }
}