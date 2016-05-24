


















// Generated on 06/04/2015 18:44:22
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameFightShowFighterMessage : Message
{

public const ushort Id = 5864;
public override ushort MessageId
{
    get { return Id; }
}

public Types.GameFightFighterInformations informations;
        

public GameFightShowFighterMessage()
{
}

public GameFightShowFighterMessage(Types.GameFightFighterInformations informations)
        {
            this.informations = informations;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteShort(informations.TypeId);
            informations.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

informations = Types.ProtocolTypeManager.GetInstance<Types.GameFightFighterInformations>(reader.ReadShort());
            informations.Deserialize(reader);
            

}


}


}