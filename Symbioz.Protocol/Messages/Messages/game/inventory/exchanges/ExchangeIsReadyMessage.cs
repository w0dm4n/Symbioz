


















// Generated on 06/04/2015 18:44:52
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ExchangeIsReadyMessage : Message
{

public const ushort Id = 5509;
public override ushort MessageId
{
    get { return Id; }
}

public uint id;
        public bool ready;
        

public ExchangeIsReadyMessage()
{
}

public ExchangeIsReadyMessage(uint id, bool ready)
        {
            this.id = id;
            this.ready = ready;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhInt(id);
            writer.WriteBoolean(ready);
            

}

public override void Deserialize(ICustomDataInput reader)
{

id = reader.ReadVarUhInt();
            if (id < 0)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < 0");
            ready = reader.ReadBoolean();
            

}


}


}