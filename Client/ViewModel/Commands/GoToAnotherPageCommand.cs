using Chat.Client.ViewModel; 

namespace Chat.Client.Commands
{
    public class GoToAnotherPageCommand : System.Windows.Input.ICommand
    {
        #region ViewModels
        private MainVM _MainVM { get; set; }
        #endregion  // ViewModels

        #region Constructor
        public GoToAnotherPageCommand(MainVM mainVM)
        {
            this._MainVM = mainVM; 
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
            if (parameterString == "Register")
            {
                this._MainVM.GoToRegisterPage(); 
            }
            else if (parameterString == "Login")
            {
                this._MainVM.GoToLoginPage(); 
            }
            else
            {
                System.Windows.MessageBox.Show($"Incorrect CommandParameter: {parameterString}"); 
            }
        }
    }
}