


















// Generated on 06/04/2015 18:44:21
using System;
using System.Collections.Generic;
using System.Linq;
using Symbioz.DofusProtocol.Types;
using Symbioz.Utils;

namespace Symbioz.DofusProtocol.Messages
{

public class GameFightSpectatorJoinMessage : GameFightJoinMessage
{

public const ushort Id = 6504;
public override ushort MessageId
{
    get { return Id; }
}

public IEnumerable<Types.NamedPartyTeam> namedPartyTeams;
        

public GameFightSpectatorJoinMessage()
{
}

public GameFightSpectatorJoinMessage(bool canBeCancelled, bool canSayReady, bool isFightStarted, short timeMaxBeforeFightStart, sbyte fightType, IEnumerable<Types.NamedPartyTeam> namedPartyTeams)
         : base(canBeCancelled, canSayReady, isFightStarted, timeMaxBeforeFightStart, fightType)
        {
            this.namedPartyTeams = namedPartyTeams;
        }
        

public override void Serialize(ICustomDataOutput writer)
{

base.Serialize(writer);
            writer.WriteUShort((ushort)namedPartyTeams.Count());
            foreach (var entry in namedPartyTeams)
            {
                 entry.Serialize(writer);
            }
            

}

public override void Deserialize(ICustomDataInput reader)
{

base.Deserialize(reader);
            var limit = reader.ReadUShort();
            namedPartyTeams = new Types.NamedPartyTeam[limit];
            for (int i = 0; i < limit; i++)
            {
                 (namedPartyTeams as Types.NamedPartyTeam[])[i] = new Types.NamedPartyTeam();
                 (namedPartyTeams as Types.NamedPartyTeam[])[i].Deserialize(reader);
            }
            

}


}


}