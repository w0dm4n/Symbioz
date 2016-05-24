


















// Generated on 06/04/2015 18:45:05
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class MimicryObjectErrorMessage : SymbioticObjectErrorMessage
{

public const ushort Id = 6461;
public override ushort MessageId
{
    get { return Id; }
}

public bool preview;
        

public MimicryObjectErrorMessage()
{
}

public MimicryObjectErrorMessage(sbyte reason, sbyte errorCode, bool preview)
         : base(reason, errorCode)
        {
            this.preview = preview;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteBoolean(preview);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            preview = reader.ReadBoolean();
            

}


}


}