namespace Chat.Server.Network
{
    /// <summary>
    /// Provides a format for message sending
    /// </summary>
    public class Message
    {
        #region Properties
        /// <summary>
        /// ID of a client 
        /// </summary>
        /// <value>Public static property</value>
        public byte ClientId { get; private set; } 
        /// <summary>
        /// Header of a message 
        /// </summary>
        /// <value>Public static property</value>
        public byte Header { get; private set; } 
        /// <summary>
        /// Text of a message 
        /// </summary>
        /// <value>Public static property</value>
        public string Text { get; private set; } 
        #endregion  // Properties

        #region Constructors
        /// <summary>
        /// Constructor of a Message class 
        /// </summary>
        /// <param name="clientId">ID of a sender</param>
        /// <param name="header">Header of a message</param>
        /// <param name="text">Text of a message</param>
        public Message(byte clientId, byte header, string text) 
        {
            this.ClientId = clientId;
            this.Header = header;
            this.Text = text;
        }
        #endregion  // Constructors
    }
}