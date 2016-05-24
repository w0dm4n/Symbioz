


















// Generated on 06/04/2015 18:44:41
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class TreasureHuntFlagRequestAnswerMessage : Message
{

public const ushort Id = 6507;
public override ushort MessageId
{
    get { return Id; }
}

public sbyte questType;
        public sbyte result;
        public sbyte index;
        

public TreasureHuntFlagRequestAnswerMessage()
{
}

public TreasureHuntFlagRequestAnswerMessage(sbyte questType, sbyte result, sbyte index)
        {
            this.questType = questType;
            this.result = result;
            this.index = index;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteSByte(questType);
            writer.WriteSByte(result);
            writer.WriteSByte(index);
            

}

public override void Deserialize(ICustomDataInput reader)
{

questType = reader.ReadSByte();
            if (questType < 0)
                throw new Exception("Forbidden value on questType = " + questType + ", it doesn't respect the following condition : questType < 0");
            result = reader.ReadSByte();
            if (result < 0)
                throw new Exception("Forbidden value on result = " + result + ", it doesn't respect the following condition : result < 0");
            index = reader.ReadSByte();
            if (index < 0)
                throw new Exception("Forbidden value on index = " + index + ", it doesn't respect the following condition : index < 0");
            

}


}


}