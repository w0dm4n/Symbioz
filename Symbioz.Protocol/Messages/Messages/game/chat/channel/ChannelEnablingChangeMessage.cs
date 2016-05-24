


















// Generated on 06/04/2015 18:44:17
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ChannelEnablingChangeMessage : Message
{

public const ushort Id = 891;
public override ushort MessageId
{
    get { return Id; }
}

public sbyte channel;
        public bool enable;
        

public ChannelEnablingChangeMessage()
{
}

public ChannelEnablingChangeMessage(sbyte channel, bool enable)
        {
            this.channel = channel;
            this.enable = enable;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteSByte(channel);
            writer.WriteBoolean(enable);
            

}

public override void Deserialize(ICustomDataInput reader)
{

channel = reader.ReadSByte();
            if (channel < 0)
                throw new Exception("Forbidden value on channel = " + channel + ", it doesn't respect the following condition : channel < 0");
            enable = reader.ReadBoolean();
            

}


}


}