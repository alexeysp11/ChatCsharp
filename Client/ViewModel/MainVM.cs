using System.Windows; 
using System.Windows.Input; 
using System.Windows.Threading; 
using Chat.Client.Commands;
using Chat.Client.View;
using Chat.Client.Exceptions;
using Chat.Client.Database; 
using Chat.Client.Network; 

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
        /// <summary>
        /// Client for using netwrok and interacting with server and other clients
        /// </summary>
        private IProtocolClient Client = null; 
        /// <summary>
        /// Timer for getting messages from the server . 
        /// </summary>
        /// <remarks>
        /// Started after user joined the chat after LoginPage. 
        /// </remarks>
        DispatcherTimer GettingMsgTimer = null;
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
            // Initialize property for getting MainWindow from the code.  
            CurrentWindow = mainWindow; 

            // ViewModels. 
            this.MessagesVM = new MessagesVM(this); 

            // Commands. 
            this.GoToAnotherPageCommand = new GoToAnotherPageCommand(this); 
            this.AuthCommand = new AuthCommand(this); 
            this.ExitCommand = new ExitCommand(this); 
            this.MessageCommand = new MessageCommand(this); 

            // Timers. 
            GettingMsgTimer = new DispatcherTimer();

            // Try to create a table for users. 
            try
            {
                DatabasePath path = new DatabasePath(); 
                SqliteDbHelper.Instance.AbsolutePathToDb = path.AbsolutePath; 
                SqliteDbHelper.Instance.CreateUserTable(); 
            }
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show($"Failed to create database:\n{e}"); 
            }
            
            // Set initial number of characters available in the message. 
            this.SetNumberOfAvailableCharsInTextBlock(); 

            // Activate welcome page 
            this.GoToWelcomePage(); 
        }
        #endregion  // Constructor

        #region Methods for going to an other page
        /// <summary>
        /// Allows to go to the welcome page  
        /// </summary>
        public void GoToWelcomePage()
        {
            // Welcome page
            CurrentWindow.Welcome.Visibility = Visibility.Visible; 
            CurrentWindow.Welcome.IsEnabled = true; 

            // Registration page
            CurrentWindow.Registration.Visibility = Visibility.Collapsed; 
            CurrentWindow.Registration.IsEnabled = false; 

            // Login page
            CurrentWindow.Login.Visibility = Visibility.Collapsed; 
            CurrentWindow.Login.IsEnabled = false; 

            // UserPage page
            CurrentWindow.UserPage.Visibility = Visibility.Collapsed; 
            CurrentWindow.UserPage.IsEnabled = false; 
        }

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
            this.ClearRegistrationFields(); 

            // Login page
            CurrentWindow.Login.Visibility = Visibility.Visible; 
            CurrentWindow.Login.IsEnabled = true; 
            this.ClearLoginFields(); 

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
            this.ClearRegistrationFields(); 

            // Login page
            CurrentWindow.Login.Visibility = Visibility.Collapsed; 
            CurrentWindow.Login.IsEnabled = false; 
            this.ClearLoginFields(); 

            // UserPage page
            CurrentWindow.UserPage.Visibility = Visibility.Visible; 
            CurrentWindow.UserPage.IsEnabled = true; 

            // Timer for getting messages from the server. 
            GettingMsgTimer.Tick += GetMessages;
            GettingMsgTimer.Interval = System.TimeSpan.FromSeconds(2);
            GettingMsgTimer.Start();
        }

        /// <summary>
        /// Allows to exit the application 
        /// </summary>
        public void Exit()
        {
            MessageBoxResult result = System.Windows.MessageBox.Show("Are you sure to exit the application?", "Exit the application", MessageBoxButton.YesNo); 
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    this.Client.SendMessage($"User {this.CurrentUser.Name} disconnected"); 
                    this.Client.CloseConnection(); 
                }
                catch (System.Exception e)
                {
                    System.Windows.MessageBox.Show($"Exception:\n{e}", "Exception"); 
                }
                finally
                {
                    this.GettingMsgTimer.Stop(); 
                    this.CurrentUser = null; 
                    Application.Current.Shutdown();
                }
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
                this.CheckUserInDatabaseOnLogin();  // Check if this user already exists in database. 
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
                    this.RegisterUserInDb();    // Insert new user into DB 
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
                try
                {
                    if (SqliteDbHelper.Instance.IsAuthenticated(user))
                    {
                        System.Windows.MessageBox.Show("Successfully submitted!\nNow you can join the Chat.", "Welcome to the Chat!");
                        this.CurrentUser = user; 
                        this.Client = new ChatTcpClient("127.0.0.0", "localhost", 13000, this.CurrentUser.Name); 
                        this.ClearLoginFields(); 
                        this.GoToUserPage(); 
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("No such user in the database.\nRegister first!", "Authentication error");
                        this.ClearLoginFields(); 
                        this.GoToRegisterPage(); 
                    }
                }
                catch (System.Exception e)
                {
                    System.Windows.MessageBox.Show($"Exception while getting user from database:\n{e}", "Exception"); 
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
                try
                {
                    if (SqliteDbHelper.Instance.IsAuthenticated(user))
                    {
                        System.Windows.MessageBox.Show($"User {user.Name} already exists in DB.\nGo to the Login Page.", "Authentication error", MessageBoxButton.OK); 
                        this.ClearRegistrationFields(); 
                        this.GoToLoginPage();
                    }
                    else
                    {
                        SqliteDbHelper.Instance.InsertDataIntoUserTable(user); 
                        if (SqliteDbHelper.Instance.IsAuthenticated(user))
                        {
                            CurrentWindow.MessageReg.Text = "Successfully submitted!"; 
                            this.ClearRegistrationFields(); 
                            this.GoToLoginPage();
                        }
                        else
                        {
                            CurrentWindow.MessageReg.Text = "Unable to insert user into database";
                        }
                    }
                }
                catch (System.Exception e)
                {
                    System.Windows.MessageBox.Show($"Exception while getting user from database:\n{e}", "Database exception"); 
                }
            }
        }
        #endregion  // Submit methods

        #region Clearing field
        /// <summary>
        /// Allows to clear all fields on the Login Page 
        /// </summary>
        private void ClearLoginFields()
        {
            CurrentWindow.UsernameLogin.Text = System.String.Empty; 
            CurrentWindow.PasswordLogin.Password = System.String.Empty; 
            CurrentWindow.MessageLogin.Text = System.String.Empty; 
        }

        /// <summary>
        /// Allows to clear all fields on the Registration Page 
        /// </summary>
        private void ClearRegistrationFields()
        {
            CurrentWindow.UsernameReg.Text = System.String.Empty; 
            CurrentWindow.EmailReg.Text = System.String.Empty; 
            CurrentWindow.PasswordBoxReg.Password = System.String.Empty; 
            CurrentWindow.ConfirmPasswordBoxReg.Password = System.String.Empty; 
            CurrentWindow.MessageReg.Text = System.String.Empty; 
        }
        #endregion  // Clearing field

        #region Communication methods 
        /// <summary>
        /// Sends message via TCP/UDP protocols 
        /// </summary>
        public void SendMessage()
        {
            try
            {
                string msg = $"{this.CurrentUser.Name}: {this.MessagesVM.MessageToSend}"; 
                this.Client.SendMessage(msg, false); 
                this.MessagesVM.MessageToSend = System.String.Empty; 
            }
            catch (System.Exception e)
            {
                System.Windows.MessageBox.Show($"Exception:\n{e}", "Exception"); 
            }
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

        /// <summary>
        /// Method for getting messages from the server
        /// </summary>
        public void GetMessages(object sender, System.EventArgs e)
        {
            string msg = this.Client.GetMessages(); 
            this.MessagesVM.MessagesInChat = $"{msg}\n"; 
        }
        #endregion  // Communication methods 
    }
}