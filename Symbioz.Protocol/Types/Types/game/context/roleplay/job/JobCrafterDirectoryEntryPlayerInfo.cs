


















// Generated on 06/04/2015 18:45:29
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class JobCrafterDirectoryEntryPlayerInfo
{

public const short Id = 194;
public virtual short TypeId
{
    get { return Id; }
}

public uint playerId;
        public string playerName;
        public sbyte alignmentSide;
        public sbyte breed;
        public bool sex;
        public bool isInWorkshop;
        public short worldX;
        public short worldY;
        public int mapId;
        public ushort subAreaId;
        public Types.PlayerStatus status;
        

public JobCrafterDirectoryEntryPlayerInfo()
{
}

public JobCrafterDirectoryEntryPlayerInfo(uint playerId, string playerName, sbyte alignmentSide, sbyte breed, bool sex, bool isInWorkshop, short worldX, short worldY, int mapId, ushort subAreaId, Types.PlayerStatus status)
        {
            this.playerId = playerId;
            this.playerName = playerName;
            this.alignmentSide = alignmentSide;
            this.breed = breed;
            this.sex = sex;
            this.isInWorkshop = isInWorkshop;
            this.worldX = worldX;
            this.worldY = worldY;
            this.mapId = mapId;
            this.subAreaId = subAreaId;
            this.status = status;
        }
        

public virtual void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhInt(playerId);
            writer.WriteUTF(playerName);
            writer.WriteSByte(alignmentSide);
            writer.WriteSByte(breed);
            writer.WriteBoolean(sex);
            writer.WriteBoolean(isInWorkshop);
            writer.WriteShort(worldX);
            writer.WriteShort(worldY);
            writer.WriteInt(mapId);
            writer.WriteVarUhShort(subAreaId);
            writer.WriteShort(status.TypeId);
            status.Serialize(writer);
            

}

public virtual void Deserialize(ICustomDataInput reader)
{

playerId = reader.ReadVarUhInt();
            if (playerId < 0)
                throw new Exception("Forbidden value on playerId = " + playerId + ", it doesn't respect the following condition : playerId < 0");
            playerName = reader.ReadUTF();
            alignmentSide = reader.ReadSByte();
            breed = reader.ReadSByte();
            if ((breed < (byte)Enums.PlayableBreedEnum.Feca) || (breed > (byte)Enums.PlayableBreedEnum.Eliotrope))
                throw new Exception("Forbidden value on breed = " + breed + ", it doesn't respect the following condition : (breed < (byte)Enums.PlayableBreedEnum.Feca) || (breed > (byte)Enums.PlayableBreedEnum.Eliotrope)");
            sex = reader.ReadBoolean();
            isInWorkshop = reader.ReadBoolean();
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
            status = Types.ProtocolTypeManager.GetInstance<Types.PlayerStatus>(reader.ReadShort());
            status.Deserialize(reader);
            

}


}


}