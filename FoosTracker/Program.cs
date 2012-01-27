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

        //create two teams (away/home)

        private static Team awayTeam;
        private static Team homeTeam;
        private static Game _currentGame;

        private const int WinningScore = 10;

        static readonly OutputPort AwayLed = new OutputPort(Pins.GPIO_PIN_D12, false);
        static readonly OutputPort HomeLed = new OutputPort(Pins.GPIO_PIN_D13, false);

        public static void Main()
        {
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
            homeTeam = new Team { Name = "home", Score = 0, TeamId = 1 };
            awayTeam = new Team { Name = "away", Score = 0, TeamId = 1 };
            //_currentGame.Teams.Add(homeTeam);
            //_currentGame.Teams.Add(awayTeam);


            //register the interrupts to keep track of button presses
            awayTeamAdd.OnInterrupt += awayTeamAdd_OnInterrupt;
            awayTeamSubtract.OnInterrupt += awayTeamSubtract_OnInterrupt;
            homeTeamAdd.OnInterrupt += homeTeamAdd_OnInterrupt;
            homeTeamSubtract.OnInterrupt += homeTeamSubtract_OnInterrupt;

            _currentGame.FieldIdentifier = Mac();

            //todo: use the mac address as the identifier


            while (true)
            {
                //todo: check for Max score/win
                CheckTeamScores();
            }
            // ReSharper disable FunctionNeverReturns
        }
        // ReSharper restore FunctionNeverReturns

        private static void awayTeamAdd_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            awayTeam.Score++;
            AwayLed.Write(true);

            //call the update webservice?
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

        private static void CheckTeamScores()
        {
            if (awayTeam.Score < WinningScore && homeTeam.Score < WinningScore) return;

            //game over someone got 10

            if (awayTeam.Score >= WinningScore)
            {
                //todo: does the Netduino need to keep track of wins/losses?

            }
            else
            {//away team lost, home team won

            }
            //todo: play game over audio

            //todo: send the game information out to web service

            //build json
            var jsonString = BuildJson();

            // convert sample to byte array
            byte[] contentBuffer = Encoding.UTF8.GetBytes(jsonString);
            // produce request
            using (Socket connection = Connect("192.168.1.9", 5000))
            {
                SendRequest(connection, jsonString);
            }

            //call NewGame to reset the teams
            NewGame();
        }


        private static void NewGame()
        {

            //reset the scores
            awayTeam.Score = 0;
            homeTeam.Score = 0;

            _currentGame = new Game();
            _currentGame.FieldIdentifier = Mac();
            //_currentGame.Teams.Add(awayTeam);
            //_currentGame.Teams.Add(homeTeam);
            //todo: call the webservice to initialize the new game


            //reset the LEDs

            HomeLed.Write(false);
            AwayLed.Write(false);
        }



        public static string BuildJson()
        {
            //sample json for GAME
            //"{\"GameId\":0,\"PlayedDate\":\"\\/Date(1327561544141)\\/\",\"CreatedDate\":\"\\/Date(-62135568000000)\\/\",\"LastUpdatedDate\":\"\\/Date(-62135568000000)\\/\",\"PortalId\":0,\"ModuleId\":0,\"Teams\":[{\"TeamId\":0,\"Name\":\"Home\",\"FirstPlayed\":\"\\/Date(-62135568000000)\\/\",\"LastPlayed\":\"\\/Date(-62135568000000)\\/\",\"CreatedDate\":\"\\/Date(-62135568000000)\\/\",\"LastUpdatedDate\":\"\\/Date(-62135568000000)\\/\",\"Score\":4,\"Games\":0,\"Wins\":1,\"Losses\":0,\"ModuleId\":0,\"CreatedByUserId\":0,\"LastUpdatedByUserId\":0,\"PortalId\":0,\"Players\":[],\"CreatedByUser\":\"\",\"LastUpdatedByUser\":\"\"},{\"TeamId\":0,\"Name\":\"Away\",\"FirstPlayed\":\"\\/Date(-62135568000000)\\/\",\"LastPlayed\":\"\\/Date(-62135568000000)\\/\",\"CreatedDate\":\"\\/Date(-62135568000000)\\/\",\"LastUpdatedDate\":\"\\/Date(-62135568000000)\\/\",\"Score\":2,\"Games\":1,\"Wins\":1,\"Losses\":0,\"ModuleId\":0,\"CreatedByUserId\":0,\"LastUpdatedByUserId\":0,\"PortalId\":0,\"Players\":[],\"CreatedByUser\":\"\",\"LastUpdatedByUser\":\"\"}],\"CreatedByUserId\":0,\"LastUpdatedByUserId\":0,\"FieldIdentifier\":\"Test\",\"CreatedByUser\":\"\",\"LastUpdatedByUser\":\"\"}";

            var sb = string.Empty;
            sb =
                "{\"Teams\":[{\"TeamId\":0,\"Name\":\"Home\",\"Score\":\"" + homeTeam.Score + "\",\"Games\":0,\"Wins\":0,\"Losses\":0},{\"TeamId\":0,\"Name\":\"Away\",\"Score\":\"" + awayTeam.Score + "\",\"Games\":0,\"Wins\":0,\"Losses\":0}],\"FieldIdentifier\":\"" + _currentGame.FieldIdentifier + "\"}";
            return sb;

        }

        //todo: record the temperature measurement at start of game, end of game
        //todo: allow user selection for teams



        static Socket Connect(string host, int timeout)
        {
            // look up host’s domain name to find IP address(es)
            IPHostEntry hostEntry = Dns.GetHostEntry(host);
            // extract a returned address
            IPAddress hostAddress = hostEntry.AddressList[0];
            IPEndPoint remoteEndPoint = new IPEndPoint(hostAddress, 80);
            // connect!
            Debug.Print("connect...");
            var connection = new Socket(AddressFamily.InterNetwork,
            SocketType.Stream, ProtocolType.Tcp);
            connection.Connect(remoteEndPoint);
            connection.SetSocketOption(SocketOptionLevel.Tcp,
            SocketOptionName.NoDelay, true);
            connection.SendTimeout = timeout;
            return connection;
        }

        static void SendRequest(Socket s, string content)
        {
            byte[] contentBuffer = Encoding.UTF8.GetBytes(content);
            const string CRLF = "\r\n";
            var requestLine = "PUT /svc/ladder/Game HTTP/1.1" + CRLF;
            byte[] requestLineBuffer = Encoding.UTF8.
            GetBytes(requestLine);
            var headers =
            "Host: dnndev" + CRLF +
            "Content-Type: application/json" + CRLF +
            "Content-Length: " + contentBuffer.Length + CRLF +
            CRLF;
            byte[] headersBuffer = Encoding.UTF8.GetBytes(headers);
            s.Send(requestLineBuffer);
            s.Send(headersBuffer);
            s.Send(contentBuffer);
        }



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
    }
}
