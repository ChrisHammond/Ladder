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
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Security;


namespace Christoc.Com.Modules.Ladder
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The Viewladder class displays the content
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class View : LadderModuleBase, IActionable
    {

        #region Event Handlers

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            Load += Page_Load;
        }


        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Page_Load runs when the control is loaded
        /// </summary>
        /// -----------------------------------------------------------------------------
        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                var
                controlToLoad = ControlToLoad();
                var mbl = (LadderModuleBase)LoadControl(controlToLoad);
                mbl.ModuleConfiguration = ModuleConfiguration;
                mbl.ID = System.IO.Path.GetFileNameWithoutExtension(controlToLoad);
                phLadder.Controls.Add(mbl);

            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private string ControlToLoad()
        {

            var df = Request.QueryString["df"];
            var cl = string.Empty;
            if (df != null)
            {
                switch (df)
                {
                    case "ViewGame":
                        return ("controls/ViewGame.ascx");
                    case "ManageGame":
                        //you can't load the Manage Game control if you don't have edit permissions
                        if (IsEditable)
                        {
                            return ("controls/ManageGame.ascx");
                        }
                        break;
                    case "ViewTeam":
                        return ("controls/ViewTeam.ascx");
                    case "ManageTeam":
                        //you can't load the Manage Team control if you don't have edit permissions
                        if(IsEditable)
                        {
                            return ("controls/ManageTeam.ascx");
                        }
                        break;

                    case "ManagePlayerList":
                        if(IsEditable)
                        {
                            return ("controls/PlayerList.ascx");
                        }
                        break;
                    default:
                        return ("controls/GameList.ascx");
                }
            }
            return "controls/GameList.ascx";

        }

        #endregion

        #region Optional Interfaces

        public ModuleActionCollection ModuleActions
        {
            get
            {
                var moduleActionCollection = new ModuleActionCollection
                                                 {
                                                     {
                                                         GetNextActionID(),
                                                         Localization.GetString("ManagePlayerList",
                                                                                LocalResourceFile), "", "", "",
                                                         DotNetNuke.Common.Globals.NavigateURL(TabId, String.Empty,
                                                                                               "df=ManagePlayerList"),
                                                         false, SecurityAccessLevel.Edit, true, false
                                                         },
                                                     {
                                                         GetNextActionID(),
                                                         Localization.GetString("ManageTeam", LocalResourceFile),
                                                         "", "", "",
                                                         DotNetNuke.Common.Globals.NavigateURL(TabId, String.Empty,
                                                                                               "df=ManageTeam"), false,
                                                         SecurityAccessLevel.Edit, true, false
                                                         },

                                                          {
                                                         GetNextActionID(),
                                                         Localization.GetString("NewGame", LocalResourceFile),
                                                         "", "", "",
                                                         GetGameManageLink(), false,
                                                         SecurityAccessLevel.Edit, true, false
                                                         }
                                                 };


                return moduleActionCollection;
            }
        }

        #endregion

    }

}
