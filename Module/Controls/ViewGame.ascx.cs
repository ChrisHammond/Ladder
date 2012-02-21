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
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Utilities;

namespace com.christoc.modules.ladder.Controls
{
    public partial class ViewGame : LadderModuleBase
    {
        protected void PageLoad(object sender, EventArgs e)
        {
            try
            {
                //TODO: allow select of the Teams

                pnlAdmin.Visible = IsEditable;
                ClientAPI.AddButtonConfirm(lbDeleteGame, Localization.GetString("DeleteConfirm", LocalResourceFile));

                var gc = new GameController();
                var currentGame = gc.GetGame(GameId);
                if(currentGame!=null)
                {
                    lblGameStart.Text = currentGame.PlayedDate.ToShortDateString();
                    lblTeam1Link.NavigateUrl = GetTeamLink(currentGame.Teams[0].TeamId);
                    lblTeam1Link.Text = currentGame.Teams[0].Name;
                    lblTeam1Score.Text = currentGame.Teams[0].Score.ToString();
                    lblTeam2Link.NavigateUrl = GetTeamLink(currentGame.Teams[1].TeamId);
                    lblTeam2Link.Text = currentGame.Teams[1].Name;
                    lblTeam2Score.Text = currentGame.Teams[1].Score.ToString();
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this,exc);
            }
        }

        protected void LbDeleteGameClick(object sender, EventArgs e)
        {
            if(GameId>0)
            {
                var gc = new GameController();
                gc.DeleteGame(GameId);
                Response.Redirect(DotNetNuke.Common.Globals.NavigateURL());
            }
        }
    }
}