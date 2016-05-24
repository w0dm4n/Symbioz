


















// Generated on 06/04/2015 18:44:43
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class SpouseInformationsMessage : Message
{

public const ushort Id = 6356;
public override ushort MessageId
{
    get { return Id; }
}

public Types.FriendSpouseInformations spouse;
        

public SpouseInformationsMessage()
{
}

public SpouseInformationsMessage(Types.FriendSpouseInformations spouse)
        {
            this.spouse = spouse;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteShort(spouse.TypeId);
            spouse.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

spouse = Types.ProtocolTypeManager.GetInstance<Types.FriendSpouseInformations>(reader.ReadShort());
            spouse.Deserialize(reader);
            

}


}


}