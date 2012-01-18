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
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;

namespace FoosTracker
{
    public class Program
    {

        //create two teams (red/blue)
        private static Team redTeam = Team.CreateTeam(1, "Red");
        private static Team blueTeam = Team.CreateTeam(2, "Blue");

        static OutputPort redLed = new OutputPort(Pins.GPIO_PIN_D12, false);
        static OutputPort blueLed = new OutputPort(Pins.GPIO_PIN_D13, false);

        public static void Main()
        {
            // write your code here

            //keep track of the scores

           

            var redTeamAdd = new InterruptPort(Pins.GPIO_PIN_D2, true, Port.ResistorMode.PullUp,
                                               Port.InterruptMode.InterruptEdgeLow);
            var redTeamSubtract = new InterruptPort(Pins.GPIO_PIN_D3, true, Port.ResistorMode.PullUp,
                                               Port.InterruptMode.InterruptEdgeLow);
            var blueTeamAdd = new InterruptPort(Pins.GPIO_PIN_D4, true, Port.ResistorMode.PullUp,
                                               Port.InterruptMode.InterruptEdgeLow);
            var blueTeamSubtract = new InterruptPort(Pins.GPIO_PIN_D5, true, Port.ResistorMode.PullUp,
                                               Port.InterruptMode.InterruptEdgeLow);

            redTeamAdd.OnInterrupt+=new NativeEventHandler(redTeamAdd_OnInterrupt);
            redTeamSubtract.OnInterrupt += new NativeEventHandler(redTeamSubtract_OnInterrupt);
            blueTeamAdd.OnInterrupt += new NativeEventHandler(blueTeamAdd_OnInterrupt);
            blueTeamSubtract.OnInterrupt += new NativeEventHandler(blueTeamSubtract_OnInterrupt);

            while(true)
            {
                //todo: check for Max score/win
                CheckTeamScores();
            }
        }

        private static void redTeamAdd_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            redTeam.AddScore();
            redLed.Write(true);
        }
        private static void redTeamSubtract_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            redTeam.SubtractScore();
            redLed.Write(false);
        }
        private static void blueTeamAdd_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            blueTeam.AddScore();
            blueLed.Write(true);
        }
        private static void blueTeamSubtract_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            blueTeam.SubtractScore();
            blueLed.Write(false);
        }

        private static void CheckTeamScores()
        {
            if(redTeam.Score>9 || blueTeam.Score>9)
            {
                //game over someone got 10
                var gameOver = true;

                if (redTeam.Score > 9)
                {
                    redTeam.Wins++;
                    blueTeam.Losses++;
                }
                else
                {//red team lost, blue team won
                    redTeam.Losses++;
                    blueTeam.Wins++;
                }
                //todo: play game over audio

                //call NewGame to reset the teams
                NewGame();
            }
        }


        private static void NewGame()
        {
            //reset the scores
            redTeam.Score = 0;
            blueTeam.Score = 0;
        }

        //todo: record the temperature measurement at start of game, end of game
        //todo: 

    }
}
