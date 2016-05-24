


















// Generated on 06/04/2015 18:44:54
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ExchangePlayerMultiCraftRequestMessage : ExchangeRequestMessage
{

public const ushort Id = 5784;
public override ushort MessageId
{
    get { return Id; }
}

public uint target;
        public uint skillId;
        

public ExchangePlayerMultiCraftRequestMessage()
{
}

public ExchangePlayerMultiCraftRequestMessage(sbyte exchangeType, uint target, uint skillId)
         : base(exchangeType)
        {
            this.target = target;
            this.skillId = skillId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteVarUhInt(target);
            writer.WriteVarUhInt(skillId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            target = reader.ReadVarUhInt();
            if (target < 0)
                throw new Exception("Forbidden value on target = " + target + ", it doesn't respect the following condition : target < 0");
            skillId = reader.ReadVarUhInt();
            if (skillId < 0)
                throw new Exception("Forbidden value on skillId = " + skillId + ", it doesn't respect the following condition : skillId < 0");
            

}


}


}