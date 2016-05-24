


















// Generated on 06/04/2015 18:45:26
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class BasicGuildInformations : AbstractSocialGroupInfos
{

public const short Id = 365;
public override short TypeId
{
    get { return Id; }
}

public uint guildId;
        public string guildName;
        

public BasicGuildInformations()
{
}

public BasicGuildInformations(uint guildId, string guildName)
        {
            this.guildId = guildId;
            this.guildName = guildName;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteVarUhInt(guildId);
            writer.WriteUTF(guildName);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            guildId = reader.ReadVarUhInt();
            if (guildId < 0)
                throw new Exception("Forbidden value on guildId = " + guildId + ", it doesn't respect the following condition : guildId < 0");
            guildName = reader.ReadUTF();
            

}


}


}