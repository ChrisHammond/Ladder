﻿/*
' Copyright (c) 2010  Christoc.com
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Common.Utilities;
using com.christoc.modules.ladder.Data;

namespace DotNetNuke.Modules.ladder.Components
{
    public class GamePlayerSettingController
    {

        
        //add player setting

        //update player setting

        //get settings for game

        public List<GamePlayerSetting> GetSettingsForGameByGame(int gameId)
        {
            return CBO.FillCollection<GamePlayerSetting>(DataProvider.Instance().GetGamePlayerSettingsByGame(gameId));
        }

        //get settings for player

        public List<GamePlayerSetting> GetSettingsForGameByPlayerId(int playerId)
        {
            return CBO.FillCollection<GamePlayerSetting>(DataProvider.Instance().GetGamePlayerSettingsByPlayer(playerId));
        }



    }
}