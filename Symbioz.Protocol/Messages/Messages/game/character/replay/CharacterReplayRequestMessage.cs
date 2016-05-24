


















// Generated on 06/04/2015 18:44:15
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class CharacterReplayRequestMessage : Message
{

public const ushort Id = 167;
public override ushort MessageId
{
    get { return Id; }
}

public int characterId;
        

public CharacterReplayRequestMessage()
{
}

public CharacterReplayRequestMessage(int characterId)
        {
            this.characterId = characterId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(characterId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

characterId = reader.ReadInt();
            if (characterId < 0)
                throw new Exception("Forbidden value on characterId = " + characterId + ", it doesn't respect the following condition : characterId < 0");
            

}


}


}