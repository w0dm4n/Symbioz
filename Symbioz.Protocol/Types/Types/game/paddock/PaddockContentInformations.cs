


















// Generated on 06/04/2015 18:45:36
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class PaddockContentInformations : PaddockInformations
{

public const short Id = 183;
public override short TypeId
{
    get { return Id; }
}

public int paddockId;
        public short worldX;
        public short worldY;
        public int mapId;
        public ushort subAreaId;
        public bool abandonned;
        public IEnumerable<Types.MountInformationsForPaddock> mountsInformations;
        

public PaddockContentInformations()
{
}

public PaddockContentInformations(ushort maxOutdoorMount, ushort maxItems, int paddockId, short worldX, short worldY, int mapId, ushort subAreaId, bool abandonned, IEnumerable<Types.MountInformationsForPaddock> mountsInformations)
         : base(maxOutdoorMount, maxItems)
        {
            this.paddockId = paddockId;
            this.worldX = worldX;
            this.worldY = worldY;
            this.mapId = mapId;
            this.subAreaId = subAreaId;
            this.abandonned = abandonned;
            this.mountsInformations = mountsInformations;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteInt(paddockId);
            writer.WriteShort(worldX);
            writer.WriteShort(worldY);
            writer.WriteInt(mapId);
            writer.WriteVarUhShort(subAreaId);
            writer.WriteBoolean(abandonned);
            writer.WriteUShort((ushort)mountsInformations.Count());
            foreach (var entry in mountsInformations)
            {
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            paddockId = reader.ReadInt();
            worldX = reader.ReadShort();
            if ((worldX < -255) || (worldX > 255))
                throw new Exception("Forbidden value on worldX = " + worldX + ", it doesn't respect the following condition : (worldX < -255) || (worldX > 255)");
            worldY = reader.ReadShort();
            if ((worldY < -255) || (worldY > 255))
                throw new Exception("Forbidden value on worldY = " + worldY + ", it doesn't respect the following condition : (worldY < -255) || (worldY > 255)");
            mapId = reader.ReadInt();
            subAreaId = reader.ReadVarUhShort();
            if (subAreaId < 0)
                throw new Exception("Forbidden value on subAreaId = " + subAreaId + ", it doesn't respect the following condition : subAreaId < 0");
            abandonned = reader.ReadBoolean();
            var limit = reader.ReadUShort();
            mountsInformations = new Types.MountInformationsForPaddock[limit];
            for (int i = 0; i < limit; i++)
            {
                 (mountsInformations as Types.MountInformationsForPaddock[])[i] = new Types.MountInformationsForPaddock();
                 (mountsInformations as Types.MountInformationsForPaddock[])[i].Deserialize(reader);
            }
            

}


}


}