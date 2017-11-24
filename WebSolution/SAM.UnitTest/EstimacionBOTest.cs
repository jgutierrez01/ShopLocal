using SAM.BusinessObjects.Produccion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SAM.Entities;
using System.Diagnostics;

namespace SAM.UnitTest
{
    
    
    /// <summary>
    ///This is a test class for EstimacionBOTest and is intended
    ///to contain all EstimacionBOTest Unit Tests
    ///</summary>
    [TestClass()]
    public class EstimacionBOTest
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
        ///A test for ObtenerConProyecto
        ///</summary>
        [TestMethod()]
        public void ObtenerConProyectoTest()
        {
            Stopwatch sw = new Stopwatch();

            Estimacion est = EstimacionBO.Instance.ObtenerConProyectoYDetalle(1);

            sw.Start();
            Estimacion est2 = EstimacionBO.Instance.ObtenerConProyectoYDetalle(1);
            sw.Stop();

            Trace.WriteLine("Query complejo: " + sw.ElapsedMilliseconds);

            est = EstimacionBO.Instance.ObtenerConProyectoYDetalle(1);

            sw.Restart();
            est2 = EstimacionBO.Instance.ObtenerConProyectoYDetalle(1);
            sw.Stop();

            Trace.WriteLine("Varios queries: " + sw.ElapsedMilliseconds);
        }

        /// <summary>
        ///A test for ObtenerEstimacionJuntaPorEstimacionID
        ///</summary>
        [TestMethod()]
        public void ObtenerEstimacionJuntaPorEstimacionIDTest()
        {
            var est = EstimacionBO.Instance.ObtenerEstimacionJuntaPorEstimacionID(1);
            Assert.IsNotNull(est);
        }
    }
}
