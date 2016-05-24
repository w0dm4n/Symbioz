


















// Generated on 06/04/2015 18:44:29
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class JobBookSubscriptionMessage : Message
{

public const ushort Id = 6593;
public override ushort MessageId
{
    get { return Id; }
}

public bool addedOrDeleted;
        public sbyte jobId;
        

public JobBookSubscriptionMessage()
{
}

public JobBookSubscriptionMessage(bool addedOrDeleted, sbyte jobId)
        {
            this.addedOrDeleted = addedOrDeleted;
            this.jobId = jobId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteBoolean(addedOrDeleted);
            writer.WriteSByte(jobId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

addedOrDeleted = reader.ReadBoolean();
            jobId = reader.ReadSByte();
            if (jobId < 0)
                throw new Exception("Forbidden value on jobId = " + jobId + ", it doesn't respect the following condition : jobId < 0");
            

}


}


}