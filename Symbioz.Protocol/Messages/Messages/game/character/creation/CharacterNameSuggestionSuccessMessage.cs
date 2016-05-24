


















// Generated on 06/04/2015 18:44:15
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class CharacterNameSuggestionSuccessMessage : Message
{

public const ushort Id = 5544;
public override ushort MessageId
{
    get { return Id; }
}

public string suggestion;
        

public CharacterNameSuggestionSuccessMessage()
{
}

public CharacterNameSuggestionSuccessMessage(string suggestion)
        {
            this.suggestion = suggestion;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUTF(suggestion);
            

}

public override void Deserialize(ICustomDataInput reader)
{

suggestion = reader.ReadUTF();
            

}


}


}