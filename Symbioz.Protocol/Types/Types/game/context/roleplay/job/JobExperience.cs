


















// Generated on 06/04/2015 18:45:30
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class JobExperience
{

public const short Id = 98;
public virtual short TypeId
{
    get { return Id; }
}

public sbyte jobId;
        public byte jobLevel;
        public ulong jobXP;
        public ulong jobXpLevelFloor;
        public ulong jobXpNextLevelFloor;
        

public JobExperience()
{
}

public JobExperience(sbyte jobId, byte jobLevel, ulong jobXP, ulong jobXpLevelFloor, ulong jobXpNextLevelFloor)
        {
            this.jobId = jobId;
            this.jobLevel = jobLevel;
            this.jobXP = jobXP;
            this.jobXpLevelFloor = jobXpLevelFloor;
            this.jobXpNextLevelFloor = jobXpNextLevelFloor;
        }
        

public virtual void Serialize(ICustomDataOutput writer)
{

writer.WriteSByte(jobId);
            writer.WriteByte(jobLevel);
            writer.WriteVarUhLong(jobXP);
            writer.WriteVarUhLong(jobXpLevelFloor);
            writer.WriteVarUhLong(jobXpNextLevelFloor);
            

}

public virtual void Deserialize(ICustomDataInput reader)
{

jobId = reader.ReadSByte();
            if (jobId < 0)
                throw new Exception("Forbidden value on jobId = " + jobId + ", it doesn't respect the following condition : jobId < 0");
            jobLevel = reader.ReadByte();
            if ((jobLevel < 0) || (jobLevel > 255))
                throw new Exception("Forbidden value on jobLevel = " + jobLevel + ", it doesn't respect the following condition : (jobLevel < 0) || (jobLevel > 255)");
            jobXP = reader.ReadVarUhLong();
            if ((jobXP < 0) || (jobXP > 9.007199254740992E15))
                throw new Exception("Forbidden value on jobXP = " + jobXP + ", it doesn't respect the following condition : (jobXP < 0) || (jobXP > 9.007199254740992E15)");
            jobXpLevelFloor = reader.ReadVarUhLong();
            if ((jobXpLevelFloor < 0) || (jobXpLevelFloor > 9.007199254740992E15))
                throw new Exception("Forbidden value on jobXpLevelFloor = " + jobXpLevelFloor + ", it doesn't respect the following condition : (jobXpLevelFloor < 0) || (jobXpLevelFloor > 9.007199254740992E15)");
            jobXpNextLevelFloor = reader.ReadVarUhLong();
            if ((jobXpNextLevelFloor < 0) || (jobXpNextLevelFloor > 9.007199254740992E15))
                throw new Exception("Forbidden value on jobXpNextLevelFloor = " + jobXpNextLevelFloor + ", it doesn't respect the following condition : (jobXpNextLevelFloor < 0) || (jobXpNextLevelFloor > 9.007199254740992E15)");
            

}


}


}