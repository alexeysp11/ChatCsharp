using System.Windows; 
using System.Windows.Input; 
using Chat.Client.Commands;
using Chat.Client.View;
using Chat.Client.Exceptions;
using Chat.Client.Model; 

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
        private MainWindow CurrentWindow { get; set; } = null; 
        #endregion  // Members

        #region Commands
        /// <summary>
        /// Allows to move to an other window
        /// </summary>
        public ICommand GoToAnotherPageCommand { get; private set; }
        /// <summary>
        /// Allows to authenticate users
        /// </summary>
        public ICommand AuthCommand { get; private set; }
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

            // Assign window
            CurrentWindow = mainWindow; 

            // Create DB 
            SqliteDbHelper.Instance.CreateUserTable(); 
        }
        #endregion  // Constructor

        #region Going to an other page
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
        #endregion  // Going to an other page

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
                    CurrentWindow.MessageReg.Text = "This user is already exists in DB"; 
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
    }
}