using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symbioz.Network.Clients;
using Symbioz.World.Records.Guilds;

namespace Symbioz.Providers.Conditions
{
    [Condition("Pw")]
    class HasGuildsCondition : Condition
    {
        public override bool Eval(WorldClient client)
        {
            if (client.Character.HasGuild)
            {
                return true;
            }
            return false;
        }
    }
    [Condition("Py")]
    class GuildLevelCondition : Condition
    {
        public override bool Eval(WorldClient client)
        {
            return GuildRecord.GetGuild(client.Character.GuildId) != null && BasicEval(ConditionValue, ComparaisonSymbol, GuildRecord.GetGuild(client.Character.GuildId).Level);
        }
    }
}
