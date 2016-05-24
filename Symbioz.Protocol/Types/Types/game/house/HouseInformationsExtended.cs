


















// Generated on 06/04/2015 18:45:34
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class HouseInformationsExtended : HouseInformations
{

public const short Id = 112;
public override short TypeId
{
    get { return Id; }
}

public Types.GuildInformations guildInfo;
        

public HouseInformationsExtended()
{
}

public HouseInformationsExtended(bool isOnSale, bool isSaleLocked, uint houseId, IEnumerable<int> doorsOnMap, string ownerName, ushort modelId, Types.GuildInformations guildInfo)
         : base(isOnSale, isSaleLocked, houseId, doorsOnMap, ownerName, modelId)
        {
            this.guildInfo = guildInfo;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            guildInfo.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            guildInfo = new Types.GuildInformations();
            guildInfo.Deserialize(reader);
            

}


}


}