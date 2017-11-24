using SAM.BusinessObjects.Workstatus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using SAM.Entities.Grid;
using System.Collections.Generic;

namespace SAM.UnitTest
{
    
    
    /// <summary>
    ///This is a test class for ReportePndBOTest and is intended
    ///to contain all ReportePndBOTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ReportePndBOTest
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
        ///A test for ObtenerJuntas
        ///</summary>
        [TestMethod()]
        public void ObtenerJuntasTest()
        {

            Stopwatch sw = new Stopwatch();

            List<GrdRequisiciones> est = ReportePndBO.Instance.ObtenerJuntas(35,-1,1);
            List<GrdRequisiciones> est2 = ReportePndBO.Instance.ObtenerJuntasUniQuery(35, -1, 1);

            sw.Start();
            est = ReportePndBO.Instance.ObtenerJuntasUniQuery(35, -1, 1);
            sw.Stop();

            Trace.WriteLine("Un Query: " + sw.ElapsedMilliseconds);

            sw.Restart();
            est2 = ReportePndBO.Instance.ObtenerJuntas(35, -1, 1);
            sw.Stop();

            Trace.WriteLine("Varios queries: " + sw.ElapsedMilliseconds);
        }
    }
}
