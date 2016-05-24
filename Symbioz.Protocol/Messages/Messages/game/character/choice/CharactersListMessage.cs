


















// Generated on 06/04/2015 18:44:13
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class CharactersListMessage : BasicCharactersListMessage
{

public const ushort Id = 151;
public override ushort MessageId
{
    get { return Id; }
}

public bool hasStartupActions;
        

public CharactersListMessage()
{
}

public CharactersListMessage(IEnumerable<Types.CharacterBaseInformations> characters, bool hasStartupActions)
         : base(characters)
        {
            this.hasStartupActions = hasStartupActions;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteBoolean(hasStartupActions);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            hasStartupActions = reader.ReadBoolean();
            

}


}


}