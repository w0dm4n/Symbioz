


















// Generated on 06/04/2015 18:44:10
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class AllianceFactsMessage : Message
{

public const ushort Id = 6414;
public override ushort MessageId
{
    get { return Id; }
}

public Types.AllianceFactSheetInformations infos;
        public IEnumerable<Types.GuildInAllianceInformations> guilds;
        public IEnumerable<ushort> controlledSubareaIds;
        public uint leaderCharacterId;
        public string leaderCharacterName;
        

public AllianceFactsMessage()
{
}

public AllianceFactsMessage(Types.AllianceFactSheetInformations infos, IEnumerable<Types.GuildInAllianceInformations> guilds, IEnumerable<ushort> controlledSubareaIds, uint leaderCharacterId, string leaderCharacterName)
        {
            this.infos = infos;
            this.guilds = guilds;
            this.controlledSubareaIds = controlledSubareaIds;
            this.leaderCharacterId = leaderCharacterId;
            this.leaderCharacterName = leaderCharacterName;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteShort(infos.TypeId);
            infos.Serialize(writer);
            writer.WriteUShort((ushort)guilds.Count());
            foreach (var entry in guilds)
            {
                 entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)controlledSubareaIds.Count());
            foreach (var entry in controlledSubareaIds)
            {
                 writer.WriteVarUhShort(entry);
            }
            writer.WriteVarUhInt(leaderCharacterId);
            writer.WriteUTF(leaderCharacterName);
            

}

public override void Deserialize(ICustomDataInput reader)
{

infos = Types.ProtocolTypeManager.GetInstance<Types.AllianceFactSheetInformations>(reader.ReadShort());
            infos.Deserialize(reader);
            var limit = reader.ReadUShort();
            guilds = new Types.GuildInAllianceInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (guilds as Types.GuildInAllianceInformations[])[i] = new Types.GuildInAllianceInformations();
                 (guilds as Types.GuildInAllianceInformations[])[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            controlledSubareaIds = new ushort[limit];
            for (int i = 0; i < limit; i++)
            {
                 (controlledSubareaIds as ushort[])[i] = reader.ReadVarUhShort();
            }
            leaderCharacterId = reader.ReadVarUhInt();
            if (leaderCharacterId < 0)
                throw new Exception("Forbidden value on leaderCharacterId = " + leaderCharacterId + ", it doesn't respect the following condition : leaderCharacterId < 0");
            leaderCharacterName = reader.ReadUTF();
            

}


}


}