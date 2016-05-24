


















// Generated on 06/04/2015 18:44:58
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ExchangeStartOkNpcTradeMessage : Message
{

public const ushort Id = 5785;
public override ushort MessageId
{
    get { return Id; }
}

public int npcId;
        

public ExchangeStartOkNpcTradeMessage()
{
}

public ExchangeStartOkNpcTradeMessage(int npcId)
        {
            this.npcId = npcId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(npcId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

npcId = reader.ReadInt();
            

}


}


}