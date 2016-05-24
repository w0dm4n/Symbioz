


















// Generated on 06/04/2015 18:45:35
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class EntityLook
{
    public EntityLook Clone()
    {
        return new EntityLook(this.bonesId, this.skins, this.indexedColors, this.scales, this.subentities);
    }
public const short Id = 55;
public virtual short TypeId
{
    get { return Id; }
}

public ushort bonesId;
        public List<ushort> skins;
        public List<int> indexedColors;
        public List<short> scales;
        public List<Types.SubEntity> subentities;
        

public EntityLook()
{
}

public EntityLook(ushort bonesId, List<ushort> skins, List<int> indexedColors, List<short> scales, List<Types.SubEntity> subentities)
        {
            this.bonesId = bonesId;
            this.skins = skins;
            this.indexedColors = indexedColors;
            this.scales = scales;
            this.subentities = subentities;
        }
        

public virtual void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhShort(bonesId);
            writer.WriteUShort((ushort)skins.Count());
            foreach (var entry in skins)
            {
                 writer.WriteVarUhShort(entry);
            }
            writer.WriteUShort((ushort)indexedColors.Count());
            foreach (var entry in indexedColors)
            {
                 writer.WriteInt(entry);
            }
            writer.WriteUShort((ushort)scales.Count());
            foreach (var entry in scales)
            {
                 writer.WriteVarShort(entry);
            }
            writer.WriteUShort((ushort)subentities.Count());
            foreach (var entry in subentities)
            {
                 entry.Serialize(writer);
            }
            

}

public virtual void Deserialize(ICustomDataInput reader)
{

bonesId = reader.ReadVarUhShort();
            if (bonesId < 0)
                throw new Exception("Forbidden value on bonesId = " + bonesId + ", it doesn't respect the following condition : bonesId < 0");
            var limit = reader.ReadUShort();
            skins = new List<ushort>();
            for (int i = 0; i < limit; i++)
            {
                skins.Add(reader.ReadVarUhShort());
            }
            limit = reader.ReadUShort();
            indexedColors = new List<int>();
            for (int i = 0; i < limit; i++)
            {
                indexedColors.Add(reader.ReadInt());
            }
            limit = reader.ReadUShort();
            scales = new List<short>();
            for (int i = 0; i < limit; i++)
            {
                scales.Add(reader.ReadVarShort());
            }
            limit = reader.ReadUShort();
            subentities = new List<SubEntity>();
            for (int i = 0; i < limit; i++)
            {
                 var subentity = new Types.SubEntity();
                subentity.Deserialize(reader);
                subentities.Add(subentity);
            }
            

}


}


}