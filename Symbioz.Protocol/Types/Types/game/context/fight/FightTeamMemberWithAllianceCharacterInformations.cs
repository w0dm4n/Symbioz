


















// Generated on 06/04/2015 18:45:25
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class FightTeamMemberWithAllianceCharacterInformations : FightTeamMemberCharacterInformations
{

public const short Id = 426;
public override short TypeId
{
    get { return Id; }
}

public Types.BasicAllianceInformations allianceInfos;
        

public FightTeamMemberWithAllianceCharacterInformations()
{
}

public FightTeamMemberWithAllianceCharacterInformations(int id, string name, byte level, Types.BasicAllianceInformations allianceInfos)
         : base(id, name, level)
        {
            this.allianceInfos = allianceInfos;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            allianceInfos.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            allianceInfos = new Types.BasicAllianceInformations();
            allianceInfos.Deserialize(reader);
            

}


}


}