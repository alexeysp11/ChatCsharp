using Chat.Client.ViewModel; 

namespace Chat.Client.Commands
{
    /// <summary>
    /// Command that allows to send and format messages 
    /// </summary>
    public class MessageCommand : System.Windows.Input.ICommand
    {
        #region ViewModels
        /// <summary>
        /// Instance of MainVM
        /// </summary>
        /// <value>Private set and get</value>
        private MainVM _MainVM { get; set; }
        #endregion  // ViewModels

        #region Constructor
        /// <summary>
        /// Constructor of MessageCommand
        /// </summary>
        /// <param name="mainVM">Instance of MainVM</param>
        public MessageCommand(MainVM mainVM)
        {
            _MainVM = mainVM; 
        }
        #endregion  // Constructor

        #region Event handling
        /// <summary>
        /// Event for handling changes of CanExecute
        /// </summary>
        public event System.EventHandler CanExecuteChanged; 

        /// <summary>
        /// Method for getting if MessageCommand can be executed 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>True if user is on the UserPage, flase if user is not on the UserPage</returns>
        public bool CanExecute(object parameter)
        {
            bool result = true; 
            try
            {
                if (this._MainVM.CurrentWindow.UserPage.IsEnabled && 
                    this._MainVM.CurrentWindow.UserPage.Visibility == System.Windows.Visibility.Visible)
                {
                    result = true; 
                }
                else
                {
                    result = false; 
                }
            }
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show($"Exception inside CommandParameter:\n{e}", "Exception"); 
            }
            return result; 
        }

        /// <summary>
        /// Invokes methods from MainVM for setting new line and sending the message 
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            string parameterString = parameter as string; 
            if (parameterString == "Send")
            {
                this._MainVM.SendMessage(); 
            }
            else
            {
                System.Windows.MessageBox.Show($"Incorrect CommandParameter: {parameterString} inside MessageCommand", "Exception"); 
            }
        }
        #endregion  // Event handling
    }
}