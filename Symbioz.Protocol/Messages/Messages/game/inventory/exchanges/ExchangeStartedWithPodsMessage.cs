


















// Generated on 06/04/2015 18:44:56
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ExchangeStartedWithPodsMessage : ExchangeStartedMessage
{

public const ushort Id = 6129;
public override ushort MessageId
{
    get { return Id; }
}

public int firstCharacterId;
        public uint firstCharacterCurrentWeight;
        public uint firstCharacterMaxWeight;
        public int secondCharacterId;
        public uint secondCharacterCurrentWeight;
        public uint secondCharacterMaxWeight;
        

public ExchangeStartedWithPodsMessage()
{
}

public ExchangeStartedWithPodsMessage(sbyte exchangeType, int firstCharacterId, uint firstCharacterCurrentWeight, uint firstCharacterMaxWeight, int secondCharacterId, uint secondCharacterCurrentWeight, uint secondCharacterMaxWeight)
         : base(exchangeType)
        {
            this.firstCharacterId = firstCharacterId;
            this.firstCharacterCurrentWeight = firstCharacterCurrentWeight;
            this.firstCharacterMaxWeight = firstCharacterMaxWeight;
            this.secondCharacterId = secondCharacterId;
            this.secondCharacterCurrentWeight = secondCharacterCurrentWeight;
            this.secondCharacterMaxWeight = secondCharacterMaxWeight;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteInt(firstCharacterId);
            writer.WriteVarUhInt(firstCharacterCurrentWeight);
            writer.WriteVarUhInt(firstCharacterMaxWeight);
            writer.WriteInt(secondCharacterId);
            writer.WriteVarUhInt(secondCharacterCurrentWeight);
            writer.WriteVarUhInt(secondCharacterMaxWeight);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            firstCharacterId = reader.ReadInt();
            firstCharacterCurrentWeight = reader.ReadVarUhInt();
            if (firstCharacterCurrentWeight < 0)
                throw new Exception("Forbidden value on firstCharacterCurrentWeight = " + firstCharacterCurrentWeight + ", it doesn't respect the following condition : firstCharacterCurrentWeight < 0");
            firstCharacterMaxWeight = reader.ReadVarUhInt();
            if (firstCharacterMaxWeight < 0)
                throw new Exception("Forbidden value on firstCharacterMaxWeight = " + firstCharacterMaxWeight + ", it doesn't respect the following condition : firstCharacterMaxWeight < 0");
            secondCharacterId = reader.ReadInt();
            secondCharacterCurrentWeight = reader.ReadVarUhInt();
            if (secondCharacterCurrentWeight < 0)
                throw new Exception("Forbidden value on secondCharacterCurrentWeight = " + secondCharacterCurrentWeight + ", it doesn't respect the following condition : secondCharacterCurrentWeight < 0");
            secondCharacterMaxWeight = reader.ReadVarUhInt();
            if (secondCharacterMaxWeight < 0)
                throw new Exception("Forbidden value on secondCharacterMaxWeight = " + secondCharacterMaxWeight + ", it doesn't respect the following condition : secondCharacterMaxWeight < 0");
            

}


}


}