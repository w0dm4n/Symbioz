


















// Generated on 06/04/2015 18:44:03
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class IdentificationFailedForBadVersionMessage : IdentificationFailedMessage
{

public const ushort Id = 21;
public override ushort MessageId
{
    get { return Id; }
}

public Types.Version requiredVersion;
        

public IdentificationFailedForBadVersionMessage()
{
}

public IdentificationFailedForBadVersionMessage(sbyte reason, Types.Version requiredVersion)
         : base(reason)
        {
            this.requiredVersion = requiredVersion;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            requiredVersion.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            requiredVersion = new Types.Version();
            requiredVersion.Deserialize(reader);
            

}


}


}