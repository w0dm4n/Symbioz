


















// Generated on 06/04/2015 18:44:12
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class BasicWhoAmIRequestMessage : Message
{

public const ushort Id = 5664;
public override ushort MessageId
{
    get { return Id; }
}

public bool verbose;
        

public BasicWhoAmIRequestMessage()
{
}

public BasicWhoAmIRequestMessage(bool verbose)
        {
            this.verbose = verbose;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteBoolean(verbose);
            

}

public override void Deserialize(ICustomDataInput reader)
{

verbose = reader.ReadBoolean();
            

}


}


}