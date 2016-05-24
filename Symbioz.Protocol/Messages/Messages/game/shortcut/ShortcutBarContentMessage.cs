


















// Generated on 06/04/2015 18:45:13
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ShortcutBarContentMessage : Message
{

public const ushort Id = 6231;
public override ushort MessageId
{
    get { return Id; }
}

public sbyte barType;
        public IEnumerable<Types.Shortcut> shortcuts;
        

public ShortcutBarContentMessage()
{
}

public ShortcutBarContentMessage(sbyte barType, IEnumerable<Types.Shortcut> shortcuts)
        {
            this.barType = barType;
            this.shortcuts = shortcuts;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteSByte(barType);
            writer.WriteUShort((ushort)shortcuts.Count());
            foreach (var entry in shortcuts)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

barType = reader.ReadSByte();
            if (barType < 0)
                throw new Exception("Forbidden value on barType = " + barType + ", it doesn't respect the following condition : barType < 0");
            var limit = reader.ReadUShort();
            shortcuts = new Types.Shortcut[limit];
            for (int i = 0; i < limit; i++)
            {
                 (shortcuts as Types.Shortcut[])[i] = Types.ProtocolTypeManager.GetInstance<Types.Shortcut>(reader.ReadShort());
                 (shortcuts as Types.Shortcut[])[i].Deserialize(reader);
            }
            

}


}


}