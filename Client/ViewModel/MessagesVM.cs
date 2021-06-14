using System.ComponentModel;

namespace Chat.Client.ViewModel
{
    /// <summary>
    /// ViewModel for sending messages using System.ComponentModel.INotifyPropertyChanged
    /// </summary>
    public class MessagesVM : INotifyPropertyChanged
    {
        #region Properties
        /// <summary>
        /// Private field for storing massages in the current chat  
        /// </summary>
        private string messagesInChat;
        /// <summary>
        /// Public property for setting and getting messages in the current chat
        /// </summary>
        /// <value>Sets and gets messagesInChat</value>
        public string MessagesInChat
        {
            get { return messagesInChat; }
            set 
            {
                messagesInChat = value;
                OnPropertyChanged("MessagesInChat");
            }
        }
        /// <summary>
        /// Private field for storing a massage that needs to be sent 
        /// </summary>
        private string messageToSend; 
        /// <summary>
        /// Public property for setting and getting a massage that needs to be sent
        /// </summary>
        /// <value>Sets and gets messageToSend</value>
        public string MessageToSend 
        {
            get { return messageToSend; } 
            set
            {
                messageToSend = value;
                OnPropertyChanged("MessageToSend");
            }
        }
        #endregion  // Properties

        #region Event handling
        /// <summary>
        /// Event for handling changes of the property 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Method that is called whenever the property is updated
        /// </summary>
        /// <param name="PropertyName">Name of an updated property</param>
        public void OnPropertyChanged(string PropertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(PropertyName);
                handler(this, e);
            }
        }
        #endregion  // Event handling
    }
}