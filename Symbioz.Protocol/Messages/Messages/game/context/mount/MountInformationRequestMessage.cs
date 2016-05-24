


















// Generated on 06/04/2015 18:44:23
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class MountInformationRequestMessage : Message
{

public const ushort Id = 5972;
public override ushort MessageId
{
    get { return Id; }
}

public double id;
        public double time;
        

public MountInformationRequestMessage()
{
}

public MountInformationRequestMessage(double id, double time)
        {
            this.id = id;
            this.time = time;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteDouble(id);
            writer.WriteDouble(time);
            

}

public override void Deserialize(ICustomDataInput reader)
{

id = reader.ReadDouble();
            if ((id < -9.007199254740992E15) || (id > 9.007199254740992E15))
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : (id < -9.007199254740992E15) || (id > 9.007199254740992E15)");
            time = reader.ReadDouble();
            if ((time < -9.007199254740992E15) || (time > 9.007199254740992E15))
                throw new Exception("Forbidden value on time = " + time + ", it doesn't respect the following condition : (time < -9.007199254740992E15) || (time > 9.007199254740992E15)");
            

}


}


}