


















// Generated on 06/04/2015 18:44:36
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class PartyLocateMembersMessage : AbstractPartyMessage
{

public const ushort Id = 5595;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<Types.PartyMemberGeoPosition> geopositions;
        

public PartyLocateMembersMessage()
{
}

public PartyLocateMembersMessage(uint partyId, IEnumerable<Types.PartyMemberGeoPosition> geopositions)
         : base(partyId)
        {
            this.geopositions = geopositions;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteUShort((ushort)geopositions.Count());
            foreach (var entry in geopositions)
            {
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            var limit = reader.ReadUShort();
            geopositions = new Types.PartyMemberGeoPosition[limit];
            for (int i = 0; i < limit; i++)
            {
                 (geopositions as Types.PartyMemberGeoPosition[])[i] = new Types.PartyMemberGeoPosition();
                 (geopositions as Types.PartyMemberGeoPosition[])[i].Deserialize(reader);
            }
            

}


}


}