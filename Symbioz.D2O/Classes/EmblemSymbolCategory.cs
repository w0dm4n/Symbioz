// automatic generation Symbioz.Sync 2015
 
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace Symbioz.D2O.Classes
{
         public class EmblemSymbolCategory
   {
[Cache]
public static List<EmblemSymbolCategory> EmblemSymbolCategorys = new List<EmblemSymbolCategory>();
public Int32 id;
public Int32 nameId;
public EmblemSymbolCategory(Int32 id,Int32 nameId)
{
this.id= id;
this.nameId= nameId;
}
}
}
