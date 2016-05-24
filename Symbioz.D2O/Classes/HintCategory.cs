// automatic generation Symbioz.Sync 2015
 
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace Symbioz.D2O.Classes
{
         public class HintCategory
   {
[Cache]
public static List<HintCategory> HintCategorys = new List<HintCategory>();
public Int32 id;
public Int32 nameId;
public HintCategory(Int32 id,Int32 nameId)
{
this.id= id;
this.nameId= nameId;
}
}
}
