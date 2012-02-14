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

namespace com.christoc.modules.ladder
{

    public class ladderModuleBase : DotNetNuke.Entities.Modules.PortalModuleBase
    {
        //with this base class you can provide any custom properties and methods that all your controls can access here, you can also access all the DNN 
        // methods and properties available off of portalmodulebase such as TabId, UserId, UserInfo, etc.
        public int GameId
        {
            get
            {
                var qs = Request.QueryString["gid"];
                if (qs != null)
                    return Convert.ToInt32(qs);
                return -1;
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

        public string GetGameLink()
        {
            return GetGameLink(GameId);
        }

        public string GetGameLink(int gameId)
        {
            return DotNetNuke.Common.Globals.NavigateURL(TabId, String.Empty, "df=ViewGame&gid=" + gameId);
        }
        public string GetGameManageLink(int gameId)
        {
            return DotNetNuke.Common.Globals.NavigateURL(TabId, String.Empty, "df=ManageGame&tid=" + gameId);
        }

        public string GetTeamLink(int teamId)
        {
            return DotNetNuke.Common.Globals.NavigateURL(TabId, String.Empty, "df=ViewTeam&tid=" + teamId);
        }
        public string GetTeamManageLink(int teamId)
        {
            return DotNetNuke.Common.Globals.NavigateURL(TabId, String.Empty, "df=ManageTeam&tid=" + teamId);
        }


    }

}
