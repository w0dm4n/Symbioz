


















// Generated on 06/04/2015 18:44:25
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class MapFightCountMessage : Message
{

public const ushort Id = 210;
public override ushort MessageId
{
    get { return Id; }
}

public ushort fightCount;
        

public MapFightCountMessage()
{
}

public MapFightCountMessage(ushort fightCount)
        {
            this.fightCount = fightCount;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhShort(fightCount);
            

}

public override void Deserialize(ICustomDataInput reader)
{

fightCount = reader.ReadVarUhShort();
            if (fightCount < 0)
                throw new Exception("Forbidden value on fightCount = " + fightCount + ", it doesn't respect the following condition : fightCount < 0");
            

}


}


}