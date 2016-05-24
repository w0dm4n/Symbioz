


















// Generated on 06/04/2015 18:45:31
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Types
{

public class QuestObjectiveInformationsWithCompletion : QuestObjectiveInformations
{

public const short Id = 386;
public override short TypeId
{
    get { return Id; }
}

public ushort curCompletion;
        public ushort maxCompletion;
        

public QuestObjectiveInformationsWithCompletion()
{
}

public QuestObjectiveInformationsWithCompletion(ushort objectiveId, bool objectiveStatus, IEnumerable<string> dialogParams, ushort curCompletion, ushort maxCompletion)
         : base(objectiveId, objectiveStatus, dialogParams)
        {
            this.curCompletion = curCompletion;
            this.maxCompletion = maxCompletion;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteVarUhShort(curCompletion);
            writer.WriteVarUhShort(maxCompletion);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            curCompletion = reader.ReadVarUhShort();
            if (curCompletion < 0)
                throw new Exception("Forbidden value on curCompletion = " + curCompletion + ", it doesn't respect the following condition : curCompletion < 0");
            maxCompletion = reader.ReadVarUhShort();
            if (maxCompletion < 0)
                throw new Exception("Forbidden value on maxCompletion = " + maxCompletion + ", it doesn't respect the following condition : maxCompletion < 0");
            

}


}


}