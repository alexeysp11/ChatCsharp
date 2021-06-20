using System; 
using System.Net; 
using System.Net.Sockets; 

namespace Chat.Server.Network
{
    /// <summary>
    /// TCP listener for Chat application 
    /// </summary>
    public class ChatTcpListener : IProtocolListener
    {
        #region Members
        /// <summary>
        /// Instance of TcpListener
        /// </summary>
        /// <value>Readonly property</value>
        private TcpListener Listener { get; } = null; 
        #endregion  // Members
        
        #region Configuration properties
        /// <summary>
        /// IP address of TCP server
        /// </summary>
        /// <value>Readonly property</value>
        public IPAddress Ip { get; }
        /// <summary>
        /// Name of a TCP server 
        /// </summary>
        /// <value>Public access for reading, private access for changing</value>
        public string ServerName { get; private set; }
        /// <summary>
        /// Port of a TCP listener 
        /// </summary>
        /// <value>Readonly property</value>
        private int Port { get; }
        #endregion  // Configuration properties

        #region Messaging properties
        /// <summary>
        /// Message to read in bytes
        /// </summary>
        private byte[] MessageToReadByte = new byte[256]; 
        /// <summary>
        /// Message to read in string
        /// </summary>
        private string MessageToReadString; 
        /// <summary>
        /// Response from server in bytes
        /// </summary>
        private byte[] ServerResponseByte = new byte[256]; 
        #endregion  // Messaging properties

        #region Constructors
        /// <summary>
        /// Default constructor for ChatTcpListener class
        /// </summary>
        public ChatTcpListener()
        {
            this.Ip = IPAddress.Parse("127.0.0.1"); 
            this.ServerName = "localhost"; 
            this.Port = 13000; 
            this.Listener = new TcpListener(this.Ip, this.Port);
        }

        /// <summary>
        /// Alternative constructor of ChatTcpListener class 
        /// </summary>
        /// <param name="ip">Port of a TCP listener</param>
        /// <param name="serverName">Name of a TCP server</param>
        /// <param name="port">Port of a TCP listener</param>
        public ChatTcpListener(string ip, string serverName, int port)
        {
            this.Ip = IPAddress.Parse(ip);
            this.ServerName = serverName; 
            this.Port = port;
            this.Listener = new TcpListener(this.Ip, this.Port);
        }
        #endregion  // Constructors

        #region Methods
        /// <summary>
        /// Allows to listens for connections from TCP network clients
        /// </summary>
        public void Listen()
        {
            TcpClient client = null;

            try
            {
                // Start listening for client requests.
                this.Listener.Start();

                while(true)
                {
                    Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also use server.AcceptSocket() here.
                    client = this.Listener.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    this.MessageToReadString = null;

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    int i;

                    // Loop to receive all the data sent by the client.
                    while( (i = stream.Read(this.MessageToReadByte, 0, this.MessageToReadByte.Length)) != 0 )
                    {
                        // Translate data bytes to a ASCII string.
                        this.MessageToReadString = System.Text.Encoding.ASCII.GetString(this.MessageToReadByte, 0, i);
                        Console.WriteLine("Received: {0}", this.MessageToReadString);

                        // Process the data sent by the client.
                        this.MessageToReadString = this.MessageToReadString;

                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(this.MessageToReadString);

                        // Send back a response.
                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine("Sent: {0}", this.MessageToReadString);
                    }
                }
            }
            catch (System.ArgumentNullException e)
            {
                System.Console.WriteLine($"ArgumentNullException: {e}");
            }
            catch (SocketException e)
            {
                System.Console.WriteLine($"SocketException: {e}");
            }
            finally
            {
                // Shutdown and end connection
                client.Close();
                this.Listener.Stop();       // Stop listening for new clients.
            }
        }

        public void GetMessage()
        {

        }
        #endregion  // Methods
    }
}