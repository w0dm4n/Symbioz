


















// Generated on 06/04/2015 18:45:09
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class AreaFightModificatorUpdateMessage : Message
{

public const ushort Id = 6493;
public override ushort MessageId
{
    get { return Id; }
}

public int spellPairId;
        

public AreaFightModificatorUpdateMessage()
{
}

public AreaFightModificatorUpdateMessage(int spellPairId)
        {
            this.spellPairId = spellPairId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(spellPairId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

spellPairId = reader.ReadInt();
            

}


}


}