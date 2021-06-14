using Chat.Client.ViewModel; 

namespace Chat.Client.Commands
{
    public class ExitCommand : System.Windows.Input.ICommand
    {
        #region ViewModels
        private MainVM _MainVM { get; set; }
        #endregion  // ViewModels

        #region Constructor
        public ExitCommand(MainVM mainVM)
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
            this._MainVM.Exit(); 
        }
    }
}