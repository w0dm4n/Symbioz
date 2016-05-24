


















// Generated on 06/04/2015 18:44:04
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class AchievementDetailedListMessage : Message
{

public const ushort Id = 6358;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<Types.Achievement> startedAchievements;
        public IEnumerable<Types.Achievement> finishedAchievements;
        

public AchievementDetailedListMessage()
{
}

public AchievementDetailedListMessage(IEnumerable<Types.Achievement> startedAchievements, IEnumerable<Types.Achievement> finishedAchievements)
        {
            this.startedAchievements = startedAchievements;
            this.finishedAchievements = finishedAchievements;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

writer.WriteUShort((ushort)startedAchievements.Count());
            foreach (var entry in startedAchievements)
            {
                 entry.Serialize(writer);
            }
            writer.WriteUShort((ushort)finishedAchievements.Count());
            foreach (var entry in finishedAchievements)
            {
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

var limit = reader.ReadUShort();
            startedAchievements = new Types.Achievement[limit];
            for (int i = 0; i < limit; i++)
            {
                 (startedAchievements as Types.Achievement[])[i] = new Types.Achievement();
                 (startedAchievements as Types.Achievement[])[i].Deserialize(reader);
            }
            limit = reader.ReadUShort();
            finishedAchievements = new Types.Achievement[limit];
            for (int i = 0; i < limit; i++)
            {
                 (finishedAchievements as Types.Achievement[])[i] = new Types.Achievement();
                 (finishedAchievements as Types.Achievement[])[i].Deserialize(reader);
            }
            

}


}


}