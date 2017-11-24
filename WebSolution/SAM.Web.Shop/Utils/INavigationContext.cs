using SAM.Entities;
using SAM.Entities.Cache;
using SAM.Web.Shop.Models;
using System.Collections.Generic;


namespace SAM.Web.Shop.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public interface INavigationContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="yardId"></param>
        void SetYard(int yardId);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        void SetProject(int projectId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        void SetProjectEdit(int projectId);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="controlNumber"></param>
        void SetControlNumber(OrdenTrabajoSpool controlNumber);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="spools"></param>
        void SetNumbersControl(string spools);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numeroControlCuadranteSQ"></param>
        void SetNumbersControlCuadranteSQ(string spools);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numeroControlCuadranteSQEditado"></param>
        void SetNumbersControlCuadranteSQEditar(string spools);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sq"></param>
        void SetSQ(string sq);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        PatioCache GetCurrentYard();


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ProyectoCache GetCurrentProject();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ProyectoCache GetCurrentProjectSQ();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ProyectoCache GetCurrentProjectSQEditar();


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ControlNumberCache GetCurrentControlNumber();


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string GetCurrentNumbersControl();

        string GetCurrentNCSQ();

        string GetCurrentNCSQEditar();

        string GetSQ();


    }
}
