


















// Generated on 06/04/2015 18:45:16
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class SystemMessageDisplayMessage : Message
{

public const ushort Id = 189;
public override ushort MessageId
{
    get { return Id; }
}

public bool hangUp;
        public ushort msgId;
        public IEnumerable<string> parameters;
        

public SystemMessageDisplayMessage()
{
}

public SystemMessageDisplayMessage(bool hangUp, ushort msgId, IEnumerable<string> parameters)
        {
            this.hangUp = hangUp;
            this.msgId = msgId;
            this.parameters = parameters;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteBoolean(hangUp);
            writer.WriteVarUhShort(msgId);
            writer.WriteUShort((ushort)parameters.Count());
            foreach (var entry in parameters)
            {
                 writer.WriteUTF(entry);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

hangUp = reader.ReadBoolean();
            msgId = reader.ReadVarUhShort();
            if (msgId < 0)
                throw new Exception("Forbidden value on msgId = " + msgId + ", it doesn't respect the following condition : msgId < 0");
            var limit = reader.ReadUShort();
            parameters = new string[limit];
            for (int i = 0; i < limit; i++)
            {
                 (parameters as string[])[i] = reader.ReadUTF();
            }
            

}


}


}