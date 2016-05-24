


















// Generated on 06/04/2015 18:45:32
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class BidExchangerObjectInfo
{

public const short Id = 122;
public virtual short TypeId
{
    get { return Id; }
}

public uint objectUID;
        public IEnumerable<Types.ObjectEffect> effects;
        public IEnumerable<int> prices;
        

public BidExchangerObjectInfo()
{
}

public BidExchangerObjectInfo(uint objectUID, IEnumerable<Types.ObjectEffect> effects, IEnumerable<int> prices)
        {
            this.objectUID = objectUID;
            this.effects = effects;
            this.prices = prices;
        }
        

public virtual void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhInt(objectUID);
            writer.WriteUShort((ushort)effects.Count());
            foreach (var entry in effects)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)prices.Count());
            foreach (var entry in prices)
            {
                 writer.WriteInt(entry);
            }
            

}

public virtual void Deserialize(ICustomDataInput reader)
{

objectUID = reader.ReadVarUhInt();
            if (objectUID < 0)
                throw new Exception("Forbidden value on objectUID = " + objectUID + ", it doesn't respect the following condition : objectUID < 0");
            var limit = reader.ReadUShort();
            effects = new Types.ObjectEffect[limit];
            for (int i = 0; i < limit; i++)
            {
                 (effects as Types.ObjectEffect[])[i] = Types.ProtocolTypeManager.GetInstance<Types.ObjectEffect>(reader.ReadShort());
                 (effects as Types.ObjectEffect[])[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            prices = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 (prices as int[])[i] = reader.ReadInt();
            }
            

}


}


}