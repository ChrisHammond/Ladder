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
using System.Data;
using System.Data.SqlClient;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Framework.Providers;
using com.christoc.modules.ladder.Components;
using Microsoft.ApplicationBlocks.Data;

namespace com.christoc.modules.ladder.Data
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// SQL Server implementation of the abstract DataProvider class
    /// </summary>
    /// -----------------------------------------------------------------------------
    public class SqlDataProvider : DataProvider
    {

        #region Private Members

        private const string ProviderType = "data";
        private const string ModuleQualifier = "ladder_";

        private readonly ProviderConfiguration _providerConfiguration = ProviderConfiguration.GetProviderConfiguration(ProviderType);
        private readonly string _connectionString;
        private readonly string _providerPath;
        private readonly string _objectQualifier;
        private readonly string _databaseOwner;

        #endregion

        #region Constructors

        public SqlDataProvider()
        {

            // Read the configuration specific information for this provider
            var objProvider = (Provider)(_providerConfiguration.Providers[_providerConfiguration.DefaultProvider]);

            // Read the attributes for this provider

            //Get Connection string from web.config
            _connectionString = Config.GetConnectionString();

            if (string.IsNullOrEmpty(_connectionString))
            {
                // Use connection string specified in provider
                _connectionString = objProvider.Attributes["connectionString"];
            }

            _providerPath = objProvider.Attributes["providerPath"];

            _objectQualifier = objProvider.Attributes["objectQualifier"];
            if (!string.IsNullOrEmpty(_objectQualifier) && _objectQualifier.EndsWith("_", StringComparison.Ordinal) == false)
            {
                _objectQualifier += "_";
            }

            _databaseOwner = objProvider.Attributes["databaseOwner"];
            if (!string.IsNullOrEmpty(_databaseOwner) && _databaseOwner.EndsWith(".", StringComparison.Ordinal) == false)
            {
                _databaseOwner += ".";
            }

        }

        #endregion

        #region Properties

        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }
        }

        public string ProviderPath
        {
            get
            {
                return _providerPath;
            }
        }

        public string ObjectQualifier
        {
            get
            {
                return _objectQualifier;
            }
        }

        public string DatabaseOwner
        {
            get
            {
                return _databaseOwner;
            }
        }

        private string NamePrefix
        {
            get { return DatabaseOwner + ObjectQualifier + ModuleQualifier; }
        }

        #endregion

        #region Private Methods

        private static object GetNull(object Field)
        {
            return Null.GetNull(Field, DBNull.Value);
        }

        #endregion

        #region Public Methods

        //public override IDataReader GetItem(int itemId)
        //{
        //    return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "spGetItem", itemId);
        //}

        //public override IDataReader GetItems(int userId, int portalId)
        //{
        //    return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "spGetItemsForUser", userId, portalId);
        //}


        #endregion

        #region Overrides of DataProvider

        public override int CreateTeam(Team t)
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, NamePrefix + "AddTeam",
                 new SqlParameter("@Name", t.Name)
                , new SqlParameter("@firstplayed", t.FirstPlayed)
                , new SqlParameter("@lastplayed", t.LastPlayed)
                , new SqlParameter("@createdbyuserid", t.CreatedByUserId)
                , new SqlParameter("@lastupdatedbyuserid", t.LastUpdatedByUserId)
                , new SqlParameter("@moduleid", t.ModuleId)
                ));
        }

        public override void UpdateTeam(Team t)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "UpdateTeam",
                new SqlParameter("@TeamId", t.TeamId)
                  , new SqlParameter("@Name", t.Name)
                 , new SqlParameter("@firstplayed", t.FirstPlayed)
                 , new SqlParameter("@lastplayed", t.LastPlayed)
                 , new SqlParameter("@lastupdatedbyuserid", t.LastUpdatedByUserId)
                 , new SqlParameter("@Games", t.Games)
                 , new SqlParameter("@Wins", t.Wins)
                 , new SqlParameter("@Losses", t.Losses)
                 );
        }

        public override void AddTeamPlayer(int teamId, int playerId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "AddTeamPlayer",
                                      new SqlParameter("@TeamId", teamId)
                                      , new SqlParameter("@PlayerId", playerId));
        }

        public override IDataReader GetTeamPlayers(int teamId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GetTeamPlayers", new SqlParameter("@TeamId", teamId));
        }

        public override IDataReader GetTeams()
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GetTeams");
        }


        public override IDataReader GetTeamsByGame(int gameId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GetTeamsByGame", new SqlParameter("@GameId", gameId));
        }
        public override IDataReader GetTeam(int teamId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GetTeam", new SqlParameter("@TeamId", teamId));
        }

        public override IDataReader GetTeam(string teamName)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GetTeamByName", new SqlParameter("@Name", teamName));
        }


        /* games */
        public override IDataReader GetGames(int portalId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GetGames", new SqlParameter("@PortalId", portalId));
        }


        public override IDataReader GetGames(int portalId, int count)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GetGamesLastX", new SqlParameter("@PortalId", portalId), new SqlParameter("@lastNumber" , count));
        }

        public override IDataReader GetGames(int portalId, DateTime startDate, DateTime endDate)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GetGamesByDateRange", new SqlParameter("@PortalId", portalId), new SqlParameter("@startDate", startDate), new SqlParameter("@endDate", endDate));
        }


        public override IDataReader GetGame(int gameId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GetGame", new SqlParameter("@GameId", gameId));
        }

        public override int AddGame(Game g)
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, NamePrefix + "AddGame"
                                                           , new SqlParameter("@PlayedDate", g.PlayedDate)
                                                           , new SqlParameter("@CreatedByUserId", g.CreatedByUserId)
                                                           , new SqlParameter("@LastUpdatedByUserId", g.LastUpdatedByUserId)
                                                           , new SqlParameter("@PortalId", g.PortalId)
                                                           , new SqlParameter("@FieldIdentifier", g.FieldIdentifier)
                                       ));
        }

        public override void UpdateGame(Game g)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "UpdateGame"
                , new SqlParameter("@GameId", g.GameId)
                                                           , new SqlParameter("@PlayedDate", g.PlayedDate)
                                                           , new SqlParameter("@LastUpdatedByUserId", g.LastUpdatedByUserId)
                                       );
        }


        public override void AddGameTeam(int gameId, int teamId, int score, bool win, bool home)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "AddGameTeam"
                , new SqlParameter("@GameId", gameId)
                , new SqlParameter("@TeamId", teamId)
                , new SqlParameter("@score", score)
                , new SqlParameter("@win", win)
                , new SqlParameter("@home", home)
                );
        }

        public override void UpdateGameTeam(int gameId, int teamId, int score, bool win, bool home)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "UpdateGameTeam"
                , new SqlParameter("@GameId", gameId)
                , new SqlParameter("@TeamId", teamId)
                , new SqlParameter("@score", score)
                , new SqlParameter("@win", win)
                , new SqlParameter("@home", home)
                );
        }

        public override IDataReader GetGameTeams(int gameId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GetGameTeams"
                                    , new SqlParameter("@GameId", gameId));
        }


        public override IDataReader GetGamePlayerSettingsByGame(int gameId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GetGamePlayerSettingsByGame", new SqlParameter("@GameId", gameId));
        }

        public override IDataReader GetGamePlayerSettingsByPlayer(int gameId, int playerId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GetGamePlayerSettingsByPlayer", new SqlParameter("@GameId", gameId), new SqlParameter("@PlayerId", playerId));
        }

        public override int CreatePlayer(int userId)
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, NamePrefix + "AddPlayer", new SqlParameter("@UserId", userId)));
        }

        public override void UpdatePlayer(Player p)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "UpdatePlayer"
                , new SqlParameter("@UserId", p.UserId)
                , new SqlParameter("@Games", p.Games)
                , new SqlParameter("@Wins", p.Wins)
                , new SqlParameter("@Losses", p.Losses)
               );
        }

        public override IDataReader GetPlayer(int playerId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GetPlayer", new SqlParameter("@PlayerId", playerId));
        }

        public override IDataReader GetNonPlayers(int portalId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GetNonPlayers", new SqlParameter("@PortalId", portalId));
        }

        #endregion

        public override void AddGamePlayerSetting(GamePlayerSetting gps)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "AddGamePlayerSetting"
                , new SqlParameter("@GameId", gps.GameId)
                , new SqlParameter("@PlayerId", gps.PlayerId)
                , new SqlParameter("@Setting", gps.Setting)
                , new SqlParameter("@Value", gps.Value)
                );
        }

        public override void UpdateGamePlayerSetting(GamePlayerSetting gps)
        {
            SqlHelper.ExecuteScalar(ConnectionString, NamePrefix + "UpdateGamePlayerSetting"
                , new SqlParameter("@GameId", gps.GameId)
                , new SqlParameter("@PlayerId", gps.PlayerId)
                , new SqlParameter("@Setting", gps.Setting)
                , new SqlParameter("@Value", gps.Value)
                );
        }



        /* field */
        public override IDataReader GetFields()
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GetFields");
        }

        public override IDataReader GetField(int fieldId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GetField", new SqlParameter("@FieldId", fieldId));
        }

        public override IDataReader GetField(string fieldIdentifier)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GetFieldByIdentifier", new SqlParameter("@FieldIdentifier", fieldIdentifier));
        }
        public override int AddField(Field f)
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, NamePrefix + "AddField"
                                                           , new SqlParameter("@FieldName", f.FieldName)
                                                           , new SqlParameter("@FieldIdentifier", f.FieldIdentifier)
                                                           , new SqlParameter("@CreatedByUserId", f.CreatedByUserId)
                                                           , new SqlParameter("@LastUpdatedByUserId", f.LastUpdatedByUserId)
                                                           , new SqlParameter("@moduleid", f.ModuleId)
                                       ));
        }

        public override void UpdateField(Field f)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "UpdateField"
                , new SqlParameter("@FieldId", f.FieldId)
                                                       , new SqlParameter("@FieldName", f.FieldName)
                                                           , new SqlParameter("@FieldIdentifier", f.FieldIdentifier)
                                                           , new SqlParameter("@LastUpdatedByUserId", f.LastUpdatedByUserId)
                                       );
        }



        public override void DeleteTeam(int teamId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "DeleteTeam"
                , new SqlParameter("@TeamId", teamId));
        }

        public override void DeleteTeamPlayers(int teamId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "DeleteTeamPlayers"
            , new SqlParameter("@TeamId", teamId));

        }

        public override void DeleteField(int fieldId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "DeleteField"
                , new SqlParameter("@FieldId", fieldId));

        }

        public override void DeleteGame(int gameId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "DeleteGame"
                , new SqlParameter("@GameId", gameId));
        }

        public override void DeleteGameTeams(int gameId)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "DeleteGameTeams"
                , new SqlParameter("@GameId", gameId));
        }
    }

}