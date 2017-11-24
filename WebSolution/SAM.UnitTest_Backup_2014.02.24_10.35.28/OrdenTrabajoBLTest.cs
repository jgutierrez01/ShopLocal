using SAM.BusinessLogic.Produccion;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SAM.Entities;
using System.Linq;
using System.Transactions;
using Mimo.Framework.Exceptions;

namespace SAM.UnitTest
{
    
    
    /// <summary>
    ///This is a test class for OrdenTrabajoBLTest and is intended
    ///to contain all OrdenTrabajoBLTest Unit Tests
    ///</summary>
    [TestClass()]
    public class OrdenTrabajoBLTest
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
        ///A test for GeneraNueva
        ///</summary>
        [TestMethod()]
        public void GeneraNuevaTest()
        {
            int proyectoID = 57;
            string csvSpools = "10420,10421,10423,10424,10425,10427,10428,10429,10430,10431,10433,10434,10435,10436,10437,10438,10439,10440,10441,10442,10444,10445,10446,10447,10448,10449,10450,10451,10452,10453,10454,10455,10456,10457,10458,10459,10460,10461,10462,10463,10464,10465,10466,10467,10468,10469,10471,10472,10473,10474,10475,10476,10477,10478,10479,10480,10482,10483,10485,10486,10487,10488,10489,10490,10491,10492,10494,10495,10497,10498,10499,10502,10505,10506,10507,10508,10509,10510,10511,10512,10513,10514,10515,10517,10518,10519";
            int[] spoolIds = (from s in csvSpools.Split(',') select int.Parse(s)).ToArray();
            int tallerID = 10;
            int numeroOdt = 2;
            DateTime fecha = DateTime.Now;
            Guid userID = new Guid("d6a113b4-464e-496f-b15d-4456cb0ae55b");
            OrdenTrabajo actual = null;

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    actual = OrdenTrabajoBL.Instance.GeneraNueva(proyectoID, spoolIds, tallerID, numeroOdt, fecha, userID, false, string.Empty, false);
                }
                catch (BaseValidationException)
                {
                }
            }

            using (TransactionScope scope = new TransactionScope())
            {
                try
                {
                    actual = OrdenTrabajoBL.Instance.GeneraNueva(proyectoID, spoolIds, tallerID, numeroOdt, fecha, userID, false, string.Empty, false);
                }
                catch (BaseValidationException)
                {
                }
            }

            Assert.IsNotNull(actual);
        }
    }
}
