


















// Generated on 06/04/2015 18:44:31
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class NpcDialogQuestionMessage : Message
{

public const ushort Id = 5617;
public override ushort MessageId
{
    get { return Id; }
}

public ushort messageId;
        public IEnumerable<string> dialogParams;
        public IEnumerable<ushort> visibleReplies;
        

public NpcDialogQuestionMessage()
{
}

public NpcDialogQuestionMessage(ushort messageId, IEnumerable<string> dialogParams, IEnumerable<ushort> visibleReplies)
        {
            this.messageId = messageId;
            this.dialogParams = dialogParams;
            this.visibleReplies = visibleReplies;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhShort(messageId);
            writer.WriteUShort((ushort)dialogParams.Count());
            foreach (var entry in dialogParams)
            {
                 writer.WriteUTF(entry);
            }
            writer.WriteUShort((ushort)visibleReplies.Count());
            foreach (var entry in visibleReplies)
            {
                 writer.WriteVarUhShort(entry);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

messageId = reader.ReadVarUhShort();
            if (messageId < 0)
                throw new Exception("Forbidden value on messageId = " + messageId + ", it doesn't respect the following condition : messageId < 0");
            var limit = reader.ReadUShort();
            dialogParams = new string[limit];
            for (int i = 0; i < limit; i++)
            {
                 (dialogParams as string[])[i] = reader.ReadUTF();
            }
            limit = reader.ReadUShort();
            visibleReplies = new ushort[limit];
            for (int i = 0; i < limit; i++)
            {
                 (visibleReplies as ushort[])[i] = reader.ReadVarUhShort();
            }
            

}


}


}