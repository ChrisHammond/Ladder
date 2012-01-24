
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

            //todo: authenticate the request, perhaps with Netduino ID as a ModuleSetting?

            if (context.Request.Url.AbsolutePath.IndexOf("NewGame") > 0 && written == false)
            {
                //create a new game
             
                //check if a Field exists, if not add it
                
                //todo: replace field identifier from JSON
                var f = FieldController.GetField("somethingHere");
                if(f==null)
                {
                    f = new Field {FieldIdentifier = "somethingHere", FieldName = "New Field"};
                    f = f.Save();
                }


                //todo:add the teams to the game
                
                var g = new Game {FieldId = f.FieldId, PortalId = PortalId, PlayedDate = DateTime.Now};

                //save the game
                var gc = new GameController();
                g = gc.SaveGame(g);



                //TODO: how to keep track of games in progress?

   
                written = true;
            }



            


            //update score

            //complete game

            //restart game
            


            throw new NotImplementedException();
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