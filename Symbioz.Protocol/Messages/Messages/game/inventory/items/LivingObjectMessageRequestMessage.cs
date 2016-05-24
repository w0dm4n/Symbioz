


















// Generated on 06/04/2015 18:45:04
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class LivingObjectMessageRequestMessage : Message
{

public const ushort Id = 6066;
public override ushort MessageId
{
    get { return Id; }
}

public ushort msgId;
        public IEnumerable<string> parameters;
        public uint livingObject;
        

public LivingObjectMessageRequestMessage()
{
}

public LivingObjectMessageRequestMessage(ushort msgId, IEnumerable<string> parameters, uint livingObject)
        {
            this.msgId = msgId;
            this.parameters = parameters;
            this.livingObject = livingObject;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhShort(msgId);
            writer.WriteUShort((ushort)parameters.Count());
            foreach (var entry in parameters)
            {
                 writer.WriteUTF(entry);
            }
            writer.WriteVarUhInt(livingObject);
            

}

public override void Deserialize(ICustomDataInput reader)
{

msgId = reader.ReadVarUhShort();
            if (msgId < 0)
                throw new Exception("Forbidden value on msgId = " + msgId + ", it doesn't respect the following condition : msgId < 0");
            var limit = reader.ReadUShort();
            parameters = new string[limit];
            for (int i = 0; i < limit; i++)
            {
                 (parameters as string[])[i] = reader.ReadUTF();
            }
            livingObject = reader.ReadVarUhInt();
            if (livingObject < 0)
                throw new Exception("Forbidden value on livingObject = " + livingObject + ", it doesn't respect the following condition : livingObject < 0");
            

}


}


}