


















// Generated on 06/04/2015 18:44:30
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class JobMultiCraftAvailableSkillsMessage : JobAllowMultiCraftRequestMessage
{

public const ushort Id = 5747;
public override ushort MessageId
{
    get { return Id; }
}

public uint playerId;
        public IEnumerable<ushort> skills;
        

public JobMultiCraftAvailableSkillsMessage()
{
}

public JobMultiCraftAvailableSkillsMessage(bool enabled, uint playerId, IEnumerable<ushort> skills)
         : base(enabled)
        {
            this.playerId = playerId;
            this.skills = skills;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteVarUhInt(playerId);
            writer.WriteUShort((ushort)skills.Count());
            foreach (var entry in skills)
            {
                 writer.WriteVarUhShort(entry);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            playerId = reader.ReadVarUhInt();
            if (playerId < 0)
                throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0");
            var limit = reader.ReadUShort();
            skills = new ushort[limit];
            for (int i = 0; i < limit; i++)
            {
                 (skills as ushort[])[i] = reader.ReadVarUhShort();
            }
            

}


}


}