


















// Generated on 06/04/2015 18:44:54
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ExchangePlayerRequestMessage : ExchangeRequestMessage
{

public const ushort Id = 5773;
public override ushort MessageId
{
    get { return Id; }
}

public uint target;
        

public ExchangePlayerRequestMessage()
{
}

public ExchangePlayerRequestMessage(sbyte exchangeType, uint target)
         : base(exchangeType)
        {
            this.target = target;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteVarUhInt(target);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            target = reader.ReadVarUhInt();
            if (target < 0)
                throw new Exception("Forbidden value on target = " + target + ", it doesn't respect the following condition : target < 0");
            

}


}


}