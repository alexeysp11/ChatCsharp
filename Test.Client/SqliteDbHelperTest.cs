using NUnit.Framework;
using Chat.Client.Database; 

namespace Test.Client
{
    public class SqliteDbHelperTest
    {
        [SetUp]
        public void Setup()
        {
            SqliteDbHelper.Instance.AbsolutePathToDb; 
        }

        [Test]
        public void InitializeUserModel_GetInstance_InstanceIsNotNull()
        {
            Assert.IsNotNull(SqliteDbHelper.Instance); 
        }

        [Test]
        public void CreateUserTable_JustCallTheMethod_TableIsCreated()
        {
            // Act
            SqliteDbHelper.Instance.CreateUserTable(); 
            bool isCreated = System.IO.File.Exists(SqliteDbHelper.Instance.AbsolutePathToDb); 

            // Assert 
            Assert.IsTrue(isCreated);
        }
    }
}