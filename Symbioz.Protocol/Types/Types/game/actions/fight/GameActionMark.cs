


















// Generated on 06/04/2015 18:45:21
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class GameActionMark
{

public const short Id = 351;
public virtual short TypeId
{
    get { return Id; }
}

public int markAuthorId;
        public sbyte markTeamId;
        public int markSpellId;
        public sbyte markSpellLevel;
        public short markId;
        public sbyte markType;
        public short markimpactCell;
        public IEnumerable<Types.GameActionMarkedCell> cells;
        public bool active;
        

public GameActionMark()
{
}

public GameActionMark(int markAuthorId, sbyte markTeamId, int markSpellId, sbyte markSpellLevel, short markId, sbyte markType, short markimpactCell, IEnumerable<Types.GameActionMarkedCell> cells, bool active)
        {
            this.markAuthorId = markAuthorId;
            this.markTeamId = markTeamId;
            this.markSpellId = markSpellId;
            this.markSpellLevel = markSpellLevel;
            this.markId = markId;
            this.markType = markType;
            this.markimpactCell = markimpactCell;
            this.cells = cells;
            this.active = active;
        }
        

public virtual void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(markAuthorId);
            writer.WriteSByte(markTeamId);
            writer.WriteInt(markSpellId);
            writer.WriteSByte(markSpellLevel);
            writer.WriteShort(markId);
            writer.WriteSByte(markType);
            writer.WriteShort(markimpactCell);
            writer.WriteUShort((ushort)cells.Count());
            foreach (var entry in cells)
            {
                 entry.Serialize(writer);
            }
            writer.WriteBoolean(active);
            

}

public virtual void Deserialize(ICustomDataInput reader)
{

markAuthorId = reader.ReadInt();
            markTeamId = reader.ReadSByte();
            if (markTeamId < 0)
                throw new Exception("Forbidden value on markTeamId = " + markTeamId + ", it doesn't respect the following condition : markTeamId < 0");
            markSpellId = reader.ReadInt();
            if (markSpellId < 0)
                throw new Exception("Forbidden value on markSpellId = " + markSpellId + ", it doesn't respect the following condition : markSpellId < 0");
            markSpellLevel = reader.ReadSByte();
            if ((markSpellLevel < 1) || (markSpellLevel > 6))
                throw new Exception("Forbidden value on markSpellLevel = " + markSpellLevel + ", it doesn't respect the following condition : (markSpellLevel < 1) || (markSpellLevel > 6)");
            markId = reader.ReadShort();
            markType = reader.ReadSByte();
            markimpactCell = reader.ReadShort();
            if ((markimpactCell < -1) || (markimpactCell > 559))
                throw new Exception("Forbidden value on markimpactCell = " + markimpactCell + ", it doesn't respect the following condition : (markimpactCell < -1) || (markimpactCell > 559)");
            var limit = reader.ReadUShort();
            cells = new Types.GameActionMarkedCell[limit];
            for (int i = 0; i < limit; i++)
            {
                 (cells as Types.GameActionMarkedCell[])[i] = new Types.GameActionMarkedCell();
                 (cells as Types.GameActionMarkedCell[])[i].Deserialize(reader);
            }
            active = reader.ReadBoolean();
            

}


}


}