


















// Generated on 06/04/2015 18:44:47
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GuildFightLeaveRequestMessage : Message
{

public const ushort Id = 5715;
public override ushort MessageId
{
    get { return Id; }
}

public int taxCollectorId;
        public uint characterId;
        

public GuildFightLeaveRequestMessage()
{
}

public GuildFightLeaveRequestMessage(int taxCollectorId, uint characterId)
        {
            this.taxCollectorId = taxCollectorId;
            this.characterId = characterId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(taxCollectorId);
            writer.WriteVarUhInt(characterId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

taxCollectorId = reader.ReadInt();
            if (taxCollectorId < 0)
                throw new Exception("Forbidden value on taxCollectorId = " + taxCollectorId + ", it doesn't respect the following condition : taxCollectorId < 0");
            characterId = reader.ReadVarUhInt();
            if (characterId < 0)
                throw new Exception("Forbidden value on characterId = " + characterId + ", it doesn't respect the following condition : characterId < 0");
            

}


}


}