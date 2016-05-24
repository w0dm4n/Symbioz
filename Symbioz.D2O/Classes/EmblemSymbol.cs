// automatic generation Symbioz.Sync 2015
 
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace Symbioz.D2O.Classes
{
         public class EmblemSymbol
   {
[Cache]
public static List<EmblemSymbol> EmblemSymbols = new List<EmblemSymbol>();
public Int32 id;
public Int32 skinId;
public Int32 iconId;
public Int32 order;
public Int32 categoryId;
public Boolean colorizable;
public EmblemSymbol(Int32 id,Int32 skinId,Int32 iconId,Int32 order,Int32 categoryId,Boolean colorizable)
{
this.id= id;
this.skinId= skinId;
this.iconId= iconId;
this.order= order;
this.categoryId= categoryId;
this.colorizable= colorizable;
}
}
}
