


















// Generated on 06/04/2015 18:44:01
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ConsoleCommandsListMessage : Message
{

public const ushort Id = 6127;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<string> aliases;
        public IEnumerable<string> args;
        public IEnumerable<string> descriptions;
        

public ConsoleCommandsListMessage()
{
}

public ConsoleCommandsListMessage(IEnumerable<string> aliases, IEnumerable<string> args, IEnumerable<string> descriptions)
        {
            this.aliases = aliases;
            this.args = args;
            this.descriptions = descriptions;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort((ushort)aliases.Count());
            foreach (var entry in aliases)
            {
                 writer.WriteUTF(entry);
            }
            writer.WriteUShort((ushort)args.Count());
            foreach (var entry in args)
            {
                 writer.WriteUTF(entry);
            }
            writer.WriteUShort((ushort)descriptions.Count());
            foreach (var entry in descriptions)
            {
                 writer.WriteUTF(entry);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

var limit = reader.ReadUShort();
            aliases = new string[limit];
            for (int i = 0; i < limit; i++)
            {
                 (aliases as string[])[i] = reader.ReadUTF();
            }
            limit = reader.ReadUShort();
            args = new string[limit];
            for (int i = 0; i < limit; i++)
            {
                 (args as string[])[i] = reader.ReadUTF();
            }
            limit = reader.ReadUShort();
            descriptions = new string[limit];
            for (int i = 0; i < limit; i++)
            {
                 (descriptions as string[])[i] = reader.ReadUTF();
            }
            

}


}


}