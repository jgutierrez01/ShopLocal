using SAM.BusinessLogic.Cruce;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SAM.BusinessObjects.Modelo;
using SAM.Entities;
using System.Collections.Generic;
using Mimo.Framework.Exceptions;

namespace SAM.UnitTest
{
    
    
    /// <summary>
    ///This is a test class for CruceSpoolTest and is intended
    ///to contain all CruceSpoolTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CruceSpoolTest
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
        //[ExpectedException(typeof(BaseValidationException))]
        public void ProcesaTest()
        {
            using (SamContext ctx = new SamContext())
            {
                int proyectoID = 57;
                int[] spoolIds = new int [] { 11222,11223,11224,11225,11226,11227,11228,11229,11230,11231,11232,11233,11234 };

                CruceSpool cruce = new CruceSpool(ctx, proyectoID, spoolIds, null);

                List<NumeroUnico> congelados = null;
                List<Spool> actual;

                actual = cruce.Procesa(out congelados);


                actual = cruce.Procesa(out congelados);

                Assert.IsNotNull(actual);
            }
        }
    }
}
