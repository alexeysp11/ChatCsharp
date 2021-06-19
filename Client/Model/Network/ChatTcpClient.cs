using System.Net.Sockets; 

namespace Chat.Client.Network
{
    /// <summary>
    /// TCP client for this application 
    /// </summary>
    public class ChatTcpClient : IProtocolClient
    {
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
        /// <summary>
        /// Instance of TcpClient
        /// </summary>
        /// <value>Readonly property</value>
        private TcpClient Client { get; } = null; 
        #endregion  // Configuration properties

        #region Messaging properties
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
        public ChatTcpClient()
        {
            this.Ip = "127.0.0.0"; 
            this.ServerName = "localhost"; 
            this.Port = 13000;
            this.Client = new TcpClient(ServerName, Port);
        }

        /// <summary>
        /// Alternative constructor of ChatTcpClient class 
        /// </summary>
        /// <param name="ip">Port of a TCP client</param>
        /// <param name="serverName">Name of a TCP server</param>
        /// <param name="port">Port of a TCP client</param>
        public ChatTcpClient(string ip, string serverName, int port)
        {
            this.Ip = ip;
            this.ServerName = serverName; 
            this.Port = port;
            this.Client = new TcpClient(ServerName, Port);
        }
        #endregion  // Constructors

        #region Methods
        /// <summary>
        /// Allows to send data to the server 
        /// </summary>
        /// <param name="message">Message to send</param>
        public void SendMessage(string message)
        {
            try
            {
                // Send data to the server 
                this.MessageByte = System.Text.Encoding.ASCII.GetBytes(message); 
                NetworkStream stream = this.Client.GetStream();
                stream.Write(this.MessageByte, 0, this.MessageByte.Length);

                // Read the first batch of the TcpServer response bytes.
                int bytes = stream.Read(ServerResponseByte, 0, ServerResponseByte.Length);
                this.ServerResponseString = System.Text.Encoding.ASCII.GetString(ServerResponseByte, 0, bytes);
                System.Windows.MessageBox.Show($"Received: {ServerResponseByte}");

                // Close everything.
                stream.Close();
                this.Client.Close();
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
        #endregion  // Methods
    }
}