


















// Generated on 06/04/2015 18:44:41
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class TreasureHuntFlagRemoveRequestMessage : Message
{

public const ushort Id = 6510;
public override ushort MessageId
{
    get { return Id; }
}

public sbyte questType;
        public sbyte index;
        

public TreasureHuntFlagRemoveRequestMessage()
{
}

public TreasureHuntFlagRemoveRequestMessage(sbyte questType, sbyte index)
        {
            this.questType = questType;
            this.index = index;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteSByte(questType);
            writer.WriteSByte(index);
            

}

public override void Deserialize(ICustomDataInput reader)
{

questType = reader.ReadSByte();
            if (questType < 0)
                throw new Exception("Forbidden value on questType = " + questType + ", it doesn't respect the following condition : questType < 0");
            index = reader.ReadSByte();
            if (index < 0)
                throw new Exception("Forbidden value on index = " + index + ", it doesn't respect the following condition : index < 0");
            

}


}


}