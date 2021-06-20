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
        /// <value></value>
        private string Username { get; set; } 
        /// <summary>
        /// Message to send in bytes
        /// </summary>
        private byte[] MessageByte; 
        /// <summary>
        /// Response from server in bytes
        /// </summary>
        private byte[] ServerResponseByte = new byte[256]; 
        /// <summary>
        /// Response from server in string 
        /// </summary>
        private string ServerResponseString; 
        #endregion  // Messaging properties

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
            this.SendMessage($"User {this.Username} connected"); 
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
            this.SendMessage($"User {this.Username} connected"); 
        }
        #endregion  // Constructors

        #region Methods
        /// <summary>
        /// Allows to send data to the server 
        /// </summary>
        /// <param name="message">Message to send</param>
        public void SendMessage(string message)
        {
            if (this.Client == null)
            {
                this.Client = new TcpClient(this.ServerName, this.Port); 
            }

            try
            {
                // Send data to the server 
                this.MessageByte = System.Text.Encoding.ASCII.GetBytes(message); 
                this._NetworkStream = this.Client.GetStream();
                this._NetworkStream.Write(this.MessageByte, 0, this.MessageByte.Length);

                // Read the first batch of the TcpServer response bytes.
                int bytes = this._NetworkStream.Read(ServerResponseByte, 0, ServerResponseByte.Length);
                this.ServerResponseString = System.Text.Encoding.ASCII.GetString(ServerResponseByte, 0, bytes);

                this.CloseConnection(); 
            }
            catch (System.ArgumentNullException e)
            {
                System.Windows.MessageBox.Show($"ArgumentNullException: {e}");
            }
            catch (SocketException e)
            {
                System.Windows.MessageBox.Show($"SocketException: {e}");
            }
        }

        public void GetMessage()
        {

        }

        /// <summary>
        /// Allows to close everything
        /// </summary>
        public void CloseConnection()
        {
            if (this.Client != null)
            {
                this._NetworkStream.Close();
                this.Client.Close();
                this.Client = null; 
            }
        }
        #endregion  // Methods
    }
}