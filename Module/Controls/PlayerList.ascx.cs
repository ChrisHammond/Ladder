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
using System.Web.UI;
using System.Web.UI.WebControls;
using com.christoc.modules.ladder.Components;
using com.christoc.modules.ladder.Data;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;

namespace com.christoc.modules.ladder.Controls
{
    public partial class PlayerList : LadderModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    Localization.LocalizeGridView(ref gvListOfNonPlayers, LocalResourceFile);

                    BindPlayerList();
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void BindPlayerList()
        {
            gvListOfNonPlayers.DataSource = DataProvider.Instance().GetNonPlayers(PortalId);
            gvListOfNonPlayers.DataBind();
        }


        protected void gbListOfNonPlayers_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var lbAdd = e.Row.FindControl("lbAdd") as LinkButton;

                var curUserId = DataBinder.Eval(e.Row.DataItem, "UserId");
                if (lbAdd != null)
                {
                    lbAdd.CommandName = "AddPlayer";
                    lbAdd.CommandArgument = curUserId.ToString();
                }
            }
        }

        protected void gbListOfNonPlayers_OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
            var userid = e.CommandArgument;

            var newP = new Player {UserId = Convert.ToInt32(userid), PortalId = PortalId};
            var pc = new PlayerController();
            pc.Save(newP);
            //todo: refresh the grid to remove the recently added player
            BindPlayerList();
        }

    }
}