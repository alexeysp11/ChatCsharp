using Chat.Client.ViewModel; 

namespace Chat.Client.Commands
{
    public class SendCommand : System.Windows.Input.ICommand
    {
        #region ViewModels
        private MainVM _MainVM { get; set; }
        #endregion  // ViewModels

        #region Constructor
        public SendCommand(MainVM mainVM)
        {
            _MainVM = mainVM; 
        }
        #endregion  // Constructor

        #region Event handling
        public event System.EventHandler CanExecuteChanged; 

        public bool CanExecute(object parameter)
        {
            bool result = false; 
            try
            {
                if (this._MainVM.CurrentWindow.UserPage.IsEnabled && 
                    this._MainVM.CurrentWindow.UserPage.Visibility == System.Windows.Visibility.Visible)
                {
                    result = true; 
                }
                System.Windows.MessageBox.Show($"CanExecute: {result}"); 
            }
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show($""); 
            }
            return result; 
        }

        public void Execute(object parameter)
        {
            this._MainVM.SendMessage(); 
        }
        #endregion  // Event handling
    }
}