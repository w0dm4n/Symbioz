


















// Generated on 06/04/2015 18:44:10
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class AllianceFactsRequestMessage : Message
{

public const ushort Id = 6409;
public override ushort MessageId
{
    get { return Id; }
}

public uint allianceId;
        

public AllianceFactsRequestMessage()
{
}

public AllianceFactsRequestMessage(uint allianceId)
        {
            this.allianceId = allianceId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhInt(allianceId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

allianceId = reader.ReadVarUhInt();
            if (allianceId < 0)
                throw new Exception("Forbidden value on allianceId = " + allianceId + ", it doesn't respect the following condition : allianceId < 0");
            

}


}


}