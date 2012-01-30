﻿/*
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
using System.IO;
using System.Net;
using System.Text;
using System.Net.Sockets;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

using Microsoft.SPOT.Net.NetworkInformation;
using SecretLabs.NETMF.Hardware.NetduinoPlus;

namespace com.christoc.netduino.FoosTracker
{
    public class Program
    {
        private static Team awayTeam;
        private static Team homeTeam;

        private static Game _currentGame;

        private const int WinningScore = 10;

        static readonly OutputPort AwayLed = new OutputPort(Pins.GPIO_PIN_D12, false);
        static readonly OutputPort HomeLed = new OutputPort(Pins.GPIO_PIN_D13, false);

        private static int gameId = 0;

        public static void Main()
        {
            //todo: winning score should be handled when creating a new game

            //configure the buttons for scoring
            var awayTeamAdd = new InterruptPort(Pins.GPIO_PIN_D2, true, Port.ResistorMode.PullUp,
                                               Port.InterruptMode.InterruptEdgeLow);
            var awayTeamSubtract = new InterruptPort(Pins.GPIO_PIN_D3, true, Port.ResistorMode.PullUp,
                                               Port.InterruptMode.InterruptEdgeLow);
            var homeTeamAdd = new InterruptPort(Pins.GPIO_PIN_D4, true, Port.ResistorMode.PullUp,
                                               Port.InterruptMode.InterruptEdgeLow);
            var homeTeamSubtract = new InterruptPort(Pins.GPIO_PIN_D5, true, Port.ResistorMode.PullUp,
                                               Port.InterruptMode.InterruptEdgeLow);

            //initialize the game and teams
            _currentGame = new Game();
            homeTeam = new Team { Name = "home", Score = 0, TeamId = 0 };
            awayTeam = new Team { Name = "away", Score = 0, TeamId = 0 };
            //_currentGame.Teams.Add(homeTeam);
            //_currentGame.Teams.Add(awayTeam);
            
            //register the interrupts to keep track of button presses
            awayTeamAdd.OnInterrupt += awayTeamAdd_OnInterrupt;
            awayTeamSubtract.OnInterrupt += awayTeamSubtract_OnInterrupt;
            homeTeamAdd.OnInterrupt += homeTeamAdd_OnInterrupt;
            homeTeamSubtract.OnInterrupt += homeTeamSubtract_OnInterrupt;

            //using the MAC address to identify the "field" or which foosball table
            _currentGame.FieldIdentifier = Mac();

            while (true)
            {
                //check for Max score/win
                CheckTeamScores();
            }
            // ReSharper disable FunctionNeverReturns
        }
        // ReSharper restore FunctionNeverReturns

        private static void awayTeamAdd_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            awayTeam.Score++;
            AwayLed.Write(true);
        }
        private static void awayTeamSubtract_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            awayTeam.Score--;
            AwayLed.Write(false);
        }
        private static void homeTeamAdd_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            homeTeam.Score++;
            HomeLed.Write(true);
        }
        private static void homeTeamSubtract_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            homeTeam.Score--;
            HomeLed.Write(false);
        }

        private static void UpdateWebScores()
        {
            //build json
            var jsonString = BuildJson();
            //todo: look into threading
            //call the webservice
            CallWebService(jsonString, "http://192.168.1.9/svc/ladder/Game");
        }

        private static void CheckTeamScores()
        {
            if (awayTeam.Score < WinningScore && homeTeam.Score < WinningScore) return;
            
            //game over a team got the winning score points, time to finish
            GameOver();
            
            //call NewGame to reset the teams, this will also send the call to the web service
            NewGame();
        }


        //code based on examples in Getting Started with the Internet Of Things book http://cjh.am/gswtiot 
        private static void CallWebService(string jsonGame, string serviceUrl)
        {
            //todo: handle problems in the web service

            byte[] buffer = Encoding.UTF8.GetBytes(jsonGame);
            // produce request
            var requestUri = serviceUrl;
            using (var request = (HttpWebRequest)WebRequest.Create(requestUri))
            {
                request.Method = "PUT";
                // headers
                request.ContentType = "application/json";
                request.ContentLength = buffer.Length;
                
                // content
                Stream s = request.GetRequestStream();
                s.Write(buffer, 0, buffer.Length);
                // send request and receive response
                using (var response = (HttpWebResponse)request.
                GetResponse())
                {
                    // consume response
                    HandleResponse(response);
                    Debug.Print("Status code: " + response.StatusCode);
                }
            }
        }
        //code based on examples in Getting Started with the Internet Of Things book http://cjh.am/gswtiot 
        private static void HandleResponse(HttpWebResponse response)
        {
            //todo: handle problems in the response

            // response body
            var buffer = new byte[response.ContentLength];
            Stream stream = response.GetResponseStream();
            int toRead = buffer.Length;
            while (toRead > 0)
            {
                // already read: buffer.Length - toRead
                int read = stream.Read(buffer, buffer.Length - toRead,
                toRead);
                toRead = toRead - read;
            }
            char[] chars = Encoding.UTF8.GetChars(buffer);
            var responseId = new string(chars);
            Debug.Print(responseId);
            gameId = int.Parse(responseId);

        }

        private static void GameOver()
        {
            //someone hit 10 points, send the results to the service before clearing and starting a new game
            UpdateWebScores();
        }

        private static void NewGame()
        {   
            //reset the scores
            awayTeam.Score = 0;
            homeTeam.Score = 0;
            gameId = 0;

            _currentGame = new Game();
            _currentGame.FieldIdentifier = Mac();

            //todo: call the webservice to initialize the new game
            
            //reset the LEDs
            HomeLed.Write(false);
            AwayLed.Write(false);
            
            //call the webservice
            UpdateWebScores();
        }
        
        public static string BuildJson()
        {
            //sample json for GAME //"{\"GameId\":0,\"PlayedDate\":\"\\/Date(1327561544141)\\/\",\"CreatedDate\":\"\\/Date(-62135568000000)\\/\",\"LastUpdatedDate\":\"\\/Date(-62135568000000)\\/\",\"PortalId\":0,\"ModuleId\":0,\"Teams\":[{\"TeamId\":0,\"Name\":\"Home\",\"FirstPlayed\":\"\\/Date(-62135568000000)\\/\",\"LastPlayed\":\"\\/Date(-62135568000000)\\/\",\"CreatedDate\":\"\\/Date(-62135568000000)\\/\",\"LastUpdatedDate\":\"\\/Date(-62135568000000)\\/\",\"Score\":4,\"Games\":0,\"Wins\":1,\"Losses\":0,\"ModuleId\":0,\"CreatedByUserId\":0,\"LastUpdatedByUserId\":0,\"PortalId\":0,\"Players\":[],\"CreatedByUser\":\"\",\"LastUpdatedByUser\":\"\"},{\"TeamId\":0,\"Name\":\"Away\",\"FirstPlayed\":\"\\/Date(-62135568000000)\\/\",\"LastPlayed\":\"\\/Date(-62135568000000)\\/\",\"CreatedDate\":\"\\/Date(-62135568000000)\\/\",\"LastUpdatedDate\":\"\\/Date(-62135568000000)\\/\",\"Score\":2,\"Games\":1,\"Wins\":1,\"Losses\":0,\"ModuleId\":0,\"CreatedByUserId\":0,\"LastUpdatedByUserId\":0,\"PortalId\":0,\"Players\":[],\"CreatedByUser\":\"\",\"LastUpdatedByUser\":\"\"}],\"CreatedByUserId\":0,\"LastUpdatedByUserId\":0,\"FieldIdentifier\":\"Test\",\"CreatedByUser\":\"\",\"LastUpdatedByUser\":\"\"}";
            var sb = string.Empty;
            sb =
                "{\"GameId\":" + gameId + ",\"Teams\":[{\"TeamId\":0,\"Name\":\"Home\",\"Score\":\"" + homeTeam.Score + "\",\"Games\":0,\"Wins\":0,\"Losses\":0},{\"TeamId\":0,\"Name\":\"Away\",\"Score\":\"" + awayTeam.Score + "\",\"Games\":0,\"Wins\":0,\"Losses\":0}],\"FieldIdentifier\":\"" + _currentGame.FieldIdentifier + "\"}";
            return sb;
        }
        
        //getting the MAC address of a Netduino plus so we can use that to identify the "field" 
        //http://snipt.net/Evotodi/get-netduino-plus-mac-address/
        public static string Mac()
        {
            NetworkInterface[] netIf = NetworkInterface.GetAllNetworkInterfaces();

            string macAddress = "";

            // Create a character array for hexidecimal conversion.
            const string hexChars = "0123456789ABCDEF";

            // Loop through the bytes.
            for (int b = 0; b < 6; b++)
            {
                // Grab the top 4 bits and append the hex equivalent to the return string.
                macAddress += hexChars[netIf[0].PhysicalAddress[b] >> 4];

                // Mask off the upper 4 bits to get the rest of it.
                macAddress += hexChars[netIf[0].PhysicalAddress[b] & 0x0F];

                // Add the dash only if the MAC address is not finished.
                if (b < 5) macAddress += "-";
            }

            return macAddress;
        }

        //todo: play game over audio
        //todo: record the temperature measurement at start of game, end of game
        //todo: allow user selection for teams

    }
}
