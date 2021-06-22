using System; 
using System.Collections.Generic; 
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
        /// 
        /// </summary>
        private int MessagesNumber = 0; 
        /// <summary>
        /// Response from server in bytes
        /// </summary>
        private List<string> MessagesList = new List<string>(); 
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
            List<Message> msgList = new List<Message>();
            this.MessageToReadString = null;
            byte id = 0; 
            byte msgIndex = 0; 

            try
            {
                Listener.Start();
                while(true)
                {
                    byte[] MessageToReadByte = new byte[256]; 
                    
                    Console.Write("[OK] ");

                    // Get client and stream. 
                    client = Listener.AcceptTcpClient();
                    NetworkStream stream = client.GetStream();
                    
                    // Read bytes from the stream. 
                    int msgLength = stream.Read(MessageToReadByte, 0, MessageToReadByte.Length); 
                                        
                    // Process request. 
                    if (MessageToReadByte[1] == 1)  // Request for messages. 
                    {
                        byte[] responseByte = new byte[1]; 
                        System.Console.WriteLine($"Request for messages from User{MessageToReadByte[0]}");
                        
                        // Assign messages in bytes and string. 
                        string messageString = "";
                        msgIndex = MessageToReadByte[2]; 

                        // Get string of messages from the last request. 
                        List<Message> msgListFromLastRequest = msgList.GetRange(msgIndex, msgList.Count - msgIndex);
                        foreach (var messageObject in msgListFromLastRequest)
                        {
                            messageString += $"User{messageObject.ClientId}: {messageObject.Text}\n"; 
                        }
                        
                        // Encode the message. 
                        byte[] text = System.Text.Encoding.ASCII.GetBytes(messageString); 
                        byte[] messages = new byte[text.Length + 2]; 
                        messages[0] = MessageToReadByte[0]; 
                        messages[1] = MessageToReadByte[1]; 
                        for (int i = 0; i < text.Length; i++)
                        {
                            messages[i+2] = text[i]; 
                        }

                        System.Console.WriteLine($"{messages[0]} {messages[1]} {messageString}.\nSize: {messages.Length}");

                        // Send messages using stream. 
                        stream.Write(messages, 0, messages.Length); 
                    }
                    else if (MessageToReadByte[1] == 2)     // Request for registration. 
                    {
                        id += 1; 
                        byte[] IDs = new byte[1]; 
                        IDs[0] = id; 
                        stream.Write(IDs, 0, IDs.Length); 
                        System.Console.WriteLine($"User{id} connected");
                    }
                    else                                    // Usually it's ordanary message with code 0. 
                    {
                        string text = System.Text.Encoding.ASCII.GetString(MessageToReadByte, 2, msgLength); 
                        msgList.Add( new Message(MessageToReadByte[0], MessageToReadByte[1], text) ); 
                        System.Console.WriteLine($"User{MessageToReadByte[0]}: {text}");
                    }
                }
            }
            catch (System.Exception e)
            {
                throw e;
            }
            finally
            {
                client.Close();             // Shutdown and end connection. 
                this.Listener.Stop();       // Stop listening for new clients.
            }
        }

        public void GetMessage()
        {

        }
        #endregion  // Methods
    }
}