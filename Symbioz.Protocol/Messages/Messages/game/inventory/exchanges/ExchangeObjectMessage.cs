


















// Generated on 06/04/2015 18:44:53
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ExchangeObjectMessage : Message
{

public const ushort Id = 5515;
public override ushort MessageId
{
    get { return Id; }
}

public bool remote;
        

public ExchangeObjectMessage()
{
}

public ExchangeObjectMessage(bool remote)
        {
            this.remote = remote;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteBoolean(remote);
            

}

public override void Deserialize(ICustomDataInput reader)
{

remote = reader.ReadBoolean();
            

}


}


}