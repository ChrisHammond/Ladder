
using System;
using System.IO;
using System.Web;
using System.Web.Script.Serialization;
using com.christoc.modules.ladder.Components;
using DotNetNuke.Entities.Portals;

namespace com.christoc.modules.ladder.svc
{
    public class PostHandler : IHttpHandler
    {

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
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


            var CurrentGame = jss.Deserialize<Game>(jsonBody);

            //todo: authenticate the request, perhaps with Netduino ID as a ModuleSetting?

            if (CurrentGame != null)
            {
                //check if a Field exists, if not add it
                var f = FieldController.GetField(CurrentGame.FieldIdentifier);
                if (f == null)
                {
                    f = new Field { FieldIdentifier = CurrentGame.FieldIdentifier, FieldName = "New Field" };
                    f = f.Save();
                }

                //todo:add the teams to the game

                //g = new Game {FieldIdentifier = f.FieldIdentifier, PortalId = PortalId, PlayedDate = DateTime.Now};

                //save the game
                var gc = new GameController();

                CurrentGame = gc.SaveGame(CurrentGame);

                //TODO: how to keep track of games in progress?
                
                written = true;
            }

            response.Write("Test");


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