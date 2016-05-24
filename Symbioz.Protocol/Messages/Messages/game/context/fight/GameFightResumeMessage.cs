


















// Generated on 06/04/2015 18:44:21
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameFightResumeMessage : GameFightSpectateMessage
{

public const ushort Id = 6067;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<Types.GameFightSpellCooldown> spellCooldowns;
        public sbyte summonCount;
        public sbyte bombCount;
        

public GameFightResumeMessage()
{
}

public GameFightResumeMessage(IEnumerable<Types.FightDispellableEffectExtendedInformations> effects, IEnumerable<Types.GameActionMark> marks, ushort gameTurn, int fightStart, IEnumerable<Types.Idol> idols, IEnumerable<Types.GameFightSpellCooldown> spellCooldowns, sbyte summonCount, sbyte bombCount)
         : base(effects, marks, gameTurn, fightStart, idols)
        {
            this.spellCooldowns = spellCooldowns;
            this.summonCount = summonCount;
            this.bombCount = bombCount;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteUShort((ushort)spellCooldowns.Count());
            foreach (var entry in spellCooldowns)
            {
                 entry.Serialize(writer);
            }
            writer.WriteSByte(summonCount);
            writer.WriteSByte(bombCount);
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            var limit = reader.ReadUShort();
            spellCooldowns = new Types.GameFightSpellCooldown[limit];
            for (int i = 0; i < limit; i++)
            {
                 (spellCooldowns as Types.GameFightSpellCooldown[])[i] = new Types.GameFightSpellCooldown();
                 (spellCooldowns as Types.GameFightSpellCooldown[])[i].Deserialize(reader);
            }
            summonCount = reader.ReadSByte();
            if (summonCount < 0)
                throw new Exception("Forbidden value on summonCount = " + summonCount + ", it doesn't respect the following condition : summonCount < 0");
            bombCount = reader.ReadSByte();
            if (bombCount < 0)
                throw new Exception("Forbidden value on bombCount = " + bombCount + ", it doesn't respect the following condition : bombCount < 0");
            

}


}


}