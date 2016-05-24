


















// Generated on 06/04/2015 18:45:14
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class StartupActionsObjetAttributionMessage : Message
{

public const ushort Id = 1303;
public override ushort MessageId
{
    get { return Id; }
}

public int actionId;
        public int characterId;
        

public StartupActionsObjetAttributionMessage()
{
}

public StartupActionsObjetAttributionMessage(int actionId, int characterId)
        {
            this.actionId = actionId;
            this.characterId = characterId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(actionId);
            writer.WriteInt(characterId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

actionId = reader.ReadInt();
            if (actionId < 0)
                throw new Exception("Forbidden value on actionId = " + actionId + ", it doesn't respect the following condition : actionId < 0");
            characterId = reader.ReadInt();
            if (characterId < 0)
                throw new Exception("Forbidden value on characterId = " + characterId + ", it doesn't respect the following condition : characterId < 0");
            

}


}


}