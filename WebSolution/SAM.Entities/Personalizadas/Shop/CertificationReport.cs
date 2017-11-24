using System; 
 using System.Collections.Generic; 
 using System.Linq; 
 using System.Text; 
 using System.Threading.Tasks;  
 
 namespace SAM.Entities.Personalizadas.Shop 
 { 
     public class CertificationReport 
     { 
         public DetailSpool DetailSpool { get; set; } 
         public List<DetailJoint> DetailJoints { get; set; } 
         public List<DetailMaterial> DetailMaterials { get; set; } 
         public DetSpool detSpool { get; set; } 
         public CheckListShipping DetailCheckListShipping { get; set; } 
 
 
         public CertificationReport() 
         { 
             this.DetailJoints = new List<DetailJoint>(); 
             this.DetailMaterials = new List<DetailMaterial>(); 
             this.DetailSpool = new DetailSpool(); 
             this.DetailCheckListShipping = new CheckListShipping(); 
         } 
 
 
         public CertificationReport(DetailSpool detSpool, List<DetailJoint> detJoints, List<DetailMaterial> detMaterials, CheckListShipping detCheckListShipping) 
         { 
             this.DetailJoints.AddRange(detJoints); 
             this.DetailMaterials.AddRange(detMaterials); 
             this.DetailSpool = detSpool; 
             this.DetailCheckListShipping = detCheckListShipping; 
         } 
     } 
 } 
