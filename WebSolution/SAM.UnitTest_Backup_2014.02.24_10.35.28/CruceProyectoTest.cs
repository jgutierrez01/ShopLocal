using SAM.BusinessLogic.Cruce;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SAM.Entities;
using SAM.Entities.Personalizadas;
using System.Collections.Generic;
using SAM.Entities.Reportes;

namespace SAM.UnitTest
{
    
    
    /// <summary>
    ///This is a test class for CruceProyectoTest and is intended
    ///to contain all CruceProyectoTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CruceProyectoTest
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
        ///A test for Procesa
        ///</summary>
        [TestMethod()]
        public void ProcesaTest()
        {
            int proyectoID = 57; // TODO: Initialize to an appropriate value
            CruceProyecto target = new CruceProyecto(proyectoID); // TODO: Initialize to an appropriate value
            List<SpoolCruce> spoolsFabricables;
            List<FaltanteCruce> faltantes;
            List<CondensadoItemCode> condensado;
            List<string> isometricosCompletos = new List<string>();

            spoolsFabricables = target.Procesa(out faltantes, out condensado, false, false);

            target = new CruceProyecto(proyectoID);
            spoolsFabricables = target.Procesa(out faltantes, out condensado, false, false);
            
            Assert.IsNotNull(spoolsFabricables);
            Assert.IsNotNull(faltantes);
            Assert.IsNotNull(condensado);
        }
    }
}
