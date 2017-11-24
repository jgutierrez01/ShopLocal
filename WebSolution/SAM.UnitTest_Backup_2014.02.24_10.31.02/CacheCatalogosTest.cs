using SAM.BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using SAM.Entities.Cache;
using SAM.BusinessObjects.Utilerias;

namespace SAM.UnitTest
{
    
    
    /// <summary>
    ///This is a test class for CacheCatalogosTest and is intended
    ///to contain all CacheCatalogosTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CacheCatalogosTest
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
        ///A test for Instance
        ///</summary>
        [TestMethod()]
        public void InstanceTest()
        {
            CacheCatalogos actual;
            actual = CacheCatalogos.Instance;
        }

        /// <summary>
        ///A test for ObtenerAceros
        ///</summary>
        [TestMethod()]
        public void ObtenerAcerosTest()
        {
            List<AceroCache> lstAceros = CacheCatalogos.Instance.ObtenerAceros();
            Assert.IsNotNull(lstAceros);
        }

        /// <summary>
        ///A test for ObtenerColores
        ///</summary>
        [TestMethod()]
        public void ObtenerColoresTest()
        {
            List<ColorCache> lstColores = CacheCatalogos.Instance.ObtenerColores();
            Assert.IsNotNull(lstColores);
        }

        /// <summary>
        ///A test for ObtenerDefectos
        ///</summary>
        [TestMethod()]
        public void ObtenerDefectosTest()
        {
            List<DefectoCache> lstDefectos = CacheCatalogos.Instance.ObtenerDefectos();
            Assert.IsNotNull(lstDefectos);
        }

        /// <summary>
        ///A test for ObtenerFabAreas
        ///</summary>
        [TestMethod()]
        public void ObtenerFabAreasTest()
        {
            List<FabAreaCache> lstFabAreas = CacheCatalogos.Instance.ObtenerFabAreas();
            Assert.IsNotNull(lstFabAreas);
        }

        /// <summary>
        ///A test for ObtenerFabricantes
        ///</summary>
        [TestMethod()]
        public void ObtenerFabricantesTest()
        {
            List<FabricanteCache> lstFabricante = CacheCatalogos.Instance.ObtenerFabricantes();
            Assert.IsNotNull(lstFabricante);
        }

        /// <summary>
        ///A test for ObtenerFamiliasAcero
        ///</summary>
        [TestMethod()]
        public void ObtenerFamiliasAceroTest()
        {
            List<FamAceroCache> lstFamiliasAcero = CacheCatalogos.Instance.ObtenerFamiliasAcero();
            Assert.IsNotNull(lstFamiliasAcero);
        }

        /// <summary>
        ///A test for ObtenerFamiliasMaterial
        ///</summary>
        [TestMethod()]
        public void ObtenerFamiliasMaterialTest()
        {
            List<FamMaterialCache> lstFamiliasMateriales = CacheCatalogos.Instance.ObtenerFamiliasMaterial();
            Assert.IsNotNull(lstFamiliasMateriales);
        }

        /// <summary>
        ///A test for ObtenerPatios
        ///</summary>
        [TestMethod()]
        public void ObtenerPatiosTest()
        {
            List<PatioCache> lstPatios = CacheCatalogos.Instance.ObtenerPatios();
            Assert.IsNotNull(lstPatios);
        }

        /// <summary>
        ///A test for ObtenerPerfiles
        ///</summary>
        [TestMethod()]
        public void ObtenerPerfilesTest()
        {
            List<PerfilCache> lstPerfiles = CacheCatalogos.Instance.ObtenerPerfiles();
            Assert.IsNotNull(lstPerfiles);
        }

        /// <summary>
        ///A test for ObtenerPermisos
        ///</summary>
        [TestMethod()]
        public void ObtenerPermisosTest()
        {
          List<PermisoCache>  lstPermisos = CacheCatalogos.Instance.ObtenerPermisos();
          Assert.IsNotNull(lstPermisos);
        }

        /// <summary>
        ///A test for ObtenerProcesosRaiz
        ///</summary>
        [TestMethod()]
        public void ObtenerProcesosRaizTest()
        {
            List<ProcesoRaizCache> lstProcesosRaiz = CacheCatalogos.Instance.ObtenerProcesosRaiz();
            Assert.IsNotNull(lstProcesosRaiz);
        }

        /// <summary>
        ///A test for ObtenerProcesosRelleno
        ///</summary>
        [TestMethod()]
        public void ObtenerProcesosRellenoTest()
        {
            List<ProcesoRellenoCache> lstProcesosRelleno = CacheCatalogos.Instance.ObtenerProcesosRelleno();
            Assert.IsNotNull(lstProcesosRelleno);
        }

        /// <summary>
        ///A test for ObtenerProveedores
        ///</summary>
        [TestMethod()]
        public void ObtenerProveedoresTest()
        {
            List<ProveedorCache> lstProveedores = CacheCatalogos.Instance.ObtenerProveedores();
            Assert.IsNotNull(lstProveedores);
        }

        /// <summary>
        ///A test for ObtenerProyectos
        ///</summary>
        [TestMethod()]
        public void ObtenerProyectosTest()
        {
            List<ProyectoCache> lstProyectos = CacheCatalogos.Instance.ObtenerProyectos();
            Assert.IsNotNull(lstProyectos);
        }

        /// <summary>
        ///A test for ObtenerTiposCorte
        ///</summary>
        [TestMethod()]
        public void ObtenerTiposCorteTest()
        {
            List<TipoCorteCache> lstTiposCorte = CacheCatalogos.Instance.ObtenerTiposCorte();
            Assert.IsNotNull(lstTiposCorte);
        }

        /// <summary>
        ///A test for ObtenerTiposJunta
        ///</summary>
        [TestMethod()]
        public void ObtenerTiposJuntaTest()
        {
            List<TipoJuntaCache> lstTipoJunta = CacheCatalogos.Instance.ObtenerTiposJunta();
            Assert.IsNotNull(lstTipoJunta);
        }

        /// <summary>
        ///A test for ObtenerTransportistas
        ///</summary>
        [TestMethod()]
        public void ObtenerTransportistasTest()
        {
            List<TransportistaCache> lstTransportistas = CacheCatalogos.Instance.ObtenerTransportistas();
            Assert.IsNotNull(lstTransportistas);
        }

        /// <summary>
        ///A test for ObtenerWps
        ///</summary>
        [TestMethod()]
        public void ObtenerWpsTest()
        {
            List<WpsCache> lstWpsCache = CacheCatalogos.Instance.ObtenerWps();
            Assert.IsNotNull(lstWpsCache);
        }
    }
}
