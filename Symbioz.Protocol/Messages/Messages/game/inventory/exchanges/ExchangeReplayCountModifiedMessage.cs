


















// Generated on 06/04/2015 18:44:55
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ExchangeReplayCountModifiedMessage : Message
{

public const ushort Id = 6023;
public override ushort MessageId
{
    get { return Id; }
}

public int count;
        

public ExchangeReplayCountModifiedMessage()
{
}

public ExchangeReplayCountModifiedMessage(int count)
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