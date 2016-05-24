


















// Generated on 06/04/2015 18:45:37
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class GuildFactSheetInformations : GuildInformations
{

public const short Id = 424;
public override short TypeId
{
    get { return Id; }
}

public uint leaderId;
        public byte guildLevel;
        public ushort nbMembers;
        

public GuildFactSheetInformations()
{
}

public GuildFactSheetInformations(uint guildId, string guildName, Types.GuildEmblem guildEmblem, uint leaderId, byte guildLevel, ushort nbMembers)
         : base(guildId, guildName, guildEmblem)
        {
            this.leaderId = leaderId;
            this.guildLevel = guildLevel;
            this.nbMembers = nbMembers;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteVarUhInt(leaderId);
            writer.WriteByte(guildLevel);
            writer.WriteVarUhShort(nbMembers);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            leaderId = reader.ReadVarUhInt();
            if (leaderId < 0)
                throw new Exception("Forbidden value on leaderId = " + leaderId + ", it doesn't respect the following condition : leaderId < 0");
            guildLevel = reader.ReadByte();
            if ((guildLevel < 0) || (guildLevel > 255))
                throw new Exception("Forbidden value on guildLevel = " + guildLevel + ", it doesn't respect the following condition : (guildLevel < 0) || (guildLevel > 255)");
            nbMembers = reader.ReadVarUhShort();
            if (nbMembers < 0)
                throw new Exception("Forbidden value on nbMembers = " + nbMembers + ", it doesn't respect the following condition : nbMembers < 0");
            

}


}


}