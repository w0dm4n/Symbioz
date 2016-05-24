


















// Generated on 06/04/2015 18:44:52
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ExchangeItemAutoCraftRemainingMessage : Message
{

public const ushort Id = 6015;
public override ushort MessageId
{
    get { return Id; }
}

public uint count;
        

public ExchangeItemAutoCraftRemainingMessage()
{
}

public ExchangeItemAutoCraftRemainingMessage(uint count)
        {
            this.count = count;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhInt(count);
            

}

public override void Deserialize(ICustomDataInput reader)
{

count = reader.ReadVarUhInt();
            if (count < 0)
                throw new Exception("Forbidden value on count = " + count + ", it doesn't respect the following condition : count < 0");
            

}


}


}