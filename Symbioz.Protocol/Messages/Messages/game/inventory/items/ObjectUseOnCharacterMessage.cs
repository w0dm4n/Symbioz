


















// Generated on 06/04/2015 18:45:06
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ObjectUseOnCharacterMessage : ObjectUseMessage
{

public const ushort Id = 3003;
public override ushort MessageId
{
    get { return Id; }
}

public uint characterId;
        

public ObjectUseOnCharacterMessage()
{
}

public ObjectUseOnCharacterMessage(uint objectUID, uint characterId)
         : base(objectUID)
        {
            this.characterId = characterId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteVarUhInt(characterId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            characterId = reader.ReadVarUhInt();
            if (characterId < 0)
                throw new Exception("Forbidden value on characterId = " + characterId + ", it doesn't respect the following condition : characterId < 0");
            

}


}


}