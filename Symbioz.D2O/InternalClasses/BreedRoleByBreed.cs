// automatic generation Symbioz.Sync 2015

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbioz.D2O.InternalClasses
{
    public class BreedRoleByBreed : ID2OInternalClass
    {
        
        public Int32 breedId;
        public Int32 roleId;
        public Int32 descriptionId;
        public Int32 value;
        public Int32 order;
        public BreedRoleByBreed(Int32 breedId, Int32 roleId, Int32 descriptionId, Int32 value, Int32 order)
        {
            this.breedId = breedId;
            this.roleId = roleId;
            this.descriptionId = descriptionId;
            this.value = value;
            this.order = order;
        }

   
    }
}
