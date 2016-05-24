


















// Generated on 06/04/2015 18:44:12
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class CurrentServerStatusUpdateMessage : Message
{

public const ushort Id = 6525;
public override ushort MessageId
{
    get { return Id; }
}

public sbyte status;
        

public CurrentServerStatusUpdateMessage()
{
}

public CurrentServerStatusUpdateMessage(sbyte status)
        {
            this.status = status;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteSByte(status);
            

}

public override void Deserialize(ICustomDataInput reader)
{

status = reader.ReadSByte();
            if (status < 0)
                throw new Exception("Forbidden value on status = " + status + ", it doesn't respect the following condition : status < 0");
            

}


}


}