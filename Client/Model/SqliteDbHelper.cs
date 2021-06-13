using System.Collections.Generic; 
using Microsoft.Data.Sqlite;

namespace Chat.Client.Model
{
    /// <summary>
    /// Class that allows to use SQLite database 
    /// </summary>
    public class SqliteDbHelper
    {
        #region Properties
        /// <summary>
        /// Request for creating the User table 
        /// </summary>
        private string CreateUserTableRequest = @"CREATE TABLE IF NOT EXISTS Users(
            Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, 
            Name TEXT, 
            Email TEXT, 
            Password TEXT)"; 
        
        /// <summary>
        /// Absolute path to the database 
        /// </summary>
        private string AbsolutePathToDb;    // Set absolute path to the DB that locates in the Model/LocalDB folder. 
        #endregion  // Properties

        #region Members
        /// <summary>
        /// Instance of SqliteDbHelper (to use Singleton pattern to avoid memory leak)
        /// </summary>
        public static SqliteDbHelper Instance { get; private set; } 
        #endregion  // Members

        #region Constructors
        /// <summary>
        /// Static constructor of SqliteDbHelper (to use Singleton pattern to avoid memory leak)
        /// </summary>
        static SqliteDbHelper()
        {
            Instance = new SqliteDbHelper(); 
        }

        /// <summary>
        /// Private constructor of SqliteDbHelper (to use Singleton pattern to avoid memory leak)
        /// </summary>
        private SqliteDbHelper() { }
        #endregion  // Constructors
        
        #region Methods for User table
        /// <summary>
        /// Allows to create User table 
        /// </summary>
        public void CreateUserTable()
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder(); 
            connectionStringBuilder.DataSource = this.AbsolutePathToDb;
            connectionStringBuilder.Mode = SqliteOpenMode.ReadWriteCreate;

            using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
            {
                try
                {
                    connection.Open(); 
                    if (System.IO.File.Exists(connectionStringBuilder.DataSource))
                    {
                        var tableCmd = connection.CreateCommand(); 
                        tableCmd.CommandText = CreateUserTableRequest; 
                        tableCmd.ExecuteNonQuery(); 
                    }
                }
                catch (System.Exception e)
                {
                    System.Windows.MessageBox.Show($"Failed to create database:\n{e}"); 
                }
            }
        }

        /// <summary>
        /// Allows to insert data about user into database 
        /// </summary>
        /// <param name="user">Instance of UserModel that consists all fields of user</param>
        public void InsertDataIntoUserTable(UserModel user)
        {
            string insertRequest = $@"INSERT INTO Users (Name, Email, Password)
                VALUES ('{user.Name}', '{user.Email}', '{user.Password}')"; 
            
            var connectionStringBuilder = new SqliteConnectionStringBuilder();
            connectionStringBuilder.DataSource = this.AbsolutePathToDb;

            using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
            {
                try
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        var insertCmd = connection.CreateCommand(); 
                        insertCmd.CommandText = insertRequest;      // SQL command. 
                        insertCmd.ExecuteNonQuery();                // Execute SQL command. 
                        transaction.Commit();                       // Commit changes. 
                    }
                }
                catch (System.Exception e)
                {
                    System.Windows.MessageBox.Show($"Failed to insert data into database:\n{e}");
                }
            }
        }

        /// <summary>
        /// Allows to make request to the database and get if user exists in database 
        /// </summary>
        /// <param name="user">Instance of UserModel that consists all fields of user</param>
        /// <returns></returns>
        public bool IsAuthenticated(UserModel user)
        {
            string request = $@"SELECT Name, Password FROM Users";

            var connectionStringBuilder = new SqliteConnectionStringBuilder();
            connectionStringBuilder.DataSource = this.AbsolutePathToDb;
            
            using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
            {
                try
                {
                    connection.Open();
                    var selectCmd = connection.CreateCommand();
                    selectCmd.CommandText = request;
                    using (var reader = selectCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader.GetString(0) == user.Name && reader.GetString(1) == user.Password)
                            {
                                return true;
                            }
                        }
                    }
                }
                catch (System.Exception e)
                {
                    System.Windows.MessageBox.Show($"Failed to get data from database:\n{e}");
                }
            }

            return false; 
        }
        #endregion  // Methods for User table
    }
}