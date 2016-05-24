


















// Generated on 06/04/2015 18:44:41
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class TreasureHuntLegendaryRequestMessage : Message
{

public const ushort Id = 6499;
public override ushort MessageId
{
    get { return Id; }
}

public ushort legendaryId;
        

public TreasureHuntLegendaryRequestMessage()
{
}

public TreasureHuntLegendaryRequestMessage(ushort legendaryId)
        {
            this.legendaryId = legendaryId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhShort(legendaryId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

legendaryId = reader.ReadVarUhShort();
            if (legendaryId < 0)
                throw new Exception("Forbidden value on legendaryId = " + legendaryId + ", it doesn't respect the following condition : legendaryId < 0");
            

}


}


}