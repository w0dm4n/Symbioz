


















// Generated on 06/04/2015 18:44:22
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class SlaveSwitchContextMessage : Message
{

public const ushort Id = 6214;
public override ushort MessageId
{
    get { return Id; }
}

public int masterId;
        public int slaveId;
        public IEnumerable<Types.SpellItem> slaveSpells;
        public Types.CharacterCharacteristicsInformations slaveStats;
        public IEnumerable<Types.Shortcut> shortcuts;
        

public SlaveSwitchContextMessage()
{
}

public SlaveSwitchContextMessage(int masterId, int slaveId, IEnumerable<Types.SpellItem> slaveSpells, Types.CharacterCharacteristicsInformations slaveStats, IEnumerable<Types.Shortcut> shortcuts)
        {
            this.masterId = masterId;
            this.slaveId = slaveId;
            this.slaveSpells = slaveSpells;
            this.slaveStats = slaveStats;
            this.shortcuts = shortcuts;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(masterId);
            writer.WriteInt(slaveId);
            writer.WriteUShort((ushort)slaveSpells.Count());
            foreach (var entry in slaveSpells)
            {
                 entry.Serialize(writer);
            }
            slaveStats.Serialize(writer);
            writer.WriteUShort((ushort)shortcuts.Count());
            foreach (var entry in shortcuts)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

masterId = reader.ReadInt();
            slaveId = reader.ReadInt();
            var limit = reader.ReadUShort();
            slaveSpells = new Types.SpellItem[limit];
            for (int i = 0; i < limit; i++)
            {
                 (slaveSpells as Types.SpellItem[])[i] = new Types.SpellItem();
                 (slaveSpells as Types.SpellItem[])[i].Deserialize(reader);
            }
            slaveStats = new Types.CharacterCharacteristicsInformations();
            slaveStats.Deserialize(reader);
            limit = reader.ReadUShort();
            shortcuts = new Types.Shortcut[limit];
            for (int i = 0; i < limit; i++)
            {
                 (shortcuts as Types.Shortcut[])[i] = Types.ProtocolTypeManager.GetInstance<Types.Shortcut>(reader.ReadShort());
                 (shortcuts as Types.Shortcut[])[i].Deserialize(reader);
            }
            

}


}


}