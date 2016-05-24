


















// Generated on 06/04/2015 18:44:24
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameRolePlayShowActorMessage : Message
{

public const ushort Id = 5632;
public override ushort MessageId
{
    get { return Id; }
}

public Types.GameRolePlayActorInformations informations;
        

public GameRolePlayShowActorMessage()
{
}

public GameRolePlayShowActorMessage(Types.GameRolePlayActorInformations informations)
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

informations = Types.ProtocolTypeManager.GetInstance<Types.GameRolePlayActorInformations>(reader.ReadShort());
            informations.Deserialize(reader);
            

}


}


}