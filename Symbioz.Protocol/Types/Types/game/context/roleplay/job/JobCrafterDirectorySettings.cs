


















// Generated on 06/04/2015 18:45:29
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class JobCrafterDirectorySettings
{

public const short Id = 97;
public virtual short TypeId
{
    get { return Id; }
}

public sbyte jobId;
        public byte minLevel;
        public bool free;
        

public JobCrafterDirectorySettings()
{
}

public JobCrafterDirectorySettings(sbyte jobId, byte minLevel, bool free)
        {
            this.jobId = jobId;
            this.minLevel = minLevel;
            this.free = free;
        }
        

public virtual void Serialize(ICustomDataOutput writer)
{

writer.WriteSByte(jobId);
            writer.WriteByte(minLevel);
            writer.WriteBoolean(free);
            

}

public virtual void Deserialize(ICustomDataInput reader)
{

jobId = reader.ReadSByte();
            if (jobId < 0)
                throw new Exception("Forbidden value on jobId = " + jobId + ", it doesn't respect the following condition : jobId < 0");
            minLevel = reader.ReadByte();
            if ((minLevel < 0) || (minLevel > 255))
                throw new Exception("Forbidden value on minLevel = " + minLevel + ", it doesn't respect the following condition : (minLevel < 0) || (minLevel > 255)");
            free = reader.ReadBoolean();
            

}


}


}