using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SAM.Entities.Personalizadas
{
    [Serializable]
    public class ItemCodeIntegrado
    {
        public ItemCodeIntegrado() { }

        public ItemCodeIntegrado(int itemCodeID, decimal d1, decimal d2)
        {
            ItemCodeID = itemCodeID;
            Diametro1 = d1;
            Diametro2 = d2;
        }

        [DataMember]
        public int ItemCodeID { get; set; }

        [DataMember]
        public decimal Diametro1 { get; set; }

        [DataMember]
        public decimal Diametro2 { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            ItemCodeIntegrado ic = obj as ItemCodeIntegrado;

            if (ic == null)
            {
                return false;
            }

            return ic.ItemCodeID == ItemCodeID && ic.Diametro1 == Diametro1 && ic.Diametro2 == Diametro2;
        }

        public bool Equals(ItemCodeIntegrado ic)
        {
            return ic.ItemCodeID == ItemCodeID && ic.Diametro1 == Diametro1 && ic.Diametro2 == Diametro2;
        }

        public override int GetHashCode()
        {
            int hash = 23;
            hash = hash*37 + ItemCodeID;
            hash = hash*37 + Diametro1.GetHashCode();
            hash = hash*37 + Diametro2.GetHashCode();
            return hash;
        }
    }
}
