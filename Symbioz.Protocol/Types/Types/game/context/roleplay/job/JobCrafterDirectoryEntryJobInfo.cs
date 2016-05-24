


















// Generated on 06/04/2015 18:45:29
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class JobCrafterDirectoryEntryJobInfo
{

public const short Id = 195;
public virtual short TypeId
{
    get { return Id; }
}

public sbyte jobId;
        public byte jobLevel;
        public bool free;
        public byte minLevel;
        

public JobCrafterDirectoryEntryJobInfo()
{
}

public JobCrafterDirectoryEntryJobInfo(sbyte jobId, byte jobLevel, bool free, byte minLevel)
        {
            this.jobId = jobId;
            this.jobLevel = jobLevel;
            this.free = free;
            this.minLevel = minLevel;
        }
        

public virtual void Serialize(ICustomDataOutput writer)
{

writer.WriteSByte(jobId);
            writer.WriteByte(jobLevel);
            writer.WriteBoolean(free);
            writer.WriteByte(minLevel);
            

}

public virtual void Deserialize(ICustomDataInput reader)
{

jobId = reader.ReadSByte();
            if (jobId < 0)
                throw new Exception("Forbidden value on jobId = " + jobId + ", it doesn't respect the following condition : jobId < 0");
            jobLevel = reader.ReadByte();
            if ((jobLevel < 1) || (jobLevel > 200))
                throw new Exception("Forbidden value on jobLevel = " + jobLevel + ", it doesn't respect the following condition : (jobLevel < 1) || (jobLevel > 200)");
            free = reader.ReadBoolean();
            minLevel = reader.ReadByte();
            if ((minLevel < 0) || (minLevel > 255))
                throw new Exception("Forbidden value on minLevel = " + minLevel + ", it doesn't respect the following condition : (minLevel < 0) || (minLevel > 255)");
            

}


}


}