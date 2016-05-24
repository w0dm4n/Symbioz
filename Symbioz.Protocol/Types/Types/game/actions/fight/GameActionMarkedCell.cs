


















// Generated on 06/04/2015 18:45:21
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class GameActionMarkedCell
{

public const short Id = 85;
public virtual short TypeId
{
    get { return Id; }
}

public ushort cellId;
        public sbyte zoneSize;
        public int cellColor;
        public sbyte cellsType;
        

public GameActionMarkedCell()
{
}

public GameActionMarkedCell(ushort cellId, sbyte zoneSize, int cellColor, sbyte cellsType)
        {
            this.cellId = cellId;
            this.zoneSize = zoneSize;
            this.cellColor = cellColor;
            this.cellsType = cellsType;
        }
        

public virtual void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhShort(cellId);
            writer.WriteSByte(zoneSize);
            writer.WriteInt(cellColor);
            writer.WriteSByte(cellsType);
            

}

public virtual void Deserialize(ICustomDataInput reader)
{

cellId = reader.ReadVarUhShort();
            if ((cellId < 0) || (cellId > 559))
                throw new Exception("Forbidden value on cellId = " + cellId + ", it doesn't respect the following condition : (cellId < 0) || (cellId > 559)");
            zoneSize = reader.ReadSByte();
            cellColor = reader.ReadInt();
            cellsType = reader.ReadSByte();
            

}


}


}