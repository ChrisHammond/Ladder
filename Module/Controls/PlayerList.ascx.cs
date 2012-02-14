using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.christoc.modules.ladder.Data;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;

namespace com.christoc.modules.ladder.Controls
{
    public partial class PlayerList : ladderModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if(!Page.IsPostBack)
                {
                    Localization.LocalizeGridView(ref gvListOfNonPlayers, LocalResourceFile);
                    gvListOfNonPlayers.DataSource = DataProvider.Instance().GetNonPlayers(PortalId);
                    gvListOfNonPlayers.DataBind();

                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this,exc);
            }
        }

        protected void gbListOfNonPlayers_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
           //TODO: we need to bind the user info so that we can allow the add button to be clicked.
        }
    }
}