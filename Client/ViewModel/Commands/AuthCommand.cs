using Chat.Client.ViewModel; 

namespace Chat.Client.Commands
{
    public class AuthCommand : System.Windows.Input.ICommand
    {
        #region ViewModels
        private MainVM _MainVM { get; set; }
        #endregion  // ViewModels

        #region Constructor
        public AuthCommand(MainVM mainVM)
        {
            _MainVM = mainVM; 
        }
        #endregion  // Constructor

        public event System.EventHandler CanExecuteChanged; 

        public bool CanExecute(object parameter)
        {
            return true; 
        }

        public void Execute(object parameter)
        {
            string parameterString = parameter as string; 
            if (parameterString == "Registration")
            {
                this._MainVM.SubmitRegistration(); 
            }
            else if (parameterString == "Login")
            {
                this._MainVM.SubmitLogin(); 
            }
            else
            {
                System.Windows.MessageBox.Show($"Incorrect CommandParameter: {parameterString}"); 
            }
        }
    }
}