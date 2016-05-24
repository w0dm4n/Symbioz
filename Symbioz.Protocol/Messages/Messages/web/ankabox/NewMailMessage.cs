


















// Generated on 06/04/2015 18:45:17
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class NewMailMessage : MailStatusMessage
{

public const ushort Id = 6292;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<int> sendersAccountId;
        

public NewMailMessage()
{
}

public NewMailMessage(ushort unread, ushort total, IEnumerable<int> sendersAccountId)
         : base(unread, total)
        {
            this.sendersAccountId = sendersAccountId;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteUShort((ushort)sendersAccountId.Count());
            foreach (var entry in sendersAccountId)
            {
                 writer.WriteInt(entry);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            var limit = reader.ReadUShort();
            sendersAccountId = new int[limit];
            for (int i = 0; i < limit; i++)
            {
                 (sendersAccountId as int[])[i] = reader.ReadInt();
            }
            

}


}


}