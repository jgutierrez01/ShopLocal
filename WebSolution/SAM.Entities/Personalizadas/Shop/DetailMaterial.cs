
using System; 
 using System.Collections.Generic; 
 using System.Linq; 
 using System.Text; 
 using System.Threading.Tasks; 
 using System.Runtime.Serialization; 
 
 
 namespace SAM.Entities.Personalizadas.Shop 
 { 
     [Serializable] 
     public class DetailMaterial 
     { 
         [DataMember] 
         public string Label { get; set; } 
 
 
         [DataMember] 
         public string UniqueNumber { get; set; } 
 
 
         [DataMember] 
         public string Heat { get; set; } 
 
 
         [DataMember] 
         public string Certified { get; set; } 
 
 
         [DataMember] 
         public string ItemCode { get; set; } 
 
 
         [DataMember] 
         public decimal DiameterOne { get; set; } 
 
 
         [DataMember] 
         public decimal DiameterTwo { get; set; } 
 
 
         [DataMember] 
         public string Descprition { get; set; } 
 
 
         [DataMember] 
         public Int32 Quantity { get; set; } 
 
 
         [DataMember] 
         public string Manufacturer { get; set; } 
 
 
         [DataMember] 
         public string Motion { get; set; } 
 
 
         [DataMember] 
         public string Notes { get; set; } 
 
 
         [DataMember] 
         public int MaterialSpoolID { get; set; } 
     } 
 } 
 


 
   