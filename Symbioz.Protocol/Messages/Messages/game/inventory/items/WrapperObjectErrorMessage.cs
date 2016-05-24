


















// Generated on 06/04/2015 18:45:07
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class WrapperObjectErrorMessage : SymbioticObjectErrorMessage
{

public const ushort Id = 6529;
public override ushort MessageId
{
    get { return Id; }
}



public WrapperObjectErrorMessage()
{
}

public WrapperObjectErrorMessage(sbyte reason, sbyte errorCode)
         : base(reason, errorCode)
        {
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            

}


}


}