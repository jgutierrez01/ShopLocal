using SAM.BusinessObjects.Catalogos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using SAM.BusinessObjects.Proyectos;

namespace SAM.UnitTest
{
    
    
    /// <summary>
    ///This is a test class for TransportistaBOTest and is intended
    ///to contain all TransportistaBOTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TransportistaBOTest
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
        ///A test for ObtenerPorProyecto
        ///</summary>
        [TestMethod()]
        public void ObtenerPorProyectoTest()
        {
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void ObtenerPorUsuarioTest()
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();
            var x = ProyectoBO.Instance.ObtenerPorUsuario(Guid.NewGuid());
            sw.Stop();

            long e1 = sw.ElapsedMilliseconds;

            sw.Restart();
            var y = ProyectoBO.Instance.ObtenerPorUsuario(Guid.NewGuid());
            sw.Stop();

            long e2 = sw.ElapsedMilliseconds;

            sw.Restart();
            var z = ProyectoBO.Instance.ObtenerPorUsuario(Guid.NewGuid());
            sw.Stop();

            long e3 = sw.ElapsedMilliseconds;


            sw.Restart();
            for (int i = 0; i < 100; i++)
            {
                ProyectoBO.Instance.ObtenerPorUsuario(Guid.NewGuid());
            }
            sw.Stop();
            long nonCompilead = sw.ElapsedMilliseconds;


            //sw.Restart();
            //for (int i = 0; i < 100; i++)
            //{
            //    ProyectoBO.Instance.ObtenerPorUsuarioCompiled(Guid.NewGuid());
            //}
            //sw.Stop();
            //long compiled = sw.ElapsedMilliseconds;


            Assert.IsNotNull(x);
        }
    }
}
