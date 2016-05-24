


















// Generated on 06/04/2015 18:45:24
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class FightCommonInformations
{

public const short Id = 43;
public virtual short TypeId
{
    get { return Id; }
}

public int fightId;
        public sbyte fightType;
        public IEnumerable<Types.FightTeamInformations> fightTeams;
        public IEnumerable<ushort> fightTeamsPositions;
        public IEnumerable<Types.FightOptionsInformations> fightTeamsOptions;
        

public FightCommonInformations()
{
}

public FightCommonInformations(int fightId, sbyte fightType, IEnumerable<Types.FightTeamInformations> fightTeams, IEnumerable<ushort> fightTeamsPositions, IEnumerable<Types.FightOptionsInformations> fightTeamsOptions)
        {
            this.fightId = fightId;
            this.fightType = fightType;
            this.fightTeams = fightTeams;
            this.fightTeamsPositions = fightTeamsPositions;
            this.fightTeamsOptions = fightTeamsOptions;
        }
        

public virtual void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(fightId);
            writer.WriteSByte(fightType);
            writer.WriteUShort((ushort)fightTeams.Count());
            foreach (var entry in fightTeams)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)fightTeamsPositions.Count());
            foreach (var entry in fightTeamsPositions)
            {
                 writer.WriteVarUhShort(entry);
            }
            writer.WriteUShort((ushort)fightTeamsOptions.Count());
            foreach (var entry in fightTeamsOptions)
            {
                 entry.Serialize(writer);
            }
            

}

public virtual void Deserialize(ICustomDataInput reader)
{

fightId = reader.ReadInt();
            fightType = reader.ReadSByte();
            if (fightType < 0)
                throw new Exception("Forbidden value on fightType = " + fightType + ", it doesn't respect the following condition : fightType < 0");
            var limit = reader.ReadUShort();
            fightTeams = new Types.FightTeamInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (fightTeams as Types.FightTeamInformations[])[i] = Types.ProtocolTypeManager.GetInstance<Types.FightTeamInformations>(reader.ReadShort());
                 (fightTeams as Types.FightTeamInformations[])[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            fightTeamsPositions = new ushort[limit];
            for (int i = 0; i < limit; i++)
            {
                 (fightTeamsPositions as ushort[])[i] = reader.ReadVarUhShort();
            }
            limit = reader.ReadUShort();
            fightTeamsOptions = new Types.FightOptionsInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (fightTeamsOptions as Types.FightOptionsInformations[])[i] = new Types.FightOptionsInformations();
                 (fightTeamsOptions as Types.FightOptionsInformations[])[i].Deserialize(reader);
            }
            

}


}


}