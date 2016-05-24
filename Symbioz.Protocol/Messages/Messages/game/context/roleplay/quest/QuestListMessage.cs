


















// Generated on 06/04/2015 18:44:38
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class QuestListMessage : Message
{

public const ushort Id = 5626;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<ushort> finishedQuestsIds;
        public IEnumerable<ushort> finishedQuestsCounts;
        public IEnumerable<Types.QuestActiveInformations> activeQuests;
        public IEnumerable<ushort> reinitDoneQuestsIds;
        

public QuestListMessage()
{
}

public QuestListMessage(IEnumerable<ushort> finishedQuestsIds, IEnumerable<ushort> finishedQuestsCounts, IEnumerable<Types.QuestActiveInformations> activeQuests, IEnumerable<ushort> reinitDoneQuestsIds)
        {
            this.finishedQuestsIds = finishedQuestsIds;
            this.finishedQuestsCounts = finishedQuestsCounts;
            this.activeQuests = activeQuests;
            this.reinitDoneQuestsIds = reinitDoneQuestsIds;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort((ushort)finishedQuestsIds.Count());
            foreach (var entry in finishedQuestsIds)
            {
                 writer.WriteVarUhShort(entry);
            }
            writer.WriteUShort((ushort)finishedQuestsCounts.Count());
            foreach (var entry in finishedQuestsCounts)
            {
                 writer.WriteVarUhShort(entry);
            }
            writer.WriteUShort((ushort)activeQuests.Count());
            foreach (var entry in activeQuests)
            {
                 writer.WriteShort(entry.TypeId);
                 entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)reinitDoneQuestsIds.Count());
            foreach (var entry in reinitDoneQuestsIds)
            {
                 writer.WriteVarUhShort(entry);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

var limit = reader.ReadUShort();
            finishedQuestsIds = new ushort[limit];
            for (int i = 0; i < limit; i++)
            {
                 (finishedQuestsIds as ushort[])[i] = reader.ReadVarUhShort();
            }
            limit = reader.ReadUShort();
            finishedQuestsCounts = new ushort[limit];
            for (int i = 0; i < limit; i++)
            {
                 (finishedQuestsCounts as ushort[])[i] = reader.ReadVarUhShort();
            }
            limit = reader.ReadUShort();
            activeQuests = new Types.QuestActiveInformations[limit];
            for (int i = 0; i < limit; i++)
            {
                 (activeQuests as Types.QuestActiveInformations[])[i] = Types.ProtocolTypeManager.GetInstance<Types.QuestActiveInformations>(reader.ReadShort());
                 (activeQuests as Types.QuestActiveInformations[])[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            reinitDoneQuestsIds = new ushort[limit];
            for (int i = 0; i < limit; i++)
            {
                 (reinitDoneQuestsIds as ushort[])[i] = reader.ReadVarUhShort();
            }
            

}


}


}