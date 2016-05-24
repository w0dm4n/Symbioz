


















// Generated on 06/04/2015 18:44:41
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class TreasureHuntGiveUpRequestMessage : Message
{

public const ushort Id = 6487;
public override ushort MessageId
{
    get { return Id; }
}

public sbyte questType;
        

public TreasureHuntGiveUpRequestMessage()
{
}

public TreasureHuntGiveUpRequestMessage(sbyte questType)
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