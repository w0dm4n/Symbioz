// automatic generation Symbioz.Sync 2015
 
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace Symbioz.D2O.Classes
{
         public class ExternalNotification
   {
[Cache]
public static List<ExternalNotification> ExternalNotifications = new List<ExternalNotification>();
public Int32 id;
public Int32 categoryId;
public Int32 iconId;
public Int32 colorId;
public Int32 descriptionId;
public Boolean defaultEnable;
public Boolean defaultSound;
public Boolean defaultMultiAccount;
public Boolean defaultNotify;
public String name;
public Int32 messageId;
public ExternalNotification(Int32 id,Int32 categoryId,Int32 iconId,Int32 colorId,Int32 descriptionId,Boolean defaultEnable,Boolean defaultSound,Boolean defaultMultiAccount,Boolean defaultNotify,String name,Int32 messageId)
{
this.id= id;
this.categoryId= categoryId;
this.iconId= iconId;
this.colorId= colorId;
this.descriptionId= descriptionId;
this.defaultEnable= defaultEnable;
this.defaultSound= defaultSound;
this.defaultMultiAccount= defaultMultiAccount;
this.defaultNotify= defaultNotify;
this.name= name;
this.messageId= messageId;
}
}
}
