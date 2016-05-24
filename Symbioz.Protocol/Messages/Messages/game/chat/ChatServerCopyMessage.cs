


















// Generated on 06/04/2015 18:44:16
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class ChatServerCopyMessage : ChatAbstractServerMessage
{

public const ushort Id = 882;
public override ushort MessageId
{
    get { return Id; }
}

public uint receiverId;
        public string receiverName;
        

public ChatServerCopyMessage()
{
}

public ChatServerCopyMessage(sbyte channel, string content, int timestamp, string fingerprint, uint receiverId, string receiverName)
         : base(channel, content, timestamp, fingerprint)
        {
            this.receiverId = receiverId;
            this.receiverName = receiverName;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteVarUhInt(receiverId);
            writer.WriteUTF(receiverName);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            receiverId = reader.ReadVarUhInt();
            if (receiverId < 0)
                throw new Exception("Forbidden value on receiverId = " + receiverId + ", it doesn't respect the following condition : receiverId < 0");
            receiverName = reader.ReadUTF();
            

}


}


}