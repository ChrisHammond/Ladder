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
using com.christoc.modules.ladder.Components;
using DotNetNuke.Services.Exceptions;

namespace com.christoc.modules.ladder.Controls
{
    public partial class ViewTeam : LadderModuleBase
    {
        protected void PageLoad(object sender, EventArgs e)
        {
            try
            {
                if(IsEditable)
                {
                    hlManageTeam.NavigateUrl = GetTeamManageLink(TeamId);
                    pnlAdmin.Enabled = true;
                }

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

    }
}