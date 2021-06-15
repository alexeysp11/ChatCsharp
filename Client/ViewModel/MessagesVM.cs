using System.ComponentModel;

namespace Chat.Client.ViewModel
{
    /// <summary>
    /// ViewModel for sending messages using System.ComponentModel.INotifyPropertyChanged
    /// </summary>
    public class MessagesVM : INotifyPropertyChanged
    {
        #region ViewModels
        /// <summary>
        /// Instance of MainVM
        /// </summary>
        private MainVM _MainVM { get; set; }
        #endregion  // ViewModels
        
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
                // Set private field. 
                messageToSend = value; 

                // Notify about length of a string. 
                this._MainVM.SetNumberOfAvailableCharsInTextBlock(messageToSend.Length); 

                // Notify the Control that property value has changed. 
                OnPropertyChanged("MessageToSend");
            }
        }
        /// <summary>
        /// Private field for storing number of characters that are available in the message
        /// </summary>
        private string charsAvailable;
        /// <summary>
        /// Public property for setting and getting number of characters that are available in the message
        /// </summary>
        /// <value>Sets and gets charsAvailable</value>
        public string CharsAvailable
        {
            get { return charsAvailable; }
            set 
            { 
                charsAvailable = value; 
                OnPropertyChanged("CharsAvailable");
            }
        }
        #endregion  // Properties

        #region Constructor 
        /// <summary>
        /// Constructor of MessagesVM 
        /// </summary>
        /// <param name="mainVM">Instance of MainVM</param>
        public MessagesVM(MainVM mainVM)
        {
            this._MainVM = mainVM;
        }
        #endregion  // Constructor 

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