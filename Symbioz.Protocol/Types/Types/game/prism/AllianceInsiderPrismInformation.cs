


















// Generated on 06/04/2015 18:45:36
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class AllianceInsiderPrismInformation : PrismInformation
{

public const short Id = 431;
public override short TypeId
{
    get { return Id; }
}

public int lastTimeSlotModificationDate;
        public uint lastTimeSlotModificationAuthorGuildId;
        public uint lastTimeSlotModificationAuthorId;
        public string lastTimeSlotModificationAuthorName;
        public IEnumerable<uint> modulesItemIds;
        

public AllianceInsiderPrismInformation()
{
}

public AllianceInsiderPrismInformation(sbyte typeId, sbyte state, int nextVulnerabilityDate, int placementDate, uint rewardTokenCount, int lastTimeSlotModificationDate, uint lastTimeSlotModificationAuthorGuildId, uint lastTimeSlotModificationAuthorId, string lastTimeSlotModificationAuthorName, IEnumerable<uint> modulesItemIds)
         : base(typeId, state, nextVulnerabilityDate, placementDate, rewardTokenCount)
        {
            this.lastTimeSlotModificationDate = lastTimeSlotModificationDate;
            this.lastTimeSlotModificationAuthorGuildId = lastTimeSlotModificationAuthorGuildId;
            this.lastTimeSlotModificationAuthorId = lastTimeSlotModificationAuthorId;
            this.lastTimeSlotModificationAuthorName = lastTimeSlotModificationAuthorName;
            this.modulesItemIds = modulesItemIds;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteInt(lastTimeSlotModificationDate);
            writer.WriteVarUhInt(lastTimeSlotModificationAuthorGuildId);
            writer.WriteVarUhInt(lastTimeSlotModificationAuthorId);
            writer.WriteUTF(lastTimeSlotModificationAuthorName);
            writer.WriteUShort((ushort)modulesItemIds.Count());
            foreach (var entry in modulesItemIds)
            {
                 writer.WriteVarUhInt(entry);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            lastTimeSlotModificationDate = reader.ReadInt();
            if (lastTimeSlotModificationDate < 0)
                throw new Exception("Forbidden value on lastTimeSlotModificationDate = " + lastTimeSlotModificationDate + ", it doesn't respect the following condition : lastTimeSlotModificationDate < 0");
            lastTimeSlotModificationAuthorGuildId = reader.ReadVarUhInt();
            if (lastTimeSlotModificationAuthorGuildId < 0)
                throw new Exception("Forbidden value on lastTimeSlotModificationAuthorGuildId = " + lastTimeSlotModificationAuthorGuildId + ", it doesn't respect the following condition : lastTimeSlotModificationAuthorGuildId < 0");
            lastTimeSlotModificationAuthorId = reader.ReadVarUhInt();
            if (lastTimeSlotModificationAuthorId < 0)
                throw new Exception("Forbidden value on lastTimeSlotModificationAuthorId = " + lastTimeSlotModificationAuthorId + ", it doesn't respect the following condition : lastTimeSlotModificationAuthorId < 0");
            lastTimeSlotModificationAuthorName = reader.ReadUTF();
            var limit = reader.ReadUShort();
            modulesItemIds = new uint[limit];
            for (int i = 0; i < limit; i++)
            {
                 (modulesItemIds as uint[])[i] = reader.ReadVarUhInt();
            }
            

}


}


}