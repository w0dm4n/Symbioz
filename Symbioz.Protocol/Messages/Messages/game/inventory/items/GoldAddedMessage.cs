


















// Generated on 06/04/2015 18:45:03
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GoldAddedMessage : Message
{

public const ushort Id = 6030;
public override ushort MessageId
{
    get { return Id; }
}

public Types.GoldItem gold;
        

public GoldAddedMessage()
{
}

public GoldAddedMessage(Types.GoldItem gold)
        {
            this.gold = gold;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

gold.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

gold = new Types.GoldItem();
            gold.Deserialize(reader);
            

}


}


}