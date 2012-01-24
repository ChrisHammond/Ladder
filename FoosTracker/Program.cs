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
using SecretLabs.NETMF.Hardware.NetduinoPlus;

namespace FoosTracker
{
    public class Program
    {

        //create two teams (away/home)
        private static Team awayTeam = Team.CreateTeam(1, "Away");
        private static Team homeTeam = Team.CreateTeam(2, "Home");

        private const int WinningScore = 10;

        static readonly OutputPort awayLed = new OutputPort(Pins.GPIO_PIN_D12, false);
        static readonly OutputPort homeLed = new OutputPort(Pins.GPIO_PIN_D13, false);

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
            awayTeam.AddScore();
            awayLed.Write(true);
        }
        private static void awayTeamSubtract_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            awayTeam.SubtractScore();
            awayLed.Write(false);
        }
        private static void homeTeamAdd_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            homeTeam.AddScore();
            homeLed.Write(true);
        }
        private static void homeTeamSubtract_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            homeTeam.SubtractScore();
            homeLed.Write(false);
        }

        private static void CheckTeamScores()
        {
            if (awayTeam.Score < WinningScore && homeTeam.Score < WinningScore) return;
            
            //game over someone got 10

            if (awayTeam.Score >= WinningScore)
            {
                awayTeam.Wins++;
                homeTeam.Losses++;
            }
            else
            {//away team lost, home team won
                awayTeam.Losses++;
                homeTeam.Wins++;
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
        }

        //todo: record the temperature measurement at start of game, end of game
        //todo: allow user selection for teams

    }
}
