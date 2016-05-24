


















// Generated on 06/04/2015 18:45:36
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class PrismFightersInformation
{

public const short Id = 443;
public virtual short TypeId
{
    get { return Id; }
}

public ushort subAreaId;
        public Types.ProtectedEntityWaitingForHelpInfo waitingForHelpInfo;
        public IEnumerable<Types.CharacterMinimalPlusLookInformations> allyCharactersInformations;
        public IEnumerable<Types.CharacterMinimalPlusLookInformations> enemyCharactersInformations;
        

public PrismFightersInformation()
{
}

public PrismFightersInformation(ushort subAreaId, Types.ProtectedEntityWaitingForHelpInfo waitingForHelpInfo, IEnumerable<Types.CharacterMinimalPlusLookInformations> allyCharactersInformations, IEnumerable<Types.CharacterMinimalPlusLookInformations> enemyCharactersInformations)
        {
            this.subAreaId = subAreaId;
            this.waitingForHelpInfo = waitingForHelpInfo;
            this.allyCharactersInformations = allyCharactersInformations;
            this.enemyCharactersInformations = enemyCharactersInformations;
        }
        

public virtual void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhShort(subAreaId);
            waitingForHelpInfo.Serialize(writer);
            writer.WriteUShort((ushort)allyCharactersInformations.Count());
            foreach (var entry in allyCharactersInformations)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)enemyCharactersInformations.Count());
            foreach (var entry in enemyCharactersInformations)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
            

}

public virtual void Deserialize(ICustomDataInput reader)
{

subAreaId = reader.ReadVarUhShort();
            if (subAreaId < 0)
                throw new Exception("Forbidden value on subAreaId = " + subAreaId + ", it doesn't respect the following condition : subAreaId < 0");
            waitingForHelpInfo = new Types.ProtectedEntityWaitingForHelpInfo();
            waitingForHelpInfo.Deserialize(reader);
            var limit = reader.ReadUShort();
            allyCharactersInformations = new Types.CharacterMinimalPlusLookInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (allyCharactersInformations as Types.CharacterMinimalPlusLookInformations[])[i] = Types.ProtocolTypeManager.GetInstance<Types.CharacterMinimalPlusLookInformations>(reader.ReadShort());
                 (allyCharactersInformations as Types.CharacterMinimalPlusLookInformations[])[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            enemyCharactersInformations = new Types.CharacterMinimalPlusLookInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (enemyCharactersInformations as Types.CharacterMinimalPlusLookInformations[])[i] = Types.ProtocolTypeManager.GetInstance<Types.CharacterMinimalPlusLookInformations>(reader.ReadShort());
                 (enemyCharactersInformations as Types.CharacterMinimalPlusLookInformations[])[i].Deserialize(reader);
            }
            

}


}


}