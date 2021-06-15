using System.Windows; 
using System.Windows.Input; 
using Chat.Client.Commands;
using Chat.Client.View;
using Chat.Client.Exceptions;
using Chat.Client.Database; 

namespace Chat.Client.ViewModel
{
    /// <summary>
    /// Main ViewModel that connects database and MainWindow
    /// </summary>
    public class MainVM
    {
        #region Members
        /// <summary>
        /// Allows to get current window and close it while moving to an other window
        /// </summary>
        public MainWindow CurrentWindow { get; private set; } = null; 
        /// <summary>
        /// Allows to get all information about the user authenticated at the current time  
        /// </summary>
        public UserModel CurrentUser { get; private set; } = null; 
        #endregion  // Members

        #region ViewModels
        /// <summary>
        /// ViewModel for displaying messages 
        /// </summary>
        public MessagesVM MessagesVM { get; private set; }
        #endregion  // ViewModels

        #region Commands
        /// <summary>
        /// Allows to move to an other window
        /// </summary>
        public ICommand GoToAnotherPageCommand { get; private set; }
        /// <summary>
        /// Allows to authenticate users
        /// </summary>
        public ICommand AuthCommand { get; private set; }
        /// <summary>
        /// Allows to exit the application 
        /// </summary>
        public ICommand ExitCommand { get; private set; }
        /// <summary>
        /// Sends a message to the chat 
        /// </summary>
        public ICommand MessageCommand { get; private set; }
        #endregion  // Commands

        #region Constructor
        /// <summary>
        /// Public constructor of MainVM
        /// </summary>
        /// <param name="mainWindow">Instance of MainWindow (for convinient way to access UI elements)</param>
        public MainVM(MainWindow mainWindow)
        {
            // Initialize commands
            this.GoToAnotherPageCommand = new GoToAnotherPageCommand(this); 
            this.AuthCommand = new AuthCommand(this); 
            this.ExitCommand = new ExitCommand(this); 
            this.MessageCommand = new MessageCommand(this); 

            // Initialize ViewModels
            this.MessagesVM = new MessagesVM(this); 

            // Assign window
            CurrentWindow = mainWindow; 

            // Create DB 
            SqliteDbHelper.Instance.CreateUserTable(); 

            // Set initial number of characters available in the message 
            this.SetNumberOfAvailableCharsInTextBlock(); 
        }
        #endregion  // Constructor

        #region Methods for going to an other page
        /// <summary>
        /// Allows to go to the register page 
        /// </summary>
        public void GoToRegisterPage()
        {
            // Welcome page
            CurrentWindow.Welcome.Visibility = Visibility.Collapsed; 
            CurrentWindow.Welcome.IsEnabled = false; 

            // Registration page
            CurrentWindow.Registration.Visibility = Visibility.Visible; 
            CurrentWindow.Registration.IsEnabled = true; 

            // Login page
            CurrentWindow.Login.Visibility = Visibility.Collapsed; 
            CurrentWindow.Login.IsEnabled = false; 

            // UserPage page
            CurrentWindow.UserPage.Visibility = Visibility.Collapsed; 
            CurrentWindow.UserPage.IsEnabled = false; 
        }

        /// <summary>
        /// Allows to go to the login page
        /// </summary>
        public void GoToLoginPage()
        {
            // Welcome page
            CurrentWindow.Welcome.Visibility = Visibility.Collapsed; 
            CurrentWindow.Welcome.IsEnabled = false; 

            // Registration page
            CurrentWindow.Registration.Visibility = Visibility.Collapsed; 
            CurrentWindow.Registration.IsEnabled = false; 

            // Login page
            CurrentWindow.Login.Visibility = Visibility.Visible; 
            CurrentWindow.Login.IsEnabled = true; 

            // UserPage page
            CurrentWindow.UserPage.Visibility = Visibility.Collapsed; 
            CurrentWindow.UserPage.IsEnabled = false; 
        }

        /// <summary>
        /// Allows to go to the user page
        /// </summary>
        public void GoToUserPage()
        {
            // Welcome page
            CurrentWindow.Welcome.Visibility = Visibility.Collapsed; 
            CurrentWindow.Welcome.IsEnabled = false; 

            // Registration page
            CurrentWindow.Registration.Visibility = Visibility.Collapsed; 
            CurrentWindow.Registration.IsEnabled = false; 

            // Login page
            CurrentWindow.Login.Visibility = Visibility.Collapsed; 
            CurrentWindow.Login.IsEnabled = false; 

            // UserPage page
            CurrentWindow.UserPage.Visibility = Visibility.Visible; 
            CurrentWindow.UserPage.IsEnabled = true; 
        }

        /// <summary>
        /// Allows to exit the application 
        /// </summary>
        public void Exit()
        {
            MessageBoxResult result = System.Windows.MessageBox.Show("Are you sure to exit the application?", "Exit the application", MessageBoxButton.YesNo); 
            if (result == MessageBoxResult.Yes)
            {
                this.CurrentUser = null; 
                Application.Current.Shutdown();
            }
        }
        #endregion  // Methods for going to an other page

        #region Submit methods
        /// <summary>
        /// Allows get if user exists in database 
        /// </summary>
        public void SubmitLogin()
        {
            if (string.IsNullOrWhiteSpace(CurrentWindow.UsernameLogin.Text) ||
                string.IsNullOrWhiteSpace(CurrentWindow.PasswordLogin.Password))
            {
                CurrentWindow.MessageLogin.Text = "Fill in all fields, please!";
            }
            else
            {
                // Check if this user already exists in database
                this.CheckUserInDatabaseOnLogin(); 
            }
        }

        /// <summary>
        /// Allows to save user in the database on the register page
        /// </summary>
        public void SubmitRegistration()
        {
            if (string.IsNullOrWhiteSpace(CurrentWindow.UsernameReg.Text) ||
                string.IsNullOrWhiteSpace(CurrentWindow.EmailReg.Text) ||
                string.IsNullOrWhiteSpace(CurrentWindow.PasswordBoxReg.Password) ||
                string.IsNullOrWhiteSpace(CurrentWindow.ConfirmPasswordBoxReg.Password))
            {
                CurrentWindow.MessageReg.Text = "Fill in all fields, please!";
            }
            else
            {
                if (CurrentWindow.PasswordBoxReg.Password == CurrentWindow.ConfirmPasswordBoxReg.Password)
                {
                    // Insert new user into DB 
                    this.RegisterUserInDb();
                }
                else
                {
                    CurrentWindow.MessageReg.Text = "Passwords do not match!";
                }
            }
        }

        /// <summary>
        /// Allows to check if the user exists in the database
        /// </summary>
        private void CheckUserInDatabaseOnLogin()
        {
            using ( UserModel user = new UserModel(CurrentWindow.UsernameLogin.Text, null, CurrentWindow.PasswordLogin.Password) )
            {
                if (SqliteDbHelper.Instance.IsAuthenticated(user))
                {
                    CurrentWindow.MessageLogin.Text = "Successfully submitted!";
                    this.CurrentUser = user; 
                    this.GoToUserPage(); 
                }
                else
                {
                    CurrentWindow.MessageLogin.Text = "Register first!";
                    this.GoToRegisterPage(); 
                }
            }
        }

        /// <summary>
        /// Allows to insert users into database 
        /// </summary>
        private void RegisterUserInDb()
        {
            using ( UserModel user = new UserModel(CurrentWindow.UsernameReg.Text, CurrentWindow.EmailReg.Text, CurrentWindow.PasswordBoxReg.Password) )
            {
                if (SqliteDbHelper.Instance.IsAuthenticated(user))
                {
                    System.Windows.MessageBox.Show("This user is already exists in DB.\nGo to the Login Page.", "Authentication error", MessageBoxButton.OK); 
                    this.GoToLoginPage();
                }
                else
                {
                    SqliteDbHelper.Instance.InsertDataIntoUserTable(user); 
                    if (SqliteDbHelper.Instance.IsAuthenticated(user))
                    {
                        CurrentWindow.MessageReg.Text = "Successfully submitted!"; 
                        this.GoToLoginPage();
                    }
                    else
                    {
                        CurrentWindow.MessageReg.Text = "Unable to insert user into database";
                    }
                }
            }
        }
        #endregion  // Submit methods

        #region Communication methods 
        /// <summary>
        /// Sends message via TCP/UDP protocols 
        /// </summary>
        public void SendMessage()
        {
            this.MessagesVM.MessagesInChat += $"{this.CurrentUser.Name}: {this.MessagesVM.MessageToSend}\n"; 
            this.MessagesVM.MessageToSend = System.String.Empty; 
        }

        /// <summary>
        /// Sets string to display number of available chars while typing the message 
        /// </summary>
        /// <param name="charsInMessage">Number of characters in the current message</param>
        public void SetNumberOfAvailableCharsInTextBlock(int charsInMessage=0)
        {
            int maxLength = this.CurrentWindow.MessageToSendTextBox.MaxLength; 
            this.MessagesVM.CharsAvailable = $" ({maxLength - charsInMessage}/{maxLength}) "; 
        }
        #endregion  // Communication methods 
    }
}