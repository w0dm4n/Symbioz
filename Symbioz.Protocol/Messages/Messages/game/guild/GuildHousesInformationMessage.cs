


















// Generated on 06/04/2015 18:44:44
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GuildHousesInformationMessage : Message
{

public const ushort Id = 5919;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<Types.HouseInformationsForGuild> housesInformations;
        

public GuildHousesInformationMessage()
{
}

public GuildHousesInformationMessage(IEnumerable<Types.HouseInformationsForGuild> housesInformations)
        {
            this.housesInformations = housesInformations;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort((ushort)housesInformations.Count());
            foreach (var entry in housesInformations)
            {
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

var limit = reader.ReadUShort();
            housesInformations = new Types.HouseInformationsForGuild[limit];
            for (int i = 0; i < limit; i++)
            {
                 (housesInformations as Types.HouseInformationsForGuild[])[i] = new Types.HouseInformationsForGuild();
                 (housesInformations as Types.HouseInformationsForGuild[])[i].Deserialize(reader);
            }
            

}


}


}