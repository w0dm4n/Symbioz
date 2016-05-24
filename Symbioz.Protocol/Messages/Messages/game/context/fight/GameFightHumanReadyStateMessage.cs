


















// Generated on 06/04/2015 18:44:19
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameFightHumanReadyStateMessage : Message
{

public const ushort Id = 740;
public override ushort MessageId
{
    get { return Id; }
}

public uint characterId;
        public bool isReady;
        

public GameFightHumanReadyStateMessage()
{
}

public GameFightHumanReadyStateMessage(uint characterId, bool isReady)
        {
            this.characterId = characterId;
            this.isReady = isReady;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhInt(characterId);
            writer.WriteBoolean(isReady);
            

}

public override void Deserialize(ICustomDataInput reader)
{

characterId = reader.ReadVarUhInt();
            if (characterId < 0)
                throw new Exception("Forbidden value on characterId = " + characterId + ", it doesn't respect the following condition : characterId < 0");
            isReady = reader.ReadBoolean();
            

}


}


}