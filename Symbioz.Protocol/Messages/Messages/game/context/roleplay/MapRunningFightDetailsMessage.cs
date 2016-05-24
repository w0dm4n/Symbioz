


















// Generated on 06/04/2015 18:44:25
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class MapRunningFightDetailsMessage : Message
{

public const ushort Id = 5751;
public override ushort MessageId
{
    get { return Id; }
}

public int fightId;
        public IEnumerable<Types.GameFightFighterLightInformations> attackers;
        public IEnumerable<Types.GameFightFighterLightInformations> defenders;
        

public MapRunningFightDetailsMessage()
{
}

public MapRunningFightDetailsMessage(int fightId, IEnumerable<Types.GameFightFighterLightInformations> attackers, IEnumerable<Types.GameFightFighterLightInformations> defenders)
        {
            this.fightId = fightId;
            this.attackers = attackers;
            this.defenders = defenders;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(fightId);
            writer.WriteUShort((ushort)attackers.Count());
            foreach (var entry in attackers)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)defenders.Count());
            foreach (var entry in defenders)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

fightId = reader.ReadInt();
            if (fightId < 0)
                throw new Exception("Forbidden value on fightId = " + fightId + ", it doesn't respect the following condition : fightId < 0");
            var limit = reader.ReadUShort();
            attackers = new Types.GameFightFighterLightInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (attackers as Types.GameFightFighterLightInformations[])[i] = Types.ProtocolTypeManager.GetInstance<Types.GameFightFighterLightInformations>(reader.ReadShort());
                 (attackers as Types.GameFightFighterLightInformations[])[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            defenders = new Types.GameFightFighterLightInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (defenders as Types.GameFightFighterLightInformations[])[i] = Types.ProtocolTypeManager.GetInstance<Types.GameFightFighterLightInformations>(reader.ReadShort());
                 (defenders as Types.GameFightFighterLightInformations[])[i].Deserialize(reader);
            }
            

}


}


}