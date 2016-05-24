


















// Generated on 06/04/2015 18:44:31
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class EntityTalkMessage : Message
{

public const ushort Id = 6110;
public override ushort MessageId
{
    get { return Id; }
}

public int entityId;
        public ushort textId;
        public IEnumerable<string> parameters;
        

public EntityTalkMessage()
{
}

public EntityTalkMessage(int entityId, ushort textId, IEnumerable<string> parameters)
        {
            this.entityId = entityId;
            this.textId = textId;
            this.parameters = parameters;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteInt(entityId);
            writer.WriteVarUhShort(textId);
            writer.WriteUShort((ushort)parameters.Count());
            foreach (var entry in parameters)
            {
                 writer.WriteUTF(entry);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

entityId = reader.ReadInt();
            textId = reader.ReadVarUhShort();
            if (textId < 0)
                throw new Exception("Forbidden value on textId = " + textId + ", it doesn't respect the following condition : textId < 0");
            var limit = reader.ReadUShort();
            parameters = new string[limit];
            for (int i = 0; i < limit; i++)
            {
                 (parameters as string[])[i] = reader.ReadUTF();
            }
            

}


}


}