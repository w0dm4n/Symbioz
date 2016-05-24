


















// Generated on 06/04/2015 18:44:08
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameActionFightSpellCastMessage : AbstractGameActionFightTargetedAbilityMessage
{

public const ushort Id = 1010;
public override ushort MessageId
{
    get { return Id; }
}

public ushort spellId;
        public sbyte spellLevel;
        public IEnumerable<short> portalsIds;
        

public GameActionFightSpellCastMessage()
{
}

public GameActionFightSpellCastMessage(ushort actionId, int sourceId, int targetId, short destinationCellId, sbyte critical, bool silentCast, ushort spellId, sbyte spellLevel, IEnumerable<short> portalsIds)
         : base(actionId, sourceId, targetId, destinationCellId, critical, silentCast)
        {
            this.spellId = spellId;
            this.spellLevel = spellLevel;
            this.portalsIds = portalsIds;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteVarUhShort(spellId);
            writer.WriteSByte(spellLevel);
            writer.WriteUShort((ushort)portalsIds.Count());
            foreach (var entry in portalsIds)
            {
                 writer.WriteShort(entry);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            spellId = reader.ReadVarUhShort();
            if (spellId < 0)
                throw new Exception("Forbidden value on spellId = " + spellId + ", it doesn't respect the following condition : spellId < 0");
            spellLevel = reader.ReadSByte();
            if ((spellLevel < 1) || (spellLevel > 6))
                throw new Exception("Forbidden value on spellLevel = " + spellLevel + ", it doesn't respect the following condition : (spellLevel < 1) || (spellLevel > 6)");
            var limit = reader.ReadUShort();
            portalsIds = new short[limit];
            for (int i = 0; i < limit; i++)
            {
                 (portalsIds as short[])[i] = reader.ReadShort();
            }
            

}


}


}