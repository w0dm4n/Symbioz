


















// Generated on 06/04/2015 18:45:10
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class PrismFightAddedMessage : Message
{

public const ushort Id = 6452;
public override ushort MessageId
{
    get { return Id; }
}

public Types.PrismFightersInformation fight;
        

public PrismFightAddedMessage()
{
}

public PrismFightAddedMessage(Types.PrismFightersInformation fight)
        {
            this.fight = fight;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

fight.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

fight = new Types.PrismFightersInformation();
            fight.Deserialize(reader);
            

}


}


}