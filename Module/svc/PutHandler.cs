
using System;
using System.IO;
using System.Web;
using System.Web.Script.Serialization;
using com.christoc.modules.ladder.Components;
using DotNetNuke.Entities.Portals;

namespace com.christoc.modules.ladder.svc
{
    public class PutHandler : IHttpHandler
    {

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {

            var gc = new GameController();

            HttpResponse response = context.Response;
            var written = false;

            //because we're coming into a URL that isn't being handled by DNN we need to figure out the PortalId
            SetPortalId(context.Request);


            var jss = new JavaScriptSerializer();

            //get the content from the post method

            var sr = new StreamReader(HttpContext.Current.Request.InputStream);
            var jsonBody = sr.ReadToEnd();


            //testing the Serialize method
            /*
            var g = new Game { FieldIdentifier = "Test", PortalId = PortalId, PlayedDate = DateTime.Now };
            var homeTeam = new Team {Name = "Home", Score = 0,Wins = 0, Losses=0, PortalId = PortalId};
            var awayTeam = new Team { Name = "Away", Score = 0, Wins = 0, Losses = 0, PortalId = PortalId };
            g.Teams.Add(homeTeam);
            g.Teams.Add(awayTeam);

            */
            
            var currentGame = jss.Deserialize<Game>(jsonBody);

            //todo: authenticate the request, perhaps with Netduino ID as a ModuleSetting?

            if (currentGame != null)
            {
                //todo: we need to lookup the game based on the GameId if passed in the json, if so, then combine all the new stats
                if (currentGame.GameId > 0)
                {
                    var existingGame = gc.GetGame(currentGame.GameId);
                    currentGame.FieldIdentifier = existingGame.FieldIdentifier;
                    currentGame.CreatedDate = existingGame.CreatedDate;
                    currentGame.CreatedByUserId = existingGame.CreatedByUserId;        
                }
                else
                {
                    currentGame.CreatedDate = DateTime.Now;
                    currentGame.CreatedByUserId = 1;
                    //check if a Field exists, if not add it
                    var f = FieldController.GetField(currentGame.FieldIdentifier);
                    if (f == null)
                    {
                        f = new Field { FieldIdentifier = currentGame.FieldIdentifier, FieldName = currentGame.FieldIdentifier, CreatedByUserId = 1, CreatedDate = DateTime.Now, LastUpdatedByUserId = 1, LastUpdatedDate = DateTime.Now };
                        f = f.Save();
                    }
                    currentGame.FieldIdentifier = f.FieldIdentifier;
                }

                currentGame.LastUpdatedByUserId = 1;
                currentGame.PlayedDate = DateTime.Now;
                currentGame.PortalId = PortalId;

                //save the game information
                currentGame = gc.SaveGame(currentGame);

                //TODO: how to keep track of games in progress?

                //return the GameID so Netduino knows to use this
                response.Write(jss.Serialize(currentGame.GameId));
                
                written = true;
            }

            
            //update score

            //complete game

            //restart game

        }



        ///<summary>
        /// Set the portalid, taking the current request and locating which portal is being called based on this request.
        /// </summary>
        /// <param name="request">request</param>
        private static void SetPortalId(HttpRequest request)
        {

            string domainName = DotNetNuke.Common.Globals.GetDomainName(request, true);

            string portalAlias = domainName.Substring(0, domainName.IndexOf("/svc"));
            PortalAliasInfo pai = PortalSettings.GetPortalAliasInfo(portalAlias);
            if (pai != null)
                PortalId = pai.PortalID;
        }

        public static int PortalId { get; set; }
    }
}