// automatic generation Symbioz.Sync 2015
 
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace Symbioz.D2O.Classes
{
         public class Document
   {
[Cache]
public static List<Document> Documents = new List<Document>();
public Int32 id;
public Int32 typeId;
public Boolean showTitle;
public Boolean showBackgroundImage;
public Int32 titleId;
public Int32 authorId;
public Int32 subTitleId;
public Int32 contentId;
public String contentCSS;
public String clientProperties;
public Document(Int32 id,Int32 typeId,Boolean showTitle,Boolean showBackgroundImage,Int32 titleId,Int32 authorId,Int32 subTitleId,Int32 contentId,String contentCSS,String clientProperties)
{
this.id= id;
this.typeId= typeId;
this.showTitle= showTitle;
this.showBackgroundImage= showBackgroundImage;
this.titleId= titleId;
this.authorId= authorId;
this.subTitleId= subTitleId;
this.contentId= contentId;
this.contentCSS= contentCSS;
this.clientProperties= clientProperties;
}
}
}
