using SAM.BusinessObjects.Ingenieria;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SAM.Entities.Personalizadas;
using SAM.Entities;

namespace SAM.UnitTest
{
    /// <summary>
    ///This is a test class for SpoolBOTest and is intended
    ///to contain all SpoolBOTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SpoolBOTest
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
        ///A test for ObtenerDetalle
        ///</summary>
        [TestMethod()]
        public void ObtenerDetalleTest()
        {
            Spool spool = SpoolBO.Instance.ObtenerDetalle(2095);
            Assert.IsNotNull(spool);
        }


        /// <summary>
        ///A test for ObtenerIngPorProyecto
        ///</summary>
        [TestMethod()]
        public void ObtenerIngPorProyectoTest()
        {
            SpoolBO.Instance.ObtenerIngPorProyecto(44);

            //SpoolBO.Instance.ObtenerIngPorProyecto2(44);

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            sw.Start();
            SpoolBO.Instance.ObtenerIngPorProyecto(44);
            sw.Stop();
            System.Diagnostics.Trace.WriteLine("Sin tracking : " + sw.ElapsedMilliseconds);

            //sw.Restart();
            //SpoolBO.Instance.ObtenerIngPorProyecto2(44);
            //sw.Stop();
            //System.Diagnostics.Trace.WriteLine("Con tracking : " + sw.ElapsedMilliseconds);

            Assert.IsFalse(false);
        }
    }
}
