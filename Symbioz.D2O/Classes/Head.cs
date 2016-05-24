// automatic generation Symbioz.Sync 2015
 
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace Symbioz.D2O.Classes
{
         public class Head
   {
[Cache]
public static List<Head> Heads = new List<Head>();
public Int32 id;
public String skins;
public String assetId;
public Int32 breed;
public Int32 gender;
public String label;
public Int32 order;
public Head(Int32 id,String skins,String assetId,Int32 breed,Int32 gender,String label,Int32 order)
{
this.id= id;
this.skins= skins;
this.assetId= assetId;
this.breed= breed;
this.gender= gender;
this.label= label;
this.order= order;
}
}
}
