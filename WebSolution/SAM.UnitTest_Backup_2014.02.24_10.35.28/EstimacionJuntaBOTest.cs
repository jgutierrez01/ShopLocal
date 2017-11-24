using SAM.BusinessObjects.Produccion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SAM.Entities.Grid;
using System.Collections.Generic;

namespace SAM.UnitTest
{
    
    
    /// <summary>
    ///This is a test class for EstimacionJuntaBOTest and is intended
    ///to contain all EstimacionJuntaBOTest Unit Tests
    ///</summary>
    [TestClass()]
    public class EstimacionJuntaBOTest
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
        ///A test for ObtenerEstimacionJuntaPorProyectoID
        ///</summary>
        [TestMethod()]
        public void ObtenerEstimacionJuntaPorProyectoIDTest()
        {
            EstimacionJuntaBO target = new EstimacionJuntaBO(); // TODO: Initialize to an appropriate value
            int proyectoID = 10; // TODO: Initialize to an appropriate value
            List<GrdEstimacionJuntaCompleta> expected = null; // TODO: Initialize to an appropriate value
            List<GrdEstimacionJuntaCompleta> actual;
            actual = target.ObtenerEstimacionJuntaPorProyectoID(proyectoID);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
