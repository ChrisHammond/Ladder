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
using System.Text;
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

            CallWebService(txtGameJson.Text);

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
            var address = "http://dnndev/svc/ladder/Game";
            var hwr = WebRequest.Create(address) as HttpWebRequest;
            if (hwr != null)
            {
                hwr.Method = "PUT";
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

                // Get response  
                //using (HttpWebResponse response = hwr.GetResponse() as HttpWebResponse)
                //{
                //    // Get the response stream  
                //    StreamReader reader = new StreamReader(hwr.GetRequestStream());

                //    // Console application output  
                //    txtResult.Text = reader.ReadToEnd();
                //}  

            }

        }
    }

}
