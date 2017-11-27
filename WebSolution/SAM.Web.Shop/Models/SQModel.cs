using Resources.Models;
using SAM.Entities.Personalizadas.Shop;
using SAM.Web.Shop.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SAM.Web.Shop.Models
{
    public class SQModel
    {
        //Modelo General
        public List<LayoutGridSQ> ListaElementos { get; set; }

        public List<LayoutGridSQ> ListaElementosPorSQ { get; set; }
        public List<LayoutGridSQ> NumerosControlAdd { get; set; }
        public List<LayoutGridSQ> NumerosControlEdit { get; set; }
        public int CuadranteID { get; set; }

        public string SeleccionAgregarEditar { get; set; }

        //Modelo General Agregar
        [Required(ErrorMessageResourceName = "Project_Required_ErrorMessage", ErrorMessageResourceType = typeof(SearchStrings))]
        public int ProjectIdADD { get; set; }
        
        public string SearchTypeADD { get; set; }

        //Modelo General Editar
        [Required(ErrorMessageResourceName = "Project_Required_ErrorMessage", ErrorMessageResourceType = typeof(SearchStrings))]
        public int ProjectIdEditar { get; set; }

        
        public string SearchTypeEdit { get; set; }

        
        public string SQ { get; set; }

        //Modelo para Agregar con opcion de cuadrante       
        public int QuadrantIdCADD { get; set; }

        //Modelo para Agregar con opcion de numero de control        
        public int QuadrantIdNCADD { get; set; }

        [Display(Name = "Wo_DisplayName", ResourceType = typeof(SearchStrings))]        
        public int? WorkOrderNumberADD { get; set; }
        
        [Display(Name = "Wo_DisplayName", ResourceType = typeof(SearchStrings))]        
        public int? ControlNumberADD { get; set; }

        //Modelo para Editar con opcion de cuadrante        
        public int QuadrantIdCEdit { get; set; }

        //Modelo para Editar con opcion de Numero de control
        public int QuadrantIdNCEdit { get; set; }

        [Display(Name = "Wo_DisplayName", ResourceType = typeof(SearchStrings))]        
        public int? ControlNumberEDIT { get; set; }

        [Display(Name = "Wo_DisplayName", ResourceType = typeof(SearchStrings))]        
        public int? WorkOrderNumberEdit { get; set; }
        public bool? TieneDatosGridEdit { get; set; }
        public bool? TieneDatosGridAdd { get; set; }

        public SQModel(string seleccionAgregarEditar, string quadrantIdCADD, string quadrantIdNCADD, string quadrantIdCEdit, string quadrantIdNCEdit, string projectIdADD, string projectIdEdit, string searchTypeADD, string searchTypeEdit, string sQ, string workOrderNumberADD, string workOrderNumberEdit, string controlNumberADD, string controlNumberEDIT, string CuadranteID)
        {
            int a;
            string cadenaProyecto = projectIdEdit==null? "0": projectIdEdit.Split(',').Length > 1 ? projectIdEdit.Split(',')[0] : projectIdEdit.Split(',')[0];
            this.SeleccionAgregarEditar = seleccionAgregarEditar;
            this.QuadrantIdCADD = quadrantIdCADD == "" || quadrantIdCADD==null ? 0 : int.Parse(quadrantIdCADD);
            this.QuadrantIdCEdit = quadrantIdCEdit == "" || quadrantIdCEdit==null ? 0 : int.Parse(quadrantIdCEdit);
            this.QuadrantIdNCEdit = quadrantIdNCEdit == ""|| quadrantIdNCEdit==null ? 0 : int.Parse(quadrantIdNCEdit);
            this.QuadrantIdNCADD = quadrantIdNCADD == ""|| quadrantIdNCADD == null ? 0 : int.Parse(quadrantIdNCADD);
            if(projectIdADD != null)
            {
                string[] ArrayProyectoAdd = projectIdADD.Split(',');
                int[] A = new int[ArrayProyectoAdd.Length];
                for (int i = 0; i < ArrayProyectoAdd.Length; i++)
                {
                    A[i] = int.Parse(ArrayProyectoAdd[i]);
                }

                int[] B = A.Distinct().ToArray();
                this.ProjectIdADD = B[0];
            }else
            {
                this.ProjectIdADD = 0;
            }
            if(projectIdEdit != null)
            {
                string[] ArrayProyectoEdit = projectIdEdit.Split(',');
                int[] C = new int[ArrayProyectoEdit.Length];
                for (int i = 0; i < ArrayProyectoEdit.Length; i++)
                {
                    C[i] = int.Parse(ArrayProyectoEdit[i]);
                }

                int[] D = C.Distinct().ToArray();
                this.ProjectIdEditar = D[0];
            }else
            {
                this.ProjectIdEditar = 0;
            }
           
            //this.ProjectIdADD = (projectIdADD == "" || projectIdADD == null) ? 0 : int.Parse(projectIdADD);
            //this.ProjectIdEditar = (projectIdEdit == ""|| projectIdEdit==null) ? 0 : (int.TryParse(cadenaProyecto, out a) ? int.Parse(cadenaProyecto) : 0) ;
            this.SearchTypeADD = searchTypeADD;
            this.SearchTypeEdit = searchTypeEdit;
            this.SQ = sQ;
            this.WorkOrderNumberADD = workOrderNumberADD == ""|| workOrderNumberADD==null ? 0 : (int.TryParse(workOrderNumberADD,out a )? int.Parse(workOrderNumberADD):0);
            this.WorkOrderNumberEdit = workOrderNumberEdit == ""|| workOrderNumberEdit==null ? 0 : (int.TryParse(workOrderNumberEdit, out a) ? int.Parse(workOrderNumberEdit) : 0)  ;
            this.ControlNumberADD = controlNumberADD == ""|| controlNumberADD==null ? 0 : (int.TryParse(controlNumberADD, out a) ? int.Parse(controlNumberADD) : 0)  ;
            this.ControlNumberEDIT = controlNumberEDIT == ""|| controlNumberEDIT==null ? 0 : (int.TryParse(controlNumberEDIT, out a) ? int.Parse(controlNumberEDIT) : 0)   ;
            this.CuadranteID = CuadranteID == "" || CuadranteID == null ? 0 : (int.TryParse(CuadranteID, out a) ? int.Parse(CuadranteID) : 0);
        }

        public SQModel()
        {

        }

    }
}