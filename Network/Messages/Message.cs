namespace Chat.Network.Messages
{
    public struct Message
    {
        public byte ClientId { get; private set; } 
        public byte MsgId { get; private set; } 

        public ushort TimeMinutesUshort { get; private set; } 
        public string TimeString
        {
            get 
            {
                int hours = (int)(TimeMinutesUshort / 60); 
                int minutes = (int)TimeMinutesUshort - (hours * 60); 
                return $"{hours}:{minutes}"; 
            }
        }

        public byte[] TextBytes { get; private set; } 
        public string TextString 
        {
            get { return System.Text.Encoding.ASCII.GetString(TextBytes); }
        }
        public string MessageString
        {
            get { return $"{TimeString} {TextString}\n"; }
        }

        public Message(byte clientId, byte msgId, ushort timeMinutesUshort, 
            byte[] textBytes)
        {
            ClientId = clientId; 
            MsgId = msgId;
            TimeMinutesUshort = timeMinutesUshort; 
            TextBytes = textBytes; 
        }
    }
}