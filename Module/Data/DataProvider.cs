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


using System.Data;
using System;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Framework.Providers;
using com.christoc.modules.ladder.Components;


namespace com.christoc.modules.ladder.Data
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// An abstract class for the data access layer
    /// </summary>
    /// -----------------------------------------------------------------------------
    public abstract class DataProvider
    {

        #region Shared/Static Methods

        private static DataProvider provider;

        // return the provider
        public static DataProvider Instance()
        {
            if (provider == null)
            {
                const string assembly = "com.christoc.modules.ladder.Data.SqlDataprovider,ladder";
                Type objectType = Type.GetType(assembly, true, true);

                provider = (DataProvider)Activator.CreateInstance(objectType);
                DataCache.SetCache(objectType.FullName, provider);
            }

            return provider;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Not returning class state information")]
        public static IDbConnection GetConnection()
        {
            const string providerType = "data";
            ProviderConfiguration _providerConfiguration = ProviderConfiguration.GetProviderConfiguration(providerType);

            Provider objProvider = ((Provider)_providerConfiguration.Providers[_providerConfiguration.DefaultProvider]);
            string _connectionString;
            if (!String.IsNullOrEmpty(objProvider.Attributes["connectionStringName"]) && !String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings[objProvider.Attributes["connectionStringName"]]))
            {
                _connectionString = System.Configuration.ConfigurationManager.AppSettings[objProvider.Attributes["connectionStringName"]];
            }
            else
            {
                _connectionString = objProvider.Attributes["connectionString"];
            }

            IDbConnection newConnection = new System.Data.SqlClient.SqlConnection();
            newConnection.ConnectionString = _connectionString.ToString();
            newConnection.Open();
            return newConnection;
        }

        #endregion

        #region Abstract methods

        /*teams*/
        public abstract int CreateTeam(Team t);
        public abstract void UpdateTeam(Team t);
        public abstract void DeleteTeam(int teamId);



        public abstract void AddTeamPlayer(int teamId, int playerId);

        public abstract IDataReader GetTeamPlayers(int teamId);
        public abstract void DeleteTeamPlayers(int teamId);

        
        public abstract IDataReader GetTeams();
        public abstract IDataReader GetTeamsByGame(int gameId);
        
        public abstract IDataReader GetTeam(int teamId);
        public abstract IDataReader GetTeam(string teamName);

       
        
        /*game player settings*/
        public abstract void AddGamePlayerSetting(GamePlayerSetting gps);
        public abstract void UpdateGamePlayerSetting(GamePlayerSetting gps);
        public abstract IDataReader GetGamePlayerSettingsByGame(int gameId);
        public abstract IDataReader GetGamePlayerSettingsByPlayer(int gameId, int playerId);
        
        /* player */
        //add player
        public abstract int CreatePlayer(int userId);
        //update player
        public abstract void UpdatePlayer(Player p);

        public abstract IDataReader GetPlayer(int playerId);

        /* field */
        public abstract IDataReader GetFields();
        public abstract IDataReader GetField(int fieldId);
        public abstract IDataReader GetField(string fieldIdentifier);
        public abstract int AddField(Field f);
        public abstract void UpdateField(Field f);
        public abstract void DeleteField(int fieldId);
        


        /* game */
        public abstract int AddGame(Game g);
        public abstract void UpdateGame(Game g);
        public abstract void DeleteGame(int gameId);

        public abstract IDataReader GetGames(int portalId);

        public abstract IDataReader GetGames(int portalId, int count);
        public abstract IDataReader GetGames(int portalId, DateTime startDate, DateTime endDate);

        public abstract IDataReader GetGame(int gameId);

        

        public abstract void AddGameTeam(int gameId, int teamId, int score, bool win, bool home);
        public abstract void UpdateGameTeam(int gameId, int teamId, int score, bool win, bool home);
        public abstract IDataReader GetGameTeams(int gameId);

        public abstract void DeleteGameTeams(int gameId);

        

        #endregion

    }

}