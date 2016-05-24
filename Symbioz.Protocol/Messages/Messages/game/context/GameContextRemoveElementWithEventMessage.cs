


















// Generated on 06/04/2015 18:44:18
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameContextRemoveElementWithEventMessage : GameContextRemoveElementMessage
{

public const ushort Id = 6412;
public override ushort MessageId
{
    get { return Id; }
}

public sbyte elementEventId;
        

public GameContextRemoveElementWithEventMessage()
{
}

public GameContextRemoveElementWithEventMessage(int id, sbyte elementEventId)
         : base(id)
        {
            this.elementEventId = elementEventId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteSByte(elementEventId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            elementEventId = reader.ReadSByte();
            if (elementEventId < 0)
                throw new Exception("Forbidden value on elementEventId = " + elementEventId + ", it doesn't respect the following condition : elementEventId < 0");
            

}


}


}