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

            //register the interrupts to keep track of button presses
            awayTeamAdd.OnInterrupt+=awayTeamAdd_OnInterrupt;
            awayTeamSubtract.OnInterrupt += awayTeamSubtract_OnInterrupt;
            homeTeamAdd.OnInterrupt += homeTeamAdd_OnInterrupt;
            homeTeamSubtract.OnInterrupt += homeTeamSubtract_OnInterrupt;

            _currentGame.FieldIdentifier = Mac();

            //todo: use the mac address as the identifier
            

            while(true)
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
            _currentGame.Teams.Add(awayTeam);
            _currentGame.Teams.Add(homeTeam);
            //todo: call the webservice to initialize the new game
        }

        //todo: record the temperature measurement at start of game, end of game
        //todo: allow user selection for teams


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
