


















// Generated on 06/04/2015 18:44:21
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameFightResumeWithSlavesMessage : GameFightResumeMessage
{

public const ushort Id = 6215;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<Types.GameFightResumeSlaveInfo> slavesInfo;
        

public GameFightResumeWithSlavesMessage()
{
}

public GameFightResumeWithSlavesMessage(IEnumerable<Types.FightDispellableEffectExtendedInformations> effects, IEnumerable<Types.GameActionMark> marks, ushort gameTurn, int fightStart, IEnumerable<Types.Idol> idols, IEnumerable<Types.GameFightSpellCooldown> spellCooldowns, sbyte summonCount, sbyte bombCount, IEnumerable<Types.GameFightResumeSlaveInfo> slavesInfo)
         : base(effects, marks, gameTurn, fightStart, idols, spellCooldowns, summonCount, bombCount)
        {
            this.slavesInfo = slavesInfo;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteUShort((ushort)slavesInfo.Count());
            foreach (var entry in slavesInfo)
            {
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            var limit = reader.ReadUShort();
            slavesInfo = new Types.GameFightResumeSlaveInfo[limit];
            for (int i = 0; i < limit; i++)
            {
                 (slavesInfo as Types.GameFightResumeSlaveInfo[])[i] = new Types.GameFightResumeSlaveInfo();
                 (slavesInfo as Types.GameFightResumeSlaveInfo[])[i].Deserialize(reader);
            }
            

}


}


}