using NUnit.Framework;
using Chat.Client.Database; 
using Chat.Client.Xml; 

namespace Test.Client
{
    public class SqliteDbHelperTest
    {
        #region Members
        UserModel TestUser = null; 
        #endregion  // Members

        #region Properties
        /// <summary>
        /// Name of a user
        /// </summary>
        string Name = "User"; 
        /// <summary>
        /// Email of a user
        /// </summary>
        string Email = "fake_email@email.com"; 
        /// <summary>
        /// Password of a user
        /// </summary>
        string Password = "some_password"; 
        /// <summary>
        /// Absolute path to the testing database 
        /// </summary>
        string TestAbsolutePathToDb; 
        #endregion  // Properties
        
        [SetUp]
        public void Setup()
        {
            DatabasePath pathObj = XmlHelper.FromXmlFile<DatabasePath>("../../../TestLocalDB/DatabasePath.xml");
            this.TestAbsolutePathToDb = pathObj.AbsolutePath; 
            SqliteDbHelper.Instance.AbsolutePathToDb = this.TestAbsolutePathToDb; 
            TestUser = new UserModel(Name, Email, Password); 

            if (System.IO.File.Exists(this.TestAbsolutePathToDb))
            {
                System.IO.File.Delete(this.TestAbsolutePathToDb); 
            }
        }

        [Test]
        public void InitializeUserModel_GetInstance_InstanceIsNotNull()
        {
            Assert.IsNotNull(SqliteDbHelper.Instance); 
        }

        [Test]
        public void InitializeUserModel_GetAbsolutePathToDb_AbsolutePathToDbIsSame()
        {
            Assert.That(TestAbsolutePathToDb, Is.EqualTo(SqliteDbHelper.Instance.AbsolutePathToDb)); 
        }

        [Test]
        public void CreateUserTable_CreateTable_TableIsCreated()
        {
            // Act
            SqliteDbHelper.Instance.CreateUserTable(); 
            bool isCreated = System.IO.File.Exists(SqliteDbHelper.Instance.AbsolutePathToDb); 

            // Assert 
            Assert.IsTrue(isCreated);
        }

        [Test]
        public void InsertDataIntoUserTable_InsertTestUser_DataInserted()
        {
            // Act
            SqliteDbHelper.Instance.InsertDataIntoUserTable(TestUser); 
            bool isAuthenticated = SqliteDbHelper.Instance.IsAuthenticated(TestUser); 

            // Assert 
            Assert.IsTrue(isAuthenticated);
        }
    }
}