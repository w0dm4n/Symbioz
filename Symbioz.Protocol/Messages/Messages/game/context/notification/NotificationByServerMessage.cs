


















// Generated on 06/04/2015 18:44:24
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class NotificationByServerMessage : Message
{

public const ushort Id = 6103;
public override ushort MessageId
{
    get { return Id; }
}

public ushort id;
        public IEnumerable<string> parameters;
        public bool forceOpen;
        

public NotificationByServerMessage()
{
}

public NotificationByServerMessage(ushort id, IEnumerable<string> parameters, bool forceOpen)
        {
            this.id = id;
            this.parameters = parameters;
            this.forceOpen = forceOpen;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhShort(id);
            writer.WriteUShort((ushort)parameters.Count());
            foreach (var entry in parameters)
            {
                 writer.WriteUTF(entry);
            }
            writer.WriteBoolean(forceOpen);
            

}

public override void Deserialize(ICustomDataInput reader)
{

id = reader.ReadVarUhShort();
            if (id < 0)
                throw new Exception("Forbidden value on id = " + id + ", it doesn't respect the following condition : id < 0");
            var limit = reader.ReadUShort();
            parameters = new string[limit];
            for (int i = 0; i < limit; i++)
            {
                 (parameters as string[])[i] = reader.ReadUTF();
            }
            forceOpen = reader.ReadBoolean();
            

}


}


}