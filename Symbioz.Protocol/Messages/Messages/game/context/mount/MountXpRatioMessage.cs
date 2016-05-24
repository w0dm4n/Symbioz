


















// Generated on 06/04/2015 18:44:24
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class MountXpRatioMessage : Message
{

public const ushort Id = 5970;
public override ushort MessageId
{
    get { return Id; }
}

public sbyte ratio;
        

public MountXpRatioMessage()
{
}

public MountXpRatioMessage(sbyte ratio)
        {
            this.ratio = ratio;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteSByte(ratio);
            

}

public override void Deserialize(ICustomDataInput reader)
{

ratio = reader.ReadSByte();
            if (ratio < 0)
                throw new Exception("Forbidden value on ratio = " + ratio + ", it doesn't respect the following condition : ratio < 0");
            

}


}


}