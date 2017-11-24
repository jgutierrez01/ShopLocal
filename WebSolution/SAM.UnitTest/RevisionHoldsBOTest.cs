using SAM.BusinessObjects.Workstatus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SAM.BusinessObjects.Modelo;

namespace SAM.UnitTest
{
    
    
    /// <summary>
    ///This is a test class for RevisionHoldsBOTest and is intended
    ///to contain all RevisionHoldsBOTest Unit Tests
    ///</summary>
    [TestClass()]
    public class RevisionHoldsBOTest
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
        ///A test for SpoolTieneHold
        ///</summary>
        [TestMethod()]
        public void SpoolTieneHoldTest()
        {
            bool expected = false;
            bool expected2 = false;

            using(SamContext ctx = new SamContext())
            {
                expected = RevisionHoldsBO.Instance.SpoolTieneHold(ctx, 6905);
                expected2 = RevisionHoldsBO.Instance.SpoolTieneHold(ctx, 6900);
            }

            Assert.IsTrue(expected);
            Assert.IsFalse(expected2);
        }

        /// <summary>
        ///A test for MaterialSpoolTieneHold
        ///</summary>
        [TestMethod()]
        public void MaterialSpoolTieneHoldTest()
        {
            bool expected = false;
            bool expected2 = false;

            using (SamContext ctx = new SamContext())
            {
                expected = RevisionHoldsBO.Instance.MaterialSpoolTieneHold(ctx, 193932);
                expected2 = RevisionHoldsBO.Instance.MaterialSpoolTieneHold(ctx, 193920);
            }

            Assert.IsTrue(expected);
            Assert.IsFalse(expected2);
        }

        /// <summary>
        ///A test for JuntaWorkstatusTieneHold
        ///</summary>
        [TestMethod()]
        public void JuntaWorkstatusTieneHoldTest()
        {
            bool expected = false;
            bool expected2 = false;

            using (SamContext ctx = new SamContext())
            {
                expected = RevisionHoldsBO.Instance.JuntaWorkstatusTieneHold(ctx, 27);
                expected2 = RevisionHoldsBO.Instance.JuntaWorkstatusTieneHold(ctx, 20);
            }

            Assert.IsTrue(expected);
            Assert.IsFalse(expected2);
        }

        /// <summary>
        ///A test for JuntaSpoolTieneHold
        ///</summary>
        [TestMethod()]
        public void JuntaSpoolTieneHoldTest()
        {
            bool expected = false;
            bool expected2 = false;

            using (SamContext ctx = new SamContext())
            {
                expected = RevisionHoldsBO.Instance.JuntaSpoolTieneHold(ctx, 86380);
                expected2 = RevisionHoldsBO.Instance.JuntaSpoolTieneHold(ctx, 86300);
            }

            Assert.IsTrue(expected);
            Assert.IsFalse(expected2);
        }

        ///// <summary>
        /////A test for AlgunaJuntaSpoolTieneHold
        /////</summary>
        [TestMethod()]
        public void AlgunaJuntaSpoolTieneHoldTest()
        {
            bool expected = false;
            bool expected2 = false;

            using (SamContext ctx = new SamContext())
            {
                expected = RevisionHoldsBO.Instance.AlgunaJuntaSpoolTieneHold(ctx, new[] { 86380, 86300 });
                expected2 = RevisionHoldsBO.Instance.AlgunaJuntaSpoolTieneHold(ctx, new[] { 86300, 86301 });
            }

            Assert.IsTrue(expected);
            Assert.IsFalse(expected2);
        }

        /// <summary>
        ///A test for AlgunSpoolTieneHold
        ///</summary>
        [TestMethod()]
        public void AlgunSpoolTieneHoldTest()
        {
            bool expected = false;
            bool expected2 = false;

            using (SamContext ctx = new SamContext())
            {
                expected = RevisionHoldsBO.Instance.AlgunSpoolTieneHold(ctx, new[] { 6905, 6900 });
                expected2 = RevisionHoldsBO.Instance.AlgunSpoolTieneHold(ctx, new[] { 6901, 6900 });
            }

            Assert.IsTrue(expected);
            Assert.IsFalse(expected2);
        }

        /// <summary>
        ///A test for AlgunMaterialSpoolTieneHold
        ///</summary>
        [TestMethod()]
        public void AlgunMaterialSpoolTieneHoldTest()
        {
            bool expected = false;
            bool expected2 = false;

            using (SamContext ctx = new SamContext())
            {
                expected = RevisionHoldsBO.Instance.AlgunMaterialSpoolTieneHold(ctx, new[] { 193932, 193920 });
                expected2 = RevisionHoldsBO.Instance.AlgunMaterialSpoolTieneHold(ctx, new[] { 193856, 193920 });
            }

            Assert.IsTrue(expected);
            Assert.IsFalse(expected2);
        }

        /// <summary>
        ///A test for AlgunaJuntaWorkstatusTieneHold
        ///</summary>
        [TestMethod()]
        public void AlgunaJuntaWorkstatusTieneHoldTest()
        {
            bool expected = false;
            bool expected2 = false;

            using (SamContext ctx = new SamContext())
            {
                expected = RevisionHoldsBO.Instance.AlgunaJuntaWorkstatusTieneHold(ctx, new[]{27,20});
                expected2 = RevisionHoldsBO.Instance.AlgunaJuntaWorkstatusTieneHold(ctx, new[]{20,29});
            }

            Assert.IsTrue(expected);
            Assert.IsFalse(expected2);
        }

    }
}
