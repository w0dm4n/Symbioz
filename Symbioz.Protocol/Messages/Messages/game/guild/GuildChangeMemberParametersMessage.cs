


















// Generated on 06/04/2015 18:44:43
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GuildChangeMemberParametersMessage : Message
{

public const ushort Id = 5549;
public override ushort MessageId
{
    get { return Id; }
}

public uint memberId;
        public ushort rank;
        public sbyte experienceGivenPercent;
        public uint rights;
        

public GuildChangeMemberParametersMessage()
{
}

public GuildChangeMemberParametersMessage(uint memberId, ushort rank, sbyte experienceGivenPercent, uint rights)
        {
            this.memberId = memberId;
            this.rank = rank;
            this.experienceGivenPercent = experienceGivenPercent;
            this.rights = rights;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhInt(memberId);
            writer.WriteVarUhShort(rank);
            writer.WriteSByte(experienceGivenPercent);
            writer.WriteVarUhInt(rights);
            

}

public override void Deserialize(ICustomDataInput reader)
{

memberId = reader.ReadVarUhInt();
            if (memberId < 0)
                throw new Exception("Forbidden value on memberId = " + memberId + ", it doesn't respect the following condition : memberId < 0");
            rank = reader.ReadVarUhShort();
            if (rank < 0)
                throw new Exception("Forbidden value on rank = " + rank + ", it doesn't respect the following condition : rank < 0");
            experienceGivenPercent = reader.ReadSByte();
            if ((experienceGivenPercent < 0) || (experienceGivenPercent > 100))
                throw new Exception("Forbidden value on experienceGivenPercent = " + experienceGivenPercent + ", it doesn't respect the following condition : (experienceGivenPercent < 0) || (experienceGivenPercent > 100)");
            rights = reader.ReadVarUhInt();
            if (rights < 0)
                throw new Exception("Forbidden value on rights = " + rights + ", it doesn't respect the following condition : rights < 0");
            

}


}


}