


















// Generated on 06/04/2015 18:44:29
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class JobCrafterDirectoryRemoveMessage : Message
{

public const ushort Id = 5653;
public override ushort MessageId
{
    get { return Id; }
}

public sbyte jobId;
        public uint playerId;
        

public JobCrafterDirectoryRemoveMessage()
{
}

public JobCrafterDirectoryRemoveMessage(sbyte jobId, uint playerId)
        {
            this.jobId = jobId;
            this.playerId = playerId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteSByte(jobId);
            writer.WriteVarUhInt(playerId);
            

}

public override void Deserialize(ICustomDataInput reader)
{

jobId = reader.ReadSByte();
            if (jobId < 0)
                throw new Exception("Forbidden value on jobId = " + jobId + ", it doesn't respect the following condition : jobId < 0");
            playerId = reader.ReadVarUhInt();
            if (playerId < 0)
                throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0");
            

}


}


}