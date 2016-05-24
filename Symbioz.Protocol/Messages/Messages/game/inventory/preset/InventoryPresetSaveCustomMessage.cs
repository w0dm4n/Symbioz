


















// Generated on 06/04/2015 18:45:07
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class InventoryPresetSaveCustomMessage : Message
{

public const ushort Id = 6329;
public override ushort MessageId
{
    get { return Id; }
}

public sbyte presetId;
        public sbyte symbolId;
        public IEnumerable<byte> itemsPositions;
        public IEnumerable<uint> itemsUids;
        

public InventoryPresetSaveCustomMessage()
{
}

public InventoryPresetSaveCustomMessage(sbyte presetId, sbyte symbolId, IEnumerable<byte> itemsPositions, IEnumerable<uint> itemsUids)
        {
            this.presetId = presetId;
            this.symbolId = symbolId;
            this.itemsPositions = itemsPositions;
            this.itemsUids = itemsUids;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteSByte(presetId);
            writer.WriteSByte(symbolId);
            writer.WriteUShort((ushort)itemsPositions.Count());
            foreach (var entry in itemsPositions)
            {
                 writer.WriteByte(entry);
            }
            writer.WriteUShort((ushort)itemsUids.Count());
            foreach (var entry in itemsUids)
            {
                 writer.WriteVarUhInt(entry);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

presetId = reader.ReadSByte();
            if (presetId < 0)
                throw new Exception("Forbidden value on presetId = " + presetId + ", it doesn't respect the following condition : presetId < 0");
            symbolId = reader.ReadSByte();
            if (symbolId < 0)
                throw new Exception("Forbidden value on symbolId = " + symbolId + ", it doesn't respect the following condition : symbolId < 0");
            var limit = reader.ReadUShort();
            itemsPositions = new byte[limit];
            for (int i = 0; i < limit; i++)
            {
                 (itemsPositions as byte[])[i] = reader.ReadByte();
            }
            limit = reader.ReadUShort();
            itemsUids = new uint[limit];
            for (int i = 0; i < limit; i++)
            {
                 (itemsUids as uint[])[i] = reader.ReadVarUhInt();
            }
            

}


}


}