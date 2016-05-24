


















// Generated on 06/04/2015 18:44:17
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class EnabledChannelsMessage : Message
{

public const ushort Id = 892;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<sbyte> channels;
        public IEnumerable<sbyte> disallowed;
        

public EnabledChannelsMessage()
{
}

public EnabledChannelsMessage(IEnumerable<sbyte> channels, IEnumerable<sbyte> disallowed)
        {
            this.channels = channels;
            this.disallowed = disallowed;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort((ushort)channels.Count());
            foreach (var entry in channels)
            {
                 writer.WriteSByte(entry);
            }
            writer.WriteUShort((ushort)disallowed.Count());
            foreach (var entry in disallowed)
            {
                 writer.WriteSByte(entry);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

var limit = reader.ReadUShort();
            channels = new sbyte[limit];
            for (int i = 0; i < limit; i++)
            {
                 (channels as sbyte[])[i] = reader.ReadSByte();
            }
            limit = reader.ReadUShort();
            disallowed = new sbyte[limit];
            for (int i = 0; i < limit; i++)
            {
                 (disallowed as sbyte[])[i] = reader.ReadSByte();
            }
            

}


}


}