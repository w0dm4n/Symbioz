


















// Generated on 06/04/2015 18:44:40
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class TreasureHuntDigRequestMessage : Message
{

public const ushort Id = 6485;
public override ushort MessageId
{
    get { return Id; }
}

public sbyte questType;
        

public TreasureHuntDigRequestMessage()
{
}

public TreasureHuntDigRequestMessage(sbyte questType)
        {
            this.questType = questType;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteSByte(questType);
            

}

public override void Deserialize(ICustomDataInput reader)
{

questType = reader.ReadSByte();
            if (questType < 0)
                throw new Exception("Forbidden value on questType = " + questType + ", it doesn't respect the following condition : questType < 0");
            

}


}


}