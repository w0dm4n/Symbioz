


















// Generated on 06/04/2015 18:44:15
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class CharacterStatsListMessage : Message
{

public const ushort Id = 500;
public override ushort MessageId
{
    get { return Id; }
}

public Types.CharacterCharacteristicsInformations stats;
        

public CharacterStatsListMessage()
{
}

public CharacterStatsListMessage(Types.CharacterCharacteristicsInformations stats)
        {
            this.stats = stats;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

stats.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

stats = new Types.CharacterCharacteristicsInformations();
            stats.Deserialize(reader);
            

}


}


}