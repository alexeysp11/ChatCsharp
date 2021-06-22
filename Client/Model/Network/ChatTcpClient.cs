using System.Net.Sockets; 

namespace Chat.Client.Network
{
    /// <summary>
    /// TCP client for this application 
    /// </summary>
    public class ChatTcpClient : IProtocolClient
    {
        #region Members
        /// <summary>
        /// Instance of TcpClient
        /// </summary>
        /// <value>Private property for getting and setting</value>
        private TcpClient Client { get; set; } = null; 
        /// <summary>
        /// Instance of NetworkStream 
        /// </summary>
        /// <value>Private property for getting and setting</value>
        private NetworkStream _NetworkStream { get; set; } = null; 
        #endregion  // Members

        #region Configuration properties
        /// <summary>
        /// IP address of TCP server
        /// </summary>
        /// <value>Readonly property</value>
        public string Ip { get; }
        /// <summary>
        /// Name of a TCP server 
        /// </summary>
        /// <value>Public access for reading, private access for changing</value>
        public string ServerName { get; private set; }
        /// <summary>
        /// Port of a TCP client 
        /// </summary>
        /// <value>Readonly property</value>
        private int Port { get; }
        #endregion  // Configuration properties

        #region Messaging properties
        /// <summary>
        /// Name of the user 
        /// </summary>
        /// <value>Private property</value>
        private string Username { get; set; } 
        /// <summary>
        /// ID of a client 
        /// </summary>
        /// <value>Private property</value>
        private byte ClientId { get; set; }
        /// <summary>
        /// Header of a message 
        /// </summary>
        /// <value>Private property</value>
        private byte MessageHeader { get; set; }
        #endregion  // Messaging properties
        
        #region Messages
        /// <summary>
        /// Message to send in bytes
        /// </summary>
        private byte[] MessageByteArray; 
        /// <summary>
        /// Response from server in bytes
        /// </summary>
        private byte[] ServerResponseByte; 
        /// <summary>
        /// Response from server in string 
        /// </summary>
        private string ServerResponseString; 
        #endregion  // Messages

        #region Constructors
        /// <summary>
        /// Default constructor for ChatTcpClient class
        /// </summary>
        public ChatTcpClient(string username)
        {
            this.Ip = "127.0.0.0"; 
            this.ServerName = "localhost"; 
            this.Port = 13000;
            this.Username = username; 
            this.CreateClient(); 
        }

        /// <summary>
        /// Alternative constructor of ChatTcpClient class 
        /// </summary>
        /// <param name="ip">Port of a TCP client</param>
        /// <param name="serverName">Name of a TCP server</param>
        /// <param name="port">Port of a TCP client</param>
        public ChatTcpClient(string ip, string serverName, int port, string username)
        {
            this.Ip = ip;
            this.ServerName = serverName; 
            this.Port = port;
            this.Username = username;
            this.CreateClient(); 
        }
        #endregion  // Constructors

        #region Methods
        /// <summary>
        /// Method for creating a client in the chat 
        /// </summary>
        private void CreateClient()
        {
            this.ClientId = 0; 
            this.MessageHeader = 2; 
            try
            {
                this.SendMessage($"User {this.Username} connected"); 
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Allows to send data to the server 
        /// </summary>
        /// <param name="message">Message to send</param>
        public void SendMessage(string message, bool isConfig=true)
        {
            if (!isConfig)
            {
                this.MessageHeader = 0; 
            }

            // Create message for sending. 
            byte[] text = System.Text.Encoding.ASCII.GetBytes(message); 
            this.MessageByteArray = new System.Byte[text.Length + 2]; 
            this.MessageByteArray[0] = this.ClientId; 
            this.MessageByteArray[1] = this.MessageHeader; 
            for (int i = 0; i < text.Length; i++)
            {
                this.MessageByteArray[i+2] = text[i]; 
            }

            // Assign client if necessary. 
            if (this.Client == null)
            {
                this.Client = new TcpClient(this.ServerName, this.Port); 
            }

            // Sending message to the server using network stream. 
            try
            {
                this._NetworkStream = this.Client.GetStream();
                this._NetworkStream.Write(this.MessageByteArray, 0, this.MessageByteArray.Length);
            }
            catch (System.Exception e)
            {
                throw e;
            }

            // Getting response from the server. 
            if (this.MessageHeader == 1)    // If it was getting all messages from the server. 
            {
                // Get messages from the server in bytes. 
                byte[] messages = new byte[1024]; 
                int msgLength;
                try
                {
                    msgLength = this._NetworkStream.Read(messages, 0, messages.Length); 
                }
                catch (System.Exception e)
                {
                    throw e;
                }

                // Process messages to ignore null characters. 
                if (msgLength <= 2)
                {
                    this.ServerResponseString = string.Empty; 
                }
                else
                {
                    // Decode processed messages and print them out. 
                    byte[] processedMessages = new byte[msgLength-2]; 
                    for (int i = 0; i < processedMessages.Length; i++)
                    {
                        processedMessages[i] = messages[i+2]; 
                    }
                    this.ServerResponseString = System.Text.Encoding.ASCII.GetString(processedMessages); 
                }
            }
            else if (this.MessageHeader == 2)   // If it was registration. 
            {
                // Get ID from the server and initialize ClientId. 
                byte[] IDs = new byte[1]; 
                try
                {
                    this._NetworkStream.Read(IDs, 0, IDs.Length);
                }
                catch (System.Exception e)
                {
                    throw e;
                }
                this.ClientId = IDs[0]; 
            }

            // Close connection with the server. 
            try
            {
                this.CloseConnection(); 
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Makes request to the server and asks if there was some messages received 
        /// </summary>
        /// <returns>String representation of all messages</returns>
        public string GetMessages()
        {
            this.MessageHeader = 1; 
            try
            {
                this.SendMessage(System.String.Empty); 
            }
            catch (System.Exception e)
            {
                throw e;
            }
            string response = this.ServerResponseString; 
            return response; 
        }

        /// <summary>
        /// Allows to close everything
        /// </summary>
        public void CloseConnection()
        {
            if (this.Client != null)
            {
                try
                {
                    if (this._NetworkStream != null)
                    {
                        this._NetworkStream.Close();
                    }
                    this.Client.Close();
                }
                catch (System.Exception e)
                {
                    throw e; 
                }
                finally
                {
                    this.Client = null; 
                }
            }
        }
        #endregion  // Methods
    }
}