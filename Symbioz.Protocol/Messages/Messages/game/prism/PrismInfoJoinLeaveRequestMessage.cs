


















// Generated on 06/04/2015 18:45:11
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class PrismInfoJoinLeaveRequestMessage : Message
{

public const ushort Id = 5844;
public override ushort MessageId
{
    get { return Id; }
}

public bool join;
        

public PrismInfoJoinLeaveRequestMessage()
{
}

public PrismInfoJoinLeaveRequestMessage(bool join)
        {
            this.join = join;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteBoolean(join);
            

}

public override void Deserialize(ICustomDataInput reader)
{

join = reader.ReadBoolean();
            

}


}


}