


















// Generated on 06/04/2015 18:44:44
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GuildHouseUpdateInformationMessage : Message
{

public const ushort Id = 6181;
public override ushort MessageId
{
    get { return Id; }
}

public Types.HouseInformationsForGuild housesInformations;
        

public GuildHouseUpdateInformationMessage()
{
}

public GuildHouseUpdateInformationMessage(Types.HouseInformationsForGuild housesInformations)
        {
            this.housesInformations = housesInformations;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

housesInformations.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

housesInformations = new Types.HouseInformationsForGuild();
            housesInformations.Deserialize(reader);
            

}


}


}