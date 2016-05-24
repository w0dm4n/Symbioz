


















// Generated on 06/04/2015 18:44:56
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ExchangeStartedMessage : Message
{

public const ushort Id = 5512;
public override ushort MessageId
{
    get { return Id; }
}

public sbyte exchangeType;
        

public ExchangeStartedMessage()
{
}

public ExchangeStartedMessage(sbyte exchangeType)
        {
            this.exchangeType = exchangeType;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteSByte(exchangeType);
            

}

public override void Deserialize(ICustomDataInput reader)
{

exchangeType = reader.ReadSByte();
            

}


}


}