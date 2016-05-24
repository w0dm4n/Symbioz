


















// Generated on 06/04/2015 18:45:24
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class FightAllianceTeamInformations : FightTeamInformations
{

public const short Id = 439;
public override short TypeId
{
    get { return Id; }
}

public sbyte relation;
        

public FightAllianceTeamInformations()
{
}

public FightAllianceTeamInformations(sbyte teamId, int leaderId, sbyte teamSide, sbyte teamTypeId, sbyte nbWaves, IEnumerable<Types.FightTeamMemberInformations> teamMembers, sbyte relation)
         : base(teamId, leaderId, teamSide, teamTypeId, nbWaves, teamMembers)
        {
            this.relation = relation;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteSByte(relation);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            relation = reader.ReadSByte();
            if (relation < 0)
                throw new Exception("Forbidden value on relation = " + relation + ", it doesn't respect the following condition : relation < 0");
            

}


}


}