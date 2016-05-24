


















// Generated on 06/04/2015 18:44:40
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class TreasureHuntAvailableRetryCountUpdateMessage : Message
{

public const ushort Id = 6491;
public override ushort MessageId
{
    get { return Id; }
}

public sbyte questType;
        public int availableRetryCount;
        

public TreasureHuntAvailableRetryCountUpdateMessage()
{
}

public TreasureHuntAvailableRetryCountUpdateMessage(sbyte questType, int availableRetryCount)
        {
            this.questType = questType;
            this.availableRetryCount = availableRetryCount;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteSByte(questType);
            writer.WriteInt(availableRetryCount);
            

}

public override void Deserialize(ICustomDataInput reader)
{

questType = reader.ReadSByte();
            if (questType < 0)
                throw new Exception("Forbidden value on questType = " + questType + ", it doesn't respect the following condition : questType < 0");
            availableRetryCount = reader.ReadInt();
            

}


}


}