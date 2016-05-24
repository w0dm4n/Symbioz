


















// Generated on 06/04/2015 18:45:07
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class SymbioticObjectErrorMessage : ObjectErrorMessage
{

public const ushort Id = 6526;
public override ushort MessageId
{
    get { return Id; }
}

public sbyte errorCode;
        

public SymbioticObjectErrorMessage()
{
}

public SymbioticObjectErrorMessage(sbyte reason, sbyte errorCode)
         : base(reason)
        {
            this.errorCode = errorCode;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteSByte(errorCode);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            errorCode = reader.ReadSByte();
            

}


}


}