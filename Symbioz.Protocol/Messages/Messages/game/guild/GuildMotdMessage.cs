


















// Generated on 06/04/2015 18:44:46
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GuildMotdMessage : Message
{

public const ushort Id = 6590;
public override ushort MessageId
{
    get { return Id; }
}

public string content;
        public int timestamp;
        

public GuildMotdMessage()
{
}

public GuildMotdMessage(string content, int timestamp)
        {
            this.content = content;
            this.timestamp = timestamp;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUTF(content);
            writer.WriteInt(timestamp);
            

}

public override void Deserialize(ICustomDataInput reader)
{

content = reader.ReadUTF();
            timestamp = reader.ReadInt();
            if (timestamp < 0)
                throw new Exception("Forbidden value on timestamp = " + timestamp + ", it doesn't respect the following condition : timestamp < 0");
            

}


}


}