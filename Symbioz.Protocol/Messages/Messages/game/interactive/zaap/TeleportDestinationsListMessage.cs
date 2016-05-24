


















// Generated on 06/04/2015 18:44:49
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class TeleportDestinationsListMessage : Message
{

public const ushort Id = 5960;
public override ushort MessageId
{
    get { return Id; }
}

public sbyte teleporterType;
        public IEnumerable<int> mapIds;
        public IEnumerable<ushort> subAreaIds;
        public IEnumerable<ushort> costs;
        public IEnumerable<sbyte> destTeleporterType;
        

public TeleportDestinationsListMessage()
{
}

public TeleportDestinationsListMessage(sbyte teleporterType, IEnumerable<int> mapIds, IEnumerable<ushort> subAreaIds, IEnumerable<ushort> costs, IEnumerable<sbyte> destTeleporterType)
        {
            this.teleporterType = teleporterType;
            this.mapIds = mapIds;
            this.subAreaIds = subAreaIds;
            this.costs = costs;
            this.destTeleporterType = destTeleporterType;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteSByte(teleporterType);
            writer.WriteUShort((ushort)mapIds.Count());
            foreach (var entry in mapIds)
            {
                 writer.WriteInt(entry);
            }
            writer.WriteUShort((ushort)subAreaIds.Count());
            foreach (var entry in subAreaIds)
            {
                 writer.WriteVarUhShort(entry);
            }
            writer.WriteUShort((ushort)costs.Count());
            foreach (var entry in costs)
            {
                 writer.WriteVarUhShort(entry);
            }
            writer.WriteUShort((ushort)destTeleporterType.Count());
            foreach (var entry in destTeleporterType)
            {
                 writer.WriteSByte(entry);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

teleporterType = reader.ReadSByte();
            if (teleporterType < 0)
                throw new Exception("Forbidden value on teleporterType = " + teleporterType + ", it doesn't respect the following condition : teleporterType < 0");
            var limit = reader.ReadUShort();
            mapIds = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 (mapIds as int[])[i] = reader.ReadInt();
            }
            limit = reader.ReadUShort();
            subAreaIds = new ushort[limit];
            for (int i = 0; i < limit; i++)
            {
                 (subAreaIds as ushort[])[i] = reader.ReadVarUhShort();
            }
            limit = reader.ReadUShort();
            costs = new ushort[limit];
            for (int i = 0; i < limit; i++)
            {
                 (costs as ushort[])[i] = reader.ReadVarUhShort();
            }
            limit = reader.ReadUShort();
            destTeleporterType = new sbyte[limit];
            for (int i = 0; i < limit; i++)
            {
                 (destTeleporterType as sbyte[])[i] = reader.ReadSByte();
            }
            

}


}


}