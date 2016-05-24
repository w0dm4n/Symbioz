


















// Generated on 06/04/2015 18:44:02
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class CredentialsAcknowledgementMessage : Message
{

public const ushort Id = 6314;
public override ushort MessageId
{
    get { return Id; }
}



public CredentialsAcknowledgementMessage()
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