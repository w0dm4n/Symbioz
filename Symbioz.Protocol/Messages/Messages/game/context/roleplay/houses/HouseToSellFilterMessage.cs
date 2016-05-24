


















// Generated on 06/04/2015 18:44:29
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class HouseToSellFilterMessage : Message
{

public const ushort Id = 6137;
public override ushort MessageId
{
    get { return Id; }
}

public int areaId;
        public sbyte atLeastNbRoom;
        public sbyte atLeastNbChest;
        public ushort skillRequested;
        public uint maxPrice;
        

public HouseToSellFilterMessage()
{
}

public HouseToSellFilterMessage(int areaId, sbyte atLeastNbRoom, sbyte atLeastNbChest, ushort skillRequested, uint maxPrice)
        {
            this.areaId = areaId;
            this.atLeastNbRoom = atLeastNbRoom;
            this.atLeastNbChest = atLeastNbChest;
            this.skillRequested = skillRequested;
            this.maxPrice = maxPrice;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(areaId);
            writer.WriteSByte(atLeastNbRoom);
            writer.WriteSByte(atLeastNbChest);
            writer.WriteVarUhShort(skillRequested);
            writer.WriteVarUhInt(maxPrice);
            

}

public override void Deserialize(ICustomDataInput reader)
{

areaId = reader.ReadInt();
            atLeastNbRoom = reader.ReadSByte();
            if (atLeastNbRoom < 0)
                throw new Exception("Forbidden value on atLeastNbRoom = " + atLeastNbRoom + ", it doesn't respect the following condition : atLeastNbRoom < 0");
            atLeastNbChest = reader.ReadSByte();
            if (atLeastNbChest < 0)
                throw new Exception("Forbidden value on atLeastNbChest = " + atLeastNbChest + ", it doesn't respect the following condition : atLeastNbChest < 0");
            skillRequested = reader.ReadVarUhShort();
            if (skillRequested < 0)
                throw new Exception("Forbidden value on skillRequested = " + skillRequested + ", it doesn't respect the following condition : skillRequested < 0");
            maxPrice = reader.ReadVarUhInt();
            if (maxPrice < 0)
                throw new Exception("Forbidden value on maxPrice = " + maxPrice + ", it doesn't respect the following condition : maxPrice < 0");
            

}


}


}