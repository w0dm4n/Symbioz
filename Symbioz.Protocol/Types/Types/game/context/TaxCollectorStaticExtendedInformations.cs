


















// Generated on 06/04/2015 18:45:24
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class TaxCollectorStaticExtendedInformations : TaxCollectorStaticInformations
{

public const short Id = 440;
public override short TypeId
{
    get { return Id; }
}

public Types.AllianceInformations allianceIdentity;
        

public TaxCollectorStaticExtendedInformations()
{
}

public TaxCollectorStaticExtendedInformations(ushort firstNameId, ushort lastNameId, Types.GuildInformations guildIdentity, Types.AllianceInformations allianceIdentity)
         : base(firstNameId, lastNameId, guildIdentity)
        {
            this.allianceIdentity = allianceIdentity;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            allianceIdentity.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            allianceIdentity = new Types.AllianceInformations();
            allianceIdentity.Deserialize(reader);
            

}


}


}