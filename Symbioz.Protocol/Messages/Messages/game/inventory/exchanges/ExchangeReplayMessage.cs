


















// Generated on 06/04/2015 18:44:55
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ExchangeReplayMessage : Message
{

public const ushort Id = 6002;
public override ushort MessageId
{
    get { return Id; }
}

public int count;
        

public ExchangeReplayMessage()
{
}

public ExchangeReplayMessage(int count)
        {
            this.count = count;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarInt(count);
            

}

public override void Deserialize(ICustomDataInput reader)
{

count = reader.ReadVarInt();
            

}


}


}