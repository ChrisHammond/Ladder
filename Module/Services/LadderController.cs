using System;
using System.Web.Mvc;
using Christoc.Com.Modules.Ladder.Components;
using DotNetNuke.Instrumentation;
using DotNetNuke.Web.Services;
using System.Web.Script.Serialization;

namespace Christoc.Com.Modules.Ladder.Services
{
    public class LadderController : DnnController
    {

        //check to make sure we can get/use games
        [DnnAuthorize(AllowAnonymous = true)]
        public ActionResult ListOfGames()
        {
            try
            {
                var gc = new GameController();
                var games = gc.GetGames(ActiveModule.PortalID, true);
                return Json(games, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exc)
            {
                DnnLog.Error(exc);
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        //below code is not currently in use and doesn't yet work
        //[SupportedModules("Ladder")]
        //[DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        [DnnAuthorize(AllowAnonymous = true)]
        //[AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveGame(Game jsonGame)
        {
            //todo: jsonGame will never be null, check for something else
            if (jsonGame != null)
            {
                var currentGame = jsonGame;
                try
                {
                    var gc = new GameController();
                    //TODO: Save Game
                    if (currentGame != null)
                    {
                        //we need to lookup the game based on the GameId if passed in the json, if so, then combine all the new stats
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
                        currentGame.PortalId = PortalSettings.PortalId;

                        //save the game information
                        currentGame = gc.SaveGame(currentGame);

                        //TODO: how to flag games in progress?

                        //return the GameID so Netduino knows to use this
                        return Json(currentGame.GameId, JsonRequestBehavior.AllowGet);
                    }


                    return null;
                }
                catch (Exception exc)
                {
                    DnnLog.Error(exc);
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            return null;
        }


    }
}