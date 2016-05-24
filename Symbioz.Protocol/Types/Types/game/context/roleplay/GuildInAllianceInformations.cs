


















// Generated on 06/04/2015 18:45:27
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class GuildInAllianceInformations : GuildInformations
{

public const short Id = 420;
public override short TypeId
{
    get { return Id; }
}

public byte guildLevel;
        public byte nbMembers;
        public bool enabled;
        

public GuildInAllianceInformations()
{
}

public GuildInAllianceInformations(uint guildId, string guildName, Types.GuildEmblem guildEmblem, byte guildLevel, byte nbMembers, bool enabled)
         : base(guildId, guildName, guildEmblem)
        {
            this.guildLevel = guildLevel;
            this.nbMembers = nbMembers;
            this.enabled = enabled;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteByte(guildLevel);
            writer.WriteByte(nbMembers);
            writer.WriteBoolean(enabled);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            guildLevel = reader.ReadByte();
            if ((guildLevel < 1) || (guildLevel > 200))
                throw new Exception("Forbidden value on guildLevel = " + guildLevel + ", it doesn't respect the following condition : (guildLevel < 1) || (guildLevel > 200)");
            nbMembers = reader.ReadByte();
            if ((nbMembers < 1) || (nbMembers > 240))
                throw new Exception("Forbidden value on nbMembers = " + nbMembers + ", it doesn't respect the following condition : (nbMembers < 1) || (nbMembers > 240)");
            enabled = reader.ReadBoolean();
            

}


}


}