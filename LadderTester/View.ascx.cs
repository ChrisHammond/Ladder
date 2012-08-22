/*
' Copyright (c) 2011  Christoc.com
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
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using DotNetNuke.Services.Exceptions;


namespace com.christoc.modules.LadderTester
{

    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The ViewLadderTester class displays the content
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class View : LadderTesterModuleBase
    {

        #region Event Handlers

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            Load += Page_Load;

        }


        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Page_Load runs when the control is loaded
        /// </summary>
        /// -----------------------------------------------------------------------------
        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //"{\"Teams\":[{\"TeamId\":0,\"Name\":\"Home\",\"Score\":\"" + homeTeam.Score + "\",\"Games\":0,\"Wins\":0,\"Losses\":0},{\"TeamId\":1,\"Name\":\"Away\",\"Score\":\"" + awayTeam.Score + "\",\"Games\":0,\"Wins\":0,\"Losses\":0}],\"FieldIdentifier\":\"" + _currentGame.FieldIdentifier + "\"}";
                if(!Page.IsPostBack)
                {
                    txtFieldIdentifier.Text = "00-B0-D0-86-BB-F7";
                    txtHomeTeam.Text = "10";
                    txtAwayTeam.Text = "5";
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        #endregion

        protected void lbSubmit_Click(object sender, EventArgs e)
        {
            //call the webservice 
            txtGameJson.Text = BuildJson();

            //CallWebService(txtGameJson.Text);

        }

        private string BuildJson()
        {
            var sb = new StringBuilder();
            if(txtGameId.Text.Trim()!=string.Empty)
                sb.Append("{\"GameId\":" + txtGameId.Text + ",");
            else
            {
                sb.Append("{");
            }

            sb.Append("\"Teams\":[{\"TeamId\":0,\"Name\":\"Home\",\"Score\":\"");
            sb.Append(txtHomeTeam.Text);
            sb.Append("\",\"Games\":0,\"Wins\":0,\"Losses\":0,\"HomeTeam\":true},{\"TeamId\":0,\"Name\":\"Away\",\"Score\":\"");
            sb.Append(txtAwayTeam.Text);
            sb.Append("\",\"Games\":0,\"Wins\":0,\"Losses\":0,\"HomeTeam\":false}],\"FieldIdentifier\":\"");
            sb.Append(txtFieldIdentifier.Text);
            sb.Append("\"}");
            return sb.ToString();
        }

        private void CallWebService(string jsonValue)
        {
            var address = txtServerUrl.Text;
            var hwr = WebRequest.Create(address) as HttpWebRequest;
            if (hwr != null)
            {
                hwr.Method = "Post";
                hwr.ContentType = "application/json";
                byte[] byteData = Encoding.UTF8.GetBytes(jsonValue);
                hwr.ContentLength = byteData.Length;
                Stream putStream = hwr.GetRequestStream();

                putStream.Write(byteData, 0, byteData.Length);

                putStream.Close();

                var response = hwr.GetResponse();

                var data = response.GetResponseStream();

                if (data != null)
                {
                    var dataReader = new StreamReader(data);
                    var responseFromServer = dataReader.ReadToEnd();

                    response.Close();

                    txtResult.Text = responseFromServer;
                    txtGameId.Text = responseFromServer;
                }



                String request = "POST /update HTTP/1.1\n";
                request += "Host: " + address + "\n";
                request += "Connection: close\n";
                //todo: should we have a specific key passed? maybe the field id, to allow the post?
                //request += "X-DNNFOOSAPIKEY: " + writeAPIKey + "\n";
                request += "Content-Type: application/json\n";
                request += "Content-Length: " + jsonValue.Length + "\n\n";

                request += jsonValue;

                try
                {
                    //provide the IP for post
                    //static string tsIP = "184.106.153.149";     // IP Address for the ThingSpeak API
                    //static Int32 tsPort = 80;                   // Port Number for ThingSpeak

                    String tsReply = sendPOST(address, 80, request);
                    
                }
                catch (SocketException se)
                {

                }
                
            }

        }

        //methods for the code below based on the ThinkSpeak API/netduino sample http://community.thingspeak.com/tutorials/netduino/create-your-own-web-of-things-using-the-netduino-plus-and-thingspeak/ 
        private static String sendPOST(String server, Int32 port, String request)
        {
            const Int32 c_microsecondsPerSecond = 1000000;

            // Create a socket connection to the specified server and port.
            using (Socket serverSocket = ConnectSocket(server, port))
            {
                // Send request to the server.
                Byte[] bytesToSend = Encoding.UTF8.GetBytes(request);
                serverSocket.Send(bytesToSend, bytesToSend.Length, 0);

                // Reusable buffer for receiving chunks of the document.
                Byte[] buffer = new Byte[1024];

                // Accumulates the received page as it is built from the buffer.
                String page = String.Empty;

                // Wait up to 30 seconds for initial data to be available.  Throws an exception if the connection is closed with no data sent.
                DateTime timeoutAt = DateTime.Now.AddSeconds(30);
                while (serverSocket.Available == 0 && DateTime.Now < timeoutAt)
                {
                    System.Threading.Thread.Sleep(100);
                }

                // Poll for data until 30-second timeout.  Returns true for data and connection closed.
                while (serverSocket.Poll(30 * c_microsecondsPerSecond, SelectMode.SelectRead))
                {
                    // If there are 0 bytes in the buffer, then the connection is closed, or we have timed out.
                    if (serverSocket.Available == 0) break;

                    // Zero all bytes in the re-usable buffer.
                    Array.Clear(buffer, 0, buffer.Length);

                    // Read a buffer-sized HTML chunk.
                    Int32 bytesRead = serverSocket.Receive(buffer);

                    // Append the chunk to the string.
                    page = page + new String(Encoding.UTF8.GetChars(buffer));
                }

                // Return the complete string.
                return page;
            }
        }

        // Creates a socket and uses the socket to connect to the server's IP address and port. (From the .NET Micro Framework SDK example)
        private static Socket ConnectSocket(String server, Int32 port)
        {
            // Get server's IP address.
            IPHostEntry hostEntry = Dns.GetHostEntry(server);
            // Create socket and connect to the server's IP address and port
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(new IPEndPoint(hostEntry.AddressList[0], port));
            return socket;
        }

        //static void delayLoop(int interval)
        //{
        //    long now = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        //    int offset = (int)(now % interval);
        //    int delay = interval - offset;
        //    Thread.Sleep(delay);
        //}
    }

}
