using Mimo.Framework.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace SAM.UnitTest
{
    
    
    /// <summary>
    ///This is a test class for RijndaelImplTest and is intended
    ///to contain all RijndaelImplTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RijndaelImplTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for CreateNewRandomSymmetricKey
        ///</summary>
        [TestMethod()]
        public void CreateNewRandomSymmetricKeyTest()
        {
            int keySize = 256; // TODO: Initialize to an appropriate value
            RijndaelParameters actual;
            actual = RijndaelImpl.CreateNewRandomSymmetricKey(keySize);

            string iv = Convert.ToBase64String(actual.IV);
            string key = Convert.ToBase64String(actual.Key);

            string crypted = Crypter.Encrypt("hola");
            string decrypted = Crypter.Decrypt(crypted);

            Assert.AreEqual(decrypted, "hola");
        }
    }
}
