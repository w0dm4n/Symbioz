


















// Generated on 06/04/2015 18:45:15
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class TitlesAndOrnamentsListMessage : Message
{

public const ushort Id = 6367;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<ushort> titles;
        public IEnumerable<ushort> ornaments;
        public ushort activeTitle;
        public ushort activeOrnament;
        

public TitlesAndOrnamentsListMessage()
{
}

public TitlesAndOrnamentsListMessage(IEnumerable<ushort> titles, IEnumerable<ushort> ornaments, ushort activeTitle, ushort activeOrnament)
        {
            this.titles = titles;
            this.ornaments = ornaments;
            this.activeTitle = activeTitle;
            this.activeOrnament = activeOrnament;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort((ushort)titles.Count());
            foreach (var entry in titles)
            {
                 writer.WriteVarUhShort(entry);
            }
            writer.WriteUShort((ushort)ornaments.Count());
            foreach (var entry in ornaments)
            {
                 writer.WriteVarUhShort(entry);
            }
            writer.WriteVarUhShort(activeTitle);
            writer.WriteVarUhShort(activeOrnament);
            

}

public override void Deserialize(ICustomDataInput reader)
{

var limit = reader.ReadUShort();
            titles = new ushort[limit];
            for (int i = 0; i < limit; i++)
            {
                 (titles as ushort[])[i] = reader.ReadVarUhShort();
            }
            limit = reader.ReadUShort();
            ornaments = new ushort[limit];
            for (int i = 0; i < limit; i++)
            {
                 (ornaments as ushort[])[i] = reader.ReadVarUhShort();
            }
            activeTitle = reader.ReadVarUhShort();
            if (activeTitle < 0)
                throw new Exception("Forbidden value on activeTitle = " + activeTitle + ", it doesn't respect the following condition : activeTitle < 0");
            activeOrnament = reader.ReadVarUhShort();
            if (activeOrnament < 0)
                throw new Exception("Forbidden value on activeOrnament = " + activeOrnament + ", it doesn't respect the following condition : activeOrnament < 0");
            

}


}


}