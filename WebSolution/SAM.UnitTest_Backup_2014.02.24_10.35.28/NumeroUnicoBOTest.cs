using SAM.BusinessObjects.Materiales;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Web;

namespace SAM.UnitTest
{
    
    
    /// <summary>
    ///This is a test class for NumeroUnicoBOTest and is intended
    ///to contain all NumeroUnicoBOTest Unit Tests
    ///</summary>
    [TestClass()]
    public class NumeroUnicoBOTest
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
        ///A test for ObtenerNumerosUnicosPorProyecto
        ///</summary>
        [TestMethod()]
        public void ObtenerNumerosUnicosPorProyectoTest()
        {
            NumeroUnicoBO.Instance.ObtenerNumerosUnicosPorProyecto(46, null, null, null, null);
            //NumeroUnicoBO.Instance.ObtenerNumerosUnicosPorProyectoOld(46, null, null, null, null);
            //NumeroUnicoBO.Instance.ObtenerNumerosUnicosPorProyectoSimple(46, null, null, null, null);

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            //sw.Start();
            //NumeroUnicoBO.Instance.ObtenerNumerosUnicosPorProyectoOld(46, null, null, null, null);
            //sw.Stop();

            //System.Diagnostics.Trace.WriteLine("Tiempo viejo: " + sw.ElapsedMilliseconds);

            //sw.Restart();
            //NumeroUnicoBO.Instance.ObtenerNumerosUnicosPorProyecto(46, null, null, null, null);
            //sw.Stop();

            //System.Diagnostics.Trace.WriteLine("Tiempo nuevo: " + sw.ElapsedMilliseconds);


            //sw.Restart();
            //NumeroUnicoBO.Instance.ObtenerPorProyecto(46);
            //sw.Stop();

            //System.Diagnostics.Trace.WriteLine("Solo nus: " + sw.ElapsedMilliseconds);

            //sw.Restart();
            //NumeroUnicoBO.Instance.ObtenerNumerosUnicosPorProyectoSimple(46, null, null, null, null);
            //sw.Stop();

            //System.Diagnostics.Trace.WriteLine("A través de SP: " + sw.ElapsedMilliseconds);

            //sw.Restart();
            //NumeroUnicoBO.Instance.ObtenerNumerosUnicosPorProyectoSimple(57, null, null, null, null);
            //sw.Stop();

            //System.Diagnostics.Trace.WriteLine("A través de SP: " + sw.ElapsedMilliseconds);

            sw.Start();
            NumeroUnicoBO.Instance.ObtenerNumerosUnicosPorProyecto(69, null, null, null, null);
            sw.Stop();

            System.Diagnostics.Trace.WriteLine("A través de SP: " + sw.ElapsedMilliseconds);
            Assert.IsTrue(true);
        }
    }
}
