


















// Generated on 06/04/2015 18:45:24
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class FightResultPlayerListEntry : FightResultFighterListEntry
{

public const short Id = 24;
public override short TypeId
{
    get { return Id; }
}

public byte level;
        public IEnumerable<Types.FightResultAdditionalData> additional;
        

public FightResultPlayerListEntry()
{
}

public FightResultPlayerListEntry(ushort outcome, sbyte wave, Types.FightLoot rewards, int id, bool alive, byte level, IEnumerable<Types.FightResultAdditionalData> additional)
         : base(outcome, wave, rewards, id, alive)
        {
            this.level = level;
            this.additional = additional;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteByte(level);
            writer.WriteUShort((ushort)additional.Count());
            foreach (var entry in additional)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            level = reader.ReadByte();
            if ((level < 1) || (level > 200))
                throw new Exception("Forbidden value on level = " + level + ", it doesn't respect the following condition : (level < 1) || (level > 200)");
            var limit = reader.ReadUShort();
            additional = new Types.FightResultAdditionalData[limit];
            for (int i = 0; i < limit; i++)
            {
                 (additional as Types.FightResultAdditionalData[])[i] = Types.ProtocolTypeManager.GetInstance<Types.FightResultAdditionalData>(reader.ReadShort());
                 (additional as Types.FightResultAdditionalData[])[i].Deserialize(reader);
            }
            

}


}


}