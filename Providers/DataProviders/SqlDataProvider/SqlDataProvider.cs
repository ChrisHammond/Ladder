/*
' Copyright (c) 2010 Christoc.com
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
using System.Data;
using System.Data.SqlClient;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Framework.Providers;
using DotNetNuke.Modules.ladder.Components;
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
                , new SqlParameter("@lastplayed", t.Name)
                , new SqlParameter("@createdbyuserid", t.Name)
                , new SqlParameter("@lastupdatedbyuserid", t.Name)
                ));
        }

        public override void UpdateTeam(Team t)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "UpdateTeam",
                new SqlParameter("@TeamId", t.TeamId)
                  , new SqlParameter("@Name", t.Name)
                 , new SqlParameter("@firstplayed", t.FirstPlayed)
                 , new SqlParameter("@lastplayed", t.Name)
                 , new SqlParameter("@lastupdatedbyuserid", t.Name)
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
            throw new NotImplementedException();
        }

        public override IDataReader GetTeam(int teamId)
        {
            throw new NotImplementedException();
        }

        public override IDataReader GetGames()
        {
            throw new NotImplementedException();
        }

        public override IDataReader GetGame(int gameId)
        {
            throw new NotImplementedException();
        }

        public override IDataReader GetGamePlayerSettingsByGame(int gameId)
        {
            throw new NotImplementedException();
        }

        public override IDataReader GetGamePlayerSettingsByPlayer(int gameId)
        {
            throw new NotImplementedException();
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
                , new SqlParameter("@Games", p.Wins)
                , new SqlParameter("@Games", p.Losses)
               );
        }

        public override IDataReader GetPlayer(int playerId)
        {
            return SqlHelper.ExecuteReader(ConnectionString, NamePrefix + "GetPlayer", new SqlParameter("@PlayerId", playerId));
        }

        public override int AddGame(Game g)
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(ConnectionString, NamePrefix + "AddGame"
                                                           , new SqlParameter("@PlayedDate", g.PlayedDate)
                                                           , new SqlParameter("@CreatedByUserId", g.CreatedByUserId)
                                                           , new SqlParameter("@LastUpdatedByUserId", g.LastUpdatedByUserId)
                                       ));
        }

        public override void UpdateGame(Game g)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, NamePrefix + "Update"
                , new SqlParameter("@GameId", g.GameId)
                                                           , new SqlParameter("@PlayedDate", g.PlayedDate)
                                                           , new SqlParameter("@LastUpdatedByUserId", g.LastUpdatedByUserId)
                                       );
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
    }

}