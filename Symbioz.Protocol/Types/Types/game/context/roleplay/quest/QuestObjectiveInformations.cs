


















// Generated on 06/04/2015 18:45:31
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class QuestObjectiveInformations
{

public const short Id = 385;
public virtual short TypeId
{
    get { return Id; }
}

public ushort objectiveId;
        public bool objectiveStatus;
        public IEnumerable<string> dialogParams;
        

public QuestObjectiveInformations()
{
}

public QuestObjectiveInformations(ushort objectiveId, bool objectiveStatus, IEnumerable<string> dialogParams)
        {
            this.objectiveId = objectiveId;
            this.objectiveStatus = objectiveStatus;
            this.dialogParams = dialogParams;
        }
        

public virtual void Serialize(ICustomDataOutput writer)
{

writer.WriteVarUhShort(objectiveId);
            writer.WriteBoolean(objectiveStatus);
            writer.WriteUShort((ushort)dialogParams.Count());
            foreach (var entry in dialogParams)
            {
                 writer.WriteUTF(entry);
            }
            

}

public virtual void Deserialize(ICustomDataInput reader)
{

objectiveId = reader.ReadVarUhShort();
            if (objectiveId < 0)
                throw new Exception("Forbidden value on objectiveId = " + objectiveId + ", it doesn't respect the following condition : objectiveId < 0");
            objectiveStatus = reader.ReadBoolean();
            var limit = reader.ReadUShort();
            dialogParams = new string[limit];
            for (int i = 0; i < limit; i++)
            {
                 (dialogParams as string[])[i] = reader.ReadUTF();
            }
            

}


}


}