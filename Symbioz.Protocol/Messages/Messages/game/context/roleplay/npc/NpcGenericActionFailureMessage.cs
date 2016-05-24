


















// Generated on 06/04/2015 18:44:31
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class NpcGenericActionFailureMessage : Message
{

public const ushort Id = 5900;
public override ushort MessageId
{
    get { return Id; }
}



public NpcGenericActionFailureMessage()
{
}



public override void Serialize(ICustomDataOutput writer)
{



}

public override void Deserialize(ICustomDataInput reader)
{



}


}


}