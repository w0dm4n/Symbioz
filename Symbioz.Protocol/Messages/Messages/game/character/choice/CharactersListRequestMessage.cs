


















// Generated on 06/04/2015 18:44:14
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class CharactersListRequestMessage : Message
{

public const ushort Id = 150;
public override ushort MessageId
{
    get { return Id; }
}



public CharactersListRequestMessage()
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