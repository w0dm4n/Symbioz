


















// Generated on 06/04/2015 18:44:12
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class BasicTimeMessage : Message
{

public const ushort Id = 175;
public override ushort MessageId
{
    get { return Id; }
}

public double timestamp;
        public short timezoneOffset;
        

public BasicTimeMessage()
{
}

public BasicTimeMessage(double timestamp, short timezoneOffset)
        {
            this.timestamp = timestamp;
            this.timezoneOffset = timezoneOffset;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteDouble(timestamp);
            writer.WriteShort(timezoneOffset);
            

}

public override void Deserialize(ICustomDataInput reader)
{

timestamp = reader.ReadDouble();
            if ((timestamp < 0) || (timestamp > 9.007199254740992E15))
                throw new Exception("Forbidden value on timestamp = " + timestamp + ", it doesn't respect the following condition : (timestamp < 0) || (timestamp > 9.007199254740992E15)");
            timezoneOffset = reader.ReadShort();
            

}


}


}