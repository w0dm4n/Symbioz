


















// Generated on 06/04/2015 18:44:52
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ExchangeCraftResultMagicWithObjectDescMessage : ExchangeCraftResultWithObjectDescMessage
{

public const ushort Id = 6188;
public override ushort MessageId
{
    get { return Id; }
}

public sbyte magicPoolStatus;
        

public ExchangeCraftResultMagicWithObjectDescMessage()
{
}

public ExchangeCraftResultMagicWithObjectDescMessage(sbyte craftResult, Types.ObjectItemNotInContainer objectInfo, sbyte magicPoolStatus)
         : base(craftResult, objectInfo)
        {
            this.magicPoolStatus = magicPoolStatus;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteSByte(magicPoolStatus);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            magicPoolStatus = reader.ReadSByte();
            

}


}


}