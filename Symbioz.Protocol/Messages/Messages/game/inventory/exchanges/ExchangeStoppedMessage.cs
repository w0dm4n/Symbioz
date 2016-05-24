


















// Generated on 06/04/2015 18:44:58
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ExchangeStoppedMessage : Message
{

public const ushort Id = 6589;
public override ushort MessageId
{
    get { return Id; }
}

public uint id;
        

public ExchangeStoppedMessage()
{
}

public ExchangeStoppedMessage(uint id)
        {
            this.id = id;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhInt(id);
            

}

public override void Deserialize(ICustomDataInput reader)
{

id = reader.ReadVarUhInt();
            if (id < 0)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < 0");
            

}


}


}