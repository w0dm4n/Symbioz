


















// Generated on 06/04/2015 18:44:15
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class LifePointsRegenBeginMessage : Message
{

public const ushort Id = 5684;
public override ushort MessageId
{
    get { return Id; }
}

public byte regenRate;
        

public LifePointsRegenBeginMessage()
{
}

public LifePointsRegenBeginMessage(byte regenRate)
        {
            this.regenRate = regenRate;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteByte(regenRate);
            

}

public override void Deserialize(ICustomDataInput reader)
{

regenRate = reader.ReadByte();
            if ((regenRate < 0) || (regenRate > 255))
                throw new Exception("Forbidden value on regenRate = " + regenRate + ", it doesn't respect the following condition : (regenRate < 0) || (regenRate > 255)");
            

}


}


}