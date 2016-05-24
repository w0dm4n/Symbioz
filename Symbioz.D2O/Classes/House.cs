// automatic generation Symbioz.Sync 2015
 
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace Symbioz.D2O.Classes
{
         public class House
   {
[Cache]
public static List<House> Houses = new List<House>();
public Int32 typeId;
public Int32 defaultPrice;
public Int32 nameId;
public Int32 descriptionId;
public Int32 gfxId;
public House(Int32 typeId,Int32 defaultPrice,Int32 nameId,Int32 descriptionId,Int32 gfxId)
{
this.typeId= typeId;
this.defaultPrice= defaultPrice;
this.nameId= nameId;
this.descriptionId= descriptionId;
this.gfxId= gfxId;
}
}
}
