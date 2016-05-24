


















// Generated on 06/04/2015 18:44:22
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameFightTurnResumeMessage : GameFightTurnStartMessage
{

public const ushort Id = 6307;
public override ushort MessageId
{
    get { return Id; }
}

public uint remainingTime;
        

public GameFightTurnResumeMessage()
{
}

public GameFightTurnResumeMessage(int id, uint waitTime, uint remainingTime)
         : base(id, waitTime)
        {
            this.remainingTime = remainingTime;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteVarUhInt(remainingTime);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            remainingTime = reader.ReadVarUhInt();
            if (remainingTime < 0)
                throw new Exception("Forbidden value on remainingTime = " + remainingTime + ", it doesn't respect the following condition : remainingTime < 0");
            

}


}


}