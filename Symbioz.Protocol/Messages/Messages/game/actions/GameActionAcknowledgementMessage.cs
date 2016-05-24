


















// Generated on 06/04/2015 18:44:05
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameActionAcknowledgementMessage : Message
{

public const ushort Id = 957;
public override ushort MessageId
{
    get { return Id; }
}

public bool valid;
        public sbyte actionId;
        

public GameActionAcknowledgementMessage()
{
}

public GameActionAcknowledgementMessage(bool valid, sbyte actionId)
        {
            this.valid = valid;
            this.actionId = actionId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteBoolean(valid);
            writer.WriteSByte(actionId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

valid = reader.ReadBoolean();
            actionId = reader.ReadSByte();
            

}


}


}