


















// Generated on 06/04/2015 18:44:48
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class IdolPartyRegisterRequestMessage : Message
{

public const ushort Id = 6582;
public override ushort MessageId
{
    get { return Id; }
}

public bool register;
        

public IdolPartyRegisterRequestMessage()
{
}

public IdolPartyRegisterRequestMessage(bool register)
        {
            this.register = register;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteBoolean(register);
            

}

public override void Deserialize(ICustomDataInput reader)
{

register = reader.ReadBoolean();
            

}


}


}