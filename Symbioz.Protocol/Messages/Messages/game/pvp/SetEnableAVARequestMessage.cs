


















// Generated on 06/04/2015 18:45:12
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class SetEnableAVARequestMessage : Message
{

public const ushort Id = 6443;
public override ushort MessageId
{
    get { return Id; }
}

public bool enable;
        

public SetEnableAVARequestMessage()
{
}

public SetEnableAVARequestMessage(bool enable)
        {
            this.enable = enable;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteBoolean(enable);
            

}

public override void Deserialize(ICustomDataInput reader)
{

enable = reader.ReadBoolean();
            

}


}


}