// automatic generation Symbioz.Sync 2015
 
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace Symbioz.D2O.Classes
{
         public class Hint
   {
[Cache]
public static List<Hint> Hints = new List<Hint>();
public Int32 id;
public Int32 categoryId;
public Int32 gfx;
public Int32 nameId;
public Int32 mapId;
public Int32 realMapId;
public Int32 x;
public Int32 y;
public Boolean outdoor;
public Int32 subareaId;
public Int32 worldMapId;
public Hint(Int32 id,Int32 categoryId,Int32 gfx,Int32 nameId,Int32 mapId,Int32 realMapId,Int32 x,Int32 y,Boolean outdoor,Int32 subareaId,Int32 worldMapId)
{
this.id= id;
this.categoryId= categoryId;
this.gfx= gfx;
this.nameId= nameId;
this.mapId= mapId;
this.realMapId= realMapId;
this.x= x;
this.y= y;
this.outdoor= outdoor;
this.subareaId= subareaId;
this.worldMapId= worldMapId;
}
}
}
