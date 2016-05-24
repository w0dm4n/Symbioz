


















// Generated on 06/04/2015 18:45:13
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class CinematicMessage : Message
{

public const ushort Id = 6053;
public override ushort MessageId
{
    get { return Id; }
}

public ushort cinematicId;
        

public CinematicMessage()
{
}

public CinematicMessage(ushort cinematicId)
        {
            this.cinematicId = cinematicId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhShort(cinematicId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

cinematicId = reader.ReadVarUhShort();
            if (cinematicId < 0)
                throw new Exception("Forbidden value on cinematicId = " + cinematicId + ", it doesn't respect the following condition : cinematicId < 0");
            

}


}


}