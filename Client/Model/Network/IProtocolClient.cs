namespace Chat.Client.Network
{
    /// <summary>
    /// Common interface for all protocols using in this application 
    /// </summary>
    public interface IProtocolClient
    {
        void SendMessage(string message);
        void GetMessage();
        void CloseConnection(); 
    }
}