


















// Generated on 06/04/2015 18:45:32
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class Item
{

public const short Id = 7;
public virtual short TypeId
{
    get { return Id; }
}



public Item()
{
}



public virtual void Serialize(ICustomDataOutput writer)
{



}

public virtual void Deserialize(ICustomDataInput reader)
{



}


}


}