


















// Generated on 06/04/2015 18:45:29
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class DecraftedItemStackInfo
{

public const short Id = 481;
public virtual short TypeId
{
    get { return Id; }
}

public uint objectUID;
        public float bonusMin;
        public float bonusMax;
        public IEnumerable<ushort> runesId;
        public IEnumerable<uint> runesQty;
        

public DecraftedItemStackInfo()
{
}

public DecraftedItemStackInfo(uint objectUID, float bonusMin, float bonusMax, IEnumerable<ushort> runesId, IEnumerable<uint> runesQty)
        {
            this.objectUID = objectUID;
            this.bonusMin = bonusMin;
            this.bonusMax = bonusMax;
            this.runesId = runesId;
            this.runesQty = runesQty;
        }
        

public virtual void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhInt(objectUID);
            writer.WriteFloat(bonusMin);
            writer.WriteFloat(bonusMax);
            writer.WriteUShort((ushort)runesId.Count());
            foreach (var entry in runesId)
            {
                 writer.WriteVarUhShort(entry);
            }
            writer.WriteUShort((ushort)runesQty.Count());
            foreach (var entry in runesQty)
            {
                 writer.WriteVarUhInt(entry);
            }
            

}

public virtual void Deserialize(ICustomDataInput reader)
{

objectUID = reader.ReadVarUhInt();
            if (objectUID < 0)
                throw new Exception("Forbidden value on objectUID = " + objectUID + ", it doesn't respect the following condition : objectUID < 0");
            bonusMin = reader.ReadFloat();
            bonusMax = reader.ReadFloat();
            var limit = reader.ReadUShort();
            runesId = new ushort[limit];
            for (int i = 0; i < limit; i++)
            {
                 (runesId as ushort[])[i] = reader.ReadVarUhShort();
            }
            limit = reader.ReadUShort();
            runesQty = new uint[limit];
            for (int i = 0; i < limit; i++)
            {
                 (runesQty as uint[])[i] = reader.ReadVarUhInt();
            }
            

}


}


}